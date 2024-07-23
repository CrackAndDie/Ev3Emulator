namespace EV3DecompilerLib.Decompile
{
	public partial class lms2012
	{
		#region lms2012 defines
		internal const int MAX_PROGRAMS = 5;                 //!< Max number of programs (including UI and direct commands) running at a time
		internal const int MAX_BREAKPOINTS = 4;                     //!< Max number of breakpoints (opCODES depends on this value)
		internal const int MAX_LABELS = 32;                    //!< Max number of labels per program
		internal const int MAX_DEVICE_TYPE = 127;                   //!< Highest type number (positive)
		internal const int MAX_VALID_TYPE = 101;      //!< Highest valid type
		internal const int MAX_DEVICE_MODES = 8;                     //!< Max number of modes in one device
		internal const int MAX_DEVICE_DATASETS = 8;                     //!< Max number of data sets in one device
		internal const int MAX_DEVICE_DATALENGTH = 32;                    //!< Max device data length

		internal const int MAX_DEVICE_BUSY_TIME = 1200;                 //!< Max number of mS a device can be busy when read

		internal const int MAX_DEVICE_TYPES = ((MAX_DEVICE_TYPE + 1) * MAX_DEVICE_MODES);//!< Max number of different device types and modes (max type data list size)

		internal const int MAX_FRAMES_PER_SEC = 10;            //!< Max frames per second update in display

		internal const int CACHE_DEEPT = 10;               //!< Max number of programs cached (in RECENT FILES MENU)
		internal const int MAX_HANDLES = 500;             //!< Max number of handles to memory pools and arrays in one program

		internal const int MAX_ARRAY_SIZE = 1000000000;     //!< Max array size
		internal const int MIN_ARRAY_ELEMENTS = 0;              //!< Min elements in a DATA8 array

		internal const int INSTALLED_MEMORY = 6000;            //!< Flash allocated to hold user programs/data
		internal const int LOW_MEMORY = 500;               //!< Low memory warning [KB]

		internal const int PRINTBUFFERSIZE = 160;
		internal const int LOGBUFFER_SIZE = 1000;               //!< Min log buffer size
		internal const int DEVICE_LOGBUF_SIZE = 300;               //!< Device log buffer size (black layer buffer)
		internal const int MIN_LIVE_UPDATE_TIME = 10;               //!< [mS] Min sample time when live update

		internal const int MIN_IIC_REPEAT_TIME = 10;              //!< [mS] Min IIC device repeat time
		internal const int MAX_IIC_REPEAT_TIME = 1000;             //!< [mS] Max IIC device repeat time

		internal const int MAX_COMMAND_BYTECODES = 64;               //!< Max number of byte codes in a debug terminal direct command
		internal const int MAX_COMMAND_LOCALS = 64;              //!< Max number of bytes allocated for direct command local variables
		internal const int MAX_COMMAND_GLOBALS = 1021;             //!< Max number of bytes allocated for direct command global variables

		internal const int UI_PRIORITY = 20;               //!< UI byte codes before switching VM thread
		internal const int C_PRIORITY = 200;               //!< C call byte codes
		internal const int PRG_PRIORITY = 200;               //!< Prg byte codes before switching VM thread

		internal const int BUTTON_DEBOUNCE_TIME = 30;
		internal const int BUTTON_START_REPEAT_TIME = 400;
		internal const int BUTTON_REPEAT_TIME = 200;

		internal const int LONG_PRESS_TIME = 3000;           //!< [mS] Time pressed before long press recognised

		internal const int ADC_REF = 5000;            //!< [mV]  maximal value on ADC
		internal const int ADC_RES = 4095;           //!< [CNT] maximal count on ADC

		internal const int IN1_ID_HYSTERESIS = 50;             //!< [mV]  half of the span one Id takes up on input connection 1 voltage
		internal const int OUT5_ID_HYSTERESIS = 100;             //!< [mV]  half of the span one Id takes up on output connection 5 voltage

		internal const int DEVICE_UPDATE_TIME = 1000000;           //!< Min device (sensor) update time [nS]
		internal const int DELAY_TO_TYPEDATA = 10000;            //!< Time from daisy chain active to upload type data [mS]
		internal const int DAISYCHAIN_MODE_TIME = 10;             //!< Time for daisy chain change mode [mS]
		internal const int MAX_FILE_HANDLES = 64;              //!< Max number of down load file handles
		internal const int MIN_HANDLE = 3;               //!< Min file handle to close

		internal const int ID_LENGTH = 7;               //!< Id length  (BT MAC id) (incl. zero terminator)
		internal const int NAME_LENGTH = 12;               //!< Name length (not including zero termination)

		internal const int ERROR_BUFFER_SIZE = 8;               //!< Number of errors in buffer

		internal const string PWM_DEVICE = "lms_pwm";    //!< PWM device name
		internal const string PWM_DEVICE_NAME = "/dev/lms_pwm";   //!< PWM device file name

		internal const string MOTOR_DEVICE = "lms_motor";   //!< TACHO device name
		internal const string MOTOR_DEVICE_NAME = "/dev/lms_motor";   //!< TACHO device file name

		internal const string ANALOG_DEVICE = "lms_analog";   //!< ANALOG device name
		internal const string ANALOG_DEVICE_NAME = "/dev/lms_analog";   //!< ANALOG device file name

		internal const string DCM_DEVICE = "lms_dcm";   //!< DCM device name
		internal const string DCM_DEVICE_NAME = "/dev/lms_dcm";   //!< DCM device file name

		internal const string UART_DEVICE = "lms_uart";   //!< UART device name
		internal const string UART_DEVICE_NAME = "/dev/lms_uart";   //!< UART device file name

		internal const string IIC_DEVICE = "lms_iic";    //!< IIC device name
		internal const string IIC_DEVICE_NAME = "/dev/lms_iic";    //!< IIC device

		internal const string BT_DEVICE = "lms_bt";     //!< BT device name
		internal const string BT_DEVICE_NAME = "/dev/lms_bt";    //!< BT device

		internal const string UPDATE_DEVICE = "lms_update";  //!< UPDATE device name
		internal const string UPDATE_DEVICE_NAME = "/dev/lms_update";  //!< UPDATE device

		internal const string TEST_PIN_DEVICE = "lms_tst_pin";   //!< TEST pin device name
		internal const string TEST_PIN_DEVICE_NAME = "/dev/lms_tst_pin"; //!< TEST pin device file name

		internal const string TEST_UART_DEVICE = "lms_tst_uart";   //!< TEST UART device name
		internal const string TEST_UART_DEVICE_NAME = "/dev/lms_tst_uart";  //!< TEST UART device file name


		internal const int DIR_DEEPT = 127;    //!< Max directory items allocated

		internal const int FILENAME_SIZE = 52;                    //!< User filename size without extension including zero
		internal const int FOLDERNAME_SIZE = 10;            //!< Folder name size relative to "lms2012" folder including zero
		internal const int SUBFOLDERNAME_SIZE = FILENAME_SIZE;    //!< Sub folder name size without "/" including zero

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

		internal const int BATT_INDICATOR_HIGH = 7500;       //!< Battery indicator high [mV]
		internal const int BATT_INDICATOR_LOW = 6200;       //!< Battery indicator low [mV]

		internal const int ACCU_INDICATOR_HIGH = 7500;     //!< Rechargeable battery indicator high [mV]
		internal const int ACCU_INDICATOR_LOW = 7100;     //!< Rechargeable battery indicator low [mV]

		/*! \endverbatim
		 *  \subpage pmbattind
		 *\n
		 *  <hr size="1"/>
		 *  <b>Low Voltage Shutdown</b>
		 *\n
		 *  <hr size="1"/>
		 *  \verbatim
		 */

		internal const int LOW_VOLTAGE_SHUTDOWN_TIME = 10000;     //!< Time from shutdown lower limit to shutdown [mS]

		internal const float BATT_WARNING_HIGH = 6.2f;       //!< Battery voltage warning upper limit [V]
		internal const float BATT_WARNING_LOW = 5.5f;       //!< Battery voltage warning lower limit [V]
		internal const float BATT_SHUTDOWN_HIGH = 5.5f;      //!< Battery voltage shutdown upper limit [V]
		internal const float BATT_SHUTDOWN_LOW = 4.5f;        //!< Battery voltage shutdown lower limit [V]

		internal const float ACCU_WARNING_HIGH = 7.1f;       //!< Rechargeable battery voltage warning upper limit [V]
		internal const float ACCU_WARNING_LOW = 6.5f;       //!< Rechargeable battery voltage warning lower limit [V]
		internal const float ACCU_SHUTDOWN_HIGH = 6.5f;       //!< Rechargeable battery voltage shutdown upper limit [V]
		internal const float ACCU_SHUTDOWN_LOW = 6.0f;       //!< Rechargeable battery voltage shutdown lower limit [V]

		// other
		internal const int LC0_MIN = -31;
		internal const int LC0_MAX = 31;
		internal const sbyte DATA8_MIN = -127;
		internal const sbyte DATA8_MAX = 127;
		internal const Int16 DATA16_MIN = -32767;
		internal const Int16 DATA16_MAX = 32767;
		internal const Int32 DATA32_MIN = -2147483647;
		internal const Int32 DATA32_MAX = 2147483647;
		internal const float DATAF_MIN = -2147483647;
		internal const float DATAF_MAX = 2147483647;
		internal const byte DATA8_NAN = 0x80; //!!
		internal const UInt16 DATA16_NAN = 0x8000; //!!
		internal const UInt32 DATA32_NAN = 0x80000000; //!!
		internal const float DATAF_NAN = 0x7FC00000;

		//class Data8(LittleEndianStructure):
		//    _fields_ = [("value", c_int8)]

		//        class Data16(LittleEndianStructure):
		//    _fields_ = [("value", c_int16)]

		//        class Data32(LittleEndianStructure):
		//    _fields_ = [("value", c_int32)]

		//        class DataFloat(LittleEndianStructure):
		//    _fields_ = [("value", c_float)]

		internal const byte PRIMPAR_SHORT = 0x00;
		internal const byte PRIMPAR_LONG = 0x80;
		internal const byte PRIMPAR_CONST = 0x00;
		internal const byte PRIMPAR_VARIABLE = 0x40;
		internal const byte PRIMPAR_LOCAL = 0x00;
		internal const byte PRIMPAR_GLOBAL = 0x20;
		internal const byte PRIMPAR_HANDLE = 0x10;
		internal const byte PRIMPAR_ADDR = 0x08;
		internal const byte PRIMPAR_INDEX = 0x1F;
		internal const byte PRIMPAR_CONST_SIGN = 0x20;
		internal const byte PRIMPAR_VALUE = 0x3F;
		internal const byte PRIMPAR_BYTES = 0x07;
		internal const byte PRIMPAR_STRING_OLD = 0;
		internal const byte PRIMPAR_1_BYTE = 1;
		internal const byte PRIMPAR_2_BYTES = 2;
		internal const byte PRIMPAR_4_BYTES = 3;
		internal const byte PRIMPAR_STRING = 4;
		internal const byte PRIMPAR_LABEL = 0x20;

		internal const byte DIRECT_COMMAND_REPLY = 0x00;
		internal const byte DIRECT_COMMAND_NO_REPLY = 0x80;
		internal const byte DIRECT_REPLY = 0x02;
		internal const byte DIRECT_REPLY_ERROR = 0x04;
		internal const byte SYSTEM_COMMAND_REPLY = 0x01;
		internal const byte SYSTEM_COMMAND_NO_REPLY = 0x81;
		internal const byte SYSTEM_REPLY = 0x03;
		internal const byte SYSTEM_REPLY_ERROR = 0x05;



		public const int OUTPUTS = 4;
		public const int INPUTS = 4;
		public const int BUTTONS = 6;
		public const int LEDS = 4;

		public const byte BUTTONTYPES = 6;

		public const int LCD_WIDTH = 178;
		public const int LCD_HEIGHT = 128;
		public const int TOPLINE_HEIGHT = 10;
		public const int LCD_STORE_LEVELS = 3;

		// as methods LV LC GV
		public static int HND(int x) => (PRIMPAR_HANDLE | x);
		public static int ADR(int x) => (PRIMPAR_ADDR | x);

		public static int LCS => (PRIMPAR_LONG | PRIMPAR_STRING);

		public static (int, int) LAB1(int v) => ((PRIMPAR_LONG | PRIMPAR_LABEL), (v & 0xFF));

		public static int LC0(int v) => ((v & PRIMPAR_VALUE) | PRIMPAR_SHORT | PRIMPAR_CONST);
		public static (int, int) LC1(int v) => ((PRIMPAR_LONG | PRIMPAR_CONST | PRIMPAR_1_BYTE), (v & 0xFF));
		public static (int, int, int) LC2(int v) => ((PRIMPAR_LONG | PRIMPAR_CONST | PRIMPAR_2_BYTES), (v & 0xFF), ((v >> 8) & 0xFF));
		public static (int, int, int, int, int) LC4(int v) => ((PRIMPAR_LONG | PRIMPAR_CONST | PRIMPAR_4_BYTES), ((int)v & 0xFF), (((int)v >> 8) & 0xFF), (((int)v >> 16) & 0xFF), (((int)v >> 24) & 0xFF));
		public static (int, int) LCA(int h) => ((PRIMPAR_LONG | PRIMPAR_CONST | PRIMPAR_1_BYTE | PRIMPAR_ADDR), (h & 0xFF));

		public static int LV0(int i) => ((i & PRIMPAR_INDEX) | PRIMPAR_SHORT | PRIMPAR_VARIABLE | PRIMPAR_LOCAL);
		public static (int, int) LV1(int i) => ((PRIMPAR_LONG | PRIMPAR_VARIABLE | PRIMPAR_LOCAL | PRIMPAR_1_BYTE), (i & 0xFF));
		public static (int, int, int) LV2(int i) => ((PRIMPAR_LONG | PRIMPAR_VARIABLE | PRIMPAR_LOCAL | PRIMPAR_2_BYTES), (i & 0xFF), ((i >> 8) & 0xFF));
		public static (int, int, int, int, int) LV4(int i) => ((PRIMPAR_LONG | PRIMPAR_VARIABLE | PRIMPAR_LOCAL | PRIMPAR_4_BYTES), (i & 0xFF), ((i >> 8) & 0xFF), ((i >> 16) & 0xFF), ((i >> 24) & 0xFF));
		public static (int, int) LVA(int h) => ((PRIMPAR_LONG | PRIMPAR_VARIABLE | PRIMPAR_LOCAL | PRIMPAR_1_BYTE | PRIMPAR_ADDR), (h & 0xFF));

		public static int GV0(int i) => ((i & PRIMPAR_INDEX) | PRIMPAR_SHORT | PRIMPAR_VARIABLE | PRIMPAR_GLOBAL);
		public static (int, int) GV1(int i) => ((PRIMPAR_LONG | PRIMPAR_VARIABLE | PRIMPAR_GLOBAL | PRIMPAR_1_BYTE), (i & 0xFF));
		public static (int, int, int) GV2(int i) => ((PRIMPAR_LONG | PRIMPAR_VARIABLE | PRIMPAR_GLOBAL | PRIMPAR_2_BYTES), (i & 0xFF), ((i >> 8) & 0xFF));
		public static (int, int, int, int, int) GV4(int i) => ((PRIMPAR_LONG | PRIMPAR_VARIABLE | PRIMPAR_GLOBAL | PRIMPAR_4_BYTES), (i & 0xFF), ((i >> 8) & 0xFF), ((i >> 16) & 0xFF), ((i >> 24) & 0xFF));
		public static (int, int) GVA(int h) => ((PRIMPAR_LONG | PRIMPAR_VARIABLE | PRIMPAR_GLOBAL | PRIMPAR_1_BYTE | PRIMPAR_ADDR), (h & 0xFF));
		#endregion

		#region cUI defines
		// DEFINES
		public const int IMAGEBUFFER_SIZE = 1000;
		public const byte KEYBUF_SIZE = 100;
		public const byte UI_WR_BUFFER_SIZE = 255;

		public const byte GRAPH_BUFFERS = (INPUTS + OUTPUTS);
		public const byte GRAPH_BUFFER_SIZE = LCD_WIDTH;

		public const byte MAX_NOTIFY_LINES = 8;
		public const byte MAX_NOTIFY_LINE_CHARS = 32;

		public const byte TEXTSIZE = 24;
		public const byte MAX_FILENAME_SIZE = 255;


		public const byte HWVERS_SIZE = 6;
		public const byte FWVERS_SIZE = 7;
		public const byte FWBUILD_SIZE = 11;
		public const byte OSVERS_SIZE = 17;
		public const byte OSBUILD_SIZE = 11;
		public const byte IPADDR_SIZE = 16;
		

		public const byte BUTTON_ACTIVE = 0x01;
		public const byte BUTTON_PRESSED = 0x02;  //!< button is pressed at the moment
		public const byte BUTTON_ACTIVATED = 0x04;  //!< button has been activated since last read
		public const byte BUTTON_LONGPRESS = 0x08;  //!< button long press detected
		public const byte BUTTON_BUMBED = 0x10;  //!< button has been pressed and released
		public const byte BUTTON_LONG_LATCH = 0x20;

		public const byte BUTTON_CLR = (BUTTON_ACTIVATED | BUTTON_LONGPRESS | BUTTON_BUMBED | BUTTON_LONG_LATCH);
		public const byte BUTTON_FLUSH = (BUTTON_ACTIVATED | BUTTON_LONGPRESS | BUTTON_BUMBED | BUTTON_LONG_LATCH);

		public const byte BUTTON_ALIVE = 0x01;
		public const byte BUTTON_CLICK = 0x02;
		public const byte BUTTON_BUFPRINT = 0x04;

		public const byte BUTTON_SET = (BUTTON_ALIVE | BUTTON_CLICK);

		public const byte REAL_ANY_BUTTON = 6;
		public const byte REAL_NO_BUTTON = 7;

		// Used when no battery is present
		public const byte DEFAULT_BATTERY_VOLTAGE = 9;

		public const byte BG_COLOR = 0;
		public const byte FG_COLOR = 1;

		public const byte TOP_BATT_ICONS = 5;
		public const byte TOP_BT_ICONS = 4;
		public const byte TOP_WIFI_ICONS = 4;

		public const byte MAX_KEYB_DEEPT = 3;
		public const byte MAX_KEYB_WIDTH = 12;
		public const byte MAX_KEYB_HEIGHT = 4;
		#endregion

		#region cLcd defines
		public const int LCD_BUFFER_SIZE = (((LCD_WIDTH + 7) / 8) * LCD_HEIGHT);
		public const int LCD_TOPLINE_SIZE = (((LCD_WIDTH + 7) / 8) * (TOPLINE_HEIGHT + 1));
		#endregion
	}
}
