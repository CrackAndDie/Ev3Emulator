﻿using Ev3Core.Enums;

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

		// TODO: check it out. wtf is going on here
		//public const byte FILENAME_SUBP = 12;
		public const byte TST_SUBP = 6;

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
		public const byte FILENAME_SUBP = 16;

		public const byte SUBPS = 17;

		public static KeyValuePair<OP, OPCODE> OC(OP opCode, byte par1, byte par2, byte par3, byte par4, byte par5, byte par6, byte par7, byte par8)
		{
			return new KeyValuePair<OP, OPCODE>(opCode, new OPCODE()
			{
				Name = Enum.GetName(opCode.GetType(), opCode).ToCharArray(),
				Pars = ((ULONG)par1) + ((ULONG)par2 << 4) + ((ULONG)par3 << 8) + ((ULONG)par4 << 12) +
						((ULONG)par5 << 16) + ((ULONG)par6 << 20) + ((ULONG)par7 << 24) + ((ULONG)par8 << 28),
			});
		}

		public static KeyValuePair<byte, SUBCODE> SC(string subCodeName, byte subcode, byte par1, byte par2, byte par3, byte par4, byte par5, byte par6, byte par7, byte par8)
		{
			return new KeyValuePair<byte, SUBCODE>(subcode, new SUBCODE()
			{
				Name = subCodeName.ToCharArray(),
				Pars = ((ULONG)par1) + ((ULONG)par2 << 4) + ((ULONG)par3 << 8) + ((ULONG)par4 << 12) +
						((ULONG)par5 << 16) + ((ULONG)par6 << 20) + ((ULONG)par7 << 24) + ((ULONG)par8 << 28),
			});
		}

		public static KeyValuePair<OP, OPCODE>[] OpCodes = 
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
			OC(   OP.opINPUT_TEST,           PAR8,                                           0,0,0,0,0,0,0         ),
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
			OC(   OP.opFILENAME,             PAR8,SUBP,FILENAME_SUBP,                        0,0,0,0,0             ),
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
			OC(   OP.opTST,                  PAR8,SUBP,TST_SUBP,                             0,0,0,0,0             ),
		};

		public static KeyValuePair<byte, KeyValuePair<byte, SUBCODE>[]>[] SubCodes =
		{
			//    ParameterFormat         SubCode                 Parameters                                      Unused
			new KeyValuePair<byte, KeyValuePair<byte, SUBCODE>[]>(
				PROGRAM_SUBP,
				new KeyValuePair<byte, SUBCODE>[]
				{
					//    VM
					SC(nameof(OBJ_STOP), OBJ_STOP, PAR16, PAR16,                                    0,0,0,0,0,0           ),
					SC(nameof(OBJ_START), OBJ_START, PAR16, PAR16,                                    0,0,0,0,0,0           ),
					SC(nameof(GET_STATUS), GET_STATUS, PAR16, PAR8,                                     0,0,0,0,0,0           ),
					SC(nameof(GET_SPEED), GET_SPEED, PAR16, PAR32,                                    0,0,0,0,0,0           ),
					SC(nameof(GET_PRGRESULT), GET_PRGRESULT, PAR16, PAR8,                                     0,0,0,0,0,0           ),
					SC(nameof(SET_INSTR), SET_INSTR, PAR16,                                          0,0,0,0,0,0,0         ),
				}),

			new KeyValuePair<byte, KeyValuePair<byte, SUBCODE>[]>(
				FILE_SUBP,
				new KeyValuePair<byte, SUBCODE>[]
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
				}),

			new KeyValuePair<byte, KeyValuePair<byte, SUBCODE>[]>(
				ARRAY_SUBP,
				new KeyValuePair<byte, SUBCODE>[]
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
				}),

			new KeyValuePair<byte, KeyValuePair<byte, SUBCODE>[]>(
				FILENAME_SUBP,
				new KeyValuePair<byte, SUBCODE>[]
				{
					SC(nameof(EXIST), EXIST, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(TOTALSIZE), TOTALSIZE, PAR8, PAR32, PAR32,                               0,0,0,0,0             ),
					SC(nameof(SPLIT), SPLIT, PAR8, PAR8, PAR8, PAR8, PAR8,                       0,0,0                 ),
					SC(nameof(MERGE), MERGE, PAR8, PAR8, PAR8, PAR8, PAR8,                       0,0,0                 ),
					SC(nameof(CHECK), CHECK, PAR8, PAR8,                                      0,0,0,0,0,0           ),
					SC(nameof(PACK), PACK, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(UNPACK), UNPACK, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(GET_FOLDERNAME), GET_FOLDERNAME, PAR8, PAR8,                                      0,0,0,0,0,0           ),
				}),

			new KeyValuePair<byte, KeyValuePair<byte, SUBCODE>[]>(
				VM_SUBP,
				new KeyValuePair<byte, SUBCODE>[]
				{
					//    VM
					SC(nameof(SET_ERROR), SET_ERROR, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(GET_ERROR), GET_ERROR, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(ERRORTEXT), ERRORTEXT, PAR8, PAR8, PAR8,                                 0,0,0,0,0             ),

					SC(nameof(GET_VOLUME), GET_VOLUME, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(SET_VOLUME), SET_VOLUME, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(GET_MINUTES), GET_MINUTES, PAR8,                                           0,0,0,0,0,0,0         ),
					SC(nameof(SET_MINUTES), SET_MINUTES, PAR8,                                           0,0,0,0,0,0,0         ),
				}),

			new KeyValuePair<byte, KeyValuePair<byte, SUBCODE>[]>(
				TST_SUBP,
				new KeyValuePair<byte, SUBCODE>[]
				{
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
				}),

			new KeyValuePair<byte, KeyValuePair<byte, SUBCODE>[]>(
				STRING_SUBP,
				new KeyValuePair<byte, SUBCODE>[]
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
				}),

			new KeyValuePair<byte, KeyValuePair<byte, SUBCODE>[]>(
				UI_READ_SUBP,
				new KeyValuePair<byte, SUBCODE>[]
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
				}),

			new KeyValuePair<byte, KeyValuePair<byte, SUBCODE>[]>(
				UI_WRITE_SUBP,
				new KeyValuePair<byte, SUBCODE>[]
				{
					SC(nameof(WRITE_FLUSH), WRITE_FLUSH,            0,                                              0,0,0,0,0,0,0         ),
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
				}),

			new KeyValuePair<byte, KeyValuePair<byte, SUBCODE>[]>(
				UI_DRAW_SUBP,
				new KeyValuePair<byte, SUBCODE>[]
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
				}),

			new KeyValuePair<byte, KeyValuePair<byte, SUBCODE>[]>(
				UI_BUTTON_SUBP,
				new KeyValuePair<byte, SUBCODE>[]
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
				}),

			new KeyValuePair<byte, KeyValuePair<byte, SUBCODE>[]>(
				COM_READ_SUBP,
				new KeyValuePair<byte, SUBCODE>[]
				{
					//    Com
					SC(nameof(COMMAND), COMMAND, PAR32, PAR32, PAR32, PAR8,                         0,0,0,0               ),
				}),
			new KeyValuePair<byte, KeyValuePair<byte, SUBCODE>[]>(
				COM_WRITE_SUBP,
				new KeyValuePair<byte, SUBCODE>[]
				{
					SC(nameof(REPLY), REPLY, PAR32, PAR32, PAR8,                               0,0,0,0,0             ),
				}),

			new KeyValuePair<byte, KeyValuePair<byte, SUBCODE>[]>(
				SOUND_SUBP,
				new KeyValuePair<byte, SUBCODE>[]
				{
					//    Sound
					SC(nameof(BREAK), BREAK,                  0,                                              0,0,0,0,0,0,0         ),
					SC(nameof(TONE), TONE, PAR8, PAR16, PAR16,                               0,0,0,0,0             ),
					SC(nameof(PLAY), PLAY, PAR8, PARS,                                      0,0,0,0,0,0           ),
					SC(nameof(REPEAT), REPEAT, PAR8, PARS,                                      0,0,0,0,0,0           ),
					SC(nameof(SERVICE), SERVICE,                0,                                              0,0,0,0,0,0,0         ),
				}),

			new KeyValuePair<byte, KeyValuePair<byte, SUBCODE>[]>(
				INPUT_SUBP,
				new KeyValuePair<byte, SUBCODE>[]
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
				}),

			new KeyValuePair<byte, KeyValuePair<byte, SUBCODE>[]>(
				MATH_SUBP,
				new KeyValuePair<byte, SUBCODE>[]
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
				}),

			new KeyValuePair<byte, KeyValuePair<byte, SUBCODE>[]>(
				COM_GET_SUBP,
				new KeyValuePair<byte, SUBCODE>[]
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
				}),

			new KeyValuePair<byte, KeyValuePair<byte, SUBCODE>[]>(
				COM_SET_SUBP,
				new KeyValuePair<byte, SUBCODE>[]
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
				}),
		};
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
    }
}
