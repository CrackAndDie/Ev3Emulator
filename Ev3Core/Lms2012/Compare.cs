﻿using Ev3Core.Extensions;
using Ev3Core.Lms2012.Interfaces;

namespace Ev3Core.Lms2012
{
    public class Compare : ICompare
    {
        public void cCompareLt8()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
            if (Tmp < (DATA8)GH.Lms.PrimParPointer().GetDATA8())
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)0);
            }
        }

        public void cCompareLt16()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer().GetDATA16();
            if (Tmp < (DATA16)GH.Lms.PrimParPointer().GetDATA16())
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)0);
            }
        }

        public void cCompareLt32()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
            if (Tmp < (DATA32)GH.Lms.PrimParPointer().GetDATA32())
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)0);
            }
        }

        public void cCompareLtF()
        {
            DATAF Tmp;

            Tmp = (DATAF)GH.Lms.PrimParPointer().GetDATAF();
            if (Tmp < (DATAF)GH.Lms.PrimParPointer().GetDATAF())
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)0);
            }
        }

        public void cCompareGt8()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
            if (Tmp > (DATA8)GH.Lms.PrimParPointer().GetDATA8())
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)0);
            }
        }

        public void cCompareGt16()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer().GetDATA16();
            if (Tmp > (DATA16)GH.Lms.PrimParPointer().GetDATA16())
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)0);
            }
        }

        public void cCompareGt32()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
            if (Tmp > (DATA32)GH.Lms.PrimParPointer().GetDATA32())
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)0);
            }
        }

        public void cCompareGtF()
        {
            DATAF Tmp;

            Tmp = (DATAF)GH.Lms.PrimParPointer().GetDATAF();
            if (Tmp > (DATAF)GH.Lms.PrimParPointer().GetDATAF())
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)0);
            }
        }

        public void cCompareEq8()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
            if (Tmp == (DATA8)GH.Lms.PrimParPointer().GetDATA8())
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)0);
            }
        }

        public void cCompareEq16()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer().GetDATA16();
            if (Tmp == (DATA16)GH.Lms.PrimParPointer().GetDATA16())
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)0);
            }
        }

        public void cCompareEq32()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
            if (Tmp == (DATA32)GH.Lms.PrimParPointer().GetDATA32())
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)0);
            }
        }

        public void cCompareEqF()
        {
            DATAF Tmp;

            Tmp = (DATAF)GH.Lms.PrimParPointer().GetDATAF();
            if (Tmp == (DATAF)GH.Lms.PrimParPointer().GetDATAF())
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)0);
            }
        }

        public void cCompareNEq8()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
            if (Tmp != (DATA8)GH.Lms.PrimParPointer().GetDATA8())
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)0);
            }
        }

        public void cCompareNEq16()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer().GetDATA16();
            if (Tmp != (DATA16)GH.Lms.PrimParPointer().GetDATA16())
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)0);
            }
        }

        public void cCompareNEq32()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
            if (Tmp != (DATA32)GH.Lms.PrimParPointer().GetDATA32())
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)0);
            }
        }

        public void cCompareNEqF()
        {
            DATAF Tmp;

            Tmp = (DATAF)GH.Lms.PrimParPointer().GetDATAF();
            if (Tmp != (DATAF)GH.Lms.PrimParPointer().GetDATAF())
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)0);
            }
        }

        public void cCompareLtEq8()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
            if (Tmp <= (DATA8)GH.Lms.PrimParPointer().GetDATA8())
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)0);
            }
        }

        public void cCompareLtEq16()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer().GetDATA16();
            if (Tmp <= (DATA16)GH.Lms.PrimParPointer().GetDATA16())
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)0);
            }
        }

        public void cCompareLtEq32()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
            if (Tmp <= (DATA32)GH.Lms.PrimParPointer().GetDATA32())
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)0);
            }
        }

        public void cCompareLtEqF()
        {
            DATAF Tmp;

            Tmp = (DATAF)GH.Lms.PrimParPointer().GetDATAF();
            if (Tmp <= (DATAF)GH.Lms.PrimParPointer().GetDATAF())
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)0);
            }
        }

        public void cCompareGtEq8()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
            if (Tmp >= (DATA8)GH.Lms.PrimParPointer().GetDATA8())
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)0);
            }
        }

        public void cCompareGtEq16()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer().GetDATA16();
            if (Tmp >= (DATA16)GH.Lms.PrimParPointer().GetDATA16())
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)0);
            }
        }

        public void cCompareGtEq32()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
            if (Tmp >= (DATA32)GH.Lms.PrimParPointer().GetDATA32())
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)0);
            }
        }

        public void cCompareGtEqF()
        {
            DATAF Tmp;

            Tmp = (DATAF)GH.Lms.PrimParPointer().GetDATAF();
            if (Tmp >= (DATAF)GH.Lms.PrimParPointer().GetDATAF())
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)0);
            }
        }

        public void cCompareSelect8()
        {
            DATA8 Flag;
            DATA8 Source1;
            DATA8 Source2;

            Flag = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
            Source1 = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
            Source2 = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
            if (Flag != 0)
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)Source1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)Source2);
            }
        }

        public void cCompareSelect16()
        {
            DATA8 Flag;
            DATA16 Source1;
            DATA16 Source2;


            Flag = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
            Source1 = (DATA16)GH.Lms.PrimParPointer().GetDATA16();
            Source2 = (DATA16)GH.Lms.PrimParPointer().GetDATA16();
            if (Flag != 0)
            {
                GH.Lms.PrimParPointer().SetDATA16((DATA16)Source1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA16((DATA16)Source2);
            }
        }

        public void cCompareSelect32()
        {
            DATA8 Flag;
            DATA32 Source1;
            DATA32 Source2;

            Flag = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
            Source1 = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
            Source2 = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
            if (Flag != 0)
            {
                GH.Lms.PrimParPointer().SetDATA32((DATA32)Source1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA32((DATA32)Source2);
            }
        }

        public void cCompareSelectF()
        {
            DATA8 Flag;
            DATAF Source1;
            DATAF Source2;

            Flag = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
            Source1 = (DATAF)GH.Lms.PrimParPointer().GetDATAF();
            Source2 = (DATAF)GH.Lms.PrimParPointer().GetDATAF();
            if (Flag != 0)
            {
                GH.Lms.PrimParPointer().SetDATAF((DATAF)Source1);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATAF((DATAF)Source2);
            }
        }
    }
}
