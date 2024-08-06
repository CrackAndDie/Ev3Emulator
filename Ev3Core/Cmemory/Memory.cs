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
		public void cMemoryGetUsage(VarPointer<int> pTotal, VarPointer<int> pFree, sbyte Force)
		{
			// TODO: do it normally if you want
			GH.VMInstance.MemorySize = INSTALLED_MEMORY;
			GH.VMInstance.MemoryFree = INSTALLED_MEMORY;
			pTotal.Data = GH.VMInstance.MemorySize;
			pFree.Data = GH.VMInstance.MemoryFree;
		}

		public RESULT cMemoryMalloc(ArrayPointer<byte> ppMemory, int Size)
		{
			RESULT Result = RESULT.FAIL;
			VarPointer<DATA32> Total = new VarPointer<DATA32>(0);
			VarPointer<DATA32> Free = new VarPointer<DATA32>(0);

			cMemoryGetUsage(Total, Free, 0);
			if (((Size + (KB - 1)) / KB) < (Free.Data - RESERVED_MEMORY))
			{
				ppMemory.Data = new byte[Size];
				if (ppMemory != null)
				{
					Result = OK;
				}
			}

			return (Result);
		}

		public RESULT cMemoryRealloc(ArrayPointer<UBYTE> pOldMemory, ArrayPointer<UBYTE> ppMemory, int Size)
		{
			RESULT Result = RESULT.FAIL;

			ppMemory.Data = new byte[Size];
			if (ppMemory != null)
			{
				Result = OK;
			}

			return (Result);
		}

		RESULT cMemoryAlloc(PRGID PrgId, DATA8 Type, GBINDEX Size, ArrayPointer<UBYTE> pMemory, VarPointer<HANDLER> pHandle)
		{
			RESULT Result = RESULT.FAIL;
			HANDLER TmpHandle;

			pHandle.Data = -1;

			if ((PrgId < MAX_PROGRAMS) && (Size > 0) && (Size <= MAX_ARRAY_SIZE))
			{
				TmpHandle = 0;

				while ((TmpHandle < MAX_HANDLES) && (GH.MemoryInstance.pPoolList[PrgId][TmpHandle].pPool != null))
				{
					TmpHandle++;
				}
				if (TmpHandle < MAX_HANDLES)
				{

					if (cMemoryMalloc(GH.MemoryInstance.pPoolList[PrgId][TmpHandle].pPool, (DATA32)Size) == OK)
					{
						pMemory.Data = GH.MemoryInstance.pPoolList[PrgId][TmpHandle].pPool.Data;
						GH.MemoryInstance.pPoolList[PrgId][TmpHandle].Type = Type;
						GH.MemoryInstance.pPoolList[PrgId][TmpHandle].Size = Size;
						pHandle.Data = TmpHandle;
						Result = OK;
					}
				}
			}

			return (Result);
		}

		ArrayPointer<UBYTE> cMemoryReallocate(PRGID PrgId, HANDLER Handle, GBINDEX Size)
		{
			ArrayPointer<UBYTE> pTmp;

			pTmp = new ArrayPointer<byte>();
			if ((PrgId < MAX_PROGRAMS) && (Handle >= 0) && (Handle < MAX_HANDLES))
			{
				if ((Size > 0) && (Size <= MAX_ARRAY_SIZE))
				{
					if (cMemoryRealloc(GH.MemoryInstance.pPoolList[PrgId][Handle].pPool, pTmp, (DATA32)Size) == OK)
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

		public RESULT cMemoryGetPointer(ushort PrgId, short Handle, ArrayPointer<UBYTE> pMemory)
		{
			RESULT Result = RESULT.FAIL;

			pMemory.Data = null;

			if ((PrgId < MAX_PROGRAMS) && (Handle >= 0) && (Handle < MAX_HANDLES))
			{
				if (GH.MemoryInstance.pPoolList[PrgId][Handle].pPool != null)
				{
					pMemory.Data = GH.MemoryInstance.pPoolList[PrgId][Handle].pPool.Data;
					Result = OK;
				}
			}

			return (Result);
		}

		public RESULT cMemoryArraryPointer(ushort PrgId, short Handle, ArrayPointer<UBYTE> pMemory)
		{
			RESULT Result = RESULT.FAIL;
			ArrayPointer<UBYTE> pTmp = new ArrayPointer<byte>();

			if (cMemoryGetPointer(PrgId, Handle, pTmp) == OK)
			{
				pMemory.Data = pTmp.Data;
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
						pFDescr = FDESCR.GetObject(GH.MemoryInstance.pPoolList[PrgId][Handle].pPool);
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

		void cMemoryFreePool(PRGID PrgId, ArrayPointer<UBYTE> pMemory)
		{
			HANDLER TmpHandle;

			TmpHandle = 0;
			while ((TmpHandle < MAX_HANDLES) && (GH.MemoryInstance.pPoolList[PrgId][TmpHandle].pPool.Data != pMemory.Data))
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

		public RESULT cMemoryOpen(ushort PrgId, uint Size, ArrayPointer<UBYTE> pMemory)
		{
			RESULT Result = RESULT.FAIL;
			VarPointer<HANDLER> TmpHandle = new VarPointer<short>(0);

			Result = cMemoryAlloc(PrgId, POOL_TYPE_MEMORY, Size, pMemory, TmpHandle);

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
			ArrayPointer<UBYTE> PrgNameBuf = new ArrayPointer<UBYTE>(new byte[vmFILENAMESIZE]);

			CommonHelper.Snprintf(PrgNameBuf, vmFILENAMESIZE, vmSETTINGS_DIR.ToArrayPointer(), "/".ToArrayPointer(), vmLASTRUN_FILE_NAME.ToArrayPointer(), vmEXT_CONFIG.ToArrayPointer());
			file = new FileInfo(string.Concat(PrgNameBuf));
			// TODO: wtf wtf wtf
			//if (file != null)
			//{
			//	File.WriteAllBytes(file.FullName, GH.MemoryInstance.Cache);
			//}

			Result = OK;

			return (Result);
		}

		public ArrayPointer<UBYTE> cMemoryResize(ushort PrgId, short TmpHandle, int Elements)
		{
			DATA32 Size;
			ArrayPointer<UBYTE> pTmp = new ArrayPointer<byte>();

			if (cMemoryGetPointer(PrgId, TmpHandle, pTmp) == OK)
			{
				Size = Elements * DESCR.GetObject(pTmp).ElementSize + DESCR.Sizeof;
				pTmp = cMemoryReallocate(PrgId, TmpHandle, (GBINDEX)Size);
				if (pTmp != null)
				{
					DESCR.GetObject(pTmp).Elements = Elements;
				}
			}
			if (pTmp != null)
			{
				pTmp = new ArrayPointer<byte>(DESCR.GetObject(pTmp).pArray);
			}

			return (pTmp);
		}

		void FindName(ArrayPointer<UBYTE> pSource, ArrayPointer<UBYTE> pPath, ArrayPointer<UBYTE> pName, ArrayPointer<UBYTE> pExt)
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
				pPath[Destination] = (byte)0;
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
				pName[Destination] = (byte)0;
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
				pExt[Destination] = (byte)0;
			}
		}

		public RESULT cMemoryCheckFilename(ArrayPointer<UBYTE> pFilename, ArrayPointer<UBYTE> pPath, ArrayPointer<UBYTE> pName, ArrayPointer<UBYTE> pExt)
		{
			RESULT Result = RESULT.FAIL;
			ArrayPointer<UBYTE> Path = new ArrayPointer<UBYTE>(new byte[vmFILENAMESIZE]);
			ArrayPointer<UBYTE> Name = new ArrayPointer<UBYTE>(new byte[vmFILENAMESIZE]);
			ArrayPointer<UBYTE> Ext = new ArrayPointer<UBYTE>(new byte[vmFILENAMESIZE]);

			if (pFilename.Length < vmFILENAMESIZE)
			{
				if (GH.Lms.ValidateString(pFilename, vmCHARSET_FILENAME) == OK)
				{
					FindName(pFilename, Path, Name, Ext);
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
				FindName(pFilename, Path, Name, Ext);
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

		RESULT ConstructFilename(PRGID PrgId, ArrayPointer<UBYTE> pFilename, ArrayPointer<UBYTE> pName, ArrayPointer<UBYTE> pDefaultExt)
		{
			RESULT Result = RESULT.FAIL;
			ArrayPointer<UBYTE> Path = new ArrayPointer<UBYTE>(new UBYTE[vmPATHSIZE]);
			ArrayPointer<UBYTE> Name = new ArrayPointer<UBYTE>(new UBYTE[vmNAMESIZE]);
			ArrayPointer<UBYTE> Ext = new ArrayPointer<UBYTE>(new UBYTE[vmEXTSIZE]);

			Result = cMemoryCheckFilename(pFilename, Path, Name, Ext);

			if (Result == OK)
			{ // Filename OK

				if (Path[0] == 0)
				{ // Default path

					CommonHelper.Snprintf(Path, vmPATHSIZE, GH.MemoryInstance.PathList[PrgId]);
				}

				if (Ext[0] == 0)
				{ // Default extension

					CommonHelper.Snprintf(Ext, vmEXTSIZE, pDefaultExt);
				}

				// Construct filename for open
				CommonHelper.Snprintf(pName, vmFILENAMESIZE, Path, Name, Ext);

			}

			return (Result);
		}


		int FindDot(ArrayPointer<UBYTE> pString)
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

		void cMemoryDeleteCacheFile(ArrayPointer<UBYTE> pFileName)
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
				CommonHelper.Strcpy(GH.MemoryInstance.Cache[Item], string.Concat(GH.MemoryInstance.Cache[Item + 1]));
				Item++;
			}
			GH.MemoryInstance.Cache[Item][0] = 0;
		}


		int cMemorySort(ArrayPointer<UBYTE> ppFirst, ArrayPointer<UBYTE> ppSecond)
		{
			int Result;
			int First, Second;
			ArrayPointer<UBYTE> pFirst = ppFirst;
			ArrayPointer<UBYTE> pSecond = ppSecond;

			First = FindDot(pFirst);
			Second = FindDot(pSecond);

			if ((First >= 0) && (Second >= 0))
			{
				Result = CommonHelper.Strcmp(pFirst.Copy(First), string.Concat(pSecond.Skip(Second).ToArray()));
				if (Result == 0)
				{
					Result = CommonHelper.Strcmp(pFirst, string.Concat(pSecond));
				}
			}
			else
			{
				if ((First < 0) && (Second < 0))
				{
					Result = CommonHelper.Strcmp(pFirst, string.Concat(pSecond));
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



		public RESULT cMemoryCheckOpenWrite(ArrayPointer<UBYTE> pFileName)
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

		public sbyte cMemoryFindFiles(ArrayPointer<UBYTE> pFolderName)
		{
			throw new NotImplementedException();
		}

		public sbyte cMemoryFindSubFolders(ArrayPointer<UBYTE> pFolderName)
		{
			throw new NotImplementedException();
		}

		public sbyte cMemoryGetCacheFiles()
		{
			throw new NotImplementedException();
		}

		public sbyte cMemoryGetCacheName(sbyte Item, sbyte MaxLength, ArrayPointer<UBYTE> pFileName, ArrayPointer<UBYTE> pName)
		{
			throw new NotImplementedException();
		}

		public RESULT cMemoryGetFolderItems(ushort PrgId, DirectoryInfo Handle, VarPointer<short> pItems)
		{
			throw new NotImplementedException();
		}

		public RESULT cMemoryGetIcon(ArrayPointer<UBYTE> pFolderName, sbyte Item, ArrayPointer<UBYTE> pImagePointer)
		{
			throw new NotImplementedException();
		}

		public RESULT cMemoryGetImage(ArrayPointer<UBYTE> pText, short Size, ArrayPointer<UBYTE> pBmp)
		{
			throw new NotImplementedException();
		}

		public RESULT cMemoryGetItem(ushort PrgId, DirectoryInfo Handle, short Item, sbyte Length, ArrayPointer<UBYTE> pName, VarPointer<sbyte> pType)
		{
			throw new NotImplementedException();
		}

		public RESULT cMemoryGetItemIcon(ushort PrgId, DirectoryInfo Handle, short Item, VarPointer<FileInfo> pHandle, ArrayPointer<UBYTE> pImagePointer)
		{
			throw new NotImplementedException();
		}

		public RESULT cMemoryGetItemName(ushort PrgId, DirectoryInfo Handle, short Item, sbyte Length, ArrayPointer<UBYTE> pName, VarPointer<sbyte> pType, VarPointer<sbyte> pPriority)
		{
			throw new NotImplementedException();
		}

		public RESULT cMemoryGetItemText(ushort PrgId, DirectoryInfo Handle, short Item, sbyte Length, ArrayPointer<UBYTE> pText)
		{
			throw new NotImplementedException();
		}

		public RESULT cMemoryGetMediaName(ArrayPointer<UBYTE> pChar, ArrayPointer<UBYTE> pName)
		{
			throw new NotImplementedException();
		}



		public void cMemoryGetResourcePath(ushort PrgId, ArrayPointer<UBYTE> pString, sbyte MaxLength)
		{
			throw new NotImplementedException();
		}

		public sbyte cMemoryGetSubFolderName(sbyte Item, sbyte MaxLength, ArrayPointer<UBYTE> pFolderName, ArrayPointer<UBYTE> pSubFolderName)
		{
			throw new NotImplementedException();
		}









		public RESULT cMemoryOpenFolder(ushort PrgId, sbyte Type, ArrayPointer<UBYTE> pFolderName, VarPointer<DirectoryInfo> pHandle)
		{
			throw new NotImplementedException();
		}

		public RESULT cMemorySetItemText(ushort PrgId, DirectoryInfo Handle, short Item, ArrayPointer<UBYTE> pText)
		{
			throw new NotImplementedException();
		}

		public void cMemoryUsage()
		{
			throw new NotImplementedException();
		}
	}
}
