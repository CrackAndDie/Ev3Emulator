namespace Ev3Core.Enums
{
	public enum BROWSERTYPE
	{
		BROWSE_FOLDERS = 0,    //!< Browser for folders
		BROWSE_FOLDS_FILES = 1,    //!< Browser for folders and files
		BROWSE_CACHE = 2,    //!< Browser for cached / recent files
		BROWSE_FILES = 3,    //!< Browser for files

		BROWSERTYPES                          //!< Maximum font types supported by the VM
	}
}

namespace Ev3Core
{
	public partial class Defines
	{
		public const int BROWSE_FOLDERS = 0;    //!< Browser for folders
		public const int BROWSE_FOLDS_FILES = 1;    //!< Browser for folders and files
		public const int BROWSE_CACHE = 2;    //!< Browser for cached / recent files
		public const int BROWSE_FILES = 3;    //!< Browser for files

		public const int BROWSERTYPES = 4;                         //!< Maximum font types supported by the VM
	}
}
