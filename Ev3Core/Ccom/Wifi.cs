using Ev3Core.Ccom.Interfaces;
using Ev3Core.Enums;

namespace Ev3Core.Ccom
{
    // useful probably from this line https://github.com/mindboards/ev3sources/blob/78ebaf5b6f8fe31cc17aa5dce0f8e4916a4fc072/lms2012/c_com/source/c_wifi.c#L2627
    public class Wifi : IWifi
    {
        public RESULT cWiFiAddHidden(ArrayPointer<UBYTE> HiddenApName, ArrayPointer<UBYTE> Security, ArrayPointer<UBYTE> PassWord)
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiConnectToAp(int Index)
        {
            throw new NotImplementedException();
        }

        public void cWiFiControl()
        {
            throw new NotImplementedException();
        }

        public void cWiFiDeleteAsKnown(int LocalIndex)
        {
            throw new NotImplementedException();
        }

        public void cWiFiDeleteInList(int Index)
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiExit()
        {
            // TODO: impl this
            return RESULT.OK;
        }

        public int cWiFiGetApListSize()
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiGetApMacAddr(ArrayPointer<UBYTE> MacAddr, int Index)
        {
            throw new NotImplementedException();
        }

        public byte cWiFiGetFlags(int Index)
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiGetHiddenMacAddr(ArrayPointer<UBYTE> MacAddr, int Index)
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiGetIndexFromName(ArrayPointer<UBYTE> Name, VarPointer<byte> Index)
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiGetIpAddr(ArrayPointer<UBYTE> IpAddress)
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiGetMyMacAddr(ArrayPointer<UBYTE> MacAddress)
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiGetName(ArrayPointer<UBYTE> ApName, int Index, DATA8 Length)
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiGetOnStatus()
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiGetStatus()
        {
            throw new NotImplementedException();
        }

        public void cWiFiIncApListSize()
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiInit()
        {
            // TODO: impl this
            return RESULT.OK;
        }

        public RESULT cWiFiKnownDongleAttached()
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiMakePsk(ArrayPointer<UBYTE> ApSsid, ArrayPointer<UBYTE> PassPhrase, int Index)
        {
            throw new NotImplementedException();
        }

        public void cWiFiMoveDownInList(int Index)
        {
            throw new NotImplementedException();
        }

        public void cWiFiMoveUpInList(int Index)
        {
            throw new NotImplementedException();
        }

        public ushort cWiFiReadTcp(ArrayPointer<UBYTE> Buffer, ushort Length)
        {
            // TODO: impl this
            return 0;
        }

        public RESULT cWiFiScanForAPs()
        {
            throw new NotImplementedException();
        }

        public void cWiFiSetBrickName()
        {
            throw new NotImplementedException();
        }

        public void cWiFiSetBtSerialNo()
        {
            throw new NotImplementedException();
        }

        public void cWiFiSetEncryptToNone(int Index)
        {
            throw new NotImplementedException();
        }

        public void cWiFiSetEncryptToWpa2(int Index)
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiSetKeyManagToNone()
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiSetKeyManagToWpa2()
        {
            throw new NotImplementedException();
        }

        public void cWiFiSetKnown(int Index)
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiSetName(ArrayPointer<UBYTE> ApName, int Index)
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiSetSsid(ArrayPointer<UBYTE> Ssid)
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiTcpConnected()
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiTurnOff()
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiTurnOn()
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiTxingBeacons()
        {
            throw new NotImplementedException();
        }

        public void cWiFiUdpClientClose()
        {
            throw new NotImplementedException();
        }

        public ushort cWiFiWriteTcp(ArrayPointer<UBYTE> Buffer, ushort Length)
        {
            // TODO: impl this
            return 0;
        }
    }
}
