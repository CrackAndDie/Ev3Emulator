namespace Ev3Core.Interfaces
{
	public interface IOutputHandler
	{
		void WritePwmData(byte[] data, int len);

		(int, int) GetMotorBusyFlags();
		void SetMotorBusyFlags(byte val);
	}
}
