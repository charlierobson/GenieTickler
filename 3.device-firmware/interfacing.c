#include "HardwareProfile.h"
#include <delays.h>

#define SHIFTCLK LATBbits.LATB2
#define SHIFTDAT LATBbits.LATB3

#define NWR    LATBbits.LATB4
#define NRD    LATBbits.LATB5
#define NMREQ  LATBbits.LATB6
#define NIORQ  LATBbits.LATB7


// 12mhz instruction clock
#define delayMicrosec() Nop();Nop();Nop();Nop();Nop();Nop();Nop();Nop();Nop();Nop();Nop();Nop();

void InitInterfacing()
{
	SHIFTCLK = 0;

	// RD, WR, MEM & IORQ = 1, SHIFTCLK,DATA = 0
	LATB = 0xF0;
	TRISB = 0x03;
}

void ShiftOut(unsigned int address)
{
	int i;

	for(i = 0; i < 16; ++i)
	{
		SHIFTDAT = address & 1;
		SHIFTCLK = 1;
		address >>= 1;
		SHIFTCLK = 0;
	}
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


unsigned int businessToggleRD(unsigned int counter)
{
	NMREQ = 0;
	NRD = counter & 1;
	return VERY_BUSY;
}

unsigned int businessToggleWR(unsigned int counter)
{
	NMREQ = 0;
	NWR = counter & 1;
	return VERY_BUSY;
}
