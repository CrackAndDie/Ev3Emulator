using Ev3Core.Enums;

namespace Ev3Core.Cui.Interfaces
{
	public interface ITerminal
	{
		RESULT dTerminalInit();

		RESULT dTerminalRead(ref UBYTE pData);

		RESULT dTerminalWrite(UBYTE[] pData, UWORD Cnt);
		RESULT dTerminalWrite(DATA8[] pData, SWORD Cnt);

		RESULT dTerminalExit();
	}
}
