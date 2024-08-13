using System.Runtime.CompilerServices;

namespace Ev3CoreUnsafe.Extensions
{
    public static class PointerExtensions
	{
		[Obsolete("Possible memory leaks")]
		public unsafe static T* AsPointer<T>(this T[] buf) where T: unmanaged
		{
			var p = (T*)Unsafe.AsPointer(ref GC.AllocateArray<T>(buf.Length, true)[0]);
			for (int i = 0; i < buf.Length; ++i)
			{
				p[i] = buf[i];
			}
			return p;
		}

        [Obsolete("Possible memory leaks")]
        public unsafe static DATA8* AsSbytePointer(this string str)
		{
			var p = (DATA8*)Unsafe.AsPointer(ref GC.AllocateArray<DATA8>(str.Length + 1, true)[0]);
			for (int i = 0; i < str.Length; ++i)
			{
				p[i] = (sbyte)str[i];
			}
			p[str.Length] = (sbyte)'\0';
			return p;
		}
	}
}
