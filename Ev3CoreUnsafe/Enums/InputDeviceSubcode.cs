namespace Ev3CoreUnsafe.Enums
{
	//! \page inputdevicesubcode Specific command parameter
	//!
	//!
	//! \verbatim
	//!

	public enum INPUT_DEVICE_SUBCODE
	{
		GET_FORMAT = 2,
		CAL_MINMAX = 3,
		CAL_DEFAULT = 4,
		GET_TYPEMODE = 5,
		GET_SYMBOL = 6,
		CAL_MIN = 7,
		CAL_MAX = 8,
		SETUP = 9,
		CLR_ALL = 10,
		GET_RAW = 11,
		GET_CONNECTION = 12,
		STOP_ALL = 13,
		GET_NAME = 21,
		GET_MODENAME = 22,
		SET_RAW = 23,
		GET_FIGURES = 24,
		GET_CHANGES = 25,
		CLR_CHANGES = 26,
		READY_PCT = 27,
		READY_RAW = 28,
		READY_SI = 29,
		GET_MINMAX = 30,
		GET_BUMPS = 31,

		INPUT_DEVICESUBCODES
	}
}

namespace Ev3CoreUnsafe
{
	public partial class Defines
	{
		public const int GET_FORMAT = 2;
		public const int CAL_MINMAX = 3;
		public const int CAL_DEFAULT = 4;
		public const int GET_TYPEMODE = 5;
		public const int GET_SYMBOL = 6;
		public const int CAL_MIN = 7;
		public const int CAL_MAX = 8;
		public const int SETUP = 9;
		public const int CLR_ALL = 10;
		public const int GET_RAW = 11;
		public const int GET_CONNECTION = 12;
		public const int STOP_ALL = 13;
		public const int GET_NAME = 21;
		public const int GET_MODENAME = 22;
		public const int SET_RAW = 23;
		public const int GET_FIGURES = 24;
		public const int GET_CHANGES = 25;
		public const int CLR_CHANGES = 26;
		public const int READY_PCT = 27;
		public const int READY_RAW = 28;
		public const int READY_SI = 29;
		public const int GET_MINMAX = 30;
		public const int GET_BUMPS = 31;

		public const int INPUT_DEVICESUBCODES = 32;
	}
}
