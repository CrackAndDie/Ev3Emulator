using Ev3Core.Enums;
using Ev3Core.Extensions;
using Ev3Core.Helpers;
using Ev3Core.Lms2012.Interfaces;
using System.Linq;
using System.Net.NetworkInformation;
using static Ev3Core.Defines;

namespace Ev3Core.Lms2012
{
	public class Lms : ILms
	{
		#region begin
		public int Main()
		{
			RESULT Result = RESULT.FAIL;
			VarPointer<UBYTE> Restart = new VarPointer<byte>();

			do
			{
				Restart.Data = 0;
				Result = mSchedInit();

				if (Result == RESULT.OK)
				{
					do
					{
						Result = mSchedCtrl(Restart);
						/*
                                if ((*UiInstance.pUi).State[BACK_BUTTON] & BUTTON_LONGPRESS)
                                {
                                  Restart  =  1;
                                  Result   =  FAIL;
                                }
                        */
					}
					while (Result == RESULT.OK);

					Result = mSchedExit();
				}
				else
				{
					//TCP      system("reboot");
				}
			}
			while (Restart.Data != 0);

			return ((int)Result);
		}
		RESULT mSchedInit()
		{
			RESULT Result = OK;
			PRGID PrgId;
			IMGHEAD pImgHead;
			DATA16 Loop;
			VarPointer<DATA8> Ok = new VarPointer<sbyte>();
			VarPointer<DATA32> Total = new VarPointer<int>();
			VarPointer<DATA32> Free = new VarPointer<int>();
			float Tmp;
			// int File;
			ArrayPointer<byte> PrgNameBuf = new ArrayPointer<byte>(new byte[vmFILENAMESIZE]);
			// char ParBuf[255];

			DateTime tv = DateTime.Now;

			GH.VMInstance.Status = 0x00;

			// TODO: idk what is this
			//ANALOG* pAdcTmp;
			//VMInstance.pAnalog = &VMInstance.Analog;
			//VMInstance.AdcFile = open(ANALOG_DEVICE_NAME, O_RDWR | O_SYNC);

			//if (VMInstance.AdcFile >= MIN_HANDLE)
			//{
			//    pAdcTmp = (ANALOG*)mmap(0, sizeof(ANALOG), PROT_READ | PROT_WRITE, MAP_FILE | MAP_SHARED, VMInstance.AdcFile, 0);

			//    if (pAdcTmp == MAP_FAILED)
			//    {
			//        //#ifndef Linux_X86
			//        LogErrorNumber(ANALOG_SHARED_MEMORY);
			//        //#endif
			//    }
			//    else
			//    {
			//        VMInstance.pAnalog = pAdcTmp;
			//    }
			//    close(VMInstance.AdcFile);
			//}
			//else
			//{
			//    //#ifndef Linux_X86
			//    LogErrorNumber(ANALOG_DEVICE_FILE_NOT_FOUND);
			//    //#endif
			//}

			// TODO: should we? Fill holes in PrimDispatchTabel
			//for (Loop = 0; Loop < PRIMDISPATHTABLE_SIZE; Loop++)
			//{
			//    if (PrimDispatchTabel[Loop] == NULL)
			//    {
			//        PrimDispatchTabel[Loop] = &Error;
			//    }
			//}

			// TODO: make sure - Be sure necessary folders exist
			//if (mkdir(vmSETTINGS_DIR, DIRPERMISSIONS) == 0)
			//{
			//    chmod(vmSETTINGS_DIR, DIRPERMISSIONS);
			//}

			CheckUsbstick(Ok, Total, Free, 0);
			CheckSdcard(Ok, Total, Free, 0);

			// Be sure necessary files exist (!!!no need to care about them probably!!!)
			//Ok = 0;
			//snprintf(PrgNameBuf, vmFILENAMESIZE, "%s/%s%s", vmSETTINGS_DIR, vmWIFI_FILE_NAME, vmEXT_TEXT);
			//File = open(PrgNameBuf, O_CREAT | O_WRONLY | O_TRUNC, SYSPERMISSIONS);
			//if (File >= MIN_HANDLE)
			//{
			//    sprintf(ParBuf, "-\t");
			//    write(File, ParBuf, strlen(ParBuf));
			//    close(File);
			//}

			//Ok = 0;
			//snprintf(PrgNameBuf, vmFILENAMESIZE, "%s/%s%s", vmSETTINGS_DIR, vmBLUETOOTH_FILE_NAME, vmEXT_TEXT);
			//File = open(PrgNameBuf, O_RDONLY);
			//if (File >= MIN_HANDLE)
			//{
			//    close(File);
			//}
			//else
			//{
			//    File = open(PrgNameBuf, O_CREAT | O_WRONLY | O_TRUNC, SYSPERMISSIONS);
			//    if (File >= MIN_HANDLE)
			//    {
			//        sprintf(ParBuf, "-\t");
			//        write(File, ParBuf, strlen(ParBuf));
			//        close(File);
			//    }
			//}

			//Ok = 0;
			//snprintf(PrgNameBuf, vmFILENAMESIZE, "%s/%s%s", vmSETTINGS_DIR, vmSLEEP_FILE_NAME, vmEXT_TEXT);
			//File = open(PrgNameBuf, O_RDONLY);
			//if (File >= MIN_HANDLE)
			//{
			//    ParBuf[0] = 0;
			//    read(File, ParBuf, sizeof(ParBuf));
			//    if (sscanf(ParBuf, "%f", &Tmp) > 0)
			//    {
			//        if ((Tmp >= (float)0) && (Tmp <= (float)127))
			//        {
			//            SetSleepMinutes((DATA8)Tmp);
			//            Ok = 1;
			//        }
			//    }
			//    else
			//    {
			//        ParBuf[5] = 0;
			//        if (strcmp(ParBuf, "never") == 0)
			//        {
			//            SetSleepMinutes(0);
			//            Ok = 1;
			//        }
			//    }
			//    close(File);
			//}
			//if (!Ok)
			//{
			//    File = open(PrgNameBuf, O_CREAT | O_WRONLY | O_TRUNC, SYSPERMISSIONS);
			//    if (File >= MIN_HANDLE)
			//    {
			//        SetSleepMinutes((DATA8)DEFAULT_SLEEPMINUTES);
			//        sprintf(ParBuf, "%dmin\t", DEFAULT_SLEEPMINUTES);
			//        write(File, ParBuf, strlen(ParBuf));
			//        close(File);
			//    }
			//}

			// TODO: init volume shite
			//Ok = 0;
			//snprintf(PrgNameBuf, vmFILENAMESIZE, "%s/%s%s", vmSETTINGS_DIR, vmVOLUME_FILE_NAME, vmEXT_TEXT);
			//File = open(PrgNameBuf, O_RDONLY);
			//if (File >= MIN_HANDLE)
			//{
			//    ParBuf[0] = 0;
			//    read(File, ParBuf, sizeof(ParBuf));
			//    if (sscanf(ParBuf, "%f", &Tmp) > 0)
			//    {
			//        if ((Tmp >= (float)0) && (Tmp <= (float)100))
			//        {
			//            SetVolumePercent((DATA8)Tmp);
			//            Ok = 1;
			//        }
			//    }
			//    close(File);
			//}

			GH.VMInstance.RefCount = 0;

			Result |= GH.Output.cOutputInit();
			Result |= GH.Input.cInputInit();
			Result |= GH.Ui.cUiInit();
			Result |= GH.Memory.cMemoryInit();
			Result |= GH.Com.cComInit();
			Result |= GH.Sound.cSoundInit();
			Result |= GH.Validate.cValidateInit();

			for (PrgId = 0; PrgId < MAX_PROGRAMS; PrgId++)
			{
				GH.VMInstance.Program[PrgId].Status = OBJSTAT.STOPPED;
				GH.VMInstance.Program[PrgId].StatusChange = 0;
			}

			SetTerminalEnable(TERMINAL_ENABLED);

			GH.VMInstance.Test = 0;

			VmPrint("\r\n\n\n\n\n\nLMS2012 VM STARTED\r\n{\r\n".ToArrayPointer());
			GH.VMInstance.ProgramId = DEBUG_SLOT;
			pImgHead = IMGHEAD.GetObject(UiImage);
			pImgHead.ImageSize = IMGHEAD.Sizeof;

			GH.Ev3System.Logger.LogInfo("VM Started");

			CommonHelper.Snprintf(GH.VMInstance.FirstProgram, MAX_FILENAME_SIZE, DEFAULT_UI.ToArrayPointer());

			ProgramReset(GH.VMInstance.ProgramId, UiImage, (GP)GH.VMInstance.FirstProgram, 0);

			return (RESULT)(Result);
		}

		RESULT mSchedCtrl(VarPointer<UBYTE> pRestart)
		{
			RESULT Result = RESULT.FAIL;
			ULONG Time;
			IP TmpIp;

			if (GH.VMInstance.DispatchStatus != DSPSTAT.STOPBREAK)
			{
				ProgramInit();
			}

			SetDispatchStatus(ObjectInit());

			Time = GH.Timer.cTimerGetuS();
			Time -= GH.VMInstance.PerformTimer;
			GH.VMInstance.PerformTime *= (DATAF)199;
			GH.VMInstance.PerformTime += (DATAF)Time;
			GH.VMInstance.PerformTime /= (DATAF)200;

			/*** Execute BYTECODES *******************************************************/

			GH.VMInstance.InstrCnt = 0;

			while (GH.VMInstance.Priority != 0)
			{
				if (GH.VMInstance.Debug != 0)
				{
					Monitor();
				}
				else
				{
					GH.VMInstance.Priority--;

					PrimDispatchTabel[GH.VMInstance.ObjectIp++](); // TODO: ++ probably before the shite
					GH.VMInstance.InstrCnt++;

				}
			}

			/*****************************************************************************/

			GH.VMInstance.PerformTimer = GH.Timer.cTimerGetuS();

			GH.VMInstance.NewTime = GetTimeMS();

			Time = GH.VMInstance.NewTime - GH.VMInstance.OldTime1;

			if (Time >= UPDATE_TIME1)
			{
				GH.VMInstance.OldTime1 += Time;

				GH.Com.cComUpdate();
				GH.Sound.cSoundUpdate();
			}


			Time = GH.VMInstance.NewTime - GH.VMInstance.OldTime2;

			if (Time >= UPDATE_TIME2)
			{
				GH.VMInstance.OldTime2 += Time;

				// usleep(10); // TODO: why???
				GH.Input.cInputUpdate((UWORD)Time);
				GH.Ui.cUiUpdate((UWORD)Time);

				if (GH.VMInstance.Test != 0)
				{
					if (GH.VMInstance.Test > (UWORD)Time)
					{
						GH.VMInstance.Test -= (UWORD)Time;
					}
					else
					{
						TstClose();
					}
				}
			}

			if (GH.VMInstance.DispatchStatus == DSPSTAT.FAILBREAK)
			{
				if (GH.VMInstance.ProgramId != GUI_SLOT)
				{
					if (GH.VMInstance.ProgramId != CMD_SLOT)
					{
						GH.UiInstance.Warning |= WARNING_DSPSTAT;
					}
					CommonHelper.Snprintf(GH.VMInstance.PrintBuffer, PRINTBUFFERSIZE, $"}}\r\nPROGRAM \"{GH.VMInstance.ProgramId}\" FAIL BREAK just before {(ulong)(GH.VMInstance.ObjectIp - GH.VMInstance.Program[GH.VMInstance.ProgramId].pImage)}!\r\n".ToArrayPointer());
					VmPrint(GH.VMInstance.PrintBuffer);
					ProgramEnd(GH.VMInstance.ProgramId);
					GH.VMInstance.Program[GH.VMInstance.ProgramId].Result = RESULT.FAIL;
				}
				else
				{
					CommonHelper.Snprintf(GH.VMInstance.PrintBuffer, PRINTBUFFERSIZE, $"UI FAIL BREAK just before {(ulong)(GH.VMInstance.ObjectIp - GH.VMInstance.Program[GH.VMInstance.ProgramId].pImage)}!\r\n".ToArrayPointer());
					VmPrint(GH.VMInstance.PrintBuffer);
					LogErrorNumber(nameof(VM_INTERNAL), Environment.StackTrace);
					pRestart.Data = 1;
				}
			}
			else
			{

				if (GH.VMInstance.DispatchStatus == DSPSTAT.INSTRBREAK)
				{
					if (GH.VMInstance.ProgramId != CMD_SLOT)
					{
						LogErrorNumber(nameof(VM_PROGRAM_INSTRUCTION_BREAK), Environment.StackTrace);
					}
					TmpIp = GH.VMInstance.ObjectIp - 1;
					CommonHelper.Snprintf(GH.VMInstance.PrintBuffer, PRINTBUFFERSIZE, $"\r\n{(UWORD)(((ULONG)TmpIp) - (ULONG)GH.VMInstance.pImage)} [{GH.VMInstance.ObjectId}] ".ToArrayPointer());
					VmPrint(GH.VMInstance.PrintBuffer);
					CommonHelper.Snprintf(GH.VMInstance.PrintBuffer, PRINTBUFFERSIZE, $"VM       ERROR    [0x{TmpIp.Data}]\r\n".ToArrayPointer());
					VmPrint(GH.VMInstance.PrintBuffer);
					GH.VMInstance.Program[GH.VMInstance.ProgramId].Result = RESULT.FAIL;
				}

				ObjectExit();

				Result = ObjectExec();

				if (Result == RESULT.STOP)
				{
					ProgramExit();
					ProgramEnd(GH.VMInstance.ProgramId);
					GH.VMInstance.DispatchStatus = DSPSTAT.NOBREAK;
				}
				else
				{
					if (GH.VMInstance.DispatchStatus != DSPSTAT.STOPBREAK)
					{
						ProgramExit();
					}
				}
			}

			if (GH.VMInstance.DispatchStatus != DSPSTAT.STOPBREAK)
			{
				Result = ProgramExec();
			}

			if (pRestart.Data == 1)
			{
				Result = RESULT.FAIL;
			}

			return (Result);
		}

		RESULT mSchedExit()
		{
			RESULT Result = OK;

			VmPrint("}\r\nVM STOPPED\r\n\n".ToArrayPointer());

			Result |= GH.Validate.cValidateExit();
			Result |= GH.Sound.cSoundExit();
			Result |= GH.Com.cComExit();
			Result |= GH.Memory.cMemoryExit();
			Result |= GH.Ui.cUiExit();
			Result |= GH.Input.cInputExit();
			Result |= GH.Output.cOutputExit();

			return (RESULT)(Result);
		}
		#endregion

		#region local methods
		DSPSTAT ObjectInit()
		{
			DSPSTAT Result = DSPSTAT.STOPBREAK;

			if ((GH.VMInstance.ObjectId > 0) && (GH.VMInstance.ObjectId <= GH.VMInstance.Objects))
			{ // object valid

				if (GH.VMInstance.pObjList[GH.VMInstance.ObjectId].ObjStatus == RUNNING)
				{ // Restore object context

					GH.VMInstance.ObjectIp = GH.VMInstance.pObjList[GH.VMInstance.ObjectId].Ip;

					GH.VMInstance.ObjectLocal = GH.VMInstance.pObjList[GH.VMInstance.ObjectId].pLocal;

					Result = DSPSTAT.NOBREAK;
				}
			}

			if ((GH.VMInstance.ProgramId == GUI_SLOT) || (GH.VMInstance.ProgramId == DEBUG_SLOT))
			{ // UI

				GH.VMInstance.Priority = UI_PRIORITY;
			}
			else
			{ // user program

				GH.VMInstance.Priority = PRG_PRIORITY;
			}

			return (Result);
		}

		void ObjectReset(OBJID ObjId)
		{
			GH.VMInstance.pObjList[ObjId].Ip = GH.VMInstance.pImage.Copy((ULONG)GH.VMInstance.pObjHead[ObjId].OffsetToInstructions);

			GH.VMInstance.pObjList[ObjId].u = (GH.VMInstance.pObjList[ObjId].u.CallerId, GH.VMInstance.pObjHead[ObjId].TriggerCount);
		}

		void ObjectExit()
		{
			if ((GH.VMInstance.ObjectId > 0) && (GH.VMInstance.ObjectId <= GH.VMInstance.Objects) && (GH.VMInstance.Program[GH.VMInstance.ProgramId].Status != OBJSTAT.STOPPED))
			{ // object valid

				if (GH.VMInstance.pObjList[GH.VMInstance.ObjectId].ObjStatus == RUNNING)
				{ // Save object context

					GH.VMInstance.pObjList[GH.VMInstance.ObjectId].Ip = GH.VMInstance.ObjectIp;
				}
			}
		}

		RESULT ObjectExec()
		{
			RESULT Result = OK;
			OBJID TmpId = 0;


			if ((GH.VMInstance.ProgramId == GUI_SLOT) && (GH.VMInstance.Program[USER_SLOT].Status != OBJSTAT.STOPPED))
			{ // When user program is running - only schedule UI background task

				if ((GH.VMInstance.Objects >= 3) && (GH.VMInstance.Program[GUI_SLOT].Status != OBJSTAT.STOPPED))
				{
					if (GH.VMInstance.ObjectId != 2)
					{
						GH.VMInstance.ObjectId = 2;
					}
					else
					{
						GH.VMInstance.ObjectId = 3;
					}
				}
			}
			else
			{
				do
				{
					// Next object
					if (++GH.VMInstance.ObjectId > GH.VMInstance.Objects)
					{
						// wrap around

						GH.VMInstance.ObjectId = 1;
					}

					if (++TmpId > GH.VMInstance.Objects)
					{
						// no programs running

						Result = RESULT.STOP;
					}

				}
				while ((Result == OK) && (GH.VMInstance.pObjList[GH.VMInstance.ObjectId].ObjStatus != RUNNING));
			}

			return (Result);
		}

		void ObjectEnQueue(OBJID Id)
		{
			if ((Id > 0) && (Id <= GH.VMInstance.Objects))
			{
				GH.VMInstance.pObjList[Id].ObjStatus = RUNNING;
				GH.VMInstance.pObjList[Id].Ip = GH.VMInstance.pImage.Copy((ULONG)GH.VMInstance.pObjHead[Id].OffsetToInstructions);
				GH.VMInstance.pObjList[Id].u = (GH.VMInstance.pObjList[Id].u.CallerId, GH.VMInstance.pObjHead[Id].TriggerCount);
			}
		}

		void ObjectDeQueue(OBJID Id)
		{
			if ((Id > 0) && (Id <= GH.VMInstance.Objects))
			{
				GH.VMInstance.pObjList[Id].Ip = GH.VMInstance.ObjectIp;
				GH.VMInstance.pObjList[Id].ObjStatus = STOPPED;

				SetDispatchStatus(STOPBREAK);
			}
		}

		void ProgramInit()
		{
			PRG pProgram;

			if (GH.VMInstance.ProgramId < MAX_PROGRAMS)
			{
				pProgram = GH.VMInstance.Program[GH.VMInstance.ProgramId];

				if (((pProgram).Status == OBJSTAT.RUNNING) || ((pProgram).Status == OBJSTAT.WAITING))
				{
					GH.VMInstance.pGlobal = pProgram.pGlobal;
					GH.VMInstance.pImage = pProgram.pImage;
					GH.VMInstance.pObjHead = pProgram.pObjHead;
					GH.VMInstance.pObjList = pProgram.pObjList;
					GH.VMInstance.Objects = pProgram.Objects;
					GH.VMInstance.ObjectId = pProgram.ObjectId;
					GH.VMInstance.ObjectIp = pProgram.ObjectIp;
					GH.VMInstance.ObjectLocal = pProgram.ObjectLocal;
					GH.VMInstance.InstrCnt = 0;
					GH.VMInstance.Debug = pProgram.Debug;

				}
			}
		}

		RESULT ProgramReset(PRGID PrgId, IP pI, GP pG, UBYTE Deb)
		{
			RESULT Result = RESULT.FAIL;
			GBINDEX Index;
			GBINDEX RamSize;
			ArrayPointer<UBYTE> pData = new ArrayPointer<byte>();
			OBJID ObjIndex;
			DATA8 No;

			GH.VMInstance.Program[PrgId].Status = OBJSTAT.STOPPED;
			GH.VMInstance.Program[PrgId].StatusChange = OBJSTAT.STOPPED;
			GH.VMInstance.Program[PrgId].Result = RESULT.FAIL;

			if (pI != null)
			{

				// Allocate memory for globals and objects

				RamSize = GetAmountOfRamForImage(pI);

				if (GH.Memory.cMemoryOpen(PrgId, RamSize, pData) == OK)
				{ // Memory reserved

					// Save start of image
					if (Deb == 1)
					{
						GH.VMInstance.Program[PrgId].Debug = 1;
					}
					else
					{
						GH.VMInstance.Program[PrgId].Debug = 0;
					}
					GH.VMInstance.Program[PrgId].pImage = pI;

					if (GH.Validate.cValidateProgram(PrgId, pI, new ArrayPointer<LABEL>(GH.VMInstance.Program[PrgId].Label), GH.VMInstance.TerminalEnabled) != OK)
					{
						if (PrgId != CMD_SLOT)
						{
							LogErrorNumber(nameof(VM_PROGRAM_VALIDATION), Environment.StackTrace);
						}
					}
					else
					{

						// Clear memory

						for (Index = 0; Index < RamSize; Index++)
						{
							pData[(int)Index] = 0;
						}

						for (No = 0; No < MAX_BREAKPOINTS; No++)
						{
							GH.VMInstance.Program[PrgId].Brkp[No].Addr = 0;
							GH.VMInstance.Program[PrgId].Brkp[No].OpCode = 0;
						}

						// Get VMInstance.Objects

						GH.VMInstance.Program[PrgId].Objects = IMGHEAD.GetObject(pI).NumberOfObjects;

						// Allocate GlobalVariables

						GH.VMInstance.Program[PrgId].pData = pData;
						if (pG != null)
						{
							GH.VMInstance.Program[PrgId].pGlobal = pG;
						}
						else
						{
							GH.VMInstance.Program[PrgId].pGlobal = pData;
						}

						pData = pData.Copy(IMGHEAD.GetObject(pI).GlobalBytes);

						// Align & allocate ObjectPointerList (+1)

						pData = new ArrayPointer<byte>(pData.Data, (pData.Offset + 3) & 0xFFFFFFFC);
						GH.VMInstance.Program[PrgId].pObjList = OBJ.GetArray(pData);
						pData = pData.Copy(OBJ.Sizeof * (GH.VMInstance.Program[PrgId].Objects + 1));

						// Make pointer to access object headers starting at one (not zero)

						GH.VMInstance.Program[PrgId].pObjHead = OBJHEAD.GetArray(pI.Copy(IMGHEAD.Sizeof - OBJHEAD.Sizeof));

						for (ObjIndex = 1; ObjIndex <= GH.VMInstance.Program[PrgId].Objects; ObjIndex++)
						{
							// Align

							pData = new ArrayPointer<byte>(pData.Data, (pData.Offset + 3) & 0xFFFFFFFC);

							// Save object pointer in Object list

							GH.VMInstance.Program[PrgId].pObjList[ObjIndex] = OBJ.GetObject(pData);

							// Initialise instruction pointer, trigger counts and status

							GH.VMInstance.Program[PrgId].pObjList[ObjIndex].Ip = pI.Copy((ULONG)GH.VMInstance.Program[PrgId].pObjHead[ObjIndex].OffsetToInstructions);

							GH.VMInstance.Program[PrgId].pObjList[ObjIndex].u = (GH.VMInstance.Program[PrgId].pObjList[ObjIndex].u.CallerId, GH.VMInstance.Program[PrgId].pObjHead[ObjIndex].TriggerCount);

							if ((GH.VMInstance.Program[PrgId].pObjList[ObjIndex].u.TriggerCount > 0 || (ObjIndex > 1)))
							{
								GH.VMInstance.Program[PrgId].pObjList[ObjIndex].ObjStatus = STOPPED;
							}
							else
							{
								if (Deb == 2)
								{
									GH.VMInstance.Program[PrgId].pObjList[ObjIndex].ObjStatus = WAITING;
								}
								else
								{
									GH.VMInstance.Program[PrgId].pObjList[ObjIndex].ObjStatus = RUNNING;
								}
							}

							if (GH.VMInstance.Program[PrgId].pObjHead[ObjIndex].OwnerObjectId != 0)
							{
								GH.VMInstance.Program[PrgId].pObjList[ObjIndex].pLocal = GH.VMInstance.Program[PrgId].pObjList[GH.VMInstance.Program[PrgId].pObjHead[ObjIndex].OwnerObjectId].Local;
							}
							else
							{
								GH.VMInstance.Program[PrgId].pObjList[ObjIndex].pLocal = GH.VMInstance.Program[PrgId].pObjList[ObjIndex].Local;
							}

							// Advance data pointer

							pData = pData.Copy(OBJ.Sizeof + GH.VMInstance.Program[PrgId].pObjHead[ObjIndex].LocalBytes);
						}

						GH.VMInstance.Program[PrgId].ObjectId = 1;
						GH.VMInstance.Program[PrgId].Status = OBJSTAT.RUNNING;
						GH.VMInstance.Program[PrgId].StatusChange = OBJSTAT.RUNNING;

						GH.VMInstance.Program[PrgId].Result = RESULT.BUSY;

						Result = OK;

						if (PrgId == USER_SLOT)
						{
							if (GH.VMInstance.RefCount == 0)
							{
								Result = 0;
								Result |= GH.Ui.cUiOpen();
								Result |= GH.Output.cOutputOpen();
								Result |= GH.Input.cInputOpen();
								Result |= GH.Com.cComOpen();
								Result |= GH.Sound.cSoundOpen();
							}
							GH.VMInstance.RefCount++;
						}
						GH.VMInstance.Program[PrgId].InstrCnt = 0;
						GH.VMInstance.Program[PrgId].StartTime = GetTimeMS();
						GH.VMInstance.Program[PrgId].RunTime = GH.Timer.cTimerGetuS();
					}
				}
			}

			return (Result);
		}

		void ProgramExit()
		{
			PRG pProgram;

			if (GH.VMInstance.ProgramId < MAX_PROGRAMS)
			{
				pProgram = GH.VMInstance.Program[GH.VMInstance.ProgramId];
				pProgram.pGlobal = GH.VMInstance.pGlobal;
				pProgram.pImage = GH.VMInstance.pImage;
				pProgram.pObjHead = GH.VMInstance.pObjHead;
				pProgram.pObjList = GH.VMInstance.pObjList;
				pProgram.Objects = GH.VMInstance.Objects;
				pProgram.ObjectId = GH.VMInstance.ObjectId;
				pProgram.ObjectIp = GH.VMInstance.ObjectIp;
				pProgram.ObjectLocal = GH.VMInstance.ObjectLocal;
				pProgram.InstrCnt += GH.VMInstance.InstrCnt;
				pProgram.Debug = GH.VMInstance.Debug;

				GH.VMInstance.InstrCnt = 0;
				pProgram.Debug = GH.VMInstance.Debug;
			}
		}

		RESULT ProgramExec()
		{
			RESULT Result = RESULT.STOP;
			OBJID TmpId = 0;


			for (TmpId = 0; (TmpId < MAX_PROGRAMS) && (Result == RESULT.STOP); TmpId++)
			{
				if (GH.VMInstance.Program[TmpId].Status != OBJSTAT.STOPPED)
				{
					Result = OK;
				}
			}
			if (Result == OK)
			{
				do
				{
					// next program

					if (++GH.VMInstance.ProgramId >= MAX_PROGRAMS)
					{
						// wrap around

						GH.VMInstance.ProgramId = 0;
					}

				}
				while (GH.VMInstance.Program[GH.VMInstance.ProgramId].Status == OBJSTAT.STOPPED);
			}


			return (Result);
		}

		GBINDEX GetAmountOfRamForImage(IP pI)
		{
			GBINDEX Bytes = 0;
			OBJID NoOfObj;
			OBJID ObjId;
			ArrayPointer<OBJHEAD> pHead;

			NoOfObj = (IMGHEAD.GetObject(pI)).NumberOfObjects;

			Bytes += (IMGHEAD.GetObject(pI)).GlobalBytes;
			Bytes = (Bytes + 3) & 0xFFFFFFFC;
			Bytes += (uint)(OBJ.Sizeof * (NoOfObj + 1));

			pHead = OBJHEAD.GetArray(pI.Copy(IMGHEAD.Sizeof));

			for (ObjId = 1; ObjId <= NoOfObj; ObjId++)
			{
				Bytes = (Bytes + 3) & 0xFFFFFFFC;
				Bytes += OBJ.Sizeof + pHead[0].LocalBytes;
				pHead++;
			}

			Bytes = 0;

			NoOfObj = (IMGHEAD.GetObject(pI)).NumberOfObjects;

			Bytes += (IMGHEAD.GetObject(pI)).GlobalBytes;
			Bytes = (Bytes + 3) & 0xFFFFFFFC;
			Bytes += (uint)(OBJ.Sizeof * (NoOfObj + 1));

			pHead = OBJHEAD.GetArray(pI.Copy(IMGHEAD.Sizeof - OBJHEAD.Sizeof));

			for (ObjId = 1; ObjId <= NoOfObj; ObjId++)
			{
				Bytes = (Bytes + 3) & 0xFFFFFFFC;
				Bytes += OBJ.Sizeof + pHead[ObjId].LocalBytes;
			}

			return (Bytes);
		}
		#endregion

		public ushort CallingObjectId()
		{
			return (GH.VMInstance.ObjectId);
		}

		public ushort CurrentProgramId()
		{
			return (GH.VMInstance.ProgramId);
		}

		public OBJSTAT ProgramStatus(ushort PrgId)
		{
			return (GH.VMInstance.Program[PrgId].Status);
		}

		public OBJSTAT ProgramStatusChange(ushort PrgId)
		{
			OBJSTAT Status;

			Status = (OBJSTAT)(DATA8)GH.VMInstance.Program[PrgId].StatusChange;
			GH.VMInstance.Program[PrgId].StatusChange = 0;

			return (Status);
		}

		IP GetImageStart()
		{
			return (GH.VMInstance.pImage);
		}

		public void SetDispatchStatus(int DspStat)
		{
			GH.VMInstance.DispatchStatus = (DSPSTAT)DspStat;

			if (GH.VMInstance.DispatchStatus != DSPSTAT.NOBREAK)
			{
				GH.VMInstance.Priority = 0;
			}
		}

		public void SetInstructions(uint Instructions)
		{
			if (Instructions <= PRG_PRIORITY)
			{
				GH.VMInstance.Priority = Instructions;
			}
		}

		public void AdjustObjectIp(int Value)
		{
			GH.VMInstance.ObjectIp += Value;
		}

		public ArrayPointer<byte> GetObjectIp()
		{
			return (GH.VMInstance.ObjectIp);
		}

		public void SetObjectIp(ArrayPointer<byte> Ip)
		{
			GH.VMInstance.ObjectIp = Ip;
		}

		ULONG GetTime()
		{
			return (GH.Timer.cTimerGetuS() - GH.VMInstance.Program[GH.VMInstance.ProgramId].RunTime);
		}

		ULONG GetTimeMS()
		{
			return (GH.Timer.cTimerGetmS());
		}

		ULONG GetTimeUS()
		{
			return (GH.Timer.cTimerGetuS());
		}

		ArrayPointer<byte> CurrentObjectIp()
		{
			return ((GH.VMInstance.ObjectIp - GH.VMInstance.pImage));
		}

		void VmPrint(ArrayPointer<UBYTE> pString)
		{
			if (GH.VMInstance.TerminalEnabled != 0)
			{
				GH.Ev3System.Logger.LogInfo(pString.AsString());
			}
		}

		void LogErrorNumber(string name, string stackTrace)
		{
			GH.Ev3System.Logger.LogWarning($"An {name} error occured in {stackTrace}");
		}

		public void SetTerminalEnable(sbyte Value)
		{
			GH.VMInstance.TerminalEnabled = Value;
		}

		public sbyte GetTerminalEnable()
		{
			return (GH.VMInstance.TerminalEnabled);
		}

		public void GetResourcePath(ArrayPointer<UBYTE> pString, sbyte MaxLength)
		{
			GH.Memory.cMemoryGetResourcePath(GH.VMInstance.ProgramId, pString, MaxLength);
		}

		public ArrayPointer<UBYTE> VmMemoryResize(short Handle, int Elements)
		{
			return (GH.Memory.cMemoryResize(GH.VMInstance.ProgramId, Handle, Elements));
		}

		void SetVolumePercent(DATA8 Volume)
		{
			GH.VMInstance.NonVol.VolumePercent = Volume;
		}

		DATA8 GetVolumePercent()
		{
			return (GH.VMInstance.NonVol.VolumePercent);
		}

		public void SetSleepMinutes(sbyte Minutes)
		{
			GH.VMInstance.NonVol.SleepMinutes = Minutes;
		}

		public sbyte GetSleepMinutes()
		{
			return (GH.VMInstance.NonVol.SleepMinutes);
		}

		void SetUiUpdate()
		{
			GH.UiInstance.UiUpdate = 1;
		}

		// TODO: ERROR defines skipped

		public DSPSTAT ExecuteByteCode(ArrayPointer<byte> pByteCode, ArrayPointer<byte> pGlobals, ArrayPointer<byte> pLocals)
		{
			DSPSTAT Result;
			ULONG Time;

			// Save running object parameters
			GH.VMInstance.ObjIpSave = GH.VMInstance.ObjectIp;
			GH.VMInstance.ObjGlobalSave = GH.VMInstance.pGlobal;
			GH.VMInstance.ObjLocalSave = GH.VMInstance.ObjectLocal;
			GH.VMInstance.DispatchStatusSave = GH.VMInstance.DispatchStatus;
			GH.VMInstance.PrioritySave = GH.VMInstance.Priority;

			// InitExecute special byte code stream
			GH.VMInstance.ObjectIp = pByteCode;
			GH.VMInstance.pGlobal = pGlobals;
			GH.VMInstance.ObjectLocal = pLocals;
			GH.VMInstance.Priority = 1;

			// Execute special byte code stream
			GH.UiInstance.ButtonState[IDX_BACK_BUTTON] &= ~BUTTON_LONGPRESS;
			while (GH.VMInstance.ObjectIp.Offset != opOBJECT_END && ((GH.UiInstance.ButtonState[IDX_BACK_BUTTON] & BUTTON_LONGPRESS) == 0))
			{
				GH.VMInstance.DispatchStatus = DSPSTAT.NOBREAK;
				GH.VMInstance.Priority = C_PRIORITY;

				while ((GH.VMInstance.Priority != 0) && (GH.VMInstance.ObjectIp.Offset != opOBJECT_END))
				{
					GH.VMInstance.Priority--;
					PrimDispatchTabel[GH.VMInstance.ObjectIp++](); // TODO: probably ++ before
				}

				GH.VMInstance.NewTime = GetTimeMS();

				Time = GH.VMInstance.NewTime - GH.VMInstance.OldTime1;

				if (Time >= UPDATE_TIME1)
				{
					GH.VMInstance.OldTime1 += Time;

					GH.Com.cComUpdate();
					GH.Sound.cSoundUpdate();
				}

				Time = GH.VMInstance.NewTime - GH.VMInstance.OldTime2;

				if (Time >= UPDATE_TIME2)
				{
					GH.VMInstance.OldTime2 += Time;

					// usleep(10); // TODO: WTF
					GH.Input.cInputUpdate((UWORD)Time);
					GH.Ui.cUiUpdate((UWORD)Time);
				}
			}
			Result = GH.VMInstance.DispatchStatus;

			GH.UiInstance.ButtonState[IDX_BACK_BUTTON] &= ~BUTTON_LONGPRESS;

			// Restore running object parameters
			GH.VMInstance.Priority = GH.VMInstance.PrioritySave;
			GH.VMInstance.DispatchStatus = GH.VMInstance.DispatchStatusSave;
			GH.VMInstance.ObjectLocal = GH.VMInstance.ObjLocalSave;
			GH.VMInstance.pGlobal = GH.VMInstance.ObjGlobalSave;
			GH.VMInstance.ObjectIp = GH.VMInstance.ObjIpSave;

			return (Result);
		}

		public ArrayPointer<byte> PrimParPointer()
		{
			ArrayPointer<byte> primParResult = new ArrayPointer<byte>();
			IMGDATA Data;

			primParResult.Offset = GH.VMInstance.Value;
			primParResult.Data = GH.VMInstance.ObjectIp;

			Data = ((IMGDATA)GH.VMInstance.ObjectIp++);
			GH.VMInstance.Handle.Offset = uint.MaxValue;

			if ((Data & PRIMPAR_LONG) != 0)
			{ // long format

				if ((Data & PRIMPAR_VARIABEL) != 0)
				{ // variabel

					switch (Data & PRIMPAR_BYTES)
					{

						case PRIMPAR_1_BYTE:
							{ // One byte to follow

								GH.VMInstance.Value = (ULONG)((IMGDATA)GH.VMInstance.ObjectIp++);
							}
							break;

						case PRIMPAR_2_BYTES:
							{ // Two bytes to follow

								GH.VMInstance.Value = (ULONG)((IMGDATA)GH.VMInstance.ObjectIp++);
								GH.VMInstance.Value |= ((ULONG)((IMGDATA)GH.VMInstance.ObjectIp++) << 8);
							}
							break;

						case PRIMPAR_4_BYTES:
							{ // Four bytes to follow

								GH.VMInstance.Value = (ULONG)((IMGDATA)GH.VMInstance.ObjectIp++);
								GH.VMInstance.Value |= ((ULONG)((IMGDATA)GH.VMInstance.ObjectIp++) << 8);
								GH.VMInstance.Value |= ((ULONG)((IMGDATA)GH.VMInstance.ObjectIp++) << 16);
								GH.VMInstance.Value |= ((ULONG)((IMGDATA)GH.VMInstance.ObjectIp++) << 24);
							}
							break;

					}
					if ((Data & PRIMPAR_GLOBAL) != 0)
					{ // global

						primParResult.Offset = GH.VMInstance.Value;
						primParResult.Data = GH.VMInstance.pGlobal;
					}
					else
					{ // local

						primParResult.Offset = GH.VMInstance.Value;
						primParResult.Data = GH.VMInstance.ObjectLocal;
					}
				}
				else
				{ // constant

					if ((Data & PRIMPAR_LABEL) != 0)
					{ // label

						GH.VMInstance.Value = (ULONG)((IMGDATA)GH.VMInstance.ObjectIp++);

						if ((GH.VMInstance.Value > 0) && (GH.VMInstance.Value < MAX_LABELS))
						{
							GH.VMInstance.Value = (ULONG)GH.VMInstance.Program[GH.VMInstance.ProgramId].Label[GH.VMInstance.Value].Addr;
							GH.VMInstance.Value -= (GH.VMInstance.ObjectIp - (ULONG)GH.VMInstance.Program[GH.VMInstance.ProgramId].pImage);
							primParResult.Offset = GH.VMInstance.Value;
							primParResult.Data = GH.VMInstance.ObjectIp;
						}
					}
					else
					{ // value

						switch (Data & PRIMPAR_BYTES)
						{
							case PRIMPAR_STRING_OLD:
							case PRIMPAR_STRING:
								{ // Zero terminated

									primParResult.Offset = (uint)GH.VMInstance.ObjectIp;
									primParResult.Data = GH.VMInstance.ObjectIp;
									while (((IMGDATA)GH.VMInstance.ObjectIp++) != 0)
									{ // Adjust Ip
									}
								}
								break;

							case PRIMPAR_1_BYTE:
								{ // One byte to follow

									GH.VMInstance.Value = (ULONG)((IMGDATA)GH.VMInstance.ObjectIp++);
									if ((GH.VMInstance.Value & 0x00000080) != 0)
									{ // Adjust if negative

										GH.VMInstance.Value |= 0xFFFFFF00;
									}
								}
								break;

							case PRIMPAR_2_BYTES:
								{ // Two bytes to follow

									GH.VMInstance.Value = (ULONG)((IMGDATA)GH.VMInstance.ObjectIp++);
									GH.VMInstance.Value |= ((ULONG)((IMGDATA)GH.VMInstance.ObjectIp++) << 8);
									if ((GH.VMInstance.Value & 0x00008000) != 0)
									{ // Adjust if negative

										GH.VMInstance.Value |= 0xFFFF0000;
									}
								}
								break;

							case PRIMPAR_4_BYTES:
								{ // Four bytes to follow

									GH.VMInstance.Value = (ULONG)((IMGDATA)GH.VMInstance.ObjectIp++);
									GH.VMInstance.Value |= ((ULONG)((IMGDATA)GH.VMInstance.ObjectIp++) << 8);
									GH.VMInstance.Value |= ((ULONG)((IMGDATA)GH.VMInstance.ObjectIp++) << 16);
									GH.VMInstance.Value |= ((ULONG)((IMGDATA)GH.VMInstance.ObjectIp++) << 24);
								}
								break;

						}
					}
				}
				if ((Data & PRIMPAR_HANDLE) != 0)
				{
					GH.VMInstance.Handle = primParResult;
					GH.Memory.cMemoryArraryPointer(GH.VMInstance.ProgramId, (short)GH.VMInstance.Handle.Offset, primParResult);
				}
				else
				{
					if ((Data & PRIMPAR_ADDR) != 0)
					{
						GH.VMInstance.Value = primParResult.Offset;
					}
				}
			}
			else
			{ // short format

				if ((Data & PRIMPAR_VARIABEL) != 0)
				{ // variabel

					GH.VMInstance.Value = (ULONG)(Data & PRIMPAR_INDEX);

					if ((Data & PRIMPAR_GLOBAL) != 0)
					{ // global

						primParResult.Offset = GH.VMInstance.Value;
						primParResult.Data = GH.VMInstance.pGlobal;
					}
					else
					{ // local
						primParResult.Offset = GH.VMInstance.Value;
						primParResult.Data = GH.VMInstance.ObjectLocal;
					}
				}
				else
				{ // constant

					GH.VMInstance.Value = (ULONG)(Data & PRIMPAR_VALUE);

					if ((Data & PRIMPAR_CONST_SIGN) != 0)
					{ // Adjust if negative

						GH.VMInstance.Value |= ~(ULONG)(PRIMPAR_VALUE);
					}
				}
			}

			return (primParResult);
		}

		public void PrimParAdvance()
		{
			IMGDATA Data;

			Data = (GH.VMInstance.ObjectIp++)[0];

			if ((Data & PRIMPAR_LONG) != 0)
			{ // long format

				if ((Data & PRIMPAR_VARIABEL) != 0)
				{ // variabel

					switch (Data & PRIMPAR_BYTES)
					{

						case PRIMPAR_1_BYTE:
							{ // One byte to follow

								GH.VMInstance.ObjectIp++;
							}
							break;

						case PRIMPAR_2_BYTES:
							{ // Two bytes to follow

								GH.VMInstance.ObjectIp++;
								GH.VMInstance.ObjectIp++;
							}
							break;

						case PRIMPAR_4_BYTES:
							{ // Four bytes to follow

								GH.VMInstance.ObjectIp++;
								GH.VMInstance.ObjectIp++;
								GH.VMInstance.ObjectIp++;
								GH.VMInstance.ObjectIp++;
							}
							break;
					}
				}
				else
				{ // constant

					if ((Data & PRIMPAR_LABEL) != 0)
					{ // label

						GH.VMInstance.ObjectIp++;
					}
					else
					{ // value

						switch (Data & PRIMPAR_BYTES)
						{
							case PRIMPAR_STRING_OLD:
							case PRIMPAR_STRING:
								{ // Zero terminated

									while ((GH.VMInstance.ObjectIp++)[0] != 0)
									{ // Adjust Ip
									}
								}
								break;

							case PRIMPAR_1_BYTE:
								{ // One byte to follow

									GH.VMInstance.ObjectIp++;
								}
								break;

							case PRIMPAR_2_BYTES:
								{ // Two bytes to follow

									GH.VMInstance.ObjectIp++;
									GH.VMInstance.ObjectIp++;
								}
								break;

							case PRIMPAR_4_BYTES:
								{ // Four bytes to follow

									GH.VMInstance.ObjectIp++;
									GH.VMInstance.ObjectIp++;
									GH.VMInstance.ObjectIp++;
									GH.VMInstance.ObjectIp++;
								}
								break;
						}
					}
				}
			}
		}

		void CopyParsToLocals(OBJID Id)
		{
			IP TmpIp;      // Save calling Ip
			IP TypeIp;     // Called Ip
			ArrayPointer<byte> pLocals;    // Called locals
			PARS NoOfPars;   // Called no of parameters
			IMGDATA Type;       // Coded type
			ArrayPointer<byte> Result;     // Pointer to value
			DATA32 Size;
			DATA8 Flag;

			TmpIp = GH.VMInstance.ObjectIp;
			TypeIp = GH.VMInstance.Program[GH.VMInstance.ProgramId].pImage;
			TypeIp = TypeIp.Copy((ULONG)GH.VMInstance.pObjHead[Id].OffsetToInstructions);
			pLocals = GH.VMInstance.pObjList[Id].pLocal;

			NoOfPars = (PARS)(TypeIp++).GetUBYTE();

			while ((NoOfPars--) != 0)
			{
				// Get type from sub preamble
				Type = (IMGDATA)(TypeIp++).GetUBYTE();

				// Get pointer to value and increment VMInstance.ObjectIP

				Result = PrimParPointer();

				if ((Type & CALLPAR_IN) != 0)
				{ // Input

					switch (Type & CALLPAR_TYPE)
					{

						case CALLPAR_DATA8:
							{
								pLocals.SetDATA8(Result.GetDATA8());
								pLocals = pLocals.Copy(1);
							}
							break;

						case CALLPAR_DATA16:
							{
								pLocals = new ArrayPointer<byte>(pLocals.Data, (uint)((pLocals.Offset + 1) & ~1));
								pLocals.SetDATA16(Result.GetDATA16());
								pLocals = pLocals.Copy(2);
							}
							break;

						case CALLPAR_DATA32:
							{
								pLocals = new ArrayPointer<byte>(pLocals.Data, (uint)((pLocals.Offset + 3) & ~3));
								pLocals.SetDATA32(Result.GetDATA32());
								pLocals = pLocals.Copy(4);
							}
							break;

						case CALLPAR_DATAF:
							{
								pLocals = new ArrayPointer<byte>(pLocals.Data, (uint)((pLocals.Offset + 3) & ~3));
								pLocals.SetDATAF(Result.GetDATAF());
								pLocals = pLocals.Copy(4);
							}
							break;

						case CALLPAR_STRING:
							{
								Size = (DATA32)(TypeIp++).GetDATA32();
								Flag = 1;
								while (Size != 0)
								{
									if (Flag != 0)
									{
										Flag = Result.GetDATA8();
									}
									pLocals.SetDATA8(Flag);
									Result = Result.Copy(1);
									pLocals = pLocals.Copy(1);
									Size--;
								}
								pLocals = new ArrayPointer<byte>(pLocals.Data, pLocals.Offset - 1);
								pLocals.SetDATA8(0);
								pLocals = pLocals.Copy(1);
							}
							break;

					}
				}
				else
				{
					if ((Type & CALLPAR_OUT) != 0)
					{ // Output

						switch (Type & CALLPAR_TYPE)
						{

							case CALLPAR_DATA8:
								{
									pLocals = pLocals.Copy(1);
								}
								break;

							case CALLPAR_DATA16:
								{
									pLocals = new ArrayPointer<byte>(pLocals.Data, (uint)((pLocals.Offset + 1) & ~1));
									pLocals = pLocals.Copy(2);
								}
								break;

							case CALLPAR_DATA32:
								{
									pLocals = new ArrayPointer<byte>(pLocals.Data, (uint)((pLocals.Offset + 3) & ~3));
									pLocals = pLocals.Copy(4);
								}
								break;

							case CALLPAR_DATAF:
								{
									pLocals = new ArrayPointer<byte>(pLocals.Data, (uint)((pLocals.Offset + 3) & ~3));
									pLocals = pLocals.Copy(4);
								}
								break;

							case CALLPAR_STRING:
								{
									Size = (DATA32)(TypeIp++).GetDATA32();
									pLocals = pLocals.Copy(Size);
								}
								break;

						}
					}
				}
			}
			GH.VMInstance.pObjList[Id].Ip = TypeIp;

			// Rewind caller Ip
			GH.VMInstance.ObjectIp = TmpIp;
		}

		void CopyLocalsToPars(OBJID Id)
		{
			IP TmpIp;      // Calling Ip
			IP TypeIp;     // Called Ip
			ArrayPointer<byte> pLocals;    // Called locals
			PARS NoOfPars;   // Called no of parameters
			IMGDATA Type;       // Coded type
			ArrayPointer<byte> Result;     // Pointer to value
			DATA32 Size;
			DATA8 Flag;

			// Point to start of parameters
			TmpIp = GH.VMInstance.ObjectIp;
			GH.VMInstance.ObjectIp = GH.VMInstance.pObjList[Id].Ip;

			// Point to start of sub
			TypeIp = GH.VMInstance.Program[GH.VMInstance.ProgramId].pImage;
			TypeIp = TypeIp.Copy((ULONG)GH.VMInstance.pObjHead[GH.VMInstance.ObjectId].OffsetToInstructions);
			pLocals = GH.VMInstance.pObjList[GH.VMInstance.ObjectId].pLocal;

			NoOfPars = (PARS)(TypeIp++).GetUBYTE();

			while ((NoOfPars--) != 0)
			{
				// Get type from sub preamble
				Type = (IMGDATA)(TypeIp++).GetUBYTE();

				// Get pointer to value and increment VMInstance.ObjectIp
				Result = PrimParPointer();

				if ((Type & CALLPAR_OUT) != 0)
				{ // Output

					switch (Type & CALLPAR_TYPE)
					{
						case CALLPAR_DATA8:
							{
								Result.SetDATA8(pLocals.GetDATA8());
								pLocals = pLocals.Copy(1);
							}
							break;

						case CALLPAR_DATA16:
							{
								pLocals = new ArrayPointer<byte>(pLocals.Data, (uint)((pLocals.Offset + 1) & ~1));
								Result.SetDATA16(pLocals.GetDATA16());
								pLocals = pLocals.Copy(2);
							}
							break;

						case CALLPAR_DATA32:
							{
								pLocals = new ArrayPointer<byte>(pLocals.Data, (uint)((pLocals.Offset + 3) & ~3));
								Result.SetDATA32(pLocals.GetDATA32());
								pLocals = pLocals.Copy(4);
							}
							break;

						case CALLPAR_DATAF:
							{
								pLocals = new ArrayPointer<byte>(pLocals.Data, (uint)((pLocals.Offset + 3) & ~3));
								Result.SetDATAF(pLocals.GetDATAF());
								pLocals = pLocals.Copy(4);
							}
							break;

						case CALLPAR_STRING:
							{
								Size = (DATA32)(TypeIp++).GetDATA32();
								Flag = 1;
								while (Size != 0)
								{
									if (Flag != 0)
									{
										Flag = (pLocals).GetDATA8();
									}
									Result.SetDATA8(Flag);
									Result = Result.Copy(1);
									pLocals = pLocals.Copy(1);
									Size--;
								}
								Result = new ArrayPointer<byte>(Result.Data, Result.Offset - 1);
								Result.SetDATA8(0);
							}
							break;

					}
				}
				else
				{
					if ((Type & CALLPAR_IN) != 0)
					{ // Input

						switch (Type & CALLPAR_TYPE)
						{

							case CALLPAR_DATA8:
								{
									pLocals = pLocals.Copy(1);
								}
								break;

							case CALLPAR_DATA16:
								{
									pLocals = new ArrayPointer<byte>(pLocals.Data, (uint)((pLocals.Offset + 1) & ~1));
									pLocals = pLocals.Copy(2);
								}
								break;

							case CALLPAR_DATA32:
								{
									pLocals = new ArrayPointer<byte>(pLocals.Data, (uint)((pLocals.Offset + 3) & ~3));
									pLocals = pLocals.Copy(4);
								}
								break;

							case CALLPAR_DATAF:
								{
									pLocals = new ArrayPointer<byte>(pLocals.Data, (uint)((pLocals.Offset + 3) & ~3));
									pLocals = pLocals.Copy(4);
								}
								break;

							case CALLPAR_STRING:
								{
									Size = (DATA32)(TypeIp++).GetDATA32();
									pLocals = pLocals.Copy(Size);
								}
								break;

						}
					}
				}
			}

			// Adjust caller Ip
			GH.VMInstance.pObjList[Id].Ip = GH.VMInstance.ObjectIp;
			// Restore calling Ip
			GH.VMInstance.ObjectIp = TmpIp;
		}

		public sbyte CheckSdcard(VarPointer<sbyte> pChanged, VarPointer<int> pTotal, VarPointer<int> pFree, sbyte Force)
		{
			DATA8 Result = 0;
			DATAF Tmp;

			pChanged.Data = 0;

			// "/media/card"
			GH.VMInstance.SdcardSize = 512; // in mb
			GH.VMInstance.SdcardFree = 512; // in mb
			GH.VMInstance.SdcardOk = 1;
			pTotal.Data = GH.VMInstance.SdcardSize;
			pFree.Data = GH.VMInstance.SdcardFree;
			Result = GH.VMInstance.SdcardOk;

			return (Result);
		}

		public sbyte CheckUsbstick(VarPointer<sbyte> pChanged, VarPointer<int> pTotal, VarPointer<int> pFree, sbyte Force)
		{
			DATA8 Result = 0;
			DATAF Tmp;

			pChanged.Data = 0;

			// "/media/usb"
			GH.VMInstance.SdcardSize = 512; // in mb
			GH.VMInstance.SdcardFree = 512; // in mb
			GH.VMInstance.SdcardOk = 1;
			pTotal.Data = GH.VMInstance.SdcardSize;
			pFree.Data = GH.VMInstance.SdcardFree;
			Result = GH.VMInstance.SdcardOk;

			return (Result);
		}

		public void Error()
		{
			ProgramEnd(GH.VMInstance.ProgramId);
			GH.VMInstance.Program[GH.VMInstance.ProgramId].Result = RESULT.FAIL;
			SetDispatchStatus(INSTRBREAK);
		}

		public void Nop()
		{
		}

		public void ProgramStop()
		{
			DATA16 PrgId;

			PrgId = (DATA16)PrimParPointer().GetDATA16();

			if (PrgId == GUI_SLOT)
			{
				PrgId = MAX_PROGRAMS;
				do
				{
					PrgId--;
					ProgramEnd((ushort)PrgId);
				}
				while (PrgId != 0);
			}
			else
			{
				if (PrgId == CURRENT_SLOT)
				{
					PrgId = (short)CurrentProgramId();
				}
				ProgramEnd((ushort)PrgId);
			}
		}

		public void ProgramStart()
		{
			PRGID PrgId;
			PRGID TmpPrgId;
			IP pI;
			UBYTE DB;
			UBYTE Flag = 0;

			PrgId = (PRGID)PrimParPointer().GetUWORD();

			// Dummy
			pI = PrimParPointer();

			pI = PrimParPointer();
			DB = PrimParPointer().GetUBYTE();


			if (GH.VMInstance.Program[PrgId].Status == OBJSTAT.STOPPED)
			{
				TmpPrgId = CurrentProgramId();

				if ((TmpPrgId == CMD_SLOT) || (TmpPrgId == TERM_SLOT))
				{ // Direct command starting a program

					if ((GH.VMInstance.Program[USER_SLOT].Status == OBJSTAT.STOPPED) && (GH.VMInstance.Program[DEBUG_SLOT].Status == OBJSTAT.STOPPED))
					{ // User and debug must be stooped

						if (ProgramReset(PrgId, pI, null, DB) == OK)
						{
							Flag = 1;
						}
					}
				}
				else
				{ // Gui, user or debug starting a program

					if (ProgramReset(PrgId, pI, null, DB) == OK)
					{
						Flag = 1;
					}
				}
			}
			if (Flag == 0)
			{
				//    LogErrorNumber(VM_PROGRAM_NOT_STARTED);
			}
		}

		public void ObjectStop()
		{
			ObjectDeQueue((OBJID)PrimParPointer().GetUWORD());
		}

		public void ObjectStart()
		{
			ObjectEnQueue((OBJID)PrimParPointer().GetUWORD());
		}

		public void ObjectTrig()
		{
			OBJID TmpId;

			TmpId = (OBJID)PrimParPointer().GetUWORD();

			GH.VMInstance.pObjList[TmpId].ObjStatus = WAITING;
			if (GH.VMInstance.pObjList[TmpId].u.TriggerCount != 0)
			{
				GH.VMInstance.pObjList[TmpId].u = (GH.VMInstance.pObjList[TmpId].u.CallerId, (ushort)(GH.VMInstance.pObjList[TmpId].u.TriggerCount - 1));
				if (GH.VMInstance.pObjList[TmpId].u.TriggerCount == 0)
				{
					ObjectReset(TmpId);
					ObjectEnQueue(TmpId);
				}
			}
		}

		public void ObjectWait()
		{
			OBJID TmpId;
			IP TmpIp;

			TmpIp = GH.VMInstance.ObjectIp;
			TmpId = (OBJID)PrimParPointer().GetUWORD();

			if (GH.VMInstance.pObjList[TmpId].ObjStatus != STOPPED)
			{
				GH.VMInstance.ObjectIp = TmpIp - 1;
				SetDispatchStatus(BUSYBREAK);
			}
		}

		public void ObjectReturn()
		{
			OBJID ObjectIdCaller;

			// Get caller id from saved
			ObjectIdCaller = GH.VMInstance.pObjList[GH.VMInstance.ObjectId].u.CallerId;

			// Copy local variables to parameters
			GH.VMInstance.ObjectLocal = GH.VMInstance.pObjList[ObjectIdCaller].pLocal;
			CopyLocalsToPars(ObjectIdCaller);

			// Stop called object and start calling object
			GH.VMInstance.pObjList[GH.VMInstance.ObjectId].Ip = GH.VMInstance.ObjectIp;
			GH.VMInstance.pObjList[GH.VMInstance.ObjectId].ObjStatus = STOPPED;

			GH.VMInstance.ObjectId = ObjectIdCaller;
			GH.VMInstance.pObjList[GH.VMInstance.ObjectId].ObjStatus = RUNNING;
			GH.VMInstance.ObjectIp = GH.VMInstance.pObjList[GH.VMInstance.ObjectId].Ip;
			GH.VMInstance.ObjectLocal = GH.VMInstance.pObjList[GH.VMInstance.ObjectId].pLocal;
		}

		public void ObjectCall()
		{
			IP TmpIp;
			OBJID ObjectIdToCall;

			// Save IP in case object are locked
			TmpIp = GetObjectIp();

			// Get object to call from byte stream
			ObjectIdToCall = (OBJID)PrimParPointer().GetUWORD();
			if (GH.VMInstance.pObjList[ObjectIdToCall].ObjStatus == STOPPED)
			{ // Object free

				// Get number of parameters
				PrimParPointer();

				// Initialise  object
				ObjectReset(ObjectIdToCall);

				// Save mother id
				GH.VMInstance.pObjList[ObjectIdToCall].u = (GH.VMInstance.ObjectId, GH.VMInstance.pObjList[ObjectIdToCall].u.TriggerCount);

				// Copy parameters to local variables
				CopyParsToLocals(ObjectIdToCall);

				// Halt calling object
				GH.VMInstance.pObjList[GH.VMInstance.ObjectId].Ip = GH.VMInstance.ObjectIp;
				GH.VMInstance.pObjList[GH.VMInstance.ObjectId].ObjStatus = HALTED;

				// Start called object
				GH.VMInstance.ObjectId = ObjectIdToCall;
				GH.VMInstance.pObjList[GH.VMInstance.ObjectId].ObjStatus = RUNNING;
				GH.VMInstance.ObjectIp = GH.VMInstance.pObjList[GH.VMInstance.ObjectId].Ip;
				GH.VMInstance.ObjectLocal = GH.VMInstance.pObjList[GH.VMInstance.ObjectId].pLocal;

			}
			else
			{ // Object locked - rewind IP

				GH.Ev3System.Logger.LogInfo($"SUBCALL {ObjectIdToCall} BUSY status = {GH.VMInstance.pObjList[ObjectIdToCall].ObjStatus}\r\n");
				SetObjectIp(TmpIp - 1);
				SetDispatchStatus(BUSYBREAK);
			}
		}

		public void ObjectEnd()
		{
			GH.VMInstance.pObjList[GH.VMInstance.ObjectId].Ip = GH.VMInstance.Program[GH.VMInstance.ProgramId].pImage.Copy((ULONG)GH.VMInstance.Program[GH.VMInstance.ProgramId].pObjHead[GH.VMInstance.ObjectId].OffsetToInstructions);
			GH.VMInstance.pObjList[GH.VMInstance.ObjectId].ObjStatus = STOPPED;
			SetDispatchStatus(STOPBREAK);
		}

		public void Sleep()
		{
			SetDispatchStatus(SLEEPBREAK);
		}

		public void ProgramInfo()
		{
			DATA8 Cmd;
			DATA16 Instr;
			PRGID PrgId;
			OBJID ObjIndex;

			Cmd = (DATA8)PrimParPointer().GetDATA8();
			PrgId = (PRGID)PrimParPointer().GetUWORD();

			switch (Cmd)
			{
				case OBJ_STOP:
					{
						ObjIndex = (OBJID)PrimParPointer().GetUWORD();
						if ((ObjIndex > 0) && (ObjIndex <= GH.VMInstance.Program[PrgId].Objects) && (GH.VMInstance.Program[PrgId].Status != OBJSTAT.STOPPED))
						{
							GH.VMInstance.Program[PrgId].pObjList[ObjIndex].ObjStatus = STOPPED;
						}
					}
					break;

				case OBJ_START:
					{
						ObjIndex = (OBJID)PrimParPointer().GetUWORD();
						if ((ObjIndex > 0) && (ObjIndex <= GH.VMInstance.Program[PrgId].Objects) && (GH.VMInstance.Program[PrgId].Status != OBJSTAT.STOPPED))
						{
							if (ObjIndex == 1)
							{
								GH.VMInstance.Program[PrgId].StartTime = GetTimeMS();
								GH.VMInstance.Program[PrgId].RunTime = GH.Timer.cTimerGetuS();
							}
							GH.VMInstance.Program[PrgId].pObjList[ObjIndex].ObjStatus = RUNNING;
							GH.VMInstance.Program[PrgId].pObjList[ObjIndex].Ip = GH.VMInstance.Program[PrgId].pImage.Copy((ULONG)GH.VMInstance.Program[PrgId].pObjHead[ObjIndex].OffsetToInstructions);
							GH.VMInstance.Program[PrgId].pObjList[ObjIndex].u = (GH.VMInstance.Program[PrgId].pObjList[ObjIndex].u.CallerId, GH.VMInstance.Program[PrgId].pObjHead[ObjIndex].TriggerCount);
						}
					}
					break;

				case GET_STATUS:
					{
						PrimParPointer().SetDATA8((DATA8)GH.VMInstance.Program[PrgId].Status);
					}
					break;

				case GET_PRGRESULT:
					{
						PrimParPointer().SetDATA8((DATA8)GH.VMInstance.Program[PrgId].Result);
					}
					break;

				case GET_SPEED:
					{
						PrimParPointer().SetDATA32((DATA32)(((float)GH.VMInstance.Program[PrgId].InstrCnt * (float)1000000) / (float)GH.VMInstance.Program[PrgId].InstrTime));
					}
					break;

				case SET_INSTR:
					{
						Instr = (DATA16)PrimParPointer().GetDATA16();
						SetInstructions((ULONG)Instr);
					}
					break;

				default:
					{
						SetDispatchStatus(FAILBREAK);
					}
					break;

			}
		}

		public void DefLabel()
		{
			PrimParPointer();
		}

		public void Probe()
		{
			PRGID PrgId;
			OBJID ObjId;
			GBINDEX RamOffset;
			ArrayPointer<byte> Ram;
			GBINDEX Size;
			GBINDEX Tmp;
			GBINDEX Lng;

			PrgId = (PRGID)PrimParPointer().GetUWORD();
			ObjId = (OBJID)PrimParPointer().GetUWORD();
			RamOffset = (GBINDEX)PrimParPointer().GetULONG();
			Size = (GBINDEX)PrimParPointer().GetULONG();

			if (GH.VMInstance.Program[PrgId].Status != OBJSTAT.STOPPED)
			{
				if (ObjId == 0)
				{
					Ram = GH.VMInstance.Program[PrgId].pGlobal;
					Lng = (IMGHEAD.GetObject(GH.VMInstance.Program[PrgId].pImage)).GlobalBytes;
				}
				else
				{
					Ram = GH.VMInstance.Program[PrgId].pObjList[ObjId].pLocal;
					Lng = GH.VMInstance.Program[PrgId].pObjHead[ObjId].LocalBytes;
				}
				Ram.Offset += RamOffset;
				if (Size == 0)
				{
					Size = Lng;
				}
				if (Size != 0)
				{
					CommonHelper.Snprintf(GH.VMInstance.PrintBuffer, PRINTBUFFERSIZE, $"    PROBE  Prg={PrgId} Obj={ObjId} Offs={(ulong)RamOffset} Lng={Size}\r\n    {{\r\n  ".ToArrayPointer());
					VmPrint(GH.VMInstance.PrintBuffer);

					for (Tmp = 0; (Tmp < Size) && (Tmp < Lng) && (Tmp < 1024); Tmp++)
					{
						if ((Tmp & 0x0F) == 0)
						{
							CommonHelper.Snprintf(GH.VMInstance.PrintBuffer, PRINTBUFFERSIZE, $"    {(ulong)(RamOffset + (GBINDEX)Tmp)} ".ToArrayPointer());
							VmPrint(GH.VMInstance.PrintBuffer);
						}
						CommonHelper.Snprintf(GH.VMInstance.PrintBuffer, PRINTBUFFERSIZE, $"{(UBYTE)(Ram.GetUBYTE() & 0xFF)} ".ToArrayPointer());
						VmPrint(GH.VMInstance.PrintBuffer);
						if (((Tmp & 0x0F) == 0xF) && (Tmp < (Size - 1)))
						{
							VmPrint("\r\n    ".ToArrayPointer());
						}
						Ram++;
					}

					VmPrint("\r\n    }\r\n".ToArrayPointer());
				}
			}
		}

		public void BreakPoint()
		{
			throw new NotImplementedException();
		}

		public void BreakSet()
		{
			throw new NotImplementedException();
		}



		public void cBranchJr()
		{
			throw new NotImplementedException();
		}







		public void Do()
		{
			throw new NotImplementedException();
		}













		public void Info()
		{
			throw new NotImplementedException();
		}

		public void MemoryRead()
		{
			throw new NotImplementedException();
		}

		public void MemoryWrite()
		{
			throw new NotImplementedException();
		}

		public void Monitor()
		{
			throw new NotImplementedException();
		}



		public void NoteToFreq()
		{
			throw new NotImplementedException();
		}





		public void PortCnvInput()
		{
			throw new NotImplementedException();
		}

		public void PortCnvOutput()
		{
			throw new NotImplementedException();
		}





		public void ProgramEnd(ushort PrgId)
		{
			throw new NotImplementedException();
		}





		public void Random()
		{
			throw new NotImplementedException();
		}

		public void SetDispatchStatus(DSPSTAT DspStat)
		{
			throw new NotImplementedException();
		}











		public void Strings()
		{
			throw new NotImplementedException();
		}

		public void System()
		{
			throw new NotImplementedException();
		}

		public void Tst()
		{
			throw new NotImplementedException();
		}

		public void TstClose()
		{
			throw new NotImplementedException();
		}

		public RESULT ValidateChar(VarPointer<byte> pChar, sbyte Set)
		{
			throw new NotImplementedException();
		}

		public RESULT ValidateString(ArrayPointer<UBYTE> pString, sbyte Set)
		{
			throw new NotImplementedException();
		}




	}
}
