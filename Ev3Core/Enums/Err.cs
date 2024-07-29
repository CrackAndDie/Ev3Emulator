namespace Ev3Core.Enums
{
	public enum ERR
	{
		TOO_MANY_ERRORS_TO_BUFFER,
		TYPEDATA_TABEL_FULL,
		TYPEDATA_FILE_NOT_FOUND,
		ANALOG_DEVICE_FILE_NOT_FOUND,
		ANALOG_SHARED_MEMORY,
		UART_DEVICE_FILE_NOT_FOUND,
		UART_SHARED_MEMORY,
		IIC_DEVICE_FILE_NOT_FOUND,
		IIC_SHARED_MEMORY,
		DISPLAY_SHARED_MEMORY,
		UI_SHARED_MEMORY,
		UI_DEVICE_FILE_NOT_FOUND,
		LCD_DEVICE_FILE_NOT_FOUND,
		OUTPUT_SHARED_MEMORY,
		COM_COULD_NOT_OPEN_FILE,
		COM_NAME_TOO_SHORT,
		COM_NAME_TOO_LONG,
		COM_INTERNAL,
		VM_INTERNAL,
		VM_PROGRAM_VALIDATION,
		VM_PROGRAM_NOT_STARTED,
		VM_PROGRAM_FAIL_BREAK,
		VM_PROGRAM_INSTRUCTION_BREAK,
		VM_PROGRAM_NOT_FOUND,
		SOUND_DEVICE_FILE_NOT_FOUND,
		SOUND_SHARED_MEMORY,
		FILE_OPEN_ERROR,
		FILE_READ_ERROR,
		FILE_WRITE_ERROR,
		FILE_CLOSE_ERROR,
		FILE_GET_HANDLE_ERROR,
		FILE_NAME_ERROR,
		USB_SHARED_MEMORY,
		OUT_OF_MEMORY,
		ERRORS
	}
}

namespace Ev3Core
{
	public partial class Defines
	{
		public const int TOO_MANY_ERRORS_TO_BUFFER = 0;
		public const int TYPEDATA_TABEL_FULL = 1;
		public const int TYPEDATA_FILE_NOT_FOUND = 2;
		public const int ANALOG_DEVICE_FILE_NOT_FOUND = 3;
		public const int ANALOG_SHARED_MEMORY = 4;
		public const int UART_DEVICE_FILE_NOT_FOUND = 5;
		public const int UART_SHARED_MEMORY = 6;
		public const int IIC_DEVICE_FILE_NOT_FOUND = 7;
		public const int IIC_SHARED_MEMORY = 8;
		public const int DISPLAY_SHARED_MEMORY = 9;
		public const int UI_SHARED_MEMORY = 10;
		public const int UI_DEVICE_FILE_NOT_FOUND = 11;
		public const int LCD_DEVICE_FILE_NOT_FOUND = 12;
		public const int OUTPUT_SHARED_MEMORY = 13;
		public const int COM_COULD_NOT_OPEN_FILE = 14;
		public const int COM_NAME_TOO_SHORT = 15;
		public const int COM_NAME_TOO_LONG = 16;
		public const int COM_INTERNAL = 17;
		public const int VM_INTERNAL = 18;
		public const int VM_PROGRAM_VALIDATION = 19;
		public const int VM_PROGRAM_NOT_STARTED = 20;
		public const int VM_PROGRAM_FAIL_BREAK = 21;
		public const int VM_PROGRAM_INSTRUCTION_BREAK = 22;
		public const int VM_PROGRAM_NOT_FOUND = 23;
		public const int SOUND_DEVICE_FILE_NOT_FOUND = 24;
		public const int SOUND_SHARED_MEMORY = 25;
		public const int FILE_OPEN_ERROR = 26;
		public const int FILE_READ_ERROR = 27;
		public const int FILE_WRITE_ERROR = 28;
		public const int FILE_CLOSE_ERROR = 29;
		public const int FILE_GET_HANDLE_ERROR = 30;
		public const int FILE_NAME_ERROR = 31;
		public const int USB_SHARED_MEMORY = 32;
		public const int OUT_OF_MEMORY = 33;
		public const int ERRORS = 34;               //!< Break because of fail
	}
}

