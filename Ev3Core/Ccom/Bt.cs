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

        public byte BtGetMode2(VarPointer<UBYTE> pMode2)
        {
            throw new NotImplementedException();
        }

        public byte BtGetOnOff(VarPointer<UBYTE> On)
        {
            throw new NotImplementedException();
        }

        public byte BtGetVisibility()
        {
            throw new NotImplementedException();
        }

        public void BtInit(ArrayPointer<UBYTE> pName)
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

        public byte cBtConnect(ArrayPointer<UBYTE> pName)
        {
            throw new NotImplementedException();
        }

        public byte cBtDeleteFavourItem(ArrayPointer<UBYTE> pName)
        {
            throw new NotImplementedException();
        }

        public ushort cBtDevWriteBuf(ArrayPointer<UBYTE> pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public ushort cBtDevWriteBuf1(ArrayPointer<UBYTE> pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public ushort cBtDevWriteBuf2(ArrayPointer<UBYTE> pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public ushort cBtDevWriteBuf3(ArrayPointer<UBYTE> pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public ushort cBtDevWriteBuf4(ArrayPointer<UBYTE> pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public ushort cBtDevWriteBuf5(ArrayPointer<UBYTE> pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public ushort cBtDevWriteBuf6(ArrayPointer<UBYTE> pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public ushort cBtDevWriteBuf7(ArrayPointer<UBYTE> pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public byte cBtDiscChNo(byte ChNo)
        {
            throw new NotImplementedException();
        }

        public byte cBtDisconnect(ArrayPointer<UBYTE> pName)
        {
            throw new NotImplementedException();
        }

        public byte cBtGetChNo(ArrayPointer<UBYTE> pName, VarPointer<UBYTE> pChNo)
        {
            throw new NotImplementedException();
        }

        public byte cBtGetConnListEntry(byte Item, ArrayPointer<UBYTE> pName, sbyte Length, VarPointer<UBYTE> pType)
        {
            throw new NotImplementedException();
        }

        public byte cBtGetDevListEntry(byte Item, VarPointer<SBYTE> pConnected, VarPointer<SBYTE> pType, ArrayPointer<UBYTE> pName, sbyte Length)
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

        public void cBtGetId(VarPointer<UBYTE> pId, byte Length)
        {
            throw new NotImplementedException();
        }

        public void cBtGetIncoming(ArrayPointer<UBYTE> pName, VarPointer<UBYTE> pCod, byte Len)
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

        public byte cBtGetSearchListEntry(byte Item, VarPointer<SBYTE> pConnected, VarPointer<SBYTE> pType, VarPointer<SBYTE> pParred, ArrayPointer<UBYTE> pName, sbyte Length)
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

        public ushort cBtI2cToBtBuf(ArrayPointer<UBYTE> pBuf, ushort Size)
        {
            throw new NotImplementedException();
        }

        public ushort cBtReadCh0(ArrayPointer<UBYTE> pBuf, ushort Length)
        {
            throw new NotImplementedException();
        }

        public ushort cBtReadCh1(ArrayPointer<UBYTE> pBuf, ushort Length)
        {
            throw new NotImplementedException();
        }

        public ushort cBtReadCh2(ArrayPointer<UBYTE> pBuf, ushort Length)
        {
            throw new NotImplementedException();
        }

        public ushort cBtReadCh3(ArrayPointer<UBYTE> pBuf, ushort Length)
        {
            throw new NotImplementedException();
        }

        public ushort cBtReadCh4(ArrayPointer<UBYTE> pBuf, ushort Length)
        {
            throw new NotImplementedException();
        }

        public ushort cBtReadCh5(ArrayPointer<UBYTE> pBuf, ushort Length)
        {
            throw new NotImplementedException();
        }

        public ushort cBtReadCh6(ArrayPointer<UBYTE> pBuf, ushort Length)
        {
            throw new NotImplementedException();
        }

        public ushort cBtReadCh7(ArrayPointer<UBYTE> pBuf, ushort Length)
        {
            throw new NotImplementedException();
        }

        public ushort cBtSetBundleId(VarPointer<UBYTE> pId)
        {
            throw new NotImplementedException();
        }

        public ushort cBtSetBundleSeedId(VarPointer<UBYTE> pSeedId)
        {
            throw new NotImplementedException();
        }

        public byte cBtSetName(ArrayPointer<UBYTE> pName, byte Length)
        {
            throw new NotImplementedException();
        }

        public byte cBtSetPasskey(byte Accept)
        {
            throw new NotImplementedException();
        }

        public byte cBtSetPin(VarPointer<UBYTE> pPin)
        {
            throw new NotImplementedException();
        }

        public void cBtSetTrustedDev(VarPointer<UBYTE> pBtAddr, VarPointer<UBYTE> pPin, byte PinSize)
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
