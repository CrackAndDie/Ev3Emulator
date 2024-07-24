using EV3DecompilerLib.Decompile;

namespace Ev3EmulatorCore.Lms.Cmemory
{
    public partial class CmemoryClass
    {
        public void cMemoryCloseFolder(ushort prgId, ref short handle)
        {
            // TODO
        }

        public lms2012.Result cMemoryOpenFolder(ushort prgId, byte tp, byte[] folderName, ref short handle)
        {
            // TODO
            return lms2012.Result.OK;
        }

		public void cMemoryCloseFile(ushort prgId, ref short handle)
		{
			// TODO
		}

		public lms2012.Result cMemoryGetItem(ushort prgId, short handle, short item, byte len, byte[] name, ref sbyte tp)
        {
            // TODO
            return lms2012.Result.OK;
        }

        public lms2012.Result cMemoryGetItemName(ushort prgId, short handle, short item, byte len, byte[] name, ref sbyte tp, ref byte priority)
        {
            // TODO
            return lms2012.Result.OK;
        }

		public lms2012.Result cMemoryGetItemText(ushort prgId, short handle, short item, byte len, byte[] text)
		{
			// TODO
			return lms2012.Result.OK;
		}

		public lms2012.Result cMemoryGetItemIcon(ushort prgId, short handle, short item, ref short pHandle, out byte[] image)
		{
            // TODO
            image = new byte[0];
			return lms2012.Result.OK;
		}

		public sbyte cMemoryGetCacheName(byte item, byte maxLen, byte[] filename, byte[] name)
        {
            // TODO
            return 0;
        }

        public lms2012.Result cMemoryGetFolderItems(ushort prgId, short handle, ref short items)
        {
            // TODO
            return lms2012.Result.OK;
        }

        public byte cMemoryGetCacheFiles()
        {
            // TODO
            return 0;
        }
    }
}
