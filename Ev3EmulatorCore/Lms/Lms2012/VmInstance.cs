using EV3DecompilerLib.Decompile;
using Ev3EmulatorCore.Lms.Cui;

namespace Ev3EmulatorCore.Lms.Lms2012
{
	public partial class LmsInstance
	{
		public class NONVOL
		{
			byte VolumePercent;                //!< System default volume [0..100%]
			byte SleepMinutes;                 //!< System sleep          [0..120min] (0 = ~)
		}

		public class BRKP
		{
			ulong Addr;                         //!< Offset to breakpoint address from image start
			lms2012.Op OpCode;                       //!< Saved substituted opcode
		}

		public class LABEL
		{
			ulong Addr;                         //!< Offset to breakpoint address from image start
		}

		public class OBJ // Object
		{
			byte[] Ip;                           //!< Object instruction pointer
			byte[] pLocal;                       //!< Local variable pointer
			short ObjStatus;   
			ushort CallerId;                   //!< Caller id used for SUBCALL to save object id to return to
			ushort TriggerCount;               //!< Trigger count used by BLOCK's trigger logic
			byte[] Local;                      //!< Poll of bytes used for local variables
		}

		public class PRG
		{
			public ulong InstrCnt;                   //!< Instruction counter used for performance analyses
			public ulong InstrTime;                  //!< Instruction time used for performance analyses

			public ulong StartTime;                  //!< Program start time [mS]
			public ulong RunTime;                    //!< Program run time [uS]

			public byte[] pImage;                     //!< Pointer to start of image
			public byte[] pData;                      //!< Pointer to start of data
			public byte[] pGlobal;                    //!< Pointer to start of global bytes
			public lms2012.ObjectHeader[] pObjHead;                   //!< Pointer to start of object headers
			public OBJ[][] pObjList;                   //!< Pointer to object pointer list
			public byte[] ObjectIp;                   //!< Working object Ip
			public byte[] ObjectLocal;                //!< Working object locals

			public ushort Objects;                    //!< No of objects in image
			public ushort ObjectId;                   //!< Active object id

			public lms2012.ObjectStatus Status;                     //!< Program status
			public lms2012.ObjectStatus StatusChange;               //!< Program status change
			public lms2012.Result Result;                     //!< Program result (OK, BUSY, FAIL)

			public BRKP[] Brkp = Enumerable.Repeat(new BRKP(), lms2012.MAX_BREAKPOINTS).ToArray();      //!< Storage for breakpoint logic

			public LABEL[] Label = Enumerable.Repeat(new LABEL(), lms2012.MAX_LABELS).ToArray();          //!< Storage for labels
			public ushort Debug;                      //!< Debug flag

			public byte[] Name = new byte[lms2012.FILENAME_SIZE];
		}

		public class GLOBALS
		{
			public NONVOL NonVol = new NONVOL();
			public byte[] FirstProgram = new byte[lms2012.MAX_FILENAME_SIZE];

			public char[] PrintBuffer = new char[lms2012.PRINTBUFFERSIZE + 1];
			public byte TerminalEnabled;

			public ushort FavouritePrg;
			public ushort ProgramId;                    //!< Program id running
			public PRG[] Program = Enumerable.Repeat(new PRG(), lms2012.MAX_PROGRAMS).ToArray();       //!< Program[0] is the UI byte codes running

			public ulong InstrCnt;                     //!< Instruction counter (performance test)
			public byte[] pImage;                       //!< Pointer to start of image
			public byte[] pGlobal;                      //!< Pointer to start of global bytes
			public lms2012.ObjectHeader[] pObjHead;                     //!< Pointer to start of object headers
			public OBJ[][] pObjList;                     //!< Pointer to object pointer list

			public byte[] ObjectIp;                     //!< Working object Ip
			public byte[] ObjectLocal;                  //!< Working object locals
			public ushort Objects;                      //!< No of objects in image
			public ushort ObjectId;                     //!< Active object id

			public byte[] ObjIpSave;
			public byte[] ObjGlobalSave;
			public byte[] ObjLocalSave;
			public lms2012.DSPSTAT DispatchStatusSave;
			public ulong PrioritySave;

			public long TimerDataSec;
			public long TimerDatanSec;

			public ushort Debug;

			public ushort Test;

			public ushort RefCount;

			public ulong TimeuS;

			public ulong OldTime1;
			public ulong OldTime2;
			public ulong NewTime;

			public lms2012.DSPSTAT DispatchStatus;               //!< Dispatch status
			public ulong Priority;                     //!< Object priority

			public ulong Value;
			public short Handle;

			public lms2012.Error[] Errors = new lms2012.Error[lms2012.ERROR_BUFFER_SIZE];
			public byte ErrorIn;
			public byte ErrorOut;

			public int MemorySize;
			public int MemoryFree;
			public ulong MemoryTimer;

			public int SdcardSize;
			public int SdcardFree;
			public ulong SdcardTimer;
			public byte SdcardOk;

			public int UsbstickSize;
			public int UsbstickFree;
			public ulong UsbstickTimer;
			public byte UsbstickOk;

			public DlcdClass.LCD LcdBuffer = new DlcdClass.LCD();                    //!< Copy of last LCD update
			public byte LcdUpdated;                   //!< LCD updated
		}
	}
}
