//-----------------------------------------------------------------------------
//
//  usbReferenceDevice.cs
//
//  USB Generic HID Communications 3_0_0_0
//
//  A reference test application for the usbGenericHidCommunications library
//  Copyright (C) 2011 Simon Inns
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
using System.Text;
using usbGenericHidCommunications;
// The following namespace allows debugging output (when compiled in debug mode)

namespace USB_Generic_HID_reference_application
{
    /// <summary>
    /// This class performs several different tests against the 
    /// reference hardware/firmware to confirm that the USB
    /// communication library is functioning correctly.
    /// 
    /// It also serves as a demonstration of how to use the class
    /// library to perform different types of read and write
    /// operations.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    internal class UsbReferenceDevice : usbGenericHidCommunication
    {
        private readonly Action<string> _logger;
        private readonly byte[] _outputBuffer = new byte[65];

        public UsbReferenceDevice(int vid, int pid, Action<string> logger) : base(vid, pid)
        {
            _logger = logger;
        }

        // Collect debug information from the device
        public string CollectDebug()
        {
            // Declare our input buffer
            var inputBuffer = new byte[65];

            // Byte 0 must be set to 0
            _outputBuffer[0] = 0;

            // Byte 1 must be set to our command
            _outputBuffer[1] = 0x10;

            // Send the collect debug command
            writeRawReportToDevice(_outputBuffer);

            // Read the response from the device
            readSingleReportFromDevice(ref inputBuffer);

            // Byte 1 contains the number of characters transfered
            return inputBuffer[1] == 0 ? string.Empty : Encoding.ASCII.GetString(inputBuffer, 2, inputBuffer[1]);
        }

        public bool SendCommand(byte commandID, int[] args = null)
        {
            _outputBuffer[0] = 0;
            _outputBuffer[1] = commandID;

            if (args != null)
            {
                var n = 2;
                foreach (var arg in args)
                {
                    _outputBuffer[n++] = (byte)(arg / 256);
                    _outputBuffer[n++] = (byte)(arg & 255);
                }
            }

            return writeRawReportToDevice(_outputBuffer);
        }

        public bool SendStop()
        {
			return SendCommand(0xF0);
        }

        public bool ReadByte(int address, ref byte data)
        {
			if (!SendCommand(0x81, new int[]{ address })) return false;

            var inputBuffer = new byte[65];

            if (!readSingleReportFromDevice(ref inputBuffer)) return false;

            data = inputBuffer[1];
            return true;
        }

        public bool ReadBlock(int address, byte[] destinationByteArray)
		{
            _logger(string.Format("BlockRead: Data length {0} (${1:X4})", destinationByteArray.Length, destinationByteArray.Length));

			var success = SendCommand(0x83, new int[]{ address, destinationByteArray.Length }); // - block read from genie
            if (success)
            {
                var totalPackets = (destinationByteArray.Length + 63) / 64;

				var packets = new byte[totalPackets][];

                for (var packet = 0; packet < totalPackets && success; ++packet)
                {
					var p =  new byte[65];
					packets[packet] = p;

                    success = readSingleReportFromDevice(ref p);
                }

				var remaining = destinationByteArray.Length;
				var offset = 0;
				
                for (var packet = 0; packet < totalPackets && success; ++packet)
                {
					var length = (remaining > 63) ? 64 : remaining;
					remaining -= length;
					
					Array.Copy(packets[packet], 0, destinationByteArray, offset, length);
					offset += 64;
				}
			}

            if (!success) _logger("BlockRead: Bulk read from device failed");
            return success;
		}

        public bool ReadContinuous(int address)
        {
			return SendCommand(0xE0, new int[]{ address } );
        }

        public bool WriteByte(int address, int data)
        {
			return SendCommand(0x80, new int[]{ address, data * 256 }) ; // - write to genie
        }

        public bool WriteBlock(int address, byte[] data)
        {
            _logger(string.Format("BlockWrite: Data length {0} (${1:X4})", data.Length, data.Length));

			var success = SendCommand(0x82, new int[]{address, data.Length});
            if (success)
            {
                var offset = 0;
                var bytesRemaining = data.Length;
                var totalPackets = (bytesRemaining + 63) / 64;

				var packets = new byte[totalPackets][];

                for (var packet = 0; packet < totalPackets; ++packet)
				{
					var packetLength = bytesRemaining > 63 ? 64 : bytesRemaining;

					var p = new  byte[65]; // fixed length packet containing variable length data
					packets[packet] = p;

					p[0] = 0;
					Array.Copy(data, offset, p, 1, packetLength);

                    bytesRemaining -= packetLength;
					offset += packetLength;
				}
				
                for (var packet = 0; packet < totalPackets && success; ++packet)
                {
                    success = writeRawReportToDevice(packets[packet]);
                }
            }

            if (!success) _logger("BlockWrite: Bulk send to device failed");
			return success;
        }

        public bool WriteContinuous(int address, int data)
        {
			return SendCommand(0xE1, new int[]{ address, data * 256 } );
        }

        public bool LoopAddr(int address, int length)
        {
			return SendCommand(0xE2, new int[]{ address, length } );
        }

        public bool LoopData()
        {
			return SendCommand(0xE3);
        }
	}
}
