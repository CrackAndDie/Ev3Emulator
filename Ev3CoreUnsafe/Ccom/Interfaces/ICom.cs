using Ev3CoreUnsafe.Enums;
using Ev3CoreUnsafe.Helpers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static Ev3CoreUnsafe.Defines;

namespace Ev3CoreUnsafe.Ccom.Interfaces
{
	public unsafe interface ICom
	{
		RESULT cComInit();

		RESULT cComOpen();

		RESULT cComClose();

		RESULT cComExit();

		void cComRead();

		void cComWrite();

		void cComGet();

		void cComSet();

		void cComUpdate();

		void cComTxUpdate(UBYTE ChNo);

		void cComRemove();

		DATA8 cComGetUsbStatus();
		UBYTE cComGetBtStatus();
		UBYTE cComGetWifiStatus();

		void cComReady();
		void cComTest();

		void cComReadData();
		void cComWriteData();

		void cComWriteFile();

		void cComOpenMailBox();
		void cComWriteMailBox();
		void cComReadMailBox();
		void cComTestMailBox();
		void cComReadyMailBox();
		void cComCloseMailBox();

		void cComGetBrickName(DATA8 Length, DATA8* pBrickName);
		DATA8 cComGetEvent();

		// DAISY chain
		// Write type data to chain
		RESULT cComSetDeviceInfo(DATA8 Length, UBYTE* pInfo);

		// Read type data from chain
		RESULT cComGetDeviceInfo(DATA8 Length, UBYTE* pInfo);

		// Write mode to chain
		RESULT cComSetDeviceType(DATA8 Layer, DATA8 Port, DATA8 Type, DATA8 Mode);

		// Read device data from chain
		RESULT cComGetDeviceData(DATA8 Layer, DATA8 Port, DATA8 Length, DATA8* pType, DATA8* pMode, DATA8* pData);
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct COMCMD                    //!< Common command struct
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCnt;
		public UBYTE Cmd;
        public fixed UBYTE PayLoad[256]; // WARNING: 256 is max PayLoad len       //!< Pay load is DIRCMD or SYSCMD
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct COMRPL                     //!< Common reply struct
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCnt;
		public UBYTE Cmd;
        public fixed UBYTE Path[256]; // WARNING: 256 is max path len
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct DIRCMD                       //!< Direct command struct
	{
		public UBYTE Globals;
		public UBYTE Locals;
		public fixed UBYTE Code[256]; // WARNING: 256 is max path len
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SYSCMDB                       //!< System command begin down load command
	{
		public UBYTE Sys;
		public UBYTE LengthLsb;
		public UBYTE LengthNsb1;
		public UBYTE LengthNsb2;
		public UBYTE LengthMsb;
        public fixed UBYTE Name[256]; // WARNING: 256 is max name len
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SYSCMDBUPL                     //!< System command begin Upload command
	{
		public UBYTE Sys;
		public UBYTE LengthLsb;
		public UBYTE LengthMsb;
		public fixed UBYTE Name[256]; // WARNING: 256 is max name len
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SYSCMDCUPL                     //!< System command Continue Upload command
	{
		public UBYTE Sys;
		public UBYTE Handle;
		public UBYTE LengthLsb;
		public UBYTE LengthMsb;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SYSCMDC                    //!< System command continue down load command
	{
		public UBYTE Sys;
		public UBYTE Handle;
        public fixed UBYTE Path[256]; // WARNING: 256 is max path len
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct BEGIN_LIST
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE BytesToReadLsb;
		public UBYTE BytesToReadMsb;
        public fixed UBYTE Path[256]; // WARNING: 256 is max path len
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RPLY_BEGIN_LIST
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
		public UBYTE ListSizeLsb;
		public UBYTE ListSizeNsb1;
		public UBYTE ListSizeNsb2;
		public UBYTE ListSizeMsb;
		public UBYTE Handle;
        public fixed UBYTE PayLoad[256]; // WARNING: 256 is max payload len
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct CONTINUE_LIST
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Handle;
		public UBYTE BytesToReadLsb;
		public UBYTE BytesToReadMsb;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RPLY_CONTINUE_LIST
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
		public UBYTE Handle;
        public fixed UBYTE Path[256]; // WARNING: 256 is max payload len
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct BEGIN_GET_FILE
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE BytesToReadLsb;
		public UBYTE BytesToReadMsb;
        public fixed UBYTE Path[256]; // WARNING: 256 is max file len
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RPLY_BEGIN_GET_FILE
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
		public UBYTE FileSizeLsb;
		public UBYTE FileSizeNsb1;
		public UBYTE FileSizeNsb2;
		public UBYTE FileSizeMsb;
		public UBYTE Handle;
        public fixed UBYTE PayLoad[256]; // WARNING: 256 is max PayLoad len
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct CONTINUE_GET_FILE
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Handle;
		public UBYTE BytesToReadLsb;
		public UBYTE BytesToReadMsb;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RPLY_CONTINUE_GET_FILE
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
		public UBYTE FileSizeLsb;
		public UBYTE FileSizeNsb1;
		public UBYTE FileSizeNsb2;
		public UBYTE FileSizeMsb;
		public UBYTE Handle;
        public fixed UBYTE PayLoad[256]; // WARNING: 256 is max PayLoad len
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct BEGIN_READ
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE BytesToReadLsb;
		public UBYTE BytesToReadMsb;
        public fixed UBYTE Path[256]; // WARNING: 256 is max Path len
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RPLY_BEGIN_READ
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
		public UBYTE FileSizeLsb;
		public UBYTE FileSizeNsb1;
		public UBYTE FileSizeNsb2;
		public UBYTE FileSizeMsb;
		public UBYTE Handle;
        public fixed UBYTE PayLoad[256]; // WARNING: 256 is max PayLoad len
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct CONTINUE_READ
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Handle;
		public UBYTE BytesToReadLsb;
		public UBYTE BytesToReadMsb;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RPLY_CONTINUE_READ
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
		public UBYTE Handle;
        public fixed UBYTE PayLoad[256]; // WARNING: 256 is max PayLoad len
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct LIST_HANDLES
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RPLY_LIST_HANDLES
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
        public fixed UBYTE PayLoad[256]; // WARNING: 256 is max PayLoad len
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct REMOVE_FILE
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
        public fixed UBYTE Name[256]; // WARNING: 256 is max Name len
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RPLY_REMOVE_FILE
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MAKE_DIR
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
        public fixed UBYTE Dir[256]; // WARNING: 256 is max Dir len
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RPLY_MAKE_DIR
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct CLOSE_HANDLE
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Handle;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RPLY_CLOSE_HANDLE
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
		public UBYTE Handle;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct BEGIN_DL
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE FileSizeLsb;
		public UBYTE FileSizeNsb1;
		public UBYTE FileSizeNsb2;
		public UBYTE FileSizeMsb;
        public fixed UBYTE Path[256]; // WARNING: 256 is max Path len
									  //  UBYTE   PayLoad[];
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RPLY_BEGIN_DL
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
		public UBYTE Handle;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct CONTINUE_DL
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Handle;
        public fixed UBYTE PayLoad[256]; // WARNING: 256 is max PayLoad len
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RPLY_CONTINUE_DL
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
		public UBYTE Handle;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct WRITE_MAILBOX
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE NameSize;
        public fixed UBYTE Name[256]; // WARNING: 256 is max Name len
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct WRITE_MAILBOX_PAYLOAD
	{
		public UBYTE SizeLsb;
		public UBYTE SizeMsb;
        public fixed UBYTE Payload[256]; // WARNING: 256 is max PayLoad len
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct BLUETOOTH_PIN
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE MacSize;
		public fixed UBYTE Mac[13];
		public UBYTE PinSize;
		public fixed UBYTE Pin[7];
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RPLY_BLUETOOTH_PIN
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
		public UBYTE MacSize;
		public fixed UBYTE Mac[13];
		public UBYTE PinSize;
		public fixed UBYTE Pin[7];
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct BUNDLE_ID
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE* BundleId;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RPLY_BUNDLE_ID
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct BUNDLE_SEED_ID
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
        public fixed UBYTE BundleSeedId[256]; // WARNING: 256 is max BundleSeedId len
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RPLY_BUNDLE_SEED_ID
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FIL
	{
		public fixed DATA8 Name[vmFILENAMESIZE];         //!< File name

		public int PlaceHolder; // there was dirent

		public ULONG Size;                         //!< File size
		public int File;

		public ULONG Length;                       //!< Total down load length
		public ULONG Pointer;                      //!<
		public UWORD State;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct TXBUF
	{
		public fixed UBYTE Buf[1024];
		public ULONG MsgLen;
		public ULONG BlockLen;
		public ULONG SendBytes;
		public ULONG BufSize;
		public FIL* pFile;
		public UBYTE FileHandle;
		public UBYTE RemoteFileHandle;
		public UBYTE State;
		public UBYTE SubState;
		public UBYTE Writing;
		public DATA8* pDir; // just a path
		public fixed UBYTE Folder[vmFILENAMESIZE];
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RXBUF
	{
		public fixed UBYTE Buf[1024];
		public ULONG MsgLen;
		public ULONG RxBytes;
		public ULONG BufSize;
		public FIL* pFile;
		public UBYTE FileHandle;
		public UBYTE State;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MAILBOX
	{
		public fixed UBYTE Name[50];
		public UBYTE Status;
		public UBYTE FifoSize;
		public UBYTE Type;
		public UWORD DataSize;
		public UWORD ReadCnt;
		public UWORD WriteCnt;
		public fixed ULONG Content[(MAILBOX_CONTENT_SIZE / 4) + 1];
	}

	public unsafe delegate UWORD ChannelFunc(UBYTE* data, UWORD len);

	public unsafe struct COM_GLOBALS
	{
		//*****************************************************************************
		// Com Global variables
		//*****************************************************************************
		public IMGDATA* Image; // must be aligned
		public DATA32 Alignment;
		public UBYTE* Globals;
		public UBYTE CommandReady;

		public ChannelFunc[] ReadChannel = new ChannelFunc[NO_OF_CHS];
		public ChannelFunc[] WriteChannel = new ChannelFunc[NO_OF_CHS];

		public FIL* Files;

		public TXBUF* TxBuf;
		public RXBUF* RxBuf;

		public MAILBOX* MailBox;

		public int Cmdfd;
		public UBYTE VmReady;
		public UBYTE ComResult;
		public UBYTE ActiveComCh;   // Temporary fix until com channel functionality is in place, Ch interleaving not possible
		public UBYTE ReplyStatus;

		public UBYTE* BrickName;

		public static int SizeofImage = Unsafe.SizeOf<IMGHEAD>() + Unsafe.SizeOf<OBJHEAD>() + USB_CMD_IN_REP_SIZE - Unsafe.SizeOf<DIRCMD>();

		public COM_GLOBALS()
		{
			Init();
		}

		public void Init()
		{
			Image = CommonHelper.Pointer1d<IMGDATA>(SizeofImage);
			Globals = CommonHelper.Pointer1d<UBYTE>(MAX_COMMAND_GLOBALS);
			Files = CommonHelper.Pointer1d<FIL>(MAX_FILE_HANDLES, true);
			TxBuf = CommonHelper.Pointer1d<TXBUF>(NO_OF_CHS, true);
			RxBuf = CommonHelper.Pointer1d<RXBUF>(NO_OF_CHS, true);
			MailBox = CommonHelper.Pointer1d<MAILBOX>(NO_OF_MAILBOXES, true);
			BrickName = CommonHelper.Pointer1d<UBYTE>(vmBRICKNAMESIZE);
		}
	}
}
