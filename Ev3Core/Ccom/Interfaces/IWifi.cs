using Ev3Core.Enums;
using Ev3Core.Helpers;
using static Ev3Core.Defines;

namespace Ev3Core.Ccom.Interfaces
{
	public interface IWifi
	{
		// Common Network stuff
		// --------------------

		RESULT cWiFiGetIpAddr(ArrayPointer<UBYTE> IpAddress);

		RESULT cWiFiGetMyMacAddr(ArrayPointer<UBYTE> MacAddress);

		RESULT cWiFiKnownDongleAttached();      // Known H/W

		//UDP functions
		//-------------

		RESULT cWiFiTxingBeacons();        // Are we active in tx beacons??

		void cWiFiUdpClientClose();

		// TCP functions
		// -------------

		RESULT cWiFiTcpConnected();         // TCP connection established?

		//RESULT cWiFiTcpTempClose(void);

		UWORD cWiFiWriteTcp(ArrayPointer<UBYTE> Buffer, UWORD Length);

		UWORD cWiFiReadTcp(ArrayPointer<UBYTE> Buffer, UWORD Length);

		// WPA and AP stuff
		// ----------------
		void cWiFiMoveUpInList(int Index);      // Direct UI function

		void cWiFiMoveDownInList(int Index);    // Direct UI function

		void cWiFiDeleteInList(int Index);      // Direct UI function

		RESULT cWiFiGetApMacAddr(ArrayPointer<UBYTE> MacAddr, int Index);

		RESULT cWiFiGetHiddenMacAddr(ArrayPointer<UBYTE> MacAddr, int Index);

		RESULT cWiFiGetName(ArrayPointer<UBYTE> ApName, int Index, DATA8 Length);  // Get the FriendlyName owned by ApTable[Index]

		RESULT cWiFiSetName(ArrayPointer<UBYTE> ApName, int Index);  // Set the FriendlyName @ ApTable[Index]  // Hidden!?

		RESULT cWiFiSetSsid(ArrayPointer<UBYTE> Ssid);

		RESULT cWiFiSetKeyManagToWpa2();

		RESULT cWiFiSetKeyManagToNone();

		RESULT cWiFiGetIndexFromName(ArrayPointer<UBYTE> Name, VarPointer<UBYTE> Index);

		void cWiFiSetEncryptToWpa2(int Index);

		void cWiFiSetEncryptToNone(int Index);

		void cWiFiSetKnown(int Index);

		void cWiFiDeleteAsKnown(int LocalIndex);

		byte cWiFiGetFlags(int Index);  // Get Flags owned by ApTable[Index]

		RESULT cWiFiConnectToAp(int Index);

		RESULT cWiFiMakePsk(ArrayPointer<UBYTE> ApSsid, ArrayPointer<UBYTE> PassPhrase, int Index);

		int cWiFiGetApListSize();

		void cWiFiIncApListSize();

		RESULT cWiFiAddHidden(ArrayPointer<UBYTE> HiddenApName, ArrayPointer<UBYTE> Security, ArrayPointer<UBYTE> PassWord);

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

	public class aps
	{
		public char[] mac_address = CommonHelper.Array1d<char>(MAC_ADDRESS_LENGTH);     // as it tells
		public char[] frequency = CommonHelper.Array1d<char>(FREQUENCY_LENGTH);         // additional info - not used
		public char[] signal_level = CommonHelper.Array1d<char>(SIGNAL_LEVEL_LENGTH);   // -
		public char[] security = CommonHelper.Array1d<char>(SECURITY_LENGTH);          // Only WPA2 or NONE
		public char[] friendly_name = CommonHelper.Array1d<char>(FRIENDLY_NAME_LENGTH); // The name, the user will see aka SSID
		public char[] key_management = CommonHelper.Array1d<char>(KEY_MGMT_LENGTH);
		public char[] pre_shared_key = CommonHelper.Array1d<char>(PSK_LENGTH);          // Preshared Key (Encryption)
		public char[] pairwise_ciphers = CommonHelper.Array1d<char>(PAIRWISE_LENGTH);
		public char[] group_ciphers = CommonHelper.Array1d<char>(GROUP_LENGTH);
		public char[] proto = CommonHelper.Array1d<char>(PROTO_LENGTH);
		public byte ap_flags;                   // Holds the capabilities etc. of the AP
	}
}
