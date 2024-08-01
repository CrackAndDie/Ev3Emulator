namespace Ev3Core.Extensions
{
	public static class EnumerableExtensions
	{
		public static object[] ToObjectArray<T>(this T[] arr)
		{
			return arr.Select(x => x as object).ToArray();
		}
	}
}
