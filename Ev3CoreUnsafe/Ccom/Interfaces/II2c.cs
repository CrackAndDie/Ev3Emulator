using Ev3CoreUnsafe.Enums;

namespace Ev3CoreUnsafe.Ccom.Interfaces
{
	public unsafe interface II2c
	{
		RESULT I2cInit(READBUF* pBuf, WRITEBUF* pWriteBuf, char* pBundleId, char* pBundleSeedId);

		void I2cExit();

		void I2cStart();
		void I2cStop();

		UWORD DataToMode2Decoding(UBYTE* pBuf, UWORD Length);
		UBYTE I2cGetBootStatus();
	}
}
