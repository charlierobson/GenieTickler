//-----------------------------------------------------------------------------
//
//  Form1.cs
//
//  USB Generic HID Communications 3_0_0_0
//
//  Modified from reference test application for the usbGenericHidCommunications
//  library which is Copyright (C) 2011 Simon Inns
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
//  Web:    http://www.waitingforfriday.com
//  Email:  simon.inns@gmail.com
//
//-----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
// ReSharper disable LocalizableElement

namespace USB_Generic_HID_reference_application
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// This is a reference application for testing the functionality of the 
        /// usbGenericHidCommunications class library.  It runs a series of 
        /// communication tests against a known USB reference device to determine
        /// if the class library is functioning correctly.
        /// 
        /// You can also use this application as a guide to integrating the 
        /// usbGenericHidCommunications class library into your projects.
        /// 
        /// See http://www,waitingforfriday.com for more detailed documentation.
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            // Create the USB reference device object (passing VID and PID)
            _theReferenceUsbDevice = new UsbReferenceDevice(0x04D8, 0x0080, ThreadSafeDebugUpdate);

            // Add a listener for usb events
            _theReferenceUsbDevice.usbEvent += usbEvent_receiver;

            // Perform an initial search for the target device
            _theReferenceUsbDevice.findTargetDevice();
        }

        // Create an instance of the USB reference device
        private readonly UsbReferenceDevice _theReferenceUsbDevice;

        private byte[] _data = new byte[0];

        private delegate void ThreadSafeDebugUpdateDelegate(string debugText);

        private BitEditPanel _addressEdit;

        private BitEditPanel _dataEdit;

        private LengthEditCombo _lengthEdit;

        private void ThreadSafeDebugUpdate(string debugText)
        {
            if (debugTextBox.InvokeRequired)
            {
                var threadSafeDebugUpdateDelegate = new ThreadSafeDebugUpdateDelegate(ThreadSafeDebugUpdate);
                debugTextBox.Invoke(threadSafeDebugUpdateDelegate, debugText);
            }
            else
            {
                debugTextBox.AppendText($"{debugText.TrimEnd()}\n");
            }
        }

        // Listener for USB events
        private void usbEvent_receiver(object o, EventArgs e)
        {
            // Check the status of the USB device and update the form accordingly
            usbToolStripStatusLabel.Text = _theReferenceUsbDevice.DeviceAttached ? "USB Device is attached" : "USB Device is detached";
        }

		private readonly object _debugCollectorLock = new object();
		
		private string GetDebugString()
		{
			lock(_debugCollectorLock)
			{
			    return _theReferenceUsbDevice.DeviceAttached ? _theReferenceUsbDevice.CollectDebug() : string.Empty;
			}
		}
        // Collect debug timer has ticked
        private void debugCollectionTimer_Tick(object sender, EventArgs e)
        {
			lock(_debugCollectorLock)
			{
				// Collect the debug information from the device
				var debugText = GetDebugString();

				// Display the debug information
				if (debugText != string.Empty)
				{
					debugTextBox.AppendText($"{debugText.TrimEnd()}\n");
				}
			}
        }

        private void CreateCheckButton(Control container, string buttonText, Action onBecomingChecked)
        {
            var checkBox = new CheckBox() { Text = buttonText, Appearance = Appearance.Button, MinimumSize = new Size(64, 23), Size = new Size(64, 23), TextAlign = ContentAlignment.MiddleCenter };
            checkBox.CheckedChanged += (sender, args) =>
            {
                var senderAsCheckBox = (CheckBox) sender;

                if (!_theReferenceUsbDevice.DeviceAttached)
                {
                    senderAsCheckBox.Checked = false;
                    return;
                }

                if (senderAsCheckBox.Checked)
                {
                    foreach (var box in container.Controls.Cast<object>().OfType<CheckBox>().Where(box => box != senderAsCheckBox && box.Checked))
                    {
                        box.Checked = false;
                    }

                    senderAsCheckBox.BackColor = Color.PaleGreen;
                    onBecomingChecked();
                }
                else
                {
                    senderAsCheckBox.BackColor = DefaultBackColor;
                        _theReferenceUsbDevice.SendStop();
				}
            };
            container.Controls.Add(checkBox);
        }

        private void CreateButton(Control container, string buttonText, Action onClick)
        {
            var button = new Button() { Text = buttonText, MinimumSize = new Size(64, 23), Size = new Size(64, 23), TextAlign = ContentAlignment.MiddleCenter };
            button.Click += (sender, args) =>
            {
                if (!_theReferenceUsbDevice.DeviceAttached)
                {
                    return;
                }

				foreach (var box in container.Controls.Cast<object>().OfType<CheckBox>().Where(box => box.Checked))
				{
					box.Checked = false;
				}

				onClick();
            };
            container.Controls.Add(button);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
			SuspendLayout();

            _addressEdit = new BitEditPanel("Address", 16)
            {
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(4, 4),
                Size = new Size(264, 60)
            };
            panelMemParams.Controls.Add(_addressEdit);

            _dataEdit = new BitEditPanel("Data", 8)
            {
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(_addressEdit.Right + 16, 4),
                Size = new Size(140, 60)
            };
            panelMemParams.Controls.Add(_dataEdit);

            var lengthLabel = new Label
            {
                Text = "Length",
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = true,
                Location = new Point(4, _addressEdit.Bottom + 8)
            };
            panelMemParams.Controls.Add(lengthLabel);

            _lengthEdit = new LengthEditCombo
            {
                Location = new Point(lengthLabel.Right, _addressEdit.Bottom + 6),
                Width = 80
            };
            panelMemParams.Controls.Add(_lengthEdit);

            CreateButton(flowLayoutPanelRadioChex, "RD", () =>
            {
                byte data = 0xff;
                if (_theReferenceUsbDevice.ReadByte(_addressEdit.Value, ref data))
                {
					_dataEdit.Value = data;
                }
            });

            CreateButton(flowLayoutPanelRadioChex, "WR", () =>
            {
				_theReferenceUsbDevice.WriteByte(_addressEdit.Value, _dataEdit.Value);
            });

            CreateCheckButton(flowLayoutPanelRadioChex, "Cont. RD", () =>
            {
                _theReferenceUsbDevice.ReadContinuous(_addressEdit.Value);
            });

            CreateCheckButton(flowLayoutPanelRadioChex, "Cont. WR", () =>
            {
                _theReferenceUsbDevice.WriteContinuous(_addressEdit.Value, _dataEdit.Value);
            });

            CreateCheckButton(flowLayoutPanelRadioChex, "Loop add.", () =>
            {
                _theReferenceUsbDevice.LoopAddr(_addressEdit.Value, _lengthEdit.Value);
            });

            CreateCheckButton(flowLayoutPanelRadioChex, "Loop dat.", () =>
            {
                _theReferenceUsbDevice.LoopData();
            });

            CreateButton(flowLayoutPanelRadioChex, "Block RD", ()=>
            {
                var fillMe = new byte[_lengthEdit.Value];

                _theReferenceUsbDevice.ReadBlock(_addressEdit.Value, fillMe);

                LoadData(fillMe);

				File.WriteAllBytes("dump.bin", fillMe);
            });

            CreateButton(flowLayoutPanelRadioChex, "Block WR", ()=>
            {
                if (_data.Length != 0)
                {
                    _theReferenceUsbDevice.WriteBlock(_addressEdit.Value, _data);
                }
                else
                {
                    ThreadSafeDebugUpdate("No data to write.");
                }
            });

			ResumeLayout();
		}

        protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			
            _theReferenceUsbDevice.usbEvent -= usbEvent_receiver;
		}

        private void listBoxData_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void LoadData(byte[] data)
        {
            _data = data;

            var hexDump = HexDump.Dump(_data);
            listBoxData.Items.Clear();
            foreach (var line in hexDump) listBoxData.Items.Add(line);
        }

        private void listBoxData_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            LoadData(File.ReadAllBytes(files[0]));
        }

        private static int GetIntTag(ToolStripItem item)
        {
            return Convert.ToInt32(item.Tag);
        }

        private void ClockedScopeTrigger_Click(object sender, EventArgs e)
        {
            int[] rates = { 128, 4096, 16384 };
            var item = (ToolStripMenuItem)sender;
            _theReferenceUsbDevice.SendCommand(0xFC, new[] { rates[GetIntTag(item)] });
            toolStripStatusLabelScopeTriggerRate.Text = $"Scope trigger: Clocked/{item.Text}";
        }

        private void PulsedScopeTrigger_Click(object sender, EventArgs e)
        {
            int[] rates = { 127, 4095, 16383 };
            var item = (ToolStripMenuItem)sender;
            _theReferenceUsbDevice.SendCommand(0xFC, new[] { rates[GetIntTag(item)] });
            toolStripStatusLabelScopeTriggerRate.Text = $"Scope trigger: Pulsed/{item.Text}";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
