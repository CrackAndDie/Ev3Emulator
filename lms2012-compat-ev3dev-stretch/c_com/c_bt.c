/*
 * LEGO® MINDSTORMS EV3
 *
 * Copyright (C) 2010-2013 The LEGO Group
 * Copyright (C) 2016 David Lechner <david@lechnology.com>
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


 /*! \page BluetoothPart Bluetooth
  *
  *- \subpage  InternalBtFunctionality
  *
  */


  /*! \page InternalBtFunctionality Bluetooth description
   *
   *  <hr size="1"/>
   *
	\verbatim

	 Support of up to 7 connection - either 7 outgoing or 1 incoming connection.

	 Scatter-net is not supported. If connected from outside (by bluetooth) then
	 the  connection is closed if an outgoing connection is made from the brick.

	 Pin agent is used to handle pairing.

	 DBUS is used to handle connection creation.

	 Sockets are used to communicate with the remote devices.


	 Bluetooth buffer setup
	 ----------------------

	 RX side:

	 Bluetooth socket
		  |
		   --> c_bt RxBuf - Fragmented async. data bytes
				   |
					--> c_bt Msg buffer  - Collected as complete LEGO protocol messages
							   |
								--> c_com rx buffer - Transferred to c_com level for interpretation


	 Tx side:

	 c_com tx buffer
		   |
			--> c_bt WriteBuf Buffer
						|
						 -->  Bluetooth socket





	 In mode2:
	 ---------

	 RX:

	 Bluetooth socket
		   |
			--> Mode2Buf - Fragmented data bytes (c_bt.c)
					|
					 --> Mode2InBuf - Fragmented data bytes (c_i2c.c)
							  |
							   --> Transfer to Mode2 decoding
									  |
									   --> READBUF (return bytes from mode2 decoding) -> for mode1 decoding
									  |
									  |
									   --> WriteBuf (return bytes from mode2 decoding) -> for tx to remote mode2 device


	 TX:

	 Mode2WriteBuf   (c_bt.c)
		 |
		  --> Transfer to mode2 decoding  (data read in c_i2c.c)
				 |
				  --> WriteBuf (return bytes from mode2 decoding) -> for tx  (c_bt.c)
											  |
											   -->  Bluetooth socket



	 CONNECTION MANAGEMENT
	 ---------------------


	   CONNECTING:

	   Connecting brick:                                        Remote brick:

	   Connect to brick (Issued from byte codes)     |
	   - Set busy flag                               |
	   - Disable page inquiry                        |
	   - Open socket                           --->  |  --->    EVT_CONN_REQUEST
													 |          - Issue remote name request
													 |
	   Optional pin/passkey exchange (agent)   <-->  |  <-->    optional pin/passkey exchange (agent)
													 |
	   EVT_CONN_COMPLETE                       <---  |  --->    EVT_CONN_COMPLETE
	   - Disable page inquiry                        |          - Disable page inquiry        (Cannot be connected to more than one, as a slave)
	   - Update Device list                          |          - Insert all info in dev list (Except connected)
	   - Update Search list                          |
													 |
	   Socket write ready (Remote socket open) <---  |  --->    Success on accept listen socket (Socket gives remote address)
	   - NoOfConnDevices++                           |          - Set slave mode
	   - Update Search list to connected             |          - NoOfConnDevices++
	   - Update Device list to connected             |          - Update Device list to connected
													 |          - Update Search list to connected
													 |


	   DISCONNECTING:

	   Disconnecting brick:                          |          Remote brick:
													 |
	   Disconnect  (Issued from byte codes)          |
	   - Close bluetooth socket                --->  |  --->    Socket indicates remote socket closed
													 |          - Close socket
													 |
													 |
	   EVT_DISCONN_COMPLETE                    <---  |  --->    EVT_DISCONN_COMPLETE
	   - Update Search list to disconnected          |          - Update Search list to disconnected
	   - Update Device list to disconnected          |          - Update Device list to disconnected
	   - NoofConnDevices--                           |          - NoofConnDevices--
	   - If NoofConnDevices = 0 -> set idle mode     |          - If NoofConnDevices = 0 -> set idle mode

	\endverbatim
	*/

#include <fcntl.h>
#include <string.h>

#include "lms2012.h"
#include "c_com.h"
#include "c_bt.h"
#include "c_i2c.h"

#ifdef DEBUG_C_BT
#define DEBUG
#endif

#define C_BT_AGENT_PATH         "/org/ev3dev/lms2012/bluez/agent"
#define C_BT_SPP_PROFILE_PATH   "/org/ev3dev/lms2012/bluez/spp_profile"
#define NONVOL_BT_DATA          "settings/nonvolbt"
#define MAX_DEV_TABLE_ENTRIES   30
#define MAX_BT_NAME_SIZE        248

#define SPP_UUID                "00001101-0000-1000-8000-00805f9b34fb"
#define SVC_UUID                "00000000-deca-fade-deca-deafdecacaff"

enum {
	I_AM_IN_IDLE = 0,
	I_AM_MASTER = 1,
	I_AM_SLAVE = 2,
	I_AM_SCANNING = 3,
	STOP_SCANNING = 4,
	TURN_ON = 5,
	TURN_OFF = 6,
	RESTART = 7,
	BLUETOOTH_OFF = 8
};

enum {
	MSG_BUF_EMPTY = 0,
	MSG_BUF_LEN = 1,
	MSG_BUF_BODY = 2,
	MSG_BUF_FULL = 3
};

// Defines related to Channels
enum {
	CH_CONNECTING,
	CH_FREE,
	CH_CONNECTED
};

/* Constants related to Decode mode */
enum {
	MODE1,      // normal
	MODE2,      // iPhone/iPad/iPod
};

// Communication sockets
typedef struct {
	SLONG     Socket;
} BTSOCKET;

typedef struct {
	WRITEBUF  WriteBuf;
	READBUF   ReadBuf;
	MSGBUF    MsgBuf;
	BTSOCKET  BtSocket;
	UBYTE     Status;
} BTCHANNEL;

// This is serialized. Don't change unless absolutely necessary.
typedef struct {
	UBYTE       DecodeMode;
	char        BundleID[MAX_BUNDLE_ID_SIZE];
	char        BundleSeedID[MAX_BUNDLE_SEED_ID_SIZE];
} NONVOLBT;

typedef struct {
	UBYTE     ChNo;
}OUTGOING;

typedef struct {
	BTCHANNEL     BtCh[NO_OF_BT_CHS];   // Communication sockets
	READBUF       Mode2Buf;
	WRITEBUF      Mode2WriteBuf;

	OUTGOING      OutGoing;
	char          Adr[13];
	UBYTE         SearchIndex;
	UBYTE         NoOfFoundDev;
	UBYTE         PageState;
	UBYTE         NoOfConnDevs;

	SLONG         State;
	SLONG         OldState;
	ULONG         Delay;
	NONVOLBT      NonVol;
	COM_EVENT     Events;
	UBYTE         DecodeMode;
	char          BtName[vmBRICKNAMESIZE];
} BT_GLOBALS;

static BT_GLOBALS BtInstance;


static void BtCloseBtSocket(SLONG* pBtSocket)
{
	
}

static void BtCloseCh(UBYTE ChIndex)
{
	
}

static void BtDisconnectAll(void)
{
	
}

/**
 * @brief               Start scanning for bluetooth devices.
 *
 * @return              OK on success, otherwise FAIL.
 */
UBYTE BtStartScan(void)
{
	return OK;
}

/**
 * @brief               Stop scanning for bluetooth devices.
 *
 * @return              OK on success, otherwise FAIL.
 */
UBYTE BtStopScan(void)
{
	return OK;
}

static UWORD cBtRead(DATA8 ch, UBYTE* pBuf, UWORD Length)
{
	MSGBUF* pMsgBuf;
	UWORD RtnLen = 0;

	pMsgBuf = &BtInstance.BtCh[ch].MsgBuf;

	return RtnLen;
}

UWORD cBtReadCh0(UBYTE* pBuf, UWORD Length)
{
	return cBtRead(0, pBuf, Length);
}

UWORD cBtReadCh1(UBYTE* pBuf, UWORD Length)
{
	return cBtRead(1, pBuf, Length);
}

UWORD cBtReadCh2(UBYTE* pBuf, UWORD Length)
{
	return cBtRead(2, pBuf, Length);
}

UWORD cBtReadCh3(UBYTE* pBuf, UWORD Length)
{
	return cBtRead(3, pBuf, Length);
}

UWORD cBtReadCh4(UBYTE* pBuf, UWORD Length)
{
	return cBtRead(4, pBuf, Length);
}

UWORD cBtReadCh5(UBYTE* pBuf, UWORD Length)
{
	return cBtRead(5, pBuf, Length);
}

UWORD cBtReadCh6(UBYTE* pBuf, UWORD Length)
{
	return cBtRead(6, pBuf, Length);
}

UWORD cBtReadCh7(UBYTE* pBuf, UWORD Length)
{
	return cBtRead(7, pBuf, Length);
}

void      DecodeMode2(void)
{
	UWORD   BytesAccepted;
	SLONG   AvailBytes;

	BtInstance.Mode2Buf.OutPtr = 0;
	BtInstance.Mode2Buf.InPtr = 0;
	BtInstance.Mode2Buf.Status = READ_BUF_EMPTY;
}


void      DecodeMode1(UBYTE BufNo)
{
	SLONG   AvailBytes;
	READBUF* pReadBuf;
	MSGBUF* pMsgBuf;

#ifdef  DEBUG
	SLONG   Test;
#endif

	/* 1. Check if there is more data to interpret */
	/* 2. Check the status of the active buffer    */
	pReadBuf = &(BtInstance.BtCh[BufNo].ReadBuf); // Source buffer
	pMsgBuf = &(BtInstance.BtCh[BufNo].MsgBuf);  // Destination Buffer

	AvailBytes = (pReadBuf->InPtr - pReadBuf->OutPtr);          /* How many bytes is ready to be read */

#ifdef DEBUG
	printf("\nDecode mode 1: Avail bytes = %d MsgBuf status = %d\n", AvailBytes, pMsgBuf->Status);
#endif

	switch (pMsgBuf->Status)
	{
	case MSG_BUF_EMPTY:
	{
		// Message buffer is empty
		pMsgBuf->InPtr = 0;

		if (TRUE == pMsgBuf->LargeMsg)
		{
			pMsgBuf->Status = MSG_BUF_BODY;
		}
		else
		{
			if (2 <= AvailBytes)
			{
				memcpy(&(pMsgBuf->Buf[pMsgBuf->InPtr]), &(pReadBuf->Buf[pReadBuf->OutPtr]), 2);
				pMsgBuf->InPtr += 2;
				pReadBuf->OutPtr += 2;
				AvailBytes -= 2;

				pMsgBuf->RemMsgLen = (int)(pMsgBuf->Buf[0]) + ((int)(pMsgBuf->Buf[1]) * 256);
				pMsgBuf->MsgLen = pMsgBuf->RemMsgLen;

				if (0 != pMsgBuf->RemMsgLen)
				{

					if (pMsgBuf->RemMsgLen <= AvailBytes)
					{
						// Rest of message is received move it to the message buffer
						memcpy(&(pMsgBuf->Buf[pMsgBuf->InPtr]), &(pReadBuf->Buf[pReadBuf->OutPtr]), (pMsgBuf->RemMsgLen));

						AvailBytes -= (pMsgBuf->RemMsgLen);
						pReadBuf->OutPtr += (pMsgBuf->RemMsgLen);
						pMsgBuf->InPtr += (pMsgBuf->RemMsgLen);
						pMsgBuf->Status = MSG_BUF_FULL;

#ifdef DEBUG
						printf(" Message is received from MSG_BUF_EMPTY: ");
						for (Test = 0; Test < ((pMsgBuf->MsgLen) + 2); Test++)
						{
							printf("%02X ", pMsgBuf->Buf[Test]);
						}
						printf("\n");
#endif

						if (0 == AvailBytes)
						{
							// Read buffer is empty
							pReadBuf->OutPtr = 0;
							pReadBuf->InPtr = 0;
							pReadBuf->Status = READ_BUF_EMPTY;
						}
					}
					else
					{
						// Still some bytes needed to be received
						// So Read buffer is emptied
						memcpy(&(pMsgBuf->Buf[pMsgBuf->InPtr]), &(pReadBuf->Buf[pReadBuf->OutPtr]), AvailBytes);

						pMsgBuf->Status = MSG_BUF_BODY;
						pMsgBuf->RemMsgLen -= AvailBytes;
						pMsgBuf->InPtr += AvailBytes;

						pReadBuf->OutPtr = 0;
						pReadBuf->InPtr = 0;
						pReadBuf->Status = READ_BUF_EMPTY;
					}
				}
				else
				{
					if (0 == AvailBytes)
					{
						// Read buffer is empty
						pReadBuf->OutPtr = 0;
						pReadBuf->InPtr = 0;
						pReadBuf->Status = READ_BUF_EMPTY;
					}
				}
			}
			else
			{
				// Only one byte has been received - first byte of length info
				memcpy(&(pMsgBuf->Buf[pMsgBuf->InPtr]), &(pReadBuf->Buf[pReadBuf->OutPtr]), 1);
				pReadBuf->OutPtr++;
				pMsgBuf->InPtr++;

				pMsgBuf->RemMsgLen = (int)(pMsgBuf->Buf[0]);
				pMsgBuf->Status = MSG_BUF_LEN;

				pReadBuf->OutPtr = 0;
				pReadBuf->InPtr = 0;
				pReadBuf->Status = READ_BUF_EMPTY;
			}
		}
	}
	break;

	case MSG_BUF_LEN:
	{
		// Read the last length bytes
		memcpy(&(pMsgBuf->Buf[pMsgBuf->InPtr]), &(pReadBuf->Buf[pReadBuf->OutPtr]), 1);
		pMsgBuf->InPtr++;
		pReadBuf->OutPtr++;
		AvailBytes--;

		pMsgBuf->RemMsgLen = (int)(pMsgBuf->Buf[0]) + ((int)(pMsgBuf->Buf[1]) * 256);
		pMsgBuf->MsgLen = pMsgBuf->RemMsgLen;

		if (0 != pMsgBuf->RemMsgLen)
		{

			if ((pMsgBuf->RemMsgLen) <= AvailBytes)
			{
				// rest of message is received move it to the message buffer
				memcpy(&(pMsgBuf->Buf[pMsgBuf->InPtr]), &(pReadBuf->Buf[pReadBuf->OutPtr]), (pMsgBuf->RemMsgLen));

				AvailBytes -= (pMsgBuf->RemMsgLen);
				pReadBuf->OutPtr += (pMsgBuf->RemMsgLen);
				pMsgBuf->InPtr += (pMsgBuf->RemMsgLen);
				pMsgBuf->Status = MSG_BUF_FULL;

#ifdef DEBUG
				printf(" Message is received from MSG_BUF_EMPTY: ");
				for (Test = 0; Test < ((pMsgBuf->MsgLen) + 2); Test++)
				{
					printf("%02X ", pMsgBuf->Buf[Test]);
				}
				printf("\n");
#endif

				if (0 == AvailBytes)
				{
					// Read buffer is empty
					pReadBuf->OutPtr = 0;
					pReadBuf->InPtr = 0;
					pReadBuf->Status = READ_BUF_EMPTY;
				}
			}
			else
			{
				// Still some bytes needed to be received
				// So receive buffer is emptied
				memcpy(&(pMsgBuf->Buf[pMsgBuf->InPtr]), &(pReadBuf->Buf[pReadBuf->OutPtr]), AvailBytes);

				pMsgBuf->Status = MSG_BUF_BODY;
				pMsgBuf->RemMsgLen -= AvailBytes;
				pMsgBuf->InPtr += AvailBytes;

				pReadBuf->OutPtr = 0;
				pReadBuf->InPtr = 0;
				pReadBuf->Status = READ_BUF_EMPTY;
			}
		}
		else
		{
			if (0 == AvailBytes)
			{
				// Read buffer is empty
				pReadBuf->OutPtr = 0;
				pReadBuf->InPtr = 0;
				pReadBuf->Status = READ_BUF_EMPTY;

				pMsgBuf->Status = MSG_BUF_EMPTY;
			}
		}
	}
	break;

	case MSG_BUF_BODY:
	{
		ULONG  BufFree;

		BufFree = (sizeof(pMsgBuf->Buf) - (pMsgBuf->InPtr));
		if (BufFree < (pMsgBuf->RemMsgLen))
		{

			pMsgBuf->LargeMsg = TRUE;

			//This is large message
			if (BufFree >= AvailBytes)
			{
				//The available bytes can be included in this buffer
				memcpy(&(pMsgBuf->Buf[pMsgBuf->InPtr]), &(pReadBuf->Buf[pReadBuf->OutPtr]), AvailBytes);

				//Buffer is still not full
				pMsgBuf->Status = MSG_BUF_BODY;
				pMsgBuf->RemMsgLen -= AvailBytes;
				pMsgBuf->InPtr += AvailBytes;

				//Readbuffer has been completely emptied
				pReadBuf->OutPtr = 0;
				pReadBuf->InPtr = 0;
				pReadBuf->Status = READ_BUF_EMPTY;
			}
			else
			{
				//The available bytes cannot all be included in the buffer
				memcpy(&(pMsgBuf->Buf[pMsgBuf->InPtr]), &(pReadBuf->Buf[pReadBuf->OutPtr]), BufFree);
				pReadBuf->OutPtr += BufFree;
				pMsgBuf->InPtr += BufFree;
				pMsgBuf->RemMsgLen -= BufFree;
				pMsgBuf->Status = MSG_BUF_FULL;
			}
		}
		else
		{
			pMsgBuf->LargeMsg = FALSE;

			if ((pMsgBuf->RemMsgLen) <= AvailBytes)
			{
				// rest of message is received move it to the message buffer
				memcpy(&(pMsgBuf->Buf[pMsgBuf->InPtr]), &(pReadBuf->Buf[pReadBuf->OutPtr]), (pMsgBuf->RemMsgLen));

				AvailBytes -= (pMsgBuf->RemMsgLen);
				pReadBuf->OutPtr += (pMsgBuf->RemMsgLen);
				pMsgBuf->InPtr += (pMsgBuf->RemMsgLen);
				pMsgBuf->Status = MSG_BUF_FULL;

#ifdef DEBUG
				printf(" Message is received from MSG_BUF_EMPTY: ");
				for (Test = 0; Test < ((pMsgBuf->MsgLen) + 2); Test++)
				{
					printf("%02X ", pMsgBuf->Buf[Test]);
				}
				printf("\n");
#endif

				if (0 == AvailBytes)
				{
					// Read buffer is empty
					pReadBuf->OutPtr = 0;
					pReadBuf->InPtr = 0;
					pReadBuf->Status = READ_BUF_EMPTY;
				}
			}
			else
			{
				// Still some bytes needed to be received
				// So receive buffer is emptied
				memcpy(&(pMsgBuf->Buf[pMsgBuf->InPtr]), &(pReadBuf->Buf[pReadBuf->OutPtr]), AvailBytes);

				pMsgBuf->Status = MSG_BUF_BODY;
				pMsgBuf->RemMsgLen -= AvailBytes;
				pMsgBuf->InPtr += AvailBytes;

				pReadBuf->OutPtr = 0;
				pReadBuf->InPtr = 0;
				pReadBuf->Status = READ_BUF_EMPTY;
			}
		}
	}
	break;

	case MSG_BUF_FULL:
	{
	}
	break;

	default:
	{
	}
	break;
	}
}

static void DecodeBtStream(UBYTE BufNo)
{
	if (BtInstance.NonVol.DecodeMode == MODE1) {
		DecodeMode1(BufNo);
	}
	else {
		DecodeMode2();
	}
}

static void BtClose(void)
{
	BtDisconnectAll();

	// if (MIN_HANDLE <= BtInstance.HciSocket.Socket)
	// {
	//   ioctl(BtInstance.HciSocket.Socket, HCIDEVDOWN, 0);
	//   hci_close_dev(BtInstance.HciSocket.Socket);
	//   BtInstance.HciSocket.Socket = -1;
	// }

	I2cStop();
}

static void BtTxMsgs(void)
{
	WRITEBUF* pWriteBuf;
	UWORD     ByteCnt;
	UWORD BytesWritten = 0;

	pWriteBuf = &BtInstance.BtCh[0].WriteBuf;

	ByteCnt = pWriteBuf->InPtr - pWriteBuf->OutPtr;

	if (!ByteCnt) {
		return;
	}

#ifdef DEBUG
		printf("transmitted Bytes to send %d, Bytes written = %d\n",
			ByteCnt, BytesWritten);
		printf(" errno = %d\n", errno);
#endif

	if (pWriteBuf->OutPtr == pWriteBuf->InPtr) {
		// All bytes has been written - clear the buffer
		pWriteBuf->InPtr = 0;
		pWriteBuf->OutPtr = 0;
	}
}

void BtUpdate(void)
{
	BtTxMsgs();
}

/**
 * @brief           Sets the mode2 state.
 *
 * @param Mode2     The new state.
 *
 * @return          OK on success, otherwise FAIL
 */
RESULT BtSetMode2(UBYTE Mode2)
{
	// TODO: Need an iDevice to test mode2

	return FAIL;
}

/**
 * @brief           Gets the mode2 state.
 *
 * @param pMode2    Pointer to hold the result.
 *
 * @return          OK.
 */
RESULT BtGetMode2(UBYTE* pMode2)
{
	*pMode2 = BtInstance.NonVol.DecodeMode;

	return OK;
}

/**
 * @brief           Set the power state of the bluetooth adapter.
 *
 * @param on        TRUE to turn on or FALSE to turn off.
 *
 * @return          OK on success, otherwise FAIL.
 */
RESULT BtSetOnOff(UBYTE on)
{
	return FAIL;
}

/**
 * @brief           Get the power state of the bluetooth adapter.
 *
 * @param on        Pointer to hold the result. Set to TRUE if powered on,
 *                  otherwise FALSE.
 *
 * @return          OK.
 */
RESULT BtGetOnOff(UBYTE* On)
{
	RESULT Result = OK;

	*On = FALSE;

	return Result;
}

/**
 * @brief           Turn on/off bluetooth visibility.
 *
 * @param on        Set to TRUE to make the adapter visible.
 *
 * @return          OK.
 */
RESULT BtSetVisibility(UBYTE on)
{
	return OK;
}

/**
 * @brief           Check if Bluetooth is visible to other devices.
 *
 * @return          TRUE if visible, FALSE otherwise.
 */
UBYTE BtGetVisibility(void)
{
	return FALSE;
}

/**
 * @brief               Connect to bluetooth device.
 *
 * This will return FAIL if a connection is already in progress or the device
 * is not found.
 *
 * @param pName         The name of the device.
 *
 * @return              OK on success, otherwise FAIL.
 */
RESULT cBtConnect(const char* pName)
{
	return FAIL;
}

UBYTE cBtDiscChNo(UBYTE ChNo)
{
	UBYTE   RtnVal;
	// UBYTE   TmpCnt;

	RtnVal = FALSE;
	// for(TmpCnt = 0; TmpCnt < MAX_DEV_TABLE_ENTRIES; TmpCnt++)
	// {
	//   if ((TRUE == BtInstance.NonVol.DevList[TmpCnt].Connected) && (ChNo == BtInstance.NonVol.DevList[TmpCnt].ChNo))
	//   {
	//     cBtDiscDevIndex(TmpCnt);
	//     TmpCnt = MAX_DEV_TABLE_ENTRIES;
	//     RtnVal = TRUE;
	//   }
	// }
	return(RtnVal);
}

/**
 * @brief               Disconnect a bluetooth device.
 *
 * @param pName         The name of the device to disconnect.
 *
 * @result              OK on success, otherwise FAIL.
 */
RESULT cBtDisconnect(const char* pName)
{
	return FAIL;
}

UBYTE     cBtI2cBufReady(void)
{
	UBYTE   RtnVal;

	RtnVal = 1;
	if (0 != BtInstance.BtCh[0].WriteBuf.InPtr)
	{
		RtnVal = 0;
	}
	return(RtnVal);
}


UWORD     cBtI2cToBtBuf(UBYTE* pBuf, UWORD Size)
{
	if (0 == BtInstance.BtCh[0].WriteBuf.InPtr)
	{
		memcpy(BtInstance.BtCh[0].WriteBuf.Buf, pBuf, Size);
		BtInstance.BtCh[0].WriteBuf.InPtr = Size;
	}
	return(Size);
}

static UWORD cBtDevWriteBuf(UBYTE index, UBYTE* pBuf, UWORD Size)
{
	if (BtInstance.BtCh[index].WriteBuf.InPtr == 0) {
		memcpy(BtInstance.BtCh[index].WriteBuf.Buf, pBuf, Size);
		BtInstance.BtCh[index].WriteBuf.InPtr = Size;
	}
	else {
		Size = 0;
	}

	return Size;
}

UWORD cBtDevWriteBuf0(UBYTE* pBuf, UWORD Size)
{
	if (MODE2 == BtInstance.NonVol.DecodeMode) {
		if (0 == BtInstance.Mode2WriteBuf.InPtr) {
			memcpy(BtInstance.Mode2WriteBuf.Buf, pBuf, Size);
			BtInstance.Mode2WriteBuf.InPtr = Size;
		}
		else {
			Size = 0;
		}
	}
	else {
		Size = cBtDevWriteBuf(0, pBuf, Size);
	}

	return Size;
}

UWORD cBtDevWriteBuf1(UBYTE* pBuf, UWORD Size)
{
	return cBtDevWriteBuf(1, pBuf, Size);
}

UWORD cBtDevWriteBuf2(UBYTE* pBuf, UWORD Size)
{
	return cBtDevWriteBuf(2, pBuf, Size);
}

UWORD cBtDevWriteBuf3(UBYTE* pBuf, UWORD Size)
{
	return cBtDevWriteBuf(3, pBuf, Size);
}

UWORD cBtDevWriteBuf4(UBYTE* pBuf, UWORD Size)
{
	return cBtDevWriteBuf(4, pBuf, Size);
}

UWORD cBtDevWriteBuf5(UBYTE* pBuf, UWORD Size)
{
	return cBtDevWriteBuf(5, pBuf, Size);
}

UWORD cBtDevWriteBuf6(UBYTE* pBuf, UWORD Size)
{
	return cBtDevWriteBuf(6, pBuf, Size);
}

UWORD cBtDevWriteBuf7(UBYTE* pBuf, UWORD Size)
{
	return cBtDevWriteBuf(7, pBuf, Size);
}

/**
 * @brief               Gets the bluetooth instance state.
 *
 * @return              OK if bluetooth is idle, BUSY if connect/disconnect
 *                      is in progress or FAIL if there was an error.
 */
RESULT cBtGetHciBusyFlag(void)
{
	return OK;
}

/**
 * @brief               Gets the number of connected devices.
 *
 * @return              The number of devices.
 */
UBYTE cBtGetNoOfConnListEntries(void)
{
	return 0;
}

/**
 * @brief           Gets an entry from the list of connected devices
 *
 * @param Item      The index of the item in the list.
 * @param pName     Preallocated char array to hold the name of the device.
 * @param Length    The length of pName
 * @param pType     Pointer to hold the type of device (icon index)
 *
 * @return          OK on success, otherwise FAIL.
 */
RESULT cBtGetConnListEntry(UBYTE Item, char* pName, SBYTE Length, UBYTE* pType)
{
	return FAIL;
}

/**
 * @brief               Get the length of the list of paired devices.
 *
 * @return              The list length.
 */
UBYTE cBtGetNoOfDevListEntries(void)
{
	return 0;
}

/**
 * @brief               Get info about a paired bluetooth device.
 *
 * @param Item          Index of the item in the paired device list.
 * @param pConnected    Pointer to hold the connected state.
 * @param pType         Pointer to hold the device type (icon).
 * @param pName         Preallocated char array to hold the name of the device.
 * @param Length        The length of pName.
 *
 * @return              OK on success, otherwise FAIL (index out of range).
 */
RESULT cBtGetDevListEntry(UBYTE Item, SBYTE* pConnected, SBYTE* pType,
	char* pName, SBYTE Length)
{
	return FAIL;
}

/**
 * @brief           Get the length of the search device list.
 *
 * @return          The length.
 */
UBYTE cBtGetNoOfSearchListEntries(void)
{
	return 0;
}

/**
 * @brief               Get information from an item in a search list entry.
 *
 * @param Item          The index of the item in the list.
 * @param pConnected    Pointer to hold the connected state.
 * @param pType         Pointer to hold the device type (icon)
 * @param pPaired       Pointer to hold the paired state.
 * @param pName         Preallocated char array to hold the name.
 * @param Length        The length of pName
 *
 * @ return             OK on success, otherwise FAIL (index out of range).
 */
RESULT cBtGetSearchListEntry(UBYTE Item, SBYTE* pConnected, SBYTE* pType,
	SBYTE* pPaired, char* pName, SBYTE Length)
{
	return FAIL;
}

/**
 * @brief               Remove a bluetooth device.
 *
 * @param pName         The name of the device to remove.
 *
 * @return              OK on success, otherwise FAIL.
 */
RESULT cBtRemoveItem(const char* pName)
{
	return FAIL;
}

/**
 * @brief               Gets the status of the bluetooth adapter.
 *
 * Used for the indicator icon. See TopLineBtIconMap in c_ui.c.
 *
 * @return              Flags indicating the status.
 */
UBYTE cBtGetStatus(void)
{
	UBYTE status = 0;
	return status;
}

void      cBtGetId(UBYTE* pId, UBYTE Length)
{
	strncpy((char*)pId, &(BtInstance.Adr[0]), Length);
}

/**
 * @brief               Set the name of the brick
 *
 * @param pName         The new name
 * @param Length        The length of pName
 *
 * @return              OK on success, otherwise FAIL
 */
RESULT cBtSetName(const char* pName, UBYTE Length)
{
	// TODO: set hostname and then send SIGHUP to bluetoothd to reload

	return FAIL;
}

UBYTE cBtGetChNo(UBYTE* pName, UBYTE* pChNos)
{
	// TODO: implement more channels

	return 0;
}

/**
 * @brief               Gets any pending requests.
 *
 * @return              EVENT_BT_PIN or EVENT_BT_REQ_CONF or EVENT_NONE.
 */
COM_EVENT cBtGetEvent(void)
{
	COM_EVENT Evt;

	Evt = BtInstance.Events;
	BtInstance.Events = EVENT_NONE;

	return Evt;
}

/**
 * @brief               Get info about an incoming bluetooth connection request.
 *
 * @param pName         Preallocated char array to hold the name.
 * @param pType         Pointer to hold the device type (icon).
 * @param Length        Length of pName.
 */
void cBtGetIncoming(char* pName, UBYTE* pType, UBYTE Length)
{

}

/**
 * @brief               Complete a pin code agent request.
 *
 * @param pPin          The pin code.
 *
 * @return              FAIL if there was not a pending request, otherwise OK.
 */
RESULT cBtSetPin(const char* pPin)
{
	return FAIL;
}

/**
 * @brief               Complete a request to confirm a passkey
 *
 * @param Accept        TRUE to accept, FALSE to reject
 *
 * @return              FAIL if there was not a pending request, otherwise OK.
 */
RESULT cBtSetPasskey(UBYTE Accept)
{
	return FAIL;
}

void    cBtSetTrustedDev(UBYTE* pBtAddr, UBYTE* pPin, UBYTE PinSize)
{
	// BtInstance.TrustedDev.PinLen = PinSize;
	// snprintf((BtInstance.TrustedDev.Pin), 7, "%s", pPin);
	// cBtStrNoColonToBa(pBtAddr, &(BtInstance.TrustedDev.Adr));
}

/**
 * @brief           Sets the bundle id.
 *
 * @param pId       The new id.
 *
 * @return          TRUE on success, otherwise FALSE (id was too long).
 */
int cBtSetBundleId(const char* pId)
{
	return FALSE;
}

/**
 * @brief           Sets the seed id.
 *
 * @param pSeedId   The new id.
 *
 * @return          TRUE on success, otherwise FALSE (id was too long).
 */
int cBtSetBundleSeedId(const char* pSeedId)
{
	return FALSE;
}

void BtInit(const char* pName)
{
	int     File;
	FILE* FSer;

	// BtInstance.OnOffSeqCnt = 0;

	//Error - Fill with zeros
	strcpy(BtInstance.Adr, "000000000000");

	// Default settings
	BtInstance.NonVol.DecodeMode = MODE1;

	snprintf(BtInstance.NonVol.BundleID, MAX_BUNDLE_ID_SIZE, "%s", LEGO_BUNDLE_ID);
	snprintf(BtInstance.NonVol.BundleSeedID, MAX_BUNDLE_SEED_ID_SIZE, "%s", LEGO_BUNDLE_SEED_ID);

	BtInstance.Events = EVENT_NONE;
	// BtInstance.TrustedDev.Status  =  FALSE;

	BtInstance.BtName[0] = 0;
	snprintf(BtInstance.BtName, vmBRICKNAMESIZE, "%s", pName);
	// bacpy(&(BtInstance.TrustedDev.Adr), BDADDR_ANY);   // BDADDR_ANY = all zeros!

	I2cInit(&BtInstance.BtCh[0].ReadBuf, &BtInstance.Mode2WriteBuf,
		BtInstance.NonVol.BundleID, BtInstance.NonVol.BundleSeedID);
}

void BtExit(void)
{
	I2cExit();
	BtClose();
}
