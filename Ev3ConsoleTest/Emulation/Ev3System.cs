using Ev3CoreUnsafe.Interfaces;

namespace Ev3ConsoleTest.Emulation
{
	internal class Ev3System : IEv3System
	{
		public ILogger Logger { get; } = new Logger();

		public ILedHandler LedHandler { get; } = new LedHandler();

		public ILcdHandler LcdHandler { get; } = new LcdHandler();

        public ISoundHandler SoundHandler { get; } = new SoundHandler();

		public IOutputHandler OutputHandler { get; } = new OutputHandler();

		public IInputHandler InputHandler => throw new NotImplementedException();
	}
}
