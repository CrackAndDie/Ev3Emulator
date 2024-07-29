namespace Ev3Core.Enums
{
	//! \page uibuttonsubcode Specific command parameter
	//!
	//!
	//! \verbatim
	//!

	public enum UI_BUTTON_SUBCODE
	{
		SHORTPRESS = 1,
		LONGPRESS = 2,
		WAIT_FOR_PRESS = 3,
		FLUSH = 4,
		PRESS = 5,
		RELEASE = 6,
		GET_HORZ = 7,
		GET_VERT = 8,
		PRESSED = 9,
		SET_BACK_BLOCK = 10,
		GET_BACK_BLOCK = 11,
		TESTSHORTPRESS = 12,
		TESTLONGPRESS = 13,
		GET_BUMBED = 14,
		GET_CLICK = 15,

		UI_BUTTON_SUBCODES
	}
}

namespace Ev3Core
{
	public partial class Defines
	{
		public const int SHORTPRESS = 1;
		public const int LONGPRESS = 2;
		public const int WAIT_FOR_PRESS = 3;
		public const int FLUSH = 4;
		public const int PRESS = 5;
		public const int RELEASE = 6;
		public const int GET_HORZ = 7;
		public const int GET_VERT = 8;
		public const int PRESSED = 9;
		public const int SET_BACK_BLOCK = 10;
		public const int GET_BACK_BLOCK = 11;
		public const int TESTSHORTPRESS = 12;
		public const int TESTLONGPRESS = 13;
		public const int GET_BUMBED = 14;
		public const int GET_CLICK = 15;

		public const int UI_BUTTON_SUBCODES = 16;
	}
}
