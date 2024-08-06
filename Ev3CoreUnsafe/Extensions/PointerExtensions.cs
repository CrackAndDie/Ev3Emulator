using System.Runtime.CompilerServices;

namespace Ev3CoreUnsafe.Extensions
{
	public static class PointerExtensions
	{
		public unsafe static T* AsPointer<T>(this T[] buf) where T: unmanaged
		{
			var p = (T*)Unsafe.AsPointer(ref GC.AllocateArray<T>(buf.Length, true)[0]);
			for (int i = 0; i < buf.Length; ++i)
			{
				p[i] = buf[i];
			}
			return p;
		}
	}
}
