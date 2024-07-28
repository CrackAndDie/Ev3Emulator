using Ev3Core.Enums;

namespace Ev3Core.Memory
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

        DSPSTAT cMemoryCloseFile(PRGID PrgId, HANDLER Handle);

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



        RESULT cMemoryOpenFolder(PRGID PrgId, DATA8 Type, DATA8[] pFolderName, HANDLER* pHandle);

        RESULT cMemoryGetFolderItems(PRGID PrgId, HANDLER Handle, DATA16[] pItems);

        RESULT cMemoryGetItemName(PRGID PrgId, HANDLER Handle, DATA16 Item, DATA8 Length, DATA8[] pName, DATA8* pType, DATA8* pPriority);

        RESULT cMemoryGetItemIcon(PRGID PrgId, HANDLER Handle, DATA16 Item, HANDLER* pHandle, DATA32[] pImagePointer);

        RESULT cMemoryGetItemText(PRGID PrgId, HANDLER Handle, DATA16 Item, DATA8 Length, DATA8[] pText);

        RESULT cMemorySetItemText(PRGID PrgId, HANDLER Handle, DATA16 Item, DATA8[] pText);

        RESULT cMemoryGetItem(PRGID PrgId, HANDLER Handle, DATA16 Item, DATA8 Length, DATA8[] pName, DATA8* pType);

        void cMemoryCloseFolder(PRGID PrgId, HANDLER* pHandle);

        void cMemoryGetUsage(DATA32* pTotal, DATA32* pFree, DATA8 Force);

        void cMemoryUsage();
    }
}
