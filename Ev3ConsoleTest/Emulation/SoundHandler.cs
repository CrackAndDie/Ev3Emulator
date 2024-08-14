using Ev3CoreUnsafe;
using Ev3CoreUnsafe.Interfaces;

namespace Ev3ConsoleTest.Emulation
{
	internal class SoundHandler : ISoundHandler
	{
		public byte PlayChunk(byte[] data)
		{
			GH.Ev3System.Logger.LogInfo("CHUNK PLAYED");
			return (byte)data.Length;
		}
	}
}
