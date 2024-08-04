using Ev3Core.Cmemory.Interfaces;
using Ev3Core.Enums;
using Ev3Core.Extensions;
using Ev3Core.Helpers;
using System.Collections.Generic;
using System.Linq;
using static Ev3Core.Defines;

namespace Ev3Core.Cmemory
{
	public class Memory : IMemory
	{
		public void cMemoryGetUsage(ref int pTotal, ref int pFree, sbyte Force)
		{
			// TODO: do it normally if you want
			GH.VMInstance.MemorySize = INSTALLED_MEMORY;
			GH.VMInstance.MemoryFree = INSTALLED_MEMORY;
			pTotal = GH.VMInstance.MemorySize;
			pFree = GH.VMInstance.MemoryFree;
		}

		public RESULT cMemoryMalloc(byte[][] ppMemory, int Size, int memind = 0)
		{
			RESULT Result = RESULT.FAIL;
			DATA32 Total = 0;
			DATA32 Free = 0;

			cMemoryGetUsage(ref Total, ref Free, 0);
			if (((Size + (KB - 1)) / KB) < (Free - RESERVED_MEMORY))
			{
				ppMemory[memind] = new byte[Size];
				if (ppMemory[memind] != null)
				{
					Result = OK;
				}
			}

			return (Result);
		}

		public RESULT cMemoryMalloc(out byte[] ppMemory, int Size)
		{
			RESULT Result = RESULT.FAIL;
			DATA32 Total = 0;
			DATA32 Free = 0;

			cMemoryGetUsage(ref Total, ref Free, 0);
			ppMemory = new byte[Size];
			if (ppMemory != null)
			{
				Result = OK;
			}

			return (Result);
		}

		public RESULT cMemoryRealloc(byte[] pOldMemory, byte[][] ppMemory, int Size, int memind = 0)
		{
			RESULT Result = RESULT.FAIL;

			ppMemory[memind] = new byte[Size];
			if (ppMemory[memind] != null)
			{
				Result = OK;
			}

			return (Result);
		}

		public RESULT cMemoryRealloc(byte[] pOldMemory, out byte[] ppMemory, int Size)
		{
			RESULT Result = RESULT.FAIL;

			ppMemory = new byte[Size];
			if (ppMemory != null)
			{
				Result = OK;
			}

			return (Result);
		}

		RESULT cMemoryAlloc(PRGID PrgId, DATA8 Type, GBINDEX Size, byte[][] pMemory, ref HANDLER pHandle, int memind = 0)
		{
			RESULT Result = RESULT.FAIL;
			HANDLER TmpHandle;

			pHandle = -1;

			if ((PrgId < MAX_PROGRAMS) && (Size > 0) && (Size <= MAX_ARRAY_SIZE))
			{
				TmpHandle = 0;

				while ((TmpHandle < MAX_HANDLES) && (GH.MemoryInstance.pPoolList[PrgId][TmpHandle].pPool != null))
				{
					TmpHandle++;
				}
				if (TmpHandle < MAX_HANDLES)
				{

					if (cMemoryMalloc(out GH.MemoryInstance.pPoolList[PrgId][TmpHandle].pPool, (DATA32)Size) == OK)
					{
						pMemory[memind] = GH.MemoryInstance.pPoolList[PrgId][TmpHandle].pPool;
						GH.MemoryInstance.pPoolList[PrgId][TmpHandle].Type = Type;
						GH.MemoryInstance.pPoolList[PrgId][TmpHandle].Size = Size;
						pHandle = TmpHandle;
						Result = OK;
					}
				}
			}

			return (Result);
		}

        byte[] cMemoryReallocate(PRGID PrgId, HANDLER Handle, GBINDEX Size)
		{
            byte[] pTmp;

			pTmp = null;
			if ((PrgId < MAX_PROGRAMS) && (Handle >= 0) && (Handle < MAX_HANDLES))
			{
				if ((Size > 0) && (Size <= MAX_ARRAY_SIZE))
				{
					if (cMemoryRealloc(GH.MemoryInstance.pPoolList[PrgId][Handle].pPool, out pTmp, (DATA32)Size) == OK)
					{
						GH.MemoryInstance.pPoolList[PrgId][Handle].pPool = pTmp;
						GH.MemoryInstance.pPoolList[PrgId][Handle].Size = Size;
					}
					else
					{
						pTmp = null;
						// TODO: log out of memory
						// printf("cMemoryReallocate out of memory\r\n");
					}
				}
			}

			return (pTmp);
		}

		public RESULT cMemoryGetPointer(ushort PrgId, short Handle, byte[][] pMemory, int memind = 0)
		{
			RESULT Result = RESULT.FAIL;

			pMemory[memind] = null;

			if ((PrgId < MAX_PROGRAMS) && (Handle >= 0) && (Handle < MAX_HANDLES))
			{
				if (GH.MemoryInstance.pPoolList[PrgId][Handle].pPool != null)
				{
					pMemory[memind] = GH.MemoryInstance.pPoolList[PrgId][Handle].pPool;
					Result = OK;
				}
			}

			return (Result);
		}

		public RESULT cMemoryGetPointer(ushort PrgId, short Handle, out byte[] pMemory)
		{
			RESULT Result = RESULT.FAIL;

			pMemory = null;

			if ((PrgId < MAX_PROGRAMS) && (Handle >= 0) && (Handle < MAX_HANDLES))
			{
				if (GH.MemoryInstance.pPoolList[PrgId][Handle].pPool != null)
				{
					pMemory = GH.MemoryInstance.pPoolList[PrgId][Handle].pPool;
					Result = OK;
				}
			}

			return (Result);
		}

		public RESULT cMemoryArraryPointer(ushort PrgId, short Handle, byte[][] pMemory, int memind = 0)
		{
			RESULT Result = RESULT.FAIL;
            byte[] pTmp;

			if (cMemoryGetPointer(PrgId, Handle, out pTmp) == OK)
			{
				pMemory[memind] = pTmp;
				Result = OK;
			}

			return (Result);
		}

		public RESULT cMemoryArraryPointer(ushort PrgId, short Handle, out byte[] pMemory)
		{
			RESULT Result = RESULT.FAIL;
            byte[] pTmp;
			pMemory = null;

			if (cMemoryGetPointer(PrgId, Handle, out pTmp) == OK)
			{
				pMemory = pTmp;
				Result = OK;
			}

			return (Result);
		}

		DSPSTAT cMemoryFreeHandle(PRGID PrgId, HANDLER Handle)
		{
			DSPSTAT Result = DSPSTAT.FAILBREAK;
			FDESCR pFDescr;

			if ((PrgId < MAX_PROGRAMS) && (Handle >= 0) && (Handle < MAX_HANDLES))
			{
				if (GH.MemoryInstance.pPoolList[PrgId][Handle].pPool != null)
				{
					if (GH.MemoryInstance.pPoolList[PrgId][Handle].Type == POOL_TYPE_FILE)
					{
						pFDescr = FDESCR.FromByteArray(GH.MemoryInstance.pPoolList[PrgId][Handle].pPool);
						// TODO: wtf is here
						//if ((pFDescr.Access) != 0)
						//{
						//	pFDescr.Access = 0;
						//	close(pFDescr.hFile);
						//	sync();
						//	Result = DSPSTAT.NOBREAK;
						//}
					}
					else
					{
						Result = DSPSTAT.NOBREAK;
					}

					GH.MemoryInstance.pPoolList[PrgId][Handle].pPool = null;
					GH.MemoryInstance.pPoolList[PrgId][Handle].Size = 0;

				}
			}

			return (Result);
		}

		void cMemoryFreePool(PRGID PrgId, byte[] pMemory)
		{
			HANDLER TmpHandle;

			TmpHandle = 0;
			while ((TmpHandle < MAX_HANDLES) && (GH.MemoryInstance.pPoolList[PrgId][TmpHandle].pPool != pMemory))
			{
				TmpHandle++;
			}
			if (TmpHandle < MAX_HANDLES)
			{
				cMemoryFreeHandle(PrgId, TmpHandle);
			}
		}

		void cMemoryFreeProgram(PRGID PrgId)
		{
			HANDLER TmpHandle;

			for (TmpHandle = 0; TmpHandle < MAX_HANDLES; TmpHandle++)
			{
				cMemoryFreeHandle(PrgId, TmpHandle);
			}

			// Ensure that path is emptied
			GH.MemoryInstance.PathList[PrgId][0] = 0;
		}

		void cMemoryFreeAll()
		{
			PRGID TmpPrgId;

			for (TmpPrgId = 0; TmpPrgId < MAX_PROGRAMS; TmpPrgId++)
			{
				cMemoryFreeProgram(TmpPrgId);
			}
		}

		public RESULT cMemoryInit()
		{
			RESULT Result = RESULT.FAIL;
			DATA8 Tmp;
			PRGID TmpPrgId;
			HANDLER TmpHandle;
			FileInfo file;
			char[] PrgNameBuf = new char[vmFILENAMESIZE];

			// TODO: wtf
			//CommonHelper.Snprintf(PrgNameBuf, 0, vmFILENAMESIZE, vmSETTINGS_DIR.ToCharArray(), "/".ToCharArray(), vmLASTRUN_FILE_NAME.ToCharArray(), vmEXT_CONFIG.ToCharArray());
			//file = new FileInfo(string.Concat(PrgNameBuf));
			//if (file != null)
			//{
			//	GH.MemoryInstance.CacheFile.ReadAllBytes(file.FullName);
			//	if (read(File, GH.MemoryInstance.Cache, sizeof(MemoryInstance.Cache)) != sizeof(MemoryInstance.Cache))
			//	{
			//		file = null;
			//	}
			//}
			//if (file == null)
			//{
			//	for (Tmp = 0; Tmp < CACHE_DEEPT; Tmp++)
			//	{
			//		GH.MemoryInstance.Cache[Tmp][0] = 0;
			//	}
			//}

			for (TmpPrgId = 0; TmpPrgId < MAX_PROGRAMS; TmpPrgId++)
			{
				for (TmpHandle = 0; TmpHandle < MAX_HANDLES; TmpHandle++)
				{
					GH.MemoryInstance.pPoolList[TmpPrgId][TmpHandle].pPool = null;
				}
			}

			GH.VMInstance.MemorySize = INSTALLED_MEMORY;
			GH.VMInstance.MemoryFree = INSTALLED_MEMORY;

			GH.MemoryInstance.SyncTime = (DATA32)0;
			GH.MemoryInstance.SyncTick = (DATA32)0;

			Result = OK;

			return (Result);
		}

		public RESULT cMemoryOpen(ushort PrgId, uint Size, byte[][] pMemory)
		{
			RESULT Result = RESULT.FAIL;
			HANDLER TmpHandle = 0;

			Result = cMemoryAlloc(PrgId, POOL_TYPE_MEMORY, Size, pMemory, ref TmpHandle);

			return (Result);
		}

		public RESULT cMemoryClose(ushort PrgId)
		{
			RESULT Result = RESULT.FAIL;

			cMemoryFreeProgram(PrgId);
			Result = OK;

			return (Result);
		}

		public RESULT cMemoryExit()
		{
			RESULT Result = RESULT.FAIL;
			FileInfo file;
			char[] PrgNameBuf = new char[vmFILENAMESIZE];

			CommonHelper.Snprintf(PrgNameBuf, 0, vmFILENAMESIZE, vmSETTINGS_DIR.ToCharArray(), "/".ToCharArray(), vmLASTRUN_FILE_NAME.ToCharArray(), vmEXT_CONFIG.ToCharArray());
			file = new FileInfo(string.Concat(PrgNameBuf));
			// TODO: wtf wtf wtf
			//if (file != null)
			//{
			//	File.WriteAllBytes(file.FullName, GH.MemoryInstance.Cache);
			//}

			Result = OK;

			return (Result);
		}

		public byte[] cMemoryResize(ushort PrgId, short TmpHandle, int Elements)
		{
			DATA32 Size;
            byte[] pTmp = null;

			if (cMemoryGetPointer(PrgId, TmpHandle, out pTmp) == OK)
			{
				Size = Elements * (DESCR.FromByteArray(pTmp)).ElementSize + DESCR.Sizeof;
				pTmp = cMemoryReallocate(PrgId, TmpHandle, (GBINDEX)Size);
				if (pTmp != null)
				{
					(DESCR.FromByteArray(pTmp)).Elements = Elements;
				}
			}
			if (pTmp != null)
			{
				pTmp = (DESCR.FromByteArray(pTmp)).pArray.ToByteArray();
			}

			return (pTmp);
		}

		void FindName(char[] pSource, char[] pPath, char[] pName, char[] pExt)
		{
			int Source = 0;
			int Destination = 0;

			while (pSource.Length > Source)
			{
				Source++;
			}
			while ((Source > 0) && (pSource[Source] != '/'))
			{
				Source--;
			}
			if (pSource[Source] == '/')
			{
				if (pPath != null)
				{
					for (Destination = 0; Destination <= Source; Destination++)
					{
						pPath[Destination] = pSource[Destination];
					}
				}
				Source++;
			}
			if (pPath != null)
			{
				pPath[Destination] = (char)0;
			}
			Destination = 0;
			while ((pSource.Length > Source) && (pSource[Source] != '.'))
			{
				if (pName != null)
				{
					pName[Destination] = pSource[Source];
					Destination++;
				}
				Source++;
			}
			if (pName != null)
			{
				pName[Destination] = (char)0;
			}
			if (pExt != null)
			{
				Destination = 0;
				while (pSource.Length > Source)
				{
					pExt[Destination] = pSource[Source];
					Source++;
					Destination++;
				}
				pExt[Destination] = (char)0;
			}
		}

		public RESULT cMemoryCheckFilename(sbyte[] pFilename, sbyte[] pPath, sbyte[] pName, sbyte[] pExt)
		{
			RESULT Result = RESULT.FAIL;
			char[] Path = new char[vmFILENAMESIZE];
			char[] Name = new char[vmFILENAMESIZE];
			char[] Ext = new char[vmFILENAMESIZE];

			if (pFilename.Length < vmFILENAMESIZE)
			{
				if (GH.Lms.ValidateString(pFilename, vmCHARSET_FILENAME) == OK)
				{
					FindName(pFilename.ToCharArray(), Path, Name, Ext);
					if (Path.Length < vmPATHSIZE)
					{
						if (pPath != null)
						{
							CommonHelper.Strcpy(pPath, string.Concat(Path));
						}
						if (Name.Length < vmNAMESIZE)
						{
							if (pName != null)
							{
								CommonHelper.Strcpy(pName, string.Concat(Name));
							}
							if (Ext.Length < vmEXTSIZE)
							{
								if (pExt != null)
								{
									CommonHelper.Strcpy(pExt, string.Concat(Ext));
								}
								Result = OK;
							}
						}
					}
				}
			}

			if (pFilename.Length < vmFILENAMESIZE)
			{
				FindName(pFilename.ToCharArray(), Path, Name, Ext);
				if (Path.Length < vmPATHSIZE)
				{
					if (pPath != null)
					{
						CommonHelper.Strcpy(pPath, string.Concat(Path));
					}
					if (Name.Length < vmNAMESIZE)
					{
						if (pName != null)
						{
							CommonHelper.Strcpy(pName, string.Concat(Name));
						}
						if (Ext.Length < vmEXTSIZE)
						{
							if (pExt != null)
							{
								CommonHelper.Strcpy(pExt, string.Concat(Ext));
							}
							Result = OK;
						}
					}
				}
			}
			if (Result != OK)
			{
				// TODO: log error
				//if (!LogErrorNumberExists(FILE_NAME_ERROR))
				//{
				//	LogErrorNumber(FILE_NAME_ERROR);
				//}
			}

			return (Result);
		}

		RESULT ConstructFilename(PRGID PrgId, char[] pFilename, char[] pName, char[] pDefaultExt)
		{
			RESULT Result = RESULT.FAIL;
			char[] Path = new char[vmPATHSIZE];
			char[] Name = new char[vmNAMESIZE];
			char[] Ext = new char[vmEXTSIZE];

			Result = cMemoryCheckFilename(pFilename.ToSbyteArray(), Path.ToSbyteArray(), Name.ToSbyteArray(), Ext.ToSbyteArray());

			if (Result == OK)
			{ // Filename OK

				if (Path[0] == 0)
				{ // Default path

					CommonHelper.Snprintf(Path, 0, vmPATHSIZE, GH.MemoryInstance.PathList[PrgId].ToCharArray());
				}

				if (Ext[0] == 0)
				{ // Default extension

					CommonHelper.Snprintf(Ext, 0, vmEXTSIZE, pDefaultExt);
				}

				// Construct filename for open
				CommonHelper.Snprintf(pName, 0, vmFILENAMESIZE, Path, Name, Ext);

			}

			return (Result);
		}


		int FindDot(char[] pString)
		{
			int Result = -1;
			int Pointer = 0;

			while (pString.Length > Pointer)
			{
				if (pString[Pointer] == '.')
				{
					Result = Pointer;
				}
				Pointer++;
			}

			return (Result);
		}

		void cMemoryDeleteCacheFile(char[] pFileName)
		{
			DATA8 Item;
			DATA8 Tmp;

			Item = 0;
			Tmp = 0;

			while ((Item < CACHE_DEEPT) && (Tmp == 0))
			{
				if (CommonHelper.Strcmp(GH.MemoryInstance.Cache[Item], string.Concat(pFileName)) == 0)
				{
					Tmp = 1;
				}
				else
				{
					Item++;
				}
			}
			while (Item < (CACHE_DEEPT - 1))
			{
				CommonHelper.Strcpy(GH.MemoryInstance.Cache[Item], string.Concat(GH.MemoryInstance.Cache[Item + 1].ToCharArray()));
				Item++;
			}
			GH.MemoryInstance.Cache[Item][0] = 0;
		}


		int cMemorySort(object[] ppFirst, object[] ppSecond)
		{
			int Result;
			int First, Second;
			char[] pFirst;
			char[] pSecond;

			pFirst = CommonHelper.CastObjectArray<char>(ppFirst);
			pSecond = CommonHelper.CastObjectArray<char>(ppSecond);

			First = FindDot(pFirst);
			Second = FindDot(pSecond);

			if ((First >= 0) && (Second >= 0))
			{
				Result = CommonHelper.Strcmp(pFirst.Skip(First).ToArray().ToSbyteArray(), string.Concat(pSecond.Skip(Second).ToArray()));
				if (Result == 0)
				{
					Result = CommonHelper.Strcmp(pFirst.ToSbyteArray(), string.Concat(pSecond));
				}
			}
			else
			{
				if ((First < 0) && (Second < 0))
				{
					Result = CommonHelper.Strcmp(pFirst.ToSbyteArray(), string.Concat(pSecond));
				}
				else
				{
					if (First < 0)
					{
						Result = 1;
					}
					else
					{
						Result = -1;
					}
				}
			}

			return (Result);
		}

		public void cMemoryArray()
		{
			throw new NotImplementedException();
		}

		public void cMemoryArrayAppend()
		{
			throw new NotImplementedException();
		}

		public void cMemoryArrayRead()
		{
			throw new NotImplementedException();
		}

		public void cMemoryArrayWrite()
		{
			throw new NotImplementedException();
		}



		public RESULT cMemoryCheckOpenWrite(sbyte[] pFileName)
		{
			throw new NotImplementedException();
		}



		public DSPSTAT cMemoryCloseFile(ushort PrgId, FileInfo Handle)
		{
			throw new NotImplementedException();
		}

		public void cMemoryCloseFolder(ushort PrgId, DirectoryInfo pHandle)
		{
			throw new NotImplementedException();
		}



		public void cMemoryFile()
		{
			throw new NotImplementedException();
		}

		public void cMemoryFileName()
		{
			throw new NotImplementedException();
		}

		public sbyte cMemoryFindFiles(sbyte[] pFolderName)
		{
			throw new NotImplementedException();
		}

		public sbyte cMemoryFindSubFolders(sbyte[] pFolderName)
		{
			throw new NotImplementedException();
		}

		public sbyte cMemoryGetCacheFiles()
		{
			throw new NotImplementedException();
		}

		public sbyte cMemoryGetCacheName(sbyte Item, sbyte MaxLength, sbyte[] pFileName, sbyte[] pName)
		{
			throw new NotImplementedException();
		}

		public RESULT cMemoryGetFolderItems(ushort PrgId, DirectoryInfo Handle, ref short pItems)
		{
			throw new NotImplementedException();
		}

		public RESULT cMemoryGetIcon(sbyte[] pFolderName, sbyte Item, int[] pImagePointer)
		{
			throw new NotImplementedException();
		}

		public RESULT cMemoryGetImage(sbyte[] pText, short Size, byte[] pBmp)
		{
			throw new NotImplementedException();
		}

		public RESULT cMemoryGetItem(ushort PrgId, DirectoryInfo Handle, short Item, sbyte Length, sbyte[] pName, ref sbyte pType)
		{
			throw new NotImplementedException();
		}

		public RESULT cMemoryGetItemIcon(ushort PrgId, DirectoryInfo Handle, short Item, ref FileInfo pHandle, int[] pImagePointer)
		{
			throw new NotImplementedException();
		}

		public RESULT cMemoryGetItemName(ushort PrgId, DirectoryInfo Handle, short Item, sbyte Length, sbyte[] pName, ref sbyte pType, ref sbyte pPriority)
		{
			throw new NotImplementedException();
		}

		public RESULT cMemoryGetItemText(ushort PrgId, DirectoryInfo Handle, short Item, sbyte Length, sbyte[] pText)
		{
			throw new NotImplementedException();
		}

		public RESULT cMemoryGetMediaName(sbyte[] pChar, sbyte[] pName)
		{
			throw new NotImplementedException();
		}



		public void cMemoryGetResourcePath(ushort PrgId, sbyte[] pString, sbyte MaxLength)
		{
			throw new NotImplementedException();
		}

		public sbyte cMemoryGetSubFolderName(sbyte Item, sbyte MaxLength, sbyte[] pFolderName, sbyte[] pSubFolderName)
		{
			throw new NotImplementedException();
		}









		public RESULT cMemoryOpenFolder(ushort PrgId, sbyte Type, sbyte[] pFolderName, ref DirectoryInfo pHandle)
		{
			throw new NotImplementedException();
		}

		public RESULT cMemorySetItemText(ushort PrgId, DirectoryInfo Handle, short Item, sbyte[] pText)
		{
			throw new NotImplementedException();
		}

		public void cMemoryUsage()
		{
			throw new NotImplementedException();
		}
	}
}
