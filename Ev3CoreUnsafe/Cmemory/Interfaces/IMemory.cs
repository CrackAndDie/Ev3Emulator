using Ev3CoreUnsafe.Enums;
using Ev3CoreUnsafe.Helpers;
using static Ev3CoreUnsafe.Defines;

namespace Ev3CoreUnsafe.Cmemory.Interfaces
{
	public unsafe interface IMemory
	{
		RESULT cMemoryInit();

		RESULT cMemoryOpen(PRGID PrgId, GBINDEX Size, void** pMemory);

		RESULT cMemoryClose(PRGID PrgId);

		RESULT cMemoryExit();

		RESULT cMemoryMalloc(void** ppMemory, DATA32 Size);

		RESULT cMemoryRealloc(void* pOldMemory, void** ppMemory, DATA32 Size);

		RESULT cMemoryGetPointer(PRGID PrgId, HANDLER Handle, void** pMemory);

		RESULT cMemoryArraryPointer(PRGID PrgId, HANDLER Handle, void** pMemory);

		DATA8 cMemoryGetCacheFiles();

		DATA8 cMemoryGetCacheName(DATA8 Item, DATA8 MaxLength, DATA8* pFileName, DATA8* pName);

		DATA8 cMemoryFindSubFolders(DATA8* pFolderName);

		DATA8 cMemoryGetSubFolderName(DATA8 Item, DATA8 MaxLength, DATA8* pFolderName, DATA8* pSubFolderName);

		DATA8 cMemoryFindFiles(DATA8* pFolderName);

		void cMemoryGetResourcePath(PRGID PrgId, DATA8* pString, DATA8 MaxLength);

		RESULT cMemoryGetIcon(DATA8* pFolderName, DATA8 Item, long* pImagePointer);

		RESULT cMemoryGetImage(DATA8* pText, DATA16 Size, UBYTE* pBmp);

		DSPSTAT cMemoryCloseFile(PRGID PrgId, HANDLER Handle);

		RESULT cMemoryCheckOpenWrite(DATA8* pFileName);

		RESULT cMemoryCheckFilename(DATA8* pFilename, DATA8* pPath, DATA8* pName, DATA8* pExt);

		RESULT cMemoryGetMediaName(DATA8* pChar, DATA8* pName);



		void cMemoryFile();

		void cMemoryArray();

		void cMemoryArrayWrite();

		void cMemoryArrayRead();

		void cMemoryArrayAppend();

		void* cMemoryResize(PRGID PrgId, HANDLER Handle, DATA32 Elements);

		void cMemoryFileName();



		RESULT cMemoryOpenFolder(PRGID PrgId, DATA8 Type, DATA8* pFolderName, HANDLER* pHandle);

		RESULT cMemoryGetFolderItems(PRGID PrgId, HANDLER Handle, DATA16* pItems);

		RESULT cMemoryGetItemName(PRGID PrgId, HANDLER Handle, DATA16 Item, DATA8 Length, DATA8* pName, DATA8* pType, DATA8* pPriority);

		RESULT cMemoryGetItemIcon(PRGID PrgId, HANDLER Handle, DATA16 Item, HANDLER* pHandle, DATA32* pImagePointer);

		RESULT cMemoryGetItemText(PRGID PrgId, HANDLER Handle, DATA16 Item, DATA8 Length, DATA8* pText);

		RESULT cMemorySetItemText(PRGID PrgId, HANDLER Handle, DATA16 Item, DATA8* pText);

		RESULT cMemoryGetItem(PRGID PrgId, HANDLER Handle, DATA16 Item, DATA8 Length, DATA8* pName, DATA8* pType);

		void cMemoryCloseFolder(PRGID PrgId, HANDLER* pHandle);

		void cMemoryGetUsage(DATA32* pTotal, DATA32* pFree, DATA8 Force);

		void cMemoryUsage();
	}

	public unsafe struct POOL
	{
		public void* pPool;
		public GBINDEX Size;
		public DATA8 Type;
	}

	public unsafe struct DESCR
	{
		public DATA32 Elements;
		public DATA32 UsedElements;
		public DATA8 ElementSize;
		public DATA8 Type;
		public DATA8 Free1;
		public DATA8 Free2;
		public DATA8* pArray; // Must be aligned
	}

	public unsafe struct FDESCR
	{
		[Obsolete("Do not use handler, just open and close")]
		public int hFile;
		public DATA8 Access;
		public fixed DATA8 Filename[vmFILENAMESIZE];
	}

	public unsafe struct MEMORY_GLOBALS
	{
		//*****************************************************************************
		// Memory Global variables
		//*****************************************************************************

		public DATA32 SyncTime;
		public DATA32 SyncTick;

		//DATA32  BytesUsed;
		public DATA8** PathList;
		public POOL** pPoolList;

		[Obsolete("Programs are not cached here")]
		public DATA8** Cache;

		public MEMORY_GLOBALS()
		{
			Init();
		}

		public void Init()
		{
			PathList = CommonHelper.Pointer2d<DATA8>(MAX_PROGRAMS, vmPATHSIZE);
			pPoolList = CommonHelper.Pointer2d<POOL>(MAX_PROGRAMS, MAX_HANDLES, true);
			Cache = CommonHelper.Pointer2d<DATA8>(CACHE_DEEPT + 1, vmFILENAMESIZE);
		}
	}

	// c_memory.c
	[Obsolete("DO NOT CAST IT FROM BYTE ARRAY")]
	public unsafe struct FOLDER
	{
		public DATA8* pDir; // path
		public DATA16 Entries;
		public DATA8 Type;
		public DATA8 Sort;
		public fixed DATA8 Folder[MAX_FILENAME_SIZE];
		public DATA8** Entry;
		public fixed DATA8 Priority[DIR_DEEPT];

		public FOLDER()
		{
			Init();
		}

		public void Init()
		{
			Entry = CommonHelper.Pointer2d<DATA8>(DIR_DEEPT, FILENAME_SIZE);
		}
	}
}
