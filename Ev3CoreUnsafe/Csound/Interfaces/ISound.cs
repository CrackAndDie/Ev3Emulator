using Ev3CoreUnsafe.Enums;
using Ev3CoreUnsafe.Helpers;
using Ev3CoreUnsafe.Lms2012.Interfaces;
using static Ev3CoreUnsafe.Defines;

namespace Ev3CoreUnsafe.Csound.Interfaces
{
	public interface ISound
	{
		RESULT cSoundInit();

		RESULT cSoundOpen();

		RESULT cSoundUpdate();

		RESULT cSoundClose();

		RESULT cSoundExit();

		void cSoundEntry();

		void cSoundReady();

		void cSoundTest();
	}

	public unsafe struct SOUND_GLOBALS
	{
		//*****************************************************************************
		// Sound Global variables
		//*****************************************************************************

		public int SoundDriverDescriptor;
		public int hSoundFile;

		public DATA8 SoundOwner;
		public DATA8 cSoundState;
		public SOUND Sound;
		public SOUND* pSound;
		public UWORD BytesLeft;
		public UWORD SoundFileFormat;
		public UWORD SoundDataLength;
		public UWORD SoundSampleRate;
		public UWORD SoundPlayMode;
		public UWORD SoundFileLength;
		public SWORD ValPrev;
		public SWORD Index;
		public SWORD Step;
		public UBYTE BytesToWrite;
		public DATA8* PathBuffer;
		// struct stat FileStatus;
		public UBYTE* SoundData; // Add up for CMD

		public SOUND_GLOBALS()
		{
			Init();
		}

		public void Init()
		{
			// structs
			Sound = *CommonHelper.PointerStruct<SOUND>();

			PathBuffer = CommonHelper.Pointer1d<DATA8>(MAX_FILENAME_SIZE);
			SoundData = CommonHelper.Pointer1d<UBYTE>(SOUND_FILE_BUFFER_SIZE + 1);
		}
	}
}
