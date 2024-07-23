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
	}
}
