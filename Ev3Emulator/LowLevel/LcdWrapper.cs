using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using System;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

		public static Bitmap GetBitmap(IntPtr buf, int size)
		{
			if (size != (LcdWrapper.vmLCD_HEIGHT * LcdWrapper.vmLCD_WIDTH))
				return null;

			byte[] data = new byte[(LcdWrapper.vmLCD_HEIGHT * LcdWrapper.vmLCD_WIDTH)];
			Marshal.Copy(buf, data, 0, (LcdWrapper.vmLCD_HEIGHT * LcdWrapper.vmLCD_WIDTH));

			var LcdBitmap = new WriteableBitmap(new PixelSize(LcdWrapper.vmLCD_WIDTH, LcdWrapper.vmLCD_HEIGHT), new Vector(96, 96), Avalonia.Platform.PixelFormat.Rgba8888);
			using (var frameBuffer = LcdBitmap.Lock())
			{
				// * 4 because orig data is grayscale
				Marshal.Copy(LcdWrapper.ConvertToRgba8888(data), 0, frameBuffer.Address, data.Length * 4);
			}
			return LcdBitmap;
		}

		private static byte[] ConvertToRgba8888(byte[] data)
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

		private static Color GetColor(byte clr)
		{
			if (clr > 0)
			{
				return Colors.Black;
			}
			else
			{
				return new Color(255, 136, 221, 178);
			}
		}
	}
}
