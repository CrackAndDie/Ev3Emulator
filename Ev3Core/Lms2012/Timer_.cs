using Ev3Core.Enums;
using Ev3Core.Extensions;
using Ev3Core.Lms2012.Interfaces;
using static Ev3Core.Defines;

namespace Ev3Core.Lms2012
{
    public class Timer_ : Interfaces.ITimer
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

            Time = (ULONG)GH.Lms.PrimParPointer().GetULONG();

            GH.Lms.PrimParPointer().SetULONG((ULONG)cTimerGetmS() + Time);
        }

        public void cTimerReady()
        {
            IP TmpIp;
            DSPSTAT DspStat = DSPSTAT.BUSYBREAK;

            TmpIp = GH.Lms.GetObjectIp();

            if ((ULONG)GH.Lms.PrimParPointer().GetULONG() <= cTimerGetmS())
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
            GH.Lms.PrimParPointer().SetDATA32((DATA32)(cTimerGetmS() - GH.VMInstance.Program[GH.Lms.CurrentProgramId()].StartTime));
        }

        public void cTimerReaduS()
        {
            GH.Lms.PrimParPointer().SetDATA32((DATA32)cTimerGetuS());
        }
    }
}
