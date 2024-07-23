﻿namespace Ev3EmulatorCore.Extensions
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

		public static T[] ConcatArrays<T>(this T[] a1, T[] a2)
		{
            var z = new T[a1.Length + a2.Length];
            a1.CopyTo(z, 0);
            a2.CopyTo(z, a1.Length);
			return z;
        }
	}
}
