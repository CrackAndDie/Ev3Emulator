namespace Ev3Core.Enums
{
	public enum WARNING
	{	
		WARNING_TEMP = 0x01,
		WARNING_CURRENT = 0x02,
		WARNING_VOLTAGE = 0x04,
		WARNING_MEMORY = 0x08,
		WARNING_DSPSTAT = 0x10,

		WARNING_BATTLOW = 0x40,
		WARNING_BUSY = 0x80,

		WARNINGS = 0x3F
	}
}

namespace Ev3Core
{
	public partial class Defines
	{
		public const int WARNING_TEMP = 0x01;
		public const int WARNING_CURRENT = 0x02;
		public const int WARNING_VOLTAGE = 0x04;
		public const int WARNING_MEMORY = 0x08;
		public const int WARNING_DSPSTAT = 0x10;

		public const int WARNING_BATTLOW = 0x40;
		public const int WARNING_BUSY = 0x80;

		public const int WARNINGS = 0x3F;
	}
}
