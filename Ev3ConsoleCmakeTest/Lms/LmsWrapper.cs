using System.Runtime.InteropServices;

namespace Ev3ConsoleCmakeTest.Lms
{
	public static class LmsWrapper
	{
		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		public extern static void w_system_startMain();

		public static void Lms()
		{
			w_system_startMain();
		}
	}
}
