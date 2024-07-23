using EV3DecompilerLib.Decompile;
using static Ev3EmulatorCore.Lms.Lms2012.LmsInstance;

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

        public ushort CurrentProgramId()
		{
			// TODO
			return 0;
		}

        public ushort CallingObjectId()
        {
            // TODO
            return 0;
        }

		public byte CheckUsbstick(ref byte changed, ref int total, ref int free, byte force)
		{
			// TODO
			return 0;
		}

        public byte CheckSdcard(ref byte changed, ref int total, ref int free, byte force)
        {
            // TODO
            return 0;
        }

		public lms2012.ObjectStatus ProgramStatusChange(ushort prgId)
		{
			// TODO
			return lms2012.ObjectStatus.STOPPED;
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
