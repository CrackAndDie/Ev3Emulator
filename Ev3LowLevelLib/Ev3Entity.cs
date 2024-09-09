using Ev3Emulator.LowLevel;

namespace Ev3LowLevelLib
{
    public class Ev3Entity
    {
        public void Init()
        {
            SystemWrapper.Init();
            FilesystemWrapper.Init();
            TimeWrapper.Init();
            InputWrapper.Init(); // TODO:
            MotorsWrapper.Init(); // TODO:
            SoundWrapper.Init(); // TODO:

            SystemWrapper.LmsExited += OnLmsVmExited;
        }

        public void InitLcd(Action<byte[]> updateLcd, Action<int> updateLed)
        {
            LcdWrapper.Init(updateLcd, updateLed);
        }

        public void InitButtons(Func<byte[]> getPressed)
        {
            ButtonsWrapper.Init(getPressed);
        }

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
    }
}
