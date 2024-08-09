using Ev3CoreUnsafe.Cui.Interfaces;
using Ev3CoreUnsafe.Enums;
using static Ev3CoreUnsafe.Defines;

namespace Ev3CoreUnsafe.Cui
{
	public unsafe class Terminal : ITerminal
	{
		RESULT TerminalResult = RESULT.FAIL;

		public RESULT dTerminalInit()
		{
			RESULT Result = RESULT.FAIL;

			//if (tcgetattr(STDIN_FILENO, &TerminalAttr) >= 0)
			//{
			//	TerminalSavedAttr = TerminalAttr;

			//	TerminalAttr.c_lflag &= ~(ECHO | ICANON | IEXTEN | ISIG);
			//	TerminalAttr.c_lflag |= ECHO;
			//	TerminalAttr.c_iflag &= ~(BRKINT | ICRNL | INPCK | ISTRIP | IXON);
			//	TerminalAttr.c_cflag &= ~(CSIZE | PARENB);
			//	TerminalAttr.c_cflag |= CS8;
			//	TerminalAttr.c_oflag &= ~(OPOST);

			//	TerminalAttr.c_cc[VMIN] = 0;
			//	TerminalAttr.c_cc[VTIME] = 0;

			//	if (tcsetattr(STDIN_FILENO, TCSANOW, &TerminalAttr) >= 0)
			//	{
			//		Result = OK;
			//	}
			//}
			TerminalResult = Result;

			GH.Ev3System.Logger.LogWarning($"Call of unimplemented shite in {Environment.StackTrace}");

			return (Result);
		}


		public RESULT dTerminalRead(UBYTE* pData)
		{
			RESULT Result = RESULT.FAIL;
			int Tmp = 0;

			if (TerminalResult == OK)
			{
				Result = RESULT.BUSY;

				// Tmp = read(STDIN_FILENO, pData, 1);
				GH.Ev3System.Logger.LogWarning($"Call of unimplemented shite in {Environment.StackTrace}");
				if (Tmp == 1)
				{
					Result = OK;

				}
			}

			return (Result);
		}


		public RESULT dTerminalWrite(UBYTE* pData, UWORD Cnt)
		{
			if (TerminalResult == OK)
			{
				//if (write(STDOUT_FILENO, pData, (size_t)Cnt) != Cnt)
				//{
				//	TerminalResult = FAIL;
				//}
				GH.Ev3System.Logger.LogWarning($"Call of unimplemented shite in {Environment.StackTrace}");
			}

			return (OK);
		}


		public RESULT dTerminalExit()
		{
			//if (TerminalResult == OK)
			//{
			//	tcsetattr(STDIN_FILENO, TCSAFLUSH, &TerminalSavedAttr);
			//}
			TerminalResult = RESULT.FAIL;

			GH.Ev3System.Logger.LogWarning($"Call of unimplemented shite in {Environment.StackTrace}");

			return (OK);
		}
	}
}
