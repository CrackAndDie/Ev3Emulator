using Ev3Core.Enums;
using Ev3Core.Lms2012.Interfaces;
using static Ev3Core.Defines;

namespace Ev3Core.Lms2012
{
    public class Math_ : IMath
    {
        public void cMathAdd8()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer();
            Tmp += (DATA8)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA8)Tmp);
        }

        public void cMathAdd16()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer();
            Tmp += (DATA16)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA16)Tmp);
        }

        public void cMathAdd32()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer();
            Tmp += (DATA32)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA32)Tmp);
        }

        public void cMathAddF()
        {
            DATAF Tmp;

            Tmp = (DATAF)GH.Lms.PrimParPointer();
            Tmp += (DATAF)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATAF)Tmp);
        }

        public void cMathSub8()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer();
            Tmp -= (DATA8)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA8)Tmp);
        }

        public void cMathSub16()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer();
            Tmp -= (DATA16)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA16)Tmp);
        }

        public void cMathSub32()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer();
            Tmp -= (DATA32)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA32)Tmp);
        }

        public void cMathSubF()
        {
            DATAF Tmp;

            Tmp = (DATAF)GH.Lms.PrimParPointer();
            Tmp -= (DATAF)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATAF)Tmp);
        }

        public void cMathMul8()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer();
            Tmp *= (DATA8)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA8)Tmp);
        }

        public void cMathMul16()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer();
            Tmp *= (DATA16)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA16)Tmp);
        }

        public void cMathMul32()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer();
            Tmp *= (DATA32)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA32)Tmp);
        }

        public void cMathMulF()
        {
            DATAF Tmp;

            Tmp = (DATAF)GH.Lms.PrimParPointer();
            Tmp *= (DATAF)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATAF)Tmp);
        }

        public void cMathDiv8()
        {
            DATA8 X, Y;

            X = (DATA8)GH.Lms.PrimParPointer();
            Y = (DATA8)GH.Lms.PrimParPointer();
            if (Y == 0)
            {
                if (X > 0)
                {
                    X = (DATA8)DATA8_MAX;
                }
                if (X < 0)
                {
                    X = (DATA8)DATA8_MIN;
                }
            }
            else
            {
                X /= Y;
            }
            GH.Lms.PrimParPointer((DATA8)X);
        }

        public void cMathDiv16()
        {
            DATA16 X, Y;

            X = (DATA16)GH.Lms.PrimParPointer();
            Y = (DATA16)GH.Lms.PrimParPointer();
            if (Y == 0)
            {
                if (X > 0)
                {
                    X = (DATA16)DATA16_MAX;
                }
                if (X < 0)
                {
                    X = (DATA16)DATA16_MIN;
                }
            }
            else
            {
                X /= Y;
            }
            GH.Lms.PrimParPointer((DATA16)X);
        }

        public void cMathDiv32()
        {
            DATA32 X, Y;

            X = (DATA32)GH.Lms.PrimParPointer();
            Y = (DATA32)GH.Lms.PrimParPointer();
            if (Y == 0)
            {
                if (X > 0)
                {
                    X = (DATA32)DATA32_MAX;
                }
                if (X < 0)
                {
                    X = (DATA32)DATA32_MIN;
                }
            }
            else
            {
                X /= Y;
            }
            GH.Lms.PrimParPointer((DATA32)X);
        }

        public void cMathDivF()
        {
            DATAF X, Y;

            X = (DATAF)GH.Lms.PrimParPointer();
            Y = (DATAF)GH.Lms.PrimParPointer();
            X /= Y;
            GH.Lms.PrimParPointer((DATAF)X);
        }

        public void cMathOr8()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer();
            Tmp |= (DATA8)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA8)Tmp);
        }

        public void cMathOr16()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer();
            Tmp |= (DATA16)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA16)Tmp);
        }

        public void cMathOr32()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer();
            Tmp |= (DATA32)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA32)Tmp);
        }

        public void cMathAnd8()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer();
            Tmp &= (DATA8)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA8)Tmp);
        }

        public void cMathAnd16()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer();
            Tmp &= (DATA16)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA16)Tmp);
        }

        public void cMathAnd32()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer();
            Tmp &= (DATA32)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA32)Tmp);
        }

        public void cMathXor8()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer();
            Tmp ^= (DATA8)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA8)Tmp);
        }

        public void cMathXor16()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer();
            Tmp ^= (DATA16)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA16)Tmp);
        }

        public void cMathXor32()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer();
            Tmp ^= (DATA32)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA32)Tmp);
        }

        public void cMathRl8()
        {
            DATA8 Tmp;

            Tmp = (DATA8)GH.Lms.PrimParPointer();
            Tmp <<= (DATA8)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA8)Tmp);
        }

        public void cMathRl16()
        {
            DATA16 Tmp;

            Tmp = (DATA16)GH.Lms.PrimParPointer();
            Tmp <<= (DATA16)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA16)Tmp);
        }

        public void cMathRl32()
        {
            DATA32 Tmp;

            Tmp = (DATA32)GH.Lms.PrimParPointer();
            Tmp <<= (DATA32)GH.Lms.PrimParPointer();
            GH.Lms.PrimParPointer((DATA32)Tmp);
        }

        public static double DegToRad(double deg)
        {
            return (Math.PI / 180) * deg;
        }

        public static float DegToRad(float deg)
        {
            return (float)((Math.PI / 180) * deg);
        }

        public static double RadToDeg(double rad)
        {
            return (180 / Math.PI) * rad;
        }

        public static float RadToDeg(float rad)
        {
            return (float)((180 / Math.PI) * rad);
        }

        public void cMath()
        {
            DATAF X;
            DATAF Y;
            DATA8 X8;
            DATA8 Y8;
            DATA16 X16;
            DATA16 Y16;
            DATA32 X32;
            DATA32 Y32;
            DATA8 Cmd;
            char[] Buf = new char[64];
            char[] pBuf;

            Cmd = (DATA8)GH.Lms.PrimParPointer();

            switch (Cmd)
            {
                case EXP:
                    {
                        X = (DATAF)GH.Lms.PrimParPointer();
                        GH.Lms.PrimParPointer((DATAF)Math.Exp(X));
                    }
                    break;

                case POW:
                    {
                        X = (DATAF)GH.Lms.PrimParPointer();
                        Y = (DATAF)GH.Lms.PrimParPointer();
                        GH.Lms.PrimParPointer((DATAF)Math.Pow(X, Y));
                    }
                    break;

                case MOD8:
                    {
                        X8 = (DATA8)GH.Lms.PrimParPointer();
                        Y8 = (DATA8)GH.Lms.PrimParPointer();
                        GH.Lms.PrimParPointer((DATA8)X8 % Y8);
                    }
                    break;

                case MOD16:
                    {
                        X16 = (DATA16)GH.Lms.PrimParPointer();
                        Y16 = (DATA16)GH.Lms.PrimParPointer();
                        GH.Lms.PrimParPointer((DATA16)X16 % Y16);
                    }
                    break;

                case MOD32:
                    {
                        X32 = (DATA32)GH.Lms.PrimParPointer();
                        Y32 = (DATA32)GH.Lms.PrimParPointer();
                        GH.Lms.PrimParPointer((DATA32)X32 % Y32);
                    }
                    break;

                case MOD:
                    {
                        X = (DATAF)GH.Lms.PrimParPointer();
                        Y = (DATAF)GH.Lms.PrimParPointer();
                        GH.Lms.PrimParPointer((DATAF)X % Y);
                    }
                    break;

                case FLOOR:
                    {
                        X = (DATAF)GH.Lms.PrimParPointer();
                        GH.Lms.PrimParPointer((DATAF)Math.Floor(X));
                    }
                    break;

                case CEIL:
                    {
                        X = (DATAF)GH.Lms.PrimParPointer();
                        GH.Lms.PrimParPointer((DATAF)Math.Ceiling(X));
                    }
                    break;

                case ROUND:
                    {
                        X = (DATAF)GH.Lms.PrimParPointer();
                        if (X < (DATAF)0)
                        {
                            GH.Lms.PrimParPointer((DATAF)Math.Ceiling(X - (DATAF)0.5));
                        }
                        else
                        {
                            GH.Lms.PrimParPointer((DATAF)Math.Floor(X + (DATAF)0.5));
                        }
                    }
                    break;

                case ABS:
                    {
                        X = (DATAF)GH.Lms.PrimParPointer();
                        GH.Lms.PrimParPointer((DATAF)Math.Abs(X));
                    }
                    break;

                case NEGATE:
                    {
                        X = (DATAF)GH.Lms.PrimParPointer();
                        GH.Lms.PrimParPointer((DATAF)((DATAF)0 - X));
                    }
                    break;

                case TRUNC:
                    {
                        X = (DATAF)GH.Lms.PrimParPointer();
                        Y8 = (DATA8)GH.Lms.PrimParPointer();

                        if (Y8 > 9)
                        {
                            Y8 = 9;
                        }
                        if (Y8 < 0)
                        {
                            Y8 = 0;
                        }

                        // TODO: why the hell is the buf used 
                        //snprintf(Buf, 64, "%f", X);

                        //pBuf = strstr(Buf, ".");
                        //if (pBuf != NULL)
                        //{
                        //    pBuf[Y8 + 1] = 0;
                        //}
                        //sscanf(Buf, "%f", &X);
                        GH.Lms.PrimParPointer((DATAF)X);
                    }
                    break;

                case SQRT:
                    {
                        X = (DATAF)GH.Lms.PrimParPointer();
                        GH.Lms.PrimParPointer((DATAF)Math.Sqrt(X));
                    }
                    break;

                case LOG:
                    {
                        X = (DATAF)GH.Lms.PrimParPointer();
                        GH.Lms.PrimParPointer((DATAF)Math.Log10(X));
                    }
                    break;

                case LN:
                    {
                        X = (DATAF)GH.Lms.PrimParPointer();
                        GH.Lms.PrimParPointer((DATAF)Math.Log(X));
                    }
                    break;

                case SIN:
                    {
                        X = (DATAF)GH.Lms.PrimParPointer();
                        X = DegToRad(X);
                        X = (float)Math.Sin(X);
                        GH.Lms.PrimParPointer((DATAF)X);
                    }
                    break;

                case COS:
                    {
                        X = (DATAF)GH.Lms.PrimParPointer();
                        X = DegToRad(X);
                        X = (float)Math.Cos(X);
                        GH.Lms.PrimParPointer((DATAF)X);
                    }
                    break;

                case TAN:
                    {
                        X = (DATAF)GH.Lms.PrimParPointer();
                        X = DegToRad(X);
                        X = (float)Math.Tan(X);
                        GH.Lms.PrimParPointer((DATAF)X);
                    }
                    break;

                case ASIN:
                    {
                        X = (DATAF)GH.Lms.PrimParPointer();
                        X = (float)Math.Asin(X);
                        X = RadToDeg(X);
                        GH.Lms.PrimParPointer((DATAF)X);
                    }
                    break;

                case ACOS:
                    {
                        X = (DATAF)GH.Lms.PrimParPointer();
                        X = (float)Math.Acos(X);
                        X = RadToDeg(X);
                        GH.Lms.PrimParPointer((DATAF)X);
                    }
                    break;

                case ATAN:
                    {
                        X = (DATAF)GH.Lms.PrimParPointer();
                        X = (float)Math.Atan(X);
                        X = RadToDeg(X);
                        GH.Lms.PrimParPointer((DATAF)X);
                    }
                    break;

            }
        }
    }
}
