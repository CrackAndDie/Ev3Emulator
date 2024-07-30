using Ev3Core.Cui.Interfaces;
using Ev3Core.Enums;

namespace Ev3Core.Cui
{
	public class Terminal : ITerminal
	{
		public RESULT dTerminalInit()
		{
			GH.Ev3System.Logger.LogWarning("Terminal init called. WTF");
			return RESULT.OK;
		}

		public RESULT dTerminalRead(ref byte pData)
		{
			GH.Ev3System.Logger.LogWarning("Terminal read called. WTF");
			return RESULT.OK;
		}

		public RESULT dTerminalWrite(byte[] pData, ushort Cnt)
		{
			GH.Ev3System.Logger.LogWarning("Terminal write called. WTF");
			return RESULT.OK;
		}

		public RESULT dTerminalWrite(sbyte[] pData, short Cnt)
		{
			GH.Ev3System.Logger.LogWarning("Terminal swrite called. WTF");
			return RESULT.OK;
		}

		public RESULT dTerminalExit()
		{
			GH.Ev3System.Logger.LogWarning("Terminal exit called. WTF");
			return RESULT.OK;
		}
	}
}
