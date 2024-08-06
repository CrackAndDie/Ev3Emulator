namespace Ev3CoreUnsafe.Enums
{
	public enum TST_SUBCODE
	{
		TST_OPEN = 10,   //!< MUST BE GREATER OR EQUAL TO "INFO_SUBCODES"
		TST_CLOSE = 11,
		TST_READ_PINS = 12,
		TST_WRITE_PINS = 13,
		TST_READ_ADC = 14,
		TST_WRITE_UART = 15,
		TST_READ_UART = 16,
		TST_ENABLE_UART = 17,
		TST_DISABLE_UART = 18,
		TST_ACCU_SWITCH = 19,
		TST_BOOT_MODE2 = 20,
		TST_POLL_MODE2 = 21,
		TST_CLOSE_MODE2 = 22,
		TST_RAM_CHECK = 23,

		TST_SUBCODES
	}
}

namespace Ev3CoreUnsafe
{
	public partial class Defines
	{
		public const int TST_OPEN = 10;   //!< MUST BE GREATER OR EQUAL TO "INFO_SUBCODES"
		public const int TST_CLOSE = 11;
		public const int TST_READ_PINS = 12;
		public const int TST_WRITE_PINS = 13;
		public const int TST_READ_ADC = 14;
		public const int TST_WRITE_UART = 15;
		public const int TST_READ_UART = 16;
		public const int TST_ENABLE_UART = 17;
		public const int TST_DISABLE_UART = 18;
		public const int TST_ACCU_SWITCH = 19;
		public const int TST_BOOT_MODE2 = 20;
		public const int TST_POLL_MODE2 = 21;
		public const int TST_CLOSE_MODE2 = 22;
		public const int TST_RAM_CHECK = 23;

		public const int TST_SUBCODES = 24;
	}
}
