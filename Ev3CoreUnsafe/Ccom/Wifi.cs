﻿using Ev3CoreUnsafe.Ccom.Interfaces;
using Ev3CoreUnsafe.Enums;

namespace Ev3CoreUnsafe.Ccom
{
    public unsafe class Wifi : IWifi
    {
        public RESULT cWiFiAddHidden(sbyte* HiddenApName, sbyte* Security, sbyte* PassWord)
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
            throw new NotImplementedException();
        }

        public int cWiFiGetApListSize()
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiGetApMacAddr(sbyte* MacAddr, int Index)
        {
            throw new NotImplementedException();
        }

        public byte cWiFiGetFlags(int Index)
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiGetHiddenMacAddr(sbyte* MacAddr, int Index)
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiGetIndexFromName(sbyte* Name, byte* Index)
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiGetIpAddr(sbyte* IpAddress)
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiGetMyMacAddr(sbyte* MacAddress)
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiGetName(sbyte* ApName, int Index, sbyte Length)
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
            throw new NotImplementedException();
        }

        public RESULT cWiFiKnownDongleAttached()
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiMakePsk(sbyte* ApSsid, sbyte* PassPhrase, int Index)
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

        public ushort cWiFiReadTcp(byte* Buffer, ushort Length)
        {
            throw new NotImplementedException();
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

        public RESULT cWiFiSetName(sbyte* ApName, int Index)
        {
            throw new NotImplementedException();
        }

        public RESULT cWiFiSetSsid(sbyte* Ssid)
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

        public ushort cWiFiWriteTcp(byte* Buffer, ushort Length)
        {
            throw new NotImplementedException();
        }
    }
}
