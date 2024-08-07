using Ev3CoreUnsafe.Lms2012.Interfaces;

namespace Ev3CoreUnsafe.Lms2012
{
	public unsafe class Branch : IBranch
	{
		public void cBranchJr()
		{
			GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
		}

		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_FALSE (FLAG, OFFSET)  </b>
		 *
		 *- Branch relative if FLAG is FALSE (zero)\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   FLAG
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_FALSE byte code
		 *
		 *
		 */
		public void cBranchJrFalse()
		{
			if (*(DATA8*)GH.Lms.PrimParPointer() == (DATA8)0)
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}


		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_TRUE (FLAG, OFFSET)  </b>
		 *
		 *- Branch relative if FLAG is TRUE (non zero)\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   FLAG
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_TRUE byte code
		 *
		 *
		 */
		public void cBranchJrTrue()
		{
			if (*(DATA8*)GH.Lms.PrimParPointer() != (DATA8)0)
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}

		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_NAN (VALUE, OFFSET)  </b>
		 *
		 *- Branch relative if VALUE is NAN (not a number)\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATAF)   VALUE
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_NAN byte code
		 *
		 *
		 */
		public void cBranchJrNan()
		{
			DATAF Tmp;

			Tmp = *(DATAF*)GH.Lms.PrimParPointer();

			if (float.IsNaN(Tmp))
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}


		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_LT8 (LEFT, RIGHT, OFFSET)  </b>
		 *
		 *- Branch relative OFFSET if LEFT is less than RIGHT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   LEFT
		 *  \param  (DATA8)   RIGHT
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_LT8 byte code
		 *
		 *
		 */
		public void cBranchJrLt8()
		{
			DATA8 Tmp;

			Tmp = *(DATA8*)GH.Lms.PrimParPointer();
			if (Tmp < *(DATA8*)GH.Lms.PrimParPointer())
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}


		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_LT16 (LEFT, RIGHT, OFFSET)  </b>
		 *
		 *- Branch relative OFFSET if LEFT is less than RIGHT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  LEFT
		 *  \param  (DATA16)  RIGHT
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_LT16 byte code
		 *
		 *
		 */
		public void cBranchJrLt16()
		{
			DATA16 Tmp;

			Tmp = *(DATA16*)GH.Lms.PrimParPointer();
			if (Tmp < *(DATA16*)GH.Lms.PrimParPointer())
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}


		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_LT32 (LEFT, RIGHT, OFFSET)  </b>
		 *
		 *- Branch relative OFFSET if LEFT is less than RIGHT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  LEFT
		 *  \param  (DATA32)  RIGHT
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_LT16 byte code
		 *
		 *
		 */
		public void cBranchJrLt32()
		{
			DATA32 Tmp;

			Tmp = *(DATA32*)GH.Lms.PrimParPointer();
			if (Tmp < *(DATA32*)GH.Lms.PrimParPointer())
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}


		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_LTF (LEFT, RIGHT, OFFSET)  </b>
		 *
		 *- Branch relative OFFSET if LEFT is less than RIGHT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATAF)   LEFT
		 *  \param  (DATAF)   RIGHT
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_LTF byte code
		 *
		 *
		 */
		public void cBranchJrLtF()
		{
			DATAF Tmp;

			Tmp = *(DATAF*)GH.Lms.PrimParPointer();
			if (Tmp < *(DATAF*)GH.Lms.PrimParPointer())
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}


		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_GT8 (LEFT, RIGHT, OFFSET)  </b>
		 *
		 *- Branch relative OFFSET if LEFT is greater than RIGHT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   LEFT
		 *  \param  (DATA8)   RIGHT
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_GT8 byte code
		 *
		 *
		 */
		public void cBranchJrGt8()
		{
			DATA8 Tmp;

			Tmp = *(DATA8*)GH.Lms.PrimParPointer();
			if (Tmp > *(DATA8*)GH.Lms.PrimParPointer())
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}


		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_GT16 (LEFT, RIGHT, OFFSET)  </b>
		 *
		 *- Branch relative OFFSET if LEFT is greater than RIGHT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  LEFT
		 *  \param  (DATA16)  RIGHT
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_GT16 byte code
		 *
		 *
		 */
		public void cBranchJrGt16()
		{
			DATA16 Tmp;

			Tmp = *(DATA16*)GH.Lms.PrimParPointer();
			if (Tmp > *(DATA16*)GH.Lms.PrimParPointer())
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}


		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_GT32 (LEFT, RIGHT, OFFSET)  </b>
		 *
		 *- Branch relative OFFSET if LEFT is greater than RIGHT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  LEFT
		 *  \param  (DATA32)  RIGHT
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_GT32 byte code
		 *
		 *
		 */
		public void cBranchJrGt32()
		{
			DATA32 Tmp;

			Tmp = *(DATA32*)GH.Lms.PrimParPointer();
			if (Tmp > *(DATA32*)GH.Lms.PrimParPointer())
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}


		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_GTF (LEFT, RIGHT, OFFSET)  </b>
		 *
		 *- Branch relative OFFSET if LEFT is greater than RIGHT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATAF)   LEFT
		 *  \param  (DATAF)   RIGHT
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_GTF byte code
		 *
		 *
		 */
		public void cBranchJrGtF()
		{
			DATAF Tmp;

			Tmp = *(DATAF*)GH.Lms.PrimParPointer();
			if (Tmp > *(DATAF*)GH.Lms.PrimParPointer())
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}


		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_EQ8 (LEFT, RIGHT, OFFSET)  </b>
		 *
		 *- Branch relative OFFSET if LEFT is equal to RIGHT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   LEFT
		 *  \param  (DATA8)   RIGHT
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_EQ8 byte code
		 *
		 *
		 */
		public void cBranchJrEq8()
		{
			DATA8 Tmp;

			Tmp = *(DATA8*)GH.Lms.PrimParPointer();
			if (Tmp == *(DATA8*)GH.Lms.PrimParPointer())
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}


		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_EQ16 (LEFT, RIGHT, OFFSET)  </b>
		 *
		 *- Branch relative OFFSET if LEFT is equal to RIGHT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  LEFT
		 *  \param  (DATA16)  RIGHT
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_EQ16 byte code
		 *
		 *
		 */
		public void cBranchJrEq16()
		{
			DATA16 Tmp;

			Tmp = *(DATA16*)GH.Lms.PrimParPointer();
			if (Tmp == *(DATA16*)GH.Lms.PrimParPointer())
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}


		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_EQ32 (LEFT, RIGHT, OFFSET)  </b>
		 *
		 *- Branch relative OFFSET if LEFT is equal to RIGHT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  LEFT
		 *  \param  (DATA32)  RIGHT
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_EQ32 byte code
		 *
		 *
		 */
		public void cBranchJrEq32()
		{
			DATA32 Tmp;

			Tmp = *(DATA32*)GH.Lms.PrimParPointer();
			if (Tmp == *(DATA32*)GH.Lms.PrimParPointer())
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}


		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_EQF (LEFT, RIGHT, OFFSET)  </b>
		 *
		 *- Branch relative OFFSET if LEFT is equal to RIGHT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATAF)   LEFT
		 *  \param  (DATAF)   RIGHT
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_EQF byte code
		 *
		 *
		 */
		public void cBranchJrEqF()
		{
			DATAF Tmp;

			Tmp = *(DATAF*)GH.Lms.PrimParPointer();
			if (Tmp == *(DATAF*)GH.Lms.PrimParPointer())
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}


		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_NEQ8 (LEFT, RIGHT, OFFSET)  </b>
		 *
		 *- Branch relative OFFSET if LEFT is not equal to RIGHT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   LEFT
		 *  \param  (DATA8)   RIGHT
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_NEQ8 byte code
		 *
		 *
		 */
		public void cBranchJrNEq8()
		{
			DATA8 Tmp;

			Tmp = *(DATA8*)GH.Lms.PrimParPointer();
			if (Tmp != *(DATA8*)GH.Lms.PrimParPointer())
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}


		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_NEQ16 (LEFT, RIGHT, OFFSET)  </b>
		 *
		 *- Branch relative OFFSET if LEFT is not equal to RIGHT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  LEFT
		 *  \param  (DATA16)  RIGHT
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_NEQ16 byte code
		 *
		 *
		 */
		public void cBranchJrNEq16()
		{
			DATA16 Tmp;

			Tmp = *(DATA16*)GH.Lms.PrimParPointer();
			if (Tmp != *(DATA16*)GH.Lms.PrimParPointer())
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}


		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_NEQ32 (LEFT, RIGHT, OFFSET)  </b>
		 *
		 *- Branch relative OFFSET if LEFT is not equal to RIGHT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  LEFT
		 *  \param  (DATA32)  RIGHT
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_NEQ32 byte code
		 *
		 *
		 */
		public void cBranchJrNEq32()
		{
			DATA32 Tmp;

			Tmp = *(DATA32*)GH.Lms.PrimParPointer();
			if (Tmp != *(DATA32*)GH.Lms.PrimParPointer())
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}


		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_NEQF (LEFT, RIGHT, OFFSET)  </b>
		 *
		 *- Branch relative OFFSET if LEFT is not equal to RIGHT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATAF)   LEFT
		 *  \param  (DATAF)   RIGHT
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_NEQF byte code
		 *
		 *
		 */
		public void cBranchJrNEqF()
		{
			DATAF Tmp;

			Tmp = *(DATAF*)GH.Lms.PrimParPointer();
			if (Tmp != *(DATAF*)GH.Lms.PrimParPointer())
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}


		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_LTEQ8 (LEFT, RIGHT, OFFSET)  </b>
		 *
		 *- Branch relative OFFSET if LEFT is less than or equal to RIGHT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   LEFT
		 *  \param  (DATA8)   RIGHT
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_LTEQ8 byte code
		 *
		 *
		 */
		public void cBranchJrLtEq8()
		{
			DATA8 Tmp;

			Tmp = *(DATA8*)GH.Lms.PrimParPointer();
			if (Tmp <= *(DATA8*)GH.Lms.PrimParPointer())
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}


		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_LTEQ16 (LEFT, RIGHT, OFFSET)  </b>
		 *
		 *- Branch relative OFFSET if LEFT is less than or equal to RIGHT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  LEFT
		 *  \param  (DATA16)  RIGHT
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_LTEQ16 byte code
		 *
		 *
		 */
		public void cBranchJrLtEq16()
		{
			DATA16 Tmp;

			Tmp = *(DATA16*)GH.Lms.PrimParPointer();
			if (Tmp <= *(DATA16*)GH.Lms.PrimParPointer())
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}


		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_LTEQ32 (LEFT, RIGHT, OFFSET)  </b>
		 *
		 *- Branch relative OFFSET if LEFT is less than or equal to RIGHT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  LEFT
		 *  \param  (DATA32)  RIGHT
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_LTEQ32 byte code
		 *
		 *
		 */
		public void cBranchJrLtEq32()
		{
			DATA32 Tmp;

			Tmp = *(DATA32*)GH.Lms.PrimParPointer();
			if (Tmp <= *(DATA32*)GH.Lms.PrimParPointer())
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}


		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_LTEQF (LEFT, RIGHT, OFFSET)  </b>
		 *
		 *- Branch relative OFFSET if LEFT is less than or equal to RIGHT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATAF)   LEFT
		 *  \param  (DATAF)   RIGHT
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_LTEQF byte code
		 *
		 *
		 */
		public void cBranchJrLtEqF()
		{
			DATAF Tmp;

			Tmp = *(DATAF*)GH.Lms.PrimParPointer();
			if (Tmp <= *(DATAF*)GH.Lms.PrimParPointer())
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}


		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_GTEQ8 (LEFT, RIGHT, OFFSET)  </b>
		 *
		 *- Branch relative OFFSET if LEFT is greater than or equal to RIGHT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   LEFT
		 *  \param  (DATA8)   RIGHT
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_GTEQ8 byte code
		 *
		 *
		 */
		public void cBranchJrGtEq8()
		{
			DATA8 Tmp;

			Tmp = *(DATA8*)GH.Lms.PrimParPointer();
			if (Tmp >= *(DATA8*)GH.Lms.PrimParPointer())
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}


		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_GTEQ16 (LEFT, RIGHT, OFFSET)  </b>
		 *
		 *- Branch relative OFFSET if LEFT is greater than or equal to RIGHT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)   LEFT
		 *  \param  (DATA16)   RIGHT
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_GTEQ16 byte code
		 *
		 *
		 */
		public void cBranchJrGtEq16()
		{
			DATA16 Tmp;

			Tmp = *(DATA16*)GH.Lms.PrimParPointer();
			if (Tmp >= *(DATA16*)GH.Lms.PrimParPointer())
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}


		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_GTEQ32 (LEFT, RIGHT, OFFSET)  </b>
		 *
		 *- Branch relative OFFSET if LEFT is greater than or equal to RIGHT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)   LEFT
		 *  \param  (DATA32)   RIGHT
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_GTEQ32 byte code
		 *
		 *
		 */
		public void cBranchJrGtEq32()
		{
			DATA32 Tmp;

			Tmp = *(DATA32*)GH.Lms.PrimParPointer();
			if (Tmp >= *(DATA32*)GH.Lms.PrimParPointer())
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}


		/*! \page cBranch
		 *  <hr size="1"/>
		 *  <b>     opJR_GTEQF (LEFT, RIGHT, OFFSET)  </b>
		 *
		 *- Branch relative OFFSET if LEFT is greater than or equal to RIGHT\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATAF)   LEFT
		 *  \param  (DATAF)   RIGHT
		 *  \param  (DATA32)  OFFSET
		 */
		/*! \brief  opJR_GTEQF byte code
		 *
		 *
		 */
		public void cBranchJrGtEqF()
		{
			DATAF Tmp;

			Tmp = *(DATAF*)GH.Lms.PrimParPointer();
			if (Tmp >= *(DATAF*)GH.Lms.PrimParPointer())
			{
				GH.Lms.AdjustObjectIp(*(IMOFFS*)GH.Lms.PrimParPointer());
			}
			else
			{
				GH.Lms.PrimParAdvance();
			}
		}
	}
}
