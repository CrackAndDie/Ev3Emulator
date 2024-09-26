using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Ev3Emulator.LowLevel
{
	public unsafe struct MOTORDATA
	{
		public int TachoCounts; // probably unreseted
		public sbyte Speed;
		public int TachoSensor; // probably reseted after each task 
	}

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

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_motors_updateMotorData(reg_w_motors_updateMotorDataAction updateMotorData);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void reg_w_motors_updateMotorDataAction(IntPtr data, int index, byte isReset);

		// TODO: custom handler post processed
		public static void Init()
		{
			reg_w_motors_getBusyFlags(GetBusyFlags);
			reg_w_motors_setBusyFlags(SetBusyFlags);
			reg_w_motors_setData(SetData);
			reg_w_motors_updateMotorData(UpdateMotorData);
		}

		public static void SetMotorTachoDelta(int port, int delta)
		{
			// TODO: locks
			CurrentMotorData[port].TachoCounts += delta;
			CurrentMotorData[port].TachoSensor += delta;
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

		private unsafe static void UpdateMotorData(IntPtr data, int index, byte isReset)
		{
			var dt = (MOTORDATA*)data.ToPointer();
			if (isReset == 1)
			{
				CurrentMotorData[index].TachoSensor = 0;
			}
			else
			{
				dt->TachoCounts = CurrentMotorData[index].TachoCounts;
				dt->TachoSensor = CurrentMotorData[index].TachoSensor;
			}
		}

		internal static MOTORDATA[] CurrentMotorData = new MOTORDATA[4];
	}
}
