namespace Ev3Core.Enums
{
	//! \page uiwritesubcode Specific command parameter
	//!
	//!
	//! \verbatim
	//!

	public enum UI_WRITE_SUBCODE
	{
		WRITE_FLUSH = 1,
		FLOATVALUE = 2,
		STAMP = 3,
		PUT_STRING = 8,
		VALUE8 = 9,
		VALUE16 = 10,
		VALUE32 = 11,
		VALUEF = 12,
		ADDRESS = 13,
		CODE = 14,
		DOWNLOAD_END = 15,
		SCREEN_BLOCK = 16,
		TEXTBOX_APPEND = 21,
		SET_BUSY = 22,
		SET_TESTPIN = 24,
		INIT_RUN = 25,
		UPDATE_RUN = 26,
		LED = 27,
		POWER = 29,
		GRAPH_SAMPLE = 30,
		TERMINAL = 31,

		UI_WRITE_SUBCODES
	}
}

namespace Ev3Core
{
	public partial class Defines
	{
		public const int WRITE_FLUSH = 1;
		public const int FLOATVALUE = 2;
		public const int STAMP = 3;
		public const int PUT_STRING = 8;
		public const int VALUE8 = 9;
		public const int VALUE16 = 10;
		public const int VALUE32 = 11;
		public const int VALUEF = 12;
		public const int ADDRESS = 13;
		public const int CODE = 14;
		public const int DOWNLOAD_END = 15;
		public const int SCREEN_BLOCK = 16;
		public const int TEXTBOX_APPEND = 21;
		public const int SET_BUSY = 22;
		public const int SET_TESTPIN = 24;
		public const int INIT_RUN = 25;
		public const int UPDATE_RUN = 26;
		public const int LED = 27;
		public const int POWER = 29;
		public const int GRAPH_SAMPLE = 30;
		public const int TERMINAL = 31;

		public const int UI_WRITE_SUBCODES = 32;
	}
}
