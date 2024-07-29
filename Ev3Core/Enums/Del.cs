namespace Ev3Core.Enums
{
	public enum DEL
	{
		DEL_NONE = 0,                    //!< No delimiter at all
		DEL_TAB = 1,                    //!< Use tab as delimiter
		DEL_SPACE = 2,                    //!< Use space as delimiter
		DEL_RETURN = 3,                    //!< Use return as delimiter
		DEL_COLON = 4,                    //!< Use colon as delimiter
		DEL_COMMA = 5,                    //!< Use comma as delimiter
		DEL_LINEFEED = 6,                    //!< Use line feed as delimiter
		DEL_CRLF = 7,                    //!< Use return+line feed as delimiter

		DELS
	}
}

namespace Ev3Core
{
	public partial class Defines
	{
		public const int DEL_NONE = 0;                    //!< No delimiter at all
		public const int DEL_TAB = 1;                    //!< Use tab as delimiter
		public const int DEL_SPACE = 2;                    //!< Use space as delimiter
		public const int DEL_RETURN = 3;                    //!< Use return as delimiter
		public const int DEL_COLON = 4;                    //!< Use colon as delimiter
		public const int DEL_COMMA = 5;                    //!< Use comma as delimiter
		public const int DEL_LINEFEED = 6;                    //!< Use line feed as delimiter
		public const int DEL_CRLF = 7;                    //!< Use return+line feed as delimiter

		public const int DELS = 8;
	}
}
