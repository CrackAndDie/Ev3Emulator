using EV3DecompilerLib.Decompile;
using Ev3EmulatorCore.Helpers;
using Ev3EmulatorCore.Lms.Cui;
using static EV3DecompilerLib.Decompile.lms2012;
using static Ev3EmulatorCore.Lms.Cui.DlcdClass;
using static Ev3EmulatorCore.Lms.Lms2012.LmsInstance;

namespace Ev3EmulatorCore.Lms.Lms2012
{
	public partial class LmsInstance
	{
		public struct NONVOL
		{
			public DATA8 VolumePercent;                //!< System default volume [0..100%]
			public DATA8 SleepMinutes;                 //!< System sleep          [0..120min] (0 = ~)
		}

		public struct LABEL
		{
			public IMINDEX Addr;                         //!< Offset to breakpoint address from image start
		}

		/*! \struct OBJ
		 *          Object data is used to hold the variables used for an object (allocated at image load time)
		 */
		public unsafe struct OBJ                      // Object
		{
			public IP Ip;                           //!< Object instruction pointer
			public LP pLocal;                       //!< Local variable pointer

			public UWORD ObjStatus;                    //!< Object status
			public (OBJID CallerId, TRIGGER TriggerCount) u;
			public VARDATA[] Local;                      //!< Poll of bytes used for local variables
		}


		/*! \struct BRKP
		 *          Breakpoint data hold information used for breakpoint
		 */
		public unsafe struct BRKP
		{
			public IMINDEX Addr;                         //!< Offset to breakpoint address from image start
			public Op OpCode;                       //!< Saved substituted opcode
		}

		public unsafe struct PRG
		{
			public ULONG InstrCnt;                   //!< Instruction counter used for performance analyses
			public ULONG InstrTime;                  //!< Instruction time used for performance analyses

			public ULONG StartTime;                  //!< Program start time [mS]
			public ULONG RunTime;                    //!< Program run time [uS]

			public IP pImage;                     //!< Pointer to start of image
			public GP pData;                      //!< Pointer to start of data
			public GP pGlobal;                    //!< Pointer to start of global bytes
			public OBJHEAD* pObjHead;                   //!< Pointer to start of object headers
			public OBJ** pObjList;                   //!< Pointer to object pointer list
			public IP ObjectIp;                   //!< Working object Ip
			public LP ObjectLocal;                //!< Working object locals

			public OBJID Objects;                    //!< No of objects in image
			public OBJID ObjectId;                   //!< Active object id

			public ObjectStatus Status;                     //!< Program status
			public ObjectStatus StatusChange;               //!< Program status change
			public Result Result;                     //!< Program result (OK, BUSY, FAIL)

			public BRKP[] Brkp = new BRKP[MAX_BREAKPOINTS];      //!< Storage for breakpoint logic

			public LABEL[] Label = new LABEL[MAX_LABELS];          //!< Storage for labels
			public UWORD Debug;                      //!< Debug flag

			public fixed DATA8 Name[FILENAME_SIZE];

			public PRG()
			{
			}
		}

		public unsafe struct COLORSTRUCT
		{
			public ULONG[][] Calibration = CommonHelper.GenerateTwoDimArray<ULONG>(CALPOINTS, COLORS);
			public fixed UWORD CalLimits[CALPOINTS - 1];
			public UWORD Crc;
			public fixed UWORD ADRaw[COLORS];
			public fixed UWORD SensorRaw[COLORS];

			public COLORSTRUCT()
			{
			}
		}

		public unsafe struct ANALOG
		{
			public fixed DATA16 InPin1[INPUTS];         //!< Analog value at input port connection 1
			public fixed DATA16 InPin6[INPUTS];         //!< Analog value at input port connection 6
			public fixed DATA16 OutPin5[OUTPUTS];       //!< Analog value at output port connection 5
			public DATA16 BatteryTemp;            //!< Battery temperature
			public DATA16 MotorCurrent;           //!< Current flowing to motors
			public DATA16 BatteryCurrent;         //!< Current flowing from the battery
			public DATA16 Cell123456;             //!< Voltage at battery cell 1, 2, 3,4, 5, and 6

			public DATA16[][] Pin1 = CommonHelper.GenerateTwoDimArray<DATA16>(INPUTS, DEVICE_LOGBUF_SIZE);      //!< Raw value from analog device
			public DATA16[][] Pin6 = CommonHelper.GenerateTwoDimArray<DATA16>(INPUTS, DEVICE_LOGBUF_SIZE);      //!< Raw value from analog device
			public fixed UWORD Actual[INPUTS];
			public fixed UWORD LogIn[INPUTS];
			public fixed UWORD LogOut[INPUTS];

			public COLORSTRUCT[] NxtCol = new COLORSTRUCT[INPUTS];

			public fixed DATA16 OutPin5Low[OUTPUTS];    //!< Analog value at output port connection 5 when connection 6 is low

			public fixed DATA8 Updated[INPUTS];

			public fixed DATA8 InDcm[INPUTS];          //!< Input port device types
			public fixed DATA8 InConn[INPUTS];

			public fixed DATA8 OutDcm[OUTPUTS];        //!< Output port device types
			public fixed DATA8 OutConn[OUTPUTS];

			public ANALOG()
			{
			}
		}

		public unsafe struct LCD
		{
			public fixed UBYTE Lcd[LCD_BUFFER_SIZE];
		}

		public unsafe struct GLOBALS
		{
			public NONVOL NonVol;
			public fixed DATA8 FirstProgram[MAX_FILENAME_SIZE];

			public fixed DATA8 PrintBuffer[PRINTBUFFERSIZE + 1];
			public DATA8 TerminalEnabled;

			public PRGID FavouritePrg;
			public PRGID ProgramId;                    //!< Program id running
			public PRG[] Program = new PRG[MAX_PROGRAMS];        //!< Program[0] is the UI byte codes running

			public ULONG InstrCnt;                     //!< Instruction counter (performance test)
			public IP pImage;                       //!< Pointer to start of image
			public GP pGlobal;                      //!< Pointer to start of global bytes
			public OBJHEAD* pObjHead;                     //!< Pointer to start of object headers
			public OBJ** pObjList;                     //!< Pointer to object pointer list

			public IP ObjectIp;                     //!< Working object Ip
			public LP ObjectLocal;                  //!< Working object locals
			public OBJID Objects;                      //!< No of objects in image
			public OBJID ObjectId;                     //!< Active object id

			public IP ObjIpSave;
			public GP ObjGlobalSave;
			public LP ObjLocalSave;
			public DSPSTAT DispatchStatusSave;
			public ULONG PrioritySave;

			public long TimerDataSec;
			public long TimerDatanSec;

			public UWORD Debug;

			public UWORD Test;

			public UWORD RefCount;

			public ULONG TimeuS;

			public ULONG OldTime1;
			public ULONG OldTime2;
			public ULONG NewTime;

			public DSPSTAT DispatchStatus;               //!< Dispatch status
			public ULONG Priority;                     //!< Object priority

			public ULONG Value;
			public HANDLER Handle;

			public Error[] Errors = new Error[ERROR_BUFFER_SIZE];
			public UBYTE ErrorIn;
			public UBYTE ErrorOut;

			public DATA32 MemorySize;
			public DATA32 MemoryFree;
			public ULONG MemoryTimer;

			public DATA32 SdcardSize;
			public DATA32 SdcardFree;
			public ULONG SdcardTimer;
			public DATA8 SdcardOk;

			public DATA32 UsbstickSize;
			public DATA32 UsbstickFree;
			public ULONG UsbstickTimer;
			public DATA8 UsbstickOk;

			public LCD LcdBuffer;                    //!< Copy of last LCD update
			public DATA8 LcdUpdated;                   //!< LCD updated

			public ANALOG Analog;
			public ANALOG* pAnalog;
			public int AdcFile;

			public GLOBALS()
			{
			}
		}
	}
}
