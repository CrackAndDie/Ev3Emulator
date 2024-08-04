using Ev3Core.Helpers;

namespace Ev3Core.Ccom.Interfaces
{
	public interface IBt
	{
		void IncommingConnect();
		void BtInit(ArrayPointer<UBYTE> pName);
		void BtExit();
		void BtUpdate();
		void BtTxMsgs();
		UBYTE cBtConnect(ArrayPointer<UBYTE> pName);
		UBYTE cBtDisconnect(ArrayPointer<UBYTE> pName);
		UBYTE cBtDiscChNo(UBYTE ChNo);

		UWORD cBtReadCh0(ArrayPointer<UBYTE> pBuf, UWORD Length);
		UWORD cBtReadCh1(ArrayPointer<UBYTE> pBuf, UWORD Length);
		UWORD cBtReadCh2(ArrayPointer<UBYTE> pBuf, UWORD Length);
		UWORD cBtReadCh3(ArrayPointer<UBYTE> pBuf, UWORD Length);
		UWORD cBtReadCh4(ArrayPointer<UBYTE> pBuf, UWORD Length);
		UWORD cBtReadCh5(ArrayPointer<UBYTE> pBuf, UWORD Length);
		UWORD cBtReadCh6(ArrayPointer<UBYTE> pBuf, UWORD Length);
		UWORD cBtReadCh7(ArrayPointer<UBYTE> pBuf, UWORD Length);

		UWORD cBtDevWriteBuf(ArrayPointer<UBYTE> pBuf, UWORD Size);
		UWORD cBtDevWriteBuf1(ArrayPointer<UBYTE> pBuf, UWORD Size);
		UWORD cBtDevWriteBuf2(ArrayPointer<UBYTE> pBuf, UWORD Size);
		UWORD cBtDevWriteBuf3(ArrayPointer<UBYTE> pBuf, UWORD Size);
		UWORD cBtDevWriteBuf4(ArrayPointer<UBYTE> pBuf, UWORD Size);
		UWORD cBtDevWriteBuf5(ArrayPointer<UBYTE> pBuf, UWORD Size);
		UWORD cBtDevWriteBuf6(ArrayPointer<UBYTE> pBuf, UWORD Size);
		UWORD cBtDevWriteBuf7(ArrayPointer<UBYTE> pBuf, UWORD Size);

		UBYTE cBtI2cBufReady();
		UWORD cBtI2cToBtBuf(ArrayPointer<UBYTE> pBuf, UWORD Size);

		// Generic Bluetooth commands
		UBYTE BtSetVisibility(UBYTE State);
		UBYTE BtGetVisibility();
		UBYTE BtSetOnOff(UBYTE On);
		UBYTE BtGetOnOff(VarPointer<UBYTE> On);
		UBYTE BtSetMode2(UBYTE Mode2);
		UBYTE BtGetMode2(VarPointer<UBYTE> pMode2);
		UBYTE BtStartScan();
		UBYTE BtStopScan();
		UBYTE cBtGetNoOfConnListEntries();
		UBYTE cBtGetConnListEntry(UBYTE Item, ArrayPointer<UBYTE> pName, SBYTE Length, VarPointer<UBYTE> pType);
		UBYTE cBtGetNoOfDevListEntries();
		UBYTE cBtGetDevListEntry(UBYTE Item, VarPointer<SBYTE> pConnected, VarPointer<SBYTE> pType, ArrayPointer<UBYTE> pName, SBYTE Length);
		UBYTE cBtDeleteFavourItem(ArrayPointer<UBYTE> pName);
		UBYTE cBtGetNoOfSearchListEntries();
		UBYTE cBtGetSearchListEntry(UBYTE Item, VarPointer<SBYTE> pConnected, VarPointer<SBYTE> pType, VarPointer<SBYTE> pParred, ArrayPointer<UBYTE> pName, SBYTE Length);
		UBYTE cBtGetHciBusyFlag();
		void DecodeMode1(UBYTE BufNo);

		UBYTE cBtGetStatus();
		void cBtGetId(VarPointer<UBYTE> pId, UBYTE Length);
		UBYTE cBtSetName(ArrayPointer<UBYTE> pName, UBYTE Length);
		UBYTE cBtGetChNo(ArrayPointer<UBYTE> pName, VarPointer<UBYTE> pChNo);
		void cBtGetIncoming(ArrayPointer<UBYTE> pName, VarPointer<UBYTE> pCod, UBYTE Len);
		UBYTE cBtGetEvent();
		UBYTE cBtSetPin(VarPointer<UBYTE> pPin);
		UBYTE cBtSetPasskey(UBYTE Accept);

		void cBtSetTrustedDev(VarPointer<UBYTE> pBtAddr, VarPointer<UBYTE> pPin, UBYTE PinSize);

		UWORD cBtSetBundleId(VarPointer<UBYTE> pId);
		UWORD cBtSetBundleSeedId(VarPointer<UBYTE> pSeedId);
	}

	// Buffer to read into from the socket
	public class READBUF
	{
		public UBYTE[] Buf = CommonHelper.Array1d<UBYTE>(1024);
		public UWORD InPtr;
		public UWORD OutPtr;
		public UWORD Status;
	}

	// Buffer to write into from the socket
	public class WRITEBUF
	{
		public UBYTE[] Buf = CommonHelper.Array1d<UBYTE>(1024);
		public UWORD InPtr;
		public UWORD OutPtr;
	}
}
