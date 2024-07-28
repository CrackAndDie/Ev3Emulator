namespace Ev3Core.Enums
{
    public enum RESULT : UBYTE
    {
        OK = 0,                    //!< No errors to report
        BUSY = 1,                    //!< Busy - try again
        FAIL = 2,                    //!< Something failed
        STOP = 4                     //!< Stopped
    }
}
