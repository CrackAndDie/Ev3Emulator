namespace Ev3CoreUnsafe.Enums
{
	//! \page infosubcode Specific command parameter
	//!
	//!
	//! \verbatim
	//!

	public enum INFO_SUBCODE
	{
		SET_ERROR = 1,
		GET_ERROR = 2,
		ERRORTEXT = 3,

		GET_VOLUME = 4,
		SET_VOLUME = 5,
		GET_MINUTES = 6,
		SET_MINUTES = 7,

		INFO_SUBCODES
	}
}

namespace Ev3CoreUnsafe
{
	public partial class Defines
	{
		public const int SET_ERROR = 1;
		public const int GET_ERROR = 2;
		public const int ERRORTEXT = 3;

		public const int GET_VOLUME = 4;
		public const int SET_VOLUME = 5;
		public const int GET_MINUTES = 6;
		public const int SET_MINUTES = 7;

		public const int INFO_SUBCODES = 8;
	}
}

