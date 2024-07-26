/*
 * LEGO® MINDSTORMS EV3
 *
 * Copyright (C) 2010-2013 The LEGO Group
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */


 /*! \page Mode2 decode interface Description
  *
  *- \subpage  Mode2Description
  *- \subpage  Mode2Decode
  *
  */

  /*! \page Mode2Description Mode2 Description
   *
   *  <hr size="1"/>
   *
   *  Description of Mode2
   *
   *  All bytes received from a remote mode2 bluetooth device
   *  should be relayed unchanged to mode2 decoding.
   *
   *  All mode2 data bytes from mode2 decoding should
   *  be transmitted unchanged to the remote Mode2
   *  bluetooth device.
   *
   *  All brick application data for the remote mode2
   *  bluetooth device should be transferred for mode2
   *  decoding.
   */

   /*! \page Mode2Decode Mode2 Decode Description
	*
	*  <hr size="1"/>
	*
	*  Interfaces to Mode2
	*/

#include  "lms2012.h"
#include  "c_com.h"
#include  "c_bt.h"
#include  "c_i2c.h"

#include  <stdio.h>
#include  <fcntl.h>
#include  <stdlib.h>
#include  <string.h>
#include  <signal.h>
#include  <time.h>
#include  <errno.h>



/* I2C Adresses */
enum
{
	WRITE_DATA = 0x2A,    // Also referred to as 0x54
	READ_STATUS = 0x2A,    // Also referred to as 0x55
	WRITE_RAW = 0x2B,    // Also referred to as 0x56
	READ_DATA = 0x2B     // Also referred to as 0x57
};

/* Bluetooth pins needs to be synchronized with pins in d_bt.c */
enum
{
	CTS_PIC,
	PIC_RST,
	PIC_EN,
	BLUETOOTH_PINS
};

enum
{
	SET = 0,
	CLEAR = 1,
	HIIMP = 2
};

enum
{
	OVERRUN_ERROR = 0x01,
	CRC_ERROR = 0x02,
	INCORRECT_ACTION = 0x03,
	UNEXPECTED_ERROR = 0x04,
	RAW_OVERRUN_ERROR = 0x05,
};


#define   I2CBUF_SIZE                   150
#define   APPDATABUF_SIZE               150
#define   MODE2BUF_SIZE                 1024   // Must be power of 2
#define   MIN_MSG_LEN                   6
#define   SLEEPuS                       ((ULONG)(1000))
#define   SEC_1                         (((ULONG)(1000000))/SLEEPuS)

typedef   struct
{
	UBYTE   Buf[MODE2BUF_SIZE];
	UWORD   InPtr;
	UWORD   OutPtr;
} MODE2BUF;


static    int       I2cFile;
static    int       BtFile;
static    int       ThreadRunState;
static    MODE2BUF  Mode2InBuf;
char* RtnMsg = "I2c Func thread end";
static    struct    i2c_rdwr_ioctl_data    msg_rdwr;
static    struct    i2c_msg                i2cmsg;

static    UBYTE     Status;

static    READBUF* pReadBuf;
static    WRITEBUF* pMode2WriteBuf;

static    char* pBundleIdString;
static    char* pBundleSeedIdString;

static    unsigned char TmpBuf[I2CBUF_SIZE + 1];


UBYTE     I2cReadStatus(UBYTE* pBuf);
UBYTE     I2cReadCts(void);
int       I2cRead(int fd, unsigned int addr, unsigned char* buf, unsigned char len);
int       I2cWrite(int fd, unsigned int addr, unsigned char* buf, unsigned char len);
void* I2cCtrl(void* ptr);

void      I2cSetPIC_RST(void);
void      I2cClearPIC_RST(void);
void      I2cSetPIC_EN(void);
void      I2cClearPIC_EN(void);
void      I2cHiImpPIC_EN(void);


#define   BUFBytesFree                  (((Mode2InBuf.OutPtr - Mode2InBuf.InPtr) - 1)      &  ((UWORD)(MODE2BUF_SIZE - 1)))
#define   BUFBytesAvail                 ((Mode2InBuf.InPtr   - Mode2InBuf.OutPtr)          &  ((UWORD)(MODE2BUF_SIZE - 1)))
#define   BUFAddInPtr(Val)              (Mode2InBuf.InPtr    = ((Mode2InBuf.InPtr + Val)   &  ((UWORD)(MODE2BUF_SIZE - 1))))
#define   BUFAddOutPtr(Val)             (Mode2InBuf.OutPtr   = ((Mode2InBuf.OutPtr + Val)  &  ((UWORD)(MODE2BUF_SIZE - 1))))

#define   DISCONNDueToErr               {\
                                          cBtDiscChNo(0);\
                                          ThreadRunState = 0;\
                                        }


RESULT    I2cInit(READBUF* pBuf, WRITEBUF* pWriteBuf, char* pBundleId, char* pBundleSeedId)
{
	RESULT  Result = FAIL;

	pReadBuf = pBuf;
	pMode2WriteBuf = pWriteBuf;
	pBundleIdString = pBundleId;
	pBundleSeedIdString = pBundleSeedId;

	return(Result);
}


void      I2cExit(void)
{
	I2cStop();
}


void      I2cStart(void)
{
	Mode2InBuf.InPtr = 0;
	Mode2InBuf.OutPtr = 0;
	Status = MODE2_BOOTING;
	ThreadRunState = 1;
}


void      I2cStop(void)
{
	ThreadRunState = 0;
}


void* I2cCtrl(void* ptr)
{
	UWORD   Size;
	UBYTE   Buf[200];
	UWORD   Check;

	I2cFile = -1;

	if (I2cFile >= MIN_HANDLE)
	{
		I2cFile = -1;
	}

	Mode2InBuf.InPtr = 0;
	Mode2InBuf.OutPtr = 0;
}

UBYTE     I2cReadCts(void)
{
	UBYTE    Buf[10];
	UBYTE    RtnVal;

	RtnVal = 0;
	return(RtnVal);
}


void      I2cSetPIC_RST(void)
{
	UBYTE   Set[2];

	Set[0] = SET;
	Set[1] = PIC_RST;
}


void      I2cClearPIC_RST(void)
{
	UBYTE   Clr[2];

	Clr[0] = CLEAR;
	Clr[1] = PIC_RST;
}


void      I2cSetPIC_EN(void)
{
	UBYTE   Set[2];

	Set[0] = SET;
	Set[1] = PIC_EN;
}


void      I2cClearPIC_EN(void)
{
	UBYTE   Clr[2];

	Clr[0] = CLEAR;
	Clr[1] = PIC_EN;
}


void      I2cHiImpPIC_EN(void)
{
	UBYTE   HiImp[2];

	HiImp[0] = HIIMP;
	HiImp[1] = PIC_EN;
}


UBYTE     I2cReadStatus(UBYTE* pBuf)
{
	return(I2cRead(I2cFile, READ_STATUS, pBuf, 3));
}


// read len bytes stored in the device at address addr in array buf
// return -1 on error, 0 on success
int I2cRead(int fd, unsigned int addr, unsigned char* buf, unsigned char len)
{
	int i;

	if (len > I2CBUF_SIZE)
	{
#ifdef DEBUG
		fprintf(stderr, "I can only write I2CBUF_SIZE bytes at a time!\n");
#endif
		Status = MODE2_FALIURE;
		return -1;
	}
	return 0;
}


int I2cWrite(int fd, unsigned int addr, unsigned char* buf, unsigned char len)
{
	int    i;
	return(len);
}


// Fill in as many bytes as possible - return value is the number of bytes transferred
UWORD     DataToMode2Decoding(UBYTE* pBuf, UWORD Length)
{
	UWORD   BytesAccepted;
	UWORD   BytesFreeInBuf;
	UWORD   BytesTransferred;
	UWORD   Cnt;

	BytesAccepted = 0;

	// If buffer is empty if can accept more bytes otherwise it will wait accepting bytes
	BytesFreeInBuf = BUFBytesFree;
	if (BytesFreeInBuf)
	{
		if (Length > BytesFreeInBuf)
		{
			BytesTransferred = BytesFreeInBuf;
		}
		else
		{
			BytesTransferred = Length;
		}

		for (Cnt = 0; Cnt < BytesTransferred; Cnt++)
		{
			Mode2InBuf.Buf[Mode2InBuf.InPtr] = pBuf[Cnt];
			BUFAddInPtr(1);
		}
		BytesAccepted = BytesTransferred;
	}
	return(BytesAccepted);
}


UBYTE     I2cGetBootStatus(void)
{
	return(Status);
}








