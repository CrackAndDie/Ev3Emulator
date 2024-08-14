using Ev3CoreUnsafe.Enums;
using Ev3CoreUnsafe.Extensions;
using Ev3CoreUnsafe.Helpers;
using Ev3CoreUnsafe.Cui.Interfaces;
using Ev3CoreUnsafe.Lms2012.Interfaces;

namespace Ev3CoreUnsafe
{
	public partial class Defines
	{
		#region lms2012.h
		public const int TERMINAL_ENABLED = 0;    //!< DEBUG terminal enabled (0 = disabled, 1 = enabled)
		public const int DEBUG_UART = 4;     //!< UART used for debug (0 = port1, 1 = port2, ... 4 = none)

		public const int EP2 = 4;    //!< Schematics revision D
		public const int FINALB = 3;    //!< Schematics revision B and C
		public const int FINAL = 2;    //!< Final prototype
		public const int SIMULATION = 0;    //!< LEGO digital simulation

		public const int PLATFORM_START = FINAL;  //!< Oldest supported hardware (older versions will use this)
		public const int PLATFORM_END = EP2;   //!< Newest supported hardware (newer versions will use this)

		// Will be removed when not used anymore
		public const int A4 = -1;
		public const int EVALBOARD = -2;
		public const int ONE2ONE = 1;    //!< First real size prototype


		public const int HARDWARE = FINAL;       //!< Actual hardware platform (must be one of above)

		//  Support for module parameter "HwId"
		//
		//  Readout   File    int       PCB
		//
		//  V1.00     10      10        MP      (h = home, e = education)
		//  V0.50     05      5         EP3
		//  V0.40     04      4         EP2
		//  V0.30     03      3         EP1     (FINALB)  (DEFAULT if file "HwId" not found)
		//  V0.20     02      2         FINAL


		public const string HwId = "03";
		public readonly static int HWID = (((HwId[0] - '0') * 10) + (HwId[1] - '0'));

		public const int TESTDEVICE = 3;
		// Hardware

		public const int OUTPUTS = vmOUTPUTS;                //!< Number of output ports in the system
		public const int INPUTS = vmINPUTS;                 //!< Number of input  ports in the system
		public const int BUTTONS = vmBUTTONS;                 //!< Number of buttons in the system
		public const int LEDS = vmLEDS;                   //!< Number of LEDs in the system

		public const int LCD_WIDTH = vmLCD_WIDTH;              //!< LCD horizontal pixels
		public const int LCD_HEIGHT = vmLCD_HEIGHT;            //!< LCD vertical pixels
		public const int TOPLINE_HEIGHT = vmTOPLINE_HEIGHT;          //!< Top line vertical pixels
		public const int LCD_STORE_LEVELS = vmLCD_STORE_LEVELS;          //!< Store levels

		// Software

		public const int FG_COLOR = vmFG_COLOR;               //!<  Foreground color
		public const int BG_COLOR = vmBG_COLOR;               //!<  Background color

		public const int CHAIN_DEPT = vmCHAIN_DEPT;                //!< Number of bricks in the USB daisy chain (master + slaves)

		public const int EVENT_BT_PIN = vmEVENT_BT_PIN;

		// Folders

		public const string MEMORY_FOLDER = vmMEMORY_FOLDER;
		public const string PROGRAM_FOLDER = vmPROGRAM_FOLDER;
		public const string DATALOG_FOLDER = vmDATALOG_FOLDER;
		public const string SDCARD_FOLDER = vmSDCARD_FOLDER;
		public const string USBSTICK_FOLDER = vmUSBSTICK_FOLDER;

		// Files
		public const string DETAILS_FILE = vmDETAILS_FILE;           //!< File containing firmware version

		// Extensions

		public const string EXT_SOUND = vmEXT_SOUND;             //!< Rudolf sound file
		public const string EXT_GRAPHICS = vmEXT_GRAPHICS;             //!< Rudolf graphics file
		public const string EXT_BYTECODE = vmEXT_BYTECODE;             //!< Rudolf byte code file
		public const string EXT_TEXT = vmEXT_TEXT;              //!< Rudolf text file
		public const string EXT_DATALOG = vmEXT_DATALOG;             //!< Rudolf datalog file
		public const string EXT_PROGRAM = vmEXT_PROGRAM;             //!< Rudolf program byte code file
		public const string EXT_CONFIG = vmEXT_CONFIG;              //!< rudolf configuration file

		/*! \page system
         *
         *  \verbatim
         */

		public const string PROJECT = "LMS2012";
		public const float VERS = 1.04f;
		public const int SPECIALVERS = 'H';      //!< Minor version (not shown if less than ASCII zero)


		public const int MAX_PROGRAMS = 5;          //!< Max number of programs (including UI and direct commands) running at a time
		public const int MAX_BREAKPOINTS = 4;          //!< Max number of breakpoints (opCODES depends on this value)
		public const int MAX_LABELS = 32;         //!< Max number of labels per program
		public const int MAX_DEVICE_TYPE = 127;     //!< Highest type number (positive)
		public const int MAX_VALID_TYPE = vmMAX_VALID_TYPE;   //!< Highest valid type
		public const int MAX_DEVICE_MODES = 8;   //!< Max number of modes in one device
		public const int MAX_DEVICE_DATASETS = 8;   //!< Max number of data sets in one device
		public const int MAX_DEVICE_DATALENGTH = 32;    //!< Max device data length

		public const int MAX_DEVICE_BUSY_TIME = 1200;     //!< Max number of mS a device can be busy when read

		public const int MAX_DEVICE_TYPES = ((MAX_DEVICE_TYPE + 1) * MAX_DEVICE_MODES);//!< Max number of different device types and modes (max type data list size)

		public const int MAX_FRAMES_PER_SEC = 10;         //!< Max frames per second update in display

		public const int CACHE_DEEPT = 10;         //!< Max number of programs cached (in RECENT FILES MENU)
		public const int MAX_HANDLES = 250;        //!< Max number of handles to memory pools and arrays in one program

		public const int MAX_ARRAY_SIZE = 1000000000;      //!< Max array size
		public const int MIN_ARRAY_ELEMENTS = 0;       //!< Min elements in a DATA8 array

		public const int INSTALLED_MEMORY = 6000;      //!< Flash allocated to hold user programs/data
		public const int RESERVED_MEMORY = 100;       //!< Memory reserve for system [KB]
		public const int LOW_MEMORY = 500;       //!< Low memory warning [KB]

		public const int LOGBUFFER_SIZE = 1000;        //!< Min log buffer size
		public const int DEVICE_LOGBUF_SIZE = 300;       //!< Device log buffer size (black layer buffer)
		public const int MIN_LIVE_UPDATE_TIME = 10;       //!< [mS] Min sample time when live update

		public const int MIN_IIC_REPEAT_TIME = 10;        //!< [mS] Min IIC device repeat time
		public const int MAX_IIC_REPEAT_TIME = 1000;        //!< [mS] Max IIC device repeat time

		public const int MAX_COMMAND_BYTECODES = 64;          //!< Max number of byte codes in a debug terminal direct command
		public const int MAX_COMMAND_LOCALS = 64;         //!< Max number of bytes allocated for direct command local variables
		public const int MAX_COMMAND_GLOBALS = 1021;         //!< Max number of bytes allocated for direct command global variables

		public const int UI_PRIORITY = 20;       //!< UI byte codes before switching VM thread
		public const int C_PRIORITY = 200;       //!< C call byte codes


		public const int PRG_PRIORITY = 200;           //!< Prg byte codes before switching VM thread

		public const int BUTTON_DEBOUNCE_TIME = 30;
		public const int BUTTON_START_REPEAT_TIME = 400;
		public const int BUTTON_REPEAT_TIME = 200;

		public const int LONG_PRESS_TIME = 3000;     //!< [mS] Time pressed before long press recognised

		public const int ADC_REF = 5000;        //!< [mV]  maximal value on ADC
		public const int ADC_RES = 4095;       //!< [CNT] maximal count on ADC

		public const int IN1_ID_HYSTERESIS = 50;           //!< [mV]  half of the span one Id takes up on input connection 1 voltage
		public const int OUT5_ID_HYSTERESIS = 100;           //!< [mV]  half of the span one Id takes up on output connection 5 voltage

		public const int DEVICE_UPDATE_TIME = 1000000;         //!< Min device (sensor) update time [nS]
		public const int DELAY_TO_TYPEDATA = 10000;          //!< Time from daisy chain active to upload type data [mS]
		public const int DAISYCHAIN_MODE_TIME = 10;         //!< Time for daisy chain change mode [mS]
		public const int MAX_FILE_HANDLES = 64;         //!< Max number of down load file handles
		public const int MIN_HANDLE = 3;          //!< Min file handle to close

		public const int ID_LENGTH = 7;         //!< Id length  (BT MAC id) (incl. zero terminator)
		public const int NAME_LENGTH = 12;         //!< Name length (not including zero termination)

		public const int ERROR_BUFFER_SIZE = 8;          //!< Number of errors in buffer

		public const string PWM_DEVICE = "lms_pwm";     //!< PWM device name
		public const string PWM_DEVICE_NAME = "/dev/lms_pwm";    //!< PWM device file name

		public const string MOTOR_DEVICE = "lms_motor";   //!< TACHO device name
		public const string MOTOR_DEVICE_NAME = "/dev/lms_motor";   //!< TACHO device file name

		public const string ANALOG_DEVICE = "lms_analog";   //!< ANALOG device name
		public const string ANALOG_DEVICE_NAME = "/dev/lms_analog";  //!< ANALOG device file name

		public const string POWER_DEVICE = "lms_power";   //!< POWER device name
		public const string POWER_DEVICE_NAME = "/dev/lms_power";  //!< POWER device file name

		public const string DCM_DEVICE = "lms_dcm";      //!< DCM device name
		public const string DCM_DEVICE_NAME = "/dev/lms_dcm";    //!< DCM device file name

		public const string UI_DEVICE = "lms_ui";    //!< UI device name
		public const string UI_DEVICE_NAME = "/dev/lms_ui";    //!< UI device file name

		public const string LCD_DEVICE = "lms_display";   //!< DISPLAY device name
		public const string LCD_DEVICE_NAME = "/dev/fb0";     //!< DISPLAY device file name

		public const string UART_DEVICE = "lms_uart";      //!< UART device name
		public const string UART_DEVICE_NAME = "/dev/lms_uart";    //!< UART device file name

		public const string USBDEV_DEVICE = "lms_usbdev";   //!< USB device
		public const string USBDEV_DEVICE_NAME = "/dev/lms_usbdev";  //!< USB device

		public const string USBHOST_DEVICE = "lms_usbhost";  //!< USB host
		public const string USBHOST_DEVICE_NAME = "/dev/lms_usbhost";  //!< USB host

		public const string SOUND_DEVICE = "lms_sound";    //!< SOUND device name
		public const string SOUND_DEVICE_NAME = "/dev/lms_sound";   //!< SOUND device

		public const string IIC_DEVICE = "lms_iic";   //!< IIC device name
		public const string IIC_DEVICE_NAME = "/dev/lms_iic";   //!< IIC device

		public const string BT_DEVICE = "lms_bt";    //!< BT device name
		public const string BT_DEVICE_NAME = "/dev/lms_bt";    //!< BT device

		public const string UPDATE_DEVICE = "lms_update";   //!< UPDATE device name
		public const string UPDATE_DEVICE_NAME = "/dev/lms_update";  //!< UPDATE device

		public const string TEST_PIN_DEVICE = "lms_tst_pin";   //!< TEST pin device name
		public const string TEST_PIN_DEVICE_NAME = "/dev/lms_tst_pin";  //!< TEST pin device file name

		public const string TEST_UART_DEVICE = "lms_tst_uart";  //!< TEST UART device name
		public const string TEST_UART_DEVICE_NAME = "/dev/lms_tst_uart"; //!< TEST UART device file name


		public const int DIR_DEEPT = vmDIR_DEEPT;      //!< Max directory items allocated

		// File

		//***********************************************************************************************************************
		//! \todo Filename sizes should only use vmPATHSIZE, vmNAMESIZE, vmEXTSIZE and vmFILENAMESIZE

		public const int FILENAMESIZE = vmFILENAMESIZE;     //!< All inclusive (path, filename, extension and zero termination)
		public const int FILENAME_SIZE = 52;      //!< User filename size without extension including zero
		public const int FOLDERNAME_SIZE = 10;     //!< Folder name size relative to "lms2012" folder including zero
		public const int SUBFOLDERNAME_SIZE = FILENAME_SIZE;    //!< Sub folder name size without "/" including zero

		public const int MAX_FILENAME_SIZE = (FOLDERNAME_SIZE + SUBFOLDERNAME_SIZE + FILENAME_SIZE + 5);

		//***********************************************************************************************************************

		public const string TYPEDATE_FILE_NAME = "typedata";        //!< TypeData filename
		public const string ICON_FILE_NAME = "icon";       //!< Icon image filename
		public const string TEXT_FILE_NAME = "text";         //!< Text filename

		public const string DEMO_FILE_NAME = "../prjs/BrkProg_SAVE/Demo.rpf";

		// Memory





		// SD card
		public const string SDCARD_DEVICE1 = "/dev/mmcblk0";
		public const string SDCARD_DEVICE2 = "/dev/mmcblk0p1";

		public const string SDCARD_MOUNT = "./mount_sdcard";
		public const string SDCARD_UNMOUNT = "./unmount_sdcard";

		// USB stick

		public const string USBSTICK_DEVICE = "/dev/sdf1";

		public const string USBSTICK_MOUNT = "./mount_usbstick";
		public const string USBSTICK_UNMOUNT = "./unmount_usbstick";

		public const string DEFAULT_FOLDER_CSHARP = "Resources";             //!< Folder containing the first small programs

		public const string DEFAULT_FOLDER = "ui";             //!< Folder containing the first small programs
		public const string DEFAULT_UI = "ui";            //!< Default user interface

		public const int DEFAULT_VOLUME = vmDEFAULT_VOLUME;
		public const int DEFAULT_SLEEPMINUTES = vmDEFAULT_SLEEPMINUTES;

		public const string COM_CMD_DEVICE_NAME = USBDEV_DEVICE_NAME;   //!< USB HID command pipe device file name

		/*! \endverbatim
         *
         */

		/*! \page powermanagement Power Management
         *
         *  This section describes various constants used in the power management
         *
         *
         *\n
         *  <hr size="1"/>
         *  <b>Battery Indicator</b>
         *\n
         *  <hr size="1"/>
         *  \verbatim
         */

		public const int BATT_INDICATOR_HIGH = 7500;      //!< Battery indicator high [mV]
		public const int BATT_INDICATOR_LOW = 6200;     //!< Battery indicator low [mV]

		public const int ACCU_INDICATOR_HIGH = 7500;       //!< Rechargeable battery indicator high [mV]
		public const int ACCU_INDICATOR_LOW = 7100;      //!< Rechargeable battery indicator low [mV]

		/*! \endverbatim
         *  \subpage pmbattind
         *\n
         *  <hr size="1"/>
         *  <b>Low Voltage Shutdown</b>
         *\n
         *  <hr size="1"/>
         *  \verbatim
         */

		public const float LOW_VOLTAGE_SHUTDOWN_TIME = 10000;      //!< Time from shutdown lower limit to shutdown [mS]

		public const float BATT_WARNING_HIGH = 6.2f;      //!< Battery voltage warning upper limit [V]
		public const float BATT_WARNING_LOW = 5.5f;     //!< Battery voltage warning lower limit [V]
		public const float BATT_SHUTDOWN_HIGH = 5.5f;      //!< Battery voltage shutdown upper limit [V]
		public const float BATT_SHUTDOWN_LOW = 4.5f;     //!< Battery voltage shutdown lower limit [V]

		public const float ACCU_WARNING_HIGH = 7.1f;     //!< Rechargeable battery voltage warning upper limit [V]
		public const float ACCU_WARNING_LOW = 6.5f;     //!< Rechargeable battery voltage warning lower limit [V]
		public const float ACCU_SHUTDOWN_HIGH = 6.5f;     //!< Rechargeable battery voltage shutdown upper limit [V]
		public const float ACCU_SHUTDOWN_LOW = 6.0f;     //!< Rechargeable battery voltage shutdown lower limit [V]

		/*! \endverbatim
         *  \subpage pmbattsd
         *\n
         */
		/*! \page powermanagement
         *
         *  <hr size="1"/>
         *  <b>High Load Shutdown</b>
         *\n
         *  <hr size="1"/>
         *  \verbatim
         */

		public const float LOAD_SHUTDOWN_FAIL = 4.0f;       //!< Current limit for instantly shutdown [A]
		public const float LOAD_SHUTDOWN_HIGH = 3.0f;      //!< Current limit for integrated current shutdown [A*S]
		public const float LOAD_BREAK_EVEN = 1.75f;     //!< Current limit for integrated current break even [A*S]

		public const float LOAD_SLOPE_UP = 0.2f;     //!< Current slope when current is above break even [A/S]
		public const float LOAD_SLOPE_DOWN = 0.1f;     //!< Current slope when current is below break even [A/S]

		/*! \endverbatim
         *  \subpage pmloadsd
         *\n
         */

		/*! \page powermanagement
         *
         *  <hr size="1"/>
         *  <b>High Temperature Shutdown</b>
         *\n
         *  <hr size="1"/>
         *  \verbatim
         */

		public const float TEMP_SHUTDOWN_FAIL = 45.0f;     //!< Temperature rise before fail shutdown  [C]
		public const float TEMP_SHUTDOWN_WARNING = 40.0f;     //!< Temperature rise before warning        [C]

		/*! \endverbatim
         *  \subpage pmtempsd
         *\n
         */

		public const int UPDATE_TIME1 = 2;             //!< Update repeat time1  [mS]
		public const int UPDATE_TIME2 = 10;            //!< Update repeat time2  [mS]
		public const int UPDATE_MEMORY = 200;          //!< Update memory size   [mS]
		public const int UPDATE_SDCARD = 500;          //!< Update sdcard size   [mS]
		public const int UPDATE_USBSTICK = 500;          //!< Update usbstick size [mS]


		// Per start of (polution) defines
		public const int MAX_SOUND_DATA_SIZE = 250;
		public const int SOUND_CHUNK = 250;
		public const int SOUND_ADPCM_CHUNK = 125;
		public const int SOUND_MASTER_CLOCK = 132000000;
		public const int SOUND_TONE_MASTER_CLOCK = 1031250;
		public const int SOUND_MIN_FRQ = 250;
		public const int SOUND_MAX_FRQ = 10000;
		public const int SOUND_MAX_LEVEL = 8;
		public const int SOUND_FILE_BUFFER_SIZE = SOUND_CHUNK + 2;// 12.8 mS @ 8KHz
		public const int SOUND_BUFFER_COUNT = 3;
		public const int SOUND_FILE_FORMAT_NORMAL = 0x0100; // RSO-file
		public const int SOUND_FILE_FORMAT_COMPRESSED = 0x0101; // RSO-file compressed
																// Per end of defines


		/*!
         *
         * \n
         */

		public static int VtoC(UWORD V)
		{
			return ((UWORD)((V * ADC_RES) / ADC_REF));
		}

		public static int CtoV(UWORD C)
		{
			return ((UWORD)((C * ADC_REF) / ADC_RES));
		}

		public static int MtoV(UWORD M)
		{
			return ((UWORD)((M * ADC_REF * 100) / (ADC_RES * 52)));
		}

		public const int KB = 1024;

		public const int TYPE_NAME_LENGTH = 11;
		public const int SYMBOL_LENGTH = 4;    //!< Symbol leng th (not including zero)

		public const int TYPE_PARAMETERS = 19; //!< Number of members in the structure above
		public const int MAX_DEVICE_INFOLENGTH = 54; //!< Number of bytes in the structure above (can not be changed)

		public const int ERR_STRING_SIZE = vmERR_STRING_SIZE;       // Inclusive zero termination

		public const int CALPOINTS = 3;

		public const int UART_DATA_LENGTH = MAX_DEVICE_DATALENGTH;
		public const int UART_BUFFER_SIZE = 64;

		public const int UART_PORT_CHANGED = 0x01;    //!< Input port changed
		public const int UART_DATA_READY = 0x08;    //!< Data is ready
		public const int UART_WRITE_REQUEST = 0x10;   //!< Write request

		public const int IIC_DATA_LENGTH = MAX_DEVICE_DATALENGTH;
		public const int IIC_NAME_LENGTH = 8;

		// THIS IS FULLY CUSTOM SHITE
		public const int IIC_SET_CONN = 0;
		public const int IIC_READ_TYPE_INFO = 1;
		public const int IIC_SETUP = 2;
		public const int IIC_SET = 3;

		public const int UART_SET_CONN = 0;
		public const int UART_READ_MODE_INFO = 1;
		public const int UART_NACK_MODE_INFO = 2;
		public const int UART_CLEAR_CHANGED = 3;

		public const int IIC_PORT_CHANGED = 0x01;    //!< Input port changed
		public const int IIC_DATA_READY = 0x08;     //!< Data is ready
		public const int IIC_WRITE_REQUEST = 0x10;     //!< Write request

		public const int TST_PIN_LENGTH = 8;

		public const int TST_UART_LENGTH = UART_BUFFER_SIZE;

		public const int LCD_BUFFER_SIZE = (((LCD_WIDTH + 7) / 8) * LCD_HEIGHT);
		public const int LCD_TOPLINE_SIZE = (((LCD_WIDTH + 7) / 8) * (TOPLINE_HEIGHT + 1));

		public const int PRINTBUFFERSIZE = 160;

		// not a defines but I don't care
		public const int FULL_SPEED = 0;
		public const int HIGH_SPEED = 1;
		#endregion

		#region lms2012.c
		private unsafe static UBYTE* _uiImage = null;
		public unsafe static UBYTE* UiImage
		{
			get
			{
				if (_uiImage != null)
				{
					return _uiImage;
				}
				List<UBYTE> tmp = new List<byte>();
				tmp.AddRange(PROGRAMHeader(0, 1, 0));// VersionInfo,Objects,GlobalBytes
				tmp.AddRange(VMTHREADHeader(0, 1));// OffsetToInstructions,LocalBytes             
				tmp.Add(opFILE); tmp.Add(LC0(LOAD_IMAGE)); tmp.Add(LC0(GUI_SLOT)); tmp.Add(GV0(0)); tmp.Add(LV0(4)); tmp.Add(LV0(0));
				tmp.Add(opPROGRAM_START); tmp.Add(LC0(GUI_SLOT)); tmp.Add(LC0(0)); tmp.Add(LV0(0)); tmp.Add(LC0(0));
				tmp.Add(opOBJECT_END);
				_uiImage = tmp.ToArray().AsPointer();
				return _uiImage;
			}
		}

		public const int IDX_BACK_BUTTON = BACK_BUTTON - 1;
		public const int PRIMDISPATHTABLE_SIZE = 256;

		// not a define but i don't care
		public static Dictionary<int, Action> PrimDispatchTabel = new Dictionary<int, Action>()
		{
			[opERROR] = GH.Lms.Error,
			[opNOP] = GH.Lms.Nop,
			[opPROGRAM_STOP] = GH.Lms.ProgramStop,
			[opPROGRAM_START] = GH.Lms.ProgramStart,
			[opOBJECT_STOP] = GH.Lms.ObjectStop,
			[opOBJECT_START] = GH.Lms.ObjectStart,
			[opOBJECT_TRIG] = GH.Lms.ObjectTrig,
			[opOBJECT_WAIT] = GH.Lms.ObjectWait,
			[opRETURN] = GH.Lms.ObjectReturn,
			[opCALL] = GH.Lms.ObjectCall,
			[opOBJECT_END] = GH.Lms.ObjectEnd,
			[opSLEEP] = GH.Lms.Sleep,
			[opPROGRAM_INFO] = GH.Lms.ProgramInfo,
			[opLABEL] = GH.Lms.DefLabel,
			[opPROBE] = GH.Lms.Probe,
			[opDO] = GH.Lms.Do,
			[opADD8] = GH.Math.cMathAdd8,
			[opADD16] = GH.Math.cMathAdd16,
			[opADD32] = GH.Math.cMathAdd32,
			[opADDF] = GH.Math.cMathAddF,
			[opSUB8] = GH.Math.cMathSub8,
			[opSUB16] = GH.Math.cMathSub16,
			[opSUB32] = GH.Math.cMathSub32,
			[opSUBF] = GH.Math.cMathSubF,
			[opMUL8] = GH.Math.cMathMul8,
			[opMUL16] = GH.Math.cMathMul16,
			[opMUL32] = GH.Math.cMathMul32,
			[opMULF] = GH.Math.cMathMulF,
			[opDIV8] = GH.Math.cMathDiv8,
			[opDIV16] = GH.Math.cMathDiv16,
			[opDIV32] = GH.Math.cMathDiv32,
			[opDIVF] = GH.Math.cMathDivF,
			[opOR8] = GH.Math.cMathOr8,
			[opOR16] = GH.Math.cMathOr16,
			[opOR32] = GH.Math.cMathOr32,

			[opAND8] = GH.Math.cMathAnd8,
			[opAND16] = GH.Math.cMathAnd16,
			[opAND32] = GH.Math.cMathAnd32,

			[opXOR8] = GH.Math.cMathXor8,
			[opXOR16] = GH.Math.cMathXor16,
			[opXOR32] = GH.Math.cMathXor32,

			[opRL8] = GH.Math.cMathRl8,
			[opRL16] = GH.Math.cMathRl16,
			[opRL32] = GH.Math.cMathRl32,
			[opINIT_BYTES] = GH.Move.cMoveInitBytes,
			[opMOVE8_8] = GH.Move.cMove8to8,
			[opMOVE8_16] = GH.Move.cMove8to16,
			[opMOVE8_32] = GH.Move.cMove8to32,
			[opMOVE8_F] = GH.Move.cMove8toF,
			[opMOVE16_8] = GH.Move.cMove16to8,
			[opMOVE16_16] = GH.Move.cMove16to16,
			[opMOVE16_32] = GH.Move.cMove16to32,
			[opMOVE16_F] = GH.Move.cMove16toF,
			[opMOVE32_8] = GH.Move.cMove32to8,
			[opMOVE32_16] = GH.Move.cMove32to16,
			[opMOVE32_32] = GH.Move.cMove32to32,
			[opMOVE32_F] = GH.Move.cMove32toF,
			[opMOVEF_8] = GH.Move.cMoveFto8,
			[opMOVEF_16] = GH.Move.cMoveFto16,
			[opMOVEF_32] = GH.Move.cMoveFto32,
			[opMOVEF_F] = GH.Move.cMoveFtoF,
			[opJR] = GH.Branch.cBranchJr,
			[opJR_FALSE] = GH.Branch.cBranchJrFalse,
			[opJR_TRUE] = GH.Branch.cBranchJrTrue,
			[opJR_NAN] = GH.Branch.cBranchJrNan,
			[opCP_LT8] = GH.Compare.cCompareLt8,
			[opCP_LT16] = GH.Compare.cCompareLt16,
			[opCP_LT32] = GH.Compare.cCompareLt32,
			[opCP_LTF] = GH.Compare.cCompareLtF,
			[opCP_GT8] = GH.Compare.cCompareGt8,
			[opCP_GT16] = GH.Compare.cCompareGt16,
			[opCP_GT32] = GH.Compare.cCompareGt32,
			[opCP_GTF] = GH.Compare.cCompareGtF,
			[opCP_EQ8] = GH.Compare.cCompareEq8,
			[opCP_EQ16] = GH.Compare.cCompareEq16,
			[opCP_EQ32] = GH.Compare.cCompareEq32,
			[opCP_EQF] = GH.Compare.cCompareEqF,
			[opCP_NEQ8] = GH.Compare.cCompareNEq8,
			[opCP_NEQ16] = GH.Compare.cCompareNEq16,
			[opCP_NEQ32] = GH.Compare.cCompareNEq32,
			[opCP_NEQF] = GH.Compare.cCompareNEqF,
			[opCP_LTEQ8] = GH.Compare.cCompareLtEq8,
			[opCP_LTEQ16] = GH.Compare.cCompareLtEq16,
			[opCP_LTEQ32] = GH.Compare.cCompareLtEq32,
			[opCP_LTEQF] = GH.Compare.cCompareLtEqF,
			[opCP_GTEQ8] = GH.Compare.cCompareGtEq8,
			[opCP_GTEQ16] = GH.Compare.cCompareGtEq16,
			[opCP_GTEQ32] = GH.Compare.cCompareGtEq32,
			[opCP_GTEQF] = GH.Compare.cCompareGtEqF,
			[opSELECT8] = GH.Compare.cCompareSelect8,
			[opSELECT16] = GH.Compare.cCompareSelect16,
			[opSELECT32] = GH.Compare.cCompareSelect32,
			[opSELECTF] = GH.Compare.cCompareSelectF,
			[opSYSTEM] = GH.Lms.System_,
			[opPORT_CNV_OUTPUT] = GH.Lms.PortCnvOutput,
			[opPORT_CNV_INPUT] = GH.Lms.PortCnvInput,
			[opNOTE_TO_FREQ] = GH.Lms.NoteToFreq,

			[opJR_LT8] = GH.Branch.cBranchJrLt8,
			[opJR_LT16] = GH.Branch.cBranchJrLt16,
			[opJR_LT32] = GH.Branch.cBranchJrLt32,
			[opJR_LTF] = GH.Branch.cBranchJrLtF,
			[opJR_GT8] = GH.Branch.cBranchJrGt8,
			[opJR_GT16] = GH.Branch.cBranchJrGt16,
			[opJR_GT32] = GH.Branch.cBranchJrGt32,
			[opJR_GTF] = GH.Branch.cBranchJrGtF,
			[opJR_EQ8] = GH.Branch.cBranchJrEq8,
			[opJR_EQ16] = GH.Branch.cBranchJrEq16,
			[opJR_EQ32] = GH.Branch.cBranchJrEq32,
			[opJR_EQF] = GH.Branch.cBranchJrEqF,
			[opJR_NEQ8] = GH.Branch.cBranchJrNEq8,
			[opJR_NEQ16] = GH.Branch.cBranchJrNEq16,
			[opJR_NEQ32] = GH.Branch.cBranchJrNEq32,
			[opJR_NEQF] = GH.Branch.cBranchJrNEqF,
			[opJR_LTEQ8] = GH.Branch.cBranchJrLtEq8,
			[opJR_LTEQ16] = GH.Branch.cBranchJrLtEq16,
			[opJR_LTEQ32] = GH.Branch.cBranchJrLtEq32,
			[opJR_LTEQF] = GH.Branch.cBranchJrLtEqF,
			[opJR_GTEQ8] = GH.Branch.cBranchJrGtEq8,
			[opJR_GTEQ16] = GH.Branch.cBranchJrGtEq16,
			[opJR_GTEQ32] = GH.Branch.cBranchJrGtEq32,
			[opJR_GTEQF] = GH.Branch.cBranchJrGtEqF,
			[opINFO] = GH.Lms.Info,
			[opSTRINGS] = GH.Lms.Strings,
			[opMEMORY_WRITE] = GH.Lms.MemoryWrite,
			[opMEMORY_READ] = GH.Lms.MemoryRead,
			[opUI_FLUSH] = GH.Ui.cUiFlush,
			[opUI_READ] = GH.Ui.cUiRead,
			[opUI_WRITE] = GH.Ui.cUiWrite,
			[opUI_BUTTON] = GH.Ui.cUiButton,
			[opUI_DRAW] = GH.Ui.cUiDraw,
			[opTIMER_WAIT] = GH.Timer.cTimerWait,
			[opTIMER_READY] = GH.Timer.cTimerReady,
			[opTIMER_READ] = GH.Timer.cTimerRead,
			[opBP0] = GH.Lms.BreakPoint,
			[opBP1] = GH.Lms.BreakPoint,
			[opBP2] = GH.Lms.BreakPoint,
			[opBP3] = GH.Lms.BreakPoint,
			[opBP_SET] = GH.Lms.BreakSet,
			[opMATH] = GH.Math.cMath,
			[opRANDOM] = GH.Lms.Random,
			[opTIMER_READ_US] = GH.Timer.cTimerReaduS,
			[opKEEP_ALIVE] = GH.Ui.cUiKeepAlive,
			[opCOM_READ] = GH.Com.cComRead,
			[opCOM_WRITE] = GH.Com.cComWrite,
			[opSOUND] = GH.Sound.cSoundEntry,
			[opSOUND_TEST] = GH.Sound.cSoundTest,
			[opSOUND_READY] = GH.Sound.cSoundReady,
			[opINPUT_SAMPLE] = GH.Input.cInputSample,
			[opINPUT_DEVICE_LIST] = GH.Input.cInputDeviceList,
			[opINPUT_DEVICE] = GH.Input.cInputDevice,
			[opINPUT_READ] = GH.Input.cInputRead,
			[opINPUT_TEST] = GH.Input.cInputTest,
			[opINPUT_READY] = GH.Input.cInputReady,
			[opINPUT_READSI] = GH.Input.cInputReadSi,
			[opINPUT_READEXT] = GH.Input.cInputReadExt,
			[opINPUT_WRITE] = GH.Input.cInputWrite,
			[opOUTPUT_SET_TYPE] = GH.Output.cOutputSetType,
			[opOUTPUT_RESET] = GH.Output.cOutputReset,
			[opOUTPUT_STOP] = GH.Output.cOutputStop,
			[opOUTPUT_POWER] = GH.Output.cOutputPower,
			[opOUTPUT_SPEED] = GH.Output.cOutputSpeed,
			[opOUTPUT_START] = GH.Output.cOutputStart,
			[opOUTPUT_POLARITY] = GH.Output.cOutputPolarity,
			[opOUTPUT_READ] = GH.Output.cOutputRead,
			[opOUTPUT_TEST] = GH.Output.cOutputTest,
			[opOUTPUT_READY] = GH.Output.cOutputReady,
			[opOUTPUT_STEP_POWER] = GH.Output.cOutputStepPower,
			[opOUTPUT_TIME_POWER] = GH.Output.cOutputTimePower,
			[opOUTPUT_STEP_SPEED] = GH.Output.cOutputStepSpeed,
			[opOUTPUT_TIME_SPEED] = GH.Output.cOutputTimeSpeed,
			[opOUTPUT_STEP_SYNC] = GH.Output.cOutputStepSync,
			[opOUTPUT_TIME_SYNC] = GH.Output.cOutputTimeSync,
			[opOUTPUT_CLR_COUNT] = GH.Output.cOutputClrCount,
			[opOUTPUT_GET_COUNT] = GH.Output.cOutputGetCount,
			[opOUTPUT_PRG_STOP] = GH.Output.cOutputPrgStop,
			[opFILE] = GH.Memory.cMemoryFile,
			[opARRAY] = GH.Memory.cMemoryArray,
			[opARRAY_WRITE] = GH.Memory.cMemoryArrayWrite,
			[opARRAY_READ] = GH.Memory.cMemoryArrayRead,
			[opARRAY_APPEND] = GH.Memory.cMemoryArrayAppend,
			[opMEMORY_USAGE] = GH.Memory.cMemoryUsage,
			[opFILENAME] = GH.Memory.cMemoryFileName,
			[opREAD8] = GH.Move.cMoveRead8,
			[opREAD16] = GH.Move.cMoveRead16,
			[opREAD32] = GH.Move.cMoveRead32,
			[opREADF] = GH.Move.cMoveReadF,
			[opWRITE8] = GH.Move.cMoveWrite8,
			[opWRITE16] = GH.Move.cMoveWrite16,
			[opWRITE32] = GH.Move.cMoveWrite32,
			[opWRITEF] = GH.Move.cMoveWriteF,
			[opCOM_READY] = GH.Com.cComReady,
			[opCOM_READDATA] = GH.Lms.Error,
			[opCOM_WRITEDATA] = GH.Lms.Error,
			[opCOM_GET] = GH.Com.cComGet,
			[opCOM_SET] = GH.Com.cComSet,
			[opCOM_TEST] = GH.Com.cComTest,
			[opCOM_REMOVE] = GH.Com.cComRemove,
			[opCOM_WRITEFILE] = GH.Com.cComWriteFile,
			[opMAILBOX_OPEN] = GH.Com.cComOpenMailBox,
			[opMAILBOX_WRITE] = GH.Com.cComWriteMailBox,
			[opMAILBOX_READ] = GH.Com.cComReadMailBox,
			[opMAILBOX_TEST] = GH.Com.cComTestMailBox,
			[opMAILBOX_READY] = GH.Com.cComReadyMailBox,
			[opMAILBOX_CLOSE] = GH.Com.cComCloseMailBox,

			[opTST] = GH.Lms.Tst
		};

		public const int NOTES = 36;

		public static NOTEFREQ[] NoteFreq = new NOTEFREQ[NOTES]
		{
			new NOTEFREQ("C4"  ,  262 ),
			new NOTEFREQ("D4"  ,  294 ),
			new NOTEFREQ("E4"  ,  330 ),
			new NOTEFREQ("F4"  ,  349 ),
			new NOTEFREQ("G4"  ,  392 ),
			new NOTEFREQ("A4"  ,  440 ),
			new NOTEFREQ("B4"  ,  494 ),
			new NOTEFREQ("C5"  ,  523 ),
			new NOTEFREQ("D5"  ,  587 ),
			new NOTEFREQ("E5"  ,  659 ),
			new NOTEFREQ("F5"  ,  698 ),
			new NOTEFREQ("G5"  ,  784 ),
			new NOTEFREQ("A5"  ,  880 ),
			new NOTEFREQ("B5"  ,  988 ),
			new NOTEFREQ("C6"  , 1047 ),
			new NOTEFREQ("D6"  , 1175 ),
			new NOTEFREQ("E6"  , 1319 ),
			new NOTEFREQ("F6"  , 1397 ),
			new NOTEFREQ("G6"  , 1568 ),
			new NOTEFREQ("A6"  , 1760 ),
			new NOTEFREQ("B6"  , 1976 ),

			new NOTEFREQ("C#4" ,  277 ),
			new NOTEFREQ("D#4" ,  311 ),

			new NOTEFREQ("F#4" ,  370 ),
			new NOTEFREQ("G#4" ,  415 ),
			new NOTEFREQ("A#4" ,  466 ),

			new NOTEFREQ("C#5" ,  554 ),
			new NOTEFREQ("D#5" ,  622 ),

			new NOTEFREQ("F#5" ,  740 ),
			new NOTEFREQ("G#5" ,  831 ),
			new NOTEFREQ("A#5" ,  932 ),

			new NOTEFREQ("C#6" , 1109 ),
			new NOTEFREQ("D#6" , 1245 ),

			new NOTEFREQ("F#6" , 1480 ),
			new NOTEFREQ("G#6" , 1661 ),
			new NOTEFREQ("A#6" , 1865 )
		};

		public static DATA8[] ValidChars = new DATA8[]
		{
			0x00,   // 0x00      NUL
			0x00,   // 0x01      SOH
			0x00,   // 0x02      STX
			0x00,   // 0x03      ETX
			0x00,   // 0x04      EOT
			0x00,   // 0x05      ENQ
			0x00,   // 0x06      ACK
			0x00,   // 0x07      BEL
			0x00,   // 0x08      BS
			0x00,   // 0x09      TAB
			0x00,   // 0x0A      LF
			0x00,   // 0x0B      VT
			0x00,   // 0x0C      FF
			0x00,   // 0x0D      CR
			0x00,   // 0x0E      SO
			0x00,   // 0x0F      SI
			0x00,   // 0x10      DLE
			0x00,   // 0x11      DC1
			0x00,   // 0x12      DC2
			0x00,   // 0x13      DC3
			0x00,   // 0x14      DC4
			0x00,   // 0x15      NAK
			0x00,   // 0x16      SYN
			0x00,   // 0x17      ETB
			0x00,   // 0x18      CAN
			0x00,   // 0x19      EM
			0x00,   // 0x1A      SUB
			0x00,   // 0x1B      ESC
			0x00,   // 0x1C      FS
			0x00,   // 0x1D      GS
			0x00,   // 0x1E      RS
			0x00,   // 0x1F      US
			0x12,   // 0x20      (space)
			0x00,   // 0x21      !
			0x00,   // 0x22      "
			0x00,   // 0x23      #
			0x00,   // 0x24      $
			0x00,   // 0x25      %
			0x00,   // 0x26      &
			0x00,   // 0x27      '
			0x00,   // 0x28      (
			0x00,   // 0x29      )
			0x00,   // 0x2A      *
			0x00,   // 0x2B      +
			0x00,   // 0x2C      ,
			0x03,   // 0x2D      -
			0x02,   // 0x2E      .
			0x02,   // 0x2F      /
			0x1F,   // 0x30      0
			0x1F,   // 0x31      1
			0x1F,   // 0x32      2
			0x1F,   // 0x33      3
			0x1F,   // 0x34      4
			0x1F,   // 0x35      5
			0x1F,   // 0x36      6
			0x1F,   // 0x37      7
			0x1F,   // 0x38      8
			0x1F,   // 0x39      9
			0x00,   // 0x3A      :
			0x00,   // 0x3B      ;
			0x00,   // 0x3C      <
			0x00,   // 0x3D      =
			0x00,   // 0x3E      >
			0x00,   // 0x3F      ?
			0x00,   // 0x40      @
			0x1F,   // 0x41      A
			0x1F,   // 0x42      B
			0x1F,   // 0x43      C
			0x1F,   // 0x44      D
			0x1F,   // 0x45      E
			0x1F,   // 0x46      F
			0x1F,   // 0x47      G
			0x1F,   // 0x48      H
			0x1F,   // 0x49      I
			0x1F,   // 0x4A      J
			0x1F,   // 0x4B      K
			0x1F,   // 0x4C      L
			0x1F,   // 0x4D      M
			0x1F,   // 0x4E      N
			0x1F,   // 0x4F      O
			0x1F,   // 0x50      P
			0x1F,   // 0x51      Q
			0x1F,   // 0x52      R
			0x1F,   // 0x53      S
			0x1F,   // 0x54      T
			0x1F,   // 0x55      U
			0x1F,   // 0x56      V
			0x1F,   // 0x57      W
			0x1F,   // 0x58      X
			0x1F,   // 0x59      Y
			0x1F,   // 0x5A      Z
			0x00,   // 0x5B      [
			0x00,   // 0x5C      '\'
			0x00,   // 0x5D      ]
			0x00,   // 0x5E      ^
			0x1F,   // 0x5F      _
			0x00,   // 0x60      `
			0x1F,   // 0x61      a
			0x1F,   // 0x62      b
			0x1F,   // 0x63      c
			0x1F,   // 0x64      d
			0x1F,   // 0x65      e
			0x1F,   // 0x66      f
			0x1F,   // 0x67      g
			0x1F,   // 0x68      h
			0x1F,   // 0x69      i
			0x1F,   // 0x6A      j
			0x1F,   // 0x6B      k
			0x1F,   // 0x6C      l
			0x1F,   // 0x6D      m
			0x1F,   // 0x6E      n
			0x1F,   // 0x6F      o
			0x1F,   // 0x70      p
			0x1F,   // 0x71      q
			0x1F,   // 0x72      r
			0x1F,   // 0x73      s
			0x1F,   // 0x74      t
			0x1F,   // 0x75      u
			0x1F,   // 0x76      v
			0x1F,   // 0x77      w
			0x1F,   // 0x78      x
			0x1F,   // 0x79      y
			0x1F,   // 0x7A      z
			0x00,   // 0x7B      {
			0x00,   // 0x7C      |
			0x00,   // 0x7D      }
			0x02,   // 0x7E      ~
			0x00    // 0x7F      
		};
		#endregion

		#region bytecodes.h
		/*! \page system System Configuration
		 *
		 *  <hr size="1"/>
		 *
		 *  Following defines sets the system configuration and limitations.\n
		 *  Defines with preceding "vm" is visible for the byte code assembler\n
		 *
		 *  \verbatim
		 */

		public const float BYTECODE_VERSION = 0.50f;

		// HARDWARE

		public const int vmOUTPUTS = 4;                  //!< Number of output ports in the system
		public const int vmINPUTS = 4;                  //!< Number of input  ports in the system
		public const int vmBUTTONS = 6;                    //!< Number of buttons in the system
		public const int vmLEDS = 4;                     //!< Number of LEDs in the system

		public const int vmLCD_WIDTH = 178;                   //!< LCD horizontal pixels
		public const int vmLCD_HEIGHT = 128;                   //!< LCD vertical pixels
		public const int vmTOPLINE_HEIGHT = 10;                     //!< Top line vertical pixels
		public const int vmLCD_STORE_LEVELS = 3;                       //!< Store levels

		public const int vmDEFAULT_VOLUME = 100;
		public const int vmDEFAULT_SLEEPMINUTES = 30;

		// SOFTWARE

		public const int vmFG_COLOR = 1;                     //!<  Forground color
		public const int vmBG_COLOR = 0;                      //!<  Background color

		public const int vmCHAIN_DEPT = 4;                     //!< Number of bricks in the USB daisy chain (master + slaves)

		public const int S_IRWXU = 00700;   /* rwx for owner */
		public const int S_IRUSR = 00400;   /* read permission for owner */
		public const int S_IWUSR = 00200;   /* write permission for owner */
		public const int S_IXUSR = 00100;   /* execute/search permission for owner */

		public const int S_IRWXG = 00070;   /* rwx for group */
		public const int S_IRGRP = 00040;/* read permission for group */
		public const int S_IWGRP = 00020;   /* write permission for group */
		public const int S_IXGRP = 00010;   /* execute/search permission for group */

		public const int S_IRWXO = 00007;   /* rwx for other */
		public const int S_IROTH = 00004;   /* read permission for other */
		public const int S_IWOTH = 00002;   /* read permission for other */
		public const int S_IXOTH = 00001;   /* execute/search permission for other */

		public const int FILEPERMISSIONS = (S_IRUSR | S_IWUSR | S_IRGRP | S_IWGRP | S_IROTH | S_IWOTH);
		public const int DIRPERMISSIONS = (S_IRWXU | S_IRWXG | S_IRWXO);
		public const int SYSPERMISSIONS = (S_IRUSR | S_IWUSR | S_IRGRP | S_IROTH);

		public const int vmPATHSIZE = 84;             //!< Max path size excluding trailing forward slash including zero termination
		public const int vmNAMESIZE = 32;              //!< Max name size including zero termination (must be divideable by 4)
		public const int vmEXTSIZE = 5;              //!< Max extension size including dot and zero termination
		public const int vmFILENAMESIZE = 120;              //!< Max filename size including path, name, extension and termination (must be divideable by 4)
		public const int vmMACSIZE = 18;               //!< Max WIFI MAC size including zero termination
		public const int vmIPSIZE = 16;                //!< Max WIFI IP size including zero termination
		public const int vmBTADRSIZE = 13;                 //!< Max bluetooth address size including zero termination

		public const int vmERR_STRING_SIZE = 32;                 // Inclusive zero termination

		public const int vmEVENT_BT_PIN = 1;
		public const int vmEVENT_BT_REQ_CONF = 2;

		public const int vmMAX_VALID_TYPE = 101;                   //!< Highest valid device type

		// FOLDERS

		public const string vmMEMORY_FOLDER = "/mnt/ramdisk";      //!< Folder for non volatile user programs/data
		public const string vmPROGRAM_FOLDER = "../prjs/BrkProg_SAVE";    //!< Folder for On Brick Programming programs
		public const string vmDATALOG_FOLDER = "../prjs/BrkDL_SAVE";      //!< Folder for On Brick Data log files
		public const string vmSDCARD_FOLDER = "../prjs/SD_Card";      //!< Folder for SD card mount
		public const string vmUSBSTICK_FOLDER = "../prjs/USB_Stick";     //!< Folder for USB stick mount

		public const string vmPRJS_DIR = "../prjs";            //!< Project folder
		public const string vmAPPS_DIR = "../apps";            //!< Apps folder
		public const string vmTOOLS_DIR = "../tools";            //!< Tools folder
		public const string vmTMP_DIR = "../tmp";               //!< Temporary folder

		public const string vmSETTINGS_DIR = "./Resources/other";           //!< Folder for non volatile settings

		public const int vmDIR_DEEPT = 127;                        //!< Max directory items allocated including "." and ".."

		// FILES USED IN APPLICATION

		public const string vmLASTRUN_FILE_NAME = "lastrun";                 //!< Last run filename
		public const string vmCALDATA_FILE_NAME = "caldata";                 //!< Calibration data filename

		// FILES USED IN APPS

		public const string vmSLEEP_FILE_NAME = "Sleep";                //!< File used in "Sleep" app to save status
		public const string vmVOLUME_FILE_NAME = "Volume";                //!< File used in "Volume" app to save status
		public const string vmWIFI_FILE_NAME = "WiFi";                //!< File used in "WiFi" app to save status
		public const string vmBLUETOOTH_FILE_NAME = "Bluetooth";                //!< File used in "Bluetooth" app to save status
		public const string vmDETAILS_FILE = "Brick Info";

		// EXTENSIONS

		public const string vmEXT_SOUND = ".rsf";                  //!< Robot Sound File
		public const string vmEXT_GRAPHICS = ".rgf";                   //!< Robot Graphics File
		public const string vmEXT_BYTECODE = ".rbf";                   //!< Robot Byte code File
		public const string vmEXT_TEXT = ".rtf";                    //!< Robot Text File
		public const string vmEXT_DATALOG = ".rdf";                    //!< Robot Datalog File
		public const string vmEXT_PROGRAM = ".rpf";                     //!< Robot Program File
		public const string vmEXT_CONFIG = ".rcf";                     //!< Robot Configuration File
		public const string vmEXT_ARCHIVE = ".raf";                     //!< Robot Archive File

		// NAME LENGTHs

		public const int vmBRICKNAMESIZE = 120;                       //!< Brick name maximal size (including zero termination)
		public const int vmBTPASSKEYSIZE = 7;                        //!< Bluetooth pass key size (including zero termination)
		public const int vmWIFIPASSKEYSIZE = 33;                       //!< WiFi pass key size (including zero termination)

		// VALID CHARACTERS

		public const int vmCHARSET_NAME = 0x01;                     //!< Character set allowed in brick name and raw filenames
		public const int vmCHARSET_FILENAME = 0x02;                     //!< Character set allowed in file names
		public const int vmCHARSET_BTPASSKEY = 0x04;                     //!< Character set allowed in bluetooth pass key
		public const int vmCHARSET_WIFIPASSKEY = 0x08;                     //!< Character set allowed in WiFi pass key
		public const int vmCHARSET_WIFISSID = 0x10;                     //!< Character set allowed in WiFi ssid


		/* \endverbatim */

		public const int DATA8_NAN = ((DATA8)(-128));
		public const int DATA16_NAN = ((DATA16)(-32768));
		public const int DATA32_NAN = ((DATA32)(-0x80000000));
		public const float DATAF_NAN = ((float)0 / (float)0); //(0x7FC00000)

		public const int DATA8_MIN = (-127);
		public const int DATA8_MAX = (127);
		public const int DATA16_MIN = (-32767);
		public const int DATA16_MAX = (32767);
		public const int DATA32_MIN = (-2147483647);
		public const int DATA32_MAX = (2147483647);
		public const int DATAF_MIN = (-2147483647);
		public const int DATAF_MAX = (2147483647);

		// GRAPHICS

		public const int vmPOP3_ABS_X = 16;     //
		public const int vmPOP3_ABS_Y = 50;     //

		public const int vmPOP3_ABS_WARN_ICON_X = 64;
		public const int vmPOP3_ABS_WARN_ICON_X1 = 40;
		public const int vmPOP3_ABS_WARN_ICON_X2 = 72;
		public const int vmPOP3_ABS_WARN_ICON_X3 = 104;
		public const int vmPOP3_ABS_WARN_ICON_Y = 60;
		public const int vmPOP3_ABS_WARN_SPEC_ICON_X = 88;
		public const int vmPOP3_ABS_WARN_SPEC_ICON_Y = 60;
		public const int vmPOP3_ABS_WARN_TEXT_X = 80;
		public const int vmPOP3_ABS_WARN_TEXT_Y = 68;
		public const int vmPOP3_ABS_WARN_YES_X = 72;
		public const int vmPOP3_ABS_WARN_YES_Y = 90;
		public const int vmPOP3_ABS_WARN_LINE_X = 21;
		public const int vmPOP3_ABS_WARN_LINE_Y = 89;
		public const int vmPOP3_ABS_WARN_LINE_ENDX = 155;


		public static UBYTE[] LONGToBytes(ULONG _x) { return new UBYTE[] { (UBYTE)((_x) & 0xFF), (UBYTE)((_x >> 8) & 0xFF), (UBYTE)((_x >> 16) & 0xFF), (UBYTE)((_x >> 24) & 0xFF) }; }
		public static UBYTE[] WORDToBytes(UWORD _x) { return new UBYTE[] { (UBYTE)((_x) & 0xFF), (UBYTE)((_x >> 8) & 0xFF) }; }
		public static UBYTE BYTEToBytes(UBYTE _x) { return (UBYTE)((_x) & 0xFF); }

		public static UBYTE[] PROGRAMHeader(UBYTE VersionInfo, UWORD NumberOfObjects, ULONG GlobalBytes)
		{
			var lst = new List<UBYTE>() { (UBYTE)'L', (UBYTE)'E', (UBYTE)'G', (UBYTE)'O' };
			lst.AddRange(LONGToBytes(0));
			lst.AddRange(WORDToBytes((UWORD)(BYTECODE_VERSION * 100.0)));
			lst.AddRange(WORDToBytes(NumberOfObjects));
			lst.AddRange(LONGToBytes(GlobalBytes));
			return lst.ToArray();
		}

		public static UBYTE[] VMTHREADHeader(ULONG OffsetToInstructions, ULONG LocalBytes)
		{
			var lst = new List<UBYTE>();
			lst.AddRange(LONGToBytes(OffsetToInstructions));
			lst.AddRange(new UBYTE[] { 0, 0, 0, 0 });
			lst.AddRange(LONGToBytes(LocalBytes));
			return lst.ToArray();
		}

		public static UBYTE[] SUBCALLHeader(ULONG OffsetToInstructions, ULONG LocalBytes)
		{
			var lst = new List<UBYTE>();
			lst.AddRange(LONGToBytes(OffsetToInstructions));
			lst.AddRange(new UBYTE[] { 0, 0, 1, 0 });
			lst.AddRange(LONGToBytes(LocalBytes));
			return lst.ToArray();
		}


		public static UBYTE[] BLOCKHeader(ULONG OffsetToInstructions, UWORD OwnerObjectId, UWORD TriggerCount)
		{
			var lst = new List<UBYTE>();
			lst.AddRange(LONGToBytes(OffsetToInstructions));
			lst.AddRange(WORDToBytes(OwnerObjectId));
			lst.AddRange(WORDToBytes(TriggerCount));
			lst.AddRange(LONGToBytes(0));
			return lst.ToArray();
		}


		//        MACROS FOR PRIMITIVES AND SYSTEM CALLS

		public const UBYTE PRIMPAR_SHORT = 0x00;
		public const UBYTE PRIMPAR_LONG = 0x80;

		public const UBYTE PRIMPAR_CONST = 0x00;
		public const UBYTE PRIMPAR_VARIABEL = 0x40;
		public const UBYTE PRIMPAR_LOCAL = 0x00;
		public const UBYTE PRIMPAR_GLOBAL = 0x20;
		public const UBYTE PRIMPAR_HANDLE = 0x10;
		public const UBYTE PRIMPAR_ADDR = 0x08;

		public const UBYTE PRIMPAR_INDEX = 0x1F;
		public const UBYTE PRIMPAR_CONST_SIGN = 0x20;
		public const UBYTE PRIMPAR_VALUE = 0x3F;

		public const UBYTE PRIMPAR_BYTES = 0x07;

		public const UBYTE PRIMPAR_STRING_OLD = 0;
		public const UBYTE PRIMPAR_1_BYTE = 1;
		public const UBYTE PRIMPAR_2_BYTES = 2;
		public const UBYTE PRIMPAR_4_BYTES = 3;
		public const UBYTE PRIMPAR_STRING = 4;
		public const UBYTE PRIMPAR_ARRAY = 5;

		public const UBYTE PRIMPAR_LABEL = 0x20;

		public static UBYTE HND(UBYTE x) { return (UBYTE)(PRIMPAR_HANDLE | x); }
		public static UBYTE ADR(UBYTE x) { return (UBYTE)(PRIMPAR_ADDR | x); }

		public const UBYTE LCS = (PRIMPAR_LONG | PRIMPAR_STRING);

		public static (UBYTE, UBYTE) LAB1(UBYTE v) { return ((PRIMPAR_LONG | PRIMPAR_LABEL), (UBYTE)(v & 0xFF)); }

		public static UBYTE LC0(UBYTE v)								{ return ((UBYTE)((v & PRIMPAR_VALUE) | PRIMPAR_SHORT | PRIMPAR_CONST)); }
		public static (UBYTE, UBYTE) LC1(UBYTE v)						{ return ((PRIMPAR_LONG | PRIMPAR_CONST | PRIMPAR_1_BYTE), (UBYTE)(v & 0xFF)); }
		public static (UBYTE, UBYTE, UBYTE) LC2(UWORD v)				{ return ((PRIMPAR_LONG | PRIMPAR_CONST | PRIMPAR_2_BYTES), (UBYTE)(v & 0xFF), (UBYTE)((v >> 8) & 0xFF)); }
		public static (UBYTE, UBYTE, UBYTE, UBYTE, UBYTE) LC4(ULONG v)  { return ((PRIMPAR_LONG | PRIMPAR_CONST | PRIMPAR_4_BYTES), (UBYTE)((ULONG)v & 0xFF), (UBYTE)(((ULONG)v >> (int)8) & 0xFF), (UBYTE)(((ULONG)v >> (int)16) & 0xFF), (UBYTE)(((ULONG)v >> (int)24) & 0xFF)); }
		public static (UBYTE, UBYTE) LCA(UBYTE h)						{ return ((PRIMPAR_LONG | PRIMPAR_CONST | PRIMPAR_1_BYTE | PRIMPAR_ARRAY), (UBYTE)(h & 0xFF)); }

		public static UBYTE LV0(UBYTE v)								{ return ((UBYTE)((v & PRIMPAR_INDEX) | PRIMPAR_SHORT | PRIMPAR_VARIABEL | PRIMPAR_LOCAL)); }
		public static (UBYTE, UBYTE) LV1(UBYTE v)						{ return ((PRIMPAR_LONG | PRIMPAR_VARIABEL | PRIMPAR_LOCAL | PRIMPAR_1_BYTE), (UBYTE)(v & 0xFF)); }
		public static (UBYTE, UBYTE, UBYTE) LV2(UWORD v)				{ return ((PRIMPAR_LONG | PRIMPAR_VARIABEL | PRIMPAR_LOCAL | PRIMPAR_2_BYTES), (UBYTE)(v & 0xFF), (UBYTE)((v >> 8) & 0xFF)); }
		public static (UBYTE, UBYTE, UBYTE, UBYTE, UBYTE) LV4(ULONG v)  { return ((PRIMPAR_LONG | PRIMPAR_VARIABEL | PRIMPAR_LOCAL | PRIMPAR_4_BYTES), (UBYTE)((ULONG)v & 0xFF), (UBYTE)(((ULONG)v >> (int)8) & 0xFF), (UBYTE)(((ULONG)v >> (int)16) & 0xFF), (UBYTE)(((ULONG)v >> (int)24) & 0xFF)); }
		public static (UBYTE, UBYTE) LVA(UBYTE h)						{ return ((PRIMPAR_LONG | PRIMPAR_VARIABEL | PRIMPAR_LOCAL | PRIMPAR_1_BYTE | PRIMPAR_ARRAY), (UBYTE)(h & 0xFF)); }

		public static UBYTE GV0(UBYTE v)								{ return ((UBYTE)((v & PRIMPAR_INDEX) | PRIMPAR_SHORT | PRIMPAR_VARIABEL | PRIMPAR_GLOBAL)); }
		public static (UBYTE, UBYTE) GV1(UBYTE v)						{ return ((PRIMPAR_LONG | PRIMPAR_VARIABEL | PRIMPAR_GLOBAL | PRIMPAR_1_BYTE), (UBYTE)(v & 0xFF)); }
		public static (UBYTE, UBYTE, UBYTE) GV2(UWORD v)				{ return ((PRIMPAR_LONG | PRIMPAR_VARIABEL | PRIMPAR_GLOBAL | PRIMPAR_2_BYTES), (UBYTE)(v & 0xFF), (UBYTE)((v >> 8) & 0xFF)); }
		public static (UBYTE, UBYTE, UBYTE, UBYTE, UBYTE) GV4(ULONG v)  { return ((PRIMPAR_LONG | PRIMPAR_VARIABEL | PRIMPAR_GLOBAL | PRIMPAR_4_BYTES), (UBYTE)((ULONG)v & 0xFF), (UBYTE)(((ULONG)v >> (int)8) & 0xFF), (UBYTE)(((ULONG)v >> (int)16) & 0xFF), (UBYTE)(((ULONG)v >> (int)24) & 0xFF)); }
		public static (UBYTE, UBYTE) GVA(UBYTE h)						{ return ((PRIMPAR_LONG | PRIMPAR_VARIABEL | PRIMPAR_GLOBAL | PRIMPAR_1_BYTE | PRIMPAR_ARRAY), (UBYTE)(h & 0xFF)); }

		//        MACROS FOR SUB CALLS


		public const int CALLPAR_IN = 0x80;
		public const int CALLPAR_OUT = 0x40;

		public const int CALLPAR_TYPE = 0x07;
		public const int CALLPAR_DATA8 = 0x00;
		public const int CALLPAR_DATA16 = 0x01;
		public const int CALLPAR_DATA32 = 0x02;
		public const int CALLPAR_DATAF = 0x03;
		public const int CALLPAR_STRING = 0x04;

		public const int IN_8 = (CALLPAR_IN | CALLPAR_DATA8);
		public const int IN_16 = (CALLPAR_IN | CALLPAR_DATA16);
		public const int IN_32 = (CALLPAR_IN | CALLPAR_DATA32);
		public const int IN_F = (CALLPAR_IN | CALLPAR_DATAF);
		public const int IN_S = (CALLPAR_IN | CALLPAR_STRING);
		public const int OUT_8 = (CALLPAR_OUT | CALLPAR_DATA8);
		public const int OUT_16 = (CALLPAR_OUT | CALLPAR_DATA16);
		public const int OUT_32 = (CALLPAR_OUT | CALLPAR_DATA32);
		public const int OUT_F = (CALLPAR_OUT | CALLPAR_DATAF);
		public const int OUT_S = (CALLPAR_OUT | CALLPAR_STRING);

		public const int IO_8 = IN_8 | OUT_8;
		public const int IO_16 = IN_16 | OUT_16;
		public const int IO_32 = IN_32 | OUT_32;
		public const int IO_F = IN_F | OUT_F;
		public const int IO_S = IN_S | OUT_S;
		#endregion

		#region bytecodes.c
		public const int MAX_SUBCODES = 33;            //!< Max number of sub codes
		public const int OPCODE_NAMESIZE = 20;        //!< Opcode and sub code name length

		public const int SUBP = 0x01;        //!< Next nibble is sub parameter table no
		public const int PARNO = 0x02;        //!< Defines no of following parameters
		public const int PARLAB = 0x03;         //!< Defines label no
		public const int PARVALUES = 0x04;        //!< Last parameter defines number of values to follow

		public const int PAR = 0x08;    //!< Plain  parameter as below:
		public const int PAR8 = (PAR + 0x00);  //!< DATA8  parameter
		public const int PAR16 = (PAR + 0x01); //!< DATA16 parameter
		public const int PAR32 = (PAR + 0x02); //!< DATA32 parameter
		public const int PARF = (PAR + 0x03);  //!< DATAF  parameter
		public const int PARS = (PAR + 0x04);  //!< DATAS  parameter
		public const int PARV = (PAR + 0x05);  //!< Parameter type variable

		public static Dictionary<int, string> ParTypeNames = new Dictionary<int, string>()
		{
			[DATA_8] = "DATA8",
			[DATA_16] = "DATA16",
			[DATA_32] = "DATA32",
			[DATA_F] = "DATAF",
			[DATA_S] = "STRING",
			[DATA_V] = "UNKNOWN",
		};

		public static Dictionary<int, DATA32> ParMin = new Dictionary<int, DATA32>()
		{
			[DATA_8] = DATA8_MIN,
			[DATA_16] = DATA16_MIN,
			[DATA_32] = DATA32_MIN,
		};

		public static Dictionary<int, DATA32> ParMax = new Dictionary<int, DATA32>()
		{
			[DATA_8] = DATA8_MAX,
			[DATA_16] = DATA16_MAX,
			[DATA_32] = DATA32_MAX,
		};

		// TODO: check it out. wtf is going on here
		//public const byte FILENAME_SUBP = 12;
		// public const byte TST_SUBP = 6;

		public const byte UI_READ_SUBP = 0;
		public const byte UI_WRITE_SUBP = 1;
		public const byte UI_DRAW_SUBP = 2;
		public const byte UI_BUTTON_SUBP = 3;
		public const byte FILE_SUBP = 4;
		public const byte PROGRAM_SUBP = 5;
		public const byte VM_SUBP = 6;
		public const byte STRING_SUBP = 7;
		public const byte COM_READ_SUBP = 8;
		public const byte COM_WRITE_SUBP = 9;
		public const byte SOUND_SUBP = 10;
		public const byte INPUT_SUBP = 11;
		public const byte ARRAY_SUBP = 12;
		public const byte MATH_SUBP = 13;
		public const byte COM_GET_SUBP = 14;
		public const byte COM_SET_SUBP = 15;

		public const byte SUBPS = 16;

		public unsafe static KeyValuePair<OP, OPCODE> OC(OP opCode, byte par1, byte par2, byte par3, byte par4, byte par5, byte par6, byte par7, byte par8)
		{
			return new KeyValuePair<OP, OPCODE>(opCode, new OPCODE()
			{
				Name = (byte*)Enum.GetName(opCode.GetType(), opCode).AsSbytePointer(),
				Pars = ((ULONG)par1) + ((ULONG)par2 << 4) + ((ULONG)par3 << 8) + ((ULONG)par4 << 12) +
						((ULONG)par5 << 16) + ((ULONG)par6 << 20) + ((ULONG)par7 << 24) + ((ULONG)par8 << 28),
			});
		}

		public unsafe static KeyValuePair<byte, SUBCODE> SC(string subCodeName, byte subcode, byte par1, byte par2, byte par3, byte par4, byte par5, byte par6, byte par7, byte par8)
		{
			return new KeyValuePair<byte, SUBCODE>(subcode, new SUBCODE()
			{
				Name = (byte*)subCodeName.AsSbytePointer(),
				Pars = ((ULONG)par1) + ((ULONG)par2 << 4) + ((ULONG)par3 << 8) + ((ULONG)par4 << 12) +
						((ULONG)par5 << 16) + ((ULONG)par6 << 20) + ((ULONG)par7 << 24) + ((ULONG)par8 << 28),
			});
		}

		public static Dictionary<OP, OPCODE> OpCodes = new Dictionary<OP, OPCODE>(new[]
		{
			//    OpCode                  Parameters                                      Unused
			//    VM
			OC(   OP.opERROR,                0,                                              0,0,0,0,0,0,0         ),
			OC(   OP.opNOP,                  0,                                              0,0,0,0,0,0,0         ),
			OC(   OP.opPROGRAM_STOP,         PAR16,                                          0,0,0,0,0,0,0         ),
			OC(   OP.opPROGRAM_START,        PAR16,PAR32,PAR32,PAR8,                         0,0,0,0               ),
			OC(   OP.opOBJECT_STOP,          PAR16,                                          0,0,0,0,0,0,0         ),
			OC(   OP.opOBJECT_START,         PAR16,                                          0,0,0,0,0,0,0         ),
			OC(   OP.opOBJECT_TRIG,          PAR16,                                          0,0,0,0,0,0,0         ),
			OC(   OP.opOBJECT_WAIT,          PAR16,                                          0,0,0,0,0,0,0         ),
			OC(   OP.opRETURN,               0,                                              0,0,0,0,0,0,0         ),
			OC(   OP.opCALL,                 PAR16,PARNO,                                    0,0,0,0,0,0           ),
			OC(   OP.opOBJECT_END,           0,                                              0,0,0,0,0,0,0         ),
			OC(   OP.opSLEEP,                0,                                              0,0,0,0,0,0,0         ),
			OC(   OP.opPROGRAM_INFO,         PAR8, SUBP, PROGRAM_SUBP,                       0,0,0,0,0             ),
			OC(   OP.opLABEL,                PARLAB,                                         0,0,0,0,0,0,0         ),
			OC(   OP.opPROBE,                PAR16,PAR16,PAR32,PAR32,                        0,0,0,0               ),
			OC(   OP.opDO,                   PAR16,PAR32,PAR32,                              0,0,0,0,0             ),
			//    Math
			OC(   OP.opADD8,                 PAR8,PAR8,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opADD16,                PAR16,PAR16,PAR16,                              0,0,0,0,0             ),
			OC(   OP.opADD32,                PAR32,PAR32,PAR32,                              0,0,0,0,0             ),
			OC(   OP.opADDF,                 PARF,PARF,PARF,                                 0,0,0,0,0             ),
			OC(   OP.opSUB8,                 PAR8,PAR8,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opSUB16,                PAR16,PAR16,PAR16,                              0,0,0,0,0             ),
			OC(   OP.opSUB32,                PAR32,PAR32,PAR32,                              0,0,0,0,0             ),
			OC(   OP.opSUBF,                 PARF,PARF,PARF,                                 0,0,0,0,0             ),
			OC(   OP.opMUL8,                 PAR8,PAR8,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opMUL16,                PAR16,PAR16,PAR16,                              0,0,0,0,0             ),
			OC(   OP.opMUL32,                PAR32,PAR32,PAR32,                              0,0,0,0,0             ),
			OC(   OP.opMULF,                 PARF,PARF,PARF,                                 0,0,0,0,0             ),
			OC(   OP.opDIV8,                 PAR8,PAR8,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opDIV16,                PAR16,PAR16,PAR16,                              0,0,0,0,0             ),
			OC(   OP.opDIV32,                PAR32,PAR32,PAR32,                              0,0,0,0,0             ),
			OC(   OP.opDIVF,                 PARF,PARF,PARF,                                 0,0,0,0,0             ),
			//    Logic
			OC(   OP.opOR8,                  PAR8,PAR8,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opOR16,                 PAR16,PAR16,PAR16,                              0,0,0,0,0             ),
			OC(   OP.opOR32,                 PAR32,PAR32,PAR32,                              0,0,0,0,0             ),
			OC(   OP.opAND8,                 PAR8,PAR8,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opAND16,                PAR16,PAR16,PAR16,                              0,0,0,0,0             ),
			OC(   OP.opAND32,                PAR32,PAR32,PAR32,                              0,0,0,0,0             ),
			OC(   OP.opXOR8,                 PAR8,PAR8,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opXOR16,                PAR16,PAR16,PAR16,                              0,0,0,0,0             ),
			OC(   OP.opXOR32,                PAR32,PAR32,PAR32,                              0,0,0,0,0             ),
			OC(   OP.opRL8,                  PAR8,PAR8,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opRL16,                 PAR16,PAR16,PAR16,                              0,0,0,0,0             ),
			OC(   OP.opRL32,                 PAR32,PAR32,PAR32,                              0,0,0,0,0             ),
			//    Move
			OC(   OP.opINIT_BYTES,           PAR8,PAR32,PARVALUES,PAR8,                      0,0,0,0               ),
			OC(   OP.opMOVE8_8,              PAR8,PAR8,                                      0,0,0,0,0,0           ),
			OC(   OP.opMOVE8_16,             PAR8,PAR16,                                     0,0,0,0,0,0           ),
			OC(   OP.opMOVE8_32,             PAR8,PAR32,                                     0,0,0,0,0,0           ),
			OC(   OP.opMOVE8_F,              PAR8,PARF,                                      0,0,0,0,0,0           ),
			OC(   OP.opMOVE16_8,             PAR16,PAR8,                                     0,0,0,0,0,0           ),
			OC(   OP.opMOVE16_16,            PAR16,PAR16,                                    0,0,0,0,0,0           ),
			OC(   OP.opMOVE16_32,            PAR16,PAR32,                                    0,0,0,0,0,0           ),
			OC(   OP.opMOVE16_F,             PAR16,PARF,                                     0,0,0,0,0,0           ),
			OC(   OP.opMOVE32_8,             PAR32,PAR8,                                     0,0,0,0,0,0           ),
			OC(   OP.opMOVE32_16,            PAR32,PAR16,                                    0,0,0,0,0,0           ),
			OC(   OP.opMOVE32_32,            PAR32,PAR32,                                    0,0,0,0,0,0           ),
			OC(   OP.opMOVE32_F,             PAR32,PARF,                                     0,0,0,0,0,0           ),
			OC(   OP.opMOVEF_8,              PARF,PAR8,                                      0,0,0,0,0,0           ),
			OC(   OP.opMOVEF_16,             PARF,PAR16,                                     0,0,0,0,0,0           ),
			OC(   OP.opMOVEF_32,             PARF,PAR32,                                     0,0,0,0,0,0           ),
			OC(   OP.opMOVEF_F,              PARF,PARF,                                      0,0,0,0,0,0           ),
			//    Branch
			OC(   OP.opJR,                   PAR32,                                          0,0,0,0,0,0,0         ),
			OC(   OP.opJR_FALSE,             PAR8,PAR32,                                     0,0,0,0,0,0           ),
			OC(   OP.opJR_TRUE,              PAR8,PAR32,                                     0,0,0,0,0,0           ),
			OC(   OP.opJR_NAN,               PARF,PAR32,                                     0,0,0,0,0,0           ),
			//    Compare
			OC(   OP.opCP_LT8,               PAR8,PAR8,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opCP_LT16,              PAR16,PAR16,PAR8,                               0,0,0,0,0             ),
			OC(   OP.opCP_LT32,              PAR32,PAR32,PAR8,                               0,0,0,0,0             ),
			OC(   OP.opCP_LTF,               PARF,PARF,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opCP_GT8,               PAR8,PAR8,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opCP_GT16,              PAR16,PAR16,PAR8,                               0,0,0,0,0             ),
			OC(   OP.opCP_GT32,              PAR32,PAR32,PAR8,                               0,0,0,0,0             ),
			OC(   OP.opCP_GTF,               PARF,PARF,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opCP_EQ8,               PAR8,PAR8,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opCP_EQ16,              PAR16,PAR16,PAR8,                               0,0,0,0,0             ),
			OC(   OP.opCP_EQ32,              PAR32,PAR32,PAR8,                               0,0,0,0,0             ),
			OC(   OP.opCP_EQF,               PARF,PARF,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opCP_NEQ8,              PAR8,PAR8,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opCP_NEQ16,             PAR16,PAR16,PAR8,                               0,0,0,0,0             ),
			OC(   OP.opCP_NEQ32,             PAR32,PAR32,PAR8,                               0,0,0,0,0             ),
			OC(   OP.opCP_NEQF,              PARF,PARF,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opCP_LTEQ8,             PAR8,PAR8,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opCP_LTEQ16,            PAR16,PAR16,PAR8,                               0,0,0,0,0             ),
			OC(   OP.opCP_LTEQ32,            PAR32,PAR32,PAR8,                               0,0,0,0,0             ),
			OC(   OP.opCP_LTEQF,             PARF,PARF,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opCP_GTEQ8,             PAR8,PAR8,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opCP_GTEQ16,            PAR16,PAR16,PAR8,                               0,0,0,0,0             ),
			OC(   OP.opCP_GTEQ32,            PAR32,PAR32,PAR8,                               0,0,0,0,0             ),
			OC(   OP.opCP_GTEQF,             PARF,PARF,PAR8,                                 0,0,0,0,0             ),
			//    Select
			OC(   OP.opSELECT8,              PAR8,PAR8,PAR8,PAR8,                            0,0,0,0               ),
			OC(   OP.opSELECT16,             PAR8,PAR16,PAR16,PAR16,                         0,0,0,0               ),
			OC(   OP.opSELECT32,             PAR8,PAR32,PAR32,PAR32,                         0,0,0,0               ),
			OC(   OP.opSELECTF,              PAR8,PARF,PARF,PARF,                            0,0,0,0               ),

			OC(   OP.opSYSTEM,               PAR8,PAR32,                                     0,0,0,0,0,0           ),
			OC(   OP.opPORT_CNV_OUTPUT,      PAR32,PAR8,PAR8,PAR8,                           0,0,0,0               ),
			OC(   OP.opPORT_CNV_INPUT,       PAR32,PAR8,PAR8,                                0,0,0,0,0             ),
			OC(   OP.opNOTE_TO_FREQ,         PAR8,PAR16,                                     0,0,0,0,0,0           ),

			//    Branch
			OC(   OP.opJR_LT8,               PAR8,PAR8,PAR32,                                0,0,0,0,0             ),
			OC(   OP.opJR_LT16,              PAR16,PAR16,PAR32,                              0,0,0,0,0             ),
			OC(   OP.opJR_LT32,              PAR32,PAR32,PAR32,                              0,0,0,0,0             ),
			OC(   OP.opJR_LTF,               PARF,PARF,PAR32,                                0,0,0,0,0             ),
			OC(   OP.opJR_GT8,               PAR8,PAR8,PAR32,                                0,0,0,0,0             ),
			OC(   OP.opJR_GT16,              PAR16,PAR16,PAR32,                              0,0,0,0,0             ),
			OC(   OP.opJR_GT32,              PAR32,PAR32,PAR32,                              0,0,0,0,0             ),
			OC(   OP.opJR_GTF,               PARF,PARF,PAR32,                                0,0,0,0,0             ),
			OC(   OP.opJR_EQ8,               PAR8,PAR8,PAR32,                                0,0,0,0,0             ),
			OC(   OP.opJR_EQ16,              PAR16,PAR16,PAR32,                              0,0,0,0,0             ),
			OC(   OP.opJR_EQ32,              PAR32,PAR32,PAR32,                              0,0,0,0,0             ),
			OC(   OP.opJR_EQF,               PARF,PARF,PAR32,                                0,0,0,0,0             ),
			OC(   OP.opJR_NEQ8,              PAR8,PAR8,PAR32,                                0,0,0,0,0             ),
			OC(   OP.opJR_NEQ16,             PAR16,PAR16,PAR32,                              0,0,0,0,0             ),
			OC(   OP.opJR_NEQ32,             PAR32,PAR32,PAR32,                              0,0,0,0,0             ),
			OC(   OP.opJR_NEQF,              PARF,PARF,PAR32,                                0,0,0,0,0             ),
			OC(   OP.opJR_LTEQ8,             PAR8,PAR8,PAR32,                                0,0,0,0,0             ),
			OC(   OP.opJR_LTEQ16,            PAR16,PAR16,PAR32,                              0,0,0,0,0             ),
			OC(   OP.opJR_LTEQ32,            PAR32,PAR32,PAR32,                              0,0,0,0,0             ),
			OC(   OP.opJR_LTEQF,             PARF,PARF,PAR32,                                0,0,0,0,0             ),
			OC(   OP.opJR_GTEQ8,             PAR8,PAR8,PAR32,                                0,0,0,0,0             ),
			OC(   OP.opJR_GTEQ16,            PAR16,PAR16,PAR32,                              0,0,0,0,0             ),
			OC(   OP.opJR_GTEQ32,            PAR32,PAR32,PAR32,                              0,0,0,0,0             ),
			OC(   OP.opJR_GTEQF,             PARF,PARF,PAR32,                                0,0,0,0,0             ),
			//    VM
			OC(   OP.opINFO,                 PAR8,SUBP,VM_SUBP,                              0,0,0,0,0             ),
			OC(   OP.opSTRINGS,              PAR8,SUBP,STRING_SUBP,                          0,0,0,0,0             ),
			OC(   OP.opMEMORY_WRITE,         PAR16,PAR16,PAR32,PAR32,PAR8,                   0,0,0                 ),
			OC(   OP.opMEMORY_READ,          PAR16,PAR16,PAR32,PAR32,PAR8,                   0,0,0                 ),
			//    UI
			OC(   OP.opUI_FLUSH,             0,                                              0,0,0,0,0,0,0         ),
			OC(   OP.opUI_READ,              PAR8,SUBP,UI_READ_SUBP,                         0,0,0,0,0             ),
			OC(   OP.opUI_WRITE,             PAR8,SUBP,UI_WRITE_SUBP,                        0,0,0,0,0             ),
			OC(   OP.opUI_BUTTON,            PAR8,SUBP,UI_BUTTON_SUBP,                       0,0,0,0,0             ),
			OC(   OP.opUI_DRAW,              PAR8,SUBP,UI_DRAW_SUBP,                         0,0,0,0,0             ),
			//    Timer
			OC(   OP.opTIMER_WAIT,           PAR32,PAR32,                                    0,0,0,0,0,0           ),
			OC(   OP.opTIMER_READY,          PAR32,                                          0,0,0,0,0,0,0         ),
			OC(   OP.opTIMER_READ,           PAR32,                                          0,0,0,0,0,0,0         ),
			//    VM
			OC(   OP.opBP0,                  0,                                              0,0,0,0,0,0,0         ),
			OC(   OP.opBP1,                  0,                                              0,0,0,0,0,0,0         ),
			OC(   OP.opBP2,                  0,                                              0,0,0,0,0,0,0         ),
			OC(   OP.opBP3,                  0,                                              0,0,0,0,0,0,0         ),
			OC(   OP.opBP_SET,               PAR16,PAR8,PAR32,                               0,0,0,0,0             ),
			OC(   OP.opMATH,                 PAR8,SUBP,MATH_SUBP,                            0,0,0,0,0             ),
			OC(   OP.opRANDOM,               PAR16,PAR16,PAR16,                              0,0,0,0,0             ),
			OC(   OP.opTIMER_READ_US,        PAR32,                                          0,0,0,0,0,0,0         ),
			OC(   OP.opKEEP_ALIVE,           PAR8,                                           0,0,0,0,0,0,0         ),
			//    Com
			OC(   OP.opCOM_READ,             PAR8,SUBP,COM_READ_SUBP,                        0,0,0,0,0             ),
			OC(   OP.opCOM_WRITE,            PAR8,SUBP,COM_WRITE_SUBP,                       0,0,0,0,0             ),
			//    Sound
			OC(   OP.opSOUND,                PAR8,SUBP,SOUND_SUBP,                           0,0,0,0,0             ),
			OC(   OP.opSOUND_TEST,           PAR8,                                           0,0,0,0,0,0,0         ),
			OC(   OP.opSOUND_READY,          0,                                              0,0,0,0,0,0,0         ),
			//    Input
			OC(   OP.opINPUT_SAMPLE,         PAR32,PAR16,PAR16,PAR8,PAR8,PAR8,PAR8,PARF                            ),
			OC(   OP.opINPUT_DEVICE_LIST,    PAR8,PAR8,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opINPUT_DEVICE,         PAR8,SUBP,INPUT_SUBP,                           0,0,0,0,0             ),
			OC(   OP.opINPUT_READ,           PAR8,PAR8,PAR8,PAR8,PAR8,                       0,0,0                 ),
			OC(   OP.opINPUT_READSI,         PAR8,PAR8,PAR8,PAR8,PARF,                       0,0,0                 ),
			// OC(   OP.opINPUT_TEST,           PAR8,                                           0,0,0,0,0,0,0         ),
			OC(   OP.opINPUT_TEST,           PAR8,PAR8,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opINPUT_READY,          PAR8,PAR8,                                      0,0,0,0,0,0           ),
			OC(   OP.opINPUT_READEXT,        PAR8,PAR8,PAR8,PAR8,PAR8,PARNO,                 0,0                   ),
			OC(   OP.opINPUT_WRITE,          PAR8,PAR8,PAR8,PAR8,                            0,0,0,0               ),
			//    Output
			OC(   OP.opOUTPUT_SET_TYPE,      PAR8,PAR8,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opOUTPUT_RESET,         PAR8,PAR8,                                      0,0,0,0,0,0           ),
			OC(   OP.opOUTPUT_STOP,          PAR8,PAR8,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opOUTPUT_SPEED,         PAR8,PAR8,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opOUTPUT_POWER,         PAR8,PAR8,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opOUTPUT_START,         PAR8,PAR8,                                      0,0,0,0,0,0           ),
			OC(   OP.opOUTPUT_POLARITY,      PAR8,PAR8,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opOUTPUT_READ,          PAR8,PAR8,PAR8,PAR32,                           0,0,0,0               ),
			OC(   OP.opOUTPUT_READY,         PAR8,PAR8,                                      0,0,0,0,0,0           ),
			OC(   OP.opOUTPUT_TEST,          PAR8,PAR8,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opOUTPUT_STEP_POWER,    PAR8,PAR8,PAR8,PAR32,PAR32,PAR32,PAR8,          0                     ),
			OC(   OP.opOUTPUT_TIME_POWER,    PAR8,PAR8,PAR8,PAR32,PAR32,PAR32,PAR8,          0                     ),
			OC(   OP.opOUTPUT_STEP_SPEED,    PAR8,PAR8,PAR8,PAR32,PAR32,PAR32,PAR8,          0                     ),
			OC(   OP.opOUTPUT_TIME_SPEED,    PAR8,PAR8,PAR8,PAR32,PAR32,PAR32,PAR8,          0                     ),
			OC(   OP.opOUTPUT_STEP_SYNC,     PAR8,PAR8,PAR8,PAR16,PAR32,PAR8,                0,0                   ),
			OC(   OP.opOUTPUT_TIME_SYNC,     PAR8,PAR8,PAR8,PAR16,PAR32,PAR8,                0,0                   ),
			OC(   OP.opOUTPUT_CLR_COUNT,     PAR8,PAR8,                                      0,0,0,0,0,0           ),
			OC(   OP.opOUTPUT_GET_COUNT,     PAR8,PAR8,PAR32,                                0,0,0,0,0             ),
			OC(   OP.opOUTPUT_PRG_STOP,      0,                                              0,0,0,0,0,0,0         ),
			//    Memory
			OC(   OP.opFILE,                 PAR8,SUBP,FILE_SUBP,                            0,0,0,0,0             ),
			OC(   OP.opARRAY,                PAR8,SUBP,ARRAY_SUBP,                           0,0,0,0,0             ),
			OC(   OP.opARRAY_WRITE,          PAR16,PAR32,PARV,                               0,0,0,0,0             ),
			OC(   OP.opARRAY_READ,           PAR16,PAR32,PARV,                               0,0,0,0,0             ),
			OC(   OP.opARRAY_APPEND,         PAR16,PARV,                                     0,0,0,0,0,0           ),
			OC(   OP.opMEMORY_USAGE,         PAR32,PAR32,                                    0,0,0,0,0,0           ),
			OC(   OP.opFILENAME,             PAR8,SUBP,ARRAY_SUBP,                        0,0,0,0,0             ),
			//    Move
			OC(   OP.opREAD8,                PAR8,PAR8,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opREAD16,               PAR16,PAR8,PAR16,                               0,0,0,0,0             ),
			OC(   OP.opREAD32,               PAR32,PAR8,PAR32,                               0,0,0,0,0             ),
			OC(   OP.opREADF,                PARF,PAR8,PARF,                                 0,0,0,0,0             ),
			OC(   OP.opWRITE8,               PAR8,PAR8,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opWRITE16,              PAR16,PAR8,PAR16,                               0,0,0,0,0             ),
			OC(   OP.opWRITE32,              PAR32,PAR8,PAR32,                               0,0,0,0,0             ),
			OC(   OP.opWRITEF,               PARF,PAR8,PARF,                                 0,0,0,0,0             ),
			//    Com
			OC(   OP.opCOM_READY,            PAR8,PAR8,                                      0,0,0,0,0,0           ),
			OC(   OP.opCOM_READDATA,         PAR8,PAR8,PAR16,PAR8,                           0,0,0,0               ),
			OC(   OP.opCOM_WRITEDATA,        PAR8,PAR8,PAR16,PAR8,                           0,0,0,0               ),
			OC(   OP.opCOM_GET,              PAR8,SUBP,COM_GET_SUBP,                         0,0,0,0,0             ),
			OC(   OP.opCOM_SET,              PAR8,SUBP,COM_SET_SUBP,                         0,0,0,0,0             ),
			OC(   OP.opCOM_TEST,             PAR8,PAR8,PAR8,                                 0,0,0,0,0             ),
			OC(   OP.opCOM_REMOVE,           PAR8,PAR8,                                      0,0,0,0,0,0           ),
			OC(   OP.opCOM_WRITEFILE,        PAR8,PAR8,PAR8,PAR8,                            0,0,0,0               ),

			OC(   OP.opMAILBOX_OPEN,         PAR8,PAR8,PAR8,PAR8,PAR8,                       0,0,0                 ),
			OC(   OP.opMAILBOX_WRITE,        PAR8,PAR8,PAR8,PAR8,PARNO,                      0,0,0                 ),
			OC(   OP.opMAILBOX_READ,         PAR8,PAR8,PARNO,                                0,0,0,0,0             ),
			OC(   OP.opMAILBOX_TEST,         PAR8,PAR8,                                      0,0,0,0,0,0           ),
			OC(   OP.opMAILBOX_READY,        PAR8,                                           0,0,0,0,0,0,0         ),
			OC(   OP.opMAILBOX_CLOSE,        PAR8,                                           0,0,0,0,0,0,0         ),
			//    Test
			OC(   OP.opTST,                  PAR8,SUBP,VM_SUBP,                             0,0,0,0,0             ),
		});

		public static Dictionary<byte, Dictionary<byte, SUBCODE>> SubCodes = new Dictionary<byte, Dictionary<byte, SUBCODE>>(new[]
		{
			//    ParameterFormat         SubCode                 Parameters                                      Unused
			new KeyValuePair<byte, Dictionary<byte, SUBCODE>>(
				PROGRAM_SUBP,
				new Dictionary<byte, SUBCODE>(new []
				{
					//    VM
					SC(nameof(OBJ_STOP), OBJ_STOP, PAR16, PAR16,                                    0,0,0,0,0,0           ),
					SC(nameof(OBJ_START), OBJ_START, PAR16, PAR16,                                    0,0,0,0,0,0           ),
					SC(nameof(GET_STATUS), GET_STATUS, PAR16, PAR8,                                     0,0,0,0,0,0           ),
					SC(nameof(GET_SPEED), GET_SPEED, PAR16, PAR32,                                    0,0,0,0,0,0           ),
					SC(nameof(GET_PRGRESULT), GET_PRGRESULT, PAR16, PAR8,                                     0,0,0,0,0,0           ),
					SC(nameof(SET_INSTR), SET_INSTR, PAR16,                                          0,0,0,0,0,0,0         ),
				})),

			new KeyValuePair<byte, Dictionary<byte, SUBCODE>>(
				FILE_SUBP,
				new Dictionary<byte, SUBCODE>(new []
				{
					//    Memory
					SC(nameof(OPEN_APPEND), OPEN_APPEND, PAR8, PAR16,                                     0,0,0,0,0,0           ),
					SC(nameof(OPEN_READ), OPEN_READ, PAR8, PAR16, PAR32,                               0,0,0,0,0             ),
					SC(nameof(OPEN_WRITE), OPEN_WRITE, PAR8, PAR16,                                     0,0,0,0,0,0           ),
					SC(nameof(READ_VALUE), READ_VALUE, PAR16, PAR8, PARF,                                0,0,0,0,0             ),
					SC(nameof(WRITE_VALUE), WRITE_VALUE, PAR16, PAR8, PARF, PAR8, PAR8,                      0,0,0                 ),
					SC(nameof(READ_TEXT), READ_TEXT, PAR16, PAR8, PAR16, PAR8,                          0,0,0,0               ),
					SC(nameof(WRITE_TEXT), WRITE_TEXT, PAR16, PAR8, PAR8,                                0,0,0,0,0             ),
					SC(nameof(CLOSE), CLOSE, PAR16,                                          0,0,0,0,0,0,0         ),
					SC(nameof(LOAD_IMAGE), LOAD_IMAGE, PAR16, PAR8, PAR32, PAR32,                         0,0,0,0               ),
					SC(nameof(GET_HANDLE), GET_HANDLE, PAR8, PAR16, PAR8,                                0,0,0,0,0             ),
					SC(nameof(MAKE_FOLDER), MAKE_FOLDER, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(GET_LOG_NAME), GET_LOG_NAME, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(GET_POOL), GET_POOL, PAR32, PAR16, PAR32,                              0,0,0,0,0             ),
					SC(nameof(GET_FOLDERS), GET_FOLDERS, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(GET_SUBFOLDER_NAME), GET_SUBFOLDER_NAME, PAR8, PAR8, PAR8, PAR8,                            0,0,0,0               ),
					SC(nameof(WRITE_LOG), WRITE_LOG, PAR16, PAR32, PAR8, PARF,                          0,0,0,0               ),
					SC(nameof(CLOSE_LOG), CLOSE_LOG, PAR16, PAR8,                                     0,0,0,0,0,0           ),
					SC(nameof(SET_LOG_SYNC_TIME), SET_LOG_SYNC_TIME, PAR32, PAR32,                                    0,0,0,0,0,0           ),
					SC(nameof(DEL_SUBFOLDER), DEL_SUBFOLDER, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(GET_LOG_SYNC_TIME), GET_LOG_SYNC_TIME, PAR32, PAR32,                                    0,0,0,0,0,0           ),
					SC(nameof(GET_IMAGE), GET_IMAGE, PAR8, PAR16, PAR8, PAR32,                          0,0,0,0               ),
					SC(nameof(GET_ITEM), GET_ITEM, PAR8, PAR8, PAR8,                                 0,0,0,0,0             ),
					SC(nameof(GET_CACHE_FILES), GET_CACHE_FILES, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(GET_CACHE_FILE), GET_CACHE_FILE, PAR8, PAR8, PAR8,                                 0,0,0,0,0             ),
					SC(nameof(PUT_CACHE_FILE), PUT_CACHE_FILE, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(DEL_CACHE_FILE), DEL_CACHE_FILE, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(OPEN_LOG), OPEN_LOG, PAR8, PAR32, PAR32, PAR32, PAR32, PAR32, PAR8, PAR16                         ),
					SC(nameof(READ_BYTES), READ_BYTES, PAR16, PAR16, PAR8,                               0,0,0,0,0             ),
					SC(nameof(WRITE_BYTES), WRITE_BYTES, PAR16, PAR16, PAR8,                               0,0,0,0,0             ),
					SC(nameof(REMOVE), REMOVE, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(MOVE), MOVE, PAR8, PAR8,                                      0,0,0,0,0,0           ),
				})),

			new KeyValuePair<byte, Dictionary<byte, SUBCODE>>(
				ARRAY_SUBP,
				new Dictionary<byte, SUBCODE>(new []
				{
					SC(nameof(CREATE8), CREATE8, PAR32, PAR16,                                    0,0,0,0,0,0           ),
					SC(nameof(CREATE16), CREATE16, PAR32, PAR16,                                    0,0,0,0,0,0           ),
					SC(nameof(CREATE32), CREATE32, PAR32, PAR16,                                    0,0,0,0,0,0           ),
					SC(nameof(CREATEF), CREATEF, PAR32, PAR16,                                    0,0,0,0,0,0           ),
					SC(nameof(RESIZE), RESIZE, PAR16, PAR32,                                    0,0,0,0,0,0           ),
					SC(nameof(DELETE), DELETE, PAR16,                                          0,0,0,0,0,0,0         ),
					SC(nameof(FILL), FILL, PAR16, PARV,                                     0,0,0,0,0,0           ),
					SC(nameof(COPY), COPY, PAR16, PAR16,                                    0,0,0,0,0,0           ),
					SC(nameof(INIT8), INIT8, PAR16, PAR32, PAR32, PARVALUES, PAR8,               0,0,0                 ),
					SC(nameof(INIT16), INIT16, PAR16, PAR32, PAR32, PARVALUES, PAR16,              0,0,0                 ),
					SC(nameof(INIT32), INIT32, PAR16, PAR32, PAR32, PARVALUES, PAR32,              0,0,0                 ),
					SC(nameof(INITF), INITF, PAR16, PAR32, PAR32, PARVALUES, PARF,               0,0,0                 ),
					SC(nameof(SIZE), SIZE, PAR16, PAR32,                                    0,0,0,0,0,0           ),
					SC(nameof(READ_CONTENT), READ_CONTENT, PAR16, PAR16, PAR32, PAR32, PAR8,                   0,0,0                 ),
					SC(nameof(WRITE_CONTENT), WRITE_CONTENT, PAR16, PAR16, PAR32, PAR32, PAR8,                   0,0,0                 ),
					SC(nameof(READ_SIZE), READ_SIZE, PAR16, PAR16, PAR32,                              0,0,0,0,0             ),

					SC(nameof(EXIST), EXIST, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(TOTALSIZE), TOTALSIZE, PAR8, PAR32, PAR32,                               0,0,0,0,0             ),
					SC(nameof(SPLIT), SPLIT, PAR8, PAR8, PAR8, PAR8, PAR8,                       0,0,0                 ),
					SC(nameof(MERGE), MERGE, PAR8, PAR8, PAR8, PAR8, PAR8,                       0,0,0                 ),
					SC(nameof(CHECK), CHECK, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(PACK), PACK, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(UNPACK), UNPACK, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(GET_FOLDERNAME), GET_FOLDERNAME, PAR8, PAR8,                                      0,0,0,0,0,0           ),
				})),

			new KeyValuePair<byte, Dictionary<byte, SUBCODE>>(
				VM_SUBP,
				new Dictionary<byte, SUBCODE>(new []
				{
					//    VM
					SC(nameof(SET_ERROR), SET_ERROR, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(GET_ERROR), GET_ERROR, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(ERRORTEXT), ERRORTEXT, PAR8, PAR8, PAR8,                                 0,0,0,0,0             ),

					SC(nameof(GET_VOLUME), GET_VOLUME, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(SET_VOLUME), SET_VOLUME, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(GET_MINUTES), GET_MINUTES, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(SET_MINUTES), SET_MINUTES, PAR8,                                           0,0,0,0,0,0,0         ),

					SC(nameof(TST_OPEN), TST_OPEN,               0,                                              0,0,0,0,0,0,0         ),
					SC(nameof(TST_CLOSE), TST_CLOSE,              0,                                              0,0,0,0,0,0,0         ),
					SC(nameof(TST_READ_PINS), TST_READ_PINS, PAR8, PAR8, PAR8,                                 0,0,0,0,0             ),
					SC(nameof(TST_WRITE_PINS), TST_WRITE_PINS, PAR8, PAR8, PAR8,                                 0,0,0,0,0             ),
					SC(nameof(TST_READ_ADC), TST_READ_ADC, PAR8, PAR16,                                     0,0,0,0,0,0           ),
					SC(nameof(TST_WRITE_UART), TST_WRITE_UART, PAR8, PAR8, PAR8,                                 0,0,0,0,0             ),
					SC(nameof(TST_READ_UART), TST_READ_UART, PAR8, PAR8, PAR8,                                 0,0,0,0,0             ),
					SC(nameof(TST_ENABLE_UART), TST_ENABLE_UART, PAR32,                                          0,0,0,0,0,0,0         ),
					SC(nameof(TST_DISABLE_UART), TST_DISABLE_UART,       0,                                              0,0,0,0,0,0,0         ),
					SC(nameof(TST_ACCU_SWITCH), TST_ACCU_SWITCH, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(TST_BOOT_MODE2), TST_BOOT_MODE2,         0,                                              0,0,0,0,0,0,0         ),
					SC(nameof(TST_POLL_MODE2), TST_POLL_MODE2, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(TST_CLOSE_MODE2), TST_CLOSE_MODE2,        0,                                              0,0,0,0,0,0,0         ),
					SC(nameof(TST_RAM_CHECK), TST_RAM_CHECK, PAR8,                                           0,0,0,0,0,0,0         ),
				})),

			new KeyValuePair<byte, Dictionary<byte, SUBCODE>>(
				STRING_SUBP,
				new Dictionary<byte, SUBCODE>(new []
				{
					SC(nameof(GET_SIZE), GET_SIZE, PAR8, PAR16,                                     0,0,0,0,0,0           ),
					SC(nameof(ADD), ADD, PAR8, PAR8, PAR8,                                 0,0,0,0,0             ),
					SC(nameof(COMPARE), COMPARE, PAR8, PAR8, PAR8,                                 0,0,0,0,0             ),
					SC(nameof(DUPLICATE), DUPLICATE, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(VALUE_TO_STRING), VALUE_TO_STRING, PARF, PAR8, PAR8, PAR8,                            0,0,0,0               ),
					SC(nameof(STRING_TO_VALUE), STRING_TO_VALUE, PAR8, PARF,                                      0,0,0,0,0,0           ),
					SC(nameof(STRIP), STRIP, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(NUMBER_TO_STRING), NUMBER_TO_STRING, PAR16, PAR8, PAR8,                                0,0,0,0,0             ),
					SC(nameof(SUB), SUB, PAR8, PAR8, PAR8,                                 0,0,0,0,0             ),
					SC(nameof(VALUE_FORMATTED), VALUE_FORMATTED, PARF, PAR8, PAR8, PAR8,                            0,0,0,0               ),
					SC(nameof(NUMBER_FORMATTED), NUMBER_FORMATTED, PAR32, PAR8, PAR8, PAR8,                           0,0,0,0               ),
				})),

			new KeyValuePair<byte, Dictionary<byte, SUBCODE>>(
				UI_READ_SUBP,
				new Dictionary<byte, SUBCODE>(new []
				{
					//    UI
					SC(nameof(GET_VBATT), GET_VBATT, PARF,                                           0,0,0,0,0,0,0         ),
					SC(nameof(GET_IBATT), GET_IBATT, PARF,                                           0,0,0,0,0,0,0         ),
					SC(nameof(GET_OS_VERS), GET_OS_VERS, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(GET_EVENT), GET_EVENT, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(GET_TBATT), GET_TBATT, PARF,                                           0,0,0,0,0,0,0         ),
					SC(nameof(GET_IINT), GET_IINT, PARF,                                           0,0,0,0,0,0,0         ),
					SC(nameof(GET_IMOTOR), GET_IMOTOR, PARF,                                           0,0,0,0,0,0,0         ),
					SC(nameof(GET_STRING), GET_STRING, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(KEY), KEY, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(GET_SHUTDOWN), GET_SHUTDOWN, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(GET_WARNING), GET_WARNING, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(GET_LBATT), GET_LBATT, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(GET_ADDRESS), GET_ADDRESS, PAR32,                                          0,0,0,0,0,0,0         ),
					SC(nameof(GET_CODE), GET_CODE, PAR32, PAR32, PAR32, PAR8,                         0,0,0,0               ),
					SC(nameof(TEXTBOX_READ), TEXTBOX_READ, PAR8, PAR32, PAR8, PAR8, PAR16, PAR8,                0,0                   ),
					SC(nameof(GET_HW_VERS), GET_HW_VERS, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(GET_FW_VERS), GET_FW_VERS, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(GET_FW_BUILD), GET_FW_BUILD, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(GET_OS_BUILD), GET_OS_BUILD, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(GET_VERSION), GET_VERSION, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(GET_IP), GET_IP, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(GET_SDCARD), GET_SDCARD, PAR8, PAR32, PAR32,                               0,0,0,0,0             ),
					SC(nameof(GET_USBSTICK), GET_USBSTICK, PAR8, PAR32, PAR32,                               0,0,0,0,0             ),
				})),

			new KeyValuePair<byte, Dictionary<byte, SUBCODE>>(
				UI_WRITE_SUBP,
				new Dictionary<byte, SUBCODE>(new []
				{
					SC(nameof(WRITE_FLUSH), WRITE_FLUSH,            0,                                              0,0,0,0,0,0,0         ),
					SC(nameof(ALLOW_PULSE), ALLOW_PULSE,            PAR8,                                              0,0,0,0,0,0,0         ),
					SC(nameof(SET_PULSE), SET_PULSE,            PAR8,                                              0,0,0,0,0,0,0         ),
					SC(nameof(FLOATVALUE), FLOATVALUE, PARF, PAR8, PAR8,                                 0,0,0,0,0             ),
					SC(nameof(STAMP), STAMP, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(PUT_STRING), PUT_STRING, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(CODE), CODE, PAR8, PAR32,                                     0,0,0,0,0,0           ),
					SC(nameof(DOWNLOAD_END), DOWNLOAD_END,           0,                                              0,0,0,0,0,0,0         ),
					SC(nameof(SCREEN_BLOCK), SCREEN_BLOCK, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(TEXTBOX_APPEND), TEXTBOX_APPEND, PAR8, PAR32, PAR8, PAR8,                           0,0,0,0               ),
					SC(nameof(SET_BUSY), SET_BUSY, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(VALUE8), VALUE8, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(VALUE16), VALUE16, PAR16,                                          0,0,0,0,0,0,0         ),
					SC(nameof(VALUE32), VALUE32, PAR32,                                          0,0,0,0,0,0,0         ),
					SC(nameof(VALUEF), VALUEF, PARF,                                           0,0,0,0,0,0,0         ),
					SC(nameof(INIT_RUN), INIT_RUN,               0,                                              0,0,0,0,0,0,0         ),
					SC(nameof(UPDATE_RUN), UPDATE_RUN,             0,                                              0,0,0,0,0,0,0         ),
					SC(nameof(LED), LED, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(POWER), POWER, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(TERMINAL), TERMINAL, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(GRAPH_SAMPLE), GRAPH_SAMPLE,           0,                                              0,0,0,0,0,0,0         ),
					SC(nameof(SET_TESTPIN), SET_TESTPIN, PAR8,                                           0,0,0,0,0,0,0         ),
				})),

			new KeyValuePair<byte, Dictionary<byte, SUBCODE>>(
				UI_DRAW_SUBP,
				new Dictionary<byte, SUBCODE>(new []
				{
					SC(nameof(UPDATE), UPDATE,                 0,                                              0,0,0,0,0,0,0         ),
					SC(nameof(CLEAN), CLEAN,                  0,                                              0,0,0,0,0,0,0         ),
					SC(nameof(FILLRECT), FILLRECT, PAR8, PAR16, PAR16, PAR16, PAR16,                   0,0,0                 ),
					SC(nameof(RECT), RECT, PAR8, PAR16, PAR16, PAR16, PAR16,                   0,0,0                 ),
					SC(nameof(PIXEL), PIXEL, PAR8, PAR16, PAR16,                               0,0,0,0,0             ),
					SC(nameof(LINE), LINE, PAR8, PAR16, PAR16, PAR16, PAR16,                   0,0,0                 ),
					SC(nameof(CIRCLE), CIRCLE, PAR8, PAR16, PAR16, PAR16,                         0,0,0,0               ),
					SC(nameof(TEXT), TEXT, PAR8, PAR16, PAR16, PAR8,                          0,0,0,0               ),
					SC(nameof(ICON), ICON, PAR8, PAR16, PAR16, PAR8, PAR8,                     0,0,0                 ),
					SC(nameof(PICTURE), PICTURE, PAR8, PAR16, PAR16, PAR32,                         0,0,0,0               ),
					SC(nameof(VALUE), VALUE, PAR8, PAR16, PAR16, PARF, PAR8, PAR8,                0,0                   ),
					SC(nameof(NOTIFICATION), NOTIFICATION, PAR8, PAR16, PAR16, PAR8, PAR8, PAR8, PAR8, PAR8                             ),
					SC(nameof(QUESTION), QUESTION, PAR8, PAR16, PAR16, PAR8, PAR8, PAR8, PAR8, PAR8                             ),
					SC(nameof(KEYBOARD), KEYBOARD, PAR8, PAR16, PAR16, PAR8, PAR8, PAR8, PAR8, PAR8                             ),
					SC(nameof(BROWSE), BROWSE, PAR8, PAR16, PAR16, PAR16, PAR16, PAR8, PAR8, PAR8                           ),
					SC(nameof(VERTBAR), VERTBAR, PAR8, PAR16, PAR16, PAR16, PAR16, PAR16, PAR16, PAR16                        ),
					SC(nameof(INVERSERECT), INVERSERECT, PAR16, PAR16, PAR16, PAR16,                        0,0,0,0               ),
					SC(nameof(SELECT_FONT), SELECT_FONT, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(TOPLINE), TOPLINE, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(FILLWINDOW), FILLWINDOW, PAR8, PAR16, PAR16,                               0,0,0,0,0             ),
					SC(nameof(SCROLL), SCROLL, PAR16,                                          0,0,0,0,0,0,0         ),
					SC(nameof(DOTLINE), DOTLINE, PAR8, PAR16, PAR16, PAR16, PAR16, PAR16, PAR16,       0                     ),
					SC(nameof(VIEW_VALUE), VIEW_VALUE, PAR8, PAR16, PAR16, PARF, PAR8, PAR8,                0,0                   ),
					SC(nameof(VIEW_UNIT), VIEW_UNIT, PAR8, PAR16, PAR16, PARF, PAR8, PAR8, PAR8, PAR8                             ),
					SC(nameof(FILLCIRCLE), FILLCIRCLE, PAR8, PAR16, PAR16, PAR16,                         0,0,0,0               ),
					SC(nameof(STORE), STORE, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(RESTORE), RESTORE, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(ICON_QUESTION), ICON_QUESTION, PAR8, PAR16, PAR16, PAR8, PAR32,                    0,0,0                 ),
					SC(nameof(BMPFILE), BMPFILE, PAR8, PAR16, PAR16, PAR8,                          0,0,0,0               ),
					SC(nameof(GRAPH_SETUP), GRAPH_SETUP, PAR16, PAR16, PAR16, PAR16, PAR8, PAR16, PAR16, PAR16                        ),
					SC(nameof(GRAPH_DRAW), GRAPH_DRAW, PAR8, PARF, PARF, PARF, PARF,                       0,0,0                 ),
					SC(nameof(POPUP), POPUP, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(TEXTBOX), TEXTBOX, PAR16, PAR16, PAR16, PAR16, PAR8, PAR32, PAR8, PAR8                          ),
				})),

			new KeyValuePair<byte, Dictionary<byte, SUBCODE>>(
				UI_BUTTON_SUBP,
				new Dictionary<byte, SUBCODE>(new []
				{
					SC(nameof(SHORTPRESS), SHORTPRESS, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(LONGPRESS), LONGPRESS, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(FLUSH), FLUSH,                  0,                                              0,0,0,0,0,0,0         ),
					SC(nameof(WAIT_FOR_PRESS), WAIT_FOR_PRESS,         0,                                              0,0,0,0,0,0,0         ),
					SC(nameof(PRESS), PRESS, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(RELEASE), RELEASE, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(GET_HORZ), GET_HORZ, PAR16,                                          0,0,0,0,0,0,0         ),
					SC(nameof(GET_VERT), GET_VERT, PAR16,                                          0,0,0,0,0,0,0         ),
					SC(nameof(PRESSED), PRESSED, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(SET_BACK_BLOCK), SET_BACK_BLOCK, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(GET_BACK_BLOCK), GET_BACK_BLOCK, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(TESTSHORTPRESS), TESTSHORTPRESS, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(TESTLONGPRESS), TESTLONGPRESS, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(GET_BUMBED), GET_BUMBED, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(GET_CLICK), GET_CLICK, PAR8,                                           0,0,0,0,0,0,0         ),
				})),

			new KeyValuePair<byte, Dictionary<byte, SUBCODE>>(
				COM_READ_SUBP,
				new Dictionary<byte, SUBCODE>(new []
				{
					//    Com
					SC(nameof(COMMAND), COMMAND, PAR32, PAR32, PAR32, PAR8,                         0,0,0,0               ),
				})),
			new KeyValuePair<byte, Dictionary<byte, SUBCODE>>(
				COM_WRITE_SUBP,
				new Dictionary<byte, SUBCODE>(new []
				{
					SC(nameof(REPLY), REPLY, PAR32, PAR32, PAR8,                               0,0,0,0,0             ),
				})),

			new KeyValuePair<byte, Dictionary<byte, SUBCODE>>(
				SOUND_SUBP,
				new Dictionary<byte, SUBCODE>(new []
				{
					//    Sound
					SC(nameof(BREAK), BREAK,                  0,                                              0,0,0,0,0,0,0         ),
					SC(nameof(TONE), TONE, PAR8, PAR16, PAR16,                               0,0,0,0,0             ),
					SC(nameof(PLAY), PLAY, PAR8, PARS,                                      0,0,0,0,0,0           ),
					SC(nameof(REPEAT), REPEAT, PAR8, PARS,                                      0,0,0,0,0,0           ),
					SC(nameof(SERVICE), SERVICE,                0,                                              0,0,0,0,0,0,0         ),
				})),

			new KeyValuePair<byte, Dictionary<byte, SUBCODE>>(
				INPUT_SUBP,
				new Dictionary<byte, SUBCODE>(new []
				{
					//    Input
					SC(nameof(GET_TYPEMODE), GET_TYPEMODE, PAR8, PAR8, PAR8, PAR8,                            0,0,0,0               ),
					SC(nameof(GET_CONNECTION), GET_CONNECTION, PAR8, PAR8, PAR8,                                 0,0,0,0,0             ),
					SC(nameof(GET_NAME), GET_NAME, PAR8, PAR8, PAR8, PAR8,                            0,0,0,0               ),
					SC(nameof(GET_SYMBOL), GET_SYMBOL, PAR8, PAR8, PAR8, PAR8,                            0,0,0,0               ),
					SC(nameof(GET_FORMAT), GET_FORMAT, PAR8, PAR8, PAR8, PAR8, PAR8, PAR8,                  0,0                   ),
					SC(nameof(GET_RAW), GET_RAW, PAR8, PAR8, PAR32,                                0,0,0,0,0             ),
					SC(nameof(GET_MODENAME), GET_MODENAME, PAR8, PAR8, PAR8, PAR8, PAR8,                       0,0,0                 ),
					SC(nameof(SET_RAW), SET_RAW, PAR8, PAR8, PAR8, PAR32,                           0,0,0,0               ),
					SC(nameof(GET_FIGURES), GET_FIGURES, PAR8, PAR8, PAR8, PAR8,                            0,0,0,0               ),
					SC(nameof(GET_CHANGES), GET_CHANGES, PAR8, PAR8, PARF,                                 0,0,0,0,0             ),
					SC(nameof(CLR_CHANGES), CLR_CHANGES, PAR8, PAR8,0,                                    0,0,0,0,0             ),
					SC(nameof(READY_PCT), READY_PCT, PAR8, PAR8, PAR8, PAR8, PARNO,                      0,0,0                 ),
					SC(nameof(READY_RAW), READY_RAW, PAR8, PAR8, PAR8, PAR8, PARNO,                      0,0,0                 ),
					SC(nameof(READY_SI), READY_SI, PAR8, PAR8, PAR8, PAR8, PARNO,                      0,0,0                 ),
					SC(nameof(GET_MINMAX), GET_MINMAX, PAR8, PAR8, PARF, PARF,                            0,0,0,0               ),
					SC(nameof(CAL_MINMAX), CAL_MINMAX, PAR8, PAR8, PAR32, PAR32,                          0,0,0,0               ),
					SC(nameof(CAL_DEFAULT), CAL_DEFAULT, PAR8, PAR8,0,                                    0,0,0,0,0             ),
					SC(nameof(CAL_MIN), CAL_MIN, PAR8, PAR8, PAR32,                                0,0,0,0,0             ),
					SC(nameof(CAL_MAX), CAL_MAX, PAR8, PAR8, PAR32,                                0,0,0,0,0             ),
					SC(nameof(GET_BUMPS), GET_BUMPS, PAR8, PAR8, PARF,                                 0,0,0,0,0             ),
					SC(nameof(SETUP), SETUP, PAR8, PAR8, PAR8, PAR16, PAR8, PAR8, PAR8, PAR8                              ),
					SC(nameof(CLR_ALL), CLR_ALL, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(STOP_ALL), STOP_ALL, PAR8,                                           0,0,0,0,0,0,0         ),
				})),

			new KeyValuePair<byte, Dictionary<byte, SUBCODE>>(
				MATH_SUBP,
				new Dictionary<byte, SUBCODE>(new []
				{
					//    Math
					SC(nameof(EXP), EXP, PARF, PARF,                                      0,0,0,0,0,0           ),
					SC(nameof(MOD), MOD, PARF, PARF, PARF,                                 0,0,0,0,0             ),
					SC(nameof(FLOOR), FLOOR, PARF, PARF,                                      0,0,0,0,0,0           ),
					SC(nameof(CEIL), CEIL, PARF, PARF,                                      0,0,0,0,0,0           ),
					SC(nameof(ROUND), ROUND, PARF, PARF,                                      0,0,0,0,0,0           ),
					SC(nameof(ABS), ABS, PARF, PARF,                                      0,0,0,0,0,0           ),
					SC(nameof(NEGATE), NEGATE, PARF, PARF,                                      0,0,0,0,0,0           ),
					SC(nameof(SQRT), SQRT, PARF, PARF,                                      0,0,0,0,0,0           ),
					SC(nameof(LOG), LOG, PARF, PARF,                                      0,0,0,0,0,0           ),
					SC(nameof(LN), LN, PARF, PARF,                                      0,0,0,0,0,0           ),
					SC(nameof(SIN), SIN, PARF, PARF,                                      0,0,0,0,0,0           ),
					SC(nameof(COS), COS, PARF, PARF,                                      0,0,0,0,0,0           ),
					SC(nameof(TAN), TAN, PARF, PARF,                                      0,0,0,0,0,0           ),
					SC(nameof(ASIN), ASIN, PARF, PARF,                                      0,0,0,0,0,0           ),
					SC(nameof(ACOS), ACOS, PARF, PARF,                                      0,0,0,0,0,0           ),
					SC(nameof(ATAN), ATAN, PARF, PARF,                                      0,0,0,0,0,0           ),
					SC(nameof(MOD8), MOD8, PAR8, PAR8, PAR8,                                 0,0,0,0,0             ),
					SC(nameof(MOD16), MOD16, PAR16, PAR16, PAR16,                              0,0,0,0,0             ),
					SC(nameof(MOD32), MOD32, PAR32, PAR32, PAR32,                              0,0,0,0,0             ),
					SC(nameof(POW), POW, PARF, PARF, PARF,                                 0,0,0,0,0             ),
					SC(nameof(TRUNC), TRUNC, PARF, PAR8, PARF,                                 0,0,0,0,0             ),
				})),

			new KeyValuePair<byte, Dictionary<byte, SUBCODE>>(
				COM_GET_SUBP,
				new Dictionary<byte, SUBCODE>(new []
				{
					//    ComGet
					SC(nameof(GET_ON_OFF), GET_ON_OFF, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(GET_VISIBLE), GET_VISIBLE, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(GET_RESULT), GET_RESULT, PAR8, PAR8, PAR8,                                 0,0,0,0,0             ),
					SC(nameof(GET_PIN), GET_PIN, PAR8, PAR8, PAR8, PAR8,                            0,0,0,0               ),
					SC(nameof(SEARCH_ITEMS), SEARCH_ITEMS, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(SEARCH_ITEM), SEARCH_ITEM, PAR8, PAR8, PAR8, PAR8, PAR8, PAR8, PAR8, PAR8                               ),
					SC(nameof(FAVOUR_ITEMS), FAVOUR_ITEMS, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(FAVOUR_ITEM), FAVOUR_ITEM, PAR8, PAR8, PAR8, PAR8, PAR8, PAR8, PAR8,             0                     ),
					SC(nameof(GET_ID), GET_ID, PAR8, PAR8, PAR8,                                 0,0,0,0,0             ),
					SC(nameof(GET_BRICKNAME), GET_BRICKNAME, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(GET_NETWORK), GET_NETWORK, PAR8, PAR8, PAR8, PAR8, PAR8,                       0,0,0                 ),
					SC(nameof(GET_PRESENT), GET_PRESENT, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(GET_ENCRYPT), GET_ENCRYPT, PAR8, PAR8, PAR8,                                 0,0,0,0,0             ),
					SC(nameof(CONNEC_ITEMS), CONNEC_ITEMS, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(CONNEC_ITEM), CONNEC_ITEM, PAR8, PAR8, PAR8, PAR8, PAR8,                       0,0,0                 ),
					SC(nameof(GET_INCOMING), GET_INCOMING, PAR8, PAR8, PAR8, PAR8,                            0,0,0,0               ),
					SC(nameof(GET_MODE2), GET_MODE2, PAR8, PAR8,                                      0,0,0,0,0,0           ),
				})),

			new KeyValuePair<byte, Dictionary<byte, SUBCODE>>(
				COM_SET_SUBP,
				new Dictionary<byte, SUBCODE>(new []
				{
					//    ComSet
					SC(nameof(SET_ON_OFF), SET_ON_OFF, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(SET_VISIBLE), SET_VISIBLE, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(SET_SEARCH), SET_SEARCH, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(SET_PIN), SET_PIN, PAR8, PAR8, PAR8,                                 0,0,0,0,0             ),
					SC(nameof(SET_PASSKEY), SET_PASSKEY, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(SET_CONNECTION), SET_CONNECTION, PAR8, PAR8, PAR8,                                 0,0,0,0,0             ),
					SC(nameof(SET_BRICKNAME), SET_BRICKNAME, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(SET_MOVEUP), SET_MOVEUP, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(SET_MOVEDOWN), SET_MOVEDOWN, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(SET_ENCRYPT), SET_ENCRYPT, PAR8, PAR8, PAR8,                                 0,0,0,0,0             ),
					SC(nameof(SET_SSID), SET_SSID, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(SET_MODE2), SET_MODE2, PAR8, PAR8,                                      0,0,0,0,0,0           ),
				})),
		});
		#endregion

		#region c_sound.h
		public const int STEP_SIZE_TABLE_ENTRIES = 89;
		public const int INDEX_TABLE_ENTRIES = 16;

		// Percentage to SoundLevel -
		// Adjust the percentage, if non-linear SPL response is needed

		public const int SND_LEVEL_1 = 13;  // 13% (12.5)
		public const int SND_LEVEL_2 = 25;  // 25%
		public const int SND_LEVEL_3 = 38;  // 38% (37.5)
		public const int SND_LEVEL_4 = 50;  // 50%
		public const int SND_LEVEL_5 = 63;  // 63% (62.5)
		public const int SND_LEVEL_6 = 75;  // 75%
		public const int SND_LEVEL_7 = 88;  // 88% (87.5)

		public const int TONE_LEVEL_1 = 8;  //  8%
		public const int TONE_LEVEL_2 = 16;  // 16%
		public const int TONE_LEVEL_3 = 24;  // 24%
		public const int TONE_LEVEL_4 = 32;  // 32%
		public const int TONE_LEVEL_5 = 40;  // 40%
		public const int TONE_LEVEL_6 = 48;  // 48%
		public const int TONE_LEVEL_7 = 56;  // 56%
		public const int TONE_LEVEL_8 = 64;  // 64%
		public const int TONE_LEVEL_9 = 72;  // 72%
		public const int TONE_LEVEL_10 = 80;  // 80%
		public const int TONE_LEVEL_11 = 88;  // 88%
		public const int TONE_LEVEL_12 = 96;  // 96%

		public static SWORD[] StepSizeTable = new SWORD[STEP_SIZE_TABLE_ENTRIES]
		{
				7, 8, 9, 10, 11, 12, 13, 14, 16, 17,
				19, 21, 23, 25, 28, 31, 34, 37, 41, 45,
				50, 55, 60, 66, 73, 80, 88, 97, 107, 118,
				130, 143, 157, 173, 190, 209, 230, 253, 279, 307,
				337, 371, 408, 449, 494, 544, 598, 658, 724, 796,
				876, 963, 1060, 1166, 1282, 1411, 1552, 1707, 1878, 2066,
				2272, 2499, 2749, 3024, 3327, 3660, 4026, 4428, 4871, 5358,
				5894, 6484, 7132, 7845, 8630, 9493, 10442, 11487, 12635, 13899,
				15289, 16818, 18500, 20350, 22385, 24623, 27086, 29794, 32767
		};

		public static SWORD[] IndexTable = new SWORD[INDEX_TABLE_ENTRIES]
		{
				-1, -1, -1, -1, 2, 4, 6, 8,
				-1, -1, -1, -1, 2, 4, 6, 8
		};

		public const int FILEFORMAT_RAW_SOUND = 0x0100;
		public const int FILEFORMAT_ADPCM_SOUND = 0x0101;
		public const int SOUND_MODE_ONCE = 0x00;
		public const int SOUND_LOOP = 0x01;
		public const int SOUND_ADPCM_INIT_VALPREV = 0x7F;
		public const int SOUND_ADPCM_INIT_INDEX = 20;
		#endregion

		#region c_memory.h
		public const int POOL_TYPE_MEMORY = 0;
		public const int POOL_TYPE_FILE = 1;
		#endregion

		#region c_memory.c
		public const int OPEN_FOR_WRITE = 1;
		public const int OPEN_FOR_APPEND = 2;
		public const int OPEN_FOR_READ = 3;
		public const int OPEN_FOR_LOG = 4;

		public const int SORT_NONE = 0;
		public const int SORT_PRJS = 1;
		public const int SORT_APPS = 2;
		public const int SORT_TOOLS = 3;

		public const int SORT_TYPES = 4;

		public static DATA8[] NoOfFavourites = new DATA8[SORT_TYPES]
		{
			0,  // -
			6,  // Prjs
			5,  // Apps
			8   // Tools
		};

		public static Dictionary<DATA8, DATA8> FavouriteExts = new Dictionary<sbyte, DATA8>()
		{
			[TYPE_BYTECODE] = 1,
			[TYPE_SOUND] = 2,
			[TYPE_GRAPHICS] = 3,
			[TYPE_TEXT] = 4,
		};

		public static string[,] pFavourites = new string[SORT_TYPES, 8]
		{	// Priority
			//  0             1                 2                 3                 4
			{ null,         null,             null,             null,             null,                 null,         null,                 null,},
			{ "",           "BrkProg_SAVE",   "BrkDL_SAVE",     "SD_Card",        "USB_Stick",          "TEST",       null,                 null },
			{ "Port View",  "Motor Control",  "IR Control",     "Brick Program",  "Brick Datalog",      null,         null,                 null },
			{ "Volume",     "Sleep",          "Bluetooth",      "WiFi",           "Brick Info",         "Test",       "Performance",        "Debug" }
		};

		// not really defined there
		public const int DT_LNK = 1; // the same shite as dir
		public const int DT_DIR = 1;
		public const int DT_FILE = 3;
		public const int DT_REG = 3; // the same shite as file
		#endregion

		#region c_com.h
		public const int SYSTEM_COMMAND_REPLY = 0x01;    //  System command, reply required
		public const int SYSTEM_COMMAND_NO_REPLY = 0x81;  //  System command, reply not required

		public const int BEGIN_DOWNLOAD = 0x92;   //  Begin file down load
		public const int CONTINUE_DOWNLOAD = 0x93;   //  Continue file down load
		public const int BEGIN_UPLOAD = 0x94;   //  Begin file upload
		public const int CONTINUE_UPLOAD = 0x95;   //  Continue file upload
		public const int BEGIN_GETFILE = 0x96;   //  Begin get bytes from a file (while writing to the file)
		public const int CONTINUE_GETFILE = 0x97;   //  Continue get byte from a file (while writing to the file)
		public const int CLOSE_FILEHANDLE = 0x98;    //  Close file handle
		public const int LIST_FILES = 0x99;    //  List files
		public const int CONTINUE_LIST_FILES = 0x9A;    //  Continue list files
		public const int CREATE_DIR = 0x9B;   //  Create directory
		public const int DELETE_FILE = 0x9C;   //  Delete
		public const int LIST_OPEN_HANDLES = 0x9D;  //  List handles
		public const int WRITEMAILBOX = 0x9E;   //  Write to mailbox
		public const int BLUETOOTHPIN = 0x9F;  //  Transfer trusted pin code to brick
		public const int ENTERFWUPDATE = 0xA0;  //  Restart the brick in Firmware update mode
		public const int SETBUNDLEID = 0xA1;  //  Set Bundle ID for mode 2
		public const int SETBUNDLESEEDID = 0xA2;  //  Set bundle seed ID for mode 2

		public const int SYSTEM_REPLY = 0x03;   //  System command reply
		public const int SYSTEM_REPLY_ERROR = 0x05;   //  System command reply error

		// SYSTEM command return codes
		public const int SUCCESS = 0x00;
		public const int UNKNOWN_HANDLE = 0x01;
		public const int HANDLE_NOT_READY = 0x02;
		public const int CORRUPT_FILE = 0x03;
		public const int NO_HANDLES_AVAILABLE = 0x04;
		public const int NO_PERMISSION = 0x05;
		public const int ILLEGAL_PATH = 0x06;
		public const int FILE_EXITS = 0x07;
		public const int END_OF_FILE = 0x08;
		public const int SIZE_ERROR = 0x09;
		public const int UNKNOWN_ERROR = 0x0A;
		public const int ILLEGAL_FILENAME = 0x0B;
		public const int ILLEGAL_CONNECTION = 0x0C;

		public const int DIRECT_COMMAND_REPLY = 0x00;   //  Direct command, reply required
		public const int DIRECT_COMMAND_NO_REPLY = 0x80;   //  Direct command, reply not required

		public const int DIRECT_REPLY = 0x02;   //  Direct command reply
		public const int DIRECT_REPLY_ERROR = 0x04;  //  Direct command reply error

		public const int DIR_CMD_REPLY_WITH_BUSY = 0x0F;  //  Direct command, reply required
		public const int DIR_CMD_NO_REPLY_WITH_BUSY = 0x8F;  //  Direct command, reply not required

		// it was enum
		public const int USBDEV = 0;
		public const int USBHOST = 1;
		public const int BTSLAVE = 2;
		public const int BTMASTER1 = 3;
		public const int BTMASTER2 = 4;
		public const int BTMASTER3 = 5;
		public const int BTMASTER4 = 6;
		public const int BTMASTER5 = 7;
		public const int BTMASTER6 = 8;
		public const int BTMASTER7 = 9;
		public const int WIFI = 10;
		public const int NO_OF_CHS = 11;

		public const int MAX_MSG_SIZE = 1024;
		public const int NO_OF_MAILBOXES = 30;
		public const int MAILBOX_CONTENT_SIZE = 250;
		public const int USB_CMD_IN_REP_SIZE = 1024;
		public const int USB_CMD_OUT_REP_SIZE = 1024;

		public const int SIZEOF_BEGINLIST = 6;
		public const int SIZEOF_RPLYBEGINLIST = 12;
		public const int SIZEOF_CONTINUELIST = 7;
		public const int SIZEOF_RPLYCONTINUELIST = 8;
		public const int SIZEOF_BEGINGETFILE = 6;
		public const int SIZEOF_RPLYBEGINGETFILE = 12;
		public const int SIZEOF_CONTINUEGETFILE = 7;
		public const int SIZEOF_RPLYCONTINUEGETFILE = 12;
		public const int SIZEOF_BEGINREAD = 6;
		public const int SIZEOF_RPLYBEGINREAD = 12;
		public const int SIZEOF_CONTINUEREAD = 7;
		public const int SIZEOF_RPLYCONTINUEREAD = 8;
		public const int SIZEOF_LISTHANDLES = 4;
		public const int SIZEOF_RPLYLISTHANDLES = 7;
		public const int SIZEOF_REMOVEFILE = 4;
		public const int SIZEOF_RPLYREMOVEFILE = 7;
		public const int SIZEOF_MAKEDIR = 4;
		public const int SIZEOF_RPLYMAKEDIR = 7;
		public const int SIZEOF_CLOSEHANDLE = 5;
		public const int SIZEOF_RPLYCLOSEHANDLE = 8;
		public const int SIZEOF_BEGINDL = 10;
		public const int SIZEOF_RPLYBEGINDL = 8;
		public const int SIZEOF_CONTINUEDL = 7;
		public const int SIZEOF_RPLYCONTINUEDL = 8;
		public const int SIZEOF_WRITEMAILBOX = 7;
		public const int SIZEOF_WRITETOMAILBOXPAYLOAD = 2;
		public const int SIZEOF_BLUETOOTHPIN = 7;
		public const int SIZEOF_RPLYBLUETOOTHPIN = 7;
		public const int SIZEOF_BUNDLEID = 6;
		public const int SIZEOF_RPLYBUNDLEID = 7;
		public const int SIZEOF_BUNDLESEEDID = 6;
		public const int SIZEOF_RPLYBUNDLESEEDID = 7;

		// it was enum
		public const int TXIDLE = 0;
		public const int TXFILEUPLOAD = 1;
		public const int TXGETFILE = 2;
		public const int TXLISTFILES = 3;
		public const int TXFOLDER = 4;
		public const int TXFILE = 5;
		public const int RXIDLE = 6;
		public const int RXFILEDL = 7;
		// it was enum
		public const int SUBSTATE_IDLE = 0;
		public const int FILE_IN_PROGRESS_WAIT_FOR_REPLY = 1;
		public const int FILE_COMPLETE_WAIT_FOR_REPLY = 2;
		// it was enum
		public const int DIR_CMD_REPLY = 0x01;
		public const int DIR_CMD_NOREPLY = 0x02;
		public const int SYS_CMD_REPLY = 0x04;
		public const int SYS_CMD_NOREPLY = 0x08;
		#endregion

		#region c_com.c
		public const int FS_IDLE = 0;

		public const int SIZEOFFILESIZE = 8;
		public const int USB_CABLE_DETECT_RATE = 15000;  // ?? Approx 5 sec. on a good day. Cable detection is a NON-critical
		#endregion

		#region c_bt.h
		public const string NONVOL_BT_DATA = "settings/nonvolbt";

		public const int MAX_DEV_TABLE_ENTRIES = 30;
		public const int BT_CH_OFFSET = 2;
		public const int MAX_NAME_SIZE = 32;
		public const int MAX_BT_NAME_SIZE = 248;

		public const int MAX_BUNDLE_ID_SIZE = 24;
		public const int MAX_BUNDLE_SEED_ID_SIZE = 11;

		// was enums
		public const int BT_SLAVE_CH0 = 0;
		public const int BT_HOST_CH0 = 1;
		public const int BT_HOST_CH1 = 2;
		public const int BT_HOST_CH2 = 3;
		public const int BT_HOST_CH3 = 4;
		public const int BT_HOST_CH4 = 5;
		public const int BT_HOST_CH5 = 6;
		public const int BT_HOST_CH6 = 7;
		public const int NO_OF_BT_CHS = 8;

		// Defines related to Device List
		public const int DEV_EMPTY = 0x00;
		public const int DEV_KNOWN = 0x01;

		public const int READ_BUF_EMPTY = 0;
		public const int READ_BUF_FULL = 1;

		public const int SCAN_OFF = 0;
		public const int SCAN_INQ_STATE = 1;
		public const int SCAN_NAME_STATE = 2;

		// Defines related to Channels
		public const int CH_CONNECTING = 0;
		public const int CH_FREE = 1;
		public const int CH_CONNECTED = 2;

		// Defines related to BtInstance.HciSocket.Busy
		public const int HCI_IDLE = 0x00;
		public const int HCI_ONOFF = 0x01;
		public const int HCI_VISIBLE = 0x02;
		public const int HCI_NAME = 0x04;
		public const int HCI_SCAN = 0x08;
		public const int HCI_CONNECT = 0x10;
		public const int HCI_DISCONNECT = 0x20;
		public const int HCI_RESTART = 0x40;
		public const int HCI_FAIL = 0x80;
		#endregion

		#region c_bt.c
		public const string LEGO_BUNDLE_SEED_ID = "9RNK8ZF528";
		public const string LEGO_BUNDLE_ID = "com.lego.lms";


		public const int I_AM_IN_IDLE = 0;
		public const int I_AM_MASTER = 1;
		public const int I_AM_SLAVE = 2;
		public const int I_AM_SCANNING = 3;
		public const int STOP_SCANNING = 4;
		public const int TURN_ON = 5;
		public const int TURN_OFF = 6;
		public const int RESTART = 7;
		public const int BLUETOOTH_OFF = 8;

		public const int MSG_BUF_EMPTY = 0;
		public const int MSG_BUF_LEN = 1;
		public const int MSG_BUF_BODY = 2;
		public const int MSG_BUF_FULL = 3;

		/* Constants related to Decode mode */
		public const int MODE1 = 0;
		public const int MODE2 = 1;
		#endregion

		#region c_daisy.h
		public const int DAISY_COMMAND_REPLY = 0x0A;   //  Daisy command, reply required
		public const int DAISY_COMMAND_NO_REPLY = 0x8A;   //  Daisy command, reply not required

		/*
		  Byte 5:   Daisy sub Command. see following defines
		*/

		public const int DAISY_CHAIN_DOWNSTREAM = 0xA0;  //  Write down into the branch of DaisyChained Bricks
		public const int DAISY_CHAIN_UPSTREAM = 0xA1;  //  Data packet from port expanding (DaisyChained) downstream Bricks
		public const int DAISY_CHAIN_INFO = 0xA2;  //  Sensor information packet (max. 54 bytes)
		public const int DAISY_UNLOCK_SLAVE = 0xA3; //  Ask the slave to start pushing data upstream
		public const int DAISY_SET_TYPE = 0xA4;//  Set mode/type downstream (no reply needed - status via array)
		public const int DAISY_CHAIN_DOWNSTREAM_WITH_BUSY = 0XA5;  //  Payload also includes Magic Cookies (4 pcs)

		public const int DAISY_REPLY = 0x08;  //  Daisy command reply
		public const int DAISY_REPLY_ERROR = 0x09;   //  Daisy command reply error

		public const int DAISY_VENDOR_ID = 0x0694;     // LEGO Group
		public const int DAISY_PRODUCT_ID = 0x0005;        // EV3, the 5'th USB device
		public const int INTERFACE_NUMBER = 0;                   // One and only

		public const int DIR_IN = 0x80;                          // USB EP direction
		public const int DIR_OUT = 0x00;                              // -
		public const int DAISY_INT_EP_ADR = 0x01;                 // HW specific

		public const int DAISY_INTERRUPT_EP_IN = DIR_IN + DAISY_INT_EP_ADR;       // Interrupt endpoint used for UPSTREAM
		public const int DAISY_INTERRUPT_EP_OUT = DIR_OUT + DAISY_INT_EP_ADR;    // Interrupt endpoint used for DOWNSTREAM

		public const int DAISY_MAX_EP_IN_SIZE = 64;                                                    // FULL speed record size
		public const int DAISY_MAX_EP_OUT_SIZE = 64;                                                    // FULL speed record size
		public const int DAISY_DEFAULT_MAX_EP_SIZE = DAISY_MAX_EP_OUT_SIZE;
		public const int DAISY_UPSTREAM_DATA_LENGTH = DAISY_DEFAULT_MAX_EP_SIZE;
		public const int DAISY_DEFAULT_TIMEOUT = 1000;                                                // Timeout set to 1 sec by default
		public const int NO_USER_DATA = 0;                                                               // No user data embedded in transfer
		public const int DAISY_MAX_INPUT_PER_LAYER = 4;                   // Hardware size
		public const int DAISY_MAX_OUTPUT_PER_LAYER = 4;                    // -
		public const int DAISY_MAX_SENSORS_PER_LAYER = (DAISY_MAX_INPUT_PER_LAYER + DAISY_MAX_OUTPUT_PER_LAYER); // Total per layer
		public const int DAISY_MAX_LAYER_DEPT = 4;                          // Max layer depth

		public const int DEVICE_MAX_DATA = 32;                                // Max data size
		public const int BUSYFLAGS = 16;                                   // Motor busyflags (grand total)
		public const int DEVICE_MAX_INCL_BUSYFLAGS = (DEVICE_MAX_DATA + BUSYFLAGS);

		public const string SLAVE_PROD_ID = "0005";
		public const string SLAVE_VENDOR_ID = "0694";

		public const int SIZEOF_DAISY_POLL = 6;
		public const int SIZEOF_UNLOCK_REPLY = 7;
		public const int SIZEOF_DAISY_READ = 8;
		public const int SIZEOF_DAISY_WRITE = 6;

		public const int SIZEOF_DAISY_DEVICE_DATA_PROLOG = 11;
		public const int SIZEOF_REPLY_DAISY_READ = (DAISY_DEFAULT_MAX_EP_SIZE - sizeof(CMDSIZE));
		public const int DAISY_DEVICE_PAYLOAD_SIZE = (DEVICE_MAX_DATA + 4);  // Status, Type, Mode, DeviceData, CheckSum
		public const int DAISY_DEVICE_CHKSUM_SIZE = 1;
		public const int DAISY_MAX_INPUT_SENSOR_INDEX = 3;
		public const int DAISY_SENSOR_OUTPUT_OFFSET = 12;
		public const int DAISY_SENSOR_DATA_SIZE = (SIZEOF_DAISY_DEVICE_DATA_PROLOG + DEVICE_MAX_DATA + DAISY_DEVICE_CHKSUM_SIZE - sizeof(CMDSIZE));
		public const int BUSYFLAGS_START_POS = (SIZEOF_DAISY_DEVICE_DATA_PROLOG + DEVICE_MAX_DATA + DAISY_DEVICE_CHKSUM_SIZE);

		public const int SIZEOF_DAISY_INFO = 6;
		public const int SIZEOF_DAISY_INFO_READ = (DAISY_DEFAULT_MAX_EP_SIZE - SIZEOF_DAISY_INFO);

		public const int LAYER_POS = 6;
		public const int SENSOR_POS = (LAYER_POS + 1);


		public const int DAISY_MAX_DATASIZE = DAISY_DEFAULT_MAX_EP_SIZE;
		public const int DAISY_DATA_PACKET = (DAISY_DEFAULT_MAX_EP_SIZE - sizeof(CMDSIZE));

		public const int PAYLOAD_OFFSET = 4;

		public const int LAYER_POS_DOWN = (STAT + 1);
		public const int SENSOR_POS_DOWN = (LAYER_POS_DOWN + 1);
		public const int TYPE_POS_DOWN = (SENSOR_POS_DOWN + 1);
		public const int MODE_POS_DOWN = (TYPE_POS_DOWN + 1);

		public const int LAYER_POS_TO_SLAVE = (STAT);
		public const int SENSOR_POS_TO_SLAVE = (LAYER_POS_TO_SLAVE + 1);
		public const int TYPE_POS_TO_SLAVE = (SENSOR_POS_TO_SLAVE + 1);
		public const int MODE_POS_TO_SLAVE = (TYPE_POS_TO_SLAVE + 1);

		public const int CHECKSUM_POS = (DAISY_DEVICE_PAYLOAD_SIZE - 1);

		public const int BUSYTIME = 25000;        // [uS]
		public const int DAISY_PRIORITY_COUNT = 2;    // 1:3
		public const int DAISY_PUSH_NOT_UNLOCKED = 0x55;

		public const int TIME_TO_CHECK_FOR_DAISY_DOWNSTREAM = 2000;   //16000
		public const string DAISY_SLAVE_SEARCH_STRING = "ID 0694:0005 Lego Group";

		// remove is when add daisy enums
		public const int STAT = 6;
		#endregion

		#region c_i2c.c
		public const int WRITE_DATA = 0x2A;    // Also referred to as 0x54
		public const int READ_STATUS = 0x2A;    // Also referred to as 0x55
		public const int WRITE_RAW = 0x2B;    // Also referred to as 0x56
		public const int READ_DATA = 0x2B;    // Also referred to as 0x57

		/* Bluetooth pins needs to be synchronized with pins in d_bt.c */
		public const int CTS_PIC = 0;
		public const int PIC_RST = 1;
		public const int PIC_EN = 2;
		public const int BLUETOOTH_PINS = 3;

		public const int SET = 0;
		public const int CLEAR = 1;
		public const int HIIMP = 2;

		public const int OVERRUN_ERROR = 0x01;
		public const int CRC_ERROR = 0x02;
		public const int INCORRECT_ACTION = 0x03;
		public const int UNEXPECTED_ERROR = 0x04;
		public const int RAW_OVERRUN_ERROR = 0x05;

		public const int I2CBUF_SIZE = 150;
		public const int APPDATABUF_SIZE = 150;
		public const int MODE2BUF_SIZE = 1024;   // Must be power of 2
		public const int MIN_MSG_LEN = 6;
		public const uint SLEEPuS = ((ULONG)(1000));
		public const uint SEC_1 = (((ULONG)(1000000)) / SLEEPuS);
		#endregion

		#region c_md5.h
		public const int MD5LEN = 32;
		#endregion

		#region c_md5.c
		public static int FF(int b, int c, int d)
		{
			return (d ^ (b & (c ^ d)));
		}
		public static int FG(int b, int c, int d)
		{
			return FF(d, b, c);
		}
		public static int FH(int b, int c, int d)
		{
			return (b ^ c ^ d);
		}
		public static int FI(int b, int c, int d)
		{
			return (c ^ (b | ~d));
		}
		#endregion

		#region c_wifi.h
		public const string WIFI_PERSISTENT_PATH = vmSETTINGS_DIR;        // FileSys guidance ;-)
		public const string WIFI_PERSISTENT_FILENAME = "WiFiConnections.dat";// Persistent storage for KNOWN connections

		public const int MAC_ADDRESS_LENGTH = 18;  // xx:xx:xx:xx:xx:xx + /0x00
		public const int FREQUENCY_LENGTH = 5;
		public const int SIGNAL_LEVEL_LENGTH = 4;
		public const int SECURITY_LENGTH = 129;//33
		public const int FRIENDLY_NAME_LENGTH = 33;

		public const int PSK_LENGTH = 65; // 32 bytes = 256 bit + /0x00
		public const int KEY_MGMT_LENGTH = 33;
		public const int PAIRWISE_LENGTH = 33;
		public const int GROUP_LENGTH = 33;
		public const int PROTO_LENGTH = 33;

		public const int WIFI_INIT_TIMEOUT = 10; //60
		public const int WIFI_INIT_DELAY = 10;

		public const string BROADCAST_IP_LOW = "255";  // "192.168.0.255"
		public const int BROADCAST_PORT = 3015;  // UDP
		public const int TCP_PORT = 5555;
		public const int BEACON_TIME = 5;        // 5 sec's between BEACONs

		public const int TIME_FOR_WIFI_DONGLE_CHECK = 10;

		public const int BLUETOOTH_SER_LENGTH = 13; // No "white" ;space separators
		public const int BRICK_HOSTNAME_LENGTH = (NAME_LENGTH + 1);

		public const int WIFI_NOT_INITIATED = 0;   // NOTHING INIT'ed
		public const int WIFI_INIT = 0;            // Do the time consumption stuff
		public const int WIFI_INITIATED = 0;      // Initiated and ready for AP:
												  // We have the H/W stack up and running
		public const int READY_FOR_AP_SEARCH = 0;  // Stack ready for further AP search
		public const int SEARCH_APS = 0;           // As it says
		public const int SEARCH_PENDING = 0;       // Time consumption search
		public const int AP_LIST_UPDATED = 0;     // User have forced a new search
		public const int AP_CONNECTING = 0;
		public const int WIFI_CONNECTED_TO_AP = 0; // User has connected to an AP
		public const int UDP_NOT_INITIATED = 0;    // Ready for starting Beacons
		public const int INIT_UDP_CONNECTION = 0;  // Do the init via UDP
		public const int UPD_FIRST_TX = 0;         // Temporary state after start Beacon TX
		public const int UDP_VISIBLE = 0;          // We TX'es a lot of Beacon
		public const int UDP_CONNECTED = 0;        // We have an active negotiation connection
		public const int TCP_NOT_CONNECTED = 0;    // Waiting for the PC to connect via TCP
		public const int TCP_CONNECTED = 0;    // We have a TCP connection established
		public const int CLOSED = 0;               // UDP/TCP closed

		public const int NOT_INIT = 0x00;
		public const int LOAD_SUPPLICANT = 0x01;
		public const int WAIT_ON_INTERFACE = 0x02;
		public const int DONE = 0x80;

		public const int TCP_IDLE = 0x00;
		public const int TCP_WAIT_ON_START = 0x01;
		public const int TCP_WAIT_ON_LENGTH = 0x02;
		public const int TCP_WAIT_ON_FIRST_CHUNK = 0x04;
		public const int TCP_WAIT_ON_ONLY_CHUNK = 0x08;
		public const int TCP_WAIT_COLLECT_BYTES = 0x10;

		public const int VISIBLE = 0x1;
		public const int CONNECTED = 0x02;
		public const int WPA2 = 0x04;
		public const int KNOWN = 0x08;
		public const int UNKNOWN = 0x80;
		public const int AP_FLAG_ADJUST_FOR_STORAGE = ((int)(~(VISIBLE + CONNECTED + UNKNOWN)));  // Set/reset for persistent storage

		public const int NO_CONNECTION = 0x0;
		public const int CONNECTION_MADE = 0x01;
		public const int SEARCHING = 0x02;
		#endregion

		#region c_input.h
		public const int INPUT_PORTS = INPUTS;
		public const int INPUT_DEVICES = (INPUT_PORTS * CHAIN_DEPT);

		public const int OUTPUT_PORTS = OUTPUTS;
		public const int OUTPUT_DEVICES = (OUTPUT_PORTS * CHAIN_DEPT);

		public const int DEVICES = (INPUT_DEVICES + OUTPUT_DEVICES);

		public const int INPUT_VALUES = (INPUT_PORTS * 3);
		public const int INPUT_VALUE_SIZE = 5;
		public const int INPUT_BUFFER_SIZE = (INPUT_VALUES * INPUT_VALUE_SIZE);
		public const int INPUT_SIZE = (INPUT_VALUES * 2);
		// public static KeyValuePair<OP, OPCODE> INPUT_DEVICE_LIST = OC(opINPUT_DEVICE_LIST,&cInputDeviceList,7,0 )
		#endregion

		#region c_input.c
		public const int LINESIZE = 255;

		public unsafe static IMGDATA* CLR_LAYER_CLR_CHANGES = (new byte[] { opINPUT_DEVICE, CLR_CHANGES, 0, 0, opINPUT_DEVICE, CLR_CHANGES, 0, 1, opINPUT_DEVICE, CLR_CHANGES, 0, 2, opINPUT_DEVICE, CLR_CHANGES, 0, 3, opOBJECT_END }).AsPointer();
		public unsafe static IMGDATA* CLR_LAYER_CLR_BUMBED = (new byte[] { opUI_BUTTON, FLUSH, opOBJECT_END }).AsPointer();
		public unsafe static IMGDATA* CLR_LAYER_OUTPUT_RESET = (new byte[] { opOUTPUT_RESET, 0, 15, opOBJECT_END }).AsPointer();
		public unsafe static IMGDATA* CLR_LAYER_OUTPUT_CLR_COUNT = (new byte[] { opOUTPUT_CLR_COUNT, 0, 15, opOBJECT_END }).AsPointer();
		public unsafe static IMGDATA* CLR_LAYER_INPUT_WRITE = (new byte[] { opINPUT_WRITE, 0, 0, 1, DEVCMD_RESET, opINPUT_WRITE, 0, 1, 1, DEVCMD_RESET, opINPUT_WRITE, 0, 2, 1, DEVCMD_RESET, opINPUT_WRITE, 0, 3, 1, DEVCMD_RESET, opOBJECT_END }).AsPointer();

		public unsafe static IMGDATA* STOP_LAYER = (new byte[] { opOUTPUT_PRG_STOP, opOBJECT_END }).AsPointer();

		public unsafe static TYPES[] TypeDefault =
		{
			//   Name										   Type                   Connection                Mode	 DataSets     Format      Figures       Decimals      Views      RawMin         RawMax            PctMin         PctMax            SiMin        SiMax            Time             IdValue        Pins             Symbol
			new TYPES("PORT ERROR".AsSbytePointer()) {      Type = TYPE_ERROR,   Connection = CONN_ERROR,   Mode = 0, DataSets = 0, Format = 0, Figures = 4, Decimals = 0, Views = 1, RawMin = 0.0f, RawMax = 0.0f,    PctMin = 0.0f, PctMax = 0.0f,   SiMin = 0.0f, SiMax = 0.0f,    InvalidTime = 0, IdValue = 0, Pins = (sbyte)'f', },
			new TYPES("NONE".AsSbytePointer())       {      Type = TYPE_NONE,    Connection = CONN_NONE,    Mode = 0, DataSets = 0, Format = 0, Figures = 4, Decimals = 0, Views = 1, RawMin = 0.0f, RawMax = 0.0f,    PctMin = 0.0f, PctMax = 0.0f,   SiMin = 0.0f, SiMax = 0.0f,    InvalidTime = 0, IdValue = 0, Pins = (sbyte)'f', },
			new TYPES("UNKNOWN".AsSbytePointer())    {      Type = TYPE_UNKNOWN, Connection = CONN_UNKNOWN, Mode = 0, DataSets = 1, Format = 1, Figures = 4, Decimals = 0, Views = 1, RawMin = 0.0f, RawMax = 1023.0f, PctMin = 0.0f, PctMax = 100.0f, SiMin = 0.0f, SiMax = 1023.0f, InvalidTime = 0, IdValue = 0, Pins = (sbyte)'f', },
			new TYPES("".AsSbytePointer())           { }
		};

		public const int SENSOR_RESOLUTION = 1023;

		/* Remember this is ARM AD converter  - 3,3 VDC as max voltage      */
		/* When in color mode background value is substracted => min = 0!!! */
		public const int AD_MAX = 2703;
		public const int AD_FS = 3300;

		public const int COLORSENSORBGMIN = (214 / (AD_FS / AD_MAX));
		public const int COLORSENSORMIN = (1 / (AD_FS / AD_MAX)); /* 1 inserted else div 0 (1L/(120/AD_MAX)) */
		public const int COLORSENSORMAX = ((AD_MAX * AD_FS) / 3300);
		public const int COLORSENSORPCTDYN = (UBYTE)(((COLORSENSORMAX - COLORSENSORMIN) * 100L) / AD_MAX);
		public const int COLORSENSORBGPCTDYN = (UBYTE)(((COLORSENSORMAX - COLORSENSORBGMIN) * 100L) / AD_MAX);
		#endregion

		#region c_ui.h
		public const int IMAGEBUFFER_SIZE = 1000;
		public const int KEYBUF_SIZE = 100;
		public const int UI_WR_BUFFER_SIZE = 255;

		public const int GRAPH_BUFFERS = (INPUTS + OUTPUTS);
		public const int GRAPH_BUFFER_SIZE = LCD_WIDTH;

		public const int MAX_NOTIFY_LINES = 8;
		public const int MAX_NOTIFY_LINE_CHARS = 32;

		public const int TEXTSIZE = 24;

		public const int HWVERS_SIZE = 6;
		public const int FWVERS_SIZE = 7;
		public const int FWBUILD_SIZE = 11;
		public const int OSVERS_SIZE = 17;
		public const int OSBUILD_SIZE = 11;
		public const int IPADDR_SIZE = 16;

		public const int BUTTON_ACTIVE = 0x01;
		public const int BUTTON_PRESSED = 0x02; //!< button is pressed at the moment
		public const int BUTTON_ACTIVATED = 0x04; //!< button has been activated since last read
		public const int BUTTON_LONGPRESS = 0x08; //!< button long press detected
		public const int BUTTON_BUMBED = 0x10; //!< button has been pressed and released
		public const int BUTTON_LONG_LATCH = 0x20;

		public const int BUTTON_CLR = (BUTTON_ACTIVATED | BUTTON_LONGPRESS | BUTTON_BUMBED | BUTTON_LONG_LATCH);
		public const int BUTTON_FLUSH = (BUTTON_ACTIVATED | BUTTON_LONGPRESS | BUTTON_BUMBED | BUTTON_LONG_LATCH);

		public const int BUTTON_ALIVE = 0x01;
		public const int BUTTON_CLICK = 0x02;
		public const int BUTTON_BUFPRINT = 0x04;

		public const int BUTTON_SET = (BUTTON_ALIVE | BUTTON_CLICK);
		#endregion

		#region c_ui.c
		public const int CALL_INTERVAL = 400;  // [mS]

		public unsafe static IMGDATA* DownloadSuccesSound = (new byte[] { opINFO, LC0(GET_VOLUME), LV0(0), opSOUND, LC0(PLAY), LV0(0), LCS, (byte)'u', (byte)'i', (byte)'/', (byte)'D', (byte)'o', (byte)'w', (byte)'n', (byte)'l', (byte)'o', (byte)'a', (byte)'d', (byte)'S', (byte)'u', (byte)'c', (byte)'c', (byte)'e', (byte)'s', 0, opSOUND_READY, opOBJECT_END }).AsPointer();

		public const int REAL_ANY_BUTTON = 6;
		public const int REAL_NO_BUTTON = 7;

		public static Dictionary<int, sbyte> MappedToReal = new Dictionary<int, sbyte>()
		{
			{ UP_BUTTON, 0 },
			{ ENTER_BUTTON, 1 },
			{ DOWN_BUTTON, 2 },
			{ RIGHT_BUTTON, 3 },
			{ LEFT_BUTTON, 4 },
			{ BACK_BUTTON, 5 },
			{ ANY_BUTTON, REAL_ANY_BUTTON },
			{ NO_BUTTON, REAL_NO_BUTTON },
		};

		public const float SHUNT_IN = 0.11f;             //  [Ohm]
		public const float AMP_CIN = 22.0f;       //  [Times]

		public const float EP2_SHUNT_IN = 0.05f;           //  [Ohm]
		public const float EP2_AMP_CIN = 15.0f;          //  [Times]

		public const float SHUNT_OUT = 0.055f;           //  [Ohm]
		public const float AMP_COUT = 19.0f;            //  [Times]

		public const float VCE = 0.05f;            //  [V]
		public const float AMP_VIN = 0.5f;            //  [Times]

		public const int AVR_CIN = 300;
		public const int AVR_COUT = 30;
		public const int AVR_VIN = 30;

		public static float CNT_V(float C) { return (((DATAF)C * (DATAF)ADC_REF) / ((DATAF)ADC_RES * (DATAF)1000.0)); }

		public const int TOP_BATT_ICONS = 5;
		public static UBYTE[] TopLineBattIconMap =
		{
			SICON_BATT_0,           //  0
			SICON_BATT_1,           //  1
			SICON_BATT_2,           //  2
			SICON_BATT_3,           //  3
			SICON_BATT_4            //  4
		};

		public const int TOP_BT_ICONS = 4;
		public static UBYTE[] TopLineBtIconMap =
		{
			SICON_BT_ON,            //  001
			SICON_BT_VISIBLE,       //  011
			SICON_BT_CONNECTED,     //  101
			SICON_BT_CONNVISIB,     //  111
		};

		public const int TOP_WIFI_ICONS = 4;
		public static UBYTE[] TopLineWifiIconMap =
		{
			SICON_WIFI_3,           //  001
			SICON_WIFI_3,           //  011
			SICON_WIFI_CONNECTED,   //  101
			SICON_WIFI_CONNECTED,   //  111
		};

		public static Dictionary<int, DATA8> FiletypeToNormalIcon = new Dictionary<int, sbyte>()
		{
			{ FILETYPE_UNKNOWN, ICON_FOLDER },
			{ TYPE_FOLDER, ICON_FOLDER },
			{ TYPE_SOUND, ICON_SOUND },
			{ TYPE_BYTECODE,  ICON_RUN },
			{ TYPE_GRAPHICS, ICON_IMAGE },
			{ TYPE_DATALOG,  ICON_OBD },
			{ TYPE_PROGRAM,  ICON_OBP },
			{ TYPE_TEXT,  ICON_TEXT }
		};

		public const int MAX_KEYB_DEEPT = 3;
		public const int MAX_KEYB_WIDTH = 12;
		public const int MAX_KEYB_HEIGHT = 4;

		public static Dictionary<int, byte[]> Delimiter = new Dictionary<int, byte[]>
		{
			[DEL_NONE] = new byte[0],
			[DEL_TAB] = new byte[1] { (byte)'\t' },
			[DEL_SPACE] = new byte[1] { (byte)' ' },
			[DEL_RETURN] = new byte[1] { (byte)'\r' },
			[DEL_COLON] = new byte[1] { (byte)':' },
			[DEL_COMMA] = new byte[1] { (byte)',' },
			[DEL_LINEFEED] = new byte[1] { (byte)'\n' },
			[DEL_CRLF] = new byte[2] { (byte)'\r', (byte)'\n' },
		};
		#endregion

		#region d_lcd.h
		public unsafe static void LCDClear(UBYTE* lcd)
		{
			for (int i = 0; i < LCD_BUFFER_SIZE; ++i)
			{
				lcd[i] = (byte)BG_COLOR;
			}
		}

		public unsafe static void LCDClearTopline(UBYTE* lcd)
		{
			for (int i = 0; i < LCD_TOPLINE_SIZE; ++i)
			{
				lcd[i] = (byte)BG_COLOR;
			}
		}

		public unsafe static void LCDErase(UBYTE* lcd)
		{
			for (int i = LCD_TOPLINE_SIZE; i < LCD_BUFFER_SIZE; ++i)
			{
				lcd[i] = (byte)BG_COLOR;
			}
		}

		public unsafe static void LCDCopy(UBYTE* slcd, UBYTE* dlcd, int size)
		{
			for (int i = 0; i < size; ++i)
			{
				dlcd[i] = slcd[i];
			}
		}
        #endregion

        #region d_lcd.c
        public static UBYTE[] PixelTab =
		{
			0x00, // 000 00000000
			0xE0, // 001 11100000
			0x1C, // 010 00011100
			0xFC, // 011 11111100
			0x03, // 100 00000011
			0xE3, // 101 11100011
			0x1F, // 110 00011111
			0xFF  // 111 11111111
		};

        // it was not a define but i don't care
        public unsafe static Dictionary<int, FONTINFO> FontInfo = new Dictionary<int, FONTINFO>()
		{
			{ NORMAL_FONT, new FONTINFO()
							{
								pFontBits    = BmpHelper.GetBytesOf(BmpType.NormalFont).AsPointer(),
								FontHeight   = 9,
								FontWidth    = 8,
								FontHorz     = 16,
								FontFirst    = 0x20,
								FontLast     = 0x7F
							}},
			{ SMALL_FONT, new FONTINFO()
							{
								pFontBits    = BmpHelper.GetBytesOf(BmpType.SmallFont).AsPointer(),
								FontHeight   = 8,
								FontWidth    = 8,
								FontHorz     = 16,
								FontFirst    = 0x20,
								FontLast     = 0x7F
							}},
			{ LARGE_FONT, new FONTINFO()
							{
								pFontBits = BmpHelper.GetBytesOf(BmpType.LargeFont).AsPointer(),
								FontHeight = 16,
								FontWidth = 16,
								FontHorz = 16,
								FontFirst = 0x20,
								FontLast = 0x7F
							}},
			{ TINY_FONT, new FONTINFO()
							{
								pFontBits = BmpHelper.GetBytesOf(BmpType.TinyFont).AsPointer(),
								FontHeight = 7,
								FontWidth = 5,
								FontHorz = 16,
								FontFirst = 0x20,
								FontLast = 0x7F
							}}
		};

		// it was also not a define
		public unsafe static Dictionary<int, ICONINFO> IconInfo = new Dictionary<int, ICONINFO>()
		{
			{ NORMAL_ICON, new ICONINFO()
							{
								pIconBits    = BmpHelper.GetBytesOf(BmpType.NormalIcons).AsPointer(),
								IconSize     = 420,
								IconHeight   = 12,
								IconWidth    = 24,
							}},
			{ SMALL_ICON, new ICONINFO()
							{
								pIconBits    = BmpHelper.GetBytesOf(BmpType.SmallIcons).AsPointer(),
								IconSize     = 176,
								IconHeight   = 8,
								IconWidth    = 16,
							}},
			{ LARGE_ICON, new ICONINFO()
							{
								pIconBits    = BmpHelper.GetBytesOf(BmpType.LargeIcons).AsPointer(),
								IconSize     = 616,
								IconHeight   = 22,
								IconWidth    = 24,
							}},
			{ MENU_ICON, new ICONINFO()
							{
								pIconBits    = BmpHelper.GetBytesOf(BmpType.MenuIcons).AsPointer(),
								IconSize     = 132,
								IconHeight   = 12,
								IconWidth    = 16,
							}},
			{ ARROW_ICON, new ICONINFO()
							{
								pIconBits    = BmpHelper.GetBytesOf(BmpType.ArrowIcons).AsPointer(),
								IconSize     = 36,
								IconHeight   = 12,
								IconWidth    = 8,
							}},
};
		#endregion
	}
}
