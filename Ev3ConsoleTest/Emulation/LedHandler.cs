using Ev3CoreUnsafe;
using Ev3CoreUnsafe.Interfaces;

namespace Ev3ConsoleTest.Emulation
{
	internal class LedHandler : ILedHandler
	{
		public void SetLed(sbyte[] buff)
		{
			GH.Ev3System.Logger.LogInfo("LED UPDATED");
		}
	}
}
