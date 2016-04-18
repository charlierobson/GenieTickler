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
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
            _theReferenceUsbDevice = new usbReferenceDevice(0x04D8, 0x0080, o => ThreadSafeDebugUpdate(o));

            // Add a listener for usb events
            _theReferenceUsbDevice.usbEvent += usbEvent_receiver;

            // Perform an initial search for the target device
            _theReferenceUsbDevice.findTargetDevice();

            _stopSignal = new AutoResetEvent(false);
        }

        // Create an instance of the USB reference device
        private readonly usbReferenceDevice _theReferenceUsbDevice;

        delegate void ThreadSafeDebugUpdateDelegate(string debugText);

        private void ThreadSafeDebugUpdate(string debugText)
        {
            if (debugTextBox.InvokeRequired)
            {
                var threadSafeDebugUpdateDelegate = new ThreadSafeDebugUpdateDelegate(ThreadSafeDebugUpdate);
                debugTextBox.Invoke(threadSafeDebugUpdateDelegate, debugText);
            }
            else
            {
                debugText.TrimEnd();
                debugText += "\n";
                debugTextBox.AppendText(debugText);
            }
        }

        // Listener for USB events
        private void usbEvent_receiver(object o, EventArgs e)
        {
            // Check the status of the USB device and update the form accordingly
            usbToolStripStatusLabel.Text = _theReferenceUsbDevice.DeviceAttached ? "USB Device is attached" : "USB Device is detached";
        }

		private object _debugCollectorLock = new object();
		
		private string GetDebugString()
		{
			lock(_debugCollectorLock)
			{
				// Collect the debug information from the device
				return _theReferenceUsbDevice.collectDebug();
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
				if (debugText != String.Empty)
				{
					debugText.TrimEnd();
					debugText += "\n";
					debugTextBox.AppendText(debugText);
				}
			}
        }

        private readonly CheckBox[] _addressBits = new CheckBox[16];
        private readonly CheckBox[] _dataBits = new CheckBox[8];
        private Label _addrhex;
        private Label _datahex;
        private AutoResetEvent _stopSignal;
        private Thread _testProcedure;

        private const int LEFT = 20;

        private void Form1_Load(object sender, EventArgs e)
        {
            var x = LEFT;
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
                _theReferenceUsbDevice.Write(DecodeBits(_addressBits), DecodeBits(_dataBits));
            };
            Controls.Add(button);

            button = new Button { Text = "Read", Location = new Point(x + 75, y+20), Width = 50 };
            button.Click += (o, args) =>
            {
                byte data;
                if (_theReferenceUsbDevice.Read(DecodeBits(_addressBits), out data))
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

            x = LEFT;
            y += 60;

            var testBtnDToggle = new CheckBox() { Text = "DToggle", Appearance = Appearance.Button, Location = new Point(x, y), MinimumSize = new Size(64, 23), Size = new Size(64, 23), TextAlign = ContentAlignment.MiddleCenter };
            testBtnDToggle.Click += (o, args) =>
            {
                DataToggleTest(testBtnDToggle);
            };
            Controls.Add(testBtnDToggle);

            x += 80;
            var testBtnContRead = new CheckBox() { Text = "ContRead", Appearance = Appearance.Button, Location = new Point(x, y), MinimumSize = new Size(64, 23), Size = new Size(64, 23), TextAlign = ContentAlignment.MiddleCenter };
            testBtnContRead.Click += (o, args) =>
            {
                ContinuousReadTest(testBtnContRead);
            };
            Controls.Add(testBtnContRead);

            x += 80;
            var testBtnContWrite = new CheckBox() { Text = "ContWrite", Appearance = Appearance.Button, Location = new Point(x, y), MinimumSize = new Size(64, 23), Size = new Size(64, 23), TextAlign = ContentAlignment.MiddleCenter };
            testBtnContWrite.Click += (o, args) =>
            {
                ContinuousWriteTest(testBtnContWrite);
            };
            Controls.Add(testBtnContWrite);

            x += 80;
            var testMemFill = new Button() { Text = "Fill", Location = new Point(x, y), MinimumSize = new Size(64, 23), Size = new Size(64, 23), TextAlign = ContentAlignment.MiddleCenter };
            testMemFill.Click += (o, args) =>
            {
                MemFill();
            };
            Controls.Add(testMemFill);

            x += 80;
            var testMemRead = new Button() { Text = "Read", Location = new Point(x, y), MinimumSize = new Size(64, 23), Size = new Size(64, 23), TextAlign = ContentAlignment.MiddleCenter };
            testMemRead.Click += (o, args) =>
            {
                MemRead();
            };
            Controls.Add(testMemRead);
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			
			StopCurrentTest();

            _theReferenceUsbDevice.usbEvent -= usbEvent_receiver;

			_stopSignal.Close();
		}

		private void MemRead()
        {
            var address = DecodeBits(_addressBits);

			var data = Enumerable.Repeat((byte)0xFF, 1024).ToArray();

            StopCurrentTest();

            if (!_theReferenceUsbDevice.DeviceAttached)
            {
               return;
            }

            StartTest(() =>
            {
                _theReferenceUsbDevice.BlockRead(address, data);
				ThreadSafeDebugUpdate(HexDump(data)); 
			});
			

        }
	
        private void MemFill()
        {
            var address = DecodeBits(_addressBits);

            StopCurrentTest();

            if (!_theReferenceUsbDevice.DeviceAttached)
            {
               return;
            }

            StartTest(() =>
            {
				byte clock = 0;
                var data = new byte[240];
				
				for(var i = 0; i < 240; ++i)
				{
					data[i] = (byte)((i / 2) | clock);
					clock ^= 0x80;
				}
                _theReferenceUsbDevice.BlockWrite(address, data);
            });
        }

        private void ContinuousWriteTest(CheckBox testBtnToggle)
        {
            StopCurrentTest();

            if (!_theReferenceUsbDevice.DeviceAttached)
            {
               testBtnToggle.Checked = false;
               return;
            }

            if (testBtnToggle.Checked)
            {
                StartTest(() =>
                {
                    var address = DecodeBits(_addressBits);
                    var data = DecodeBits(_dataBits);
                    testBtnToggle.BackColor = testBtnToggle.BackColor == Color.Chartreuse ? DefaultBackColor : Color.Chartreuse;

                    do
                    {
                        _theReferenceUsbDevice.Write(address, data);
                    }
                    while (!_stopSignal.WaitOne(20));

                    testBtnToggle.BackColor = DefaultBackColor;
                });
            }
        }

        private void ContinuousReadTest(CheckBox testBtnToggle)
        {
            if (!_theReferenceUsbDevice.DeviceAttached)
            {
               testBtnToggle.Checked = false;
               return;
            }

			var address = DecodeBits(_addressBits);
			_theReferenceUsbDevice.ContRead(address, testBtnToggle.Checked);
        }

        private void DataToggleTest(CheckBox testBtnToggle)
        {
            StopCurrentTest();

            if (!_theReferenceUsbDevice.DeviceAttached)
            {
               testBtnToggle.Checked = false;
               return;
            }

            if (testBtnToggle.Checked)
            {
                StartTest(() =>
                {
                    var address = DecodeBits(_addressBits);
                    var data = DecodeBits(_dataBits);

                    do
                    {
                        Debug.WriteLine(string.Format("data = {0}", data));

                        _theReferenceUsbDevice.Write(address, data);
                        data ^= 0xff;

                        testBtnToggle.BackColor = testBtnToggle.BackColor == Color.Chartreuse ? DefaultBackColor : Color.Chartreuse;
                    }
                    while (!_stopSignal.WaitOne(500));

                    testBtnToggle.BackColor = DefaultBackColor;
                });
            }
        }

        private void StartTest(Action action)
        {
            _testProcedure = new Thread(new ThreadStart(action));
            _testProcedure.Start();
        }

        private void StopCurrentTest()
        {
            if (_testProcedure == null) return;

            _stopSignal.Set();
            _testProcedure.Join();
            _testProcedure = null;
        }

        private static int DecodeBits(CheckBox[] bitCollection)
        {
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
            var bitMask = 1;

            foreach (var bit in bitCollection)
            {
                bit.Checked = (data & bitMask) != 0;
                bitMask <<= 1;
            }
        }
		
		private static string HexDump(byte[] bytes, int bytesPerLine = 16)
        {
            if (bytes == null) return "<null>";
            int bytesLength = bytes.Length;

            char[] HexChars = "0123456789ABCDEF".ToCharArray();

            int firstHexColumn =
                  8                   // 8 characters for the address
                + 3;                  // 3 spaces

            int firstCharColumn = firstHexColumn
                + bytesPerLine * 3       // - 2 digit for the hexadecimal value and 1 space
                + (bytesPerLine - 1) / 8 // - 1 extra space every 8 characters from the 9th
                + 2;                  // 2 spaces 

            int lineLength = firstCharColumn
                + bytesPerLine           // - characters to show the ascii value
                + Environment.NewLine.Length; // Carriage return and line feed (should normally be 2)

            char[] line = (new String(' ', lineLength - Environment.NewLine.Length) + Environment.NewLine).ToCharArray();
            int expectedLines = (bytesLength + bytesPerLine - 1) / bytesPerLine;
            StringBuilder result = new StringBuilder(expectedLines * lineLength);

            for (int i = 0; i < bytesLength; i += bytesPerLine)
            {
                line[0] = HexChars[(i >> 28) & 0xF];
                line[1] = HexChars[(i >> 24) & 0xF];
                line[2] = HexChars[(i >> 20) & 0xF];
                line[3] = HexChars[(i >> 16) & 0xF];
                line[4] = HexChars[(i >> 12) & 0xF];
                line[5] = HexChars[(i >> 8) & 0xF];
                line[6] = HexChars[(i >> 4) & 0xF];
                line[7] = HexChars[(i >> 0) & 0xF];

                int hexColumn = firstHexColumn;
                int charColumn = firstCharColumn;

                for (int j = 0; j < bytesPerLine; j++)
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
                        byte b = bytes[i + j];
                        line[hexColumn] = HexChars[(b >> 4) & 0xF];
                        line[hexColumn + 1] = HexChars[b & 0xF];
                        line[charColumn] = (b < 32 ? '.' : (char)b);
                    }
                    hexColumn += 3;
                    charColumn++;
                }
                result.Append(line);
            }
            return result.ToString();
		}
    }
}