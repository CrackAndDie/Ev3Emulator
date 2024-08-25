using System.Runtime.InteropServices;

namespace Ev3Emulator.LowLevel
{
	public static class LcdWrapper
	{
		public const int vmLCD_HEIGHT = 128;
		public const int vmLCD_WIDTH = 178;

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_lcd_updateLcd(reg_w_lcd_updateLcdAction updateLcd);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void reg_w_lcd_updateLcdAction(IntPtr buff, int size);

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_lcd_updateLed(reg_w_lcd_updateLedAction updateLed);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void reg_w_lcd_updateLedAction(int state);

		public static void Init(reg_w_lcd_updateLcdAction updateLcd, reg_w_lcd_updateLedAction updateLed)
		{
			reg_w_lcd_updateLcd(updateLcd);
			reg_w_lcd_updateLed(updateLed);
		}

		public static byte[] GetBitmapData(IntPtr buf, int size)
		{
			if (size != (LcdWrapper.vmLCD_HEIGHT * LcdWrapper.vmLCD_WIDTH))
				return null;

			byte[] data = new byte[(LcdWrapper.vmLCD_HEIGHT * LcdWrapper.vmLCD_WIDTH)];
			Marshal.Copy(buf, data, 0, (LcdWrapper.vmLCD_HEIGHT * LcdWrapper.vmLCD_WIDTH));
			
			return data;
		}

		public static byte[] ConvertToRgba8888(byte[] data)
		{
			byte[] outData = new byte[data.Length * 4];
			for (int i = 0; i < data.Length; i++)
			{
				var clr = GetColor(data[i]);
				outData[i * 4] = (byte)clr.R;
				outData[i * 4 + 1] = (byte)clr.G;
				outData[i * 4 + 2] = (byte)clr.B;

				outData[i * 4 + 3] = 255; // alpha
			}
			return outData;
		}

		private static (byte A, byte R, byte G, byte B) GetColor(byte clr)
		{
			if (clr > 0)
			{
				return (0, 0, 0, 0);
			}
			else
			{
				return (255, 136, 221, 178);
			}
		}
	}
}
