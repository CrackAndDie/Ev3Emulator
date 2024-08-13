using Ev3CoreUnsafe.Enums;
using Ev3CoreUnsafe.Helpers;
using System.Runtime.InteropServices;
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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct POOL
	{
		public void* pPool;
		public GBINDEX Size;
		public DATA8 Type;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
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
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FOLDER
	{
		public DATA8* pDir; // path
		public DATA16 Entries;
		public DATA8 Type;
		public DATA8 Sort;
		public fixed DATA8 Folder[MAX_FILENAME_SIZE];

        #region entries
        public fixed DATA8 Entry0[FILENAME_SIZE];
		public fixed DATA8 Entry1[FILENAME_SIZE];
		public fixed DATA8 Entry2[FILENAME_SIZE];
		public fixed DATA8 Entry3[FILENAME_SIZE];
		public fixed DATA8 Entry4[FILENAME_SIZE];
		public fixed DATA8 Entry5[FILENAME_SIZE];
		public fixed DATA8 Entry6[FILENAME_SIZE];
		public fixed DATA8 Entry7[FILENAME_SIZE];
		public fixed DATA8 Entry8[FILENAME_SIZE];
		public fixed DATA8 Entry9[FILENAME_SIZE];
        public fixed DATA8 Entry10[FILENAME_SIZE];
        public fixed DATA8 Entry11[FILENAME_SIZE];
        public fixed DATA8 Entry12[FILENAME_SIZE];
        public fixed DATA8 Entry13[FILENAME_SIZE];
        public fixed DATA8 Entry14[FILENAME_SIZE];
        public fixed DATA8 Entry15[FILENAME_SIZE];
        public fixed DATA8 Entry16[FILENAME_SIZE];
        public fixed DATA8 Entry17[FILENAME_SIZE];
        public fixed DATA8 Entry18[FILENAME_SIZE];
        public fixed DATA8 Entry19[FILENAME_SIZE];
        public fixed DATA8 Entry20[FILENAME_SIZE];
        public fixed DATA8 Entry21[FILENAME_SIZE];
        public fixed DATA8 Entry22[FILENAME_SIZE];
        public fixed DATA8 Entry23[FILENAME_SIZE];
        public fixed DATA8 Entry24[FILENAME_SIZE];
        public fixed DATA8 Entry25[FILENAME_SIZE];
        public fixed DATA8 Entry26[FILENAME_SIZE];
        public fixed DATA8 Entry27[FILENAME_SIZE];
        public fixed DATA8 Entry28[FILENAME_SIZE];
        public fixed DATA8 Entry29[FILENAME_SIZE];
        public fixed DATA8 Entry30[FILENAME_SIZE];
        public fixed DATA8 Entry31[FILENAME_SIZE];
        public fixed DATA8 Entry32[FILENAME_SIZE];
        public fixed DATA8 Entry33[FILENAME_SIZE];
        public fixed DATA8 Entry34[FILENAME_SIZE];
        public fixed DATA8 Entry35[FILENAME_SIZE];
        public fixed DATA8 Entry36[FILENAME_SIZE];
        public fixed DATA8 Entry37[FILENAME_SIZE];
        public fixed DATA8 Entry38[FILENAME_SIZE];
        public fixed DATA8 Entry39[FILENAME_SIZE];
        public fixed DATA8 Entry40[FILENAME_SIZE];
        public fixed DATA8 Entry41[FILENAME_SIZE];
        public fixed DATA8 Entry42[FILENAME_SIZE];
        public fixed DATA8 Entry43[FILENAME_SIZE];
        public fixed DATA8 Entry44[FILENAME_SIZE];
        public fixed DATA8 Entry45[FILENAME_SIZE];
        public fixed DATA8 Entry46[FILENAME_SIZE];
        public fixed DATA8 Entry47[FILENAME_SIZE];
        public fixed DATA8 Entry48[FILENAME_SIZE];
        public fixed DATA8 Entry49[FILENAME_SIZE];
        public fixed DATA8 Entry50[FILENAME_SIZE];
        public fixed DATA8 Entry51[FILENAME_SIZE];
        public fixed DATA8 Entry52[FILENAME_SIZE];
        public fixed DATA8 Entry53[FILENAME_SIZE];
        public fixed DATA8 Entry54[FILENAME_SIZE];
        public fixed DATA8 Entry55[FILENAME_SIZE];
        public fixed DATA8 Entry56[FILENAME_SIZE];
        public fixed DATA8 Entry57[FILENAME_SIZE];
        public fixed DATA8 Entry58[FILENAME_SIZE];
        public fixed DATA8 Entry59[FILENAME_SIZE];
        public fixed DATA8 Entry60[FILENAME_SIZE];
        public fixed DATA8 Entry61[FILENAME_SIZE];
        public fixed DATA8 Entry62[FILENAME_SIZE];
        public fixed DATA8 Entry63[FILENAME_SIZE];
        public fixed DATA8 Entry64[FILENAME_SIZE];
        public fixed DATA8 Entry65[FILENAME_SIZE];
        public fixed DATA8 Entry66[FILENAME_SIZE];
        public fixed DATA8 Entry67[FILENAME_SIZE];
        public fixed DATA8 Entry68[FILENAME_SIZE];
        public fixed DATA8 Entry69[FILENAME_SIZE];
        public fixed DATA8 Entry70[FILENAME_SIZE];
        public fixed DATA8 Entry71[FILENAME_SIZE];
        public fixed DATA8 Entry72[FILENAME_SIZE];
        public fixed DATA8 Entry73[FILENAME_SIZE];
        public fixed DATA8 Entry74[FILENAME_SIZE];
        public fixed DATA8 Entry75[FILENAME_SIZE];
        public fixed DATA8 Entry76[FILENAME_SIZE];
        public fixed DATA8 Entry77[FILENAME_SIZE];
        public fixed DATA8 Entry78[FILENAME_SIZE];
        public fixed DATA8 Entry79[FILENAME_SIZE];
        public fixed DATA8 Entry80[FILENAME_SIZE];
        public fixed DATA8 Entry81[FILENAME_SIZE];
        public fixed DATA8 Entry82[FILENAME_SIZE];
        public fixed DATA8 Entry83[FILENAME_SIZE];
        public fixed DATA8 Entry84[FILENAME_SIZE];
        public fixed DATA8 Entry85[FILENAME_SIZE];
        public fixed DATA8 Entry86[FILENAME_SIZE];
        public fixed DATA8 Entry87[FILENAME_SIZE];
        public fixed DATA8 Entry88[FILENAME_SIZE];
        public fixed DATA8 Entry89[FILENAME_SIZE];
        public fixed DATA8 Entry90[FILENAME_SIZE];
        public fixed DATA8 Entry91[FILENAME_SIZE];
        public fixed DATA8 Entry92[FILENAME_SIZE];
        public fixed DATA8 Entry93[FILENAME_SIZE];
        public fixed DATA8 Entry94[FILENAME_SIZE];
        public fixed DATA8 Entry95[FILENAME_SIZE];
        public fixed DATA8 Entry96[FILENAME_SIZE];
        public fixed DATA8 Entry97[FILENAME_SIZE];
        public fixed DATA8 Entry98[FILENAME_SIZE];
        public fixed DATA8 Entry99[FILENAME_SIZE];
        public fixed DATA8 Entry100[FILENAME_SIZE];
        public fixed DATA8 Entry101[FILENAME_SIZE];
        public fixed DATA8 Entry102[FILENAME_SIZE];
        public fixed DATA8 Entry103[FILENAME_SIZE];
        public fixed DATA8 Entry104[FILENAME_SIZE];
        public fixed DATA8 Entry105[FILENAME_SIZE];
        public fixed DATA8 Entry106[FILENAME_SIZE];
        public fixed DATA8 Entry107[FILENAME_SIZE];
        public fixed DATA8 Entry108[FILENAME_SIZE];
        public fixed DATA8 Entry109[FILENAME_SIZE];
        public fixed DATA8 Entry110[FILENAME_SIZE];
        public fixed DATA8 Entry111[FILENAME_SIZE];
        public fixed DATA8 Entry112[FILENAME_SIZE];
        public fixed DATA8 Entry113[FILENAME_SIZE];
        public fixed DATA8 Entry114[FILENAME_SIZE];
        public fixed DATA8 Entry115[FILENAME_SIZE];
        public fixed DATA8 Entry116[FILENAME_SIZE];
        public fixed DATA8 Entry117[FILENAME_SIZE];
        public fixed DATA8 Entry118[FILENAME_SIZE];
        public fixed DATA8 Entry119[FILENAME_SIZE];
        public fixed DATA8 Entry120[FILENAME_SIZE];
        public fixed DATA8 Entry121[FILENAME_SIZE];
        public fixed DATA8 Entry122[FILENAME_SIZE];
        public fixed DATA8 Entry123[FILENAME_SIZE];
        public fixed DATA8 Entry124[FILENAME_SIZE];
        public fixed DATA8 Entry125[FILENAME_SIZE];
        public fixed DATA8 Entry126[FILENAME_SIZE];
        #endregion

        public fixed DATA8 Priority[DIR_DEEPT];
	}
}
