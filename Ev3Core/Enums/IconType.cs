namespace Ev3Core.Enums
{
	public enum ICONTYPE
	{
		NORMAL_ICON = 0,    //!< "24x12_Files_Folders_Settings.bmp"
		SMALL_ICON = 1,
		LARGE_ICON = 2,    //!< "24x22_Yes_No_OFF_FILEOps.bmp"
		MENU_ICON = 3,
		ARROW_ICON = 4,    //!< "8x12_miniArrows.bmp"

		ICONTYPES                             //!< Maximum icon types supported by the VM
	}
}

namespace Ev3Core
{
	public partial class Defines
	{
		public const int NORMAL_ICON = 0;    //!< "24x12_Files_Folders_Settings.bmp"
		public const int SMALL_ICON = 1;
		public const int LARGE_ICON = 2;    //!< "24x22_Yes_No_OFF_FILEOps.bmp"
		public const int MENU_ICON = 3;
		public const int ARROW_ICON = 4;    //!< "8x12_miniArrows.bmp"

		public const int ICONTYPES = 5;                            //!< Maximum icon types supported by the VM
	}
}
