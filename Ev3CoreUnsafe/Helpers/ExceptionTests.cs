namespace Ev3CoreUnsafe.Helpers
{
	public class ExceptionTests
	{
		public unsafe void Call()
		{
			//byte* notAllocated = CommonHelper.AllocateByteArray(1001);

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

			CommonHelper.DeleteByteArray((byte*)100000);

			System.Console.WriteLine("asdasd");
		}
	}
}
