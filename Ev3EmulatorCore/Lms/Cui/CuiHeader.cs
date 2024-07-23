using EV3DecompilerLib.Decompile;
using Ev3EmulatorCore.Helpers;
using System.Security.Cryptography.X509Certificates;

namespace Ev3EmulatorCore.Lms.Cui
{
	// types to check https://github.com/mindboards/ev3sources/blob/master/lms2012/lms2012/source/lmstypes.h
	// the file is https://github.com/mindboards/ev3sources/blob/master/lms2012/c_ui/source/c_ui.h
	public partial class CuiClass
	{
        public static KeyValuePair<lms2012.ButtonType, byte>[] MappedToReal =
        {
            new KeyValuePair<lms2012.ButtonType, byte>(lms2012.ButtonType.UP_BUTTON, 0),
            new KeyValuePair<lms2012.ButtonType, byte>(lms2012.ButtonType.ENTER_BUTTON, 1),
            new KeyValuePair<lms2012.ButtonType, byte>(lms2012.ButtonType.DOWN_BUTTON, 2),
            new KeyValuePair<lms2012.ButtonType, byte>(lms2012.ButtonType.RIGHT_BUTTON, 3),
            new KeyValuePair<lms2012.ButtonType, byte>(lms2012.ButtonType.LEFT_BUTTON, 4),
            new KeyValuePair<lms2012.ButtonType, byte>(lms2012.ButtonType.BACK_BUTTON, 5),
            new KeyValuePair<lms2012.ButtonType, byte>(lms2012.ButtonType.ANY_BUTTON, lms2012.REAL_ANY_BUTTON),
            new KeyValuePair<lms2012.ButtonType, byte>(lms2012.ButtonType.NO_BUTTON, lms2012.REAL_NO_BUTTON),
        };

		public static KeyValuePair<lms2012.FileType, lms2012.NIcon>[] FiletypeToNormalIcon =
	    {
			new KeyValuePair<lms2012.FileType, lms2012.NIcon>(lms2012.FileType.FILETYPE_UNKNOWN, lms2012.NIcon.ICON_FOLDER),
			new KeyValuePair<lms2012.FileType, lms2012.NIcon>(lms2012.FileType.TYPE_FOLDER, lms2012.NIcon.ICON_FOLDER),
			new KeyValuePair<lms2012.FileType, lms2012.NIcon>(lms2012.FileType.TYPE_SOUND, lms2012.NIcon.ICON_SOUND),
			new KeyValuePair<lms2012.FileType, lms2012.NIcon>(lms2012.FileType.TYPE_BYTECODE, lms2012.NIcon.ICON_RUN),
			new KeyValuePair<lms2012.FileType, lms2012.NIcon>(lms2012.FileType.TYPE_GRAPHICS, lms2012.NIcon.ICON_IMAGE),
			new KeyValuePair<lms2012.FileType, lms2012.NIcon>(lms2012.FileType.TYPE_DATALOG, lms2012.NIcon.ICON_OBD),
			new KeyValuePair<lms2012.FileType, lms2012.NIcon>(lms2012.FileType.TYPE_PROGRAM, lms2012.NIcon.ICON_OBP),
			new KeyValuePair<lms2012.FileType, lms2012.NIcon>(lms2012.FileType.TYPE_TEXT, lms2012.NIcon.ICON_TEXT),
		};

		byte[] DownloadSuccesSound = { (byte)lms2012.Op.INFO, (byte)lms2012.LC0((int)lms2012.InfoSubcode.GET_VOLUME), (byte)lms2012.LV0(0), (byte)lms2012.Op.SOUND, (byte)lms2012.LC0((int)lms2012.SoundSubcode.PLAY), (byte)lms2012.LV0(0), (byte)lms2012.LCS, (byte)'u', (byte)'i', (byte)'/', (byte)'D', (byte)'o', (byte)'w', (byte)'n', (byte)'l', (byte)'o', (byte)'a', (byte)'d', (byte)'S', (byte)'u', (byte)'c', (byte)'c', (byte)'e', (byte)'s', 0, (byte)lms2012.Op.SOUND_READY, (byte)lms2012.Op.OBJECT_END };
		
		public static byte[] TopLineBattIconMap =
		{
            (byte)lms2012.StatusIcon.SICON_BATT_0,           //  0
            (byte)lms2012.StatusIcon.SICON_BATT_1,           //  1
            (byte)lms2012.StatusIcon.SICON_BATT_2,           //  2
            (byte)lms2012.StatusIcon.SICON_BATT_3,           //  3
            (byte)lms2012.StatusIcon.SICON_BATT_4            //  4
        };

		public static byte[] TopLineBtIconMap =
		{
            (byte)lms2012.StatusIcon.SICON_BT_ON,            //  001
            (byte)lms2012.StatusIcon.SICON_BT_VISIBLE,       //  011
            (byte)lms2012.StatusIcon.SICON_BT_CONNECTED,     //  101
            (byte)lms2012.StatusIcon.SICON_BT_CONNVISIB,     //  111
        };

		public static byte[] TopLineWifiIconMap =
		{
            (byte)lms2012.StatusIcon.SICON_WIFI_3,            //  001
            (byte)lms2012.StatusIcon.SICON_WIFI_3,       //  011
            (byte)lms2012.StatusIcon.SICON_BT_CONNECTED,     //  101
            (byte)lms2012.StatusIcon.SICON_BT_CONNVISIB,     //  111
        };

        public class GRAPH
		{
			IntPtr pMin; // float
			IntPtr pMax; // float
			IntPtr pVal; // float
			IntPtr pOffset; // int 16
			IntPtr pSpan; // int 16
			float[][] Buffer = CommonHelper.GenerateTwoDimArray<float>(lms2012.GRAPH_BUFFERS, lms2012.GRAPH_BUFFER_SIZE);
			Int16 Pointer;
			Int16 GraphStartX;
			Int16 GraphSizeX;
			byte Type;
			byte Items;
			byte Initialized;

			float Value;
			float Inc;
			byte Down;

			public GRAPH()
			{
			}
		}

		public class NOTIFY
		{
		    public Int16 ScreenStartX;
			public Int16 ScreenStartY;
			public Int16 ScreenWidth;
			public Int16 ScreenHeight;
			public Int16 NoOfIcons;
			public Int16 NoOfChars;
			public Int16 FontWidth;
			public Int16 TextStartX;
			public Int16 TextStartY;
			public Int16 TextSpaceY;
			public Int16 TextChars;
			public Int16 TextLines;
			public byte[][] TextLine = CommonHelper.GenerateTwoDimArray<byte>(lms2012.MAX_NOTIFY_LINES, lms2012.MAX_NOTIFY_LINE_CHARS);
			public Int16 IconWidth;
			public Int16 IconHeight;
			public Int16 IconStartX;
			public Int16 IconStartY;
			public Int16 IconSpaceX;
			public Int16 LineStartX;
			public Int16 LineStartY;
			public Int16 LineEndX;
			public Int16 YesNoStartX;
			public Int16 YesNoStartY;
			public lms2012.FontType Font;
			public byte NeedUpdate;

			public NOTIFY()
			{
			}
		}

		public class IQUESTION
		{
			public Int16 ScreenStartX;
			public Int16 ScreenStartY;
			public Int16 ScreenWidth;
			public Int16 ScreenHeight;
			public Int16 Frame;
			public Int32 Icons;
			public Int16 NoOfIcons;
			public Int16 IconWidth;
			public Int16 IconHeight;
			public Int16 IconStartX;
			public Int16 IconStartY;
			public Int16 IconSpaceX;
			public Int16 PointerX;
			public Int16 SelectStartX;
			public Int16 SelectStartY;
			public Int16 SelectWidth;
			public Int16 SelectHeight;
			public Int16 SelectSpaceX;
			public byte NeedUpdate;
		}

		public class TQUESTION
		{
			public Int16 ScreenStartX;
			public Int16 ScreenStartY;
			public Int16 ScreenWidth;
			public Int16 ScreenHeight;
			public Int16 NoOfIcons;
			public Int16 IconWidth;
			public Int16 IconHeight;
			public Int16 IconStartX;
			public Int16 IconStartY;
			public Int16 IconSpaceX;
			public Int16 LineStartX;
			public Int16 LineStartY;
			public Int16 LineEndX;
			public Int16 YesNoStartX;
			public Int16 YesNoStartY;
			public Int16 YesNoSpaceX;
			public byte Default;
			public byte NeedUpdate;
		}

		public class KEYB
		{
			public Int16 ScreenStartX;
			public Int16 ScreenStartY;
			public Int16 IconStartX;
			public Int16 IconStartY;
			public Int16 TextStartX;
			public Int16 TextStartY;
			public Int16 StringStartX;
			public Int16 StringStartY;
			public Int16 KeybStartX;
			public Int16 KeybStartY;
			public Int16 KeybSpaceX;
			public Int16 KeybSpaceY;
			public Int16 KeybWidth;
			public Int16 KeybHeight;
			public Int16 PointerX;
			public Int16 PointerY;
			public Int16 OldX;
			public Int16 OldY;
			public byte Layout;
			public byte CharSet;
			public byte NeedUpdate;
		}

		public class BROWSER
		{
			public Int16 ScreenStartX;
			public Int16 ScreenStartY;
			public Int16 ScreenWidth;
			public Int16 ScreenHeight;
			public Int16 CharWidth;
			public Int16 CharHeight;
			public Int16 LineSpace;
			public Int16 LineHeight;
			public Int16 IconWidth;
			public Int16 IconHeight;
			public Int16 IconStartX;
			public Int16 IconStartY;
			public Int16 TextStartX;
			public Int16 TextStartY;
			public Int16 SelectStartX;
			public Int16 SelectStartY;
			public Int16 SelectWidth;
			public Int16 SelectHeight;
			public Int16 ScrollStartX;
			public Int16 ScrollStartY;
			public Int16 ScrollWidth;
			public Int16 ScrollHeight;
			public Int16 NobHeight;
			public Int16 ScrollSpan;

			public Int16 Chars;
			public Int16 Lines;

			public Int16 hFolders;
			public Int16 hFiles;
			public UInt16 PrgId;
			public UInt16 ObjId;

			public Int16 OldFiles;
			public Int16 Folders;                      // Number of folders [0..DIR_DEEPT]
			public Int16 OpenFolder;                   // Folder number open (0 = none) [0..DIR_DEEPT]
			public Int16 Files;                        // Number of files in open folder [0..DIR_DEEPT]
			public Int16 ItemStart;                    // Item number at top of list (shown)
			public Int16 ItemPointer;                  // Item list pointer - folder or file

			public byte NeedUpdate;                   // Flag set if returning without closing browser

			// removed some defines

			public byte[] TopFolder = new byte[lms2012.MAX_FILENAME_SIZE];
			public byte[] SubFolder = new byte[lms2012.MAX_FILENAME_SIZE];
			public byte[] FullPath = new byte[lms2012.MAX_FILENAME_SIZE];
			public byte[] Filename = new byte[lms2012.MAX_FILENAME_SIZE];
			public byte[] Text = new byte[lms2012.TEXTSIZE];

			public BROWSER()
			{
			}
		}

		public class TXTBOX
		{
			Int16 ScreenStartX;
			Int16 ScreenStartY;
			Int16 ScreenWidth;
			Int16 ScreenHeight;
			Int16 CharWidth;
			Int16 CharHeight;
			Int16 LineSpace;
			Int16 LineHeight;
			byte NeedUpdate;                   // Flag set if returning without closing browser
			Int16 TextStartX;
			Int16 TextStartY;
			Int16 SelectStartX;
			Int16 SelectStartY;
			Int16 SelectWidth;
			Int16 SelectHeight;
			Int16 ScrollStartX;
			Int16 ScrollStartY;
			Int16 ScrollWidth;
			Int16 ScrollHeight;
			Int16 NobHeight;
			Int16 ScrollSpan;

			Int16 Chars;
			Int16 Lines;

			Int16 Items;
			Int16 ItemStart;                    // Item number at top of list (shown)
			Int16 ItemPointer;                  // Item list pointer - folder or file

			byte Font;
			byte[] Text = new byte[lms2012.TEXTSIZE];

			public TXTBOX()
			{
			}
		}

		public class UI_GLOBALS
		{
			//*****************************************************************************
			// Ui Global variables
			//*****************************************************************************

			public DlcdClass.LCD LcdSafe = new DlcdClass.LCD();
			public DlcdClass.LCD LcdSave = new DlcdClass.LCD();
			public DlcdClass.LCD[] LcdPool = new DlcdClass.LCD[lms2012.LCD_STORE_LEVELS] 
			{
                new DlcdClass.LCD(),
                new DlcdClass.LCD(),
				new DlcdClass.LCD()
            };
			public DlcdClass.LCD LcdBuffer = new DlcdClass.LCD();
			public DlcdClass.LCD Lcd = new DlcdClass.LCD(); // ClcdClass.LCD

			//UI UiSafe;
			//UI* pUi;

			//ANALOG Analog;
			//ANALOG* pAnalog;

			public NOTIFY Notify = new NOTIFY();
			public TQUESTION Question = new TQUESTION();
			public IQUESTION IconQuestion = new IQUESTION();
			public BROWSER Browser = new BROWSER();
			public KEYB Keyboard = new KEYB();
			public GRAPH Graph = new GRAPH();
			public TXTBOX Txtbox = new TXTBOX();

			public int PowerFile;
			public int UiFile;
			public int AdcFile;
			public int DispFile;

			public ulong SleepTimer;

			public ulong MilliSeconds;
			public ulong RunScreenTimer;
			public ulong PowerTimer;
			public ulong VoltageTimer;

			// removed some defines here

			public float CinCnt;
			public float CoutCnt;
			public float VinCnt;

			// removed some defines here

			public float Tbatt;
			public float Vbatt;
			public float Ibatt;
			public float Imotor;
			public float Iintegrated;

			public float PowerInitialized;

			public byte UpdateState;
			public ulong UpdateStateTimer;
			public byte ReadyForWarnings;

			public byte TopLineEnabled;
			public byte RunScreenEnabled;
			public byte RunLedEnabled;
			public byte BackButtonBlocked;
			public byte RunScreenNumber;
			public byte RunScreenCounter;
			public byte Escape;

			public byte LedState;

			public byte[] ButtonState = new byte[lms2012.BUTTONS];
			public UInt16[] ButtonTimer = new UInt16[lms2012.BUTTONS];
			public UInt16[] ButtonDebounceTimer = new UInt16[lms2012.BUTTONS];
			public UInt16[] ButtonRepeatTimer = new UInt16[lms2012.BUTTONS];
			public byte Activated;

			public byte ScreenBusy;
			public byte ScreenBlocked;
			public UInt16 ScreenPrgId;
			public UInt16 ScreenObjId;

			public byte ShutDown;
			public byte Accu;
			public byte PowerShutdown;
			public byte PowerState;
			public byte VoltShutdown;
			public byte VoltageState;
			public byte Warnlight;
			public byte Warning;
			public byte WarningShowed;
			public byte WarningConfirmed;
			public byte UiUpdate;

			public byte BtOn;
			public byte WiFiOn;

			public Int16 BattIndicatorHigh;
			public Int16 BattIndicatorLow;
			public float BattWarningHigh;
			public float BattWarningLow;
			public float BattShutdownHigh;
			public float BattShutdownLow;
			public byte Event;

			public byte Click;

			public lms2012.FontType Font;

			public byte[] ImageBuffer = new byte[lms2012.IMAGEBUFFER_SIZE];

			public byte[] KeyBuffer = new byte[lms2012.KEYBUF_SIZE + 1];
			public byte KeyBufIn;
			public byte Keys;

			public byte[] UiWrBuffer = new byte[lms2012.UI_WR_BUFFER_SIZE];
			public Int16 UiWrBufferSize;

			public ulong Point;
			public ulong Size;

			public char[] HwVers = new char[lms2012.HWVERS_SIZE];
			public char[] FwVers = new char[lms2012.FWVERS_SIZE];
			public char[] FwBuild = new char[lms2012.FWBUILD_SIZE];
			public char[] OsVers = new char[lms2012.OSVERS_SIZE];
			public char[] OsBuild = new char[lms2012.OSBUILD_SIZE];
			public char[] IpAddr = new char[lms2012.IPADDR_SIZE];

			public byte Hw;

			public byte[] Globals = new byte[lms2012.MAX_COMMAND_GLOBALS];

			public UI_GLOBALS()
			{
			}
		}
	}
}
