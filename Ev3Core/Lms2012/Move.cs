using Ev3Core.Lms2012.Interfaces;
using static Ev3Core.Defines;

namespace Ev3Core.Lms2012
{
    public class Move : IMove
    {
        public void cMoveInitBytes()
        {
            DATA8[] pDestination;
            DATA8 pDestinationInd = 0;
            DATA32 Length;

            pDestination = (DATA8[])GH.Lms.PrimParPointer();
            Length = (DATA32)GH.Lms.PrimParPointer();

            while (Length != 0)
            {
                pDestination[pDestinationInd] = (DATA8)GH.Lms.PrimParPointer();
                pDestinationInd++;
                Length--;
            }
        }

        public void cMove8to8()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA8)Tmp);
        }

        public void cMove8to16()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer();

            if (Tmp != DATA8_NAN)
            {
                GH.Lms.PrimParPointer((DATA16)Tmp);
            }
            else
            {
                GH.Lms.PrimParPointer((DATA16)DATA16_NAN);
            }
        }

        public void cMove8to32()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer();
            if (Tmp != DATA8_NAN)
            {
                GH.Lms.PrimParPointer((DATA32)Tmp);
            }
            else
            {
                GH.Lms.PrimParPointer((DATA32)DATA32_NAN);
            }
        }

        public void cMove8toF()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer();
            if (Tmp != DATA8_NAN)
            {
                GH.Lms.PrimParPointer((DATAF)Tmp);
            }
            else
            {
                GH.Lms.PrimParPointer((DATAF)DATAF_NAN);
            }
        }

        public void cMove16to8()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer();
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
                GH.Lms.PrimParPointer((DATA8)Tmp);
            }
            else
            {
                GH.Lms.PrimParPointer((DATA8)DATA8_NAN);
            }
        }

        public void cMove16to16()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA16)Tmp);
        }

        public void cMove16to32()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer();
            if (Tmp != DATA16_NAN)
            {
                GH.Lms.PrimParPointer((DATA32)Tmp);
            }
            else
            {
                GH.Lms.PrimParPointer((DATA32)DATA32_NAN);
            }
        }

        public void cMove16toF()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer();
            if (Tmp != DATA16_NAN)
            {
                GH.Lms.PrimParPointer((DATAF)Tmp);
            }
            else
            {
                GH.Lms.PrimParPointer((DATAF)DATAF_NAN);
            }
        }

        public void cMove32to8()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer();
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
                GH.Lms.PrimParPointer((DATA8)Tmp);
            }
            else
            {
                GH.Lms.PrimParPointer((DATA8)DATA8_NAN);
            }
        }

        public void cMove32to16()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer();
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
                GH.Lms.PrimParPointer((DATA16)Tmp);
            }
            else
            {
                GH.Lms.PrimParPointer((DATA16)DATA16_NAN);
            }
        }

        public void cMove32to32()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA32)Tmp);
        }

        public void cMove32toF()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer();
            if (Tmp != DATA32_NAN)
            {
                GH.Lms.PrimParPointer((DATAF)Tmp);
            }
            else
            {
                GH.Lms.PrimParPointer((DATAF)DATAF_NAN);
            }
        }

        public void cMoveFto8()
        {
            DATAF Tmp;

            Tmp = (DATAF)GH.Lms.PrimParPointer();
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
                GH.Lms.PrimParPointer((DATA8)Tmp);
            }
            else
            {
                GH.Lms.PrimParPointer((DATA8)DATA8_NAN);
            }
        }

        public void cMoveFto16()
        {
            DATAF Tmp;

            Tmp = (DATAF)GH.Lms.PrimParPointer();
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
                GH.Lms.PrimParPointer((DATA16)Tmp);
            }
            else
            {
                GH.Lms.PrimParPointer((DATA16)DATA16_NAN);
            }
        }

        public void cMoveFto32()
        {
            DATAF Tmp;

            Tmp = (DATAF)GH.Lms.PrimParPointer();
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
                GH.Lms.PrimParPointer((DATA32)Tmp);
            }
            else
            {
                GH.Lms.PrimParPointer((DATA32)DATA32_NAN);
            }
        }

        public void cMoveFtoF()
        {
            DATAF Tmp;

            Tmp = (DATAF)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATAF)Tmp);
        }

        public void cMoveRead8()
        {
            DATA8[] pTmp;
            DATA8 Index;

            pTmp = (DATA8[])GH.Lms.PrimParPointer();
            Index = (DATA8)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA8)pTmp[Index]);
        }

        public void cMoveRead16()
        {
            DATA16[] pTmp;
            DATA8 Index;

            pTmp = (DATA16[])GH.Lms.PrimParPointer();
            Index = (DATA8)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA16)pTmp[Index]);
        }

        public void cMoveRead32()
        {
            DATA32[] pTmp;
            DATA8 Index;

            pTmp = (DATA32[])GH.Lms.PrimParPointer();
            Index = (DATA8)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA32)pTmp[Index]);
        }

        public void cMoveReadF()
        {
            DATAF[] pTmp;
            DATA8 Index;

            pTmp = (DATAF[])GH.Lms.PrimParPointer();
            Index = (DATA8)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATAF)pTmp[Index]);
        }

        public void cMoveWrite8()
        {
            DATA8 Tmp;
            DATA8[] pTmp;
            DATA8 Index;

            Tmp = (DATA8)GH.Lms.PrimParPointer();
            Index = (DATA8)GH.Lms.PrimParPointer();
            pTmp = (DATA8[])GH.Lms.PrimParPointer();
            pTmp[Index] = Tmp;
        }

        public void cMoveWrite16()
        {
            DATA16 Tmp;
            DATA16[] pTmp;
            DATA8 Index;

            Tmp = (DATA16)GH.Lms.PrimParPointer();
            Index = (DATA8)GH.Lms.PrimParPointer();
            pTmp = (DATA16[])GH.Lms.PrimParPointer();
            pTmp[Index] = Tmp;
        }

        public void cMoveWrite32()
        {
            DATA32 Tmp;
            DATA32[] pTmp;
            DATA8 Index;

            Tmp = (DATA32)GH.Lms.PrimParPointer();
            Index = (DATA8)GH.Lms.PrimParPointer();
            pTmp = (DATA32[])GH.Lms.PrimParPointer();
            pTmp[Index] = Tmp;
        }

        public void cMoveWriteF()
        {
            DATAF Tmp;
            DATAF[] pTmp;
            DATA8 Index;

            Tmp = (DATAF)GH.Lms.PrimParPointer();
            Index = (DATA8)GH.Lms.PrimParPointer();
            pTmp = (DATAF[])GH.Lms.PrimParPointer();
            pTmp[Index] = Tmp;
        }
    }
}
