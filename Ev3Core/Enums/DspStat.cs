namespace Ev3Core.Enums
{
    /*! \enum DSPSTAT
     *
     *        Dispatch status values
     */
    public enum DSPSTAT : int
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
