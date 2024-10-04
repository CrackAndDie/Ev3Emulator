using System;
using System.Runtime.InteropServices;
using Ev3LowLevelLib;

namespace Ev3Emulator.LowLevel
{
    public unsafe struct ANALOG
    {
        public fixed short InPin1[4];         //!< Analog value at input port connection 1
        public fixed short InPin6[4];         //!< Analog value at input port connection 6
        public fixed short OutPin5[4];       //!< Analog value at output port connection 5
        public short BatteryTemp;            //!< Battery temperature
        public short MotorCurrent;           //!< Current flowing to motors
        public short BatteryCurrent;         //!< Current flowing from the battery
        public short Cell123456;             //!< Voltage at battery cell 1, 2, 3,4, 5, and 6

        public fixed short OutPin5Low[4];    //!< Analog value at output port connection 5 when connection 6 is low

        public fixed sbyte Updated[4];		// set to 1 when update INPUT value

        public fixed sbyte InDcm[4];          //!< Input port device types
        public fixed sbyte InConn[4];

        public fixed sbyte OutDcm[4];        //!< Output port device types
        public fixed sbyte OutConn[4];

		// non converted (custom shite here)
		public fixed sbyte OutPortUpdateStep[4];
		public fixed sbyte InPortUpdateStep[4];
	}

	public static class InputWrapper
	{
        [DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
        private extern static void reg_w_input_updateANALOG(reg_w_input_updateANALOGAction updateANALOG);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void reg_w_input_updateANALOGAction(IntPtr data);

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_input_updateUART(reg_w_input_updateUARTAction updateUART);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void reg_w_input_updateUARTAction(IntPtr data, int port, int index, int mode);

		// TODO: custom handler post processed
		public unsafe static void Init(Action<IntPtr, int, int, int> updateUart)
		{
			_updateUart = updateUart;

            reg_w_input_updateANALOG(UpdateANALOG);
			reg_w_input_updateUART(UpdateUART);

			// init current analog
			CurrentAnalogData.OutPortUpdateStep[0] = 0;
			CurrentAnalogData.OutPortUpdateStep[1] = 0;
			CurrentAnalogData.OutPortUpdateStep[2] = 0;
			CurrentAnalogData.OutPortUpdateStep[3] = 0;

			CurrentAnalogData.Updated[0] = 1;
			CurrentAnalogData.Updated[1] = 1;
			CurrentAnalogData.Updated[2] = 1;
			CurrentAnalogData.Updated[3] = 1;
		}

		public unsafe static void SetOutPort(int port, SensorType sens)
		{
			// TODO: locks
			var sensData = SensorData.AllSensorData[sens];
			CurrentAnalogData.OutDcm[port] = (sbyte)sensData.Dcm;
			CurrentAnalogData.OutConn[port] = (sbyte)sensData.Conn;

			CurrentAnalogData.OutPortUpdateStep[port] = 1; // step to update the port
		}

		public unsafe static void SetInPort(int port, SensorType sens)
		{
			// TODO: locks
			var sensData = SensorData.AllSensorData[sens];
			CurrentAnalogData.InDcm[port] = (sbyte)sensData.Dcm;
			CurrentAnalogData.InConn[port] = (sbyte)sensData.Conn;

			CurrentAnalogData.InPortUpdateStep[port] = 1; // step to update the port

			CurrentAnalogData.Updated[port] = 1;
		}

		internal unsafe static void SetPortRawValue(int port, short value)
		{
			CurrentAnalogData.InPin6[port] = value;
		}

        private unsafe static void UpdateANALOG(IntPtr data)
        {
            var dt = (ANALOG*)data.ToPointer();

			for (int i = 0; i < 4; ++i)
			{
				// this kostyl is because port cannot be chaned directly from for example large motor to medium
				// it has to be reseted at first and only then set to a new value
				if (CurrentAnalogData.OutPortUpdateStep[i] == 1)
				{
					dt->OutConn[i] = 0;
					dt->OutDcm[i] = 0;
					CurrentAnalogData.OutPortUpdateStep[i] = 0; // reset update step
				}
				else
				{
					dt->OutConn[i] = CurrentAnalogData.OutConn[i];
					dt->OutDcm[i] = CurrentAnalogData.OutDcm[i];
				}

				if (CurrentAnalogData.InPortUpdateStep[i] == 1)
				{
					dt->InConn[i] = 0;
					dt->InDcm[i] = 0;
					CurrentAnalogData.InPortUpdateStep[i] = 0; // reset update step
				}
				else
				{
					dt->InConn[i] = CurrentAnalogData.InConn[i];
					dt->InDcm[i] = CurrentAnalogData.InDcm[i];
				}
					
				dt->Updated[i] = CurrentAnalogData.Updated[i]; // updated status
				dt->InPin6[i] = CurrentAnalogData.InPin6[i];   // raw values
				// reset updated status
				//if (CurrentAnalogData.Updated[i] == 1)
				//	CurrentAnalogData.Updated[i] = 0; // TODO: uncomment
			}

			// TODO: ...
		}

		private unsafe static void UpdateUART(IntPtr data, int port, int index, int mode)
		{
			_updateUart?.Invoke(data, port, index, mode);
		}

		internal static ANALOG CurrentAnalogData = new ANALOG();

		private static Action<IntPtr, int, int, int> _updateUart;
	}
}
