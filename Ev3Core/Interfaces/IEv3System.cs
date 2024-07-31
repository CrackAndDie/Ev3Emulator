namespace Ev3Core.Interfaces
{
	public interface IEv3System
	{
		ILogger Logger { get; }

		ILedHandler LedHandler { get; }
		ILcdHandler LcdHandler { get; }
		ISoundHandler SoundHandler { get; }
	}
}
