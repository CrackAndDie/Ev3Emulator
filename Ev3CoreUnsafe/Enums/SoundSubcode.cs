namespace Ev3CoreUnsafe.Enums
{
	//! \page soundsubcode Specific command parameter
	//!
	//!
	//! \verbatim
	//!

	public enum SOUND_SUBCODE
	{
		BREAK = 0,
		TONE = 1,
		PLAY = 2,
		REPEAT = 3,
		SERVICE = 4,

		SOUND_SUBCODES
	}
}

namespace Ev3CoreUnsafe
{
	public partial class Defines
	{
		public const int BREAK = 0;
		public const int TONE = 1;
		public const int PLAY = 2;
		public const int REPEAT = 3;
		public const int SERVICE = 4;

		public const int SOUND_SUBCODES = 5;
	}
}
