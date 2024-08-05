using Ev3Core.Extensions;
using Ev3Core.Lms2012.Interfaces;
using static Ev3Core.Defines;

namespace Ev3Core.Lms2012
{
    public class Move : IMove
    {
        public void cMoveInitBytes()
        {
            ArrayPointer<UBYTE> pDestination;
            DATA8 pDestinationInd = 0;
            DATA32 Length;

            pDestination = GH.Lms.PrimParPointer();
            Length = (DATA32)GH.Lms.PrimParPointer().GetDATA8();

            while (Length != 0)
            {
                pDestination[pDestinationInd] = (UBYTE)GH.Lms.PrimParPointer().GetUBYTE();
                pDestinationInd++;
                Length--;
            }
        }

        public void cMove8to8()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
            GH.Lms.PrimParPointer().SetDATA8((DATA8)Tmp);
        }

        public void cMove8to16()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer().GetDATA8();

            if (Tmp != DATA8_NAN)
            {
                GH.Lms.PrimParPointer().SetDATA16((DATA16)Tmp);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA16((DATA16)DATA16_NAN);
            }
        }

        public void cMove8to32()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
            if (Tmp != DATA8_NAN)
            {
                GH.Lms.PrimParPointer().SetDATA32((DATA32)Tmp);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA32((DATA32)DATA32_NAN);
            }
        }

        public void cMove8toF()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
            if (Tmp != DATA8_NAN)
            {
                GH.Lms.PrimParPointer().SetDATAF((DATAF)Tmp);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATAF((DATAF)DATAF_NAN);
            }
        }

        public void cMove16to8()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer().GetDATA16();
            if (Tmp != DATA16_NAN)
            {
                if (Tmp > (DATA16)DATA8_MAX)
                {
                    Tmp = (DATA16)DATA8_MAX;
                }
                if (Tmp < (DATA16)DATA8_MIN)
                {
                    Tmp = (DATA16)DATA8_MIN;
                }
                GH.Lms.PrimParPointer().SetDATA8((DATA8)Tmp);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)DATA8_NAN);
            }
        }

        public void cMove16to16()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer().GetDATA16();
            GH.Lms.PrimParPointer().SetDATA16((DATA16)Tmp);
        }

        public void cMove16to32()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer().GetDATA16();
            if (Tmp != DATA16_NAN)
            {
                GH.Lms.PrimParPointer().SetDATA32((DATA32)Tmp);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA32((DATA32)DATA32_NAN);
            }
        }

        public void cMove16toF()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer().GetDATA16();
            if (Tmp != DATA16_NAN)
            {
                GH.Lms.PrimParPointer().SetDATAF((DATAF)Tmp);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATAF((DATAF)DATAF_NAN);
            }
        }

        public void cMove32to8()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
            if (Tmp != DATA32_NAN)
            {
                if (Tmp > (DATA32)DATA8_MAX)
                {
                    Tmp = (DATA32)DATA8_MAX;
                }
                if (Tmp < (DATA32)DATA8_MIN)
                {
                    Tmp = (DATA32)DATA8_MIN;
                }
                GH.Lms.PrimParPointer().SetDATA8((DATA8)Tmp);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)DATA8_NAN);
            }
        }

        public void cMove32to16()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
            if (Tmp != DATA32_NAN)
            {
                if (Tmp > (DATA32)DATA16_MAX)
                {
                    Tmp = (DATA32)DATA16_MAX;
                }
                if (Tmp < (DATA32)DATA16_MIN)
                {
                    Tmp = (DATA32)DATA16_MIN;
                }
                GH.Lms.PrimParPointer().SetDATA16((DATA16)Tmp);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA16((DATA16)DATA16_NAN);
            }
        }

        public void cMove32to32()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
            GH.Lms.PrimParPointer().SetDATA32((DATA32)Tmp);
        }

        public void cMove32toF()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
            if (Tmp != DATA32_NAN)
            {
                GH.Lms.PrimParPointer().SetDATAF((DATAF)Tmp);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATAF((DATAF)DATAF_NAN);
            }
        }

        public void cMoveFto8()
        {
            DATAF Tmp;

            Tmp = (DATAF)GH.Lms.PrimParPointer().GetDATAF();
            if (!(float.IsNaN(Tmp)))
            {
                if (Tmp > (DATAF)DATA8_MAX)
                {
                    Tmp = (DATAF)DATA8_MAX;
                }
                if (Tmp < (DATAF)DATA8_MIN)
                {
                    Tmp = (DATAF)DATA8_MIN;
                }
                GH.Lms.PrimParPointer().SetDATA8((DATA8)Tmp);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA8((DATA8)DATA8_NAN);
            }
        }

        public void cMoveFto16()
        {
            DATAF Tmp;

            Tmp = (DATAF)GH.Lms.PrimParPointer().GetDATAF();
            if (!(float.IsNaN(Tmp)))
            {
                if (Tmp > (DATAF)DATA16_MAX)
                {
                    Tmp = (DATAF)DATA16_MAX;
                }
                if (Tmp < (DATAF)DATA16_MIN)
                {
                    Tmp = (DATAF)DATA16_MIN;
                }
                GH.Lms.PrimParPointer().SetDATA16((DATA16)Tmp);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA16((DATA16)DATA16_NAN);
            }
        }

        public void cMoveFto32()
        {
            DATAF Tmp;

            Tmp = (DATAF)GH.Lms.PrimParPointer().GetDATAF();
            if (!(float.IsNaN(Tmp)))
            {
                if (Tmp > (DATAF)DATA32_MAX)
                {
                    Tmp = (DATAF)DATA32_MAX;
                }
                if (Tmp < (DATAF)DATA32_MIN)
                {
                    Tmp = (DATAF)DATA32_MIN;
                }
                GH.Lms.PrimParPointer().SetDATA32((DATA32)Tmp);
            }
            else
            {
                GH.Lms.PrimParPointer().SetDATA32((DATA32)DATA32_NAN);
            }
        }

        public void cMoveFtoF()
        {
            DATAF Tmp;

            Tmp = (DATAF)GH.Lms.PrimParPointer().GetDATAF();
            GH.Lms.PrimParPointer().SetDATAF((DATAF)Tmp);
        }

        public void cMoveRead8()
        {
            DATA8[] pTmp;
            DATA8 Index;

            pTmp = (DATA8[])GH.Lms.PrimParPointer().GetArrayDATA8();
            Index = (DATA8)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer().SetDATA8((DATA8)pTmp[Index]);
        }

        public void cMoveRead16()
        {
            DATA16[] pTmp;
            DATA8 Index;

            pTmp = (DATA16[])GH.Lms.PrimParPointer().GetArrayDATA16();
            Index = (DATA8)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer().SetDATA16((DATA16)pTmp[Index]);
        }

        public void cMoveRead32()
        {
            DATA32[] pTmp;
            DATA8 Index;

            pTmp = (DATA32[])GH.Lms.PrimParPointer().GetArrayDATA32();
            Index = (DATA8)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer().SetDATA32((DATA32)pTmp[Index]);
        }

        public void cMoveReadF()
        {
            DATAF[] pTmp;
            DATA8 Index;

            pTmp = (DATAF[])GH.Lms.PrimParPointer().GetArrayDATAF();
            Index = (DATA8)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer().SetDATAF((DATAF)pTmp[Index]);
        }

        public void cMoveWrite8()
        {
            DATA8 Tmp;
            ArrayPointer<UBYTE> pTmp;
            DATA8 Index;

            Tmp = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
            Index = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
            pTmp = GH.Lms.PrimParPointer();
            pTmp.SetDATA8(Tmp, false, (uint)Index);
        }

        public void cMoveWrite16()
        {
            DATA16 Tmp;
			ArrayPointer<UBYTE> pTmp;
			DATA8 Index;

            Tmp = (DATA16)GH.Lms.PrimParPointer().GetDATA16();
            Index = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
            pTmp = GH.Lms.PrimParPointer();
			pTmp.SetDATA16(Tmp, false, (uint)Index * 2);
		}

        public void cMoveWrite32()
        {
            DATA32 Tmp;
			ArrayPointer<UBYTE> pTmp;
			DATA8 Index;

            Tmp = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
            Index = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
            pTmp = GH.Lms.PrimParPointer();
			pTmp.SetDATA32(Tmp, false, (uint)Index * 4);
		}

        public void cMoveWriteF()
        {
            DATAF Tmp;
			ArrayPointer<UBYTE> pTmp;
			DATA8 Index;

            Tmp = (DATAF)GH.Lms.PrimParPointer().GetDATAF();
            Index = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
            pTmp = GH.Lms.PrimParPointer();
			pTmp.SetDATAF(Tmp, false, (uint)Index * 4);
		}
    }
}
