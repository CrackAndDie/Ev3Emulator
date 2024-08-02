using Ev3Core.Lms2012.Interfaces;

namespace Ev3Core.Interfaces
{
	public interface IInputHandler
	{
		void WriteUartData(byte[] data, int len);
		void WriteI2cData(byte[] data, int len);

		void IoctlI2c(int command, IICDAT data);
	}
}
