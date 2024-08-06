namespace Ev3CoreUnsafe.Enums
{
	public enum FONTTYPE
	{
		NORMAL_FONT = 0,
		SMALL_FONT = 1,
		LARGE_FONT = 2,
		TINY_FONT = 3,

		FONTTYPES                             //!< Maximum font types supported by the VM
	}
}

namespace Ev3CoreUnsafe
{
	public partial class Defines
	{
		public const int NORMAL_FONT = 0;
		public const int SMALL_FONT = 1;
		public const int LARGE_FONT = 2;
		public const int TINY_FONT = 3;

		public const int FONTTYPES = 4;                            //!< Maximum font types supported by the VM
	}
}
