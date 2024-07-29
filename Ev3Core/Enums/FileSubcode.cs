namespace Ev3Core.Enums
{
	//! \page memoryfilesubcode Specific command parameter
	//!
	//!
	//! \verbatim
	//!

	public enum FILE_SUBCODE
	{
		OPEN_APPEND = 0,
		OPEN_READ = 1,
		OPEN_WRITE = 2,
		READ_VALUE = 3,
		WRITE_VALUE = 4,
		READ_TEXT = 5,
		WRITE_TEXT = 6,
		CLOSE = 7,
		LOAD_IMAGE = 8,
		GET_HANDLE = 9,
		MAKE_FOLDER = 10,
		GET_POOL = 11,
		SET_LOG_SYNC_TIME = 12,
		GET_FOLDERS = 13,
		GET_LOG_SYNC_TIME = 14,
		GET_SUBFOLDER_NAME = 15,
		WRITE_LOG = 16,
		CLOSE_LOG = 17,
		GET_IMAGE = 18,
		GET_ITEM = 19,
		GET_CACHE_FILES = 20,
		PUT_CACHE_FILE = 21,
		GET_CACHE_FILE = 22,
		DEL_CACHE_FILE = 23,
		DEL_SUBFOLDER = 24,
		GET_LOG_NAME = 25,

		OPEN_LOG = 27,
		READ_BYTES = 28,
		WRITE_BYTES = 29,
		REMOVE = 30,
		MOVE = 31,

		FILE_SUBCODES
	}
}

namespace Ev3Core
{
	public partial class Defines
	{
		public const int OPEN_APPEND = 0;
		public const int OPEN_READ = 1;
		public const int OPEN_WRITE = 2;
		public const int READ_VALUE = 3;
		public const int WRITE_VALUE = 4;
		public const int READ_TEXT = 5;
		public const int WRITE_TEXT = 6;
		public const int CLOSE = 7;
		public const int LOAD_IMAGE = 8;
		public const int GET_HANDLE = 9;
		public const int MAKE_FOLDER = 10;
		public const int GET_POOL = 11;
		public const int SET_LOG_SYNC_TIME = 12;
		public const int GET_FOLDERS = 13;
		public const int GET_LOG_SYNC_TIME = 14;
		public const int GET_SUBFOLDER_NAME = 15;
		public const int WRITE_LOG = 16;
		public const int CLOSE_LOG = 17;
		public const int GET_IMAGE = 18;
		public const int GET_ITEM = 19;
		public const int GET_CACHE_FILES = 20;
		public const int PUT_CACHE_FILE = 21;
		public const int GET_CACHE_FILE = 22;
		public const int DEL_CACHE_FILE = 23;
		public const int DEL_SUBFOLDER = 24;
		public const int GET_LOG_NAME = 25;

		public const int OPEN_LOG = 27;
		public const int READ_BYTES = 28;
		public const int WRITE_BYTES = 29;
		public const int REMOVE = 30;
		public const int MOVE = 31;

		public const int FILE_SUBCODES = 32;
	}
}
