using Ev3CoreUnsafe.Enums;
using System.Runtime.InteropServices;

namespace Ev3CoreUnsafe.Lms2012.Interfaces
{
	public unsafe interface IValidate
	{
		RESULT cValidateInit();

		RESULT cValidateExit();

		RESULT cValidateDisassemble(IP pI, IMINDEX* pIndex, LABEL* pLabel);

		RESULT cValidateProgram(PRGID PrgId, IP pI, LABEL* pLabel, DATA8 Disassemble);
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct VALIDATE_GLOBALS
	{
		//*****************************************************************************
		// Validate Global variables
		//*****************************************************************************

		public int Row;
		public IMINDEX ValidateErrorIndex;
	}
}
