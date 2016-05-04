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
	LATB = 0xF0;		// RD, WR, MEM & IORQ = 1
	TRISB = 0x03;		// SCL/SDA are inputs

	TRISD = 0xff;

	SSPADD = (120/1)-1; // Rate - 100KHz

	OpenI2C(MASTER, SLEW_ON);
	IdleI2C();

	StartI2C();
	IdleI2C();
	WriteI2C(0x40);
	IdleI2C();
	WriteI2C(0); // iodira register
	IdleI2C();
	WriteI2C(0); // all out
	IdleI2C();
	// (auto-increment register to iodirb)
	WriteI2C(0); // all out
	IdleI2C();
	StopI2C();
	IdleI2C();
}


// these need to be global so the inline asm can correctly access them
unsigned char gpa, gpb;
unsigned char ah, al;

void addrToGP(unsigned int address)
{
	ah = address >> 8;
	al = address & 255;

_asm
	movlb al			// bank select. hopefully gpa,gpb,al,ah are all in the same bank ;)

	RRCF al,1,1
	RLCF gpb,1,1
	RRCF al,1,1
	RRCF gpa,1,1

	RRCF al,1,1
	RLCF gpb,1,1
	RRCF al,1,1
	RRCF gpa,1,1

	RRCF al,1,1
	RLCF gpb,1,1
	RRCF al,1,1
	RRCF gpa,1,1

	RRCF al,1,1
	RLCF gpb,1,1
	RRCF al,1,1
	RRCF gpa,1,1

	RRCF ah,1,1
	RLCF gpb,1,1
	RRCF ah,1,1
	RRCF gpa,1,1

	RRCF ah,1,1
	RLCF gpb,1,1
	RRCF ah,1,1
	RRCF gpa,1,1

	RRCF ah,1,1
	RLCF gpb,1,1
	RRCF ah,1,1
	RRCF gpa,1,1

	RRCF ah,1,1
	RLCF gpb,1,1
	RRCF ah,1,1
	RRCF gpa,1,1
_endasm
}


void ShiftOut(unsigned int address)
{
	// mutate the bits
	addrToGP(address);

	StartI2C();
	IdleI2C();
	WriteI2C(0x40);
	IdleI2C();
	WriteI2C(0x12); // select gpioa register
	IdleI2C();
	WriteI2C(gpa);
	IdleI2C();
	// (auto-increment register to gpiob)
	WriteI2C(gpb);
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
