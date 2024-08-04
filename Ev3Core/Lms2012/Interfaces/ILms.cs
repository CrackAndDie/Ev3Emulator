using Ev3Core.Enums;
using Ev3Core.Helpers;
using System.Runtime.Intrinsics.X86;
using static Ev3Core.Defines;

namespace Ev3Core.Lms2012.Interfaces
{
	public interface ILms
	{
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

        // was not presented in lms2012.h
        ArrayPointer<byte> PrimParPointer(); // to get
		void PrimParAdvance();

        PRGID CurrentProgramId();
		OBJID CallingObjectId();
		DSPSTAT ExecuteByteCode(IP pByteCode, GP pGlobals, LP pLocals);
		void SetTerminalEnable(DATA8 Value);
		DATA8 GetTerminalEnable();
		void SetSleepMinutes(DATA8 Minutes);
		DATA8 GetSleepMinutes();
		void ProgramEnd(PRGID PrgId);
		RESULT ValidateChar(ref DATA8 pChar, DATA8 Set);
		RESULT ValidateString(DATA8[] pString, DATA8 Set); // pay attention to this shite. strings are immutable in c#
        byte[] VmMemoryResize(HANDLER Handle, DATA32 Elements);
		void SetDispatchStatus(DSPSTAT DspStat);
		void SetDispatchStatus(int DspStat);
		void GetResourcePath(char[] pString, DATA8 MaxLength);
		void SetInstructions(ULONG Instructions);
		void AdjustObjectIp(IMOFFS Value);


        IP GetObjectIp();
		void SetObjectIp(IP Ip);

		OBJSTAT ProgramStatus(PRGID PrgId);
		OBJSTAT ProgramStatusChange(PRGID PrgId);

		DATA8 CheckUsbstick(ref DATA8 pChanged, ref DATA32 pTotal, ref DATA32 pFree, DATA8 Force);
		DATA8 CheckSdcard(ref DATA8 pChanged, ref DATA32 pTotal, ref DATA32 pFree, DATA8 Force);
	}

	public class OBJ // Object
	{
		public IP Ip;                           //!< Object instruction pointer
		public LP pLocal;                       //!< Local variable pointer
		public UWORD ObjStatus;                    //!< Object status
		public (OBJID CallerId, TRIGGER TriggerCount) u;                   //!< Caller id used for SUBCALL to save object id to return to
		public VARDATA[] Local;                      //!< Poll of bytes used for local variables
	}

	public class BRKP
	{
		public IMINDEX Addr;                         //!< Offset to breakpoint address from image start
		public OP OpCode;                       //!< Saved substituted opcode
	}

	public class PRG
	{
		public ULONG InstrCnt;                   //!< Instruction counter used for performance analyses
		public ULONG InstrTime;                  //!< Instruction time used for performance analyses

		public ULONG StartTime;                  //!< Program start time [mS]
		public ULONG RunTime;                    //!< Program run time [uS]

		public IP pImage;                     //!< Pointer to start of image
		public int pImageInd;                     //!< Pointer to start of image
		public GP pData;                      //!< Pointer to start of data
		public GP pGlobal;                    //!< Pointer to start of global bytes
		public OBJHEAD[] pObjHead;                   //!< Pointer to start of object headers
		public OBJ[][] pObjList;                   //!< Pointer to object pointer list
		public IP ObjectIp;                   //!< Working object Ip
		public LP ObjectLocal;                //!< Working object locals

		public OBJID Objects;                    //!< No of objects in image
		public OBJID ObjectId;                   //!< Active object id

		public OBJSTAT Status;                     //!< Program status
		public OBJSTAT StatusChange;               //!< Program status change
		public RESULT Result;                     //!< Program result (OK, BUSY, FAIL)

		public BRKP[] Brkp = CommonHelper.Array1d<BRKP>(MAX_BREAKPOINTS, true);      //!< Storage for breakpoint logic

		public LABEL[] Label = CommonHelper.Array1d<LABEL>(MAX_LABELS, true);           //!< Storage for labels
		public UWORD Debug;                      //!< Debug flag

		public DATA8[] Name = CommonHelper.Array1d<DATA8>(FILENAME_SIZE);           //!
	}

	public class TYPES : ICloneable // if data type changes - remember to change "cInputTypeDataInit" !
	{
		public SBYTE[] Name = CommonHelper.Array1d<SBYTE>(TYPE_NAME_LENGTH + 1); //!< Device name
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
		public SBYTE[] Symbol = CommonHelper.Array1d<SBYTE>(SYMBOL_LENGTH + 1);  //!< SI unit symbol
		public UWORD Align;

		public object Clone()
		{
			return new TYPES()
			{
				Name = (SBYTE[])Name.Clone(),
				Type = Type,
				Connection = Connection,
				Mode = Mode,
				DataSets = DataSets,
				Format = Format,
				Figures = Figures,
				Decimals = Decimals,
				Views = Views,
				RawMin = RawMin,
				RawMax = RawMax,
				PctMin = PctMin,
				PctMax = PctMax,
				SiMin = SiMin,
				SiMax = SiMax,
				InvalidTime = InvalidTime,
				IdValue = IdValue,
				Pins = Pins,
				Symbol = (SBYTE[])Symbol.Clone(),
				Align = Align,
			};
		}
	}

	public class COLORSTRUCT
	{
		public ULONG[][] Calibration = CommonHelper.Array2d<ULONG>(CALPOINTS, COLORS);
		public UWORD[] CalLimits = CommonHelper.Array1d<UWORD>(CALPOINTS - 1);
		public UWORD Crc;
		public UWORD[] ADRaw = CommonHelper.Array1d<UWORD>(COLORS);
		public UWORD[] SensorRaw = CommonHelper.Array1d<UWORD>(COLORS);
	}

	public class ANALOG
	{
		public DATA16[] InPin1 = CommonHelper.Array1d<DATA16>(INPUTS);         //!< Analog value at input port connection 1
		public DATA16[] InPin6 = CommonHelper.Array1d<DATA16>(INPUTS);         //!< Analog value at input port connection 6
		public DATA16[] OutPin5 = CommonHelper.Array1d<DATA16>(OUTPUTS);       //!< Analog value at output port connection 5
		public DATA16 BatteryTemp;            //!< Battery temperature
		public DATA16 MotorCurrent;           //!< Current flowing to motors
		public DATA16 BatteryCurrent;         //!< Current flowing from the battery
		public DATA16 Cell123456;             //!< Voltage at battery cell 1, 2, 3,4, 5, and 6
		public DATA16[][] Pin1 = CommonHelper.Array2d<DATA16>(INPUTS, DEVICE_LOGBUF_SIZE);      //!< Raw value from analog device
		public DATA16[][] Pin6 = CommonHelper.Array2d<DATA16>(INPUTS, DEVICE_LOGBUF_SIZE);      //!< Raw value from analog device
		public UWORD[] Actual = CommonHelper.Array1d<UWORD>(INPUTS);
		public UWORD[] LogIn = CommonHelper.Array1d<UWORD>(INPUTS);
		public UWORD[] LogOut = CommonHelper.Array1d<UWORD>(INPUTS);
		public COLORSTRUCT[] NxtCol = CommonHelper.Array1d<COLORSTRUCT>(INPUTS, true);
		public DATA16[] OutPin5Low = CommonHelper.Array1d<DATA16>(OUTPUTS);    //!< Analog value at output port connection 5 when connection 6 is low

		public DATA8[] Updated = CommonHelper.Array1d<DATA8>(INPUTS);

		public DATA8[] InDcm = CommonHelper.Array1d<DATA8>(INPUTS);          //!< Input port device types
		public DATA8[] InConn = CommonHelper.Array1d<DATA8>(INPUTS);

		public DATA8[] OutDcm = CommonHelper.Array1d<DATA8>(OUTPUTS);        //!< Output port device types
		public DATA8[] OutConn = CommonHelper.Array1d<DATA8>(OUTPUTS);
		public UWORD PreemptMilliSeconds;
	}

	public class UART
	{
		public TYPES[][] TypeData = CommonHelper.Array2d<TYPES>(INPUTS, MAX_DEVICE_MODES, true); //!< TypeData
		public DATA8[][] Raw = CommonHelper.Array2d<DATA8>(INPUTS, UART_DATA_LENGTH);      //!< Raw value from UART device
		public DATA8[] Status = CommonHelper.Array1d<DATA8>(INPUTS);                     //!< Status
		public DATA8[][] Output = CommonHelper.Array2d<DATA8>(INPUTS, UART_DATA_LENGTH);   //!< Bytes to UART device
		public DATA8[] OutputLength = CommonHelper.Array1d<DATA8>(INPUTS);
	}

	public class DEVCON
	{
		public DATA8[] Connection = CommonHelper.Array1d<DATA8>(INPUTS);
		public DATA8[] Type = CommonHelper.Array1d<DATA8>(INPUTS);
		public DATA8[] Mode = CommonHelper.Array1d<DATA8>(INPUTS);
	}

	public class UARTCTL
	{
		public TYPES TypeData;
		public DATA8 Port;
		public DATA8 Mode;
	}

	public class IIC
	{
		public TYPES[][] TypeData = CommonHelper.Array2d<TYPES>(INPUTS, MAX_DEVICE_MODES, true); //!< TypeData
		public DATA8[][] Raw = CommonHelper.Array2d<DATA8>(INPUTS, IIC_DATA_LENGTH);      //!< Raw value from IIC device
		public DATA8[] Status = CommonHelper.Array1d<DATA8>(INPUTS);                     //!< Status
		public DATA8[] Changed = CommonHelper.Array1d<DATA8>(INPUTS);
		public DATA8[][] Output = CommonHelper.Array2d<DATA8>(INPUTS, IIC_DATA_LENGTH);    //!< Bytes to IIC device
		public DATA8[] OutputLength = CommonHelper.Array1d<DATA8>(INPUTS);
	}

	public class IICCTL
	{
		public TYPES TypeData = new TYPES();
		public DATA8 Port;
		public DATA8 Mode;
	}

	public class IICDAT
	{
		public RESULT Result;
		public DATA8 Port;
		public DATA8 Repeat;
		public DATA16 Time;
		public DATA8 WrLng;
		public DATA8[] WrData = CommonHelper.Array1d<DATA8>(IIC_DATA_LENGTH);
		public DATA8 RdLng;
		public DATA8[] RdData = CommonHelper.Array1d<DATA8>(IIC_DATA_LENGTH);
	}

	public class IICSTR
	{
		public DATA8 Port;
		public DATA16 Time;
		public DATA8 Type;
		public DATA8 Mode;
		public DATA8[] Manufacturer = CommonHelper.Array1d<DATA8>(IIC_NAME_LENGTH + 1);
		public DATA8[] SensorType = CommonHelper.Array1d<DATA8>(IIC_NAME_LENGTH + 1);
		public DATA8 SetupLng;
		public ULONG SetupString;
		public DATA8 PollLng;
		public ULONG PollString;
		public DATA8 ReadLng;

		public const int Sizeof = 24;

		public static IICSTR FromBytes(object[] data)
		{
			// TODO:
			return null;
		}
	}

	public class TSTPIN
	{
		public DATA8 Port;
		public DATA8 Length;
		public DATA8[] String = CommonHelper.Array1d<DATA8>(TST_PIN_LENGTH + 1);
	}

	public class TSTUART
	{
		public DATA32 Bitrate;
		public DATA8 Port;
		public DATA8 Length;
		public DATA8[] String = CommonHelper.Array1d<DATA8>(TST_UART_LENGTH);
	}

	public class UI
	{
		public DATA8[] Pressed = CommonHelper.Array1d<DATA8>(BUTTONS);                   //!< Pressed status
	}

	public class LCD
	{
		public UBYTE[] Lcd = CommonHelper.Array1d<UBYTE>(LCD_BUFFER_SIZE);

		public static int LcdSizeof = LCD_BUFFER_SIZE;
	}

	public class SOUND
	{
		public DATA8 Status;                       //!< Status
	}

	public class USB_SPEED
	{
		public DATA8 Speed;
	}

	public class NONVOL
	{
		public DATA8 VolumePercent;                //!< System default volume [0..100%]
		public DATA8 SleepMinutes;                 //!< System sleep          [0..120min] (0 = ~)
	}

	/*
	 *    Motor/OUTPUT Typedef
	 */
	public class MOTORDATA
	{
		public SLONG TachoCounts;
		public SBYTE Speed;
		public SLONG TachoSensor;
	}

	public class STEPPOWER
	{
		public UBYTE Cmd;
		public DATA8 Nos;
		public DATA8 Power;
		public DATA32 Step1;
		public DATA32 Step2;
		public DATA32 Step3;
		public DATA8 Brake;

		public const int Sizeof = 16;

		public byte[] ToByteArray()
		{
			var st1 = CommonHelper.GetBytes(Step1);
			var st2 = CommonHelper.GetBytes(Step2);
			var st3 = CommonHelper.GetBytes(Step3);
			return new byte[] { Cmd, (byte)Nos, (byte)Power, st1[0], st1[1], st1[2], st1[3], st2[0], st2[1], st2[2], st2[3], st3[0], st3[1], st3[2], st3[3], (byte)Brake };
		}
	}

	public class TIMEPOWER
	{
		public UBYTE Cmd;
		public DATA8 Nos;
		public DATA8 Power;
		public DATA32 Time1;
		public DATA32 Time2;
		public DATA32 Time3;
		public DATA8 Brake;

		public const int Sizeof = 16;

		public byte[] ToByteArray()
		{
			var st1 = CommonHelper.GetBytes(Time1);
			var st2 = CommonHelper.GetBytes(Time2);
			var st3 = CommonHelper.GetBytes(Time3);
			return new byte[] { Cmd, (byte)Nos, (byte)Power, st1[0], st1[1], st1[2], st1[3], st2[0], st2[1], st2[2], st2[3], st3[0], st3[1], st3[2], st3[3], (byte)Brake };
		}
	}

	public class STEPSPEED
	{
		public UBYTE Cmd;
		public DATA8 Nos;
		public DATA8 Speed;
		public DATA32 Step1;
		public DATA32 Step2;
		public DATA32 Step3;
		public DATA8 Brake;

		public const int Sizeof = 16;

		public byte[] ToByteArray()
		{
			var st1 = CommonHelper.GetBytes(Step1);
			var st2 = CommonHelper.GetBytes(Step2);
			var st3 = CommonHelper.GetBytes(Step3);
			return new byte[] { Cmd, (byte)Nos, (byte)Speed, st1[0], st1[1], st1[2], st1[3], st2[0], st2[1], st2[2], st2[3], st3[0], st3[1], st3[2], st3[3], (byte)Brake };
		}
	}

	public class TIMESPEED
	{
		public UBYTE Cmd;
		public DATA8 Nos;
		public DATA8 Speed;
		public DATA32 Time1;
		public DATA32 Time2;
		public DATA32 Time3;
		public DATA8 Brake;

		public const int Sizeof = 16;

		public byte[] ToByteArray()
		{
			var st1 = CommonHelper.GetBytes(Time1);
			var st2 = CommonHelper.GetBytes(Time2);
			var st3 = CommonHelper.GetBytes(Time3);
			return new byte[] { Cmd, (byte)Nos, (byte)Speed, st1[0], st1[1], st1[2], st1[3], st2[0], st2[1], st2[2], st2[3], st3[0], st3[1], st3[2], st3[3], (byte)Brake };
		}
	}

	public class STEPSYNC
	{
		public UBYTE Cmd;
		public DATA8 Nos;
		public DATA8 Speed;
		public DATA16 Turn;
		public DATA32 Step;
		public DATA8 Brake;

		public const int Sizeof = 10;

		public byte[] ToByteArray()
		{
			var st1 = CommonHelper.GetBytes(Turn);
			var st2 = CommonHelper.GetBytes(Step);
			return new byte[] { Cmd, (byte)Nos, (byte)Speed, st1[0], st1[1], st2[0], st2[1], st2[2], st2[3], (byte)Brake };
		}
	}

	public class TIMESYNC
	{
		public UBYTE Cmd;
		public DATA8 Nos;
		public DATA8 Speed;
		public DATA16 Turn;
		public DATA32 Time;
		public DATA8 Brake;

		public const int Sizeof = 10;

		public byte[] ToByteArray()
		{
			var st1 = CommonHelper.GetBytes(Turn);
			var st2 = CommonHelper.GetBytes(Time);
			return new byte[] { Cmd, (byte)Nos, (byte)Speed, st1[0], st1[1], st2[0], st2[1], st2[2], st2[3], (byte)Brake };
		}
	}

	public class GLOBALS
	{
		public NONVOL NonVol = new NONVOL();
		public DATA8[] FirstProgram = CommonHelper.Array1d<DATA8>(MAX_FILENAME_SIZE);

		public char[] PrintBuffer = CommonHelper.Array1d<char>(PRINTBUFFERSIZE + 1);
		public DATA8 TerminalEnabled;

		public PRGID FavouritePrg;
		public PRGID ProgramId;                    //!< Program id running
		public PRG[] Program = CommonHelper.Array1d<PRG>(MAX_PROGRAMS, true);        //!< Program[0] is the UI byte codes running

		public ULONG InstrCnt;                     //!< Instruction counter (performance test)
		public IP pImage;                       //!< Pointer to start of image
		public GP pGlobal;                      //!< Pointer to start of global bytes
		public OBJHEAD[] pObjHead;                     //!< Pointer to start of object headers
		public OBJ[][] pObjList;                     //!< Pointer to object pointer list

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
		public ArraySubset<byte> Handle;

		public ERR[] Errors = CommonHelper.Array1d<ERR>(ERROR_BUFFER_SIZE);
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

		public LCD LcdBuffer = new LCD();                    //!< Copy of last LCD update
		public DATA8 LcdUpdated;                   //!< LCD updated

		public ANALOG Analog = new ANALOG();
		public int AdcFile;

		public DATA8 Status;
	}
}
