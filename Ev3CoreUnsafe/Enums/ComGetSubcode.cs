namespace Ev3CoreUnsafe.Enums
{
	//! \page comgetsubcode Specific command parameter
	//!
	//! \verbatim
	//!

	public enum COM_GET_SUBCODE
	{
		GET_ON_OFF = 1,                    //!< Set, Get
		GET_VISIBLE = 2,                    //!< Set, Get
		GET_RESULT = 4,                    //!<      Get
		GET_PIN = 5,                    //!< Set, Get
		SEARCH_ITEMS = 8,                    //!<      Get
		SEARCH_ITEM = 9,                    //!<      Get
		FAVOUR_ITEMS = 10,                   //!<      Get
		FAVOUR_ITEM = 11,                   //!<      Get
		GET_ID = 12,
		GET_BRICKNAME = 13,
		GET_NETWORK = 14,
		GET_PRESENT = 15,
		GET_ENCRYPT = 16,
		CONNEC_ITEMS = 17,
		CONNEC_ITEM = 18,
		GET_INCOMING = 19,
		GET_MODE2 = 20,

		COM_GET_SUBCODES
	}
}

namespace Ev3CoreUnsafe
{
	public partial class Defines
	{
		public const int GET_ON_OFF = 1;                    //!< Set; Get
		public const int GET_VISIBLE = 2;                    //!< Set; Get
		public const int GET_RESULT = 4;                    //!<      Get
		public const int GET_PIN = 5;                    //!< Set; Get
		public const int SEARCH_ITEMS = 8;                    //!<      Get
		public const int SEARCH_ITEM = 9;                    //!<      Get
		public const int FAVOUR_ITEMS = 10;                   //!<      Get
		public const int FAVOUR_ITEM = 11;                   //!<      Get
		public const int GET_ID = 12;
		public const int GET_BRICKNAME = 13;
		public const int GET_NETWORK = 14;
		public const int GET_PRESENT = 15;
		public const int GET_ENCRYPT = 16;
		public const int CONNEC_ITEMS = 17;
		public const int CONNEC_ITEM = 18;
		public const int GET_INCOMING = 19;
		public const int GET_MODE2 = 20;

		public const int COM_GET_SUBCODES = 21;
	}
}
