using Ev3CoreUnsafe.Enums;
using Ev3CoreUnsafe.Extensions;
using Ev3CoreUnsafe.Helpers;
using Ev3CoreUnsafe.Lms2012.Interfaces;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using static Ev3CoreUnsafe.Defines;

namespace Ev3CoreUnsafe.Lms2012
{
	public unsafe class Lms : ILms
	{
		/*! \brief    Get calling object id
 *
 *  \return   OBJID Current object id
 *
 */
		public OBJID CallingObjectId()
		{
			return (GH.VMInstance.ObjectId);
		}


		/*! \brief    Get current program id
		 *
		 *  \return   PRGID Current program id
		 *
		 */
		public PRGID CurrentProgramId()
		{
			return (GH.VMInstance.ProgramId);
		}


		/*! \brief    Get program status
		 *
		 *  \return   OBJSTAT Program status
		 *
		 */
		public OBJSTAT ProgramStatus(PRGID PrgId)
		{
			return (GH.VMInstance.Program[PrgId].Status);
		}


		/*! \brief    Get program status change
		 *
		 *  \return   OBJSTAT Program status change
		 *
		 */
		public OBJSTAT ProgramStatusChange(PRGID PrgId)
		{
			OBJSTAT Status;

			Status = (OBJSTAT)(DATA8)GH.VMInstance.Program[PrgId].StatusChange;
			GH.VMInstance.Program[PrgId].StatusChange = 0;

			return (Status);
		}


		/*! \brief    Get pointer to actual start of byte code image
		 *
		 *  \return   IP Pointer to image
		 *
		 */
		public IP GetImageStart()
		{
			return (GH.VMInstance.pImage);
		}


		/*! \brief    Set object (dispatch) status
		 *
		 *  \param    DspStat New dispatch status
		 *
		 */
		public void SetDispatchStatus(DSPSTAT DspStat)
		{
			GH.VMInstance.DispatchStatus = DspStat;

			if (GH.VMInstance.DispatchStatus != DSPSTAT.NOBREAK)
			{
				GH.VMInstance.Priority = 0;
			}

		}


		/*! \brief    Set instructions
		 *
		 *  \param    ULONG Instructions
		 *
		 */
		public void SetInstructions(ULONG Instructions)
		{
			if (Instructions <= PRG_PRIORITY)
			{
				GH.VMInstance.Priority = Instructions;
			}
		}


		/*! \brief    Adjust current instruction pointer
		 *
		 *  \param    Value Signed offset to add
		 *
		 */
		public void AdjustObjectIp(IMOFFS Value)
		{
			GH.VMInstance.ObjectIp += Value;
		}


		/*! \brief    Get current instruction pointer
		 *
		 *  \return   Current instruction pointer
		 *
		 */
		public IP GetObjectIp()
		{
			return (GH.VMInstance.ObjectIp);
		}


		/*! \brief    Set current instruction pointer
		 *
		 *  \param    Ip New instruction pointer value
		 *
		 */
		public void SetObjectIp(IP Ip)
		{
			GH.VMInstance.ObjectIp = Ip;
		}


		public ULONG GetTime()
		{
			return (GH.Timer.cTimerGetuS() - GH.VMInstance.Program[GH.VMInstance.ProgramId].RunTime);
		}


		public ULONG GetTimeMS()
		{
			return (GH.Timer.cTimerGetmS());
		}


		public ULONG GetTimeUS()
		{
			return (GH.Timer.cTimerGetuS());
		}


		public ULONG CurrentObjectIp()
		{
			return ((ULONG)(GH.VMInstance.ObjectIp - GH.VMInstance.pImage));
		}


		public void VmPrint(DATA8* pString)
		{
			GH.printf(CommonHelper.GetString(pString));
		}


		public void SetTerminalEnable(DATA8 Value)
		{
			GH.VMInstance.TerminalEnabled = Value;
		}


		public DATA8 GetTerminalEnable()
		{
			return (GH.VMInstance.TerminalEnabled);
		}


		public void GetResourcePath(DATA8* pString, DATA8 MaxLength)
		{
			GH.Memory.cMemoryGetResourcePath(GH.VMInstance.ProgramId, pString, MaxLength);
		}


		public void* VmMemoryResize(HANDLER Handle, DATA32 Elements)
		{
			return (GH.Memory.cMemoryResize(GH.VMInstance.ProgramId, Handle, Elements));
		}


		public void SetVolumePercent(DATA8 Volume)
		{
			(*GH.VMInstance.NonVol).VolumePercent = Volume;
		}


		public DATA8 GetVolumePercent()
		{
			return ((*GH.VMInstance.NonVol).VolumePercent);
		}


		public void SetSleepMinutes(DATA8 Minutes)
		{
			(*GH.VMInstance.NonVol).SleepMinutes = Minutes;
		}


		public DATA8 GetSleepMinutes()
		{
			return ((*GH.VMInstance.NonVol).SleepMinutes);
		}


		public void SetUiUpdate()
		{
			GH.UiInstance.UiUpdate = 1;
		}

		public void LogErrorNumber(ERR Err)
		{
			UBYTE Tmp;

			if (Err > TOO_MANY_ERRORS_TO_BUFFER)
			{
				Tmp = GH.VMInstance.ErrorIn;
				if (++Tmp >= ERROR_BUFFER_SIZE)
				{
					Tmp = 0;
				}
				if (Tmp != GH.VMInstance.ErrorOut)
				{
					GH.VMInstance.Errors[GH.VMInstance.ErrorIn] = Err;
					GH.VMInstance.ErrorIn = Tmp;
				}
				else
				{
					CommonHelper.snprintf(GH.VMInstance.PrintBuffer, PRINTBUFFERSIZE, $"\r\n#### {nameof(TOO_MANY_ERRORS_TO_BUFFER)} ####\r\n\n");
					VmPrint(GH.VMInstance.PrintBuffer);
				}
			}
		}


		public DATA8 LogErrorNumberExists(ERR Error)
		{
			DATA8 Result = 0;
			UBYTE Tmp;

			Tmp = GH.VMInstance.ErrorOut;
			while ((Tmp != GH.VMInstance.ErrorIn) && (Result == 0))
			{
				if (GH.VMInstance.Errors[Tmp] == Error)
				{
					Result = 1;
				}
				else
				{
					if (++Tmp >= ERROR_BUFFER_SIZE)
					{
						Tmp = 0;
					}
				}
			}

			return (Result);
		}


		public ERR LogErrorGet()
		{
			ERR Number = 0;

			if (GH.VMInstance.ErrorIn != GH.VMInstance.ErrorOut)
			{
				Number = GH.VMInstance.Errors[GH.VMInstance.ErrorOut];
				if (++GH.VMInstance.ErrorOut >= ERROR_BUFFER_SIZE)
				{
					GH.VMInstance.ErrorOut = 0;
				}
			}

			return (Number);
		}


		public void CleanLogErrors()
		{
			GH.VMInstance.ErrorIn = 0;
			GH.VMInstance.ErrorOut = 0;
		}


		/*! \brief    Execute byte code stream (C-call)
		 *
		 *  This call is able to execute up to "C_PRIORITY" byte codes instructions (no header necessary)
		 *
		 *  \param  (IP)      Pointer to byte code stream (last byte code must be "opOBJECT_END")
		 *  \param  (GP)      Pointer to global variables to use  (if none -> null)
		 *  \param  (LP)      Pointer to Local variables to use   (if none -> null)
		 *  \return (DSPSTAT) Dispatch status
		 *
		 *  Example:
		 *
		 *  IMGDATA BC1[] = { opUI_WRITE,PUT_STRING,LCS,'L','E','G','O','\r','\n',0,opUI_WRITE,WRITE_FLUSH,opOBJECT_END };
		 *  ExecuteByteCode(BC1,null,null);
		 *
		 *
		 */
		public DSPSTAT ExecuteByteCode(IP pByteCode, GP pGlobals, LP pLocals)
		{
			DSPSTAT Result;
			ULONG Time;

			// Save running object parameters
			GH.VMInstance.ObjIpSave = GH.VMInstance.ObjectIp;
			GH.VMInstance.ObjGlobalSave = GH.VMInstance.pGlobal;
			GH.VMInstance.ObjLocalSave = GH.VMInstance.ObjectLocal;
			GH.VMInstance.DispatchStatusSave = GH.VMInstance.DispatchStatus;
			GH.VMInstance.PrioritySave = GH.VMInstance.Priority;

			// InitExecute special byte code stream
			GH.VMInstance.ObjectIp = pByteCode;
			GH.VMInstance.pGlobal = pGlobals;
			GH.VMInstance.ObjectLocal = pLocals;
			GH.VMInstance.Priority = 1;

			// Execute special byte code stream
			GH.UiInstance.ButtonState[IDX_BACK_BUTTON] &= ~BUTTON_LONGPRESS;
			while ((*GH.VMInstance.ObjectIp != opOBJECT_END) && ((GH.UiInstance.ButtonState[IDX_BACK_BUTTON] & BUTTON_LONGPRESS) == 0))
			{
				GH.VMInstance.DispatchStatus = DSPSTAT.NOBREAK;
				GH.VMInstance.Priority = C_PRIORITY;

				while ((GH.VMInstance.Priority != 0) && (*GH.VMInstance.ObjectIp != opOBJECT_END))
				{
					GH.VMInstance.Priority--;
					PrimDispatchTabel[*(GH.VMInstance.ObjectIp++)]();
				}

				GH.VMInstance.NewTime = GetTimeMS();

				Time = GH.VMInstance.NewTime - GH.VMInstance.OldTime1;

				if (Time >= UPDATE_TIME1)
				{
					GH.VMInstance.OldTime1 += Time;

					GH.Com.cComUpdate();
					GH.Sound.cSoundUpdate();
				}

				Time = GH.VMInstance.NewTime - GH.VMInstance.OldTime2;

				if (Time >= UPDATE_TIME2)
				{
					GH.VMInstance.OldTime2 += Time;

					Thread.Sleep(1); // TODO: do i need the sleep?
					GH.Input.cInputUpdate((UWORD)Time);
					GH.Ui.cUiUpdate((UWORD)Time);
				}
			}
			Result = GH.VMInstance.DispatchStatus;

			GH.UiInstance.ButtonState[IDX_BACK_BUTTON] &= ~BUTTON_LONGPRESS;

			// Restore running object parameters
			GH.VMInstance.Priority = GH.VMInstance.PrioritySave;
			GH.VMInstance.DispatchStatus = GH.VMInstance.DispatchStatusSave;
			GH.VMInstance.ObjectLocal = GH.VMInstance.ObjLocalSave;
			GH.VMInstance.pGlobal = GH.VMInstance.ObjGlobalSave;
			GH.VMInstance.ObjectIp = GH.VMInstance.ObjIpSave;

			return (Result);
		}


		/*! \page howtobytecodes How To Pass Parameters
		 *
		 *  Examples of parsing parameters to byte codes :
		 *
		 * \verbatim


		1.  Move 8 bit signed value to floating point variable


			opMOVE8_F (SOURCE, DESTINATION)

			Parameters:
				(DATA8) SOURCE              (could be constant or variable number)

			Returns:
				(DATAF) DESTINATINATION     (this must be a variable number that has 4 bytes allocated and is aligned at a 4 byte boundary)






		2.  Pass two parameters to a SUBCALL

			Parameters must be aligned so 32 bit variables come first then 16 bit and at last 8 bit variables

			Parameters are placed in the SUBCALLs local variable space so the first parameter will be LV0(0)

			When referring to parameters in a SUBCALL refer to local variables.

			Array handles must be seen as input (or input/output) parameters because it is a handle (number)
			to be copied into the subcalls local parameters.
		   \endverbatim
		-   \ref subparexample1

		 *
		 */


		/*! \page parameterencoding Parameter Encoding
		 *
		 *  Parameter types and values for primitives, system calls and subroutine calls are encoded in the callers byte code stream as follows:
		 *
		 *  opADD8 (ParCode1, ParCode2, ParCodeN)
		 * \verbatim
		Bits  76543210
			  --------
			  0Ttxxxxx    short format
			  ||||||||
			  |0||||||    constant
			  ||||||||
			  ||0vvvvv    positive value
			  ||1vvvvv    negative value
			  |||
			  |1|         variable
			  | |
			  | 0iiiii    local index
			  | 1iiiii    global index
			  |
			  1ttt-bbb    long format
			   ||| |||
			   0|| |||    constant
			   ||| |||
			   |0| |||    value
			   |1| |||    label
			   ||| |||
			   1|| |||    variable
				|| |||
				0| |||    local
				1| |||    global
				 | |||
				 0 |||    value
				 1 |||    handle
				   |||
				   000    Zero terminated string  (subject to change)
				   001    1 bytes to follow       (subject to change)
				   010    2 bytes to follow       (subject to change)
				   011    4 bytes to follow       (subject to change)
				   100    Zero terminated string  \endverbatim
		 *
		 */


		/*! \brief    Get next encoded parameter from byte code stream
		 *
		 *  \return   void Pointer to value
		 *
		 *
		 */
		public void* PrimParPointer()
		{
			void* Result;
			IMGDATA Data;

			Result = (void*)Unsafe.AsPointer(ref GH.VMInstance.Value);
			Data = *((IMGDATA*)GH.VMInstance.ObjectIp++);
			GH.VMInstance.Handle = -1;

			if ((Data & PRIMPAR_LONG) != 0)
			{ // long format

				if ((Data & PRIMPAR_VARIABEL) != 0)
				{ // variabel

					switch (Data & PRIMPAR_BYTES)
					{

						case PRIMPAR_1_BYTE:
							{ // One byte to follow

								GH.VMInstance.Value = (ULONG)(*((IMGDATA*)GH.VMInstance.ObjectIp++));
							}
							break;

						case PRIMPAR_2_BYTES:
							{ // Two bytes to follow

								GH.VMInstance.Value = (ULONG)(*((IMGDATA*)GH.VMInstance.ObjectIp++));
								GH.VMInstance.Value |= ((ULONG)(*((IMGDATA*)GH.VMInstance.ObjectIp++) << 8));
							}
							break;

						case PRIMPAR_4_BYTES:
							{ // Four bytes to follow

								GH.VMInstance.Value = (ULONG)(*((IMGDATA*)GH.VMInstance.ObjectIp++));
								GH.VMInstance.Value |= ((ULONG)(*((IMGDATA*)GH.VMInstance.ObjectIp++) << 8));
								GH.VMInstance.Value |= ((ULONG)(*((IMGDATA*)GH.VMInstance.ObjectIp++) << 16));
								GH.VMInstance.Value |= ((ULONG)(*((IMGDATA*)GH.VMInstance.ObjectIp++) << 24));
							}
							break;

					}
					if ((Data & PRIMPAR_GLOBAL) != 0)
					{ // global

						Result = (void*)(&GH.VMInstance.pGlobal[GH.VMInstance.Value]);
					}
					else
					{ // local

						Result = (void*)(&GH.VMInstance.ObjectLocal[GH.VMInstance.Value]);
					}
				}
				else
				{ // constant

					if ((Data & PRIMPAR_LABEL) != 0)
					{ // label

						GH.VMInstance.Value = (ULONG)(*((IMGDATA*)GH.VMInstance.ObjectIp++));

						if ((GH.VMInstance.Value > 0) && (GH.VMInstance.Value < MAX_LABELS))
						{
							GH.VMInstance.Value = (ULONG)((LABEL*)GH.VMInstance.Program[GH.VMInstance.ProgramId].Label)[GH.VMInstance.Value].Addr;
							GH.VMInstance.Value -= ((ULONG)GH.VMInstance.ObjectIp - (ULONG)GH.VMInstance.Program[GH.VMInstance.ProgramId].pImage);
							Result = (void*)Unsafe.AsPointer(ref GH.VMInstance.Value);
						}
					}
					else
					{ // value

						switch (Data & PRIMPAR_BYTES)
						{
							case PRIMPAR_STRING_OLD:
							case PRIMPAR_STRING:
								{ // Zero terminated

									Result = (DATA8*)GH.VMInstance.ObjectIp;
									while (*((IMGDATA*)GH.VMInstance.ObjectIp++) != 0)
									{ // Adjust Ip
									}
								}
								break;

							case PRIMPAR_1_BYTE:
								{ // One byte to follow

									GH.VMInstance.Value = (ULONG)(*((IMGDATA*)GH.VMInstance.ObjectIp++));
									if ((GH.VMInstance.Value & 0x00000080) != 0)
									{ // Adjust if negative

										GH.VMInstance.Value |= 0xFFFFFF00;
									}
								}
								break;

							case PRIMPAR_2_BYTES:
								{ // Two bytes to follow

									GH.VMInstance.Value = (ULONG)(*((IMGDATA*)GH.VMInstance.ObjectIp++));
									GH.VMInstance.Value |= ((ULONG)(*((IMGDATA*)GH.VMInstance.ObjectIp++) << 8));
									if ((GH.VMInstance.Value & 0x00008000) != 0)
									{ // Adjust if negative

										GH.VMInstance.Value |= 0xFFFF0000;
									}
								}
								break;

							case PRIMPAR_4_BYTES:
								{ // Four bytes to follow

									GH.VMInstance.Value = (ULONG)(*((IMGDATA*)GH.VMInstance.ObjectIp++));
									GH.VMInstance.Value |= ((ULONG)(*((IMGDATA*)GH.VMInstance.ObjectIp++) << 8));
									GH.VMInstance.Value |= ((ULONG)(*((IMGDATA*)GH.VMInstance.ObjectIp++) << 16));
									GH.VMInstance.Value |= ((ULONG)(*((IMGDATA*)GH.VMInstance.ObjectIp++) << 24));
								}
								break;
						}
					}
				}
				if ((Data & PRIMPAR_HANDLE) != 0)
				{
					GH.VMInstance.Handle = *(HANDLER*)Result;
					GH.Memory.cMemoryArraryPointer(GH.VMInstance.ProgramId, GH.VMInstance.Handle, &Result);
				}
				else
				{
					if ((Data & PRIMPAR_ADDR) != 0)
					{
						Result = (void*)*(DATA32*)Result;
						GH.VMInstance.Value = (uint)(DATA32)Result;
					}
				}
			}
			else
			{ // short format

				if ((Data & PRIMPAR_VARIABEL) != 0)
				{ // variabel

					GH.VMInstance.Value = (ULONG)(Data & PRIMPAR_INDEX);

					if ((Data & PRIMPAR_GLOBAL) != 0)
					{ // global

						Result = (void*)(&GH.VMInstance.pGlobal[GH.VMInstance.Value]);
					}
					else
					{ // local

						Result = (void*)(&GH.VMInstance.ObjectLocal[GH.VMInstance.Value]);
					}
				}
				else
				{ // constant

					GH.VMInstance.Value = (ULONG)(Data & PRIMPAR_VALUE);

					if ((Data & PRIMPAR_CONST_SIGN) != 0)
					{ // Adjust if negative

						GH.VMInstance.Value |= ~(ULONG)(PRIMPAR_VALUE);
					}
				}
			}

			return (Result);
		}


		/*! \brief    Skip next encoded parameter from byte code stream
		 *
		 *
		 */
		public void PrimParAdvance()
		{
			IMGDATA Data;

			Data = *((IMGDATA*)GH.VMInstance.ObjectIp++);

			if ((Data & PRIMPAR_LONG) != 0)
			{ // long format

				if ((Data & PRIMPAR_VARIABEL) != 0)
				{ // variabel

					switch (Data & PRIMPAR_BYTES)
					{

						case PRIMPAR_1_BYTE:
							{ // One byte to follow

								GH.VMInstance.ObjectIp++;
							}
							break;

						case PRIMPAR_2_BYTES:
							{ // Two bytes to follow

								GH.VMInstance.ObjectIp++;
								GH.VMInstance.ObjectIp++;
							}
							break;

						case PRIMPAR_4_BYTES:
							{ // Four bytes to follow

								GH.VMInstance.ObjectIp++;
								GH.VMInstance.ObjectIp++;
								GH.VMInstance.ObjectIp++;
								GH.VMInstance.ObjectIp++;
							}
							break;
					}
				}
				else
				{ // constant

					if ((Data & PRIMPAR_LABEL) != 0)
					{ // label

						GH.VMInstance.ObjectIp++;

					}
					else
					{ // value

						switch (Data & PRIMPAR_BYTES)
						{
							case PRIMPAR_STRING_OLD:
							case PRIMPAR_STRING:
								{ // Zero terminated

									while (*((IMGDATA*)GH.VMInstance.ObjectIp++) != 0)
									{ // Adjust Ip
									}
								}
								break;

							case PRIMPAR_1_BYTE:
								{ // One byte to follow

									GH.VMInstance.ObjectIp++;
								}
								break;

							case PRIMPAR_2_BYTES:
								{ // Two bytes to follow

									GH.VMInstance.ObjectIp++;
									GH.VMInstance.ObjectIp++;
								}
								break;

							case PRIMPAR_4_BYTES:
								{ // Four bytes to follow

									GH.VMInstance.ObjectIp++;
									GH.VMInstance.ObjectIp++;
									GH.VMInstance.ObjectIp++;
									GH.VMInstance.ObjectIp++;
								}
								break;
						}
					}
				}
			}
		}


		/*! \page parameterencoding
		 *
		 *  \anchor subpar
		 *
		 *  For subroutine parameters additional information about no of pars, direction and size are needed and placed in front of the called code\n
		 *  Parameters MUST be sorted so: largest (4 bytes) parameters is first and smallest (1 byte) is last in the list.
		 *
		 *  OffsetToInstructions -> NoOfPars, ParType1, ParType2, ParTypen
		 * \verbatim
		Bits  76543210
			  --------
			  io---bbb    long format
			  ||   |||
			  1x   |||    parameter in
			  x1   |||    parameter out
				   |||
				   000    8  bits
				   001    16 bits
				   010    32 bits
				   011    float
				   100    Zero terminated string (next byte tells allocated size) \endverbatim
		 *
		 */
		/*! \brief    Copy encoded parameters to local variables
		 *
		 *  \param    Id Object to copy to
		 *
		 *
		 */
		public void CopyParsToLocals(OBJID Id)
		{
			IP TmpIp;      // Save calling Ip
			IP TypeIp;     // Called Ip
			void* pLocals;    // Called locals
			PARS NoOfPars;   // Called no of parameters
			IMGDATA Type;       // Coded type
			void* Result;     // Pointer to value
			DATA32 Size;
			DATA8 Flag;

			TmpIp = GH.VMInstance.ObjectIp;
			TypeIp = GH.VMInstance.Program[GH.VMInstance.ProgramId].pImage;
			TypeIp = &TypeIp[(ULONG)GH.VMInstance.pObjHead[Id].OffsetToInstructions];
			pLocals = (*GH.VMInstance.pObjList[Id]).pLocal;

			NoOfPars = (PARS)(*((IMGDATA*)TypeIp++));

			while (NoOfPars-- != 0)
			{
				// Get type from sub preamble
				Type = (IMGDATA)(*((IMGDATA*)TypeIp++));

				// Get pointer to value and increment GH.VMInstance.ObjectIP

				Result = PrimParPointer();

				if ((Type & CALLPAR_IN) != 0)
				{ // Input

					switch (Type & CALLPAR_TYPE)
					{

						case CALLPAR_DATA8:
							{
								(*(DATA8*)pLocals) = *(DATA8*)Result;
								pLocals = ((DATA8*)pLocals) + 1;
							}
							break;

						case CALLPAR_DATA16:
							{
								pLocals = (void*)(((IMINDEX)pLocals + 1) & ~1);
								(*(DATA16*)pLocals) = *(DATA16*)Result;
								pLocals = ((DATA16*)pLocals) + 1;
							}
							break;

						case CALLPAR_DATA32:
							{
								pLocals = (void*)(((IMINDEX)pLocals + 3) & ~3);
								(*(DATA32*)pLocals) = *(DATA32*)Result;
								pLocals = ((DATA32*)pLocals) + 1;
							}
							break;

						case CALLPAR_DATAF:
							{
								pLocals = (void*)(((IMINDEX)pLocals + 3) & ~3);
								(*(DATAF*)pLocals) = *(DATAF*)Result;
								pLocals = ((DATAF*)pLocals) + 1;
							}
							break;

						case CALLPAR_STRING:
							{
								Size = (DATA32)(*((IMGDATA*)TypeIp++));
								Flag = 1;
								while (Size != 0)
								{
									if (Flag != 0)
									{
										Flag = *(DATA8*)Result;
									}
									(*(DATA8*)pLocals) = Flag;
									Result = ((DATA8*)Result) + 1;
									pLocals = ((DATA8*)pLocals) + 1;
									Size--;
								}
								pLocals = ((DATA8*)pLocals) - 1;
								(*(DATA8*)pLocals) = 0;
								pLocals = ((DATA8*)pLocals) + 1;
							}
							break;

					}
				}
				else
				{
					if ((Type & CALLPAR_OUT) != 0)
					{ // Output

						switch (Type & CALLPAR_TYPE)
						{

							case CALLPAR_DATA8:
								{
									pLocals = ((DATA8*)pLocals) + 1;
								}
								break;

							case CALLPAR_DATA16:
								{
									pLocals = (void*)(((IMINDEX)pLocals + 1) & ~1);
									pLocals = ((DATA16*)pLocals) + 1;
								}
								break;

							case CALLPAR_DATA32:
								{
									pLocals = (void*)(((IMINDEX)pLocals + 3) & ~3);
									pLocals = ((DATA32*)pLocals) + 1;
								}
								break;

							case CALLPAR_DATAF:
								{
									pLocals = (void*)(((IMINDEX)pLocals + 3) & ~3);
									pLocals = ((DATAF*)pLocals) + 1;
								}
								break;

							case CALLPAR_STRING:
								{
									Size = (DATA32)(*((IMGDATA*)TypeIp++));
									pLocals = ((DATA8*)pLocals) + Size;
								}
								break;

						}
					}
				}
			}
		  (*GH.VMInstance.pObjList[Id]).Ip = TypeIp;

			// Rewind caller Ip
			GH.VMInstance.ObjectIp = TmpIp;
		}


		/*! \brief    Copy local variables to encoded parameters
		 *
		 *  \param    Id Object to copy to
		 *
		 *
		 */
		public void CopyLocalsToPars(OBJID Id)
		{
			IP TmpIp;      // Calling Ip
			IP TypeIp;     // Called Ip
			void* pLocals;    // Called locals
			PARS NoOfPars;   // Called no of parameters
			IMGDATA Type;       // Coded type
			void* Result;     // Pointer to value
			DATA32 Size;
			DATA8 Flag;

			// Point to start of parameters
			TmpIp = GH.VMInstance.ObjectIp;
			GH.VMInstance.ObjectIp = (*GH.VMInstance.pObjList[Id]).Ip;

			// Point to start of sub
			TypeIp = GH.VMInstance.Program[GH.VMInstance.ProgramId].pImage;
			TypeIp = &TypeIp[(ULONG)GH.VMInstance.pObjHead[GH.VMInstance.ObjectId].OffsetToInstructions];
			pLocals = (*GH.VMInstance.pObjList[GH.VMInstance.ObjectId]).pLocal;

			NoOfPars = (PARS)(*((IMGDATA*)TypeIp++));

			while (NoOfPars-- != 0)
			{
				// Get type from sub preamble
				Type = (IMGDATA)(*((IMGDATA*)TypeIp++));

				// Get pointer to value and increment GH.VMInstance.ObjectIp
				Result = PrimParPointer();

				if ((Type & CALLPAR_OUT) != 0)
				{ // Output

					switch (Type & CALLPAR_TYPE)
					{

						case CALLPAR_DATA8:
							{
								*(DATA8*)Result = (*(DATA8*)pLocals);
								pLocals = ((DATA8*)pLocals) + 1;
							}
							break;

						case CALLPAR_DATA16:
							{
								pLocals = (void*)(((IMINDEX)pLocals + 1) & ~1);
								*(DATA16*)Result = (*(DATA16*)pLocals);
								pLocals = ((DATA16*)pLocals) + 1;
							}
							break;

						case CALLPAR_DATA32:
							{
								pLocals = (void*)(((IMINDEX)pLocals + 3) & ~3);
								*(DATA32*)Result = (*(DATA32*)pLocals);
								pLocals = ((DATA32*)pLocals) + 1;
							}
							break;

						case CALLPAR_DATAF:
							{
								pLocals = (void*)(((IMINDEX)pLocals + 3) & ~3);
								*(DATAF*)Result = (*(DATAF*)pLocals);
								pLocals = ((DATAF*)pLocals) + 1;
							}
							break;

						case CALLPAR_STRING:
							{
								Size = (DATA32)(*((IMGDATA*)TypeIp++));
								Flag = 1;
								while (Size != 0)
								{
									if (Flag != 0)
									{
										Flag = (*(DATA8*)pLocals);
									}
									*(DATA8*)Result = Flag;
									Result = ((DATA8*)Result) + 1;
									pLocals = ((DATA8*)pLocals) + 1;
									Size--;
								}
								Result = ((DATA8*)Result) - 1;
								*(DATA8*)Result = 0;
							}
							break;

					}
				}
				else
				{
					if ((Type & CALLPAR_IN) != 0)
					{ // Input

						switch (Type & CALLPAR_TYPE)
						{

							case CALLPAR_DATA8:
								{
									pLocals = ((DATA8*)pLocals) + 1;
								}
								break;

							case CALLPAR_DATA16:
								{
									pLocals = (void*)(((IMINDEX)pLocals + 1) & ~1);
									pLocals = ((DATA16*)pLocals) + 1;
								}
								break;

							case CALLPAR_DATA32:
								{
									pLocals = (void*)(((IMINDEX)pLocals + 3) & ~3);
									pLocals = ((DATA32*)pLocals) + 1;
								}
								break;

							case CALLPAR_DATAF:
								{
									pLocals = (void*)(((IMINDEX)pLocals + 3) & ~3);
									pLocals = ((DATAF*)pLocals) + 1;
								}
								break;

							case CALLPAR_STRING:
								{
									Size = (DATA32)(*((IMGDATA*)TypeIp++));
									pLocals = ((DATA8*)pLocals) + Size;
								}
								break;

						}
					}
				}
			}

		  // Adjust caller Ip
		  (*GH.VMInstance.pObjList[Id]).Ip = GH.VMInstance.ObjectIp;
			// Restore calling Ip
			GH.VMInstance.ObjectIp = TmpIp;
		}


		//*****************************************************************************
		// VM routines
		//*****************************************************************************

		/*! \brief    Initialise object instruction pointer and trigger counter
		 *
		 *  \param    ObjId Object to reset
		 *
		 */
		public void ObjectReset(OBJID ObjId)
		{
			(*GH.VMInstance.pObjList[ObjId]).Ip = &GH.VMInstance.pImage[(ULONG)GH.VMInstance.pObjHead[ObjId].OffsetToInstructions];

			(*GH.VMInstance.pObjList[ObjId]).TriggerCount = GH.VMInstance.pObjHead[ObjId].TriggerCount;
		}


		/*! \brief    Get amount of ram to allocate for program
		 *
		 *  \param    pI Pointer to image
		 *
		 */
		public GBINDEX GetAmountOfRamForImage(IP pI)
		{
			GBINDEX Bytes = 0;
			OBJID NoOfObj;
			OBJID ObjId;
			OBJHEAD* pHead;

			NoOfObj = (*(IMGHEAD*)pI).NumberOfObjects;

			Bytes += (*(IMGHEAD*)pI).GlobalBytes;
			Bytes = (Bytes + 3) & 0xFFFFFFFC;
			Bytes += (uint)(8 * (NoOfObj + 1));

			pHead = (OBJHEAD*)&pI[sizeof(IMGHEAD)];

			for (ObjId = 1; ObjId <= NoOfObj; ObjId++)
			{
				Bytes = (Bytes + 3) & 0xFFFFFFFC;
				Bytes += (uint)(OBJ.Sizeof + (*pHead).LocalBytes);
				pHead++;
			}

			Bytes = 0;

			NoOfObj = (*(IMGHEAD*)pI).NumberOfObjects;

			Bytes += (*(IMGHEAD*)pI).GlobalBytes;
			Bytes = (Bytes + 3) & 0xFFFFFFFC;
			Bytes += (uint)(8 * (NoOfObj + 1));

			pHead = (OBJHEAD*)&pI[sizeof(IMGHEAD) - sizeof(OBJHEAD)];

			for (ObjId = 1; ObjId <= NoOfObj; ObjId++)
			{
				Bytes = (Bytes + 3) & 0xFFFFFFFC;
				Bytes += (uint)(OBJ.Sizeof + pHead[ObjId].LocalBytes);
			}

			return (Bytes);
		}


		/*! \brief    Initialise program for execution
		 *
		 *  \param    PrgId Program id (index)
		 *  \param    pI    Pointer to image
		 *  \param    pG    Pointer to global variables
		 *  \param    Deb   debug flag
		 *
		 *
		 */
		public RESULT ProgramReset(PRGID PrgId, IP pI, GP pG, UBYTE Deb)
		{

			RESULT Result = RESULT.FAIL;
			GBINDEX Index;
			GBINDEX RamSize;
			VARDATA* pData;
			OBJID ObjIndex;
			DATA8 No;

			GH.VMInstance.Program[PrgId].Status = OBJSTAT.STOPPED;
			GH.VMInstance.Program[PrgId].StatusChange = OBJSTAT.STOPPED;
			GH.VMInstance.Program[PrgId].Result = RESULT.FAIL;

			if (pI != null)
			{

				// Allocate memory for globals and objects

				RamSize = GetAmountOfRamForImage(pI);

				if (GH.Memory.cMemoryOpen(PrgId, RamSize, (void**)&pData) == OK)
				{ // Memory reserved

					// Save start of image
					if (Deb == 1)
					{
						GH.VMInstance.Program[PrgId].Debug = 1;
					}
					else
					{
						GH.VMInstance.Program[PrgId].Debug = 0;
					}
					GH.VMInstance.Program[PrgId].pImage = pI;

					if (GH.Validate.cValidateProgram(PrgId, pI, (LABEL*)GH.VMInstance.Program[PrgId].Label, GH.VMInstance.TerminalEnabled) != OK)
					{
						if (PrgId != CMD_SLOT)
						{
							LogErrorNumber((ERR)VM_PROGRAM_VALIDATION);
						}
					}
					else
					{

						// Clear memory

						for (Index = 0; Index < RamSize; Index++)
						{
							pData[Index] = 0;
						}

						for (No = 0; No < MAX_BREAKPOINTS; No++)
						{
							((BRKP*)GH.VMInstance.Program[PrgId].Brkp)[No].Addr = 0;
							((BRKP*)GH.VMInstance.Program[PrgId].Brkp)[No].OpCode = 0;
						}

						// Get GH.VMInstance.Objects

						GH.VMInstance.Program[PrgId].Objects = (*(IMGHEAD*)pI).NumberOfObjects;

						// Allocate GlobalVariables

						GH.VMInstance.Program[PrgId].pData = pData;
						if (pG != null)
						{
							GH.VMInstance.Program[PrgId].pGlobal = pG;
						}
						else
						{
							GH.VMInstance.Program[PrgId].pGlobal = pData;
						}

						pData = &pData[(*(IMGHEAD*)pI).GlobalBytes];

						// Align & allocate ObjectPointerList (+1)

						pData = (VARDATA*)(((ULONG)pData + 3) & 0xFFFFFFFC);
						GH.VMInstance.Program[PrgId].pObjList = (OBJ**)pData;
						pData = &pData[8 * (GH.VMInstance.Program[PrgId].Objects + 1)];

						// Make pointer to access object headers starting at one (not zero)

						GH.VMInstance.Program[PrgId].pObjHead = (OBJHEAD*)&pI[sizeof(IMGHEAD) - sizeof(OBJHEAD)];

						for (ObjIndex = 1; ObjIndex <= GH.VMInstance.Program[PrgId].Objects; ObjIndex++)
						{
							// Align

							pData = (VARDATA*)(((ULONG)pData + 3) & 0xFFFFFFFC);

							// Save object pointer in Object list

							OBJ* dataTmp = (OBJ*)pData;
							GH.VMInstance.Program[PrgId].pObjList[ObjIndex] = dataTmp;

							// Initialise instruction pointer, trigger counts and status

							(*GH.VMInstance.Program[PrgId].pObjList[ObjIndex]).Ip = &pI[(ULONG)GH.VMInstance.Program[PrgId].pObjHead[ObjIndex].OffsetToInstructions];

							(*GH.VMInstance.Program[PrgId].pObjList[ObjIndex]).TriggerCount = GH.VMInstance.Program[PrgId].pObjHead[ObjIndex].TriggerCount;

							if (((*GH.VMInstance.Program[PrgId].pObjList[ObjIndex]).TriggerCount != 0) || (ObjIndex > 1))
							{
								(*GH.VMInstance.Program[PrgId].pObjList[ObjIndex]).ObjStatus = (ushort)OBJSTAT.STOPPED;
							}
							else
							{
								if (Deb == 2)
								{
									(*GH.VMInstance.Program[PrgId].pObjList[ObjIndex]).ObjStatus = WAITING;
								}
								else
								{
									(*GH.VMInstance.Program[PrgId].pObjList[ObjIndex]).ObjStatus = (ushort)OBJSTAT.RUNNING;
								}
							}

							if (GH.VMInstance.Program[PrgId].pObjHead[ObjIndex].OwnerObjectId != 0)
							{
								(*GH.VMInstance.Program[PrgId].pObjList[ObjIndex]).pLocal = (*GH.VMInstance.Program[PrgId].pObjList[GH.VMInstance.Program[PrgId].pObjHead[ObjIndex].OwnerObjectId]).Local;
							}
							else
							{
								(*GH.VMInstance.Program[PrgId].pObjList[ObjIndex]).pLocal = (*GH.VMInstance.Program[PrgId].pObjList[ObjIndex]).Local;
							}

							// Advance data pointer

							pData = &pData[OBJ.Sizeof + GH.VMInstance.Program[PrgId].pObjHead[ObjIndex].LocalBytes];
						}

						GH.VMInstance.Program[PrgId].ObjectId = 1;
						GH.VMInstance.Program[PrgId].Status = OBJSTAT.RUNNING;
						GH.VMInstance.Program[PrgId].StatusChange = OBJSTAT.RUNNING;

						GH.VMInstance.Program[PrgId].Result = RESULT.BUSY;

						Result = OK;

						if (PrgId == USER_SLOT)
						{

							if (GH.VMInstance.RefCount == 0)
							{
								Result = 0;
								Result |= GH.Ui.cUiOpen();
								Result |= GH.Output.cOutputOpen();
								Result |= GH.Input.cInputOpen();
								Result |= GH.Com.cComOpen();
								Result |= GH.Sound.cSoundOpen();
							}
							GH.VMInstance.RefCount++;
						}
						GH.VMInstance.Program[PrgId].InstrCnt = 0;
						GH.VMInstance.Program[PrgId].StartTime = GetTimeMS();
						GH.VMInstance.Program[PrgId].RunTime = GH.Timer.cTimerGetuS();
					}
				}
			}

			return (Result);
		}


		/*! \brief    Exit program nicely (freeing up memory)
		 *
		 *  \param    PrgId Program id (index)
		 *
		 *
		 */
		public void ProgramEnd(PRGID PrgId)
		{
			if (GH.VMInstance.Program[PrgId].Status != OBJSTAT.STOPPED)
			{

				GH.VMInstance.Program[PrgId].InstrTime = GH.Timer.cTimerGetuS() - GH.VMInstance.Program[PrgId].RunTime;

				GH.VMInstance.Program[PrgId].Objects = 0;
				GH.VMInstance.Program[PrgId].Status = OBJSTAT.STOPPED;
				GH.VMInstance.Program[PrgId].StatusChange = OBJSTAT.STOPPED;
				if (PrgId != 0)
				{
					if (PrgId != GH.VMInstance.ProgramId)
					{
						GH.VMInstance.Program[PrgId].InstrCnt += GH.VMInstance.InstrCnt;
					}
				}

				if (PrgId == USER_SLOT)
				{
					if (GH.VMInstance.RefCount != 0)
					{
						GH.VMInstance.RefCount--;
					}
					if (GH.VMInstance.RefCount == 0)
					{
						GH.Sound.cSoundClose();
						GH.Com.cComClose();
						GH.Input.cInputClose();
						GH.Output.cOutputClose();
						GH.Ui.cUiClose();
					}
				}

				GH.Memory.cMemoryClose(PrgId);

				if (PrgId == GH.VMInstance.ProgramId)
				{
					SetDispatchStatus(DSPSTAT.PRGBREAK);
				}

				GH.VMInstance.Program[PrgId].Result = OK;
			}
		}


		/*! \brief    Switch in current program
		 *            ProgramId holds program to switch to
		 *
		 */
		public void ProgramInit()
		{
			PRG* pProgram;

			if (GH.VMInstance.ProgramId < MAX_PROGRAMS)
			{
				pProgram = &GH.VMInstance.Program[GH.VMInstance.ProgramId];

				if (((*pProgram).Status == OBJSTAT.RUNNING) || ((*pProgram).Status == OBJSTAT.WAITING))
				{
					GH.VMInstance.pGlobal = (*pProgram).pGlobal;
					GH.VMInstance.pImage = (*pProgram).pImage;
					GH.VMInstance.pObjHead = (*pProgram).pObjHead;
					GH.VMInstance.pObjList = (*pProgram).pObjList;
					GH.VMInstance.Objects = (*pProgram).Objects;
					GH.VMInstance.ObjectId = (*pProgram).ObjectId;
					GH.VMInstance.ObjectIp = (*pProgram).ObjectIp;
					GH.VMInstance.ObjectLocal = (*pProgram).ObjectLocal;
					GH.VMInstance.InstrCnt = 0;
					GH.VMInstance.Debug = (*pProgram).Debug;

				}
			}
		}


		/*! \brief    Switch out current program
		 *            ProgramId holds program to switch from
		 *
		 */
		public void ProgramExit()
		{
			PRG* pProgram;

			if (GH.VMInstance.ProgramId < MAX_PROGRAMS)
			{
				pProgram = &GH.VMInstance.Program[GH.VMInstance.ProgramId];
				(*pProgram).pGlobal = GH.VMInstance.pGlobal;
				(*pProgram).pImage = GH.VMInstance.pImage;
				(*pProgram).pObjHead = GH.VMInstance.pObjHead;
				(*pProgram).pObjList = GH.VMInstance.pObjList;
				(*pProgram).Objects = GH.VMInstance.Objects;
				(*pProgram).ObjectId = GH.VMInstance.ObjectId;
				(*pProgram).ObjectIp = GH.VMInstance.ObjectIp;
				(*pProgram).ObjectLocal = GH.VMInstance.ObjectLocal;
				(*pProgram).InstrCnt += GH.VMInstance.InstrCnt;
				(*pProgram).Debug = GH.VMInstance.Debug;

				GH.VMInstance.InstrCnt = 0;
				(*pProgram).Debug = GH.VMInstance.Debug;
			}
		}


		/*! \brief    Find a program to run
		 *            ProgramId holds found program
		 *
		 *  \return   RESULT Succes [OK or RESULT.STOP]
		 */
		public RESULT ProgramExec()
		{
			RESULT Result = RESULT.STOP;
			OBJID TmpId = 0;


			for (TmpId = 0; (TmpId < MAX_PROGRAMS) && (Result == RESULT.STOP); TmpId++)
			{
				if (GH.VMInstance.Program[TmpId].Status != OBJSTAT.STOPPED)
				{
					Result = OK;
				}
			}
			if (Result == OK)
			{
				do
				{
					// next program

					if (++GH.VMInstance.ProgramId >= MAX_PROGRAMS)
					{
						// wrap around

						GH.VMInstance.ProgramId = 0;
					}

				}
				while (GH.VMInstance.Program[GH.VMInstance.ProgramId].Status == OBJSTAT.STOPPED);
			}


			return (Result);
		}


		/*! \brief    Restore object context
		 *
		 *            Restore object context if id is valid and running by loading current IP and current pointer to Locals
		 *
		 *            Uses following from current program context:
		 *            ObjectId, Objects, pObjList, pObjHead, ObjectIp, ObjectLocal
		 *
		 *  \return   DSPSTAT New value for dispatch status [DSPSTAT.NOBREAK, DSPSTAT.STOPBREAK]
		 *
		 *
		 */
		public DSPSTAT ObjectInit()
		{
			DSPSTAT Result = DSPSTAT.STOPBREAK;

			if ((GH.VMInstance.ObjectId > 0) && (GH.VMInstance.ObjectId <= GH.VMInstance.Objects))
			{ // object valid

				if ((*GH.VMInstance.pObjList[GH.VMInstance.ObjectId]).ObjStatus == RUNNING)
				{ // Restore object context

					GH.VMInstance.ObjectIp = (*GH.VMInstance.pObjList[GH.VMInstance.ObjectId]).Ip;

					GH.VMInstance.ObjectLocal = (*GH.VMInstance.pObjList[GH.VMInstance.ObjectId]).pLocal;

					Result = DSPSTAT.NOBREAK;
				}
			}

			if ((GH.VMInstance.ProgramId == GUI_SLOT) || (GH.VMInstance.ProgramId == DEBUG_SLOT))
			{ // UI

				GH.VMInstance.Priority = UI_PRIORITY;
			}
			else
			{ // user program

				GH.VMInstance.Priority = PRG_PRIORITY;
			}

			return (Result);
		}


		/*! \brief    Save object context
		 *
		 *            ObjectId holds object to switch from
		 *
		 *            Save object context if id is valid and running by saving current IP
		 *
		 *            Uses following from current program context:
		 *            ObjectId, Objects, pObjList, ObjectIp
		 *
		 */
		public void ObjectExit()
		{
			if ((GH.VMInstance.ObjectId > 0) && (GH.VMInstance.ObjectId <= GH.VMInstance.Objects) && (GH.VMInstance.Program[GH.VMInstance.ProgramId].Status != OBJSTAT.STOPPED))
			{ // object valid

				if ((*GH.VMInstance.pObjList[GH.VMInstance.ObjectId]).ObjStatus == RUNNING)
				{ // Save object context

					(*GH.VMInstance.pObjList[GH.VMInstance.ObjectId]).Ip = GH.VMInstance.ObjectIp;
				}
			}
		}


		/*! \brief    Find next object to run
		 *
		 *            Uses following from current program context:
		 *            ObjectId, Objects, pObjList
		 *
		 *  \return   RESULT  Succes [OK or RESULT.STOP]
		 */
		public RESULT ObjectExec()
		{
			RESULT Result = OK;
			OBJID TmpId = 0;


			if ((GH.VMInstance.ProgramId == GUI_SLOT) && (GH.VMInstance.Program[USER_SLOT].Status != OBJSTAT.STOPPED))
			{ // When user program is running - only schedule UI background task

				if ((GH.VMInstance.Objects >= 3) && (GH.VMInstance.Program[GUI_SLOT].Status != OBJSTAT.STOPPED))
				{
					if (GH.VMInstance.ObjectId != 2)
					{
						GH.VMInstance.ObjectId = 2;
					}
					else
					{
						GH.VMInstance.ObjectId = 3;
					}
				}
			}
			else
			{
				do
				{
					// Next object
					if (++GH.VMInstance.ObjectId > GH.VMInstance.Objects)
					{
						// wrap around

						GH.VMInstance.ObjectId = 1;
					}

					if (++TmpId > GH.VMInstance.Objects)
					{
						// no programs running

						Result = RESULT.STOP;
					}

				}
				while ((Result == OK) && ((*GH.VMInstance.pObjList[GH.VMInstance.ObjectId]).ObjStatus != RUNNING));
			}

			return (Result);
		}


		/*! \brief    Put object on run queue
		 *
		 *  \param    Id Object to queue
		 *
		 */
		public void ObjectEnQueue(OBJID Id)
		{
			if ((Id > 0) && (Id <= GH.VMInstance.Objects))
			{
				(*GH.VMInstance.pObjList[Id]).ObjStatus = RUNNING;
				(*GH.VMInstance.pObjList[Id]).Ip = &GH.VMInstance.pImage[(ULONG)GH.VMInstance.pObjHead[Id].OffsetToInstructions];
				(*GH.VMInstance.pObjList[Id]).TriggerCount = GH.VMInstance.pObjHead[Id].TriggerCount;
			}
		}


		/*! \brief    Remove object from run queue
		 *
		 *  \param    Id Object to enqueue
		 *
		 */
		public void ObjectDeQueue(OBJID Id)
		{
			if ((Id > 0) && (Id <= GH.VMInstance.Objects))
			{
				(*GH.VMInstance.pObjList[Id]).Ip = GH.VMInstance.ObjectIp;
				(*GH.VMInstance.pObjList[Id]).ObjStatus = STOPPED;

				SetDispatchStatus(DSPSTAT.STOPBREAK);
			}
		}


		public DATA8 CheckSdcard(DATA8* pChanged, DATA32* pTotal, DATA32* pFree, DATA8 Force)
		{
			DATA8 Result = 0;
			DATAF Tmp;
			DATA8* Name = CommonHelper.Pointer1d<DATA8>(vmNAMESIZE);
			DATA8* Buffer = CommonHelper.Pointer1d<DATA8>(250);

			*pChanged = 0;
			*pTotal = 0;
			*pFree = 0;

			ULONG Time;
			// struct statvfs Status;

			Time = GH.VMInstance.NewTime - GH.VMInstance.SdcardTimer;

			if ((Time >= UPDATE_SDCARD) || (GH.VMInstance.SdcardTimer == 0) || (Force != 0))
			{ // Update values

				GH.VMInstance.SdcardTimer += Time;

				if (GH.Memory.cMemoryGetMediaName("m".AsSbytePointer(), Name) == OK)
				{
					// TODO: probably no need
					//if (GH.VMInstance.SdcardOk == 0)
					//{
					//	GH.VMInstance.SdcardOk = 1;
					//	CommonHelper.snprintf(Buffer, 250, "ln -s /media/card %s &> /dev/null", vmSDCARD_FOLDER);
					//	system(Buffer);
					//	sync();
					//	*pChanged = 1;
					//	GH.printf("system(%s)\r\n", Buffer);
					//}
					//CommonHelper.snprintf(Buffer, 250, "/media/card");
					//statvfs(Buffer, &Status);
					//if (*pChanged != 0)
					//{
					//	GH.printf("statvfs(%s)\r\n", Buffer);

					//	GH.printf("f_bsize   %ld\r\n", Status.f_bsize);
					//	GH.printf("f_frsize  %ld\r\n", Status.f_frsize);
					//	GH.printf("f_blocks  %ld\r\n", Status.f_blocks);
					//	GH.printf("f_bavail  %ld\r\n", Status.f_bavail);
					//	GH.printf("f_files   %ld\r\n", Status.f_files);
					//	GH.printf("f_ffree   %ld\r\n", Status.f_ffree);
					//	GH.printf("f_favail  %ld\r\n", Status.f_favail);
					//	GH.printf("f_fside   %ld\r\n", Status.f_fsid);
					//	GH.printf("f_flag    %ld\r\n", Status.f_flag);
					//	GH.printf("f_namemax %ld\r\n", Status.f_namemax);
					//}
					//Tmp = (DATAF)Status.f_blocks;
					//Tmp = (Tmp + (DATAF)(KB - 1)) / (DATAF)KB;
					//Tmp *= (DATAF)Status.f_bsize;
					//GH.VMInstance.SdcardSize = (DATA32)Tmp;
					//Tmp = (DATAF)Status.f_bavail;
					//Tmp = (Tmp + (DATAF)(KB - 1)) / (DATAF)KB;
					//Tmp *= (DATAF)Status.f_bsize;

					//if (GH.VMInstance.SdcardFree != (DATA32)Tmp)
					//{
					//	GH.printf("%d T=%-8d F=%-8d\r\n", GH.VMInstance.SdcardOk, GH.VMInstance.SdcardSize, GH.VMInstance.SdcardFree);
					//}
					//GH.VMInstance.SdcardFree = (DATA32)Tmp;

					GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");
				}

				else
				{
					if (GH.VMInstance.SdcardOk == 1)
					{
						GH.VMInstance.SdcardOk = 0;
						//CommonHelper.snprintf(Buffer, 250, "rm -r %s &> /dev/null", vmSDCARD_FOLDER);
						//system(Buffer);
						//*pChanged = 1;
						//if (*pChanged)
						//{
						//	GH.printf("system(%s)\r\n", Buffer);
						//}
						GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");
					}
					GH.VMInstance.SdcardSize = 0;
					GH.VMInstance.SdcardFree = 0;
				}
				if (*pChanged != 0)
				{
					GH.printf($"{GH.VMInstance.SdcardOk} T={GH.VMInstance.SdcardSize} F={GH.VMInstance.SdcardFree}\r\n");
				}
			}
			*pTotal = GH.VMInstance.SdcardSize;
			*pFree = GH.VMInstance.SdcardFree;
			Result = GH.VMInstance.SdcardOk;

			return (Result);
		}


		public DATA8 CheckUsbstick(DATA8* pChanged, DATA32* pTotal, DATA32* pFree, DATA8 Force)
		{
			DATA8 Result = 0;

			*pChanged = 0;
			*pTotal = 0;
			*pFree = 0;

			ULONG Time;
			// struct statvfs Status;
			DATAF Tmp;
			DATA8* Name = CommonHelper.Pointer1d<DATA8>(vmNAMESIZE);
			DATA8* Buffer = CommonHelper.Pointer1d<DATA8>(250);

			Time = GH.VMInstance.NewTime - GH.VMInstance.UsbstickTimer;

			if ((Time >= UPDATE_USBSTICK) || (GH.VMInstance.UsbstickTimer == 0) || (Force != 0))
			{ // Update values

				GH.VMInstance.UsbstickTimer += Time;

				if (GH.Memory.cMemoryGetMediaName("s".AsSbytePointer(), Name) == OK)
				{
					//					if (GH.VMInstance.UsbstickOk == 0)
					//					{
					//						GH.VMInstance.UsbstickOk = 1;
					//						CommonHelper.snprintf(Buffer, 250, "ln -s /media/usb %s &> /dev/null", vmUSBSTICK_FOLDER);
					//						system(Buffer);
					//						sync();
					//						*pChanged = 1;
					//# ifdef DEBUG_USBSTICK
					//						GH.printf("system(%s)\r\n", Buffer);
					//#endif
					//					}
					//					CommonHelper.snprintf(Buffer, 250, "/media/usb");
					//					statvfs(Buffer, &Status);
					//# ifdef DEBUG_USBSTICK
					//					if (*pChanged)
					//					{
					//						GH.printf("statvfs(%s)\r\n", Buffer);

					//						GH.printf("f_bsize   %ld\r\n", Status.f_bsize);
					//						GH.printf("f_frsize  %ld\r\n", Status.f_frsize);
					//						GH.printf("f_blocks  %ld\r\n", Status.f_blocks);
					//						GH.printf("f_bavail  %ld\r\n", Status.f_bavail);
					//						GH.printf("f_files   %ld\r\n", Status.f_files);
					//						GH.printf("f_ffree   %ld\r\n", Status.f_ffree);
					//						GH.printf("f_favail  %ld\r\n", Status.f_favail);
					//						GH.printf("f_fside   %ld\r\n", Status.f_fsid);
					//						GH.printf("f_flag    %ld\r\n", Status.f_flag);
					//						GH.printf("f_namemax %ld\r\n", Status.f_namemax);
					//					}
					//#endif
					//					Tmp = (DATAF)Status.f_blocks;
					//					Tmp = (Tmp + (DATAF)(KB - 1)) / (DATAF)KB;
					//					Tmp *= (DATAF)Status.f_bsize;
					//					GH.VMInstance.UsbstickSize = (DATA32)Tmp;
					//					Tmp = (DATAF)Status.f_bavail;
					//					Tmp = (Tmp + (DATAF)(KB - 1)) / (DATAF)KB;
					//					Tmp *= (DATAF)Status.f_bsize;

					//# ifdef DEBUG_USBSTICK
					//					if (GH.VMInstance.UsbstickFree != (DATA32)Tmp)
					//					{
					//						GH.printf("%d T=%-8d F=%-8d\r\n", GH.VMInstance.UsbstickOk, GH.VMInstance.UsbstickSize, GH.VMInstance.UsbstickFree);
					//					}
					//#endif
					//					GH.VMInstance.UsbstickFree = (DATA32)Tmp;
					GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");
				}
				else
				{
					if (GH.VMInstance.UsbstickOk == 1)
					{
						GH.VMInstance.UsbstickOk = 0;
						//CommonHelper.snprintf(Buffer, 250, "rm -r %s &> /dev/null", vmUSBSTICK_FOLDER);
						//system(Buffer);
						//*pChanged = 1;
						//if (*pChanged)
						//{
						//	GH.printf("system(%s)\r\n", Buffer);
						//}
						GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");
					}
					GH.VMInstance.UsbstickSize = 0;
					GH.VMInstance.UsbstickFree = 0;
				}
				//    if (*pChanged)
				{
					GH.printf($"{GH.VMInstance.UsbstickOk} T={GH.VMInstance.UsbstickSize} F={GH.VMInstance.UsbstickFree}\r\n");
				}
			}
			*pTotal = GH.VMInstance.UsbstickSize;
			*pFree = GH.VMInstance.UsbstickFree;
			Result = GH.VMInstance.UsbstickOk;

			return (Result);
		}


		public RESULT mSchedInit(int argc, string[] argv)
		{
			DATA32 Result = OK;
			PRGID PrgId;
			IMGHEAD* pImgHead;
			DATA16 Loop;
			DATA8 Ok;
			DATA32 Total;
			DATA32 Free;
			float Tmp;
			DATA8* PrgNameBuf = CommonHelper.Pointer1d<DATA8>(vmFILENAMESIZE);
			DATA8* ParBuf = CommonHelper.Pointer1d<DATA8>(255);

			GH.VMInstance.Status = 0x00;

			ANALOG* pAdcTmp;

			GH.VMInstance.pAnalog = (ANALOG*)GH.VMInstance.Analog;
			// TODO: analog shite
			//GH.VMInstance.AdcFile = open(ANALOG_DEVICE_NAME, O_RDWR | O_SYNC);

			//if (GH.VMInstance.AdcFile >= MIN_HANDLE)
			//{
			//	pAdcTmp = (ANALOG*)mmap(0, sizeof(ANALOG), PROT_READ | PROT_WRITE, MAP_FILE | MAP_SHARED, GH.VMInstance.AdcFile, 0);

			//	if (pAdcTmp == MAP_FAILED)
			//	{
			//		//#ifndef Linux_X86
			//		LogErrorNumber(ANALOG_SHARED_MEMORY);
			//		//#endif
			//	}
			//	else
			//	{
			//		GH.VMInstance.pAnalog = pAdcTmp;
			//	}
			//	close(GH.VMInstance.AdcFile);
			//}
			//else
			//{
			//	//#ifndef Linux_X86
			//	LogErrorNumber(ANALOG_DEVICE_FILE_NOT_FOUND);
			//	//#endif
			//}

			GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");

			// Fill holes in PrimDispatchTabel
			for (Loop = 0; Loop < PRIMDISPATHTABLE_SIZE; Loop++)
			{
				if (!PrimDispatchTabel.ContainsKey(Loop))
				{
					PrimDispatchTabel.Add(Loop, Error);
				}
			}

			// Be sure necessary folders exist
			if (Directory.CreateDirectory(vmSETTINGS_DIR) != null)
			{
				// chmod(vmSETTINGS_DIR, DIRPERMISSIONS);
			}

			CheckUsbstick(&Ok, &Total, &Free, 0);
			CheckSdcard(&Ok, &Total, &Free, 0);

			// Be sure necessary files exist
			Ok = 0;
			CommonHelper.snprintf(PrgNameBuf, vmFILENAMESIZE, $"{vmSETTINGS_DIR}/{vmWIFI_FILE_NAME}{vmEXT_TEXT}");
			using FileStream wifiFs = File.OpenWrite(CommonHelper.GetString(PrgNameBuf));
			if (wifiFs != null)
			{
				CommonHelper.sprintf(ParBuf, "-\t");
				wifiFs.WriteUnsafe((byte*)ParBuf, 0, CommonHelper.strlen(ParBuf));
				wifiFs.Close();
			}

			// TODO: no need for bluetooth probably
			//Ok = 0;
			//CommonHelper.snprintf(PrgNameBuf, vmFILENAMESIZE, "%s/%s%s", vmSETTINGS_DIR, vmBLUETOOTH_FILE_NAME, vmEXT_TEXT);
			//File = open(PrgNameBuf, O_RDONLY);
			//if (File >= MIN_HANDLE)
			//{
			//	close(File);
			//}
			//else
			//{
			//	File = open(PrgNameBuf, O_CREAT | O_WRONLY | O_TRUNC, SYSPERMISSIONS);
			//	if (File >= MIN_HANDLE)
			//	{
			//		CommonHelper.sprintf(ParBuf, "-\t");
			//		write(File, ParBuf, CommonHelper.strlen(ParBuf));
			//		close(File);
			//	}
			//}

			// TODO: wtf is this ahahaha sleep in file ahahaha wtf
			//Ok = 0;
			//CommonHelper.snprintf(PrgNameBuf, vmFILENAMESIZE, "%s/%s%s", vmSETTINGS_DIR, vmSLEEP_FILE_NAME, vmEXT_TEXT);
			//File = open(PrgNameBuf, O_RDONLY);
			//if (File >= MIN_HANDLE)
			//{
			//	ParBuf[0] = 0;
			//	read(File, ParBuf, sizeof(ParBuf));
			//	if (sscanf(ParBuf, "%f", &Tmp) > 0)
			//	{
			//		if ((Tmp >= (float)0) && (Tmp <= (float)127))
			//		{
			//			SetSleepMinutes((DATA8)Tmp);
			//			Ok = 1;
			//		}
			//	}
			//	else
			//	{
			//		ParBuf[5] = 0;
			//		if (CommonHelper.strcmp(ParBuf, "never") == 0)
			//		{
			//			SetSleepMinutes(0);
			//			Ok = 1;
			//		}
			//	}
			//	close(File);
			//}
			//if (!Ok)
			//{
			//	File = open(PrgNameBuf, O_CREAT | O_WRONLY | O_TRUNC, SYSPERMISSIONS);
			//	if (File >= MIN_HANDLE)
			//	{
			//		SetSleepMinutes((DATA8)DEFAULT_SLEEPMINUTES);
			//		CommonHelper.sprintf(ParBuf, "%dmin\t", DEFAULT_SLEEPMINUTES);
			//		write(File, ParBuf, CommonHelper.strlen(ParBuf));
			//		close(File);
			//	}
			//}

			// TODO: sound file (probably no need)
			//Ok = 0;
			//CommonHelper.snprintf(PrgNameBuf, vmFILENAMESIZE, "%s/%s%s", vmSETTINGS_DIR, vmVOLUME_FILE_NAME, vmEXT_TEXT);
			//File = open(PrgNameBuf, O_RDONLY);
			//if (File >= MIN_HANDLE)
			//{
			//	ParBuf[0] = 0;
			//	read(File, ParBuf, sizeof(ParBuf));
			//	if (sscanf(ParBuf, "%f", &Tmp) > 0)
			//	{
			//		if ((Tmp >= (float)0) && (Tmp <= (float)100))
			//		{
			//			SetVolumePercent((DATA8)Tmp);
			//			Ok = 1;
			//		}
			//	}
			//	close(File);
			//}
			//if (!Ok)
			//{
			//	SetVolumePercent((DATA8)DEFAULT_VOLUME);
			//	File = open(PrgNameBuf, O_CREAT | O_WRONLY | O_TRUNC, SYSPERMISSIONS);
			//	if (File >= MIN_HANDLE)
			//	{
			//		CommonHelper.sprintf(ParBuf, "%d%%\t", DEFAULT_VOLUME);
			//		write(File, ParBuf, CommonHelper.strlen(ParBuf));
			//		close(File);
			//	}
			//}

			GH.VMInstance.RefCount = 0;

			Result |= (int)GH.Output.cOutputInit();
			Result |= (int)GH.Input.cInputInit();
			Result |= (int)GH.Ui.cUiInit();
			Result |= (int)GH.Memory.cMemoryInit();
			Result |= (int)GH.Com.cComInit();
			Result |= (int)GH.Sound.cSoundInit();

			GH.Validate.cValidateInit();

			for (PrgId = 0; PrgId < MAX_PROGRAMS; PrgId++)
			{
				GH.VMInstance.Program[PrgId].Status = OBJSTAT.STOPPED;
				GH.VMInstance.Program[PrgId].StatusChange = 0;
			}

			SetTerminalEnable(TERMINAL_ENABLED);

			GH.VMInstance.Test = 0;

			VmPrint("\r\n\n\n\n\n\nLMS2012 VM STARTED\r\n{\r\n".AsSbytePointer());
			GH.VMInstance.ProgramId = DEBUG_SLOT;
			pImgHead = (IMGHEAD*)UiImage;
			(*pImgHead).ImageSize = 40; // sizeof(UiImage); 

			GH.Ev3System.Logger.LogInfo("VM Started");

			if (argc >= 2)
			{
				CommonHelper.snprintf((DATA8*)GH.VMInstance.FirstProgram, MAX_FILENAME_SIZE, argv[1]);
			}
			else
			{
				CommonHelper.snprintf((DATA8*)GH.VMInstance.FirstProgram, MAX_FILENAME_SIZE, DEFAULT_UI);
			}

			ProgramReset(GH.VMInstance.ProgramId, UiImage, (GP)GH.VMInstance.FirstProgram, 0);

			return (RESULT)(Result);
		}

		public RESULT mSchedCtrl(UBYTE* pRestart)
		{
			RESULT Result = RESULT.FAIL;
			ULONG Time;
			IP TmpIp;
			IMINDEX Index;

			if (GH.VMInstance.DispatchStatus != DSPSTAT.STOPBREAK)
			{
				ProgramInit();
			}

			SetDispatchStatus(ObjectInit());

			if (GH.VMInstance.Program[USER_SLOT].Status != OBJSTAT.STOPPED)
			{
				GH.printf($"\r\n  {GH.VMInstance.ProgramId}  {GH.VMInstance.ObjectId}");
			}

			Time = GH.Timer.cTimerGetuS();
			Time -= GH.VMInstance.PerformTimer;
			GH.VMInstance.PerformTime *= (DATAF)199;
			GH.VMInstance.PerformTime += (DATAF)Time;
			GH.VMInstance.PerformTime /= (DATAF)200;

			/*** Execute BYTECODES *******************************************************/

			GH.VMInstance.InstrCnt = 0;

			(*GH.VMInstance.pAnalog).PreemptMilliSeconds = 0;

			while (GH.VMInstance.Priority != 0)
			{
				if (GH.VMInstance.Debug != 0)
				{
					Monitor();
				}
				else
				{
					GH.VMInstance.Priority--;
					if (GH.VMInstance.ProgramId != GUI_SLOT)
					{
						Index = (IMINDEX)GH.VMInstance.ObjectIp - (IMINDEX)GH.VMInstance.pImage;
						GH.Validate.cValidateDisassemble(GH.VMInstance.pImage, &Index, (LABEL*)GH.VMInstance.Program[GH.VMInstance.ProgramId].Label);
					}
					PrimDispatchTabel[*(GH.VMInstance.ObjectIp++)]();
					GH.VMInstance.InstrCnt++;
					if (GH.VMInstance.Program[USER_SLOT].Status != OBJSTAT.STOPPED)
					{
						GH.printf(".");
					}
				}
			}

			/*****************************************************************************/

			GH.VMInstance.PerformTimer = GH.Timer.cTimerGetuS();

			GH.VMInstance.NewTime = GetTimeMS();

			Time = GH.VMInstance.NewTime - GH.VMInstance.OldTime1;

			if (Time >= UPDATE_TIME1)
			{
				GH.VMInstance.OldTime1 += Time;

				if (Time >= 3)
				{
					GH.printf($"{Time} {GH.VMInstance.InstrCnt}\r\n");
				}
				GH.Com.cComUpdate();
				GH.Sound.cSoundUpdate();
			}


			Time = GH.VMInstance.NewTime - GH.VMInstance.OldTime2;

			if (Time >= UPDATE_TIME2)
			{
				GH.VMInstance.OldTime2 += Time;

				Thread.Sleep(1); // TODO: do i really need it?
				GH.Input.cInputUpdate((UWORD)Time);
				GH.Ui.cUiUpdate((UWORD)Time);

				if (GH.VMInstance.Test != 0)
				{
					if (GH.VMInstance.Test > (UWORD)Time)
					{
						GH.VMInstance.Test -= (UWORD)Time;
					}
					else
					{
						TstClose();
					}
				}
			}

			if (GH.VMInstance.DispatchStatus == DSPSTAT.FAILBREAK)
			{
				if (GH.VMInstance.ProgramId != GUI_SLOT)
				{
					if (GH.VMInstance.ProgramId != CMD_SLOT)
					{
						GH.UiInstance.Warning |= WARNING_DSPSTAT;
					}
					CommonHelper.snprintf(GH.VMInstance.PrintBuffer, PRINTBUFFERSIZE, $"}}\r\nPROGRAM \"{GH.VMInstance.ProgramId}\" RESULT.FAIL BREAK just before {(ulong)(GH.VMInstance.ObjectIp - GH.VMInstance.Program[GH.VMInstance.ProgramId].pImage)}!\r\n");
					VmPrint(GH.VMInstance.PrintBuffer);
					ProgramEnd(GH.VMInstance.ProgramId);
					GH.VMInstance.Program[GH.VMInstance.ProgramId].Result = RESULT.FAIL;
				}
				else
				{
					CommonHelper.snprintf(GH.VMInstance.PrintBuffer, PRINTBUFFERSIZE, $"UI RESULT.FAIL BREAK just before {(ulong)(GH.VMInstance.ObjectIp - GH.VMInstance.Program[GH.VMInstance.ProgramId].pImage)}!\r\n");
					VmPrint(GH.VMInstance.PrintBuffer);
					LogErrorNumber((ERR)VM_INTERNAL);
					*pRestart = 1;
				}
			}
			else
			{

				if (GH.VMInstance.DispatchStatus == DSPSTAT.INSTRBREAK)
				{
					if (GH.VMInstance.ProgramId != CMD_SLOT)
					{
						LogErrorNumber((ERR)VM_PROGRAM_INSTRUCTION_BREAK);
					}
					TmpIp = GH.VMInstance.ObjectIp - 1;
					CommonHelper.snprintf(GH.VMInstance.PrintBuffer, PRINTBUFFERSIZE, $"\r\n{(UWORD)(((ULONG)TmpIp) - (ULONG)GH.VMInstance.pImage)} [{GH.VMInstance.ObjectId}] ");
					VmPrint(GH.VMInstance.PrintBuffer);
					CommonHelper.snprintf(GH.VMInstance.PrintBuffer, PRINTBUFFERSIZE, $"VM       ERROR    [0x{*TmpIp:B}]\r\n");
					VmPrint(GH.VMInstance.PrintBuffer);
					GH.VMInstance.Program[GH.VMInstance.ProgramId].Result = RESULT.FAIL;
				}

				ObjectExit();

				Result = ObjectExec();

				if (Result == RESULT.STOP)
				{
					ProgramExit();
					ProgramEnd(GH.VMInstance.ProgramId);
					GH.VMInstance.DispatchStatus = DSPSTAT.NOBREAK;
				}
				else
				{
					if (GH.VMInstance.DispatchStatus != DSPSTAT.STOPBREAK)
					{
						ProgramExit();
					}
				}
			}

			if (GH.VMInstance.DispatchStatus != DSPSTAT.STOPBREAK)
			{
				Result = ProgramExec();
			}

			if (*pRestart == 1)
			{
				Result = RESULT.FAIL;
			}

			Thread.Sleep(1); // TODO: do i really need it?

			return (Result);
		}


		public RESULT mSchedExit()
		{
			DATA32 Result = OK;

			VmPrint("}\r\nVM OBJSTAT.STOPPED\r\n\n".AsSbytePointer());

			// TODO: usb and sd card shite
			//# ifndef DISABLE_SDCARD_SUPPORT
			//			char SDBuffer[250];
			//			if (GH.VMInstance.SdcardOk == 1)
			//			{
			//				sync();
			//				GH.VMInstance.SdcardOk = 0;
			//				CommonHelper.snprintf(SDBuffer, 250, "rm -r %s &> /dev/null", vmSDCARD_FOLDER);
			//				system(SDBuffer);
			//			}
			//#endif

			//# ifndef DISABLE_USBSTICK_SUPPORT
			//			char USBBuffer[250];
			//			if (GH.VMInstance.UsbstickOk == 1)
			//			{
			//				sync();
			//				GH.VMInstance.UsbstickOk = 0;
			//				CommonHelper.snprintf(USBBuffer, 250, "rm -r %s &> /dev/null", vmUSBSTICK_FOLDER);
			//				system(USBBuffer);
			//			}
			//#endif

			Result |= (int)GH.Validate.cValidateExit();
			Result |= (int)GH.Sound.cSoundExit();
			Result |= (int)GH.Com.cComExit();
			Result |= (int)GH.Memory.cMemoryExit();
			Result |= (int)GH.Ui.cUiExit();
			Result |= (int)GH.Input.cInputExit();
			Result |= (int)GH.Output.cOutputExit();

			return (RESULT)(Result);
		}

		public int Main()
		{
			return main(0, new string[0]);
		}


		public int main(int argc, string[] argv)
		{
			RESULT Result = RESULT.FAIL;
			UBYTE Restart;

			do
			{
				Restart = 0;
				Result = mSchedInit(argc, argv);

				if (Result == OK)
				{
					do
					{
						Result = mSchedCtrl(&Restart);
						/*
								if ((*GH.UiInstance.pUi).State[BACK_BUTTON] & BUTTON_LONGPRESS)
								{
								  Restart  =  1;
								  Result   =  RESULT.FAIL;
								}
						*/

						// TODO: comment
						// DEBUG SLOWER
						Thread.Sleep(100);
					}
					while (Result == OK);

					Result = mSchedExit();
				}
				else
				{
					//TCP      system("reboot");
				}
			}
			while (Restart != 0);

			return ((int)Result);
		}

		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opERROR </b>
		 *
		 *- This code does not exist in normal program\n
		 *- Dispatch status changes to INSTRBREAK
		 *
		 */
		/*! \brief    opOUTPUT_READY byte code
		 *
		 *            Uses following from current program context:
		 *            DispatchStatus
		 */
		public void Error()
		{
			// TODO: probably uncomment
			// ProgramEnd(GH.VMInstance.ProgramId);
			// GH.VMInstance.Program[GH.VMInstance.ProgramId].Result = RESULT.FAIL;
			// SetDispatchStatus((DSPSTAT)INSTRBREAK);
			GH.Ev3System.Logger.LogError($"An error occured in prgId: {GH.VMInstance.ProgramId}");
		}


		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opNOP </b>
		 *
		 *- This code does absolutely nothing\n
		 *
		 */
		/*! \brief  opNOP byte code
		 *
		 */
		public void Nop()
		{
		}


		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opPROGRAM_STOP (PRGID)</b>
		 *
		 *- Stops specific program id slot\n
		 *- Dispatch status changes to PRGBREAK
		 *
		 *  \param  (DATA16)  PRGID  - Program id (GUI_SLOT = all, CURRENT_SLOT = current) (see \ref prgid)
		 */
		/*! \brief    opPROGRAM_STOP byte code
		 *
		 *            Stops specific program id slot
		 *
		 *            Uses following from current program context:
		 *            Program[], DispatchStatus
		 *
		 */
		public void ProgramStop()
		{
			DATA16 PrgId;

			PrgId = *(DATA16*)PrimParPointer();

			if (PrgId == GUI_SLOT)
			{
				PrgId = MAX_PROGRAMS;
				do
				{
					PrgId--;
					ProgramEnd((ushort)PrgId);
				}
				while (PrgId != 0);
			}
			else
			{
				unchecked
				{
					if (PrgId == (short)CURRENT_SLOT)
					{
						PrgId = (short)CurrentProgramId();
					}
					ProgramEnd((ushort)PrgId);
				}
			}
		}


		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opPROGRAM_START (PRGID, SIZE, *IP, DEBUG)</b>
		 *
		 *- Start program id slot\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)    PRGID  - Program id  (see \ref prgid)
		 *  \param  (DATA32)    SIZE   - Size of image\n
		 *  \param  (DATA32)    *IP    - Address of image (value from opFILE(LOAD_IMAGE,..)  )
		 *  \param  (DATA8)     DEBUG  - Debug mode (0=normal, 1=debug, 2=don't execute)
		 */
		/*! \brief    opPROGRAM_START byte code
		 *
		 *            Start program id slot
		 *
		 *            Uses following from current program context:
		 *            Program[], DispatchStatus
		 *
		 */
		public void ProgramStart()
		{
			PRGID PrgId;
			PRGID TmpPrgId;
			IP pI;
			UBYTE DB;
			UBYTE Flag = 0;


			PrgId = *(PRGID*)PrimParPointer();

			// Dummy
			pI = *(IP*)PrimParPointer();

			pI = *(IP*)PrimParPointer();
			DB = *(UBYTE*)PrimParPointer();


			if (GH.VMInstance.Program[PrgId].Status == OBJSTAT.STOPPED)
			{
				TmpPrgId = CurrentProgramId();

				if ((TmpPrgId == CMD_SLOT) || (TmpPrgId == TERM_SLOT))
				{ // Direct command starting a program

					if ((GH.VMInstance.Program[USER_SLOT].Status == OBJSTAT.STOPPED) && (GH.VMInstance.Program[DEBUG_SLOT].Status == OBJSTAT.STOPPED))
					{ // User and debug must be stooped

						if (ProgramReset(PrgId, pI, null, DB) == OK)
						{
							Flag = 1;
						}
					}
				}
				else
				{ // Gui, user or debug starting a program

					if (ProgramReset(PrgId, pI, null, DB) == OK)
					{
						Flag = 1;
					}
				}
			}
			if (Flag == 0)
			{
				//    LogErrorNumber(VM_PROGRAM_NOT_STARTED);
			}
		}


		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opOBJECT_STOP (OBJID)</b>
		 *
		 *- Stops specific object\n
		 *- Dispatch status changes to DSPSTAT.STOPBREAK
		 *
		 *  \param  (DATA16)  OBJID  - Object id
		 */
		/*! \brief    opOBJECT_STOP byte code
		 *
		 *            Stops specific object
		 *
		 *            Uses following from current program context:
		 *            pObjectList, ObjectIp, DispatchStatus
		 *
		 */
		public void ObjectStop()
		{
			ObjectDeQueue(*(OBJID*)PrimParPointer());
		}


		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opOBJECT_START (OBJID)</b>
		 *
		 *- Start specific object\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  OBJID  - Object id
		 */
		/*! \brief    opOBJECT_START byte code
		 *
		 *            Start specific object
		 *
		 *            Uses following from current program context:
		 *            pObjectList
		 *
		 */
		public void ObjectStart()
		{
			ObjectEnQueue(*(OBJID*)PrimParPointer());
		}


		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opOBJECT_TRIG (OBJID)</b>
		 *
		 *- Triggers object and run the object if fully triggered\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  OBJID  - Object id
		 */
		/*! \brief    opOBJECT_TRIG byte code
		 *
		 *            Triggers object and run the object if fully triggered
		 *
		 *            Uses following from current program context:
		 *            pObjList
		 *
		 */
		public void ObjectTrig()
		{
			OBJID TmpId;

			TmpId = *(OBJID*)PrimParPointer();

			(*GH.VMInstance.pObjList[TmpId]).ObjStatus = WAITING;
			if ((*GH.VMInstance.pObjList[TmpId]).TriggerCount != 0)
			{
				((*GH.VMInstance.pObjList[TmpId]).TriggerCount)--;
				if ((*GH.VMInstance.pObjList[TmpId]).TriggerCount == 0)
				{
					ObjectReset(TmpId);
					ObjectEnQueue(TmpId);
					/*
					  #ifdef OLDCALL
						  ObjectEnQueue(TmpId);
					  #else
						  (*GH.VMInstance.pObjList[GH.VMInstance.ObjectId]).Ip      =  GH.VMInstance.ObjectIp;
						  GH.VMInstance.ObjectId                                 =  TmpId;
						  (*GH.VMInstance.pObjList[GH.VMInstance.ObjectId]).ObjStatus  =  OBJSTAT.RUNNING;
						  GH.VMInstance.ObjectIp        =  (*GH.VMInstance.pObjList[GH.VMInstance.ObjectId]).Ip;
						  GH.VMInstance.ObjectLocal     =  (*GH.VMInstance.pObjList[GH.VMInstance.ObjectId]).pLocal;
					  #endif
					*/
				}
			}
		}


		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opOBJECT_WAIT (OBJID)</b>
		 *
		 *- Wait until object has run\n
		 *- Dispatch status can change to DSPSTAT.BUSYBREAK
		 *
		 *  \param  (DATA16)  OBJID  - Object id
		 */
		/*! \brief    opOBJECT_WAIT byte code
		 *
		 *            Wait until object has run
		 *
		 *            Uses following from current program context:
		 *            ObjectIp, pObjList, DispatchStatus
		 *
		 */
		public void ObjectWait()
		{
			OBJID TmpId;
			IP TmpIp;

			TmpIp = GH.VMInstance.ObjectIp;
			TmpId = *(OBJID*)PrimParPointer();

			if ((*GH.VMInstance.pObjList[TmpId]).ObjStatus != STOPPED)
			{
				GH.VMInstance.ObjectIp = TmpIp - 1;
				SetDispatchStatus(DSPSTAT.BUSYBREAK);
			}
		}


		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opRETURN </b>
		 *
		 *- Return from byte code subroutine\n
		 *- Dispatch status changes to DSPSTAT.STOPBREAK
		 *
		 */
		/*! \brief    opRETURN byte code
		 *
		 *            Return from byte code subroutine
		 *
		 *            Uses following from current program context:
		 *            ObjectId, pObjList, DispatchStatus
		 *
		 */
		public void ObjectReturn()
		{
			OBJID ObjectIdCaller;

			// Get caller id from saved
			ObjectIdCaller = (*GH.VMInstance.pObjList[GH.VMInstance.ObjectId]).CallerId;

			// Copy local variables to parameters
			GH.VMInstance.ObjectLocal = (*GH.VMInstance.pObjList[ObjectIdCaller]).pLocal;
			CopyLocalsToPars(ObjectIdCaller);

			// Stop called object and start calling object
			ObjectDeQueue(GH.VMInstance.ObjectId);
			ObjectEnQueue(ObjectIdCaller);
		}


		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opCALL (OBJID, PARAMETERS, ....)</b>
		 *
		 *- Calls byte code subroutine\n
		 *- Dispatch status changes to DSPSTAT.STOPBREAK or DSPSTAT.BUSYBREAK
		 *
		 *  \param  (DATA16)  OBJID       - Object id
		 *  \param  (DATA8)   PARAMETERS  - Number of parameters\n
		 *
		 *- \ref parameterencoding
		 *
		 */
		/*! \brief    opCALL byte code
		 *
		 *            Calls byte code subroutine
		 *
		 *            Uses following from current program context:
		 *            ObjectIp, pObjList, pObjHead, DispatchStatus
		 *
		 */
		public void ObjectCall()
		{
			IP TmpIp;
			OBJID ObjectIdToCall;

			// Save IP in case object are locked
			TmpIp = GetObjectIp();

			// Get object to call from byte stream
			ObjectIdToCall = *(OBJID*)PrimParPointer();
			if ((*GH.VMInstance.pObjList[ObjectIdToCall]).ObjStatus == STOPPED)
			{ // Object free

				// Get number of parameters
				PrimParPointer();

				// Initialise  object
				ObjectReset(ObjectIdToCall);

				// Save mother id
				(*GH.VMInstance.pObjList[ObjectIdToCall]).CallerId = GH.VMInstance.ObjectId;

				// Copy parameters to local variables
				CopyParsToLocals(ObjectIdToCall);

				// Halt calling object
				(*GH.VMInstance.pObjList[GH.VMInstance.ObjectId]).Ip = GH.VMInstance.ObjectIp;
				(*GH.VMInstance.pObjList[GH.VMInstance.ObjectId]).ObjStatus = HALTED;

				// Start called object
				SetDispatchStatus(DSPSTAT.STOPBREAK);
				ObjectEnQueue(ObjectIdToCall);
			}
			else
			{ // Object locked - rewind IP

				GH.printf($"SUBCALL {ObjectIdToCall} RESULT.BUSY status = {(*GH.VMInstance.pObjList[ObjectIdToCall]).ObjStatus}\r\n");
				SetObjectIp(TmpIp - 1);
				SetDispatchStatus(DSPSTAT.BUSYBREAK);
			}
		}


		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opOBJECT_END </b>
		 *
		 *- Stops current object\n
		 *- Dispatch status changes to DSPSTAT.STOPBREAK
		 *
		 */
		/*! \brief    opOBJECT_END byte code
		 *
		 *            Stops current object
		 *
		 *            Uses following from current program context:
		 *            pObjectList, ObjectIp, DispatchStatus
		 *
		 */
		public void ObjectEnd()
		{
			(*GH.VMInstance.pObjList[GH.VMInstance.ObjectId]).Ip = &GH.VMInstance.Program[GH.VMInstance.ProgramId].pImage[(ULONG)GH.VMInstance.Program[GH.VMInstance.ProgramId].pObjHead[GH.VMInstance.ObjectId].OffsetToInstructions];
			(*GH.VMInstance.pObjList[GH.VMInstance.ObjectId]).ObjStatus = STOPPED;
			SetDispatchStatus(DSPSTAT.STOPBREAK);
		}


		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opSLEEP </b>
		 *
		 *- Breaks execution of current VMTHREAD\n
		 *- Dispatch status changes to INSTRBREAK
		 *
		 */
		/*! \brief    opSLEEP byte code
		 *
		 *            Breaks execution of current VMTHREAD
		 *
		 *            Uses following from current program context:
		 *            DispatchStatus
		 *
		 *
		 */
		public void Sleep()
		{
			SetDispatchStatus(DSPSTAT.SLEEPBREAK);
		}


		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opPROGRAM_INFO (CMD, PRGID, DATA)</b>
		 *
		 *- Get program data\n
		 *- Dispatch status can change to FAILBREAK
		 *
		 *  \param  (DATA8)   CMD     - \ref programinfosubcode
		 *
		 *\n
		 *  - CMD = OBJ_STOP
		 *    - \param  (DATA16)    PRGID  - Program slot number  (see \ref prgid)
		 *    - \param  (DATA16)    OBJID  - Object id\n
		 *
		 *\n
		 *  - CMD = OBJ_START
		 *    - \param  (DATA16)    PRGID  - Program slot number  (see \ref prgid)
		 *    - \param  (DATA16)    OBJID  - Object id\n
		 *
		 *\n
		 *  - CMD = GET_STATUS
		 *    - \param  (DATA16)    PRGID  - Program slot number  (see \ref prgid)
		 *    - \return (DATA8)     DATA   - Program status\n
		 *
		 *\n
		 *  - CMD = GET_PRGRESULT
		 *    - \param  (DATA16)    PRGID  - Program slot number  (see \ref prgid)
		 *    - \return (DATA8)     DATA   - Program result [OK, RESULT.BUSY, RESULT.FAIL]\n
		 *
		 *\n
		 *  - CMD = GET_SPEED
		 *    - \param  (DATA16)    PRGID  - Program slot number  (see \ref prgid)
		 *    - \return (DATA32)    DATA   - Program speed [instr/S]\n
		 *
		 *\n
		 */
		/*! \brief    opPROGRAM_INFO byte code
		 *
		 *            Get program informations
		 *
		 *            Uses following from current program context:
		 *            None
		 *
		 */
		public void ProgramInfo()
		{
			DATA8 Cmd;
			DATA16 Instr;
			PRGID PrgId;
			OBJID ObjIndex;

			Cmd = *(DATA8*)PrimParPointer();
			PrgId = *(PRGID*)PrimParPointer();

			switch (Cmd)
			{
				case OBJ_STOP:
					{
						ObjIndex = *(OBJID*)PrimParPointer();
						if ((ObjIndex > 0) && (ObjIndex <= GH.VMInstance.Program[PrgId].Objects) && (GH.VMInstance.Program[PrgId].Status != OBJSTAT.STOPPED))
						{
							(*GH.VMInstance.Program[PrgId].pObjList[ObjIndex]).ObjStatus = STOPPED;
						}
					}
					break;

				case OBJ_START:
					{
						ObjIndex = *(OBJID*)PrimParPointer();
						if ((ObjIndex > 0) && (ObjIndex <= GH.VMInstance.Program[PrgId].Objects) && (GH.VMInstance.Program[PrgId].Status != OBJSTAT.STOPPED))
						{
							if (ObjIndex == 1)
							{
								GH.VMInstance.Program[PrgId].StartTime = GetTimeMS();
								GH.VMInstance.Program[PrgId].RunTime = GH.Timer.cTimerGetuS();
							}
						  (*GH.VMInstance.Program[PrgId].pObjList[ObjIndex]).ObjStatus = RUNNING;
							(*GH.VMInstance.Program[PrgId].pObjList[ObjIndex]).Ip = &GH.VMInstance.Program[PrgId].pImage[(ULONG)GH.VMInstance.Program[PrgId].pObjHead[ObjIndex].OffsetToInstructions];
							(*GH.VMInstance.Program[PrgId].pObjList[ObjIndex]).TriggerCount = GH.VMInstance.Program[PrgId].pObjHead[ObjIndex].TriggerCount;
						}
					}
					break;

				case GET_STATUS:
					{
						*(DATA8*)PrimParPointer() = (DATA8)GH.VMInstance.Program[PrgId].Status;
					}
					break;

				case GET_PRGRESULT:
					{
						*(DATA8*)PrimParPointer() = (DATA8)GH.VMInstance.Program[PrgId].Result;
					}
					break;

				case GET_SPEED:
					{
						*(DATA32*)PrimParPointer() = (DATA32)(((float)GH.VMInstance.Program[PrgId].InstrCnt * (float)1000000) / (float)GH.VMInstance.Program[PrgId].InstrTime);
					}
					break;

				case SET_INSTR:
					{
						Instr = *(DATA16*)PrimParPointer();
						SetInstructions((ULONG)Instr);
					}
					break;

				default:
					{
						SetDispatchStatus(DSPSTAT.FAILBREAK);
					}
					break;

			}
		}


		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opLABEL (NO)</b>
		 *
		 *- This code does nothing besides marking an address to a label\n
		 *
		 *  \param    (DATA8)   NO - Label number
		 */
		/*! \brief  opLABEL byte code
		 *
		 */
		public void DefLabel()
		{
			PrimParPointer();
		}


		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opPROBE (PRGID, OBJID, OFFSET, SIZE)</b>
		 *
		 *- Display globals or object locals on terminal\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  PRGID   - Program slot number (see \ref prgid)
		 *  \param  (DATA16)  OBJID   - Object id (zero means globals)
		 *  \param  (DATA32)  OFFSET  - Offset (start from)
		 *  \param  (DATA32)  SIZE    - Size (length of dump) zero means all (max 1024)
		 */
		/*! \brief    opPROBE byte code
		 *
		 *            Display globals or object locals on terminal
		 *
		 *            Uses following from current program context:
		 *            None
		 *
		 */
		public void Probe()
		{
			PRGID PrgId;
			OBJID ObjId;
			GBINDEX RamOffset;
			VARDATA* Ram;
			GBINDEX Size;
			GBINDEX Tmp;
			GBINDEX Lng;

			PrgId = *(PRGID*)PrimParPointer();
			ObjId = *(OBJID*)PrimParPointer();
			RamOffset = *(GBINDEX*)PrimParPointer();
			Size = *(GBINDEX*)PrimParPointer();

			if (GH.VMInstance.Program[PrgId].Status != OBJSTAT.STOPPED)
			{
				if (ObjId == 0)
				{
					Ram = GH.VMInstance.Program[PrgId].pGlobal;
					Lng = (*(IMGHEAD*)GH.VMInstance.Program[PrgId].pImage).GlobalBytes;
				}
				else
				{
					Ram = (*GH.VMInstance.Program[PrgId].pObjList[ObjId]).pLocal;
					Lng = GH.VMInstance.Program[PrgId].pObjHead[ObjId].LocalBytes;
				}
				Ram += RamOffset;
				if (Size == 0)
				{
					Size = Lng;
				}
				if (Size != 0)
				{
					CommonHelper.snprintf(GH.VMInstance.PrintBuffer, PRINTBUFFERSIZE, $"    PROBE  Prg={PrgId} Obj={ObjId} Offs={(ulong)RamOffset} Lng={(ulong)Size}\r\n    {{\r\n  ");
					VmPrint(GH.VMInstance.PrintBuffer);

					for (Tmp = 0; (Tmp < Size) && (Tmp < Lng) && (Tmp < 1024); Tmp++)
					{
						if ((Tmp & 0x0F) == 0)
						{
							CommonHelper.snprintf(GH.VMInstance.PrintBuffer, PRINTBUFFERSIZE, $"    {(ulong)(RamOffset + (GBINDEX)Tmp):B} ");
							VmPrint(GH.VMInstance.PrintBuffer);
						}
						CommonHelper.snprintf(GH.VMInstance.PrintBuffer, PRINTBUFFERSIZE, $"{(UBYTE)(*Ram & 0xFF):B} ");
						VmPrint(GH.VMInstance.PrintBuffer);
						if (((Tmp & 0x0F) == 0xF) && (Tmp < (Size - 1)))
						{
							VmPrint("\r\n    ".AsSbytePointer());
						}
						Ram++;
					}

					VmPrint("\r\n    }\r\n".AsSbytePointer());
				}
			}
		}


		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opDO (PRGID, IMAGE)</b>
		 *
		 *- Run byte code snippet\n
		 *- Dispatch status can change to DSPSTAT.BUSYBREAK
		 *
		 *  \param    (DATA16)   PRGID    - Program slot number
		 *  \param    (DATA32)   *IMAGE   - Address of image
		 *  \param    (DATA32)   *GLOBAL  - Address of global variables
		 */
		/*! \brief    opDO byte code
		 *
		 *            Run byte code snippet
		 *
		 *            Uses following from current program context:
		 *            ObjectIp, ProgramId
		 *
		 */
		public void Do()
		{
			DATA16 PrgId;
			DATA32 pImage;
			DATA32 pGlobal;

			PrgId = *(DATA16*)PrimParPointer();
			pImage = *(DATA32*)PrimParPointer();
			pGlobal = *(DATA32*)PrimParPointer();

			if (ProgramStatus((ushort)PrgId) != OBJSTAT.STOPPED)
			{
				ProgramEnd((ushort)PrgId);
			}
			if (ProgramStatus((ushort)PrgId) == OBJSTAT.STOPPED)
			{
				if (ProgramReset((ushort)PrgId, (IP)pImage, (GP)pGlobal, 0) != OK)
				{
					if (PrgId != CMD_SLOT)
					{
						LogErrorNumber(ERR.VM_PROGRAM_NOT_STARTED);
					}
				}
			}
			GH.Ui.cUiAlive();

		}


		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opBP0 - opBP3 </b>
		 *
		 *- Display globals or object locals on terminal\n
		 *- Removes it self when done
		 *- Dispatch status unchanged
		 *
		 */
		/*! \brief    opBP0 - opBP3 byte code
		 *
		 *            Display globals or object locals on terminal
		 *
		 *            Uses following from current program context:
		 *            pObjHead, ObjectIp, ProgramId
		 *
		 */
		public void BreakPoint()
		{
			IP TmpIp;
			DATA8 No;
			float Instr;

			TmpIp = (--GH.VMInstance.ObjectIp);
			No = *(DATA8*)TmpIp;

			if (((BRKP*)GH.VMInstance.Program[GH.VMInstance.ProgramId].Brkp)[No & 0x03].OpCode != 0)
			{
				*(DATA8*)TmpIp = (sbyte)((BRKP*)GH.VMInstance.Program[GH.VMInstance.ProgramId].Brkp)[No & 0x03].OpCode;
			}
			else
			{
				GH.VMInstance.ObjectIp++;
			}

			if ((No & 0x03) == 3)
			{
				GH.Ui.cUiTestpin(1);
				GH.Ui.cUiTestpin(0);
			}
			else
			{
				Instr = GH.VMInstance.Program[GH.VMInstance.ProgramId].InstrCnt + GH.VMInstance.InstrCnt;

				CommonHelper.snprintf(GH.VMInstance.PrintBuffer, PRINTBUFFERSIZE, $"\r\nBREAKPOINT #{No & 0x03} ({Instr})");
				VmPrint(GH.VMInstance.PrintBuffer);

				GH.VMInstance.Debug = 1;
			}
			PrimDispatchTabel[*(GH.VMInstance.ObjectIp++)]();
			*(DATA8*)TmpIp = No;
		}


		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opBP_SET (PRGID, NO, ADDRESS)</b>
		 *
		 *- Set break point in byte code program\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  PRGID   - Program slot number (see \ref prgid)
		 *  \param  (DATA8)   NO      - Breakpoint number [0..2] (3 = trigger out on TP4)
		 *  \param  (DATA32)  ADDRESS - Address (Offset from start of image) (0 = remove breakpoint)
		 */
		/*! \brief    opBP_SET byte code
		 *
		 *            Set break point in byte code program
		 *
		 *            Uses following from current program context:
		 *            None
		 *
		 */
		public void BreakSet()
		{
			PRGID PrgId;
			DATA8 No;
			DATA32 Addr;

			PrgId = *(PRGID*)PrimParPointer();
			No = *(DATA8*)PrimParPointer();
			Addr = *(DATA32*)PrimParPointer();

			if (No < MAX_BREAKPOINTS)
			{
				if (GH.VMInstance.Program[PrgId].Status != OBJSTAT.STOPPED)
				{
					if (Addr != 0)
					{
						((BRKP*)GH.VMInstance.Program[PrgId].Brkp)[No].Addr = (uint)Addr;
						((BRKP*)GH.VMInstance.Program[PrgId].Brkp)[No].OpCode = (OP)GH.VMInstance.Program[PrgId].pImage[Addr];
						GH.VMInstance.Program[PrgId].pImage[Addr] = (byte)(opBP0 + No);
					}
					else
					{
						if ((((BRKP*)GH.VMInstance.Program[PrgId].Brkp)[No].Addr != 0) && (((BRKP*)GH.VMInstance.Program[PrgId].Brkp)[No].OpCode != 0))
						{
							Addr = (int)((BRKP*)GH.VMInstance.Program[PrgId].Brkp)[No].Addr;
							GH.VMInstance.Program[PrgId].pImage[Addr] = (byte)((BRKP*)GH.VMInstance.Program[PrgId].Brkp)[No].OpCode;
						}
						((BRKP*)GH.VMInstance.Program[PrgId].Brkp)[No].Addr = 0;
						((BRKP*)GH.VMInstance.Program[PrgId].Brkp)[No].OpCode = 0;
					}
				}
			}
		}


		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opRANDOM (MIN, MAX, VALUE)</b>
		 *
		 *- Get random value\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  MIN     - Minimum value
		 *  \param  (DATA16)  MAX     - Maximum value
		 *  \return (DATA16)  VALUE   - Value
		 */
		/*! \brief    opRANDOM byte code
		 *
		 *            Get random VALUE between MIN and MAX
		 *
		 *            Uses following from current program context:
		 *            None
		 *
		 */
		public void Random()
		{
			DATA16 Min;
			DATA16 Max;
			DATA16 Result;

			Min = *(DATA16*)PrimParPointer();
			Max = *(DATA16*)PrimParPointer();

			Result = (DATA16)(new System.Random()).Next(DATA16.MinValue, DATA16.MaxValue);

			if (Result < 0)
			{
				Result = (short)(0 - Result);
			}

			Result = ((short)(((Result * (Max - Min)) + 16383) / 32767));
			Result += Min;

			*(DATA16*)PrimParPointer() = Result;
		}


		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opINFO (CMD, ....)  </b>
		 *
		 *- Info functions entry\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   CMD           - \ref infosubcode
		 *
		 *\n
		 *  - CMD = SET_ERROR
		 *    -  \param  (DATA8)   NUMBER   - Error number\n
		 *
		 *\n
		 *  - CMD = GET_ERROR
		 *    -  \return (DATA8)   NUMBER   - Error number\n
		 *
		 *\n
		 *  - CMD = ERRORTEXT
		 *\n  Convert error number to text string\n
		 *    -  \param  (DATA8)   NUMBER       - Error number\n
		 *    -  \param  (DATA8)   LENGTH       - Maximal length of string returned (-1 = no check)\n
		 *    -  \return (DATA8)   DESTINATION  - String variable or handle to string\n
		 *
		 *\n
		 *  - CMD = GET_VOLUME
		 *    -  \return (DATA8)   VALUE    - Volume [0..100%]\n
		 *
		 *\n
		 *  - CMD = SET_VOLUME
		 *    -  \param  (DATA8)   VALUE    - Volume [0..100%]\n
		 *
		 *\n
		 *  - CMD = GET_MINUTES
		 *    -  \return (DATA8)   VALUE    - Minutes to sleep [0..120min] (0 = ~)\n
		 *
		 *\n
		 *  - CMD = SET_MINUTES
		 *    -  \param  (DATA8)   VALUE    - Minutes to sleep [0..120min] (0 = ~)\n
		 *
		 *\n
		 *
		 */
		/*! \brief  opINFO byte code
		 *
		 */
		public void Info()
		{
			DATA8 Cmd;
			DATA8 Tmp;
			DATA8 Length;
			DATA8* pDestination;
			DATA8 Number;

			Cmd = *(DATA8*)PrimParPointer();
			switch (Cmd)
			{ // Function

				case SET_ERROR:
					{
						LogErrorNumber((ERR)(*(DATA8*)PrimParPointer()));
					}
					break;

				case GET_ERROR:
					{
						*(DATA8*)PrimParPointer() = (sbyte)LogErrorGet();
					}
					break;

				case ERRORTEXT:
					{
						Number = *(DATA8*)PrimParPointer();
						Length = *(DATA8*)PrimParPointer();
						pDestination = (DATA8*)PrimParPointer();

						if (GH.VMInstance.Handle >= 0)
						{
							if (Number < ERRORS)
							{
								Tmp = (sbyte)(Number + 1);
							}
							else
							{
								Tmp = 10;
							}
							if ((Length > Tmp) || (Length == -1))
							{
								Length = Tmp;
							}
							pDestination = (DATA8*)VmMemoryResize(GH.VMInstance.Handle, (DATA32)Length);
						}
						if (pDestination != null)
						{
							*pDestination = 0;

							if (Number != 0)
							{
								if (Number < ERRORS)
								{
									CommonHelper.snprintf((DATA8*)pDestination, Length, $"{Number}");
								}
								else
								{
									CommonHelper.snprintf((DATA8*)pDestination, Length, $"Number {Number}");
								}
							}
						}
					}
					break;

				case GET_VOLUME:
					{
						*(DATA8*)PrimParPointer() = GetVolumePercent();
					}
					break;

				case SET_VOLUME:
					{
						SetVolumePercent(*(DATA8*)PrimParPointer());
					}
					break;

				case GET_MINUTES:
					{
						*(DATA8*)PrimParPointer() = GetSleepMinutes();
					}
					break;

				case SET_MINUTES:
					{
						SetSleepMinutes(*(DATA8*)PrimParPointer());
					}
					break;

			}
		}


		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opSTRINGS (CMD, ....)  </b>
		 *
		 *- String function entry\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   CMD     - \ref stringsubcode
		 *
		 *\n
		 *  - CMD = GET_SIZE
		 *\n  Get size of string (not including zero termination)\n
		 *    -  \param  (DATA8)   SOURCE1      - String variable or handle to string\n
		 *    -  \return (DATA16)  SIZE         - Size\n
		 *
		 *\n
		 *  - CMD = ADD
		 *\n  Add two strings (SOURCE1 + SOURCE2 -> DESTINATION)\n
		 *    -  \param  (DATA8)   SOURCE1      - String variable or handle to string\n
		 *    -  \param  (DATA8)   SOURCE2      - String variable or handle to string\n
		 *    -  \return (DATA8)   DESTINATION  - String variable or handle to string\n
		 *
		 *\n
		 *  - CMD = COMPARE
		 *\n  Compare two strings\n
		 *    -  \param  (DATA8)   SOURCE1      - String variable or handle to string\n
		 *    -  \param  (DATA8)   SOURCE2      - String variable or handle to string\n
		 *    -  \return (DATA8)   RESULT       - Result (0 = not equal, 1 = equal)\n
		 *
		 *\n
		 *  - CMD = DUPLICATE
		 *\n  Duplicate a string (SOURCE1 -> DESTINATION)\n
		 *    -  \param  (DATA8)   SOURCE1      - String variable or handle to string\n
		 *    -  \return (DATA8)   DESTINATION  - String variable or handle to string\n
		 *
		 *\n
		 *  - CMD = VALUE_TO_STRING
		 *\n  Convert floating point value to a string (strips trailing zeroes)\n
		 *    -  \param  (DATAF)   VALUE        - Value to write (if "nan" op to 4 dashes is returned: "----")\n
		 *    -  \param  (DATA8)   FIGURES      - Total number of figures inclusive decimal point (FIGURES < 0 -> Left adjusted)\n
		 *    -  \param  (DATA8)   DECIMALS     - Number of decimals\n
		 *    -  \return (DATA8)   DESTINATION  - String variable or handle to string\n
		 *
		 *\n
		 *  - CMD = NUMBER_TO_STRING
		 *\n  Convert integer value to a string\n
		 *    -  \param  (DATA16)  VALUE        - Value to write\n
		 *    -  \param  (DATA8)   FIGURES      - Total number of figures\n
		 *    -  \return (DATA8)   DESTINATION  - String variable or handle to string\n
		 *
		 *\n
		 *  - CMD = STRING_TO_VALUE
		 *\n  Convert string to floating point value\n
		 *    -  \param  (DATA8)   SOURCE1      - String variable or handle to string\n
		 *    -  \return (DATAF)   VALUE        - Value\n
		 *
		 *\n
		 *  - CMD = STRIP
		 *\n  Strip a string for spaces (SOURCE1 -> DESTINATION)\n
		 *    -  \param  (DATA8)   SOURCE1      - String variable or handle to string\n
		 *    -  \return (DATA8)   DESTINATION  - String variable or handle to string\n
		 *
		 *\n
		 *  - CMD = SUB
		 *\n  Return DESTINATION: a substring from SOURCE1 that starts were SOURCE2 ends\n
		 *    -  \param  (DATA8)   SOURCE1      - String variable or handle to string\n
		 *    -  \param  (DATA8)   SOURCE2      - String variable or handle to string\n
		 *    -  \return (DATA8)   DESTINATION  - String variable or handle to string\n
		 *
		 *\n
		 *  - CMD = VALUE_FORMATTED
		 *\n  Convert floating point value to a formatted string\n
		 *    -  \param  (DATAF)   VALUE        - Value to write\n
		 *    -  \param  (DATA8)   FORMAT       - Format string variable or handle to string\n
		 *    -  \param  (DATA8)   SIZE         - Total size of destination string\n
		 *    -  \return (DATA8)   DESTINATION  - String variable or handle to string\n
		 *
		 *\n
		 *  - CMD = NUMBER_FORMATTED
		 *\n  Convert integer number to a formatted string\n
		 *    -  \param  (DATA32)  NUMBER       - Number to write\n
		 *    -  \param  (DATA8)   FORMAT       - Format string variable or handle to string\n
		 *    -  \param  (DATA8)   SIZE         - Total size of destination string\n
		 *    -  \return (DATA8)   DESTINATION  - String variable or handle to string\n
		 *
		 *\n
		 *
		 *\ref stringexample1 "Program Example"
		 */
		/*! \brief  opSTRINGS byte code
		 *
		 */
		public void Strings()
		{
			DATA8 Cmd;
			DATA8* pSource1;
			DATA8* pSource2;
			DATA8* pDestination;
			DATAF DataF;
			DATA16 Data16;
			DATA32 Data32;
			DATA32 Start;
			DATA8 Tmp;
			DATA8 Figures;
			DATA8 Decimals;
			DATA8* Buffer = CommonHelper.Pointer1d<DATA8>(1024);

			Cmd = *(DATA8*)PrimParPointer();
			switch (Cmd)
			{ // Function

				case GET_SIZE:
					{
						pSource1 = (DATA8*)PrimParPointer();
						*(DATA16*)PrimParPointer() = (DATA16)CommonHelper.strlen((DATA8*)pSource1);
					}
					break;

				case ADD:
					{
						pSource1 = (DATA8*)PrimParPointer();
						pSource2 = (DATA8*)PrimParPointer();
						pDestination = (DATA8*)PrimParPointer();
						if (GH.VMInstance.Handle >= 0)
						{
							Data32 = (DATA32)CommonHelper.strlen((DATA8*)pSource1);
							Data32 += (DATA32)CommonHelper.strlen((DATA8*)pSource2);
							Data32 += 1;
							if (Data32 > MIN_ARRAY_ELEMENTS)
							{
								pDestination = (DATA8*)VmMemoryResize(GH.VMInstance.Handle, Data32);
							}
						}
						if (pDestination != null)
						{
							CommonHelper.sprintf((DATA8*)pDestination, $"{CommonHelper.GetString((DATA8*)pSource1)}{CommonHelper.GetString((DATA8*)pSource2)}");
						}
					}
					break;

				case COMPARE:
					{
						pSource1 = (DATA8*)PrimParPointer();
						pSource2 = (DATA8*)PrimParPointer();

						if (CommonHelper.strcmp((DATA8*)pSource1, (DATA8*)pSource2) == 0)
						{
							*(DATA8*)PrimParPointer() = 1;
						}
						else
						{
							*(DATA8*)PrimParPointer() = 0;
						}
					}
					break;

				case DUPLICATE:
					{
						pSource1 = (DATA8*)PrimParPointer();
						pDestination = (DATA8*)PrimParPointer();
						if (GH.VMInstance.Handle >= 0)
						{
							Data32 = (DATA32)CommonHelper.strlen((DATA8*)pSource1);
							Data32 += 1;
							if (Data32 > MIN_ARRAY_ELEMENTS)
							{
								pDestination = (DATA8*)VmMemoryResize(GH.VMInstance.Handle, Data32);
							}
						}
						if (pDestination != null)
						{
							CommonHelper.strcpy((DATA8*)pDestination, (DATA8*)pSource1);
						}
					}
					break;

				case VALUE_TO_STRING:
					{
						DataF = *(DATAF*)PrimParPointer();
						Figures = *(DATA8*)PrimParPointer();
						Decimals = *(DATA8*)PrimParPointer();
						pDestination = (DATA8*)PrimParPointer();
						if (GH.VMInstance.Handle >= 0)
						{
							if (Figures >= 0)
							{
								Data32 = (DATA32)Figures;
							}
							else
							{
								Data32 = (DATA32)(0 - Figures);
							}
							Data32 += 2;
							if (Data32 > MIN_ARRAY_ELEMENTS)
							{
								pDestination = (DATA8*)VmMemoryResize(GH.VMInstance.Handle, Data32);
							}
						}
						if (pDestination != null)
						{
							if (float.IsNaN(DataF))
							{
								if (Figures < 0)
								{ // "----    "
									Figures = (sbyte)(0 - Figures);

									Tmp = 0;
									while ((Tmp < 4) && (Tmp < Figures))
									{
										pDestination[Tmp] = (sbyte)'-';
										Tmp++;
									}
									while (Tmp < Figures)
									{
										pDestination[Tmp] = (sbyte)' ';
										Tmp++;
									}
								}
								else
								{ // "    ----"

									Tmp = 0;
									while (Tmp < (Figures - 4))
									{
										pDestination[Tmp] = (sbyte)' ';
										Tmp++;
									}
									while (Tmp < Figures)
									{
										pDestination[Tmp] = (sbyte)'-';
										Tmp++;
									}
								}
								pDestination[Tmp] = 0;
							}
							else
							{
								if (Figures >= 0)
								{
									CommonHelper.snprintf((DATA8*)pDestination, Figures + 1, CommonHelper.GetString(DataF, Figures, Decimals));
								}
								else
								{
									Figures = (sbyte)(0 - Figures);
									CommonHelper.snprintf((DATA8*)pDestination, Figures + 1, CommonHelper.GetString(DataF, -1, Decimals));
								}
								if (Decimals != 0)
								{ // Strip trailing zeroes

									Figures = (DATA8)CommonHelper.strlen((DATA8*)pDestination);

									while ((Figures != 0) && ((pDestination[Figures] == '0') || (pDestination[Figures] == 0)))
									{
										pDestination[Figures] = 0;
										Figures--;
									}
									if (pDestination[Figures] == '.')
									{
										pDestination[Figures] = 0;
									}
								}
							}
						}
					}
					break;

				case NUMBER_TO_STRING:
					{
						Data16 = *(DATA16*)PrimParPointer();
						Figures = *(DATA8*)PrimParPointer();
						pDestination = (DATA8*)PrimParPointer();
						if (GH.VMInstance.Handle >= 0)
						{
							Data32 = (DATA32)Figures;
							Data32 += 2;
							if (Data32 > MIN_ARRAY_ELEMENTS)
							{
								pDestination = (DATA8*)VmMemoryResize(GH.VMInstance.Handle, Data32);
							}
						}
						if (pDestination != null)
						{
							CommonHelper.snprintf((DATA8*)pDestination, Figures + 1, $"{Data16}");
						}
					}
					break;

				case STRING_TO_VALUE:
					{
						pSource1 = (DATA8*)PrimParPointer();

						Data16 = 0;
						while (pSource1[Data16] != 0)
						{
							if (pSource1[Data16] == ',')
							{
								pSource1[Data16] = (sbyte)'.';
							}
							Data16++;
						}

						CommonHelper.sscanf((DATA8*)pSource1, "%f", &DataF);

						*(DATAF*)PrimParPointer() = DataF;
					}
					break;

				case STRIP:
					{
						pSource1 = (DATA8*)PrimParPointer();
						pDestination = (DATA8*)PrimParPointer();
						if (GH.VMInstance.Handle >= 0)
						{
							Data32 = (DATA32)CommonHelper.strlen((DATA8*)pSource1);
							Data32 += 1;
							if (Data32 > MIN_ARRAY_ELEMENTS)
							{
								pDestination = (DATA8*)VmMemoryResize(GH.VMInstance.Handle, Data32);
							}
						}
						if (pDestination != null)
						{
							while (*pSource1 != 0)
							{
								if ((*pSource1 != ' '))
								{
									*pDestination = *pSource1;
									pDestination++;
								}
								pSource1++;
							}
							*pDestination = *pSource1;
						}
					}
					break;

				case SUB:
					{
						pSource1 = (DATA8*)PrimParPointer();
						pSource2 = (DATA8*)PrimParPointer();
						pDestination = (DATA8*)PrimParPointer();

						Start = (DATA32)CommonHelper.strlen((DATA8*)pSource2);
						if (GH.VMInstance.Handle >= 0)
						{
							Data32 = (DATA32)CommonHelper.strlen((DATA8*)pSource1);
							Data32 += 1;
							if (Data32 > MIN_ARRAY_ELEMENTS)
							{
								pDestination = (DATA8*)VmMemoryResize(GH.VMInstance.Handle, Data32);
							}
						}
						if (pDestination != null)
						{
							CommonHelper.snprintf(Buffer, 1024, CommonHelper.GetString(CommonHelper.strstr((DATA8*)pSource1, (DATA8*)pSource2)));
							CommonHelper.sprintf((DATA8*)pDestination, CommonHelper.GetString(&Buffer[(DATA16)Start]));
						}
					}
					break;

				case VALUE_FORMATTED:
					{
						DataF = *(DATAF*)PrimParPointer();
						pSource1 = (DATA8*)PrimParPointer();
						Figures = *(DATA8*)PrimParPointer();
						pDestination = (DATA8*)PrimParPointer();

						// TODO: WARNING: format is not implemented
						//CommonHelper.snprintf(Buffer, 1024, (DATA8*)pSource1, DataF);
						CommonHelper.snprintf(Buffer, 1024, DataF.ToString());
						if (GH.VMInstance.Handle >= 0)
						{
							Data32 = (DATA32)CommonHelper.strlen((DATA8*)Buffer);
							Data32 += 1;
							if (Data32 > MIN_ARRAY_ELEMENTS)
							{
								pDestination = (DATA8*)VmMemoryResize(GH.VMInstance.Handle, Data32);
							}
							Figures = (DATA8)Data32;
						}
						if (pDestination != null)
						{
							CommonHelper.snprintf((DATA8*)pDestination, Figures, CommonHelper.GetString(Buffer));
						}

					}
					break;

				case NUMBER_FORMATTED:
					{
						Data32 = *(DATA32*)PrimParPointer();
						pSource1 = (DATA8*)PrimParPointer();
						Figures = *(DATA8*)PrimParPointer();
						pDestination = (DATA8*)PrimParPointer();

						// TODO: WARNING: format is not implemented
						//CommonHelper.snprintf(Buffer, 1024, (DATA8*)pSource1, Data32);
						CommonHelper.snprintf(Buffer, 1024, Data32.ToString());
						if (GH.VMInstance.Handle >= 0)
						{
							Data32 = (DATA32)CommonHelper.strlen((DATA8*)Buffer);
							Data32 += 1;
							if (Data32 > MIN_ARRAY_ELEMENTS)
							{
								pDestination = (DATA8*)VmMemoryResize(GH.VMInstance.Handle, Data32);
							}
							Figures = (DATA8)Data32;
						}
						if (pDestination != null)
						{
							CommonHelper.snprintf((DATA8*)pDestination, Figures, CommonHelper.GetString(Buffer));
						}

					}
					break;

			}
		}


		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opMEMORY_WRITE (PRGID, OBJID, OFFSET, SIZE, ARRAY)</b>
		 *
		 *- Write VM memory\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  PRGID   - Program slot number (must be running) (see \ref prgid)
		 *  \param  (DATA16)  OBJID   - Object id (zero means globals)
		 *  \param  (DATA32)  OFFSET  - Offset (start from)
		 *  \param  (DATA32)  SIZE    - Size (length of array to write)
		 *  \param  (DATA8)   ARRAY   - First element of DATA8 array to write\n
		 *
		 *\ref opMEMORY_WRITE1 "Direct command example"
		 */
		/*! \brief    opMEMORY_WRITE byte code
		 *
		 *            Write to  VM memory
		 *
		 *            Uses following from current program context:
		 *            None
		 *
		 */
		public void MemoryWrite()
		{
			PRGID PrgId;
			OBJID ObjId;
			GBINDEX Offset;
			GBINDEX Size;
			DATA8* pArray;
			VARDATA* Ram;
			GBINDEX Tmp;
			GBINDEX Lng;

			PrgId = *(PRGID*)PrimParPointer();
			ObjId = *(OBJID*)PrimParPointer();
			Offset = *(GBINDEX*)PrimParPointer();
			Size = *(GBINDEX*)PrimParPointer();
			pArray = (DATA8*)PrimParPointer();

			//  GH.printf("p=%-1d o=%-2d f=%-4d s=%-4d d=0x%02X\r\n",PrgId,ObjId,Offset,Size,*pArray);

			if (PrgId < MAX_PROGRAMS)
			{
				if (GH.VMInstance.Program[PrgId].Status != OBJSTAT.STOPPED)
				{
					if ((ObjId >= 0) && (ObjId <= GH.VMInstance.Program[PrgId].Objects))
					{
						if (ObjId == 0)
						{
							Ram = GH.VMInstance.Program[PrgId].pGlobal;
							Lng = (*(IMGHEAD*)GH.VMInstance.Program[PrgId].pImage).GlobalBytes;
						}
						else
						{
							Ram = (*GH.VMInstance.Program[PrgId].pObjList[ObjId]).pLocal;
							Lng = GH.VMInstance.Program[PrgId].pObjHead[ObjId].LocalBytes;
						}

						for (Tmp = 0; Tmp < Size; Tmp++)
						{
							if ((Tmp + Offset) < Lng)
							{
								Ram[Tmp + Offset] = (byte)pArray[Tmp];
							}
						}
					}
				}
			}
		}


		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opMEMORY_READ (PRGID, OBJID, OFFSET, SIZE, ARRAY)</b>
		 *
		 *- Read VM memory\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA16)  PRGID   - Program slot number (must be running) (see \ref prgid)
		 *  \param  (DATA16)  OBJID   - Object id (zero means globals)
		 *  \param  (DATA32)  OFFSET  - Offset (start from)
		 *  \param  (DATA32)  SIZE    - Size (length of array to read)
		 *  \return (DATA8)   ARRAY   - First element of DATA8 array to receive data\n
		 *
		 *  \ref opMEMORY_READ1 "Direct command example"
		 */
		/*! \brief    opMEMORY_READ byte code
		 *
		 *            Read VM memory
		 *
		 *            Uses following from current program context:
		 *            None
		 *
		 */
		public void MemoryRead()
		{
			PRGID PrgId;
			OBJID ObjId;
			GBINDEX Offset;
			GBINDEX Size;
			DATA8* pArray;
			VARDATA* Ram;
			GBINDEX Tmp;
			GBINDEX Lng;

			PrgId = *(PRGID*)PrimParPointer();
			ObjId = *(OBJID*)PrimParPointer();
			Offset = *(GBINDEX*)PrimParPointer();
			Size = *(GBINDEX*)PrimParPointer();
			pArray = (DATA8*)PrimParPointer();

			if (PrgId < MAX_PROGRAMS)
			{
				if (GH.VMInstance.Program[PrgId].Status != OBJSTAT.STOPPED)
				{
					if ((ObjId >= 0) && (ObjId <= GH.VMInstance.Program[PrgId].Objects))
					{
						if (ObjId == 0)
						{
							Ram = GH.VMInstance.Program[PrgId].pGlobal;
							Lng = (*(IMGHEAD*)GH.VMInstance.Program[PrgId].pImage).GlobalBytes;
						}
						else
						{
							Ram = (*GH.VMInstance.Program[PrgId].pObjList[ObjId]).pLocal;
							Lng = GH.VMInstance.Program[PrgId].pObjHead[ObjId].LocalBytes;
						}

						for (Tmp = 0; Tmp < Size; Tmp++)
						{
							if ((Tmp + Offset) < Lng)
							{
								pArray[Tmp] = (sbyte)Ram[Tmp + Offset];
							}
							else
							{
								pArray[Tmp] = 0;
							}
						}
					}
				}
			}
		}


		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opPORT_CNV_OUTPUT (PortIn, Layer, Bitfield, Inverted)</b>
		 *
		 *- Convert encoded port to Layer and Bitfield\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  PortIn   - EncodedPortNumber
		 *  \return (DATA8)   Layer    - Layer
		 *  \return (DATA8)   Bitfield - Bitfield
		 *  \return (DATA8)   Inverted - True if left/right motor are inverted (ie, C&A)
		 *
		 */
		public void PortCnvOutput()
		{
			DATA32 Ports = *(DATA32*)PrimParPointer();
			DATA32 inputPorts = Ports;
			DATA8 secondPortBitfield = (sbyte)((DATA8)1 << ((inputPorts % 10) - 1));
			inputPorts /= 10;
			DATA8 firstPortBitfield = (sbyte)((DATA8)1 << ((inputPorts % 10) - 1));
			inputPorts /= 10;
			DATA8 layer = (sbyte)((DATA8)(inputPorts % 10) - 1);
			if (layer < 0)
			{
				layer = 0;
			}
			DATA8 bitfield = (sbyte)(firstPortBitfield | secondPortBitfield);
			*(DATA8*)PrimParPointer() = layer;
			*(DATA8*)PrimParPointer() = bitfield;
			*(DATA8*)PrimParPointer() = (sbyte)(firstPortBitfield > secondPortBitfield ? 1 : 0);
		}


		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opPORT_CNV_INPUT (PortIn, Layer, PortOut)</b>
		 *
		 *- Convert encoded port to Layer and Port\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  PortIn   - EncodedPortNumber
		 *  \return (DATA8)   Layer - Layer
		 *  \return (DATA8)   PortOut - 0-index port for use with VM commands
		 *
		 */
		public void PortCnvInput()
		{
			DATA32 inputPorts = *(DATA32*)PrimParPointer();
			DATA8 port = (sbyte)((DATA8)(inputPorts % 10) - 1);
			inputPorts /= 100;
			DATA8 layer = (sbyte)((DATA8)(inputPorts % 10) - 1);
			if (layer < 0)
			{
				layer = 0;
			}
			*(DATA8*)PrimParPointer() = layer;
			*(DATA8*)PrimParPointer() = port;
		}

		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opNOTE_TO_FREQ (NOTE, FREQ)</b>
		 *
		 *- Convert note to tone\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   NOTE     - Note string (HND) (e.c. "C#4")
		 *  \return (DATA16)  FREQ     - Frequency [Hz]
		 *
		 */
		public void NoteToFreq()
		{
			DATA8* pNote;
			DATA16 Freq;
			DATA8 Note;

			Freq = 440;

			pNote = (DATA8*)PrimParPointer();

			for (Note = 0; Note < NOTES; Note++)
			{
				fixed (NOTEFREQ* tmpP = &NoteFreq[0])
				if (CommonHelper.strcmp((DATA8*)tmpP[Note].Note, (DATA8*)pNote) == 0)
				{
					Freq = NoteFreq[Note].Freq;
				}
			}

			*(DATA16*)PrimParPointer() = Freq;
		}

		/*! \page VM
		 *  <hr size="1"/>
		 *  <b>     opSYSTEM(COMMAND, STATUS)</b>
		 *
		 *- Executes a system command\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   COMMAND  - Command string (HND)
		 *  \return (DATA32)  STATUS   - Return status of the command
		 *
		 */
		public void System_()
		{
			DATA32 Status = -1;
			DATA8* pCmd;

			pCmd = (DATA8*)PrimParPointer();

			// TODO: no system bytecode execute
			//Status = (DATA32)system((char*)pCmd);
			//sync();

			*(DATA32*)PrimParPointer() = Status;
		}


		//*****************************************************************************

		ULONG SavedPriority;
		public void Monitor()
		{
			IMINDEX Index;
			LBINDEX Lv, Ln;
			OBJID ObjId;
			OBJID OwnerObjId;
			LP pObjectLocal;
			char ObjStat;
			DATA8 Esc;


			if (GH.VMInstance.Debug == 1)
			{
				SavedPriority = GH.VMInstance.Priority;

				CommonHelper.snprintf(GH.VMInstance.PrintBuffer, PRINTBUFFERSIZE, $"\r\n         {GH.VMInstance.ProgramId} {GH.VMInstance.ObjectId} IP={(ulong)GH.VMInstance.ObjectIp:B} -> ");
				VmPrint(GH.VMInstance.PrintBuffer);

				Ln = 16;

				for (Lv = 0; Lv < Ln; Lv++)
				{
					CommonHelper.snprintf(GH.VMInstance.PrintBuffer, PRINTBUFFERSIZE, $"{GH.VMInstance.ObjectIp[Lv] & 0xFF:B} ");
					VmPrint(GH.VMInstance.PrintBuffer);
					if (((Lv & 0x3) == 0x3) && ((Lv & 0xF) != 0xF))
					{
						VmPrint(" ".AsSbytePointer());
					}
					if (((Lv & 0xF) == 0xF) && (Lv < (Ln - 1)))
					{
						VmPrint("\r\n".AsSbytePointer());
					}
				}
				VmPrint("\r\n".AsSbytePointer());

				CommonHelper.snprintf(GH.VMInstance.PrintBuffer, PRINTBUFFERSIZE, $"              GV={(ulong)GH.VMInstance.pGlobal:B} -> ");
				VmPrint(GH.VMInstance.PrintBuffer);

				Ln = (*(IMGHEAD*)GH.VMInstance.pImage).GlobalBytes;

				for (Lv = 0; Lv < Ln; Lv++)
				{
					CommonHelper.snprintf(GH.VMInstance.PrintBuffer, PRINTBUFFERSIZE, $"{GH.VMInstance.pGlobal[Lv] & 0xFF:B} ");
					VmPrint(GH.VMInstance.PrintBuffer);
					if (((Lv & 0x3) == 0x3) && ((Lv & 0xF) != 0xF))
					{
						VmPrint(" ".AsSbytePointer());
					}
					if (((Lv & 0xF) == 0xF) && (Lv < (Ln - 1)))
					{
						VmPrint("\r\n                             ".AsSbytePointer());
					}
				}
				VmPrint("\r\n".AsSbytePointer());

				for (ObjId = 1; ObjId <= GH.VMInstance.Objects; ObjId++)
				{
					switch ((*GH.VMInstance.pObjList[ObjId]).ObjStatus)
					{
						case RUNNING:
							{
								ObjStat = 'r';
							}
							break;

						case WAITING:
							{
								ObjStat = 'w';
							}
							break;

						case STOPPED:
							{
								ObjStat = 's';
							}
							break;

						case HALTED:
							{
								ObjStat = 'h';
							}
							break;

						default:
							{
								ObjStat = '?';
							}
							break;

					}
					if (ObjId == GH.VMInstance.ObjectId)
					{
						ObjStat = '>';
					}

					OwnerObjId = GH.VMInstance.pObjHead[ObjId].OwnerObjectId;
					pObjectLocal = (*GH.VMInstance.pObjList[ObjId]).pLocal;

					if (OwnerObjId != 0)
					{ // Reuse locals from owner

						Ln = GH.VMInstance.pObjHead[OwnerObjId].LocalBytes;

					}
					else
					{ // Use local allocated to object

						Ln = GH.VMInstance.pObjHead[ObjId].LocalBytes;
					}



					CommonHelper.snprintf(GH.VMInstance.PrintBuffer, PRINTBUFFERSIZE, $"         {(char)ObjStat} {ObjId} LV={(ulong)pObjectLocal:B} -> ");
					VmPrint(GH.VMInstance.PrintBuffer);

					for (Lv = 0; Lv < Ln; Lv++)
					{
						CommonHelper.snprintf(GH.VMInstance.PrintBuffer, PRINTBUFFERSIZE, $"{pObjectLocal[Lv] & 0xFF:B} ");
						VmPrint(GH.VMInstance.PrintBuffer);
						if (((Lv & 0x3) == 0x3) && ((Lv & 0xF) != 0xF))
						{
							VmPrint(" ".AsSbytePointer());
						}
						if (((Lv & 0xF) == 0xF) && (Lv < (Ln - 1)))
						{
							VmPrint("\r\n                             ".AsSbytePointer());
						}
					}
					VmPrint("\r\n".AsSbytePointer());

				}

				VmPrint("\r\n".AsSbytePointer());

				Index = (IMINDEX)GH.VMInstance.ObjectIp - (IMINDEX)GH.VMInstance.pImage;
				GH.Validate.cValidateDisassemble(GH.VMInstance.pImage, &Index, (LABEL*)GH.VMInstance.Program[GH.VMInstance.ProgramId].Label);
				GH.VMInstance.Debug++;
				GH.Ui.cUiEscape();
			}
			if (GH.VMInstance.Debug == 2)
			{
				Esc = GH.Ui.cUiEscape();
				switch (Esc)
				{
					case (sbyte)' ':
						{
							GH.VMInstance.Priority = SavedPriority;
							GH.VMInstance.Priority--;
							PrimDispatchTabel[*(GH.VMInstance.ObjectIp++)]();
							GH.VMInstance.Debug--;
						}
						break;

					case (sbyte)'<':
						{
							GH.VMInstance.Priority = SavedPriority;
							GH.VMInstance.Priority--;
							PrimDispatchTabel[*(GH.VMInstance.ObjectIp++)]();
							GH.VMInstance.Debug = 0;
						}
						break;

					default:
						{
							GH.VMInstance.Priority = 0;
						}
						break;

				}
			}
		}


		public RESULT TstOpen(UWORD Time)
		{
			RESULT Result = RESULT.FAIL;
			int File;

			if ((Time > 0) && (Time <= 30000))
			{
				if (GH.VMInstance.Test == 0)
				{
					// TODO: tst pin shite
					//File = open(TEST_PIN_DEVICE_NAME, O_RDWR | O_SYNC);
					//if (File >= MIN_HANDLE)
					//{
					//	ioctl(File, TST_PIN_ON, null);
					//	close(File);

					//	File = open(TEST_UART_DEVICE_NAME, O_RDWR | O_SYNC);
					//	if (File >= MIN_HANDLE)
					//	{
					//		ioctl(File, TST_UART_ON, null);
					//		close(File);
					//		GH.VMInstance.Test = Time;
					//		Result = OK;
					//	}
					//}
					GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");
				}
				else
				{
					GH.VMInstance.Test = Time;
					Result = OK;
				}
			}

			return (Result);
		}


		public void TstClose()
		{
			int File;

			if (GH.VMInstance.Test != 0)
			{
				// TODO: tst pin shite
				//File = open(TEST_UART_DEVICE_NAME, O_RDWR | O_SYNC);
				//if (File >= MIN_HANDLE)
				//{
				//	ioctl(File, TST_UART_OFF, null);
				//	close(File);

				//	File = open(TEST_PIN_DEVICE_NAME, O_RDWR | O_SYNC);
				//	if (File >= MIN_HANDLE)
				//	{
				//		ioctl(File, TST_PIN_OFF, null);
				//		close(File);
				//		GH.VMInstance.Test = 0;
				//	}
				//}
				GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");
			}
		}


		/*! \page TST
		 *  <hr size="1"/>
		 *  <b>     opTST (CMD, ....)  </b>
		 *
		 *- System test functions entry\n
		 *- This set of commands are for test only as they change behaviour in some driver
		 *  modules.
		 *  When test is open every command keeps the test byte codes enabled for 10 seconds
		 *\n
		 *
		 *  \param  (DATA8)   CMD     - \ref tstsubcode
		 *
		 *\n
		 *  - CMD = TST_OPEN
		 *\n  Enables test byte codes for 10 seconds\n
		 *
		 *\n
		 *  - CMD = TST_CLOSE
		 *\n  Disables test byte codes\n
		 *
		 *\n
		 *  - CMD = TST_READ_PINS
		 *\n  Read connector pin status\n
		 *    -  \param  (DATA8)   PORT         - Input connector [0..3], output connector [16..19]\n
		 *    -  \param  (DATA8)   LENGTH       - Number of pins in returned string\n
		 *    -  \return (DATA8)   STRING       - String variable start index ('0' = low, '1' = high)\n
		 *\n
		 *\n
		 *    Input\n
		 *    --------\n
		 *    STRING[0]     Pin1   I_ONx    current source control output ['0','1']\n
		 *    STRING[1]     Pin2   LEGDETx  ['0','1']\n
		 *    STRING[2]     Pin5   DIGIx0   ['0','1']\n
		 *    STRING[3]     Pin6   DIGIx1   ['0','1']\n
		 *    STRING[4]     -      TXINx_EN ['0','1']\n
		 *\n
		 *    Output\n
		 *    --------\n
		 *    STRING[0]     Pin1   MxIN0    ['0','1']\n
		 *    STRING[1]     Pin2   MxIN1    ['0','1']\n
		 *    STRING[2]     Pin5   DETx0    ['0','1']\n
		 *    STRING[3]     Pin5   INTx0    ['0','1']\n
		 *    STRING[4]     Pin6   DIRx     ['0','1']\n
		 *
		 *
		 *\n
		 *  - CMD = TST_WRITE_PINS
		 *\n  Write to connector pin\n
		 *    -  \param  (DATA8)   PORT         - Input connector [0..3], output connector [16..19]\n
		 *    -  \param  (DATA8)   LENGTH       - Number of pins to write\n
		 *    -  \return (DATA8)   STRING       - String variable start index ('0' = set low, '1' = set high, 'X' = tristate, '-' = don't touch)\n
		 *\n
		 *\n
		 *    Input\n
		 *    --------\n
		 *    STRING[0]     Pin1   I_ONx    current source control output ['0','1','X','-']\n
		 *    STRING[1]     Pin2   LEGDETx  ['0','1','X','-']\n
		 *    STRING[2]     Pin5   DIGIx0   ['0','1','X','-']\n
		 *    STRING[3]     Pin6   DIGIx1   ['0','1','X','-']\n
		 *    STRING[4]     -      TXINx_EN ['0','1','X','-']\n
		 *\n
		 *    Output\n
		 *    --------\n
		 *    STRING[0]     Pin1   MxIN0    ['0','1','X','-']\n
		 *    STRING[1]     Pin2   MxIN1    ['0','1','X','-']\n
		 *    STRING[2]     Pin5   DETx0    Write   ['0','1','X','-']\n
		 *    STRING[3]     Pin5   INTx0    Read    ['0','1','X','-']\n
		 *    STRING[4]     Pin6   DIRx     ['0','1','X','-']\n
		 *
		 *
		 *\n
		 *  - CMD = TST_READ_ADC
		 *\n  Read raw count from ADC\n
		 *    -  \param  (DATA8)   INDEX        - Input mapped index (see below) [0..15]\n
		 *    -  \return (DATA16)  VALUE        - Raw count [0..4095]\n
		 *\n
		 *\n
		 *    INDEX 0..3    Input  connector pin 1 ( 0=conn1,  1=conn2,  2=conn3,  3=conn4)\n
		 *    INDEX 4..7    Input  connector pin 6 ( 4=conn1,  5=conn2,  6=conn3,  7=conn4)\n
		 *    INDEX 8..11   Output connector pin 5 ( 8=conn1,  9=conn2, 10=conn3, 11=conn4)\n
		 *\n
		 *    INDEX 12      Battery temperature\n
		 *    INDEX 13      Current flowing to motors\n
		 *    INDEX 14      Current flowing from the battery\n
		 *    INDEX 15      Voltage at battery cell 1, 2, 3,4, 5, and 6\n
		 *
		 *
		 *\n
		 *  - CMD = TST_ENABLE_UART
		 *\n  Enable all UARTs\n
		 *    -  \param  (DATA32)  BITRATE      - Bit rate [2400..115200 b/S]\n
		 *
		 *
		 *\n
		 *  - CMD = TST_WRITE_UART
		 *\n  Write data to port through UART\n
		 *    -  \param  (DATA8)   PORT         - Input connector [0..3]\n
		 *    -  \param  (DATA8)   LENGTH       - Length of string to write [0..63]\n
		 *    -  \param  (DATA8)   STRING       - String of data\n
		 *
		 *
		 *\n
		 *  - CMD = TST_READ_UART
		 *\n  Read data from port through UART\n
		 *    -  \param  (DATA8)   PORT         - Input connector [0..3]\n
		 *    -  \param  (DATA8)   LENGTH       - Length of string to read [0..63]\n
		 *    -  \param  (DATA8)   STRING       - String of data\n
		 *
		 *
		 *\n
		 *  - CMD = TST_DISABLE_UART
		 *\n  Disable all UARTs\n
		 *
		 *
		 *\n
		 *  - CMD = TST_ACCU_SWITCH
		 *\n  Read accu switch state\n
		 *    -  \return (DATA8)   ACTIVE       - State [0..1]\n
		 *
		 *
		 *\n
		 *  - CMD = TST_BOOT_MODE"
		 *\n  Turn on mode2\n
		 *
		 *
		 *\n
		 *  - CMD = TST_POLL_MODE2
		 *\n  Read mode2 status\n
		 *    -  \return (DATA8)   STATUS       - State [0..2]\n
		 *
		 *
		 *\n
		 *  - CMD = TST_CLOSE_MODE2
		 *\n  Closes mode2\n
		 *
		 *
		 *
		 *\n
		 *  - CMD = TST_RAM_CHECK
		 *\n  Read RAM test status status\n
		 *    -  \return (DATA8)   STATUS       - State [0,1]  0 = RESULT.FAIL, 1 = SUCCESS\n
		 *
		 *
		 *\n
		 *
		 *  <hr size="1"/>
		 *  <b>Direct test command examples:</b>
		 *\verbatim
		*

		--------------------------------------
		Set DIGIBI0, low
		--------------------------------------
					  v
		12000000000000FF0AFF0D0105842D2D302D2D00

		0300000002

		--------------------------------------
		Set DIGIBI0, high
		--------------------------------------
					  v
		12000000000000FF0AFF0D0105842D2D312D2D00

		0300000002

		--------------------------------------
		Set DIGIBI0, float
		--------------------------------------
					  v
		12000000000000FF0AFF0D0105842D2D582D2D00

		0300000002

		--------------------------------------
		Read ADC value, Port 2, Pin 6
		--------------------------------------
					  v
		0B000000000200FF0AFF0E0560

		0500000002xxxx
		xxxx  = A/D count [0..4095]

		--------------------------------------
		Activate 9V, Input Port 2
		--------------------------------------
					  v
		12000000000000FF0AFF0D010584312D2D2D2D00

		0300000002

		--------------------------------------
		Read ADC value, Port 2, Pin 1
		--------------------------------------
					  v
		0B000000000200FF0AFF0E0160

		0500000002xxxx
		xxxx  = A/D count [0..4095]

		--------------------------------------
		Deactivate 9V, Input Port 2
		--------------------------------------
					  v
		12000000000000FF0AFF0D010584302D2D2D2D00

		0300000002

		--------------------------------------
		Transmit and receive "UUUUU" on Port 2
		--------------------------------------
					  v
		20000000000500FF0AFF118300C20100FF0F010584555555555500FF10010560FF12

		0800000002xxxxxxxxxx
		xxxxxxxxxx = 5555555555 = "UUUUU"

		--------------------------------------
		Read rechargeable battery availability
		--------------------------------------
					  v
		0A000000000100FF0AFF1360

		0400000002xx
		xx    = SW1     xx = 00 not active, xx = 01 active


		--------------------------------------
		Check for mode2
		--------------------------------------

		Initiate mode2:
					  v
		09000000000000FF0AFF14

		0300000002


		Poll mode2 status:
					  v
		0A000000000100FF0AFF1560

		0400000002xx

		xx = Mode2 status  xx = 00 success, xx = 01 in progress, xx = 02 error


		Close mode2:
					  v
		09000000000000FF0AFF16

		0300000002

		--------------------------------------
		RAM Check poll
		--------------------------------------

		Poll RAM check:
					  v
		0A000000000100FF0AFF1760

		0400000002xx

		xx = RAM check  xx = 00 OK, xx = 02 RESULT.FAIL


		****************************************************************
		**** OTHER FUNCTIONS WITH NORMAL BYTE CODES ********************
		****************************************************************

		--------------------------------------
		Read UI Button status
		--------------------------------------
					  v
		1D000000000600830901608309026183090362830904638309056483090665

		0900000002uueeddrrllbb
		uu    = up      uu = 00 not active, uu = 01 active
		ee    = enter   ee = 00 not active, ee = 01 active
		dd    = down    dd = 00 not active, dd = 01 active
		rr    = right   rr = 00 not active, rr = 01 active
		ll    = left    ll = 00 not active, ll = 01 active
		bb    = back    bb = 00 not active, bb = 01 active

		--------------------------------------
		Tone 1 KHz, forever
		--------------------------------------
					  v
		0D0000000000009401816482E80300

		0300000002

		--------------------------------------
		Stop Sound output
		--------------------------------------
					  v
		070000000000009400

		0300000002

		--------------------------------------
		Show Chess pattern 1 (start with black)
		--------------------------------------
					  v
		160000000000008412008413000000841382550000008400

		0300000002

		--------------------------------------
		Show Chess pattern 1 (start with white)
		--------------------------------------
					  v
		160000000000008412008413000000841382AA0000008400

		0300000002

		--------------------------------------
		Clear display again
		--------------------------------------
					  v
		0C00000000000084130000008400

		0300000002

		--------------------------------------
		Check for SD card
		--------------------------------------
					  v
		0A000000000900811E686064

		0C00000002ttttttttffffffffpp
		tttttttt = total size
		ffffffff = free space
		pp       = present [00 = no, 01 = yes]

		--------------------------------------
		Bluetooth on and visible
		--------------------------------------
					  v
		13000000000000D4010201D00200D4020201D00200

		0300000002

		--------------------------------------
		Bluetooth off
		--------------------------------------
					  v
		0C000000000000D4010200D00200

		0300000002

		*\endverbatim
		 *
		 *
		 */
		/*! \brief  opTST byte code
		 *
		 */
		public void Tst()
		{
			DATA8 Cmd;
			DATA8* pPins;
			DATA8 Index;
			DATA8 Data8;
			DATA16 Value;
			DATA8* Buffer = CommonHelper.Pointer1d<DATA8>(2);
			TSTPIN Tstpin;
			TSTUART Tstuart;
			int File;

			Cmd = *(DATA8*)PrimParPointer();

			if (Cmd == TST_OPEN)
			{ // Test open

				TstOpen(10000);
			}
			else
			{
				if (GH.VMInstance.Test != 0)
				{
					TstOpen(10000);

					switch (Cmd)
					{ // Function

						case TST_READ_PINS:
							{ // Read pins

								Tstpin.Port = *(DATA8*)PrimParPointer();
								Tstpin.Length = *(DATA8*)PrimParPointer();
								pPins = (DATA8*)PrimParPointer();

								// TODO: tst pin shite
								//File = open(TEST_PIN_DEVICE_NAME, O_RDWR | O_SYNC);
								//if (File >= 0)
								//{
								//	ioctl(File, TST_PIN_READ, &Tstpin);
								//	close(File);
								//	for (Index = 0; Index < Tstpin.Length; Index++)
								//	{
								//		pPins[Index] = Tstpin.String[Index];
								//	}
								//}
								GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");
							}
							break;

						case TST_WRITE_PINS:
							{ // Write to pins

								Tstpin.Port = *(DATA8*)PrimParPointer();
								Tstpin.Length = *(DATA8*)PrimParPointer();
								pPins = (DATA8*)PrimParPointer();

								for (Index = 0; Index < Tstpin.Length; Index++)
								{
									Tstpin.String[Index] = pPins[Index];
								}

								// TODO: tst pin shite
								//File = open(TEST_PIN_DEVICE_NAME, O_RDWR | O_SYNC);
								//if (File >= MIN_HANDLE)
								//{
								//	ioctl(File, TST_PIN_WRITE, &Tstpin);
								//	close(File);
								//}
								GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");

							}
							break;

						case TST_READ_ADC:
							{
								Index = *(DATA8*)PrimParPointer();
								Value = DATA16_NAN;

								if ((Index >= 0) && (Index < 4))
								{
									Value = (*GH.InputInstance.pAnalog).InPin1[Index];
								}
								if ((Index >= 4) && (Index < 8))
								{
									Value = (*GH.InputInstance.pAnalog).InPin6[Index - 4];
								}
								if ((Index >= 8) && (Index < 12))
								{
									Value = (*GH.InputInstance.pAnalog).OutPin5[Index - 8];
								}
								if (Index == 12)
								{
									Value = (*GH.InputInstance.pAnalog).BatteryTemp;
								}
								if (Index == 13)
								{
									Value = (*GH.InputInstance.pAnalog).MotorCurrent;
								}
								if (Index == 14)
								{
									Value = (*GH.InputInstance.pAnalog).BatteryCurrent;
								}
								if (Index == 15)
								{
									Value = (*GH.InputInstance.pAnalog).Cell123456;
								}

								*(DATA16*)PrimParPointer() = Value;
							}
							break;

						case TST_ENABLE_UART:
							{
								Tstuart.Bitrate = *(DATA32*)PrimParPointer();
								// TODO: tst pin shite
								//File = open(TEST_UART_DEVICE_NAME, O_RDWR | O_SYNC);
								//if (File >= 0)
								//{
								//	ioctl(File, TST_UART_EN, &Tstuart);
								//	close(File);
								//}
								GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");
							}
							break;

						case TST_DISABLE_UART:
							{
								// TODO: tst pin shite
								//File = open(TEST_UART_DEVICE_NAME, O_RDWR | O_SYNC);
								//if (File >= MIN_HANDLE)
								//{
								//	ioctl(File, TST_UART_DIS, &Tstuart);
								//	close(File);
								//}
								GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");
							}
							break;

						case TST_WRITE_UART:
							{
								Tstuart.Port = *(DATA8*)PrimParPointer();
								Tstuart.Length = *(DATA8*)PrimParPointer();
								pPins = (DATA8*)PrimParPointer();

								if (Tstuart.Port < INPUTS)
								{
									for (Index = 0; (Index < Tstuart.Length) && (Index < TST_UART_LENGTH); Index++)
									{
										Tstuart.String[Index] = pPins[Index];
									}
									// TODO: tst pin shite
									//File = open(TEST_UART_DEVICE_NAME, O_RDWR | O_SYNC);
									//if (File >= MIN_HANDLE)
									//{
									//	ioctl(File, TST_UART_WRITE, &Tstuart);
									//	close(File);
									//}
									GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");
								}
							}
							break;

						case TST_READ_UART:
							{
								Tstuart.Port = *(DATA8*)PrimParPointer();
								Tstuart.Length = *(DATA8*)PrimParPointer();
								pPins = (DATA8*)PrimParPointer();

								for (Index = 0; (Index < Tstuart.Length) && (Index < TST_UART_LENGTH); Index++)
								{
									Tstuart.String[Index] = 0;
								}
								if (Tstuart.Port < INPUTS)
								{
									// TODO: tst pin shite
									//File = open(TEST_UART_DEVICE_NAME, O_RDWR | O_SYNC);
									//if (File >= MIN_HANDLE)
									//{
									//	ioctl(File, TST_UART_READ, &Tstuart);
									//	close(File);
									//}
									GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");
								}
								for (Index = 0; (Index < Tstuart.Length) && (Index < TST_UART_LENGTH); Index++)
								{
									pPins[Index] = Tstuart.String[Index];
								}
							}
							break;

						case TST_ACCU_SWITCH:
							{
								Data8 = 0;
								// TODO: accu shite
								//if (GH.UiInstance.PowerFile >= MIN_HANDLE)
								//{
								//	read(GH.UiInstance.PowerFile, Buffer, 2);
								//	if (Buffer[0] == '1')
								//	{
								//		Data8 = 1;
								//	}
								//}
								GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");
								*(DATA8*)PrimParPointer() = Data8;
							}
							break;

						case TST_BOOT_MODE2:
							{
								GH.I2c.I2cStart();
							}
							break;

						case TST_POLL_MODE2:
							{
								Data8 = (sbyte)GH.I2c.I2cGetBootStatus();
								*(DATA8*)PrimParPointer() = Data8;
							}
							break;

						case TST_CLOSE_MODE2:
							{
								GH.I2c.I2cStop();
							}
							break;

						case TST_RAM_CHECK:
							{
								ULONG RamCheckFile;
								// TODO: ram check
								//UBYTE RamStatus[2];

								//RamCheckFile = open(UPDATE_DEVICE_NAME, O_RDWR);
								Data8 = FAIL;
								//if (RamCheckFile >= 0)
								//{
								//	read(RamCheckFile, RamStatus, 2);
								//	close(RamCheckFile);

								//	if ((RamStatus[0] == ((UBYTE)(~(RamStatus[1])))) && (0 == RamStatus[0]))
								//	{
								//		Data8 = OK;
								//	}
								//}
								GH.Ev3System.Logger.LogWarning($"Usage of unimplemented shite in {Environment.StackTrace}");
								*(DATA8*)PrimParPointer() = Data8;
							}
							break;

						default:
							{ // Test close

								TstClose();
							}
							break;

					}
				}
				else
				{
					ProgramEnd(GH.VMInstance.ProgramId);
					GH.VMInstance.Program[GH.VMInstance.ProgramId].Result = RESULT.FAIL;
					SetDispatchStatus(DSPSTAT.INSTRBREAK);
				}
			}
		}
		
		public RESULT ValidateChar(DATA8* pChar, DATA8 Set)
		{
			RESULT Result = OK;

			var num = (byte)*pChar;
			if ((ValidChars[num] & Set) == 0)
			{
				*pChar = (sbyte)'_';
				Result = RESULT.FAIL;
			}

			return (Result);
		}


		public RESULT ValidateString(DATA8* pString, DATA8 Set)
		{
			RESULT Result = OK;

			while (*pString != 0)
			{
				Result |= ValidateChar(pString, Set);
				pString++;
			}

			return (Result);
		}
	}
}
