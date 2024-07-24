using static EV3DecompilerLib.Decompile.lms2012;

namespace Ev3EmulatorCore.Lms.Cmemory
{
    public partial class CmemoryClass
    {
        public unsafe void cMemoryCloseFolder(PRGID prgId, HANDLER* handle)
        {
            // TODO
        }

        public unsafe Result cMemoryOpenFolder(PRGID prgId, DATA8 tp, DATA8* folderName, HANDLER* handle)
        {
            // TODO
            return Result.OK;
        }

		public DSPSTAT cMemoryCloseFile(PRGID prgId, HANDLER handle)
		{
            // TODO
            return DSPSTAT.NOBREAK;
		}

		public unsafe Result cMemoryGetItem(PRGID prgId, HANDLER handle, DATA16 item, DATA8 len, DATA8* name, DATA8* tp)
        {
            // TODO
            return Result.OK;
        }

        public unsafe Result cMemoryGetItemName(PRGID prgId, HANDLER handle, DATA16 item, DATA8 len, DATA8* name, DATA8* tp, DATA8* priority)
        {
            // TODO
            return Result.OK;
        }

		public unsafe Result cMemoryGetItemText(PRGID prgId, HANDLER handle, DATA16 item, DATA8 len, DATA8* text)
		{
			// TODO
			return Result.OK;
		}

		public unsafe Result cMemoryGetItemIcon(PRGID prgId, HANDLER handle, DATA16 item, HANDLER* pHandle, DATA32* image)
		{
            // TODO
			return Result.OK;
		}

		public unsafe DATA8 cMemoryGetCacheName(DATA8 item, DATA8 maxLen, DATA8* filename, DATA8* name)
        {
            // TODO
            return 0;
        }

        public unsafe Result cMemoryGetFolderItems(PRGID prgId, HANDLER handle, DATA16* items)
        {
            // TODO
            return Result.OK;
        }

        public DATA8 cMemoryGetCacheFiles()
        {
            // TODO
            return 0;
        }
    }
}
