namespace Ev3CoreUnsafe.Enums
{
    /*! \enum DSPSTAT
     *
     *        Dispatch status values
     */
    public enum DSPSTAT 
    {
        NOBREAK = 0x0100,               //!< Dispatcher running (looping)
        STOPBREAK = 0x0200,               //!< Break because of program stop
        SLEEPBREAK = 0x0400,               //!< Break because of sleeping
        INSTRBREAK = 0x0800,               //!< Break because of opcode break
        BUSYBREAK = 0x1000,               //!< Break because of waiting for completion
        PRGBREAK = 0x2000,               //!< Break because of program break
        USERBREAK = 0x4000,               //!< Break because of user decision
        FAILBREAK = 0x8000                //!< Break because of fail
    }
}

namespace Ev3CoreUnsafe
{
	public partial class Defines
	{
		public const int NOBREAK = 0x0100;               //!< Dispatcher running (looping)
		public const int STOPBREAK = 0x0200;               //!< Break because of program stop
		public const int SLEEPBREAK = 0x0400;               //!< Break because of sleeping
		public const int INSTRBREAK = 0x0800;               //!< Break because of opcode break
		public const int BUSYBREAK = 0x1000;               //!< Break because of waiting for completion
		public const int PRGBREAK = 0x2000;               //!< Break because of program break
		public const int USERBREAK = 0x4000;               //!< Break because of user decision
        public const int FAILBREAK = 0x8000;                //!< Break because of fail
	}
}
