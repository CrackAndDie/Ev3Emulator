using System.Drawing;
using System.Runtime.InteropServices;

namespace Ev3ConsoleCmakeTest.Tests
{
	public struct FILESYSTEM_ENTITY
	{
		// is this directory
		public byte isDir;
		// for dir to enumerate items
		public byte searchOffset;
		// 0 - success, 1 - error ....
		public byte result;
		// 0 - not exists, 1 - exists
		public byte exists;
	}

	public static class TestWrapper
	{
		[DllImport(@"TestProject", CallingConvention = CallingConvention.Cdecl)]
		public extern static void getAbb(ref int abb);
		[DllImport(@"TestProject", CallingConvention = CallingConvention.Cdecl)]
		public extern static int getAnime();
		[DllImport(@"TestProject", CallingConvention = CallingConvention.Cdecl)]
		public extern static void getCringe(ref int abb);
		[DllImport(@"TestProject", CallingConvention = CallingConvention.Cdecl)]
		public extern static void getCringe222(ref int abb);
		[DllImport(@"TestProject", CallingConvention = CallingConvention.Cdecl)]
		public extern static void getCringe333();


		[DllImport(@"TestProject", CallingConvention = CallingConvention.Cdecl)]
		public extern static void reg_w_filesystem_readDir(w_filesystem_readDirAction func);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate FILESYSTEM_ENTITY w_filesystem_readDirAction(FILESYSTEM_ENTITY ent);

		[DllImport(@"TestProject", CallingConvention = CallingConvention.Cdecl)]
		public extern static void reg_w_filesystem_readDir222(reg_w_filesystem_readDir222Action func);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void reg_w_filesystem_readDir222Action(IntPtr buf, int len);

		public static void Test()
		{
			//int bbb = getAnime();
			//getAbb(ref bbb);
			//bbb = getAnime();
			//getCringe(ref bbb);

			//int asdasd = 0;
			//reg_w_filesystem_readDir(FncTest);
			//getCringe222(ref asdasd);

			reg_w_filesystem_readDir222(FncTest2);
			getCringe333();
		}

		private static FILESYSTEM_ENTITY FncTest(FILESYSTEM_ENTITY ent)
		{
			ent.searchOffset++;
			return ent;
		}

		private static void FncTest2(IntPtr buf, int len)
		{ 
			byte[] managedArray = new byte[len];
			Marshal.Copy(buf, managedArray, 0, len);
		}
	}
}
