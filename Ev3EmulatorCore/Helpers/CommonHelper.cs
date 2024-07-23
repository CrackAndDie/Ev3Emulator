namespace Ev3EmulatorCore.Helpers
{
	public static class CommonHelper
	{
		public static T[][] GenerateTwoDimArray<T>(int a, int b)
		{
			T[][] arr = new T[a][];
			for (int i = 0; i < arr.GetLength(0); i++)
			{
				arr[i] = new T[b];
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
    }
}
