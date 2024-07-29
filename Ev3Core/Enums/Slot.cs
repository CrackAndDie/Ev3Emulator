namespace Ev3Core.Enums
{
	/*! \page programid Program ID's (Slots)

    \anchor prgid

    \verbatim */

	public enum SLOT
	{
		GUI_SLOT = 0,    //!< Program slot reserved for executing the user interface
		USER_SLOT = 1,    //!< Program slot used to execute user projects, apps and tools
		CMD_SLOT = 2,    //!< Program slot used for direct commands coming from c_com
		TERM_SLOT = 3,    //!< Program slot used for direct commands coming from c_ui
		DEBUG_SLOT = 4,    //!< Program slot used to run the debug ui

		SLOTS,                                //!< Maximum slots supported by the VM

		// ONLY VALID IN opPROGRAM_STOP
		CURRENT_SLOT = -1
	}
}

namespace Ev3Core
{
	public partial class Defines
	{
		public const int GUI_SLOT = 0;    //!< Program slot reserved for executing the user interface
		public const int USER_SLOT = 1;    //!< Program slot used to execute user projects, apps and tools
		public const int CMD_SLOT = 2;    //!< Program slot used for direct commands coming from c_com
		public const int TERM_SLOT = 3;    //!< Program slot used for direct commands coming from c_ui
		public const int DEBUG_SLOT = 4;    //!< Program slot used to run the debug ui

		public const int SLOTS = 5;                                //!< Maximum slots supported by the VM

		// ONLY VALID IN opPROGRAM_STOP
		public const int CURRENT_SLOT = -1;
	}
}
