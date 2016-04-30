/************************************************************************
	HardwareProfile.h

    usbGenericHidCommunication reference firmware 3_0_0_0
    Copyright (C) 2011 Simon Inns

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

	Email: simon.inns@gmail.com

************************************************************************/

#ifndef HARDWAREPROFILE_H
#define HARDWAREPROFILE_H

// USB stack hardware selection options ----------------------------------------------------------------

// (This section is the set of definitions required by the MCHPFSUSB framework.)

#define self_power          1
#define USB_BUS_SENSE       1

// Uncomment the following line to make the output HEX of this project work with the MCHPUSB Bootloader    
//#define PROGRAMMABLE_WITH_USB_MCHPUSB_BOOTLOADER

// Uncomment the following line to make the output HEX of this project work with the HID Bootloader
#define PROGRAMMABLE_WITH_USB_HID_BOOTLOADER		

// Application specific hardware definitions ------------------------------------------------------------

// Oscillator frequency (48Mhz with a 20Mhz external oscillator)
#define CLOCK_FREQ 48000000

// Device Vendor Indentifier (VID) (0x04D8 is Microchip's VID)
#define USB_VID	0x04D8

// Device Product Indentifier (PID) (0x0080)
#define USB_PID	0x0080

// Manufacturer string descriptor
#define MSDLENGTH	10
#define MSD		'S','i','r',' ','M','o','r','r','i','s'

// Product String descriptor
#define PSDLENGTH	13
#define PSD		'G','e','n','i','e',' ','T','i','c','k','l','e','r'

// Device serial number string descriptor
#define DSNLENGTH	7
#define DSN		'S','M','B','_','0','.','1'

// Common useful definitions
#define INPUT_PIN 1
#define OUTPUT_PIN 0
#define FLAG_FALSE 0
#define FLAG_TRUE 1

// Comment out the following line if you do not want the debug
// feature of the firmware (saves code and RAM space when off)
//
// Note: if you use this feature you must compile with the large
// memory model on (for 24-bit pointers) so that the sprintf()
// function will work correctly.  If you do not require debug it's
// recommended that you compile with the small memory model and 
// remove any references to <strings.h> and sprintf().
#define DEBUGON

// PIC to hardware pin mapping and control macros

#define mInitStatusLeds()	LATC &= 0b11111001; TRISC &= 0b11111001;
#define mStatusLED0			LATCbits.LATC1
#define mStatusLED1			LATCbits.LATC2

#define mInitScopeTaps()	TRISA &= 0b11111110;
#define mScopeTrigger		LATAbits.LATA0


#define mStatusLED0_on()	mStatusLED0 = 1;
#define mStatusLED1_on()	mStatusLED1 = 1;

#define mStatusLED0_off()	mStatusLED0 = 0;
#define mStatusLED1_off()	mStatusLED1 = 0;

#define mStatusLED0_Toggle()     mStatusLED0 = !mStatusLED0;
#define mStatusLED1_Toggle()     mStatusLED1 = !mStatusLED1;


#define NOT_BUSY 65535
#define QUITE_BUSY 32768
#define VERY_BUSY 16384

#endif
