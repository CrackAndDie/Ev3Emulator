using Ev3Core.Enums;

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

		RESULT cInputCompressDevice(ref DATA8 pDevice, UBYTE Layer, UBYTE Port);

		RESULT cInputGetDeviceData(DATA8 Layer, DATA8 Port, DATA8 Length, ref DATA8 pType, ref DATA8 pMode, DATA8[] pData);

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
}
