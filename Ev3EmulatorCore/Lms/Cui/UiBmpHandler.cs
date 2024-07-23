namespace Ev3EmulatorCore.Lms.Cui
{
	public class UiBmpHandler
	{
		private static string _basePathToBmp = "./Resources/Bmp/";

		private List<BmpImage> _images = new List<BmpImage>()
		{
			new BmpImage("mindstorms", BmpType.Mindstorms),
			new BmpImage("Ani1x", BmpType.Ani1x),
			new BmpImage("Ani2x", BmpType.Ani2x),
			new BmpImage("Ani3x", BmpType.Ani3x),
			new BmpImage("Ani4x", BmpType.Ani4x),
			new BmpImage("Ani5x", BmpType.Ani5x),
			new BmpImage("Ani6x", BmpType.Ani6x),
			new BmpImage("144x48_POP2", BmpType.POP2),
			new BmpImage("144x65_POP3", BmpType.POP3),
			new BmpImage("144x116_POP6", BmpType.POP6),

			new BmpImage("KEY_CAPCHAR", BmpType.KeyboardCap),
			new BmpImage("KEY_NumSymb", BmpType.KeyboardNum),
			new BmpImage("KEY_SmCHAR", BmpType.KeyboardSmp),
		};

		public List<BmpImage> Images => _images;

		public BmpImage Get(BmpType name)
		{
			return _images.FirstOrDefault(x => x.Name == name);
		}

		public byte[] GetBytesOf(BmpType name)
		{
			return _images.FirstOrDefault(x => x.Name == name).Data;
		}

		public enum BmpType
		{
			Mindstorms,
			Ani1x,
			Ani2x,
			Ani3x,
			Ani4x,
			Ani5x,
			Ani6x,
			POP2,
			POP3,
			POP6,
			KeyboardCap,
			KeyboardNum,
			KeyboardSmp,
		}

		public class BmpImage
		{
			public BmpType Name { get; set; }
			public string RealName { get; set; }

			public short Width { get; set; }
			public short Height { get; set; }
			public byte[] Data { get; private set; }

			public BmpImage(string real, BmpType name) 
			{
				RealName = real;
				Name = name;

				string path = $"{_basePathToBmp}{real}.bmp";
				var outputImage = SkiaSharp.SKBitmap.Decode(path);
				if (!(outputImage.BytesPerPixel == 3 || outputImage.BytesPerPixel == 4))
					throw new Exception($"Unsupported Bits per image ({outputImage.BytesPerPixel}) for BmpSharp");

				var bitsPerPixel = outputImage.BytesPerPixel == 4 ? BmpSharp.BitsPerPixelEnum.RGBA32 : BmpSharp.BitsPerPixelEnum.RGB24;
				var bmp = new BmpSharp.Bitmap(outputImage.Width, outputImage.Height, outputImage.Bytes, bitsPerPixel);

				Width = (short)outputImage.Width;
				Height = (short)outputImage.Height;
				Data = GetBinaryBytes(bmp.GetBmpBytes(), outputImage.BytesPerPixel);
			}

			private byte[] GetBinaryBytes(byte[] bmp, int perPixel)
			{
				List<byte> bytes = new List<byte>();

				int currVal = 0;
				int currValNumber = 0;
				foreach (byte b in bmp)
				{
					currVal += b;
					currValNumber++;

					if (currValNumber == perPixel)
					{
						bool isBlackPixel = (currVal / perPixel) > 0;
						bytes.Add(isBlackPixel ? (byte)1 : (byte)0);

						currVal = 0;
						currValNumber = 0;
					}
				}

				return bytes.ToArray();
			}
		}
	}
}
