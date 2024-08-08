using Ev3CoreUnsafe.Enums;
using Ev3CoreUnsafe.Helpers;
using static Ev3CoreUnsafe.Defines;

namespace Ev3CoreUnsafe.Lms2012.Interfaces
{
	public unsafe interface ILms
	{
		int Main();

		void PrimParAdvance();                    // Dummy get parameter

		void* PrimParPointer();                    // Get pointer to primitives and system calls parameters

		IP GetImageStart();                     // Get pointer to start of image

		void SetDispatchStatus(DSPSTAT Status);       // Set dispatch status (result from executing byte code)

		void SetInstructions(ULONG Instructions);     // Set number of instructions before VMThread change

		PRGID CurrentProgramId();                  // Get current program id

		OBJSTAT ProgramStatus(PRGID PrgId);              // Get program status

		OBJSTAT ProgramStatusChange(PRGID PrgId);        // Get program status change

		void ProgramEnd(PRGID PrgId);

		OBJID CallingObjectId();                   // Get calling objects id

		void AdjustObjectIp(IMOFFS Value);            // Adjust IP

		IP GetObjectIp();                       // Get IP

		void SetObjectIp(IP Ip);                      // Set IP

		ULONG GetTimeUS();                         // Get uS

		ULONG GetTimeMS();                         // Get mS

		ULONG GetTime();                           // Get actual program time

		ULONG CurrentObjectIp();                   // Get current object ip

		void VmPrint(char* pString);                  // print string

		void SetTerminalEnable(DATA8 Value);          // Terminal enable/disable

		DATA8 GetTerminalEnable();                 // Get terminal enable state

		void GetResourcePath(char* pString, DATA8 MaxLength);// Get resource path

		void* VmMemoryResize(HANDLER Handle, DATA32 Elements);

		void SetVolumePercent(DATA8 Volume);

		DATA8 GetVolumePercent();

		void SetSleepMinutes(DATA8 Minutes);

		DATA8 GetSleepMinutes();

		DSPSTAT ExecuteByteCode(IP pByteCode, GP pGlobals, LP pLocals); // Execute byte code stream (C-call)

		DATA8 CheckSdcard(DATA8* pChanged, DATA32* pTotal, DATA32* pFree, DATA8 Force);

		DATA8 CheckUsbstick(DATA8* pChanged, DATA32* pTotal, DATA32* pFree, DATA8 Force);

		void SetUiUpdate();

		RESULT ValidateChar(DATA8* pChar, DATA8 Set);

		RESULT ValidateString(DATA8* pString, DATA8 Set);

		ERR LogErrorGet();

		// in lms2012.c
		void Error();
		void Nop();
		void ObjectStop();
		void ObjectStart();
		void ObjectTrig();
		void ObjectWait();
		void ObjectCall();
		void ObjectReturn();
		void ObjectEnd();
		void ProgramStart();
		void ProgramStop();
		void Sleep();
		void ProgramInfo();
		void DefLabel();
		void Do();
		void Probe();
		void BreakPoint();
		void BreakSet();
		void Random();
		void Info();
		void Strings();
		void MemoryWrite();
		void MemoryRead();
		void cBranchJr();
		void PortCnvOutput();
		void PortCnvInput();
		void NoteToFreq();
		void System();
		void Monitor();

		void TstClose();
		void Tst();
	}

	/*! \page memorylayout Memory Layout
	 *
	 *  RAM layout
	 *
	 *-   GlobalVariables                   (aligned)
	 *
	 *-   ObjectPointerList                 (aligned)
	 *
	 *-   OBJ                               (aligned)
	 *  -   Ip                              (4 bytes)
	 *  -   Status                          (2 bytes)
	 *  -   TriggerCount/CallerId           (2 bytes)
	 *  -   Local                           (0..MAX Bytes)\n
	 *
	 */

	/*! \struct OBJ
	 *          Object data is used to hold the variables used for an object (allocated at image load time)
	 */
	public unsafe struct OBJ                        // Object
	{
		public IP Ip;                           //!< Object instruction pointer
		public LP pLocal;                       //!< Local variable pointer

		public UWORD ObjStatus;                    //!< Object status

		public UNIONu u;
		public VARDATA* Local;                      //!< Poll of bytes used for local variables
	}

	public unsafe struct UNIONu
	{
		public OBJID CallerId;                   //!< Caller id used for SUBCALL to save object id to return to
		public TRIGGER TriggerCount;               //!< Trigger count used by BLOCK's trigger logic
	}

	/*! \struct BRKP
	 *          Breakpoint data hold information used for breakpoint
	 */
	public unsafe struct BRKP
	{
		public IMINDEX Addr;                         //!< Offset to breakpoint address from image start
		public OP OpCode;                       //!< Saved substituted opcode
	}

	/*! \struct PRG
	 *          Program data hold information about a program
	 */
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

		public OBJSTAT Status;                     //!< Program status
		public OBJSTAT StatusChange;               //!< Program status change
		public RESULT Result;                     //!< Program result (OK, BUSY, FAIL)

		public fixed UBYTE Brkp[8 * MAX_BREAKPOINTS];      //!< Storage for breakpoint logic // 8 - is a sizeof(BRKP)

		public fixed UBYTE Label[4 * MAX_LABELS];          //!< Storage for labels // 4 - is a sizeof(LABEL)
		public UWORD Debug;                      //!< Debug flag

		public fixed UBYTE Name[FILENAME_SIZE];
	}

	/*! \struct TYPES
	 *          Device type data
	 */
	public unsafe struct TYPES // if data type changes - remember to change "cInputTypeDataInit" !
	{
		public fixed UBYTE Name[TYPE_NAME_LENGTH + 1]; //!< Device name
		public DATA8 Type;                       //!< Device type
		public DATA8 Connection;
		public DATA8 Mode;                       //!< Device mode
		public DATA8 DataSets;
		public DATA8 Format;
		public DATA8 Figures;
		public DATA8 Decimals;
		public DATA8 Views;
		public DATAF RawMin;                     //!< Raw minimum value      (e.c. 0.0)
		public DATAF RawMax;                     //!< Raw maximum value      (e.c. 1023.0)
		public DATAF PctMin;                     //!< Percent minimum value  (e.c. -100.0)
		public DATAF PctMax;                     //!< Percent maximum value  (e.c. 100.0)
		public DATAF SiMin;                      //!< SI unit minimum value  (e.c. -100.0)
		public DATAF SiMax;                      //!< SI unit maximum value  (e.c. 100.0)
		public UWORD InvalidTime;                //!< mS from type change to valid data
		public UWORD IdValue;                    //!< Device id value        (e.c. 0 ~ UART)
		public DATA8 Pins;                       //!< Device pin setup
		public fixed SBYTE Symbol[SYMBOL_LENGTH + 1];  //!< SI unit symbol
		public UWORD Align;
	}

	public unsafe struct COLORSTRUCT
	{
		public fixed ULONG Calibration0[COLORS];
		public fixed ULONG Calibration1[COLORS];
		public fixed ULONG Calibration2[COLORS];
		public fixed UWORD CalLimits[CALPOINTS - 1];
		public UWORD Crc;
		public fixed UWORD ADRaw[COLORS];
		public fixed UWORD SensorRaw[COLORS];
	}

	/*! \page AnalogModuleMemory
	 *  <b>     Shared Memory </b>
	 *
	 *  <hr size="1"/>
	 *
	 *  It is possible to get a pointer to the raw analogue values for use in userspace
	 *  this pointer will point to a struct and the layout is following:
	 *
	 *  \verbatim
	 */

	public unsafe struct ANALOG
	{
		public fixed DATA16 InPin1[INPUTS];         //!< Analog value at input port connection 1
		public fixed DATA16 InPin6[INPUTS];         //!< Analog value at input port connection 6
		public fixed DATA16 OutPin5[OUTPUTS];       //!< Analog value at output port connection 5
		public DATA16 BatteryTemp;            //!< Battery temperature
		public DATA16 MotorCurrent;           //!< Current flowing to motors
		public DATA16 BatteryCurrent;         //!< Current flowing from the battery
		public DATA16 Cell123456;             //!< Voltage at battery cell 1, 2, 3,4, 5, and 6
		public fixed DATA16 Pin10[DEVICE_LOGBUF_SIZE];      //!< Raw value from analog device
		public fixed DATA16 Pin11[DEVICE_LOGBUF_SIZE];      //!< Raw value from analog device
		public fixed DATA16 Pin12[DEVICE_LOGBUF_SIZE];      //!< Raw value from analog device
		public fixed DATA16 Pin13[DEVICE_LOGBUF_SIZE];      //!< Raw value from analog device
		public fixed DATA16 Pin60[DEVICE_LOGBUF_SIZE];      //!< Raw value from analog device
		public fixed DATA16 Pin61[DEVICE_LOGBUF_SIZE];      //!< Raw value from analog device
		public fixed DATA16 Pin62[DEVICE_LOGBUF_SIZE];      //!< Raw value from analog device
		public fixed DATA16 Pin63[DEVICE_LOGBUF_SIZE];      //!< Raw value from analog device
		public fixed UWORD Actual[INPUTS];
		public fixed UWORD LogIn[INPUTS];
		public fixed UWORD LogOut[INPUTS];
		public fixed UBYTE NxtCol[72 * INPUTS]; // 72 - is a sizeof(COLORSTRUCT)
		public fixed DATA16 OutPin5Low[OUTPUTS];    //!< Analog value at output port connection 5 when connection 6 is low

		public fixed DATA8 Updated[INPUTS];

		public fixed DATA8 InDcm[INPUTS];          //!< Input port device types
		public fixed DATA8 InConn[INPUTS];

		public fixed DATA8 OutDcm[OUTPUTS];        //!< Output port device types
		public fixed DATA8 OutConn[OUTPUTS];
		public UWORD PreemptMilliSeconds;
	}

	public unsafe struct UART
	{
		public fixed UBYTE TypeData0[56 * MAX_DEVICE_MODES]; //!< TypeData // 56 - is a sizeof(TYPES)
		public fixed UBYTE TypeData1[56 * MAX_DEVICE_MODES]; //!< TypeData // 56 - is a sizeof(TYPES)
		public fixed UBYTE TypeData2[56 * MAX_DEVICE_MODES]; //!< TypeData // 56 - is a sizeof(TYPES)
		public fixed UBYTE TypeData3[56 * MAX_DEVICE_MODES]; //!< TypeData // 56 - is a sizeof(TYPES)

		public fixed DATA8 Raw0[UART_DATA_LENGTH];
		public fixed DATA8 Raw1[UART_DATA_LENGTH];
		public fixed DATA8 Raw2[UART_DATA_LENGTH];
		public fixed DATA8 Raw3[UART_DATA_LENGTH];

		public fixed DATA8 Status[INPUTS];                     //!< Status
		public fixed DATA8 Output0[UART_DATA_LENGTH];   //!< Bytes to UART device
		public fixed DATA8 Output1[UART_DATA_LENGTH];   //!< Bytes to UART device
		public fixed DATA8 Output2[UART_DATA_LENGTH];   //!< Bytes to UART device
		public fixed DATA8 Output3[UART_DATA_LENGTH];   //!< Bytes to UART device
		public fixed DATA8 OutputLength[INPUTS];
	}

	public unsafe struct DEVCON
	{
		public fixed DATA8 Connection[INPUTS];
		public fixed DATA8 Type[INPUTS];
		public fixed DATA8 Mode[INPUTS];
	}

	public unsafe struct UARTCTL
	{
		public TYPES TypeData;
		public DATA8 Port;
		public DATA8 Mode;
	}

	public unsafe struct IIC
	{
		public fixed UBYTE TypeData0[56 * MAX_DEVICE_MODES]; //!< TypeData // 56 - is a sizeof(TYPES)
		public fixed UBYTE TypeData1[56 * MAX_DEVICE_MODES]; //!< TypeData // 56 - is a sizeof(TYPES)
		public fixed UBYTE TypeData2[56 * MAX_DEVICE_MODES]; //!< TypeData // 56 - is a sizeof(TYPES)
		public fixed UBYTE TypeData3[56 * MAX_DEVICE_MODES]; //!< TypeData // 56 - is a sizeof(TYPES)

		public fixed DATA8 Raw0[IIC_DATA_LENGTH];      //!< Raw value from IIC device
		public fixed DATA8 Raw1[IIC_DATA_LENGTH];      //!< Raw value from IIC device
		public fixed DATA8 Raw2[IIC_DATA_LENGTH];      //!< Raw value from IIC device
		public fixed DATA8 Raw3[IIC_DATA_LENGTH];      //!< Raw value from IIC device

		public fixed DATA8 Status[INPUTS];                     //!< Status
		public fixed DATA8 Changed[INPUTS];
		public fixed DATA8 Output0[IIC_DATA_LENGTH];    //!< Bytes to IIC device
		public fixed DATA8 Output1[IIC_DATA_LENGTH];    //!< Bytes to IIC device
		public fixed DATA8 Output2[IIC_DATA_LENGTH];    //!< Bytes to IIC device
		public fixed DATA8 Output3[IIC_DATA_LENGTH];    //!< Bytes to IIC device
		public fixed DATA8 OutputLength[INPUTS];
	}

	public unsafe struct IICCTL
	{
		public TYPES TypeData;
		public DATA8 Port;
		public DATA8 Mode;
	}

	public unsafe struct IICDAT
	{
		public RESULT Result;
		public DATA8 Port;
		public DATA8 Repeat;
		public DATA16 Time;
		public DATA8 WrLng;
		public fixed DATA8 WrData[IIC_DATA_LENGTH];
		public DATA8 RdLng;
		public fixed DATA8 RdData[IIC_DATA_LENGTH];
	}

	public unsafe struct IICSTR
	{
		public DATA8 Port;
		public DATA16 Time;
		public DATA8 Type;
		public DATA8 Mode;
		public fixed DATA8 Manufacturer[IIC_NAME_LENGTH + 1];
		public fixed DATA8 SensorType[IIC_NAME_LENGTH + 1];
		public DATA8 SetupLng;
		public ULONG SetupString;
		public DATA8 PollLng;
		public ULONG PollString;
		public DATA8 ReadLng;
	}

	public unsafe struct TSTPIN
	{
		public DATA8 Port;
		public DATA8 Length;
		public fixed DATA8 String[TST_PIN_LENGTH + 1];
	}

	public unsafe struct TSTUART
	{
		public DATA32 Bitrate;
		public DATA8 Port;
		public DATA8 Length;
		public fixed DATA8 String[TST_UART_LENGTH];
	}

	public unsafe struct UI
	{
		public fixed DATA8 Pressed[BUTTONS];                   //!< Pressed status
    }

	public unsafe struct LCD
	{
		public fixed UBYTE Lcd[LCD_BUFFER_SIZE];
	}

	public unsafe struct SOUND
	{
		public DATA8 Status;                       //!< Status
	}

	public unsafe struct USB_SPEED
	{
		public DATA8 Speed;
	}

	public unsafe struct NONVOL
	{
		public DATA8 VolumePercent;                //!< System default volume [0..100%]
		public DATA8 SleepMinutes;                 //!< System sleep          [0..120min] (0 = ~)
	}

	public unsafe struct MOTORDATA
	{
		public SLONG TachoCounts;
		public SBYTE Speed;
		public SLONG TachoSensor;
	}

	public unsafe struct STEPPOWER
	{
		public DATA8 Cmd;
		public DATA8 Nos;
		public DATA8 Power;
		public DATA32 Step1;
		public DATA32 Step2;
		public DATA32 Step3;
		public DATA8 Brake;
	}

	public unsafe struct TIMEPOWER
	{
		public DATA8 Cmd;
		public DATA8 Nos;
		public DATA8 Power;
		public DATA32 Time1;
		public DATA32 Time2;
		public DATA32 Time3;
		public DATA8 Brake;
	}

	public unsafe struct STEPSPEED
	{
		public DATA8 Cmd;
		public DATA8 Nos;
		public DATA8 Speed;
		public DATA32 Step1;
		public DATA32 Step2;
		public DATA32 Step3;
		public DATA8 Brake;
	}

	public unsafe struct TIMESPEED
	{
		public DATA8 Cmd;
		public DATA8 Nos;
		public DATA8 Speed;
		public DATA32 Time1;
		public DATA32 Time2;
		public DATA32 Time3;
		public DATA8 Brake;
	}

	public unsafe struct STEPSYNC
	{
		public DATA8 Cmd;
		public DATA8 Nos;
		public DATA8 Speed;
		public DATA16 Turn;
		public DATA32 Step;
		public DATA8 Brake;
	}

	public unsafe struct TIMESYNC
	{
		public DATA8 Cmd;
		public DATA8 Nos;
		public DATA8 Speed;
		public DATA16 Turn;
		public DATA32 Time;
		public DATA8 Brake;
	}

	public unsafe struct GLOBALS
	{
		public NONVOL NonVol;
		public DATA8* FirstProgram;

		public DATA8* PrintBuffer;
		public DATA8 TerminalEnabled;

		public PRGID FavouritePrg;
		public PRGID ProgramId;                    //!< Program id running
		public PRG* Program;        //!< Program[0] is the UI byte codes running

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

		public ULONG PerformTimer;
		public DATAF PerformTime;

		public DSPSTAT DispatchStatus;               //!< Dispatch status
		public ULONG Priority;                     //!< Object priority

		public ULONG Value;
		public HANDLER Handle;

		public ERR* Errors;
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

		public DATA8 Status;

		public GLOBALS()
		{
			NonVol = *CommonHelper.PointerStruct<NONVOL>();
			LcdBuffer = *CommonHelper.PointerStruct<LCD>();
			Analog = *CommonHelper.PointerStruct<ANALOG>();

			FirstProgram = CommonHelper.Pointer1d<DATA8>(MAX_FILENAME_SIZE);
			PrintBuffer = CommonHelper.Pointer1d<DATA8>(PRINTBUFFERSIZE + 1);
			Program = CommonHelper.Pointer1d<PRG>(MAX_PROGRAMS, true);
			Errors = CommonHelper.Pointer1d<ERR>(ERROR_BUFFER_SIZE);
		}
	}
}
