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

		public static int Strcmp(byte[] first, string second)
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

		public static bool Strstr(byte[] b, byte[] a)
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

		public static int Memcmp(byte[] a, byte[] b, int num)
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

		public static void Memmove(byte[] a, byte[] b, int num, int aP = 0, int bP = 0)
		{
			for (int i = 0; i < num; ++i)
			{
				a[i + aP] = b[i + bP];
			}
		}

		public static void Memset(byte[] a, byte val, int num, int aP = 0)
		{
			for (int i = aP; i < aP + num; ++i)
			{
				a[i] = val;
			}
		}
	}
}
