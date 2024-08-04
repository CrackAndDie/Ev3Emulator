using Ev3Core.Ccom.Interfaces;
using Ev3Core.Enums;

namespace Ev3Core.Ccom
{
    public class I2c : II2c
    {
        // TODO: ms should be implemented
        public ushort DataToMode2Decoding(ArrayPointer<UBYTE> pBuf, ushort Length)
        {
            throw new NotImplementedException();
        }

        public void I2cExit()
        {
            throw new NotImplementedException();
        }

        public byte I2cGetBootStatus()
        {
            throw new NotImplementedException();
        }

        public RESULT I2cInit(READBUF pBuf, WRITEBUF pWriteBuf, VarPointer<DATA8> pBundleId, VarPointer<DATA8> pBundleSeedId)
        {
            throw new NotImplementedException();
        }

        public void I2cStart()
        {
            throw new NotImplementedException();
        }

        public void I2cStop()
        {
            throw new NotImplementedException();
        }
    }
}
