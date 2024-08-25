using System;
using System.Runtime.InteropServices;

namespace Ev3Emulator.LowLevel
{
	public static class MotorsWrapper
	{
		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_motors_getBusyFlags(reg_w_motors_getBusyFlagsAction getBusyFlags);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void reg_w_motors_getBusyFlagsAction(ref int f1, ref int f2);

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_motors_setBusyFlags(reg_w_motors_setBusyFlagsAction setBusyFlags);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void reg_w_motors_setBusyFlagsAction(int f1);

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_motors_setData(reg_w_motors_setDataAction setData);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void reg_w_motors_setDataAction(IntPtr data, int len);

		// TODO: custom handler post processed
		public static void Init()
		{
			reg_w_motors_getBusyFlags(GetBusyFlags);
			reg_w_motors_setBusyFlags(SetBusyFlags);
			reg_w_motors_setData(SetData);
		}

		private static void GetBusyFlags(ref int f1, ref int f2)
		{
			// TODO: 
		}

		private static void SetBusyFlags(int f1)
		{
			// TODO: 
		}

		private static void SetData(IntPtr data, int len)
		{
			// TODO: 
		}
	}
}
