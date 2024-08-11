namespace Ev3CoreUnsafe.Interfaces
{
	public interface IOutputHandler
	{
		void WritePwmData(byte[] data);

		(int, int) GetMotorBusyFlags();
		void SetMotorBusyFlags(byte val);
	}
}
