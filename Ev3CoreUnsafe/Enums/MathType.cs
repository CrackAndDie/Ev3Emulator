namespace Ev3CoreUnsafe.Enums
{
	public enum MATHTYPE
	{
		EXP = 1,    //!< e^x            r = expf(x)
		MOD = 2,    //!< Modulo         r = fmod(x,y)
		FLOOR = 3,    //!< Floor          r = floor(x)
		CEIL = 4,    //!< Ceiling        r = ceil(x)
		ROUND = 5,    //!< Round          r = round(x)
		ABS = 6,    //!< Absolute       r = fabs(x)
		NEGATE = 7,    //!< Negate         r = 0.0 - x
		SQRT = 8,    //!< Squareroot     r = sqrt(x)
		LOG = 9,    //!< Log            r = log10(x)
		LN = 10,   //!< Ln             r = log(x)
		SIN = 11,   //!<
		COS = 12,   //!<
		TAN = 13,   //!<
		ASIN = 14,   //!<
		ACOS = 15,   //!<
		ATAN = 16,   //!<
		MOD8 = 17,   //!< Modulo DATA8   r = x % y
		MOD16 = 18,   //!< Modulo DATA16  r = x % y
		MOD32 = 19,   //!< Modulo DATA32  r = x % y
		POW = 20,   //!< Exponent       r = powf(x,y)
		TRUNC = 21,   //!< Truncate       r = (float)((int)(x * pow(y))) / pow(y)

		MATHTYPES                             //!< Maximum number of math functions supported by the VM
	}
}

namespace Ev3CoreUnsafe
{
	public partial class Defines
	{
		public const int EXP = 1;    //!< e^x            r = expf(x)
		public const int MOD = 2;    //!< Modulo         r = fmod(x;y)
		public const int FLOOR = 3;    //!< Floor          r = floor(x)
		public const int CEIL = 4;    //!< Ceiling        r = ceil(x)
		public const int ROUND = 5;    //!< Round          r = round(x)
		public const int ABS = 6;    //!< Absolute       r = fabs(x)
		public const int NEGATE = 7;    //!< Negate         r = 0.0 - x
		public const int SQRT = 8;    //!< Squareroot     r = sqrt(x)
		public const int LOG = 9;    //!< Log            r = log10(x)
		public const int LN = 10;   //!< Ln             r = log(x)
		public const int SIN = 11;   //!<
		public const int COS = 12;   //!<
		public const int TAN = 13;   //!<
		public const int ASIN = 14;   //!<
		public const int ACOS = 15;   //!<
		public const int ATAN = 16;   //!<
		public const int MOD8 = 17;   //!< Modulo DATA8   r = x % y
		public const int MOD16 = 18;   //!< Modulo DATA16  r = x % y
		public const int MOD32 = 19;   //!< Modulo DATA32  r = x % y
		public const int POW = 20;   //!< Exponent       r = powf(x;y)
		public const int TRUNC = 21;   //!< Truncate       r = (float)((int)(x * pow(y))) / pow(y)

		public const int MATHTYPES = 22;                            //!< Maximum number of math functions supported by the VM
	}
}
