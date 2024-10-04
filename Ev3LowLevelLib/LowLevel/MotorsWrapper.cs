using Ev3LowLevelLib;
using System;
using System.Diagnostics;
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

	public unsafe struct MOTORDATAFLOATED
	{
		public float TachoCounts; // probably unreseted
		public sbyte Speed;
		public float TachoSensor; // probably reseted after each task 
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

			_calcTachoTaskCts = new CancellationTokenSource();
			_calcTachoTask = Task.Run(async () =>
			{
				_lastTachoTimestamp = Stopwatch.GetTimestamp();
				while (_calcTachoTaskCts != null && !_calcTachoTaskCts.IsCancellationRequested)
				{
					var ticks = Stopwatch.GetTimestamp() - _lastTachoTimestamp;
					_lastTachoTimestamp = Stopwatch.GetTimestamp();
					var coeff = 0.1f / (ticks / (float)Stopwatch.Frequency);
					for (int i = 0; i < 4; ++i)
					{
						int speed;
						lock (_currentMotorDataLock)
							speed = CurrentMotorData[i].Speed;
						SetMotorTachoDelta(i, (speed / 10f) * coeff);
					}
					await Task.Delay(100);
				}
			}, _calcTachoTaskCts.Token);
		}

		// it is public because it is like when you rotate the wheel via arm
		public static void SetMotorSpeed(int port, int speed)
		{
			lock (_currentMotorDataLock)
				CurrentMotorData[port].Speed = (sbyte)speed;
			SetMotorSpeedEvent?.Invoke(port, speed);
		}

		public static void StopWrapper()
		{
			_calcTachoTaskCts?.Cancel();
		}

		private static void SetMotorTachoDelta(int port, float delta)
		{
			lock (_currentMotorDataLock)
			{
				CurrentMotorData[port].TachoCounts += delta;
				CurrentMotorData[port].TachoSensor += delta;
			}
			GetMotorTachoEvent?.Invoke(port, (int)CurrentMotorData[port].TachoCounts);
		}

		private static void GetBusyFlags(ref int f1, ref int f2)
		{
			// TODO: 
		}

		private static void SetBusyFlags(int f1)
		{
			// TODO: 
		}

		private unsafe static void SetData(IntPtr data, int len)
		{
			var dt = (byte*)data.ToPointer();
			if (dt == null) // if nullptr
				return;

			switch ((Op)dt[0])
			{
				case Op.opOUTPUT_POWER:
					var ports = NosToMotorPorts(dt[1]);
					foreach (var port in ports)
					{
						SetMotorSpeed(port, (sbyte)dt[2]);
					}
					break;
			}
			// TODO: 
			// SetMotorSpeed
		}

		private unsafe static void UpdateMotorData(IntPtr data, int index, byte isSet)
		{
			var dt = (MOTORDATA*)data.ToPointer();
			lock (_currentMotorDataLock)
			{
				if (isSet == 1)
				{
					CurrentMotorData[index].TachoSensor = dt->TachoSensor;
					CurrentMotorData[index].Speed = dt->Speed;
					SetMotorSpeedEvent?.Invoke(index, CurrentMotorData[index].Speed);
				}
				else
				{
					dt->Speed = (sbyte)CurrentMotorData[index].Speed;
					dt->TachoCounts = (int)CurrentMotorData[index].TachoCounts;
					dt->TachoSensor = (int)CurrentMotorData[index].TachoSensor;
				}
			}
		}

		private static List<int> NosToMotorPorts(int nos)
		{
			List<int> ports = new List<int>();
			if ((nos & 1) != 0)
				ports.Add(0);
			if ((nos & 2) != 0)
				ports.Add(1);
			if ((nos & 4) != 0)
				ports.Add(2);
			if ((nos & 8) != 0)
				ports.Add(3);
			return ports;
		}

		public static event Action<int, int> SetMotorSpeedEvent;
		public static event Action<int, int> GetMotorTachoEvent;

		private static object _currentMotorDataLock = new object();
		private static MOTORDATAFLOATED[] CurrentMotorData = new MOTORDATAFLOATED[4];

		private static Task _calcTachoTask = null;
		private static CancellationTokenSource _calcTachoTaskCts = null;
		private static long _lastTachoTimestamp = 0;
	}
}
