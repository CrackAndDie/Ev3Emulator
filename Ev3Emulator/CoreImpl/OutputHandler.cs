using Ev3CoreUnsafe.Interfaces;

namespace Ev3Emulator.CoreImpl
{
	internal class OutputHandler : IOutputHandler
	{
		public (int, int) GetMotorBusyFlags()
		{
			// TODO:
			return (0, 0);
		}

		public void SetMotorBusyFlags(byte val)
		{
			// TODO:
		}

		public void WritePwmData(byte[] data)
		{
			// TODO:
		}
	}
}
