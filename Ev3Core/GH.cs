using Ev3Core.Lms2012;
using Ev3Core.Lms2012.Interfaces;

namespace Ev3Core
{
    public static class GH // Global Holder
    {
        public readonly static ILms Lms = new Lms();
    }
}
