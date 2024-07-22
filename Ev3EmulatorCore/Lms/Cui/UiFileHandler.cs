namespace Ev3EmulatorCore.Lms.Cui
{
	// this is a pseudo UI file
	// control files like /dev/tty0
	public class UiFileHandler
	{
		public const int BUTTON_FILE_HANDLER = 1;
		public const int LED_LEFT_RED_FILE_HANDLER = 2;
		public const int LED_RIGHT_RED_FILE_HANDLER = 3;
		public const int LED_LEFT_GREEN_FILE_HANDLER = 4;
		public const int LED_RIGHT_GREEN_FILE_HANDLER = 5;

		public const int UI_FILE_HANDLER = 6;
		public const int POWER_FILE_HANDLER = 7;
		public const int ADC_FILE_HANDLER = 8;

		public int Write(int handler, byte[] data, int size)
		{
			// TODO
			return 0;
		}

		public bool IsButtonPressed(int button)
		{
			// TODO
			return false;
		}

		public void OnExit()
		{
			// do something
		}
	}
}
