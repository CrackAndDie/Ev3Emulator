using Ev3Core.Enums;
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

            Time = (ULONG)GH.Lms.PrimParPointer();

            GH.Lms.PrimParPointer((ULONG)cTimerGetmS() + Time);
        }

        public void cTimerReady()
        {
            IP TmpIp;
            int TmpIpInd;
            DSPSTAT DspStat = DSPSTAT.BUSYBREAK;

            TmpIp = GH.Lms.GetObjectIp();
            TmpIpInd = GH.Lms.GetObjectIpInd();

            if ((ULONG)GH.Lms.PrimParPointer() <= cTimerGetmS())
            {
                DspStat = DSPSTAT.NOBREAK;
            }
            if (DspStat == DSPSTAT.BUSYBREAK)
            { // Rewind IP

                GH.Lms.SetObjectIp(TmpIp);
                GH.Lms.SetObjectIpInd(TmpIpInd - 1);
            }
            GH.Lms.SetDispatchStatus(DspStat);
        }

        public void cTimerRead()
        {
            GH.Lms.PrimParPointer((DATA32)(cTimerGetmS() - GH.VMInstance.Program[GH.Lms.CurrentProgramId()].StartTime));
        }

        public void cTimerReaduS()
        {
            GH.Lms.PrimParPointer((DATA32)cTimerGetuS());
        }
    }
}
