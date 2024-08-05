using Ev3Core.Enums;
using Ev3Core.Lms2012.Interfaces;
using static Ev3Core.Defines;

namespace Ev3Core.Csound.Interfaces
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

    public class SOUND_GLOBALS
    {
        //*****************************************************************************
        // Sound Global variables
        //*****************************************************************************

        public int SoundDriverDescriptor;
        public FileInfo hSoundFile;

        public DATA8 SoundOwner;
        public DATA8 cSoundState;
        public SOUND Sound = new SOUND();
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
        public ArrayPointer<UBYTE> PathBuffer = new ArrayPointer<UBYTE>(new byte[MAX_FILENAME_SIZE]);
        public ArrayPointer<UBYTE> SoundData = new ArrayPointer<UBYTE>(new UBYTE[SOUND_FILE_BUFFER_SIZE + 1]); // Add up for CMD
    }
}
