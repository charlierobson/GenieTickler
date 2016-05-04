#include "HardwareProfile.h"
#include <delays.h>
#include <p18f4550.h>
#include <pconfig.h>
#include <i2c.h>

#define SHIFTCLK LATBbits.LATB2
#define SHIFTDAT LATBbits.LATB3

#define NWR    LATBbits.LATB4
#define NRD    LATBbits.LATB5
#define NMREQ  LATBbits.LATB6
#define NIORQ  LATBbits.LATB7

// 12mhz instruction clock
#define delayMicrosec() Nop();Nop();Nop();Nop();Nop();Nop();Nop();Nop();Nop();Nop();Nop();Nop();
#define delayHalfMicrosec() Nop();Nop();Nop();Nop();Nop();Nop();

extern unsigned int gAddress;
extern unsigned int gLength;

unsigned int gAddressOffset;

void InitInterfacing()
{
	// RD, WR, MEM & IORQ = 1
	LATB = 0xF0;
	TRISB = 0x03;		// SCL/SDA inputs

	TRISD = 0xff;

	OpenI2C(MASTER, SLEW_ON);
	SSPADD = (120/1)-1; // Rate
	IdleI2C();

	StartI2C();
	IdleI2C();
	WriteI2C(0x40);
	IdleI2C();
	WriteI2C(0); // iodira
	IdleI2C();
	WriteI2C(0); // all outputs
	IdleI2C();
	// auto-increments to iodirb
	WriteI2C(0); // all outputs
	IdleI2C();
	StopI2C();
	IdleI2C();
}

long a;

void ShiftOut(unsigned int address)
{
	++a;

	StartI2C();
	IdleI2C();
	WriteI2C(0x40);
	IdleI2C();
	WriteI2C(0x12); // gpioa
	IdleI2C();
	WriteI2C(address / 256);
	IdleI2C();
	// auto-increments to gpiob
	WriteI2C(address & 255);
	IdleI2C();
	StopI2C();
	IdleI2C();
}

void Write(unsigned int address, unsigned char data)
{
	ShiftOut(address);

	TRISD = 0x00;
	LATD = data;

	NMREQ = 0;
	NWR = 0;
	delayMicrosec();
	NWR = 1;
	NMREQ = 1;

	TRISD = 0xFF;
}

unsigned char Read(unsigned int address)
{
	unsigned char data;

	ShiftOut(address);

	TRISD = 0xFF;

	NMREQ = 0;
	NRD = 0;
	delayMicrosec();
	data = PORTD;
	NRD = 1;
	NMREQ = 1;

	return data;
}

unsigned int businessContRD(void)
{
	NMREQ = 0;
	NRD = 0;
	delayMicrosec();
	NRD = 1;
	NMREQ = 1;
	return VERY_BUSY;
}

unsigned int businessContWR()
{
	NMREQ = 0;
	NWR = 0;
	delayMicrosec();
	NWR = 1;
	NMREQ = 1;
	return VERY_BUSY;
}

unsigned int businessExerciseAddr()
{
	ShiftOut(gAddress + gAddressOffset);
	NMREQ = 0;
	NMREQ = 1;

	++gAddressOffset;
	gAddressOffset &= (gLength - 1);

	return VERY_BUSY;
}

unsigned int businessExerciseData()
{
	PORTD++;
	return VERY_BUSY;
}
