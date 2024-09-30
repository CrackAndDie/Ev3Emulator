using Ev3Emulator.LowLevel;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;

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
					return 0;
			}
			return 0;
		}

		public Func<float>[] _getUsSensor = new Func<float>[4];
		#endregion

		public void InitLcd(Action<byte[]> updateLcd, Action<int> updateLed)
        {
            LcdWrapper.Init(updateLcd, updateLed);
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
