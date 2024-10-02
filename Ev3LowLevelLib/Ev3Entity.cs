using Ev3Emulator.LowLevel;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using System.Diagnostics;
using Ev3LowLevelLib.Other;

namespace Ev3LowLevelLib
{
    public class Ev3Entity
    {
        public void Init()
        {
            SystemWrapper.Init();
            FilesystemWrapper.Init();
            TimeWrapper.Init();
            InputWrapper.Init(OnUpdateUart); // TODO:
            MotorsWrapper.Init(); // TODO:
            SoundWrapper.Init(); // TODO:

            SystemWrapper.LmsExited += OnLmsVmExited;

            MotorsWrapper.SetMotorSpeedEvent += OnSetMotorSpeed;

		}

		#region Ports
        public void SetOutPort(int port, SensorType sens)
        {
            InputWrapper.SetOutPort(port, sens);
		}

		public void SetInPort(int port, SensorType sens)
		{
			InputWrapper.SetInPort(port, sens);
		}
		#endregion

		#region Motors
		// it is public because it is like when you rotate the wheel via arm
		public void SetMotorSpeed(int port, int speed)
        {
            MotorsWrapper.SetMotorSpeed(port, speed);
		}

		private void OnSetMotorSpeed(int port, int speed)
		{
			MotorSpeedChanged?.Invoke(port, speed);
		}

		public event Action<int, int> MotorSpeedChanged;
		#endregion

		#region Touch sensor
		public void SetTouchSensor(int port, bool value)
		{
			InputWrapper.SetPortRawValue(port, (short)(value ? 2000 : 1000)); // the values are from typedata.rcf
		}
		#endregion

		#region Common sensors
        private unsafe void OnUpdateUart(IntPtr data, int port, int index, int mode)
        {
			// TODO: idk what is INDEX but it should be checked on ir sensor
			// (probably DataSets is amount of data to be sent and index is a current element in the pckt)
			var dt = (float*)data.ToPointer();

			float outData = 0;
			switch (InputWrapper.CurrentAnalogData.InDcm[port])
			{
				// us
				case 30:
					outData = GetUsData(port, index, mode);
					break;
				// ir
				case 33:
					outData = GetIrData(port, index, mode);
					break;
				// gyro
				case 32:
					outData = GetGyroData(port, index, mode);
					break;
				// color
				case 29:
					outData = GetColorData(port, index, mode);
					break;
			}

			dt[index] = outData;
		}
		#endregion

		#region Us sensor
		public void InitUsSensor(int port, Func<float> updateUsSensor)
		{
			_getUsSensor[port] = updateUsSensor;
		}

		public void ResetUsSensor(int port)
		{
			_getUsSensor[port] = null;
		}

		private float GetUsData(int port, int index, int mode)
        {
			var usAct = _getUsSensor[port];
			if (usAct == null)
				return 0;

			// TODO: check: no need to do anything with index???

			switch (mode)
			{
				case 0:
					return usAct.Invoke() * 10;
				case 1:
					return (usAct.Invoke()) * (1 / 2.54f) * 10;
				case 2:
					// TODO: idk what is mode 2 for us sens
					// could not be done probably http://legoengineering.com/ev3-sensors/index.html
					return 0;
			}
			return 0;
		}

		private Func<float>[] _getUsSensor = new Func<float>[4];
		#endregion

		#region Ir sensor
		public void InitIrSensor(int port, Func<sbyte> updateIrSensor)
		{
			_getIrSensor[port] = updateIrSensor;
		}

		public void ResetIrSensor(int port)
		{
			_getIrSensor[port] = null;
		}

		private float GetIrData(int port, int index, int mode)
		{
			var irAct = _getIrSensor[port];
			if (irAct == null)
				return 0;

			// TODO: check: no need to do anything with index???

			switch (mode)
			{
				case 0:
					return irAct.Invoke();
				case 1:
				case 2:
					// TODO: idk what is mode 1/2 for ir sens
					// could not be implemented http://legoengineering.com/ev3-sensors/index.html
					// because of beacon remote ctrl
					return 0;
			}
			return 0;
		}

		private Func<sbyte>[] _getIrSensor = new Func<sbyte>[4];
		#endregion

		#region Gyro sensor
		public void InitGyroSensor(int port, Func<short> updateGyroSensor)
		{
			_getGyroSensor[port] = updateGyroSensor;
			_lastGyroValues[port] = 0;
			_lastGyroTimestamps[port] = Stopwatch.GetTimestamp();
		}

		public void ResetGyroSensor(int port)
		{
			_getGyroSensor[port] = null;
		}

		private float GetGyroData(int port, int index, int mode)
		{
			var gyroAct = _getGyroSensor[port];
			if (gyroAct == null)
				return 0;

			var ticks = Stopwatch.GetTimestamp() - _lastGyroTimestamps[port];
			_lastGyroTimestamps[port] = Stopwatch.GetTimestamp();

			// TODO: check: no need to do anything with index???

			float val = 0;

			var gyro = gyroAct.Invoke();
			var gDiff = gyro - _lastGyroValues[port];
			var coeff = 1 / (ticks / (float)Stopwatch.Frequency);
			var degPerSec = _gyroRateFilters[port].Update(gDiff * coeff);

			_lastGyroValues[port] = gyro;

			switch (mode)
			{
				case 0:
					val = gyro;
					break;
				case 1:
					val = degPerSec;
					break;
			}
			return val;
		}

		private Func<short>[] _getGyroSensor = new Func<short>[4];
		private float[] _lastGyroValues = new float[4];
		private long[] _lastGyroTimestamps = new long[4];
		private MeanFilter[] _gyroRateFilters = new MeanFilter[] { new MeanFilter(5), new MeanFilter(5), new MeanFilter(5), new MeanFilter(5) };
		#endregion

		#region Color sensor
		// color, reflect, ambient
		public void InitColorSensor(int port, Func<(byte, byte, byte)> updateColorSensor)
		{
			_getColorSensor[port] = updateColorSensor;
		}

		public void ResetColorSensor(int port)
		{
			_getColorSensor[port] = null;
		}

		private float GetColorData(int port, int index, int mode)
		{
			var colorAct = _getColorSensor[port];
			if (colorAct == null)
				return 0;

			// TODO: check: no need to do anything with index???

			float val = 0;

			var result = colorAct.Invoke();
			switch (mode)
			{
				case 0:
					val = result.Item2; // reflect
					break;
				case 1:
					val = result.Item3; // ambient
					break;
				case 2:
					val = result.Item1; // color
					break;
			}
			return val;
		}

		private Func<(byte, byte, byte)>[] _getColorSensor = new Func<(byte, byte, byte)>[4];
		#endregion

		public void InitLcd(Action<byte[]> updateLcd, Action<int> updateLed, bool desertColors = false)
        {
            LcdWrapper.Init(updateLcd, updateLed, desertColors);
        }

        public void InitButtons(Func<byte[]> getPressed)
        {
            ButtonsWrapper.Init(getPressed);
        }

		#region Lms vm
		public void StopVm()
        {
            if (IsVmRunning)
            {
				SystemWrapper.StopLms();
				// _ev3Thread.Join(); // does not work https://stackoverflow.com/questions/6427079/join-refuses-to-acknowledge-that-a-child-thread-has-terminated-after-the-isaliv
				// so i use pseudo join hehehe :)))
				//while (_ev3Thread.IsAlive)
				//{
				//	Thread.Sleep(100);
				//}
			}
		}

        public void OnCenterButtonPressed()
        {
            if (IsVmRunning)
                return;

            _pressCts = new CancellationTokenSource();
            Task.Delay(_pressTime, _pressCts.Token).ContinueWith(t => 
            {
                TryStartLms();
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public void OnCenterButtonReleased() 
        {
            _pressCts.Cancel();
        }

        private void TryStartLms()
        {
            if (IsVmRunning)
                return;

            _ev3Thread = new Thread(SystemWrapper.MainLms);
            // _ev3Thread.IsBackground = true;

			_ev3Thread.Start();
        }

        private void OnLmsVmExited()
        {
            LmsExited?.Invoke();
        }

		public event Action LmsExited;

		public bool IsVmRunning => (_ev3Thread != null && _ev3Thread.IsAlive);

		private Thread _ev3Thread;
		private const int _pressTime = 2000; // in ms
		private CancellationTokenSource _pressCts;
		#endregion
	}
}
