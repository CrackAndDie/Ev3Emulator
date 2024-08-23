using System;
using System.Runtime.InteropServices;

namespace Ev3Emulator.LowLevel
{
	public static class LcdWrapper
	{
		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_lcd_updateLcd(reg_w_lcd_updateLcdAction updateLcd);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void reg_w_lcd_updateLcdAction(IntPtr buff, int size);

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_lcd_updateLed(reg_w_lcd_updateLedAction updateLed);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void reg_w_lcd_updateLedAction(IntPtr buff, int size);

		public static void Init(reg_w_lcd_updateLcdAction updateLcd, reg_w_lcd_updateLedAction updateLed)
		{
			reg_w_lcd_updateLcd(updateLcd);
			reg_w_lcd_updateLed(updateLed);
		}
	}
}
