using System;
using System.Runtime.InteropServices;

namespace Ev3Emulator.LowLevel
{
	public static class TimeWrapper
	{
		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_time_getMs(reg_w_time_getMsAction getMs);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate uint reg_w_time_getMsAction();

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_time_getUs(reg_w_time_getUsAction getUs);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate uint reg_w_time_getUsAction();

		public static void Init()
		{
			reg_w_time_getMs(GetMs);
			reg_w_time_getUs(GetUs);
		}

		private static uint GetMs()
		{
			return (uint)DateTime.Now.TimeOfDay.TotalMilliseconds;
		}

		private static uint GetUs()
		{
			return (uint)DateTime.Now.TimeOfDay.TotalMicroseconds;
		}
	}
}
