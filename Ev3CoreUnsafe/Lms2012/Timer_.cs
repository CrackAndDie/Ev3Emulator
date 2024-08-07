using Ev3CoreUnsafe.Enums;
using Ev3CoreUnsafe.Lms2012.Interfaces;

namespace Ev3CoreUnsafe.Lms2012
{
	public unsafe class Timer_ : Interfaces.ITimer
	{
		public uint cTimerGetmS()
		{
			return (uint)DateTime.Now.TimeOfDay.TotalMilliseconds;
		}

		public uint cTimerGetuS()
		{
			return (uint)DateTime.Now.TimeOfDay.TotalMicroseconds;
		}

		public void cTimerWait()
		{
			ULONG Time;

			Time = *(ULONG*)GH.Lms.PrimParPointer();

			*(ULONG*)GH.Lms.PrimParPointer() = cTimerGetmS() + Time;
		}

		public void cTimerReady()
		{
			IP TmpIp;
			DSPSTAT DspStat = DSPSTAT.BUSYBREAK;

			TmpIp = GH.Lms.GetObjectIp();

			if (*(ULONG*)GH.Lms.PrimParPointer() <= cTimerGetmS())
			{
				DspStat = DSPSTAT.NOBREAK;
			}
			if (DspStat == DSPSTAT.BUSYBREAK)
			{ // Rewind IP

				GH.Lms.SetObjectIp(TmpIp - 1);
			}
			GH.Lms.SetDispatchStatus(DspStat);
		}

		public void cTimerRead()
		{
			*(DATA32*)GH.Lms.PrimParPointer() = (DATA32)(cTimerGetmS() - GH.VMInstance.Program[GH.Lms.CurrentProgramId()].StartTime);
		}

		public void cTimerReaduS()
		{
			*(DATA32*)GH.Lms.PrimParPointer() = (DATA32)cTimerGetuS();
		}
	}
}
