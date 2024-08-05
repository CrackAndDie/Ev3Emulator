using Ev3Core.Cinput.Interfaces;
using System.Runtime.InteropServices;

namespace Ev3Core.Helpers
{
	public static class CommonHelper
	{
		public static T[][] Array2d<T>(int a, int b, bool inst = false) where T : new()
		{
			T[][] arr = new T[a][];
			for (int i = 0; i < arr.GetLength(0); i++)
			{
				arr[i] = new T[b];

				if (!inst)
					continue;
				for (int j = 0; j < b; ++j)
				{
					arr[i][j] = new T();
				}
			}
			return arr;
		}

		public static T[] Array1d<T>(int a, bool inst = false) where T : new()
		{
			T[] arr = new T[a];

			if (!inst)
				return arr;

			for (int j = 0; j < a; ++j)
			{
				arr[j] = new T();
			}
			return arr;
		}

		public static TTo[] CastArray<TFrom, TTo>(TFrom[] arr) 
			where TFrom : struct
			where TTo : struct
		{
			ReadOnlySpan<TFrom> shortSpan = new ReadOnlySpan<TFrom>(arr);
			return MemoryMarshal.Cast<TFrom, TTo>(shortSpan).ToArray();
		}

		public static TTo[] CastObjectArray<TTo>(object[] arr)
		{
			return arr.Select(x => (TTo)x).ToArray();
		}

		public static byte[] GetBytes<T>(T data, bool isLe = true) where T : struct
		{
			var result = (byte[])typeof(BitConverter).GetMethod("GetBytes", new[] { typeof(T) }).Invoke(null, new[] { (object)data });
			if ((isLe && BitConverter.IsLittleEndian) || (!isLe && !BitConverter.IsLittleEndian))
				return result;
			if ((isLe && !BitConverter.IsLittleEndian) || (!isLe && BitConverter.IsLittleEndian))
				Array.Reverse(result);
			return result;
		}

		public static int Strcmp(ArrayPointer<UBYTE> first, string second)
		{
			if (first.Length > second.Length)
				return 1;
			if (first.Length < second.Length)
				return -1;

			for (int i = 0; i < first.Length; ++i)
			{
				char c1 = (char)first[i];
				char c2 = (char)second[i];
				if (c1 != c2)
				{
					return c1 > c2 ? 1 : -1;
				}
			}
			return 0;
		}

		public static int Strcmp(ArrayPointer<UBYTE> first, ArrayPointer<UBYTE> second)
		{
			if (first.Length > second.Length)
				return 1;
			if (first.Length < second.Length)
				return -1;

			for (int i = 0; i < first.Length; ++i)
			{
				char c1 = (char)first[i];
				char c2 = (char)second[i];
				if (c1 != c2)
				{
					return c1 > c2 ? 1 : -1;
				}
			}
			return 0;
		}

		public static bool Strstr(ArrayPointer<UBYTE> b, ArrayPointer<UBYTE> a)
		{
			if (b.Length < a.Length)
				return false;

			int j = 0;
			for (int i = 0; i < b.Length; ++i)
			{
				if (j >= a.Length)
					break;

				if (a[j] != b[i])
				{
					j = 0;
				}
				else
				{
					j++;
				}
			}

			return j == a.Length;
		}

		public static int Memcmp(ArrayPointer<UBYTE> a, ArrayPointer<UBYTE> b, int num)
		{
			for (int i = 0; i < num; ++i)
			{
				if (a[i] != b[i])
				{
					return a[i] > b[i] ? 1 : -1;
				}
			}

			return 0;
		}

		public static void Memmove(ArrayPointer<UBYTE> a, ArrayPointer<UBYTE> b, int num)
		{
			for (int i = 0; i < num; ++i)
			{
				a[i] = b[i];
			}
		}

		public static void Memset(ArrayPointer<UBYTE> a, byte val, int num)
		{
			for (int i = 0; i < num; ++i)
			{
				a[i] = val;
			}
		}

		public static int Snprintf(ArrayPointer<UBYTE> dst, int max, params ArrayPointer<UBYTE>[] data)
		{
			int dstBeg = 0;
            foreach (var dt in data)
			{
				int dtIndexer = 0;
				for (int i = dstBeg; i < Math.Min(Math.Min(dst.Length, dstBeg + max), dt.Length); ++i)
				{
					dst[i] = dt[dtIndexer];
					dtIndexer++;
				}
				dstBeg += dt.Length;
			}
			return 0; // idk what to return
		}

		public static int Sprintf(ArrayPointer<UBYTE> dst, params ArrayPointer<UBYTE>[] data)
		{
            int dstBeg = 0;
            foreach (var dt in data)
			{
				int dtIndexer = 0;
				for (int i = dstBeg; i < Math.Min(dst.Length, dt.Length); ++i)
				{
					dst[i] = dt[dtIndexer];
					dtIndexer++;
				}
				dstBeg += dt.Length;
			}
			return 0; // idk what to return
		}

		public static void Strncpy(ArrayPointer<UBYTE> dst, ArrayPointer<UBYTE> src, int num)
		{
			for (int i = 0; i < Math.Min(src.Length, num); ++i)
			{
				dst[i] = src[i];
			}
		}

		public static void Strcpy(ArrayPointer<UBYTE> dst, string src)
		{
			for (int i = 0; i < Math.Min(src.Length, dst.Length); ++i)
			{
				dst[i] = (byte)src[i];
			}
		}
	}
}
