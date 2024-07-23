namespace Ev3EmulatorCore.Extensions
{
	public static class BufferExtensions
	{
		public static void WriteString(this char[] buff, string value)
		{
			for (int i = 0; i < Math.Min(buff.Length, value.Length); ++i)
			{
				buff[i] = value[i];
			}
		}

		public static void WriteString(this byte[] buff, string value)
		{
			for (int i = 0; i < Math.Min(buff.Length, value.Length); ++i)
			{
				buff[i] = (byte)value[i];
			}
		}

		public static void WriteBytes(this byte[] buff, byte[] value)
		{
			for (int i = 0; i < Math.Min(buff.Length, value.Length); ++i)
			{
				buff[i] = (byte)value[i];
			}
		}
	}
}
