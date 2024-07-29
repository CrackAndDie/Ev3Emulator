namespace Ev3Core.Enums
{
	//! \page uireadsubcode Specific command parameter
	//!
	//!
	//! \verbatim
	//!

	public enum UI_READ_SUBCODE
	{
		GET_VBATT = 1,
		GET_IBATT = 2,
		GET_OS_VERS = 3,
		GET_EVENT = 4,
		GET_TBATT = 5,
		GET_IINT = 6,
		GET_IMOTOR = 7,
		GET_STRING = 8,
		GET_HW_VERS = 9,
		GET_FW_VERS = 10,
		GET_FW_BUILD = 11,
		GET_OS_BUILD = 12,
		GET_ADDRESS = 13,
		GET_CODE = 14,
		KEY = 15,
		GET_SHUTDOWN = 16,
		GET_WARNING = 17,
		GET_LBATT = 18,
		TEXTBOX_READ = 21,
		GET_VERSION = 26,
		GET_IP = 27,
		GET_POWER = 29,
		GET_SDCARD = 30,
		GET_USBSTICK = 31,

		UI_READ_SUBCODES
	}
}

namespace Ev3Core
{
	public partial class Defines
	{
		public const int GET_VBATT = 1;
		public const int GET_IBATT = 2;
		public const int GET_OS_VERS = 3;
		public const int GET_EVENT = 4;
		public const int GET_TBATT = 5;
		public const int GET_IINT = 6;
		public const int GET_IMOTOR = 7;
		public const int GET_STRING = 8;
		public const int GET_HW_VERS = 9;
		public const int GET_FW_VERS = 10;
		public const int GET_FW_BUILD = 11;
		public const int GET_OS_BUILD = 12;
		public const int GET_ADDRESS = 13;
		public const int GET_CODE = 14;
		public const int KEY = 15;
		public const int GET_SHUTDOWN = 16;
		public const int GET_WARNING = 17;
		public const int GET_LBATT = 18;
		public const int TEXTBOX_READ = 21;
		public const int GET_VERSION = 26;
		public const int GET_IP = 27;
		public const int GET_POWER = 29;
		public const int GET_SDCARD = 30;
		public const int GET_USBSTICK = 31;

		public const int UI_READ_SUBCODES = 32;
	}
}