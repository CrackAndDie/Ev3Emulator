namespace Ev3CoreUnsafe.Enums
{
	public enum BTTYPE
	{
		BTTYPE_PC = 3,    //!< Bluetooth type PC
		BTTYPE_PHONE = 4,    //!< Bluetooth type PHONE
		BTTYPE_BRICK = 5,    //!< Bluetooth type BRICK
		BTTYPE_UNKNOWN = 6,    //!< Bluetooth type UNKNOWN

		BTTYPES
	}
}

namespace Ev3CoreUnsafe
{
	public partial class Defines
	{
		public const int BTTYPE_PC = 3;    //!< Bluetooth type PC
		public const int BTTYPE_PHONE = 4;    //!< Bluetooth type PHONE
		public const int BTTYPE_BRICK = 5;    //!< Bluetooth type BRICK
		public const int BTTYPE_UNKNOWN = 6;    //!< Bluetooth type UNKNOWN

		public const int BTTYPES = 7;
	}
}
