using Ev3CoreUnsafe;
using Ev3CoreUnsafe.Interfaces;

namespace Ev3ConsoleTest.Emulation
{
    internal class LcdHandler : ILcdHandler
    {
        public void Exit()
        {
            GH.Ev3System.Logger.LogInfo("LCD EXIT");
        }

        public void Init()
        {
            GH.Ev3System.Logger.LogInfo("LCD INIT");
        }

        public void UpdateLcd(byte[] data)
        {
            GH.Ev3System.Logger.LogInfo("LCD UPDATED");
        }
    }
}
