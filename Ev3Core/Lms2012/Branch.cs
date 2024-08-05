using Ev3Core.Extensions;
using Ev3Core.Lms2012.Interfaces;

namespace Ev3Core.Lms2012
{
    public class Branch : IBranch
    {
        public void cBranchJr()
        {
            GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
        }

        public void cBranchJrFalse()
        {
            if ((DATA8)GH.Lms.PrimParPointer().GetDATA8() == (DATA8)0)
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }

        public void cBranchJrTrue()
        {
            if ((DATA8)GH.Lms.PrimParPointer().GetDATA8() != (DATA8)0)
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }

        public void cBranchJrNan()
        {
            DATAF Tmp;

            Tmp = (DATAF)GH.Lms.PrimParPointer().GetDATAF();

            if (float.IsNaN(Tmp))
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }

        public void cBranchJrLt8()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
            if (Tmp < (DATA8)GH.Lms.PrimParPointer().GetDATA8())
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }

        public void cBranchJrLt16()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer().GetDATA16();
            if (Tmp < (DATA16)GH.Lms.PrimParPointer().GetDATA16())
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }

        public void cBranchJrLt32()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
            if (Tmp < (DATA32)GH.Lms.PrimParPointer().GetDATA32())
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }

        public void cBranchJrLtF()
        {
            DATAF Tmp;

            Tmp = (DATAF)GH.Lms.PrimParPointer().GetDATAF();
            if (Tmp < (DATAF)GH.Lms.PrimParPointer().GetDATAF())
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }

        public void cBranchJrGt8()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
            if (Tmp > (DATA8)GH.Lms.PrimParPointer().GetDATA8())
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }

        public void cBranchJrGt16()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer().GetDATA16();
            if (Tmp > (DATA16)GH.Lms.PrimParPointer().GetDATA16())
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }

        public void cBranchJrGt32()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
            if (Tmp > (DATA32)GH.Lms.PrimParPointer().GetDATA32())
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }

        public void cBranchJrGtF()
        {
            DATAF Tmp;

            Tmp = (DATAF)GH.Lms.PrimParPointer().GetDATAF();
            if (Tmp > (DATAF)GH.Lms.PrimParPointer().GetDATAF())
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }

        public void cBranchJrEq8()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
            if (Tmp == (DATA8)GH.Lms.PrimParPointer().GetDATA8())
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }

        public void cBranchJrEq16()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer().GetDATA16();
            if (Tmp == (DATA16)GH.Lms.PrimParPointer().GetDATA16())
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }

        public void cBranchJrEq32()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
            if (Tmp == (DATA32)GH.Lms.PrimParPointer().GetDATA32())
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }

        public void cBranchJrEqF()
        {
            DATAF Tmp;

            Tmp = (DATAF)GH.Lms.PrimParPointer().GetDATAF();
            if (Tmp == (DATAF)GH.Lms.PrimParPointer().GetDATAF())
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }

        public void cBranchJrNEq8()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
            if (Tmp != (DATA8)GH.Lms.PrimParPointer().GetDATA8())
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }

        public void cBranchJrNEq16()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer().GetDATA16();
            if (Tmp != (DATA16)GH.Lms.PrimParPointer().GetDATA16())
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }

        public void cBranchJrNEq32()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
            if (Tmp != (DATA32)GH.Lms.PrimParPointer().GetDATA32())
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }

        public void cBranchJrNEqF()
        {
            DATAF Tmp;

            Tmp = (DATAF)GH.Lms.PrimParPointer().GetDATAF();
            if (Tmp != (DATAF)GH.Lms.PrimParPointer().GetDATAF())
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }

        public void cBranchJrLtEq8()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
            if (Tmp <= (DATA8)GH.Lms.PrimParPointer().GetDATA8())
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }

        public void cBranchJrLtEq16()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer().GetDATA16();
            if (Tmp <= (DATA16)GH.Lms.PrimParPointer().GetDATA16())
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }

        public void cBranchJrLtEq32()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
            if (Tmp <= (DATA32)GH.Lms.PrimParPointer().GetDATA32())
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }

        public void cBranchJrLtEqF()
        {
            DATAF Tmp;

            Tmp = (DATAF)GH.Lms.PrimParPointer().GetDATAF();
            if (Tmp <= (DATAF)GH.Lms.PrimParPointer().GetDATAF())
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }

        public void cBranchJrGtEq8()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
            if (Tmp >= (DATA8)GH.Lms.PrimParPointer().GetDATA8())
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }

        public void cBranchJrGtEq16()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer().GetDATA16();
            if (Tmp >= (DATA16)GH.Lms.PrimParPointer().GetDATA16())
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }

        public void cBranchJrGtEq32()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
            if (Tmp >= (DATA32)GH.Lms.PrimParPointer().GetDATA32())
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }

        public void cBranchJrGtEqF()
        {
            DATAF Tmp;

            Tmp = (DATAF)GH.Lms.PrimParPointer().GetDATAF();
            if (Tmp >= (DATAF)GH.Lms.PrimParPointer().GetDATAF())
            {
                GH.Lms.AdjustObjectIp((IMOFFS)GH.Lms.PrimParPointer().GetDATA32());
            }
            else
            {
                GH.Lms.PrimParAdvance();
            }
        }
    }
}
