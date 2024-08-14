using Ev3CoreUnsafe.Helpers;
using System.Runtime.CompilerServices;

namespace Ev3CoreUnsafe.Extensions
{
    public static class PointerExtensions
	{
		[Obsolete("Memory leaks")]
		public unsafe static T* AsPointer<T>(this T[] buf) where T: unmanaged
		{
			var p = (T*)CommonHelper.AllocateByteArray(sizeof(T) * buf.Length);
			for (int i = 0; i < buf.Length; ++i)
			{
				p[i] = buf[i];
			}
			return p;
		}

        [Obsolete("Memory leaks")]
        public unsafe static DATA8* AsSbytePointer(this string str)
		{
			var p = (DATA8*)CommonHelper.AllocateByteArray(str.Length + 1);
			for (int i = 0; i < str.Length; ++i)
			{
				p[i] = (sbyte)str[i];
			}
			p[str.Length] = (sbyte)'\0';
			return p;
		}
	}
}
