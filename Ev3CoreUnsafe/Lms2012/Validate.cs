using Ev3CoreUnsafe.Enums;
using Ev3CoreUnsafe.Helpers;
using Ev3CoreUnsafe.Lms2012.Interfaces;
using System.Runtime.CompilerServices;
using static Ev3CoreUnsafe.Defines;

namespace Ev3CoreUnsafe.Lms2012
{
	public unsafe class Validate : IValidate
	{
		public void ShowOpcode(OP OpCode, DATA8* Buf, int Lng)
		{
			ULONG Pars;
			UBYTE Sub;
			UBYTE Tab;
			UBYTE ParType;
			UBYTE Flag = 0;
			DATA8* TmpBuf = CommonHelper.Pointer1d<DATA8>(255);
			int Length;
			int Size;


			Buf[0] = 0;
			if ((long)OpCodes[OpCode].Name != 0)
			{ // Byte code exist

				Size = 0;
				Length = CommonHelper.snprintf(&Buf[Size], Lng - Size, $"  {CommonHelper.GetString(OpCodes[OpCode].Name)},");
				Size += Length;
				Size += CommonHelper.snprintf(&Buf[Size], Lng - Size, new string(' ', 3 + OPCODE_NAMESIZE - Length));

				// Get opcode parameter types
				Pars = OpCodes[OpCode].Pars;
				do
				{ // check for every parameter

					ParType = (byte)(Pars & 0x0F);
					if (ParType == SUBP)
					{ // Check existence of sub command

						Pars >>= 4;
						Tab = (byte)(Pars & 0x0F);

						for (Sub = 0; Sub < MAX_SUBCODES; Sub++)
						{
							Pars = 0;
							if (SubCodes[Tab][Sub].Name != null)
							{
								if (Flag == 0)
								{
									CommonHelper.snprintf(TmpBuf, 255, $"{CommonHelper.GetString(Buf)}");
								}
								else
								{
									Size += CommonHelper.snprintf(&Buf[Size], Lng - Size, "\r\n");
									Size += CommonHelper.snprintf(&Buf[Size], Lng - Size, $"{CommonHelper.GetString(TmpBuf)}");
								}
								Flag = 1;
								Size += CommonHelper.snprintf(&Buf[Size], Lng - Size, $"{CommonHelper.GetString(SubCodes[Tab][Sub].Name)}, ");
								Pars = SubCodes[Tab][Sub].Pars;
							}
							while ((Pars & 0x0F) >= PAR)
							{ // Check parameter

								if (((Pars >> 4) & 0x0F) != SUBP)
								{
									Size += CommonHelper.snprintf(&Buf[Size], Lng - Size, $"{ParTypeNames[(int)((Pars & 0x0F) - PAR)]}, ");
								}

								// Next parameter
								Pars >>= 4;
							}
						}
					}
					if (ParType == PARNO)
					{ // Check parameter

						if (((Pars >> 4) & 0x0F) != SUBP)
						{
							Size += CommonHelper.snprintf(&Buf[Size], Lng - Size, $"{ParTypeNames[DATA_8]}, ");
						}

						// Next parameter
						Pars >>= 4;
					}
					if (ParType == PARLAB)
					{ // Check parameter

						if (((Pars >> 4) & 0x0F) != SUBP)
						{
							Size += CommonHelper.snprintf(&Buf[Size], Lng - Size, $"{ParTypeNames[DATA_8]}, ");
						}

						// Next parameter
						Pars >>= 4;
					}
					if (ParType == PARVALUES)
					{ // Check parameter

						// Next parameter
						Pars >>= 4;
					}
					if (ParType >= PAR)
					{ // Check parameter

						if (((Pars >> 4) & 0x0F) != SUBP)
						{
							Size += CommonHelper.snprintf(&Buf[Size], Lng - Size, $"{ParTypeNames[(int)((Pars & 0x0F) - PAR)]}, ");
						}

						// Next parameter
						Pars >>= 4;
					}
				}
				while (ParType != 0);
				Size += CommonHelper.snprintf(&Buf[Size], Lng - Size, "\r\n");
			}
		}

		public RESULT cValidateInit()
		{
			RESULT Result = RESULT.FAIL;
			//# ifdef DEBUG
			//			FILE* pFile;
			//			UWORD OpCode;
			//			char Buffer[8000];

			//			pFile = fopen("../../../bytecodeassembler/o.c", "w");
			//			fprintf(pFile, "//******************************************************************************\r\n");
			//			fprintf(pFile, "//Test Supported Opcodes in V%4.2f\r\n", VERS);
			//			fprintf(pFile, "//******************************************************************************\r\n\n");
			//			fprintf(pFile, "#define DATA8          LC0(0)                   \r\n");
			//			fprintf(pFile, "#define DATA16         GV0(0)                   \r\n");
			//			fprintf(pFile, "#define DATA32         LV0(0)                   \r\n");
			//			fprintf(pFile, "#define DATAF          LC0(0)                   \r\n");
			//			fprintf(pFile, "#define UNKNOWN        LV0(0)                   \r\n");
			//			fprintf(pFile, "#define STRING         LCS,'T','E','S','T',0    \r\n");
			//			fprintf(pFile, "\r\n");
			//			fprintf(pFile, "UBYTE     prg[] =\r\n");
			//			fprintf(pFile, "{\r\n");
			//			fprintf(pFile, "  PROGRAMHeader(0,2,4),\r\n");
			//			fprintf(pFile, "  VMTHREADHeader(0,4),\r\n");
			//			fprintf(pFile, "  VMTHREADHeader(0,4),\r\n");
			//			fprintf(pFile, "\r\n");
			//			for (OpCode = 0; OpCode < 256; OpCode++)
			//			{
			//				if ((OpCode != opERROR) && (OpCode != opNOP))
			//				{
			//					ShowOpcode((UBYTE)OpCode, Buffer, 8000);
			//					fprintf(pFile, "%s", Buffer);
			//					if (OpCode == 0x60)
			//					{
			//						//        fprintf(pFile,"  0x60,\r\n");
			//					}
			//				}
			//			}
			//			ShowOpcode(opOBJECT_END, Buffer, 8000);
			//			fprintf(pFile, "%s", Buffer);
			//			fprintf(pFile, "\r\n");
			//			fprintf(pFile, "};\r\n\n");
			//			fprintf(pFile, "//******************************************************************************\r\n");
			//			fclose(pFile);

			//			if (system("~/projects/lms2012/bytecodeassembler/oasm") >= 0)
			//			{
			//				GH.printf("Compiling\r\n");
			//				sync();
			//			}


			//#endif
			Result = OK;

			return (Result);
		}


		public RESULT cValidateExit()
		{
			RESULT Result = RESULT.FAIL;

			Result = OK;

			return (Result);
		}


		public void cValidateSetErrorIndex(IMINDEX Index)
		{
			if (Index == 0)
			{
				GH.ValidateInstance.ValidateErrorIndex = 0;
			}
			if (GH.ValidateInstance.ValidateErrorIndex == 0)
			{
				GH.ValidateInstance.ValidateErrorIndex = Index;
			}
		}


		public IMINDEX cValidateGetErrorIndex()
		{
			return (GH.ValidateInstance.ValidateErrorIndex);
		}


		public RESULT cValidateDisassemble(IP pI, IMINDEX* pIndex, LABEL* pLabel)
		{
			RESULT Result = RESULT.FAIL;  // Current status
			OP OpCode;         // Current opcode
			ULONG Pars;           // Current parameter types
			UBYTE ParType;        // Current parameter type
			UBYTE Sub = 0;            // Current sub code if any
			UBYTE Tab;            // Sub code table index
			ULONG Value;
			UBYTE ParCode = 0;
			void* pParValue;
			DATA8 Parameters;
			DATA32 Bytes = 0;
			int Indent;
			int LineLength;

			// Check for validation error
			if ((*pIndex) == cValidateGetErrorIndex())
			{
				GH.printf("ERROR ");
			}

			// Get opcode
			OpCode = (OP)(pI[*pIndex] & 0xFF);

			// Check if opcode exists
			if ((long)OpCodes[OpCode].Name != 0)
			{ // Byte code exists

				Parameters = 0;

				// Print opcode
				LineLength = GH.printf($"  /* {*pIndex} */  {CommonHelper.GetString(OpCodes[OpCode].Name)},");
				Indent = LineLength;

				(*pIndex)++;

				// Check if object ends or error
				if ((OpCode == OP.opOBJECT_END) || (OpCode == opERROR))
				{
					Result = RESULT.STOP;
				}
				else
				{
					Result = OK;

					// Get opcode parameter types
					Pars = OpCodes[OpCode].Pars;
					do
					{ // check for every parameter

						// Isolate current parameter type
						ParType = (byte)(Pars & 0x0F);

						if (ParType == SUBP)
						{ // Prior plain parameter was a sub code

							// Get sub code from last plain parameter
							// Sub = *(DATA8*)pParValue; // TODO: wtf uninit used

							// Isolate next parameter type
							Pars >>= 4;
							Tab = (byte)(Pars & 0x0F);
							Pars = 0;

							// Check if sub code in right range
							if (Sub < MAX_SUBCODES)
							{ // Sub code range ok

								// Check if sub code exists
								if (SubCodes[Tab][Sub].Name != null)
								{ // Ok

									if ((ParCode & PRIMPAR_LONG) != 0)
									{ // long format

										if ((ParCode & PRIMPAR_BYTES) == PRIMPAR_1_BYTE)
										{
											LineLength += GH.printf($"LC1({CommonHelper.GetString(SubCodes[Tab][Sub].Name)}),");
										}
										if ((ParCode & PRIMPAR_BYTES) == PRIMPAR_2_BYTES)
										{
											LineLength += GH.printf($"LC2({CommonHelper.GetString(SubCodes[Tab][Sub].Name)}),");
										}
										if ((ParCode & PRIMPAR_BYTES) == PRIMPAR_4_BYTES)
										{
											LineLength += GH.printf($"LC4({CommonHelper.GetString(SubCodes[Tab][Sub].Name)}),");
										}
									}
									else
									{
										LineLength += GH.printf($"LC0({CommonHelper.GetString(SubCodes[Tab][Sub].Name)}),");
									}

									// Get sub code parameter list
									Pars = SubCodes[Tab][Sub].Pars;
								}
							}
						}

						if (ParType == PARVALUES)
						{
							// Bytes = *(DATA32*)pParValue; // TODO: wtf uninit used
							// Next parameter
							Pars >>= 4;
							Pars &= 0x0F;

							while (Bytes != 0)
							{
								//***************************************************************************
								if ((LineLength + 10) >= 80)
								{
									GH.printf($"\r\n{new string(' ', Indent)}");
									LineLength = Indent;
								}
								Value = (ULONG)0;
								pParValue = (void*)&Value;
								ParCode = (byte)((UBYTE)pI[(*pIndex)++] & 0xFF);

								// Calculate parameter value

								if ((ParCode & PRIMPAR_LONG) != 0)
								{ // long format

									if ((ParCode & PRIMPAR_HANDLE) != 0)
									{
										LineLength += GH.printf("HND(");
									}
									else
									{
										if ((ParCode & PRIMPAR_ADDR) != 0)
										{
											LineLength += GH.printf("ADR(");
										}
									}
									if ((ParCode & PRIMPAR_VARIABEL) != 0)
									{ // variabel

										if ((ParCode & PRIMPAR_GLOBAL) != 0)
										{ // global

											LineLength += GH.printf("GV");
										}
										else
										{ // local

											LineLength += GH.printf("LV");
										}
										switch (ParCode & PRIMPAR_BYTES)
										{
											case PRIMPAR_1_BYTE:
												{
													Value = (ULONG)pI[(*pIndex)++];

													LineLength += GH.printf($"1({(int)Value})");
												}
												break;

											case PRIMPAR_2_BYTES:
												{
													Value = (ULONG)pI[(*pIndex)++];
													Value |= ((ULONG)pI[(*pIndex)++] << 8);

													LineLength += GH.printf($"2({(int)Value})");
												}
												break;

											case PRIMPAR_4_BYTES:
												{
													Value = (ULONG)pI[(*pIndex)++];
													Value |= ((ULONG)pI[(*pIndex)++] << 8);
													Value |= ((ULONG)pI[(*pIndex)++] << 16);
													Value |= ((ULONG)pI[(*pIndex)++] << 24);

													LineLength += GH.printf($"4({(int)Value})");
												}
												break;

										}
									}
									else
									{ // constant

										if ((ParCode & PRIMPAR_HANDLE) != 0)
										{
											LineLength += GH.printf("HND(");
										}
										else
										{
											if ((ParCode & PRIMPAR_ADDR) != 0)
											{
												LineLength += GH.printf("ADR(");
											}
										}
										if ((ParCode & PRIMPAR_LABEL) != 0)
										{ // label

											Value = (ULONG)pI[(*pIndex)++];
											if ((Value & 0x00000080) != 0)
											{ // Adjust if negative

												Value |= 0xFFFFFF00;
											}

											LineLength += GH.printf($"LAB1({(int)(Value & 0xFF)})");
										}
										else
										{ // value

											switch (ParCode & PRIMPAR_BYTES)
											{
												case PRIMPAR_STRING_OLD:
												case PRIMPAR_STRING:
													{

														LineLength += GH.printf("LCS,");

														while (pI[(*pIndex)] != 0)
														{ // Adjust Ip

															if ((LineLength + 5) >= 80)
															{
																GH.printf($"\r\n{new string(' ', Indent)}");
																LineLength = Indent;
															}

															switch (pI[(*pIndex)])
															{
																case (byte)'\r':
																	{
																		LineLength += GH.printf("'\\r',");
																	}
																	break;

																case (byte)'\n':
																	{
																		LineLength += GH.printf("'\\n',");
																	}
																	break;

																default:
																	{
																		LineLength += GH.printf($"'{(char)pI[(*pIndex)]}',");
																	}
																	break;

															}

														  (*pIndex)++;
														}
												  (*pIndex)++;

														if ((LineLength + 2) >= 80)
														{
															GH.printf($"\r\n{new string(' ', Indent)}");
															LineLength = Indent;
														}
														LineLength += GH.printf("0");
													}
													break;

												case PRIMPAR_1_BYTE:
													{
														Value = (ULONG)pI[(*pIndex)++];
														if ((Value & 0x00000080) != 0)
														{ // Adjust if negative

															Value |= 0xFFFFFF00;
														}
														if ((Pars & 0x0f) != SUBP)
														{
															LineLength += GH.printf($"LC1({(int)Value})");
														}
													}
													break;

												case PRIMPAR_2_BYTES:
													{
														Value = (ULONG)pI[(*pIndex)++];
														Value |= ((ULONG)pI[(*pIndex)++] << 8);
														if ((Value & 0x00008000) != 0)
														{ // Adjust if negative

															Value |= 0xFFFF0000;
														}
														if ((Pars & 0x0f) != SUBP)
														{
															LineLength += GH.printf($"LC2({(int)Value})");
														}
													}
													break;

												case PRIMPAR_4_BYTES:
													{
														Value = (ULONG)pI[(*pIndex)++];
														Value |= ((ULONG)pI[(*pIndex)++] << 8);
														Value |= ((ULONG)pI[(*pIndex)++] << 16);
														Value |= ((ULONG)pI[(*pIndex)++] << 24);
														if ((Pars & 0x0f) != SUBP)
														{
															LineLength += GH.printf($"LC4({(long)Value})");
														}
													}
													break;

											}
										}
									}
									if ((ParCode & PRIMPAR_HANDLE) != 0)
									{
										LineLength += GH.printf("),");
									}
									else
									{
										if ((ParCode & PRIMPAR_ADDR) != 0)
										{
											LineLength += GH.printf("),");
										}
										else
										{
											LineLength += GH.printf(",");
										}
									}
								}
								else
								{ // short format

									if ((ParCode & PRIMPAR_VARIABEL) != 0)
									{ // variabel

										if ((ParCode & PRIMPAR_GLOBAL) != 0)
										{ // global

											LineLength += GH.printf($"GV0({(uint)(ParCode & PRIMPAR_INDEX)})");
										}
										else
										{ // local

											LineLength += GH.printf($"LV0({(uint)(ParCode & PRIMPAR_INDEX)})");
										}
									}
									else
									{ // constant

										Value = (ULONG)(ParCode & PRIMPAR_VALUE);

										if ((ParCode & PRIMPAR_CONST_SIGN) != 0)
										{ // Adjust if negative

											Value |= ~(ULONG)(PRIMPAR_VALUE);
										}
										LineLength += GH.printf($"LC0({(int)Value})");

									}
									LineLength += GH.printf(",");
								}
								if (ParType == PARNO)
								{
									if ((ParCode & PRIMPAR_VARIABEL) == 0)
									{
										Parameters = (DATA8)(*(DATA32*)pParValue);
									}
								}
								if (ParType == PARLAB)
								{
									if ((ParCode & PRIMPAR_VARIABEL) == 0)
									{
									}
								}
								//***************************************************************************
								Bytes--;
							}
							Pars = 0;
						}

						if ((ParType >= PAR) || (ParType == PARNO) || (ParType == PARLAB))
						{ // Plain parameter

							if ((LineLength + 10) >= 80)
							{
								GH.printf($"\r\n{new string(' ', Indent)}"); 
								LineLength = Indent;
							}
							Value = (ULONG)0;
							pParValue = (void*)&Value;
							ParCode = (UBYTE)(pI[(*pIndex)++] & 0xFF);

							// Next parameter
							Pars >>= 4;

							// Calculate parameter value

							if ((ParCode & PRIMPAR_LONG) != 0)
							{ // long format

								if ((ParCode & PRIMPAR_HANDLE) != 0)
								{
									LineLength += GH.printf("HND(");
								}
								else
								{
									if ((ParCode & PRIMPAR_ADDR) != 0)
									{
										LineLength += GH.printf("ADR(");
									}
								}
								if ((ParCode & PRIMPAR_VARIABEL) != 0)
								{ // variabel

									if ((ParCode & PRIMPAR_GLOBAL) != 0)
									{ // global

										LineLength += GH.printf("GV");
									}
									else
									{ // local

										LineLength += GH.printf("LV");
									}
									switch (ParCode & PRIMPAR_BYTES)
									{
										case PRIMPAR_1_BYTE:
											{
												Value = (ULONG)pI[(*pIndex)++];

												LineLength += GH.printf($"1({(int)Value})");
											}
											break;

										case PRIMPAR_2_BYTES:
											{
												Value = (ULONG)pI[(*pIndex)++];
												Value |= ((ULONG)pI[(*pIndex)++] << 8);

												LineLength += GH.printf($"2({(int)Value})");
											}
											break;

										case PRIMPAR_4_BYTES:
											{
												Value = (ULONG)pI[(*pIndex)++];
												Value |= ((ULONG)pI[(*pIndex)++] << 8);
												Value |= ((ULONG)pI[(*pIndex)++] << 16);
												Value |= ((ULONG)pI[(*pIndex)++] << 24);

												LineLength += GH.printf($"4({(int)Value})");
											}
											break;

									}
								}
								else
								{ // constant

									if ((ParCode & PRIMPAR_HANDLE) != 0)
									{
										LineLength += GH.printf("HND(");
									}
									else
									{
										if ((ParCode & PRIMPAR_ADDR) != 0)
										{
											LineLength += GH.printf("ADR(");
										}
									}
									if ((ParCode & PRIMPAR_LABEL) != 0)
									{ // label

										Value = (ULONG)pI[(*pIndex)++];
										if ((Value & 0x00000080) != 0)
										{ // Adjust if negative

											Value |= 0xFFFFFF00;
										}

										LineLength += GH.printf($"LAB1({(int)(Value & 0xFF)})");
									}
									else
									{ // value

										switch (ParCode & PRIMPAR_BYTES)
										{
											case PRIMPAR_STRING_OLD:
											case PRIMPAR_STRING:
												{

													LineLength += GH.printf("LCS,");

													while (pI[(*pIndex)] != 0)
													{ // Adjust Ip

														if ((LineLength + 5) >= 80)
														{
															GH.printf($"\r\n{new string(' ', Indent)}"); 
															LineLength = Indent;
														}

														switch (pI[(*pIndex)])
														{
															case (byte)'\r':
																{
																	LineLength += GH.printf("'\\r',");
																}
																break;

															case (byte)'\n':
																{
																	LineLength += GH.printf("'\\n',");
																}
																break;

															default:
																{
																	LineLength += GH.printf($"'{(char)pI[(*pIndex)]}',");
																}
																break;

														}

													  (*pIndex)++;
													}
													(*pIndex)++;

													if ((LineLength + 2) >= 80)
													{
														GH.printf($"\r\n{new string(' ', Indent)}"); 
														LineLength = Indent;
													}
													LineLength += GH.printf("0");
												}
												break;

											case PRIMPAR_1_BYTE:
												{
													Value = (ULONG)pI[(*pIndex)++];
													if ((Value & 0x00000080) != 0)
													{ // Adjust if negative

														Value |= 0xFFFFFF00;
													}
													if ((Pars & 0x0f) != SUBP)
													{
														LineLength += GH.printf($"LC1({(int)Value})");
													}
												}
												break;

											case PRIMPAR_2_BYTES:
												{
													Value = (ULONG)pI[(*pIndex)++];
													Value |= ((ULONG)pI[(*pIndex)++] << 8);
													if ((Value & 0x00008000) != 0)
													{ // Adjust if negative

														Value |= 0xFFFF0000;
													}
													if ((Pars & 0x0f) != SUBP)
													{
														LineLength += GH.printf($"LC2({(int)Value})");
													}
												}
												break;

											case PRIMPAR_4_BYTES:
												{
													Value = (ULONG)pI[(*pIndex)++];
													Value |= ((ULONG)pI[(*pIndex)++] << 8);
													Value |= ((ULONG)pI[(*pIndex)++] << 16);
													Value |= ((ULONG)pI[(*pIndex)++] << 24);
													if ((Pars & 0x0f) != SUBP)
													{
														LineLength += GH.printf($"LC4({(long)Value})");
													}
												}
												break;

										}
									}
								}
								if ((ParCode & PRIMPAR_HANDLE) != 0)
								{
									LineLength += GH.printf("),");
								}
								else
								{
									if ((ParCode & PRIMPAR_ADDR) != 0)
									{
										LineLength += GH.printf("),");
									}
									else
									{
										LineLength += GH.printf(",");
									}
								}
							}
							else
							{ // short format

								if ((ParCode & PRIMPAR_VARIABEL) != 0)
								{ // variabel

									if ((ParCode & PRIMPAR_GLOBAL) != 0)
									{ // global

										LineLength += GH.printf($"GV0({(uint)(ParCode & PRIMPAR_INDEX)})");
									}
									else
									{ // local

										LineLength += GH.printf($"LV0({(uint)(ParCode & PRIMPAR_INDEX)})");
									}
								}
								else
								{ // constant

									Value = (ULONG)(ParCode & PRIMPAR_VALUE);

									if ((ParCode & PRIMPAR_CONST_SIGN) != 0)
									{ // Adjust if negative

										Value |= ~(ULONG)(PRIMPAR_VALUE);
									}
									if ((Pars & 0x0f) != SUBP)
									{
										LineLength += GH.printf($"LC0({(int)Value})");
									}

								}
								if ((Pars & 0x0f) != SUBP)
								{
									LineLength += GH.printf(",");
								}
							}
							if (ParType == PARNO)
							{
								if ((ParCode & PRIMPAR_VARIABEL) == 0)
								{
									Parameters = (DATA8)(*(DATA32*)pParValue);
								}
							}
							if (ParType == PARLAB)
							{
								if ((ParCode & PRIMPAR_VARIABEL) == 0)
								{


								}
							}
							if (Parameters != 0)
							{
								Parameters--;
								Pars = PAR32;
							}

						}
					}
					while (ParType != 0 || Parameters != 0);
				}
				GH.printf("\r\n");
			}

			return (Result);
		}


		public RESULT cValidateDisassembleProgram(PRGID PrgId, IP pI, LABEL* pLabel)
		{
			RESULT Result = OK;
			IMINDEX Size;
			OBJID ObjIndex;
			IMINDEX MinIndex;
			IMINDEX MaxIndex;
			IMINDEX Index;
			IMINDEX Addr;
			ULONG LastOffset;
			OBJID LastObject;
			UBYTE Stop;
			IMINDEX TmpIndex;
			UBYTE Type;
			DATA32 Lng;

			IMGHEAD* pIH;
			OBJHEAD* pOH;
			OBJID Objects;

			pIH = (IMGHEAD*)pI;
			pOH = (OBJHEAD*)&pI[sizeof(IMGHEAD) - sizeof(OBJHEAD)];
			Objects = (*(IMGHEAD*)pI).NumberOfObjects;
			Size = (*(IMGHEAD*)pI).ImageSize;

			GH.printf("\r\n//****************************************************************************");
			GH.printf("\r\n// Disassembler Listing");
			GH.printf("\r\n//****************************************************************************");
			GH.printf("\r\n\nUBYTE     prg[] =\r\n{\r\n");

			Addr = 0;
			LastOffset = 0;
			LastObject = 0;
			// Check for validation error
			if ((cValidateGetErrorIndex() != 0) && (cValidateGetErrorIndex() < Unsafe.SizeOf<IMGHEAD>()))
			{
				GH.printf("ERROR ");
			}
			GH.printf($"  /* {Addr} */  PROGRAMHeader({(float)(*pIH).VersionInfo / (float)100},{(*pIH).NumberOfObjects},{(*pIH).GlobalBytes}),\r\n");
			Addr += (uint)Unsafe.SizeOf<IMGHEAD>();

			for (ObjIndex = 1; ObjIndex <= Objects; ObjIndex++)
			{
				if ((cValidateGetErrorIndex() >= Addr) && (cValidateGetErrorIndex() < (Addr + sizeof(OBJHEAD))))
				{
					GH.printf("ERROR ");
				}
				if (pOH[ObjIndex].OwnerObjectId != 0)
				{ // BLOCK object

					GH.ValidateInstance.Row = GH.printf($"  /* {Addr} */  BLOCKHeader({(ULONG)pOH[ObjIndex].OffsetToInstructions},{pOH[ObjIndex].OwnerObjectId},{pOH[ObjIndex].TriggerCount}),");
				}
				else
				{
					if (pOH[ObjIndex].TriggerCount == 1)
					{ // SUBCALL object

						if (LastOffset == (ULONG)pOH[ObjIndex].OffsetToInstructions)
						{ // SUBCALL alias object

							GH.ValidateInstance.Row = GH.printf($"  /* {Addr} */  SUBCALLHeader({(ULONG)pOH[ObjIndex].OffsetToInstructions},{pOH[ObjIndex].LocalBytes}),");
						}
						else
						{
							GH.ValidateInstance.Row = GH.printf($"  /* {Addr} */  SUBCALLHeader({(ULONG)pOH[ObjIndex].OffsetToInstructions},{pOH[ObjIndex].LocalBytes}),");
						}
					}
					else
					{ // VMTHREAD object

						GH.ValidateInstance.Row = GH.printf($"  /* {Addr} */  VMTHREADHeader({(ULONG)pOH[ObjIndex].OffsetToInstructions},{pOH[ObjIndex].LocalBytes}),");
					}
				}
				GH.ValidateInstance.Row = 41 - GH.ValidateInstance.Row;
				if (LastOffset == (ULONG)pOH[ObjIndex].OffsetToInstructions)
				{
					GH.printf($"// Object {ObjIndex} (Alias for object {LastObject})\r\n");
				}
				else
				{
					GH.printf($"// Object {ObjIndex}\r\n");
				}
				GH.ValidateInstance.Row = 0;
				LastOffset = (ULONG)pOH[ObjIndex].OffsetToInstructions;
				LastObject = ObjIndex;
				Addr += (uint)Unsafe.SizeOf<OBJHEAD>();
			}

			for (ObjIndex = 1; ObjIndex <= Objects; ObjIndex++)
			{ // for every object - find minimum and maximum instruction pointer values

				MinIndex = (IMINDEX)pOH[ObjIndex].OffsetToInstructions;

				if (ObjIndex == Objects)
				{
					MaxIndex = Size;
				}
				else
				{
					MaxIndex = (IMINDEX)pOH[ObjIndex + 1].OffsetToInstructions;
				}

				if (pOH[ObjIndex].OwnerObjectId != 0)
				{ // BLOCK object

					GH.printf($"\r\n  /* Object {ObjIndex} (BLOCK) [{(ulong)MinIndex}..{(ulong)MaxIndex}] */\r\n\n");
				}
				else
				{
					if (pOH[ObjIndex].TriggerCount == 1)
					{ // SUBCALL object

						GH.printf($"\r\n  /* Object {ObjIndex} (SUBCALL) [{(ulong)MinIndex}..{(ulong)MaxIndex}] */\r\n\n");

						GH.ValidateInstance.Row += GH.printf($"  /* {MinIndex} */  ");
						TmpIndex = (IMINDEX)pI[MinIndex++];
						GH.printf($"{TmpIndex},");
						while (TmpIndex != 0)
						{
							Type = pI[MinIndex++];
							if ((Type & CALLPAR_IN) != 0)
							{
								GH.printf("IN_");
							}
							if ((Type & CALLPAR_OUT) != 0)
							{
								GH.printf("OUT_");
							}
							switch (Type & CALLPAR_TYPE)
							{
								case CALLPAR_DATA8:
									{
										GH.printf("8,");
									}
									break;

								case CALLPAR_DATA16:
									{
										GH.printf("16,");
									}
									break;

								case CALLPAR_DATA32:
									{
										GH.printf("32,");
									}
									break;

								case CALLPAR_DATAF:
									{
										GH.printf("F,");
									}
									break;

								case CALLPAR_STRING:
									{
										Lng = (DATA32)pI[MinIndex++];
										GH.printf($"({Lng}),");
									}
									break;

							}
							TmpIndex--;
						}
						GH.printf("\r\n\n");

					}

					else
					{ // VMTHREAD object

						GH.printf($"\r\n  /* Object {ObjIndex} (VMTHREAD) [{(ulong)MinIndex}..{(ulong)MaxIndex}] */\r\n\n");
					}
				}

				Stop = OK;
				GH.ValidateInstance.Row = 0;

				for (Index = MinIndex; ((MaxIndex == 0) || (Index < MaxIndex)) && (Stop == OK);)
				{ // for every opcode - find number of parameters

					Stop = (byte)cValidateDisassemble(pI, &Index, pLabel);
				}

				Addr = Index;

			}
			GH.printf("};\r\n");
			GH.printf("\r\n//****************************************************************************\r\n\n");


			return (Result);
		}

		public RESULT cValidateCheckAlignment(ULONG Value, DATA8 Type)
		{
			RESULT Result = OK;

			switch (Type)
			{
				case PAR16:
					{
						if ((Value & 1) != 0)
						{
							Result = RESULT.FAIL;
						}
					}
					break;

				case PAR32:
					{
						if ((Value & 3) != 0)
						{
							Result = RESULT.FAIL;
						}
					}
					break;

				case PARF:
					{
						if ((Value & 3) != 0)
						{
							Result = RESULT.FAIL;
						}
					}
					break;

				default:
					{
						Result = OK;
					}
					break;

			}

			return (Result);
		}


		public RESULT cValidateBytecode(IP pI, IMINDEX* pIndex, LABEL* pLabel)
		{
			RESULT Result = RESULT.FAIL;
			RESULT Aligned = OK;
			OP OpCode;
			ULONG Pars;
			UBYTE Sub = 0;
			IMGDATA Tab;
			ULONG Value;
			UBYTE ParType = PAR;
			UBYTE ParCode;
			void* pParValue;
			DATA8 Parameters;
			DATA8 ParNo;
			DATA32 Bytes = 0;

			OpCode = (OP)(pI[*pIndex] & 0xFF);

			if ((long)OpCodes[OpCode].Name != 0)
			{ // Byte code exist

				Parameters = 0;
				ParNo = 0;

				(*pIndex)++;

				if ((OpCode == opERROR) || (OpCode == OP.opOBJECT_END))
				{
					if (OpCode == OP.opOBJECT_END)
					{
						Result = RESULT.STOP;
					}
				}
				else
				{
					Result = RESULT.FAIL;

					if (OpCode == OP.opJR_TRUE)
					{
						Bytes = 0;
					}
					// Get opcode parameter types
					Pars = OpCodes[OpCode].Pars;
					do
					{ // check for every parameter

						ParType = (byte)(Pars & 0x0F);
						if (ParType == 0)
						{
							Result = OK;
						}

						if (ParType == SUBP)
						{ // Check existence of sub command

							// Sub = *(DATA8*)pParValue; // TODO: wtf uninit usage
							Pars >>= 4;
							Tab = (byte)(Pars & 0x0F);
							Pars = 0;

							if (Sub < MAX_SUBCODES)
							{
								if (SubCodes[Tab][Sub].Name != null)
								{
									Pars = SubCodes[Tab][Sub].Pars;
									Result = OK;
								}
							}
						}

						if (ParType == PARVALUES)
						{ // Last parameter tells number of bytes to follow

							// Bytes = *(DATA32*)pParValue; // TODO: wtf uninit usage

							Pars >>= 4;
							Pars &= 0x0F;
							//***************************************************************************

							while (Bytes != 0)
							{
								Value = (ULONG)0;
								pParValue = (void*)&Value;
								ParCode = (UBYTE)(pI[(*pIndex)++] & 0xFF);
								Aligned = OK;

								// Calculate parameter value
								if ((ParCode & PRIMPAR_LONG) != 0)
								{ // long format

									if ((ParCode & PRIMPAR_VARIABEL) != 0)
									{ // variabel

										switch (ParCode & PRIMPAR_BYTES)
										{
											case PRIMPAR_1_BYTE:
												{
													Value = (ULONG)pI[(*pIndex)++];
												}
												break;

											case PRIMPAR_2_BYTES:
												{
													Value = (ULONG)pI[(*pIndex)++];
													Value |= ((ULONG)pI[(*pIndex)++] << 8);
												}
												break;

											case PRIMPAR_4_BYTES:
												{
													Value = (ULONG)pI[(*pIndex)++];
													Value |= ((ULONG)pI[(*pIndex)++] << 8);
													Value |= ((ULONG)pI[(*pIndex)++] << 16);
													Value |= ((ULONG)pI[(*pIndex)++] << 24);
												}
												break;

										}
									}
									else
									{ // constant

										if ((ParCode & PRIMPAR_LABEL) != 0)
										{ // label

											Value = (ULONG)pI[(*pIndex)++];
											if ((Value & 0x00000080) != 0)
											{ // Adjust if negative

												Value |= 0xFFFFFF00;
											}
										}
										else
										{ // value

											switch (ParCode & PRIMPAR_BYTES)
											{
												case PRIMPAR_STRING_OLD:
												case PRIMPAR_STRING:
													{
														while (pI[(*pIndex)] != 0)
														{ // Adjust Ip
															(*pIndex)++;
														}
												  (*pIndex)++;
													}
													break;

												case PRIMPAR_1_BYTE:
													{
														Value = (ULONG)pI[(*pIndex)++];
														if ((Value & 0x00000080) != 0)
														{ // Adjust if negative

															Value |= 0xFFFFFF00;
														}
													}
													break;

												case PRIMPAR_2_BYTES:
													{
														Value = (ULONG)pI[(*pIndex)++];
														Value |= ((ULONG)pI[(*pIndex)++] << 8);
														if ((Value & 0x00008000) != 0)
														{ // Adjust if negative

															Value |= 0xFFFF0000;
														}
													}
													break;

												case PRIMPAR_4_BYTES:
													{
														Value = (ULONG)pI[(*pIndex)++];
														Value |= ((ULONG)pI[(*pIndex)++] << 8);
														Value |= ((ULONG)pI[(*pIndex)++] << 16);
														Value |= ((ULONG)pI[(*pIndex)++] << 24);
													}
													break;

											}
										}
									}
								}
								else
								{ // short format

									if ((ParCode & PRIMPAR_VARIABEL) != 0)
									{ // variabel

										Value = (ULONG)(ParCode & PRIMPAR_VALUE);
									}
									else
									{ // constant

										Value = (ULONG)(ParCode & PRIMPAR_VALUE);

										if ((ParCode & PRIMPAR_CONST_SIGN) != 0)
										{ // Adjust if negative

											Value |= ~(ULONG)(PRIMPAR_VALUE);
										}
									}
								}

								// Check parameter value
								if ((Pars >= PAR8) && (Pars <= PAR32))
								{
									if (((*(DATA32*)pParValue) >= ParMin[ParType - PAR]) && ((*(DATA32*)pParValue) <= ParMax[ParType - PAR]))
									{
										Result = OK;
									}
								}
								if ((Pars == PARF))
								{
									if ((*(DATAF*)pParValue >= DATAF_MIN) && (*(DATAF*)pParValue <= DATAF_MAX))
									{
										Result = OK;
									}
								}
								if (Pars == PARV)
								{
									Result = OK;
								}
								Bytes--;
							}
							//***************************************************************************
							Pars = 0;
						}

						if ((ParType >= PAR) || (ParType == PARNO) || (ParType == PARLAB))
						{ // Check parameter

							Value = (ULONG)0;
							pParValue = (void*)&Value;
							ParCode = (UBYTE)(pI[(*pIndex)++] & 0xFF);
							Aligned = OK;

							// Calculate parameter value



							if ((ParCode & PRIMPAR_LONG) != 0)
							{ // long format

								if ((ParCode & PRIMPAR_VARIABEL) != 0)
								{ // variabel

									switch (ParCode & PRIMPAR_BYTES)
									{
										case PRIMPAR_1_BYTE:
											{
												Value = (ULONG)pI[(*pIndex)++];
											}
											break;

										case PRIMPAR_2_BYTES:
											{
												Value = (ULONG)pI[(*pIndex)++];
												Value |= ((ULONG)pI[(*pIndex)++] << 8);
											}
											break;

										case PRIMPAR_4_BYTES:
											{
												Value = (ULONG)pI[(*pIndex)++];
												Value |= ((ULONG)pI[(*pIndex)++] << 8);
												Value |= ((ULONG)pI[(*pIndex)++] << 16);
												Value |= ((ULONG)pI[(*pIndex)++] << 24);
											}
											break;

									}
								}
								else
								{ // constant

									if ((ParCode & PRIMPAR_LABEL) != 0)
									{ // label

										Value = (ULONG)pI[(*pIndex)++];
										if ((Value & 0x00000080) != 0)
										{ // Adjust if negative

											Value |= 0xFFFFFF00;
										}
									}
									else
									{ // value

										switch (ParCode & PRIMPAR_BYTES)
										{
											case PRIMPAR_STRING_OLD:
											case PRIMPAR_STRING:
												{
													while (pI[(*pIndex)] != 0)
													{ // Adjust Ip
														(*pIndex)++;
													}
											  (*pIndex)++;
												}
												break;

											case PRIMPAR_1_BYTE:
												{
													Value = (ULONG)pI[(*pIndex)++];
													if ((Value & 0x00000080) != 0)
													{ // Adjust if negative

														Value |= 0xFFFFFF00;
													}
												}
												break;

											case PRIMPAR_2_BYTES:
												{
													Value = (ULONG)pI[(*pIndex)++];
													Value |= ((ULONG)pI[(*pIndex)++] << 8);
													if ((Value & 0x00008000) != 0)
													{ // Adjust if negative

														Value |= 0xFFFF0000;
													}
												}
												break;

											case PRIMPAR_4_BYTES:
												{
													Value = (ULONG)pI[(*pIndex)++];
													Value |= ((ULONG)pI[(*pIndex)++] << 8);
													Value |= ((ULONG)pI[(*pIndex)++] << 16);
													Value |= ((ULONG)pI[(*pIndex)++] << 24);
												}
												break;

										}
									}
								}
							}
							else
							{ // short format

								if ((ParCode & PRIMPAR_VARIABEL) != 0)
								{ // variabel

									Value = (ULONG)(ParCode & PRIMPAR_VALUE);
								}
								else
								{ // constant

									Value = (ULONG)(ParCode & PRIMPAR_VALUE);

									if ((ParCode & PRIMPAR_CONST_SIGN) != 0)
									{ // Adjust if negative

										Value |= ~(ULONG)(PRIMPAR_VALUE);
									}
								}
							}

							if ((ParCode & PRIMPAR_VARIABEL) != 0)
							{
								Result = OK;
							}
							else
							{ // Check constant parameter value

								if ((ParType >= PAR8) && (ParType <= PAR32))
								{
									if (((*(DATA32*)pParValue) >= ParMin[ParType - PAR]) && ((*(DATA32*)pParValue) <= ParMax[ParType - PAR]))
									{
										Result = OK;
									}
								}
								if ((ParType == PARF))
								{
									Result = OK;
								}
							}
							if (ParType == PARV)
							{
								Result = OK;
							}
							if (ParType == PARNO)
							{ // Check number of parameters

								ParNo = 1;
								if ((ParCode & PRIMPAR_VARIABEL) == 0)
								{ // Must be constant

									if (((*(DATA32*)pParValue) >= 0) && ((*(DATA32*)pParValue) <= DATA8_MAX))
									{ // Must be positive and than less than or equal to 127

										Parameters = (DATA8)(*(DATA32*)pParValue);
										Result = OK;
									}
								}
							}
							if (ParType == PARLAB)
							{ // Check number of parameters

								if ((ParCode & PRIMPAR_VARIABEL) == 0)
								{ // Must be constant

									if (((*(DATA32*)pParValue) >= 0) && ((*(DATA32*)pParValue) < MAX_LABELS))
									{
										if (pLabel != null)
										{
											pLabel[Value].Addr = *pIndex;
										}

										Result = OK;
									}
								}
							}

							// Check allocation
							if (ParNo == 0)
							{
								if ((ParCode & PRIMPAR_LONG) != 0)
								{ // long format

									if ((ParCode & PRIMPAR_VARIABEL) != 0)
									{ // variabel

										if ((ParCode & PRIMPAR_HANDLE) != 0)
										{ // handle

											Aligned = cValidateCheckAlignment(Value, PAR16);
										}
										else
										{

											if ((ParCode & PRIMPAR_GLOBAL) != 0)
											{ // global
											}
											else
											{ // local
											}
											Aligned = cValidateCheckAlignment(Value, (sbyte)ParType);
										}
									}
									else
									{ // constant

										if ((ParCode & PRIMPAR_LABEL) != 0)
										{ // label

										}
										else
										{ // value

										}
									}
								}
								else
								{ // short format

									if ((ParCode & PRIMPAR_VARIABEL) != 0)
									{ // variabel

										if ((ParCode & PRIMPAR_GLOBAL) != 0)
										{ // global
										}
										else
										{ // local
										}
										Aligned = cValidateCheckAlignment(Value, (sbyte)ParType);
									}
								}
							}

							// Next parameter
							Pars >>= 4;
							if (Parameters != 0)
							{
								Parameters--;
								Pars = PAR32;
							}

						}
						if (Aligned != OK)
						{
							Result = RESULT.FAIL;
						}
					}
					while ((ParType != 0) && (Result == OK));
				}
			}

			return (Result);
		}

		public RESULT cValidateProgram(PRGID PrgId, IP pI, LABEL* pLabel, DATA8 Disassemble)
		{
			RESULT Result;
			IMGHEAD* pIH;             // Pointer to image header
			IMINDEX TotalSize;        // Total image size
			OBJID Objects;          // Total number of objects
			IMINDEX TmpSize;
			OBJHEAD* pOH;
			OBJID ObjIndex;
			IMINDEX ImageIndex;       // Index into total image
			IMINDEX TmpIndex = 0;     // Lached "ImageIndex"
			UBYTE ParIndex;
			UBYTE Type;


			GH.printf("Validation started\r\n");
			cValidateSetErrorIndex(0);
			pIH = (IMGHEAD*)pI;

			TotalSize = (*(IMGHEAD*)pI).ImageSize;
			Objects = (*(IMGHEAD*)pI).NumberOfObjects;

			// Check size
			ImageIndex = (uint)(Unsafe.SizeOf<IMGHEAD>() + Objects * Unsafe.SizeOf<OBJHEAD>());

			if ((TotalSize < ImageIndex) || (Objects == 0))
			{ // Size too small

				cValidateSetErrorIndex(4);
			}
			else
			{
				pOH = (OBJHEAD*)&pI[sizeof(IMGHEAD) - sizeof(OBJHEAD)];

				// Scan headers for size of instructions
				TmpSize = 0;
				for (ObjIndex = 1; ObjIndex <= Objects; ObjIndex++)
				{
					TmpSize += (IMINDEX)pOH[ObjIndex].OffsetToInstructions;
				}
				Result = OK;

				// Scan every object
				for (ObjIndex = 1; (ObjIndex <= Objects) && (Result == OK); ObjIndex++)
				{
					if ((long)pOH[ObjIndex].OffsetToInstructions == 0)
					{
						pOH[ObjIndex].OffsetToInstructions = (IP)ImageIndex;
					}

					// Check for SUBCALL parameter description
					if ((pOH[ObjIndex].OwnerObjectId == 0) && (pOH[ObjIndex].TriggerCount == 1))
					{ // SUBCALL object

						if (pOH[ObjIndex].OffsetToInstructions >= (IP)ImageIndex)
						{ // Only if not alias

							ParIndex = (byte)pI[ImageIndex++];
							while (ParIndex != 0)
							{
								Type = pI[ImageIndex++];
								if ((Type & CALLPAR_TYPE) == CALLPAR_STRING)
								{
									ImageIndex++;
								}
								ParIndex--;
							}
						}
						else
						{
							Result = RESULT.STOP;
						}
					}

					// Scan all byte codes in object
					while ((Result == OK) && (ImageIndex < TotalSize))
					{
						TmpIndex = ImageIndex;
						Result = cValidateBytecode(pI, &ImageIndex, pLabel);
					}
					if (Result == RESULT.FAIL)
					{
						cValidateSetErrorIndex(TmpIndex);
					}
					else
					{
						Result = OK;
					}
				}
				if (ImageIndex != TotalSize)
				{
					GH.printf($"{ImageIndex} {TotalSize}\r\n");
					cValidateSetErrorIndex(TmpIndex);
				}
			}

			// Check version
			if (((*pIH).VersionInfo < ((UWORD)(BYTECODE_VERSION * 100.0))) && ((*pIH).VersionInfo != 0))
			{
				cValidateSetErrorIndex(8);
			}

			GH.printf("Validation ended\r\n");

			if (Disassemble != 0)
			{
				cValidateDisassembleProgram(PrgId, pI, pLabel);
			}
			// Result of validation
			if (cValidateGetErrorIndex() != 0)
			{
				if (cValidateGetErrorIndex() == 8)
				{
					Result = OK;
				}
				else
				{
					Result = RESULT.FAIL;
				}
			}
			else
			{
				Result = OK;
			}

			return (Result);
		}
	}
}
