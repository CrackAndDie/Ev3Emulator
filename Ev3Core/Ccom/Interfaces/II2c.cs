using Ev3Core.Enums;

namespace Ev3Core.Ccom.Interfaces
{
	public interface II2c
	{
		RESULT I2cInit(READBUF pBuf, WRITEBUF pWriteBuf, VarPointer<DATA8> pBundleId, VarPointer<DATA8> pBundleSeedId);

		void I2cExit();

		void I2cStart();
		void I2cStop();

		UWORD DataToMode2Decoding(ArrayPointer<UBYTE> pBuf, UWORD Length);
		UBYTE I2cGetBootStatus();
	}
}
