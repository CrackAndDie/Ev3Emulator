using Ev3CoreUnsafe.Enums;
using Ev3CoreUnsafe.Helpers;
using Ev3CoreUnsafe.Lms2012.Interfaces;
using System.Runtime.CompilerServices;
using static Ev3CoreUnsafe.Defines;

namespace Ev3CoreUnsafe.Cui.Interfaces
{
	public interface IUi
	{
		RESULT cUiInit();

		RESULT cUiOpen();

		void cUiUpdate(UWORD Time);

		RESULT cUiClose();

		RESULT cUiExit();

		DATA8 cUiEscape();

		void cUiTestpin(DATA8 State);

		void cUiAlive();

		void cUiFlush();

		void cUiButton();

		void cUiRead();

		void cUiWrite();

		void cUiDraw();

		void cUiKeepAlive();
	}

	public unsafe struct GRAPH
	{
		public DATAF* pMin;
		public DATAF* pMax;
		public DATAF* pVal;
		public DATA16* pOffset;
		public DATA16* pSpan;
		public fixed DATAF Buffer0[GRAPH_BUFFER_SIZE];
		public fixed DATAF Buffer1[GRAPH_BUFFER_SIZE];
		public fixed DATAF Buffer2[GRAPH_BUFFER_SIZE];
		public fixed DATAF Buffer3[GRAPH_BUFFER_SIZE];
		public fixed DATAF Buffer4[GRAPH_BUFFER_SIZE];
		public fixed DATAF Buffer5[GRAPH_BUFFER_SIZE];
		public fixed DATAF Buffer6[GRAPH_BUFFER_SIZE];
		public fixed DATAF Buffer7[GRAPH_BUFFER_SIZE];
		public DATA16 Pointer;
		public DATA16 GraphStartX;
		public DATA16 GraphSizeX;
		public DATA8 Type;
		public DATA8 Items;
		public DATA8 Initialized;

		public DATAF Value;
		public DATAF Inc;
		public DATA8 Down;
	}

	public unsafe struct NOTIFY
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
		public fixed DATA8 TextLine0[MAX_NOTIFY_LINE_CHARS];
		public fixed DATA8 TextLine1[MAX_NOTIFY_LINE_CHARS];
		public fixed DATA8 TextLine2[MAX_NOTIFY_LINE_CHARS];
		public fixed DATA8 TextLine3[MAX_NOTIFY_LINE_CHARS];
		public fixed DATA8 TextLine4[MAX_NOTIFY_LINE_CHARS];
		public fixed DATA8 TextLine5[MAX_NOTIFY_LINE_CHARS];
		public fixed DATA8 TextLine6[MAX_NOTIFY_LINE_CHARS];
		public fixed DATA8 TextLine7[MAX_NOTIFY_LINE_CHARS];
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
	}

	public unsafe struct IQUESTION
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

	public unsafe struct TQUESTION
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

	public unsafe struct KEYB
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

	public unsafe struct UI_GLOBALS
	{
		//*****************************************************************************
		// Ui Global variables
		//*****************************************************************************

		public LCD LcdSafe;
		public LCD LcdSave;
		public LCD* LcdPool;
		public LCD LcdBuffer;
		public LCD* pLcd;

		public UI UiSafe;
		public UI* pUi;

		public ANALOG Analog;
		public ANALOG* pAnalog;

		public NOTIFY Notify;
		public TQUESTION Question;
		public IQUESTION IconQuestion;
		public BROWSER Browser;
		public KEYB Keyboard;
		public GRAPH Graph;
		public TXTBOX Txtbox;

		// redirected
		//public int PowerFile;
		//public int UiFile;
		//public int AdcFile;
		//public int DispFile;

		public ULONG SleepTimer;

		public ULONG MilliSeconds;
		public ULONG RunScreenTimer;
		public ULONG PowerTimer;
		public ULONG VoltageTimer;

		public DATAF CinCnt;
		public DATAF CoutCnt;
		public DATAF VinCnt;

		public DATA32 TempTimer;

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

		public DATA8* ButtonState;
		public DATA16* ButtonTimer;
		public DATA16* ButtonDebounceTimer;
		public DATA16* ButtonRepeatTimer;
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

		public IMGDATA* ImageBuffer;

		public DATA8* KeyBuffer;
		public DATA8 KeyBufIn;
		public DATA8 Keys;

		public DATA8* UiWrBuffer;
		public DATA16 UiWrBufferSize;

		public IMINDEX Point;
		public IMINDEX Size;

		public DATA8* HwVers;
		public DATA8* FwVers;
		public DATA8* FwBuild;
		public DATA8* OsVers;
		public DATA8* OsBuild;
		public DATA8* IpAddr;

		public DATA8 Hw;

		public IMGDATA* Globals;

		public UI_GLOBALS()
		{
			// structs 
			LcdSafe = *CommonHelper.PointerStruct<LCD>();
			LcdSave = *CommonHelper.PointerStruct<LCD>();
			LcdBuffer = *CommonHelper.PointerStruct<LCD>();
			UiSafe = *CommonHelper.PointerStruct<UI>();
			Analog = *CommonHelper.PointerStruct<ANALOG>();
			Notify = *CommonHelper.PointerStruct<NOTIFY>();
			Question = *CommonHelper.PointerStruct<TQUESTION>();
			IconQuestion = *CommonHelper.PointerStruct<IQUESTION>();
			Browser = *CommonHelper.PointerStruct<BROWSER>();
			Keyboard = *CommonHelper.PointerStruct<KEYB>();
			Graph = *CommonHelper.PointerStruct<GRAPH>();
			Txtbox = *CommonHelper.PointerStruct<TXTBOX>();

			// LCD* a = (LCD*)Unsafe.AsPointer<LCD>(ref LcdBuffer);

			LcdPool = CommonHelper.Pointer1d<LCD>(LCD_STORE_LEVELS, true);
			ButtonState = CommonHelper.Pointer1d<DATA8>(BUTTONS);
			ButtonTimer = CommonHelper.Pointer1d<DATA16>(BUTTONS);
			ButtonDebounceTimer = CommonHelper.Pointer1d<DATA16>(BUTTONS);
			ButtonRepeatTimer = CommonHelper.Pointer1d<DATA16>(BUTTONS);

			ImageBuffer = CommonHelper.Pointer1d<IMGDATA>(IMAGEBUFFER_SIZE);
			KeyBuffer = CommonHelper.Pointer1d<DATA8>(KEYBUF_SIZE + 1);
			UiWrBuffer = CommonHelper.Pointer1d<DATA8>(UI_WR_BUFFER_SIZE);

			HwVers = CommonHelper.Pointer1d<DATA8>(HWVERS_SIZE);
			FwVers = CommonHelper.Pointer1d<DATA8>(FWVERS_SIZE);
			FwBuild = CommonHelper.Pointer1d<DATA8>(FWBUILD_SIZE);
			OsVers = CommonHelper.Pointer1d<DATA8>(OSVERS_SIZE);
			OsBuild = CommonHelper.Pointer1d<DATA8>(OSBUILD_SIZE);
			IpAddr = CommonHelper.Pointer1d<DATA8>(IPADDR_SIZE);

			Globals = CommonHelper.Pointer1d<IMGDATA>(MAX_COMMAND_GLOBALS);
		}
	}
}
