using Ev3CoreUnsafe.Lms2012.Interfaces;

namespace Ev3CoreUnsafe.Interfaces
{
	public interface IInputHandler
	{
		void WriteUartData(byte[] data, int len);
		void WriteI2cData(byte[] data, int len);

		void IoctlI2c(int command, IICDAT data);
		void IoctlI2c(int command, IICSTR data);
		void IoctlI2c(int command, DEVCON data);

        void IoctlUart(int command, UARTCTL data);
        void IoctlUart(int command, DEVCON data);
    }
}
