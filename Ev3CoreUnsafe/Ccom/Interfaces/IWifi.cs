using Ev3CoreUnsafe.Enums;

namespace Ev3CoreUnsafe.Ccom.Interfaces
{
	public unsafe interface IWifi
	{
		RESULT cWiFiGetIpAddr(DATA8* IpAddress);

		RESULT cWiFiGetMyMacAddr(DATA8* MacAddress);

		RESULT cWiFiKnownDongleAttached();      // Known H/W

		//UDP functions
		//-------------

		RESULT cWiFiTxingBeacons();        // Are we active in tx beacons??

		void cWiFiUdpClientClose();

		// TCP functions
		// -------------

		RESULT cWiFiTcpConnected();         // TCP connection established?

		//RESULT cWiFiTcpTempClose(void);

		UWORD cWiFiWriteTcp(UBYTE* Buffer, UWORD Length);

		UWORD cWiFiReadTcp(UBYTE* Buffer, UWORD Length);

		// WPA and AP stuff
		// ----------------
		void cWiFiMoveUpInList(int Index);      // Direct UI function

		void cWiFiMoveDownInList(int Index);    // Direct UI function

		void cWiFiDeleteInList(int Index);      // Direct UI function

		RESULT cWiFiGetApMacAddr(DATA8* MacAddr, int Index);

		RESULT cWiFiGetHiddenMacAddr(DATA8* MacAddr, int Index);

		RESULT cWiFiGetName(DATA8* ApName, int Index, DATA8 Length);  // Get the FriendlyName owned by ApTable[Index]

		RESULT cWiFiSetName(DATA8* ApName, int Index);  // Set the FriendlyName @ ApTable[Index]  // Hidden!?

		RESULT cWiFiSetSsid(DATA8* Ssid);

		RESULT cWiFiSetKeyManagToWpa2();

		RESULT cWiFiSetKeyManagToNone();

		RESULT cWiFiGetIndexFromName(DATA8* Name, UBYTE* Index);

		void cWiFiSetEncryptToWpa2(int Index);

		void cWiFiSetEncryptToNone(int Index);

		void cWiFiSetKnown(int Index);

		void cWiFiDeleteAsKnown(int LocalIndex);

		byte cWiFiGetFlags(int Index);  // Get Flags owned by ApTable[Index]

		RESULT cWiFiConnectToAp(int Index);

		RESULT cWiFiMakePsk(DATA8* ApSsid, DATA8* PassPhrase, int Index);

		int cWiFiGetApListSize();

		void cWiFiIncApListSize();

		RESULT cWiFiAddHidden(DATA8* HiddenApName, DATA8* Security, DATA8* PassWord);

		RESULT cWiFiScanForAPs();

		RESULT cWiFiGetOnStatus();

		// Common Control
		// --------------

		RESULT cWiFiGetStatus();

		void cWiFiSetBtSerialNo();

		void cWiFiSetBrickName();

		void cWiFiControl();

		RESULT cWiFiTurnOn();        // TURN ON

		RESULT cWiFiTurnOff();       // TURN OFF

		RESULT cWiFiExit();

		RESULT cWiFiInit();
	}
}
