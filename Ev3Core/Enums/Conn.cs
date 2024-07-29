namespace Ev3Core.Enums
{
	public enum CONN
	{
		CONN_UNKNOWN = 111,  //!< Connection is fake (test)

		CONN_DAISYCHAIN = 117,  //!< Connection is daisy chained
		CONN_NXT_COLOR = 118,  //!< Connection type is NXT color sensor
		CONN_NXT_DUMB = 119,  //!< Connection type is NXT analog sensor
		CONN_NXT_IIC = 120,  //!< Connection type is NXT IIC sensor

		CONN_INPUT_DUMB = 121,  //!< Connection type is LMS2012 input device with ID resistor
		CONN_INPUT_UART = 122,  //!< Connection type is LMS2012 UART sensor

		CONN_OUTPUT_DUMB = 123,  //!< Connection type is LMS2012 output device with ID resistor
		CONN_OUTPUT_INTELLIGENT = 124,  //!< Connection type is LMS2012 output device with communication
		CONN_OUTPUT_TACHO = 125,  //!< Connection type is LMS2012 tacho motor with series ID resistance

		CONN_NONE = 126,  //!< Port empty or not available
		CONN_ERROR = 127,  //!< Port not empty and type is invalid
	}
}

namespace Ev3Core
{
	public partial class Defines
	{
		public const int CONN_UNKNOWN = 111;  //!< Connection is fake (test)

		public const int CONN_DAISYCHAIN = 117;  //!< Connection is daisy chained
		public const int CONN_NXT_COLOR = 118;  //!< Connection type is NXT color sensor
		public const int CONN_NXT_DUMB = 119;  //!< Connection type is NXT analog sensor
		public const int CONN_NXT_IIC = 120;  //!< Connection type is NXT IIC sensor

		public const int CONN_INPUT_DUMB = 121;  //!< Connection type is LMS2012 input device with ID resistor
		public const int CONN_INPUT_UART = 122;  //!< Connection type is LMS2012 UART sensor

		public const int CONN_OUTPUT_DUMB = 123;  //!< Connection type is LMS2012 output device with ID resistor
		public const int CONN_OUTPUT_INTELLIGENT = 124;  //!< Connection type is LMS2012 output device with communication
		public const int CONN_OUTPUT_TACHO = 125;  //!< Connection type is LMS2012 tacho motor with series ID resistance

		public const int CONN_NONE = 126;  //!< Port empty or not available
		public const int CONN_ERROR = 127;  //!< Port not empty and type is invalid
	}
}


