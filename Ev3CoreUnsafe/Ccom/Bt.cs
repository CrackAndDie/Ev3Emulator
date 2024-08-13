using Ev3CoreUnsafe.Ccom.Interfaces;

namespace Ev3CoreUnsafe.Ccom
{
    public unsafe class Bt : IBt
    {
        public void BtExit()
        {
            GH.Ev3System.Logger.LogInfo("BtExit called");
        }

        public byte BtGetMode2(byte* pMode2)
        {
            throw new NotImplementedException();
        }

        public byte BtGetOnOff(byte* On)
        {
            throw new NotImplementedException();
        }

        public byte BtGetVisibility()
        {
            throw new NotImplementedException();
        }

        public void BtInit(sbyte* pName)
        {
            GH.Ev3System.Logger.LogInfo("BT INIT CALLED");
        }

        public byte BtSetMode2(byte Mode2)
        {
            throw new NotImplementedException();
        }

        public byte BtSetOnOff(byte On)
        {
            throw new NotImplementedException();
        }

        public byte BtSetVisibility(byte State)
        {
            throw new NotImplementedException();
        }

        public byte BtStartScan()
        {
            throw new NotImplementedException();
        }

        public byte BtStopScan()
        {
            throw new NotImplementedException();
        }

        public void BtTxMsgs()
        {
            throw new NotImplementedException();
        }

        public void BtUpdate()
        {
            GH.Ev3System.Logger.LogInfo("Bt update called");
        }

        public byte cBtConnect(byte* pName)
        {
            throw new NotImplementedException();
        }

        public byte cBtDeleteFavourItem(byte* pName)
        {
            throw new NotImplementedException();
        }

        public ushort cBtDevWriteBuf(byte* pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public ushort cBtDevWriteBuf1(byte* pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public ushort cBtDevWriteBuf2(byte* pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public ushort cBtDevWriteBuf3(byte* pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public ushort cBtDevWriteBuf4(byte* pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public ushort cBtDevWriteBuf5(byte* pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public ushort cBtDevWriteBuf6(byte* pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public ushort cBtDevWriteBuf7(byte* pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public byte cBtDiscChNo(byte ChNo)
        {
            throw new NotImplementedException();
        }

        public byte cBtDisconnect(byte* pName)
        {
            throw new NotImplementedException();
        }

        public byte cBtGetChNo(byte* pName, byte* pChNo)
        {
            throw new NotImplementedException();
        }

        public byte cBtGetConnListEntry(byte Item, byte* pName, sbyte Length, byte* pType)
        {
            throw new NotImplementedException();
        }

        public byte cBtGetDevListEntry(byte Item, sbyte* pConnected, sbyte* pType, byte* pName, sbyte Length)
        {
            throw new NotImplementedException();
        }

        public byte cBtGetEvent()
        {
            GH.Ev3System.Logger.LogInfo("cBtGetEvent called");
            return 0;
        }

        public byte cBtGetHciBusyFlag()
        {
            throw new NotImplementedException();
        }

        public void cBtGetId(byte* pId, byte Length)
        {
            throw new NotImplementedException();
        }

        public void cBtGetIncoming(byte* pName, byte* pCod, byte Len)
        {
            throw new NotImplementedException();
        }

        public byte cBtGetNoOfConnListEntries()
        {
            throw new NotImplementedException();
        }

        public byte cBtGetNoOfDevListEntries()
        {
            throw new NotImplementedException();
        }

        public byte cBtGetNoOfSearchListEntries()
        {
            throw new NotImplementedException();
        }

        public byte cBtGetSearchListEntry(byte Item, sbyte* pConnected, sbyte* pType, sbyte* pParred, byte* pName, sbyte Length)
        {
            throw new NotImplementedException();
        }

        public byte cBtGetStatus()
        {
            throw new NotImplementedException();
        }

        public byte cBtI2cBufReady()
        {
            throw new NotImplementedException();
        }

        public ushort cBtI2cToBtBuf(byte* pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public ushort cBtReadCh0(byte* pBuf, ushort Length)
        {
            throw new NotImplementedException();
        }

        public ushort cBtReadCh1(byte* pBuf, ushort Length)
        {
            throw new NotImplementedException();
        }

        public ushort cBtReadCh2(byte* pBuf, ushort Length)
        {
            throw new NotImplementedException();
        }

        public ushort cBtReadCh3(byte* pBuf, ushort Length)
        {
            throw new NotImplementedException();
        }

        public ushort cBtReadCh4(byte* pBuf, ushort Length)
        {
            throw new NotImplementedException();
        }

        public ushort cBtReadCh5(byte* pBuf, ushort Length)
        {
            throw new NotImplementedException();
        }

        public ushort cBtReadCh6(byte* pBuf, ushort Length)
        {
            throw new NotImplementedException();
        }

        public ushort cBtReadCh7(byte* pBuf, ushort Length)
        {
            throw new NotImplementedException();
        }

        public ushort cBtSetBundleId(byte* pId)
        {
            throw new NotImplementedException();
        }

        public ushort cBtSetBundleSeedId(byte* pSeedId)
        {
            throw new NotImplementedException();
        }

        public byte cBtSetName(byte* pName, byte Length)
        {
            throw new NotImplementedException();
        }

        public byte cBtSetPasskey(byte Accept)
        {
            throw new NotImplementedException();
        }

        public byte cBtSetPin(byte* pPin)
        {
            throw new NotImplementedException();
        }

        public void cBtSetTrustedDev(byte* pBtAddr, byte* pPin, byte PinSize)
        {
            throw new NotImplementedException();
        }

        public void DecodeMode1(byte BufNo)
        {
            throw new NotImplementedException();
        }

        public void IncommingConnect()
        {
            throw new NotImplementedException();
        }
    }
}
