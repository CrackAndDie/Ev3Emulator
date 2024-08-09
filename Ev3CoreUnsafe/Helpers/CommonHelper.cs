using Ev3CoreUnsafe.Cinput.Interfaces;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ev3CoreUnsafe.Helpers
{
	public static class CommonHelper
	{
        public static long DirSize(DirectoryInfo d)
        {
            long size = 0;
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += DirSize(di);
            }
            return size;
        }

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

        public unsafe static T* PointerStruct<T>() where T : new()
        {
            // T* inst = (T*)Unsafe.AsPointer(ref GC.AllocateArray<T>(Unsafe.SizeOf<T>(), true)[0]);
            T* inst = (T*)Unsafe.AsPointer(ref GC.AllocateArray<T>(1, true)[0]);
            return inst;
        }

        public unsafe static T* Pointer1d<T>(int a, bool inst = false) where T : new()
		{
			T* arr = (T*)Unsafe.AsPointer(ref GC.AllocateArray<T>(a, true)[0]);

			if (!inst)
				return arr;

			for (int j = 0; j < a; ++j)
			{
				arr[j] = new T();
			}
			return arr;
		}

		public unsafe static T** Pointer2d<T>(int a, int b, bool inst = false) where T : new()
		{
			T** arr = (T**)Unsafe.AsPointer(ref GC.AllocateArray<T[]>(a, true)[0]);
			for (int i = 0; i < a; i++)
			{
				arr[i] = (T*)Unsafe.AsPointer(ref GC.AllocateArray<T>(b, true)[0]);

				if (!inst)
					continue;
				for (int j = 0; j < b; ++j)
				{
					arr[i][j] = new T();
				}
			}
			return arr;
		}

		public unsafe static string GetString<T>(T* buf)
		{
			List<char> arr = new List<char>();
			while ((char)(object)*buf != '\0')
			{
				arr.Add((char)(object)*buf);
				buf++;
			}
			return string.Concat(arr);
		}

		public static string GetString(float val, int before = -1, int after = -1)
		{
			if (before == -1 && after == -1)
				return val.ToString();

			if (before == -1)
			{
				return val.ToString($"F{after}");
			}

			if (after == -1)
			{
				var tmpIn = val.ToString();
				tmpIn = tmpIn.Length >= before ? tmpIn : new string(' ', before - tmpIn.Length) + tmpIn;
				return tmpIn;
			}

			var tmp = val.ToString($"F{after}");
			tmp = tmp.Length >= before ? tmp : new string(' ', before - tmp.Length) + tmp;
			return tmp;
		}

		public unsafe static T[] GetArray<T>(T* p, int count)
		{
			T[] values = new T[count];
			for (int i = 0; i < count; i++)
			{
				values[i] = p[i];
			}
			return values;
		}

		public unsafe static void CopyToPointer(byte* dst, byte[] src)
		{
			for (int i = 0; i < src.Length; i++)
			{
				dst[i] = src[i];
			}
		}

		// c shite
		public unsafe static int snprintf(DATA8* buf, int count, params string[] strs)
		{
			int curr = 0;
			bool brakeOut = false;
			foreach (var str in strs)
			{
				foreach (var c in str)
				{
					if (curr++ > count)
					{
						brakeOut = true;
						break;
					}

					// probably no need to add nullterm to the buf (yes, tested)
					if (c == '\0')
						continue;

					buf[0] = (sbyte)c;
					buf++;
				}

				if (brakeOut)
					break;
			}

			buf[0] = (sbyte)'\0';
			return curr;
		}

		public unsafe static int sprintf(DATA8* buf, params string[] strs)
		{
			int curr = 0;
			bool brakeOut = false;
			foreach (var str in strs)
			{
				foreach (var c in str)
				{
					if (buf[0] == 0)
					{
						brakeOut = true;
						break;
					}

					// probably no need to add nullterm to the buf (yes, tested)
					if (c == '\0')
						continue;

					buf[0] = (sbyte)c;
					buf++;
					curr++;
				}

				if (brakeOut)
					break;
			}

			buf[0] = (sbyte)'\0';
			return curr;
		}

		public unsafe static int strlen(DATA8* buf)
		{
			int count = 0;
			while (*buf++ != 0)
			{
				count++;
			}
			return count;
		}

		public unsafe static void sscanf(DATA8* buf, string format, float* res) // format is unused
		{
			string s = GetString(buf);
			*res = float.Parse(s, CultureInfo.InvariantCulture);
		}

		public unsafe static long atol(DATA8* buf)
		{
			var str = GetString(buf);
			return long.Parse(str, CultureInfo.InvariantCulture);
		}

		public unsafe static void memset(UBYTE* buf, int val, int num)
		{
			for (int i = 0; i < num; ++i)
			{
				buf[i] = (byte)val;
			}
		}

		public unsafe static int strcmp(DATA8* str1, DATA8* str2)
		{
			int curr = 0;
			while (true)
			{
				bool done1 = str1[curr] == 0;
				bool done2 = str1[curr] == 0;

				if (done1 && done2)
					break;

				if (done1)
					return -1;
				if (done2)
					return 1;

				if (str1[curr] != str2[curr])
				{
					return str1[curr] > str2[curr] ? 1 : -1;
				}
				curr++;
			}
			return 0;
		}

		public unsafe static DATA8* strncpy(DATA8* dst, DATA8* src, int num)
		{
			for (int i = 0; i < num; ++i)
			{
				if (dst[i] == 0) // if it is already the end
					return dst;

				if (src[i] == 0)
				{
					dst[i] = src[i];
					return dst;
				}

				dst[i] = src[i];
			}
			return dst;
		}

		public unsafe static DATA8* strcpy(DATA8* dst, DATA8* src)
		{
			int i = 0;
			while (true)
			{
				if (dst[i] == 0) // if it is already the end
					return dst;

				if (src[i] == 0)
				{
					dst[i] = src[i];
					return dst;
				}

				dst[i] = src[i];
				i++;
			}
		}

		public unsafe static DATA8* strstr(DATA8* str1, DATA8* str2)
		{
			int localPointer = 0;
			int i = 0;
			while (true)
			{
				localPointer = 0;

				if (str1[i] == 0)
					break;

				if (str1[i] == str1[localPointer])
				{
					while (true)
					{
						if (str2[localPointer] == 0)
							return &str1[i];

						if (str1[i + localPointer] == 0)
							break;

						if (str1[i + localPointer] != str2[localPointer])
							break;

						localPointer++;
					}
				}

				i++;
			}
			return null;
		}

		public unsafe static byte* memcpy(byte* dst, byte* src, int num)
		{
			for (int i = 0; i < num; ++i)
			{
				dst[i] = src[i];
			}
			return dst;
		}
	}
}
