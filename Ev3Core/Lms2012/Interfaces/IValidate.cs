using Ev3Core.Enums;

namespace Ev3Core.Lms2012.Interfaces
{
    public interface IValidate
    {
        RESULT cValidateInit();

        RESULT cValidateExit();

        RESULT cValidateDisassemble(IP pI, VarPointer<IMINDEX> pIndex, ArrayPointer<LABEL> pLabel);

        RESULT cValidateProgram(PRGID PrgId, IP pI, ArrayPointer<LABEL> pLabel, DATA8 Disassemble);
    }

    public class VALIDATE_GLOBALS
    {
        //*****************************************************************************
        // Validate Global variables
        //*****************************************************************************

        public int Row;
        public IMINDEX ValidateErrorIndex;
    }
}
