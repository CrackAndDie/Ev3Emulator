using System.Runtime.InteropServices;

namespace Ev3Emulator.LowLevel
{
	public static class ButtonsWrapper
	{
		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_button_getPressed(reg_w_button_getPressedAction getPressed);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate IntPtr reg_w_button_getPressedAction();

		public static void Init(reg_w_button_getPressedAction getPressed)
		{
			reg_w_button_getPressed(getPressed);
		}
	}
}
