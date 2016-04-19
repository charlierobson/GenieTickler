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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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

        private void ThreadSafeDebugUpdate(string debugText)
        {
            if (debugTextBox.InvokeRequired)
            {
                var threadSafeDebugUpdateDelegate = new ThreadSafeDebugUpdateDelegate(ThreadSafeDebugUpdate);
                debugTextBox.Invoke(threadSafeDebugUpdateDelegate, debugText);
            }
            else
            {
                debugTextBox.AppendText(string.Format("{0}\n", debugText.TrimEnd()));
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
					debugTextBox.AppendText(string.Format("{0}\n", debugText.TrimEnd()));
				}
			}
        }

        private readonly CheckBox[] _addressBits = new CheckBox[16];
        private readonly CheckBox[] _dataBits = new CheckBox[8];
        private Label _addrhex;
        private Label _datahex;

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

        private void Form1_Load(object sender, EventArgs e)
        {
            var x = 20;
            var y = 40;

            Controls.Add(new Label { Text = "Address", Location = new Point(x, y - 20), AutoSize = true });
            _addrhex = new Label() { Text = "0000", Location = new Point(x + 64, y - 20), AutoSize = true, Font = new Font("FixedSys", 10) };
            Controls.Add(_addrhex);
            for (var i = 15; i > -1; --i)
            {
                var check = new CheckBox {Tag = i, Location = new Point(x, y), Size = new Size(16,16)};
                check.CheckedChanged += (o, args) => { _addrhex.Text = DecodeBits(_addressBits).ToString("X4"); };

                _addressBits[i] = check;
                Controls.Add(check);

                x += 16;
            }

            x += 32;

            var button = new Button { Text = "Write", Location = new Point(x, y+20), AutoSize = true, Width = 50 };
            button.Click += (o, args) =>
            {
                _theReferenceUsbDevice.WriteSingleByte(DecodeBits(_addressBits), DecodeBits(_dataBits));
            };
            Controls.Add(button);

            button = new Button { Text = "Read", Location = new Point(x + 75, y+20), Width = 50 };
            button.Click += (o, args) =>
            {
                byte data = 0;
                if (_theReferenceUsbDevice.ReadSingleByte(DecodeBits(_addressBits), ref data))
                {
                    EncodeBits(_dataBits, data);
                }
            };
            Controls.Add(button);

            Controls.Add(new Label { Text = "Data", Location = new Point(x, y - 20), AutoSize = true });
            _datahex = new Label() { Text = "00", Location = new Point(x + 60, y - 20), AutoSize = true, Font = new Font("FixedSys", 10) };
            Controls.Add(_datahex);
            for (var i = 7; i > -1; --i)
            {
                var check = new CheckBox { Tag = i, Location = new Point(x, y), Size = new Size(16, 16)};
                check.CheckedChanged += (o, args) => { _datahex.Text = DecodeBits(_dataBits).ToString("X2"); };

                _dataBits[i] = check;
                Controls.Add(check);

                x += 16;
            }

            CreateCheckButton(flowLayoutPanelRadioChex, "Cont. RD", () =>
            {
                var address = DecodeBits(_addressBits);
                _theReferenceUsbDevice.ContRead(address);
            });

            CreateCheckButton(flowLayoutPanelRadioChex, "Cont. WR", () =>
            {
                var address = DecodeBits(_addressBits);
                var data = DecodeBits(_dataBits);
                _theReferenceUsbDevice.ContWrite(address, data);
            });

            CreateCheckButton(flowLayoutPanelRadioChex, "Block WR", ()=>
            {
                if (_data.Count() != 0)
                {
                    var address = DecodeBits(_addressBits);
                    _theReferenceUsbDevice.BlockWrite(address, _data);
                }
                else
                {
                    ThreadSafeDebugUpdate("No data to write.");
                }
            });

            CreateCheckButton(flowLayoutPanelRadioChex, "Block RD", ()=>
            {
                var address = DecodeBits(_addressBits);

                var fillMe = new byte[1024];

                _theReferenceUsbDevice.BlockRead(address, fillMe);

                LoadData(fillMe);
            });
		}

        private static byte[] GetSimpleParallelData(int count)
        {
            byte clock = 0;
            var data = new byte[count];
            for (var i = 0; i < count; ++i)
            {
                data[i] = (byte)(((i & 255) / 2) | clock);
                clock ^= 0x80;
            }
            return data;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			
            _theReferenceUsbDevice.usbEvent -= usbEvent_receiver;
		}

        private static int DecodeBits(CheckBox[] bitCollection)
        {
            // assumes that the checkboxes in the collection are added in place postion order 0 -> N

            var value = 0;
            var bitMask = 1;

            foreach (var bit in bitCollection)
            {
                if (bit.Checked)
                    value |= bitMask;
                bitMask <<= 1;
            }

            return value;
        }

        private void EncodeBits(CheckBox[] bitCollection, int data)
        {
            // assumes that the checkboxes in the collection are added in place postion order 0 -> N

            var bitMask = 1;

            foreach (var bit in bitCollection)
            {
                bit.Checked = (data & bitMask) != 0;
                bitMask <<= 1;
            }
        }
		
		private static List<string> HexDump(byte[] bytes, int bytesPerLine = 16)
        {
            if (bytes == null) return null;

            var bytesLength = bytes.Length;

            var hexChars = "0123456789ABCDEF".ToCharArray();

		    const int firstHexColumn = 8 + 3; // 8 characters for the address + 3 spaces

            var firstCharColumn = firstHexColumn
                + bytesPerLine * 3       // - 2 digit for the hexadecimal value and 1 space
                + (bytesPerLine - 1) / 8 // - 1 extra space every 8 characters from the 9th
                + 2;                  // 2 spaces 

            var lineLength = firstCharColumn
                + bytesPerLine           // - characters to show the ascii value
                + Environment.NewLine.Length; // Carriage return and line feed (should normally be 2)

            var line = (new string(' ', lineLength - Environment.NewLine.Length) + Environment.NewLine).ToCharArray();
            var expectedLines = (bytesLength + bytesPerLine - 1) / bytesPerLine;

            var result = new List<string>();

            for (var i = 0; i < bytesLength; i += bytesPerLine)
            {
                line[0] = hexChars[(i >> 28) & 0xF];
                line[1] = hexChars[(i >> 24) & 0xF];
                line[2] = hexChars[(i >> 20) & 0xF];
                line[3] = hexChars[(i >> 16) & 0xF];
                line[4] = hexChars[(i >> 12) & 0xF];
                line[5] = hexChars[(i >> 8) & 0xF];
                line[6] = hexChars[(i >> 4) & 0xF];
                line[7] = hexChars[(i >> 0) & 0xF];

                var hexColumn = firstHexColumn;
                var charColumn = firstCharColumn;

                for (var j = 0; j < bytesPerLine; j++)
                {
                    if (j > 0 && (j & 7) == 0) hexColumn++;
                    if (i + j >= bytesLength)
                    {
                        line[hexColumn] = ' ';
                        line[hexColumn + 1] = ' ';
                        line[charColumn] = ' ';
                    }
                    else
                    {
                        var b = bytes[i + j];
                        line[hexColumn] = hexChars[(b >> 4) & 0xF];
                        line[hexColumn + 1] = hexChars[b & 0xF];
                        line[charColumn] = b < 32 ? '.' : (char)b;
                    }
                    hexColumn += 3;
                    charColumn++;
                }

                result.Add(new string(line));
            }

            return result;
		}

        private void listBoxData_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void LoadData(byte[] data)
        {
            _data = data;

            var hexDump = HexDump(_data);
            listBoxData.Items.Clear();
            foreach (var line in hexDump) listBoxData.Items.Add(line);
        }

        private void listBoxData_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            LoadData(File.ReadAllBytes(files[0]));
        }
    }
}
