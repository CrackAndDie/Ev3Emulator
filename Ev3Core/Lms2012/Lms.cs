using Ev3Core.Enums;
using Ev3Core.Lms2012.Interfaces;

namespace Ev3Core.Lms2012
{
    public class Lms : ILms
    {
        public void BreakPoint()
        {
            throw new NotImplementedException();
        }

        public void BreakSet()
        {
            throw new NotImplementedException();
        }

		public ushort CallingObjectId()
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

		public ushort CurrentProgramId()
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

		public DSPSTAT ExecuteByteCode(byte[] pByteCode, byte[] pGlobals, byte[] pLocals)
		{
			throw new NotImplementedException();
		}

		public byte[] GetObjectIp()
		{
			throw new NotImplementedException();
		}

		public int GetObjectIpInd()
		{
			throw new NotImplementedException();
		}

		public sbyte GetSleepMinutes()
		{
			throw new NotImplementedException();
		}

		public sbyte GetTerminalEnable()
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

		public object PrimParPointer()
		{
			throw new NotImplementedException();
		}

		public void PrimParPointer(object data)
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

		public OBJSTAT ProgramStatus(ushort PrgId)
		{
			throw new NotImplementedException();
		}

		public OBJSTAT ProgramStatusChange(ushort PrgId)
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

		public void SetDispatchStatus(int DspStat)
		{
			throw new NotImplementedException();
		}

		public void SetObjectIp(byte[] Ip)
		{
			throw new NotImplementedException();
		}

		public void SetObjectIpInd(int ind)
		{
			throw new NotImplementedException();
		}

		public void SetSleepMinutes(sbyte Minutes)
		{
			throw new NotImplementedException();
		}

		public void SetTerminalEnable(sbyte Value)
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

		public object[] VmMemoryResize(short Handle, int Elements)
		{
			throw new NotImplementedException();
		}
	}
}
