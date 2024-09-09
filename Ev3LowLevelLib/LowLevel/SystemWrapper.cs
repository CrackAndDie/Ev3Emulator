using System.Runtime.InteropServices;

namespace Ev3Emulator.LowLevel
{
	public static class SystemWrapper
	{
		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void w_system_startMain();

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_system_getStopMain(reg_w_system_getStopMainAction getStopMain);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate byte reg_w_system_getStopMainAction();

		public static void Init()
		{
			reg_w_system_getStopMain(GetStopMain);
		}

		public static void MainLms()
		{
			_isVmShouldBeStopped = false;

			w_system_startMain();
			LmsExited?.Invoke();
		}

		public static void StopLms()
		{
			lock (_vmStopLock)
			{
				_isVmShouldBeStopped = true;
				_isVmShouldBeStoppedCalled = false;
			}
		}

		private static byte GetStopMain()
		{
			lock (_vmStopLock)
			{
				_isVmShouldBeStoppedCalled = true;
				return _isVmShouldBeStopped ? (byte)1 : (byte)0;
			}
		}

		public static event Action LmsExited;

		private static bool _isVmShouldBeStopped = false;
		private static bool _isVmShouldBeStoppedCalled = false;
		private static object _vmStopLock = new object();
	}
}
