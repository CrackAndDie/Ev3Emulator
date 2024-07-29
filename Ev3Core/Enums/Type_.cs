namespace Ev3Core.Enums
{
	// Reserved device types
	public enum TYPE
	{
		//  TYPE_KEEP                     =   0,  //!< Type value that won't change type in byte codes
		TYPE_NXT_TOUCH = 1,  //!< Device is NXT touch sensor
		TYPE_NXT_LIGHT = 2,  //!< Device is NXT light sensor
		TYPE_NXT_SOUND = 3,  //!< Device is NXT sound sensor
		TYPE_NXT_COLOR = 4,  //!< Device is NXT color sensor

		TYPE_TACHO = 7,  //!< Device is a tacho motor
		TYPE_MINITACHO = 8,  //!< Device is a mini tacho motor
		TYPE_NEWTACHO = 9,  //!< Device is a new tacho motor

		TYPE_THIRD_PARTY_START = 50,
		TYPE_THIRD_PARTY_END = 99,

		TYPE_IIC_UNKNOWN = 100,

		TYPE_NXT_TEST = 101,  //!< Device is a NXT ADC test sensor

		TYPE_NXT_IIC = 123,  //!< Device is NXT IIC sensor
		TYPE_TERMINAL = 124,  //!< Port is connected to a terminal
		TYPE_UNKNOWN = 125,  //!< Port not empty but type has not been determined
		TYPE_NONE = 126,  //!< Port empty or not available
		TYPE_ERROR = 127,  //!< Port not empty and type is invalid
	}
}

namespace Ev3Core
{
	public partial class Defines
	{
		public const int TYPE_NXT_TOUCH = 1;  //!< Device is NXT touch sensor
		public const int TYPE_NXT_LIGHT = 2;  //!< Device is NXT light sensor
		public const int TYPE_NXT_SOUND = 3;  //!< Device is NXT sound sensor
		public const int TYPE_NXT_COLOR = 4;  //!< Device is NXT color sensor

		public const int TYPE_TACHO = 7;  //!< Device is a tacho motor
		public const int TYPE_MINITACHO = 8;  //!< Device is a mini tacho motor
		public const int TYPE_NEWTACHO = 9;  //!< Device is a new tacho motor

		public const int TYPE_THIRD_PARTY_START = 50;
		public const int TYPE_THIRD_PARTY_END = 99;

		public const int TYPE_IIC_UNKNOWN = 100;

		public const int TYPE_NXT_TEST = 101;  //!< Device is a NXT ADC test sensor

		public const int TYPE_NXT_IIC = 123;  //!< Device is NXT IIC sensor
		public const int TYPE_TERMINAL = 124;  //!< Port is connected to a terminal
		public const int TYPE_UNKNOWN = 125;  //!< Port not empty but type has not been determined
		public const int TYPE_NONE = 126;  //!< Port empty or not available
		public const int TYPE_ERROR = 127;  //!< Port not empty and type is invalid
	}
}
