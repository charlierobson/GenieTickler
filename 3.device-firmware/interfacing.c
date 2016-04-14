#include <delays.h>

#define ASSERT_MEMRQ
#define DEASSERT_MEMRQ

#define ASSERT_WR
#define DEASSERT_WR

#define SHIFTCLK LATBbits.LATB2
#define SHIFTDAT LATBbits.LATB3

#define NWR    LATBbits.LATB4
#define NRD    LATBbits.LATB5
#define NMREQ  LATBbits.LATB6
#define NIORQ  LATBbits.LATB7


// 12mhz instruction clock
#define delayMicrosec() Nop();Nop();Nop();Nop();Nop();Nop();Nop();Nop();Nop();Nop();Nop();Nop();

void InitShifter()
{
	SHIFTCLK = 0;
}

void ShiftOut(int address)
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

void Write(int address, unsigned char data)
{
	return;

	ShiftOut(address);

	TRISD = 0x00;
	LATD = data;

	NMREQ = 0;
	NWR = 0;
	delayMicrosec();
	NWR = 1;
	NMREQ = 1;

	//TRISD = 0xFF;
}


unsigned char Read(int address)
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
