using System.Runtime.InteropServices;

namespace Ev3Emulator.LowLevel
{
	public static class ButtonsWrapper
	{
		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_button_getPressed(reg_w_button_getPressedAction getPressed);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate IntPtr reg_w_button_getPressedAction();

		// up enter down right left back
		public static void Init(Func<byte[]> getPressed)
		{
			_getPressed = getPressed;
			_buttonsPointer = Marshal.AllocHGlobal(6); // for 6 buttons

			reg_w_button_getPressed(GetPressed);
		}

		private static IntPtr GetPressed()
		{
			var bytes = _getPressed.Invoke();
			Marshal.Copy(bytes, 0, _buttonsPointer, bytes.Length);
			return _buttonsPointer;
		}

		private static Func<byte[]> _getPressed;
		private static IntPtr _buttonsPointer;
	}
}
