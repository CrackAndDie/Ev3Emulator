using Ev3CoreUnsafe.Enums;

namespace Ev3CoreUnsafe.Cui.Interfaces
{
	public interface ITerminal
	{
		unsafe RESULT dTerminalInit();

		unsafe RESULT dTerminalRead(UBYTE* pData);

		unsafe RESULT dTerminalWrite(UBYTE* pData, UWORD Cnt);

		unsafe RESULT dTerminalExit();
	}
}
