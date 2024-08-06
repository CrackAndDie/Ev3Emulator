namespace Ev3CoreUnsafe.Enums
{
	//! \page comsetsubcode Specific command parameter
	//!
	//! \verbatim
	//!

	public enum COM_SET_SUBCODE
	{
		SET_ON_OFF = 1,                    //!< Set, Get
		SET_VISIBLE = 2,                    //!< Set, Get
		SET_SEARCH = 3,                    //!< Set
		SET_PIN = 5,                    //!< Set, Get
		SET_PASSKEY = 6,                    //!< Set
		SET_CONNECTION = 7,                    //!< Set
		SET_BRICKNAME = 8,
		SET_MOVEUP = 9,
		SET_MOVEDOWN = 10,
		SET_ENCRYPT = 11,
		SET_SSID = 12,
		SET_MODE2 = 13,

		COM_SET_SUBCODES
	}
}

namespace Ev3CoreUnsafe
{
	public partial class Defines
	{
		public const int SET_ON_OFF = 1;                    //!< Set; Get
		public const int SET_VISIBLE = 2;                    //!< Set; Get
		public const int SET_SEARCH = 3;                    //!< Set
		public const int SET_PIN = 5;                    //!< Set; Get
		public const int SET_PASSKEY = 6;                    //!< Set
		public const int SET_CONNECTION = 7;                    //!< Set
		public const int SET_BRICKNAME = 8;
		public const int SET_MOVEUP = 9;
		public const int SET_MOVEDOWN = 10;
		public const int SET_ENCRYPT = 11;
		public const int SET_SSID = 12;
		public const int SET_MODE2 = 13;

		public const int COM_SET_SUBCODES = 14;
	}
}
