using Ev3CoreUnsafe.Interfaces;

namespace Ev3Emulator.CoreImpl
{
	internal class SoundHandler : ISoundHandler
	{
		public byte PlayChunk(byte[] data)
		{
			// TODO:
			return (byte)data.Length;
		}
	}
}
