using Ev3CoreUnsafe;
using Ev3CoreUnsafe.Interfaces;

namespace Ev3ConsoleTest.Emulation
{
	internal class SoundHandler : ISoundHandler
	{
        public event Action DonePlaying;

        public byte PlayChunk(byte[] data)
		{
			GH.Ev3System.Logger.LogInfo("CHUNK PLAYED");

			DonePlaying?.Invoke();

            return (byte)data.Length;
		}
	}
}
