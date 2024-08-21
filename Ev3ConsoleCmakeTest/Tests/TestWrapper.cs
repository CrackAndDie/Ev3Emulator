using System.Runtime.InteropServices;

namespace Ev3ConsoleCmakeTest.Tests
{
	public static class TestWrapper
	{
		[DllImport(@"TestProject", CallingConvention = CallingConvention.Cdecl)]
		public extern static void getAbb(ref int abb);
		[DllImport(@"TestProject", CallingConvention = CallingConvention.Cdecl)]
		public extern static int getAnime();
		[DllImport(@"TestProject", CallingConvention = CallingConvention.Cdecl)]
		public extern static void getCringe(ref int abb);

		public static void Test()
		{
			int bbb = getAnime();
			getAbb(ref bbb);
			bbb = getAnime();
			getCringe(ref bbb);
		}
	}
}
