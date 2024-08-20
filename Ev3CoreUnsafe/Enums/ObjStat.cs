namespace Ev3CoreUnsafe.Enums
{
	public enum OBJSTAT
	{
		RUNNING = 0x0010,                     //!< Object code is running
		WAITING = 0x0020,                     //!< Object is waiting for final trigger
		STOPPED = 0x0040,                     //!< Object is stopped or not triggered yet
		HALTED = 0x0080,                     //!< Object is halted because a call is in progress
	}
}

namespace Ev3CoreUnsafe
{
	public partial class Defines
	{
		public const int RUNNING = 0x0010;                     //!< Object code is running
		public const int WAITING = 0x0020;                     //!< Object is waiting for final trigger
		public const int STOPPED = 0x0040;                     //!< Object is stopped or not triggered yet
		public const int HALTED = 0x0080;
	}
}
