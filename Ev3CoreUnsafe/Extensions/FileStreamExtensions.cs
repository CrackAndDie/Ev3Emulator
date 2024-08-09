using Ev3CoreUnsafe.Helpers;

namespace Ev3CoreUnsafe.Extensions
{
	public static class FileStreamExtensions
	{
		public unsafe static int ReadUnsafe(this FileStream fs, byte* buf, int offset, int count)
		{
			byte[] realBuf = new byte[count];
			int read = fs.Read(realBuf, offset, count);
			for (int i = 0; i < read; i++)
			{
				buf[i] = realBuf[i];
			}
			return read;
		}

		public unsafe static void WriteUnsafe(this FileStream fs, byte* buf, int offset, int count)
		{
			byte[] realBuf = CommonHelper.GetArray(buf, count);
			fs.Write(realBuf, offset, count);
		}
	}
}
