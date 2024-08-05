using Ev3Core.Enums;
using Ev3Core.Helpers;
using Ev3Core.Lms2012.Interfaces;
using static Ev3Core.Defines;

namespace Ev3Core.Coutput.Interfaces
{
    public interface IOutput
    {
        RESULT cOutputInit();

        RESULT cOutputOpen();

        RESULT cOutputClose();

        RESULT cOutputExit();

        void cOutputSetTypes(ArrayPointer<UBYTE> pTypes);
        void cOutputSetType();
        UBYTE cMotorGetBusyFlags();
        void cMotorSetBusyFlags(UBYTE Flags);
        void ResetDelayCounter(UBYTE Pattern);

        void cOutputReset();
        void cOutputStop();
        void cOutputPrgStop();
        void cOutputPower();
        void cOutputSpeed();
        void cOutputStart();
        void cOutputPolarity();

        void cOutputRead();
        void cOutputTest();
        void cOutputReady();
        void cOutputStepPower();
        void cOutputTimePower();
        void cOutputStepSpeed();
        void cOutputTimeSpeed();
        void cOutputStepSync();
        void cOutputTimeSync();
        void cOutputClrCount();
        void cOutputGetCount();
    }

    public class OUTPUT_GLOBALS
    {
        //*****************************************************************************
        // Output Global variables
        //*****************************************************************************

        public ArrayPointer<UBYTE> OutputType = new ArrayPointer<UBYTE>(CommonHelper.Array1d<UBYTE>(OUTPUTS));
        public ArrayPointer<OBJID> Owner = new ArrayPointer<OBJID>(CommonHelper.Array1d<OBJID>(OUTPUTS));

        public int PwmFile;
        public int MotorFile;

        public ArrayPointer<MOTORDATA> MotorData = new ArrayPointer<MOTORDATA>(CommonHelper.Array1d<MOTORDATA>(OUTPUTS, true));
        public int pMotor = 0;
    }
}
