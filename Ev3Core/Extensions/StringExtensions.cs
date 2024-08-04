using Ev3Core.Enums;
using Ev3Core.Helpers;

namespace Ev3Core.Extensions
{
	public static class StringExtensions
	{
		public static sbyte[] ToSbyteArray(this string str)
		{
			return ToSbyteArray(str.ToCharArray());
		}

        public static byte[] ToByteArray(this string str)
        {
            return ToByteArray(str.ToCharArray());
        }

        public static sbyte[] ToSbyteArray(this char[] arr)
		{
			return CommonHelper.CastArray<char, sbyte>(arr);
		}

        public static byte[] ToByteArray(this char[] arr)
        {
            return CommonHelper.CastArray<char, byte>(arr);
        }

        public static byte[] ToByteArray(this sbyte[] arr)
		{
			return CommonHelper.CastArray<sbyte, byte>(arr);
		}

		public static char[] ToCharArray(this sbyte[] arr)
		{
			return CommonHelper.CastArray<sbyte, char>(arr);
		}

		public static string AsString(this sbyte[] arr)
		{
			return string.Concat(arr.Select(x => (char)x));
		}
	}
}
