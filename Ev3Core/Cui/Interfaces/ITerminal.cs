using Ev3Core.Enums;

namespace Ev3Core.Cui.Interfaces
{
	public interface ITerminal
	{
		RESULT dTerminalInit();

		RESULT dTerminalRead(VarPointer<byte> pData);

		RESULT dTerminalWrite(ArrayPointer<UBYTE> pData, UWORD Cnt);

		RESULT dTerminalExit();
	}
}
