using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ev3Core.Lms2012.Interfaces
{
    public interface IMath
    {

        void cMathAdd8();

        void cMathAdd16();

        void cMathAdd32();

        void cMathAddF();

        void cMathSub8();

        void cMathSub16();

        void cMathSub32();

        void cMathSubF();

        void cMathMul8();

        void cMathMul16();

        void cMathMul32();

        void cMathMulF();

        void cMathDiv8();

        void cMathDiv16();

        void cMathDiv32();

        void cMathDivF();

        void cMathOr8();

        void cMathOr16();

        void cMathOr32();

        void cMathAnd8();

        void cMathAnd16();

        void cMathAnd32();

        void cMathXor8();

        void cMathXor16();

        void cMathXor32();

        void cMathRl8();

        void cMathRl16();

        void cMathRl32();

        void cMath();
    }
}
