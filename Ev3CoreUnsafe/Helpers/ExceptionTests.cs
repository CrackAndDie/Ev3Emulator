﻿using System.Runtime.InteropServices;

namespace Ev3CoreUnsafe.Helpers
{
	public class ExceptionTests
	{
		public unsafe void Call()
		{
			byte* notAllocated = CommonHelper.AllocateByteArray(3);

			IMGHEAD* tmp = (IMGHEAD*)notAllocated;
			IMGHEAD tmp2 = *tmp;

			tmp->GlobalBytes = 0;
			tmp2.GlobalBytes = 0;

			CommonHelper.DeleteByteArray((byte*)&tmp[6]);

			//for (int i = 0; i < 20; ++i)
			//{
			//	notAllocated[1000 + i] = 3;
			//}


			// CommonHelper.memset(notAllocated, 0, 1000);

			// var a = *(&notAllocated[1000]);

			// CommonHelper.DeleteByteArray(&notAllocated[3]);

			// System.Console.WriteLine("asdasd");
			//         System.Console.WriteLine("asdasd");
			//         System.Console.WriteLine("asdasd");
			//         System.Console.WriteLine("asdasd");
			//         System.Console.WriteLine("asdasd");

			// CommonHelper.DeleteByteArray((byte*)100000);

			//IntPtr handler = Marshal.AllocHGlobal(3);
			//var ptr = (byte*)handler.ToPointer();

			//CommonHelper.memset(ptr, 0, 3);

			//var handler2 = new IntPtr(&ptr[6]);
			//Marshal.FreeHGlobal(handler2);

			Thread.Sleep(2000);

			System.Console.WriteLine("asdasd");
		}
	}
}
