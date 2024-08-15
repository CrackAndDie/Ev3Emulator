using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Ev3CoreUnsafe;
using Ev3CoreUnsafe.Interfaces;
using System;
using System.Runtime.InteropServices;

namespace Ev3Emulator.CoreImpl
{
	internal class LcdHandler : ILcdHandler
	{
		public Action<Bitmap> BitmapAction { get; set; }

		public void Exit()
		{
			
		}

		public void Init()
		{
		}

		public void UpdateLcd(byte[] data)
		{
			if (data.Length != (Defines.vmLCD_HEIGHT * Defines.vmLCD_WIDTH))
				return;

            var LcdBitmap = new WriteableBitmap(new PixelSize(Defines.vmLCD_WIDTH, Defines.vmLCD_HEIGHT), new Vector(96, 96), Avalonia.Platform.PixelFormat.Rgba8888);
            using (var frameBuffer = LcdBitmap.Lock())
			{
				// * 4 because orig data is grayscale
				Marshal.Copy(ConvertToRgba8888(data), 0, frameBuffer.Address, data.Length * 4);
			}
			BitmapAction?.Invoke(LcdBitmap);
        }

		public static byte[] ConvertToRgba8888(byte[] data)
		{
			byte[] outData = new byte[data.Length * 4];
			for (int i = 0; i < data.Length; i++)
			{
				var clr = GetColor(data[i]);
				outData[i * 4] =	 (byte)clr.R;
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
