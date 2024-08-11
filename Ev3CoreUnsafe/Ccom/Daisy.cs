using Ev3CoreUnsafe.Ccom.Interfaces;
using Ev3CoreUnsafe.Enums;

namespace Ev3CoreUnsafe.Ccom
{
    public unsafe class Daisy : IDaisy
    {
        public RESULT cDaisyChained()
        {
            throw new NotImplementedException();
        }

        public byte cDaisyCheckBusyBit(byte Layer, byte PortBits)
        {
            throw new NotImplementedException();
        }

        public RESULT cDaisyCheckBusyIndex(byte Layer, byte Port)
        {
            throw new NotImplementedException();
        }

        public RESULT cDaisyClose()
        {
            throw new NotImplementedException();
        }

        public void cDaisyCmd(RXBUF* pRxBuf, TXBUF* pTxBuf)
        {
            throw new NotImplementedException();
        }

        public void cDaisyControl()
        {
            throw new NotImplementedException();
        }

        public ushort cDaisyData(byte** pData)
        {
            throw new NotImplementedException();
        }

        public RESULT cDaisyDownStreamCmd(sbyte* pData, sbyte Length, sbyte Layer)
        {
            throw new NotImplementedException();
        }

        public RESULT cDaisyExit()
        {
            throw new NotImplementedException();
        }

        public RESULT cDaisyGetDeviceInfo(sbyte Length, byte* pInfo)
        {
            throw new NotImplementedException();
        }

        public RESULT cDaisyGetDownstreamData(sbyte Layer, sbyte Sensor, sbyte Length, sbyte* pType, sbyte* pMode, sbyte* pData)
        {
            throw new NotImplementedException();
        }

        public int cDaisyGetInterruptPacketSize()
        {
            throw new NotImplementedException();
        }

        public int cDaisyGetLastWriteResult()
        {
            throw new NotImplementedException();
        }

        public int cDaisyGetLastWriteState()
        {
            throw new NotImplementedException();
        }

        public byte cDaisyGetOwnLayer()
        {
            throw new NotImplementedException();
        }

        public sbyte cDaisyGetUsbUpStreamSpeed()
        {
            throw new NotImplementedException();
        }

        public RESULT cDaisyInit()
        {
            throw new NotImplementedException();
        }

        public RESULT cDaisyMotorDownStream(sbyte* pData, sbyte Length, sbyte Layer, sbyte PortField)
        {
            throw new NotImplementedException();
        }

        public RESULT cDaisyOpen()
        {
            throw new NotImplementedException();
        }

        public void cDaisyPollFromDownstream()
        {
            throw new NotImplementedException();
        }

        public void cDaisyPrepareNext()
        {
            throw new NotImplementedException();
        }

        public void cDaisyPushUpStream()
        {
            throw new NotImplementedException();
        }

        public void cDaisyReadFromDownStream()
        {
            throw new NotImplementedException();
        }

        public RESULT cDaisyReady()
        {
            throw new NotImplementedException();
        }

        public void cDaisySetBusyFlags(byte Layer, byte Port, byte MagicCookie)
        {
            throw new NotImplementedException();
        }

        public RESULT cDaisySetDeviceInfo(sbyte Length, byte* pInfo)
        {
            throw new NotImplementedException();
        }

        public RESULT cDaisySetDeviceType(sbyte Layer, sbyte Port, sbyte Type, sbyte Mode)
        {
            throw new NotImplementedException();
        }

        public void cDaisySetOwnLayer(byte Layer)
        {
            throw new NotImplementedException();
        }

        public void cDaisySetTimeout(int TimeOut)
        {
            throw new NotImplementedException();
        }

        public void cDaisyStuffTxed()
        {
            throw new NotImplementedException();
        }

        public int cDaisyWrite()
        {
            throw new NotImplementedException();
        }

        public int cDaisyWriteDone()
        {
            throw new NotImplementedException();
        }

        public void DecrementDaisyPushCounter()
        {
            throw new NotImplementedException();
        }

        public uint GetDaisyPushCounter()
        {
            throw new NotImplementedException();
        }

        public int GetSlaveUnlocked()
        {
            throw new NotImplementedException();
        }

        public int GetUnlocked()
        {
            throw new NotImplementedException();
        }

        public void ResetDaisyPushCounter()
        {
            throw new NotImplementedException();
        }

        public void SetSlaveUnlocked(int Status)
        {
            throw new NotImplementedException();
        }

        public void SetUnlocked(int Status)
        {
            throw new NotImplementedException();
        }
    }
}
