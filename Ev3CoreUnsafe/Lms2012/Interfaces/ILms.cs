using Ev3CoreUnsafe.Enums;
using Ev3CoreUnsafe.Helpers;
using static Ev3CoreUnsafe.Defines;

namespace Ev3CoreUnsafe.Lms2012.Interfaces
{
	public interface ILms
	{
		unsafe void PrimParAdvance();                    // Dummy get parameter

		unsafe void* PrimParPointer();                    // Get pointer to primitives and system calls parameters

		unsafe IP GetImageStart();                     // Get pointer to start of image

		unsafe void SetDispatchStatus(DSPSTAT Status);       // Set dispatch status (result from executing byte code)

		unsafe void SetInstructions(ULONG Instructions);     // Set number of instructions before VMThread change

		unsafe PRGID CurrentProgramId();                  // Get current program id

		unsafe OBJSTAT ProgramStatus(PRGID PrgId);              // Get program status

		unsafe OBJSTAT ProgramStatusChange(PRGID PrgId);        // Get program status change

		unsafe void ProgramEnd(PRGID PrgId);

		unsafe OBJID CallingObjectId();                   // Get calling objects id

		unsafe void AdjustObjectIp(IMOFFS Value);            // Adjust IP

		unsafe IP GetObjectIp();                       // Get IP

		unsafe void SetObjectIp(IP Ip);                      // Set IP

		unsafe ULONG GetTimeUS();                         // Get uS

		unsafe ULONG GetTimeMS();                         // Get mS

		unsafe ULONG GetTime();                           // Get actual program time

		unsafe ULONG CurrentObjectIp();                   // Get current object ip

		unsafe void VmPrint(char* pString);                  // print string

		unsafe void SetTerminalEnable(DATA8 Value);          // Terminal enable/disable

		unsafe DATA8 GetTerminalEnable();                 // Get terminal enable state

		unsafe void GetResourcePath(char* pString, DATA8 MaxLength);// Get resource path

		unsafe void* VmMemoryResize(HANDLER Handle, DATA32 Elements);

		unsafe void SetVolumePercent(DATA8 Volume);

		unsafe DATA8 GetVolumePercent();

		unsafe void SetSleepMinutes(DATA8 Minutes);

		unsafe DATA8 GetSleepMinutes();

		unsafe DSPSTAT ExecuteByteCode(IP pByteCode, GP pGlobals, LP pLocals); // Execute byte code stream (C-call)

		unsafe DATA8 CheckSdcard(DATA8* pChanged, DATA32* pTotal, DATA32* pFree, DATA8 Force);

		unsafe DATA8 CheckUsbstick(DATA8* pChanged, DATA32* pTotal, DATA32* pFree, DATA8 Force);

		unsafe void SetUiUpdate();

		unsafe RESULT ValidateChar(DATA8* pChar, DATA8 Set);

		unsafe RESULT ValidateString(DATA8* pString, DATA8 Set);

		unsafe ERR LogErrorGet();

		// in lms2012.c
		unsafe void Error();
		unsafe void Nop();
		unsafe void ObjectStop();
		unsafe void ObjectStart();
		unsafe void ObjectTrig();
		unsafe void ObjectWait();
		unsafe void ObjectCall();
		unsafe void ObjectReturn();
		unsafe void ObjectEnd();
		unsafe void ProgramStart();
		unsafe void ProgramStop();
		unsafe void Sleep();
		unsafe void ProgramInfo();
		unsafe void DefLabel();
		unsafe void Do();
		unsafe void Probe();
		unsafe void BreakPoint();
		unsafe void BreakSet();
		unsafe void Random();
		unsafe void Info();
		unsafe void Strings();
		unsafe void MemoryWrite();
		unsafe void MemoryRead();
		unsafe void cBranchJr();
		unsafe void PortCnvOutput();
		unsafe void PortCnvInput();
		unsafe void NoteToFreq();
		unsafe void System();
		unsafe void Monitor();

		unsafe void TstClose();
		unsafe void Tst();
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

		public BRKP* Brkp;      //!< Storage for breakpoint logic

		public LABEL* Label;          //!< Storage for labels
		public UWORD Debug;                      //!< Debug flag

		public UBYTE* Name;

		public PRG()
		{
			Brkp = CommonHelper.Pointer1d<BRKP>(MAX_BREAKPOINTS, true);
			Label = CommonHelper.Pointer1d<LABEL>(MAX_LABELS, true);
			Name = CommonHelper.Pointer1d<UBYTE>(FILENAME_SIZE, false);
		}
	}

	/*! \struct TYPES
	 *          Device type data
	 */
	public unsafe struct TYPES // if data type changes - remember to change "cInputTypeDataInit" !
	{
		public UBYTE* Name; //!< Device name
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
		public SBYTE* Symbol;  //!< SI unit symbol
		public UWORD Align;

		public TYPES()
		{
			Symbol = CommonHelper.Pointer1d<SBYTE>(SYMBOL_LENGTH + 1, false);
			Name = CommonHelper.Pointer1d<UBYTE>(TYPE_NAME_LENGTH + 1, false);
		}
	}

	public unsafe struct COLORSTRUCT
	{
		public ULONG** Calibration;
		public UWORD* CalLimits;
		public UWORD Crc;
		public UWORD* ADRaw;
		public UWORD* SensorRaw;

		public COLORSTRUCT()
		{
			Calibration = CommonHelper.Pointer2d<ULONG>(CALPOINTS, COLORS);
			CalLimits = CommonHelper.Pointer1d<UWORD>(CALPOINTS - 1);
			ADRaw = CommonHelper.Pointer1d<UWORD>(COLORS);
			SensorRaw = CommonHelper.Pointer1d<UWORD>(COLORS);
		}
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
		public DATA16* InPin1;         //!< Analog value at input port connection 1
		public DATA16* InPin6;         //!< Analog value at input port connection 6
		public DATA16* OutPin5;       //!< Analog value at output port connection 5
		public DATA16 BatteryTemp;            //!< Battery temperature
		public DATA16 MotorCurrent;           //!< Current flowing to motors
		public DATA16 BatteryCurrent;         //!< Current flowing from the battery
		public DATA16 Cell123456;             //!< Voltage at battery cell 1, 2, 3,4, 5, and 6
		public DATA16** Pin1;      //!< Raw value from analog device
		public DATA16** Pin6;      //!< Raw value from analog device
		public UWORD* Actual;
		public UWORD* LogIn;
		public UWORD* LogOut;
		public COLORSTRUCT* NxtCol;
		public DATA16* OutPin5Low;    //!< Analog value at output port connection 5 when connection 6 is low

		public DATA8* Updated;

		public DATA8* InDcm;          //!< Input port device types
		public DATA8* InConn;

		public DATA8* OutDcm;        //!< Output port device types
		public DATA8* OutConn;
		public UWORD PreemptMilliSeconds;

		public ANALOG()
		{
			InPin1 = CommonHelper.Pointer1d<DATA16>(INPUTS);
			InPin6 = CommonHelper.Pointer1d<DATA16>(INPUTS);
			OutPin5 = CommonHelper.Pointer1d<DATA16>(OUTPUTS);
			Pin1 = CommonHelper.Pointer2d<DATA16>(INPUTS, DEVICE_LOGBUF_SIZE);
			Pin6 = CommonHelper.Pointer2d<DATA16>(INPUTS, DEVICE_LOGBUF_SIZE);
			Actual = CommonHelper.Pointer1d<UWORD>(INPUTS);
			LogIn = CommonHelper.Pointer1d<UWORD>(INPUTS);
			LogOut = CommonHelper.Pointer1d<UWORD>(INPUTS);
			NxtCol = CommonHelper.Pointer1d<COLORSTRUCT>(INPUTS, true);
			OutPin5Low = CommonHelper.Pointer1d<DATA16>(OUTPUTS);
			Updated = CommonHelper.Pointer1d<DATA8>(INPUTS);
			InDcm = CommonHelper.Pointer1d<DATA8>(INPUTS);
			InConn = CommonHelper.Pointer1d<DATA8>(INPUTS);
			OutDcm = CommonHelper.Pointer1d<DATA8>(OUTPUTS);
			OutConn = CommonHelper.Pointer1d<DATA8>(OUTPUTS);
		}
	}

	public unsafe struct UART
	{
		public TYPES** TypeData; //!< TypeData

		public DATA8** Raw;

		public DATA8* Status;                     //!< Status
		public DATA8** Output;   //!< Bytes to UART device
		public DATA8* OutputLength;

		public UART()
		{
			TypeData = CommonHelper.Pointer2d<TYPES>(INPUTS, MAX_DEVICE_MODES, true);
			Raw = CommonHelper.Pointer2d<DATA8>(INPUTS, UART_DATA_LENGTH);
			Status = CommonHelper.Pointer1d<DATA8>(INPUTS);
			Output = CommonHelper.Pointer2d<DATA8>(INPUTS, UART_DATA_LENGTH);
			OutputLength = CommonHelper.Pointer1d<DATA8>(INPUTS);
		}
	}

	public unsafe struct DEVCON
	{
		public DATA8* Connection;
		public DATA8* Type;
		public DATA8* Mode;

		public DEVCON()
		{
			Connection = CommonHelper.Pointer1d<DATA8>(INPUTS);
			Type = CommonHelper.Pointer1d<DATA8>(INPUTS);
			Mode = CommonHelper.Pointer1d<DATA8>(INPUTS);
		}
	}

	public unsafe struct UARTCTL
	{
		public TYPES TypeData;
		public DATA8 Port;
		public DATA8 Mode;
	}

	public unsafe struct IIC
	{
		public TYPES** TypeData; //!< TypeData

		public DATA8** Raw;      //!< Raw value from IIC device

		public DATA8* Status;                     //!< Status
		public DATA8* Changed;
		public DATA8** Output;    //!< Bytes to IIC device
		public DATA8* OutputLength;

		public IIC()
		{
			TypeData = CommonHelper.Pointer2d<TYPES>(INPUTS, MAX_DEVICE_MODES, true);
			Raw = CommonHelper.Pointer2d<DATA8>(INPUTS, IIC_DATA_LENGTH);
			Status = CommonHelper.Pointer1d<DATA8>(INPUTS);
			Changed = CommonHelper.Pointer1d<DATA8>(INPUTS);
			Output = CommonHelper.Pointer2d<DATA8>(INPUTS, IIC_DATA_LENGTH);
			OutputLength = CommonHelper.Pointer1d<DATA8>(INPUTS);
		}
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
		public DATA8* WrData;
		public DATA8 RdLng;
		public DATA8* RdData;

		public IICDAT()
		{
			WrData = CommonHelper.Pointer1d<DATA8>(IIC_DATA_LENGTH);
			RdData = CommonHelper.Pointer1d<DATA8>(IIC_DATA_LENGTH);
		}
	}

	public unsafe struct IICSTR
	{
		public DATA8 Port;
		public DATA16 Time;
		public DATA8 Type;
		public DATA8 Mode;
		public DATA8* Manufacturer;
		public DATA8* SensorType;
		public DATA8 SetupLng;
		public ULONG SetupString;
		public DATA8 PollLng;
		public ULONG PollString;
		public DATA8 ReadLng;

		public IICSTR()
		{
			Manufacturer = CommonHelper.Pointer1d<DATA8>(IIC_NAME_LENGTH + 1);
			SensorType = CommonHelper.Pointer1d<DATA8>(IIC_NAME_LENGTH + 1);
		}
	}

	public unsafe struct TSTPIN
	{
		public DATA8 Port;
		public DATA8 Length;
		public DATA8* String;

		public TSTPIN()
		{
			String = CommonHelper.Pointer1d<DATA8>(TST_PIN_LENGTH + 1);
		}
	}

	public unsafe struct TSTUART
	{
		public DATA32 Bitrate;
		public DATA8 Port;
		public DATA8 Length;
		public DATA8* String;

		public TSTUART()
		{
			String = CommonHelper.Pointer1d<DATA8>(TST_UART_LENGTH);
		}
	}

	public unsafe struct UI
	{
		public DATA8* Pressed;                   //!< Pressed status

		public UI()
		{
			Pressed = CommonHelper.Pointer1d<DATA8>(BUTTONS);
		}
	}

	public unsafe struct LCD
	{
		public UBYTE* Lcd;

		public LCD()
		{
			Lcd = CommonHelper.Pointer1d<UBYTE>(LCD_BUFFER_SIZE);
		}
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
			FirstProgram = CommonHelper.Pointer1d<DATA8>(MAX_FILENAME_SIZE);
			PrintBuffer = CommonHelper.Pointer1d<DATA8>(PRINTBUFFERSIZE + 1);
			Program = CommonHelper.Pointer1d<PRG>(MAX_PROGRAMS, true);
			Errors = CommonHelper.Pointer1d<ERR>(ERROR_BUFFER_SIZE);
		}
	}
}
