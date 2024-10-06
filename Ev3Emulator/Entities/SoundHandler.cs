using Ev3LowLevelLib;
using Hypocrite.Core.Container;
using LibVLCSharp.Shared;
using System.IO;
using System.Threading;

namespace Ev3Emulator.Entities
{
	public class SoundHandler
	{
		public void Init()
		{
			Ev3Entity.InitSound(PlayTone, IsSoundPlaying, PlaySound);
			_libVlc = new LibVLC();
			_mediaPlayer = new MediaPlayer(_libVlc);
        }

		public void PlayTone(short freq, ushort duration)
		{
			// TODO:
		}

		public int IsSoundPlaying()
		{
			return _mediaPlayer.IsPlaying ? 1 : 0;
		}

		public void PlaySound(string name, int size, int rate)
		{
			string normalName = name;

            if (_mediaPlayer.IsPlaying)
            {
                _mediaPlayer.Stop(); // stop current
            }

            var media = new Media(_libVlc, normalName, FromType.FromPath);
            _mediaPlayer.Media = media;
			_mediaPlayer.Volume = 100; // TODO: get volume from brick
			_mediaPlayer.Play();
        }

		[Injection]
		public Ev3Entity Ev3Entity { get; set; }

		private LibVLC _libVlc;
		private MediaPlayer _mediaPlayer;
	}
}
