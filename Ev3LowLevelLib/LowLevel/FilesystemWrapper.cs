using System.Runtime.InteropServices;

namespace Ev3Emulator.LowLevel
{
	public unsafe struct FILESYSTEM_ENTITY_MANAGED
	{
		// is this directory
		public byte isDir;
		// for dir to enumerate items
		public byte searchOffset;
		// 0 - success, 1 - error ....
		public byte result;
		// 0 - not exists, 1 - exists
		public byte exists;

		// just a name without any path shite!!!
		public string name;
	}

	public unsafe struct FILESYSTEM_ENTITY
	{
		// is this directory
		public byte isDir;
		// for dir to enumerate items
		public byte searchOffset;
		// 0 - success, 1 - error ....
		public byte result;
		// 0 - not exists, 1 - exists
		public byte exists;

		// just a name without any path shite!!!
		public IntPtr name;
	}

	public static class FilesystemWrapper
	{
		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_filesystem_createDir(w_filesystem_createDirAction createDir);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int w_filesystem_createDirAction(string name);

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_filesystem_readDir(w_filesystem_readDirAction readDir);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate FILESYSTEM_ENTITY w_filesystem_readDirAction(string name, int offset);

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_filesystem_scanDir(w_filesystem_scanDirAction scanDir);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate IntPtr w_filesystem_scanDirAction(string name, ref int amount, byte sort);

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_filesystem_getUsedMemory(w_filesystem_getUsedMemoryAction getUsedMemory);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int w_filesystem_getUsedMemoryAction();

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_filesystem_sync(w_filesystem_syncAction sync);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void w_filesystem_syncAction();

		public static void Init()
		{
			reg_w_filesystem_createDir(CreateDir);
			reg_w_filesystem_readDir(ReadDir);
			reg_w_filesystem_scanDir(ScanDir);
			reg_w_filesystem_getUsedMemory(GetUsedMemory);
			reg_w_filesystem_sync(Sync);
		}

		private static int CreateDir(string path)
		{
			if (Directory.Exists(path))
				return 0;

			Directory.CreateDirectory(path);
			return 0;
		}

		private static FILESYSTEM_ENTITY ReadDir(string name, int offset)
		{
			if (!Directory.Exists(name))
				return new FILESYSTEM_ENTITY()
				{
					exists = 0,
					result = 1,
				};

			var elements = GetSortedElements(name);
			if (offset >= elements.Length)
				return new FILESYSTEM_ENTITY()
				{
					exists = 0,
					result = 1,
				};

			var element = elements[offset].ToUnmanaged();
			// dir.searchOffset++; // it is applied in C side
			return element;
		}

		private unsafe static IntPtr ScanDir(string name, ref int amout, byte sort)
		{
			if (!Directory.Exists(name))
				return IntPtr.Zero;

			var elements = GetSortedElements(name);
			var bytes = elements.ToUnsafeByteArray();
			IntPtr p = Marshal.AllocHGlobal(bytes.Length);
			Marshal.Copy(bytes, 0, p, bytes.Length);
			return p;
		}

		private static int GetUsedMemory()
		{
			// TODO: ...
			return 0;
		}

		private static void Sync()
		{
			// TODO: do i need it
		}

		// ----- helper methods

		public unsafe static byte[] ToUnsafeByteArray(this FILESYSTEM_ENTITY_MANAGED[] data)
		{
			List<byte> outBytes = new List<byte>();
			foreach (var el in data)
			{
				var usafeEl = el.ToUnmanaged();
				outBytes.Add(usafeEl.isDir);
				outBytes.Add(usafeEl.searchOffset);
				outBytes.Add(usafeEl.result);
				outBytes.Add(usafeEl.exists);
#warning won't work on x64
				outBytes.AddRange(IntToBytes((int)usafeEl.name)); 
			}
			return outBytes.ToArray();
		}

		public unsafe static FILESYSTEM_ENTITY ToUnmanaged(this FILESYSTEM_ENTITY_MANAGED entity)
		{
			// THIS SHOULD BE FREED IN C SIDE
			byte* strPtr = (byte*)Marshal.AllocHGlobal(entity.name.Length + 1);
			for (int i = 0; i < entity.name.Length; i++)
			{
				strPtr[i] = (byte)entity.name[i];
			}
			strPtr[entity.name.Length] = 0; // null term

			return new FILESYSTEM_ENTITY()
			{
				name = (IntPtr)strPtr,
				exists = entity.exists,
				isDir = entity.isDir,
				result = entity.result,
				searchOffset = entity.searchOffset,
			};
		}

		private static FILESYSTEM_ENTITY_MANAGED[] GetSortedElements(string dir)
		{
			// at first place folders
			List<FILESYSTEM_ENTITY_MANAGED> items = new List<FILESYSTEM_ENTITY_MANAGED>();

			var dirInfo = new DirectoryInfo(dir);
			var folders = dirInfo.GetDirectories();
			var foldersSorted = folders.OrderBy(x => x.Name);
			foreach (var fld in foldersSorted)
			{
				items.Add(new FILESYSTEM_ENTITY_MANAGED()
				{
					isDir = 1,
					exists = 1,
					result = 0,
					searchOffset = 0,
					name = fld.Name,
				});
			}
			var files = dirInfo.GetFiles();
			var filesSorted = folders.OrderBy(x => x.Name);
			foreach (var file in filesSorted)
			{
				items.Add(new FILESYSTEM_ENTITY_MANAGED()
				{
					isDir = 0,
					exists = 1,
					result = 0,
					searchOffset = 0,
					name = file.Name,
				});
			}
			return items.ToArray();
		}

		private static byte[] IntToBytes(int value)
		{
			return new byte[] { (byte)((value) & 0xFF), (byte)(((value) >> 8) & 0xFF), (byte)(((value) >> 16) & 0xFF), (byte)(((value) >> 24) & 0xFF)};
		}
	}
}
