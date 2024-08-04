using Ev3Core.Ccom.Interfaces;

namespace Ev3Core.Ccom
{
    public class Bt : IBt
    {
        // TODO: mb something should be implemented
        public void BtExit()
        {
            throw new NotImplementedException();
        }

        public byte BtGetMode2(ref byte pMode2)
        {
            throw new NotImplementedException();
        }

        public byte BtGetOnOff(ref byte On)
        {
            throw new NotImplementedException();
        }

        public byte BtGetVisibility()
        {
            throw new NotImplementedException();
        }

        public void BtInit(char[] pName)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public byte cBtConnect(byte[] pName)
        {
            throw new NotImplementedException();
        }

        public byte cBtDeleteFavourItem(byte[] pName)
        {
            throw new NotImplementedException();
        }

        public ushort cBtDevWriteBuf(byte[] pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public ushort cBtDevWriteBuf1(byte[] pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public ushort cBtDevWriteBuf2(byte[] pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public ushort cBtDevWriteBuf3(byte[] pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public ushort cBtDevWriteBuf4(byte[] pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public ushort cBtDevWriteBuf5(byte[] pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public ushort cBtDevWriteBuf6(byte[] pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public ushort cBtDevWriteBuf7(byte[] pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public byte cBtDiscChNo(byte ChNo)
        {
            throw new NotImplementedException();
        }

        public byte cBtDisconnect(byte[] pName)
        {
            throw new NotImplementedException();
        }

        public byte cBtGetChNo(byte[] pName, ref byte pChNo)
        {
            throw new NotImplementedException();
        }

        public byte cBtGetConnListEntry(byte Item, byte[] pName, sbyte Length, ref byte pType)
        {
            throw new NotImplementedException();
        }

        public byte cBtGetDevListEntry(byte Item, ref sbyte pConnected, ref sbyte pType, byte[] pName, sbyte Length)
        {
            throw new NotImplementedException();
        }

        public byte cBtGetEvent()
        {
            throw new NotImplementedException();
        }

        public byte cBtGetHciBusyFlag()
        {
            throw new NotImplementedException();
        }

        public void cBtGetId(ref byte pId, byte Length)
        {
            throw new NotImplementedException();
        }

        public void cBtGetIncoming(byte[] pName, ref byte pCod, byte Len)
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

        public byte cBtGetSearchListEntry(byte Item, ref sbyte pConnected, ref sbyte pType, ref sbyte pParred, byte[] pName, sbyte Length)
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

        public ushort cBtI2cToBtBuf(byte[] pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public ushort cBtReadCh0(byte[] pBuf, ushort Length)
        {
            throw new NotImplementedException();
        }

        public ushort cBtReadCh1(byte[] pBuf, ushort Length)
        {
            throw new NotImplementedException();
        }

        public ushort cBtReadCh2(byte[] pBuf, ushort Length)
        {
            throw new NotImplementedException();
        }

        public ushort cBtReadCh3(byte[] pBuf, ushort Length)
        {
            throw new NotImplementedException();
        }

        public ushort cBtReadCh4(byte[] pBuf, ushort Length)
        {
            throw new NotImplementedException();
        }

        public ushort cBtReadCh5(byte[] pBuf, ushort Length)
        {
            throw new NotImplementedException();
        }

        public ushort cBtReadCh6(byte[] pBuf, ushort Length)
        {
            throw new NotImplementedException();
        }

        public ushort cBtReadCh7(byte[] pBuf, ushort Length)
        {
            throw new NotImplementedException();
        }

        public ushort cBtSetBundleId(ref byte pId)
        {
            throw new NotImplementedException();
        }

        public ushort cBtSetBundleSeedId(ref byte pSeedId)
        {
            throw new NotImplementedException();
        }

        public byte cBtSetName(byte[] pName, byte Length)
        {
            throw new NotImplementedException();
        }

        public byte cBtSetPasskey(byte Accept)
        {
            throw new NotImplementedException();
        }

        public byte cBtSetPin(ref byte pPin)
        {
            throw new NotImplementedException();
        }

        public void cBtSetTrustedDev(ref byte pBtAddr, ref byte pPin, byte PinSize)
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
