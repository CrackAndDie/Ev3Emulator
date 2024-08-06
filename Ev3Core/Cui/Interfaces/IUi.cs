using Ev3Core.Enums;
using Ev3Core.Helpers;
using Ev3Core.Lms2012.Interfaces;
using static Ev3Core.Defines;

namespace Ev3Core.Cui.Interfaces
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

    public class GRAPH
    {
        public DATAF[] pMin;
        public DATAF[] pMax;
        public DATAF[] pVal;
        public DATA16[] pOffset;
        public DATA16[] pSpan;
        public DATAF[][] Buffer = CommonHelper.Array2d<DATAF>(GRAPH_BUFFERS, GRAPH_BUFFER_SIZE);
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

    public class NOTIFY
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
        public ArrayPointer<ArrayPointer<UBYTE>> TextLine = ArrayPointer<UBYTE>.From2d(CommonHelper.Array2d<UBYTE>(MAX_NOTIFY_LINES, MAX_NOTIFY_LINE_CHARS));
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

    public class IQUESTION
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

    public class TQUESTION
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

    public class KEYB
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

    public class BROWSER
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

        public VarPointer<DirectoryInfo> hFolders = new VarPointer<DirectoryInfo>();
        public VarPointer<DirectoryInfo> hFiles = new VarPointer<DirectoryInfo>();
		public PRGID PrgId;
        public OBJID ObjId;

        public DATA16 OldFiles;
        public VarPointer<DATA16> Folders;                      // Number of folders [0..DIR_DEEPT]
        public DATA16 OpenFolder;                   // Folder number open (0 = none) [0..DIR_DEEPT]
        public VarPointer<DATA16> Files;                        // Number of files in open folder [0..DIR_DEEPT]
        public DATA16 ItemStart;                    // Item number at top of list (shown)
        public VarPointer<DATA16> ItemPointer;                  // Item list pointer - folder or file

        public DATA8 NeedUpdate;                   // Flag set if returning without closing browser

        public DATA8 Sdcard;

        public DATA8 Usbstick;

        public ArrayPointer<UBYTE> TopFolder = new ArrayPointer<UBYTE>(CommonHelper.Array1d<UBYTE>(MAX_FILENAME_SIZE));
        public ArrayPointer<UBYTE> SubFolder = new ArrayPointer<UBYTE>(CommonHelper.Array1d<UBYTE>(MAX_FILENAME_SIZE));
        public ArrayPointer<UBYTE> FullPath = new ArrayPointer<UBYTE>(CommonHelper.Array1d<UBYTE>(MAX_FILENAME_SIZE));
        public ArrayPointer<UBYTE> Filename = new ArrayPointer<UBYTE>(CommonHelper.Array1d<UBYTE>(MAX_FILENAME_SIZE));
        public ArrayPointer<UBYTE> Text = new ArrayPointer<UBYTE>(CommonHelper.Array1d<UBYTE>(TEXTSIZE));
    }

    public class TXTBOX
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

        public VarPointer<DATA8> Font = new VarPointer<sbyte>();
        public ArrayPointer<UBYTE> Text = new ArrayPointer<UBYTE>(CommonHelper.Array1d<UBYTE>(TEXTSIZE));
    }

    public class UI_GLOBALS
    {
        //*****************************************************************************
        // Ui Global variables
        //*****************************************************************************

        public LCD LcdSafe = new LCD();
        public LCD LcdSave = new LCD();
        public LCD[] LcdPool = CommonHelper.Array1d<LCD>(LCD_STORE_LEVELS, true);
        public LCD LcdBuffer = new LCD();
        public LCD pLcd = new LCD();

        public UI UiSafe = new UI();
        public UI pUi = new UI();

        public ANALOG Analog = new ANALOG();

        public NOTIFY Notify = new NOTIFY();
        public TQUESTION Question = new TQUESTION();
        public IQUESTION IconQuestion = new IQUESTION();
        public BROWSER Browser = new BROWSER();
        public KEYB Keyboard = new KEYB();
        public GRAPH Graph = new GRAPH();
        public TXTBOX Txtbox = new TXTBOX();

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

        public DATA8[] ButtonState = CommonHelper.Array1d<DATA8>(BUTTONS);
        public DATA16[] ButtonTimer = CommonHelper.Array1d<DATA16>(BUTTONS);
        public DATA16[] ButtonDebounceTimer = CommonHelper.Array1d<DATA16>(BUTTONS);
        public DATA16[] ButtonRepeatTimer = CommonHelper.Array1d<DATA16>(BUTTONS);
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

        public ArrayPointer<UBYTE> ImageBuffer = new ArrayPointer<UBYTE>(CommonHelper.Array1d<IMGDATA>(IMAGEBUFFER_SIZE));

        public ArrayPointer<UBYTE> KeyBuffer = new ArrayPointer<UBYTE>(CommonHelper.Array1d<UBYTE>(KEYBUF_SIZE + 1));
        public DATA8 KeyBufIn;
        public DATA8 Keys;

        public ArrayPointer<UBYTE> UiWrBuffer = new ArrayPointer<UBYTE>(CommonHelper.Array1d<UBYTE>(UI_WR_BUFFER_SIZE));
        public DATA16 UiWrBufferSize;

        public IMINDEX Point;
        public IMINDEX Size;

        public ArrayPointer<UBYTE> HwVers = new ArrayPointer<UBYTE>(CommonHelper.Array1d<UBYTE>(HWVERS_SIZE));
        public ArrayPointer<UBYTE> FwVers = new ArrayPointer<UBYTE>(CommonHelper.Array1d<UBYTE>(FWVERS_SIZE));
        public ArrayPointer<UBYTE> FwBuild = new ArrayPointer<UBYTE>(CommonHelper.Array1d<UBYTE>(FWBUILD_SIZE));
        public ArrayPointer<UBYTE> OsVers = new ArrayPointer<UBYTE>(CommonHelper.Array1d<UBYTE>(OSVERS_SIZE));
        public ArrayPointer<UBYTE> OsBuild = new ArrayPointer<UBYTE>(CommonHelper.Array1d<UBYTE>(OSBUILD_SIZE));
        public ArrayPointer<UBYTE> IpAddr = new ArrayPointer<UBYTE>(CommonHelper.Array1d<UBYTE>(IPADDR_SIZE));

        public DATA8 Hw;

        public ArrayPointer<UBYTE> Globals = new ArrayPointer<UBYTE>(CommonHelper.Array1d<IMGDATA>(MAX_COMMAND_GLOBALS));
    }
}
