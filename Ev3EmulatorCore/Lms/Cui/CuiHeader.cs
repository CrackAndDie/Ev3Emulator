using EV3DecompilerLib.Decompile;
using Ev3EmulatorCore.Helpers;
using YamlDotNet.Core.Tokens;
using static EV3DecompilerLib.Decompile.lms2012;
using static Ev3EmulatorCore.Lms.Lms2012.LmsInstance;

namespace Ev3EmulatorCore.Lms.Cui
{
	// types to check https://github.com/mindboards/ev3sources/blob/master/lms2012/lms2012/source/lmstypes.h
	// the file is https://github.com/mindboards/ev3sources/blob/master/lms2012/c_ui/source/c_ui.h
	public partial class CuiClass
	{
		public static KeyValuePair<ButtonType, DATA8>[] MappedToReal =
		{
			new KeyValuePair<ButtonType, DATA8>(ButtonType.UP_BUTTON, 0),
			new KeyValuePair<ButtonType, DATA8>(ButtonType.ENTER_BUTTON, 1),
			new KeyValuePair<ButtonType, DATA8>(ButtonType.DOWN_BUTTON, 2),
			new KeyValuePair<ButtonType, DATA8>(ButtonType.RIGHT_BUTTON, 3),
			new KeyValuePair<ButtonType, DATA8>(ButtonType.LEFT_BUTTON, 4),
			new KeyValuePair<ButtonType, DATA8>(ButtonType.BACK_BUTTON, 5),
			new KeyValuePair<ButtonType, DATA8>(ButtonType.ANY_BUTTON, REAL_ANY_BUTTON),
			new KeyValuePair<ButtonType, DATA8>(ButtonType.NO_BUTTON, REAL_NO_BUTTON),
		};

		public static KeyValuePair<FileType, DATA8>[] FiletypeToNormalIcon =
		{
			new KeyValuePair<FileType, DATA8>(FileType.FILETYPE_UNKNOWN, (DATA8)NIcon.ICON_FOLDER),
			new KeyValuePair<FileType, DATA8>(FileType.TYPE_FOLDER, (DATA8)NIcon.ICON_FOLDER),
			new KeyValuePair<FileType, DATA8>(FileType.TYPE_SOUND, (DATA8)NIcon.ICON_SOUND),
			new KeyValuePair<FileType, DATA8>(FileType.TYPE_BYTECODE, (DATA8)NIcon.ICON_RUN),
			new KeyValuePair<FileType, DATA8>(FileType.TYPE_GRAPHICS, (DATA8)NIcon.ICON_IMAGE),
			new KeyValuePair<FileType, DATA8>(FileType.TYPE_DATALOG, (DATA8)NIcon.ICON_OBD),
			new KeyValuePair<FileType, DATA8>(FileType.TYPE_PROGRAM, (DATA8)NIcon.ICON_OBP),
			new KeyValuePair<FileType, DATA8>(FileType.TYPE_TEXT, (DATA8)NIcon.ICON_TEXT),
		};

		public static KeyValuePair<Delimeter, string>[] Delimiter =
		{
			new KeyValuePair<Delimeter, string>(Delimeter.DEL_NONE, ""),
			new KeyValuePair<Delimeter, string>(Delimeter.DEL_TAB, "\t"),
			new KeyValuePair<Delimeter, string>(Delimeter.DEL_SPACE, " "),
			new KeyValuePair<Delimeter, string>(Delimeter.DEL_RETURN, "\r"),
			new KeyValuePair<Delimeter, string>(Delimeter.DEL_COLON, ":"),
			new KeyValuePair<Delimeter, string>(Delimeter.DEL_COMMA, ","),
			new KeyValuePair<Delimeter, string>(Delimeter.DEL_LINEFEED, "\n"),
			new KeyValuePair<Delimeter, string>(Delimeter.DEL_CRLF, "\r\n"),
		};

		IMGDATA[] DownloadSuccesSound = { (byte)Op.INFO, (byte)LC0((int)InfoSubcode.GET_VOLUME), (byte)LV0(0), (byte)Op.SOUND, (byte)LC0((int)SoundSubcode.PLAY), (byte)LV0(0), (byte)LCS, (byte)'u', (byte)'i', (byte)'/', (byte)'D', (byte)'o', (byte)'w', (byte)'n', (byte)'l', (byte)'o', (byte)'a', (byte)'d', (byte)'S', (byte)'u', (byte)'c', (byte)'c', (byte)'e', (byte)'s', 0, (byte)Op.SOUND_READY, (byte)Op.OBJECT_END };

		public static UBYTE[] TopLineBattIconMap =
		{
			(UBYTE)StatusIcon.SICON_BATT_0,           //  0
            (UBYTE)StatusIcon.SICON_BATT_1,           //  1
            (UBYTE)StatusIcon.SICON_BATT_2,           //  2
            (UBYTE)StatusIcon.SICON_BATT_3,           //  3
            (UBYTE)StatusIcon.SICON_BATT_4            //  4
        };

		public static UBYTE[] TopLineBtIconMap =
		{
			(UBYTE)StatusIcon.SICON_BT_ON,            //  001
            (UBYTE)StatusIcon.SICON_BT_VISIBLE,       //  011
            (UBYTE)StatusIcon.SICON_BT_CONNECTED,     //  101
            (UBYTE)StatusIcon.SICON_BT_CONNVISIB,     //  111
        };

		public static UBYTE[] TopLineWifiIconMap =
		{
			(UBYTE)StatusIcon.SICON_WIFI_3,            //  001
            (UBYTE)StatusIcon.SICON_WIFI_3,       //  011
            (UBYTE)StatusIcon.SICON_BT_CONNECTED,     //  101
            (UBYTE)StatusIcon.SICON_BT_CONNVISIB,     //  111
        };

		public unsafe struct GRAPH
		{
			public DATAF* pMin;
			public DATAF* pMax;
			public DATAF* pVal;
			public DATA16* pOffset;
			public DATA16* pSpan;
			public DATAF[][] Buffer = CommonHelper.GenerateTwoDimArray<DATAF>(GRAPH_BUFFERS, GRAPH_BUFFER_SIZE);
			public DATA16 Pointer;
			public DATA16 GraphStartX;
			public DATA16 GraphSizeX;
			public DATA8 Type;
			public DATA8 Items;
			public DATA8 Initialized;

			public DATAF Value;
			public DATAF Inc;
			public DATA8 Down;

			public GRAPH()
			{
			}
		}

		public struct NOTIFY
		{
			public DATA16 ScreenStartX;
			public DATA16 ScreenStartY;
			public DATA16 ScreenWidth;
			public DATA16 ScreenHeight;
			public DATA16 NoOfIcons;
			public DATA16 NoOfChars;
			public DATA16 FontWidth;
			public DATA16 TextStartX;
			public DATA16 TextStartY;
			public DATA16 TextSpaceY;
			public DATA16 TextChars;
			public DATA16 TextLines;
			public DATA8[][] TextLine = CommonHelper.GenerateTwoDimArray<DATA8>(MAX_NOTIFY_LINES, MAX_NOTIFY_LINE_CHARS);
			public DATA16 IconWidth;
			public DATA16 IconHeight;
			public DATA16 IconStartX;
			public DATA16 IconStartY;
			public DATA16 IconSpaceX;
			public DATA16 LineStartX;
			public DATA16 LineStartY;
			public DATA16 LineEndX;
			public DATA16 YesNoStartX;
			public DATA16 YesNoStartY;
			public DATA8 Font;
			public DATA8 NeedUpdate;

			public NOTIFY()
			{
			}
		}

		public struct IQUESTION
		{
			public DATA16 ScreenStartX;
			public DATA16 ScreenStartY;
			public DATA16 ScreenWidth;
			public DATA16 ScreenHeight;
			public DATA16 Frame;
			public DATA32 Icons;
			public DATA16 NoOfIcons;
			public DATA16 IconWidth;
			public DATA16 IconHeight;
			public DATA16 IconStartX;
			public DATA16 IconStartY;
			public DATA16 IconSpaceX;
			public DATA16 PointerX;
			public DATA16 SelectStartX;
			public DATA16 SelectStartY;
			public DATA16 SelectWidth;
			public DATA16 SelectHeight;
			public DATA16 SelectSpaceX;
			public DATA8 NeedUpdate;
		}

		public struct TQUESTION
		{
			public DATA16 ScreenStartX;
			public DATA16 ScreenStartY;
			public DATA16 ScreenWidth;
			public DATA16 ScreenHeight;
			public DATA16 NoOfIcons;
			public DATA16 IconWidth;
			public DATA16 IconHeight;
			public DATA16 IconStartX;
			public DATA16 IconStartY;
			public DATA16 IconSpaceX;
			public DATA16 LineStartX;
			public DATA16 LineStartY;
			public DATA16 LineEndX;
			public DATA16 YesNoStartX;
			public DATA16 YesNoStartY;
			public DATA16 YesNoSpaceX;
			public DATA8 Default;
			public DATA8 NeedUpdate;
		}

		public struct KEYB
		{
			public DATA16 ScreenStartX;
			public DATA16 ScreenStartY;
			public DATA16 IconStartX;
			public DATA16 IconStartY;
			public DATA16 TextStartX;
			public DATA16 TextStartY;
			public DATA16 StringStartX;
			public DATA16 StringStartY;
			public DATA16 KeybStartX;
			public DATA16 KeybStartY;
			public DATA16 KeybSpaceX;
			public DATA16 KeybSpaceY;
			public DATA16 KeybWidth;
			public DATA16 KeybHeight;
			public DATA16 PointerX;
			public DATA16 PointerY;
			public DATA16 OldX;
			public DATA16 OldY;
			public DATA8 Layout;
			public DATA8 CharSet;
			public DATA8 NeedUpdate;
		}

		public unsafe struct BROWSER
		{
			public DATA16 ScreenStartX;
			public DATA16 ScreenStartY;
			public DATA16 ScreenWidth;
			public DATA16 ScreenHeight;
			public DATA16 CharWidth;
			public DATA16 CharHeight;
			public DATA16 LineSpace;
			public DATA16 LineHeight;
			public DATA16 IconWidth;
			public DATA16 IconHeight;
			public DATA16 IconStartX;
			public DATA16 IconStartY;
			public DATA16 TextStartX;
			public DATA16 TextStartY;
			public DATA16 SelectStartX;
			public DATA16 SelectStartY;
			public DATA16 SelectWidth;
			public DATA16 SelectHeight;
			public DATA16 ScrollStartX;
			public DATA16 ScrollStartY;
			public DATA16 ScrollWidth;
			public DATA16 ScrollHeight;
			public DATA16 NobHeight;
			public DATA16 ScrollSpan;

			public DATA16 Chars;
			public DATA16 Lines;

			public HANDLER hFolders;
			public HANDLER hFiles;
			public PRGID PrgId;
			public OBJID ObjId;

			public DATA16 OldFiles;
			public DATA16 Folders;                      // Number of folders [0..DIR_DEEPT]
			public DATA16 OpenFolder;                   // Folder number open (0 = none) [0..DIR_DEEPT]
			public DATA16 Files;                        // Number of files in open folder [0..DIR_DEEPT]
			public DATA16 ItemStart;                    // Item number at top of list (shown)
			public DATA16 ItemPointer;                  // Item list pointer - folder or file

			public DATA8 NeedUpdate;                   // Flag set if returning without closing browser

			public DATA8 Sdcard;

			public DATA8 Usbstick;

			public fixed DATA8 TopFolder[MAX_FILENAME_SIZE];
			public fixed DATA8 SubFolder[MAX_FILENAME_SIZE];
			public fixed DATA8 FullPath[MAX_FILENAME_SIZE];
			public fixed DATA8 Filename[MAX_FILENAME_SIZE];
			public fixed DATA8 Text[TEXTSIZE];
		}

		public unsafe struct TXTBOX
		{
			public DATA16 ScreenStartX;
			public DATA16 ScreenStartY;
			public DATA16 ScreenWidth;
			public DATA16 ScreenHeight;
			public DATA16 CharWidth;
			public DATA16 CharHeight;
			public DATA16 LineSpace;
			public DATA16 LineHeight;
			public DATA8 NeedUpdate;                   // Flag set if returning without closing browser
			public DATA16 TextStartX;
			public DATA16 TextStartY;
			public DATA16 SelectStartX;
			public DATA16 SelectStartY;
			public DATA16 SelectWidth;
			public DATA16 SelectHeight;
			public DATA16 ScrollStartX;
			public DATA16 ScrollStartY;
			public DATA16 ScrollWidth;
			public DATA16 ScrollHeight;
			public DATA16 NobHeight;
			public DATA16 ScrollSpan;

			public DATA16 Chars;
			public DATA16 Lines;

			public DATA16 Items;
			public DATA16 ItemStart;                    // Item number at top of list (shown)
			public DATA16 ItemPointer;                  // Item list pointer - folder or file

			public DATA8 Font;
			public fixed DATA8 Text[TEXTSIZE];
		}

		public unsafe struct UI
		{
			public fixed DATA8 Pressed[BUTTONS];                   //!< Pressed status
		}

		public unsafe struct UI_GLOBALS
		{
			//*****************************************************************************
			// Ui Global variables
			//*****************************************************************************

			public LCD LcdSafe;
			public LCD LcdSave;
			public LCD[] LcdPool = new LCD[LCD_STORE_LEVELS]
			{
				new LCD(),
				new LCD(),
				new LCD(),
			};
			public LCD LcdBuffer;
			public LCD* pLcd;

			public UI UiSafe;
			public UI* pUi;

			public ANALOG Analog;
			public ANALOG* pAnalog;

			public	NOTIFY Notify;
			public TQUESTION Question;
			public IQUESTION IconQuestion;
			public BROWSER Browser;
			public KEYB Keyboard;
			public GRAPH Graph;
			public TXTBOX Txtbox;

			public int PowerFile;
			public int UiFile;
			public int AdcFile;
			public int DispFile;

			public ULONG SleepTimer;

			public ULONG MilliSeconds;
			public ULONG RunScreenTimer;
			public ULONG PowerTimer;
			public ULONG VoltageTimer;

			public DATAF CinCnt;
			public DATAF CoutCnt;
			public DATAF VinCnt;

			public DATAF Tbatt;
			public DATAF Vbatt;
			public DATAF Ibatt;
			public DATAF Imotor;
			public DATAF Iintegrated;

			public UBYTE PowerInitialized;

			public DATA8 UpdateState;
			public ULONG UpdateStateTimer;
			public UBYTE ReadyForWarnings;

			public DATA8 TopLineEnabled;
			public DATA8 RunScreenEnabled;
			public DATA8 RunLedEnabled;
			public DATA8 BackButtonBlocked;
			public DATA8 RunScreenNumber;
			public DATA8 RunScreenCounter;
			public DATA8 Escape;

			public DATA8 LedState;

			public fixed DATA8 ButtonState[BUTTONS];
			public fixed DATA16 ButtonTimer[BUTTONS];
			public fixed DATA16 ButtonDebounceTimer[BUTTONS];
			public fixed DATA16 ButtonRepeatTimer[BUTTONS];
			public DATA8 Activated;

			public DATA8 ScreenBusy;
			public DATA8 ScreenBlocked;
			public PRGID ScreenPrgId;
			public OBJID ScreenObjId;

			public DATA8 ShutDown;
			public DATA8 Accu;
			public DATA8 PowerShutdown;
			public DATA8 PowerState;
			public DATA8 VoltShutdown;
			public DATA8 VoltageState;
			public DATA8 Warnlight;
			public DATA8 Warning;
			public DATA8 WarningShowed;
			public DATA8 WarningConfirmed;
			public DATA8 UiUpdate;

			public DATA8 BtOn;
			public DATA8 WiFiOn;

			public DATA16 BattIndicatorHigh;
			public DATA16 BattIndicatorLow;
			public DATAF BattWarningHigh;
			public DATAF BattWarningLow;
			public DATAF BattShutdownHigh;
			public DATAF BattShutdownLow;

			public DATA8 Event;

			public DATA8 Click;

			public DATA8 Font;

			public fixed IMGDATA ImageBuffer[IMAGEBUFFER_SIZE];

			public fixed DATA8 KeyBuffer[KEYBUF_SIZE + 1];
			public DATA8 KeyBufIn;
			public DATA8 Keys;

			public fixed DATA8 UiWrBuffer[UI_WR_BUFFER_SIZE];
			public DATA16 UiWrBufferSize;

			public IMINDEX Point;
			public IMINDEX Size;

			public fixed DATA8 HwVers[HWVERS_SIZE];
			public fixed DATA8 FwVers[FWVERS_SIZE];
			public fixed DATA8 FwBuild[FWBUILD_SIZE];
			public fixed DATA8 OsVers[OSVERS_SIZE];
			public fixed DATA8 OsBuild[OSBUILD_SIZE];
			public fixed DATA8 IpAddr[IPADDR_SIZE];

			public DATA8 Hw;

			public fixed IMGDATA Globals[MAX_COMMAND_GLOBALS];

			public UI_GLOBALS()
			{
			}
		}
	}
}
