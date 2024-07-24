using EV3DecompilerLib.Decompile;
using System.Runtime.CompilerServices;
using static EV3DecompilerLib.Decompile.lms2012;
using static Ev3EmulatorCore.Lms.Lms2012.LmsInstance;

namespace Ev3EmulatorCore.Lms.Lms2012
{
	public partial class LmsInstance
	{
		public DATA8 GetTerminalEnabled()
		{
			// TODO
			return 1;
		}

        public void SetTerminalEnable(DATA8 value)
        {
            // TODO
        }

        public DATA8 GetSleepMinutes()
		{
			// TODO
			return 0;
		}

		public PRGID CurrentProgramId()
		{
			// TODO
			return 0;
		}

		public OBJID CallingObjectId()
		{
			// TODO
			return 0;
		}

		public unsafe IP GetObjectIp()
		{
			// TODO
			return null;
		}

        public unsafe void SetObjectIp(IP ip)
        {
            // TODO
        }

		public void SetDispatchStatus(DSPSTAT stat)
		{
			// TODO
		}

        public unsafe DATA8 CheckUsbstick(DATA8* changed, DATA32* total, DATA32* free, DATA8 force)
		{
			// TODO
			return 0;
		}

		public unsafe DATA8 CheckSdcard(DATA8* changed, DATA32* total, DATA32* free, DATA8 force)
		{
			// TODO
			return 0;
		}

		public ObjectStatus ProgramStatusChange(PRGID prgId)
		{
			// TODO
			return ObjectStatus.STOPPED;
		}

		
		public unsafe void* PrimParPointer()
		{
			// TODO
			return null;
		}

        public unsafe Result ValidateChar(DATA8* pChar, DATA8 Set)
		{
			Result Result = Result.OK;
			// TODO
			return (Result);
		}
	}
}
