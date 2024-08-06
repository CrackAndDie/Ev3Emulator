using Ev3Core.Enums;
using Ev3Core.Extensions;
using Ev3Core.Helpers;
using System.IO;
using static Ev3Core.Defines;

namespace Ev3Core.Cmemory.Interfaces
{
    public interface IMemory
    {
        RESULT cMemoryInit();

        RESULT cMemoryOpen(PRGID PrgId, GBINDEX Size, ArrayPointer<UBYTE> pMemory);

        RESULT cMemoryClose(PRGID PrgId);

        RESULT cMemoryExit();

        RESULT cMemoryMalloc(ArrayPointer<UBYTE> ppMemory, DATA32 Size);

        RESULT cMemoryRealloc(ArrayPointer<UBYTE> pOldMemory, ArrayPointer<UBYTE> ppMemory, DATA32 Size);

        RESULT cMemoryGetPointer(PRGID PrgId, HANDLER Handle, ArrayPointer<UBYTE> pMemory);

        RESULT cMemoryArraryPointer(PRGID PrgId, HANDLER Handle, ArrayPointer<UBYTE> pMemory);

        DATA8 cMemoryGetCacheFiles();

        DATA8 cMemoryGetCacheName(DATA8 Item, DATA8 MaxLength, ArrayPointer<UBYTE> pFileName, ArrayPointer<UBYTE> pName);

        DATA8 cMemoryFindSubFolders(ArrayPointer<UBYTE> pFolderName);

        DATA8 cMemoryGetSubFolderName(DATA8 Item, DATA8 MaxLength, ArrayPointer<UBYTE> pFolderName, ArrayPointer<UBYTE> pSubFolderName);

        DATA8 cMemoryFindFiles(ArrayPointer<UBYTE> pFolderName);

        void cMemoryGetResourcePath(PRGID PrgId, ArrayPointer<UBYTE> pString, DATA8 MaxLength);

        RESULT cMemoryGetIcon(ArrayPointer<UBYTE> pFolderName, DATA8 Item, ArrayPointer<UBYTE> pImagePointer);

        RESULT cMemoryGetImage(ArrayPointer<UBYTE> pText, DATA16 Size, ArrayPointer<UBYTE> pBmp);

        DSPSTAT cMemoryCloseFile(PRGID PrgId, FileInfo Handle);

        RESULT cMemoryCheckOpenWrite(ArrayPointer<UBYTE> pFileName);

        RESULT cMemoryCheckFilename(ArrayPointer<UBYTE> pFilename, ArrayPointer<UBYTE> pPath, ArrayPointer<UBYTE> pName, ArrayPointer<UBYTE> pExt);

        RESULT cMemoryGetMediaName(ArrayPointer<UBYTE> pChar, ArrayPointer<UBYTE> pName);



        void cMemoryFile();

        void cMemoryArray();

        void cMemoryArrayWrite();

        void cMemoryArrayRead();

        void cMemoryArrayAppend();

		ArrayPointer<UBYTE> cMemoryResize(PRGID PrgId, HANDLER Handle, DATA32 Elements);

        void cMemoryFileName();



        RESULT cMemoryOpenFolder(PRGID PrgId, DATA8 Type, ArrayPointer<UBYTE> pFolderName, VarPointer<DirectoryInfo> pHandle);

        RESULT cMemoryGetFolderItems(PRGID PrgId, DirectoryInfo Handle, VarPointer<DATA16> pItems);

        RESULT cMemoryGetItemName(PRGID PrgId, DirectoryInfo Handle, DATA16 Item, DATA8 Length, ArrayPointer<UBYTE> pName, VarPointer<DATA8> pType, VarPointer<DATA8> pPriority);

        RESULT cMemoryGetItemIcon(PRGID PrgId, DirectoryInfo Handle, DATA16 Item, VarPointer<FileInfo> pHandle, ArrayPointer<UBYTE> pImagePointer);

        RESULT cMemoryGetItemText(PRGID PrgId, DirectoryInfo Handle, DATA16 Item, DATA8 Length, ArrayPointer<UBYTE> pText);

        RESULT cMemorySetItemText(PRGID PrgId, DirectoryInfo Handle, DATA16 Item, ArrayPointer<UBYTE> pText);

        RESULT cMemoryGetItem(PRGID PrgId, DirectoryInfo Handle, DATA16 Item, DATA8 Length, ArrayPointer<UBYTE> pName, VarPointer<DATA8> pType);

        void cMemoryCloseFolder(PRGID PrgId, DirectoryInfo pHandle);

        void cMemoryGetUsage(VarPointer<DATA32> pTotal, VarPointer<DATA32> pFree, DATA8 Force);

        void cMemoryUsage();
    }

    public class POOL
    {
        public ArrayPointer<UBYTE> pPool = new ArrayPointer<byte>();
        public GBINDEX Size;
        public DATA8 Type;
    }

    public class DESCR : IByteArrayCastable
    {
        public DATA32 Elements
		{
			get
			{
				return CurrentPointer.GetDATA32(false, 0);
			}
			set
			{
				CurrentPointer.SetDATA32(value, false, 0);
			}
		}
		public DATA32 UsedElements
		{
			get
			{
				return CurrentPointer.GetDATA32(false, 4);
			}
			set
			{
				CurrentPointer.SetDATA32(value, false, 4);
			}
		}
		public DATA8 ElementSize
		{
			get
			{
				return CurrentPointer.GetDATA8(false, 8);
			}
			set
			{
				CurrentPointer.SetDATA8(value, false, 8);
			}
		}
		public DATA8 Type
		{
			get
			{
				return CurrentPointer.GetDATA8(false, 9);
			}
			set
			{
				CurrentPointer.SetDATA8(value, false, 9);
			}
		}
		public DATA8 Free1
		{
			get
			{
				return CurrentPointer.GetDATA8(false, 10);
			}
			set
			{
				CurrentPointer.SetDATA8(value, false, 10);
			}
		}
		public DATA8 Free2
		{
			get
			{
				return CurrentPointer.GetDATA8(false, 11);
			}
			set
			{
				CurrentPointer.SetDATA8(value, false, 11);
			}
		}
		private ArrayPointer<UBYTE> _parray = new ArrayPointer<byte>(); // Must be aligned
		public ArrayPointer<UBYTE> pArray // Must be aligned
		{
			get
			{
				var off = CurrentPointer.GetULONG(false, 12);
				return new ArrayPointer<byte>(_parray.Data, off);
			}
			set
			{
				_parray.Data = value.Data;
				CurrentPointer.SetULONG(value.Offset, false, 12);
			}
		} 

		public const int Sizeof = 16;

		public static DESCR GetObject(ArrayPointer<byte> arr, int tmpOffset = 0)
		{
			var el = new DESCR();
			el.CurrentPointer = arr.Copy(tmpOffset);
			return el;
		}

		public static ArrayPointer<DESCR> GetArray(ArrayPointer<byte> arr, int tmpOffset = 0)
		{
			List<DESCR> tmp = new List<DESCR>();
			for (int i = 0; i < arr.Length / Sizeof; ++i)
			{
				var el = new DESCR();
				el.CurrentPointer = arr.Copy(tmpOffset + (i * Sizeof));
				tmp.Add(el);
			}
			return new ArrayPointer<DESCR>(tmp.ToArray());
		}

		public GP CurrentPointer { get; set; } = new ArrayPointer<byte>(new byte[Sizeof]);
	}

    public class FDESCR : IByteArrayCastable
	{
        public int hFile
		{
			get
			{
				return CurrentPointer.GetDATA32(false, 0);
			}
			set
			{
				CurrentPointer.SetDATA32(value, false, 0);
			}
		}
		public DATA8 Access
		{
			get
			{
				return CurrentPointer.GetDATA8(false, 4);
			}
			set
			{
				CurrentPointer.SetDATA8(value, false, 4);
			}
		}
		private ArrayPointer<byte> _filename = new ArrayPointer<byte>(CommonHelper.Array1d<byte>(vmFILENAMESIZE));
        public ArrayPointer<byte> Filename
        {
			get
			{
				var off = CurrentPointer.GetULONG(false, 5); // isn't aligned?
				return new ArrayPointer<byte>(_filename.Data, off);
			}
			set
			{
				_filename.Data = value.Data;
				CurrentPointer.SetULONG(value.Offset, false, 5); // isn't aligned?
			}
		}

		public const int Sizeof = 9;

		public static FDESCR GetObject(ArrayPointer<byte> arr, int tmpOffset = 0)
		{
			var el = new FDESCR();
			el.CurrentPointer = arr.Copy(tmpOffset);
			return el;
		}

		public static ArrayPointer<FDESCR> GetArray(ArrayPointer<byte> arr, int tmpOffset = 0)
		{
			List<FDESCR> tmp = new List<FDESCR>();
			for (int i = 0; i < arr.Length / Sizeof; ++i)
			{
				var el = new FDESCR();
				el.CurrentPointer = arr.Copy(tmpOffset + (i * Sizeof));
				tmp.Add(el);
			}
			return new ArrayPointer<FDESCR>(tmp.ToArray());
		}

		public GP CurrentPointer { get; set; } = new ArrayPointer<byte>(new byte[Sizeof]);
	}

    public class MEMORY_GLOBALS
    {
        //*****************************************************************************
        // Memory Global variables
        //*****************************************************************************

        public DATA32 SyncTime;
        public DATA32 SyncTick;

        //DATA32  BytesUsed;
        public ArrayPointer<ArrayPointer<UBYTE>> PathList = ArrayPointer<UBYTE>.From2d(CommonHelper.Array2d<UBYTE>(MAX_PROGRAMS, vmPATHSIZE));
        public ArrayPointer<ArrayPointer<POOL>> pPoolList = ArrayPointer<POOL>.From2d(CommonHelper.Array2d<POOL>(MAX_PROGRAMS, MAX_HANDLES, true));

        public ArrayPointer<ArrayPointer<UBYTE>> Cache = ArrayPointer<UBYTE>.From2d(CommonHelper.Array2d<UBYTE>(CACHE_DEEPT + 1, vmFILENAMESIZE));

    }
}
