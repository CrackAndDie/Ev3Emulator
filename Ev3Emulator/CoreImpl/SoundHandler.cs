using Ev3CoreUnsafe.Interfaces;
using System;

namespace Ev3Emulator.CoreImpl
{
	internal class SoundHandler : ISoundHandler
	{
        public event Action DonePlaying;

        public byte PlayChunk(byte[] data)
		{
            // TODO:
            DonePlaying?.Invoke();

            return (byte)data.Length;
		}
	}
}
