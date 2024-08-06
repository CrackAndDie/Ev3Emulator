namespace Ev3CoreUnsafe.Enums
{
	public enum SOUND_STATES
	{
		SOUND_STOPPED,
		SOUND_SETUP_FILE,
		SOUND_FILE_PLAYING,
		SOUND_FILE_LOOPING,
		SOUND_TONE_PLAYING,
		SOUND_TONE_LOOPING
	}
}

namespace Ev3CoreUnsafe
{
	public partial class Defines
	{
		public const int SOUND_STOPPED = 0;
		public const int SOUND_SETUP_FILE = 1;
		public const int SOUND_FILE_PLAYING = 2;
		public const int SOUND_FILE_LOOPING = 3;
		public const int SOUND_TONE_PLAYING = 4;
		public const int SOUND_TONE_LOOPING = 5;
	}
}

