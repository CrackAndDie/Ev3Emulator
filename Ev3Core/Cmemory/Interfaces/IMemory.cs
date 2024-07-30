using Ev3Core.Enums;
using Ev3Core.Helpers;
using static Ev3Core.Defines;

namespace Ev3Core.Cmemory.Interfaces
{
    public interface IMemory
    {
        RESULT cMemoryInit();

        RESULT cMemoryOpen(PRGID PrgId, GBINDEX Size, object[][] pMemory);

        RESULT cMemoryClose(PRGID PrgId);

        RESULT cMemoryExit();

        RESULT cMemoryMalloc(object[][] ppMemory, DATA32 Size);

        RESULT cMemoryRealloc(object[] pOldMemory, object[][] ppMemory, DATA32 Size);

        RESULT cMemoryGetPointer(PRGID PrgId, HANDLER Handle, object[][] pMemory);

        RESULT cMemoryArraryPointer(PRGID PrgId, HANDLER Handle, object[][] pMemory);

        DATA8 cMemoryGetCacheFiles();

        DATA8 cMemoryGetCacheName(DATA8 Item, DATA8 MaxLength, char[] pFileName, char[] pName);

        DATA8 cMemoryFindSubFolders(char[] pFolderName);

        DATA8 cMemoryGetSubFolderName(DATA8 Item, DATA8 MaxLength, char[] pFolderName, char[] pSubFolderName);

        DATA8 cMemoryFindFiles(char[] pFolderName);

        void cMemoryGetResourcePath(PRGID PrgId, char[] pString, DATA8 MaxLength);

        RESULT cMemoryGetIcon(DATA8[] pFolderName, DATA8 Item, DATA32[] pImagePointer);

        RESULT cMemoryGetImage(DATA8[] pText, DATA16 Size, UBYTE[] pBmp);

        DSPSTAT cMemoryCloseFile(PRGID PrgId, FileInfo Handle);

        RESULT cMemoryCheckOpenWrite(char[] pFileName);

        RESULT cMemoryCheckFilename(char[] pFilename, char[] pPath, char[] pName, char[] pExt);

        RESULT cMemoryGetMediaName(char[] pChar, char[] pName);



        void cMemoryFile();

        void cMemoryArray();

        void cMemoryArrayWrite();

        void cMemoryArrayRead();

        void cMemoryArrayAppend();

        object[] cMemoryResize(PRGID PrgId, HANDLER Handle, DATA32 Elements);

        void cMemoryFileName();



        RESULT cMemoryOpenFolder(PRGID PrgId, DATA8 Type, DATA8[] pFolderName, ref DirectoryInfo pHandle);

        RESULT cMemoryGetFolderItems(PRGID PrgId, HANDLER Handle, DATA16[] pItems);

        RESULT cMemoryGetItemName(PRGID PrgId, HANDLER Handle, DATA16 Item, DATA8 Length, DATA8[] pName, ref DATA8 pType, ref DATA8 pPriority);

        RESULT cMemoryGetItemIcon(PRGID PrgId, HANDLER Handle, DATA16 Item, ref HANDLER pHandle, DATA32[] pImagePointer);

        RESULT cMemoryGetItemText(PRGID PrgId, HANDLER Handle, DATA16 Item, DATA8 Length, DATA8[] pText);

        RESULT cMemorySetItemText(PRGID PrgId, HANDLER Handle, DATA16 Item, DATA8[] pText);

        RESULT cMemoryGetItem(PRGID PrgId, HANDLER Handle, DATA16 Item, DATA8 Length, DATA8[] pName, ref DATA8 pType);

        void cMemoryCloseFolder(PRGID PrgId, DirectoryInfo pHandle);

        void cMemoryGetUsage(ref DATA32 pTotal, ref DATA32 pFree, DATA8 Force);

        void cMemoryUsage();
    }

    public class POOL
    {
        public object[] pPool;
        public GBINDEX Size;
        public DATA8 Type;
    }

    public class DESCR
    {
        public DATA32 Elements;
        public DATA32 UsedElements;
        public DATA8 ElementSize;
        public DATA8 Type;
        public DATA8 Free1;
        public DATA8 Free2;
        public DATA8[] pArray; // Must be aligned
    }

    public class FDESCR
    {
        public int hFile;
        public DATA8 Access;
        public char[] Filename = CommonHelper.Array1d<char>(vmFILENAMESIZE);
    }

    public class MEMORY_GLOBALS
    {
        //*****************************************************************************
        // Memory Global variables
        //*****************************************************************************

        public DATA32 SyncTime;
        public DATA32 SyncTick;

        //DATA32  BytesUsed;
        public DATA8[][] PathList = CommonHelper.Array2d<DATA8>(MAX_PROGRAMS, vmPATHSIZE);
        public POOL[][] pPoolList = CommonHelper.Array2d<POOL>(MAX_PROGRAMS, MAX_HANDLES, true);

        public DATA8[][] Cache = CommonHelper.Array2d<DATA8>(CACHE_DEEPT + 1, vmFILENAMESIZE);

    }
}
