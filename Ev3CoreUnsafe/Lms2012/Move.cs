using Ev3CoreUnsafe.Lms2012.Interfaces;
using static Ev3CoreUnsafe.Defines;

namespace Ev3CoreUnsafe.Lms2012
{
	public unsafe class Move : IMove
	{
		/*! \page cMove Move
		 *  <hr size="1"/>
		 *  <b>     opINIT_BYTES (DESTINATION, LENGTH, SOURCE)  </b>
		 *
		 *- Move LENGTH number of DATA8 from BYTE STREAM to memory DESTINATION START\n
		 *- Dispatch status unchanged
		 *
		 *  \return (DATA8)   DESTINATION   - First element in DATA8 array to be initiated
		 *  \param  (DATA32)  LENGTH        - Number of elements to initiate
		 *  \param  (DATA8)   SOURCE        - First element to initiate DATA8 array with
		 */
		/*! \brief  opINIT_BYTES
		 *
		 */
		public void cMoveInitBytes()
		{
			DATA8* pDestination;
			DATA32 Length;

			pDestination = (DATA8*)GH.Lms.PrimParPointer();
			Length = *(DATA32*)GH.Lms.PrimParPointer();

			while (Length != 0)
			{
				*pDestination = *(DATA8*)GH.Lms.PrimParPointer();
				pDestination++;
				Length--;
			}
		}


		/*! \page cMove
		 *  <hr size="1"/>
		 *  <b>     opMOVE8_8 (SOURCE, DESTINATION)  </b>
		 *
		 *- Move 8 bit value from SOURCE to DESTINATION\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   SOURCE
		 *  \return (DATA8)   DESTINATION
		 */
		/*! \brief  opMOVE8_8
		 *
		 */
		public void cMove8to8()
		{
			DATA8 Tmp;

			Tmp = *(DATA8*)GH.Lms.PrimParPointer();
			*(DATA8*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page cMove
		 *  <hr size="1"/>
		 *  <b>     opMOVE8_16 (SOURCE, DESTINATION)  </b>
		 *
		 *- Move 8 bit value from SOURCE to DESTINATION\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   SOURCE
		 *  \return (DATA16)  DESTINATION
		 */
		/*! \brief  opMOVE8_16
		 *
		 */
		public void cMove8to16()
		{
			DATA8 Tmp;

			Tmp = *(DATA8*)GH.Lms.PrimParPointer();

			if (Tmp != DATA8_NAN)
			{
				*(DATA16*)GH.Lms.PrimParPointer() = (DATA16)Tmp;
			}
			else
			{
				*(DATA16*)GH.Lms.PrimParPointer() = DATA16_NAN;
			}
		}


		/*! \page cMove
		 *  <hr size="1"/>
		 *  <b>     opMOVE8_32 (SOURCE, DESTINATION)  </b>
		 *
		 *- Move 8 bit value from SOURCE to DESTINATION\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   SOURCE
		 *  \return (DATA32)  DESTINATION
		 */
		/*! \brief  opMOVE8_32
		 *
		 */
		public void cMove8to32()
		{
			DATA8 Tmp;

			Tmp = *(DATA8*)GH.Lms.PrimParPointer();
			if (Tmp != DATA8_NAN)
			{
				*(DATA32*)GH.Lms.PrimParPointer() = (DATA32)Tmp;
			}
			else
			{
				*(DATA32*)GH.Lms.PrimParPointer() = DATA32_NAN;
			}
		}


		/*! \page cMove
		 *  <hr size="1"/>
		 *  <b>     opMOVE8_F (SOURCE, DESTINATION)  </b>
		 *
		 *- Move 8 bit value from SOURCE to DESTINATION\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   SOURCE
		 *  \return (DATAF)   DESTINATION
		 */
		/*! \brief  opMOVE8_F
		 *
		 */
		public void cMove8toF()
		{
			DATA8 Tmp;

			Tmp = *(DATA8*)GH.Lms.PrimParPointer();
			if (Tmp != DATA8_NAN)
			{
				*(DATAF*)GH.Lms.PrimParPointer() = (DATAF)Tmp;
			}
			else
			{
				*(DATAF*)GH.Lms.PrimParPointer() = DATAF_NAN;
			}
		}


		/*! \page cMove
		 *  <hr size="1"/>
		 *  <b>     opMOVE16_8 (SOURCE, DESTINATION)  </b>
		 *
		 *- Move 16 bit value from SOURCE to DESTINATION\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  SOURCE
		 *  \return (DATA8)   DESTINATION
		 */
		/*! \brief  opMOVE16_8
		 *
		 */
		public void cMove16to8()
		{
			DATA16 Tmp;

			Tmp = *(DATA16*)GH.Lms.PrimParPointer();
			if (Tmp != DATA16_NAN)
			{
				if (Tmp > (DATA16)DATA8_MAX)
				{
					Tmp = (DATA16)DATA8_MAX;
				}
				if (Tmp < (DATA16)DATA8_MIN)
				{
					Tmp = (DATA16)DATA8_MIN;
				}
				*(DATA8*)GH.Lms.PrimParPointer() = (DATA8)Tmp;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = DATA8_NAN;
			}
		}


		/*! \page cMove
		 *  <hr size="1"/>
		 *  <b>     opMOVE16_16 (SOURCE, DESTINATION)  </b>
		 *
		 *- Move 16 bit value from SOURCE to DESTINATION\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  SOURCE
		 *  \return (DATA16)  DESTINATION
		 */
		/*! \brief  opMOVE16_16
		 *
		 */
		public void cMove16to16()
		{
			DATA16 Tmp;

			Tmp = *(DATA16*)GH.Lms.PrimParPointer();
			*(DATA16*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page cMove
		 *  <hr size="1"/>
		 *  <b>     opMOVE16_32 (SOURCE, DESTINATION)  </b>
		 *
		 *- Move 16 bit value from SOURCE to DESTINATION\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  SOURCE
		 *  \return (DATA32)  DESTINATION
		 */
		/*! \brief  opMOVE16_32
		 *
		 */
		public void cMove16to32()
		{
			DATA16 Tmp;

			Tmp = *(DATA16*)GH.Lms.PrimParPointer();
			if (Tmp != DATA16_NAN)
			{
				*(DATA32*)GH.Lms.PrimParPointer() = (DATA32)Tmp;
			}
			else
			{
				*(DATA32*)GH.Lms.PrimParPointer() = DATA32_NAN;
			}
		}


		/*! \page cMove
		 *  <hr size="1"/>
		 *  <b>     opMOVE16_F (SOURCE, DESTINATION)  </b>
		 *
		 *- Move 16 bit value from SOURCE to DESTINATION\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  SOURCE
		 *  \return (DATAF)   DESTINATION
		 */
		/*! \brief  opMOVE16_F
		 *
		 */
		public void cMove16toF()
		{
			DATA16 Tmp;

			Tmp = *(DATA16*)GH.Lms.PrimParPointer();
			if (Tmp != DATA16_NAN)
			{
				*(DATAF*)GH.Lms.PrimParPointer() = (DATAF)Tmp;
			}
			else
			{
				*(DATAF*)GH.Lms.PrimParPointer() = DATAF_NAN;
			}
		}


		/*! \page cMove
		 *  <hr size="1"/>
		 *  <b>     opMOVE32_8 (SOURCE, DESTINATION)  </b>
		 *
		 *- Move 32 bit value from SOURCE to DESTINATION\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  SOURCE
		 *  \return (DATA8)   DESTINATION
		 */
		/*! \brief  opMOVE32_8
		 *
		 */
		public void cMove32to8()
		{
			DATA32 Tmp;

			Tmp = *(DATA32*)GH.Lms.PrimParPointer();
			if (Tmp != DATA32_NAN)
			{
				if (Tmp > (DATA32)DATA8_MAX)
				{
					Tmp = (DATA32)DATA8_MAX;
				}
				if (Tmp < (DATA32)DATA8_MIN)
				{
					Tmp = (DATA32)DATA8_MIN;
				}
				*(DATA8*)GH.Lms.PrimParPointer() = (DATA8)Tmp;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = DATA8_NAN;
			}
		}


		/*! \page cMove
		 *  <hr size="1"/>
		 *  <b>     opMOVE32_16 (SOURCE, DESTINATION)  </b>
		 *
		 *- Move 32 bit value from SOURCE to DESTINATION\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  SOURCE
		 *  \return (DATA16)  DESTINATION
		 */
		/*! \brief  opMOVE32_16
		 *
		 */
		public void cMove32to16()
		{
			DATA32 Tmp;

			Tmp = *(DATA32*)GH.Lms.PrimParPointer();
			if (Tmp != DATA32_NAN)
			{
				if (Tmp > (DATA32)DATA16_MAX)
				{
					Tmp = (DATA32)DATA16_MAX;
				}
				if (Tmp < (DATA32)DATA16_MIN)
				{
					Tmp = (DATA32)DATA16_MIN;
				}
				*(DATA16*)GH.Lms.PrimParPointer() = (DATA16)Tmp;
			}
			else
			{
				*(DATA16*)GH.Lms.PrimParPointer() = DATA16_NAN;
			}
		}


		/*! \page cMove
		 *  <hr size="1"/>
		 *  <b>     opMOVE32_32 (SOURCE, DESTINATION)  </b>
		 *
		 *- Move 32 bit value from SOURCE to DESTINATION\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  SOURCE
		 *  \return (DATA32)  DESTINATION
		 */
		/*! \brief  opMOVE32_32
		 *
		 */
		public void cMove32to32()
		{
			DATA32 Tmp;

			Tmp = *(DATA32*)GH.Lms.PrimParPointer();
			*(DATA32*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page cMove
		 *  <hr size="1"/>
		 *  <b>     opMOVE32_F (SOURCE, DESTINATION)  </b>
		 *
		 *- Move 32 bit value from SOURCE to DESTINATION\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  SOURCE
		 *  \return (DATAF)   DESTINATION
		 */
		/*! \brief  opMOVE32_F
		 *
		 */
		public void cMove32toF()
		{
			DATA32 Tmp;

			Tmp = *(DATA32*)GH.Lms.PrimParPointer();
			if (Tmp != DATA32_NAN)
			{
				*(DATAF*)GH.Lms.PrimParPointer() = (DATAF)Tmp;
			}
			else
			{
				*(DATAF*)GH.Lms.PrimParPointer() = DATAF_NAN;
			}
		}


		/*! \page cMove
		 *  <hr size="1"/>
		 *  <b>     opMOVEF_8 (SOURCE, DESTINATION)  </b>
		 *
		 *- Move floating point value from SOURCE to DESTINATION\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATAF)   SOURCE
		 *  \return (DATA8)   DESTINATION
		 */
		/*! \brief  opMOVEF_8
		 *
		 */
		public void cMoveFto8()
		{
			DATAF Tmp;

			Tmp = *(DATAF*)GH.Lms.PrimParPointer();
			if (!(float.IsNaN(Tmp)))
			{
				if (Tmp > (DATAF)DATA8_MAX)
				{
					Tmp = (DATAF)DATA8_MAX;
				}
				if (Tmp < (DATAF)DATA8_MIN)
				{
					Tmp = (DATAF)DATA8_MIN;
				}
				*(DATA8*)GH.Lms.PrimParPointer() = (DATA8)Tmp;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = DATA8_NAN;
			}
		}


		/*! \page cMove
		 *  <hr size="1"/>
		 *  <b>     opMOVEF_16 (SOURCE, DESTINATION)  </b>
		 *
		 *- Move floating point value from SOURCE to DESTINATION\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATAF)   SOURCE
		 *  \return (DATA16)  DESTINATION
		 */
		/*! \brief  opMOVEF_16
		 *
		 */
		public void cMoveFto16()
		{
			DATAF Tmp;

			Tmp = *(DATAF*)GH.Lms.PrimParPointer();
			if (!(float.IsNaN(Tmp)))
			{
				if (Tmp > (DATAF)DATA16_MAX)
				{
					Tmp = (DATAF)DATA16_MAX;
				}
				if (Tmp < (DATAF)DATA16_MIN)
				{
					Tmp = (DATAF)DATA16_MIN;
				}
				*(DATA16*)GH.Lms.PrimParPointer() = (DATA16)Tmp;
			}
			else
			{
				*(DATA16*)GH.Lms.PrimParPointer() = DATA16_NAN;
			}
		}


		/*! \page cMove
		 *  <hr size="1"/>
		 *  <b>     opMOVEF_32 (SOURCE, DESTINATION)  </b>
		 *
		 *- Move floating point value from SOURCE to DESTINATION\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATAF)   SOURCE
		 *  \return (DATA32)  DESTINATION
		 */
		/*! \brief  opMOVEF_32
		 *
		 */
		public void cMoveFto32()
		{
			DATAF Tmp;

			Tmp = *(DATAF*)GH.Lms.PrimParPointer();
			if (!(float.IsNaN(Tmp)))
			{
				if (Tmp > (DATAF)DATA32_MAX)
				{
					Tmp = (DATAF)DATA32_MAX;
				}
				if (Tmp < (DATAF)DATA32_MIN)
				{
					Tmp = (DATAF)DATA32_MIN;
				}
				*(DATA32*)GH.Lms.PrimParPointer() = (DATA32)Tmp;
			}
			else
			{
				*(DATA32*)GH.Lms.PrimParPointer() = DATA32_NAN;
			}
		}


		/*! \page cMove
		 *  <hr size="1"/>
		 *  <b>     opMOVEF_F (SOURCE, DESTINATION)  </b>
		 *
		 *- Move floating point value from SOURCE to DESTINATION\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATAF)   SOURCE
		 *  \return (DATAF)   DESTINATION
		 */
		/*! \brief  opMOVEF_F
		 *
		 */
		public void cMoveFtoF()
		{
			DATAF Tmp;

			Tmp = *(DATAF*)GH.Lms.PrimParPointer();
			*(DATAF*)GH.Lms.PrimParPointer() = Tmp;
		}


		/*! \page cMove
		 *  <hr size="1"/>
		 *  <b>     opREAD8 (SOURCE, INDEX, DESTINATION)  </b>
		 *
		 *- Read 8 bit value from SOURCE[INDEX] to DESTINATION\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   SOURCE      - First value in array of values\n
		 *  \param  (DATA8)   INDEX       - Index to array member to read\n
		 *  \return (DATA8)   DESTINATION - Variable to receive read value\n
		 */
		/*! \brief  opREAD8
		 *
		 */
		public void cMoveRead8()
		{
			DATA8* pTmp;
			DATA8 Index;

			pTmp = (DATA8*)GH.Lms.PrimParPointer();
			Index = *(DATA8*)GH.Lms.PrimParPointer();
			*(DATA8*)GH.Lms.PrimParPointer() = pTmp[Index];
		}


		/*! \page cMove
		 *  <hr size="1"/>
		 *  <b>     opREAD16 (SOURCE, INDEX, DESTINATION)  </b>
		 *
		 *- Read 16 bit value from SOURCE[INDEX] to DESTINATION\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  SOURCE      - First value in array of values\n
		 *  \param  (DATA8)   INDEX       - Index to array member to read\n
		 *  \return (DATA16)  DESTINATION - Variable to receive read value\n
		 */
		/*! \brief  opREAD16
		 *
		 */
		public void cMoveRead16()
		{
			DATA16* pTmp;
			DATA8 Index;

			pTmp = (DATA16*)GH.Lms.PrimParPointer();
			Index = *(DATA8*)GH.Lms.PrimParPointer();
			*(DATA16*)GH.Lms.PrimParPointer() = pTmp[Index];
		}


		/*! \page cMove
		 *  <hr size="1"/>
		 *  <b>     opREAD32 (SOURCE, INDEX, DESTINATION)  </b>
		 *
		 *- Read 32 bit value from SOURCE[INDEX] to DESTINATION\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  SOURCE      - First value in array of values\n
		 *  \param  (DATA8)   INDEX       - Index to array member to read\n
		 *  \return (DATA32)  DESTINATION - Variable to receive read value\n
		 */
		/*! \brief  opREAD32
		 *
		 */
		public void cMoveRead32()
		{
			DATA32* pTmp;
			DATA8 Index;

			pTmp = (DATA32*)GH.Lms.PrimParPointer();
			Index = *(DATA8*)GH.Lms.PrimParPointer();
			*(DATA32*)GH.Lms.PrimParPointer() = pTmp[Index];
		}


		/*! \page cMove
		 *  <hr size="1"/>
		 *  <b>     opREADF (SOURCE, INDEX, DESTINATION)  </b>
		 *
		 *- Read floating point value from SOURCE[INDEX] to DESTINATION\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATAF)   SOURCE      - First value in array of values\n
		 *  \param  (DATA8)   INDEX       - Index to array member to read\n
		 *  \return (DATAF)   DESTINATION - Variable to receive read value\n
		 */
		/*! \brief  opREADF
		 *
		 */
		public void cMoveReadF()
		{
			DATAF* pTmp;
			DATA8 Index;

			pTmp = (DATAF*)GH.Lms.PrimParPointer();
			Index = *(DATA8*)GH.Lms.PrimParPointer();
			*(DATAF*)GH.Lms.PrimParPointer() = pTmp[Index];
		}


		/*! \page cMove
		 *  <hr size="1"/>
		 *  <b>     opWRITE8 (SOURCE, INDEX, DESTINATION)  </b>
		 *
		 *- Write 8 bit value from SOURCE to DESTINATION[INDEX]\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   SOURCE      - Variable to write\n
		 *  \param  (DATA8)   INDEX       - Index to array member to write\n
		 *  \param  (DATA8)   DESTINATION - Array to receive write value\n
		 */
		/*! \brief  opWRITE8
		 *
		 */
		public void cMoveWrite8()
		{
			DATA8 Tmp;
			DATA8* pTmp;
			DATA8 Index;

			Tmp = *(DATA8*)GH.Lms.PrimParPointer();
			Index = *(DATA8*)GH.Lms.PrimParPointer();
			pTmp = (DATA8*)GH.Lms.PrimParPointer();
			pTmp[Index] = Tmp;
		}


		/*! \page cMove
		 *  <hr size="1"/>
		 *  <b>     opWRITE16 (SOURCE, INDEX, DESTINATION)  </b>
		 *
		 *- Write 16 bit value from SOURCE to DESTINATION[INDEX]\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  SOURCE      - Variable to write\n
		 *  \param  (DATA8)   INDEX       - Index to array member to write\n
		 *  \param  (DATA16)  DESTINATION - Array to receive write value\n
		 */
		/*! \brief  opWRITE16
		 *
		 */
		public void cMoveWrite16()
		{
			DATA16 Tmp;
			DATA16* pTmp;
			DATA8 Index;

			Tmp = *(DATA16*)GH.Lms.PrimParPointer();
			Index = *(DATA8*)GH.Lms.PrimParPointer();
			pTmp = (DATA16*)GH.Lms.PrimParPointer();
			pTmp[Index] = Tmp;
		}


		/*! \page cMove
		 *  <hr size="1"/>
		 *  <b>     opWRITE32 (SOURCE, INDEX, DESTINATION)  </b>
		 *
		 *- Write 32 bit value from SOURCE to DESTINATION[INDEX]\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  SOURCE      - Variable to write\n
		 *  \param  (DATA8)   INDEX       - Index to array member to write\n
		 *  \param  (DATA32)  DESTINATION - Array to receive write value\n
		 */
		/*! \brief  opWRITE32
		 *
		 */
		public void cMoveWrite32()
		{
			DATA32 Tmp;
			DATA32* pTmp;
			DATA8 Index;

			Tmp = *(DATA32*)GH.Lms.PrimParPointer();
			Index = *(DATA8*)GH.Lms.PrimParPointer();
			pTmp = (DATA32*)GH.Lms.PrimParPointer();
			pTmp[Index] = Tmp;
		}


		/*! \page cMove
		 *  <hr size="1"/>
		 *  <b>     opWRITEF (SOURCE, INDEX, DESTINATION)  </b>
		 *
		 *- Write floating point value from SOURCE to DESTINATION[INDEX]\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATAF)   SOURCE      - Variable to write\n
		 *  \param  (DATA8)   INDEX       - Index to array member to write\n
		 *  \param  (DATAF)   DESTINATION - Array to receive write value\n
		 */
		/*! \brief  opWRITEF
		 *
		 */
		public void cMoveWriteF()
		{
			DATAF Tmp;
			DATAF* pTmp;
			DATA8 Index;

			Tmp = *(DATAF*)GH.Lms.PrimParPointer();
			Index = *(DATA8*)GH.Lms.PrimParPointer();
			pTmp = (DATAF*)GH.Lms.PrimParPointer();
			pTmp[Index] = Tmp;
		}
	}
}
