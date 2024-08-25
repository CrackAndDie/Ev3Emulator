using Ev3Emulator.LowLevel;
using System.Runtime.InteropServices;

namespace Ev3ConsoleCmakeTest.Lms
{
	public static class LmsWrapper
	{
		public static void Lms()
		{
            Console.ReadLine();

            // inits
            FilesystemWrapper.Init();
            TimeWrapper.Init();
            InputWrapper.Init(); // TODO:
            MotorsWrapper.Init(); // TODO:
            SoundWrapper.Init(); // TODO:
            LcdWrapper.Init(UpdateLcd, UpdateLed);
            ButtonsWrapper.Init(UpdateButtons);

            Console.WriteLine("Starting ev3 thread");
            //_ev3Thread = new Thread(SystemWrapper.MainLms);
            //_ev3Thread.Start();
            SystemWrapper.MainLms();
            Console.WriteLine("Done ev3 thread");
        }

        private static void UpdateLcd(IntPtr buf, int size)
        {
            
        }

        private static void UpdateLed(int state)
        {
            // TODO: 
        }

        private static IntPtr UpdateButtons()
        {
            // TODO:
            var bytes = new byte[] { 0, 0, 0, 0, 0, 0 };
            IntPtr p = Marshal.AllocHGlobal(bytes.Length);
            Marshal.Copy(bytes, 0, p, bytes.Length);
            return p;
        }

        private static Thread _ev3Thread;
    }
}
