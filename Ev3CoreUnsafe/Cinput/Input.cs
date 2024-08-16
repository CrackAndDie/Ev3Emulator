using Ev3CoreUnsafe.Cinput.Interfaces;
using Ev3CoreUnsafe.Enums;
using Ev3CoreUnsafe.Extensions;
using Ev3CoreUnsafe.Helpers;
using Ev3CoreUnsafe.Lms2012.Interfaces;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using static Ev3CoreUnsafe.Defines;

namespace Ev3CoreUnsafe.Cinput
{
	public unsafe class Input : IInput
	{
        DATA8* DaisyBuf = CommonHelper.Pointer1d<DATA8>(64);
        DATA8 ActLayer = 0;
        DATA8 MoreLayers = 0;

        public void cInputPrintLine(DATA8* pBuffer)
		{
			GH.printf(CommonHelper.GetString(pBuffer));
		}

		public void cInputShowTypeData()
		{
			DATA8* Buffer = CommonHelper.Pointer1d<DATA8>(LINESIZE);
			UBYTE Index;
			DATA8 Type;
			DATA8 Mode;
			DATA8 Connection;
			DATA8 LastType;
			int Pos;


			CommonHelper.snprintf(Buffer, LINESIZE, "//! \\page types Known Device Types\r\n");
			cInputPrintLine(Buffer);
			CommonHelper.snprintf(Buffer, LINESIZE, "//!\r\n");
			cInputPrintLine(Buffer);
			CommonHelper.snprintf(Buffer, LINESIZE, "//! <hr size=\"1\"/>\r\n");
			cInputPrintLine(Buffer);
			CommonHelper.snprintf(Buffer, LINESIZE, "//! Following devices are supported (except devices starting with //)\\n\r\n");
			cInputPrintLine(Buffer);
			CommonHelper.snprintf(Buffer, LINESIZE, "//! Devices marked with * means that the data is supplied by the device itself\\n\r\n");
			cInputPrintLine(Buffer);
			CommonHelper.snprintf(Buffer, LINESIZE, "//! Devices marked with # is not supported in View and Datalog apps\r\n");
			cInputPrintLine(Buffer);
			CommonHelper.snprintf(Buffer, LINESIZE, "//!\r\n");
			cInputPrintLine(Buffer);
			CommonHelper.snprintf(Buffer, LINESIZE, "//! \\verbatim\r\n");
			cInputPrintLine(Buffer);
			CommonHelper.snprintf(Buffer, LINESIZE, "\r\n");
			cInputPrintLine(Buffer);
			CommonHelper.snprintf(Buffer, LINESIZE, "//  Type  Mode  Name      DataSets  Format  Figures  Decimals  Views  Conn. Pins  RawMin   RawMax   PctMin  PctMax  SiMin    SiMax    Time  IdValue  Symbol\r\n");
			cInputPrintLine(Buffer);
			CommonHelper.snprintf(Buffer, LINESIZE, "//  ----  ----  --------  --------  ------  -------  --------  -----  ----- ----  -------  -------  ------  ------  -------  -------  ----  -------  --------\r\n");
			cInputPrintLine(Buffer);

			LastType = TYPE_ERROR;
			Index = 0;


			while ((Index < GH.InputInstance.MaxDeviceTypes) && (GH.InputInstance.TypeData[Index].Name[0] != 0))
			{
				Type = GH.InputInstance.TypeData[Index].Type;

				if (Type <= MAX_VALID_TYPE)
				{
					Mode = GH.InputInstance.TypeData[Index].Mode;
					Connection = GH.InputInstance.TypeData[Index].Connection;

					if (Type != LastType)
					{
						CommonHelper.snprintf(Buffer, LINESIZE, "\r\n");
						cInputPrintLine(Buffer);

						LastType = Type;
					}

					Pos = 0;

					if (Connection == CONN_INPUT_UART)
					{
						if (Mode < GH.InputInstance.TypeData[Index].Views)
						{
							Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, "*   ");
						}
						else
						{
							Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, "* # ");
						}
					}
					else
					{
						Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, "    ");
					}

					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, $"{Type}");

					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, "  ");
					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, $"{Mode}");

					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, "  ");
					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, CommonHelper.GetString((sbyte*)GH.InputInstance.TypeData[Index].Name));

					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, "  ");
					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, $"{GH.InputInstance.TypeData[Index].DataSets}");

					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, "       ");
					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, $"{GH.InputInstance.TypeData[Index].Format}");

					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, "     ");
					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, $"{GH.InputInstance.TypeData[Index].Figures}");

					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, "        ");
					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, $"{GH.InputInstance.TypeData[Index].Decimals}");

					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, "         ");
					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, $"{GH.InputInstance.TypeData[Index].Views}");

					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, "      ");
					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, $"{Connection}");

					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, "   ");
					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, $"0x{GH.InputInstance.TypeData[Index].Pins}");

					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, " ");
					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, CommonHelper.GetString(GH.InputInstance.TypeData[Index].RawMin, 8, 1));

					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, " ");
					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, CommonHelper.GetString(GH.InputInstance.TypeData[Index].RawMin, 8, 1));

					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, "    ");
					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, CommonHelper.GetString(GH.InputInstance.TypeData[Index].PctMin, 4, 0));

					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, "    ");
					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, CommonHelper.GetString(GH.InputInstance.TypeData[Index].PctMax, 4, 0));

					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, " ");
					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, CommonHelper.GetString(GH.InputInstance.TypeData[Index].SiMin, 8, 1));

					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, " ");
					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, CommonHelper.GetString(GH.InputInstance.TypeData[Index].SiMax, 8, 1));

					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, "  ");
					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, $"{GH.InputInstance.TypeData[Index].InvalidTime}");

					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, "    ");
					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, $"{GH.InputInstance.TypeData[Index].IdValue}");

					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, "  ");

					if (GH.InputInstance.TypeData[Index].Symbol[0] != 0)
					{
						Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, CommonHelper.GetString(GH.InputInstance.TypeData[Index].Symbol));
					}
					else
					{
						Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, "_");
					}

					Pos += CommonHelper.snprintf(&Buffer[Pos], LINESIZE - Pos, "\r\n");
					cInputPrintLine(Buffer);
				}
				Index++;
			}
			CommonHelper.snprintf(Buffer, LINESIZE, "\r\n");
			cInputPrintLine(Buffer);

			CommonHelper.snprintf(Buffer, LINESIZE, "//!  \\endverbatim\r\n");
			cInputPrintLine(Buffer);
			CommonHelper.snprintf(Buffer, LINESIZE, "//!  See connection types \\ref connectiontypes \"Conn.\"\r\n");
			cInputPrintLine(Buffer);
			CommonHelper.snprintf(Buffer, LINESIZE, "//!  \r\n");
			cInputPrintLine(Buffer);
			CommonHelper.snprintf(Buffer, LINESIZE, "//!  \\n\r\n");
			cInputPrintLine(Buffer);

		}

		public void Memcpy(byte* pDest, byte* pSrc, int No)
		{
			CommonHelper.memcpy(pDest, pSrc, No);
		}

		public void ClrLayer()
		{
			GH.Lms.ExecuteByteCode(CLR_LAYER_CLR_CHANGES, null, null);
			GH.Lms.ExecuteByteCode(CLR_LAYER_CLR_BUMBED, null, null);
			GH.Lms.ExecuteByteCode(CLR_LAYER_OUTPUT_RESET, null, null);
			GH.Lms.ExecuteByteCode(CLR_LAYER_OUTPUT_CLR_COUNT, null, null);
			GH.Lms.ExecuteByteCode(CLR_LAYER_INPUT_WRITE, null, null);
		}

		public void StopLayer()
		{
			GH.Lms.ExecuteByteCode(STOP_LAYER, null, null);
		}

		//                DEVICE MAPPING
		//
		// Device         0   1   2   3   4   5   6   7   8   9   10  11  12  13  14  15  16  17  18  19  20  21  22  23  24  25  26  27  28  29  30  31
		//
		// Layer          0   0   0   0   1   1   1   1   2   2   2   2   3   3   3   3   0   0   0   0   1   1   1   1   2   2   2   2   3   3   3   3
		// Port (INPUT)   0   1   2   3   0   1   2   3   0   1   2   3   0   1   2   3   16  17  18  19  16  17  18  19  16  17  18  19  16  17  18  19
		// Port (OUTPUT)  -   -   -   -   -   -   -   -   -   -   -   -   -   -   -   -   0   1   2   3   0   1   2   3   0   1   2   3   0   1   2   3
		// Output         0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   1   1   1   1   1   1   1   1   1   1   1   1   1   1   1   1

		public RESULT cInputExpandDevice(DATA8 Device, DATA8* pLayer, DATA8* pPort, DATA8* pOutput)
		{ // pPort: pOutput=0 -> 0..3 , pOutput=1 -> 0..3

			RESULT Result = RESULT.FAIL;

			if ((Device >= 0) && (Device < DEVICES))
			{
				if (Device >= INPUT_DEVICES)
				{ // OUTPUT

					*pOutput = 1;
					Device -= INPUT_DEVICES;
					*pPort = ((sbyte)(Device % OUTPUT_PORTS));
					*pPort += INPUT_DEVICES;
				}
				else
				{ // INPUT

					*pOutput = 0;
					*pPort = (sbyte)(Device % INPUT_PORTS);

				}
				*pLayer = (sbyte)(Device / CHAIN_DEPT);

				Result = OK;
			}

			return (Result);
		}


		public RESULT cInputCompressDevice(DATA8* pDevice, UBYTE Layer, UBYTE Port)
		{ // Port: Input 0..3 , Output 16..19

			RESULT Result = RESULT.FAIL;

			if (Port >= INPUT_DEVICES)
			{ // OUTPUT

				*pDevice = ((sbyte)(OUTPUT_PORTS * Layer));
				*pDevice += (sbyte)Port;

			}
			else
			{ // INPUT

				*pDevice = (sbyte)(INPUT_PORTS * Layer);
				*pDevice += (sbyte)Port;
			}

			if ((*pDevice >= 0) && (*pDevice < DEVICES))
			{
				Result = OK;
			}

			return (Result);
		}


		public RESULT cInputInsertNewIicString(DATA8 Type, DATA8 Mode, DATA8* pManufacturer, DATA8* pSensorType, DATA8 SetupLng, ULONG SetupString, DATA8 PollLng, ULONG PollString, DATA8 ReadLng)
		{
			RESULT Result = RESULT.FAIL;  // RESULT.FAIL=full, OK=new, RESULT.BUSY=found
			IICSTR* pTmp;
			UWORD Index = 0;

			if ((Type >= 0) && (Type < (MAX_DEVICE_TYPE + 1)) && (Mode >= 0) && (Mode < MAX_DEVICE_MODES))
			{ // Type and mode valid


				while ((Index < GH.InputInstance.IicDeviceTypes) && (Result != RESULT.BUSY))
				{ // trying to find device type

					if ((GH.InputInstance.IicString[Index].Type == Type) && (GH.InputInstance.IicString[Index].Mode == Mode))
					{ // match on type and mode

						Result = RESULT.BUSY;
					}
					Index++;
				}
				if (Result != RESULT.BUSY)
				{ // device type not found

					if (GH.InputInstance.IicDeviceTypes < MAX_DEVICE_TYPES)
					{ // Allocate room for a new type/mode

						if (GH.Memory.cMemoryRealloc((void*)GH.InputInstance.IicString, (void**)&pTmp, (DATA32)(sizeof(IICSTR) * (GH.InputInstance.IicDeviceTypes + 1))) == OK)
						{ // Success

							GH.InputInstance.IicString = pTmp;

							GH.InputInstance.IicString[Index].Type = Type;
							GH.InputInstance.IicString[Index].Mode = Mode;
							CommonHelper.snprintf((DATA8*)GH.InputInstance.IicString[Index].Manufacturer, IIC_NAME_LENGTH + 1, CommonHelper.GetString((DATA8*)pManufacturer));
							CommonHelper.snprintf((DATA8*)GH.InputInstance.IicString[Index].SensorType, IIC_NAME_LENGTH + 1, CommonHelper.GetString((DATA8*)pSensorType));
							GH.InputInstance.IicString[Index].SetupLng = SetupLng;
							GH.InputInstance.IicString[Index].SetupString = SetupString;
							GH.InputInstance.IicString[Index].PollLng = PollLng;
							GH.InputInstance.IicString[Index].PollString = PollString;
							GH.InputInstance.IicString[Index].ReadLng = ReadLng;
							//          GH.printf("cInputInsertNewIicString  %-3u %01u IIC %u 0x%08X %u 0x%08X %s %s\r\n",Type,Mode,SetupLng,SetupString,PollLng,PollString,pManufacturer,pSensorType);

							GH.InputInstance.IicDeviceTypes++;
							Result = OK;
						}
					}
				}
				if (Result == RESULT.FAIL)
				{ // No room for type/mode

					GH.Ev3System.Logger.LogError($"Error {nameof(TYPEDATA_TABEL_FULL)} occured in {Environment.StackTrace}");
				}
			}
			else
			{ // Type or mode invalid

				GH.printf($"Iic  error {Type}: m={Mode} IIC\r\n");
			}

			return (Result);
		}

		
		public RESULT cInputGetIicString(DATA8 Type, DATA8 Mode, IICSTR* IicStr)
		{
			RESULT Result = RESULT.FAIL;  // RESULT.FAIL=full, OK=new, RESULT.BUSY=found
			UWORD Index = 0;


			(*IicStr).SetupLng = 0;
			(*IicStr).SetupString = 0;
			(*IicStr).PollLng = 0;
			(*IicStr).PollString = 0;
			(*IicStr).ReadLng = 0;

			unchecked 
			{ 
				if ((Type >= 0) && (Type < (sbyte)(MAX_DEVICE_TYPE + 1)) && (Mode >= 0) && (Mode < MAX_DEVICE_MODES))
				{ // Type and mode valid


					while ((Index < GH.InputInstance.IicDeviceTypes) && (Result != OK))
					{ // trying to find device type

						if ((GH.InputInstance.IicString[Index].Type == Type) && (GH.InputInstance.IicString[Index].Mode == Mode))
						{ // match on type and mode

							(*IicStr).Type = Type;
							(*IicStr).Mode = Mode;
							CommonHelper.snprintf((DATA8*)IicStr->Manufacturer, IIC_NAME_LENGTH + 1, CommonHelper.GetString((DATA8*)GH.InputInstance.IicString[Index].Manufacturer));
							CommonHelper.snprintf((DATA8*)IicStr->SensorType, IIC_NAME_LENGTH + 1, CommonHelper.GetString((DATA8*)GH.InputInstance.IicString[Index].SensorType));
							(*IicStr).SetupLng = GH.InputInstance.IicString[Index].SetupLng;
							(*IicStr).SetupString = GH.InputInstance.IicString[Index].SetupString;
							(*IicStr).PollLng = GH.InputInstance.IicString[Index].PollLng;
							(*IicStr).PollString = GH.InputInstance.IicString[Index].PollString;
							(*IicStr).ReadLng = GH.InputInstance.IicString[Index].ReadLng;

							Result = OK;
						}
						Index++;
					}
				}
            }

            return (Result);
		}


		public RESULT cInputGetNewTypeDataPointer(SBYTE* pName, DATA8 Type, DATA8 Mode, DATA8 Connection, TYPES** ppPlace)
		{
			RESULT Result = RESULT.FAIL;  // RESULT.FAIL=full, OK=new, RESULT.BUSY=found
			UWORD Index = 0;

			*ppPlace = null;

			if ((Type >= 0) && (Type < (MAX_DEVICE_TYPE + 1)) && (Mode >= 0) && (Mode < MAX_DEVICE_MODES))
			{ // Type and mode valid

				while ((Index < GH.InputInstance.MaxDeviceTypes) && (Result != RESULT.BUSY))
				{ // trying to find device type

					if ((GH.InputInstance.TypeData[Index].Type == Type) && (GH.InputInstance.TypeData[Index].Mode == Mode) && (GH.InputInstance.TypeData[Index].Connection == Connection))
					{ // match on type, mode and connection

						*ppPlace = &GH.InputInstance.TypeData[Index];
						Result = RESULT.BUSY;
					}
					Index++;
				}
				if (Result != RESULT.BUSY)
				{ // device type not found

					if (GH.InputInstance.MaxDeviceTypes < MAX_DEVICE_TYPES)
					{ // Allocate room for a new type/mode

						if (GH.Memory.cMemoryRealloc((void*)GH.InputInstance.TypeData, (void**)ppPlace, (DATA32)(sizeof(TYPES) * (GH.InputInstance.MaxDeviceTypes + 1))) == OK)
						{ // Success

							GH.InputInstance.TypeData = *ppPlace;

							*ppPlace = &GH.InputInstance.TypeData[GH.InputInstance.MaxDeviceTypes];
							GH.InputInstance.TypeModes[Type]++;
							GH.InputInstance.MaxDeviceTypes++;
							Result = OK;
						}
					}
				}
				if (Result == RESULT.FAIL)
				{ // No room for type/mode

					GH.Ev3System.Logger.LogError($"Error {nameof(TYPEDATA_TABEL_FULL)} occured in {Environment.StackTrace}");
				}
			}
			else
			{ // Type or mode invalid

				GH.printf($"Type error {Type}: m={Mode} c={Connection} n={CommonHelper.GetString(pName)}\r\n");
			}

			return (Result);
		}


		public RESULT cInputInsertTypeData(DATA8* pFilename)
		{
			RESULT Result = RESULT.FAIL;
			DATA8* Buf = CommonHelper.Pointer1d<DATA8>(256);
			DATA8* Name = CommonHelper.Pointer1d<DATA8>(256);
			DATA8* Symbol = CommonHelper.Pointer1d<DATA8>(256);
			DATA8* Manufacturer = CommonHelper.Pointer1d<DATA8>(256);
			DATA8* SensorType = CommonHelper.Pointer1d<DATA8>(256);
			uint Type;
			uint Connection;
			uint Mode;
			uint DataSets;
			uint Format;
			uint Figures;
			uint Decimals;
			uint Views;
			uint Pins;
			uint Time;
			uint IdValue;
			uint SetupLng;
			uint SetupString;
			uint PollLng;
			uint PollString;
			int ReadLng;
			string Buf2;
			TYPES Tmp = new TYPES();
			TYPES* pTypes;
			int Count;

			var fileNameee = CommonHelper.GetString(pFilename);
			if (!File.Exists(fileNameee))
				return Result;

			using FileStream fileStream = File.OpenRead(fileNameee);
            using StreamReader reader = new StreamReader(fileStream);

			do
			{
                Buf2 = reader.ReadLine();
                if (!string.IsNullOrWhiteSpace(Buf2))
                {
					if ((Buf2[0] != '/') && (Buf2[0] != '*'))
					{
                        string[] data = Buf2.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        Type = uint.Parse(data[0]);
                        Mode = uint.Parse(data[1]);
                        Name = data[2].AsSbytePointer();
                        DataSets = uint.Parse(data[3]);
                        Format = uint.Parse(data[4]);
                        Figures = uint.Parse(data[5]);
                        Decimals = uint.Parse(data[6]);
                        Views = uint.Parse(data[7]);
                        Connection = uint.Parse(data[8]);
                        Pins = Convert.ToUInt32(data[9], 16);
                        Tmp.RawMin = float.Parse(data[10], CultureInfo.InvariantCulture);
                        Tmp.RawMax = float.Parse(data[11], CultureInfo.InvariantCulture);
                        Tmp.PctMin = float.Parse(data[12], CultureInfo.InvariantCulture);
                        Tmp.PctMax = float.Parse(data[13], CultureInfo.InvariantCulture);
                        Tmp.SiMin = float.Parse(data[14], CultureInfo.InvariantCulture);
                        Tmp.SiMax = float.Parse(data[15], CultureInfo.InvariantCulture);
                        Time = uint.Parse(data[16]);
                        IdValue = uint.Parse(data[17]);
                        Symbol = data[18].AsSbytePointer();

                        Count = 19; // anime hahaha
                        if (Count == TYPE_PARAMETERS)
						{
							Tmp.Type = (DATA8)Type;
							Tmp.Mode = (DATA8)Mode;
							Tmp.DataSets = (DATA8)DataSets;
							Tmp.Format = (DATA8)Format;
							Tmp.Figures = (DATA8)Figures;
							Tmp.Decimals = (DATA8)Decimals;
							Tmp.Connection = (DATA8)Connection;
							Tmp.Views = (DATA8)Views;
							Tmp.Pins = (DATA8)Pins;
							Tmp.InvalidTime = (UWORD)Time;
							Tmp.IdValue = (UWORD)IdValue;

							Result = cInputGetNewTypeDataPointer((SBYTE*)Name, (DATA8)Type, (DATA8)Mode, (DATA8)Connection, &pTypes);
							//            GH.printf("cInputTypeDataInit\r\n");
							if (Result == OK)
							{
								(*pTypes) = Tmp;

								Count = 0;
								while ((Name[Count] != 0) && (Count < TYPE_NAME_LENGTH))
								{
									if (Name[Count] == '_')
									{
										(*pTypes).Name[Count] = (byte)' ';
									}
									else
									{
										(*pTypes).Name[Count] = (byte)Name[Count];
									}
									Count++;
								}
								(*pTypes).Name[Count] = 0;

								if (Symbol[0] == '_')
								{
									(*pTypes).Symbol[0] = 0;
								}
								else
								{
									Count = 0;
									while ((Symbol[Count] != 0) && (Count < SYMBOL_LENGTH))
									{
										if (Symbol[Count] == '_')
										{
											(*pTypes).Symbol[Count] = (sbyte)' ';
										}
										else
										{
											(*pTypes).Symbol[Count] = Symbol[Count];
										}
										Count++;
									}
									(*pTypes).Symbol[Count] = 0;
								}
								if (Tmp.Connection == CONN_NXT_IIC)
								{ // NXT IIC sensor

                                    // setup string + poll string
                                    // 3 0x01420000 2 0x01000000

                                    data = Buf2.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
									Type = uint.Parse(data[0]);
                                    Mode = uint.Parse(data[1]);
                                    Name = data[2].AsSbytePointer();
                                    DataSets = uint.Parse(data[3]);
                                    Format = uint.Parse(data[4]);
                                    Figures = uint.Parse(data[5]);
                                    Decimals = uint.Parse(data[6]);
                                    Views = uint.Parse(data[7]);
                                    Connection = uint.Parse(data[8]);
                                    Pins = Convert.ToUInt32(data[9], 16);
                                    Tmp.RawMin = float.Parse(data[10], CultureInfo.InvariantCulture);
                                    Tmp.RawMax = float.Parse(data[11], CultureInfo.InvariantCulture);
                                    Tmp.PctMin = float.Parse(data[12], CultureInfo.InvariantCulture);
                                    Tmp.PctMax = float.Parse(data[13], CultureInfo.InvariantCulture);
                                    Tmp.SiMin = float.Parse(data[14], CultureInfo.InvariantCulture);
                                    Tmp.SiMax = float.Parse(data[15], CultureInfo.InvariantCulture);
                                    Time = uint.Parse(data[16]);
                                    IdValue = uint.Parse(data[17]);
                                    Symbol = data[18].AsSbytePointer();
                                    Manufacturer = data[19].AsSbytePointer();
                                    SensorType = data[20].AsSbytePointer();
                                    SetupLng = uint.Parse(data[21]);
                                    SetupString = Convert.ToUInt32(data[22], 16);
                                    PollLng = uint.Parse(data[23]);
                                    PollString = Convert.ToUInt32(data[24], 16);
                                    ReadLng = int.Parse(data[25]);

                                    Count = 26; // anime hahaha
                                    if (Count == (TYPE_PARAMETERS + 7))
									{
										cInputInsertNewIicString((sbyte)Type, (sbyte)Mode, (DATA8*)Manufacturer, (DATA8*)SensorType, (DATA8)SetupLng, (ULONG)SetupString, (DATA8)PollLng, (ULONG)PollString, (DATA8)ReadLng);
										//                  GH.printf("%02u %01u IIC %u 0x%08X %u 0x%08X %u\r\n",Type,Mode,SetupLng,SetupString,PollLng,PollString,ReadLng);
									}
								}
							}
						}
					}
				}
			}
			while (Buf2 != null);

			Result = OK;

			return (Result);
		}


		public void cInputTypeDataInit()
		{
			DATA8* PrgNameBuf = CommonHelper.Pointer1d<DATA8>(255);
			UWORD Index = 0;
			UBYTE TypeDataFound = 0;

			// Set TypeMode to mode = 0
			Index = 0;
			while (Index < (MAX_DEVICE_TYPE + 1))
			{
				GH.InputInstance.TypeModes[Index] = 0;
				Index++;
			}

			// Insert default types into TypeData
			Index = 0;
			while ((Index < (GH.InputInstance.MaxDeviceTypes + 1)) && (TypeDefault[Index].Name[0] != 0))
			{
				GH.InputInstance.TypeData[Index] = TypeDefault[Index];

				fixed (UBYTE* tmpP = &TypeDefault[Index].Name[0])
				{
					CommonHelper.snprintf((DATA8*)GH.InputInstance.TypeData[Index].Name, TYPE_NAME_LENGTH + 1, CommonHelper.GetString((DATA8*)tmpP));
				}

				if (GH.InputInstance.TypeData[Index].Type == TYPE_NONE)
				{
					GH.InputInstance.NoneIndex = Index;
				}
				if (GH.InputInstance.TypeData[Index].Type == TYPE_UNKNOWN)
				{
					GH.InputInstance.UnknownIndex = Index;
				}
				GH.InputInstance.TypeModes[GH.InputInstance.TypeData[Index].Type]++;
				Index++;
			}

			//  GH.printf("Search start\r\n");
			CommonHelper.snprintf(PrgNameBuf, vmFILENAMESIZE, $"{vmSETTINGS_DIR}/{TYPEDATE_FILE_NAME}{EXT_CONFIG}");

			if (cInputInsertTypeData(PrgNameBuf) == OK)
			{
				TypeDataFound = 1;
			}

			for (Index = TYPE_THIRD_PARTY_START; Index <= TYPE_THIRD_PARTY_END; Index++)
			{
				CommonHelper.snprintf(PrgNameBuf, vmFILENAMESIZE, $"{vmSETTINGS_DIR}/{TYPEDATE_FILE_NAME}{Index}{EXT_CONFIG}");
				if (cInputInsertTypeData(PrgNameBuf) == OK)
				{
					TypeDataFound = 1;
				}
			}
			//  GH.printf("Done\r\n");

			if (TypeDataFound == 0)
			{
				GH.Ev3System.Logger.LogError($"Error {nameof(TYPEDATA_FILE_NOT_FOUND)} occured in {Environment.StackTrace}");
			}
		}


		public RESULT cInputSetupDevice(DATA8 Device, DATA8 Repeat, DATA16 Time, DATA8 WrLng, DATA8* pWrData, DATA8 RdLng, DATA8* pRdData)
		{

			(*GH.InputInstance.IicDat).Result = RESULT.FAIL;

			if (Device < INPUTS)
			{
				if (GH.InputInstance.DeviceData[Device].Connection == CONN_NXT_IIC)
				{ // Device is an IIC device

					if (WrLng > MAX_DEVICE_DATALENGTH)
					{
						WrLng = MAX_DEVICE_DATALENGTH;
					}
					if (RdLng > MAX_DEVICE_DATALENGTH)
					{
						RdLng = MAX_DEVICE_DATALENGTH;
					}

					if (Time != 0)
					{
						if (Time < MIN_IIC_REPEAT_TIME)
						{
							Time = MIN_IIC_REPEAT_TIME;
						}
						if (Time > MAX_IIC_REPEAT_TIME)
						{
							Time = MAX_IIC_REPEAT_TIME;
						}
					}

					(*GH.InputInstance.IicDat).Result = RESULT.BUSY;
					(*GH.InputInstance.IicDat).Port = Device;
					(*GH.InputInstance.IicDat).Repeat = Repeat;
					(*GH.InputInstance.IicDat).Time = Time;
					(*GH.InputInstance.IicDat).WrLng = WrLng;
					(*GH.InputInstance.IicDat).RdLng = RdLng;

					Memcpy((byte*)(*GH.InputInstance.IicDat).WrData, (byte*)pWrData, (*GH.InputInstance.IicDat).WrLng);

					GH.Ev3System.InputHandler.IoctlI2c(IIC_SETUP, (*GH.InputInstance.IicDat));

					if ((*GH.InputInstance.IicDat).Result == OK)
					{
						Memcpy((byte*)pRdData, (byte*)(*GH.InputInstance.IicDat).RdData, (*GH.InputInstance.IicDat).RdLng);
					}
				}
			}

			return ((*GH.InputInstance.IicDat).Result);
		}

		public RESULT cInputFindDumbInputDevice(DATA8 Device, DATA8 Type, DATA8 Mode, UWORD* pTypeIndex)
		{
			RESULT Result = RESULT.FAIL;
			UWORD IdValue = 0;
			UWORD Index = 0;
			UWORD Tmp;

			// get actual id value
			IdValue = (ushort)CtoV((ushort)(*GH.InputInstance.pAnalog).InPin1[Device]);

			while ((Index < GH.InputInstance.MaxDeviceTypes) && (Result != OK))
			{
				Tmp = GH.InputInstance.TypeData[Index].IdValue;

				if (Tmp >= IN1_ID_HYSTERESIS)
				{
					if ((IdValue >= (Tmp - IN1_ID_HYSTERESIS)) && (IdValue < (Tmp + IN1_ID_HYSTERESIS)))
					{ // id value match

						if (Type == TYPE_UNKNOWN)
						{ // first search

							// "type data" entry found
							*pTypeIndex = Index;

						}
						else
						{ // next search

							if (Type == GH.InputInstance.TypeData[Index].Type)
							{ //

								*pTypeIndex = Index;
							}
						}
						if (Mode == GH.InputInstance.TypeData[Index].Mode)
						{ // mode match

							// "type data" entry found
							*pTypeIndex = Index;

							// skip looping
							Result = OK;
						}
					}
				}
				Index++;
			}

			return (Result);
		}


		public RESULT cInputFindDumbOutputDevice(DATA8 Device, DATA8 Type, DATA8 Mode, UWORD* pTypeIndex)
		{
			RESULT Result = RESULT.FAIL;
			UWORD IdValue = 0;
			UWORD Index = 0;
			UWORD Tmp;

			// get actual id value
			IdValue = (ushort)(*GH.InputInstance.pAnalog).OutPin5Low[Device - INPUT_DEVICES];

			while ((Index < GH.InputInstance.MaxDeviceTypes) && (Result != OK))
			{
				Tmp = GH.InputInstance.TypeData[Index].IdValue;

				if (Tmp >= OUT5_ID_HYSTERESIS)
				{
					if ((IdValue >= (Tmp - OUT5_ID_HYSTERESIS)) && (IdValue < (Tmp + OUT5_ID_HYSTERESIS)))
					{ // id value match

						// "type data" entry found
						*pTypeIndex = Index;

						if (Mode == GH.InputInstance.TypeData[Index].Mode)
						{ // mode match

							// "type data" entry found
							*pTypeIndex = Index;

							// skip looping
							Result = OK;
						}
					}
				}
				Index++;
			}

			return (Result);
		}


		public RESULT cInputFindDevice(DATA8 Type, DATA8 Mode, UWORD* pTypeIndex)
		{
			RESULT Result = RESULT.FAIL;
			UWORD Index = 0;

			while ((Index < GH.InputInstance.MaxDeviceTypes) && (Result != OK))
			{
				if (Type == GH.InputInstance.TypeData[Index].Type)
				{ // type match

					*pTypeIndex = Index;

					if (Mode == GH.InputInstance.TypeData[Index].Mode)
					{ // mode match

						// "type data" entry found
						*pTypeIndex = Index;

						// skip looping
						Result = OK;
					}
				}
				Index++;
			}

			return (Result);
		}


		public void cInputResetDevice(DATA8 Device, UWORD TypeIndex)
		{
			PRGID TmpPrgId;

			GH.InputInstance.DeviceType[Device] = GH.InputInstance.TypeData[TypeIndex].Type;
			GH.InputInstance.DeviceMode[Device] = GH.InputInstance.TypeData[TypeIndex].Mode;

			GH.InputInstance.DeviceData[Device].InvalidTime = GH.InputInstance.TypeData[TypeIndex].InvalidTime;
			GH.InputInstance.DeviceData[Device].DevStatus = RESULT.BUSY;

			// save new type
			GH.InputInstance.DeviceData[Device].TypeIndex = TypeIndex;

			if (GH.InputInstance.DeviceType[Device] != TYPE_UNKNOWN)
			{
				// configuration changed
				for (TmpPrgId = 0; TmpPrgId < MAX_PROGRAMS; TmpPrgId++)
				{
					GH.InputInstance.ConfigurationChanged[TmpPrgId]++;
				}
			}
		}


		public void cInputSetDeviceType(DATA8 Device, DATA8 Type, DATA8 Mode, int Line)
		{
			UWORD Index;
			DATA8* Buf = CommonHelper.Pointer1d<DATA8>(INPUTS * 2 + 1);
			UWORD TypeIndex;
			DATA8 Layer;
			DATA8 Port;
			DATA8 Output;

			if (cInputExpandDevice(Device, &Layer, &Port, &Output) == OK)
			{ // Device within range

				if (GH.InputInstance.DeviceData[Device].Connection == CONN_NONE)
				{
					Type = TYPE_NONE;
				}
				// default type is unknown!
				TypeIndex = GH.InputInstance.UnknownIndex;

				if (Layer == 0)
				{ // Local device

					if (Output == 0)
					{ // Input device

						// TRY TO FIND DUMB INPUT DEVICE
						if (GH.InputInstance.DeviceData[Device].Connection == CONN_INPUT_DUMB)
						{ // search "type data" for matching "dumb" input device

							cInputFindDumbInputDevice(Device, Type, Mode, &TypeIndex);
						}

						// IF NOT FOUND YET - TRY TO FIND TYPE ANYWAY
						if (TypeIndex == GH.InputInstance.UnknownIndex)
						{ // device not found or not "dumb" input/output device

							cInputFindDevice(Type, Mode, &TypeIndex);
						}

						if (GH.InputInstance.DeviceData[Device].TypeIndex != TypeIndex)
						{ // type or mode has changed

							cInputResetDevice(Device, TypeIndex);

							(*GH.InputInstance.pUart).Status[Device] &= ~UART_DATA_READY;
							(*GH.InputInstance.pIic).Status[Device] &= ~IIC_DATA_READY;
							(*GH.InputInstance.pAnalog).Updated[Device] = 0;

							if (GH.InputInstance.DeviceData[Device].Connection == CONN_NXT_IIC)
							{ // Device is an IIC device

								
								cInputGetIicString(GH.InputInstance.DeviceType[Device], GH.InputInstance.DeviceMode[Device], (IICSTR*)GH.InputInstance.IicStr);
								(*GH.InputInstance.IicStr).Port = Device;
								(*GH.InputInstance.IicStr).Time = (short)GH.InputInstance.DeviceData[Device].InvalidTime;

								if (((*GH.InputInstance.IicStr).SetupLng != 0) || ((*GH.InputInstance.IicStr).PollLng != 0))
								{
									//              GH.printf("%u %-4u %-3u %01u IIC %u 0x%08X %u 0x%08X %d\r\n",GH.InputInstance.IicStr.Port,GH.InputInstance.IicStr.Time,GH.InputInstance.IicStr.Type,GH.InputInstance.IicStr.Mode,GH.InputInstance.IicStr.SetupLng,GH.InputInstance.IicStr.SetupString,GH.InputInstance.IicStr.PollLng,GH.InputInstance.IicStr.PollString,GH.InputInstance.IicStr.ReadLng);

									GH.Ev3System.InputHandler.IoctlI2c(IIC_SET, (*GH.InputInstance.IicStr));
								}
							}

							// SETUP DRIVERS
							for (Index = 0; Index < INPUTS; Index++)
							{ // Initialise pin setup string to do nothing

								Buf[Index] = (sbyte)'-';
							}
							Buf[Index] = 0;

							// insert "pins" in setup string
							Buf[Device] = GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Device].TypeIndex].Pins;

							// write setup string to "Device Connection Manager" driver
							if (GH.InputInstance.DcmFile >= MIN_HANDLE)
							{
								// TODO: ???
								// write(GH.InputInstance.DcmFile, Buf, INPUTS);
							}

							for (Index = 0; Index < INPUTS; Index++)
							{ // build setup string for UART and IIC driver

								(*GH.InputInstance.DevCon).Connection[Index] = GH.InputInstance.DeviceData[Index].Connection;
								(*GH.InputInstance.DevCon).Type[Index] = GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Index].TypeIndex].Type;
								(*GH.InputInstance.DevCon).Mode[Index] = GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Index].TypeIndex].Mode;
							}

							// write setup string to "UART Device Controller" driver
							GH.Ev3System.InputHandler.IoctlUart(UART_SET_CONN, (*GH.InputInstance.DevCon));
							GH.Ev3System.InputHandler.IoctlI2c(IIC_SET_CONN, (*GH.InputInstance.DevCon));

							GH.printf($"c_input   cInputSetDeviceType: I   D={Device} C={GH.InputInstance.DeviceData[Device].Connection} Ti={GH.InputInstance.DeviceData[Device].TypeIndex} N={CommonHelper.GetString((sbyte*)GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Device].TypeIndex].Name)}\r\n");
						}
					}
					else
					{ // Output device

						// TRY TO FIND DUMB OUTPUT DEVICE
						if (GH.InputInstance.DeviceData[Device].Connection == CONN_OUTPUT_DUMB)
						{ // search "type data" for matching "dumb" output device

							cInputFindDumbInputDevice(Device, Type, Mode, &TypeIndex);
						}

						// IF NOT FOUND YET - TRY TO FIND TYPE ANYWAY
						if (TypeIndex == GH.InputInstance.UnknownIndex)
						{ // device not found or not "dumb" input/output device

							cInputFindDevice(Type, Mode, &TypeIndex);
						}

						if (GH.InputInstance.DeviceData[Device].TypeIndex != TypeIndex)
						{ // type or mode has changed

							cInputResetDevice(Device, TypeIndex);

							for (Index = 0; Index < OUTPUT_PORTS; Index++)
							{ // build setup string "type" for output

								Buf[Index] = GH.InputInstance.DeviceType[Index + INPUT_DEVICES];
							}
							Buf[Index] = 0;
							GH.Output.cOutputSetTypes(Buf);

							GH.printf($"c_input   cInputSetDeviceType: O   D={Device} C={GH.InputInstance.DeviceData[Device].Connection} Ti={GH.InputInstance.DeviceData[Device].TypeIndex} N={CommonHelper.GetString((sbyte*)GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Device].TypeIndex].Name)}\r\n");
						}
					}


				}
				else
				{ // Not local device

					// IF NOT FOUND YET - TRY TO FIND TYPE ANYWAY
					if (TypeIndex == GH.InputInstance.UnknownIndex)
					{ // device not found or not "dumb" input/output device

						cInputFindDevice(Type, Mode, &TypeIndex);
					}

					if (GH.InputInstance.DeviceData[Device].TypeIndex != TypeIndex)
					{ // type or mode has changed

						if ((GH.InputInstance.DeviceData[Device].Connection != CONN_NONE) && (GH.InputInstance.DeviceData[Device].Connection != CONN_ERROR) && (GH.InputInstance.DeviceType[Device] != TYPE_UNKNOWN))
						{
							if (GH.Daisy.cDaisyReady() != RESULT.BUSY)
							{

								cInputResetDevice(Device, TypeIndex);
								GH.InputInstance.DeviceData[Device].InvalidTime = DAISYCHAIN_MODE_TIME;
								cInputComSetDeviceType(Layer, Port, GH.InputInstance.DeviceType[Device], GH.InputInstance.DeviceMode[Device]);

								GH.printf($"c_input   cInputSetDeviceType: D   D={Device} C={GH.InputInstance.DeviceData[Device].Connection} Ti={GH.InputInstance.DeviceData[Device].TypeIndex} N={CommonHelper.GetString((sbyte*)GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Device].TypeIndex].Name)}\r\n");

								if (Device == TESTDEVICE)
								{
									GH.VMInstance.Status &= ~0x08;
								}
							}
							else
							{
								if (Device == TESTDEVICE)
								{
									GH.VMInstance.Status |= 0x08;
								}
								GH.InputInstance.DeviceData[Device].DevStatus = RESULT.BUSY;
							}
						}
						else
						{
							cInputResetDevice(Device, TypeIndex);
						}

					}
				}
				GH.printf($"{Line} - D={Device} SetDevice T={GH.InputInstance.DeviceType[Device]} M={GH.InputInstance.DeviceMode[Device]} C={GH.InputInstance.DeviceData[Device].Connection}\r\n");
			}
		}


		public void cInputCalDataInit()
		{
			DATA8 Type;
			DATA8 Mode;
			int File;
			DATA8* PrgNameBuf = CommonHelper.Pointer1d<DATA8>(vmFILENAMESIZE);
			// TODO: calib shite
			CommonHelper.snprintf(PrgNameBuf, vmFILENAMESIZE, $"{vmSETTINGS_DIR}/{vmCALDATA_FILE_NAME}{vmEXT_CONFIG}");
			//File = open(PrgNameBuf, O_RDONLY);
			//if (File >= MIN_HANDLE)
			//{
			//	if (read(File, (UBYTE*)&GH.InputInstance.Calib, sizeof(GH.InputInstance.Calib)) != sizeof(GH.InputInstance.Calib))
			//	{
			//		close(File);
			//		File = -1;
			//	}
			//	else
			//	{
			//		close(File);
			//	}
			//}
			//if (File < 0)
			//{
			//	for (Type = 0; Type < (MAX_DEVICE_TYPE); Type++)
			//	{
			//		for (Mode = 0; Mode < MAX_DEVICE_MODES; Mode++)
			//		{
			//			GH.InputInstance.Calib[Type][Mode].InUse = 0;
			//		}
			//	}
			//}
		}


		public void cInputCalDataExit()
		{
			int File;
			DATA8* PrgNameBuf = CommonHelper.Pointer1d<DATA8>(vmFILENAMESIZE);
            // TODO: calib shite
            CommonHelper.snprintf(PrgNameBuf, vmFILENAMESIZE, $"{vmSETTINGS_DIR}/{vmCALDATA_FILE_NAME}{vmEXT_CONFIG}");
			//File = open(PrgNameBuf, O_CREAT | O_WRONLY | O_TRUNC, SYSPERMISSIONS);
			//if (File >= MIN_HANDLE)
			//{
			//	write(File, (UBYTE*)&GH.InputInstance.Calib, sizeof(GH.InputInstance.Calib));
			//	close(File);
			//}
		}

		public void cInputCalcFullScale(UWORD* pRawVal, UWORD ZeroPointOffset, UBYTE PctFullScale, UBYTE InvStatus)
		{
			if (*pRawVal >= ZeroPointOffset)
			{
				*pRawVal -= ZeroPointOffset;
			}
			else
			{
				*pRawVal = 0;
			}

			*pRawVal = (ushort)((*pRawVal * 100) / PctFullScale);
			if (*pRawVal > SENSOR_RESOLUTION)
			{
				*pRawVal = SENSOR_RESOLUTION;
			}
			if (1 == InvStatus)
			{
				*pRawVal = (ushort)(SENSOR_RESOLUTION - *pRawVal);
			}
		}


		public void cInputCalibrateColor(COLORSTRUCT* pC, UWORD* pNewVals)
		{
			UBYTE CalRange;

			if ((pC->ADRaw[BLANK]) < pC->CalLimits[1])
			{
				CalRange = 2;
			}
			else
			{
				if ((pC->ADRaw[BLANK]) < pC->CalLimits[0])
				{
					CalRange = 1;
				}
				else
				{
					CalRange = 0;
				}
			}

			pNewVals[RED] = 0;
			if ((pC->ADRaw[RED]) > (pC->ADRaw[BLANK]))
			{
				pNewVals[RED] = (UWORD)(((ULONG)((pC->ADRaw[RED]) - (pC->ADRaw[BLANK])) * ((*pC).GetCalib(CalRange)[RED])) >> 16);
			}

			pNewVals[GREEN] = 0;
			if ((pC->ADRaw[GREEN]) > (pC->ADRaw[BLANK]))
			{
				pNewVals[GREEN] = (UWORD)(((ULONG)((pC->ADRaw[GREEN]) - (pC->ADRaw[BLANK])) * ((*pC).GetCalib(CalRange)[GREEN])) >> 16);
			}

			pNewVals[BLUE] = 0;
			if ((pC->ADRaw[BLUE]) > (pC->ADRaw[BLANK]))
			{
				pNewVals[BLUE] = (UWORD)(((ULONG)((pC->ADRaw[BLUE]) - (pC->ADRaw[BLANK])) * ((*pC).GetCalib(CalRange)[BLUE])) >> 16);
			}

			pNewVals[BLANK] = (pC->ADRaw[BLANK]);
			cInputCalcFullScale(&(pNewVals[BLANK]), COLORSENSORBGMIN, COLORSENSORBGPCTDYN, 0);
			(pNewVals[BLANK]) = (UWORD)(((ULONG)(pNewVals[BLANK]) * ((*pC).GetCalib(CalRange)[BLANK])) >> 16);
		}


		public DATAF cInputCalculateColor(COLORSTRUCT* pC)
		{
			DATAF Result;


			Result = DATAF_NAN;

			// Color Sensor values has been calculated -
			// now calculate the color and put it in Sensor value
			if (((pC->SensorRaw[RED]) > (pC->SensorRaw[BLUE])) &&
				((pC->SensorRaw[RED]) > (pC->SensorRaw[GREEN])))
			{

				// If all 3 colors are less than 65 OR (Less that 110 and bg less than 40)
				if (((pC->SensorRaw[RED]) < 65) ||
					(((pC->SensorRaw[BLANK]) < 40) && ((pC->SensorRaw[RED]) < 110)))
				{
					Result = (DATAF)BLACKCOLOR;
				}
				else
				{
					if (((((pC->SensorRaw[BLUE]) >> 2) + ((pC->SensorRaw[BLUE]) >> 3) + (pC->SensorRaw[BLUE])) < (pC->SensorRaw[GREEN])) &&
						((((pC->SensorRaw[GREEN]) << 1)) > (pC->SensorRaw[RED])))
					{
						Result = (DATAF)YELLOWCOLOR;
					}
					else
					{

						if ((((pC->SensorRaw[GREEN]) << 1) - ((pC->SensorRaw[GREEN]) >> 2)) < (pC->SensorRaw[RED]))
						{

							Result = (DATAF)REDCOLOR;
						}
						else
						{

							if ((((pC->SensorRaw[BLUE]) < 70) ||
								((pC->SensorRaw[GREEN]) < 70)) ||
							   (((pC->SensorRaw[BLANK]) < 140) && ((pC->SensorRaw[RED]) < 140)))
							{
								Result = (DATAF)BLACKCOLOR;
							}
							else
							{
								Result = (DATAF)WHITECOLOR;
							}
						}
					}
				}
			}
			else
			{

				// Red is not the dominant color
				if ((pC->SensorRaw[GREEN]) > (pC->SensorRaw[BLUE]))
				{

					// Green is the dominant color
					// If all 3 colors are less than 40 OR (Less that 70 and bg less than 20)
					if (((pC->SensorRaw[GREEN]) < 40) ||
						(((pC->SensorRaw[BLANK]) < 30) && ((pC->SensorRaw[GREEN]) < 70)))
					{
						Result = (DATAF)BLACKCOLOR;
					}
					else
					{
						if ((((pC->SensorRaw[BLUE]) << 1)) < (pC->SensorRaw[RED]))
						{
							Result = (DATAF)YELLOWCOLOR;
						}
						else
						{
							if ((((pC->SensorRaw[RED]) + ((pC->SensorRaw[RED]) >> 2)) < (pC->SensorRaw[GREEN])) ||
								(((pC->SensorRaw[BLUE]) + ((pC->SensorRaw[BLUE]) >> 2)) < (pC->SensorRaw[GREEN])))
							{
								Result = (DATAF)GREENCOLOR;
							}
							else
							{
								if ((((pC->SensorRaw[RED]) < 70) ||
									((pC->SensorRaw[BLUE]) < 70)) ||
									(((pC->SensorRaw[BLANK]) < 140) && ((pC->SensorRaw[GREEN]) < 140)))
								{
									Result = (DATAF)BLACKCOLOR;
								}
								else
								{
									Result = (DATAF)WHITECOLOR;
								}
							}
						}
					}
				}
				else
				{

					// Blue is the most dominant color
					// Colors can be blue, white or black
					// If all 3 colors are less than 48 OR (Less that 85 and bg less than 25)
					if (((pC->SensorRaw[BLUE]) < 48) ||
						(((pC->SensorRaw[BLANK]) < 25) && ((pC->SensorRaw[BLUE]) < 85)))
					{
						Result = (DATAF)BLACKCOLOR;
					}
					else
					{
						if ((((((pC->SensorRaw[RED]) * 48) >> 5) < (pC->SensorRaw[BLUE])) &&
							((((pC->SensorRaw[GREEN]) * 48) >> 5) < (pC->SensorRaw[BLUE])))
							||
							(((((pC->SensorRaw[RED]) * 58) >> 5) < (pC->SensorRaw[BLUE])) ||
							 ((((pC->SensorRaw[GREEN]) * 58) >> 5) < (pC->SensorRaw[BLUE]))))
						{
							Result = (DATAF)BLUECOLOR;
						}
						else
						{

							// Color is white or Black
							if ((((pC->SensorRaw[RED]) < 60) ||
								((pC->SensorRaw[GREEN]) < 60)) ||
							   (((pC->SensorRaw[BLANK]) < 110) && ((pC->SensorRaw[BLUE]) < 120)))
							{
								Result = (DATAF)BLACKCOLOR;
							}
							else
							{
								if ((((pC->SensorRaw[RED]) + ((pC->SensorRaw[RED]) >> 3)) < (pC->SensorRaw[BLUE])) ||
									(((pC->SensorRaw[GREEN]) + ((pC->SensorRaw[GREEN]) >> 3)) < (pC->SensorRaw[BLUE])))
								{
									Result = (DATAF)BLUECOLOR;
								}
								else
								{
									Result = (DATAF)WHITECOLOR;
								}
							}
						}
					}
				}
			}


			return (Result);
		}

		public RESULT cInputGetColor(DATA8 Device, DATA8* pData)
		{
			RESULT Result = RESULT.FAIL;

			cInputCalibrateColor((COLORSTRUCT*)(*GH.InputInstance.pAnalog).NxtCol[Device], ((COLORSTRUCT*)(*GH.InputInstance.pAnalog).NxtCol)[Device].SensorRaw);

			switch (GH.InputInstance.DeviceMode[Device])
			{
				case 2:
					{ // NXT-COL-COL

						((float*)pData)[0] = cInputCalculateColor((COLORSTRUCT*)(*GH.InputInstance.pAnalog).NxtCol[Device]);
						Result = OK;
					}
					break;

				case 1:
					{ // NXT-COL-AMB

						Memcpy((byte*)pData, (byte*)&((COLORSTRUCT*)(*GH.InputInstance.pAnalog).NxtCol)[Device].ADRaw[BLANK], 2);
						Result = OK;
					}
					break;

				case 0:
					{ // NXT-COL-RED

						Memcpy((byte*)pData, (byte*)&((COLORSTRUCT*)(*GH.InputInstance.pAnalog).NxtCol)[Device].ADRaw[RED], 2);
						Result = OK;
					}
					break;

				case 3:
					{ // NXT-COL-GRN

						Memcpy((byte*)pData, (byte*)&((COLORSTRUCT*)(*GH.InputInstance.pAnalog).NxtCol)[Device].ADRaw[GREEN], 2);
						Result = OK;
					}
					break;

				case 4:
					{ // NXT-COL-BLU

						Memcpy((byte*)pData, (byte*)&((COLORSTRUCT*)(*GH.InputInstance.pAnalog).NxtCol)[Device].ADRaw[BLUE], 2);
						Result = OK;
					}
					break;

				case 5:
					{ // NXT-COL-RAW

						Memcpy((byte*)pData, (byte*)&((COLORSTRUCT*)(*GH.InputInstance.pAnalog).NxtCol)[Device].SensorRaw[0], (COLORS * 2));
						Result = OK;
					}
					break;
			}

			return (Result);
		}

		public DATAF cInputGetColor(DATA8 Device, DATA8 Index)
		{
			DATAF Result;

			Result = DATAF_NAN;

			cInputCalibrateColor(&((COLORSTRUCT*)(*GH.InputInstance.pAnalog).NxtCol)[Device], ((COLORSTRUCT*)(*GH.InputInstance.pAnalog).NxtCol)[Device].SensorRaw);

			switch (GH.InputInstance.DeviceMode[Device])
			{
				case 2:
					{ // NXT-COL-COL

						Result = cInputCalculateColor(&((COLORSTRUCT*)(*GH.InputInstance.pAnalog).NxtCol)[Device]);
					}
					break;

				case 1:
					{ // NXT-COL-AMB

						Result = ((COLORSTRUCT*)(*GH.InputInstance.pAnalog).NxtCol)[Device].ADRaw[BLANK];
					}
					break;

				case 0:
					{ // NXT-COL-RED

						Result = ((COLORSTRUCT*)(*GH.InputInstance.pAnalog).NxtCol)[Device].ADRaw[RED];
					}
					break;

				case 3:
					{ // NXT-COL-GRN

						Result = ((COLORSTRUCT*)(*GH.InputInstance.pAnalog).NxtCol)[Device].ADRaw[GREEN];
					}
					break;

				case 4:
					{ // NXT-COL-BLU

						Result = ((COLORSTRUCT*)(*GH.InputInstance.pAnalog).NxtCol)[Device].ADRaw[BLUE];
					}
					break;

				case 5:
					{ // NXT-COL-RAW

						if (Index < COLORS)
						{
							Result = (DATAF)((COLORSTRUCT*)(*GH.InputInstance.pAnalog).NxtCol)[Device].SensorRaw[Index];
						}
					}
					break;
			}

			return (Result);
		}

		public RESULT cInputSetChainedDeviceType(DATA8 Layer, DATA8 Port, DATA8 Type, DATA8 Mode)
		{
			RESULT Result = RESULT.FAIL;
			DATA8 Device;

			Result = cInputCompressDevice(&Device, (byte)Layer, (byte)Port);
			if (Result == OK)
			{ // Device valid

				if ((GH.InputInstance.DeviceType[Device] > 0) && (GH.InputInstance.DeviceType[Device] <= MAX_VALID_TYPE))
				{ // Device type valid

					if (Type != TYPE_UNKNOWN)
					{
						cInputSetDeviceType(Device, Type, Mode, 1385); // 1385 was a current line
					}
					GH.printf($"c_input   cInputSetDeviceType:     L={Layer} P={Port} T={Type} M={Mode}\r\n");
				}
			}
			else
			{
				GH.printf($"c_input   cInputSetDeviceType: RESULT.FAIL  L={Layer} P={Port} T={Type} M={Mode}\r\n");
			}

			return (Result);
		}


		public RESULT cInputComSetDeviceInfo(DATA8 Length, UBYTE* pInfo)
		{
			RESULT Result = RESULT.FAIL;
			TYPES* pType;

			if (GH.Daisy.cDaisyReady() != RESULT.BUSY)
			{
				Result = GH.Com.cComSetDeviceInfo(Length, pInfo);
			}
			else
			{
				Result = RESULT.BUSY;
			}

			pType = (TYPES*)pInfo;

			if (Result == OK)
			{
				GH.printf($"c_com     GH.Com.cComSetDeviceInfo:       l={Length} N={CommonHelper.GetString((sbyte*)(*pType).Name)}\r\n");
			}
			else
			{
				if (Result == RESULT.BUSY)
				{
					GH.printf($"c_com     GH.Com.cComSetDeviceInfo: RESULT.BUSY  l={Length} N={CommonHelper.GetString((sbyte*)(*pType).Name)}\r\n");
				}
				else
				{
					GH.printf($"c_com     GH.Com.cComSetDeviceInfo: RESULT.FAIL  l={Length} N={CommonHelper.GetString((sbyte*)(*pType).Name)}\r\n");
				}
			}

			return (Result);
		}


		public RESULT cInputComGetDeviceInfo(DATA8 Length, UBYTE* pInfo)
		{
			RESULT Result = RESULT.FAIL;
			TYPES* pType;

            GH.Ev3System.Logger.LogInfo($"info: {(int)pInfo}");

            GH.Ev3System.Logger.LogInfo("Calling cComGetDeviceInfo in cInputComGetDeviceInfo");
			Result = GH.Com.cComGetDeviceInfo(Length, pInfo);

            GH.Ev3System.Logger.LogInfo("after Calling cComGetDeviceInfo in cInputComGetDeviceInfo");
            if (Result == OK)
			{
                GH.Ev3System.Logger.LogInfo($"info: {(int)pInfo}");
                pType = (TYPES*)pInfo;
				GH.printf($"c_com     GH.Com.cComGetDeviceInfo:       C={(*pType).Connection} N={CommonHelper.GetString((sbyte*)(*pType).Name)}\r\n");
			}

			GH.Ev3System.Logger.LogInfo("After shite in cInputComGetDeviceInfo");
			return (Result);
		}


		public RESULT cInputComSetDeviceType(DATA8 Layer, DATA8 Port, DATA8 Type, DATA8 Mode)
		{
			RESULT Result = RESULT.FAIL;

			Result = GH.Com.cComSetDeviceType(Layer, Port, Type, Mode);

			if (Result == OK)
			{
				GH.printf($"c_com     GH.Com.cComSetDeviceType:       L={Layer} P={Port} T={Type} M={Mode}\r\n");
			}
			else
			{
				if (Result == RESULT.BUSY)
				{
					GH.printf($"c_com     GH.Com.cComSetDeviceType: RESULT.BUSY  L={Layer} P={Port} T={Type} M={Mode}\r\n");
				}
				else
				{
					GH.printf("c_com     GH.Com.cComSetDeviceType: RESULT.FAIL  L={Layer} P={Port} T={Type} M={Mode}\r\n");
				}
			}

			return (Result);
		}


		public RESULT cInputComGetDeviceData(DATA8 Layer, DATA8 Port, DATA8 Length, DATA8* pType, DATA8* pMode, DATA8* pData)
		{
			RESULT Result = RESULT.FAIL;

			Result = GH.Com.cComGetDeviceData(Layer, Port, Length, pType, pMode, pData);

			if (Result == OK)
			{
				GH.printf($"c_com     GH.Com.cComGetDeviceData:       L={Layer} P={Port} T={*pType} M={*pMode} 0x{*pData}\r\n");
			}
			else
			{
				if (Result == RESULT.BUSY)
				{
					GH.printf($"c_com     GH.Com.cComGetDeviceData: RESULT.BUSY  L={Layer} P={Port} T={*pType} M={*pMode}\r\n");
				}
				else
				{
					GH.printf($"c_com     GH.Com.cComGetDeviceData: RESULT.FAIL  L={Layer} P={Port} T={*pType} M={*pMode}\r\n");
				}
			}

			return (Result);
		}


		public RESULT cInputComGetDeviceType(DATA8 Layer, DATA8 Port, DATA8 Length, DATA8* pType, DATA8* pMode, DATA8* pData)
		{
			RESULT Result = RESULT.FAIL;

			Result = GH.Com.cComGetDeviceData(Layer, Port, Length, pType, pMode, pData);

			return (Result);
		}


		public RESULT cInputGetDeviceData(DATA8 Layer, DATA8 Port, DATA8 Length, DATA8* pType, DATA8* pMode, DATA8* pData)
		{
			RESULT Result;

			Result = cInputGetData(Layer, Port, 0, null, Length, pType, pMode, pData);

			if (Result == OK)
			{
				GH.printf($"c_com     cInputGetDeviceData:    L={Layer} P={Port} T={*pType} M={*pMode}\r\n");
			}
			else
			{
				if (Result == RESULT.BUSY)
				{
					GH.printf($"c_com     cInputGetDeviceData: RESULT.BUSY L={Layer} P={Port} T={*pType} M={*pMode}\r\n");
				}
				else
				{
					GH.printf($"c_com     cInputGetDeviceData: RESULT.FAIL L={Layer} P={Port} T={*pType} M={*pMode}\r\n");
				}
			}

			return (Result);
		}

		UWORD Cnt_cInputGetData;
		UWORD Old_cInputGetData;
		public RESULT cInputGetData(DATA8 Layer, DATA8 Port, DATA16 Time, DATA16* pInit, DATA8 Length, DATA8* pType, DATA8* pMode, DATA8* pData)
		{
			RESULT Result = RESULT.FAIL;
			DATA8 Device;

			if (cInputCompressDevice(&Device, (byte)Layer, (byte)Port) == OK)
			{ // Device valid

				GH.printf($"c_input   cInputGetDeviceData      D={Device} L={Layer} P={Port} l={Length}\r\n");

				if (Length >= MAX_DEVICE_DATALENGTH)
				{ // Length OK

					CommonHelper.memset((byte*)pData, 0, Length);

					*pType = GH.InputInstance.DeviceType[Device];
					*pMode = GH.InputInstance.DeviceMode[Device];

					if ((GH.InputInstance.DeviceData[Device].Connection != CONN_NONE) && (GH.InputInstance.DeviceData[Device].Connection != CONN_ERROR))
					{ // Device connected

						if (Layer == 0)
						{ // Device local

							if (Port < INPUT_DEVICES)
							{ // Device is input device

								if ((Time < 0) || (Time > DEVICE_LOGBUF_SIZE))
								{
									Time = 0;
								}

								if (GH.InputInstance.DeviceData[Device].Connection == CONN_INPUT_UART)
								{ // Device is an UART device

									Memcpy((byte*)pData, (byte*)(*GH.InputInstance.pUart).GetRaw(Device),UART_DATA_LENGTH);
									Result = OK;
								}
								else
								{ // Device is not an UART device

									if (GH.InputInstance.DeviceData[Device].Connection == CONN_NXT_IIC)
									{ // Device is an IIC device
										Memcpy((byte*)pData, (byte*)(*GH.InputInstance.pIic).GetRaw(Device),IIC_DATA_LENGTH);										
										Result = OK;
									}
									else
									{ // Device is not an IIC device

										if (GH.InputInstance.DeviceData[Device].Connection == CONN_INPUT_DUMB)
										{ // Device is a new dumb input device

											if ((GH.InputInstance.DeviceType[Device] == 16) && (GH.InputInstance.DeviceMode[Device] == 1))
											{
												Memcpy((byte*)pData, (byte*)&GH.InputInstance.DeviceData[Device].Changes, 4);
											}
											else
											{
												Memcpy((byte*)pData, (byte*)&(*GH.InputInstance.pAnalog).InPin6[Device], 2);
											}
											Result = OK;
										}
										else
										{
											if (GH.InputInstance.DeviceData[Device].Connection == CONN_NXT_COLOR)
											{ // Device is a nxt color sensor

												Result = cInputGetColor(Device, pData);
											}
											else
											{ // Device is an old dumb input device

												if ((GH.InputInstance.DeviceType[Device] == 1) && (GH.InputInstance.DeviceMode[Device] == 1))
												{
													Memcpy((byte*)pData, (byte*)&GH.InputInstance.DeviceData[Device].Bumps, 4);
												}
												else
												{
													Memcpy((byte*)pData, (byte*)&(*GH.InputInstance.pAnalog).InPin1[Device], 2);
												}
												Result = OK;
											}
										}
									}
								}
							}
							else
							{ // Device is output device

								if (GH.InputInstance.DeviceMode[Device] == 2)
								{
									Memcpy((byte*)pData, (byte*)&GH.OutputInstance.pMotor[Device - INPUT_DEVICES].Speed, 1);
									Result = OK;
								}
								else
								{
									Memcpy((byte*)pData, (byte*)&GH.OutputInstance.pMotor[Device - INPUT_DEVICES].TachoSensor, 4);
									Result = OK;
								}

								GH.InputInstance.DeviceData[Device].DevStatus = OK;
							}
							if (Result == OK)
							{
								Result = GH.InputInstance.DeviceData[Device].DevStatus;
							}

						}
						else
						{ // Device is daisy chained

							Result = cInputComGetDeviceData(Layer, Port, MAX_DEVICE_DATALENGTH, pType, pMode, pData);

						}
					}
				}
			}

			return (Result);
		}

		public DATAF cInputReadDeviceRaw(DATA8 Device, DATA8 Index, DATA16 Time, DATA16* pInit)
		{
			DATAF Result;
			DATA8 DataSets;
			void* pResult;

			Result = DATAF_NAN;

			GH.printf($"c_input   cInputReadDeviceRaw:     D={Device} B={GH.InputInstance.DeviceData[Device].DevStatus}\r\n");
			if ((Device >= 0) && (Device < DEVICES) && (Index >= 0) && (Index < MAX_DEVICE_DATASETS))
			{
				// Parameters are valid

				if (GH.InputInstance.DeviceData[Device].DevStatus == OK)
				{

					if (Device < INPUTS)
					{
						// Device is a local sensor

						if ((GH.InputInstance.DeviceData[Device].Connection != CONN_NONE) && (GH.InputInstance.DeviceData[Device].Connection != CONN_ERROR))
						{
							// Device is connected right

							if (GH.InputInstance.DeviceData[Device].Connection == CONN_INPUT_UART)
							{
								// Device is a UART sensor

								pResult = (void*)(*GH.InputInstance.pUart).GetRaw(Device);
								DataSets = GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Device].TypeIndex].DataSets;

								if (Index < DataSets)
								{
									switch (GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Device].TypeIndex].Format & 0x0F)
									{
										case DATA_8:
											{
												GH.InputInstance.DeviceData[Device].Raw[Index] = *(((DATA8*)pResult) + Index);
											}
											break;

										case DATA_16:
											{
												GH.InputInstance.DeviceData[Device].Raw[Index] = *(((DATA16*)pResult) + Index);
											}
											break;

										case DATA_32:
											{
												GH.InputInstance.DeviceData[Device].Raw[Index] = *(((DATA32*)pResult) + Index);
											}
											break;

										case DATA_F:
											{
												GH.InputInstance.DeviceData[Device].Raw[Index] = *(((DATAF*)pResult) + Index);
											}
											break;

										default:
											{
												GH.InputInstance.DeviceData[Device].Raw[Index] = DATAF_NAN;
											}
											break;

									}
								}
								else
								{
									GH.InputInstance.DeviceData[Device].Raw[Index] = DATAF_NAN;
								}
							}
							else
							{
								// Device is not a UART sensor

								if (GH.InputInstance.DeviceData[Device].Connection == CONN_INPUT_DUMB)
								{
									// Device is new dumb

									GH.InputInstance.DeviceData[Device].Raw[Index] = (DATAF)(*GH.InputInstance.pAnalog).InPin6[Device];
								}
								else
								{
									if (GH.InputInstance.DeviceData[Device].Connection == CONN_NXT_COLOR)
									{
										// Device is nxt color

										GH.InputInstance.DeviceData[Device].Raw[Index] = (DATAF)cInputGetColor(Device, Index);
									}
									else
									{
										// Device is old dumb

										GH.InputInstance.DeviceData[Device].Raw[Index] = (DATAF)(*GH.InputInstance.pAnalog).InPin1[Device];
									}
								}
							}
						}
						else
						{
							GH.InputInstance.DeviceData[Device].Raw[Index] = DATAF_NAN;
						}
					}
					if ((Device >= INPUT_DEVICES) && (Device < (INPUT_DEVICES + OUTPUTS)))
					{
						// Device is connected on output port

						if ((GH.InputInstance.DeviceData[Device].Connection == CONN_NONE) || (GH.InputInstance.DeviceData[Device].Connection == CONN_ERROR))
						{
							GH.InputInstance.DeviceData[Device].Raw[Index] = DATAF_NAN;
						}
						else
						{
							if (GH.InputInstance.DeviceMode[Device] == 2)
							{
								GH.InputInstance.DeviceData[Device].Raw[Index] = (DATAF)GH.OutputInstance.pMotor[Device - INPUT_DEVICES].Speed;
							}
							else
							{
								GH.InputInstance.DeviceData[Device].Raw[Index] = (DATAF)GH.OutputInstance.pMotor[Device - INPUT_DEVICES].TachoSensor;
							}
						}

					}
					Result = GH.InputInstance.DeviceData[Device].Raw[Index];
				}
			}

			return (Result);
		}

		public void cInputWriteDeviceRaw(DATA8 Device, DATA8 Connection, DATA8 Type, DATAF DataF)
		{
			UBYTE Byte;
			UWORD Word;

			if (Device < INPUTS)
			{
				if (Type == TYPE_KEEP)
				{
					Type = GH.InputInstance.DeviceType[Device];
				}
				if (GH.InputInstance.DeviceType[Device] != Type)
				{
					GH.InputInstance.DeviceData[Device].Connection = Connection;
					cInputSetDeviceType(Device, Type, 0, 1808); // 1808 was a current line
				}
				if (GH.InputInstance.DeviceData[Device].Connection == CONN_INPUT_UART)
				{
				}
				else
				{
					if (GH.InputInstance.DeviceData[Device].Connection == CONN_NXT_IIC)
					{
						Byte = (UBYTE)DataF;
						Word = (UWORD)DataF;


						if ((GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Device].TypeIndex].Format & 0x0F) == 0)
						{
							(*GH.InputInstance.pIic).GetRaw(Device)[0] = (sbyte)Byte;
							(*GH.InputInstance.pIic).GetRaw(Device)[1] = 0;
						}
						else
						{
							(*GH.InputInstance.pIic).GetRaw(Device)[0] = (sbyte)Word;
							(*GH.InputInstance.pIic).GetRaw(Device)[1] = (sbyte)(Word >> 8);
						}
						GH.InputInstance.DeviceData[Device].DevStatus = OK;
					}
					else
					{
						if (GH.InputInstance.DeviceData[Device].Connection == CONN_INPUT_DUMB)
						{
							(*GH.InputInstance.pAnalog).InPin6[Device] = (DATA16)DataF;
						}
						else
						{
							(*GH.InputInstance.pAnalog).InPin1[Device] = (DATA16)DataF;
						}
						GH.InputInstance.DeviceData[Device].DevStatus = OK;
					}
				}
			}
			if ((Device >= INPUT_DEVICES) && (Device < (INPUT_DEVICES + OUTPUTS)))
			{
				if (Type == TYPE_KEEP)
				{
					Type = GH.InputInstance.DeviceType[Device];
				}
				if (GH.InputInstance.DeviceType[Device] != Type)
				{
					GH.InputInstance.DeviceData[Device].Connection = CONN_UNKNOWN;
					cInputSetDeviceType(Device, Type, 0, 1856); // 1856 was a currrent line
				}
				if (GH.InputInstance.DeviceMode[Device] == 2)
				{
					GH.OutputInstance.pMotor[Device - INPUT_DEVICES].Speed = (sbyte)(DATA32)DataF;
				}
				else
				{
					GH.OutputInstance.pMotor[Device - INPUT_DEVICES].TachoSensor = (DATA32)DataF;
				}
				GH.InputInstance.DeviceData[Device].DevStatus = OK;
			}
		}


		public DATA8 cInputReadDevicePct(DATA8 Device, DATA8 Index, DATA16 Time, DATA16* pInit)
		{
			DATA8 Result = DATA8_NAN;
			UWORD TypeIndex;
			DATAF Raw;
			DATA8 Type;
			DATA8 Mode;
			DATAF Min;
			DATAF Max;
			DATAF Pct;

			Raw = cInputReadDeviceRaw(Device, Index, Time, pInit);

			if (!(float.IsNaN(Raw)))
			{
				TypeIndex = GH.InputInstance.DeviceData[Device].TypeIndex;

				Type = GH.InputInstance.TypeData[TypeIndex].Type;
				Mode = GH.InputInstance.TypeData[TypeIndex].Mode;
				Min = GH.InputInstance.TypeData[TypeIndex].RawMin;
				Max = GH.InputInstance.TypeData[TypeIndex].RawMax;

				if ((Type > 0) && (Type < (MAX_DEVICE_TYPE + 1)) && (Mode >= 0) && (Mode < MAX_DEVICE_MODES))
				{
					if (GH.InputInstance.Calib[Type][Mode].InUse != 0)
					{
						Min = GH.InputInstance.Calib[Type][Mode].Min;
						Max = GH.InputInstance.Calib[Type][Mode].Max;
					}
				}

				Pct = (((Raw - Min) * (GH.InputInstance.TypeData[TypeIndex].PctMax - GH.InputInstance.TypeData[TypeIndex].PctMin)) / (Max - Min) + GH.InputInstance.TypeData[TypeIndex].PctMin);

				if (Pct > GH.InputInstance.TypeData[TypeIndex].PctMax)
				{
					Pct = GH.InputInstance.TypeData[TypeIndex].PctMax;
				}
				if (Pct < GH.InputInstance.TypeData[TypeIndex].PctMin)
				{
					Pct = GH.InputInstance.TypeData[TypeIndex].PctMin;
				}
				Result = (DATA8)Pct;
			}

			return (Result);
		}


		public DATAF cInputReadDeviceSi(DATA8 Device, DATA8 Index, DATA16 Time, DATA16* pInit)
		{
			UWORD TypeIndex;
			DATAF Raw;
			DATA8 Type;
			DATA8 Mode;
			DATA8 Connection;
			DATAF Min;
			DATAF Max;

			Raw = cInputReadDeviceRaw(Device, Index, Time, pInit);

			if (!(float.IsNaN(Raw)))
			{
				TypeIndex = GH.InputInstance.DeviceData[Device].TypeIndex;

				Type = GH.InputInstance.TypeData[TypeIndex].Type;
				Mode = GH.InputInstance.TypeData[TypeIndex].Mode;
				Min = GH.InputInstance.TypeData[TypeIndex].RawMin;
				Max = GH.InputInstance.TypeData[TypeIndex].RawMax;

				if ((Type > 0) && (Type < (MAX_DEVICE_TYPE + 1)) && (Mode >= 0) && (Mode < MAX_DEVICE_MODES))
				{
					if (GH.InputInstance.Calib[Type][Mode].InUse != 0)
					{
						Min = GH.InputInstance.Calib[Type][Mode].Min;
						Max = GH.InputInstance.Calib[Type][Mode].Max;
					}
				}

				Raw = (((Raw - Min) * (GH.InputInstance.TypeData[TypeIndex].SiMax - GH.InputInstance.TypeData[TypeIndex].SiMin)) / (Max - Min) + GH.InputInstance.TypeData[TypeIndex].SiMin);

				// Limit values on dumb connections if "pct" or "_"
				Connection = GH.InputInstance.TypeData[TypeIndex].Connection;
				if ((Connection == CONN_NXT_DUMB) || (Connection == CONN_INPUT_DUMB) || (Connection == CONN_OUTPUT_DUMB) || (Connection == CONN_OUTPUT_TACHO))
				{
					if ((GH.InputInstance.TypeData[TypeIndex].Symbol[0] == 'p') || (GH.InputInstance.TypeData[TypeIndex].Symbol[0] == ' ') || (GH.InputInstance.TypeData[TypeIndex].Symbol[0] == 0))
					{
						if (Raw > GH.InputInstance.TypeData[TypeIndex].SiMax)
						{
							Raw = GH.InputInstance.TypeData[TypeIndex].SiMax;
						}
						if (Raw < GH.InputInstance.TypeData[TypeIndex].SiMin)
						{
							Raw = GH.InputInstance.TypeData[TypeIndex].SiMin;
						}
					}
				}

			}

			return (Raw);
		}


		public RESULT cInputCheckUartInfo(UBYTE Port)
		{
			RESULT Result = RESULT.BUSY;
			TYPES* pTmp;


			if (GH.InputInstance.UartFile >= MIN_HANDLE)
			{ // Driver installed

				if (((*GH.InputInstance.pUart).Status[Port] & UART_PORT_CHANGED) != 0)
				{ // something has changed

					if (GH.InputInstance.TmpMode[Port] > 0)
					{ // check each mode

						GH.InputInstance.TmpMode[Port]--;

						// Get info
						(*GH.InputInstance.UartCtl).Port = (sbyte)Port;
						(*GH.InputInstance.UartCtl).Mode = GH.InputInstance.TmpMode[Port];
						GH.Ev3System.InputHandler.IoctlUart(UART_READ_MODE_INFO, (*GH.InputInstance.UartCtl));

						if ((*GH.InputInstance.UartCtl).TypeData.Name[0] != 0)
						{ // Info available

							Result = cInputGetNewTypeDataPointer((sbyte*)(*GH.InputInstance.UartCtl).TypeData.Name, (*GH.InputInstance.UartCtl).TypeData.Type, (*GH.InputInstance.UartCtl).TypeData.Mode, CONN_INPUT_UART, &pTmp);
							if (pTmp != null)
							{ // Tabel index found

								if (GH.InputInstance.DeviceType[Port] == TYPE_UNKNOWN)
								{ // Use first mode info to set type

									GH.InputInstance.DeviceType[Port] = (*GH.InputInstance.UartCtl).TypeData.Type;
								}

								if (Result == OK)
								{ // New mode

									// Insert in tabel
									Memcpy((byte*)pTmp, (byte*)Unsafe.AsPointer(ref (*GH.InputInstance.UartCtl).TypeData), sizeof(TYPES));

								}
								if (cInputComSetDeviceInfo(MAX_DEVICE_INFOLENGTH, (UBYTE*)(byte*)Unsafe.AsPointer(ref (*GH.InputInstance.UartCtl).TypeData)) == RESULT.BUSY)
								{ // Chain not ready - roll back

                                    GH.Ev3System.InputHandler.IoctlUart(UART_NACK_MODE_INFO, (*GH.InputInstance.UartCtl));
									GH.InputInstance.TmpMode[Port]++;
								}

								GH.printf($"P={Port} T={(*GH.InputInstance.UartCtl).TypeData.Type} M={(*GH.InputInstance.UartCtl).TypeData.Mode} N={CommonHelper.GetString((sbyte*)(*GH.InputInstance.UartCtl).TypeData.Name)}\r\n");
							}
						}
					}
					else
					{ // All modes received set device mode 0

						(*GH.InputInstance.UartCtl).Port = (sbyte)Port;
                        GH.Ev3System.InputHandler.IoctlUart(UART_CLEAR_CHANGED, (*GH.InputInstance.UartCtl));
						(*GH.InputInstance.pUart).Status[Port] &= ~UART_PORT_CHANGED;
						cInputSetDeviceType((sbyte)Port, GH.InputInstance.DeviceType[Port], 0, 2034); // 2034 was a current line
					}
				}

				if (((*GH.InputInstance.pUart).Status[Port] & UART_DATA_READY) != 0)
				{
					if (((*GH.InputInstance.pUart).Status[Port] & UART_PORT_CHANGED) == 0)
					{
						Result = OK;
					}
				}
			}

			return (Result);
		}


		public RESULT cInputCheckIicInfo(UBYTE Port)
		{
			RESULT Result = RESULT.BUSY;
			DATA8 Type;
			DATA8 Mode;
			UWORD Index;

			if (GH.InputInstance.IicFile >= MIN_HANDLE)
			{ // Driver installed

				if ((*GH.InputInstance.pAnalog).InDcm[Port] == TYPE_NXT_IIC)
				{

					if ((*GH.InputInstance.pIic).Changed[Port] != 0)
					{ // something has changed

						(*GH.InputInstance.IicStr).Port = (sbyte)Port;

                        GH.Ev3System.InputHandler.IoctlI2c(IIC_READ_TYPE_INFO, (*GH.InputInstance.IicStr));

						Index = IIC_NAME_LENGTH;
						while ((Index != 0) && (((*GH.InputInstance.IicStr).Manufacturer[Index] == ' ') || ((*GH.InputInstance.IicStr).Manufacturer[Index] == 0)))
						{
							(*GH.InputInstance.IicStr).Manufacturer[Index] = 0;
							Index--;
						}
						Index = IIC_NAME_LENGTH;
						while ((Index != 0) && (((*GH.InputInstance.IicStr).SensorType[Index] == ' ') || ((*GH.InputInstance.IicStr).SensorType[Index] == 0)))
						{
							(*GH.InputInstance.IicStr).SensorType[Index] = 0;
							Index--;
						}

						// Find 3th party type
						Type = TYPE_IIC_UNKNOWN;
						Mode = 0;
						Index = 0;
						while ((Index < GH.InputInstance.IicDeviceTypes) && (Type == TYPE_IIC_UNKNOWN))
						{ // Check list

                            if (CommonHelper.strcmp((DATA8*)(*GH.InputInstance.IicStr).Manufacturer, (DATA8*)GH.InputInstance.IicString[Index].Manufacturer) == 0)
							{ // Manufacturer found

                                if (CommonHelper.strcmp((DATA8*)(*GH.InputInstance.IicStr).SensorType, (DATA8*)GH.InputInstance.IicString[Index].SensorType) == 0)
								{ // Type found

									Type = GH.InputInstance.IicString[Index].Type;
									Mode = GH.InputInstance.IicString[Index].Mode;
								}
							}
							Index++;
						}
						cInputSetDeviceType((sbyte)Port, Type, Mode, 2106); // 2106 was a current line
					}
					(*GH.InputInstance.pIic).Changed[Port] = 0;
				}
				if (((*GH.InputInstance.pIic).Status[Port] & IIC_DATA_READY) != 0)
				{
					Result = OK;
				}
				else
				{
					Result = RESULT.BUSY;
				}
			}

			return (Result);
		}


		/*! \brief    Update Device Types
		 *
		 *            Called when the VM read the device list
		 *
		 */
		public void cInputDcmUpdate(UWORD Time)
		{
			RESULT Result = RESULT.BUSY;
			DATA8 Device;
			DATA8 Port;
			TYPES* Tmp = CommonHelper.PointerStruct<TYPES>();
			TYPES* pTmp;
			DATA16 Index;
			DATA8 Layer;
			DATA8 Output;
			DATA8 Type;
			DATA8 Mode;

			if (GH.InputInstance.DCMUpdate != 0)
			{
				for (Device = 0; Device < DEVICES; Device++)
				{

					if ((Device >= 0) && (Device < INPUTS))
					{ // Device is local input port

						Port = Device;

						if (GH.InputInstance.DeviceData[Device].Connection != (*GH.InputInstance.pAnalog).InConn[Port])
						{ // Connection type has changed

							GH.InputInstance.DeviceData[Device].Connection = (*GH.InputInstance.pAnalog).InConn[Port];
							cInputSetDeviceType(Device, (*GH.InputInstance.pAnalog).InDcm[Port], 0, 2156); // 2156 was a current line
							GH.InputInstance.DeviceMode[Device] = 0;
							GH.InputInstance.TmpMode[Device] = MAX_DEVICE_MODES;
							GH.InputInstance.DeviceData[Device].DevStatus = RESULT.BUSY;
						}

						if (GH.InputInstance.DeviceData[Device].Connection == CONN_INPUT_UART)
						{ // UART device

							Result = cInputCheckUartInfo((byte)Port);

						}
						else
						{
							if (GH.InputInstance.DeviceData[Device].Connection == CONN_NXT_IIC)
							{ // IIC device

								Result = cInputCheckIicInfo((byte)Port);
							}
							else
							{ // Analogue device

								if ((*GH.InputInstance.pAnalog).Updated[Device] != 0)
								{
									Result = OK;
								}
							}
						}
					}
					else
					{
						if ((Device >= INPUT_DEVICES) && (Device < (INPUT_DEVICES + OUTPUTS)))
						{ // Device is local output port

							Port = (sbyte)(Device - INPUT_DEVICES);

							if (GH.InputInstance.DeviceData[Device].Connection != (*GH.InputInstance.pAnalog).OutConn[Port])
							{ // Connection type has changed

								GH.InputInstance.DeviceData[Device].Connection = (*GH.InputInstance.pAnalog).OutConn[Port];
								cInputSetDeviceType(Device, (*GH.InputInstance.pAnalog).OutDcm[Port], 0, 2196); // 2196 was a current line
							}

							Result = OK;

						}
						else
						{ // Device is from daisy chain

							cInputExpandDevice(Device, &Layer, &Port, &Output);

							Result = cInputComGetDeviceType(Layer, Port, MAX_DEVICE_DATALENGTH, &Type, &Mode, (DATA8*)Tmp);

							if ((Type > 0) && (Type <= MAX_VALID_TYPE) && (Result != RESULT.FAIL))
							{
								GH.InputInstance.DeviceData[Device].Connection = CONN_DAISYCHAIN;
							}
							else
							{
								Type = TYPE_NONE;
								GH.InputInstance.DeviceData[Device].Connection = CONN_NONE;
							}

							if (GH.InputInstance.DeviceType[Device] != Type)
							{
								cInputSetDeviceType(Device, Type, 0, 2221); // 2221 was a current line
							}
						}
					}

					if ((GH.InputInstance.DeviceData[Device].Connection == CONN_NONE) || (GH.InputInstance.DeviceData[Device].Connection == CONN_ERROR))
					{
						GH.InputInstance.DeviceData[Device].DevStatus = RESULT.BUSY;
					}
					else
					{
						if (GH.InputInstance.DeviceData[Device].InvalidTime >= Time)
						{
							GH.InputInstance.DeviceData[Device].InvalidTime -= Time;
							GH.InputInstance.DeviceData[Device].DevStatus = RESULT.BUSY;
						}
						else
						{
							GH.InputInstance.DeviceData[Device].InvalidTime = 0;
							if (Result == OK)
							{
								GH.InputInstance.DeviceData[Device].DevStatus = OK;

								GH.printf($"D={(int)Device} M={GH.InputInstance.DeviceMode[Device]} OK\r\n");
							}
							else
							{
								GH.InputInstance.DeviceData[Device].DevStatus = RESULT.BUSY;
							}
						}
					}
					if (GH.InputInstance.DeviceData[Device].TimeoutTimer >= Time)
					{
						GH.InputInstance.DeviceData[Device].TimeoutTimer -= Time;
					}
					else
					{
						GH.InputInstance.DeviceData[Device].TimeoutTimer = 0;
					}


				}
			}

			GH.Ev3System.Logger.LogInfo("Calling cInputComGetDeviceInfo in cInputDcmUpdate");
			if (cInputComGetDeviceInfo(MAX_DEVICE_INFOLENGTH, (UBYTE*)Tmp) == OK)
			{
				Result = cInputGetNewTypeDataPointer((sbyte*)(*Tmp).Name, (*Tmp).Type, (*Tmp).Mode, (*Tmp).Connection, &pTmp);
				if (pTmp != null)
				{
					if (Result == OK)
					{
						(*pTmp) = *Tmp;
						GH.printf($"c_input   cInputDcmUpdate: NEW     T={(*Tmp).Type} M={(*Tmp).Mode} C={(*Tmp).Connection} N={CommonHelper.GetString((sbyte*)(*Tmp).Name)}\r\n");
					}
					else
					{
						GH.printf($"c_input   cInputDcmUpdate: KNOWN   T={(*Tmp).Type} M={(*Tmp).Mode} C={(*Tmp).Connection} N={CommonHelper.GetString((sbyte*)(*Tmp).Name)}\r\n");
					}
				}
			}
			GH.Ev3System.Logger.LogInfo("After call of cInputComGetDeviceInfo in cInputDcmUpdate");

			if (GH.InputInstance.TypeDataIndex < GH.InputInstance.MaxDeviceTypes)
			{ // Upload TypeData info through daisy chain

				if (GH.InputInstance.TypeDataTimer == 0)
				{
					Index = GH.InputInstance.TypeDataIndex;
					GH.InputInstance.TypeDataIndex++;

					if (GH.InputInstance.TypeData[Index].Name[0] != 0)
					{
						if (GH.InputInstance.TypeData[Index].Type <= MAX_VALID_TYPE)
						{ // Entry valid

							if (cInputComSetDeviceInfo(MAX_DEVICE_INFOLENGTH, (UBYTE*)&GH.InputInstance.TypeData[Index]) == RESULT.BUSY)
							{ // Roll back

								GH.InputInstance.TypeDataIndex--;

								GH.VMInstance.Status |= 0x10;
							}
							else
							{
								GH.VMInstance.Status &= ~0x10;
							}

						}
					}
				}
				else
				{
					if (GH.InputInstance.TypeDataTimer >= Time)
					{
						GH.InputInstance.TypeDataTimer -= (short)Time;
					}
					else
					{
						GH.InputInstance.TypeDataTimer = 0;
					}
				}
			}
			else
			{
				GH.InputInstance.TypeDataIndex = DATA16_MAX;
			}

            GH.Ev3System.Logger.LogInfo("After big if stmt in cInputDcmUpdate");

            GH.VMInstance.Status &= ~0x07;

			if (GH.InputInstance.DeviceData[TESTDEVICE].DevStatus == OK)
			{
				GH.VMInstance.Status |= 0x01;
			}
			else
			{
				if (GH.InputInstance.DeviceData[TESTDEVICE].DevStatus == RESULT.BUSY)
				{
					GH.VMInstance.Status |= 0x02;
				}
				else
				{
					GH.VMInstance.Status |= 0x04;
				}
			}

            GH.Ev3System.Logger.LogInfo("exit in cInputDcmUpdate");
        }


		public RESULT cInputStartTypeDataUpload()
		{
			GH.InputInstance.TypeDataIndex = 0;
			GH.InputInstance.TypeDataTimer = DELAY_TO_TYPEDATA;

			return (OK);
		}

		private DATAF* ValuecInputUpdate = (DATAF*)CommonHelper.AllocateByteArray(4);
		public void cInputUpdate(UWORD Time)
		{
			DATA8 Device;
			
			DATAF Diff;

			cInputDcmUpdate(Time);

			for (Device = 0; Device < INPUT_PORTS; Device++)
			{ // check each port for changes

				if ((GH.InputInstance.DeviceType[Device] == 1) || (GH.InputInstance.DeviceType[Device] == 16))
				{
					if (GH.InputInstance.DeviceType[Device] == 1)
					{
						*ValuecInputUpdate = (DATAF)(*GH.InputInstance.pAnalog).InPin1[Device];
					}
					if (GH.InputInstance.DeviceType[Device] == 16)
					{
						*ValuecInputUpdate = (DATAF)(*GH.InputInstance.pAnalog).InPin6[Device];
					}

					Diff = *ValuecInputUpdate - GH.InputInstance.DeviceData[Device].OldRaw;

					if (Diff >= (DATAF)500)
					{
						GH.InputInstance.DeviceData[Device].Bumps += (DATA32)1;
					}
					if (Diff <= -500)
					{
						GH.InputInstance.DeviceData[Device].Changes += (DATA32)1;
					}

					GH.InputInstance.DeviceData[Device].OldRaw = *ValuecInputUpdate;
				}
				else
				{
					GH.InputInstance.DeviceData[Device].Changes = (DATA32)0;
					GH.InputInstance.DeviceData[Device].Bumps = (DATA32)0;
					GH.InputInstance.DeviceData[Device].OldRaw = (DATAF)0;
				}
			}
		}


		public RESULT cInputInit()
		{
			RESULT Result = OK;
			ANALOG* pAdcTmp;
			UART* pUartTmp;
			IIC* pIicTmp;
			PRGID TmpPrgId;
			UWORD Tmp;
			UWORD Set;

			GH.InputInstance.TypeDataIndex = DATA16_MAX;

			GH.InputInstance.MaxDeviceTypes = 3;

			fixed (TYPES** tmpP = &GH.InputInstance.TypeData)
				GH.Memory.cMemoryMalloc((void**)tmpP, (DATA32)(sizeof(TYPES) * GH.InputInstance.MaxDeviceTypes));

			GH.InputInstance.IicDeviceTypes = 1;

            fixed (IICSTR** tmpP = &GH.InputInstance.IicString)
                GH.Memory.cMemoryMalloc((void**)tmpP, (DATA32)(sizeof(IICSTR) * GH.InputInstance.IicDeviceTypes));

			GH.InputInstance.pAnalog = (ANALOG*)GH.InputInstance.Analog;

			GH.InputInstance.pUart = (UART*)GH.InputInstance.Uart;

			GH.InputInstance.pIic = (IIC*)GH.InputInstance.Iic;
			// TODO: mapping shite (probably no need)

			//GH.InputInstance.AdcFile = open(ANALOG_DEVICE_NAME, O_RDWR | O_SYNC);
			//GH.InputInstance.UartFile = open(UART_DEVICE_NAME, O_RDWR | O_SYNC);
			//GH.InputInstance.DcmFile = open(DCM_DEVICE_NAME, O_RDWR | O_SYNC);
			//GH.InputInstance.IicFile = open(IIC_DEVICE_NAME, O_RDWR | O_SYNC);
			GH.InputInstance.DCMUpdate = 1;

			if (GH.InputInstance.AdcFile >= MIN_HANDLE)
			{
				//pAdcTmp = (ANALOG*)mmap(0, sizeof(ANALOG), PROT_READ | PROT_WRITE, MAP_FILE | MAP_SHARED, GH.InputInstance.AdcFile, 0);

				//if (pAdcTmp == MAP_FAILED)
				//{
				//	GH.Ev3System.Logger.LogError($"Error {nameof(ANALOG_SHARED_MEMORY)} occured in {Environment.StackTrace}");
				//	Result = RESULT.FAIL;
				//	GH.InputInstance.DCMUpdate = 0;
				//}
				//else
				//{
				//	GH.InputInstance.pAnalog = pAdcTmp;
				//}
			}
			else
			{
				GH.Ev3System.Logger.LogError($"Error {nameof(ANALOG_DEVICE_FILE_NOT_FOUND)} occured in {Environment.StackTrace}");
				// Result = RESULT.FAIL;
				GH.InputInstance.DCMUpdate = 0;
			}


			if (GH.InputInstance.UartFile >= MIN_HANDLE)
			{
				//pUartTmp = (UART*)mmap(0, sizeof(UART), PROT_READ | PROT_WRITE, MAP_FILE | MAP_SHARED, GH.InputInstance.UartFile, 0);

				//if (pUartTmp == MAP_FAILED)
				//{
				//	GH.Ev3System.Logger.LogError($"Error {nameof(UART_SHARED_MEMORY)} occured in {Environment.StackTrace}");
				//	Result = RESULT.FAIL;
				//	GH.InputInstance.DCMUpdate = 0;
				//}
				//else
				//{
				//	GH.InputInstance.pUart = pUartTmp;
				//}
			}
			else
			{
				GH.Ev3System.Logger.LogError($"Error {nameof(UART_DEVICE_FILE_NOT_FOUND)} occured in {Environment.StackTrace}");
				// Result = RESULT.FAIL;
				//    GH.InputInstance.DCMUpdate   =  0;
			}


			if (GH.InputInstance.IicFile >= MIN_HANDLE)
			{
				//pIicTmp = (IIC*)mmap(0, sizeof(UART), PROT_READ | PROT_WRITE, MAP_FILE | MAP_SHARED, GH.InputInstance.IicFile, 0);

				//if (pIicTmp == MAP_FAILED)
				//{
				//	GH.Ev3System.Logger.LogError($"Error {nameof(IIC_SHARED_MEMORY)} occured in {Environment.StackTrace}");
				//	Result = RESULT.FAIL;
				//	GH.InputInstance.DCMUpdate = 0;
				//}
				//else
				//{
				//	GH.InputInstance.pIic = pIicTmp;
				//}
			}
			else
			{
				GH.Ev3System.Logger.LogError($"Error {nameof(IIC_DEVICE_FILE_NOT_FOUND)} occured in {Environment.StackTrace}");
				// Result = RESULT.FAIL;
				//    GH.InputInstance.DCMUpdate   =  0;
			}


			cInputTypeDataInit();
			cInputCalDataInit();

			for (Tmp = 0; Tmp < DEVICES; Tmp++)
			{
				for (Set = 0; Set < MAX_DEVICE_DATASETS; Set++)
				{
					GH.InputInstance.DeviceData[Tmp].Raw[Set] = DATAF_NAN;
				}
				GH.InputInstance.DeviceData[Tmp].Owner = 0;
				GH.InputInstance.DeviceData[Tmp].Busy = 0;
				GH.InputInstance.DeviceData[Tmp].Connection = CONN_NONE;
				GH.InputInstance.DeviceData[Tmp].DevStatus = RESULT.BUSY;
				GH.InputInstance.DeviceData[Tmp].TypeIndex = GH.InputInstance.NoneIndex;
				GH.InputInstance.DeviceType[Tmp] = TYPE_NONE;
				GH.InputInstance.DeviceMode[Tmp] = 0;
				GH.InputInstance.DeviceData[Tmp].Changes = (DATA32)0;
				GH.InputInstance.DeviceData[Tmp].Bumps = (DATA32)0;
			}

			for (Tmp = 0; Tmp < INPUT_PORTS; Tmp++)
			{
				GH.InputInstance.TmpMode[Tmp] = MAX_DEVICE_MODES;
			}

			for (TmpPrgId = 0; TmpPrgId < MAX_PROGRAMS; TmpPrgId++)
			{
				GH.InputInstance.ConfigurationChanged[TmpPrgId] = 0;
			}

			return (Result);
		}


		public RESULT cInputOpen()
		{
			RESULT Result = RESULT.FAIL;

			Result = OK;

			return (Result);
		}


		public RESULT cInputClose()
		{
			RESULT Result = RESULT.FAIL;

			Result = OK;

			return (Result);
		}


		public RESULT cInputExit()
		{
			RESULT Result = RESULT.FAIL;

			cInputCalDataExit();

            // TODO: mapping shite (probably no need)
            //if (GH.InputInstance.AdcFile >= MIN_HANDLE)
            //{
            //	munmap(GH.InputInstance.pAnalog, sizeof(ANALOG));
            //	close(GH.InputInstance.AdcFile);
            //}

            //if (GH.InputInstance.UartFile >= MIN_HANDLE)
            //{
            //	munmap(GH.InputInstance.pUart, sizeof(UART));
            //	close(GH.InputInstance.UartFile);
            //}

            //if (GH.InputInstance.IicFile >= MIN_HANDLE)
            //{
            //	munmap(GH.InputInstance.pIic, sizeof(IIC));
            //	close(GH.InputInstance.IicFile);
            //}

            //if (GH.InputInstance.DcmFile >= MIN_HANDLE)
            //{
            //	close(GH.InputInstance.DcmFile);
            //}

            // TODO: memory free
            //if (GH.InputInstance.IicString != null)
            //{
            //	free((void*)GH.InputInstance.IicString);
            //}
            //if (GH.InputInstance.TypeData != null)
            //{
            //	free((void*)GH.InputInstance.TypeData);
            //}

            Result = OK;

			return (Result);
		}


		public DATA8 cInputGetDevice()
		{
			DATA8 Layer;
			DATA8 No;

			Layer = *(DATA8*)GH.Lms.PrimParPointer();
			No = *(DATA8*)GH.Lms.PrimParPointer();

			return ((sbyte)(No + (Layer * INPUT_PORTS)));
		}


		public void cInputSetType(DATA8 Device, DATA8 Type, DATA8 Mode, int Line)
		{
			if (GH.InputInstance.DeviceData[Device].DevStatus == OK)
			{
				if (Type == TYPE_KEEP)
				{
					Type = GH.InputInstance.DeviceType[Device];
				}
				if (Mode == MODE_KEEP)
				{ // Get actual mode

					Mode = GH.InputInstance.DeviceMode[Device];
				}
				if ((GH.InputInstance.DeviceType[Device] != Type) || (GH.InputInstance.DeviceMode[Device] != Mode))
				{
					cInputSetDeviceType(Device, Type, Mode, Line);
				}
			}
		}


		//******* BYTE CODE SNIPPETS **************************************************


		/*! \page cInput Input
		 *  <hr size="1"/>
		 *  <b>     opINPUT_DEVICE_LIST (LENGTH, ARRAY, CHANGED)  </b>
		 *
		 *- Read all available devices on input and output(chain)\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   LENGTH   - Maximum number of device types (normally 32)
		 *  \return (DATA8)   ARRAY    - First element of DATA8 array of types (normally 32)
		 *  \return (DATA8)   CHANGED  - Changed status
		 */
		/*! \brief  opINPUT_DEVICE_LIST byte code
		 *
		 */
		public void cInputDeviceList()
		{
			PRGID TmpPrgId;
			DATA8 Length;
			DATA8* pDevices;
			DATA8* pChanged;
			DATA8 Count;

			TmpPrgId = GH.Lms.CurrentProgramId();
			Length = *(DATA8*)GH.Lms.PrimParPointer();
			pDevices = (DATA8*)GH.Lms.PrimParPointer();
			pChanged = (DATA8*)GH.Lms.PrimParPointer();

			*pChanged = GH.InputInstance.ConfigurationChanged[TmpPrgId];
			GH.InputInstance.ConfigurationChanged[TmpPrgId] = 0;
			Count = 0;
			while ((Count < Length) && (Count < DEVICES))
			{
				pDevices[Count] = GH.InputInstance.DeviceType[Count];

				Count++;
			}
			if (Count < Length)
			{
				pDevices[Count] = 0;
			}
		}


		/*! \page cInput
		 *  <hr size="1"/>
		 *  <b>     opINPUT_DEVICE (CMD, .....)  </b>
		 *
		 *- Read information about device\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   CMD               - \ref inputdevicesubcode
		 *
		 *\n
		 *  - CMD = GET_TYPEMODE
		 *\n  Get device type and mode\n
		 *    -  \param  (DATA8)   LAYER        - Chain layer number [0..3]
		 *    -  \param  (DATA8)   NO           - Port number
		 *    -  \return (DATA8) \ref types "TYPE" - Device type
		 *    -  \return (DATA8)   MODE         - Device mode [0..7]
		 *
		 *\n
		 *  - CMD = GET_NAME
		 *\n  Get device name\n
		 *    -  \param  (DATA8)   LAYER        - Chain layer number [0..3]
		 *    -  \param  (DATA8)   NO           - Port number
		 *    -  \param  (DATA8)   LENGTH       - Maximal length of string returned (-1 = no check)\n
		 *    -  \return (DATA8)   DESTINATION  - String variable or handle to string\n
		 *
		 *\n
		 *  - CMD = GET_SYMBOL
		 *\n  Get device symbol\n
		 *    -  \param  (DATA8)   LAYER        - Chain layer number [0..3]
		 *    -  \param  (DATA8)   NO           - Port number
		 *    -  \param  (DATA8)   LENGTH       - Maximal length of string returned (-1 = no check)\n
		 *    -  \return (DATA8)   DESTINATION  - String variable or handle to string\n
		 *
		 *\n
		 *  - CMD = GET_FORMAT
		 *    -  \param  (DATA8)   LAYER        - Chain layer number [0..3]
		 *    -  \param  (DATA8)   NO           - Port number
		 *    -  \return (DATA8)   DATASETS     - Number of data sets\n
		 *    -  \return (DATA8)   FORMAT       - Format [0..3]\n
		 *    -  \return (DATA8)   MODES        - Number of modes [1..8]\n
		 *    -  \return (DATA8)   VIEWS        - Number of views [1..8]\n
		 *
		 *\n
		 *  - CMD = GET_RAW
		 *    -  \param  (DATA8)   LAYER        - Chain layer number [0..3]
		 *    -  \param  (DATA8)   NO           - Port number
		 *    -  \return (DATA32)  VALUE        - 32 bit raw value\n
		 *
		 *\n
		 *  - CMD = GET_MODENAME
		 *\n  Get device mode name\n
		 *    -  \param  (DATA8)   LAYER        - Chain layer number [0..3]
		 *    -  \param  (DATA8)   NO           - Port number
		 *    -  \param  (DATA8)   MODE         - Mode\n
		 *    -  \param  (DATA8)   LENGTH       - Maximal length of string returned (-1 = no check)\n
		 *    -  \return (DATA8)   DESTINATION  - String variable or handle to string\n
		 *
		 *\n
		 *  - CMD = GET_FIGURES
		 *    -  \param  (DATA8)   LAYER        - Chain layer number [0..3]
		 *    -  \param  (DATA8)   NO           - Port number
		 *    -  \return (DATA8)   FIGURES      - Total number of figures (inclusive decimal point and decimals\n
		 *    -  \return (DATA8)   DECIMALS     - Number of decimals\n
		 *
		 *\n
		 *  - CMD = GET_MINMAX
		 *    -  \param  (DATA8)   LAYER        - Chain layer number [0..3]
		 *    -  \param  (DATA8)   NO           - Port number
		 *    -  \return (DATAF)   MIN          - Min SI value\n
		 *    -  \return (DATAF)   MAX          - Max SI value\n
		 *
		 *\n
		 *  - CMD = READY_PCT
		 *    -  \param  (DATA8)   LAYER        - Chain layer number [0..3]
		 *    -  \param  (DATA8)   NO           - Port number
		 *    -  \param  (DATA8) \ref types "TYPE" - Device type (0 = don't change type)
		 *    -  \param  (DATA8)   MODE         - Device mode [0..7] (-1 = don't change mode)
		 *    -  \param  (DATA8)   VALUES       - Number of return values
		 *
		 *       if (VALUES == 1)
		 *       \return (DATA8)   VALUE1       - First value from input
		 *
		 *\n
		 *  - CMD = READY_RAW
		 *    -  \param  (DATA8)   LAYER        - Chain layer number [0..3]
		 *    -  \param  (DATA8)   NO           - Port number
		 *    -  \param  (DATA8) \ref types "TYPE" - Device type (0 = don't change type)
		 *    -  \param  (DATA8)   MODE         - Device mode [0..7] (-1 = don't change mode)
		 *    -  \param  (DATA8)   VALUES       - Number of return values
		 *
		 *       if (VALUES == 1)
		 *       \return (DATA32)  VALUE1       - First value from input
		 *
		 *\n
		 *  - CMD = READY_SI
		 *    -  \param  (DATA8)   LAYER        - Chain layer number [0..3]
		 *    -  \param  (DATA8)   NO           - Port number
		 *    -  \param  (DATA8) \ref types "TYPE" - Device type (0 = don't change type)
		 *    -  \param  (DATA8)   MODE         - Device mode [0..7] (-1 = don't change mode)
		 *    -  \param  (DATA8)   VALUES       - Number of return values
		 *
		 *       if (VALUES == 1)
		 *       \return (DATAF)   VALUE1       - First value from input
		 *
		 *\n
		 *  - CMD = GET_CHANGES
		 *\n  Get positive changes since last clear\n
		 *    -  \param  (DATA8)   LAYER        - Chain layer number [0..3]
		 *    -  \param  (DATA8)   NO           - Port number
		 *    -  \return (DATAF)   VALUE        - Positive changes since last clear\n
		 *
		 *\n
		 *  - CMD = GET_BUMPS
		 *\n  Get negative changes since last clear\n
		 *    -  \param  (DATA8)   LAYER        - Chain layer number [0..3]
		 *    -  \param  (DATA8)   NO           - Port number
		 *    -  \return (DATAF)   VALUE        - Negative changes since last clear\n
		 *
		 *\n
		 *  - CMD = CLR_CHANGES
		 *\n  Clear changes and bumps\n
		 *    -  \param  (DATA8)   LAYER        - Chain layer number [0..3]
		 *    -  \param  (DATA8)   NO           - Port number
		 *
		 *\n
		 *  - CMD = CAL_MINMAX
		 *\n  Apply new minimum and maximum raw value for device type to be used in scaling PCT and SI\n
		 *    -  \param  (DATA8) \ref types "TYPE" - Device type [1..101]
		 *    -  \param  (DATA8)   MODE         - Device mode [0..7]
		 *    -  \param  (DATA32)  CAL_MIN      - 32 bit raw minimum value (Zero)\n
		 *    -  \param  (DATA32)  CAL_MAX      - 32 bit raw maximum value (Full scale)\n
		 *
		 *\n
		 *  - CMD = CAL_MIN
		 *\n  Apply new minimum raw value for device type to be used in scaling PCT and SI\n
		 *    -  \param  (DATA8) \ref types "TYPE" - Device type [1..101]
		 *    -  \param  (DATA8)   MODE         - Device mode [0..7]
		 *    -  \param  (DATA32)  CAL_MIN      - 32 bit SI minimum value (Zero)\n
		 *
		 *\n
		 *  - CMD = CAL_MAX
		 *\n  Apply new maximum raw value for device type to be used in scaling PCT and SI\n
		 *    -  \param  (DATA8) \ref types "TYPE" - Device type [1..101]
		 *    -  \param  (DATA8)   MODE         - Device mode [0..7]
		 *    -  \param  (DATA32)  CAL_MAX      - 32 bit SI maximum value (Full scale)\n
		 *
		 *\n
		 *  - CMD = CAL_DEFAULT
		 *\n  Apply the default minimum and maximum raw value for device type to be used in scaling PCT and SI\n
		 *    -  \param  (DATA8) \ref types "TYPE" - Device type [1..101]
		 *    -  \param  (DATA8)   MODE         - Device mode [0..7]
		 *
		 *\n
		 *  - CMD = SETUP
		 *\n  Generic setup/read IIC sensors \ref cinputdevicesetup "Example"\n
		 *    -  \param  (DATA8)   LAYER        - Chain layer number [0..3]
		 *    -  \param  (DATA8)   NO           - Port number
		 *    -  \param  (DATA8)   REPEAT       - Repeat setup/read "REPEAT" times  (0 = infinite)
		 *    -  \param  (DATA16)  TIME         - Time between repeats [10..1000mS] (0 = 10)
		 *    -  \param  (DATA8)   WRLNG        - No of bytes to write
		 *    -  \param  (DATA8)   WRDATA       - DATA8 array  (handle) of data to write\n
		 *    -  \param  (DATA8)   RDLNG        - No of bytes to read
		 *    -  \return (DATA8)   RDDATA       - DATA8 array  (handle) to read into\n
		 *
		 *\n
		 *  - CMD = CLR_ALL
		 *\n  Clear all devices (e.c. counters, angle, ...)\n
		 *    -  \param  (DATA8)   LAYER        - Chain layer number [0..3] (-1 = all)
		 *
		 *\n
		 *  - CMD = STOP_ALL
		 *\n  Stop all devices (e.c. motors, ...)\n
		 *    -  \param  (DATA8)   LAYER        - Chain layer number [0..3] (-1 = all)
		 *
		 *\n
		 *
		 */
		/*! \brief  opINPUT_DEVICE byte code
		 *
		 */
		public void cInputDevice()
		{
			IP TmpIp;
			DATA8 Cmd;
			DATA8 Device = 0;
			DATA8 Layer;
			DATA8 Length;
			DATA8* pDestination;
			DATA8 Count = 0;
			DATA8 Modes = 0;
			DATA8* TmpName;
			DATA8 Data8 = 0;
			DATA32 Data32 = 0;
			DATA8 Type;
			DATA8 Mode;
			DATA8 Connection;
			DATA8 Values;
			DATA8 Value;
			DATA8 Views = 0;
			UWORD Index = 0;
			OBJID Owner;
			DATA8 Busy;
			DATAF DataF = (DATAF)0;
			DATAF Min = (DATAF)0;
			DATAF Max = (DATAF)0;
			DATA8 Tmp;
			DATA8 Repeat;
			DATA16 Time;
			DATA8 WrLng;
			DATA8 RdLng;
			DATA8* pWrData;
			DATA8* pRdData;
			UWORD TypeIndex;
			RESULT Result;


			TmpIp = GH.Lms.GetObjectIp();
			Cmd = *(DATA8*)GH.Lms.PrimParPointer();
			if ((Cmd != CAL_MINMAX) && (Cmd != CAL_MIN) && (Cmd != CAL_MAX) && (Cmd != CAL_DEFAULT) && (Cmd != CLR_ALL) && (Cmd != STOP_ALL))
			{
				Device = cInputGetDevice();
			}

			switch (Cmd)
			{ // Function

				case CAL_MINMAX:
					{
						Type = *(DATA8*)GH.Lms.PrimParPointer();
						Mode = *(DATA8*)GH.Lms.PrimParPointer();
						Min = (DATAF) (* (DATA32*)GH.Lms.PrimParPointer());
						Max = (DATAF)( * (DATA32*)GH.Lms.PrimParPointer());

						if ((Type > 0) && (Type < (MAX_DEVICE_TYPE + 1)) && (Mode >= 0) && (Mode < MAX_DEVICE_MODES))
						{
							GH.InputInstance.Calib[Type][Mode].Min = Min;
							GH.InputInstance.Calib[Type][Mode].Max = Max;
						}
					}
					break;

				case CAL_MIN:
					{
						Type = *(DATA8*)GH.Lms.PrimParPointer();
						Mode = *(DATA8*)GH.Lms.PrimParPointer();
						Min = (DATAF) (* (DATA32*)GH.Lms.PrimParPointer());

						if ((Type > 0) && (Type < (MAX_DEVICE_TYPE + 1)) && (Mode >= 0) && (Mode < MAX_DEVICE_MODES))
						{
							if (cInputFindDevice(Type, Mode, &TypeIndex) == OK)
							{
								if (GH.InputInstance.Calib[Type][Mode].InUse == 0)
								{
									GH.InputInstance.Calib[Type][Mode].InUse = 1;
									GH.InputInstance.Calib[Type][Mode].Max = GH.InputInstance.TypeData[TypeIndex].RawMax;
								}
								else
								{
									Min = GH.InputInstance.Calib[Type][Mode].Min + (((Min - GH.InputInstance.TypeData[TypeIndex].SiMin) * (GH.InputInstance.Calib[Type][Mode].Max - GH.InputInstance.Calib[Type][Mode].Min)) / (GH.InputInstance.TypeData[TypeIndex].SiMax - GH.InputInstance.TypeData[TypeIndex].SiMin));
								}
								GH.InputInstance.Calib[Type][Mode].Min = Min;
							}
						}
					}
					break;

				case CAL_MAX:
					{
						Type = *(DATA8*)GH.Lms.PrimParPointer();
						Mode = *(DATA8*)GH.Lms.PrimParPointer();
						Max = (DATAF)(*(DATA32*)GH.Lms.PrimParPointer());

						if ((Type > 0) && (Type < (MAX_DEVICE_TYPE + 1)) && (Mode >= 0) && (Mode < MAX_DEVICE_MODES))
						{
							if (cInputFindDevice(Type, Mode, &TypeIndex) == OK)
							{
								if (GH.InputInstance.Calib[Type][Mode].InUse == 0)
								{
									GH.InputInstance.Calib[Type][Mode].InUse = 1;
									GH.InputInstance.Calib[Type][Mode].Min = GH.InputInstance.TypeData[TypeIndex].RawMin;
								}
								else
								{
									Max = GH.InputInstance.Calib[Type][Mode].Min + (((Max - GH.InputInstance.TypeData[TypeIndex].SiMin) * (GH.InputInstance.Calib[Type][Mode].Max - GH.InputInstance.Calib[Type][Mode].Min)) / (GH.InputInstance.TypeData[TypeIndex].SiMax - GH.InputInstance.TypeData[TypeIndex].SiMin));
								}
								GH.InputInstance.Calib[Type][Mode].Max = Max;
							}
						}
					}
					break;

				case CAL_DEFAULT:
					{
						Type = *(DATA8*)GH.Lms.PrimParPointer();
						Mode = *(DATA8*)GH.Lms.PrimParPointer();

						if ((Type > 0) && (Type < (MAX_DEVICE_TYPE + 1)) && (Mode >= 0) && (Mode < MAX_DEVICE_MODES))
						{
							GH.InputInstance.Calib[Type][Mode].InUse = 0;
						}
					}
					break;

				case GET_TYPEMODE:
					{
						Type = TYPE_NONE;
						Mode = 0;

						if (Device < DEVICES)
						{
							Type = GH.InputInstance.DeviceType[Device];
							Mode = GH.InputInstance.DeviceMode[Device];
						}
						*(DATA8*)GH.Lms.PrimParPointer() = Type;
						*(DATA8*)GH.Lms.PrimParPointer() = Mode;
					}
					break;

				case GET_CONNECTION:
					{
						Connection = TYPE_NONE;

						if (Device < DEVICES)
						{
							Connection = GH.InputInstance.DeviceData[Device].Connection;
						}
						*(DATA8*)GH.Lms.PrimParPointer() = Connection;
					}
					break;

				case GET_NAME:
					{
						Length = *(DATA8*)GH.Lms.PrimParPointer();
						pDestination = (DATA8*)GH.Lms.PrimParPointer();
						Count = 0;

						if (Device < DEVICES)
						{

							TmpName = (sbyte*)GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Device].TypeIndex].Name;

							if (GH.VMInstance.Handle >= 0)
							{
								Tmp = (sbyte)((DATA8)CommonHelper.strlen((DATA8*)TmpName) + 1);

								if (Length == -1)
								{
									Length = Tmp;
								}
								pDestination = (DATA8*)GH.Lms.VmMemoryResize(GH.VMInstance.Handle, (DATA32)Length);
							}
							if (pDestination != null)
							{
								while ((Count < (Length - 1)) && (TmpName[Count] != 0))
								{
									pDestination[Count] = TmpName[Count];

									Count++;
								}
								while (Count < (Length - 1))
								{
									pDestination[Count] = (sbyte)' ';

									Count++;
								}
							}
						}
						if (pDestination != null)
						{
							pDestination[Count] = 0;
						}
					}
					break;

				case GET_SYMBOL:
					{
						Length = *(DATA8*)GH.Lms.PrimParPointer();
						pDestination = (DATA8*)GH.Lms.PrimParPointer();
						Count = 0;

						if (Device < DEVICES)
						{

							TmpName = GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Device].TypeIndex].Symbol;

							if (GH.VMInstance.Handle >= 0)
							{
								Tmp = (sbyte)((DATA8)CommonHelper.strlen((DATA8*)TmpName) + 1);

								if (Length == -1)
								{
									Length = Tmp;
								}
								pDestination = (DATA8*)GH.Lms.VmMemoryResize(GH.VMInstance.Handle, (DATA32)Length);
							}
							if (pDestination != null)
							{
								while ((Count < (Length - 1)) && (TmpName[Count] != 0))
								{
									pDestination[Count] = TmpName[Count];

									Count++;
								}
								while (Count < (Length - 1))
								{
									pDestination[Count] = (sbyte)' ';

									Count++;
								}
							}
						}
						if (pDestination != null)
						{
							pDestination[Count] = 0;
						}
					}
					break;

				case GET_FORMAT:
					{
						if (Device < DEVICES)
						{
							Count = (DATA8)GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Device].TypeIndex].DataSets;
							Data8 = (DATA8)GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Device].TypeIndex].Format;
							Modes = (DATA8)GH.InputInstance.TypeModes[GH.InputInstance.DeviceType[Device]];
							Views = (DATA8)GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Device].TypeIndex].Views;
						}
						*(DATA8*)GH.Lms.PrimParPointer() = Count;
						*(DATA8*)GH.Lms.PrimParPointer() = Data8;
						*(DATA8*)GH.Lms.PrimParPointer() = Modes;
						*(DATA8*)GH.Lms.PrimParPointer() = Views;
					}
					break;

				case GET_RAW:
					{
						Data32 = DATA32_NAN;
						if (Device < DEVICES)
						{
							DataF = cInputReadDeviceRaw(Device, 0, 0, null);
							if (!float.IsNaN(DataF))
							{
								Data32 = (DATA32)DataF;
							}
						}
						*(DATA32*)GH.Lms.PrimParPointer() = Data32;
					}
					break;

				case GET_FIGURES:
					{
						if (Device < DEVICES)
						{
							Count = (DATA8)GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Device].TypeIndex].Figures;
							Data8 = (DATA8)GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Device].TypeIndex].Decimals;
						}
						*(DATA8*)GH.Lms.PrimParPointer() = Count;
						*(DATA8*)GH.Lms.PrimParPointer() = Data8;
					}
					break;

				case GET_MINMAX:
					{
						if (Device < DEVICES)
						{
							Min = (DATAF)GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Device].TypeIndex].SiMin;
							Max = (DATAF)GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Device].TypeIndex].SiMax;
						}
						*(DATAF*)GH.Lms.PrimParPointer() = Min;
						*(DATAF*)GH.Lms.PrimParPointer() = Max;
					}
					break;

				case GET_MODENAME:
					{
						Mode = *(DATA8*)GH.Lms.PrimParPointer();
						Length = *(DATA8*)GH.Lms.PrimParPointer();
						pDestination = (DATA8*)GH.Lms.PrimParPointer();
						Count = 0;

						if (Device < DEVICES)
						{
							Type = GH.InputInstance.DeviceType[Device];
							if ((Type >= 0) && (Type < (MAX_DEVICE_TYPE + 1)))
							{
								// try to find device type
								Index = 0;
								while (Index < GH.InputInstance.MaxDeviceTypes)
								{
									if (GH.InputInstance.TypeData[Index].Type == Type)
									{ // match on type

										if (GH.InputInstance.TypeData[Index].Mode == Mode)
										{ // match on mode

											TmpName = (sbyte*)GH.InputInstance.TypeData[Index].Name;

											if (GH.VMInstance.Handle >= 0)
											{
												Tmp = (sbyte)((DATA8)CommonHelper.strlen((DATA8*)TmpName) + 1);

												if (Length == -1)
												{
													Length = Tmp;
												}
												pDestination = (DATA8*)GH.Lms.VmMemoryResize(GH.VMInstance.Handle, (DATA32)Length);
											}
											if (pDestination != null)
											{
												while ((Count < (Length - 1)) && (TmpName[Count] != 0))
												{
													pDestination[Count] = TmpName[Count];

													Count++;
												}
												while (Count < (Length - 1))
												{
													pDestination[Count] = (sbyte)' ';

													Count++;
												}
											}
											Index = GH.InputInstance.MaxDeviceTypes;
										}
									}
									Index++;
								}
							}
						}
						if (pDestination != null)
						{
							pDestination[Count] = 0;
						}
					}
					break;

				case SET_RAW:
					{
						Type = *(DATA8*)GH.Lms.PrimParPointer();
						Data32 = *(DATA32*)GH.Lms.PrimParPointer();

						cInputWriteDeviceRaw(Device, GH.InputInstance.DeviceData[Device].Connection, Type, Data32);
					}
					break;

				case GET_CHANGES:
					{
						DataF = DATAF_NAN;
						DataF = (DATAF)0;
						if (Device < DEVICES)
						{
							if ((GH.InputInstance.DeviceData[Device].Connection != CONN_NONE) && (GH.InputInstance.DeviceData[Device].Connection != CONN_ERROR))
							{
								DataF = (DATAF)GH.InputInstance.DeviceData[Device].Changes;
							}
						}
						*(DATAF*)GH.Lms.PrimParPointer() = DataF;
					}
					break;

				case GET_BUMPS:
					{
						DataF = DATAF_NAN;
						DataF = (DATAF)0;
						if (Device < DEVICES)
						{
							if ((GH.InputInstance.DeviceData[Device].Connection != CONN_NONE) && (GH.InputInstance.DeviceData[Device].Connection != CONN_ERROR))
							{
								DataF = (DATAF)GH.InputInstance.DeviceData[Device].Bumps;
							}
						}
						*(DATAF*)GH.Lms.PrimParPointer() = DataF;
					}
					break;

				case CLR_CHANGES:
					{
						if (Device < DEVICES)
						{
							GH.InputInstance.DeviceData[Device].Changes = (DATA32)0;
							GH.InputInstance.DeviceData[Device].Bumps = (DATA32)0;
						}
					}
					break;

				case READY_PCT: // Fall through
				case READY_RAW: // Fall through
				case READY_SI:
					{
						Type = *(DATA8*)GH.Lms.PrimParPointer();
						Mode = *(DATA8*)GH.Lms.PrimParPointer();
						Values = *(DATA8*)GH.Lms.PrimParPointer();

						Value = 0;
						Busy = 0;
						Owner = GH.Lms.CallingObjectId();

						if (Device == TESTDEVICE)
						{
							GH.printf($"c_input   opINPUT_DEVICE READY_XX: D={Device} T={Type} M={Mode} B={GH.InputInstance.DeviceData[Device].DevStatus} C={GH.InputInstance.DeviceData[Device].Connection}\r\n");
						}
						if (Device < DEVICES)
						{ // Device valid

							if ((GH.InputInstance.DeviceData[Device].Connection != CONN_NONE) && (GH.InputInstance.DeviceData[Device].Connection != CONN_ERROR))
							{ // Device present

								if (Type == TYPE_KEEP)
								{ // Get actual type

									Type = GH.InputInstance.DeviceType[Device];
								}
								if (Mode == MODE_KEEP)
								{ // Get actual mode

									Mode = GH.InputInstance.DeviceMode[Device];
								}
								if (GH.InputInstance.DeviceData[Device].Busy == 0)
								{
									if ((GH.InputInstance.DeviceType[Device] != Type) || (GH.InputInstance.DeviceMode[Device] != Mode))
									{ // Must change type or mode so check if owner is OK

										if ((GH.InputInstance.DeviceData[Device].Owner == 0) || (GH.InputInstance.DeviceData[Device].Owner == Owner))
										{ // Owner is OK

											GH.printf($"c_input   opINPUT_DEVICE READY_XX: D={Device} Change to type {Type} mode {Mode}\r\n");
											GH.InputInstance.DeviceData[Device].Owner = Owner;
											cInputSetDeviceType(Device, Type, Mode, 3307); // 3307 was a current line
											GH.InputInstance.DeviceData[Device].TimeoutTimer = MAX_DEVICE_BUSY_TIME;
											GH.InputInstance.DeviceData[Device].Busy = 0;
											if (Device == TESTDEVICE)
											{
												GH.VMInstance.Status &= ~0x40;
											}
										}
										else
										{ // Another owner

											GH.printf($"c_input   opINPUT_DEVICE READY_XX: D={Device} Trying to change to type {Type} mode {Mode}\r\n");
											if (Device == TESTDEVICE)
											{
												GH.VMInstance.Status |= 0x40;
											}
											Busy = 1;
										}
									}
								}
								if (Busy == 0)
								{
									if (GH.InputInstance.DeviceData[Device].DevStatus == RESULT.BUSY)
									{
										Busy = 1;

										if (GH.InputInstance.DeviceData[Device].Busy == 0)
										{
											GH.InputInstance.DeviceData[Device].TimeoutTimer = MAX_DEVICE_BUSY_TIME;
											GH.InputInstance.DeviceData[Device].Busy = 1;
										}
										else
										{
											if (GH.InputInstance.DeviceData[Device].TimeoutTimer == 0)
											{
												GH.printf($"c_input   opINPUT_DEVICE READY_XX: D={Device} Timeout when trying to change to type {Type} mode {Mode}\r\n");
												GH.InputInstance.DeviceData[Device].Owner = 0;
												GH.InputInstance.DeviceData[Device].Busy = 0;
												Busy = 0;
											}
										}
									}
									else
									{
										while ((Value < Values) && (Value < MAX_DEVICE_DATASETS))
										{
											switch (Cmd)
											{
												case READY_PCT:
													{
														*(DATA8*)GH.Lms.PrimParPointer() = (DATA8)cInputReadDevicePct(Device, Value, 0, null);
													}
													break;

												case READY_RAW:
													{
														DataF = cInputReadDeviceRaw(Device, Value, 0, null);
														if (float.IsNaN(DataF))
														{
															*(DATA32*)GH.Lms.PrimParPointer() = DATA32_NAN;
														}
														else
														{
															*(DATA32*)GH.Lms.PrimParPointer() = (DATA32)DataF;
														}
													}
													break;

												case READY_SI:
													{
														DataF = (DATAF)cInputReadDeviceSi(Device, Value, 0, null);
														*(DATAF*)GH.Lms.PrimParPointer() = DataF;

													}
													break;

											}
											Value++;
										}
									}

								}
							}
							else
							{
								GH.Lms.SetInstructions(0);
							}
							if (GH.InputInstance.DeviceData[Device].DevStatus != RESULT.BUSY)
							{
								GH.InputInstance.DeviceData[Device].Owner = 0;
								GH.InputInstance.DeviceData[Device].Busy = 0;
								if (Device == TESTDEVICE)
								{
									GH.VMInstance.Status &= ~0x20;
								}
							}
							else
							{
								if (Device == TESTDEVICE)
								{
									GH.VMInstance.Status |= 0x20;
								}
							}
						}

						if (Busy != 0)
						{ // Busy -> block VMThread

							GH.Lms.SetObjectIp(TmpIp - 1);
							GH.Lms.SetDispatchStatus(DSPSTAT.BUSYBREAK);
						}
						else
						{ // Not busy -> be sure to pop all parameters

							while (Value < Values)
							{
								switch (Cmd)
								{
									case READY_PCT:
										{
											*(DATA8*)GH.Lms.PrimParPointer() = DATA8_NAN;
										}
										break;

									case READY_RAW:
										{
											*(DATA32*)GH.Lms.PrimParPointer() = DATA32_NAN;
										}
										break;

									case READY_SI:
										{
											*(DATAF*)GH.Lms.PrimParPointer() = DATAF_NAN;
										}
										break;

								}
								Value++;
							}
						}
					}
					break;

				case SETUP:
					{ // INPUT_DEVICE(SETUP,LAYER,NO,REPEAT,TIME,WRLNG,WRDATA,RDLNG,RDDATA)

						Repeat = *(DATA8*)GH.Lms.PrimParPointer();
						Time = *(DATA16*)GH.Lms.PrimParPointer();
						WrLng = *(DATA8*)GH.Lms.PrimParPointer();
						pWrData = (DATA8*)GH.Lms.PrimParPointer();
						RdLng = *(DATA8*)GH.Lms.PrimParPointer();
						pRdData = (DATA8*)GH.Lms.PrimParPointer();

						if (GH.VMInstance.Handle >= 0)
						{
							pRdData = (DATA8*)GH.Lms.VmMemoryResize(GH.VMInstance.Handle, (DATA32)RdLng);
						}
						if (pRdData != null)
						{

							if (cInputSetupDevice(Device, Repeat, Time, WrLng, pWrData, RdLng, pRdData) == RESULT.BUSY)
							{ // Busy -> block VMThread

								GH.Lms.SetObjectIp(TmpIp - 1);
								GH.Lms.SetDispatchStatus(DSPSTAT.BUSYBREAK);
							}
						}
					}
					break;

				case CLR_ALL:
					{
						Layer = *(DATA8*)GH.Lms.PrimParPointer();

						if (Layer == 0)
						{
							ClrLayer();
						}
						else
						{
							Result = OK;

							if (Layer == -1)
							{
								MoreLayers = 1;
								Layer = ActLayer;
							}

							if (Layer == 0)
							{
								ClrLayer();
							}
							else
							{
								Result = GH.Daisy.cDaisyReady();

								if (Result == OK)
								{ // Ready for command

									DaisyBuf[0] = 0;
									DaisyBuf[1] = 0;
									unchecked { DaisyBuf[2] = (sbyte)opINPUT_DEVICE; }
									DaisyBuf[3] = CLR_ALL;
									DaisyBuf[4] = 0;

									Result = GH.Daisy.cDaisyDownStreamCmd(DaisyBuf, 5, Layer);
								}
							}

							if (Result != RESULT.BUSY)
							{ // If job done

								if (MoreLayers != 0)
								{ // More layers ?

									ActLayer++;
									if (ActLayer >= CHAIN_DEPT)
									{ // No more layers

										ActLayer = 0;
										MoreLayers = 0;
									}
									else
									{ // Next layer

										Result = RESULT.BUSY;
									}
								}
							}

							if (Result == RESULT.BUSY)
							{ // Job not done - hold execution

								GH.Lms.SetObjectIp(TmpIp - 1);
								GH.Lms.SetDispatchStatus(DSPSTAT.BUSYBREAK);
							}
						}
					}
					break;

				case STOP_ALL:
					{
						Layer = *(DATA8*)GH.Lms.PrimParPointer();

						if (Layer == 0)
						{
							StopLayer();
						}
						else
						{
							Result = OK;

							if (Layer == -1)
							{
								MoreLayers = 1;
								Layer = ActLayer;
							}

							if (Layer == 0)
							{
								StopLayer();
							}
							else
							{
								Result = GH.Daisy.cDaisyReady();

								if (Result == OK)
								{ // Ready for command

									DaisyBuf[0] = 0;
									DaisyBuf[1] = 0;
									unchecked { DaisyBuf[2] = (sbyte)opINPUT_DEVICE; }
									DaisyBuf[3] = STOP_ALL;
									DaisyBuf[4] = 0;

									Result = GH.Daisy.cDaisyDownStreamCmd(DaisyBuf, 5, Layer);
								}
							}

							if (Result != RESULT.BUSY)
							{ // If job done

								if (MoreLayers != 0)
								{ // More layers ?

									ActLayer++;
									if (ActLayer >= CHAIN_DEPT)
									{ // No more layers

										ActLayer = 0;
										MoreLayers = 0;
									}
									else
									{ // Next layer

										Result = RESULT.BUSY;
									}
								}
							}

							if (Result == RESULT.BUSY)
							{ // Job not done - hold execution

								GH.Lms.SetObjectIp(TmpIp - 1);
								GH.Lms.SetDispatchStatus(DSPSTAT.BUSYBREAK);
							}
						}
					}
					break;

			}
		}


		/*! \page cInput
		 *  <hr size="1"/>
		 *  <b>     opINPUT_READ (LAYER, NO, TYPE, MODE, PCT)  </b>
		 *
		 *- Read device value in Percent\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   LAYER   - Chain layer number [0..3]
		 *  \param  (DATA8)   NO      - Port number
		 *  \param  (DATA8) \ref types "TYPE" - Device type (0 = don't change type)
		 *  \param  (DATA8)   MODE    - Device mode [0..7] (-1 = don't change mode)
		 *  \return (DATA8)   PCT     - Percent value from device
		 */
		/*! \brief  opINPUT_READ byte code
		 *
		 */
		public void cInputRead()
		{
			DATA8 Type;
			DATA8 Mode;
			DATA8 Device;

			Device = cInputGetDevice();
			Type = *(DATA8*)GH.Lms.PrimParPointer();
			Mode = *(DATA8*)GH.Lms.PrimParPointer();

			if (Device < DEVICES)
			{
				cInputSetType(Device, Type, Mode, 3653); // 3653 was a current line
			}
			*(DATA8*)GH.Lms.PrimParPointer() = cInputReadDevicePct(Device, 0, 0, null);
		}


		/*! \page cInput
		 *  <hr size="1"/>
		 *  <b>     opINPUT_READSI (LAYER, NO, TYPE, MODE, SI)  </b>
		 *
		 *- Read device value in SI units\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   LAYER   - Chain layer number [0..3]
		 *  \param  (DATA8)   NO      - Port number
		 *  \param  (DATA8) \ref types "TYPE" - Device type (0 = don't change type)
		 *  \param  (DATA8)   MODE    - Device mode [0..7] (-1 = don't change mode)
		 *  \return (DATAF)   SI      - SI unit value from device
		 */
		/*! \brief  opINPUT_READSI byte code
		 *
		 */
		public void cInputReadSi()
		{
			DATA8 Type;
			DATA8 Mode;
			DATA8 Device;

			Device = cInputGetDevice();
			Type = *(DATA8*)GH.Lms.PrimParPointer();
			Mode = *(DATA8*)GH.Lms.PrimParPointer();

			GH.printf($"c_input   opINPUT_READSI:          D={Device} B={GH.InputInstance.DeviceData[Device].DevStatus}\r\n");
			if (Device < DEVICES)
			{
				cInputSetType(Device, Type, Mode, 3688); // 3688 was a current line
			}
			*(DATAF*)GH.Lms.PrimParPointer() = cInputReadDeviceSi(Device, 0, 0, null);
		}


		/*! \page   cInput
		 *  <hr size="1"/>
		 *  <b>     opINPUT_TEST (LAYER, NO, RESULT.BUSY) </b>
		 *
		 *- Test if device busy (changing type or mode)\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   LAYER    - Chain layer number [0..3]
		 *  \param  (DATA8)   NO       - Port number
		 *  \return (DATA8)   RESULT.BUSY     - Device busy flag (0 = ready, 1 = busy)
		 *
		 */
		/*! \brief  opINPUT_TEST byte code
		 *
		 */
		public void cInputTest()
		{
			DATA8 Busy = 1;
			DATA8 Device;

			Device = cInputGetDevice();

			if (Device < DEVICES)
			{
				GH.printf($"c_input   opINPUT_TEST:            D={Device} B={GH.InputInstance.DeviceData[Device].DevStatus}\r\n");
				if (GH.InputInstance.DeviceData[Device].DevStatus != RESULT.BUSY)
				{
					Busy = 0;
				}
			}
			*(DATA8*)GH.Lms.PrimParPointer() = Busy;
		}



		/*! \page   cInput
		 *  <hr size="1"/>
		 *  <b>     opINPUT_READY (LAYER, NO) </b>
		 *
		 *- Wait for device ready (wait for valid data)\n
		 *- Dispatch status can change to DSPSTAT.BUSYBREAK
		 *
		 *  \param  (DATA8)   LAYER   - Chain layer number [0..3]
		 *  \param  (DATA8)   NO      - Port number
		 */
		/*! \brief  opINPUT_READY byte code
		 *
		 */
		public void cInputReady()
		{
			IP TmpIp;
			DATA8 Device;

			TmpIp = GH.Lms.GetObjectIp();
			Device = cInputGetDevice();

			if (Device < DEVICES)
			{
				if (GH.InputInstance.DeviceData[Device].DevStatus == RESULT.BUSY)
				{
					GH.Lms.SetObjectIp(TmpIp - 1);
					GH.Lms.SetDispatchStatus(DSPSTAT.BUSYBREAK);
				}
			}
		}


		/*! \page cInput
		 *  <hr size="1"/>
		 *  <b>     opINPUT_WRITE (LAYER, NO, BYTES, DATA)  </b>
		 *
		 *- Write data to device (only UART devices)\n
		 *- Dispatch status can change to DSPSTAT.BUSYBREAK
		 *
		 *  \param  (DATA8)   LAYER   - Chain layer number [0..3]
		 *  \param  (DATA8)   NO      - Port number
		 *  \param  (DATA8)   BYTES   - No of bytes to write [1..32]
		 *  \param  (DATA8)   DATA    - First byte in DATA8 array to write
		 */
		/*! \brief  opINPUT_WRITE byte code
		 *
		 */
		public void cInputWrite()
		{
			DATA8 Bytes;
			DATA8* Data;
			DATA8 Device;
			DATA8 Tmp;
			IP TmpIp;
			DATA8* Buffer = CommonHelper.Pointer1d<DATA8>(UART_DATA_LENGTH + 1);
			DSPSTAT DspStat = DSPSTAT.FAILBREAK;

			TmpIp = GH.Lms.GetObjectIp();

			Device = cInputGetDevice();
			Bytes = *(DATA8*)GH.Lms.PrimParPointer();
			Data = (DATA8*)GH.Lms.PrimParPointer();

			if (Device < INPUT_DEVICES)
			{
				if (GH.InputInstance.DeviceType[Device] != TYPE_TERMINAL)
				{
					if (GH.InputInstance.DeviceData[Device].Connection == CONN_INPUT_UART)
					{
						if ((Bytes > 0) && (Bytes <= UART_DATA_LENGTH))
						{
							if (((*GH.InputInstance.pUart).Status[Device] & UART_WRITE_REQUEST) != 0)
							{
								DspStat = DSPSTAT.BUSYBREAK;
							}
							else
							{
								GH.InputInstance.DeviceData[Device].DevStatus = RESULT.BUSY;

								(*GH.InputInstance.pUart).Status[Device] &= ~UART_DATA_READY;

								Buffer[0] = Device;
								for (Tmp = 0; Tmp < Bytes; Tmp++)
								{
									Buffer[Tmp + 1] = Data[Tmp];
								}

								// write setup string to "UART Device Controller" driver
								if (GH.InputInstance.UartFile >= MIN_HANDLE)
								{
									GH.Ev3System.InputHandler.WriteUartData(CommonHelper.GetArray((byte*)Buffer, Bytes + 1));
								}
								DspStat = DSPSTAT.NOBREAK;
							}
						}
					}
					else
					{

						if (GH.InputInstance.DeviceData[Device].Connection == CONN_NXT_IIC)
						{
							if ((Bytes > 0) && (Bytes <= IIC_DATA_LENGTH))
							{
								if (((*GH.InputInstance.pIic).Status[Device] & IIC_WRITE_REQUEST) != 0)
								{
									DspStat = DSPSTAT.BUSYBREAK;
								}
								else
								{
									GH.InputInstance.DeviceData[Device].DevStatus = RESULT.BUSY;

									(*GH.InputInstance.pIic).Status[Device] &= ~IIC_DATA_READY;

									Buffer[0] = Device;
									for (Tmp = 0; Tmp < Bytes; Tmp++)
									{
										Buffer[Tmp + 1] = Data[Tmp];
									}

									// write setup string to "IIC Device Controller" driver
									if (GH.InputInstance.IicFile >= MIN_HANDLE)
									{
                                        GH.Ev3System.InputHandler.WriteI2cData(CommonHelper.GetArray((byte*)Buffer, Bytes + 1));
									}
									DspStat = DSPSTAT.NOBREAK;
								}
							}
						}
						else
						{ // don't bother if not UART or IIC device

							DspStat = DSPSTAT.NOBREAK;
						}
					}
				}
				else
				{ // don't bother if TERMINAL

					DspStat = DSPSTAT.NOBREAK;
				}
			}
			if (DspStat == DSPSTAT.BUSYBREAK)
			{ // Rewind IP

				GH.Lms.SetObjectIp(TmpIp - 1);
			}
			GH.Lms.SetDispatchStatus(DspStat);
		}


		/*! \page cInput
		 *  <hr size="1"/>
		 *  <b>     opINPUT_READEXT (LAYER, NO, TYPE, MODE, FORMAT, VALUES, VALUE1)  </b>
		 *
		 *- Read device value\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   LAYER   - Chain layer number [0..3]
		 *  \param  (DATA8)   NO      - Port number
		 *  \param  (DATA8) \ref types "TYPE" - Device type (0 = don't change type)
		 *  \param  (DATA8)   MODE    - Device mode [0..7] (-1 = don't change mode)
		 *  \param  (DATA8) \ref formats "FORMAT"  - Format (PCT, RAW, SI ...)
		 *  \param  (DATA8)   VALUES  - Number of return values
		 *
		 *  if (VALUES == 1)
		 *  \return (FORMAT)  VALUE1  - First value from device
		 */
		/*! \brief  opINPUT_READEXT byte code
		 *
		 */
		public void cInputReadExt()
		{
			DATAF Raw;
			DATA8 Type;
			DATA8 Mode;
			DATA8 Format;
			DATA8 Values;
			DATA8 Device;
			DATA8 Value;

			Device = cInputGetDevice();
			Type = *(DATA8*)GH.Lms.PrimParPointer();
			Mode = *(DATA8*)GH.Lms.PrimParPointer();
			Format = *(DATA8*)GH.Lms.PrimParPointer();
			Values = *(DATA8*)GH.Lms.PrimParPointer();
			Value = 0;

			if (Device < DEVICES)
			{
				cInputSetType(Device, Type, Mode, 3918); // 3918 was a current line

				while ((Value < Values) && (Value < MAX_DEVICE_MODES))
				{
					switch (Format)
					{
						case DATA_PCT:
							{
								*(DATA8*)GH.Lms.PrimParPointer() = (DATA8)cInputReadDevicePct(Device, Value, 0, null);
							}
							break;

						case DATA_RAW:
							{
								Raw = cInputReadDeviceRaw(Device, Value, 0, null);
								if (float.IsNaN(Raw))
								{
									*(DATA32*)GH.Lms.PrimParPointer() = DATA32_NAN;
								}
								else
								{
									*(DATA32*)GH.Lms.PrimParPointer() = (DATA32)Raw;
								}
							}
							break;

						case DATA_SI:
							{
								*(DATAF*)GH.Lms.PrimParPointer() = (DATAF)cInputReadDeviceSi(Device, Value, 0, null);
							}
							break;

						default:
							{
								*(DATA8*)GH.Lms.PrimParPointer() = DATA8_NAN;
							}
							break;

					}
					Value++;
				}
			}
			while (Value < Values)
			{
				switch (Format)
				{
					case DATA_PCT:
						{
							*(DATA8*)GH.Lms.PrimParPointer() = DATA8_NAN;
						}
						break;

					case DATA_RAW:
						{
							*(DATA32*)GH.Lms.PrimParPointer() = DATA32_NAN;
						}
						break;

					case DATA_SI:
						{
							*(DATAF*)GH.Lms.PrimParPointer() = DATAF_NAN;
						}
						break;

					default:
						{
							*(DATA8*)GH.Lms.PrimParPointer() = DATA8_NAN;
						}
						break;

				}
				Value++;
			}
		}


		/*! \page cInput
		 *  <hr size="1"/>
		 *  <b>     opINPUT_SAMPLE (TIME, SAMPLES, INIT, DEVICES, TYPES, MODES, DATASETS, VALUES)  </b>
		 *
		 *- Sample devices (see \ref cinputsample "Example")\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA32)  TIME      - Sample time [mS]
		 *  \param  (DATA16)  SAMPLES   - Number of samples
		 *  \param  (DATA16)  INIT      - DATA16 array (handle) - to start/reset buffer -> fill array with -1 otherwise don't change
		 *  \param  (DATA8)   DEVICES   - DATA8 array  (handle) with devices to sample
		 *  \param  (DATA8)   TYPES     - DATA8 array  (handle) with types
		 *  \param  (DATA8)   MODES     - DATA8 array  (handle) with modes
		 *  \param  (DATA8)   DATASETS  - DATA8 array  (handle) with data sets
		 *  \return (DATAF)   VALUES    - DATAF array  (handle) with values
		 *
		 */
		/*! \brief  opINPUT_SAMPLE byte code
		 *
		 */
		public void cInputSample()
		{
			DATA32 SampleTime;
			DATA32 Data32;
			DATA16 NoOfPorts;
			DATA16* pInits;
			DATA8* pDevices;
			DATA8* pTypes;
			DATA8* pModes;
			DATA8* pDataSets;
			DATAF* pValues;
			DATA16 Index;
			DATA8 Device;
			DATA8 Type;
			DATA8 Mode;


			SampleTime = *(DATA32*)GH.Lms.PrimParPointer();
			NoOfPorts = *(DATA16*)GH.Lms.PrimParPointer();
			pInits = (DATA16*)GH.Lms.PrimParPointer();
			pDevices = (DATA8*)GH.Lms.PrimParPointer();
			pTypes = (DATA8*)GH.Lms.PrimParPointer();
			pModes = (DATA8*)GH.Lms.PrimParPointer();
			pDataSets = (DATA8*)GH.Lms.PrimParPointer();
			pValues = (DATAF*)GH.Lms.PrimParPointer();
			if (GH.VMInstance.Handle >= 0)
			{
				Data32 = (DATA32)NoOfPorts;
				if (Data32 > MIN_ARRAY_ELEMENTS)
				{
					pValues = (DATAF*)GH.Lms.VmMemoryResize(GH.VMInstance.Handle, Data32);
				}
			}
			if (pValues != null)
			{
				for (Index = 0; Index < NoOfPorts; Index++)
				{ // Service all devices

					Device = pDevices[Index];

					if (Device >= INPUTS)
					{
						Device += 12;
					}

					pValues[Index] = DATAF_NAN;

					if ((Device >= 0) && (Device < DEVICES))
					{
						Type = pTypes[Index];
						Mode = pModes[Index];

						cInputSetType(Device, Type, Mode, 4066); // 4066 was a current line

						pValues[Index] = cInputReadDeviceSi(Device, pDataSets[Index], (short)SampleTime, &pInits[Index]);
					}
				}
			}
		}
	}
}
