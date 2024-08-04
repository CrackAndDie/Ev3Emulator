using Ev3Core.Enums;
using Ev3Core.Helpers;
using Ev3Core.Lms2012.Interfaces;
using static Ev3Core.Defines;

namespace Ev3Core.Cinput.Interfaces
{
    public interface IInput
    {
        RESULT cInputInit();

        RESULT cInputOpen();

        RESULT cInputClose();

        RESULT cInputExit();

        void cInputChar(DATA8 Char);

        void cInputUpdate(UWORD Time);

        RESULT cInputCompressDevice(VarPointer<DATA8> pDevice, UBYTE Layer, UBYTE Port);

        RESULT cInputGetDeviceData(DATA8 Layer, DATA8 Port, DATA8 Length, VarPointer<DATA8> pType, VarPointer<DATA8> pMode, ArrayPointer<UBYTE> pData);

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

    public class DEVICE
    {
        public UWORD InvalidTime;                        //!< mS from type change to valid data
        public UWORD TimeoutTimer;                       //!< mS allowed to be busy timer
        public UWORD TypeIndex;                          //!< Index to information in "TypeData" table
        public UBYTE Connection;                         //!< Connection type (from DCM)
        public OBJID Owner;
        public RESULT DevStatus;
        public DATA8 Busy;
        public DATAF[] Raw = CommonHelper.Array1d<DATAF>(MAX_DEVICE_DATASETS);           //!< Raw value (only updated when "cInputReadDeviceRaw" function is called)

        public DATAF OldRaw;
        public DATA32 Changes;
        public DATA32 Bumps;

        public UWORD Timer;
        public UBYTE Dir;
    }

    public class CALIB
    {
        public DATA8 InUse;
        public DATAF Min;
        public DATAF Max;
    }

    public class INPUT_GLOBALS
    {
        //*****************************************************************************
        // Input Global variables
        //*****************************************************************************

        public UBYTE[] Data = CommonHelper.Array1d<UBYTE>(MAX_DEVICE_DATALENGTH);

        public ANALOG Analog = new ANALOG();

        public UART Uart = new UART();

        public IIC Iic = new IIC();

        public int UartFile;
        public int AdcFile;
        public int DcmFile;
        public int IicFile;

        public DEVCON DevCon = new DEVCON();
        public UARTCTL UartCtl = new UARTCTL();
        public IICCTL IicCtl = new IICCTL();
        public IICDAT IicDat = new IICDAT();

        public DATA32 InputNull;

        public ArrayPointer<UBYTE> TmpMode = new ArrayPointer<UBYTE>(CommonHelper.Array1d<UBYTE>(INPUT_PORTS));

        public ArrayPointer<UBYTE> ConfigurationChanged = new ArrayPointer<UBYTE>(CommonHelper.Array1d<UBYTE>(MAX_PROGRAMS));

        public ArrayPointer<UBYTE> DeviceType = new ArrayPointer<UBYTE>(CommonHelper.Array1d<UBYTE>(DEVICES));              //!< Type of all devices - for easy upload
        public ArrayPointer<UBYTE> DeviceMode = new ArrayPointer<UBYTE>(CommonHelper.Array1d<UBYTE>(DEVICES));              //!< Mode of all devices
        public DEVICE[] DeviceData = CommonHelper.Array1d<DEVICE>(DEVICES, true);              //!< Data for all devices

        public UWORD NoneIndex;
        public UWORD UnknownIndex;
        public DATA8 DCMUpdate;

        public ArrayPointer<UBYTE> TypeModes =  new ArrayPointer<UBYTE>(CommonHelper.Array1d<UBYTE>(MAX_DEVICE_TYPE + 1));   //!< No of modes for specific type

        public UWORD MaxDeviceTypes;                   //!< Number of device type/mode entries in tabel
        public TYPES[] TypeData;                        //!< Type specific data
        public UWORD IicDeviceTypes;                   //!< Number of IIC device type/mode entries in tabel
        public IICSTR[] IicString;
        public IICSTR IicStr = new IICSTR();

        public DATA16 TypeDataTimer;
        public DATA16 TypeDataIndex;

        public CALIB[][] Calib = CommonHelper.Array2d<CALIB>(MAX_DEVICE_TYPE, MAX_DEVICE_MODES, true);
    }
}
