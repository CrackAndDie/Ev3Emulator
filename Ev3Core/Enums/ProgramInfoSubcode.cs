namespace Ev3Core.Enums
{
	//! \page programinfosubcode Specific command parameter
	//!
	//!
	//! \verbatim
	//!

	public enum PROGRAM_INFO_SUBCODE
	{
		OBJ_STOP = 0,    // VM
		OBJ_START = 4,    // VM
		GET_STATUS = 22,   // VM
		GET_SPEED = 23,   // VM
		GET_PRGRESULT = 24,   // VM
		SET_INSTR = 25,   // VM

		PROGRAM_INFO_SUBCODES,
	}
}

namespace Ev3Core
{
	public partial class Defines
	{
		public const int OBJ_STOP = 0;    // VM
		public const int OBJ_START = 4;    // VM
		public const int GET_STATUS = 22;   // VM
		public const int GET_SPEED = 23;   // VM
		public const int GET_PRGRESULT = 24;   // VM
		public const int SET_INSTR = 25;   // VM

		public const int PROGRAM_INFO_SUBCODES = 26;
	}
}
