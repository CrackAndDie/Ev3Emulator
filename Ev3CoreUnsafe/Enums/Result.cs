namespace Ev3CoreUnsafe.Enums
{
    public enum RESULT : UBYTE
    {
        OK = 0,                    //!< No errors to report
        BUSY = 1,                    //!< Busy - try again
        FAIL = 2,                    //!< Something failed
        STOP = 4                     //!< Stopped
    }
}

namespace Ev3CoreUnsafe
{
	public partial class Defines
	{
		public const int OK = 0;                    //!< No errors to report
		public const int BUSY = 1;                    //!< Busy - try again
		public const int FAIL = 2;                    //!< Something failed
		public const int STOP = 4;                    //!< Stopped
	}
}
