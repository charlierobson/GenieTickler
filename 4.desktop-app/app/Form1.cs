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
            ThreadSafeDebugUpdate(string.Format("object: {0}  Args: {1}  Att? {2}", o, e, _theReferenceUsbDevice.DeviceAttached));

            // Check the status of the USB device and update the form accordingly
            usbToolStripStatusLabel.Text = _theReferenceUsbDevice.DeviceAttached ? "USB Device is attached" : "USB Device is detached";
        }

        // Collect debug timer has ticked
        private void debugCollectionTimer_Tick(object sender, EventArgs e)
        {
            // Collect the debug information from the device
            var debugString = _theReferenceUsbDevice.collectDebug();

            // Display the debug information
            if (debugString != String.Empty)
            {
                debugTextBox.AppendText(debugString);
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
        }

        private void MemFill()
        {
            var address = DecodeBits(_addressBits);

            StopCurrentTest();
            StartTest(() =>
            {
                var data = new byte[240];
                _theReferenceUsbDevice.BlockWrite(address, data);
            });
        }

        private void ContinuousWriteTest(CheckBox testBtnToggle)
        {
            StopCurrentTest();

            //if (!_theReferenceUsbDevice.DeviceAttached)
            //{
            //    oCheckBox.Checked = false;
            //    return;
            //}

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
            StopCurrentTest();

            //if (!_theReferenceUsbDevice.DeviceAttached)
            //{
            //    oCheckBox.Checked = false;
            //    return;
            //}

            if (testBtnToggle.Checked)
            {
                StartTest(() =>
                {
                    var address = DecodeBits(_addressBits);
                    testBtnToggle.BackColor = testBtnToggle.BackColor == Color.Chartreuse ? DefaultBackColor : Color.Chartreuse;

                    do
                    {
                        byte data;
                        _theReferenceUsbDevice.Read(address, out data);
                    }
                    while (!_stopSignal.WaitOne(20));

                    testBtnToggle.BackColor = DefaultBackColor;
                });
            }
        }

        private void DataToggleTest(CheckBox testBtnToggle)
        {
            StopCurrentTest();

            //if (!_theReferenceUsbDevice.DeviceAttached)
            //{
            //    oCheckBox.Checked = false;
            //    return;
            //}

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
    }
}