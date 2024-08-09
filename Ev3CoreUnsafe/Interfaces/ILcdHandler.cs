namespace Ev3CoreUnsafe.Interfaces
{
	public interface ILcdHandler
	{
		void Init();
		void UpdateLcd(byte[] data);
		void Exit();
	}
}
