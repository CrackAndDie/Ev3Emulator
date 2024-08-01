using Ev3Core.Ccom.Interfaces;
using Ev3Core.Cmemory.Interfaces;
using Ev3Core.Coutput;
using Ev3Core.Coutput.Interfaces;
using Ev3Core.Csound;
using Ev3Core.Csound.Interfaces;
using Ev3Core.Cui;
using Ev3Core.Cui.Interfaces;
using Ev3Core.Interfaces;
using Ev3Core.Lms2012;
using Ev3Core.Lms2012.Interfaces;

namespace Ev3Core
{
    public static class GH // Global Holder
    {
        public static IEv3System Ev3System { get; set; }

        // lms2012
        public readonly static ILms Lms = new Lms();
        public readonly static GLOBALS VMInstance = new GLOBALS();

        // c_memory
        public readonly static IMemory Memory = new Cmemory.Memory();
        public readonly static MEMORY_GLOBALS MemoryInstance = new MEMORY_GLOBALS();

        // c_com
        public readonly static ICom Com = null;

        // c_sound
        public readonly static ISound Sound = new Sound();
        public readonly static SOUND_GLOBALS SoundInstance = new SOUND_GLOBALS();

        // c_ui
        public readonly static ITerminal Terminal = new Terminal();
        public readonly static ILcd Lcd = new Lcd();
		public readonly static UI_GLOBALS UiInstance = new UI_GLOBALS();

        // c_output
        public readonly static IOutput Output = new Output();
        public readonly static OUTPUT_GLOBALS OutputInstance = new OUTPUT_GLOBALS();
	}
}
