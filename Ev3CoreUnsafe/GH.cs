using Ev3CoreUnsafe.Ccom.Interfaces;
using Ev3CoreUnsafe.Cinput;
using Ev3CoreUnsafe.Cinput.Interfaces;
using Ev3CoreUnsafe.Cmemory.Interfaces;
using Ev3CoreUnsafe.Coutput;
using Ev3CoreUnsafe.Coutput.Interfaces;
using Ev3CoreUnsafe.Csound;
using Ev3CoreUnsafe.Csound.Interfaces;
using Ev3CoreUnsafe.Cui;
using Ev3CoreUnsafe.Cui.Interfaces;
using Ev3CoreUnsafe.Interfaces;
using Ev3CoreUnsafe.Lms2012;
using Ev3CoreUnsafe.Lms2012.Interfaces;

namespace Ev3CoreUnsafe
{
    public static class GH
    {
        public static IEv3System Ev3System { get; set; }

        // lms2012
        public readonly static ILms Lms = new Lms();
        public readonly static IBranch Branch = new Branch();
        public readonly static ICompare Compare = new Compare();
        public readonly static IMath Math = new Math_();
        public readonly static IMove Move = new Move();
        public readonly static Lms2012.Interfaces.ITimer Timer = new Timer_();
        public readonly static IValidate Validate = new Validate();
        public static GLOBALS VMInstance = new GLOBALS();
        public static VALIDATE_GLOBALS ValidateInstance = new VALIDATE_GLOBALS();

        // c_memory
        public readonly static IMemory Memory = new Cmemory.Memory();
        public static MEMORY_GLOBALS MemoryInstance = new MEMORY_GLOBALS();

        // c_com
        public readonly static ICom Com = new Com_();
        public readonly static IBt Bt = new Bt();
        public readonly static II2c I2c = new I2c();
        public readonly static IDaisy Daisy = new Daisy();
        public readonly static IMd5 Md5 = new Md5_();
        public readonly static IWifi Wifi = new Wifi();
        public static COM_GLOBALS ComInstance = new COM_GLOBALS();

        // c_sound
        public readonly static ISound Sound = new Sound();
        public static SOUND_GLOBALS SoundInstance = new SOUND_GLOBALS();

        // c_ui
        public readonly static IUi Ui = new Ui();
        public readonly static ITerminal Terminal = new Terminal();
        public readonly static ILcd Lcd = new Lcd();
        public static UI_GLOBALS UiInstance = new UI_GLOBALS();

        // c_output
        public readonly static IOutput Output = new Output();
        public static OUTPUT_GLOBALS OutputInstance = new OUTPUT_GLOBALS();

        // c_input
        public readonly static IInput Input = new Input();
        public static INPUT_GLOBALS InputInstance = new INPUT_GLOBALS();

        public static void Main()
        {
            Lms.Main();
        }

        // other shite
        public static int printf(string ln)
        {
            Ev3System.Logger.LogInfo(ln);
            return ln.Length;
        }
    }
}
