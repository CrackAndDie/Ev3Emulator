namespace Ev3Core.Interfaces
{
	public interface IInputHandler
	{
		void WriteUartData(byte[] data, int len);
		void WriteI2cData(byte[] data, int len);
	}
}
