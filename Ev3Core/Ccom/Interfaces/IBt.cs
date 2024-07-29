using Ev3Core.Helpers;

namespace Ev3Core.Ccom.Interfaces
{
	public interface IBt
	{
		void IncommingConnect();
		void BtInit(char[] pName);
		void BtExit();
		void BtUpdate();
		void BtTxMsgs();
		UBYTE cBtConnect(UBYTE[] pName);
		UBYTE cBtDisconnect(UBYTE[] pName);
		UBYTE cBtDiscChNo(UBYTE ChNo);

		UWORD cBtReadCh0(UBYTE[] pBuf, UWORD Length);
		UWORD cBtReadCh1(UBYTE[] pBuf, UWORD Length);
		UWORD cBtReadCh2(UBYTE[] pBuf, UWORD Length);
		UWORD cBtReadCh3(UBYTE[] pBuf, UWORD Length);
		UWORD cBtReadCh4(UBYTE[] pBuf, UWORD Length);
		UWORD cBtReadCh5(UBYTE[] pBuf, UWORD Length);
		UWORD cBtReadCh6(UBYTE[] pBuf, UWORD Length);
		UWORD cBtReadCh7(UBYTE[] pBuf, UWORD Length);

		UWORD cBtDevWriteBuf(UBYTE[] pBuf, UWORD Size);
		UWORD cBtDevWriteBuf1(UBYTE[] pBuf, UWORD Size);
		UWORD cBtDevWriteBuf2(UBYTE[] pBuf, UWORD Size);
		UWORD cBtDevWriteBuf3(UBYTE[] pBuf, UWORD Size);
		UWORD cBtDevWriteBuf4(UBYTE[] pBuf, UWORD Size);
		UWORD cBtDevWriteBuf5(UBYTE[] pBuf, UWORD Size);
		UWORD cBtDevWriteBuf6(UBYTE[] pBuf, UWORD Size);
		UWORD cBtDevWriteBuf7(UBYTE[] pBuf, UWORD Size);

		UBYTE cBtI2cBufReady();
		UWORD cBtI2cToBtBuf(UBYTE[] pBuf, UWORD Size);

		// Generic Bluetooth commands
		UBYTE BtSetVisibility(UBYTE State);
		UBYTE BtGetVisibility();
		UBYTE BtSetOnOff(UBYTE On);
		UBYTE BtGetOnOff(ref UBYTE On);
		UBYTE BtSetMode2(UBYTE Mode2);
		UBYTE BtGetMode2(ref UBYTE pMode2);
		UBYTE BtStartScan();
		UBYTE BtStopScan();
		UBYTE cBtGetNoOfConnListEntries();
		UBYTE cBtGetConnListEntry(UBYTE Item, UBYTE[] pName, SBYTE Length, ref UBYTE pType);
		UBYTE cBtGetNoOfDevListEntries();
		UBYTE cBtGetDevListEntry(UBYTE Item, ref SBYTE pConnected, ref SBYTE pType, UBYTE[] pName, SBYTE Length);
		UBYTE cBtDeleteFavourItem(UBYTE[] pName);
		UBYTE cBtGetNoOfSearchListEntries();
		UBYTE cBtGetSearchListEntry(UBYTE Item, ref SBYTE pConnected, ref SBYTE pType, ref SBYTE pParred, UBYTE[] pName, SBYTE Length);
		UBYTE cBtGetHciBusyFlag();
		void DecodeMode1(UBYTE BufNo);

		UBYTE cBtGetStatus();
		void cBtGetId(ref UBYTE pId, UBYTE Length);
		UBYTE cBtSetName(UBYTE[] pName, UBYTE Length);
		UBYTE cBtGetChNo(UBYTE[] pName, ref UBYTE pChNo);
		void cBtGetIncoming(UBYTE[] pName, ref UBYTE pCod, UBYTE Len);
		UBYTE cBtGetEvent();
		UBYTE cBtSetPin(ref UBYTE pPin);
		UBYTE cBtSetPasskey(UBYTE Accept);

		void cBtSetTrustedDev(ref UBYTE pBtAddr, ref UBYTE pPin, UBYTE PinSize);

		UWORD cBtSetBundleId(ref UBYTE pId);
		UWORD cBtSetBundleSeedId(ref UBYTE pSeedId);
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
