using Ev3CoreUnsafe.Cinput.Interfaces;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ev3CoreUnsafe.Helpers
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
	}
}
