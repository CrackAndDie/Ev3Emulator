using Ev3CoreUnsafe.Enums;
using System.Runtime.InteropServices;

namespace Ev3CoreUnsafe.Helpers
{
    public enum arawf : byte
    {
        annd = 2,
        dawdaw = 3
    }

	public struct anime222
	{
		public byte test;
		public arawf aaa;
	}

	public class ExceptionTests
	{
		public unsafe void Call()
		{
			byte* notAllocated = CommonHelper.AllocateByteArray(2);

            //IMGHEAD* tmp = (IMGHEAD*)notAllocated; 
            //IMGHEAD tmp2 = *tmp;

            //tmp->GlobalBytes = 0;
            //tmp2.GlobalBytes = 0;

            //CommonHelper.DeleteByteArray((byte*)&tmp[6]);

            //anime222* a = (anime222*)notAllocated;
            //anime222 a2 = *a;

            //Thread.Sleep(1000);

            //arawf b = a2.aaa;

            var sz = sizeof(arawf);


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

            Thread.Sleep(1000);

            // System.Console.WriteLine("asdasd");

            Console.ReadKey();
		}
	}
}
