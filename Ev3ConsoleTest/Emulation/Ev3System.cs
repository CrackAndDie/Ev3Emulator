using Ev3CoreUnsafe.Interfaces;

namespace Ev3ConsoleTest.Emulation
{
	internal class Ev3System : IEv3System
	{
		public ILogger Logger { get; } = new Logger();

		public ILedHandler LedHandler => throw new NotImplementedException();

		public ILcdHandler LcdHandler => throw new NotImplementedException();

		public ISoundHandler SoundHandler => throw new NotImplementedException();

		public IOutputHandler OutputHandler => throw new NotImplementedException();

		public IInputHandler InputHandler => throw new NotImplementedException();
	}
}
