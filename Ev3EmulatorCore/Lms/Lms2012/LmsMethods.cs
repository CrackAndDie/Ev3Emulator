using EV3DecompilerLib.Decompile;

namespace Ev3EmulatorCore.Lms.Lms2012
{
	public partial class LmsInstance
	{
		public bool GetTerminalEnabled()
		{
			// TODO
			return true;
		}

		public byte GetSleepMinutes()
		{
			// TODO
			return 0;
		}

		public lms2012.Result ValidateChar(byte[] pChar, byte Set)
		{
			lms2012.Result Result = lms2012.Result.OK;

			if ((lms2012.ValidChars[pChar[0]] & Set) == 0)
			{
				pChar[0] = (byte)'_';
				Result = lms2012.Result.FAIL;
			}

			return (Result);
		}
	}
}
