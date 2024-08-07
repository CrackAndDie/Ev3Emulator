using Ev3CoreUnsafe.Enums;
using Ev3CoreUnsafe.Helpers;
using System.Runtime.CompilerServices;
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

	public unsafe struct COMCMD                    //!< Common command struct
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCnt;
		public UBYTE Cmd;
		public UBYTE* PayLoad;                    //!< Pay load is DIRCMD or SYSCMD
	}

	public unsafe struct COMRPL                     //!< Common reply struct
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCnt;
		public UBYTE Cmd;
		public UBYTE* PayLoad;
	}

	public unsafe struct DIRCMD                       //!< Direct command struct
	{
		public UBYTE Globals;
		public UBYTE Locals;
		public UBYTE* Code;
	}

	public unsafe struct SYSCMDB                       //!< System command begin down load command
	{
		public UBYTE Sys;
		public UBYTE LengthLsb;
		public UBYTE LengthNsb1;
		public UBYTE LengthNsb2;
		public UBYTE LengthMsb;
		public UBYTE* Name;
	}

	public unsafe struct SYSCMDBUPL                     //!< System command begin Upload command
	{
		public UBYTE Sys;
		public UBYTE LengthLsb;
		public UBYTE LengthMsb;
		public UBYTE* Name;
	}

	public unsafe struct SYSCMDCUPL                     //!< System command Continue Upload command
	{
		public UBYTE Sys;
		public UBYTE Handle;
		public UBYTE LengthLsb;
		public UBYTE LengthMsb;
	}

	public unsafe struct SYSCMDC                    //!< System command continue down load command
	{
		public UBYTE Sys;
		public UBYTE Handle;
		public UBYTE* PayLoad;
	}

	public unsafe struct BEGIN_LIST
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE BytesToReadLsb;
		public UBYTE BytesToReadMsb;
		public UBYTE* Path;
	}

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
		public UBYTE* PayLoad;
	}

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

	public unsafe struct RPLY_CONTINUE_LIST
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
		public UBYTE Handle;
		public UBYTE* PayLoad;
	}

	public unsafe struct BEGIN_GET_FILE
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE BytesToReadLsb;
		public UBYTE BytesToReadMsb;
		public UBYTE* Path;
	}

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
		public UBYTE* PayLoad;
	}

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
		public UBYTE* PayLoad;
	}

	public unsafe struct BEGIN_READ
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE BytesToReadLsb;
		public UBYTE BytesToReadMsb;
		public UBYTE* Path;
	}

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
		public UBYTE* PayLoad;
	}

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

	public unsafe struct RPLY_CONTINUE_READ
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
		public UBYTE Handle;
		public UBYTE* PayLoad;
	}

	public unsafe struct LIST_HANDLES
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
	}

	public unsafe struct RPLY_LIST_HANDLES
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
		public UBYTE* PayLoad;
	}

	public unsafe struct REMOVE_FILE
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE* Name;
	}

	public unsafe struct RPLY_REMOVE_FILE
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
	}

	public unsafe struct MAKE_DIR
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE* Dir;
	}

	public unsafe struct RPLY_MAKE_DIR
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
	}

	public unsafe struct CLOSE_HANDLE
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Handle;
	}

	public unsafe struct RPLY_CLOSE_HANDLE
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
		public UBYTE Handle;
	}

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
		public UBYTE* Path;
		//  UBYTE   PayLoad[];
	}

	public unsafe struct RPLY_BEGIN_DL
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
		public UBYTE Handle;
	}

	public unsafe struct CONTINUE_DL
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Handle;
		public UBYTE* PayLoad;
	}

	public unsafe struct RPLY_CONTINUE_DL
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
		public UBYTE Handle;
	}

	public unsafe struct WRITE_MAILBOX
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE NameSize;
		public UBYTE* Name;
	}

	public unsafe struct WRITE_MAILBOX_PAYLOAD
	{
		public UBYTE SizeLsb;
		public UBYTE SizeMsb;
		public UBYTE* Payload;
	}

	public unsafe struct BLUETOOTH_PIN
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE MacSize;
		public UBYTE* Mac;
		public UBYTE PinSize;
		public UBYTE* Pin;

		public BLUETOOTH_PIN()
		{
			Init();
		}

		public void Init()
		{
			Mac = CommonHelper.Pointer1d<UBYTE>(13);
			Pin = CommonHelper.Pointer1d<UBYTE>(7);
		}
	}

	public unsafe struct RPLY_BLUETOOTH_PIN
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
		public UBYTE MacSize;
		public UBYTE* Mac;
		public UBYTE PinSize;
		public UBYTE* Pin;

		public RPLY_BLUETOOTH_PIN()
		{
			Init();
		}

		public void Init()
		{
			Mac = CommonHelper.Pointer1d<UBYTE>(13);
			Pin = CommonHelper.Pointer1d<UBYTE>(7);
		}
	}

	public unsafe struct BUNDLE_ID
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE* BundleId;
	}

	public unsafe struct RPLY_BUNDLE_ID
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
	}

	public unsafe struct BUNDLE_SEED_ID
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE* BundleSeedId;
	}

	public unsafe struct RPLY_BUNDLE_SEED_ID
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
	}

	public unsafe struct FIL
	{
		public DATA8* Name;         //!< File name
							 // struct dirent   **namelist;
		public ULONG Size;                         //!< File size
		public int File;

		public ULONG Length;                       //!< Total down load length
		public ULONG Pointer;                      //!<
		public UWORD State;

		public FIL()
		{
			Init();
		}

		public void Init()
		{
			Name = CommonHelper.Pointer1d<DATA8>(vmFILENAMESIZE);
		}
	}


	public unsafe struct TXBUF
	{
		public UBYTE* Buf;
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
		public UBYTE* Folder;

		public TXBUF()
		{
			Init();
		}

		public void Init()
		{
			Buf = CommonHelper.Pointer1d<UBYTE>(1024);
			Folder = CommonHelper.Pointer1d<UBYTE>(vmFILENAMESIZE);
		}
	}

	public unsafe struct RXBUF
	{
		public UBYTE* Buf;
		public ULONG MsgLen;
		public ULONG RxBytes;
		public ULONG BufSize;
		public FIL* pFile;
		public UBYTE FileHandle;
		public UBYTE State;

		public RXBUF()
		{
			Init();
		}

		public void Init()
		{
			Buf = CommonHelper.Pointer1d<UBYTE>(1024);
		}
	}

	public unsafe struct MAILBOX
	{
		public UBYTE* Name;
		public UBYTE Status;
		public UBYTE FifoSize;
		public UBYTE Type;
		public UWORD DataSize;
		public UWORD ReadCnt;
		public UWORD WriteCnt;
		public ULONG* Content;

		public MAILBOX()
		{
			Init();
		}

		public void Init()
		{
			Name = CommonHelper.Pointer1d<UBYTE>(50);
			Content = CommonHelper.Pointer1d<ULONG>((MAILBOX_CONTENT_SIZE / 4) + 1);
		}
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

		public COM_GLOBALS()
		{
			Init();
		}

		public void Init()
		{
			Image = CommonHelper.Pointer1d<IMGDATA>(Unsafe.SizeOf<IMGHEAD>() + Unsafe.SizeOf<OBJHEAD>() + USB_CMD_IN_REP_SIZE - Unsafe.SizeOf<DIRCMD>());
			Globals = CommonHelper.Pointer1d<UBYTE>(MAX_COMMAND_GLOBALS);
			Files = CommonHelper.Pointer1d<FIL>(MAX_FILE_HANDLES, true);
			TxBuf = CommonHelper.Pointer1d<TXBUF>(NO_OF_CHS, true);
			RxBuf = CommonHelper.Pointer1d<RXBUF>(NO_OF_CHS, true);
			MailBox = CommonHelper.Pointer1d<MAILBOX>(NO_OF_MAILBOXES, true);
			BrickName = CommonHelper.Pointer1d<UBYTE>(vmBRICKNAMESIZE);
		}
	}
}
