namespace Ev3CoreUnsafe.Interfaces
{
	public interface IEv3System
	{
		ILogger Logger { get; }

		ILedHandler LedHandler { get; }
		ILcdHandler LcdHandler { get; }
		ISoundHandler SoundHandler { get; }
		IOutputHandler OutputHandler { get; }
		IInputHandler InputHandler { get; }
	}
}
