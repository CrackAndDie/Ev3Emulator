using Ev3CoreUnsafe.Enums;

namespace Ev3CoreUnsafe.Helpers
{
	public static class BmpHelper
	{
		public static string BasePathToBmp = "./Resources/";

		private static List<BmpImage> _images = new List<BmpImage>()
		{
			new BmpImage("icontypes/16x8_TopBar_Icons", BmpType.SmallIcons),
			new BmpImage("icontypes/16x12_MenuItems", BmpType.MenuIcons),
			new BmpImage("icontypes/24x12_Files_Folders_Settings", BmpType.NormalIcons),
			new BmpImage("icontypes/24x22_Yes_No_OFF_FILEOps", BmpType.LargeIcons),
			new BmpImage("icontypes/8x12_miniArrows", BmpType.ArrowIcons),

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

			new BmpImage("24x12_PCApp", BmpType.App),

			// FONTS
			new BmpImage("fonts/brick_font_2011_v1.3-bold", BmpType.SmallFont),
			new BmpImage("fonts/brick_font_2011_v1.3-regular", BmpType.NormalFont),
			new BmpImage("fonts/brick_font_LARGE", BmpType.LargeFont),
			new BmpImage("fonts/small_font", BmpType.TinyFont),
		};
		public static List<BmpImage> Images => _images;

		public static BmpImage Get(BmpType name)
		{
			return _images.FirstOrDefault(x => x.Name == name);
		}

		public static byte[] GetBytesOf(BmpType name)
		{
			return _images.FirstOrDefault(x => x.Name == name).Data;
		}
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

			string path = $"{BmpHelper.BasePathToBmp}{real}.bmp";
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
