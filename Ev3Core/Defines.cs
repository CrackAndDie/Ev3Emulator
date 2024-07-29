namespace Ev3Core
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

		public const string vmSETTINGS_DIR = "../sys/settings";           //!< Folder for non volatile settings

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

		public static UBYTE LC0(UBYTE v) { return ((UBYTE)((v & PRIMPAR_VALUE) | PRIMPAR_SHORT | PRIMPAR_CONST)); }
		public static (UBYTE, UBYTE) LC1(UBYTE v) { return ((PRIMPAR_LONG | PRIMPAR_CONST | PRIMPAR_1_BYTE), (UBYTE)(v & 0xFF)); }
		public static (UBYTE, UBYTE, UBYTE) LC2(UWORD v) { return ((PRIMPAR_LONG | PRIMPAR_CONST | PRIMPAR_2_BYTES), (UBYTE)(v & 0xFF), (UBYTE)((v >> 8) & 0xFF)); }
		public static (UBYTE, UBYTE, UBYTE, UBYTE, UBYTE) LC4(ULONG v) { return ((PRIMPAR_LONG | PRIMPAR_CONST | PRIMPAR_4_BYTES), (UBYTE)((ULONG)v & 0xFF), (UBYTE)(((ULONG)v >> (int)8) & 0xFF), (UBYTE)(((ULONG)v >> (int)16) & 0xFF), (UBYTE)(((ULONG)v >> (int)24) & 0xFF)); }
		public static (UBYTE, UBYTE) LCA(UBYTE h) { return ((PRIMPAR_LONG | PRIMPAR_CONST | PRIMPAR_1_BYTE | PRIMPAR_ARRAY), (UBYTE)(h & 0xFF)); }

		public static UBYTE LV0(UBYTE v) { return ((UBYTE)((v & PRIMPAR_VALUE) | PRIMPAR_SHORT | PRIMPAR_LOCAL)); }
		public static (UBYTE, UBYTE) LV1(UBYTE v) { return ((PRIMPAR_LONG | PRIMPAR_LOCAL | PRIMPAR_1_BYTE), (UBYTE)(v & 0xFF)); }
		public static (UBYTE, UBYTE, UBYTE) LV2(UWORD v) { return ((PRIMPAR_LONG | PRIMPAR_LOCAL | PRIMPAR_2_BYTES), (UBYTE)(v & 0xFF), (UBYTE)((v >> 8) & 0xFF)); }
		public static (UBYTE, UBYTE, UBYTE, UBYTE, UBYTE) LV4(ULONG v) { return ((PRIMPAR_LONG | PRIMPAR_LOCAL | PRIMPAR_4_BYTES), (UBYTE)((ULONG)v & 0xFF), (UBYTE)(((ULONG)v >> (int)8) & 0xFF), (UBYTE)(((ULONG)v >> (int)16) & 0xFF), (UBYTE)(((ULONG)v >> (int)24) & 0xFF)); }
		public static (UBYTE, UBYTE) LVA(UBYTE h) { return ((PRIMPAR_LONG | PRIMPAR_LOCAL | PRIMPAR_1_BYTE | PRIMPAR_ARRAY), (UBYTE)(h & 0xFF)); }

		public static UBYTE GV0(UBYTE v) { return ((UBYTE)((v & PRIMPAR_VALUE) | PRIMPAR_SHORT | PRIMPAR_GLOBAL)); }
		public static (UBYTE, UBYTE) GV1(UBYTE v) { return ((PRIMPAR_LONG | PRIMPAR_GLOBAL | PRIMPAR_1_BYTE), (UBYTE)(v & 0xFF)); }
		public static (UBYTE, UBYTE, UBYTE) GV2(UWORD v) { return ((PRIMPAR_LONG | PRIMPAR_GLOBAL | PRIMPAR_2_BYTES), (UBYTE)(v & 0xFF), (UBYTE)((v >> 8) & 0xFF)); }
		public static (UBYTE, UBYTE, UBYTE, UBYTE, UBYTE) GV4(ULONG v) { return ((PRIMPAR_LONG | PRIMPAR_GLOBAL | PRIMPAR_4_BYTES), (UBYTE)((ULONG)v & 0xFF), (UBYTE)(((ULONG)v >> (int)8) & 0xFF), (UBYTE)(((ULONG)v >> (int)16) & 0xFF), (UBYTE)(((ULONG)v >> (int)24) & 0xFF)); }
		public static (UBYTE, UBYTE) GVA(UBYTE h) { return ((PRIMPAR_LONG | PRIMPAR_GLOBAL | PRIMPAR_1_BYTE | PRIMPAR_ARRAY), (UBYTE)(h & 0xFF)); }

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

		public const int FILENAME_SUBP = 12;
		public const int TST_SUBP = 6;
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
	}
}
