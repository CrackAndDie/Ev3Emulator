using Ev3CoreUnsafe.Cmemory.Interfaces;
using Ev3CoreUnsafe.Enums;
using Ev3CoreUnsafe.Extensions;
using Ev3CoreUnsafe.Helpers;
using System.Runtime.CompilerServices;
using static Ev3CoreUnsafe.Defines;

namespace Ev3CoreUnsafe.Cmemory
{
	public unsafe class Memory : IMemory
	{
		public void cMemoryGetUsage(DATA32* pTotal, DATA32* pFree, DATA8 Force)
		{
			ULONG Time;

			Time = GH.VMInstance.NewTime - GH.VMInstance.MemoryTimer;

			if ((Time >= UPDATE_MEMORY) || (GH.VMInstance.MemoryTimer == 0) || (Force != 0))
			{ // Update values

				GH.VMInstance.MemoryTimer += Time;

				GH.VMInstance.MemorySize = INSTALLED_MEMORY;
				GH.VMInstance.MemoryFree = INSTALLED_MEMORY;

			}

			*pTotal = GH.VMInstance.MemorySize;
			*pFree = GH.VMInstance.MemoryFree;
		}

		private DATA32* TotalcMemoryMalloc = (DATA32*)CommonHelper.AllocateByteArray(4);
		private DATA32* FreecMemoryMalloc = (DATA32*)CommonHelper.AllocateByteArray(4);
		public RESULT cMemoryMalloc(void** ppMemory, DATA32 Size)
		{
			RESULT Result = RESULT.FAIL;
			

			cMemoryGetUsage(TotalcMemoryMalloc, FreecMemoryMalloc, 0);
			if (((Size + (KB - 1)) / KB) < (*FreecMemoryMalloc - RESERVED_MEMORY))
			{
				*ppMemory = CommonHelper.AllocateByteArray(Size);
				//*ppMemory = malloc((int)Size);
				if (*ppMemory != null)
				{
					Result = OK;
				}
			}

			return (Result);
		}


		public RESULT cMemoryRealloc(void* pOldMemory, void** ppMemory, DATA32 Size)
		{
			RESULT Result = RESULT.FAIL;

			CommonHelper.DeleteByteArray((byte*)pOldMemory);
			*ppMemory = CommonHelper.AllocateByteArray(Size);
            if (*ppMemory != null)
			{
				Result = OK;
			}

			return (Result);
		}


		public RESULT cMemoryAlloc(PRGID PrgId, DATA8 Type, GBINDEX Size, void** pMemory, HANDLER* pHandle)
		{
			RESULT Result = RESULT.FAIL;
			HANDLER TmpHandle;

			*pHandle = -1;

			if ((PrgId < MAX_PROGRAMS) && (Size > 0) && (Size <= MAX_ARRAY_SIZE))
			{
				TmpHandle = 0;

				while ((TmpHandle < MAX_HANDLES) && (GH.MemoryInstance.pPoolList[PrgId][TmpHandle].pPool != null))
				{
					TmpHandle++;
				}
				if (TmpHandle < MAX_HANDLES)
				{

					if (cMemoryMalloc(&GH.MemoryInstance.pPoolList[PrgId][TmpHandle].pPool, (DATA32)Size) == OK)
					{
						*pMemory = GH.MemoryInstance.pPoolList[PrgId][TmpHandle].pPool;
						GH.MemoryInstance.pPoolList[PrgId][TmpHandle].Type = Type;
						GH.MemoryInstance.pPoolList[PrgId][TmpHandle].Size = Size;
						*pHandle = TmpHandle;
						Result = OK;
						GH.printf($"  Malloc P={(uint)PrgId} H={(uint)TmpHandle} T={(uint)Type} S={(ulong)Size} A={(long)GH.MemoryInstance.pPoolList[PrgId][TmpHandle].pPool}\r\n");
					}
				}
			}
			if (Result != OK)
			{
				GH.printf($"  Malloc error P={(uint)PrgId} S={(ulong)Size}\r\n");
			}

			return (Result);
		}

		private void* pTmpcMemoryReallocate;
		public void* cMemoryReallocate(PRGID PrgId, HANDLER Handle, GBINDEX Size)
		{


			pTmpcMemoryReallocate = null;
			if ((PrgId < MAX_PROGRAMS) && (Handle >= 0) && (Handle < MAX_HANDLES))
			{
				if ((Size > 0) && (Size <= MAX_ARRAY_SIZE))
				{
					fixed (void** pp = &pTmpcMemoryReallocate)
						if (cMemoryRealloc(GH.MemoryInstance.pPoolList[PrgId][Handle].pPool, pp, (DATA32)Size) == OK)
						{
							GH.MemoryInstance.pPoolList[PrgId][Handle].pPool = pTmpcMemoryReallocate;
							GH.MemoryInstance.pPoolList[PrgId][Handle].Size = Size;
						}
						else
						{
								pTmpcMemoryReallocate = null;
							GH.printf("cMemoryReallocate out of memory\r\n");
						}
				}
			}
			if (pTmpcMemoryReallocate != null)
			{
				GH.printf($"  Reallocate  P={(uint)PrgId} H={(uint)Handle}     S={(ulong)Size} A={(long)GH.MemoryInstance.pPoolList[PrgId][Handle].pPool}\r\n");
			}
			else
			{
				GH.printf($"  Reallocate error P={(uint)PrgId} H={(uint)Handle} S={(ulong)Size}\r\n");
			}

			return (pTmpcMemoryReallocate);
		}


		public RESULT cMemoryGetPointer(PRGID PrgId, HANDLER Handle, void** pMemory)
		{
			RESULT Result = RESULT.FAIL;

			*pMemory = null;

			if ((PrgId < MAX_PROGRAMS) && (Handle >= 0) && (Handle < MAX_HANDLES))
			{
				if (GH.MemoryInstance.pPoolList[PrgId][Handle].pPool != null)
				{
					*pMemory = GH.MemoryInstance.pPoolList[PrgId][Handle].pPool;
					Result = OK;
				}
			}
			if (Result != OK)
			{
				GH.printf($"  Get pointer error P={(uint)PrgId} H={(uint)Handle}\r\n");
			}

			return (Result);
		}

		private void* pTmpcMemoryArraryPointer;
		public RESULT cMemoryArraryPointer(PRGID PrgId, HANDLER Handle, void** pMemory)
		{
			RESULT Result = RESULT.FAIL;
			
			fixed (void** pp = &pTmpcMemoryArraryPointer)
			if (cMemoryGetPointer(PrgId, Handle, pp) == OK)
			{
				*pMemory = (*(DESCR*)pTmpcMemoryArraryPointer).pArray;
				Result = OK;
			}

			return (Result);
		}


		public DSPSTAT cMemoryFreeHandle(PRGID PrgId, HANDLER Handle)
		{
			DSPSTAT Result = DSPSTAT.FAILBREAK;
			FDESCR* pFDescr;

			if ((PrgId < MAX_PROGRAMS) && (Handle >= 0) && (Handle < MAX_HANDLES))
			{
				if (GH.MemoryInstance.pPoolList[PrgId][Handle].pPool != null)
				{
					if (GH.MemoryInstance.pPoolList[PrgId][Handle].Type == POOL_TYPE_FILE)
					{
						pFDescr = (FDESCR*)GH.MemoryInstance.pPoolList[PrgId][Handle].pPool;
						// TODO: not really closed
						//if (((*pFDescr).Access))
						//{
						//	(*pFDescr).Access = 0;
						//	close((*pFDescr).hFile);
						//	sync();
						//	Result = DSPSTAT.NOBREAK;
						//}
						GH.printf($"  Close file {CommonHelper.GetString((*pFDescr).Filename)}\r\n");
					}
					else
					{
						Result = DSPSTAT.NOBREAK;
					}

					GH.printf($"  Free   P={(uint)PrgId} H={(uint)Handle} T={(uint)GH.MemoryInstance.pPoolList[PrgId][Handle].Type} S={(ulong)GH.MemoryInstance.pPoolList[PrgId][Handle].Size} A={(long)GH.MemoryInstance.pPoolList[PrgId][Handle].pPool}\r\n");
                    CommonHelper.DeleteByteArray((byte*)GH.MemoryInstance.pPoolList[PrgId][Handle].pPool);
                    GH.MemoryInstance.pPoolList[PrgId][Handle].pPool = null;
					GH.MemoryInstance.pPoolList[PrgId][Handle].Size = 0;

				}
            }

			return (Result);
		}


		public void cMemoryFreePool(PRGID PrgId, void* pMemory)
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


		public void cMemoryFreeProgram(PRGID PrgId)
		{
			HANDLER TmpHandle;

			for (TmpHandle = 0; TmpHandle < MAX_HANDLES; TmpHandle++)
			{
				cMemoryFreeHandle(PrgId, TmpHandle);
			}

			// Ensure that path is emptied
			GH.MemoryInstance.PathList[PrgId][0] = 0;
		}


		public void cMemoryFreeAll()
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
			int File;
			DATA8* PrgNameBuf = CommonHelper.Pointer1d<DATA8>(vmFILENAMESIZE);

			CommonHelper.snprintf(PrgNameBuf, vmFILENAMESIZE, "%s/%s%s", vmSETTINGS_DIR, vmLASTRUN_FILE_NAME, vmEXT_CONFIG);
#warning NOT CACHED
			//File = open(PrgNameBuf, O_RDONLY);
			//if (File >= MIN_HANDLE)
			//{
			//	if (read(File, GH.MemoryInstance.Cache, sizeof(GH.MemoryInstance.Cache)) != sizeof(GH.MemoryInstance.Cache))
			//	{
			//		close(File);
			//		File = -1;
			//	}
			//	else
			//	{
			//		close(File);
			//	}
			//}
			//if (File < 0)
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

		private HANDLER* TmpHandlecMemoryOpen = (HANDLER*)CommonHelper.AllocateByteArray(2);
		public RESULT cMemoryOpen(PRGID PrgId, GBINDEX Size, void** pMemory)
		{
			RESULT Result = RESULT.FAIL;

			Result = cMemoryAlloc(PrgId, POOL_TYPE_MEMORY, Size, pMemory, TmpHandlecMemoryOpen);

			return (Result);
		}


		public RESULT cMemoryClose(PRGID PrgId)
		{
			RESULT Result = RESULT.FAIL;

			cMemoryFreeProgram(PrgId);
			Result = OK;

			return (Result);
		}


		public RESULT cMemoryExit()
		{
			RESULT Result = RESULT.FAIL;
			int File;
			DATA8* PrgNameBuf = CommonHelper.Pointer1d<DATA8>(vmFILENAMESIZE);

			CommonHelper.snprintf(PrgNameBuf, vmFILENAMESIZE, $"{vmSETTINGS_DIR}/{vmLASTRUN_FILE_NAME}{vmEXT_CONFIG}");
#warning NOT CACHED
			//File = open(PrgNameBuf, O_CREAT | O_WRONLY | O_TRUNC, FILEPERMISSIONS);
			//if (File >= MIN_HANDLE)
			//{
			//	write(File, GH.MemoryInstance.Cache, sizeof(GH.MemoryInstance.Cache));
			//	close(File);
			//}

			Result = OK;

			return (Result);
		}

		private void* pTmpcMemoryResize;
		public void* cMemoryResize(PRGID PrgId, HANDLER TmpHandle, DATA32 Elements)
		{
			DATA32 Size;
			pTmpcMemoryResize = null;

			fixed (void** pp = &pTmpcMemoryResize)
			if (cMemoryGetPointer(PrgId, TmpHandle, pp) == OK)
			{
				Size = Elements * (*(DESCR*)pTmpcMemoryResize).ElementSize + sizeof(DESCR);
				pTmpcMemoryResize = cMemoryReallocate(PrgId, TmpHandle, (GBINDEX)Size);
				if (pTmpcMemoryResize != null)
				{
					(*(DESCR*)pTmpcMemoryResize).Elements = Elements;
				}

				GH.printf($"  Resize P={(uint)PrgId} H={(uint)TmpHandle} T={(uint)GH.MemoryInstance.pPoolList[PrgId][TmpHandle].Type} S={(ulong)GH.MemoryInstance.pPoolList[PrgId][TmpHandle].Size} A={(long)GH.MemoryInstance.pPoolList[PrgId][TmpHandle].pPool}\r\n");
			}
			if (pTmpcMemoryResize != null)
			{
				pTmpcMemoryResize = (*(DESCR*)pTmpcMemoryResize).pArray;
			}

			return (pTmpcMemoryResize);
		}


		public void FindName(DATA8* pSource, DATA8* pPath, DATA8* pName, DATA8* pExt)
		{
			int Source = 0;
			int Destination = 0;

			while (pSource[Source] != 0)
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
				pPath[Destination] = 0;
			}
			Destination = 0;
			while ((pSource[Source] != 0) && (pSource[Source] != '.'))
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
				pName[Destination] = 0;
			}
			if (pExt != null)
			{
				Destination = 0;
				while (pSource[Source] != 0)
				{
					pExt[Destination] = pSource[Source];
					Source++;
					Destination++;
				}
				pExt[Destination] = 0;
			}
		}


		public RESULT cMemoryCheckFilename(DATA8* pFilename, DATA8* pPath, DATA8* pName, DATA8* pExt)
		{
			RESULT Result = RESULT.FAIL;
			DATA8* Path = CommonHelper.Pointer1d<DATA8>(vmFILENAMESIZE);
            DATA8* Name = CommonHelper.Pointer1d<DATA8>(vmFILENAMESIZE);
            DATA8* Ext = CommonHelper.Pointer1d<DATA8>(vmFILENAMESIZE);

            if (CommonHelper.strlen(pFilename) < vmFILENAMESIZE)
			{
				if (GH.Lms.ValidateString((DATA8*)pFilename, vmCHARSET_FILENAME) == OK)
				{
					FindName(pFilename, Path, Name, Ext);
					if (CommonHelper.strlen(Path) < vmPATHSIZE)
					{
						if (pPath != null)
						{
							CommonHelper.strcpy(pPath, Path);
						}
						if (CommonHelper.strlen(Name) < vmNAMESIZE)
						{
							if (pName != null)
							{
								CommonHelper.strcpy(pName, Name);
							}
							if (CommonHelper.strlen(Ext) < vmEXTSIZE)
							{
								if (pExt != null)
								{
									CommonHelper.strcpy(pExt, Ext);
								}
								Result = OK;
							}
						}
					}
				}
			}
			if (Result != OK)
			{
				GH.printf($"Filename error in [{CommonHelper.GetString(pFilename)}]\r\n");
				GH.Ev3System.Logger.LogError($"An error occured with number {FILE_NAME_ERROR} in {Environment.StackTrace}");
			}

			return (Result);
		}


		public RESULT ConstructFilename(PRGID PrgId, DATA8* pFilename, DATA8* pName, DATA8* pDefaultExt)
		{
			RESULT Result = RESULT.FAIL;
			DATA8* Path = CommonHelper.Pointer1d<DATA8>(vmPATHSIZE);
			DATA8* Name = CommonHelper.Pointer1d<DATA8>(vmNAMESIZE);
			DATA8* Ext = CommonHelper.Pointer1d<DATA8>(vmEXTSIZE);

			Result = cMemoryCheckFilename(pFilename, Path, Name, Ext);

			if (Result == OK)
			{ // Filename OK

				if (Path[0] == 0)
				{ // Default path

					CommonHelper.snprintf(Path, vmPATHSIZE, CommonHelper.GetString(GH.MemoryInstance.PathList[PrgId]));
				}

				if (Ext[0] == 0)
				{ // Default extension

					CommonHelper.snprintf(Ext, vmEXTSIZE, CommonHelper.GetString(pDefaultExt));
				}

				// Construct filename for open
				CommonHelper.snprintf(pName, vmFILENAMESIZE, $"{CommonHelper.GetString(Path)}{CommonHelper.GetString(Name)}{CommonHelper.GetString(Ext)}");

				GH.printf($"c_memory  ConstructFilename:       [{CommonHelper.GetString(pName)}]\r\n");

			}

			return (Result);
		}


		public int FindDot(DATA8* pString)
		{
			int Result = -1;
			int Pointer = 0;

			while (pString[Pointer] != 0) 
			{
				if (pString[Pointer] == '.')
				{
					Result = Pointer;
				}
				Pointer++;
			}

			return (Result);
		}

		public void cMemoryDeleteCacheFile(DATA8* pFileName)
		{
			DATA8 Item;
			DATA8 Tmp;

			GH.printf($"DEL_CACHE_FILE {CommonHelper.GetString(pFileName)}\r\n");

			Item = 0;
			Tmp = 0;

			while ((Item < CACHE_DEEPT) && (Tmp == 0))
			{
				if (CommonHelper.strcmp(GH.MemoryInstance.Cache[Item], pFileName) == 0)
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
				CommonHelper.strcpy(GH.MemoryInstance.Cache[Item], GH.MemoryInstance.Cache[Item + 1]);
				Item++;
			}
			GH.MemoryInstance.Cache[Item][0] = 0;
		}

		private DATA8* pFirst;
		private DATA8* pSecond;
		public int cMemorySort(void* ppFirst, void* ppSecond)
		{
			int Result;
			int First, Second;

			// uncomment anime
			// pFirst = (*(const struct dirent **)ppFirst)->d_name;
			// uncomment anime
			// pSecond = (*(const struct dirent **)ppSecond)->d_name;

			// TODO: probably shite
			pFirst = (DATA8*)ppFirst;
            pSecond = (DATA8*)ppSecond;

            First = FindDot(pFirst);
			Second = FindDot(pSecond);

			if ((First >= 0) && (Second >= 0))
			{
				Result = CommonHelper.strcmp(&pFirst[First], &pSecond[Second]);
				if (Result == 0)
				{
					Result = CommonHelper.strcmp(pFirst, pSecond);
				}
			}
			else
			{
				if ((First < 0) && (Second < 0))
				{
					Result = CommonHelper.strcmp(pFirst, pSecond);
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


		public DATA8 cMemoryFindSubFolders(DATA8* pFolderName)
		{
			// uncomment anime
			// struct dirent **NameList;
			int Items;
			DATA8 Folders = 0;

            DirectoryInfo directory = new DirectoryInfo(CommonHelper.GetString(pFolderName));
            DirectoryInfo[] directories = directory.GetDirectories();

            Items = directories.Length;
			if (Items >= 0)
			{
				while (Items-- != 0)
				{
					if ((directories[Items]).Name[0] != '.')
					{
						if (((directories[Items]).Name[0] != 'C') || ((directories[Items]).Name[1] != 'V') || ((directories[Items]).Name[2] != 'S'))
						{
							Folders++;
						}
					}
				}
			}

			return (Folders);
		}

		public DATA8 cMemoryFindType(DATA8* pExt)
		{
			DATA8 Result = 0;

			if (pExt[0] != 0)
			{
				if (CommonHelper.strcmp(pExt, EXT_SOUND.AsSbytePointer()) == 0)
				{
					Result = TYPE_SOUND;
				}
				else
				{
					if (CommonHelper.strcmp(pExt, EXT_GRAPHICS.AsSbytePointer()) == 0)
					{
						Result = TYPE_GRAPHICS;
					}
					else
					{
						if (CommonHelper.strcmp(pExt, EXT_BYTECODE.AsSbytePointer()) == 0)
						{
							Result = TYPE_BYTECODE;
						}
						else
						{
							if (CommonHelper.strcmp(pExt, EXT_DATALOG.AsSbytePointer()) == 0)
							{
								Result = TYPE_DATALOG;
							}
							else
							{
								if (CommonHelper.strcmp(pExt, EXT_PROGRAM.AsSbytePointer()) == 0)
								{
									Result = TYPE_PROGRAM;
								}
								else
								{
									if (CommonHelper.strcmp(pExt, EXT_TEXT.AsSbytePointer()) == 0)
									{
										Result = TYPE_TEXT;
									}
									else
									{
									}
								}
							}
						}
					}
				}
			}
			else
			{
				Result = TYPE_FOLDER;
			}

			return (Result);
		}


		public DATA8 cMemoryGetSubFolderName(DATA8 Item, DATA8 MaxLength, DATA8* pFolderName, DATA8* pSubFolderName)
		{
			DATA8 Filetype = 0;
			//uncomment anime
			// struct dirent **NameList;
			int Items;
			int Tmp;
			DATA8 Char;
			DATA8 Folders = 0;

			pSubFolderName[0] = 0;

            DirectoryInfo directory = new DirectoryInfo(CommonHelper.GetString(pFolderName));
            DirectoryInfo[] directories = directory.GetDirectories();

            Items = directories.Length;
			Tmp = 0;

			if (Items >= 0)
			{
				while (Tmp < Items)
				{
					if ((directories[Tmp]).Name[0] != '.')
					{
						if (((directories[Tmp]).Name[0] != 'C') || ((directories[Tmp]).Name[1] != 'V') || ((directories[Tmp]).Name[2] != 'S'))
						{
							Folders++;
							if (Item == Folders)
							{
								Char = 0;
								while (((directories[Tmp]).Name.AsSbytePointer()[Char] != 0) && ((directories[Tmp]).Name.AsSbytePointer()[Char] != '.'))
								{
									Char++;
								}
								if ((directories[Tmp]).Name[Char] == '.')
								{
									Filetype = cMemoryFindType(&(directories[Tmp]).Name.AsSbytePointer()[Char]);

									// delete extension
									var tmpAnime = (directories[Tmp]).Name.AsSbytePointer();
                                    tmpAnime[Char] = 0;
									CommonHelper.snprintf(pSubFolderName, (int)MaxLength, CommonHelper.GetString(tmpAnime));
								}
								else
								{ // must be a folder or file without extension
									CommonHelper.snprintf(pSubFolderName, (int)MaxLength, (directories[Tmp]).Name);
									Filetype = TYPE_FOLDER;
								}
							}
						}
					}
					Tmp++;
				}
			}

			return (Filetype);
		}


		public void cMemoryDeleteSubFolders(DATA8* pFolderName)
		{
			// uncomment anime
			// struct dirent *d;
			DATA8* buf = CommonHelper.Pointer1d<DATA8>(256);
			DATA8 DeleteOk = 1;

			if (CommonHelper.strcmp(pFolderName, DEMO_FILE_NAME.AsSbytePointer()) == 0)
			{
				DeleteOk = 0;
			}

			if (DeleteOk != 0)
			{
				Directory.Delete(CommonHelper.GetString(pFolderName), true);
			}
		}

		public DATA32 cMemoryFindSize(DATA8* pFolderName, DATA32* pFiles)
		{
			// uncomment anime
			// struct dirent **NameList;
			// struct stat Status;
			int Items;
			DATA32 Size = 0;

            DirectoryInfo directory = new DirectoryInfo(CommonHelper.GetString(pFolderName));

            *pFiles = directory.GetFiles().Length + directory.GetDirectories().Length;
			Size = (int)(CommonHelper.DirSize(directory) / KB);

			return (Size);
		}

		public DATA8 cMemoryGetCacheName(DATA8 Item, DATA8 MaxLength, DATA8* pFileName, DATA8* pName)
		{
			DATA8 Result = 0;
			DATA8* Path = CommonHelper.Pointer1d<DATA8>(vmPATHSIZE);
			DATA8* Name = CommonHelper.Pointer1d<DATA8>(vmNAMESIZE);
			DATA8* Ext = CommonHelper.Pointer1d<DATA8>(vmEXTSIZE);
			DATA8* Filename = CommonHelper.Pointer1d<DATA8>(vmFILENAMESIZE);

			if ((Item > 0) && (Item <= CACHE_DEEPT))
			{
				if ((long)GH.MemoryInstance.Cache[Item - 1] != 0)
				{
					CommonHelper.snprintf(Filename, vmFILENAMESIZE, CommonHelper.GetString(GH.MemoryInstance.Cache[Item - 1]));

					if (cMemoryCheckFilename(Filename, Path, Name, Ext) == OK)
					{
						CommonHelper.snprintf(pFileName, MaxLength, CommonHelper.GetString(Filename));
						if (MaxLength >= 2)
						{
							if (CommonHelper.strlen(Name) >= MaxLength)
							{
								Name[MaxLength - 1] = 0;
								Name[MaxLength - 2] = 0x7F;
							}
							CommonHelper.snprintf(pName, MaxLength, CommonHelper.GetString(Name));
							Result = cMemoryFindType(Ext);
						}
					}
				}
			}

			return (Result);
		}


		public DATA8 cMemoryGetCacheFiles()
		{
			DATA8 Result = 0;
			DATA8 Tmp;

			for (Tmp = 0; Tmp < CACHE_DEEPT; Tmp++)
			{
				if (GH.MemoryInstance.Cache[Tmp][0] != 0)
				{
					Result++;
				}
			}

			return (Result);
		}


		public DATA8 cMemoryFindFiles(DATA8* pFolderName)
		{
			// uncomment anime
			// struct dirent **NameList;
			int Items;
			DATA8 Files = 0;

            DirectoryInfo directory = new DirectoryInfo(CommonHelper.GetString(pFolderName));
            FileInfo[] files = directory.GetFiles();

            Items = files.Length;
			if (Items >= 0)
			{
				while (Items-- != 0)
				{
					if ((files[Items]).Name[0] != '.')
					{
						if (((files[Items]).Name[0] != 'C') || ((files[Items]).Name[1] != 'V') || ((files[Items]).Name[2] != 'S'))
						{
							Files++;
						}
					}
				}
			}

			return (Files);
		}


		public void cMemoryGetResourcePath(PRGID PrgId, DATA8* pString, DATA8 MaxLength)
		{
			CommonHelper.snprintf(pString, MaxLength, CommonHelper.GetString(GH.MemoryInstance.PathList[PrgId]));
		}

		private IP pImagecMemoryGetIcon = CommonHelper.AllocateByteArray(1);
		private HANDLER* TmpHandlecMemoryGetIcon = (HANDLER*)CommonHelper.AllocateByteArray(2);
		public RESULT cMemoryGetIcon(DATA8* pFolderName, DATA8 Item, DATA32* pImagePointer)
		{
			RESULT Result = RESULT.FAIL;

			DATA32 ISize;
			
			PRGID TmpPrgId;
			DATA8* PrgNamePath = CommonHelper.Pointer1d<DATA8>(SUBFOLDERNAME_SIZE);
			DATA8* PrgNameBuf = CommonHelper.Pointer1d<DATA8>(MAX_FILENAME_SIZE);

			TmpPrgId = GH.Lms.CurrentProgramId();

			cMemoryGetSubFolderName(Item, SUBFOLDERNAME_SIZE, pFolderName, PrgNamePath);

			if (PrgNamePath[0] != 0)
			{
				CommonHelper.snprintf(PrgNameBuf, MAX_FILENAME_SIZE, $"{CommonHelper.GetString(pFolderName)}{CommonHelper.GetString(PrgNamePath)}/icon{EXT_GRAPHICS}");

				var pFile = File.ReadAllBytes(CommonHelper.GetString(PrgNameBuf));
				if (null != pFile)
				{
                    ISize = pFile.Length;
                    // allocate memory to contain the whole file:
					fixed (byte** pp = &pImagecMemoryGetIcon)
                    if (cMemoryAlloc(TmpPrgId, POOL_TYPE_MEMORY, (GBINDEX)ISize, (void**)pp, TmpHandlecMemoryGetIcon) == OK)
					{
						CommonHelper.CopyToPointer(pImagecMemoryGetIcon, pFile);

						*pImagePointer = (DATA32)pImagecMemoryGetIcon;
						Result = OK;
					}
				}
			}

			return (Result);
		}


		public void cMemoryFilename(PRGID PrgId, DATA8* pName, DATA8* pExt, DATA8 Length, DATA8* pResult)
		{
			if ((pName[0] == '.') || (pName[0] == '/') || (pName[0] == '~') || (pName[0] == '\"'))
			{ // Path to file

				CommonHelper.snprintf(pResult, Length, $"{CommonHelper.GetString(pName)}{CommonHelper.GetString(pExt)}");
			}
			else
			{ // Local file

				CommonHelper.snprintf(pResult, Length, $"{CommonHelper.GetString(GH.MemoryInstance.PathList[PrgId])}{CommonHelper.GetString(pName)}{CommonHelper.GetString(pExt)}");
			}
			GH.printf($"Filename = [{CommonHelper.GetString(pResult)}]\r\n");
		}

		private FDESCR* pFDescrcMemoryGetFileHandle;
		[Obsolete("WTF do i need a handle")]
		public DSPSTAT cMemoryGetFileHandle(PRGID PrgId, DATA8* pFileName, HANDLER* pHandle, DATA8* pOpenForWrite)
		{
			DSPSTAT Result = DSPSTAT.FAILBREAK;
			
			HANDLER TmpHandle;

			*pHandle = -1;
			*pOpenForWrite = 0;

			TmpHandle = 0;
			while ((TmpHandle < MAX_HANDLES) && (*pHandle == -1))
			{
				if (GH.MemoryInstance.pPoolList[PrgId][TmpHandle].Type == POOL_TYPE_FILE)
				{
					fixed (FDESCR** pp = &pFDescrcMemoryGetFileHandle)
					if (cMemoryGetPointer(PrgId, TmpHandle, (void**)pp) == OK)
					{
						if ((*pFDescrcMemoryGetFileHandle).Access != 0)
						{
							if (CommonHelper.strcmp(pFileName, (*pFDescrcMemoryGetFileHandle).Filename) == 0)
							{
								*pHandle = TmpHandle;
								if (((*pFDescrcMemoryGetFileHandle).Access == OPEN_FOR_WRITE) || ((*pFDescrcMemoryGetFileHandle).Access == OPEN_FOR_APPEND) || ((*pFDescrcMemoryGetFileHandle).Access == OPEN_FOR_LOG))
								{
									*pOpenForWrite = 1;
								}

								Result = DSPSTAT.NOBREAK;

							}
						}
					}
				}
				TmpHandle++;
			}

			GH.printf($"Handle for file {*pHandle} {CommonHelper.GetString(pFileName)}\r\n");
			Result = DSPSTAT.NOBREAK;

			return (Result);
		}

		private HANDLER* HandlecMemoryCheckOpenWrite = (HANDLER*)CommonHelper.AllocateByteArray(2);
		private DATA8* OpenForWritecMemoryCheckOpenWrite = (DATA8*)CommonHelper.AllocateByteArray(1);
		public RESULT cMemoryCheckOpenWrite(DATA8* pFileName)
		{
			RESULT Result = RESULT.FAIL;
			
			DATA8* FilenameBuf = CommonHelper.Pointer1d<DATA8>(vmFILENAMESIZE);

			if (ConstructFilename(USER_SLOT, pFileName, FilenameBuf, "".AsSbytePointer()) == OK)
			{
				cMemoryGetFileHandle(USER_SLOT, FilenameBuf, HandlecMemoryCheckOpenWrite, OpenForWritecMemoryCheckOpenWrite);
				if (*OpenForWritecMemoryCheckOpenWrite != 0)
				{
					Result = OK;
				}
			}

			return (Result);
		}

		private FDESCR* pFDescrcMemoryOpenFile;
		public DSPSTAT cMemoryOpenFile(PRGID PrgId, DATA8 Access, DATA8* pFileName, HANDLER* pHandle, DATA32* pSize)
		{
			DSPSTAT Result = DSPSTAT.FAILBREAK;
			
			// uncomment anime
			// struct stat FileStatus;

			*pHandle = 0;
			*pSize = 0;

			switch (Access)
			{
				case OPEN_FOR_WRITE:
					{
						GH.printf($"Open for write  {CommonHelper.GetString(pFileName)}\r\n");
					}
					break;

				case OPEN_FOR_APPEND:
					{
						GH.printf($"Open for append {CommonHelper.GetString(pFileName)}\r\n");
					}
					break;

				case OPEN_FOR_READ:
					{
						Result = DSPSTAT.NOBREAK;
						GH.printf($"Open for read   {CommonHelper.GetString(pFileName)}\r\n");
					}
					break;

				case OPEN_FOR_LOG:
					{
						GH.printf($"Open for append {CommonHelper.GetString(pFileName)}\r\n");
					}
					break;

			}

#warning is the pFDescrcMemoryOpenFile allocated to place there something
			fixed (FDESCR** pp = &pFDescrcMemoryOpenFile)
			if (cMemoryAlloc(PrgId, POOL_TYPE_FILE, (GBINDEX)sizeof(FDESCR), (void**)pp, pHandle) == OK) 
			{
				(*pFDescrcMemoryOpenFile).hFile = 0;
				(*pFDescrcMemoryOpenFile).Access = Access;
				CommonHelper.strcpy((*pFDescrcMemoryOpenFile).Filename, pFileName);

				CommonHelper.snprintf((*pFDescrcMemoryOpenFile).Filename, MAX_FILENAME_SIZE, CommonHelper.GetString(pFileName));

				var fInfo = new FileInfo(CommonHelper.GetString(pFileName));
				*pSize = (int)fInfo.Length;

				Result = DSPSTAT.NOBREAK;
			}
			else
			{
				// close(hFile);
			}

			if (Result == DSPSTAT.FAILBREAK)
			{
				GH.Ev3System.Logger.LogError($"An error occured with number {FILE_OPEN_ERROR} in {Environment.StackTrace}");
			}

			return (Result);
		}

		private FDESCR* pFDescrcMemoryWriteFile;
		public DSPSTAT cMemoryWriteFile(PRGID PrgId, HANDLER Handle, DATA32 Size, DATA8 Del, DATA8* pSource)
		{
			DSPSTAT Result = DSPSTAT.FAILBREAK;
			
			fixed (FDESCR** pp = &pFDescrcMemoryWriteFile)
			if (cMemoryGetPointer(PrgId, Handle, (void**)pp) == OK)
			{
				if (((*pFDescrcMemoryWriteFile).Access == OPEN_FOR_WRITE) || ((*pFDescrcMemoryWriteFile).Access == OPEN_FOR_APPEND) || ((*pFDescrcMemoryWriteFile).Access == OPEN_FOR_LOG))
				{
					File.WriteAllBytes(CommonHelper.GetString((*pFDescrcMemoryWriteFile).Filename), CommonHelper.GetArray((byte*)pSource, Size));
					GH.printf($"Write to  {Handle}    {CommonHelper.GetString((*pFDescrcMemoryWriteFile).Filename)} [{Size}]\r\n");
					if (Del < DELS)
					{
						if (Del != DEL_NONE)
						{
							Size = Delimiter[Del].Length;
							File.WriteAllBytes(CommonHelper.GetString((*pFDescrcMemoryWriteFile).Filename), Delimiter[Del]);
							Result = DSPSTAT.NOBREAK;
						}
						else
						{
							Result = DSPSTAT.NOBREAK;
						}
					}
				}
			}

			if (Result == DSPSTAT.FAILBREAK)
			{
				GH.Ev3System.Logger.LogError($"An error occured with number {FILE_WRITE_ERROR} in {Environment.StackTrace}");
			}

			return (Result);
		}

		private FDESCR* pFDescrcMemoryReadFile;
		private DATA8* TmpcMemoryReadFile = (DATA8*)CommonHelper.AllocateByteArray(1);
		public DSPSTAT cMemoryReadFile(PRGID PrgId, HANDLER Handle, DATA32 Size, DATA8 Del, DATA8* pDestination)
		{
			DSPSTAT Result = DSPSTAT.FAILBREAK;
			
			DATA8 No;
			
			DATA8 Last;

			fixed (FDESCR** pp = &pFDescrcMemoryReadFile)
			if (cMemoryGetPointer(PrgId, Handle, (void**)pp) == OK)
			{
				if (((*pFDescrcMemoryReadFile).Access == OPEN_FOR_READ))
				{
					GH.printf($"Read from {Handle}    {CommonHelper.GetString((*pFDescrcMemoryReadFile).Filename)} [{Size}]\r\n");
					if (GH.VMInstance.Handle >= 0)
					{
						if (Size > MIN_ARRAY_ELEMENTS)
						{
							pDestination = (DATA8*)GH.Lms.VmMemoryResize(GH.VMInstance.Handle, Size);
						}
					}
					No = 1;
					Last = 0;
					using var fStr = File.OpenRead(CommonHelper.GetString((*pFDescrcMemoryReadFile).Filename));
					while ((No == 1) && (Size > 0))
					{
						No = (DATA8)fStr.ReadUnsafe((byte*)TmpcMemoryReadFile, 0, 1);

						if (Del < DELS)
						{
							if (Del != DEL_NONE)
							{
								if (Del != DEL_CRLF)
								{
									if (*TmpcMemoryReadFile == Delimiter[Del][0])
									{
										No = 0;
									}
								}
								else
								{
									if ((*TmpcMemoryReadFile == Delimiter[Del][1]) && (Last == Delimiter[Del][0]))
									{
										No = 0;
									}
									Last = *TmpcMemoryReadFile;
								}
							}
						}

						if (No != 0)
						{
							*pDestination = *TmpcMemoryReadFile;
							pDestination++;
							Size--;
						}
					}
					fStr.Close();
					if (Size != 0)
					{
						*pDestination = 0;
					}

					Result = DSPSTAT.NOBREAK;
				}
			}

			if (Result == DSPSTAT.FAILBREAK)
			{
				GH.Ev3System.Logger.LogError($"An error occured with number {FILE_READ_ERROR} in {Environment.StackTrace}");
			}

			return (Result);
		}

		private FDESCR* pFDescrcMemoryCloseFile;
		public DSPSTAT cMemoryCloseFile(PRGID PrgId, HANDLER Handle)
		{
			DSPSTAT Result = DSPSTAT.FAILBREAK;

			
			fixed (FDESCR** pp = &pFDescrcMemoryCloseFile)
			if (GH.MemoryInstance.pPoolList[PrgId][Handle].Type == POOL_TYPE_FILE)
			{
				if (cMemoryGetPointer(PrgId, Handle, (void**)pp) == OK)
				{
					GH.printf($"Close file {Handle}   {CommonHelper.GetString((*pFDescrcMemoryCloseFile).Filename)}\r\n");
				}
			}
			else
			{
				GH.printf($"Close pool {Handle}\r\n");
			}

			Result = cMemoryFreeHandle(PrgId, Handle);

			if (Result == DSPSTAT.FAILBREAK)
			{
				GH.Ev3System.Logger.LogError($"An error occured with number {FILE_CLOSE_ERROR} in {Environment.StackTrace}");
			}

			return (Result);
		}

		private FDESCR* pFDescrcMemoryFindLogName;
		public void cMemoryFindLogName(PRGID PrgId, DATA8* pName)
		{
			HANDLER TmpHandle;
			

			pName[0] = 0;

			if ((PrgId >= 0) && (PrgId < MAX_PROGRAMS))
			{
				for (TmpHandle = 0; TmpHandle < MAX_HANDLES; TmpHandle++)
				{
					if (GH.MemoryInstance.pPoolList[PrgId][TmpHandle].pPool != null)
					{
						if (GH.MemoryInstance.pPoolList[PrgId][TmpHandle].Type == POOL_TYPE_FILE)
						{
							pFDescrcMemoryFindLogName = (FDESCR*)GH.MemoryInstance.pPoolList[PrgId][TmpHandle].pPool;
							if ((*pFDescrcMemoryFindLogName).Access == OPEN_FOR_LOG)
							{
								CommonHelper.snprintf(pName, MAX_FILENAME_SIZE, CommonHelper.GetString((*pFDescrcMemoryFindLogName).Filename));
								TmpHandle = MAX_HANDLES;
							}
						}
					}
				}
			}
		}


		public RESULT cMemoryGetImage(DATA8* pFileName, DATA16 Size, UBYTE* pBmp)
		{
			RESULT Result = RESULT.FAIL;
			PRGID TmpPrgId;
			DATA8* FilenameBuf = CommonHelper.Pointer1d<DATA8>(MAX_FILENAME_SIZE);

			TmpPrgId = GH.Lms.CurrentProgramId();

			if (ConstructFilename(TmpPrgId, pFileName, FilenameBuf, EXT_GRAPHICS.AsSbytePointer()) == OK)
			{
				using var file = File.OpenRead(CommonHelper.GetString(FilenameBuf));
				file.ReadUnsafe(pBmp, 0, (int)Size);
				file.Close();
				Result = OK;
			}

			return (Result);
		}


		public RESULT cMemoryGetMediaName(DATA8* pChar, DATA8* pName)
		{
			// TODO: mount shite
			RESULT Result = RESULT.FAIL;
			//FILE* pFile;
			//// uncomment anime
			//// struct mntent *mountEntry;

			//pFile = setmntent("/proc/mounts", "r");

			//while ((mountEntry = getmntent(pFile)) != null)
			//{
			//	if (pChar[0] == 'm') // MMCCard detection
			//	{

			//		if (!CommonHelper.strcmp(mountEntry->mnt_dir, "/media/card"))
			//		{
			//			pName = "card";
			//			Result = OK;
			//		}

			//	}
			//	if (pChar[0] == 's') // MassStorage detection
			//	{
			//		if (!CommonHelper.strcmp(mountEntry->mnt_dir, "/media/usb"))
			//		{
			//			pName = "usb";
			//			Result = OK;
			//		}
			//	}
			//}

			//endmntent(pFile);

			GH.Ev3System.Logger.LogWarning($"Call of unimplemented shite {Environment.StackTrace}");

			return (Result);
		}

		public void cMemorySortEntry(FOLDER* pMemory, UBYTE Type, DATA8* pName)
		{
			DATA8 Sort;
			DATA8 Sort1;
			DATA8 Sort2;
			DATA8 Pointer;
			DATA8 Priority;
			DATA8 TmpPriority;
			DATA8 Favourites;
			DATA8* TmpEntry = CommonHelper.Pointer1d<DATA8>(FILENAME_SIZE);

			Sort = (*pMemory).Sort;
			Favourites = NoOfFavourites[Sort];
			Priority = Favourites;

			if ((Type != DT_DIR) && (Type != DT_LNK))
			{ // Files

				// Get extension
				Pointer = 0;
				while ((pName[Pointer] != 0) && (pName[Pointer] != '.'))
				{
					Pointer++;
				}

				// Priorities
				Priority = FavouriteExts[cMemoryFindType(&pName[Pointer])];
				if (Priority == 0)
				{
					Priority = FILETYPES;
				}
				Favourites = FILETYPES;
			}
			else
			{ // Folders

				if (Sort == SORT_PRJS)
				{
					Priority = 0;

					for (Pointer = 1; Pointer < NoOfFavourites[Sort]; Pointer++)
					{
						if (CommonHelper.strcmp(pName, pFavourites[Sort, Pointer].AsSbytePointer()) == 0)
						{
							Priority = Pointer;
						}
					}
				}
				else
				{
					for (Pointer = 0; Pointer < NoOfFavourites[Sort]; Pointer++)
					{
						if (CommonHelper.strcmp(pName, pFavourites[Sort, Pointer].AsSbytePointer()) == 0)
						{
							Priority = Pointer;
						}
					}
				}
			}
			CommonHelper.snprintf((*pMemory).GetEntry((*pMemory).Entries), FILENAME_SIZE, CommonHelper.GetString(pName));
			(*pMemory).Priority[(*pMemory).Entries] = Priority;
			((*pMemory).Entries)++;
			if (Priority < Favourites)
			{
				for (Sort1 = 0; Sort1 < ((*pMemory).Entries - 1); Sort1++)
				{
					for (Sort2 = 0; Sort2 < ((*pMemory).Entries - 1); Sort2++)
					{
						if ((*pMemory).Priority[Sort2 + 1] < (*pMemory).Priority[Sort2])
						{
							TmpPriority = (*pMemory).Priority[Sort2];
							CommonHelper.strcpy(TmpEntry, (*pMemory).GetEntry(Sort2));
							(*pMemory).Priority[Sort2] = (*pMemory).Priority[Sort2 + 1];
							CommonHelper.strcpy((*pMemory).GetEntry(Sort2), (*pMemory).GetEntry(Sort2 + 1));
							(*pMemory).Priority[Sort2 + 1] = TmpPriority;
							CommonHelper.strcpy((*pMemory).GetEntry(Sort2 + 1), TmpEntry);
						}
					}
				}
			}
		}


		public void cMemorySortList(FOLDER* pMemory)
		{
			DATA16 Pointer;

			for (Pointer = 0; Pointer < (*pMemory).Entries; Pointer++)
			{
				GH.printf($"[{CommonHelper.GetString((*pMemory).Folder)}]({(*pMemory).Sort})({(*pMemory).Priority[Pointer]}) {CommonHelper.GetString((*pMemory).GetEntry(Pointer))}\r\n");
			}
		}


		/*
		 *  Opens directory stream and allocate space for that and its items
		 *  Gives back the memory handle
		 *
		 */
		private FOLDER* pMemorycMemoryOpenFolder;
		public RESULT cMemoryOpenFolder(PRGID PrgId, DATA8 Type, DATA8* pFolderName, HANDLER* pHandle)
		{
			RESULT Result;
			
			fixed (FOLDER** pp = &pMemorycMemoryOpenFolder)
			Result = cMemoryAlloc(PrgId, POOL_TYPE_MEMORY, (GBINDEX)sizeof(FOLDER), ((void**)pp), pHandle);
			if (Result == OK)
			{
				(*pMemorycMemoryOpenFolder).pDir = null;
				(*pMemorycMemoryOpenFolder).Entries = 0;
				(*pMemorycMemoryOpenFolder).Type = Type;
				CommonHelper.snprintf((*pMemorycMemoryOpenFolder).Folder, MAX_FILENAME_SIZE, CommonHelper.GetString(pFolderName));
				(*pMemorycMemoryOpenFolder).pDir = (*pMemorycMemoryOpenFolder).Folder;
				if (*(*pMemorycMemoryOpenFolder).pDir == 0)
				{
					Result = RESULT.FAIL;
				}
				else
				{
					if (CommonHelper.strcmp(pFolderName, vmPRJS_DIR.AsSbytePointer()) == 0)
					{
						(*pMemorycMemoryOpenFolder).Sort = SORT_PRJS;
					}
					else
					{
						if (CommonHelper.strcmp(pFolderName, vmAPPS_DIR.AsSbytePointer()) == 0)
						{
							(*pMemorycMemoryOpenFolder).Sort = SORT_APPS;
						}
						else
						{
							if (CommonHelper.strcmp(pFolderName, vmTOOLS_DIR.AsSbytePointer()) == 0)
							{
								(*pMemorycMemoryOpenFolder).Sort = SORT_TOOLS;
							}
							else
							{
								(*pMemorycMemoryOpenFolder).Sort = SORT_NONE;
							}
						}
					}
				}
			}

			return (Result);
		}


		/*
		 *  Count and sort items - one for each call
		 *  Return total count
		 */
		private FOLDER* pMemorycMemoryGetFolderItems;
		[Obsolete("Idk how it works")]
		public RESULT cMemoryGetFolderItems(PRGID PrgId, HANDLER Handle, DATA16* pItems)
		{
			RESULT Result;
			
			DATA8* Ext = CommonHelper.Pointer1d<DATA8>(vmEXTSIZE);
			// uncomment anime
			//  struct dirent *pEntry;

			fixed (FOLDER** pp = &pMemorycMemoryGetFolderItems)
			Result = cMemoryGetPointer(PrgId, Handle, ((void**)pp));

			if (Result == OK)
			{ // Handle ok

				if (*(*pMemorycMemoryGetFolderItems).pDir != 0)
				{
					var dirInfo = new DirectoryInfo(CommonHelper.GetString((*pMemorycMemoryGetFolderItems).pDir));

					if ((*pMemorycMemoryGetFolderItems).Entries < DIR_DEEPT)
					{
						if (dirInfo.Name[0] != '.')
						{
							if (dirInfo.Name != "CVS")
							{
								if ((*pMemorycMemoryGetFolderItems).Type == TYPE_FOLDER)
								{
									cMemorySortEntry(pMemorycMemoryGetFolderItems, DT_DIR, dirInfo.Name.AsSbytePointer());
									GH.printf($"[{CommonHelper.GetString((*pMemorycMemoryGetFolderItems).Folder)}]({(*pMemorycMemoryGetFolderItems).Sort}) {dirInfo.Name}\r\n");
								}

								else
								{
									FindName(dirInfo.Name.AsSbytePointer(), null, null, Ext);
									if (cMemoryFindType(Ext) != 0)
									{
										cMemorySortEntry(pMemorycMemoryGetFolderItems, DT_FILE, dirInfo.Name.AsSbytePointer());
										GH.printf($"[{CommonHelper.GetString((*pMemorycMemoryGetFolderItems).Folder)}]({(*pMemorycMemoryGetFolderItems).Sort}) {dirInfo.Name}\r\n");
									}
								}
							}
						}
						
						Result = RESULT.BUSY;
					}

					else
					{ // No more entries

						cMemorySortList(pMemorycMemoryGetFolderItems);
						(*pMemorycMemoryGetFolderItems).pDir = null;
					}
				}
			}
			*pItems = ((*pMemorycMemoryGetFolderItems).Entries);

			return (Result);
		}


		/*
		 *  Get display-able name - only name not path and extension
		 */
		private FOLDER* pMemorycMemoryGetItemName;
		public RESULT cMemoryGetItemName(PRGID PrgId, HANDLER Handle, DATA16 Item, DATA8 Length, DATA8* pName, DATA8* pType, DATA8* pPriority)
		{
			RESULT Result = RESULT.FAIL;
			
			DATA8* Name = CommonHelper.Pointer1d<DATA8>(vmNAMESIZE);
			DATA8* Ext = CommonHelper.Pointer1d<DATA8>(vmEXTSIZE);

			fixed (FOLDER** pp = &pMemorycMemoryGetItemName)
			Result = cMemoryGetPointer(PrgId, Handle, ((void**)pp));
			*pType = 0;
			*pPriority = 127;

			if (Result == OK)
			{ // Handle ok

				if ((Item > 0) && (Item <= (*pMemorycMemoryGetItemName).Entries))
				{ // Item ok

					if (Length >= 2)
					{
						if (cMemoryCheckFilename((*pMemorycMemoryGetItemName).GetEntry(Item - 1), null, Name, Ext) == OK)
						{
							*pType = cMemoryFindType(Ext);
							if (CommonHelper.strlen(Name) >= Length)
							{
								Name[Length - 1] = 0;
								Name[Length - 2] = 0x7F;
							}

							CommonHelper.snprintf(pName, (int)Length, CommonHelper.GetString(Name));
							*pPriority = (*pMemorycMemoryGetItemName).Priority[Item - 1];
						}
						else
						{
							Result = RESULT.FAIL;
						}
					}
					else
					{
						Result = RESULT.FAIL;
					}
				}
				else
				{
					Result = RESULT.FAIL;
				}

			}
			return (Result);
		}


		/*
		 *  Get icon image
		 */
		private FOLDER* pMemorycMemoryGetItemIcon;
		private IP pImagecMemoryGetItemIcon;
		public RESULT cMemoryGetItemIcon(PRGID PrgId, HANDLER Handle, DATA16 Item, HANDLER* pHandle, DATA32* pImagePointer)
		{
			RESULT Result = RESULT.FAIL;
			
			DATA8* Filename = CommonHelper.Pointer1d<DATA8>(MAX_FILENAME_SIZE);
			DATA32 ISize;
			
			// uncomment anime
			//  struct stat FileStatus;

			fixed (FOLDER** pp = &pMemorycMemoryGetItemIcon)
				Result = cMemoryGetPointer(PrgId, Handle, ((void**)pp));

			if (Result == OK)
			{ // Handle ok

				Result = RESULT.FAIL;
				if ((Item > 0) && (Item <= (*pMemorycMemoryGetItemIcon).Entries))
				{ // Item ok

					CommonHelper.snprintf(Filename, MAX_FILENAME_SIZE, $"{CommonHelper.GetString((*pMemorycMemoryGetItemIcon).Folder)}/{CommonHelper.GetString((*pMemorycMemoryGetItemIcon).GetEntry(Item - 1))}/{ICON_FILE_NAME}{EXT_GRAPHICS}");

					using var hFile = File.OpenRead(CommonHelper.GetString(Filename));

					ISize = (int)hFile.Length;

					// allocate memory to contain the whole file:
					fixed (byte** pp2 = &pImagecMemoryGetItemIcon)
					if (cMemoryAlloc(PrgId, POOL_TYPE_MEMORY, (GBINDEX)ISize, (void**)pp2, pHandle) == OK)
					{

						if ((DATA32)hFile.ReadUnsafe(pImagecMemoryGetItemIcon, 0, ISize) == ISize)
						{
							*pImagePointer = (DATA32)pImagecMemoryGetItemIcon;
							Result = OK;
						}
					}

					hFile.Close();
				}
			}

			return (Result);
		}


		/*
		 *  Get text
		 */
		private FOLDER* pMemorycMemoryGetItemText;
		private DATA8* TmpcMemoryGetItemText = (DATA8*)CommonHelper.AllocateByteArray(1);
		public RESULT cMemoryGetItemText(PRGID PrgId, HANDLER Handle, DATA16 Item, DATA8 Length, DATA8* pText)
		{
			RESULT Result = RESULT.FAIL;
			
			DATA8* Filename = CommonHelper.Pointer1d<DATA8>(MAX_FILENAME_SIZE);
			
			DATA8* Termination = "\t".AsSbytePointer();
			int No;

			for (*TmpcMemoryGetItemText = 0; *TmpcMemoryGetItemText < Length; (*TmpcMemoryGetItemText)++)
			{
				pText[*TmpcMemoryGetItemText] = 0;
			}
			fixed (FOLDER** pp = &pMemorycMemoryGetItemText)
			Result = cMemoryGetPointer(PrgId, Handle, ((void**)pp));

			if (Result == OK)
			{ // Handle ok

				if ((Item > 0) && (Item <= (*pMemorycMemoryGetItemText).Entries) && Length != 0)
				{ // Item ok

					//      CommonHelper.snprintf(Filename,MAX_FILENAME_SIZE,"%s/%s/%s%s",(*pMemory).Folder,(*pMemory).Entry[Item - 1],TEXT_FILE_NAME,EXT_TEXT);
					CommonHelper.snprintf(Filename, MAX_FILENAME_SIZE, $"{vmSETTINGS_DIR}/{CommonHelper.GetString((*pMemorycMemoryGetItemText).GetEntry(Item - 1))}{EXT_TEXT}");
					using var hFile = File.OpenRead(CommonHelper.GetString(Filename));
					Result = OK;
					No = 1;
					while ((No == 1) && (Length > 1))
					{
						No = hFile.ReadUnsafe((byte*)TmpcMemoryGetItemText, 0, 1);
						if ((*TmpcMemoryGetItemText == Termination[0]) || (*TmpcMemoryGetItemText == '\r') || (*TmpcMemoryGetItemText == '\n'))
						{
							No = 0;
						}
						if (No != 0)
						{
							*pText = *TmpcMemoryGetItemText;
							pText++;
							*pText = 0;
							Length--;
						}
					}
					hFile.Close();
				}
				else
				{
					Result = RESULT.FAIL;
				}

			}

			return (Result);
		}


		/*
		 *  Get text
		 */
		private FOLDER* pMemorycMemorySetItemText;
		public RESULT cMemorySetItemText(PRGID PrgId, HANDLER Handle, DATA16 Item, DATA8* pText)
		{
			RESULT Result = RESULT.FAIL;
			
			DATA8* Filename = CommonHelper.Pointer1d<DATA8>(MAX_FILENAME_SIZE);
			DATA8 Length;

			Length = (DATA8)CommonHelper.strlen(pText);
			fixed (FOLDER** pp = &pMemorycMemorySetItemText)
			Result = cMemoryGetPointer(PrgId, Handle, ((void**)pp));

			if (Result == OK)
			{ // Handle ok

				if ((Item > 0) && (Item <= (*pMemorycMemorySetItemText).Entries) && Length != 0)
				{ // Item ok

					CommonHelper.snprintf(Filename, MAX_FILENAME_SIZE, $"{CommonHelper.GetString((*pMemorycMemorySetItemText).Folder)}/{CommonHelper.GetString((*pMemorycMemorySetItemText).GetEntry(Item - 1))}/{TEXT_FILE_NAME}{EXT_TEXT}");

					var pFile = File.OpenWrite(CommonHelper.GetString(Filename));
					pFile.WriteUnsafe((byte*)pText, 0, Length);
					Result = OK;
					pFile.Close();
				}
				else
				{
					Result = RESULT.FAIL;
				}

			}

			return (Result);
		}


		/*
		 *  Get full name including path and extension
		 */
		private FOLDER* pMemorycMemoryGetItem;
		public RESULT cMemoryGetItem(PRGID PrgId, HANDLER Handle, DATA16 Item, DATA8 Length, DATA8* pName, DATA8* pType)
		{
			RESULT Result = RESULT.FAIL;
			
			DATA8* Folder = CommonHelper.Pointer1d<DATA8>(vmPATHSIZE);
			DATA8* Name = CommonHelper.Pointer1d<DATA8>(vmNAMESIZE);
			DATA8* Ext = CommonHelper.Pointer1d<DATA8>(vmEXTSIZE);

			fixed (FOLDER** pp = &pMemorycMemoryGetItem)
			Result = cMemoryGetPointer(PrgId, Handle, ((void**)pp));
			*pType = 0;

			if (Result == OK)
			{ // Handle ok

				if ((Item > 0) && (Item <= (*pMemorycMemoryGetItem).Entries))
				{ // Item ok

					if (cMemoryCheckFilename((*pMemorycMemoryGetItem).GetEntry(Item - 1), Folder, Name, Ext) == OK)
					{
						*pType = cMemoryFindType(Ext);
						CommonHelper.snprintf(pName, (int)Length, $"{CommonHelper.GetString((*pMemorycMemoryGetItem).Folder)}{CommonHelper.GetString(Folder)}/{CommonHelper.GetString(Name)}");
					}
					else
					{
						Result = RESULT.FAIL;
					}
				}
				else
				{
					Result = RESULT.FAIL;
				}

			}

			return (Result);
		}


		/*
		 *  Close directory stream and free memory handle
		 */
		private FOLDER* pMemorycMemoryCloseFolder;
		public void cMemoryCloseFolder(PRGID PrgId, HANDLER* pHandle)
		{
			RESULT Result;
			
			fixed (FOLDER** pp = &pMemorycMemoryCloseFolder)
			Result = cMemoryGetPointer(PrgId, *pHandle, ((void**)pp));

			if (Result == OK)
			{ // Handle ok

				if ((*pMemorycMemoryCloseFolder).pDir != null)
				{
					// closedir((*pMemory).pDir);
					GH.Ev3System.Logger.LogWarning($"Call of unimplemented shite in {Environment.StackTrace}");
				}
				cMemoryFreePool(PrgId, (void*)pMemorycMemoryCloseFolder);
			}
			*pHandle = 0;
		}


		//******* BYTE CODE SNIPPETS **************************************************


		/*! \page cMemory Memory
		 *  <hr size="1"/>
		 *  \subpage MemoryLibraryCodes
		 *  \n
		 *  \n
		 *  <b>     opFILE (CMD, ....)  </b>
		 *
		 *- Memory file entry\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   CMD               - \ref memoryfilesubcode
		 *
		 *\n
		 *  - CMD = OPEN_APPEND
		 *\n  Create file or open for append (if name starts with '~','/' or '.' it is not from current folder) \n
		 *    -  \param  (DATA8)    NAME        - First character in file name (character string)\n
		 *    -  \return (HANDLER)  HANDLE      - Handle to file\n
		 *
		 *\n
		 *  - CMD = OPEN_READ
		 *\n  Open file for read (if name starts with '~','/' or '.' it is not from current folder) \n
		 *    -  \param  (DATA8)    NAME        - First character in file name (character string)\n
		 *    -  \return (HANDLER)  HANDLE      - Handle to file\n
		 *    -  \return (DATA32)   SIZE        - File size (0 = not found)\n
		 *
		 *\n
		 *  - CMD = OPEN_WRITE
		 *\n  Create file for write (if name starts with '~','/' or '.' it is not from current folder) \n
		 *    -  \param  (DATA8)    NAME        - First character in file name (character string)\n
		 *    -  \return (HANDLER)  HANDLE      - Handle to file\n
		 *
		 *\n
		 *  - CMD = CLOSE
		 *\n  Close file\n
		 *    -  \param  (HANDLER)  HANDLE      - Handle to file\n
		 *
		 *\n
		 *  - CMD = REMOVE
		 *\n  Delete file (if name starts with '~','/' or '.' it is not from current folder) \n
		 *    -  \param  (DATA8)    NAME        - First character in file name (character string)\n
		 *
		 *\n
		 *  - CMD = MOVE
		 *\n  Move file SOURCE to DEST (if name starts with '~','/' or '.' it is not from current folder) \n
		 *    -  \param  (DATA8)    SOURCE      - First character in source file name (character string)\n
		 *    -  \param  (DATA8)    DEST        - First character in destination file name (character string)\n
		 *
		 *\n
		 *  - CMD = WRITE_TEXT
		 *\n  Write text to file \n
		 *    -  \param  (HANDLER)  HANDLE      - Handle to file\n
		 *    -  \param  (DATA8)     \ref delimiters "DEL" - Delimiter code\n
		 *    -  \param  (DATA8)    TEXT        - First character in text to write (character string)\n
		 *
		 *\n
		 *  - CMD = READ_TEXT
		 *\n  Read text from file \n
		 *    -  \param  (HANDLER)  HANDLE      - Handle to file\n
		 *    -  \param  (DATA8)     \ref delimiters "DEL" - Delimiter code\n
		 *    -  \param  (DATA8)    LENGTH      - Maximal string length\n
		 *    -  \return (DATA8)    TEXT        - First character in text to read (character string)\n
		 *
		 *\n
		 *  - CMD = WRITE_VALUE
		 *\n  Write floating point value to file \n
		 *    -  \param  (HANDLER)  HANDLE      - Handle to file\n
		 *    -  \param  (DATA8)     \ref delimiters "DEL" - Delimiter code\n
		 *    -  \param  (DATAF)    VALUE       - Value to write\n
		 *    -  \param  (DATA8)    FIGURES     - Total number of figures inclusive decimal point\n
		 *    -  \param  (DATA8)    DECIMALS    - Number of decimals\n
		 *
		 *\n
		 *  - CMD = READ_VALUE
		 *\n  Read floating point value from file \n
		 *    -  \param  (HANDLER)  HANDLE      - Handle to file\n
		 *    -  \param  (DATA8)     \ref delimiters "DEL" - Delimiter code\n
		 *    -  \return (DATAF)    VALUE       - Value to write\n
		 *
		 *\n
		 *  - CMD = WRITE_BYTES
		 *\n  Write a number of bytes to file \n
		 *    -  \param  (HANDLER)  HANDLE      - Handle to file\n
		 *    -  \param  (DATA16)   BYTES       - Number of bytes to write\n
		 *    -  \param  (DATA8)    SOURCE      - First byte in byte stream to write\n
		 *
		 *\n
		 *  - CMD = READ_BYTES
		 *\n  Read a number of bytes from file \n
		 *    -  \param  (HANDLER)  HANDLE      - Handle to file\n
		 *    -  \param  (DATA16)   BYTES       - Number of bytes to write\n
		 *    -  \return (DATA8)    DESTINATION - First byte in byte stream to write\n
		 *
		 *\n
		 *  - CMD = OPEN_LOG
		 *\n  Create file for data logging (if name starts with '~','/' or '.' it is not from current folder) (see \ref cinputsample "Example")\n
		 *    -  \param  (DATA8)    NAME        - First character in file name (character string)\n
		 *    -  \param  (DATA32)   syncedTime  -
		 *    -  \param  (DATA32)   syncedTick  -
		 *    -  \param  (DATA32)   nowTick     -
		 *    -  \param  (DATA32)   sample_interval_in_ms -
		 *    -  \param  (DATA32)   duration_in_ms -
		 *    -  \param  (DATA8)    SDATA       - First character in sensor type data (character string)\n
		 *    -  \return (HANDLER)  HANDLE      - Handle to file\n
		 *
		 *\n
		 *  - CMD = WRITE_LOG
		 *\n  Write time slot samples to file (see \ref cinputsample "Example")\n
		 *    -  \param  (HANDLER)  HANDLE      - Handle to file\n
		 *    -  \param  (DATA32)   TIME        - Relative time in mS\n
		 *    -  \param  (DATA8)    ITEMS       - Total number of values in this time slot\n
		 *    .  \param  (DATAF)    VALUES      - DATAF array (handle) containing values\n
		 *
		 *\n
		 *  - CMD = CLOSE_LOG
		 *\n  Close data log file (see \ref cinputsample "Example")\n
		 *    -  \param  (HANDLER)  HANDLE      - Handle to file\n
		 *    -  \param  (DATA8)    NAME        - First character in file name (character string)\n
		 *
		 *\n
		 *  - CMD = GET_LOG_NAME
		 *\n  Get the current open log filename\n
		 *    -  \param  (DATA8)    LENGTH      - Max string length (don't care if NAME is a HND\n
		 *    -  \param  (DATA8)    NAME        - First character in file name (character string or HND)\n
		 *
		 *\n
		 *  - CMD = GET_HANDLE
		 *\n  Get handle from filename (if name starts with '~','/' or '.' it is not from current folder) \n
		 *    -  \param  (DATA8)    NAME        - First character in file name (character string)\n
		 *    -  \return (HANDLER)  HANDLE      - Handle to file\n
		 *    -  \return (DATA8)    WRITE       - Open for write / append (0 = no, 1 = yes)\n
		 *
		 *\n
		 *  - CMD = MAKE_FOLDER
		 *\n  Make folder if not present\n
		 *    -  \param  (DATA8)    NAME        - First character in folder name (character string)\n
		 *    -  \return (DATA8)    SUCCESS     - Success (0 = no, 1 = yes)\n
		 *
		 *\n
		 *  - CMD = LOAD_IMAGE
		 *    -  \param  (DATA16)   PRGID   - Program id (see \ref prgid)\n
		 *    -  \param  (DATA8)    NAME    - First character in name (character string)\n
		 *    -  \return (DATA32)   SIZE    - Size\n
		 *    -  \return (DATA32)   *IP     - Address of image\n
		 *
		 *\n
		 *  - CMD = GET_POOL
		 *    -  \param  (DATA32)   SIZE    - Size of pool\n
		 *    -  \return (HANDLER)  HANDLE  - Handle to pool\n
		 *    -  \return (DATA32)   *IP     - Address of image\n
		 *
		 *\n
		 *  - CMD = GET_FOLDERS
		 *    -  \param  (DATA8)    NAME    - First character in folder name (ex "../prjs/")\n
		 *    -  \return (DATA8)    ITEMS   - No of sub folders\n
		 *
		 *\n
		 *  - CMD = GET_SUBFOLDER_NAME
		 *    -  \param  (DATA8)    NAME    - First character in folder name (ex "../prjs/")\n
		 *    -  \param  (DATA8)    ITEM    - Sub folder index [1..ITEMS]\n
		 *    -  \param  (DATA8)    LENGTH  - Maximal string length\n
		 *    -  \return (DATA8)    STRING  - First character in character string\n
		 *
		 *\n
		 *  - CMD = DEL_SUBFOLDER
		 *    -  \param  (DATA8)    NAME    - First character in folder name (ex "../prjs/")\n
		 *    -  \param  (DATA8)    ITEM    - Sub folder index [1..ITEMS]\n
		 *
		 *\n
		 *  - CMD = SET_LOG_SYNC_TIME
		 *    -  \param  (DATA32)   TIME    - Sync time used in data log files\n
		 *    -  \param  (DATA32)   TICK    - Sync tick used in data log files\n
		 *
		 *\n
		 *  - CMD = GET_LOG_SYNC_TIME
		 *    -  \return (DATA32)   TIME    - Sync time used in data log files\n
		 *    -  \return (DATA32)   TICK    - Sync tick used in data log files\n
		 *
		 *\n
		 *  - CMD = GET_IMAGE
		 *    -  \param  (DATA8)    NAME    - First character in folder name (ex "../prjs/")\n
		 *    -  \param  (DATA16)   PRGID   - Program id (see \ref prgid)\n
		 *    -  \param  (DATA8)    ITEM    - Sub folder index [1..ITEMS]\n
		 *    -  \return (DATA32)   *IP     - Address of image\n
		 *
		 *\n
		 *  - CMD = GET_ITEM
		 *    -  \param  (DATA8)    NAME    - First character in folder name (ex "../prjs/")\n
		 *    -  \param  (DATA8)    STRING  - First character in item name string\n
		 *    -  \return (DATA8)    ITEM    - Sub folder index [1..ITEMS]\n
		 *
		 *\n
		 *  - CMD = GET_CACHE_FILES
		 *    -  \return (DATA8)    ITEMS   - No of files in cache\n
		 *
		 *\n
		 *  - CMD = GET_CACHE_FILE
		 *    -  \param  (DATA8)    ITEM    - Cache index [1..ITEMS]\n
		 *    -  \param  (DATA8)    LENGTH  - Maximal string length\n
		 *    -  \return (DATA8)    STRING  - First character in character string\n
		 *
		 *\n
		 *  - CMD = PUT_CACHE_FILE
		 *    -  \param  (DATA8)    STRING  - First character in character string\n
		 *
		 *\n
		 *  - CMD = DEL_CACHE_FILE
		 *    -  \param  (DATA8)    ITEM    - Cache index [1..ITEMS]\n
		 *    -  \param  (DATA8)    LENGTH  - Maximal string length\n
		 *    -  \return (DATA8)    STRING  - First character in character string\n
		 *
		 *\n
		 *
		 */
		/*! \brief  opFILE byte code
		 *
		 */
		private HANDLER* TmpHandlecMemoryFile = (HANDLER*)CommonHelper.AllocateByteArray(2);
		private DATA32* ISizecMemoryFile = (DATA32*)CommonHelper.AllocateByteArray(4);
		private DATAF* DataFcMemoryFile = (DATAF*)CommonHelper.AllocateByteArray(4);
		private void* pTmpcMemoryFile;
		private DATA32* TotalRamcMemoryFile = (DATA32*)CommonHelper.AllocateByteArray(4);
		private DATA32* FreeRamcMemoryFile = (DATA32*)CommonHelper.AllocateByteArray(4);
		private DATA8* TmpcMemoryFile = (DATA8*)CommonHelper.AllocateByteArray(1);
		private HANDLER* TmpHandle2cMemoryFile = (HANDLER*)CommonHelper.AllocateByteArray(2);
		private IP pImagecMemoryFile;
		public void cMemoryFile()
		{

			IP TmpIp;
			DSPSTAT DspStat = DSPSTAT.BUSYBREAK;
			DATA8 Cmd;
			long ImagePointer;
			
			
			
			PRGID TmpPrgId;
			DATA32 Data32;

			DATA8* pFileName;
			DATA8* FilenameBuf = CommonHelper.Pointer1d<DATA8>(vmFILENAMESIZE);
			DATA8* PathBuf = CommonHelper.Pointer1d<DATA8>(vmPATHSIZE);
			DATA8* NameBuf = CommonHelper.Pointer1d<DATA8>(vmNAMESIZE);
			DATA8* ExtBuf = CommonHelper.Pointer1d<DATA8>(vmEXTSIZE);

			DATA8* SourceBuf = CommonHelper.Pointer1d<DATA8>(vmFILENAMESIZE);
			DATA8* DestinationBuf = CommonHelper.Pointer1d<DATA8>(vmFILENAMESIZE);

			DATA8* pSource;
			DATA8* pDestination;

			DATA8* PrgNamePath = CommonHelper.Pointer1d<DATA8>(SUBFOLDERNAME_SIZE);
			DATA8* PrgNameBuf = CommonHelper.Pointer1d<DATA8>(MAX_FILENAME_SIZE);
			DATA8* DestinationName = CommonHelper.Pointer1d<DATA8>(MAX_FILENAME_SIZE);
			DATA16 PrgNo;
			DATA8 Item;
			DATA8 Items;
			DATA8 Lng;
			
			DATA8 Del;
			DATA8* pFolderName;
			DATA8* Buffer = CommonHelper.Pointer1d<DATA8>(LOGBUFFER_SIZE);
			
			DATA16 Bytes;
			DATA8 Figures;
			DATA8 Decimals;
			// uncomment anime
			// struct stat FileStatus;
			DATAF* pValue;
			DATA32 Time;
			DESCR* pDescr;
			DATA32 UsedElements;
			DATA32 Elements;
			DATA32 ElementSize;

			DATA32 STime;
			DATA32 STick;
			DATA32 NTick;
			DATA32 SIIM;
			DATA32 DIM;
			DATA8* pSData;
			DATA8 Error = 0;
			

			
			

			TmpPrgId = GH.Lms.CurrentProgramId();
			TmpIp = GH.Lms.GetObjectIp();
			Cmd = *(DATA8*)GH.Lms.PrimParPointer();
			ImagePointer = (DATA32)0;
			*ISizecMemoryFile = (DATA32)0;

			switch (Cmd)
			{ // Function

				case OPEN_APPEND:
					{
						pFileName = (DATA8*)GH.Lms.PrimParPointer();

						if (ConstructFilename(TmpPrgId, pFileName, FilenameBuf, "".AsSbytePointer()) == OK)
						{

							GH.printf($"c_memory  cMemoryFile: OPEN_APPEND [{CommonHelper.GetString(FilenameBuf)}]\r\n");
							DspStat = cMemoryOpenFile(TmpPrgId, OPEN_FOR_APPEND, FilenameBuf, TmpHandlecMemoryFile, ISizecMemoryFile);
						}

						*(DATA16*)GH.Lms.PrimParPointer() = *TmpHandlecMemoryFile;
					}
					break;

				case OPEN_READ:
					{
						pFileName = (DATA8*)GH.Lms.PrimParPointer();

						if (ConstructFilename(TmpPrgId, pFileName, FilenameBuf, "".AsSbytePointer()) == OK)
						{

							GH.printf($"c_memory  cMemoryFile: OPEN_READ   [{CommonHelper.GetString(FilenameBuf)}]\r\n");
							DspStat = cMemoryOpenFile(TmpPrgId, OPEN_FOR_READ, FilenameBuf, TmpHandlecMemoryFile, ISizecMemoryFile);
						}

						*(DATA16*)GH.Lms.PrimParPointer() = *TmpHandlecMemoryFile;
						*(DATA32*)GH.Lms.PrimParPointer() = *ISizecMemoryFile;
					}
					break;

				case OPEN_WRITE:
					{
						pFileName = (DATA8*)GH.Lms.PrimParPointer();

						if (ConstructFilename(TmpPrgId, pFileName, FilenameBuf, "".AsSbytePointer()) == OK)
						{

							GH.printf($"c_memory  cMemoryFile: OPEN_WRITE  [{CommonHelper.GetString(FilenameBuf)}]\r\n");
							DspStat = cMemoryOpenFile(TmpPrgId, OPEN_FOR_WRITE, FilenameBuf, TmpHandlecMemoryFile, ISizecMemoryFile);

						}

						*(DATA16*)GH.Lms.PrimParPointer() = *TmpHandlecMemoryFile;
					}
					break;

				case CLOSE:
					{
						*TmpHandlecMemoryFile = *(DATA16*)GH.Lms.PrimParPointer();

						DspStat = cMemoryCloseFile(TmpPrgId, *TmpHandlecMemoryFile);
					}
					break;

				case WRITE_TEXT:
					{
						*TmpHandlecMemoryFile = *(DATA16*)GH.Lms.PrimParPointer();
						Del = *(DATA8*)GH.Lms.PrimParPointer();
						pSource = (DATA8*)GH.Lms.PrimParPointer();

						DspStat = cMemoryWriteFile(TmpPrgId, *TmpHandlecMemoryFile, (DATA32)CommonHelper.strlen(pSource), Del, pSource);
					}
					break;

				case READ_TEXT:
					{
						*TmpHandlecMemoryFile = *(DATA16*)GH.Lms.PrimParPointer();
						Del = *(DATA8*)GH.Lms.PrimParPointer();
						Lng = (DATA8)(*(DATA16*)GH.Lms.PrimParPointer());
						pDestination = (DATA8*)GH.Lms.PrimParPointer();

						DspStat = cMemoryReadFile(TmpPrgId, *TmpHandlecMemoryFile, (DATA32)Lng, Del, pDestination);
					}
					break;

				case WRITE_VALUE:
					{
						*TmpHandlecMemoryFile = *(DATA16*)GH.Lms.PrimParPointer();
						Del = *(DATA8*)GH.Lms.PrimParPointer();
						*DataFcMemoryFile = *(DATAF*)GH.Lms.PrimParPointer();
						Figures = *(DATA8*)GH.Lms.PrimParPointer();
						Decimals = *(DATA8*)GH.Lms.PrimParPointer();

						CommonHelper.snprintf(Buffer, LOGBUFFER_SIZE, CommonHelper.GetString(*DataFcMemoryFile, Figures, Decimals));
						DspStat = cMemoryWriteFile(TmpPrgId, *TmpHandlecMemoryFile, (DATA32)CommonHelper.strlen(Buffer), Del, (DATA8*)Buffer);
					}
					break;

				case READ_VALUE:
					{
						*TmpHandlecMemoryFile = *(DATA16*)GH.Lms.PrimParPointer();
						Del = *(DATA8*)GH.Lms.PrimParPointer();

						Lng = 64;
						Buffer[0] = 0;
						pDestination = (DATA8*)Buffer;
						*DataFcMemoryFile = (DATAF)0;
						DspStat = cMemoryReadFile(TmpPrgId, *TmpHandlecMemoryFile, (DATA32)Lng, Del, pDestination);
						CommonHelper.sscanf(Buffer, "%f", DataFcMemoryFile);

						*(DATAF*)GH.Lms.PrimParPointer() = *DataFcMemoryFile;
					}
					break;

				case WRITE_BYTES:
					{
						*TmpHandlecMemoryFile = *(DATA16*)GH.Lms.PrimParPointer();
						Bytes = *(DATA16*)GH.Lms.PrimParPointer();
						pSource = (DATA8*)GH.Lms.PrimParPointer();

						DspStat = cMemoryWriteFile(TmpPrgId, *TmpHandlecMemoryFile, (DATA32)Bytes, DEL_NONE, pSource);
					}
					break;

				case READ_BYTES:
					{
						*TmpHandlecMemoryFile = *(DATA16*)GH.Lms.PrimParPointer();
						Bytes = *(DATA16*)GH.Lms.PrimParPointer();
						pDestination = (DATA8*)GH.Lms.PrimParPointer();

						DspStat = cMemoryReadFile(TmpPrgId, *TmpHandlecMemoryFile, (DATA32)Bytes, DEL_NONE, pDestination);
					}
					break;

				case OPEN_LOG:
					{
						pFileName = (DATA8*)GH.Lms.PrimParPointer();

						STime = *(DATA32*)GH.Lms.PrimParPointer();
						STick = *(DATA32*)GH.Lms.PrimParPointer();
						NTick = *(DATA32*)GH.Lms.PrimParPointer();
						SIIM = *(DATA32*)GH.Lms.PrimParPointer();
						DIM = *(DATA32*)GH.Lms.PrimParPointer();

						pSData = (DATA8*)GH.Lms.PrimParPointer();

						*TmpHandlecMemoryFile = 0;

						if (ConstructFilename(TmpPrgId, pFileName, FilenameBuf, vmEXT_DATALOG.AsSbytePointer()) == OK)
						{

							GH.printf($"c_memory  cMemoryFile: OPEN_LOG    [{CommonHelper.GetString(FilenameBuf)}]\r\n");
							Bytes = (short)CommonHelper.snprintf(Buffer, LOGBUFFER_SIZE, $"Sync data\t{STime}\t{STick}\t{NTick}\t{SIIM}\t{DIM}\r\n{CommonHelper.GetString(pSData)}");

							DspStat = DSPSTAT.NOBREAK;

							if ((SIIM < MIN_LIVE_UPDATE_TIME) || (FilenameBuf[0] == 0))
							{ // Log in ram

								if (FilenameBuf[0] != 0)
								{
									DspStat = cMemoryOpenFile(TmpPrgId, OPEN_FOR_LOG, FilenameBuf, TmpHandlecMemoryFile, ISizecMemoryFile);
								}
								if (DspStat == DSPSTAT.NOBREAK)
								{
									Elements = LOGBUFFER_SIZE;

									ElementSize = sizeof(DATA8);
									*ISizecMemoryFile = Elements * ElementSize + sizeof(DESCR);

									fixed (void** pp = &pTmpcMemoryFile)
									if (cMemoryAlloc(TmpPrgId, POOL_TYPE_MEMORY, (GBINDEX)(*ISizecMemoryFile), (void**)pp, TmpHandlecMemoryFile) == OK)
									{
										(*(DESCR*)pTmpcMemoryFile).Type = DATA_8;
										(*(DESCR*)pTmpcMemoryFile).ElementSize = (DATA8)ElementSize;
										(*(DESCR*)pTmpcMemoryFile).Elements = Elements;
										(*(DESCR*)pTmpcMemoryFile).UsedElements = 0;

										if (pFileName[0] != 0)
										{
											GH.printf($"LOG_OPEN  {*TmpHandlecMemoryFile} into ram file {CommonHelper.GetString(FilenameBuf)}\r\n");
											GH.printf($"  header  {*TmpHandlecMemoryFile} into ram file %{Bytes} bytes\r\n");
										}
										else
										{
											GH.printf($"LOG_OPEN  {*TmpHandlecMemoryFile} into ram\r\n");
											GH.printf($"  header  {*TmpHandlecMemoryFile} into ram {Bytes} bytes\r\n");
										}
										pDescr = (DESCR*)pTmpcMemoryFile;

										pDestination = (DATA8*)(*pDescr).pArray;
										UsedElements = (*pDescr).UsedElements;

										Elements = 0;
										while (Bytes != 0)
										{
											pDestination[UsedElements] = Buffer[Elements];
											UsedElements++;
											Elements++;
											Bytes--;
										}
									  (*pDescr).UsedElements = UsedElements;
									}
									else
									{
										DspStat = DSPSTAT.FAILBREAK;
									}
								}
							}
							else
							{ // Log in file

								DspStat = cMemoryOpenFile(TmpPrgId, OPEN_FOR_LOG, FilenameBuf, TmpHandlecMemoryFile, ISizecMemoryFile);
								if (DspStat == DSPSTAT.NOBREAK)
								{
									DspStat = cMemoryWriteFile(TmpPrgId, *TmpHandlecMemoryFile, (DATA32)Bytes, DEL_NONE, (DATA8*)Buffer);
									GH.printf($"LOG_OPEN  {*TmpHandlecMemoryFile} into file {CommonHelper.GetString(pFileName)}\r\n");
									GH.printf($"  header  {*TmpHandlecMemoryFile} file {Bytes} bytes\r\n");
								}
							}
						}
						*(HANDLER*)GH.Lms.PrimParPointer() = (HANDLER)(*TmpHandlecMemoryFile);
					}
					break;

				case WRITE_LOG:
					{
						*TmpHandlecMemoryFile = *(DATA16*)GH.Lms.PrimParPointer();
						Time = *(DATA32*)GH.Lms.PrimParPointer();
						Items = *(DATA8*)GH.Lms.PrimParPointer();
						pValue = (DATAF*)GH.Lms.PrimParPointer();

						DspStat = DSPSTAT.FAILBREAK;

						if (Items != 0)
						{
							Bytes = (DATA16)CommonHelper.snprintf(Buffer, LOGBUFFER_SIZE, $"{Time}\t");
							for (Item = 0; Item < Items; Item++)
							{

								if (Item != (Items - 1))
								{
									Bytes += (short)CommonHelper.snprintf(&Buffer[Bytes], LOGBUFFER_SIZE - Bytes, $"{CommonHelper.GetString(pValue[Item], -1, 1)}\t");
								}
								else
								{
									Bytes += (short)CommonHelper.snprintf(&Buffer[Bytes], LOGBUFFER_SIZE - Bytes, $"{CommonHelper.GetString(pValue[Item], -1, 1)}\r\n");
								}
							}

							if (GH.MemoryInstance.pPoolList[TmpPrgId][*TmpHandlecMemoryFile].Type == POOL_TYPE_MEMORY)
							{ // Log to memory

								fixed (void** pp = &pTmpcMemoryFile)
								if (cMemoryGetPointer(TmpPrgId, *TmpHandlecMemoryFile, pp) == OK)
								{
									pDescr = (DESCR*)pTmpcMemoryFile;

									UsedElements = (DATA32)Bytes + (*pDescr).UsedElements;
									Elements = (*pDescr).Elements;

									if (UsedElements > Elements)
									{
										Elements += LOGBUFFER_SIZE;
										GH.printf($"LOG_WRITE {*TmpHandlecMemoryFile} resizing ram to {Elements}\r\n");
										cMemoryGetUsage(TotalRamcMemoryFile, FreeRamcMemoryFile, 0);
										GH.printf($"Free memory {*FreeRamcMemoryFile} KB\r\n");
										if (*FreeRamcMemoryFile > RESERVED_MEMORY)
										{
											if (cMemoryResize(TmpPrgId, *TmpHandlecMemoryFile, Elements) == null)
											{
												Error = OUT_OF_MEMORY;
											}
										}
										else
										{
											Error = OUT_OF_MEMORY;
										}

									}
									if (Error == 0)
									{
										fixed (void** pp2 = &pTmpcMemoryFile)
										if (cMemoryGetPointer(TmpPrgId, *TmpHandlecMemoryFile, pp2) == OK)
										{
											pDescr = (DESCR*)pTmpcMemoryFile;

											pDestination = (DATA8*)(*pDescr).pArray;
											UsedElements = (*pDescr).UsedElements;

											GH.printf($"LOG_WRITE {*TmpHandlecMemoryFile} ram {Bytes} bytes\r\n");
											CommonHelper.memcpy((byte*)&pDestination[UsedElements], (byte*)Buffer, (int)Bytes);
											(*pDescr).UsedElements = UsedElements + (DATA32)Bytes;

											DspStat = DSPSTAT.NOBREAK;

										}
										else
										{
											Error = OUT_OF_MEMORY;
										}
									}
								}

							}
							else
							{ // Log to file

								GH.printf($"LOG_WRITE {*TmpHandlecMemoryFile} file {Bytes} bytes\r\n");
								DspStat = cMemoryWriteFile(TmpPrgId, *TmpHandlecMemoryFile, (DATA32)Bytes, DEL_NONE, (DATA8*)Buffer);
							}
						}
						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case CLOSE_LOG:
					{
						*TmpHandlecMemoryFile = *(DATA16*)GH.Lms.PrimParPointer();
						pFileName = (DATA8*)GH.Lms.PrimParPointer();

						if (ConstructFilename(TmpPrgId, pFileName, FilenameBuf, vmEXT_DATALOG.AsSbytePointer()) == OK)
						{
							DspStat = cMemoryGetFileHandle(TmpPrgId, FilenameBuf, TmpHandle2cMemoryFile, TmpcMemoryFile);
							GH.printf($"c_memory  cMemoryFile: CLOSE_LOG   [{CommonHelper.GetString(FilenameBuf)}]\r\n");

							if (GH.MemoryInstance.pPoolList[TmpPrgId][*TmpHandlecMemoryFile].Type == POOL_TYPE_MEMORY)
							{ // Log to memory

								if (FilenameBuf[0] != 0)
								{
									fixed (void** pp2 = &pTmpcMemoryFile)
									if (cMemoryGetPointer(TmpPrgId, *TmpHandlecMemoryFile, pp2) == OK)
									{
										pDescr = (DESCR*)pTmpcMemoryFile;

										if (DspStat == DSPSTAT.NOBREAK)
										{
											pSource = (DATA8*)(*pDescr).pArray;
											UsedElements = (*pDescr).UsedElements;

											Buffer[0] = (sbyte)'F';
											Buffer[1] = (sbyte)'F';
											Buffer[2] = (sbyte)'F';
											Buffer[3] = (sbyte)'F';
											Buffer[4] = (sbyte)'F';
											Buffer[5] = (sbyte)'F';
											Buffer[6] = (sbyte)'F';
											Buffer[7] = (sbyte)'F';
											Buffer[8] = (sbyte)'\r';
											Buffer[9] = (sbyte)'\n';

											Bytes = 10;


											GH.printf($"LOG_WRITE {*TmpHandlecMemoryFile} ram {Bytes} log end signature\r\n");
											CommonHelper.memcpy((byte*)&pSource[UsedElements], (byte*)Buffer, (int)Bytes);
											UsedElements += (DATA32)Bytes;

											cMemoryGetUsage(TotalRamcMemoryFile, FreeRamcMemoryFile, 0);
											if (UsedElements > ((*FreeRamcMemoryFile - RESERVED_MEMORY) * KB))
											{
												UsedElements = (*FreeRamcMemoryFile - RESERVED_MEMORY) * KB;
											}

											GH.printf($"LOG_CLOSE {*TmpHandlecMemoryFile} ram and save {UsedElements} bytes to {CommonHelper.GetString(pFileName)}\r\n");
											DspStat = cMemoryWriteFile(TmpPrgId, *TmpHandle2cMemoryFile, (DATA32)UsedElements, DEL_NONE, pSource);
										}
										if (DspStat == DSPSTAT.NOBREAK)
										{
											DspStat = cMemoryCloseFile(TmpPrgId, *TmpHandle2cMemoryFile);
										}
									}
								}
								else
								{
									cMemoryFreeHandle(TmpPrgId, *TmpHandlecMemoryFile);

									DspStat = DSPSTAT.NOBREAK;
								}
							}
							else
							{
								Buffer[0] = (sbyte)'F';
								Buffer[1] = (sbyte)'F';
								Buffer[2] = (sbyte)'F';
								Buffer[3] = (sbyte)'F';
								Buffer[4] = (sbyte)'F';
								Buffer[5] = (sbyte)'F';
								Buffer[6] = (sbyte)'F';
								Buffer[7] = (sbyte)'F';
								Buffer[8] = (sbyte)'\r';
								Buffer[9] = (sbyte)'\n';

								Bytes = 10;

								GH.printf($"LOG_WRITE {*TmpHandlecMemoryFile} file {Bytes} 0xFF\r\n");
								DspStat = cMemoryWriteFile(TmpPrgId, *TmpHandlecMemoryFile, (DATA32)Bytes, DEL_NONE, (DATA8*)Buffer);

								DspStat = cMemoryCloseFile(TmpPrgId, *TmpHandlecMemoryFile);

								GH.printf($"LOG_CLOSE {*TmpHandlecMemoryFile} file\r\n");
							}
						}
						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case GET_LOG_NAME:
					{
						Lng = *(DATA8*)GH.Lms.PrimParPointer();
						pFileName = (DATA8*)GH.Lms.PrimParPointer();

						cMemoryFindLogName(TmpPrgId, DestinationName);

						if (GH.VMInstance.Handle >= 0)
						{
							Data32 = (DATA32)CommonHelper.strlen(DestinationName);
							Data32 += 1;
							if (Data32 > MIN_ARRAY_ELEMENTS)
							{
								pFileName = (DATA8*)GH.Lms.VmMemoryResize(GH.VMInstance.Handle, Data32);
							}
							Lng = (DATA8)Data32;
						}
						if (pFileName != null)
						{
							CommonHelper.snprintf(pFileName, (int)Lng, CommonHelper.GetString(DestinationName));
						}
						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case GET_HANDLE:
					{
						pFileName = (DATA8*)GH.Lms.PrimParPointer();

						*TmpHandlecMemoryFile = 0;
						*TmpcMemoryFile = 0;

						if (ConstructFilename(TmpPrgId, pFileName, FilenameBuf, "".AsSbytePointer()) == OK)
						{
							DspStat = cMemoryGetFileHandle(TmpPrgId, FilenameBuf, TmpHandlecMemoryFile, TmpcMemoryFile);
							GH.printf($"c_memory  cMemoryFile: GET_HANDLE  [{CommonHelper.GetString(FilenameBuf)}]\r\n");
						}

						*(DATA16*)GH.Lms.PrimParPointer() = *TmpHandlecMemoryFile;
						*(DATA8*)GH.Lms.PrimParPointer() = *TmpcMemoryFile;
					}
					break;

				case REMOVE:
					{
						pFileName = (DATA8*)GH.Lms.PrimParPointer();

						if (ConstructFilename(TmpPrgId, pFileName, FilenameBuf, "".AsSbytePointer()) == OK)
						{
							cMemoryDeleteSubFolders(FilenameBuf);
							GH.printf($"c_memory  cMemoryFile: REMOVE      [{CommonHelper.GetString(FilenameBuf)}]\r\n");
							GH.Lms.SetUiUpdate();
						}
						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case MAKE_FOLDER:
					{
						pDestination = (DATA8*)GH.Lms.PrimParPointer();

						CommonHelper.snprintf(PathBuf, vmPATHSIZE, CommonHelper.GetString(pDestination));

						Data32 = (DATA32)CommonHelper.strlen(PathBuf);
						if (Data32 != 0)
						{
							if (PathBuf[Data32 - 1] == '/')
							{
								PathBuf[Data32 - 1] = 0;
							}
						}

						*TmpcMemoryFile = 0;
						if (Directory.Exists(CommonHelper.GetString(PathBuf)))
						{
							*TmpcMemoryFile = 1;
							GH.printf($"c_memory  cMemoryFile: MAKE_FOLDER [{CommonHelper.GetString(PathBuf)}] already present\r\n");
						}
						else
						{
							Directory.CreateDirectory(CommonHelper.GetString(PathBuf));

							GH.printf($"c_memory  cMemoryFile: MAKE_FOLDER [{CommonHelper.GetString(PathBuf)}]\r\n");

							if (Directory.Exists(CommonHelper.GetString(PathBuf)))
							{
								*TmpcMemoryFile = 1;
							}
						}
						*(DATA8*)GH.Lms.PrimParPointer() = *TmpcMemoryFile;

						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case MOVE:
					{
						pSource = (DATA8*)GH.Lms.PrimParPointer();
						pDestination = (DATA8*)GH.Lms.PrimParPointer();

						if (ConstructFilename(TmpPrgId, pSource, SourceBuf, "".AsSbytePointer()) == OK)
						{
							if (ConstructFilename(TmpPrgId, pDestination, DestinationBuf, "".AsSbytePointer()) == OK)
							{

								CommonHelper.snprintf(Buffer, LOGBUFFER_SIZE, $"cp -r \"{CommonHelper.GetString(SourceBuf)}\" \"{CommonHelper.GetString(DestinationBuf)}\"");
								GH.printf($"c_memory  cMemoryFile: MOVE        [{CommonHelper.GetString(Buffer)}]\r\n");

								if (Directory.Exists(CommonHelper.GetString(DestinationBuf)) || File.Exists(CommonHelper.GetString(DestinationBuf)))
								{ // Exist

									cMemoryDeleteSubFolders(DestinationBuf);
									GH.printf($"  c_memory  cMemoryFile: remove    [{CommonHelper.GetString(DestinationBuf)}]\r\n");
								}

								Directory.Move(CommonHelper.GetString(SourceBuf), CommonHelper.GetString(DestinationBuf));
								GH.Lms.SetUiUpdate();
							}
						}

						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case LOAD_IMAGE:
					{
						PrgNo = *(DATA16*)GH.Lms.PrimParPointer();
						pFileName = (DATA8*)GH.Lms.PrimParPointer();
						DspStat = DSPSTAT.FAILBREAK;
						*ISizecMemoryFile = 0;
						ImagePointer = 0;

						if (GH.Lms.ProgramStatus((ushort)PrgNo) == OBJSTAT.STOPPED)
						{
							if (cMemoryCheckFilename(pFileName, PathBuf, NameBuf, ExtBuf) == OK)
							{ // Filename OK

								if (PathBuf[0] == 0)
								{ // Default path

									CommonHelper.snprintf(PathBuf, vmPATHSIZE, $"{DEFAULT_FOLDER}/");
								}

								// WARNING: "Resources" added to the beginning of the path
								CommonHelper.snprintf(PathBuf, vmPATHSIZE, $"{DEFAULT_FOLDER_CSHARP}/{CommonHelper.GetString(PathBuf)}");

								if (ExtBuf[0] == 0)
								{ // Default extension

									CommonHelper.snprintf(ExtBuf, vmEXTSIZE, EXT_BYTECODE);
								}

								// Save path
								CommonHelper.snprintf(GH.MemoryInstance.PathList[PrgNo], vmPATHSIZE, CommonHelper.GetString(PathBuf));

								// Save name for run screen
								CommonHelper.snprintf((sbyte*)GH.VMInstance.Program[PrgNo].Name, vmNAMESIZE, CommonHelper.GetString(NameBuf));

								// Construct filename for open
								CommonHelper.snprintf(FilenameBuf, vmFILENAMESIZE, $"{CommonHelper.GetString(PathBuf)}{CommonHelper.GetString(NameBuf)}{CommonHelper.GetString(ExtBuf)}");

								GH.printf($"c_memory  cMemoryFile: LOAD_IMAGE  [{CommonHelper.GetString(FilenameBuf)}]\r\n");
								using var hFile = File.OpenRead(CommonHelper.GetString(FilenameBuf));

								*ISizecMemoryFile = (int)hFile.Length;

								// allocate memory to contain the whole file:
								fixed (byte** pp = &pImagecMemoryFile)
								if (cMemoryAlloc((ushort)PrgNo, POOL_TYPE_MEMORY, (GBINDEX)(* ISizecMemoryFile), (void**)pp, TmpHandlecMemoryFile) == OK)
								{
									hFile.ReadUnsafe(pImagecMemoryFile, 0, *ISizecMemoryFile);
									ImagePointer = (DATA32)pImagecMemoryFile;
									DspStat = DSPSTAT.NOBREAK;
								}

								hFile.Close();

								GH.printf($"c_memory  cMemoryFile: LOAD_IMAGE done reading file [{CommonHelper.GetString(FilenameBuf)}]\r\n");

								*(DATA32*)GH.Lms.PrimParPointer() = *ISizecMemoryFile;
								*(DATA32*)GH.Lms.PrimParPointer() = (int)ImagePointer;
							}
							else
							{
								GH.Lms.PrimParPointer();
								GH.Lms.PrimParPointer();
							}
						}
						else
						{
							GH.Lms.PrimParPointer();
							GH.Lms.PrimParPointer();
						}
						DspStat = DSPSTAT.NOBREAK;

						GH.printf($"c_memory  cMemoryFile: LOAD_IMAGE done [{CommonHelper.GetString(FilenameBuf)}]\r\n");
					}
					break;

				case GET_POOL:
					{
						*ISizecMemoryFile = *(DATA32*)GH.Lms.PrimParPointer();
						DspStat = DSPSTAT.FAILBREAK;
						*TmpHandlecMemoryFile = -1;

						fixed (byte** pp = &pImagecMemoryFile)
						if (cMemoryAlloc(TmpPrgId, POOL_TYPE_MEMORY, (GBINDEX)(*ISizecMemoryFile), (void**)pp, TmpHandlecMemoryFile) == OK)
						{
							ImagePointer = (DATA32)pImagecMemoryFile;
							DspStat = DSPSTAT.NOBREAK;
						}

						*(DATA16*)GH.Lms.PrimParPointer() = *TmpHandlecMemoryFile;
						*(DATA32*)GH.Lms.PrimParPointer() = (int)ImagePointer;
					}
					break;

				case GET_FOLDERS:
					{
						pFolderName = (DATA8*)GH.Lms.PrimParPointer();
						*(DATA8*)GH.Lms.PrimParPointer() = cMemoryFindSubFolders(pFolderName);

						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case GET_SUBFOLDER_NAME:
					{
						pFolderName = (DATA8*)GH.Lms.PrimParPointer();
						Item = *(DATA8*)GH.Lms.PrimParPointer();
						Lng = *(DATA8*)GH.Lms.PrimParPointer();
						pDestination = (DATA8*)GH.Lms.PrimParPointer();

						cMemoryGetSubFolderName(Item, Lng, pFolderName, pDestination);

						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case DEL_SUBFOLDER:
					{
						pFolderName = (DATA8*)GH.Lms.PrimParPointer();
						Item = *(DATA8*)GH.Lms.PrimParPointer();
						cMemoryGetSubFolderName(Item, SUBFOLDERNAME_SIZE, pFolderName, PrgNamePath);

						CommonHelper.snprintf(PrgNameBuf, MAX_FILENAME_SIZE, $"{CommonHelper.GetString(pFolderName)}{CommonHelper.GetString(PrgNamePath)}");
						GH.printf($"Trying to delete {CommonHelper.GetString(PrgNameBuf)}\r\n");
						cMemoryDeleteSubFolders(PrgNameBuf);

						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case SET_LOG_SYNC_TIME:
					{
						GH.MemoryInstance.SyncTime = *(DATA32*)GH.Lms.PrimParPointer();
						GH.MemoryInstance.SyncTick = *(DATA32*)GH.Lms.PrimParPointer();

						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case GET_LOG_SYNC_TIME:
					{
						*(DATA32*)GH.Lms.PrimParPointer() = GH.MemoryInstance.SyncTime;
						*(DATA32*)GH.Lms.PrimParPointer() = GH.MemoryInstance.SyncTick;

						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case GET_ITEM:
					{
						pFolderName = (DATA8*)GH.Lms.PrimParPointer();
						pFileName = (DATA8*)GH.Lms.PrimParPointer();
						DspStat = DSPSTAT.NOBREAK;

						Items = cMemoryFindSubFolders(pFolderName);
						*TmpcMemoryFile = 0;
						Item = 0;

						while ((Items > 0) && (Item == 0))
						{
							(*TmpcMemoryFile)++;
							cMemoryGetSubFolderName(*TmpcMemoryFile, SUBFOLDERNAME_SIZE, pFolderName, PrgNamePath);
							GH.printf($"{CommonHelper.GetString(pFileName)} {CommonHelper.GetString(PrgNamePath)}\r\n");
							if (CommonHelper.strcmp(pFileName, PrgNamePath) == 0)
							{
								Item = *TmpcMemoryFile;
								GH.printf($"Found {Item}\r\n");
							}
							Items--;
						}
						*(DATA8*)GH.Lms.PrimParPointer() = Item;

					}
					break;

				case GET_CACHE_FILES:
					{
						Items = 0;
						for (*TmpcMemoryFile = 0; *TmpcMemoryFile < CACHE_DEEPT; (*TmpcMemoryFile)++)
						{
							if (GH.MemoryInstance.Cache[*TmpcMemoryFile][0] != 0)
							{
								Items++;
							}
						}
						*(DATA8*)GH.Lms.PrimParPointer() = Items;
						GH.printf($"GET_CACHE_FILES {Items}\r\n");
						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case PUT_CACHE_FILE:
					{
						pFileName = (DATA8*)GH.Lms.PrimParPointer();

						GH.printf($"PUT_CACHE_FILE {CommonHelper.GetString(pFileName)}\r\n");
						DspStat = DSPSTAT.NOBREAK;

						if (cMemoryCheckFilename(pFileName, PathBuf, NameBuf, ExtBuf) == OK)
						{ // Filename OK

							Item = 0;
							*TmpcMemoryFile = 0;
							while ((Item < CACHE_DEEPT) && (*TmpcMemoryFile == 0))
							{
								if (CommonHelper.strcmp(GH.MemoryInstance.Cache[Item], pFileName) == 0)
								{
									*TmpcMemoryFile = 1;
								}
								else
								{
									Item++;
								}
							}
							while (Item < (CACHE_DEEPT - 1))
							{
								CommonHelper.strcpy(GH.MemoryInstance.Cache[Item], GH.MemoryInstance.Cache[Item + 1]);
								Item++;
							}
							GH.MemoryInstance.Cache[Item][0] = 0;

							for (Item = (CACHE_DEEPT - 1); Item > 0; Item--)
							{
								CommonHelper.strcpy(GH.MemoryInstance.Cache[Item], GH.MemoryInstance.Cache[Item - 1]);
							}
							CommonHelper.strcpy(GH.MemoryInstance.Cache[Item], pFileName);
						}
					}
					break;

				case DEL_CACHE_FILE:
					{
						pFileName = (DATA8*)GH.Lms.PrimParPointer();

						cMemoryDeleteCacheFile(pFileName);

						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case GET_CACHE_FILE:
					{
						Item = *(DATA8*)GH.Lms.PrimParPointer();
						Lng = *(DATA8*)GH.Lms.PrimParPointer();
						pDestination = (DATA8*)GH.Lms.PrimParPointer();

						DspStat = DSPSTAT.FAILBREAK;
						if ((Item > 0) && (Item <= CACHE_DEEPT))
						{
							if ((long)GH.MemoryInstance.Cache[Item - 1] != 0)
							{
								CommonHelper.snprintf(pDestination, Lng, CommonHelper.GetString(GH.MemoryInstance.Cache[Item - 1]));
								DspStat = DSPSTAT.NOBREAK;
							}
						}
					}
					break;

				default:
					{
						DspStat = DSPSTAT.FAILBREAK;
					}
					break;

			}

			if (Error != 0)
			{
				GH.Ev3System.Logger.LogError($"An error occured with number {Error} in {Environment.StackTrace}");
			}

			if (DspStat == DSPSTAT.BUSYBREAK)
			{ // Rewind IP

				GH.Lms.SetObjectIp(TmpIp - 1);
			}
			GH.Lms.SetDispatchStatus(DspStat);
		}


		/*! \page cMemory
		 *  <hr size="1"/>
		 *  <b>     opARRAY (CMD, ....)  </b>
		 *
		 *- Array entry\n
		 *- Dispatch status can change to DSPSTAT.BUSYBREAK or DSPSTAT.FAILBREAK
		 *
		 *  \param  (DATA8)   CMD     - \ref memoryarraysubcode
		 *
		 *\n
		 *  - CMD = CREATE8
		 *    -   \param  (DATA32)  ELEMENTS  - Number of elements\n
		 *    -   \return (HANDLER) HANDLE    - Array handle\n
		 *
		 *\n
		 *  - CMD = CREATE16
		 *    -   \param  (DATA32)  ELEMENTS  - Number of elements\n
		 *    -   \return (HANDLER) HANDLE    - Array handle\n
		 *
		 *\n
		 *  - CMD = CREATE32
		 *    -   \param  (DATA32)  ELEMENTS  - Number of elements\n
		 *    -   \return (HANDLER) HANDLE    - Array handle\n
		 *
		 *\n
		 *  - CMD = CREATEF
		 *    -   \param  (DATA32)  ELEMENTS  - Number of elements\n
		 *    -   \return (HANDLER) HANDLE    - Array handle\n
		 *
		 *\n
		 *  - CMD = SIZE
		 *    -   \param  (HANDLER) HANDLE    - Array handle\n
		 *    -   \return (DATA32)  ELEMENTS  - Total number of elements in array\n
		 *
		 *\n
		 *  - CMD = RESIZE
		 *    -   \param  (HANDLER) HANDLE    - Array handle\n
		 *    -   \param  (DATA32)  ELEMENTS  - Total number of elements\n
		 *
		 *\n
		 *  - CMD = DELETE
		 *    -   \param  (HANDLER) HANDLE    - Array handle\n
		 *
		 *\n
		 *  - CMD = FILL
		 *    -   \param  (HANDLER) HANDLE    - Array handle\n
		 *    -   \param  (type)    VALUE     - Value to write - type depends on type of array\n
		 *
		 *\n
		 *  - CMD = COPY
		 *    -   \param  (HANDLER) HSOURCE   - Source array handle\n
		 *    -   \param  (HANDLER) HDEST     - Destination array handle\n
		 *
		 *\n
		 *  - CMD = INIT8
		 *    -   \param  (HANDLER) HANDLE    - Array handle\n
		 *    -   \param  (DATA32)  INDEX     - Index to element to write
		 *    -   \param  (DATA32)  ELEMENTS  - Number of elements to write\n
		 *
		 *    Below a number of VALUES equal to "ELEMENTS"
		 *    -   \param  (DATA8)   VALUE     - First value to write - type must be equal to the array type\n
		 *
		 *\n
		 *  - CMD = INIT16
		 *    -   \param  (HANDLER) HANDLE    - Array handle\n
		 *    -   \param  (DATA32)  INDEX     - Index to element to write
		 *    -   \param  (DATA32)  ELEMENTS  - Number of elements to write\n
		 *
		 *    Below a number of VALUES equal to "ELEMENTS"
		 *    -   \param  (DATA16)  VALUE     - First value to write - type must be equal to the array type\n
		 *
		 *\n
		 *  - CMD = INIT32
		 *    -   \param  (HANDLER) HANDLE    - Array handle\n
		 *    -   \param  (DATA32)  INDEX     - Index to element to write
		 *    -   \param  (DATA32)  ELEMENTS  - Number of elements to write\n
		 *
		 *    Below a number of VALUES equal to "ELEMENTS"
		 *    -   \param  (DATA32)  VALUE     - First value to write - type must be equal to the array type\n
		 *
		 *\n
		 *  - CMD = INITF
		 *    -   \param  (HANDLER) HANDLE    - Array handle\n
		 *    -   \param  (DATA32)  INDEX     - Index to element to write
		 *    -   \param  (DATA32)  ELEMENTS  - Number of elements to write\n
		 *
		 *    Below a number of VALUES equal to "ELEMENTS"
		 *    -   \param  (DATAF)   VALUE     - First value to write - type must be equal to the array type\n
		 *
		 *\n
		 *  - CMD = READ_CONTENT
		 *    -   \param  (DATA16)  PRGID     - Program slot number (must be running) (see \ref prgid)\n
		 *    -   \param  (HANDLER) HANDLE    - Array handle\n
		 *    -   \param  (DATA32)  INDEX     - Index to first byte to read\n
		 *    -   \param  (DATA32)  BYTES     - Number of bytes to read\n
		 *    -   \return (DATA8)   ARRAY     - First byte of array to receive data\n
		 *
		 *\n
		 *  - CMD = WRITE_CONTENT
		 *    -   \param  (DATA16)  PRGID     - Program slot number (must be running) (see \ref prgid)\n
		 *    -   \param  (HANDLER) HANDLE    - Array handle\n
		 *    -   \param  (DATA32)  INDEX     - Index to first byte to write\n
		 *    -   \param  (DATA32)  BYTES     - Number of bytes to write\n
		 *    -   \param  (DATA8)   ARRAY     - First byte of array to deliver data\n
		 *
		 *\n
		 *  - CMD = READ_SIZE
		 *    -   \param  (DATA16)  PRGID     - Program slot number (must be running) (see \ref prgid)\n
		 *    -   \param  (HANDLER) HANDLE    - Array handle\n
		 *    -   \return (DATA32)  BYTES     - Number of bytes in array\n
		 *
		 *\n
		 *
		 */


		/*! \brief  opARRAY byte code
		 *
		 */
		private void* pTmpcMemoryArray;
		private void* pSourcecMemoryArray;
		private void* pDestcMemoryArray;
		public void cMemoryArray()
		{
			DSPSTAT DspStat = DSPSTAT.BUSYBREAK;
			PRGID TmpPrgId;
			PRGID PrgId;
			IP TmpIp;
			DATA8 Cmd;
			HANDLER TmpHandle;
			HANDLER hSource;
			HANDLER hDest;
			
			DATA32 Elements;
			DATA32 Index;
			DATA32 ISize;
			DATA32 ElementSize;
			DATA8* pData8;
			DATA16* pData16;
			DATA32* pData32;
			DATAF* pDataF;
			DATA8* pDData8;
			DATA8 Data8;
			DATA16 Data16;
			DATA32 Data32;
			DATAF DataF;
			DATA32 Bytes;
			void* pArray;

			TmpPrgId = GH.Lms.CurrentProgramId();
			TmpIp = GH.Lms.GetObjectIp();

			Cmd = *(DATA8*)GH.Lms.PrimParPointer();

			switch (Cmd)
			{ // Function

				case CREATE8:
					{
						Elements = *(DATA32*)GH.Lms.PrimParPointer();

						if (Elements < MIN_ARRAY_ELEMENTS)
						{
							Elements = MIN_ARRAY_ELEMENTS;
						}

						ElementSize = sizeof(DATA8);
						ISize = Elements * ElementSize + sizeof(DESCR);

						fixed (void** pp = &pTmpcMemoryArray)
						if (cMemoryAlloc(TmpPrgId, POOL_TYPE_MEMORY, (GBINDEX)ISize, (void**)pp, &TmpHandle) == OK)
						{
							(*(DESCR*)pTmpcMemoryArray).Type = DATA_8;
							(*(DESCR*)pTmpcMemoryArray).ElementSize = (DATA8)ElementSize;
							(*(DESCR*)pTmpcMemoryArray).Elements = Elements;

							DspStat = DSPSTAT.NOBREAK;
						}
						else
						{
							DspStat = DSPSTAT.FAILBREAK;
						}

						*(HANDLER*)GH.Lms.PrimParPointer() = (HANDLER)TmpHandle;
					}
					break;

				case CREATE16:
					{
						Elements = *(DATA32*)GH.Lms.PrimParPointer();
						ElementSize = sizeof(DATA16);
						ISize = Elements * ElementSize + sizeof(DESCR);

						fixed (void** pp = &pTmpcMemoryArray)
						if (cMemoryAlloc(TmpPrgId, POOL_TYPE_MEMORY, (GBINDEX)ISize, (void**)pp, &TmpHandle) == OK)
						{
							(*(DESCR*)pTmpcMemoryArray).Type = DATA_16;
							(*(DESCR*)pTmpcMemoryArray).ElementSize = (DATA8)ElementSize;
							(*(DESCR*)pTmpcMemoryArray).Elements = Elements;

							DspStat = DSPSTAT.NOBREAK;
						}
						else
						{
							DspStat = DSPSTAT.FAILBREAK;
						}

						*(HANDLER*)GH.Lms.PrimParPointer() = (HANDLER)TmpHandle;
					}
					break;

				case CREATE32:
					{
						Elements = *(DATA32*)GH.Lms.PrimParPointer();
						ElementSize = sizeof(DATA32);
						ISize = Elements * ElementSize + sizeof(DESCR);

						fixed (void** pp = &pTmpcMemoryArray)
						if (cMemoryAlloc(TmpPrgId, POOL_TYPE_MEMORY, (GBINDEX)ISize, (void**)pp, &TmpHandle) == OK)
						{
							(*(DESCR*)pTmpcMemoryArray).Type = DATA_32;
							(*(DESCR*)pTmpcMemoryArray).ElementSize = (DATA8)ElementSize;
							(*(DESCR*)pTmpcMemoryArray).Elements = Elements;

							DspStat = DSPSTAT.NOBREAK;
						}
						else
						{
							DspStat = DSPSTAT.FAILBREAK;
						}

						*(HANDLER*)GH.Lms.PrimParPointer() = (HANDLER)TmpHandle;
					}
					break;

				case CREATEF:
					{
						Elements = *(DATA32*)GH.Lms.PrimParPointer();
						ElementSize = sizeof(DATAF);
						ISize = Elements * ElementSize + sizeof(DESCR);

						fixed (void** pp = &pTmpcMemoryArray)
						if (cMemoryAlloc(TmpPrgId, POOL_TYPE_MEMORY, (GBINDEX)ISize, (void**)pp, &TmpHandle) == OK)
						{
							(*(DESCR*)pTmpcMemoryArray).Type = DATA_F;
							(*(DESCR*)pTmpcMemoryArray).ElementSize = (DATA8)ElementSize;
							(*(DESCR*)pTmpcMemoryArray).Elements = Elements;

							DspStat = DSPSTAT.NOBREAK;
						}
						else
						{
							DspStat = DSPSTAT.FAILBREAK;
						}

						*(HANDLER*)GH.Lms.PrimParPointer() = (HANDLER)TmpHandle;
					}
					break;

				case SIZE:
					{
						TmpHandle = *(HANDLER*)GH.Lms.PrimParPointer();
						Elements = 0;
						TmpPrgId = GH.Lms.CurrentProgramId();

						fixed (void** pp = &pTmpcMemoryArray)
						if (cMemoryGetPointer(TmpPrgId, TmpHandle, pp) == OK)
						{
							Elements = (*(DESCR*)pTmpcMemoryArray).Elements;
						}

						*(DATA32*)GH.Lms.PrimParPointer() = Elements;
						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case RESIZE:
					{
						TmpHandle = *(HANDLER*)GH.Lms.PrimParPointer();
						Elements = *(DATA32*)GH.Lms.PrimParPointer();

						if (cMemoryResize(TmpPrgId, TmpHandle, Elements) != null)
						{
							DspStat = DSPSTAT.NOBREAK;
						}
						else
						{
							DspStat = DSPSTAT.FAILBREAK;
						}
					}
					break;

				case DELETE:
					{
						TmpHandle = *(HANDLER*)GH.Lms.PrimParPointer();
						cMemoryFreeHandle(TmpPrgId, TmpHandle);
						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case FILL:
					{
						TmpPrgId = GH.Lms.CurrentProgramId();
						TmpHandle = *(HANDLER*)GH.Lms.PrimParPointer();

						fixed (void** pp = &pTmpcMemoryArray)
						if (cMemoryGetPointer(TmpPrgId, TmpHandle, pp) == OK)
						{
							pArray = (*(DESCR*)pTmpcMemoryArray).pArray;
							Elements = (*(DESCR*)pTmpcMemoryArray).Elements;
							Index = 0;
							switch ((*(DESCR*)pTmpcMemoryArray).Type)
							{
								case DATA_8:
									{
										Data8 = *(DATA8*)GH.Lms.PrimParPointer();
										pData8 = (DATA8*)pArray;

										while (Index < Elements)
										{
											pData8[Index] = Data8;
											Index++;
										}
										DspStat = DSPSTAT.NOBREAK;
									}
									break;

								case DATA_16:
									{
										Data16 = *(DATA16*)GH.Lms.PrimParPointer();
										pData16 = (DATA16*)pArray;

										while (Index < Elements)
										{
											pData16[Index] = Data16;
											Index++;
										}
										DspStat = DSPSTAT.NOBREAK;
									}
									break;

								case DATA_32:
									{
										Data32 = *(DATA32*)GH.Lms.PrimParPointer();
										pData32 = (DATA32*)pArray;

										while (Index < Elements)
										{
											pData32[Index] = Data32;
											Index++;
										}
										DspStat = DSPSTAT.NOBREAK;
									}
									break;

								case DATA_F:
									{
										DataF = *(DATAF*)GH.Lms.PrimParPointer();
										pDataF = (DATAF*)pArray;

										while (Index < Elements)
										{
											pDataF[Index] = DataF;
											Index++;
										}
										DspStat = DSPSTAT.NOBREAK;
									}
									break;

							}
						}
					}
					break;

				case COPY:
					{
						hSource = *(HANDLER*)GH.Lms.PrimParPointer();
						hDest = *(HANDLER*)GH.Lms.PrimParPointer();

						DspStat = DSPSTAT.FAILBREAK;
						fixed (void** pp = &pSourcecMemoryArray)
						if (cMemoryGetPointer(TmpPrgId, hSource, pp) == OK)
						{
							fixed (void** ppD = &pDestcMemoryArray)
							if (cMemoryGetPointer(TmpPrgId, hDest, ppD) == OK)
							{
								if ((*(DESCR*)pSourcecMemoryArray).Type == (*(DESCR*)pDestcMemoryArray).Type)
								{
									Elements = (*(DESCR*)pSourcecMemoryArray).Elements;
									DspStat = DSPSTAT.NOBREAK;

									if (cMemoryResize(TmpPrgId, hDest, Elements) == null)
									{
										DspStat = DSPSTAT.FAILBREAK;
									}
									if (DspStat == DSPSTAT.NOBREAK)
									{
										fixed (void** ppD2 = &pDestcMemoryArray)
										if (cMemoryGetPointer(TmpPrgId, hDest, ppD2) == OK)
										{
											ISize = Elements * (*(DESCR*)pSourcecMemoryArray).ElementSize;
											CommonHelper.memcpy((byte*)(*(DESCR*)pDestcMemoryArray).pArray, (byte*)(*(DESCR*)pSourcecMemoryArray).pArray, ISize);
										}
										else
										{
											DspStat = DSPSTAT.FAILBREAK;
										}
									}
								}
							}
						}
					}
					break;

				case INIT8:
					{
						TmpHandle = *(HANDLER*)GH.Lms.PrimParPointer();
						Index = *(DATA32*)GH.Lms.PrimParPointer();
						Elements = *(DATA32*)GH.Lms.PrimParPointer();

						DspStat = DSPSTAT.FAILBREAK;

						fixed (void** pp = &pTmpcMemoryArray)
						if (cMemoryGetPointer(TmpPrgId, TmpHandle, pp) == OK)
						{
							if ((Index >= 0) && ((Index + Elements) <= (*(DESCR*)pTmpcMemoryArray).Elements))
							{
								pArray = (*(DESCR*)pTmpcMemoryArray).pArray;
								pData8 = (DATA8*)pArray;

								while (Elements != 0)
								{
									pData8[Index] = *(DATA8*)GH.Lms.PrimParPointer();
									Elements--;
									Index++;
								}
								DspStat = DSPSTAT.NOBREAK;
							}
						}
					}
					break;

				case INIT16:
					{
						TmpHandle = *(HANDLER*)GH.Lms.PrimParPointer();
						Index = *(DATA32*)GH.Lms.PrimParPointer();
						Elements = *(DATA32*)GH.Lms.PrimParPointer();

						DspStat = DSPSTAT.FAILBREAK;

						fixed (void** pp = &pTmpcMemoryArray)
						if (cMemoryGetPointer(TmpPrgId, TmpHandle, pp) == OK)
						{
							if ((Index >= 0) && ((Index + Elements) <= (*(DESCR*)pTmpcMemoryArray).Elements))
							{
								pArray = (*(DESCR*)pTmpcMemoryArray).pArray;
								pData16 = (DATA16*)pArray;

								while (Elements != 0)
								{
									pData16[Index] = *(DATA16*)GH.Lms.PrimParPointer();
									Elements--;
									Index++;
								}
								DspStat = DSPSTAT.NOBREAK;
							}
						}
					}
					break;

				case INIT32:
					{
						TmpHandle = *(HANDLER*)GH.Lms.PrimParPointer();
						Index = *(DATA32*)GH.Lms.PrimParPointer();
						Elements = *(DATA32*)GH.Lms.PrimParPointer();

						DspStat = DSPSTAT.FAILBREAK;

						fixed (void** pp = &pTmpcMemoryArray)
						if (cMemoryGetPointer(TmpPrgId, TmpHandle, pp) == OK)
						{
							if ((Index >= 0) && ((Index + Elements) <= (*(DESCR*)pTmpcMemoryArray).Elements))
							{
								pArray = (*(DESCR*)pTmpcMemoryArray).pArray;
								pData32 = (DATA32*)pArray;

								while (Elements != 0)
								{
									pData32[Index] = *(DATA32*)GH.Lms.PrimParPointer();
									Elements--;
									Index++;
								}
								DspStat = DSPSTAT.NOBREAK;
							}
						}
					}
					break;

				case INITF:
					{
						TmpHandle = *(HANDLER*)GH.Lms.PrimParPointer();
						Index = *(DATA32*)GH.Lms.PrimParPointer();
						Elements = *(DATA32*)GH.Lms.PrimParPointer();

						DspStat = DSPSTAT.FAILBREAK;

						fixed (void** pp = &pTmpcMemoryArray)
						if (cMemoryGetPointer(TmpPrgId, TmpHandle, pp) == OK)
						{
							if ((Index >= 0) && ((Index + Elements) <= (*(DESCR*)pTmpcMemoryArray).Elements))
							{
								pArray = (*(DESCR*)pTmpcMemoryArray).pArray;
								pDataF = (DATAF*)pArray;

								while (Elements != 0)
								{
									pDataF[Index] = *(DATAF*)GH.Lms.PrimParPointer();
									Elements--;
									Index++;
								}
								DspStat = DSPSTAT.NOBREAK;
							}
						}
					}
					break;

				case READ_CONTENT:
					{
						PrgId = *(UWORD*)GH.Lms.PrimParPointer();
						TmpHandle = *(DATA16*)GH.Lms.PrimParPointer();
						Index = *(DATA32*)GH.Lms.PrimParPointer();
						Bytes = *(DATA32*)GH.Lms.PrimParPointer();
						pDData8 = (DATA8*)GH.Lms.PrimParPointer();

						DspStat = DSPSTAT.FAILBREAK;

						if (PrgId == (PRGID)CURRENT_SLOT)
						{
							PrgId = TmpPrgId;
						}

						fixed (void** pp = &pTmpcMemoryArray)
						if (cMemoryGetPointer(PrgId, TmpHandle, pp) == OK)
						{
							if (GH.VMInstance.Handle >= 0)
							{
								pDData8 = (DATA8*)GH.Lms.VmMemoryResize(GH.VMInstance.Handle, Bytes);
							}

							ISize = (*(DESCR*)pTmpcMemoryArray).Elements * (DATA32)((*(DESCR*)pTmpcMemoryArray).ElementSize);

							if ((Index >= 0) && (pDData8 != null))
							{
								pArray = (*(DESCR*)pTmpcMemoryArray).pArray;

								pData8 = (DATA8*)pArray;
								Data32 = 0;

								while ((Data32 < Bytes) && (Index < ISize))
								{
									pDData8[Data32] = pData8[Index];
									Data32++;
									Index++;
								}
								while (Data32 < Bytes)
								{
									pDData8[Data32] = 0;
									Data32++;
								}
								DspStat = DSPSTAT.NOBREAK;
							}
						}
					}
					break;

				case WRITE_CONTENT:
					{
						PrgId = *(UWORD*)GH.Lms.PrimParPointer();
						TmpHandle = *(DATA16*)GH.Lms.PrimParPointer();
						Index = *(DATA32*)GH.Lms.PrimParPointer();
						Bytes = *(DATA32*)GH.Lms.PrimParPointer();
						pDData8 = (DATA8*)GH.Lms.PrimParPointer();

						DspStat = DSPSTAT.FAILBREAK;

						GH.printf($"ARRAY WRITE_CONTENT CP={TmpPrgId} PP={PrgId}\r\n");

						if (PrgId == ushort.MaxValue)
						{
							PrgId = TmpPrgId;
						}

						fixed (void** pp = &pTmpcMemoryArray)
						if (cMemoryGetPointer(PrgId, TmpHandle, pp) == OK)
						{
							ElementSize = (DATA32)(*(DESCR*)pTmpcMemoryArray).ElementSize;
							if (ElementSize != 0)
							{
								Elements = (Index + Bytes + (ElementSize - 1)) / ElementSize;
								ISize = Elements * ElementSize;

								pTmpcMemoryArray = cMemoryResize(PrgId, TmpHandle, Elements);
								if (pTmpcMemoryArray != null)
								{
									if ((Index >= 0) && (pDData8 != null))
									{
										pData8 = (DATA8*)pTmpcMemoryArray;
										Data32 = 0;

										while ((Data32 < Bytes) && (Index < ISize))
										{
											pData8[Index] = pDData8[Data32];
											Data32++;
											Index++;
										}
										DspStat = DSPSTAT.NOBREAK;
									}
								}
							}
						}
					}
					break;

				case READ_SIZE:
					{
						PrgId = *(UWORD*)GH.Lms.PrimParPointer();
						TmpHandle = *(DATA16*)GH.Lms.PrimParPointer();

						Bytes = 0;
						DspStat = DSPSTAT.FAILBREAK;

						if (PrgId == ushort.MaxValue)
						{
							PrgId = TmpPrgId;
						}

						fixed (void** pp = &pTmpcMemoryArray)
						if (cMemoryGetPointer(PrgId, TmpHandle, pp) == OK)
						{
							Bytes = (*(DESCR*)pTmpcMemoryArray).Elements * (DATA32)((*(DESCR*)pTmpcMemoryArray).ElementSize);
						}

						*(DATA32*)GH.Lms.PrimParPointer() = Bytes;
						DspStat = DSPSTAT.NOBREAK;
					}
					break;

			}

			if (DspStat == DSPSTAT.BUSYBREAK)
			{ // Rewind IP

				GH.Lms.SetObjectIp(TmpIp - 1);
			}
			GH.Lms.SetDispatchStatus(DspStat);
		}


		/*! \page cMemory
		 *
		 *  <hr size="1"/>
		 *  <b>     opARRAY_WRITE (HANDLE, INDEX, VALUE)  </b>
		 *
		 *- Array element write\n
		 *- Dispatch status can change to DSPSTAT.FAILBREAK
		 *
		 *  \param  (HANDLER) HANDLE    - Array handle
		 *  \param  (DATA32)  INDEX     - Index to element to write
		 *  \param  (type)    VALUE     - Value to write - type depends on type of array\n
		 *
		 *\n
		 *
		 */
		/*! \brief  opARRAY_WRITE byte code
		 *
		 */
		private void* pTmpcMemoryArrayWrite;
		private void* pValuecMemoryArrayWrite;
		public void cMemoryArrayWrite()
		{
			DSPSTAT DspStat = DSPSTAT.FAILBREAK;
			PRGID TmpPrgId;
			HANDLER TmpHandle;
			
			DESCR* pDescr;
			DATA32 Elements;
			DATA32 Index;
			void* pArray;
			DATA8* pData8;
			DATA16* pData16;
			DATA32* pData32;
			DATAF* pDataF;

			TmpPrgId = GH.Lms.CurrentProgramId();
			TmpHandle = *(HANDLER*)GH.Lms.PrimParPointer();
			Index = *(DATA32*)GH.Lms.PrimParPointer();
			pValuecMemoryArrayWrite = GH.Lms.PrimParPointer();

			fixed (void** pp = &pTmpcMemoryArrayWrite)
			if (cMemoryGetPointer(TmpPrgId, TmpHandle, pp) == OK)
			{
				pDescr = (DESCR*)pTmpcMemoryArrayWrite;
				if (Index >= 0)
				{
					Elements = Index + 1;

					DspStat = DSPSTAT.NOBREAK;
					if (Elements > (*pDescr).Elements)
					{
						if (cMemoryResize(TmpPrgId, TmpHandle, Elements) == null)
						{
							DspStat = DSPSTAT.FAILBREAK;
						}
					}
					if (DspStat == DSPSTAT.NOBREAK)
					{
						fixed (void** pp2 = &pTmpcMemoryArrayWrite)
						if (cMemoryGetPointer(TmpPrgId, TmpHandle, pp2) == OK)
						{
							pDescr = (DESCR*)pTmpcMemoryArrayWrite;
							pArray = (*pDescr).pArray;
							GH.printf($"  Write  P={(uint)TmpPrgId} H={(uint)TmpHandle}     I={(ulong)Index} A={(long)pArray}\r\n");
							switch ((*pDescr).Type)
							{
								case DATA_8:
									{
										pData8 = (DATA8*)pArray;
										pData8[Index] = *(DATA8*)pValuecMemoryArrayWrite;
										DspStat = DSPSTAT.NOBREAK;
									}
									break;

								case DATA_16:
									{
										pData16 = (DATA16*)pArray;
										pData16[Index] = *(DATA16*)pValuecMemoryArrayWrite;
										DspStat = DSPSTAT.NOBREAK;
									}
									break;

								case DATA_32:
									{
										pData32 = (DATA32*)pArray;
										pData32[Index] = *(DATA32*)pValuecMemoryArrayWrite;
										DspStat = DSPSTAT.NOBREAK;
									}
									break;

								case DATA_F:
									{
										pDataF = (DATAF*)pArray;
										pDataF[Index] = *(DATAF*)pValuecMemoryArrayWrite;
										DspStat = DSPSTAT.NOBREAK;
									}
									break;

							}
						}
					}
				}
			}
			if (DspStat != DSPSTAT.NOBREAK)
			{
				GH.printf($"  WR ERR P={(uint)TmpPrgId} H={(uint)TmpHandle}     I={(ulong)Index}\r\n");
				GH.Lms.SetDispatchStatus(DspStat);
			}
		}


		/*! \page cMemory
		 *
		 *  <hr size="1"/>
		 *  <b>     opARRAY_READ (HANDLE, INDEX, VALUE)  </b>
		 *
		 *- Array element write\n
		 *- Dispatch status can change to DSPSTAT.FAILBREAK
		 *
		 *  \param  (HANDLER) HANDLE    - Array handle
		 *  \param  (DATA32)  INDEX     - Index of element to read
		 *  \return (type)    VALUE     - Value to read - type depends on type of array
		 *
		 *\n
		 *
		 */
		/*! \brief  opARRAY_READ byte code
		 *
		 */
		private void* pTmpcMemoryArrayRead;
		public void cMemoryArrayRead()
		{
			DSPSTAT DspStat = DSPSTAT.FAILBREAK;
			PRGID TmpPrgId;
			HANDLER TmpHandle;
			
			DATA32 Index;
			void* pArray;
			DATA8* pData8;
			DATA16* pData16;
			DATA32* pData32;
			DATAF* pDataF;

			TmpPrgId = GH.Lms.CurrentProgramId();
			TmpHandle = *(HANDLER*)GH.Lms.PrimParPointer();
			Index = *(DATA32*)GH.Lms.PrimParPointer();

			fixed (void** pp = &pTmpcMemoryArrayRead)
			if (cMemoryGetPointer(TmpPrgId, TmpHandle, pp) == OK)
			{
				if ((Index >= 0) && (Index < (*(DESCR*)pTmpcMemoryArrayRead).Elements))
				{
					pArray = (*(DESCR*)pTmpcMemoryArrayRead).pArray;
					GH.printf($"  Read   P={(uint)TmpPrgId} H={(uint)TmpHandle}     I={(ulong)Index} A={(long)pArray}\r\n");
					switch ((*(DESCR*)pTmpcMemoryArrayRead).Type)
					{
						case DATA_8:
							{
								pData8 = (DATA8*)pArray;
								*(DATA8*)GH.Lms.PrimParPointer() = pData8[Index];
								DspStat = DSPSTAT.NOBREAK;
							}
							break;

						case DATA_16:
							{
								pData16 = (DATA16*)pArray;
								*(DATA16*)GH.Lms.PrimParPointer() = pData16[Index];
								DspStat = DSPSTAT.NOBREAK;
							}
							break;

						case DATA_32:
							{
								pData32 = (DATA32*)pArray;
								*(DATA32*)GH.Lms.PrimParPointer() = pData32[Index];
								DspStat = DSPSTAT.NOBREAK;
							}
							break;

						case DATA_F:
							{
								pDataF = (DATAF*)pArray;
								*(DATAF*)GH.Lms.PrimParPointer() = pDataF[Index];
								DspStat = DSPSTAT.NOBREAK;
							}
							break;

					}
				}
			}
			if (DspStat != DSPSTAT.NOBREAK)
			{
				GH.Lms.PrimParAdvance();
				GH.Lms.SetDispatchStatus(DspStat);
			}
		}


		/*! \page cMemory
		 *
		 *  <hr size="1"/>
		 *  <b>     opARRAY_APPEND (HANDLE, VALUE)  </b>
		 *
		 *- Array element append\n
		 *- Dispatch status can change to DSPSTAT.FAILBREAK
		 *
		 *  \param  (HANDLER) HANDLE    - Array handle
		 *  \param  (type)    VALUE     - Value (new element) to append - type depends on type of array\n
		 *
		 *\n
		 *
		 */
		/*! \brief  opARRAY_APPEND byte code
		 *
		 */
		private DATA32* IndexcMemoryArrayAppend = (DATA32*)CommonHelper.AllocateByteArray(4);
		private void* pTmpcMemoryArrayAppend;
		private void* pValuecMemoryArrayAppend;
		public void cMemoryArrayAppend()
		{
			DSPSTAT DspStat = DSPSTAT.FAILBREAK;
			PRGID TmpPrgId;
			HANDLER TmpHandle;
			
			DESCR* pDescr;
			DATA32 Elements;
			void* pArray;
			DATA8* pData8;
			DATA16* pData16;
			DATA32* pData32;
			DATAF* pDataF;

			TmpPrgId = GH.Lms.CurrentProgramId();
			TmpHandle = *(HANDLER*)GH.Lms.PrimParPointer();
			pValuecMemoryArrayAppend = GH.Lms.PrimParPointer();

			fixed (void** pp = &pTmpcMemoryArrayAppend)
			if (cMemoryGetPointer(TmpPrgId, TmpHandle, pp) == OK)
			{
				pDescr = (DESCR*)pTmpcMemoryArrayAppend;
				*IndexcMemoryArrayAppend = (*pDescr).Elements;
				Elements = *IndexcMemoryArrayAppend + 1;

				DspStat = DSPSTAT.NOBREAK;
				if (Elements > (*pDescr).Elements)
				{
					if (cMemoryResize(TmpPrgId, TmpHandle, Elements) == null)
					{
						DspStat = DSPSTAT.FAILBREAK;
					}
				}
				if (DspStat == DSPSTAT.NOBREAK)
				{
					fixed (void** pp2 = &pTmpcMemoryArrayAppend)
					if (cMemoryGetPointer(TmpPrgId, TmpHandle, pp2) == OK)
					{
						pDescr = (DESCR*)pTmpcMemoryArrayAppend;
						pArray = (*pDescr).pArray;
						GH.printf($"  Append P={(uint)TmpPrgId} H={(uint)TmpHandle}     I={(ulong)*IndexcMemoryArrayAppend} A={(long)pArray}");
						switch ((*pDescr).Type)
						{
							case DATA_8:
								{
									GH.printf($" V={(int)*(DATA8*)pValuecMemoryArrayAppend}");
									pData8 = (DATA8*)pArray;
									pData8[*IndexcMemoryArrayAppend] = *(DATA8*)pValuecMemoryArrayAppend;
									DspStat = DSPSTAT.NOBREAK;
								}
								break;

							case DATA_16:
								{
									GH.printf($" V={(int)*(DATA16*)pValuecMemoryArrayAppend}");
									pData16 = (DATA16*)pArray;
									pData16[*IndexcMemoryArrayAppend] = *(DATA16*)pValuecMemoryArrayAppend;
									DspStat = DSPSTAT.NOBREAK;
								}
								break;

							case DATA_32:
								{
									GH.printf($" V={(int)*(DATA32*)pValuecMemoryArrayAppend}");
									pData32 = (DATA32*)pArray;
									pData32[*IndexcMemoryArrayAppend] = *(DATA32*)pValuecMemoryArrayAppend;
									DspStat = DSPSTAT.NOBREAK;
								}
								break;

							case DATA_F:
								{
									GH.printf($" V={*(DATAF*)pValuecMemoryArrayAppend}");
									pDataF = (DATAF*)pArray;
									pDataF[*IndexcMemoryArrayAppend] = *(DATAF*)pValuecMemoryArrayAppend;
									DspStat = DSPSTAT.NOBREAK;
								}
								break;

						}
						GH.printf("\r\n");
					}
				}
			}
			if (DspStat != DSPSTAT.NOBREAK)
			{
				GH.printf($"  WR ERR P={(uint)TmpPrgId} H={(uint)TmpHandle}     I={(ulong)*IndexcMemoryArrayAppend}\r\n");
				GH.Lms.SetDispatchStatus(DspStat);
			}
		}


		/*! \page cMemory
		 *
		 *  <hr size="1"/>
		 *  <b>     opMEMORY_USAGE (TOTAL, FREE)  </b>
		 *
		 *- Get memory usage\n
		 *- Dispatch status unchanged
		 *
		 *  \return (DATA32)  TOTAL     - Total memory [KB]
		 *  \return (DATA32)  FREE      - Free  memory [KB]
		 *
		 *\n
		 *
		 */
		/*! \brief  opMEMORY_USAGE byte code
		 *
		 */
		private DATA32* TotalcMemoryUsage = (DATA32*)CommonHelper.AllocateByteArray(4);
		private DATA32* FreecMemoryUsage = (DATA32*)CommonHelper.AllocateByteArray(4);
		public void cMemoryUsage()
		{
			cMemoryGetUsage(TotalcMemoryUsage, FreecMemoryUsage, 0);

			if (*TotalcMemoryUsage < 0)
			{
				*TotalcMemoryUsage = 0;
			}
			if (*FreecMemoryUsage < 0)
			{
				*FreecMemoryUsage = 0;
			}

			*(DATA32*)GH.Lms.PrimParPointer() = *TotalcMemoryUsage;
			*(DATA32*)GH.Lms.PrimParPointer() = *FreecMemoryUsage;

		}


		/*! \page cMemory
		 *  <hr size="1"/>
		 *  <b>     opFILENAME (CMD, ....)  </b>
		 *
		 *- Memory filename entry\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   CMD               - \ref memoryfilenamesubcode
		 *
		 *\n
		 *  - CMD = EXIST
		 *\n  Test if file exists (if name starts with '~','/' or '.' it is not from current folder) \n
		 *    -  \param  (DATA8)    NAME        - First character in file name (character string)\n
		 *    -  \return (DATA8)    FLAG        - Exist (0 = no, 1 = yes)\n
		 *
		 *\n
		 *  - CMD = TOTALSIZE
		 *\n  Calculate folder/file size (if name starts with '~','/' or '.' it is not from current folder) \n
		 *    -  \param  (DATA8)    NAME        - First character in file name (character string)\n
		 *    -  \return (DATA32)   FILES       - Total number of files\n
		 *    -  \return (DATA32)   SIZE        - Total folder size [KB]\n
		 *
		 *\n
		 *  - CMD = SPLIT
		 *\n  Split filename into Folder, name, extension \n
		 *    -  \param  (DATA8)    FILENAME    - First character in file name (character string) "../folder/subfolder/name.ext"\n
		 *    -  \param  (DATA8)    LENGTH      - Maximal length for each of the below parameters\n
		 *    -  \return (DATA8)    FOLDER      - First character in folder name (character string) "../folder/subfolder"\n
		 *    -  \return (DATA8)    NAME        - First character in name (character string) "name"\n
		 *    -  \return (DATA8)    EXT         - First character in extension (character string) ".ext"\n
		 *
		 *\n
		 *  - CMD = MERGE
		 *\n  Merge Folder, name, extension into filename\n
		 *    -  \param  (DATA8)    FOLDER      - First character in folder name (character string) "../folder/subfolder"\n
		 *    -  \param  (DATA8)    NAME        - First character in name (character string) "name"\n
		 *    -  \param  (DATA8)    EXT         - First character in extension (character string) ".ext"\n
		 *    -  \param  (DATA8)    LENGTH      - Maximal length for the below parameter\n
		 *    -  \return (DATA8)    FILENAME    - First character in file name (character string) "../folder/subfolder/name.ext"\n
		 *
		 *\n
		 *  - CMD = CHECK
		 *\n  Check filename\n
		 *    -  \param  (DATA8)    FILENAME    - First character in file name (character string) "../folder/subfolder/name.ext"\n
		 *    -  \return (DATA8)    OK          - Filename ok (0 = RESULT.FAIL, 1 = OK)\n
		 *
		 *\n
		 *  - CMD = PACK
		 *\n  Pack file or folder into "raf" container\n
		 *    -  \param  (DATA8)    FILENAME    - First character in file name (character string) "../folder/subfolder/name.ext"\n
		 *
		 *\n
		 *  - CMD = UNPACK
		 *\n  Unpack "raf" container\n
		 *    -  \param  (DATA8)    FILENAME    - First character in file name (character string) "../folder/subfolder/name"\n
		 *
		 *\n
		 *  - CMD = GET_FOLDERNAME
		 *\n  Get current folder name\n
		 *    -  \param  (DATA8)    LENGTH      - Maximal length for the below parameter\n
		 *    -  \return (DATA8)    FOLDERNAME  - First character in folder name (character string) "../folder/subfolder"\n
		 *
		 *\n
		 *
		 */
		/*! \brief  opFILENAME byte code
		 *
		 */
		private DATA32* FilescMemoryFileName = (DATA32*)CommonHelper.AllocateByteArray(4);
		public void cMemoryFileName()
		{
			PRGID TmpPrgId;
			DATA8 Cmd;
			DATA8 Tmp;
			// uncomment anime
			// struct stat FileStatus;
			DATA8* Filename = CommonHelper.Pointer1d<DATA8>(MAX_FILENAME_SIZE);
			DATA8* Folder = CommonHelper.Pointer1d<DATA8>(MAX_FILENAME_SIZE);
			DATA8* Name = CommonHelper.Pointer1d<DATA8>(MAX_FILENAME_SIZE);
			DATA8* Ext = CommonHelper.Pointer1d<DATA8>(MAX_FILENAME_SIZE);
			DATA8* Buffer = CommonHelper.Pointer1d<DATA8>(2 * MAX_FILENAME_SIZE + 32);
			DATA8 Length;
			DATA8* pFilename;
			DATA8* pFolder;
			DATA8* pName;
			DATA8* pExt;
			HANDLER hFilename;
			HANDLER hFolder;
			HANDLER hName;
			HANDLER hExt;
			DATA32 Lng;
			DATA32 Size;
			

			TmpPrgId = GH.Lms.CurrentProgramId();
			Cmd = *(DATA8*)GH.Lms.PrimParPointer();

			switch (Cmd)
			{ // Function

				case EXIST:
					{
						pFilename = (DATA8*)GH.Lms.PrimParPointer();
						cMemoryFilename(TmpPrgId, pFilename, "".AsSbytePointer(), MAX_FILENAME_SIZE, Filename);

						Tmp = 0;
						if (File.Exists(CommonHelper.GetString(Filename)))
						{
							Tmp = 1;
						}
						GH.printf($"c_memory  cMemoryFileName: EXIST   [{CommonHelper.GetString(Filename)}] = {Tmp}\r\n");
						*(DATA8*)GH.Lms.PrimParPointer() = Tmp;
					}
					break;

				case TOTALSIZE:
					{
						pFilename = (DATA8*)GH.Lms.PrimParPointer();
						cMemoryFilename(TmpPrgId, pFilename, "".AsSbytePointer(), MAX_FILENAME_SIZE, Filename);
						Size = cMemoryFindSize(Filename, FilescMemoryFileName);

						*(DATA32*)GH.Lms.PrimParPointer() = *FilescMemoryFileName;
						*(DATA32*)GH.Lms.PrimParPointer() = Size;
					}
					break;

				case SPLIT:
					{
						pFilename = (DATA8*)GH.Lms.PrimParPointer();
						Length = *(DATA8*)GH.Lms.PrimParPointer();
						pFolder = (DATA8*)GH.Lms.PrimParPointer();
						hFolder = GH.VMInstance.Handle;
						pName = (DATA8*)GH.Lms.PrimParPointer();
						hName = GH.VMInstance.Handle;
						pExt = (DATA8*)GH.Lms.PrimParPointer();
						hExt = GH.VMInstance.Handle;

						Tmp = Length;

						// Split pFilename
						FindName(pFilename, Folder, Name, Ext);

						// Make pFolder
						Length = Tmp;
						if (hFolder >= 0)
						{
							Lng = CommonHelper.strlen(Folder) + 1;
							if (Lng > MIN_ARRAY_ELEMENTS)
							{
								pFolder = (DATA8*)GH.Lms.VmMemoryResize(hFolder, Lng);
							}
							Length = (DATA8)Lng;
						}
						CommonHelper.snprintf(pFolder, (int)Length, CommonHelper.GetString(Folder));

						// Make pName
						Length = Tmp;
						if (hName >= 0)
						{
							Lng = CommonHelper.strlen(Name) + 1;
							if (Lng > MIN_ARRAY_ELEMENTS)
							{
								pName = (DATA8*)GH.Lms.VmMemoryResize(hName, Lng);
							}
							Length = (DATA8)Lng;
						}
						CommonHelper.snprintf(pName, (int)Length, CommonHelper.GetString(Name));

						// Make pExt
						Length = Tmp;
						if (hExt >= 0)
						{
							Lng = CommonHelper.strlen(Ext) + 1;
							if (Lng > MIN_ARRAY_ELEMENTS)
							{
								pExt = (DATA8*)GH.Lms.VmMemoryResize(hExt, Lng);
							}
							Length = (DATA8)Lng;
						}
						CommonHelper.snprintf(pExt, (int)Length, CommonHelper.GetString(Ext));

					}
					break;

				case MERGE:
					{
						pFolder = (DATA8*)GH.Lms.PrimParPointer();
						pName = (DATA8*)GH.Lms.PrimParPointer();
						pExt = (DATA8*)GH.Lms.PrimParPointer();
						Length = *(DATA8*)GH.Lms.PrimParPointer();
						pFilename = (DATA8*)GH.Lms.PrimParPointer();
						hFilename = GH.VMInstance.Handle;

						// Merge pFolder, pName and pExt
						CommonHelper.snprintf(Filename, MAX_FILENAME_SIZE, $"{CommonHelper.GetString(pFolder)}/{CommonHelper.GetString(pName)}{CommonHelper.GetString(pExt)}");

						// Make pFilename
						if (hFilename >= 0)
						{
							Lng = CommonHelper.strlen(Filename) + 1;
							if (Lng > MIN_ARRAY_ELEMENTS)
							{
								pFilename = (DATA8*)GH.Lms.VmMemoryResize(hFilename, Lng);
							}
							Length = (DATA8)Lng;
						}
						CommonHelper.snprintf(pFilename, (int)Length, CommonHelper.GetString(Filename));
					}
					break;

				case CHECK:
					{
						Tmp = 0;
						pFilename = (DATA8*)GH.Lms.PrimParPointer();
						if (cMemoryCheckFilename(pFilename, null, null, null) == OK)
						{
							Tmp = 1;
						}

						*(DATA8*)GH.Lms.PrimParPointer() = Tmp;
					}
					break;

				case PACK:
					{
						pName = (DATA8*)GH.Lms.PrimParPointer();

						// Split pFilename
						FindName(pName, Folder, Name, Ext);

						//CommonHelper.snprintf(Buffer, 2 * MAX_FILENAME_SIZE + 32, "tar -cz -f %s%s%s -C %s %s%s &> /dev/null", Folder, Name, vmEXT_ARCHIVE, Folder, Name, Ext);
						//system(Buffer);
						//sync();

						GH.Ev3System.Logger.LogWarning($"Call of unimplemented PACK shite in {Environment.StackTrace}");
					}
					break;

				case UNPACK:
					{
						pName = (DATA8*)GH.Lms.PrimParPointer();

						// Split pFilename
						FindName(pName, Folder, Name, Ext);

						//CommonHelper.snprintf(Buffer, 2 * MAX_FILENAME_SIZE + 32, "tar -xz -f %s%s%s -C %s &> /dev/null", Folder, Name, vmEXT_ARCHIVE, Folder);
						//system(Buffer);
						//sync();

						GH.Ev3System.Logger.LogWarning($"Call of unimplemented UNPACK shite in {Environment.StackTrace}");
					}
					break;

				case GET_FOLDERNAME:
					{
						Length = *(DATA8*)GH.Lms.PrimParPointer();
						pFilename = (DATA8*)GH.Lms.PrimParPointer();
						hFilename = GH.VMInstance.Handle;

						cMemoryGetResourcePath(TmpPrgId, Filename, MAX_FILENAME_SIZE);
						Lng = CommonHelper.strlen(Filename);


						if (Lng > 0)
						{
							if (Filename[Lng - 1] == '/')
							{
								Lng--;
								Filename[Lng] = 0;
							}
						}

						// Make pFilename
						if (hFilename >= 0)
						{
							Lng++;
							if (Lng > MIN_ARRAY_ELEMENTS)
							{
								pFilename = (DATA8*)GH.Lms.VmMemoryResize(hFilename, Lng);
							}
							Length = (DATA8)Lng;
						}
						if (pFilename != null)
						{
							CommonHelper.snprintf(pFilename, (int)Length, CommonHelper.GetString(Filename));
						}
					}
					break;

			}
		}
	}
}
