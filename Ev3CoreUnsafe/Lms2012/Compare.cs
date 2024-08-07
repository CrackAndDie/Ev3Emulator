using Ev3CoreUnsafe.Lms2012.Interfaces;

namespace Ev3CoreUnsafe.Lms2012
{
	public unsafe class Compare : ICompare
	{
		/*! \page cCompare Compare
		 *  <hr size="1"/>
		 *  <b>     opCP_LT8 (LEFT, RIGHT, FLAG)  </b>
		 *
		 *- If LEFT is less than RIGTH - set FLAG \n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   LEFT
		 *  \param  (DATA8)   RIGHT
		 *  \return (DATA8)   FLAG
		 */
		/*! \brief  opCP_LT8 byte code
		 *
		 *
		 */
		public void cCompareLt8()
		{
			DATA8 Tmp;

			Tmp = *(DATA8*)GH.Lms.PrimParPointer();
			if (Tmp < *(DATA8*)GH.Lms.PrimParPointer())
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 1;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 0;
			}
		}


		/*! \page cCompare
		 *  <hr size="1"/>
		 *  <b>     opCP_LT16 (LEFT, RIGHT, FLAG)  </b>
		 *
		 *- If LEFT is less than RIGTH - set FLAG \n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  LEFT
		 *  \param  (DATA16)  RIGHT
		 *  \return (DATA8)   FLAG
		 */
		/*! \brief  opCP_LT16 byte code
		 *
		 *
		 */
		public void cCompareLt16()
		{
			DATA16 Tmp;

			Tmp = *(DATA16*)GH.Lms.PrimParPointer();
			if (Tmp < *(DATA16*)GH.Lms.PrimParPointer())
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 1;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 0;
			}
		}


		/*! \page cCompare
		 *  <hr size="1"/>
		 *  <b>     opCP_LT32 (LEFT, RIGHT, FLAG)  </b>
		 *
		 *- If LEFT is less than RIGTH - set FLAG \n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  LEFT
		 *  \param  (DATA32)  RIGHT
		 *  \return (DATA8)   FLAG
		 */
		/*! \brief  opCP_LT32 byte code
		 *
		 *
		 */
		public void cCompareLt32()
		{
			DATA32 Tmp;

			Tmp = *(DATA32*)GH.Lms.PrimParPointer();
			if (Tmp < *(DATA32*)GH.Lms.PrimParPointer())
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 1;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 0;
			}
		}


		/*! \page cCompare Compare
		 *  <hr size="1"/>
		 *  <b>     opCP_LTF (LEFT, RIGHT, FLAG)  </b>
		 *
		 *- If LEFT is less than RIGTH - set FLAG \n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATAF)   LEFT
		 *  \param  (DATAF)   RIGHT
		 *  \return (DATA8)   FLAG
		 */
		/*! \brief  opCP_LTF byte code
		 *
		 *
		 */
		public void cCompareLtF()
		{
			DATAF Tmp;

			Tmp = *(DATAF*)GH.Lms.PrimParPointer();
			if (Tmp < *(DATAF*)GH.Lms.PrimParPointer())
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 1;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 0;
			}
		}


		/*! \page cCompare
		 *  <hr size="1"/>
		 *  <b>     opCP_GT8 (LEFT, RIGHT, FLAG)  </b>
		 *
		 *- If LEFT is greater than RIGTH - set FLAG \n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   LEFT
		 *  \param  (DATA8)   RIGHT
		 *  \return (DATA8)   FLAG
		 */
		/*! \brief  opCP_GT8 byte code
		 *
		 *
		 */
		public void cCompareGt8()
		{
			DATA8 Tmp;

			Tmp = *(DATA8*)GH.Lms.PrimParPointer();
			if (Tmp > *(DATA8*)GH.Lms.PrimParPointer())
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 1;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 0;
			}
		}


		/*! \page cCompare
		 *  <hr size="1"/>
		 *  <b>     opCP_GT16 (LEFT, RIGHT, FLAG)  </b>
		 *
		 *- If LEFT is greater than RIGTH - set FLAG \n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  LEFT
		 *  \param  (DATA16)  RIGHT
		 *  \return (DATA8)   FLAG
		 */
		/*! \brief  opCP_GT16 byte code
		 *
		 *
		 */
		public void cCompareGt16()
		{
			DATA16 Tmp;

			Tmp = *(DATA16*)GH.Lms.PrimParPointer();
			if (Tmp > *(DATA16*)GH.Lms.PrimParPointer())
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 1;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 0;
			}
		}


		/*! \page cCompare
		 *  <hr size="1"/>
		 *  <b>     opCP_GT32 (LEFT, RIGHT, FLAG)  </b>
		 *
		 *- If LEFT is greater than RIGTH - set FLAG \n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  LEFT
		 *  \param  (DATA32)  RIGHT
		 *  \return (DATA8)   FLAG
		 */
		/*! \brief  opCP_GT32 byte code
		 *
		 *
		 */
		public void cCompareGt32()
		{
			DATA32 Tmp;

			Tmp = *(DATA32*)GH.Lms.PrimParPointer();
			if (Tmp > *(DATA32*)GH.Lms.PrimParPointer())
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 1;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 0;
			}
		}


		/*! \page cCompare Compare
		 *  <hr size="1"/>
		 *  <b>     opCP_GTF (LEFT, RIGHT, FLAG)  </b>
		 *
		 *- If LEFT is greater than RIGTH - set FLAG \n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATAF)   LEFT
		 *  \param  (DATAF)   RIGHT
		 *  \return (DATA8)   FLAG
		 */
		/*! \brief  opCP_GTF byte code
		 *
		 *
		 */
		public void cCompareGtF()
		{
			DATAF Tmp;

			Tmp = *(DATAF*)GH.Lms.PrimParPointer();
			if (Tmp > *(DATAF*)GH.Lms.PrimParPointer())
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 1;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 0;
			}
		}


		/*! \page cCompare
		 *  <hr size="1"/>
		 *  <b>     opCP_EQ8 (LEFT, RIGHT, FLAG)  </b>
		 *
		 *- If LEFT is equal to RIGTH - set FLAG \n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   LEFT
		 *  \param  (DATA8)   RIGHT
		 *  \return (DATA8)   FLAG
		 */
		/*! \brief  opCP_EQ8 byte code
		 *
		 *
		 */
		public void cCompareEq8()
		{
			DATA8 Tmp;

			Tmp = *(DATA8*)GH.Lms.PrimParPointer();
			if (Tmp == *(DATA8*)GH.Lms.PrimParPointer())
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 1;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 0;
			}
		}


		/*! \page cCompare
		 *  <hr size="1"/>
		 *  <b>     opCP_EQ16 (LEFT, RIGHT, FLAG)  </b>
		 *
		 *- If LEFT is equal to RIGTH - set FLAG \n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  LEFT
		 *  \param  (DATA16)  RIGHT
		 *  \return (DATA8)   FLAG
		 */
		/*! \brief  opCP_EQ16 byte code
		 *
		 *
		 */
		public void cCompareEq16()
		{
			DATA16 Tmp;

			Tmp = *(DATA16*)GH.Lms.PrimParPointer();
			if (Tmp == *(DATA16*)GH.Lms.PrimParPointer())
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 1;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 0;
			}
		}


		/*! \page cCompare
		 *  <hr size="1"/>
		 *  <b>     opCP_EQ32 (LEFT, RIGHT, FLAG)  </b>
		 *
		 *- If LEFT is equal to RIGTH - set FLAG \n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  LEFT
		 *  \param  (DATA32)  RIGHT
		 *  \return (DATA8)   FLAG
		 */
		/*! \brief  opCP_EQ32 byte code
		 *
		 *
		 */
		public void cCompareEq32()
		{
			DATA32 Tmp;

			Tmp = *(DATA32*)GH.Lms.PrimParPointer();
			if (Tmp == *(DATA32*)GH.Lms.PrimParPointer())
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 1;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 0;
			}
		}


		/*! \page cCompare
		 *  <hr size="1"/>
		 *  <b>     opCP_EQF (LEFT, RIGHT, FLAG)  </b>
		 *
		 *- If LEFT is equal to RIGTH - set FLAG \n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATAF)   LEFT
		 *  \param  (DATAF)   RIGHT
		 *  \return (DATA8)   FLAG
		 */
		/*! \brief  opCP_EQF byte code
		 *
		 *
		 */
		public void cCompareEqF()
		{
			DATAF Tmp;

			Tmp = *(DATAF*)GH.Lms.PrimParPointer();
			if (Tmp == *(DATAF*)GH.Lms.PrimParPointer())
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 1;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 0;
			}
		}


		/*! \page cCompare
		 *  <hr size="1"/>
		 *  <b>     opCP_NEQ8 (LEFT, RIGHT, FLAG)  </b>
		 *
		 *- If LEFT is not equal to RIGTH - set FLAG \n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   LEFT
		 *  \param  (DATA8)   RIGHT
		 *  \return (DATA8)   FLAG
		 */
		/*! \brief  opCP_NEQ8 byte code
		 *
		 *
		 */
		public void cCompareNEq8()
		{
			DATA8 Tmp;

			Tmp = *(DATA8*)GH.Lms.PrimParPointer();
			if (Tmp != *(DATA8*)GH.Lms.PrimParPointer())
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 1;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 0;
			}
		}


		/*! \page cCompare
		 *  <hr size="1"/>
		 *  <b>     opCP_NEQ16 (LEFT, RIGHT, FLAG)  </b>
		 *
		 *- If LEFT is not equal to RIGTH - set FLAG \n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  LEFT
		 *  \param  (DATA16)  RIGHT
		 *  \return (DATA8)   FLAG
		 */
		/*! \brief  opCP_NEQ16 byte code
		 *
		 *
		 */
		public void cCompareNEq16()
		{
			DATA16 Tmp;

			Tmp = *(DATA16*)GH.Lms.PrimParPointer();
			if (Tmp != *(DATA16*)GH.Lms.PrimParPointer())
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 1;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 0;
			}
		}


		/*! \page cCompare
		 *  <hr size="1"/>
		 *  <b>     opCP_NEQ32 (LEFT, RIGHT, FLAG)  </b>
		 *
		 *- If LEFT is not equal to RIGTH - set FLAG \n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  LEFT
		 *  \param  (DATA32)  RIGHT
		 *  \return (DATA8)   FLAG
		 */
		/*! \brief  opCP_NEQ32 byte code
		 *
		 *
		 */
		public void cCompareNEq32()
		{
			DATA32 Tmp;

			Tmp = *(DATA32*)GH.Lms.PrimParPointer();
			if (Tmp != *(DATA32*)GH.Lms.PrimParPointer())
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 1;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 0;
			}
		}


		/*! \page cCompare
		 *  <hr size="1"/>
		 *  <b>     opCP_NEQF (LEFT, RIGHT, FLAG)  </b>
		 *
		 *- If LEFT is not equal to RIGTH - set FLAG \n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATAF)   LEFT
		 *  \param  (DATAF)   RIGHT
		 *  \return (DATA8)   FLAG
		 */
		/*! \brief  opCP_NEQF byte code
		 *
		 *
		 */
		public void cCompareNEqF()
		{
			DATAF Tmp;

			Tmp = *(DATAF*)GH.Lms.PrimParPointer();
			if (Tmp != *(DATAF*)GH.Lms.PrimParPointer())
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 1;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 0;
			}
		}


		/*! \page cCompare
		 *  <hr size="1"/>
		 *  <b>     opCP_LTEQ8 (LEFT, RIGHT, FLAG)  </b>
		 *
		 *- If LEFT is less than or equal to RIGTH - set FLAG \n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   LEFT
		 *  \param  (DATA8)   RIGHT
		 *  \return (DATA8)   FLAG
		 */
		/*! \brief  opCP_LTEQ8 byte code
		 *
		 *
		 */
		public void cCompareLtEq8()
		{
			DATA8 Tmp;

			Tmp = *(DATA8*)GH.Lms.PrimParPointer();
			if (Tmp <= *(DATA8*)GH.Lms.PrimParPointer())
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 1;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 0;
			}
		}


		/*! \page cCompare
		 *  <hr size="1"/>
		 *  <b>     opCP_LTEQ16 (LEFT, RIGHT, FLAG)  </b>
		 *
		 *- If LEFT is less than or equal to RIGTH - set FLAG \n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  LEFT
		 *  \param  (DATA16)  RIGHT
		 *  \return (DATA8)   FLAG
		 */
		/*! \brief  opCP_LTEQ16 byte code
		 *
		 *
		 */
		public void cCompareLtEq16()
		{
			DATA16 Tmp;

			Tmp = *(DATA16*)GH.Lms.PrimParPointer();
			if (Tmp <= *(DATA16*)GH.Lms.PrimParPointer())
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 1;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 0;
			}
		}


		/*! \page cCompare
		 *  <hr size="1"/>
		 *  <b>     opCP_LTEQ32 (LEFT, RIGHT, FLAG)  </b>
		 *
		 *- If LEFT is less than or equal to RIGTH - set FLAG \n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  LEFT
		 *  \param  (DATA32)  RIGHT
		 *  \return (DATA8)   FLAG
		 */
		/*! \brief  opCP_LTEQ32 byte code
		 *
		 *
		 */
		public void cCompareLtEq32()
		{
			DATA32 Tmp;

			Tmp = *(DATA32*)GH.Lms.PrimParPointer();
			if (Tmp <= *(DATA32*)GH.Lms.PrimParPointer())
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 1;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 0;
			}
		}


		/*! \page cCompare Compare
		 *  <hr size="1"/>
		 *  <b>     opCP_LTEQF (LEFT, RIGHT, FLAG)  </b>
		 *
		 *- If LEFT is less than or equal to RIGTH - set FLAG \n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATAF)   LEFT
		 *  \param  (DATAF)   RIGHT
		 *  \return (DATA8)   FLAG
		 */
		/*! \brief  opCP_LTEQF byte code
		 *
		 *
		 */
		public void cCompareLtEqF()
		{
			DATAF Tmp;

			Tmp = *(DATAF*)GH.Lms.PrimParPointer();
			if (Tmp <= *(DATAF*)GH.Lms.PrimParPointer())
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 1;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 0;
			}
		}


		/*! \page cCompare
		 *  <hr size="1"/>
		 *  <b>     opCP_GTEQ8 (LEFT, RIGHT, FLAG)  </b>
		 *
		 *- If LEFT is greater than or equal to RIGTH - set FLAG \n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   LEFT
		 *  \param  (DATA8)   RIGHT
		 *  \return (DATA8)   FLAG
		 */
		/*! \brief  opCP_GTEQ8 byte code
		 *
		 *
		 */
		public void cCompareGtEq8()
		{
			DATA8 Tmp;

			Tmp = *(DATA8*)GH.Lms.PrimParPointer();
			if (Tmp >= *(DATA8*)GH.Lms.PrimParPointer())
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 1;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 0;
			}
		}


		/*! \page cCompare
		 *  <hr size="1"/>
		 *  <b>     opCP_GTEQ16 (LEFT, RIGHT, FLAG)  </b>
		 *
		 *- If LEFT is greater than or equal to RIGTH - set FLAG \n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  LEFT
		 *  \param  (DATA16)  RIGHT
		 *  \return (DATA8)   FLAG
		 */
		/*! \brief  opCP_GTEQ16 byte code
		 *
		 *
		 */
		public void cCompareGtEq16()
		{
			DATA16 Tmp;

			Tmp = *(DATA16*)GH.Lms.PrimParPointer();
			if (Tmp >= *(DATA16*)GH.Lms.PrimParPointer())
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 1;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 0;
			}
		}


		/*! \page cCompare
		 *  <hr size="1"/>
		 *  <b>     opCP_GTEQ32 (LEFT, RIGHT, FLAG)  </b>
		 *
		 *- If LEFT is greater than or equal to RIGTH - set FLAG \n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  LEFT
		 *  \param  (DATA32)  RIGHT
		 *  \return (DATA8)   FLAG
		 */
		/*! \brief  opCP_GTEQ32 byte code
		 *
		 *
		 */
		public void cCompareGtEq32()
		{
			DATA32 Tmp;

			Tmp = *(DATA32*)GH.Lms.PrimParPointer();
			if (Tmp >= *(DATA32*)GH.Lms.PrimParPointer())
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 1;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 0;
			}
		}


		/*! \page cCompare Compare
		 *  <hr size="1"/>
		 *  <b>     opCP_GTEQF (LEFT, RIGHT, FLAG)  </b>
		 *
		 *- If LEFT is greater than or equal to RIGTH - set FLAG \n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATAF)   LEFT
		 *  \param  (DATAF)   RIGHT
		 *  \return (DATA8)   FLAG
		 */
		/*! \brief  opCP_GTEQF byte code
		 *
		 *
		 */
		public void cCompareGtEqF()
		{
			DATAF Tmp;

			Tmp = *(DATAF*)GH.Lms.PrimParPointer();
			if (Tmp >= *(DATAF*)GH.Lms.PrimParPointer())
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 1;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 0;
			}
		}


		/*! \page Select Select
		 *  <hr size="1"/>
		 *  <b>     opSELECT8 (FLAG, SOURCE1, SOURCE2, *RESULT)  </b>
		 *
		 *- If FLAG is set move SOURCE1 to RESULT else move SOURCE2 to RESULT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   FLAG
		 *  \param  (DATA8)   SOURCE1
		 *  \param  (DATA8)   SOURCE2
		 *  \return (DATA8)   *RESULT
		 */
		/*! \brief  opSELECT8 byte code
		 *
		 *
		 */
		public void cCompareSelect8()
		{
			DATA8 Flag;
			DATA8 Source1;
			DATA8 Source2;

			Flag = *(DATA8*)GH.Lms.PrimParPointer();
			Source1 = *(DATA8*)GH.Lms.PrimParPointer();
			Source2 = *(DATA8*)GH.Lms.PrimParPointer();
			if (Flag != 0)
			{
				*(DATA8*)GH.Lms.PrimParPointer() = Source1;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = Source2;
			}
		}


		/*! \page Select
		 *  <hr size="1"/>
		 *  <b>     opSELECT16 (FLAG, SOURCE1, SOURCE2, *RESULT)  </b>
		 *
		 *- If FLAG is set move SOURCE1 to RESULT else move SOURCE2 to RESULT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   FLAG
		 *  \param  (DATA16)  SOURCE1
		 *  \param  (DATA16)  SOURCE2
		 *  \return (DATA16)  *RESULT
		 */
		/*! \brief  opSELECT16 byte code
		 *
		 *
		 */
		public void cCompareSelect16()
		{
			DATA8 Flag;
			DATA16 Source1;
			DATA16 Source2;


			Flag = *(DATA8*)GH.Lms.PrimParPointer();
			Source1 = *(DATA16*)GH.Lms.PrimParPointer();
			Source2 = *(DATA16*)GH.Lms.PrimParPointer();
			if (Flag != 0)
			{
				*(DATA16*)GH.Lms.PrimParPointer() = Source1;
			}
			else
			{
				*(DATA16*)GH.Lms.PrimParPointer() = Source2;
			}
		}


		/*! \page Select
		 *  <hr size="1"/>
		 *  <b>     opSELECT32 (FLAG, SOURCE1, SOURCE2, *RESULT)  </b>
		 *
		 *- If FLAG is set move SOURCE1 to RESULT else move SOURCE2 to RESULT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   FLAG
		 *  \param  (DATA32)  SOURCE1
		 *  \param  (DATA32)  SOURCE2
		 *  \return (DATA32)  *RESULT
		 */
		/*! \brief  opSELECT32 byte code
		 *
		 *
		 */
		public void cCompareSelect32()
		{
			DATA8 Flag;
			DATA32 Source1;
			DATA32 Source2;

			Flag = *(DATA8*)GH.Lms.PrimParPointer();
			Source1 = *(DATA32*)GH.Lms.PrimParPointer();
			Source2 = *(DATA32*)GH.Lms.PrimParPointer();
			if (Flag != 0)
			{
				*(DATA32*)GH.Lms.PrimParPointer() = Source1;
			}
			else
			{
				*(DATA32*)GH.Lms.PrimParPointer() = Source2;
			}
		}


		/*! \page Select
		 *  <hr size="1"/>
		 *  <b>     opSELECTF (FLAG, SOURCE1, SOURCE2, *RESULT)  </b>
		 *
		 *- If FLAG is set move SOURCE1 to RESULT else move SOURCE2 to RESULT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   FLAG
		 *  \param  (DATAF)   SOURCE1
		 *  \param  (DATAF)   SOURCE2
		 *  \return (DATAF)   *RESULT
		 */
		/*! \brief  opSELECTF byte code
		 *
		 *
		 */
		public void cCompareSelectF()
		{
			DATA8 Flag;
			DATAF Source1;
			DATAF Source2;

			Flag = *(DATA8*)GH.Lms.PrimParPointer();
			Source1 = *(DATAF*)GH.Lms.PrimParPointer();
			Source2 = *(DATAF*)GH.Lms.PrimParPointer();
			if (Flag != 0)
			{
				*(DATAF*)GH.Lms.PrimParPointer() = Source1;
			}
			else
			{
				*(DATAF*)GH.Lms.PrimParPointer() = Source2;
			}
		}
	}
}
