using Ev3Core.Enums;
using Ev3Core.Extensions;
using Ev3Core.Helpers;
using Ev3Core.Lms2012.Interfaces;
using System.Linq;
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
            pImgHead = (new IMGHEAD()).GetObject(UiImage);
            pImgHead.ImageSize = IMGHEAD.SizeOf;

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
                    GH.VMInstance.pObjHead = pProgram.pObjHeadReal;
                    GH.VMInstance.pObjList = pProgram.pObjListReal;
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

                        GH.VMInstance.Program[PrgId].Objects = (pI.GetObject<IMGHEAD>(new IMGHEAD())).NumberOfObjects;

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

                        pData = pData.Copy((pI.GetObject<IMGHEAD>(new IMGHEAD())).GlobalBytes);

                        // Align & allocate ObjectPointerList (+1)

                        pData = new ArrayPointer<byte>(pData.Data, (pData.Offset + 3) & 0xFFFFFFFC);
                        GH.VMInstance.Program[PrgId].pObjList = pData;
                        pData = pData.Copy(OBJ.Sizeof * (GH.VMInstance.Program[PrgId].Objects + 1));

                        // Make pointer to access object headers starting at one (not zero)

                        GH.VMInstance.Program[PrgId].pObjHead = pI.Copy(IMGHEAD.SizeOf - OBJHEAD.SizeOf);

                        for (ObjIndex = 1; ObjIndex <= GH.VMInstance.Program[PrgId].Objects; ObjIndex++)
                        {
                            // Align

                            pData = new ArrayPointer<byte>(pData.Data, (pData.Offset + 3) & 0xFFFFFFFC);

                            // Save object pointer in Object list

                            GH.VMInstance.Program[PrgId].pObjListReal[ObjIndex] = pData.GetObject<OBJ>(new OBJ());

                            // Initialise instruction pointer, trigger counts and status

#warning anime
                            // TODO: AGAIN THERE IS SO MUCH C SHITE - it won't work
                            GH.VMInstance.Program[PrgId].pObjListReal[ObjIndex].Ip = pI.Copy((ULONG)GH.VMInstance.Program[PrgId].pObjHeadReal[ObjIndex].OffsetToInstructions);

                            GH.VMInstance.Program[PrgId].pObjListReal[ObjIndex].u.TriggerCount = GH.VMInstance.Program[PrgId].pObjHeadReal[ObjIndex].TriggerCount;

                            if ((GH.VMInstance.Program[PrgId].pObjListReal[ObjIndex].u.TriggerCount > 0 || (ObjIndex > 1)))
                            {
                                GH.VMInstance.Program[PrgId].pObjListReal[ObjIndex].ObjStatus = STOPPED;
                            }
                            else
                            {
                                if (Deb == 2)
                                {
                                    GH.VMInstance.Program[PrgId].pObjListReal[ObjIndex].ObjStatus = WAITING;
                                }
                                else
                                {
                                    GH.VMInstance.Program[PrgId].pObjListReal[ObjIndex].ObjStatus = RUNNING;
                                }
                            }

                            if (GH.VMInstance.Program[PrgId].pObjHeadReal[ObjIndex].OwnerObjectId != 0)
                            {
                                GH.VMInstance.Program[PrgId].pObjListReal[ObjIndex].pLocal = GH.VMInstance.Program[PrgId].pObjListReal[GH.VMInstance.Program[PrgId].pObjHeadReal[ObjIndex].OwnerObjectId].Local;
                            }
                            else
                            {
                                GH.VMInstance.Program[PrgId].pObjListReal[ObjIndex].pLocal = GH.VMInstance.Program[PrgId].pObjListReal[ObjIndex].Local;
                            }

                            // Advance data pointer

                            pData = pData.Copy(OBJ.Sizeof + GH.VMInstance.Program[PrgId].pObjHeadReal[ObjIndex].LocalBytes);
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

        public sbyte CheckSdcard(VarPointer<sbyte> pChanged, VarPointer<int> pTotal, VarPointer<int> pFree, sbyte Force)
        {
            throw new NotImplementedException();
        }

        public sbyte CheckUsbstick(VarPointer<sbyte> pChanged, VarPointer<int> pTotal, VarPointer<int> pFree, sbyte Force)
        {
            throw new NotImplementedException();
        }



        public void DefLabel()
        {
            throw new NotImplementedException();
        }

        public void Do()
        {
            throw new NotImplementedException();
        }

        public void Error()
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

        public void Nop()
        {
            throw new NotImplementedException();
        }

        public void NoteToFreq()
        {
            throw new NotImplementedException();
        }

        public void ObjectCall()
        {
            throw new NotImplementedException();
        }

        public void ObjectEnd()
        {
            throw new NotImplementedException();
        }

        public void ObjectReturn()
        {
            throw new NotImplementedException();
        }

        public void ObjectStart()
        {
            throw new NotImplementedException();
        }

        public void ObjectStop()
        {
            throw new NotImplementedException();
        }

        public void ObjectTrig()
        {
            throw new NotImplementedException();
        }

        public void ObjectWait()
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

        public void PrimParAdvance()
        {
            throw new NotImplementedException();
        }

        public void Probe()
        {
            throw new NotImplementedException();
        }

        public void ProgramEnd(ushort PrgId)
        {
            throw new NotImplementedException();
        }

        public void ProgramInfo()
        {
            throw new NotImplementedException();
        }

        public void ProgramStart()
        {
            throw new NotImplementedException();
        }



        public void ProgramStop()
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









        public void Sleep()
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
