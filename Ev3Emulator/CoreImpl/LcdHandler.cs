using Avalonia;
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

		private byte[] ConvertToRgba8888(byte[] data)
		{
			byte[] outData = new byte[data.Length * 4];
			for (int i = 0; i < data.Length; i++)
			{
				outData[i * 4] = (byte)(data[i] * 255);

				outData[i * 4 + 3] = 255; // alpha
			}
			return outData;
		}
	}
}
