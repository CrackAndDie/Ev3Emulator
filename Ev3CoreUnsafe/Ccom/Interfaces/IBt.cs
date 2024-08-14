using Ev3CoreUnsafe.Helpers;
using System.Runtime.InteropServices;
using static Ev3CoreUnsafe.Defines;

namespace Ev3CoreUnsafe.Ccom.Interfaces
{
	public unsafe interface IBt
	{
		void IncommingConnect();
		void BtInit(DATA8* pName);
		void BtExit();
		void BtUpdate();
		void BtTxMsgs();
		UBYTE cBtConnect(UBYTE* pName);
		UBYTE cBtDisconnect(UBYTE* pName);
		UBYTE cBtDiscChNo(UBYTE ChNo);

		UWORD cBtReadCh0(UBYTE* pBuf, UWORD Length);
		UWORD cBtReadCh1(UBYTE* pBuf, UWORD Length);
		UWORD cBtReadCh2(UBYTE* pBuf, UWORD Length);
		UWORD cBtReadCh3(UBYTE* pBuf, UWORD Length);
		UWORD cBtReadCh4(UBYTE* pBuf, UWORD Length);
		UWORD cBtReadCh5(UBYTE* pBuf, UWORD Length);
		UWORD cBtReadCh6(UBYTE* pBuf, UWORD Length);
		UWORD cBtReadCh7(UBYTE* pBuf, UWORD Length);

		UWORD cBtDevWriteBuf(UBYTE* pBuf, UWORD Size);
		UWORD cBtDevWriteBuf1(UBYTE* pBuf, UWORD Size);
		UWORD cBtDevWriteBuf2(UBYTE* pBuf, UWORD Size);
		UWORD cBtDevWriteBuf3(UBYTE* pBuf, UWORD Size);
		UWORD cBtDevWriteBuf4(UBYTE* pBuf, UWORD Size);
		UWORD cBtDevWriteBuf5(UBYTE* pBuf, UWORD Size);
		UWORD cBtDevWriteBuf6(UBYTE* pBuf, UWORD Size);
		UWORD cBtDevWriteBuf7(UBYTE* pBuf, UWORD Size);

		UBYTE cBtI2cBufReady();
		UWORD cBtI2cToBtBuf(UBYTE* pBuf, UWORD Size);

		// Generic Bluetooth commands
		UBYTE BtSetVisibility(UBYTE State);
		UBYTE BtGetVisibility();
		UBYTE BtSetOnOff(UBYTE On);
		UBYTE BtGetOnOff(UBYTE* On);
		UBYTE BtSetMode2(UBYTE Mode2);
		UBYTE BtGetMode2(UBYTE* pMode2);
		UBYTE BtStartScan();
		UBYTE BtStopScan();
		UBYTE cBtGetNoOfConnListEntries();
		UBYTE cBtGetConnListEntry(UBYTE Item, UBYTE* pName, SBYTE Length, UBYTE* pType);
		UBYTE cBtGetNoOfDevListEntries();
		UBYTE cBtGetDevListEntry(UBYTE Item, SBYTE* pConnected, SBYTE* pType, UBYTE* pName, SBYTE Length);
		UBYTE cBtDeleteFavourItem(UBYTE* pName);
		UBYTE cBtGetNoOfSearchListEntries();
		UBYTE cBtGetSearchListEntry(UBYTE Item, SBYTE* pConnected, SBYTE* pType, SBYTE* pParred, UBYTE* pName, SBYTE Length);
		UBYTE cBtGetHciBusyFlag();
		void DecodeMode1(UBYTE BufNo);

		UBYTE cBtGetStatus();
		void cBtGetId(UBYTE* pId, UBYTE Length);
		UBYTE cBtSetName(UBYTE* pName, UBYTE Length);
		UBYTE cBtGetChNo(UBYTE* pName, UBYTE* pChNo);
		void cBtGetIncoming(UBYTE* pName, UBYTE* pCod, UBYTE Len);
		UBYTE cBtGetEvent();
		UBYTE cBtSetPin(UBYTE* pPin);
		UBYTE cBtSetPasskey(UBYTE Accept);

		void cBtSetTrustedDev(UBYTE* pBtAddr, UBYTE* pPin, UBYTE PinSize);

		UWORD cBtSetBundleId(UBYTE* pId);
		UWORD cBtSetBundleSeedId(UBYTE* pSeedId);
	}

    // Buffer to read into from the socket
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct READBUF
	{
		public fixed UBYTE Buf[1024];
		public UWORD InPtr;
		public UWORD OutPtr;
		public UWORD Status;
	}

    // Buffer to write into from the socket
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct WRITEBUF
	{
		public fixed UBYTE Buf[1024];
		public UWORD InPtr;
		public UWORD OutPtr;
	}

    // Buffer to fill complete message into from READBUF
    // only one Messages can fit into this buffer
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MSGBUF
	{
		public fixed UBYTE Buf[1024];
		public UWORD InPtr;
		public UWORD MsgLen;
		public UWORD RemMsgLen;
		public UWORD Status;
		public UBYTE LargeMsg;
	}

    // Control socket
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct HCISOCKET
	{
		public SLONG Socket;
		public UBYTE WaitForEvent;
		public UBYTE Busy;
	}

    // Socket to listen for incoming requests
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct LISTENSOCKET
	{
		public SLONG Socket;
		public ULONG opt;
	}

    // Communication sockets
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct BTSOCKET
	{
		public SLONG Socket;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct DEVICELIST
	{
		public fixed DATA8 Name[MAX_BT_NAME_SIZE];
		public UWORD ConnHandle;
		public fixed UBYTE DevClass[3];
		public fixed UBYTE Passkey[6];
		public UBYTE Connected;
		public UBYTE ChNo;
		public UBYTE Status;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SEARCHLIST
	{
		public fixed DATA8 Name[MAX_BT_NAME_SIZE];
		public fixed UBYTE DevClass[3];
		public UBYTE Connected;
		public UBYTE Parred;
		public UBYTE ChNo;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct BTCH
	{
		public WRITEBUF* WriteBuf;
		public READBUF* ReadBuf;
		public MSGBUF* MsgBuf;
		public BTSOCKET* BtSocket;
		public UBYTE* Status;

		public BTCH()
		{
			Init();
		}

		public void Init()
		{
			// structs 
			WriteBuf = CommonHelper.PointerStruct<WRITEBUF>();
			ReadBuf = CommonHelper.PointerStruct<READBUF>();
			MsgBuf = CommonHelper.PointerStruct<MSGBUF>();
			BtSocket = CommonHelper.PointerStruct<BTSOCKET>();
			BtSocket = CommonHelper.PointerStruct<BTSOCKET>();
		}
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NONVOLBT
	{
		public fixed UBYTE DevList[262 * MAX_DEV_TABLE_ENTRIES]; // 262 - is sizeof(DEVICELIST)
		public UBYTE DevListEntries;
		public UBYTE Visible;
		public UBYTE On;
		public UBYTE DecodeMode;
		public fixed UBYTE BundleID[MAX_BUNDLE_ID_SIZE];
		public fixed UBYTE BundleSeedID[MAX_BUNDLE_SEED_ID_SIZE];
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct INCOMMING
	{
		public ULONG Passkey;
		public UWORD ConnHandle;
		public fixed DATA8 Name[MAX_BT_NAME_SIZE];
		public fixed UBYTE DevClass[3];
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct OUTGOING
	{
		public UBYTE ChNo;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct TRUSTED_DEV
	{
		public UBYTE PinLen;
		public fixed UBYTE Pin[10];
		public UBYTE Status;
	}

	public unsafe struct BT_GLOBALS
	{
		//*****************************************************************************
		// Bluetooth Global variables
		//*****************************************************************************

		public HCISOCKET* HciSocket;            // Control socket
		public LISTENSOCKET* ListenSocket;         // Socket to listen for incoming requests
		public BTCH* BtCh;   // Communication sockets
		public READBUF* Mode2Buf;
		public WRITEBUF* Mode2WriteBuf;

		public SEARCHLIST* SearchList;
		public INCOMMING* Incoming;
		public OUTGOING* OutGoing;
		public TRUSTED_DEV* TrustedDev;
		public DATA8* Adr;
		public UBYTE SearchIndex;
		public UBYTE NoOfFoundDev;
		public UBYTE NoOfFoundNames;
		public UBYTE PageState;
		public UBYTE ScanState;
		public UBYTE NoOfConnDevs;

		public SLONG State;
		public SLONG OldState;
		public ULONG Delay;
		public NONVOLBT* NonVol;
		public UBYTE OnOffSeqCnt;
		public UBYTE RestartSeqCnt;
		public UBYTE Events;
		public UBYTE DecodeMode;
		public UBYTE SspPairingMethod;
		public DATA8* BtName;

		public BT_GLOBALS()
		{
			Init();
		}

		public void Init()
		{
			HciSocket = CommonHelper.PointerStruct<HCISOCKET>();
			ListenSocket = CommonHelper.PointerStruct<LISTENSOCKET>();
			Mode2Buf = CommonHelper.PointerStruct<READBUF>();
			Mode2WriteBuf = CommonHelper.PointerStruct<WRITEBUF>();
			Incoming = CommonHelper.PointerStruct<INCOMMING>();
			OutGoing = CommonHelper.PointerStruct<OUTGOING>();
			TrustedDev = CommonHelper.PointerStruct<TRUSTED_DEV>();
			NonVol = CommonHelper.PointerStruct<NONVOLBT>();

			BtCh = CommonHelper.Pointer1d<BTCH>(NO_OF_BT_CHS, true);
			SearchList = CommonHelper.Pointer1d<SEARCHLIST>(MAX_DEV_TABLE_ENTRIES, true);
			Adr = CommonHelper.Pointer1d<DATA8>(13);
			BtName = CommonHelper.Pointer1d<DATA8>(vmBRICKNAMESIZE);
		}
	}
}
