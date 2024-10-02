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

		public static void Init(Action<byte[]> updateLcd, Action<int> updateLed, bool desertColors = false)
		{
			_updateLcd = updateLcd;
			_updateLed = updateLed;
			_desertColors = desertColors;

			reg_w_lcd_updateLcd(UpdateLcd);
			reg_w_lcd_updateLed(UpdateLed);
		}

		private static void UpdateLcd(IntPtr buff, int size)
		{
			var bmpData = LcdWrapper.GetBitmapData(buff, size);

			if (LcdWrapper.vmLCD_WIDTH * LcdWrapper.vmLCD_HEIGHT != bmpData.Length)
				return;

			_updateLcd?.Invoke(LcdWrapper.ConvertToRgba8888(bmpData));
		}

		private static void UpdateLed(int state)
		{
			_updateLed?.Invoke(state);
		}

		public static byte[] GetBitmapData(IntPtr buf, int size)
		{
			if (size != (LcdWrapper.vmLCD_HEIGHT * LcdWrapper.vmLCD_WIDTH))
				return null;

			byte[] data = new byte[(LcdWrapper.vmLCD_HEIGHT * LcdWrapper.vmLCD_WIDTH)];
			Marshal.Copy(buf, data, 0, (LcdWrapper.vmLCD_HEIGHT * LcdWrapper.vmLCD_WIDTH));
			
			return data;
		}

		public static byte[] ConvertToRgba8888(byte[] data, bool isDisabled = false)
		{
			byte[] outData = new byte[data.Length * 4];
			for (int i = 0; i < data.Length; i++)
			{
				(byte A, byte R, byte G, byte B) clr;
				if (!_desertColors)
					clr = GetColor(data[i], isDisabled);
				else
					clr = GetDesertColor(data[i], isDisabled);
				outData[i * 4] = (byte)clr.R;
				outData[i * 4 + 1] = (byte)clr.G;
				outData[i * 4 + 2] = (byte)clr.B;

				outData[i * 4 + 3] = (byte)clr.A; // alpha
			}
			return outData;
		}

		private static (byte A, byte R, byte G, byte B) GetColor(byte clr, bool isDisabled = false)
		{
			if (isDisabled)
                return (255, 36, 121, 78);

            if (clr > 0)
			{
				return (255, 0, 0, 0);
			}
			else
			{
				return (255, 136, 221, 178);
			}
		}

		private static (byte A, byte R, byte G, byte B) GetDesertColor(byte clr, bool isDisabled = false)
		{
			if (isDisabled)
				return (40, 0, 0, 0);

			if (clr > 0)
			{
				return (255, 0, 0, 0);
			}
			else
			{
				return (0, 0, 0, 0);
			}
		}

		private static Action<byte[]> _updateLcd;
		private static Action<int> _updateLed;

		private static bool _desertColors = false; // just for Ev3Emulator themes
	}
}
