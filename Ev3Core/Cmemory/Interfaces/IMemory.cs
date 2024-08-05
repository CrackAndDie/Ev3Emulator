using Ev3Core.Enums;
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

    public class DESCR : IByteCastable<DESCR>
    {
        public DATA32 Elements;
        public DATA32 UsedElements;
        public DATA8 ElementSize;
        public DATA8 Type;
        public DATA8 Free1;
        public DATA8 Free2;
        public UBYTE[] pArray; // Must be aligned

        public const int Sizeof = 16;

		public DESCR GetObject(GP buff, bool updateOffset = false)
		{
            //TODO:!!!!
            return this;
		}

		public void SetData(GP buff, bool updateOffset = false)
		{
			//TODO:!!!!
		}
	}

    public class FDESCR : IByteCastable<FDESCR>
	{
        public int hFile;
        public DATA8 Access;
        public char[] Filename = CommonHelper.Array1d<char>(vmFILENAMESIZE);

		public FDESCR GetObject(GP buff, bool updateOffset = false)
		{
            //TODO:!!!!
            return this;
		}

		public void SetData(GP buff, bool updateOffset = false)
		{
			//TODO:!!!!
		}
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
