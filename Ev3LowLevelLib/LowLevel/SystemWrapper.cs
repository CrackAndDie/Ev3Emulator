using System.Runtime.InteropServices;

namespace Ev3Emulator.LowLevel
{
	public static class SystemWrapper
	{
		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void w_system_startMain();

        [DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
        private extern static void w_system_stopMain();

        public static event Action LmsExited;

		public static void MainLms()
		{
			w_system_startMain();
			LmsExited?.Invoke();
		}

		public static void StopLms()
		{
			w_system_stopMain();
        }
	}
}
