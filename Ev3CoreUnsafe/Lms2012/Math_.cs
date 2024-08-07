using Ev3CoreUnsafe.Lms2012.Interfaces;
using static Ev3CoreUnsafe.Defines;

namespace Ev3CoreUnsafe.Lms2012
{
	public unsafe class Math_ : IMath
	{
		/*! \page cMath Math
		 *  <hr size="1"/>
		 *  <b>     opADD8 (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- Add two 8 bit values DESTINATION = SOURCE1 + SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   SOURCE1
		 *  \param  (DATA8)   SOURCE2
		 *  \return (DATA8)   DESTINATION
		 */
		/*! \brief  opADD8
		 *
		 *
		 */
		public void cMathAdd8()
		{
			DATA8 Tmp;

			Tmp = *(DATA8*)GH.Lms.PrimParPointer();
			Tmp += *(DATA8*)GH.Lms.PrimParPointer();
			*(DATA8*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page cMath
		 *  <hr size="1"/>
		 *  <b>     opADD16 (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- Add two 16 bit values DESTINATION = SOURCE1 + SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  SOURCE1
		 *  \param  (DATA16)  SOURCE2
		 *  \return (DATA16)  DESTINATION
		 */
		/*! \brief  opADD16
		 *
		 *
		 */
		public void cMathAdd16()
		{
			DATA16 Tmp;

			Tmp = *(DATA16*)GH.Lms.PrimParPointer();
			Tmp += *(DATA16*)GH.Lms.PrimParPointer();
			*(DATA16*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page cMath
		 *  <hr size="1"/>
		 *  <b>     opADD32 (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- Add two 32 bit values DESTINATION = SOURCE1 + SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  SOURCE1
		 *  \param  (DATA32)  SOURCE2
		 *  \return (DATA32)  DESTINATION
		 */
		/*! \brief  opADD32
		 *
		 *
		 */
		public void cMathAdd32()
		{
			DATA32 Tmp;

			Tmp = *(DATA32*)GH.Lms.PrimParPointer();
			Tmp += *(DATA32*)GH.Lms.PrimParPointer();
			*(DATA32*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page cMath
		 *  <hr size="1"/>
		 *  <b>     opADDF (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- Add two floating point values DESTINATION = SOURCE1 + SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATAF)   SOURCE1
		 *  \param  (DATAF)   SOURCE2
		 *  \return (DATAF)   DESTINATION
		 */
		/*! \brief  opADDF
		 *
		 *
		 */
		public void cMathAddF()
		{
			DATAF Tmp;

			Tmp = *(DATAF*)GH.Lms.PrimParPointer();
			Tmp += *(DATAF*)GH.Lms.PrimParPointer();
			*(DATAF*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page cMath
		 *  <hr size="1"/>
		 *  <b>     opSUB8 (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- Subtract two 8 bit values DESTINATION = SOURCE1 - SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   SOURCE1
		 *  \param  (DATA8)   SOURCE2
		 *  \return (DATA8)   DESTINATION
		 */
		/*! \brief  opSUB8
		 *
		 *
		 */
		public void cMathSub8()
		{
			DATA8 Tmp;

			Tmp = *(DATA8*)GH.Lms.PrimParPointer();
			Tmp -= *(DATA8*)GH.Lms.PrimParPointer();
			*(DATA8*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page cMath
		 *  <hr size="1"/>
		 *  <b>     opSUB16 (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- Subtract two 16 bit values DESTINATION = SOURCE1 - SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  SOURCE1
		 *  \param  (DATA16)  SOURCE2
		 *  \return (DATA16)  DESTINATION
		 */
		/*! \brief  opSUB16
		 *
		 *
		 */
		public void cMathSub16()
		{
			DATA16 Tmp;

			Tmp = *(DATA16*)GH.Lms.PrimParPointer();
			Tmp -= *(DATA16*)GH.Lms.PrimParPointer();
			*(DATA16*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page cMath
		 *  <hr size="1"/>
		 *  <b>     opSUB32 (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- Subtract two 32 bit values DESTINATION = SOURCE1 - SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  SOURCE1
		 *  \param  (DATA32)  SOURCE2
		 *  \return (DATA32)  DESTINATION
		 */
		/*! \brief  opSUB32
		 *
		 *
		 */
		public void cMathSub32()
		{
			DATA32 Tmp;

			Tmp = *(DATA32*)GH.Lms.PrimParPointer();
			Tmp -= *(DATA32*)GH.Lms.PrimParPointer();
			*(DATA32*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page cMath
		 *  <hr size="1"/>
		 *  <b>     opSUBF (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- Subtract two floating point values DESTINATION = SOURCE1 - SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATAF)   SOURCE1
		 *  \param  (DATAF)   SOURCE2
		 *  \return (DATAF)   DESTINATION
		 */
		/*! \brief  opSUBF
		 *
		 *
		 */
		public void cMathSubF()
		{
			DATAF Tmp;

			Tmp = *(DATAF*)GH.Lms.PrimParPointer();
			Tmp -= *(DATAF*)GH.Lms.PrimParPointer();
			*(DATAF*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page cMath
		 *  <hr size="1"/>
		 *  <b>     opMUL8 (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- Multiply two 8 bit values DESTINATION = SOURCE1 * SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   SOURCE1
		 *  \param  (DATA8)   SOURCE2
		 *  \return (DATA8)   DESTINATION
		 */
		/*! \brief  opMUL8
		 *
		 *
		 */
		public void cMathMul8()
		{
			DATA8 Tmp;

			Tmp = *(DATA8*)GH.Lms.PrimParPointer();
			Tmp *= *(DATA8*)GH.Lms.PrimParPointer();
			*(DATA8*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page cMath
		 *  <hr size="1"/>
		 *  <b>     opMUL16 (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- Multiply two 16 bit values DESTINATION = SOURCE1 * SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  SOURCE1
		 *  \param  (DATA16)  SOURCE2
		 *  \return (DATA16)  DESTINATION
		 */
		/*! \brief  opMUL16
		 *
		 *
		 */
		public void cMathMul16()
		{
			DATA16 Tmp;

			Tmp = *(DATA16*)GH.Lms.PrimParPointer();
			Tmp *= *(DATA16*)GH.Lms.PrimParPointer();
			*(DATA16*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page cMath
		 *  <hr size="1"/>
		 *  <b>     opMUL32 (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- Multiply two 32 bit values DESTINATION = SOURCE1 * SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  SOURCE1
		 *  \param  (DATA32)  SOURCE2
		 *  \return (DATA32)  DESTINATION
		 */
		/*! \brief  opMUL32
		 *
		 *
		 */
		public void cMathMul32()
		{
			DATA32 Tmp;

			Tmp = *(DATA32*)GH.Lms.PrimParPointer();
			Tmp *= *(DATA32*)GH.Lms.PrimParPointer();
			*(DATA32*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page cMath
		 *  <hr size="1"/>
		 *  <b>     opMULF (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- Multiply two floating point values DESTINATION = SOURCE1 * SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATAF)   SOURCE1
		 *  \param  (DATAF)   SOURCE2
		 *  \return (DATAF)   DESTINATION
		 */
		/*! \brief  opMULF
		 *
		 *
		 */
		public void cMathMulF()
		{
			DATAF Tmp;

			Tmp = *(DATAF*)GH.Lms.PrimParPointer();
			Tmp *= *(DATAF*)GH.Lms.PrimParPointer();
			*(DATAF*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page cMath
		 *  <hr size="1"/>
		 *  <b>     opDIV8 (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- Divide two 8 bit values DESTINATION = SOURCE1 / SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   SOURCE1
		 *  \param  (DATA8)   SOURCE2
		 *  \return (DATA8)   DESTINATION
		 */
		/*! \brief  opDIV8
		 *
		 *
		 */
		public void cMathDiv8()
		{
			DATA8 X, Y;

			X = *(DATA8*)GH.Lms.PrimParPointer();
			Y = *(DATA8*)GH.Lms.PrimParPointer();
			if (Y == 0)
			{
				if (X > 0)
				{
					X = (DATA8)DATA8_MAX;
				}
				if (X < 0)
				{
					X = (DATA8)DATA8_MIN;
				}
			}
			else
			{
				X /= Y;
			}
			*(DATA8*)GH.Lms.PrimParPointer() = X;
		}


		/*! \page cMath
		 *  <hr size="1"/>
		 *  <b>     opDIV16 (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- Divide two 16 bit values DESTINATION = SOURCE1 / SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  SOURCE1
		 *  \param  (DATA16)  SOURCE2
		 *  \return (DATA16)  DESTINATION
		 */
		/*! \brief  opDIV16
		 *
		 *
		 */
		public void cMathDiv16()
		{
			DATA16 X, Y;

			X = *(DATA16*)GH.Lms.PrimParPointer();
			Y = *(DATA16*)GH.Lms.PrimParPointer();
			if (Y == 0)
			{
				if (X > 0)
				{
					X = (DATA16)DATA16_MAX;
				}
				if (X < 0)
				{
					X = (DATA16)DATA16_MIN;
				}
			}
			else
			{
				X /= Y;
			}
			*(DATA16*)GH.Lms.PrimParPointer() = X;
		}


		/*! \page cMath
		 *  <hr size="1"/>
		 *  <b>     opDIV32 (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- Divide two 32 bit values DESTINATION = SOURCE1 / SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  SOURCE1
		 *  \param  (DATA32)  SOURCE2
		 *  \return (DATA32)  DESTINATION
		 */
		/*! \brief  opDIV32
		 *
		 *
		 */
		public void cMathDiv32()
		{
			DATA32 X, Y;

			X = *(DATA32*)GH.Lms.PrimParPointer();
			Y = *(DATA32*)GH.Lms.PrimParPointer();
			if (Y == 0)
			{
				if (X > 0)
				{
					X = (DATA32)DATA32_MAX;
				}
				if (X < 0)
				{
					X = (DATA32)DATA32_MIN;
				}
			}
			else
			{
				X /= Y;
			}
			*(DATA32*)GH.Lms.PrimParPointer() = X;
		}


		/*! \page cMath
		 *  <hr size="1"/>
		 *  <b>     opDIVF (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- Divide two floating point values DESTINATION = SOURCE1 / SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATAF)   SOURCE1
		 *  \param  (DATAF)   SOURCE2
		 *  \return (DATAF)   DESTINATION
		 */
		/*! \brief  opDIVF
		 *
		 *
		 */
		public void cMathDivF()
		{
			DATAF X, Y;

			X = *(DATAF*)GH.Lms.PrimParPointer();
			Y = *(DATAF*)GH.Lms.PrimParPointer();
			X /= Y;
			*(DATAF*)GH.Lms.PrimParPointer() = X;
		}


		/*! \page Logic Logic
		 *  <hr size="1"/>
		 *  <b>     opOR8 (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- Or two 8 bit values DESTINATION = SOURCE1 | SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   SOURCE1
		 *  \param  (DATA8)   SOURCE2
		 *  \return (DATA8)   DESTINATION
		 */
		/*! \brief  opOR8
		 *
		 *
		 */
		public void cMathOr8()
		{
			DATA8 Tmp;

			Tmp = *(DATA8*)GH.Lms.PrimParPointer();
			Tmp |= *(DATA8*)GH.Lms.PrimParPointer();
			*(DATA8*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page Logic
		 *  <hr size="1"/>
		 *  <b>     opOR16 (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- Or two 16 bit values DESTINATION = SOURCE1 | SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  SOURCE1
		 *  \param  (DATA16)  SOURCE2
		 *  \return (DATA16)  DESTINATION
		 */
		/*! \brief  opOR16
		 *
		 *
		 */
		public void cMathOr16()
		{
			DATA16 Tmp;

			Tmp = *(DATA16*)GH.Lms.PrimParPointer();
			Tmp |= *(DATA16*)GH.Lms.PrimParPointer();
			*(DATA16*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page Logic
		 *  <hr size="1"/>
		 *  <b>     opOR32 (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- Or two 32 bit values DESTINATION = SOURCE1 | SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  SOURCE1
		 *  \param  (DATA32)  SOURCE2
		 *  \return (DATA32)  DESTINATION
		 */
		/*! \brief  opOR32
		 *
		 *
		 */
		public void cMathOr32()
		{
			DATA32 Tmp;

			Tmp = *(DATA32*)GH.Lms.PrimParPointer();
			Tmp |= *(DATA32*)GH.Lms.PrimParPointer();
			*(DATA32*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page Logic
		 *  <hr size="1"/>
		 *  <b>     opAND8 (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- And two 8 bit values DESTINATION = SOURCE1 & SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   SOURCE1
		 *  \param  (DATA8)   SOURCE2
		 *  \return (DATA8)   DESTINATION
		 */
		/*! \brief  opAND8
		 *
		 *
		 */
		public void cMathAnd8()
		{
			DATA8 Tmp;

			Tmp = *(DATA8*)GH.Lms.PrimParPointer();
			Tmp &= *(DATA8*)GH.Lms.PrimParPointer();
			*(DATA8*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page Logic
		 *  <hr size="1"/>
		 *  <b>     opAND16 (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- And two 16 bit values DESTINATION = SOURCE1 & SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  SOURCE1
		 *  \param  (DATA16)  SOURCE2
		 *  \return (DATA16)  DESTINATION
		 */
		/*! \brief  opAND16
		 *
		 *
		 */
		public void cMathAnd16()
		{
			DATA16 Tmp;

			Tmp = *(DATA16*)GH.Lms.PrimParPointer();
			Tmp &= *(DATA16*)GH.Lms.PrimParPointer();
			*(DATA16*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page Logic
		 *  <hr size="1"/>
		 *  <b>     opAND32 (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- And two 32 bit values DESTINATION = SOURCE1 & SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  SOURCE1
		 *  \param  (DATA32)  SOURCE2
		 *  \return (DATA32)  DESTINATION
		 */
		/*! \brief  opAND32
		 *
		 *
		 */
		public void cMathAnd32()
		{
			DATA32 Tmp;

			Tmp = *(DATA32*)GH.Lms.PrimParPointer();
			Tmp &= *(DATA32*)GH.Lms.PrimParPointer();
			*(DATA32*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page Logic
		 *  <hr size="1"/>
		 *  <b>     opXOR8 (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- Exclusive or two 8 bit values DESTINATION = SOURCE1 ^ SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   SOURCE1
		 *  \param  (DATA8)   SOURCE2
		 *  \return (DATA8)   DESTINATION
		 */
		/*! \brief  opXOR8
		 *
		 *
		 */
		public void cMathXor8()
		{
			DATA8 Tmp;

			Tmp = *(DATA8*)GH.Lms.PrimParPointer();
			Tmp ^= *(DATA8*)GH.Lms.PrimParPointer();
			*(DATA8*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page Logic
		 *  <hr size="1"/>
		 *  <b>     opXOR16 (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- Exclusive or two 16 bit values DESTINATION = SOURCE1 ^ SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  SOURCE1
		 *  \param  (DATA16)  SOURCE2
		 *  \return (DATA16)  DESTINATION
		 */
		/*! \brief  opXOR16
		 *
		 *
		 */
		public void cMathXor16()
		{
			DATA16 Tmp;

			Tmp = *(DATA16*)GH.Lms.PrimParPointer();
			Tmp ^= *(DATA16*)GH.Lms.PrimParPointer();
			*(DATA16*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page Logic
		 *  <hr size="1"/>
		 *  <b>     opXOR32 (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- Exclusive or two 32 bit values DESTINATION = SOURCE1 ^ SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  SOURCE1
		 *  \param  (DATA32)  SOURCE2
		 *  \return (DATA32)  DESTINATION
		 */
		/*! \brief  opXOR32
		 *
		 *
		 */
		public void cMathXor32()
		{
			DATA32 Tmp;

			Tmp = *(DATA32*)GH.Lms.PrimParPointer();
			Tmp ^= *(DATA32*)GH.Lms.PrimParPointer();
			*(DATA32*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page Logic
		 *  <hr size="1"/>
		 *  <b>     opRL8 (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- Rotate left 8 bit value DESTINATION = SOURCE1 << SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   SOURCE1
		 *  \param  (DATA8)   SOURCE2
		 *  \return (DATA8)   DESTINATION
		 */
		/*! \brief  opRL8
		 *
		 *
		 */
		public void cMathRl8()
		{
			DATA8 Tmp;

			Tmp = *(DATA8*)GH.Lms.PrimParPointer();
			Tmp <<= *(DATA8*)GH.Lms.PrimParPointer();
			*(DATA8*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page Logic
		 *  <hr size="1"/>
		 *  <b>     opRL16 (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- Rotate left 16 bit value DESTINATION = SOURCE1 << SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  SOURCE1
		 *  \param  (DATA16)  SOURCE2
		 *  \return (DATA16)  DESTINATION
		 */
		/*! \brief  opRL16
		 *
		 *
		 */
		public void cMathRl16()
		{
			DATA16 Tmp;

			Tmp = *(DATA16*)GH.Lms.PrimParPointer();
			Tmp <<= *(DATA16*)GH.Lms.PrimParPointer();
			*(DATA16*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page Logic
		 *  <hr size="1"/>
		 *  <b>     opRL32 (SOURCE1, SOURCE2, DESTINATION)  </b>
		 *
		 *- Rotate left 32 bit value DESTINATION = SOURCE1 << SOURCE2\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  SOURCE1
		 *  \param  (DATA32)  SOURCE2
		 *  \return (DATA32)  DESTINATION
		 */
		/*! \brief  opRL32
		 *
		 *
		 */
		public void cMathRl32()
		{
			DATA32 Tmp;

			Tmp = *(DATA32*)GH.Lms.PrimParPointer();
			Tmp <<= *(DATA32*)GH.Lms.PrimParPointer();
			*(DATA32*)GH.Lms.PrimParPointer() = Tmp;
		}

		public double DegToRad(double deg) => deg * (3.14 / 180);
		public double RadToDeg(double rad) => rad * (180 / 3.14);


		/*! \page cMath
		 *  <hr size="1"/>
		 *  <b>     opMATH (CMD, ....)  </b>
		 *
		 *- Math function entry\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   CMD               - \ref mathsubcode
		 *
		 *\n
		 *  - CMD = EXP
		 *\n  e^X (R = expf(X))\n
		 *    -  \param  (DATAF)    X           -
		 *    -  \return (DATAF)    R           -
		 *
		 *\n
		 *  - CMD = POW
		 *\n  Exponent (R = powf(X,Y))\n
		 *    -  \param  (DATAF)    X           -
		 *    -  \param  (DATAF)    Y           -
		 *    -  \return (DATAF)    R           -
		 *
		 *\n
		 *  - CMD = MOD8
		 *\n  Modulo (R = X % Y)\n
		 *    -  \param  (DATA8)    X           -
		 *    -  \param  (DATA8)    Y           -
		 *    -  \return (DATA8)    R           -
		 *
		 *\n
		 *  - CMD = MOD16
		 *\n  Modulo (R = X % Y)\n
		 *    -  \param  (DATA16)   X           -
		 *    -  \param  (DATA16)   Y           -
		 *    -  \return (DATA16)   R           -
		 *
		 *\n
		 *  - CMD = MOD32
		 *\n  Modulo (R = X % Y)\n
		 *    -  \param  (DATA32)   X           -
		 *    -  \param  (DATA32)   Y           -
		 *    -  \return (DATA32)   R           -
		 *
		 *\n
		 *  - CMD = MOD
		 *\n  Modulo (R = fmod(X,Y))\n
		 *    -  \param  (DATAF)    X           -
		 *    -  \param  (DATAF)    Y           -
		 *    -  \return (DATAF)    R           -
		 *
		 *\n
		 *  - CMD = FLOOR
		 *\n  Floor (R = floor(X))\n
		 *    -  \param  (DATAF)    X           -
		 *    -  \return (DATAF)    R           -
		 *
		 *\n
		 *  - CMD = CEIL
		 *\n  Ceil (R = ceil(X))\n
		 *    -  \param  (DATAF)    X           -
		 *    -  \return (DATAF)    R           -
		 *
		 *\n
		 *  - CMD = ROUND
		 *\n  Round (R = round(X))\n
		 *    -  \param  (DATAF)    X           -
		 *    -  \return (DATAF)    R           -
		 *
		 *\n
		 *  - CMD = ABS
		 *\n  Absolut (R = fabs(X))\n
		 *    -  \param  (DATAF)    X           -
		 *    -  \return (DATAF)    R           -
		 *
		 *\n
		 *  - CMD = NEGATE
		 *\n  Negate (R = 0.0 - X)\n
		 *    -  \param  (DATAF)    X           -
		 *    -  \return (DATAF)    R           -
		 *
		 *\n
		 *  - CMD = TRUNC
		 *\n  Truncate\n
		 *    -  \param  (DATAF)    X           - Value
		 *    -  \param  (DATA8)    P           - Precision [0..9]
		 *    -  \return (DATAF)    R           - Result
		 *
		 *\n
		 *  - CMD = SQRT
		 *\n  Squareroot (R = sqrt(X))\n
		 *    -  \param  (DATAF)    X           -
		 *    -  \return (DATAF)    R           -
		 *
		 *\n
		 *  - CMD = LOG
		 *\n  Log (R = log10(X))\n
		 *    -  \param  (DATAF)    X           -
		 *    -  \return (DATAF)    R           -
		 *
		 *\n
		 *  - CMD = LN
		 *\n  Ln (R = log(X))\n
		 *    -  \param  (DATAF)    X           -
		 *    -  \return (DATAF)    R           -
		 *
		 *\n
		 *  - CMD = SIN
		 *\n  Sin (R = sinf(X))\n
		 *    -  \param  (DATAF)    X           -
		 *    -  \return (DATAF)    R           -
		 *
		 *\n
		 *  - CMD = COS
		 *\n  Cos (R = cos(X))\n
		 *    -  \param  (DATAF)    X           -
		 *    -  \return (DATAF)    R           -
		 *
		 *\n
		 *  - CMD = TAN
		 *\n  Tan (R = tanf(X))\n
		 *    -  \param  (DATAF)    X           -
		 *    -  \return (DATAF)    R           -
		 *
		 *\n
		 *  - CMD = ASIN
		 *\n  ASin (R = asinf(X))\n
		 *    -  \param  (DATAF)    X           -
		 *    -  \return (DATAF)    R           -
		 *
		 *\n
		 *  - CMD = ACOS
		 *\n  ACos (R = acos(X))\n
		 *    -  \param  (DATAF)    X           -
		 *    -  \return (DATAF)    R           -
		 *
		 *\n
		 *  - CMD = ATAN
		 *\n  ATan (R = atanf(X))\n
		 *    -  \param  (DATAF)    X           -
		 *    -  \return (DATAF)    R           -
		 *
		 *\n
		 *
		 */
		/*! \brief  opMATH
		 *
		 *
		 */
		public void cMath()
		{
			DATAF X;
			DATAF Y;
			DATA8 X8;
			DATA8 Y8;
			DATA16 X16;
			DATA16 Y16;
			DATA32 X32;
			DATA32 Y32;
			DATA8 Cmd;
			//char Buf[64];
			//char* pBuf;

			Cmd = *(DATA8*)GH.Lms.PrimParPointer();

			switch (Cmd)
			{
				case EXP:
					{
						X = *(DATAF*)GH.Lms.PrimParPointer();
						*(DATAF*)GH.Lms.PrimParPointer() = (float)Math.Exp(X);
					}
					break;

				case POW:
					{
						X = *(DATAF*)GH.Lms.PrimParPointer();
						Y = *(DATAF*)GH.Lms.PrimParPointer();
						*(DATAF*)GH.Lms.PrimParPointer() = (float)Math.Pow(X, Y);
					}
					break;

				case MOD8:
					{
						X8 = *(DATA8*)GH.Lms.PrimParPointer();
						Y8 = *(DATA8*)GH.Lms.PrimParPointer();
						*(DATA8*)GH.Lms.PrimParPointer() = (sbyte)(X8 % Y8);
					}
					break;

				case MOD16:
					{
						X16 = *(DATA16*)GH.Lms.PrimParPointer();
						Y16 = *(DATA16*)GH.Lms.PrimParPointer();
						*(DATA16*)GH.Lms.PrimParPointer() = (short)(X16 % Y16);
					}
					break;

				case MOD32:
					{
						X32 = *(DATA32*)GH.Lms.PrimParPointer();
						Y32 = *(DATA32*)GH.Lms.PrimParPointer();
						*(DATA32*)GH.Lms.PrimParPointer() = X32 % Y32;
					}
					break;

				case MOD:
					{
						X = *(DATAF*)GH.Lms.PrimParPointer();
						Y = *(DATAF*)GH.Lms.PrimParPointer();
						*(DATAF*)GH.Lms.PrimParPointer() = X % Y;
					}
					break;

				case FLOOR:
					{
						X = *(DATAF*)GH.Lms.PrimParPointer();
						*(DATAF*)GH.Lms.PrimParPointer() = (float)Math.Floor(X);
					}
					break;

				case CEIL:
					{
						X = *(DATAF*)GH.Lms.PrimParPointer();
						*(DATAF*)GH.Lms.PrimParPointer() = (float)Math.Ceiling(X);
					}
					break;

				case ROUND:
					{
						X = *(DATAF*)GH.Lms.PrimParPointer();
						if (X < (DATAF)0)
						{
							*(DATAF*)GH.Lms.PrimParPointer() = (float)Math.Ceiling(X - (DATAF)0.5);
						}
						else
						{
							*(DATAF*)GH.Lms.PrimParPointer() = (float)Math.Floor(X + (DATAF)0.5);
						}
					}
					break;

				case ABS:
					{
						X = *(DATAF*)GH.Lms.PrimParPointer();
						*(DATAF*)GH.Lms.PrimParPointer() = (float)Math.Abs(X);
					}
					break;

				case NEGATE:
					{
						X = *(DATAF*)GH.Lms.PrimParPointer();
						*(DATAF*)GH.Lms.PrimParPointer() = (DATAF)0 - X;
					}
					break;

				case TRUNC:
					{
						X = *(DATAF*)GH.Lms.PrimParPointer();
						Y8 = *(DATA8*)GH.Lms.PrimParPointer();

						if (Y8 > 9)
						{
							Y8 = 9;
						}
						if (Y8 < 0)
						{
							Y8 = 0;
						}

						// TODO: do i need it?
						//snprintf(Buf, 64, "%f", X);

						//pBuf = strstr(Buf, ".");
						//if (pBuf != NULL)
						//{
						//	pBuf[Y8 + 1] = 0;
						//}
						//sscanf(Buf, "%f", &X);
						*(DATAF*)GH.Lms.PrimParPointer() = X;
					}
					break;

				case SQRT:
					{
						X = *(DATAF*)GH.Lms.PrimParPointer();
						*(DATAF*)GH.Lms.PrimParPointer() = (float)Math.Sqrt(X);
					}
					break;

				case LOG:
					{
						X = *(DATAF*)GH.Lms.PrimParPointer();
						*(DATAF*)GH.Lms.PrimParPointer() = (float)Math.Log10(X);
					}
					break;

				case LN:
					{
						X = *(DATAF*)GH.Lms.PrimParPointer();
						*(DATAF*)GH.Lms.PrimParPointer() = (float)Math.Log(X);
					}
					break;

				case SIN:
					{
						X = *(DATAF*)GH.Lms.PrimParPointer();
						X = (float)DegToRad(X);
						X = (float)Math.Sin(X);
						*(DATAF*)GH.Lms.PrimParPointer() = X;
					}
					break;

				case COS:
					{
						X = *(DATAF*)GH.Lms.PrimParPointer();
						X = (float)DegToRad(X);
						X = (float)Math.Cos(X);
						*(DATAF*)GH.Lms.PrimParPointer() = X;
					}
					break;

				case TAN:
					{
						X = *(DATAF*)GH.Lms.PrimParPointer();
						X = (float)DegToRad(X);
						X = (float)Math.Tan(X);
						*(DATAF*)GH.Lms.PrimParPointer() = X;
					}
					break;

				case ASIN:
					{
						X = *(DATAF*)GH.Lms.PrimParPointer();
						X = (float)Math.Asin(X);
						X = (float)RadToDeg(X);
						*(DATAF*)GH.Lms.PrimParPointer() = X;
					}
					break;

				case ACOS:
					{
						X = *(DATAF*)GH.Lms.PrimParPointer();
						X = (float)Math.Acos(X);
						X = (float)RadToDeg(X);
						*(DATAF*)GH.Lms.PrimParPointer() = X;
					}
					break;

				case ATAN:
					{
						X = *(DATAF*)GH.Lms.PrimParPointer();
						X = (float)Math.Atan(X);
						X = (float)RadToDeg(X);
						*(DATAF*)GH.Lms.PrimParPointer() = X;
					}
					break;

			}

		}
	}
}
