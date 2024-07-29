namespace Ev3Core.Enums
{
	//! \page memoryfilenamesubcode Specific command parameter
	//!
	//! \verbatim
	//!

	public enum FILENAME_SUBCODE
	{
		EXIST = 16,     //!< MUST BE GREATER OR EQUAL TO "ARRAY_SUBCODES"
		TOTALSIZE = 17,
		SPLIT = 18,
		MERGE = 19,
		CHECK = 20,
		PACK = 21,
		UNPACK = 22,
		GET_FOLDERNAME = 23,

		FILENAME_SUBCODES
	}
}

namespace Ev3Core
{
	public partial class Defines
	{
		public const int EXIST = 16;     //!< MUST BE GREATER OR EQUAL TO "ARRAY_SUBCODES"
		public const int TOTALSIZE = 17;
		public const int SPLIT = 18;
		public const int MERGE = 19;
		public const int CHECK = 20;
		public const int PACK = 21;
		public const int UNPACK = 22;
		public const int GET_FOLDERNAME = 23;

		public const int FILENAME_SUBCODES = 24;
	}
}