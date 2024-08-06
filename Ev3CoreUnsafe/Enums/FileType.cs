namespace Ev3CoreUnsafe.Enums
{
	public enum FILETYPE
	{
		FILETYPE_UNKNOWN = 0x00,
		TYPE_FOLDER = 0x01,
		TYPE_SOUND = 0x02,
		TYPE_BYTECODE = 0x03,
		TYPE_GRAPHICS = 0x04,
		TYPE_DATALOG = 0x05,
		TYPE_PROGRAM = 0x06,
		TYPE_TEXT = 0x07,
		TYPE_SDCARD = 0x10,
		TYPE_USBSTICK = 0x20,

		FILETYPES,                                //!< Maximum icon types supported by the VM

		TYPE_RESTART_BROWSER = -1,
		TYPE_REFRESH_BROWSER = -2
	}
}

namespace Ev3CoreUnsafe
{
	public partial class Defines
	{
		public const int FILETYPE_UNKNOWN = 0x00;
		public const int TYPE_FOLDER = 0x01;
		public const int TYPE_SOUND = 0x02;
		public const int TYPE_BYTECODE = 0x03;
		public const int TYPE_GRAPHICS = 0x04;
		public const int TYPE_DATALOG = 0x05;
		public const int TYPE_PROGRAM = 0x06;
		public const int TYPE_TEXT = 0x07;
		public const int TYPE_SDCARD = 0x10;
		public const int TYPE_USBSTICK = 0x20;

		public const int FILETYPES = 33;                                //!< Maximum icon types supported by the VM

		public const int TYPE_RESTART_BROWSER = -1;
		public const int TYPE_REFRESH_BROWSER = -2;
	}
}