using Ev3Core.Enums;
using Ev3Core.Helpers;

namespace Ev3Core.Extensions
{
	public static class StringExtensions
	{
		public static sbyte[] ToSbyteArray(this string str)
		{
			return CommonHelper.CastArray<char, sbyte>(str.ToCharArray());
		}

		public static char[] ToCharArray(this sbyte[] arr)
		{
			return arr.Select(x => (char)x).ToArray();
		}

		public static string AsString(this sbyte[] arr)
		{
			return string.Concat(arr.Select(x => (char)x));
		}
	}
}
