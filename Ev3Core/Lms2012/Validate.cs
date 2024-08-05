using Ev3Core.Enums;
using Ev3Core.Lms2012.Interfaces;

namespace Ev3Core.Lms2012
{
    public class Validate : IValidate
    {
        public RESULT cValidateDisassemble(GP pI, VarPointer<uint> pIndex, ArrayPointer<LABEL> pLabel)
        {
            throw new NotImplementedException();
        }

        public RESULT cValidateExit()
        {
            throw new NotImplementedException();
        }

        public RESULT cValidateInit()
        {
            throw new NotImplementedException();
        }

        public RESULT cValidateProgram(ushort PrgId, GP pI, ArrayPointer<LABEL> pLabel, sbyte Disassemble)
        {
            throw new NotImplementedException();
        }
    }
}
