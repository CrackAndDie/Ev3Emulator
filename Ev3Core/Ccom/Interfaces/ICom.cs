using Ev3Core.Ccom.Interfaces;
using Ev3Core.Enums;
using Ev3Core.Helpers;
using static Ev3Core.Defines;

namespace Ev3Core.Ccom.Interfaces
{
	public interface ICom
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

		void cComGetBrickName(DATA8 Length, ArrayPointer<UBYTE> pBrickName);
		DATA8 cComGetEvent();

		// DAISY chain
		// Write type data to chain
		RESULT cComSetDeviceInfo(DATA8 Length, ArrayPointer<UBYTE> pInfo);

		// Read type data from chain
		RESULT cComGetDeviceInfo(DATA8 Length, ArrayPointer<UBYTE> pInfo);

		// Write mode to chain
		RESULT cComSetDeviceType(DATA8 Layer, DATA8 Port, DATA8 Type, DATA8 Mode);

		// Read device data from chain
		RESULT cComGetDeviceData(DATA8 Layer, DATA8 Port, DATA8 Length, VarPointer<DATA8> pType, VarPointer<DATA8> pMode, ArrayPointer<UBYTE> pData);
	}

	public class COMCMD                        //!< Common command struct
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCnt;
		public UBYTE Cmd;
		public UBYTE[] PayLoad;                    //!< Pay load is DIRCMD or SYSCMD
	}

	public class COMRPL                       //!< Common reply struct
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCnt;
		public UBYTE Cmd;
		public UBYTE[] PayLoad;
	}

	public class DIRCMD                       //!< Direct command struct
	{
		public UBYTE Globals;
		public UBYTE Locals;
		public UBYTE[] Code;
	}

	public class SYSCMDB                      //!< System command begin down load command
	{
		public UBYTE Sys;
		public UBYTE LengthLsb;
		public UBYTE LengthNsb1;
		public UBYTE LengthNsb2;
		public UBYTE LengthMsb;
		public UBYTE[] Name;
	}

	public class SYSCMDBUPL                      //!< System command begin Upload command
	{
		public UBYTE Sys;
		public UBYTE LengthLsb;
		public UBYTE LengthMsb;
		public UBYTE[] Name;
	}

	public class SYSCMDCUPL                       //!< System command Continue Upload command
	{
		public UBYTE Sys;
		public UBYTE Handle;
		public UBYTE LengthLsb;
		public UBYTE LengthMsb;
	}

	public class SYSCMDC                      //!< System command continue down load command
	{
		public UBYTE Sys;
		public UBYTE Handle;
		public UBYTE[] PayLoad;
	}

	public class BEGIN_LIST
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE BytesToReadLsb;
		public UBYTE BytesToReadMsb;
		public UBYTE[] Path;
	}

	public class RPLY_BEGIN_LIST
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
		public UBYTE[] PayLoad;
	}

	public class CONTINUE_LIST
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Handle;
		public UBYTE BytesToReadLsb;
		public UBYTE BytesToReadMsb;
	}

	public class RPLY_CONTINUE_LIST
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
		public UBYTE Handle;
		public UBYTE[] PayLoad;
	}

	public class BEGIN_GET_FILE
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE BytesToReadLsb;
		public UBYTE BytesToReadMsb;
		public UBYTE[] Path;
	}

	public class RPLY_BEGIN_GET_FILE
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
		public UBYTE[] PayLoad;
	}

	public class CONTINUE_GET_FILE
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Handle;
		public UBYTE BytesToReadLsb;
		public UBYTE BytesToReadMsb;
	}

	public class RPLY_CONTINUE_GET_FILE
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
		public UBYTE[] PayLoad;
	}

	public class BEGIN_READ
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE BytesToReadLsb;
		public UBYTE BytesToReadMsb;
		public UBYTE[] Path;
	}

	public class RPLY_BEGIN_READ
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
		public UBYTE[] PayLoad;
	}

	public class CONTINUE_READ
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Handle;
		public UBYTE BytesToReadLsb;
		public UBYTE BytesToReadMsb;
	}

	public class RPLY_CONTINUE_READ
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
		public UBYTE Handle;
		public UBYTE[] PayLoad;
	}

	public class LIST_HANDLES
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
	}

	public class RPLY_LIST_HANDLES
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
		public UBYTE[] PayLoad;
	}

	public class REMOVE_FILE
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE[] Name;
	}

	public class RPLY_REMOVE_FILE
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
	}

	public class MAKE_DIR
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE[] Dir;
	}

	public class RPLY_MAKE_DIR
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
	}

	public class CLOSE_HANDLE
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Handle;
	}

	public class RPLY_CLOSE_HANDLE
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
		public UBYTE Handle;
	}

	public class BEGIN_DL
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE FileSizeLsb;
		public UBYTE FileSizeNsb1;
		public UBYTE FileSizeNsb2;
		public UBYTE FileSizeMsb;
		public UBYTE[] Path;
		//  UBYTE   PayLoad[];
	}

	public class RPLY_BEGIN_DL
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
		public UBYTE Handle;
	}

	public class CONTINUE_DL
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Handle;
		public UBYTE[] PayLoad;
	}

	public class RPLY_CONTINUE_DL
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
		public UBYTE Handle;
	}

	public class WRITE_MAILBOX
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE NameSize;
		public UBYTE[] Name;
	}

	public class WRITE_MAILBOX_PAYLOAD
	{
		public UBYTE SizeLsb;
		public UBYTE SizeMsb;
		public UBYTE[] Payload;
	}

	public class BLUETOOTH_PIN
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE MacSize;
		public UBYTE[] Mac = CommonHelper.Array1d<UBYTE>(13);
		public UBYTE PinSize;
		public UBYTE[] Pin = CommonHelper.Array1d<UBYTE>(7);
	}

	public class RPLY_BLUETOOTH_PIN
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
		public UBYTE MacSize;
		public UBYTE[] Mac = CommonHelper.Array1d<UBYTE>(13);
		public UBYTE PinSize;
		public UBYTE[] Pin = CommonHelper.Array1d<UBYTE>(7);
	}

	public class BUNDLE_ID
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE[] BundleId;
	}

	public class RPLY_BUNDLE_ID
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
	}

	public class BUNDLE_SEED_ID
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE[] BundleSeedId;
	}

	public class RPLY_BUNDLE_SEED_ID
	{
		public CMDSIZE CmdSize;
		public MSGCNT MsgCount;
		public UBYTE CmdType;
		public UBYTE Cmd;
		public UBYTE Status;
	}

	public class FIL
	{
		public char[] Name = CommonHelper.Array1d<char>(vmFILENAMESIZE);         //!< File name
		public ULONG Size;                         //!< File size
		public int File;

		public ULONG Length;                       //!< Total down load length
		public ULONG Pointer;                      //!<
		public UWORD State;
	}

	public class TXBUF
	{
		public UBYTE[] Buf = CommonHelper.Array1d<UBYTE>(1024);
		public ULONG MsgLen;
		public ULONG BlockLen;
		public ULONG SendBytes;
		public ULONG BufSize;
		public FIL pFile;
		public UBYTE FileHandle;
		public UBYTE RemoteFileHandle;
		public UBYTE State;
		public UBYTE SubState;
		public UBYTE Writing;
		public DirectoryInfo pDir;
		public UBYTE[] Folder = CommonHelper.Array1d<UBYTE>(vmFILENAMESIZE);
	}

	public class RXBUF
	{
		public UBYTE[] Buf = CommonHelper.Array1d<UBYTE>(1024);
		public ULONG MsgLen;
		public ULONG RxBytes;
		public ULONG BufSize;
		public FIL pFile;
		public UBYTE FileHandle;
		public UBYTE State;
	}

	public class MAILBOX
	{
		public UBYTE[] Name = CommonHelper.Array1d<UBYTE>(50);
		public UBYTE Status;
		public UBYTE FifoSize;
		public UBYTE Type;
		public UWORD DataSize;
		public UWORD ReadCnt;
		public UWORD WriteCnt;
		public ULONG[] Content = CommonHelper.Array1d<ULONG>((MAILBOX_CONTENT_SIZE / 4) + 1);
	}

	public class COM_GLOBALS
	{
		//*****************************************************************************
		// Com Global variables
		//*****************************************************************************
		[Obsolete]
		public IMGDATA[] Image = CommonHelper.Array1d<IMGDATA>(IMGHEAD.Sizeof + OBJHEAD.Sizeof + USB_CMD_IN_REP_SIZE - 6); // must be aligned // TODO: check it properly
		public DATA32 Alignment;
		public UBYTE[] Globals = CommonHelper.Array1d<UBYTE>(MAX_COMMAND_GLOBALS);
		public UBYTE CommandReady;

		public Func<ArrayPointer<UBYTE>, UWORD, UWORD>[] ReadChannel = new Func<ArrayPointer<UBYTE>, UWORD, UWORD>[NO_OF_CHS];
		public Func<ArrayPointer<UBYTE>, UWORD, UWORD>[] WriteChannel = new Func<ArrayPointer<UBYTE>, UWORD, UWORD>[NO_OF_CHS];

		public FIL[] Files = CommonHelper.Array1d<FIL>(MAX_FILE_HANDLES, true);

		public TXBUF[] TxBuf = CommonHelper.Array1d<TXBUF>(NO_OF_CHS, true);
		public RXBUF[] RxBuf = CommonHelper.Array1d<RXBUF>(NO_OF_CHS, true);

		public MAILBOX[] MailBox = CommonHelper.Array1d<MAILBOX>(NO_OF_MAILBOXES, true);

		public int Cmdfd;
		public UBYTE VmReady;
		public UBYTE ComResult;
		public UBYTE ActiveComCh;   // Temporary fix until com channel functionality is in place, Ch interleaving not possible
		public UBYTE ReplyStatus;

		public UBYTE[] BrickName = CommonHelper.Array1d<UBYTE>(vmBRICKNAMESIZE);

	}
}
