using Ev3CoreUnsafe;
using Ev3CoreUnsafe.Interfaces;

namespace Ev3ConsoleTest.Emulation
{
	internal class OutputHandler : IOutputHandler
	{
		public (int, int) GetMotorBusyFlags()
		{
			GH.Ev3System.Logger.LogInfo("mine GetMotorBusyFlags called");
			return (0, 0);
		}

		public void SetMotorBusyFlags(byte val)
		{
			GH.Ev3System.Logger.LogInfo("mine SetMotorBusyFlags called");
		}

		public void WritePwmData(byte[] data)
		{
			GH.Ev3System.Logger.LogInfo("mine WritePwmData called");
		}
	}
}
