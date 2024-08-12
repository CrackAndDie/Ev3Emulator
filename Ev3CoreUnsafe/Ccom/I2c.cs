using Ev3CoreUnsafe.Ccom.Interfaces;
using Ev3CoreUnsafe.Enums;

namespace Ev3CoreUnsafe.Ccom
{
    public unsafe class I2c : II2c
    {
        public ushort DataToMode2Decoding(byte* pBuf, ushort Length)
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

        public RESULT I2cInit(READBUF* pBuf, WRITEBUF* pWriteBuf, char* pBundleId, char* pBundleSeedId)
        {
            GH.Ev3System.Logger.LogInfo("I2C INIT CALLED");
            return RESULT.OK;
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
