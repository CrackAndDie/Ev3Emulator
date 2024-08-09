using Ev3CoreUnsafe.Enums;
using Ev3CoreUnsafe.Helpers;
using Ev3CoreUnsafe.Lms2012.Interfaces;
using static Ev3CoreUnsafe.Defines;

namespace Ev3CoreUnsafe.Coutput.Interfaces
{
	public unsafe interface IOutput
	{
		RESULT cOutputInit();

		RESULT cOutputOpen();

		RESULT cOutputClose();

		RESULT cOutputExit();

		void cOutputSetTypes(DATA8* pTypes);
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

	public unsafe struct OUTPUT_GLOBALS
	{
		//*****************************************************************************
		// Output Global variables
		//*****************************************************************************

		public DATA8* OutputType;
		public OBJID* Owner;

		public int PwmFile;
		public int MotorFile;

		public MOTORDATA* MotorData;
		public MOTORDATA* pMotor;

		public OUTPUT_GLOBALS()
		{
			Init();
		}

		public void Init()
		{
			OutputType = CommonHelper.Pointer1d<DATA8>(OUTPUTS);
			Owner = CommonHelper.Pointer1d<OBJID>(OUTPUTS);
			MotorData = CommonHelper.Pointer1d<MOTORDATA>(OUTPUTS, true);
		}
	}
}
