using Ev3CoreUnsafe.Enums;
using Ev3CoreUnsafe.Helpers;
using Ev3CoreUnsafe.Lms2012.Interfaces;
using System.Runtime.InteropServices;
using static Ev3CoreUnsafe.Defines;

namespace Ev3CoreUnsafe.Cinput.Interfaces
{
	public unsafe interface IInput
	{
		RESULT cInputInit();

		RESULT cInputOpen();

		RESULT cInputClose();

		RESULT cInputExit();

		// void cInputChar(DATA8 Char); - probably no need

		void cInputUpdate(UWORD Time);

		RESULT cInputCompressDevice(DATA8* pDevice, UBYTE Layer, UBYTE Port);

		RESULT cInputGetDeviceData(DATA8 Layer, DATA8 Port, DATA8 Length, DATA8* pType, DATA8* pMode, DATA8* pData);

		RESULT cInputSetChainedDeviceType(DATA8 Layer, DATA8 Port, DATA8 Type, DATA8 Mode);

		void cInputDeviceList();

		RESULT cInputStartTypeDataUpload();


		void cInputDevice();

		void cInputRead();

		void cInputReadSi();

		void cInputReadExt();

		void cInputTest();

		void cInputReady();

		void cInputWrite();

		void cInputSample();
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct DEVICE
	{
		public UWORD InvalidTime;                        //!< mS from type change to valid data
		public UWORD TimeoutTimer;                       //!< mS allowed to be busy timer
		public UWORD TypeIndex;                          //!< Index to information in "TypeData" table
		public DATA8 Connection;                         //!< Connection type (from DCM)
		public OBJID Owner;
		public RESULT DevStatus;
		public DATA8 Busy;
		public fixed DATAF Raw[MAX_DEVICE_DATASETS];           //!< Raw value (only updated when "cInputReadDeviceRaw" function is called)
		public DATAF OldRaw;
		public DATA32 Changes;
		public DATA32 Bumps;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct CALIB
	{
		public DATA8 InUse;
		public DATAF Min;
		public DATAF Max;
	}

	public unsafe struct INPUT_GLOBALS
	{
		//*****************************************************************************
		// Input Global variables
		//*****************************************************************************

		// UBYTE Data[MAX_DEVICE_DATALENGTH];

		public ANALOG Analog;
		public ANALOG* pAnalog;

		public UART Uart;
		public UART* pUart;

		public IIC Iic;
		public IIC* pIic;

		public int UartFile;
		public int AdcFile;
		public int DcmFile;
		public int IicFile;

		public DEVCON DevCon;
		public UARTCTL UartCtl;
		public IICCTL IicCtl;
		public IICDAT IicDat;

		public DATA32 InputNull;

		public DATA8* TmpMode;

		public DATA8* ConfigurationChanged;

		public DATA8* DeviceType;              //!< Type of all devices - for easy upload
		public DATA8* DeviceMode;              //!< Mode of all devices
		public DEVICE* DeviceData;              //!< Data for all devices

		public UWORD NoneIndex;
		public UWORD UnknownIndex;
		public DATA8 DCMUpdate;

		public DATA8* TypeModes;   //!< No of modes for specific type

		public UWORD MaxDeviceTypes;                   //!< Number of device type/mode entries in tabel
		public TYPES* TypeData;                        //!< Type specific data
		public UWORD IicDeviceTypes;                   //!< Number of IIC device type/mode entries in tabel
		public IICSTR* IicString;
		public IICSTR IicStr;

		public DATA16 TypeDataTimer;
		public DATA16 TypeDataIndex;

		public CALIB** Calib;

		public INPUT_GLOBALS()
		{
			Init();
		}

		public void Init()
		{
			// structs
			Analog = *CommonHelper.PointerStruct<ANALOG>();
			Uart = *CommonHelper.PointerStruct<UART>();
			Iic = *CommonHelper.PointerStruct<IIC>();
			DevCon = *CommonHelper.PointerStruct<DEVCON>();
			UartCtl = *CommonHelper.PointerStruct<UARTCTL>();
			IicCtl = *CommonHelper.PointerStruct<IICCTL>();
			IicDat = *CommonHelper.PointerStruct<IICDAT>();
			IicStr = *CommonHelper.PointerStruct<IICSTR>();

			TmpMode = CommonHelper.Pointer1d<DATA8>(INPUT_PORTS);
			ConfigurationChanged = CommonHelper.Pointer1d<DATA8>(MAX_PROGRAMS);
			DeviceType = CommonHelper.Pointer1d<DATA8>(DEVICES);
			DeviceMode = CommonHelper.Pointer1d<DATA8>(DEVICES);
			DeviceData = CommonHelper.Pointer1d<DEVICE>(DEVICES, true);
			TypeModes = CommonHelper.Pointer1d<DATA8>(MAX_DEVICE_TYPE + 1);
			Calib = CommonHelper.Pointer2d<CALIB>(MAX_DEVICE_TYPE, MAX_DEVICE_MODES, true);
		}
	}
}
