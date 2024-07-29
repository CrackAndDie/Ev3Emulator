namespace Ev3Core.Enums
{
	//! \page memoryarraysubcode Specific command parameter
	//!
	//!
	//! \verbatim
	//!

	public enum ARRAY_SUBCODE
	{
		DELETE = 0,
		CREATE8 = 1,
		CREATE16 = 2,
		CREATE32 = 3,
		CREATEF = 4,
		RESIZE = 5,
		FILL = 6,
		COPY = 7,
		INIT8 = 8,
		INIT16 = 9,
		INIT32 = 10,
		INITF = 11,
		SIZE = 12,
		READ_CONTENT = 13,
		WRITE_CONTENT = 14,
		READ_SIZE = 15,

		ARRAY_SUBCODES
	}
}

namespace Ev3Core
{
	public partial class Defines
	{
		public const int DELETE = 0;
		public const int CREATE8 = 1;
		public const int CREATE16 = 2;
		public const int CREATE32 = 3;
		public const int CREATEF = 4;
		public const int RESIZE = 5;
		public const int FILL = 6;
		public const int COPY = 7;
		public const int INIT8 = 8;
		public const int INIT16 = 9;
		public const int INIT32 = 10;
		public const int INITF = 11;
		public const int SIZE = 12;
		public const int READ_CONTENT = 13;
		public const int WRITE_CONTENT = 14;
		public const int READ_SIZE = 15;

		public const int ARRAY_SUBCODES = 16;
	}
}
