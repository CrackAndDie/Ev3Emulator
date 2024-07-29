namespace Ev3Core.Enums
{
	//! \page stringsubcode Specific command parameter
	//!
	//!
	//! \verbatim
	//!

	public enum STRING_SUBCODE
	{
		GET_SIZE = 1,    // VM       get string size
		ADD = 2,    // VM       add two strings
		COMPARE = 3,    // VM       compare two strings
		DUPLICATE = 5,    // VM       duplicate one string to another
		VALUE_TO_STRING = 6,
		STRING_TO_VALUE = 7,
		STRIP = 8,
		NUMBER_TO_STRING = 9,
		SUB = 10,
		VALUE_FORMATTED = 11,
		NUMBER_FORMATTED = 12,

		STRING_SUBCODES
	}
}

namespace Ev3Core
{
	public partial class Defines
	{
		public const int GET_SIZE = 1;    // VM       get string size
		public const int ADD = 2;    // VM       add two strings
		public const int COMPARE = 3;    // VM       compare two strings
		public const int DUPLICATE = 5;    // VM       duplicate one string to another
		public const int VALUE_TO_STRING = 6;
		public const int STRING_TO_VALUE = 7;
		public const int STRIP = 8;
		public const int NUMBER_TO_STRING = 9;
		public const int SUB = 10;
		public const int VALUE_FORMATTED = 11;
		public const int NUMBER_FORMATTED = 12;

		public const int STRING_SUBCODES = 13;
	}
}
