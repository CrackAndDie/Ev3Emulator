using System.Runtime.InteropServices;

namespace Ev3Emulator.LowLevel
{
	public static class SystemWrapper
	{
		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void w_system_startMain();

		public static void Main()
		{
			w_system_startMain();
		}
	}
}
