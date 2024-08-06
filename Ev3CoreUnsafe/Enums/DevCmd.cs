namespace Ev3CoreUnsafe.Enums
{
	public enum DEVCMD
	{
		DEVCMD_RESET = 0x11,           //!< UART device reset
		DEVCMD_FIRE = 0x11,           //!< UART device fire   (ultrasonic)
		DEVCMD_CHANNEL = 0x12,           //!< UART device channel (IR seeker)

		DEVCMDS
	}
}

namespace Ev3CoreUnsafe
{
	public partial class Defines
	{
		public const int DEVCMD_RESET = 0x11;           //!< UART device reset
		public const int DEVCMD_FIRE = 0x11;           //!< UART device fire   (ultrasonic)
		public const int DEVCMD_CHANNEL = 0x12;           //!< UART device channel (IR seeker)

		public const int DEVCMDS = 19;
	}
}
