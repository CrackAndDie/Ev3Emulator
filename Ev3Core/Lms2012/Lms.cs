using Ev3Core.Enums;
using Ev3Core.Extensions;
using Ev3Core.Lms2012.Interfaces;
using System.Linq;
using static Ev3Core.Defines;

namespace Ev3Core.Lms2012
{
    public class Lms : ILms
    {
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

        void VmPrint(char[] pString)
        {
            if (GH.VMInstance.TerminalEnabled != 0)
            {
                // TODO: print to terminal
                // printf("%s", pString);
            }
        }

        public void SetTerminalEnable(sbyte Value)
        {
            GH.VMInstance.TerminalEnabled = Value;
        }

        public sbyte GetTerminalEnable()
        {
            return (GH.VMInstance.TerminalEnabled);
        }

        public void GetResourcePath(char[] pString, sbyte MaxLength)
        {
            GH.Memory.cMemoryGetResourcePath(GH.VMInstance.ProgramId, pString.ToSbyteArray(), MaxLength);
        }

        public byte[] VmMemoryResize(short Handle, int Elements)
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
                    GH.Memory.cMemoryArraryPointer(GH.VMInstance.ProgramId, (short)GH.VMInstance.Handle.Offset, out primParResult.Data);
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

		public sbyte CheckSdcard(ref sbyte pChanged, ref int pTotal, ref int pFree, sbyte Force)
		{
			throw new NotImplementedException();
		}

		public sbyte CheckUsbstick(ref sbyte pChanged, ref int pTotal, ref int pFree, sbyte Force)
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

		public RESULT ValidateChar(ref sbyte pChar, sbyte Set)
		{
			throw new NotImplementedException();
		}

		public RESULT ValidateString(sbyte[] pString, sbyte Set)
		{
			throw new NotImplementedException();
		}

		


	}
}
