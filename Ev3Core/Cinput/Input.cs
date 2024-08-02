using Ev3Core.Cinput.Interfaces;
using Ev3Core.Enums;
using Ev3Core.Extensions;
using Ev3Core.Helpers;
using Ev3Core.Lms2012.Interfaces;
using System.Globalization;
using static Ev3Core.Defines;

namespace Ev3Core.Cinput
{
	public class Input : IInput
	{
		TYPES[] TypeDefault =
		{
			//   Name										   Type                   Connection                Mode	 DataSets     Format      Figures       Decimals      Views      RawMin         RawMax            PctMin         PctMax            SiMin        SiMax            Time             IdValue        Pins             Symbol
			new TYPES() { Name = "PORT ERROR".ToSbyteArray(), Type = TYPE_ERROR,   Connection = CONN_ERROR,   Mode = 0, DataSets = 0, Format = 0, Figures = 4, Decimals = 0, Views = 1, RawMin = 0.0f, RawMax = 0.0f,    PctMin = 0.0f, PctMax = 0.0f,   SiMin = 0.0f, SiMax = 0.0f,    InvalidTime = 0, IdValue = 0, Pins = (sbyte)'f', Symbol = new sbyte[0] },
			new TYPES() { Name = "NONE".ToSbyteArray(),       Type = TYPE_NONE,    Connection = CONN_NONE,    Mode = 0, DataSets = 0, Format = 0, Figures = 4, Decimals = 0, Views = 1, RawMin = 0.0f, RawMax = 0.0f,    PctMin = 0.0f, PctMax = 0.0f,   SiMin = 0.0f, SiMax = 0.0f,    InvalidTime = 0, IdValue = 0, Pins = (sbyte)'f', Symbol = new sbyte[0] },
			new TYPES() { Name = "UNKNOWN".ToSbyteArray(),    Type = TYPE_UNKNOWN, Connection = CONN_UNKNOWN, Mode = 0, DataSets = 1, Format = 1, Figures = 4, Decimals = 0, Views = 1, RawMin = 0.0f, RawMax = 1023.0f, PctMin = 0.0f, PctMax = 100.0f, SiMin = 0.0f, SiMax = 1023.0f, InvalidTime = 0, IdValue = 0, Pins = (sbyte)'f', Symbol = new sbyte[0] },
			new TYPES() { Name = "\0".ToSbyteArray() }
		};

		IMGDATA[] CLR_LAYER_CLR_CHANGES = { opINPUT_DEVICE, CLR_CHANGES, 0, 0, opINPUT_DEVICE, CLR_CHANGES, 0, 1, opINPUT_DEVICE, CLR_CHANGES, 0, 2, opINPUT_DEVICE, CLR_CHANGES, 0, 3, opOBJECT_END };
		IMGDATA[] CLR_LAYER_CLR_BUMBED = { opUI_BUTTON, FLUSH, opOBJECT_END };
		IMGDATA[] CLR_LAYER_OUTPUT_RESET = { opOUTPUT_RESET, 0, 15, opOBJECT_END };
		IMGDATA[] CLR_LAYER_OUTPUT_CLR_COUNT = { opOUTPUT_CLR_COUNT, 0, 15, opOBJECT_END };
		IMGDATA[] CLR_LAYER_INPUT_WRITE = { opINPUT_WRITE, 0, 0, 1, DEVCMD_RESET, opINPUT_WRITE, 0, 1, 1, DEVCMD_RESET, opINPUT_WRITE, 0, 2, 1, DEVCMD_RESET, opINPUT_WRITE, 0, 3, 1, DEVCMD_RESET, opOBJECT_END };

		IMGDATA[] STOP_LAYER = { opOUTPUT_PRG_STOP, opOBJECT_END };

		void ClrLayer()
		{
			GH.Lms.ExecuteByteCode(CLR_LAYER_CLR_CHANGES, null, null);
			GH.Lms.ExecuteByteCode(CLR_LAYER_CLR_BUMBED, null, null);
			GH.Lms.ExecuteByteCode(CLR_LAYER_OUTPUT_RESET, null, null);
			GH.Lms.ExecuteByteCode(CLR_LAYER_OUTPUT_CLR_COUNT, null, null);
			GH.Lms.ExecuteByteCode(CLR_LAYER_INPUT_WRITE, null, null);
		}

		void StopLayer()
		{
			GH.Lms.ExecuteByteCode(STOP_LAYER, null, null);
		}

		RESULT cInputExpandDevice(DATA8 Device, ref DATA8 pLayer, ref DATA8 pPort, ref DATA8 pOutput)
		{ // pPort: pOutput=0 -> 0..3 , pOutput=1 -> 0..3

			RESULT Result = RESULT.FAIL;

			if ((Device >= 0) && (Device < DEVICES))
			{
				if (Device >= INPUT_DEVICES)
				{ // OUTPUT

					pOutput = 1;
					Device -= INPUT_DEVICES;
					pPort = ((sbyte)(Device % OUTPUT_PORTS));
					pPort += INPUT_DEVICES;
				}
				else
				{ // INPUT

					pOutput = 0;
					pPort = (sbyte)(Device % INPUT_PORTS);

				}
				pLayer = (sbyte)(Device / CHAIN_DEPT);

				Result = OK;
			}

			return (Result);
		}

		public RESULT cInputCompressDevice(ref sbyte pDevice, byte Layer, byte Port)
		{
			RESULT Result = RESULT.FAIL;

			if (Port >= INPUT_DEVICES)
			{ // OUTPUT

				pDevice = (sbyte)(OUTPUT_PORTS * Layer);
				pDevice += (sbyte)Port;

			}
			else
			{ // INPUT

				pDevice = ((sbyte)(INPUT_PORTS * Layer));
				pDevice += (sbyte)Port;
			}

			if ((pDevice >= 0) && (pDevice < DEVICES))
			{
				Result = OK;
			}

			return (Result);
		}

		RESULT cInputInsertNewIicString(DATA8 Type, DATA8 Mode, DATA8[] pManufacturer, DATA8[] pSensorType, DATA8 SetupLng, ULONG SetupString, DATA8 PollLng, ULONG PollString, DATA8 ReadLng)
		{
			RESULT Result = RESULT.FAIL;  // FAIL=full, OK=new, BUSY=found
			IICSTR pTmp;
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

						// TODO: IIC shite
						//object[] iicBytes;
						//if (GH.Memory.cMemoryRealloc(GH.InputInstance.IicString, out iicBytes, (DATA32)(IICSTR.Sizeof * (GH.InputInstance.IicDeviceTypes + 1))) == OK)
						//{ // Success

						//	pTmp = IICSTR.FromBytes(iicBytes);

						//	GH.InputInstance.IicString = pTmp;

						//	GH.InputInstance.IicString[Index].Type = Type;
						//	GH.InputInstance.IicString[Index].Mode = Mode;
						//	snprintf((char*)InputInstance.IicString[Index].Manufacturer, IIC_NAME_LENGTH + 1, "%s", (char*)pManufacturer);
						//	snprintf((char*)InputInstance.IicString[Index].SensorType, IIC_NAME_LENGTH + 1, "%s", (char*)pSensorType);
						//	GH.InputInstance.IicString[Index].SetupLng = SetupLng;
						//	GH.InputInstance.IicString[Index].SetupString = SetupString;
						//	GH.InputInstance.IicString[Index].PollLng = PollLng;
						//	GH.InputInstance.IicString[Index].PollString = PollString;
						//	GH.InputInstance.IicString[Index].ReadLng = ReadLng;
						//	//          printf("cInputInsertNewIicString  %-3u %01u IIC %u 0x%08X %u 0x%08X %s %s\r\n",Type,Mode,SetupLng,SetupString,PollLng,PollString,pManufacturer,pSensorType);

						//	GH.InputInstance.IicDeviceTypes++;
						//	Result = OK;
						//}

						GH.Ev3System.Logger.LogWarning($"Called commented shite in {System.Environment.StackTrace}");
					}
				}
				if (Result == RESULT.FAIL)
				{ // No room for type/mode

					GH.Ev3System.Logger.LogWarning($"Fail {TYPEDATA_TABEL_FULL} in {System.Environment.StackTrace}");
				}
			}
			else
			{ // Type or mode invalid

				GH.Ev3System.Logger.LogWarning($"Iic  error {Type}: m={Mode} IIC");
			}

			return (Result);
		}

		RESULT cInputGetIicString(DATA8 Type, DATA8 Mode, IICSTR IicStr)
		{
			RESULT Result = RESULT.FAIL;  // FAIL=full, OK=new, BUSY=found
			UWORD Index = 0;

			IicStr.SetupLng = 0;
			IicStr.SetupString = 0;
			IicStr.PollLng = 0;
			IicStr.PollString = 0;
			IicStr.ReadLng = 0;

			if ((Type >= 0) && (Type < (MAX_DEVICE_TYPE + 1)) && (Mode >= 0) && (Mode < MAX_DEVICE_MODES))
			{ // Type and mode valid

				while ((Index < GH.InputInstance.IicDeviceTypes) && (Result != OK))
				{ // trying to find device type

					if ((GH.InputInstance.IicString[Index].Type == Type) && (GH.InputInstance.IicString[Index].Mode == Mode))
					{ // match on type and mode

						IicStr.Type = Type;
						IicStr.Mode = Mode;
						CommonHelper.Snprintf(IicStr.Manufacturer, 0, IIC_NAME_LENGTH + 1, GH.InputInstance.IicString[Index].Manufacturer);
						CommonHelper.Snprintf(IicStr.SensorType, 0, IIC_NAME_LENGTH + 1, GH.InputInstance.IicString[Index].SensorType);
						IicStr.SetupLng = GH.InputInstance.IicString[Index].SetupLng;
						IicStr.SetupString = GH.InputInstance.IicString[Index].SetupString;
						IicStr.PollLng = GH.InputInstance.IicString[Index].PollLng;
						IicStr.PollString = GH.InputInstance.IicString[Index].PollString;
						IicStr.ReadLng = GH.InputInstance.IicString[Index].ReadLng;

						Result = OK;
					}
					Index++;
				}
			}

			return (Result);
		}


		RESULT cInputGetNewTypeDataPointer(SBYTE[] pName, DATA8 Type, DATA8 Mode, DATA8 Connection, out TYPES ppPlace)
		{
			RESULT Result = RESULT.FAIL;  // FAIL=full, OK=new, BUSY=found
			UWORD Index = 0;

			ppPlace = null;

			if ((Type >= 0) && (Type < (MAX_DEVICE_TYPE + 1)) && (Mode >= 0) && (Mode < MAX_DEVICE_MODES))
			{ // Type and mode valid

				while ((Index < GH.InputInstance.MaxDeviceTypes) && (Result != RESULT.BUSY))
				{ // trying to find device type

					if ((GH.InputInstance.TypeData[Index].Type == Type) && (GH.InputInstance.TypeData[Index].Mode == Mode) && (GH.InputInstance.TypeData[Index].Connection == Connection))
					{ // match on type, mode and connection

						ppPlace = GH.InputInstance.TypeData[Index];
						Result = RESULT.BUSY;
					}
					Index++;
				}
				if (Result != RESULT.BUSY)
				{ // device type not found

					if (GH.InputInstance.MaxDeviceTypes < MAX_DEVICE_TYPES)
					{ // Allocate room for a new type/mode

						// TODO: IIC shite
						//if (cMemoryRealloc((void*)InputInstance.TypeData, (void**)ppPlace, (DATA32)(sizeof(TYPES) * (InputInstance.MaxDeviceTypes + 1))) == OK)
						//{ // Success

						//	InputInstance.TypeData = *ppPlace;

						//	*ppPlace = &InputInstance.TypeData[InputInstance.MaxDeviceTypes];
						//	InputInstance.TypeModes[Type]++;
						//	InputInstance.MaxDeviceTypes++;
						//	Result = OK;
						//}
						GH.Ev3System.Logger.LogWarning($"Called commented shite in {System.Environment.StackTrace}");
					}
				}
				if (Result == RESULT.FAIL)
				{ // No room for type/mode

					GH.Ev3System.Logger.LogWarning($"Fail {TYPEDATA_TABEL_FULL} in {System.Environment.StackTrace}");
				}
			}
			else
			{ // Type or mode invalid

				GH.Ev3System.Logger.LogWarning($"Type error {Type}: m={Mode} c={Connection} n={pName}");
			}

			return (Result);
		}

		RESULT cInputInsertTypeData(char[] pFilename)
		{
			RESULT Result = RESULT.FAIL;
			string Buf;
			char[] Name = new char[256];
			char[] Symbol = new char[256];
			char[] Manufacturer = new char[256];
			char[] SensorType = new char[256];
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
			TYPES Tmp = new TYPES();
			TYPES pTypes;
			int Count;

			using FileStream fileStream = File.OpenRead(string.Concat(pFilename));
			using StreamReader reader = new StreamReader(fileStream);
			do
			{
				Buf = reader.ReadLine();
				if (Buf != null)
				{
					if ((Buf[0] != '/') && (Buf[0] != '*'))
					{
						string[] data = Buf.Split();
						Type = uint.Parse(data[0]);
						Mode = uint.Parse(data[1]);
						Name = data[2].ToCharArray();
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
						Symbol = data[18].ToCharArray();

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

							Result = cInputGetNewTypeDataPointer(Name.ToSbyteArray(), (DATA8)Type, (DATA8)Mode, (DATA8)Connection, out pTypes);
							//            printf("cInputTypeDataInit\r\n");
							if (Result == OK)
							{
								pTypes = Tmp;

								Count = 0;
								while ((Name.Length > Count) && (Count < TYPE_NAME_LENGTH))
								{
									if (Name[Count] == '_')
									{
										pTypes.Name[Count] = (sbyte)' ';
									}
									else
									{
										pTypes.Name[Count] = (sbyte)Name[Count];
									}
									Count++;
								}
								pTypes.Name[Count] = 0;

								if (Symbol[0] == '_')
								{
									pTypes.Symbol[0] = 0;
								}
								else
								{
									Count = 0;
									while ((Symbol.Length > Count) && (Count < SYMBOL_LENGTH))
									{
										if (Symbol[Count] == '_')
										{
											pTypes.Symbol[Count] = (sbyte)' ';
										}
										else
										{
											pTypes.Symbol[Count] = (sbyte)Symbol[Count];
										}
										Count++;
									}
									pTypes.Symbol[Count] = 0;
								}
								if (Tmp.Connection == CONN_NXT_IIC)
								{ // NXT IIC sensor

									// setup string + poll string
									// 3 0x01420000 2 0x01000000

									data = Buf.Split();
									Type = uint.Parse(data[0]);
									Mode = uint.Parse(data[1]);
									Name = data[2].ToCharArray();
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
									Symbol = data[18].ToCharArray();
									Manufacturer = data[19].ToCharArray();
									SensorType = data[20].ToCharArray();
									SetupLng = uint.Parse(data[21]);
									SetupString = Convert.ToUInt32(data[22], 16);
									PollLng = uint.Parse(data[23]);
									PollString = Convert.ToUInt32(data[24], 16);
									ReadLng = int.Parse(data[25]);

									Count = 26; // anime hahaha
									if (Count == (TYPE_PARAMETERS + 7))
									{
										cInputInsertNewIicString((sbyte)Type, (sbyte)Mode, Manufacturer.ToSbyteArray(), SensorType.ToSbyteArray(), (DATA8)SetupLng, (ULONG)SetupString, (DATA8)PollLng, (ULONG)PollString, (DATA8)ReadLng);
										//                  printf("%02u %01u IIC %u 0x%08X %u 0x%08X %u\r\n",Type,Mode,SetupLng,SetupString,PollLng,PollString,ReadLng);
									}
								}
							}
						}
					}
				}
			}
			while (Buf != null);

			Result = OK;

			return (Result);
		}

		void cInputTypeDataInit()
		{
			char[] PrgNameBuf = new char[255];
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
			while ((Index < (GH.InputInstance.MaxDeviceTypes + 1)) && (TypeDefault[Index].Name != null))
			{
				GH.InputInstance.TypeData[Index] = TypeDefault[Index];

				CommonHelper.Snprintf(GH.InputInstance.TypeData[Index].Name, 0, TYPE_NAME_LENGTH + 1, TypeDefault[Index].Name);

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

			//  printf("Search start\r\n");
			CommonHelper.Snprintf(PrgNameBuf, 0, vmFILENAMESIZE, vmSETTINGS_DIR.ToCharArray(), "/".ToCharArray(), TYPEDATE_FILE_NAME.ToCharArray(), EXT_CONFIG.ToCharArray());

			if (cInputInsertTypeData(PrgNameBuf) == OK)
			{
				TypeDataFound = 1;
			}

			for (Index = TYPE_THIRD_PARTY_START; Index <= TYPE_THIRD_PARTY_END; Index++)
			{
				CommonHelper.Snprintf(PrgNameBuf, 0, vmFILENAMESIZE, vmSETTINGS_DIR.ToCharArray(), "/".ToCharArray(), TYPEDATE_FILE_NAME.ToCharArray(), Index.ToString("00").ToCharArray(), EXT_CONFIG.ToCharArray());
				if (cInputInsertTypeData(PrgNameBuf) == OK)
				{
					TypeDataFound = 1;
				}
			}
			//  printf("Done\r\n");

			if (TypeDataFound == 0)
			{
				GH.Ev3System.Logger.LogWarning($"Fail {TYPEDATA_TABEL_FULL} in {System.Environment.StackTrace}");
			}
		}

		RESULT cInputSetupDevice(DATA8 Device, DATA8 Repeat, DATA16 Time, DATA8 WrLng, DATA8[] pWrData, DATA8 RdLng, DATA8[] pRdData)
		{

			GH.InputInstance.IicDat.Result = RESULT.FAIL;

			if (Device < INPUTS)
			{
				if (GH.InputInstance.DeviceData[Device].Connection == CONN_NXT_IIC)
				{ // Device is an IIC device

					if (GH.InputInstance.IicFile >= MIN_HANDLE)
					{ // Driver installed

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

						GH.InputInstance.IicDat.Result = RESULT.BUSY;
						GH.InputInstance.IicDat.Port = Device;
						GH.InputInstance.IicDat.Repeat = Repeat;
						GH.InputInstance.IicDat.Time = Time;
						GH.InputInstance.IicDat.WrLng = WrLng;
						GH.InputInstance.IicDat.RdLng = RdLng;

						Array.Copy(pWrData, 0, GH.InputInstance.IicDat.WrData, 0, GH.InputInstance.IicDat.WrLng);

						GH.Ev3System.InputHandler.IoctlI2c(IIC_SETUP, GH.InputInstance.IicDat);

						if (GH.InputInstance.IicDat.Result == OK)
						{
							Array.Copy(GH.InputInstance.IicDat.RdData, 0, pRdData, 0, GH.InputInstance.IicDat.RdLng);
						}
					}
				}
			}

			return (GH.InputInstance.IicDat.Result);
		}

		RESULT cInputFindDumbInputDevice(DATA8 Device, DATA8 Type, DATA8 Mode, ref UWORD pTypeIndex)
		{
			RESULT Result = RESULT.FAIL;
			UWORD IdValue = 0;
			UWORD Index = 0;
			UWORD Tmp;

			// get actual id value
			IdValue = (ushort)CtoV((ushort)GH.InputInstance.Analog.InPin1[Device]);

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
							pTypeIndex = Index;

						}
						else
						{ // next search

							if (Type == GH.InputInstance.TypeData[Index].Type)
							{ //

								pTypeIndex = Index;
							}
						}
						if (Mode == GH.InputInstance.TypeData[Index].Mode)
						{ // mode match

							// "type data" entry found
							pTypeIndex = Index;

							// skip looping
							Result = OK;
						}
					}
				}
				Index++;
			}

			return (Result);
		}


		RESULT cInputFindDumbOutputDevice(DATA8 Device, DATA8 Type, DATA8 Mode, ref UWORD pTypeIndex)
		{
			RESULT Result = RESULT.FAIL;
			UWORD IdValue = 0;
			UWORD Index = 0;
			UWORD Tmp;

			// get actual id value
			IdValue = (ushort)GH.InputInstance.Analog.OutPin5Low[Device - INPUT_DEVICES];

			while ((Index < GH.InputInstance.MaxDeviceTypes) && (Result != OK))
			{
				Tmp = GH.InputInstance.TypeData[Index].IdValue;

				if (Tmp >= OUT5_ID_HYSTERESIS)
				{
					if ((IdValue >= (Tmp - OUT5_ID_HYSTERESIS)) && (IdValue < (Tmp + OUT5_ID_HYSTERESIS)))
					{ // id value match

						// "type data" entry found
						pTypeIndex = Index;

						if (Mode == GH.InputInstance.TypeData[Index].Mode)
						{ // mode match

							// "type data" entry found
							pTypeIndex = Index;

							// skip looping
							Result = OK;
						}
					}
				}
				Index++;
			}

			return (Result);
		}

		RESULT cInputFindDevice(DATA8 Type, DATA8 Mode, ref UWORD pTypeIndex)
		{
			RESULT Result = RESULT.FAIL;
			UWORD Index = 0;

			while ((Index < GH.InputInstance.MaxDeviceTypes) && (Result != OK))
			{
				if (Type == GH.InputInstance.TypeData[Index].Type)
				{ // type match

					pTypeIndex = Index;

					if (Mode == GH.InputInstance.TypeData[Index].Mode)
					{ // mode match

						// "type data" entry found
						pTypeIndex = Index;

						// skip looping
						Result = OK;
					}
				}
				Index++;
			}

			return (Result);
		}


		void cInputResetDevice(DATA8 Device, UWORD TypeIndex)
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


		void cInputSetDeviceType(DATA8 Device, DATA8 Type, DATA8 Mode, int Line)
		{
			UWORD Index;
			char[] Buf = new char[INPUTS * 2 + 1];
			UWORD TypeIndex;
			DATA8 Layer = 0;
			DATA8 Port = 0;
			DATA8 Output = 0;

			if (cInputExpandDevice(Device, ref Layer, ref Port, ref Output) == OK)
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

							cInputFindDumbInputDevice(Device, Type, Mode, ref TypeIndex);
						}

						// IF NOT FOUND YET - TRY TO FIND TYPE ANYWAY
						if (TypeIndex == GH.InputInstance.UnknownIndex)
						{ // device not found or not "dumb" input/output device

							cInputFindDevice(Type, Mode, ref TypeIndex);
						}

						if (GH.InputInstance.DeviceData[Device].TypeIndex != TypeIndex)
						{ // type or mode has changed

							cInputResetDevice(Device, TypeIndex);

							GH.InputInstance.Uart.Status[Device] &= ~UART_DATA_READY;
							GH.InputInstance.Iic.Status[Device] &= ~IIC_DATA_READY;
							GH.InputInstance.Analog.Updated[Device] = 0;

							if (GH.InputInstance.DeviceData[Device].Connection == CONN_NXT_IIC)
							{ // Device is an IIC device

								cInputGetIicString(GH.InputInstance.DeviceType[Device], GH.InputInstance.DeviceMode[Device], GH.InputInstance.IicStr);
								GH.InputInstance.IicStr.Port = Device;
								GH.InputInstance.IicStr.Time = (short)GH.InputInstance.DeviceData[Device].InvalidTime;

								if ((GH.InputInstance.IicStr.SetupLng != 0) || (GH.InputInstance.IicStr.PollLng != 0))
								{
									//              printf("%u %-4u %-3u %01u IIC %u 0x%08X %u 0x%08X %d\r\n",InputInstance.IicStr.Port,InputInstance.IicStr.Time,InputInstance.IicStr.Type,InputInstance.IicStr.Mode,InputInstance.IicStr.SetupLng,InputInstance.IicStr.SetupString,InputInstance.IicStr.PollLng,InputInstance.IicStr.PollString,InputInstance.IicStr.ReadLng);

									if (GH.InputInstance.IicFile >= MIN_HANDLE)
									{
										// TODO: wtf
										// ioctl(InputInstance.IicFile, IIC_SET, &InputInstance.IicStr);
									}
								}
							}

							// SETUP DRIVERS
							for (Index = 0; Index < INPUTS; Index++)
							{ // Initialise pin setup string to do nothing

								Buf[Index] = '-';
							}
							Buf[Index] = (char)0;

							// insert "pins" in setup string
							Buf[Device] = (char)GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Device].TypeIndex].Pins;

							// write setup string to "Device Connection Manager" driver
							if (GH.InputInstance.DcmFile >= MIN_HANDLE)
							{
								// TODO: wtf
								//write(InputInstance.DcmFile, Buf, INPUTS);
							}

							for (Index = 0; Index < INPUTS; Index++)
							{ // build setup string for UART and IIC driver

								GH.InputInstance.DevCon.Connection[Index] = GH.InputInstance.DeviceData[Index].Connection;
								GH.InputInstance.DevCon.Type[Index] = GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Index].TypeIndex].Type;
								GH.InputInstance.DevCon.Mode[Index] = GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Index].TypeIndex].Mode;
							}

							// write setup string to "UART Device Controller" driver
							if (GH.InputInstance.UartFile >= MIN_HANDLE)
							{
								// TODO: wtf
								// ioctl(InputInstance.UartFile, UART_SET_CONN, &InputInstance.DevCon);
							}
							if (GH.InputInstance.IicFile >= MIN_HANDLE)
							{
								// TODO: wtf
								// ioctl(InputInstance.IicFile, IIC_SET_CONN, &InputInstance.DevCon);
							}
						}
					}
					else
					{ // Output device

						// TRY TO FIND DUMB OUTPUT DEVICE
						if (GH.InputInstance.DeviceData[Device].Connection == CONN_OUTPUT_DUMB)
						{ // search "type data" for matching "dumb" output device

							cInputFindDumbInputDevice(Device, Type, Mode, ref TypeIndex);
						}

						// IF NOT FOUND YET - TRY TO FIND TYPE ANYWAY
						if (TypeIndex == GH.InputInstance.UnknownIndex)
						{ // device not found or not "dumb" input/output device

							cInputFindDevice(Type, Mode, ref TypeIndex);
						}

						if (GH.InputInstance.DeviceData[Device].TypeIndex != TypeIndex)
						{ // type or mode has changed

							cInputResetDevice(Device, TypeIndex);

							for (Index = 0; Index < OUTPUT_PORTS; Index++)
							{ // build setup string "type" for output

								Buf[Index] = (char)GH.InputInstance.DeviceType[Index + INPUT_DEVICES];
							}
							Buf[Index] = (char)0;
							GH.Output.cOutputSetTypes(Buf);
						}
					}


				}
				else
				{ // Not local device

					// IF NOT FOUND YET - TRY TO FIND TYPE ANYWAY
					if (TypeIndex == GH.InputInstance.UnknownIndex)
					{ // device not found or not "dumb" input/output device

						cInputFindDevice(Type, Mode, ref TypeIndex);
					}

					if (GH.InputInstance.DeviceData[Device].TypeIndex != TypeIndex)
					{ // type or mode has changed

						// TODO: daisy chain was here

					}
				}
			}
		}

		void cInputCalDataInit()
		{
			DATA8 Type;
			DATA8 Mode;
			int File;
			char[] PrgNameBuf = new char[vmFILENAMESIZE];

			CommonHelper.Snprintf(PrgNameBuf, 0, vmFILENAMESIZE, vmSETTINGS_DIR.ToCharArray(), "/".ToCharArray(), vmCALDATA_FILE_NAME.ToCharArray(), vmEXT_CONFIG.ToCharArray());
			// TODO: calibration
			//File = open(PrgNameBuf, O_RDONLY);
			//if (File >= MIN_HANDLE)
			//{
			//	if (read(File, (UBYTE*)&InputInstance.Calib, sizeof(InputInstance.Calib)) != sizeof(InputInstance.Calib))
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
			//			InputInstance.Calib[Type][Mode].InUse = 0;
			//		}
			//	}
			//}
		}

		void cInputCalDataExit()
		{
			int File;
			char[] PrgNameBuf = new char[vmFILENAMESIZE];

			// TODO: calibration
			//snprintf(PrgNameBuf, vmFILENAMESIZE, "%s/%s%s", vmSETTINGS_DIR, vmCALDATA_FILE_NAME, vmEXT_CONFIG);
			//File = open(PrgNameBuf, O_CREAT | O_WRONLY | O_TRUNC, SYSPERMISSIONS);
			//if (File >= MIN_HANDLE)
			//{
			//	write(File, (UBYTE*)&InputInstance.Calib, sizeof(InputInstance.Calib));
			//	close(File);
			//}
		}

		void cInputCalcFullScale(ref UWORD pRawVal, UWORD ZeroPointOffset, UBYTE PctFullScale, UBYTE InvStatus)
		{
			if (pRawVal >= ZeroPointOffset)
			{
				pRawVal -= ZeroPointOffset;
			}
			else
			{
				pRawVal = 0;
			}

			pRawVal = (ushort)((pRawVal * 100) / PctFullScale);
			if (pRawVal > SENSOR_RESOLUTION)
			{
				pRawVal = SENSOR_RESOLUTION;
			}
			if (1 == InvStatus)
			{
				pRawVal = (ushort)(SENSOR_RESOLUTION - pRawVal);
			}
		}


		void cInputCalibrateColor(COLORSTRUCT pC, UWORD[] pNewVals)
		{
			UBYTE CalRange;

			if ((pC.ADRaw[BLANK]) < pC.CalLimits[1])
			{
				CalRange = 2;
			}
			else
			{
				if ((pC.ADRaw[BLANK]) < pC.CalLimits[0])
				{
					CalRange = 1;
				}
				else
				{
					CalRange = 0;
				}
			}

			pNewVals[RED] = 0;
			if ((pC.ADRaw[RED]) > (pC.ADRaw[BLANK]))
			{
				pNewVals[RED] = (UWORD)(((ULONG)((pC.ADRaw[RED]) - (pC.ADRaw[BLANK])) * (pC.Calibration[CalRange][RED])) >> 16);
			}

			pNewVals[GREEN] = 0;
			if ((pC.ADRaw[GREEN]) > (pC.ADRaw[BLANK]))
			{
				pNewVals[GREEN] = (UWORD)(((ULONG)((pC.ADRaw[GREEN]) - (pC.ADRaw[BLANK])) * (pC.Calibration[CalRange][GREEN])) >> 16);
			}

			pNewVals[BLUE] = 0;
			if ((pC.ADRaw[BLUE]) > (pC.ADRaw[BLANK]))
			{
				pNewVals[BLUE] = (UWORD)(((ULONG)((pC.ADRaw[BLUE]) - (pC.ADRaw[BLANK])) * (pC.Calibration[CalRange][BLUE])) >> 16);
			}

			pNewVals[BLANK] = (pC.ADRaw[BLANK]);
			cInputCalcFullScale(ref pNewVals[BLANK], COLORSENSORBGMIN, COLORSENSORBGPCTDYN, 0);
			(pNewVals[BLANK]) = (UWORD)(((ULONG)(pNewVals[BLANK]) * (pC.Calibration[CalRange][BLANK])) >> 16);
		}


		DATAF cInputCalculateColor(COLORSTRUCT pC)
		{
			DATAF Result;

			Result = DATAF_NAN;

			// Color Sensor values has been calculated -
			// now calculate the color and put it in Sensor value
			if (((pC.SensorRaw[RED]) > (pC.SensorRaw[BLUE])) &&
				((pC.SensorRaw[RED]) > (pC.SensorRaw[GREEN])))
			{

				// If all 3 colors are less than 65 OR (Less that 110 and bg less than 40)
				if (((pC.SensorRaw[RED]) < 65) ||
					(((pC.SensorRaw[BLANK]) < 40) && ((pC.SensorRaw[RED]) < 110)))
				{
					Result = (DATAF)BLACKCOLOR;
				}
				else
				{
					if (((((pC.SensorRaw[BLUE]) >> 2) + ((pC.SensorRaw[BLUE]) >> 3) + (pC.SensorRaw[BLUE])) < (pC.SensorRaw[GREEN])) &&
						((((pC.SensorRaw[GREEN]) << 1)) > (pC.SensorRaw[RED])))
					{
						Result = (DATAF)YELLOWCOLOR;
					}
					else
					{

						if ((((pC.SensorRaw[GREEN]) << 1) - ((pC.SensorRaw[GREEN]) >> 2)) < (pC.SensorRaw[RED]))
						{

							Result = (DATAF)REDCOLOR;
						}
						else
						{

							if ((((pC.SensorRaw[BLUE]) < 70) ||
								((pC.SensorRaw[GREEN]) < 70)) ||
							   (((pC.SensorRaw[BLANK]) < 140) && ((pC.SensorRaw[RED]) < 140)))
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
				if ((pC.SensorRaw[GREEN]) > (pC.SensorRaw[BLUE]))
				{

					// Green is the dominant color
					// If all 3 colors are less than 40 OR (Less that 70 and bg less than 20)
					if (((pC.SensorRaw[GREEN]) < 40) ||
						(((pC.SensorRaw[BLANK]) < 30) && ((pC.SensorRaw[GREEN]) < 70)))
					{
						Result = (DATAF)BLACKCOLOR;
					}
					else
					{
						if ((((pC.SensorRaw[BLUE]) << 1)) < (pC.SensorRaw[RED]))
						{
							Result = (DATAF)YELLOWCOLOR;
						}
						else
						{
							if ((((pC.SensorRaw[RED]) + ((pC.SensorRaw[RED]) >> 2)) < (pC.SensorRaw[GREEN])) ||
								(((pC.SensorRaw[BLUE]) + ((pC.SensorRaw[BLUE]) >> 2)) < (pC.SensorRaw[GREEN])))
							{
								Result = (DATAF)GREENCOLOR;
							}
							else
							{
								if ((((pC.SensorRaw[RED]) < 70) ||
									((pC.SensorRaw[BLUE]) < 70)) ||
									(((pC.SensorRaw[BLANK]) < 140) && ((pC.SensorRaw[GREEN]) < 140)))
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
					if (((pC.SensorRaw[BLUE]) < 48) ||
						(((pC.SensorRaw[BLANK]) < 25) && ((pC.SensorRaw[BLUE]) < 85)))
					{
						Result = (DATAF)BLACKCOLOR;
					}
					else
					{
						if ((((((pC.SensorRaw[RED]) * 48) >> 5) < (pC.SensorRaw[BLUE])) &&
							((((pC.SensorRaw[GREEN]) * 48) >> 5) < (pC.SensorRaw[BLUE])))
							||
							(((((pC.SensorRaw[RED]) * 58) >> 5) < (pC.SensorRaw[BLUE])) ||
							 ((((pC.SensorRaw[GREEN]) * 58) >> 5) < (pC.SensorRaw[BLUE]))))
						{
							Result = (DATAF)BLUECOLOR;
						}
						else
						{

							// Color is white or Black
							if ((((pC.SensorRaw[RED]) < 60) ||
								((pC.SensorRaw[GREEN]) < 60)) ||
							   (((pC.SensorRaw[BLANK]) < 110) && ((pC.SensorRaw[BLUE]) < 120)))
							{
								Result = (DATAF)BLACKCOLOR;
							}
							else
							{
								if ((((pC.SensorRaw[RED]) + ((pC.SensorRaw[RED]) >> 3)) < (pC.SensorRaw[BLUE])) ||
									(((pC.SensorRaw[GREEN]) + ((pC.SensorRaw[GREEN]) >> 3)) < (pC.SensorRaw[BLUE])))
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

		// TODO: there was daisy RESULT    cInputGetColor(DATA8 Device,DATA8 *pData)

		DATAF cInputGetColor(DATA8 Device, DATA8 Index)
		{
			DATAF Result;

			Result = DATAF_NAN;

			cInputCalibrateColor(GH.InputInstance.Analog.NxtCol[Device], GH.InputInstance.Analog.NxtCol[Device].SensorRaw);

			switch (GH.InputInstance.DeviceMode[Device])
			{
				case 2:
					{ // NXT-COL-COL

						Result = cInputCalculateColor(GH.InputInstance.Analog.NxtCol[Device]);
					}
					break;

				case 1:
					{ // NXT-COL-AMB

						Result = GH.InputInstance.Analog.NxtCol[Device].ADRaw[BLANK];
					}
					break;

				case 0:
					{ // NXT-COL-RED

						Result = GH.InputInstance.Analog.NxtCol[Device].ADRaw[RED];
					}
					break;

				case 3:
					{ // NXT-COL-GRN

						Result = GH.InputInstance.Analog.NxtCol[Device].ADRaw[GREEN];
					}
					break;

				case 4:
					{ // NXT-COL-BLU

						Result = GH.InputInstance.Analog.NxtCol[Device].ADRaw[BLUE];
					}
					break;

				case 5:
					{ // NXT-COL-RAW

						if (Index < COLORS)
						{
							Result = (DATAF)GH.InputInstance.Analog.NxtCol[Device].SensorRaw[Index];
						}
					}
					break;
			}

			return (Result);
		}

		// TODO: here was also some daisy shite

		DATAF cInputReadDeviceRaw(DATA8 Device, DATA8 Index, DATA16 Time, ref DATA16 pInit)
		{
			DATAF Result;
			DATA8 DataSets;
			sbyte[] pResult;

			Result = DATAF_NAN;


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

								pResult = GH.InputInstance.Uart.Raw[Device];
								DataSets = GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Device].TypeIndex].DataSets;

								if (Index < DataSets)
								{
									switch (GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Device].TypeIndex].Format & 0x0F)
									{
										case DATA_8:
											{
												GH.InputInstance.DeviceData[Device].Raw[Index] = (DATAF)pResult[Index];
											}
											break;

										case DATA_16:
											{
												GH.InputInstance.DeviceData[Device].Raw[Index] = (DATAF)(BitConverter.ToInt16(new byte[] { (byte)pResult[Index], (byte)pResult[Index + 1] }));
											}
											break;

										case DATA_32:
											{
												GH.InputInstance.DeviceData[Device].Raw[Index] = (DATAF)(BitConverter.ToInt32(new byte[] { (byte)pResult[Index], (byte)pResult[Index + 1], (byte)pResult[Index + 2], (byte)pResult[Index + 3] }));
											}
											break;

										case DATA_F:
											{
												GH.InputInstance.DeviceData[Device].Raw[Index] = (DATAF)(BitConverter.ToSingle(new byte[] { (byte)pResult[Index], (byte)pResult[Index + 1], (byte)pResult[Index + 2], (byte)pResult[Index + 3] }));
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

									GH.InputInstance.DeviceData[Device].Raw[Index] = (DATAF)GH.InputInstance.Analog.InPin6[Device];
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

										GH.InputInstance.DeviceData[Device].Raw[Index] = (DATAF)GH.InputInstance.Analog.InPin1[Device];
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
								GH.InputInstance.DeviceData[Device].Raw[Index] = (DATAF)GH.OutputInstance.MotorData[Device - INPUT_DEVICES].Speed;
							}
							else
							{
								GH.InputInstance.DeviceData[Device].Raw[Index] = (DATAF)GH.OutputInstance.MotorData[Device - INPUT_DEVICES].TachoSensor;
							}
						}

					}
					Result = GH.InputInstance.DeviceData[Device].Raw[Index];
				}
			}

			return (Result);
		}

		void cInputWriteDeviceRaw(DATA8 Device, DATA8 Connection, DATA8 Type, DATAF DataF)
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
					cInputSetDeviceType(Device, Type, 0, 0);
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
							GH.InputInstance.Iic.Raw[Device][0] = (sbyte)Byte;
							GH.InputInstance.Iic.Raw[Device][1] = 0;
						}
						else
						{
							GH.InputInstance.Iic.Raw[Device][0] = (sbyte)(UBYTE)Word;
							GH.InputInstance.Iic.Raw[Device][1] = (sbyte)(UBYTE)(Word >> 8);
						}
						GH.InputInstance.DeviceData[Device].DevStatus = OK;
					}
					else
					{
						if (GH.InputInstance.DeviceData[Device].Connection == CONN_INPUT_DUMB)
						{
							GH.InputInstance.Analog.InPin6[Device] = (DATA16)DataF;
						}
						else
						{
							GH.InputInstance.Analog.InPin1[Device] = (DATA16)DataF;
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
					cInputSetDeviceType(Device, Type, 0, 0);
				}
				if (GH.InputInstance.DeviceMode[Device] == 2)
				{
					GH.OutputInstance.MotorData[Device - INPUT_DEVICES].Speed = (sbyte)(DATA32)DataF;
				}
				else
				{
					GH.OutputInstance.MotorData[Device - INPUT_DEVICES].TachoSensor = (DATA32)DataF;
				}
				GH.InputInstance.DeviceData[Device].DevStatus = OK;
			}
		}

		DATA8 cInputReadDevicePct(DATA8 Device, DATA8 Index, DATA16 Time, ref DATA16 pInit)
		{
			DATA8 Result = DATA8_NAN;
			UWORD TypeIndex;
			DATAF Raw;
			DATA8 Type;
			DATA8 Mode;
			DATAF Min;
			DATAF Max;
			DATAF Pct;

			Raw = cInputReadDeviceRaw(Device, Index, Time, ref pInit);

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

		DATAF cInputReadDeviceSi(DATA8 Device, DATA8 Index, DATA16 Time, ref DATA16 pInit)
		{
			UWORD TypeIndex;
			DATAF Raw;
			DATA8 Type;
			DATA8 Mode;
			DATA8 Connection;
			DATAF Min;
			DATAF Max;

			Raw = cInputReadDeviceRaw(Device, Index, Time, ref pInit);

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


		RESULT cInputCheckUartInfo(UBYTE Port)
		{
			RESULT Result = RESULT.BUSY;
			TYPES pTmp;


			if (GH.InputInstance.UartFile >= MIN_HANDLE)
			{ // Driver installed

				if ((GH.InputInstance.Uart.Status[Port] & UART_PORT_CHANGED) != 0)
				{ // something has changed

					if (GH.InputInstance.TmpMode[Port] > 0)
					{ // check each mode

						GH.InputInstance.TmpMode[Port]--;

						// Get info
						GH.InputInstance.UartCtl.Port = (sbyte)Port;
						GH.InputInstance.UartCtl.Mode = GH.InputInstance.TmpMode[Port];

						// TODO: wtf
						// ioctl(InputInstance.UartFile, UART_READ_MODE_INFO, &InputInstance.UartCtl);

						if (GH.InputInstance.UartCtl.TypeData.Name != null)
						{ // Info available

							Result = cInputGetNewTypeDataPointer(GH.InputInstance.UartCtl.TypeData.Name, GH.InputInstance.UartCtl.TypeData.Type, GH.InputInstance.UartCtl.TypeData.Mode, CONN_INPUT_UART, out pTmp);
							if (pTmp != null)
							{ // Tabel index found

								if (GH.InputInstance.DeviceType[Port] == TYPE_UNKNOWN)
								{ // Use first mode info to set type

									GH.InputInstance.DeviceType[Port] = GH.InputInstance.UartCtl.TypeData.Type;
								}

								if (Result == OK)
								{ // New mode

									// Insert in tabel
									GH.InputInstance.UartCtl.TypeData = pTmp.Clone() as TYPES;
								}
								// TODO: daisy shite was here
							}
						}
					}
					else
					{ // All modes received set device mode 0

						GH.InputInstance.UartCtl.Port = (sbyte)Port;
						// TODO: wtf
						// ioctl(InputInstance.UartFile, UART_CLEAR_CHANGED, &InputInstance.UartCtl);
						GH.InputInstance.Uart.Status[Port] &= ~UART_PORT_CHANGED;
						cInputSetDeviceType((sbyte)Port, GH.InputInstance.DeviceType[Port], 0, 0);
					}
				}

				if ((GH.InputInstance.Uart.Status[Port] & UART_DATA_READY) != 0)
				{
					if ((GH.InputInstance.Uart.Status[Port] & UART_PORT_CHANGED) == 0)
					{
						Result = OK;
					}
				}
			}

			return (Result);
		}

		RESULT cInputCheckIicInfo(UBYTE Port)
		{
			RESULT Result = RESULT.BUSY;
			DATA8 Type;
			DATA8 Mode;
			UWORD Index;

			if (GH.InputInstance.IicFile >= MIN_HANDLE)
			{ // Driver installed

				if (GH.InputInstance.Analog.InDcm[Port] == TYPE_NXT_IIC)
				{

					if (GH.InputInstance.Iic.Changed[Port] != 0)
					{ // something has changed

						GH.InputInstance.IicStr.Port = (sbyte)Port;

						//TODO: wtf
						// ioctl(InputInstance.IicFile, IIC_READ_TYPE_INFO, &InputInstance.IicStr);

						Index = IIC_NAME_LENGTH;
						while ((Index != 0) && ((GH.InputInstance.IicStr.Manufacturer[Index] == ' ') || (GH.InputInstance.IicStr.Manufacturer[Index] == 0)))
						{
							GH.InputInstance.IicStr.Manufacturer[Index] = 0;
							Index--;
						}
						Index = IIC_NAME_LENGTH;
						while ((Index != 0) && ((GH.InputInstance.IicStr.SensorType[Index] == ' ') || (GH.InputInstance.IicStr.SensorType[Index] == 0)))
						{
							GH.InputInstance.IicStr.SensorType[Index] = 0;
							Index--;
						}

						// Find 3th party type
						Type = TYPE_IIC_UNKNOWN;
						Mode = 0;
						Index = 0;
						while ((Index < GH.InputInstance.IicDeviceTypes) && (Type == TYPE_IIC_UNKNOWN))
						{ // Check list

							if (CommonHelper.Strcmp(GH.InputInstance.IicStr.Manufacturer, GH.InputInstance.IicString[Index].Manufacturer) == 0)
							{ // Manufacturer found

								if (CommonHelper.Strcmp(GH.InputInstance.IicStr.SensorType, GH.InputInstance.IicString[Index].SensorType) == 0)
								{ // Type found

									Type = GH.InputInstance.IicString[Index].Type;
									Mode = GH.InputInstance.IicString[Index].Mode;
								}
							}
							Index++;
						}
						cInputSetDeviceType((sbyte)Port, Type, Mode, 0);
					}
					GH.InputInstance.Iic.Changed[Port] = 0;
				}
				if ((GH.InputInstance.Iic.Status[Port] & IIC_DATA_READY) != 0)
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

		void cInputDcmUpdate(UWORD Time)
		{
			RESULT Result = RESULT.BUSY;
			DATA8 Device;
			DATA8 Port;

			// TODO: warning: daisy removed

			if (GH.InputInstance.DCMUpdate != 0)
			{
				for (Device = 0; Device < DEVICES; Device++)
				{

					if ((Device >= 0) && (Device < INPUTS))
					{ // Device is local input port

						Port = Device;

						if (GH.InputInstance.DeviceData[Device].Connection != GH.InputInstance.Analog.InConn[Port])
						{ // Connection type has changed

							GH.InputInstance.DeviceData[Device].Connection = GH.InputInstance.Analog.InConn[Port];
							cInputSetDeviceType(Device, GH.InputInstance.Analog.InDcm[Port], 0, 0);
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

								if (GH.InputInstance.Analog.Updated[Device] != 0)
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

							if (GH.InputInstance.DeviceData[Device].Connection != GH.InputInstance.Analog.OutConn[Port])
							{ // Connection type has changed

								GH.InputInstance.DeviceData[Device].Connection = GH.InputInstance.Analog.OutConn[Port];
								cInputSetDeviceType(Device, GH.InputInstance.Analog.OutDcm[Port], 0, 0);
							}

							Result = OK;

						}
						else
						{ // Device is from daisy chain
							GH.InputInstance.DeviceData[Device].Connection = CONN_NONE;
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
		}

		public RESULT cInputStartTypeDataUpload()
		{
			GH.InputInstance.TypeDataIndex = 0;
			GH.InputInstance.TypeDataTimer = DELAY_TO_TYPEDATA;

			return (OK);
		}

		public void cInputUpdate(ushort Time)
		{
			DATA8 Device;
			DATAF Value = 0;
			DATAF Diff;

			cInputDcmUpdate(Time);

			for (Device = 0; Device < INPUT_PORTS; Device++)
			{ // check each port for changes

				if ((GH.InputInstance.DeviceType[Device] == 1) || (GH.InputInstance.DeviceType[Device] == 16))
				{
					if (GH.InputInstance.DeviceType[Device] == 1)
					{
						Value = (DATAF)GH.InputInstance.Analog.InPin1[Device];
					}
					if (GH.InputInstance.DeviceType[Device] == 16)
					{
						Value = (DATAF)GH.InputInstance.Analog.InPin6[Device];
					}

					Diff = Value - GH.InputInstance.DeviceData[Device].OldRaw;

					if (Diff >= (DATAF)500)
					{
						GH.InputInstance.DeviceData[Device].Bumps += (DATA32)1;
					}
					if (Diff <= (DATAF)500)
					{
						GH.InputInstance.DeviceData[Device].Changes += (DATA32)1;
					}

					GH.InputInstance.DeviceData[Device].OldRaw = Value;
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
			ANALOG pAdcTmp;
			UART pUartTmp;
			IIC pIicTmp;
			PRGID TmpPrgId;
			UWORD Tmp;
			UWORD Set;

			GH.InputInstance.TypeDataIndex = DATA16_MAX;
			GH.InputInstance.MaxDeviceTypes = 3;
			GH.InputInstance.IicDeviceTypes = 1;
			GH.InputInstance.DCMUpdate = 1;

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
			RESULT Result = OK;

			return (Result);
		}

		public RESULT cInputClose()
		{
			RESULT Result = OK;

			return (Result);
		}

		public RESULT cInputExit()
		{
			RESULT Result = RESULT.FAIL;

			cInputCalDataExit();

			Result = OK;

			return (Result);
		}

		DATA8 cInputGetDevice()
		{
			DATA8 Layer;
			DATA8 No;

			Layer = (DATA8)GH.Lms.PrimParPointer();
			No = (DATA8)GH.Lms.PrimParPointer();

			return ((sbyte)(No + (Layer * INPUT_PORTS)));
		}

		void cInputSetType(DATA8 Device, DATA8 Type, DATA8 Mode, int Line)
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

		public void cInputDeviceList()
		{
			PRGID TmpPrgId;
			DATA8 Length;
			DATA8[] pDevices;
			DATA8[] pChanged;
			DATA8 Count;

			TmpPrgId = GH.Lms.CurrentProgramId();
			Length = (DATA8)GH.Lms.PrimParPointer();
			pDevices = (DATA8[])GH.Lms.PrimParPointer();
			pChanged = (DATA8[])GH.Lms.PrimParPointer();

			pChanged[0] = GH.InputInstance.ConfigurationChanged[TmpPrgId];
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

		public void cInputDevice()
		{
			IP TmpIp;
			int TmpIpInd;
			DATA8 Cmd;
			DATA8 Device = 0;
			DATA8 Layer;
			DATA8 Length;
			DATA8[] pDestination;
			DATA8 Count = 0;
			DATA8 Modes = 0;
			DATA8[] TmpName;
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
			DATA8[] pWrData;
			DATA8[] pRdData;
			UWORD TypeIndex = 0;
			RESULT Result;

			TmpIp = GH.Lms.GetObjectIp();
			TmpIpInd = GH.Lms.GetObjectIpInd();
			Cmd = (DATA8)GH.Lms.PrimParPointer();
			if ((Cmd != CAL_MINMAX) && (Cmd != CAL_MIN) && (Cmd != CAL_MAX) && (Cmd != CAL_DEFAULT) && (Cmd != CLR_ALL) && (Cmd != STOP_ALL))
			{
				Device = cInputGetDevice();
			}

			switch (Cmd)
			{ // Function

				case CAL_MINMAX:
					{
						Type = (DATA8)GH.Lms.PrimParPointer();
						Mode = (DATA8)GH.Lms.PrimParPointer();
						Min = (DATAF)(DATA32)GH.Lms.PrimParPointer();
						Max = (DATAF)(DATA32)GH.Lms.PrimParPointer();

						if ((Type > 0) && (Type < (MAX_DEVICE_TYPE + 1)) && (Mode >= 0) && (Mode < MAX_DEVICE_MODES))
						{
							GH.InputInstance.Calib[Type][Mode].Min = Min;
							GH.InputInstance.Calib[Type][Mode].Max = Max;
						}
					}
					break;

				case CAL_MIN:
					{
						Type = (DATA8)GH.Lms.PrimParPointer();
						Mode = (DATA8)GH.Lms.PrimParPointer();
						Min = (DATAF)(DATA32)GH.Lms.PrimParPointer();

						if ((Type > 0) && (Type < (MAX_DEVICE_TYPE + 1)) && (Mode >= 0) && (Mode < MAX_DEVICE_MODES))
						{
							if (cInputFindDevice(Type, Mode, ref TypeIndex) == OK)
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
						Type = (DATA8)GH.Lms.PrimParPointer();
						Mode = (DATA8)GH.Lms.PrimParPointer();
						Max = (DATAF)(DATA32)GH.Lms.PrimParPointer();

						if ((Type > 0) && (Type < (MAX_DEVICE_TYPE + 1)) && (Mode >= 0) && (Mode < MAX_DEVICE_MODES))
						{
							if (cInputFindDevice(Type, Mode, ref TypeIndex) == OK)
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
						Type = (DATA8)GH.Lms.PrimParPointer();
						Mode = (DATA8)GH.Lms.PrimParPointer();

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
						GH.Lms.PrimParPointer((DATA8)Type);
						GH.Lms.PrimParPointer((DATA8)Mode);
					}
					break;

				case GET_CONNECTION:
					{
						Connection = TYPE_NONE;

						if (Device < DEVICES)
						{
							Connection = GH.InputInstance.DeviceData[Device].Connection;
						}
						GH.Lms.PrimParPointer((DATA8)Connection);
					}
					break;

				case GET_NAME:
					{
						Length = (DATA8)GH.Lms.PrimParPointer();
						pDestination = (DATA8[])GH.Lms.PrimParPointer();
						Count = 0;

						if (Device < DEVICES)
						{

							TmpName = GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Device].TypeIndex].Name;

							if (GH.VMInstance.Handle >= 0)
							{
								Tmp = (DATA8)(TmpName.Length + 1);

								if (Length == -1)
								{
									Length = Tmp;
								}
								pDestination = CommonHelper.CastObjectArray<DATA8>(GH.Lms.VmMemoryResize(GH.VMInstance.Handle, (DATA32)Length));
							}
							if (pDestination != null)
							{
								while ((Count < (Length - 1)) && (TmpName.Length > Count))
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
						Length = (DATA8)GH.Lms.PrimParPointer();
						pDestination = (DATA8[])GH.Lms.PrimParPointer();
						Count = 0;

						if (Device < DEVICES)
						{

							TmpName = GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Device].TypeIndex].Symbol;

							if (GH.VMInstance.Handle >= 0)
							{
								Tmp = (DATA8)(TmpName.Length + 1);

								if (Length == -1)
								{
									Length = Tmp;
								}
								pDestination = CommonHelper.CastObjectArray<DATA8>(GH.Lms.VmMemoryResize(GH.VMInstance.Handle, (DATA32)Length));
							}
							if (pDestination != null)
							{
								while ((Count < (Length - 1)) && (TmpName.Length > Count))
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
						GH.Lms.PrimParPointer((DATA8)Count);
						GH.Lms.PrimParPointer((DATA8)Data8);
						GH.Lms.PrimParPointer((DATA8)Modes);
						GH.Lms.PrimParPointer((DATA8)Views);
					}
					break;

				case GET_RAW:
					{
						Data32 = DATA32_NAN;
						if (Device < DEVICES)
						{
							short _tmppp = 0;
							DataF = cInputReadDeviceRaw(Device, 0, 0, ref _tmppp);
							if (!float.IsNaN(DataF))
							{
								Data32 = (DATA32)DataF;
							}
						}
						GH.Lms.PrimParPointer((DATA32)Data32);
					}
					break;

				case GET_FIGURES:
					{
						if (Device < DEVICES)
						{
							Count = (DATA8)GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Device].TypeIndex].Figures;
							Data8 = (DATA8)GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Device].TypeIndex].Decimals;
						}
						GH.Lms.PrimParPointer((DATA8)Count);
						GH.Lms.PrimParPointer((DATA8)Data8);
					}
					break;

				case GET_MINMAX:
					{
						if (Device < DEVICES)
						{
							Min = (DATAF)GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Device].TypeIndex].SiMin;
							Max = (DATAF)GH.InputInstance.TypeData[GH.InputInstance.DeviceData[Device].TypeIndex].SiMax;
						}
						GH.Lms.PrimParPointer((DATAF)Min);
						GH.Lms.PrimParPointer((DATAF)Max);
					}
					break;

				case GET_MODENAME:
					{
						Mode = (DATA8)GH.Lms.PrimParPointer();
						Length = (DATA8)GH.Lms.PrimParPointer();
						pDestination = (DATA8[])GH.Lms.PrimParPointer();
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

											TmpName = GH.InputInstance.TypeData[Index].Name;

											if (GH.VMInstance.Handle >= 0)
											{
												Tmp = (DATA8)(TmpName.Length + 1);

												if (Length == -1)
												{
													Length = Tmp;
												}
												pDestination = CommonHelper.CastObjectArray<DATA8>(GH.Lms.VmMemoryResize(GH.VMInstance.Handle, (DATA32)Length));
											}
											if (pDestination != null)
											{
												while ((Count < (Length - 1)) && (TmpName.Length > Count))
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
						Type = (DATA8)GH.Lms.PrimParPointer();
						Data32 = (DATA32)GH.Lms.PrimParPointer();

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
						GH.Lms.PrimParPointer((DATAF)DataF);
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
						GH.Lms.PrimParPointer((DATAF)DataF);
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
						Type = (DATA8)GH.Lms.PrimParPointer();
						Mode = (DATA8)GH.Lms.PrimParPointer();
						Values = (DATA8)GH.Lms.PrimParPointer();

						Value = 0;
						Busy = 0;
						Owner = GH.Lms.CallingObjectId();

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

											GH.InputInstance.DeviceData[Device].Owner = Owner;
											cInputSetDeviceType(Device, Type, Mode, 0);
											GH.InputInstance.DeviceData[Device].TimeoutTimer = MAX_DEVICE_BUSY_TIME;
											GH.InputInstance.DeviceData[Device].Busy = 0;

											if (Device == TESTDEVICE)
											{
												GH.VMInstance.Status &= ~0x40;
											}

										}
										else
										{ // Another owner

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
											short _tmppp = 0;
											switch (Cmd)
											{
												case READY_PCT:
													{
														GH.Lms.PrimParPointer((DATA8)cInputReadDevicePct(Device, Value, 0, ref _tmppp));
													}
													break;

												case READY_RAW:
													{
														DataF = cInputReadDeviceRaw(Device, Value, 0, ref _tmppp);
														if (float.IsNaN(DataF))
														{
															GH.Lms.PrimParPointer((DATA32)DATA32_NAN);
														}
														else
														{
															GH.Lms.PrimParPointer((DATA32)DataF);
														}
													}
													break;

												case READY_SI:
													{
														DataF = (DATAF)cInputReadDeviceSi(Device, Value, 0, ref _tmppp);
														GH.Lms.PrimParPointer((DATAF)DataF);

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

							GH.Lms.SetObjectIp(TmpIp);
							GH.Lms.SetObjectIpInd(TmpIpInd - 1);
							GH.Lms.SetDispatchStatus(BUSYBREAK);
						}
						else
						{ // Not busy -> be sure to pop all parameters

							while (Value < Values)
							{
								switch (Cmd)
								{
									case READY_PCT:
										{
											GH.Lms.PrimParPointer((DATA8)DATA8_NAN);
										}
										break;

									case READY_RAW:
										{
											GH.Lms.PrimParPointer((DATA32)DATA32_NAN);
										}
										break;

									case READY_SI:
										{
											GH.Lms.PrimParPointer((DATAF)DATAF_NAN);
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

						Repeat = (DATA8)GH.Lms.PrimParPointer();
						Time = (DATA16)GH.Lms.PrimParPointer();
						WrLng = (DATA8)GH.Lms.PrimParPointer();
						pWrData = (DATA8[])GH.Lms.PrimParPointer();
						RdLng = (DATA8)GH.Lms.PrimParPointer();
						pRdData = (DATA8[])GH.Lms.PrimParPointer();

						if (GH.VMInstance.Handle >= 0)
						{
							pRdData = CommonHelper.CastObjectArray<DATA8>(GH.Lms.VmMemoryResize(GH.VMInstance.Handle, (DATA32)RdLng));
						}
						if (pRdData != null)
						{

							if (cInputSetupDevice(Device, Repeat, Time, WrLng, pWrData, RdLng, pRdData) == RESULT.BUSY)
							{ // Busy -> block VMThread

								GH.Lms.SetObjectIp(TmpIp);
								GH.Lms.SetObjectIpInd(TmpIpInd - 1);
								GH.Lms.SetDispatchStatus(BUSYBREAK);
							}
						}
					}
					break;

				case CLR_ALL:
					{
						Layer = (DATA8)GH.Lms.PrimParPointer();

						if (Layer == 0)
						{
							ClrLayer();
						}
						// TODO: here was some daisy shite
					}
					break;

				case STOP_ALL:
					{
						Layer = (DATA8)GH.Lms.PrimParPointer();

						if (Layer == 0)
						{
							StopLayer();
						}
						// TODO: here was some daisy shite
					}
					break;

			}
		}

		public void cInputRead()
		{
			DATA8 Type;
			DATA8 Mode;
			DATA8 Device;

			Device = cInputGetDevice();
			Type = (DATA8)GH.Lms.PrimParPointer();
			Mode = (DATA8)GH.Lms.PrimParPointer();

			if (Device < DEVICES)
			{
				cInputSetType(Device, Type, Mode, 0);
			}
			short _tmppp = 0;
			GH.Lms.PrimParPointer((DATA8)cInputReadDevicePct(Device, 0, 0, ref _tmppp));
		}

		public void cInputReadSi()
		{
			DATA8 Type;
			DATA8 Mode;
			DATA8 Device;

			Device = cInputGetDevice();
			Type = (DATA8)GH.Lms.PrimParPointer();
			Mode = (DATA8)GH.Lms.PrimParPointer();

			if (Device < DEVICES)
			{
				cInputSetType(Device, Type, Mode, 0);
			}
			short _tmppp = 0;
			GH.Lms.PrimParPointer((DATAF)cInputReadDeviceSi(Device, 0, 0, ref _tmppp));
		}

		public void cInputTest()
		{
			DATA8 Busy = 1;
			DATA8 Device;

			Device = cInputGetDevice();

			if (Device < DEVICES)
			{
				if (GH.InputInstance.DeviceData[Device].DevStatus != RESULT.BUSY)
				{
					Busy = 0;
				}
			}
			GH.Lms.PrimParPointer((DATA8)Busy);
		}

		public void cInputReady()
		{
			IP TmpIp;
			int TmpIpInd;
			DATA8 Device;

			TmpIp = GH.Lms.GetObjectIp();
			TmpIpInd = GH.Lms.GetObjectIpInd();
			Device = cInputGetDevice();

			if (Device < DEVICES)
			{
				if (GH.InputInstance.DeviceData[Device].DevStatus == RESULT.BUSY)
				{
					GH.Lms.SetObjectIp(TmpIp);
					GH.Lms.SetObjectIpInd(TmpIpInd - 1);
					GH.Lms.SetDispatchStatus(BUSYBREAK);
				}
			}
		}

		public void cInputWrite()
		{
			DATA8 Bytes;
			DATA8[] Data;
			DATA8 Device;
			DATA8 Tmp;
			IP TmpIp;
			int TmpIpInd;
			UBYTE[] Buffer = new UBYTE[UART_DATA_LENGTH + 1];
			DSPSTAT DspStat = DSPSTAT.FAILBREAK;

			TmpIp = GH.Lms.GetObjectIp();
			TmpIpInd = GH.Lms.GetObjectIpInd();

			Device = cInputGetDevice();
			Bytes = (DATA8)GH.Lms.PrimParPointer();
			Data = (DATA8[])GH.Lms.PrimParPointer();

			if (Device < INPUT_DEVICES)
			{
				if (GH.InputInstance.DeviceType[Device] != TYPE_TERMINAL)
				{
					if (GH.InputInstance.DeviceData[Device].Connection == CONN_INPUT_UART)
					{
						if ((Bytes > 0) && (Bytes <= UART_DATA_LENGTH))
						{
							if ((GH.InputInstance.Uart.Status[Device] & UART_WRITE_REQUEST) != 0)
							{
								DspStat = DSPSTAT.BUSYBREAK;
							}
							else
							{
								GH.InputInstance.DeviceData[Device].DevStatus = RESULT.BUSY;

								GH.InputInstance.Uart.Status[Device] &= ~UART_DATA_READY;

								Buffer[0] = (byte)Device;
								for (Tmp = 0; Tmp < Bytes; Tmp++)
								{
									Buffer[Tmp + 1] = (byte)Data[Tmp];
								}

								GH.Ev3System.InputHandler.WriteUartData(Buffer, Bytes + 1);

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
								if ((GH.InputInstance.Iic.Status[Device] & IIC_WRITE_REQUEST) != 0)
								{
									DspStat = DSPSTAT.BUSYBREAK;
								}
								else
								{
									GH.InputInstance.DeviceData[Device].DevStatus = RESULT.BUSY;

									GH.InputInstance.Iic.Status[Device] &= ~IIC_DATA_READY;

									Buffer[0] = (byte)Device;
									for (Tmp = 0; Tmp < Bytes; Tmp++)
									{
										Buffer[Tmp + 1] = (byte)Data[Tmp];
									}

									GH.Ev3System.InputHandler.WriteI2cData(Buffer, Bytes + 1);

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

				GH.Lms.SetObjectIp(TmpIp);
				GH.Lms.SetObjectIpInd(TmpIpInd - 1);
			}
			GH.Lms.SetDispatchStatus(DspStat);
		}

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
			Type = (DATA8)GH.Lms.PrimParPointer();
			Mode = (DATA8)GH.Lms.PrimParPointer();
			Format = (DATA8)GH.Lms.PrimParPointer();
			Values = (DATA8)GH.Lms.PrimParPointer();
			Value = 0;

			if (Device < DEVICES)
			{
				cInputSetType(Device, Type, Mode, 0);

				short _tmppp = 0;
				while ((Value < Values) && (Value < MAX_DEVICE_MODES))
				{
					switch (Format)
					{
						case DATA_PCT:
							{
								GH.Lms.PrimParPointer((DATA8)cInputReadDevicePct(Device, Value, 0, ref _tmppp));
							}
							break;

						case DATA_RAW:
							{
								Raw = cInputReadDeviceRaw(Device, Value, 0, ref _tmppp);
								if (float.IsNaN(Raw))
								{
									GH.Lms.PrimParPointer((DATA32)DATA32_NAN);
								}
								else
								{
									GH.Lms.PrimParPointer((DATA32)Raw);
								}
							}
							break;

						case DATA_SI:
							{
								GH.Lms.PrimParPointer((DATAF)cInputReadDeviceSi(Device, Value, 0, ref _tmppp));
							}
							break;

						default:
							{
								GH.Lms.PrimParPointer((DATA8)DATA8_NAN);
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
							GH.Lms.PrimParPointer((DATA8)DATA8_NAN);
						}
						break;

					case DATA_RAW:
						{
							GH.Lms.PrimParPointer((DATA32)DATA32_NAN);
						}
						break;

					case DATA_SI:
						{
							GH.Lms.PrimParPointer((DATAF)DATAF_NAN);
						}
						break;

					default:
						{
							GH.Lms.PrimParPointer((DATA8)DATA8_NAN);
						}
						break;

				}
				Value++;
			}
		}

		public void cInputSample()
		{
			DATA32 SampleTime;
			DATA32 Data32;
			DATA16 NoOfPorts;
			DATA16[] pInits;
			DATA8[] pDevices;
			DATA8[] pTypes;
			DATA8[] pModes;
			DATA8[] pDataSets;
			DATAF[] pValues;
			DATA16 Index;
			DATA8 Device;
			DATA8 Type;
			DATA8 Mode;


			SampleTime = (DATA32)GH.Lms.PrimParPointer();
			NoOfPorts = (DATA16)GH.Lms.PrimParPointer();
			pInits = (DATA16[])GH.Lms.PrimParPointer();
			pDevices = (DATA8[])GH.Lms.PrimParPointer();
			pTypes = (DATA8[])GH.Lms.PrimParPointer();
			pModes = (DATA8[])GH.Lms.PrimParPointer();
			pDataSets = (DATA8[])GH.Lms.PrimParPointer();
			pValues = (DATAF[])GH.Lms.PrimParPointer();
			if (GH.VMInstance.Handle >= 0)
			{
				Data32 = (DATA32)NoOfPorts;
				if (Data32 > MIN_ARRAY_ELEMENTS)
				{
					pValues = CommonHelper.CastObjectArray< DATAF > (GH.Lms.VmMemoryResize(GH.VMInstance.Handle, Data32));
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

						cInputSetType(Device, Type, Mode, 0);

						pValues[Index] = cInputReadDeviceSi(Device, pDataSets[Index], (short)SampleTime, ref pInits[Index]);
					}
				}
			}
		}

		public void cInputChar(sbyte Char)
		{
			// TODO: probably daisy shite
			GH.Ev3System.Logger.LogWarning($"Call of not implemented method in {System.Environment.StackTrace}");
		}

		public RESULT cInputGetDeviceData(sbyte Layer, sbyte Port, sbyte Length, ref sbyte pType, ref sbyte pMode, sbyte[] pData)
		{
			RESULT Result = RESULT.OK;

			// TODO: probably daisy shite
			GH.Ev3System.Logger.LogWarning($"Call of not implemented method in {System.Environment.StackTrace}");

			return (Result);
		}

		public RESULT cInputSetChainedDeviceType(sbyte Layer, sbyte Port, sbyte Type, sbyte Mode)
		{
			RESULT Result = RESULT.OK;

			// TODO: probably daisy shite
			GH.Ev3System.Logger.LogWarning($"Call of not implemented method in {System.Environment.StackTrace}");

			return (Result);
		}
	}
}
