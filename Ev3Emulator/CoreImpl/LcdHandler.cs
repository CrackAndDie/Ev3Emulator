using Avalonia.Media.Imaging;
using BmpSharp;
using Ev3CoreUnsafe.Interfaces;
using System.Runtime.InteropServices;

namespace Ev3Emulator.CoreImpl
{
	internal class LcdHandler : ILcdHandler
	{
		public WriteableBitmap Bitmap { get; set; }

		public void Exit()
		{
			
		}

		public void Init()
		{
		}

		public void UpdateLcd(byte[] data)
		{
			using (var frameBuffer = Bitmap.Lock())
			{
				// * 4 because orig data is grayscale
				Marshal.Copy(ConvertToRgba8888(data), 0, frameBuffer.Address, data.Length * 4); 
			}
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
