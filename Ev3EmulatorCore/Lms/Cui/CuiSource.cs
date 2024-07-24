using EV3DecompilerLib.Decompile;
using Ev3EmulatorCore.Lms.Lms2012;
using static EV3DecompilerLib.Decompile.lms2012;
using static Ev3EmulatorCore.Lms.Lms2012.LmsInstance;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ev3EmulatorCore.Lms.Cui
{
	// https://github.com/mindboards/ev3sources/blob/master/lms2012/c_ui/source/c_ui.c
	public unsafe partial class CuiClass
	{
		// TODO: there was a battery shite

		public void cUiDownloadSuccesSound()
		{
			VARDATA[] Locals = new VARDATA[1];
			fixed (byte* pSuccess = &DownloadSuccesSound[0])
			fixed (byte* pLocals = &Locals[0])
			{
				Inst.ExecuteByteCode(pSuccess, null, pLocals);
			}
		}

		void cUiButtonClr()
		{
			DATA8 Button;

			for (Button = 0; Button < BUTTONS; Button++)
			{
				Inst.UiInstance.ButtonState[Button] &= ~BUTTON_CLR;
			}
		}

		void cUiButtonFlush()
		{
			DATA8 Button;

			for (Button = 0; Button < BUTTONS; Button++)
			{
				Inst.UiInstance.ButtonState[Button] &= ~BUTTON_FLUSH;
			}
		}

		void cUiSetLed(DATA8 State)
		{
			DATA8[] Buffer = new DATA8[2];

			Inst.UiInstance.LedState = State;

			if (Inst.UiInstance.UiFile >= MIN_HANDLE)
			{
				if (Inst.UiInstance.Warnlight != 0)
				{
					if ((State == (sbyte)LedPattern.LED_GREEN_FLASH) || (State == (sbyte)LedPattern.LED_RED_FLASH) || (State == (sbyte)LedPattern.LED_ORANGE_FLASH))
					{
						Buffer[0] = (sbyte)LedPattern.LED_ORANGE_FLASH + '0';
					}
					else
					{
						if ((State == (sbyte)LedPattern.LED_GREEN_PULSE) || (State == (sbyte)LedPattern.LED_RED_PULSE) || (State == (sbyte)LedPattern.LED_ORANGE_PULSE))
						{
							Buffer[0] = (sbyte)LedPattern.LED_ORANGE_PULSE + '0';
						}
						else
						{
							Buffer[0] = (sbyte)LedPattern.LED_ORANGE + '0';
						}
					}
				}
				else
				{
					Buffer[0] = (sbyte)(Inst.UiInstance.LedState + '0');
				}
				Buffer[1] = 0;
				write(Inst.UiInstance.UiFile, Buffer, 2);
			}
		}


		void cUiAlive()
		{
			Inst.UiInstance.SleepTimer = 0;
		}


		Result cUiInit()
		{
			Result Result = Result.OK;
			UI* pUiTmp;
			ANALOG* pAdcTmp;
			UBYTE Tmp;
			DATAF Hw;
			DATA8[] Buffer = new DATA8[32];
			DATA8[] OsBuf = new DATA8[2000];
			int Lng;
			int Start;
			int Sid;

			cUiAlive();

			Inst.UiInstance.ReadyForWarnings = 0;
			Inst.UiInstance.UpdateState = 0;
			Inst.UiInstance.RunLedEnabled = 0;
			Inst.UiInstance.RunScreenEnabled = 0;
			Inst.UiInstance.TopLineEnabled = 0;
			Inst.UiInstance.BackButtonBlocked = 0;
			Inst.UiInstance.Escape = 0;
			Inst.UiInstance.KeyBufIn = 0;
			Inst.UiInstance.Keys = 0;
			Inst.UiInstance.UiWrBufferSize = 0;

			Inst.UiInstance.ScreenBusy = 0;
			Inst.UiInstance.ScreenBlocked = 0;
			Inst.UiInstance.ScreenPrgId = ushort.MaxValue;
			Inst.UiInstance.ScreenObjId = ushort.MaxValue;

			Inst.UiInstance.PowerInitialized = 0;
			Inst.UiInstance.ShutDown = 0;

			Inst.UiInstance.PowerShutdown = 0;
			Inst.UiInstance.PowerState = 0;
			Inst.UiInstance.VoltShutdown = 0;
			Inst.UiInstance.Warnlight = 0;
			Inst.UiInstance.Warning = 0;
			Inst.UiInstance.WarningShowed = 0;
			Inst.UiInstance.WarningConfirmed = 0;
			Inst.UiInstance.VoltageState = 0;

			fixed (LCD* pLcdSafe = &Inst.UiInstance.LcdSafe)
            Inst.UiInstance.pLcd = pLcdSafe;
            fixed (UI* pUiSafe = &Inst.UiInstance.UiSafe)
            Inst.UiInstance.pUi = pUiSafe;
            fixed (UI* pUiSafe = &Inst.UiInstance.UiSafe)
            Inst.UiInstance.pAnalog = &Inst.UiInstance.Analog;

			Inst.UiInstance.Browser.PrgId = 0;
			Inst.UiInstance.Browser.ObjId = 0;

			Inst.UiInstance.Tbatt = 0.0f;
			Inst.UiInstance.Vbatt = 9.0f;
			Inst.UiInstance.Ibatt = 0.0f;
			Inst.UiInstance.Imotor = 0.0f;
			Inst.UiInstance.Iintegrated = 0.0f;

			Inst.UiInstance.Ibatt = 0.1f;
			Inst.UiInstance.Imotor = 0.0f;

			Result = dTerminalInit();

			Inst.UiInstance.PowerFile = open(POWER_DEVICE_NAME, O_RDWR);
			Inst.UiInstance.UiFile = open(UI_DEVICE_NAME, O_RDWR | O_SYNC);
			Inst.UiInstance.AdcFile = open(ANALOG_DEVICE_NAME, O_RDWR | O_SYNC);

			dLcdInit((*Inst.UiInstance.pLcd).Lcd);

			Hw = 0;
			if (Inst.UiInstance.UiFile >= MIN_HANDLE)
			{
				pUiTmp = (UI*)mmap(0, sizeof(UI), PROT_READ | PROT_WRITE, MAP_FILE | MAP_SHARED, Inst.UiInstance.UiFile, 0);

				if (pUiTmp == MAP_Result.FAILED)
				{
					LogErrorNumber(UI_SHARED_MEMORY);
					Result = Result.Result.FAIL;
				}
				else
				{
					Inst.UiInstance.pUi = pUiTmp;
				}

				read(Inst.UiInstance.UiFile, Inst.UiInstance.HwVers, HWVERS_SIZE);
				sscanf(&Inst.UiInstance.HwVers[1], "%f", &Hw);
			}
			else
			{
				LogErrorNumber(UI_DEVICE_FILE_NOT_FOUND);
				Result = Result.Result.FAIL;
			}
			Hw *= (DATAF)10;
			Inst.UiInstance.Hw = (DATA8)Hw;

			if (Inst.UiInstance.AdcFile >= MIN_HANDLE)
			{
				pAdcTmp = (ANALOG*)mmap(0, sizeof(ANALOG), PROT_READ | PROT_WRITE, MAP_FILE | MAP_SHARED, Inst.UiInstance.AdcFile, 0);

				if (pAdcTmp == MAP_Result.FAILED)
				{
					LogErrorNumber(ANALOG_SHARED_MEMORY);
					Result = Result.FAIL;
				}
				else
				{
					Inst.UiInstance.pAnalog = pAdcTmp;
				}
			}
			else
			{
				LogErrorNumber(ANALOG_DEVICE_FILE_NOT_FOUND);
				Result = Result.FAIL;
			}

			if (SPECIALVERS < '0')
			{
				snprintf(Inst.UiInstance.FwVers, FWVERS_SIZE, "V%4.2f", VERS);
			}
			else
			{
				snprintf(Inst.UiInstance.FwVers, FWVERS_SIZE, "V%4.2f%c", VERS, SPECIALVERS);
			}


			snprintf(Buffer, 32, "%s %s", __DATE__, __TIME__);
#warning uncomment
			// strptime((const char*)Buffer,(const char*)"%B %d %Y %H:%M:%S",(struct tm*)&tm);
			strftime(Inst.UiInstance.FwBuild, FWBUILD_SIZE, "%y%m%d%H%M", &tm);

#warning uncomment
			// pOs = (struct utsname*)OsBuf;
			uname(pOs);

			snprintf(Inst.UiInstance.OsVers, OSVERS_SIZE, "%s %s", (*pOs).sysname, (*pOs).release);

			sprintf((char*)Inst.UiInstance.OsBuild, "?");

			Lng = strlen((*pOs).version) - 9;
			if (Lng > 0)
			{
				(*pOs).version[Lng++] = ' ';
				(*pOs).version[Lng++] = ' ';
				(*pOs).version[Lng++] = ' ';
				(*pOs).version[Lng++] = ' ';

				Lng = strlen((*pOs).version);
				Tmp = 0;
				Start = 0;

				while ((Start < Lng) && (Tmp == 0))
				{
#warning uncomment
					// if (strptime((const char*)&(*pOs).version[Start],(const char*)"%B %d %H:%M:%S %Y",(struct tm*)&tm) != null)
					{
						Tmp = 1;
					}
					Start++;
				}
				if (Tmp != 0)
				{
					strftime((char*)Inst.UiInstance.OsBuild, OSBUILD_SIZE, "%y%m%d%H%M", &tm);
				}
			}

			Inst.UiInstance.IpAddr[0] = 0;
			Sid = socket(AF_INET, SOCK_DGRAM, 0);
			Sifr.ifr_addr.sa_family = AF_INET;
			strncpy(Sifr.ifr_name, "eth0", IFNAMSIZ - 1);
			if (ioctl(Sid, SIOCGIFADDR, &Sifr) >= 0)
			{
#warning uncomment
				// snprintf(Inst.UiInstance.IpAddr, IPADDR_SIZE, "%s", inet_ntoa(((struct sockaddr_in *)&Sifr.ifr_addr)->sin_addr));
			}
			close(Sid);

			cUiButtonClr();

			Inst.UiInstance.BattIndicatorHigh = BATT_INDICATOR_HIGH;
			Inst.UiInstance.BattIndicatorLow = BATT_INDICATOR_LOW;
			Inst.UiInstance.BattWarningHigh = BATT_WARNING_HIGH;
			Inst.UiInstance.BattWarningLow = BATT_WARNING_LOW;
			Inst.UiInstance.BattShutdownHigh = BATT_SHUTDOWN_HIGH;
			Inst.UiInstance.BattShutdownLow = BATT_SHUTDOWN_LOW;

			Inst.UiInstance.Accu = 0;
			if (Inst.UiInstance.PowerFile >= MIN_HANDLE)
			{
				read(Inst.UiInstance.PowerFile, Buffer, 2);
				if (Buffer[0] == '1')
				{
					Inst.UiInstance.Accu = 1;
					Inst.UiInstance.BattIndicatorHigh = ACCU_INDICATOR_HIGH;
					Inst.UiInstance.BattIndicatorLow = ACCU_INDICATOR_LOW;
					Inst.UiInstance.BattWarningHigh = ACCU_WARNING_HIGH;
					Inst.UiInstance.BattWarningLow = ACCU_WARNING_LOW;
					Inst.UiInstance.BattShutdownHigh = ACCU_SHUTDOWN_HIGH;
					Inst.UiInstance.BattShutdownLow = ACCU_SHUTDOWN_LOW;
				}
			}

			return (Result);
		}


		Result cUiOpen()
		{
			Result Result = Result.FAIL;

			// Save screen before run
			LCDCopy(&Inst.UiInstance.LcdSafe, &Inst.UiInstance.LcdPool[0], sizeof(LCD));

			cUiButtonClr();
			cUiSetLed((sbyte)LedPattern.LED_GREEN_PULSE);
			Inst.UiInstance.RunScreenEnabled = 3;
			Inst.UiInstance.RunLedEnabled = 1;
			Inst.UiInstance.TopLineEnabled = 0;

			Result = Result.OK;

			return (Result);
		}


		Result cUiClose()
		{
			Result Result = Result.FAIL;

			Inst.UiInstance.Warning &= (sbyte)~Warning.WARNING_BUSY;
			Inst.UiInstance.RunLedEnabled = 0;
			Inst.UiInstance.RunScreenEnabled = 0;
			Inst.UiInstance.TopLineEnabled = 1;
			Inst.UiInstance.BackButtonBlocked = 0;
			Inst.UiInstance.Browser.NeedUpdate = 1;
			cUiSetLed((sbyte)LedPattern.LED_GREEN);

			cUiButtonClr();

			Result = Result.OK;

			return (Result);
		}


		Result cUiExit()
		{
			Result Result = Result.FAIL;

			Result = dTerminalExit();

			if (Inst.UiInstance.AdcFile >= MIN_HANDLE)
			{
				munmap(Inst.UiInstance.pAnalog, sizeof(ANALOG));
				close(Inst.UiInstance.AdcFile);
			}

			if (Inst.UiInstance.UiFile >= MIN_HANDLE)
			{
				munmap(Inst.UiInstance.pUi, sizeof(UI));
				close(Inst.UiInstance.UiFile);
			}

			if (Inst.UiInstance.PowerFile >= MIN_HANDLE)
			{
				close(Inst.UiInstance.PowerFile);
			}

			Result = Result.OK;

			return (Result);
		}


		void cUiUpdateButtons(DATA16 Time)
		{
			DATA8 Button;

			for (Button = 0; Button < BUTTONS; Button++)
			{

				// Check real hardware buttons

				if ((*Inst.UiInstance.pUi).Pressed[Button] != 0)
				{ // Button pressed

					if (Inst.UiInstance.ButtonDebounceTimer[Button] == 0)
					{ // Button activated

						Inst.UiInstance.ButtonState[Button] |= (sbyte)BUTTON_ACTIVE;
					}

					Inst.UiInstance.ButtonDebounceTimer[Button] = BUTTON_DEBOUNCE_TIME;
				}
				else
				{ // Button not pressed

					if (Inst.UiInstance.ButtonDebounceTimer[Button] > 0)
					{ // Debounce delay

						Inst.UiInstance.ButtonDebounceTimer[Button] -= Time;

						if (Inst.UiInstance.ButtonDebounceTimer[Button] <= 0)
						{ // Button released

							Inst.UiInstance.ButtonState[Button] &= ~BUTTON_ACTIVE;
							Inst.UiInstance.ButtonDebounceTimer[Button] = 0;
						}
					}
				}

				// Check virtual buttons (hardware, direct command, PC)

				if ((Inst.UiInstance.ButtonState[Button] & BUTTON_ACTIVE) != 0)
				{
					if ((Inst.UiInstance.ButtonState[Button] & BUTTON_PRESSED) == 0)
					{ // Button activated

						Inst.UiInstance.Activated = (sbyte)BUTTON_SET;
						Inst.UiInstance.ButtonState[Button] |= (sbyte)BUTTON_PRESSED;
						Inst.UiInstance.ButtonState[Button] |= (sbyte)BUTTON_ACTIVATED;
						Inst.UiInstance.ButtonTimer[Button] = 0;
						Inst.UiInstance.ButtonRepeatTimer[Button] = BUTTON_START_REPEAT_TIME;
					}

					// Control auto repeat

					if (Inst.UiInstance.ButtonRepeatTimer[Button] > Time)
					{
						Inst.UiInstance.ButtonRepeatTimer[Button] -= Time;
					}
					else
					{
						if ((Button != 1) && (Button != 5))
						{ // No repeat on ENTER and BACK

							Inst.UiInstance.Activated |= (sbyte)BUTTON_SET;
							Inst.UiInstance.ButtonState[Button] |= (sbyte)BUTTON_ACTIVATED;
							Inst.UiInstance.ButtonRepeatTimer[Button] = BUTTON_REPEAT_TIME;
						}
					}

					// Control long press

					Inst.UiInstance.ButtonTimer[Button] += Time;

					if (Inst.UiInstance.ButtonTimer[Button] >= LONG_PRESS_TIME)
					{
						if ((Inst.UiInstance.ButtonState[Button] & BUTTON_LONG_LATCH) == 0)
						{ // Only once

							Inst.UiInstance.ButtonState[Button] |= (sbyte)BUTTON_LONG_LATCH;
						}
						Inst.UiInstance.ButtonState[Button] |= (sbyte)BUTTON_LONGPRESS;
					}

				}
				else
				{
					if ((Inst.UiInstance.ButtonState[Button] & BUTTON_PRESSED) != 0)
					{ // Button released

						Inst.UiInstance.ButtonState[Button] &= ~BUTTON_PRESSED;
						Inst.UiInstance.ButtonState[Button] &= ~BUTTON_LONG_LATCH;
						Inst.UiInstance.ButtonState[Button] |= (sbyte)BUTTON_BUMBED;
					}
				}
			}
		}

		Result cUiUpdateInput()
		{
			UBYTE Key;

			if (GetTerminalEnable() != 0)
			{
				if (dTerminalRead(&Key) == Result.OK)
				{
					switch (Key)
					{
						case (byte)' ':
							{
								Inst.UiInstance.Escape = (sbyte)Key;
							}
							break;

						case (byte)'<':
							{
								Inst.UiInstance.Escape = (sbyte)Key;
							}
							break;

						case (byte)'\r':
						case (byte)'\n':
							{
								if (Inst.UiInstance.KeyBufIn != 0)
								{
									Inst.UiInstance.Keys = Inst.UiInstance.KeyBufIn;
									Inst.UiInstance.KeyBufIn = 0;
								}
							}
							break;

						default:
							{
								Inst.UiInstance.KeyBuffer[Inst.UiInstance.KeyBufIn] = (sbyte)Key;
								if (++Inst.UiInstance.KeyBufIn >= KEYBUF_SIZE)
								{
									Inst.UiInstance.KeyBufIn--;
								}
								Inst.UiInstance.KeyBuffer[Inst.UiInstance.KeyBufIn] = 0;
							}
							break;

					}
				}
			}
			dLcdRead();

			return (Result.OK);
		}

		DATA8 cUiEscape()
		{
			DATA8 Result;

			Result = Inst.UiInstance.Escape;
			Inst.UiInstance.Escape = 0;

			return (Result);
		}


		void cUiTestpin(DATA8 State)
		{
			DATA8 Data8;

			Data8 = State;
			if (Inst.UiInstance.PowerFile >= MIN_HANDLE)
			{
				write(Inst.UiInstance.PowerFile, &Data8, 1);
			}
		}


		UBYTE AtoN(DATA8 Char)
		{
			UBYTE Tmp = 0;

			if ((Char >= '0') && (Char <= '9'))
			{
				Tmp = (UBYTE)(Char - '0');
			}
			else
			{
				Char |= 0x20;

				if ((Char >= 'a') && (Char <= 'f'))
				{
					Tmp = (UBYTE)((Char - 'a') + 10);
				}
			}

			return (Tmp);
		}


		void cUiFlushBuffer()
		{
			if (Inst.UiInstance.UiWrBufferSize != 0)
			{
				if (GetTerminalEnable() != 0)
				{
					dTerminalWrite((UBYTE*)Inst.UiInstance.UiWrBuffer, Inst.UiInstance.UiWrBufferSize);
				}
				Inst.UiInstance.UiWrBufferSize = 0;
			}
		}


		void cUiWriteString(DATA8* pString)
		{
			while (*pString != 0)
			{
				Inst.UiInstance.UiWrBuffer[Inst.UiInstance.UiWrBufferSize] = *pString;
				if (++Inst.UiInstance.UiWrBufferSize >= UI_WR_BUFFER_SIZE)
				{
					cUiFlushBuffer();
				}
				pString++;
			}
		}

		DATA8 cUiButtonRemap(DATA8 Mapped)
		{
			DATA8 Real;

			if ((Mapped >= 0) && (Mapped < BUTTONTYPES))
			{
				Real = MappedToReal[Mapped];
			}
			else
			{
				Real = REAL_ANY_BUTTON;
			}

			return (Real);
		}


		void cUiSetPress(DATA8 Button, DATA8 Press)
		{
			Button = cUiButtonRemap(Button);

			if (Button < BUTTONS)
			{
				if (Press != 0)
				{
					Inst.UiInstance.ButtonState[Button] |= (sbyte)BUTTON_ACTIVE;
				}
				else
				{
					Inst.UiInstance.ButtonState[Button] &= ~BUTTON_ACTIVE;
				}
			}
			else
			{
				if (Button == REAL_ANY_BUTTON)
				{
					if (Press != 0)
					{
						for (Button = 0; Button < BUTTONS; Button++)
						{
							Inst.UiInstance.ButtonState[Button] |= (sbyte)BUTTON_ACTIVE;
						}
					}
					else
					{
						for (Button = 0; Button < BUTTONS; Button++)
						{
							Inst.UiInstance.ButtonState[Button] &= ~BUTTON_ACTIVE;
						}
					}
				}
			}
		}


		DATA8 cUiGetPress(DATA8 Button)
		{
			DATA8 Result = 0;

			Button = cUiButtonRemap(Button);

			if (Button < BUTTONS)
			{
				if ((Inst.UiInstance.ButtonState[Button] & BUTTON_PRESSED) != 0)
				{
					Result = 1;
				}
			}
			else
			{
				if (Button == REAL_ANY_BUTTON)
				{
					for (Button = 0; Button < BUTTONS; Button++)
					{
						if ((Inst.UiInstance.ButtonState[Button] & BUTTON_PRESSED) != 0)
						{
							Result = 1;
						}
					}
				}
			}

			return (Result);
		}


		DATA8 cUiTestShortPress(DATA8 Button)
		{
			DATA8 Result = 0;

			Button = cUiButtonRemap(Button);

			if (Button < BUTTONS)
			{
				if ((Inst.UiInstance.ButtonState[Button] & BUTTON_ACTIVATED) != 0)
				{
					Result = 1;
				}
			}
			else
			{
				if (Button == REAL_ANY_BUTTON)
				{
					for (Button = 0; Button < BUTTONS; Button++)
					{
						if ((Inst.UiInstance.ButtonState[Button] & BUTTON_ACTIVATED) != 0)
						{
							Result = 1;
						}
					}
				}
				else
				{
					if (Button == REAL_NO_BUTTON)
					{
						Result = 1;
						for (Button = 0; Button < BUTTONS; Button++)
						{
							if ((Inst.UiInstance.ButtonState[Button] & BUTTON_ACTIVATED) != 0)
							{
								Result = 0;
							}
						}
					}
				}
			}

			return (Result);
		}


		DATA8 cUiGetShortPress(DATA8 Button)
		{
			DATA8 Result = 0;

			Button = cUiButtonRemap(Button);

			if (Button < BUTTONS)
			{
				if ((Inst.UiInstance.ButtonState[Button] & BUTTON_ACTIVATED) != 0)
				{
					Inst.UiInstance.ButtonState[Button] &= ~BUTTON_ACTIVATED;
					Result = 1;
				}
			}
			else
			{
				if (Button == REAL_ANY_BUTTON)
				{
					for (Button = 0; Button < BUTTONS; Button++)
					{
						if ((Inst.UiInstance.ButtonState[Button] & BUTTON_ACTIVATED) != 0)
						{
							Inst.UiInstance.ButtonState[Button] &= ~BUTTON_ACTIVATED;
							Result = 1;
						}
					}
				}
				else
				{
					if (Button == REAL_NO_BUTTON)
					{
						Result = 1;
						for (Button = 0; Button < BUTTONS; Button++)
						{
							if ((Inst.UiInstance.ButtonState[Button] & BUTTON_ACTIVATED) != 0)
							{
								Inst.UiInstance.ButtonState[Button] &= ~BUTTON_ACTIVATED;
								Result = 0;
							}
						}
					}
				}
			}
			if (Result != 0)
			{
				Inst.UiInstance.Click = 1;
			}

			return (Result);
		}


		DATA8 cUiGetBumbed(DATA8 Button)
		{
			DATA8 Result = 0;

			Button = cUiButtonRemap(Button);

			if (Button < BUTTONS)
			{
				if ((Inst.UiInstance.ButtonState[Button] & BUTTON_BUMBED) != 0)
				{
					Inst.UiInstance.ButtonState[Button] &= ~BUTTON_BUMBED;
					Result = 1;
				}
			}
			else
			{
				if (Button == REAL_ANY_BUTTON)
				{
					for (Button = 0; Button < BUTTONS; Button++)
					{
						if ((Inst.UiInstance.ButtonState[Button] & BUTTON_BUMBED) != 0)
						{
							Inst.UiInstance.ButtonState[Button] &= ~BUTTON_BUMBED;
							Result = 1;
						}
					}
				}
				else
				{
					if (Button == REAL_NO_BUTTON)
					{
						Result = 1;
						for (Button = 0; Button < BUTTONS; Button++)
						{
							if ((Inst.UiInstance.ButtonState[Button] & BUTTON_BUMBED) != 0)
							{
								Inst.UiInstance.ButtonState[Button] &= ~BUTTON_BUMBED;
								Result = 0;
							}
						}
					}
				}
			}

			return (Result);
		}


		DATA8 cUiTestLongPress(DATA8 Button)
		{
			DATA8 Result = 0;

			Button = cUiButtonRemap(Button);

			if (Button < BUTTONS)
			{
				if ((Inst.UiInstance.ButtonState[Button] & BUTTON_LONGPRESS) != 0)
				{
					Result = 1;
				}
			}
			else
			{
				if (Button == REAL_ANY_BUTTON)
				{
					for (Button = 0; Button < BUTTONS; Button++)
					{
						if ((Inst.UiInstance.ButtonState[Button] & BUTTON_LONGPRESS) != 0)
						{
							Result = 1;
						}
					}
				}
				else
				{
					if (Button == REAL_NO_BUTTON)
					{
						Result = 1;
						for (Button = 0; Button < BUTTONS; Button++)
						{
							if ((Inst.UiInstance.ButtonState[Button] & BUTTON_LONGPRESS) != 0)
							{
								Result = 0;
							}
						}
					}
				}
			}

			return (Result);
		}


		DATA8 cUiGetLongPress(DATA8 Button)
		{
			DATA8 Result = 0;

			Button = cUiButtonRemap(Button);

			if (Button < BUTTONS)
			{
				if ((Inst.UiInstance.ButtonState[Button] & BUTTON_LONGPRESS) != 0)
				{
					Inst.UiInstance.ButtonState[Button] &= ~BUTTON_LONGPRESS;
					Result = 1;
				}
			}
			else
			{
				if (Button == REAL_ANY_BUTTON)
				{
					for (Button = 0; Button < BUTTONS; Button++)
					{
						if ((Inst.UiInstance.ButtonState[Button] & BUTTON_LONGPRESS) != 0)
						{
							Inst.UiInstance.ButtonState[Button] &= ~BUTTON_LONGPRESS;
							Result = 1;
						}
					}
				}
				else
				{
					if (Button == REAL_NO_BUTTON)
					{
						Result = 1;
						for (Button = 0; Button < BUTTONS; Button++)
						{
							if ((Inst.UiInstance.ButtonState[Button] & BUTTON_LONGPRESS) != 0)
							{
								Inst.UiInstance.ButtonState[Button] &= ~BUTTON_LONGPRESS;
								Result = 0;
							}
						}
					}
				}
			}
			if (Result != 0)
			{
				Inst.UiInstance.Click = 1;
			}

			return (Result);
		}


		DATA16 cUiTestHorz()
		{
			DATA16 Result = 0;

			if (cUiTestShortPress((sbyte)ButtonType.LEFT_BUTTON) != 0)
			{
				Result = -1;
			}
			if (cUiTestShortPress((sbyte)ButtonType.RIGHT_BUTTON) != 0)
			{
				Result = 1;
			}

			return (Result);
		}


		DATA16 cUiGetHorz()
		{
			DATA16 Result = 0;

			if (cUiGetShortPress((sbyte)ButtonType.LEFT_BUTTON) != 0)
			{
				Result = -1;
			}
			if (cUiGetShortPress((sbyte)ButtonType.RIGHT_BUTTON) != 0)
			{
				Result = 1;
			}

			return (Result);
		}


		DATA16 cUiGetVert()
		{
			DATA16 Result = 0;

			if (cUiGetShortPress((sbyte)ButtonType.UP_BUTTON) != 0)
			{
				Result = -1;
			}
			if (cUiGetShortPress((sbyte)ButtonType.DOWN_BUTTON) != 0)
			{
				Result = 1;
			}

			return (Result);
		}


		DATA8 cUiWaitForPress()
		{
			DATA8 Result = 0;

			Result = cUiTestShortPress((sbyte)ButtonType.ANY_BUTTON);

			return (Result);
		}


		DATA16 cUiAlignX(DATA16 X)
		{
			return ((short)((X + 7) & ~7));
		}

		void cUiUpdateCnt()
		{
			if (Inst.UiInstance.PowerInitialized != 0)
			{
				Inst.UiInstance.CinCnt *= (DATAF)(AVR_CIN - 1);
				Inst.UiInstance.CoutCnt *= (DATAF)(AVR_COUT - 1);
				Inst.UiInstance.VinCnt *= (DATAF)(AVR_VIN - 1);

				Inst.UiInstance.CinCnt += (DATAF)(*Inst.UiInstance.pAnalog).BatteryCurrent;
				Inst.UiInstance.CoutCnt += (DATAF)(*Inst.UiInstance.pAnalog).MotorCurrent;
				Inst.UiInstance.VinCnt += (DATAF)(*Inst.UiInstance.pAnalog).Cell123456;

				Inst.UiInstance.CinCnt /= (DATAF)(AVR_CIN);
				Inst.UiInstance.CoutCnt /= (DATAF)(AVR_COUT);
				Inst.UiInstance.VinCnt /= (DATAF)(AVR_VIN);
			}
			else
			{
				Inst.UiInstance.CinCnt = (DATAF)(*Inst.UiInstance.pAnalog).BatteryCurrent;
				Inst.UiInstance.CoutCnt = (DATAF)(*Inst.UiInstance.pAnalog).MotorCurrent;
				Inst.UiInstance.VinCnt = (DATAF)(*Inst.UiInstance.pAnalog).Cell123456;
				Inst.UiInstance.PowerInitialized = 1;
			}
		}


		void cUiUpdatePower()
		{
			DATAF CinV;
			DATAF CoutV;

			if ((Inst.UiInstance.Hw == FINAL) || (Inst.UiInstance.Hw == FINALB))
			{
				CinV = CNT_V(Inst.UiInstance.CinCnt) / AMP_CIN;
				Inst.UiInstance.Vbatt = (CNT_V(Inst.UiInstance.VinCnt) / AMP_VIN) + CinV + VCE;

				Inst.UiInstance.Ibatt = CinV / SHUNT_IN;
				CoutV = CNT_V(Inst.UiInstance.CoutCnt) / AMP_COUT;
				Inst.UiInstance.Imotor = CoutV / SHUNT_OUT;

			}
			else
			{
				CinV = CNT_V(Inst.UiInstance.CinCnt) / EP2_AMP_CIN;
				Inst.UiInstance.Vbatt = (CNT_V(Inst.UiInstance.VinCnt) / AMP_VIN) + CinV + VCE;

				Inst.UiInstance.Ibatt = CinV / EP2_SHUNT_IN;
				Inst.UiInstance.Imotor = 0;
			}
		}

		void cUiUpdateTopline()
		{
			DATA16 X1, X2;
			DATA16 V;
            DATA8 BtStatus;
            DATA8 WifiStatus;
			DATA8 TmpStatus;
			DATA8[] Name = new DATA8[NAME_LENGTH + 1];

			DATA32 Total;
			DATA32 Free;

			if (Inst.UiInstance.TopLineEnabled != 0)
			{
				// Clear top line
				LCDClearTopline(Inst.UiInstance.pLcd);

				// Show BT status
				TmpStatus = 0;
				BtStatus = cComGetBtStatus();
				if (BtStatus > 0)
				{
					TmpStatus = 1;
					BtStatus >>= 1;
					if ((BtStatus >= 0) && (BtStatus < TOP_BT_ICONS))
					{
						dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, 0, 1, IconType.SMALL_ICON, TopLineBtIconMap[BtStatus]);
					}
				}
				if (Inst.UiInstance.BtOn != TmpStatus)
				{
					Inst.UiInstance.BtOn = TmpStatus;
					Inst.UiInstance.UiUpdate = 1;
				}

				// Show WIFI status
				TmpStatus = 0;
				WifiStatus = cComGetWifiStatus();
				if (WifiStatus > 0)
				{
					TmpStatus = 1;
					WifiStatus >>= 1;
					if ((WifiStatus >= 0) && (WifiStatus < TOP_WIFI_ICONS))
					{
						dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, 16, 1, IconType.SMALL_ICON, TopLineWifiIconMap[WifiStatus]);
					}
				}
				if (Inst.UiInstance.WiFiOn != TmpStatus)
				{
					Inst.UiInstance.WiFiOn = TmpStatus;
					Inst.UiInstance.UiUpdate = 1;
				}

				// Show brick name
				cComGetBrickName(NAME_LENGTH + 1, Name);

				X1 = dLcdGetFontWidth(SMALL_FONT);
				X2 = (short)(LCD_WIDTH / X1);
				X2 -= strlen((char*)Name);
				X2 /= 2;
				X2 *= X1;
				dLcdDrawText((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, X2, 1, FontType.SMALL_FONT, Name);

				// Calculate number of icons
				X1 = dLcdGetIconWidth(SMALL_ICON);
				X2 = (short)((LCD_WIDTH - X1) / X1);

				// Show USB status
				if (cComGetUsbStatus() != 0)
				{
					dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, (X2 - 1) * X1, 1, IconType.SMALL_ICON, SICON_USB);
				}

				// Show battery
				V = (DATA16)(Inst.UiInstance.Vbatt * 1000.0);
				V -= Inst.UiInstance.BattIndicatorLow;
				V = (short)((V * (TOP_BATT_ICONS - 1)) / (Inst.UiInstance.BattIndicatorHigh - Inst.UiInstance.BattIndicatorLow));
				if (V > (TOP_BATT_ICONS - 1))
				{
					V = (TOP_BATT_ICONS - 1);
				}
				if (V < 0)
				{
					V = 0;
				}
				dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, X2 * X1, 1, IconType.SMALL_ICON, TopLineBattIconMap[V]);

				// Show bottom line
				dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, 0, TOPLINE_HEIGHT, LCD_WIDTH, TOPLINE_HEIGHT);
			}
		}


		void cUiUpdateLcd()
		{
			Inst.UiInstance.Font = (sbyte)FontType.NORMAL_FONT;
			cUiUpdateTopline();
			dLcdUpdate(Inst.UiInstance.pLcd);
		}

		void cUiRunScreen()
		{
			// 100mS
			if (Inst.UiInstance.ScreenBlocked == 0)
			{
				switch (Inst.UiInstance.RunScreenEnabled)
				{
					case 0:
						{
						}
						break;
					case 1:
						{ // 100mS

							Inst.UiInstance.RunScreenEnabled++;
						}
						break;
					case 2:
						{ // 200mS

							Inst.UiInstance.RunScreenEnabled++;
						}
						break;
					case 3:
						{
							// Init animation number
							Inst.UiInstance.RunScreenNumber = 1;

							// Clear screen
							LCDClear((*Inst.UiInstance.pLcd).Lcd);
							cUiUpdateLcd();


							// Enable top line

							// Draw fixed image
							dLcdDrawPicture((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, 8, 39, mindstorms_width, mindstorms_height, (UBYTE*)mindstorms_bits);
							cUiUpdateLcd();

							// Draw user program name
							dLcdDrawText((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, 8, 79, 1, (DATA8*)VMInstance.Program[Slot.USER_SLOT].Name);
							cUiUpdateLcd();

							Inst.UiInstance.RunScreenTimer = 0;
							Inst.UiInstance.RunScreenCounter = 0;

							Inst.UiInstance.RunScreenEnabled++;

							if (Inst.UiInstance.RunLedEnabled != 0)
							{
								cUiSetLed((sbyte)LedPattern.LED_GREEN_PULSE);
							}

							dLcdDrawPicture((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, 8, 67, Ani1x_width, Ani1x_height, (UBYTE*)Ani1x_bits);
							cUiUpdateLcd();

							Inst.UiInstance.RunScreenTimer = Inst.UiInstance.MilliSeconds;
							Inst.UiInstance.RunScreenEnabled++;
						}
						break;
					case 5:
						{ // 100mS

							Inst.UiInstance.RunScreenEnabled++;
						}
						break;
					case 6:
						{ // 200mS

							Inst.UiInstance.RunScreenEnabled++;
						}
						break;
					case 7:
						{ // 300mS

							Inst.UiInstance.RunScreenEnabled++;
						}
						break;
					case 8:
						{ // 400mS

							Inst.UiInstance.RunScreenEnabled++;
						}
						break;
					case 9:
						{ // 500mS -> Ani2

							dLcdDrawPicture((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, 8, 67, Ani2x_width, Ani2x_height, (UBYTE*)Ani2x_bits);
							cUiUpdateLcd();

							Inst.UiInstance.RunScreenEnabled++;
						}
						break;
					case 10:
						{ // 600mS -> Ani3

							dLcdDrawPicture((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, 8, 67, Ani3x_width, Ani3x_height, (UBYTE*)Ani3x_bits);
							cUiUpdateLcd();

							Inst.UiInstance.RunScreenEnabled++;
						}
						break;
					case 11:
						{ // 700ms -> Ani4

							dLcdDrawPicture((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, 8, 67, Ani4x_width, Ani4x_height, (UBYTE*)Ani4x_bits);
							cUiUpdateLcd();

							Inst.UiInstance.RunScreenEnabled++;
						}
						break;
					case 12:
						{ // 800ms -> Ani5

							dLcdDrawPicture((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, 8, 67, Ani5x_width, Ani5x_height, (UBYTE*)Ani5x_bits);
							cUiUpdateLcd();

							Inst.UiInstance.RunScreenEnabled++;
						}
						break;
					default:
						{ // 900ms -> Ani6

							dLcdDrawPicture((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, 8, 67, Ani6x_width, Ani6x_height, (UBYTE*)Ani6x_bits);
							cUiUpdateLcd();

							Inst.UiInstance.RunScreenEnabled = 4;
						}
						break;

				}
			}
		}

		void cUiCheckVoltage()
		{ // 400mS

			cUiUpdatePower();

			if (Inst.UiInstance.Vbatt >= Inst.UiInstance.BattWarningHigh)
			{
				unchecked { Inst.UiInstance.Warning &= (sbyte)~Warning.WARNING_BATTLOW; }
			}

			if (Inst.UiInstance.Vbatt <= Inst.UiInstance.BattWarningLow)
			{
				Inst.UiInstance.Warning |= (sbyte)Warning.WARNING_BATTLOW;
			}

			if (Inst.UiInstance.Vbatt >= Inst.UiInstance.BattShutdownHigh)
			{ // Good

				unchecked
				{
					Inst.UiInstance.Warning &= (sbyte)~Warning.WARNING_VOLTAGE;
				}
			}

			if (Inst.UiInstance.Vbatt < Inst.UiInstance.BattShutdownLow)
			{ // Bad

				Inst.UiInstance.Warning |= (sbyte)Warning.WARNING_VOLTAGE;

				ProgramEnd(Slot.USER_SLOT);

				if ((Inst.UiInstance.MilliSeconds - Inst.UiInstance.VoltageTimer) >= LOW_VOLTAGE_SHUTDOWN_TIME)
				{ // Realy bad

					Inst.UiInstance.ShutDown = 1;
				}
			}
			else
			{
				Inst.UiInstance.VoltageTimer = Inst.UiInstance.MilliSeconds;
			}
		}

		void cUiCheckPower(UWORD Time)
		{ // 400mS

			DATA16 X, Y;
			DATAF I;
			DATAF Slope;

			I = Inst.UiInstance.Ibatt + Inst.UiInstance.Imotor;

			if (I > LOAD_BREAK_EVEN)
			{
				Slope = LOAD_SLOPE_UP;
			}
			else
			{
				Slope = LOAD_SLOPE_DOWN;
			}

			Inst.UiInstance.Iintegrated += (I - LOAD_BREAK_EVEN) * (Slope * (DATAF)Time / 1000.0);

			if (Inst.UiInstance.Iintegrated < 0.0)
			{
				Inst.UiInstance.Iintegrated = 0.0f;
			}
			if (Inst.UiInstance.Iintegrated > LOAD_SHUTDOWN_Result.FAIL)
			{
				Inst.UiInstance.Iintegrated = LOAD_SHUTDOWN_Result.FAIL;
			}

			if ((Inst.UiInstance.Iintegrated >= LOAD_SHUTDOWN_HIGH) || (I >= LOAD_SHUTDOWN_Result.FAIL))
			{
				Inst.UiInstance.Warning |= (sbyte)Warning.WARNING_CURRENT;
				Inst.UiInstance.PowerShutdown = 1;
			}
			if (Inst.UiInstance.Iintegrated <= LOAD_BREAK_EVEN)
			{
				Inst.UiInstance.Warning &= ~(sbyte)Warning.WARNING_CURRENT;
				Inst.UiInstance.PowerShutdown = 0;
			}

			if (Inst.UiInstance.PowerShutdown != 0)
			{
				if (Inst.UiInstance.ScreenBlocked == 0)
				{
					if (ProgramStatus(Slot.USER_SLOT) != STOPPED)
					{
						Inst.UiInstance.PowerState = 1;
					}
				}
				ProgramEnd(Slot.USER_SLOT);
			}

			switch (Inst.UiInstance.PowerState)
			{
				case 0:
					{
						if (Inst.UiInstance.PowerShutdown != 0)
						{
							Inst.UiInstance.PowerState++;
						}
					}
					break;

				case 1:
					{
						if (Inst.UiInstance.ScreenBusy == 0)
						{
							if ((Inst.UiInstance.VoltShutdown) == 0)
							{
								Inst.UiInstance.ScreenBlocked = 1;
								Inst.UiInstance.PowerState++;
							}
						}
					}
					break;

				case 2:
					{
						LCDCopy(&Inst.UiInstance.LcdSafe, &Inst.UiInstance.LcdSave, sizeof(Inst.UiInstance.LcdSave));
						Inst.UiInstance.PowerState++;
					}
					break;

				case 3:
					{
						X = 16;
						Y = 52;

						dLcdDrawPicture((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, X, Y, POP3_width, POP3_height, (UBYTE*)POP3_bits);

						dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, X + 48, Y + 10, IconType.LARGE_ICON, WARNSIGN);
						dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, X + 72, Y + 10, IconType.LARGE_ICON, WARN_POWER);
						dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, X + 5, Y + 39, X + 138, Y + 39);
						dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, X + 56, Y + 40, IconType.LARGE_ICON, YES_SEL);
						dLcdUpdate(Inst.UiInstance.pLcd);
						cUiButtonFlush();
						Inst.UiInstance.PowerState++;
					}
					break;

				case 4:
					{
						if (cUiGetShortPress((sbyte)ButtonType.ENTER_BUTTON) != 0)
						{
							if (Inst.UiInstance.ScreenBlocked != 0)
							{
								Inst.UiInstance.ScreenBlocked = 0;
							}
							LCDCopy(&Inst.UiInstance.LcdSave, &Inst.UiInstance.LcdSafe, sizeof(Inst.UiInstance.LcdSafe));
							dLcdUpdate(Inst.UiInstance.pLcd);
							Inst.UiInstance.PowerState++;
						}
						if ((Inst.UiInstance.PowerShutdown) == 0)
						{
							Inst.UiInstance.PowerState = 0;
						}
					}
					break;

				case 5:
					{
						if ((Inst.UiInstance.PowerShutdown) == 0)
						{
							Inst.UiInstance.PowerState = 0;
						}
					}
					break;

			}
		}

		void cUiCheckMemory()
		{ // 400mS

			DATA32 Total;
			DATA32 Free;

			cMemoryGetUsage(&Total, &Free, 0);

			if (Free > LOW_MEMORY)
			{ // Good

				Inst.UiInstance.Warning &= ~(sbyte)Warning.WARNING_MEMORY;
			}
			else
			{ // Bad

				Inst.UiInstance.Warning |= (sbyte)Warning.WARNING_MEMORY;
			}
		}

		void cUiCheckAlive(UWORD Time)
		{
			ULONG Timeout;

			Inst.UiInstance.SleepTimer += Time;

			if ((Inst.UiInstance.Activated & BUTTON_ALIVE) != 0)
			{
				Inst.UiInstance.Activated &= ~BUTTON_ALIVE;
				cUiAlive();
			}
			Timeout = (ULONG)GetSleepMinutes();

			if (Timeout != 0)
			{
				if (Inst.UiInstance.SleepTimer >= (Timeout * 60000L))
				{
					Inst.UiInstance.ShutDown = 1;
				}
			}
		}

		void cUiUpdate(UWORD Time)
		{
			DATA8 Warning;
			DATA8 Tmp;

			Inst.UiInstance.MilliSeconds += (ULONG)Time;

			cUiUpdateButtons((short)Time);
			cUiUpdateInput();
			cUiUpdateCnt();

			if ((Inst.UiInstance.MilliSeconds - Inst.UiInstance.UpdateStateTimer) >= 50)
			{
				Inst.UiInstance.UpdateStateTimer = Inst.UiInstance.MilliSeconds;

				if (Inst.UiInstance.Event == 0)
				{
					Inst.UiInstance.Event = cComGetEvent();
				}

				switch (Inst.UiInstance.UpdateState++)
				{
					case 0:
						{ //  50 mS

						}
						break;

					case 1:
						{ // 100 mS

							cUiRunScreen();
						}
						break;

					case 2:
						{ // 150 mS

						}
						break;

					case 3:
						{ // 200 mS

							cUiRunScreen();
						}
						break;

					case 4:
						{ // 250 mS

						}
						break;

					case 5:
						{ // 300 mS

							cUiRunScreen();
						}
						break;

					case 6:
						{ // 350 mS

							if (Inst.UiInstance.ScreenBusy == 0)
							{
								cUiUpdateTopline();
								dLcdUpdate(Inst.UiInstance.pLcd);
							}
						}
						break;

					default:
						{ // 400 mS

							cUiRunScreen();
							Inst.UiInstance.UpdateState = 0;
							Inst.UiInstance.ReadyForWarnings = 1;
						}
						break;

				}

				if (Inst.UiInstance.Warning != 0)
				{ // Some warning present

					if (Inst.UiInstance.Warnlight == 0)
					{ // If not on - turn orange light on

						Inst.UiInstance.Warnlight = 1;
						cUiSetLed(Inst.UiInstance.LedState);
					}
				}
				else
				{ // No warning

					if (Inst.UiInstance.Warnlight != 0)
					{ // If orange light on - turn it off

						Inst.UiInstance.Warnlight = 0;
						cUiSetLed(Inst.UiInstance.LedState);
					}
				}

				// Get valid popup warnings
				Warning = (sbyte)(Inst.UiInstance.Warning & (sbyte)lms2012.Warning.WARNINGS);

				// Find warnings that has not been showed
				Tmp = (sbyte)(Warning & ~Inst.UiInstance.WarningShowed);

				if (Tmp != 0)
				{ // Show popup

					if (Inst.UiInstance.ScreenBusy == 0)
					{ // Wait on screen

						if (Inst.UiInstance.ScreenBlocked == 0)
						{
							LCDCopy(&Inst.UiInstance.LcdSafe, &Inst.UiInstance.LcdSave, sizeof(Inst.UiInstance.LcdSave));
							dLcdDrawPicture((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, vmPOP3_ABS_X, vmPOP3_ABS_Y, POP3_width, POP3_height, (UBYTE*)POP3_bits);

							if ((Tmp & (sbyte)lms2012.Warning.WARNING_TEMP) != 0)
							{
								dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, vmPOP3_ABS_WARN_ICON_X1, vmPOP3_ABS_WARN_ICON_Y, LARGE_ICON, WARNSIGN);
								dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, vmPOP3_ABS_WARN_ICON_X2, vmPOP3_ABS_WARN_SPEC_ICON_Y, LARGE_ICON, WARN_POWER);
								dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, vmPOP3_ABS_WARN_ICON_X3, vmPOP3_ABS_WARN_SPEC_ICON_Y, LARGE_ICON, TO_MANUAL);
								Inst.UiInstance.WarningShowed |= (sbyte)lms2012.Warning.WARNING_TEMP;
							}
							else
							{
								if ((Tmp & (sbyte)lms2012.Warning.WARNING_CURRENT) != 0)
								{
									dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, vmPOP3_ABS_WARN_ICON_X1, vmPOP3_ABS_WARN_ICON_Y, LARGE_ICON, WARNSIGN);
									dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, vmPOP3_ABS_WARN_ICON_X2, vmPOP3_ABS_WARN_SPEC_ICON_Y, LARGE_ICON, WARN_POWER);
									dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, vmPOP3_ABS_WARN_ICON_X3, vmPOP3_ABS_WARN_SPEC_ICON_Y, LARGE_ICON, TO_MANUAL);
									Inst.UiInstance.WarningShowed |= (sbyte)lms2012.Warning.WARNING_CURRENT;
								}
								else
								{
									if ((Tmp & (sbyte)lms2012.Warning.WARNING_VOLTAGE) != 0)
									{
										dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, vmPOP3_ABS_WARN_ICON_X, vmPOP3_ABS_WARN_ICON_Y, LARGE_ICON, WARNSIGN);
										dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, vmPOP3_ABS_WARN_SPEC_ICON_X, vmPOP3_ABS_WARN_SPEC_ICON_Y, LARGE_ICON, WARN_BATT);
										Inst.UiInstance.WarningShowed |= (sbyte)lms2012.Warning.WARNING_VOLTAGE;
									}
									else
									{
										if ((Tmp & (sbyte)lms2012.Warning.WARNING_MEMORY) != 0)
										{
											dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, vmPOP3_ABS_WARN_ICON_X, vmPOP3_ABS_WARN_ICON_Y, LARGE_ICON, WARNSIGN);
											dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, vmPOP3_ABS_WARN_SPEC_ICON_X, vmPOP3_ABS_WARN_SPEC_ICON_Y, LARGE_ICON, WARN_MEMORY);
											Inst.UiInstance.WarningShowed |= (sbyte)lms2012.Warning.WARNING_MEMORY;
										}
										else
										{
											if ((Tmp & (sbyte)lms2012.Warning.WARNING_DSPSTAT) != 0)
											{
												dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, vmPOP3_ABS_WARN_ICON_X, vmPOP3_ABS_WARN_ICON_Y, LARGE_ICON, WARNSIGN);
												dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, vmPOP3_ABS_WARN_SPEC_ICON_X, vmPOP3_ABS_WARN_SPEC_ICON_Y, LARGE_ICON, PROGRAM_ERROR);
												Inst.UiInstance.WarningShowed |= (sbyte)lms2012.Warning.WARNING_DSPSTAT;
											}
											else
											{
											}
										}
									}
								}
							}
							dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, vmPOP3_ABS_WARN_LINE_X, vmPOP3_ABS_WARN_LINE_Y, vmPOP3_ABS_WARN_LINE_ENDX, vmPOP3_ABS_WARN_LINE_Y);
							dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, FG_COLOR, vmPOP3_ABS_WARN_YES_X, vmPOP3_ABS_WARN_YES_Y, LARGE_ICON, YES_SEL);
							dLcdUpdate(Inst.UiInstance.pLcd);
							cUiButtonFlush();
							Inst.UiInstance.ScreenBlocked = 1;
						}
					}
				}

				// Find warnings that have been showed but not confirmed
				Tmp = Inst.UiInstance.WarningShowed;
				Tmp &= (sbyte)~Inst.UiInstance.WarningConfirmed;

				if (Tmp != 0)
				{
					if (cUiGetShortPress((sbyte)ButtonType.ENTER_BUTTON) != 0)
					{
						Inst.UiInstance.ScreenBlocked = 0;
						LCDCopy(&Inst.UiInstance.LcdSave, &Inst.UiInstance.LcdSafe, sizeof(Inst.UiInstance.LcdSafe));
						dLcdUpdate(Inst.UiInstance.pLcd);
						if ((Tmp & (sbyte)lms2012.Warning.WARNING_TEMP) != 0)
						{
							Inst.UiInstance.WarningConfirmed |= (sbyte)lms2012.Warning.WARNING_TEMP;
						}
						else
						{
							if ((Tmp & (sbyte)lms2012.Warning.WARNING_CURRENT) != 0)
							{
								Inst.UiInstance.WarningConfirmed |= (sbyte)lms2012.Warning.WARNING_CURRENT;
							}
							else
							{
								if ((Tmp & (sbyte)lms2012.Warning.WARNING_VOLTAGE) != 0)
								{
									Inst.UiInstance.WarningConfirmed |= (sbyte)lms2012.Warning.WARNING_VOLTAGE;
								}
								else
								{
									if ((Tmp & (sbyte)lms2012.Warning.WARNING_MEMORY) != 0)
									{
										Inst.UiInstance.WarningConfirmed |= (sbyte)lms2012.Warning.WARNING_MEMORY;
									}
									else
									{
										if ((Tmp & (sbyte)lms2012.Warning.WARNING_DSPSTAT) != 0)
										{
											Inst.UiInstance.WarningConfirmed |= (sbyte)lms2012.Warning.WARNING_DSPSTAT;
											Inst.UiInstance.Warning &= ~(sbyte)lms2012.Warning.WARNING_DSPSTAT;
										}
										else
										{
										}
									}
								}
							}
						}
					}
				}

				// Find warnings that have been showed, confirmed and removed
				Tmp = (sbyte)~Warning;
				Tmp &= (sbyte)lms2012.Warning.WARNINGS;
				Tmp &= Inst.UiInstance.WarningShowed;
				Tmp &= Inst.UiInstance.WarningConfirmed;

				Inst.UiInstance.WarningShowed &= (sbyte)~Tmp;
				Inst.UiInstance.WarningConfirmed &= (sbyte)~Tmp;
			}
		}

		DATA8 cUiNotification(DATA8 Color, DATA16 X, DATA16 Y, DATA8 Icon1, DATA8 Icon2, DATA8 Icon3, DATA8* pText, DATA8* pState)
		{
			Result Result = Result.BUSY;
			NOTIFY* pQ;
			DATA16 AvailableX;
			DATA16 UsedX;
			DATA16 Line;
			DATA16 CharIn;
			DATA16 CharOut;
			DATA8 Character;
			DATA16 X2;
			DATA16 Y2;
			DATA16 AvailableY;
			DATA16 UsedY;

			pQ = &Inst.UiInstance.Notify;

			if (*pState == 0)
			{
				*pState = 1;
				(*pQ).ScreenStartX = X;
				(*pQ).ScreenStartY = Y;
				(*pQ).ScreenWidth = POP3_width;
				(*pQ).ScreenHeight = POP3_height;
				(*pQ).IconStartY = (*pQ).ScreenStartY + 10;
				(*pQ).IconWidth = dLcdGetIconWidth(LARGE_ICON);
				(*pQ).IconHeight = dLcdGetIconHeight(LARGE_ICON);
				(*pQ).IconSpaceX = (*pQ).IconWidth;

				(*pQ).YesNoStartX = (short)((*pQ).ScreenStartX + ((*pQ).ScreenWidth / 2));
				(*pQ).YesNoStartX -= (short)(((*pQ).IconWidth + 8) / 2);
				(*pQ).YesNoStartX = cUiAlignX((*pQ).YesNoStartX);
				(*pQ).YesNoStartY = (short)((*pQ).ScreenStartY + 40);

				(*pQ).LineStartX = (short)((*pQ).ScreenStartX + 5);
				(*pQ).LineStartY = (short)((*pQ).ScreenStartY + 39);
				(*pQ).LineEndX = (short)((*pQ).LineStartX + 134);

				// Find no of icons
				(*pQ).NoOfIcons = 0;
				if (Icon1 > (sbyte)NIcon.ICON_NONE)
				{
					(*pQ).NoOfIcons++;
				}
				if (Icon2 > (sbyte)NIcon.ICON_NONE)
				{
					(*pQ).NoOfIcons++;
				}
				if (Icon3 > (sbyte)NIcon.ICON_NONE)
				{
					(*pQ).NoOfIcons++;
				}

			  // Find no of text lines
			  (*pQ).TextLines = 0;
				if (pText[0] != 0)
				{
					(*pQ).IconStartX = (short)((*pQ).ScreenStartX + 8);
					(*pQ).IconStartX = cUiAlignX((*pQ).IconStartX);

					AvailableX = (*pQ).ScreenWidth;
					AvailableX -= (short)((((*pQ).IconStartX - (*pQ).ScreenStartX)) * 2);

					AvailableX -= (short)((*pQ).NoOfIcons * (*pQ).IconSpaceX);


					(*pQ).NoOfChars = strlen((char*)pText);


					(*pQ).Font = (sbyte)FontType.SMALL_FONT;
					(*pQ).FontWidth = dLcdGetFontWidth((*pQ).Font);
					UsedX = (short)((*pQ).FontWidth * (*pQ).NoOfChars);

					Line = 0;

					if (UsedX <= AvailableX)
					{ // One line - small font

						if ((AvailableX - UsedX) >= 32)
						{
							(*pQ).IconStartX += 32;
						}

						snprintf((char*)(*pQ).TextLine[Line], MAX_NOTIFY_LINE_CHARS, "%s", pText);
						Line++;
						(*pQ).TextLines++;

						(*pQ).TextStartX = (short)((*pQ).IconStartX + ((*pQ).NoOfIcons * (*pQ).IconSpaceX));
						(*pQ).TextStartY = (short)((*pQ).ScreenStartY + 18);
						(*pQ).TextSpaceY = dLcdGetFontHeight((*pQ).Font) + 1;
					}
					else
					{ // one or more lines - tiny font

						(*pQ).Font = (sbyte)FontType.TINY_FONT;
						(*pQ).FontWidth = dLcdGetFontWidth((*pQ).Font);
						UsedX = (short)((*pQ).FontWidth * (*pQ).NoOfChars);
						AvailableX -= (*pQ).FontWidth;

						CharIn = 0;

						while ((pText[CharIn] != 0) && (Line < MAX_NOTIFY_LINES))
						{
							CharOut = 0;
							UsedX = 0;
							while ((pText[CharIn] != 0) && (CharOut < MAX_NOTIFY_LINE_CHARS) && (UsedX < (AvailableX - (*pQ).FontWidth)))
							{
								Character = pText[CharIn];
								if (Character == '_')
								{
									Character = (sbyte)' ';
								}
							  (*pQ).TextLine[Line][CharOut] = Character;
								CharIn++;
								CharOut++;
								UsedX += (*pQ).FontWidth;
							}
							while ((CharOut > 8) && (pText[CharIn] != ' ') && (pText[CharIn] != '_') && (pText[CharIn] != 0))
							{
								CharIn--;
								CharOut--;
							}
							if (pText[CharIn] != 0)
							{
								CharIn++;
							}
						  (*pQ).TextLine[Line][CharOut] = 0;
							Line++;
						}

					  (*pQ).TextLines = Line;

						(*pQ).TextStartX = (short)((*pQ).IconStartX + ((*pQ).NoOfIcons * (*pQ).IconSpaceX) + (*pQ).FontWidth);
						(*pQ).TextSpaceY = dLcdGetFontHeight((*pQ).Font) + 1;


						AvailableY = (short)((*pQ).LineStartY - ((*pQ).ScreenStartY + 5));
						UsedY = (short)((*pQ).TextLines * (*pQ).TextSpaceY);

						while (UsedY > AvailableY)
						{
							(*pQ).TextLines--;
							UsedY = (short)((*pQ).TextLines * (*pQ).TextSpaceY);
						}
						Y2 = (short)((AvailableY - UsedY) / 2);

						(*pQ).TextStartY = (short)((*pQ).ScreenStartY + Y2 + 5);
					}

				}
				else
				{
					(*pQ).IconStartX = (short)((*pQ).ScreenStartX + ((*pQ).ScreenWidth / 2));
					(*pQ).IconStartX -= (short)(((*pQ).IconWidth + 8) / 2);
					(*pQ).IconStartX -= (short)(((*pQ).NoOfIcons / 2) * (*pQ).IconWidth);
					(*pQ).IconStartX = cUiAlignX((*pQ).IconStartX);
					(*pQ).TextStartY = (short)((*pQ).ScreenStartY + 8);
				}

			  (*pQ).NeedUpdate = 1;
			}

			if ((*pQ).NeedUpdate != 0)
			{
				//* UPDATE ***************************************************************************************************
				(*pQ).NeedUpdate = 0;

				dLcdDrawPicture((*Inst.UiInstance.pLcd).Lcd, Color, (*pQ).ScreenStartX, (*pQ).ScreenStartY, POP3_width, POP3_height, (UBYTE*)POP3_bits);

				X2 = (*pQ).IconStartX;

				if (Icon1 > (sbyte)NIcon.ICON_NONE)
				{
					dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, Color, X2, (*pQ).IconStartY, IconType.LARGE_ICON, Icon1);
					X2 += (*pQ).IconSpaceX;
				}
				if (Icon2 > (sbyte)NIcon.ICON_NONE)
				{
					dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, Color, X2, (*pQ).IconStartY, IconType.LARGE_ICON, Icon2);
					X2 += (*pQ).IconSpaceX;
				}
				if (Icon3 > (sbyte)NIcon.ICON_NONE)
				{
					dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, Color, X2, (*pQ).IconStartY, IconType.LARGE_ICON, Icon3);
					X2 += (*pQ).IconSpaceX;
				}

				Line = 0;
				Y2 = (*pQ).TextStartY;
				while (Line < (*pQ).TextLines)
				{
					dLcdDrawText((*Inst.UiInstance.pLcd).Lcd, Color, (*pQ).TextStartX, Y2, (*pQ).Font, (*pQ).TextLine[Line]);
					Y2 += (*pQ).TextSpaceY;
					Line++;
				}

				dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, Color, (*pQ).LineStartX, (*pQ).LineStartY, (*pQ).LineEndX, (*pQ).LineStartY);

				dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, Color, (*pQ).YesNoStartX, (*pQ).YesNoStartY, IconType.LARGE_ICON, YES_SEL);

				cUiUpdateLcd();
				Inst.UiInstance.ScreenBusy = 0;
			}

			if (cUiGetShortPress((sbyte)ButtonType.ENTER_BUTTON) != 0)
			{
				cUiButtonFlush();
				Result = Result.OK;
				*pState = 0;
			}

			return (sbyte)(Result);
		}


		DATA8 cUiQuestion(DATA8 Color, DATA16 X, DATA16 Y, DATA8 Icon1, DATA8 Icon2, DATA8* pText, DATA8* pState, DATA8* pAnswer)
		{
			Result Result = Result.BUSY;
			TQUESTION* pQ;
			DATA16 Inc;

			pQ = &Inst.UiInstance.Question;

			Inc = cUiGetHorz();
			if (Inc != 0)
			{
				(*pQ).NeedUpdate = 1;

				*pAnswer += (sbyte)(Inc);

				if (*pAnswer > 1)
				{
					*pAnswer = 1;
					(*pQ).NeedUpdate = 0;
				}
				if (*pAnswer < 0)
				{
					*pAnswer = 0;
					(*pQ).NeedUpdate = 0;
				}
			}

			if (*pState == 0)
			{
				*pState = 1;
				(*pQ).ScreenStartX = X;
				(*pQ).ScreenStartY = Y;
				(*pQ).IconWidth = dLcdGetIconWidth(IconType.LARGE_ICON);
				(*pQ).IconHeight = dLcdGetIconHeight(IconType.LARGE_ICON);

				(*pQ).NoOfIcons = 0;
				if (Icon1 > (sbyte)NIcon.ICON_NONE)
				{
					(*pQ).NoOfIcons++;
				}
				if (Icon2 > (sbyte)NIcon.ICON_NONE)
				{
					(*pQ).NoOfIcons++;
				}
			  (*pQ).Default = *pAnswer;

				(*pQ).NeedUpdate = 1;
			}


			if ((*pQ).NeedUpdate != 0)
			{
				//* UPDATE ***************************************************************************************************
				(*pQ).NeedUpdate = 0;

				dLcdDrawPicture((*Inst.UiInstance.pLcd).Lcd, Color, (*pQ).ScreenStartX, (*pQ).ScreenStartY, POP3_width, POP3_height, (UBYTE*)POP3_bits);
				(*pQ).ScreenWidth = POP3_width;
				(*pQ).ScreenHeight = POP3_height;

				(*pQ).IconStartX = (short)((*pQ).ScreenStartX + ((*pQ).ScreenWidth / 2));
				if ((*pQ).NoOfIcons > 1)
				{
					(*pQ).IconStartX -= (*pQ).IconWidth;
				}
				else
				{
					(*pQ).IconStartX -= (short)((*pQ).IconWidth / 2);
				}
				(*pQ).IconStartX = cUiAlignX((*pQ).IconStartX);
				(*pQ).IconSpaceX = (*pQ).IconWidth;
				(*pQ).IconStartY = (short)((*pQ).ScreenStartY + 10);

				(*pQ).YesNoStartX = (short)((*pQ).ScreenStartX + ((*pQ).ScreenWidth / 2));
				(*pQ).YesNoStartX -= 8;
				(*pQ).YesNoStartX -= (*pQ).IconWidth;
				(*pQ).YesNoStartX = cUiAlignX((*pQ).YesNoStartX);
				(*pQ).YesNoSpaceX = (short)((*pQ).IconWidth + 16);
				(*pQ).YesNoStartY = (short)((*pQ).ScreenStartY + 40);

				(*pQ).LineStartX = (short)((*pQ).ScreenStartX + 5);
				(*pQ).LineStartY = (short)((*pQ).ScreenStartY + 39);
				(*pQ).LineEndX = (short)((*pQ).LineStartX + 134);

				switch ((*pQ).NoOfIcons)
				{
					case 1:
						{
							dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, Color, (*pQ).IconStartX, (*pQ).IconStartY, IconType.LARGE_ICON, Icon1);
						}
						break;

					case 2:
						{
							dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, Color, (*pQ).IconStartX, (*pQ).IconStartY, IconType.LARGE_ICON, Icon1);
							dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, Color, (*pQ).IconStartX + (*pQ).IconSpaceX, (*pQ).IconStartY, IconType.LARGE_ICON, Icon2);
						}
						break;

				}

				if (*pAnswer == 0)
				{
					dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, Color, (*pQ).YesNoStartX, (*pQ).YesNoStartY, IconType.LARGE_ICON, NO_SEL);
					dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, Color, (*pQ).YesNoStartX + (*pQ).YesNoSpaceX, (*pQ).YesNoStartY, IconType.LARGE_ICON, YES_NOTSEL);
				}
				else
				{
					dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, Color, (*pQ).YesNoStartX, (*pQ).YesNoStartY, IconType.LARGE_ICON, NO_NOTSEL);
					dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, Color, (*pQ).YesNoStartX + (*pQ).YesNoSpaceX, (*pQ).YesNoStartY, IconType.LARGE_ICON, YES_SEL);
				}

				dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, Color, (*pQ).LineStartX, (*pQ).LineStartY, (*pQ).LineEndX, (*pQ).LineStartY);

				cUiUpdateLcd();
				Inst.UiInstance.ScreenBusy = 0;
			}
			if (cUiGetShortPress((sbyte)ButtonType.ENTER_BUTTON) != 0)
			{
				cUiButtonFlush();
				Result = Result.OK;
				*pState = 0;
			}
			if (cUiGetShortPress((sbyte)ButtonType.BACK_BUTTON) != 0)
			{
				cUiButtonFlush();
				Result = Result.OK;
				*pState = 0;
				*pAnswer = -1;
			}

			return (sbyte)(Result);
		}


		Result cUiIconQuestion(DATA8 Color, DATA16 X, DATA16 Y, DATA8* pState, DATA32* pIcons)
		{
			Result Result = Result.BUSY;
			IQUESTION* pQ;
			DATA32 Mask;
			DATA32 TmpIcons;
			DATA16 Tmp;
			DATA16 Loop;
			DATA8 Icon;

			pQ = &Inst.UiInstance.IconQuestion;

			if (*pState == 0)
			{
				*pState = 1;
				(*pQ).ScreenStartX = X;
				(*pQ).ScreenStartY = Y;
				(*pQ).ScreenWidth = POP2_width;
				(*pQ).ScreenHeight = POP2_height;
				(*pQ).IconWidth = dLcdGetIconWidth(IconType.LARGE_ICON);
				(*pQ).IconHeight = dLcdGetIconHeight(IconType.LARGE_ICON);
				(*pQ).Frame = 2;
				(*pQ).Icons = *pIcons;
				(*pQ).NoOfIcons = 0;
				(*pQ).PointerX = 0;

				TmpIcons = (*pQ).Icons;
				while (TmpIcons != 0)
				{
					if ((TmpIcons & 1) != 0)
					{
						(*pQ).NoOfIcons++;
					}
					TmpIcons >>= 1;
				}

				if ((*pQ).NoOfIcons != 0)
				{
					(*pQ).IconStartY = (short)((*pQ).ScreenStartY + (((*pQ).ScreenHeight - (*pQ).IconHeight) / 2));

					(*pQ).IconSpaceX = (short)((((*pQ).ScreenWidth - ((*pQ).IconWidth * (*pQ).NoOfIcons)) / (*pQ).NoOfIcons) + (*pQ).IconWidth);
					(*pQ).IconSpaceX = (short)((*pQ).IconSpaceX & ~7);

					Tmp = (short)((*pQ).IconSpaceX * (*pQ).NoOfIcons - ((*pQ).IconSpaceX - (*pQ).IconWidth));

					(*pQ).IconStartX = (short)((*pQ).ScreenStartX + (((*pQ).ScreenWidth - Tmp) / 2));
					(*pQ).IconStartX = (short)(((*pQ).IconStartX + 7) & ~7);

					(*pQ).SelectStartX = (short)((*pQ).IconStartX - 1);
					(*pQ).SelectStartY = (short)((*pQ).IconStartY - 1);
					(*pQ).SelectWidth = (short)((*pQ).IconWidth + 2);
					(*pQ).SelectHeight = (short)((*pQ).IconHeight + 2);
					(*pQ).SelectSpaceX = (*pQ).IconSpaceX;
				}

				(*pQ).NeedUpdate = 1;
			}

			if ((*pQ).NoOfIcons != 0)
			{
				// Check for move pointer
				Tmp = cUiGetHorz();
				if (Tmp != 0)
				{
					(*pQ).PointerX += Tmp;

					(*pQ).NeedUpdate = 1;

					if ((*pQ).PointerX < 0)
					{
						(*pQ).PointerX = 0;
						(*pQ).NeedUpdate = 0;
					}
					if ((*pQ).PointerX >= (*pQ).NoOfIcons)
					{
						(*pQ).PointerX = (short)((*pQ).NoOfIcons - 1);
						(*pQ).NeedUpdate = 0;
					}
				}
			}

			if ((*pQ).NeedUpdate != 0)
			{
				//* UPDATE ***************************************************************************************************
				(*pQ).NeedUpdate = 0;

				dLcdDrawPicture((*Inst.UiInstance.pLcd).Lcd, Color, (*pQ).ScreenStartX, (*pQ).ScreenStartY, POP2_width, POP2_height, (UBYTE*)POP2_bits);
				(*pQ).ScreenWidth = POP2_width;
				(*pQ).ScreenHeight = POP2_height;

				// Show icons
				Loop = 0;
				Icon = 0;
				TmpIcons = (*pQ).Icons;
				while (Loop < (*pQ).NoOfIcons)
				{
					while ((TmpIcons & 1) == 0)
					{
						Icon++;
						TmpIcons >>= 1;
					}
					dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, Color, (*pQ).IconStartX + (*pQ).IconSpaceX * Loop, (*pQ).IconStartY, LARGE_ICON, Icon);
					Loop++;
					Icon++;
					TmpIcons >>= 1;
				}

				// Show selection
				dLcdInverseRect((*Inst.UiInstance.pLcd).Lcd, (*pQ).SelectStartX + (*pQ).SelectSpaceX * (*pQ).PointerX, (*pQ).SelectStartY, (*pQ).SelectWidth, (*pQ).SelectHeight);

				// Update screen
				cUiUpdateLcd();
				Inst.UiInstance.ScreenBusy = 0;
			}
			if (cUiGetShortPress((sbyte)ButtonType.ENTER_BUTTON) != 0)
			{
				if ((*pQ).NoOfIcons != 0)
				{
					Mask = 0x00000001;
					TmpIcons = (*pQ).Icons;
					Loop = (short)((*pQ).PointerX + 1);

					do
					{
						if ((TmpIcons & Mask) != 0)
						{
							Loop--;
						}
						Mask <<= 1;
					}
					while (Loop != 0 && Mask != 0);
					Mask >>= 1;
					*pIcons = Mask;
				}
				else
				{
					*pIcons = 0;
				}
				cUiButtonFlush();
				Result = Result.OK;
				*pState = 0;
			}
			if (cUiGetShortPress((sbyte)ButtonType.BACK_BUTTON) != 0)
			{
				*pIcons = 0;
				cUiButtonFlush();
				Result = Result.OK;
				*pState = 0;
			}

			return (Result);
		}

		DATA8 cUiKeyboard(DATA8 Color, DATA16 X, DATA16 Y, DATA8 Icon, DATA8 Lng, DATA8* pText, DATA8* pCharSet, DATA8* pAnswer)
		{
			KEYB* pK;
			DATA16 Width;
			DATA16 Height;
			DATA16 Inc;
			DATA16 SX;
			DATA16 SY;
			DATA16 X3;
			DATA16 X4;
			DATA16 Tmp;
			DATA8 TmpChar;
			DATA8 SelectedChar = 0;

			//            Value    Marking
			//  table  >  0x20  -> normal rect
			//  table  =  0x20  -> spacebar
			//  table  =  0     -> end
			//  table  =  0x01  -> num
			//  table  =  0x02  -> cap
			//  table  =  0x03  -> non cap
			//  table  =  0x04  -> big cap
			//  table  =  0x08  -> backspace
			//  table  =  0x0D  -> enter
			//


			DATA8[][][] KeyboardLayout = new DATA8[lms2012.MAX_KEYB_DEEPT][][];
			DATA8[][] lay1 = new DATA8[lms2012.MAX_KEYB_HEIGHT][];
			DATA8[][] lay2 = new DATA8[lms2012.MAX_KEYB_HEIGHT][];
			DATA8[][] lay3 = new DATA8[lms2012.MAX_KEYB_HEIGHT][];

			lay1[0] = new DATA8[lms2012.MAX_KEYB_WIDTH] { (DATA8)'Q', (DATA8)'W', (DATA8)'E', (DATA8)'R', (DATA8)'T', (DATA8)'Y', (DATA8)'U', (DATA8)'I', (DATA8)'O', (DATA8)'P', 0x08, 0x00 };
			lay1[1] = new DATA8[lms2012.MAX_KEYB_WIDTH] { 0x03, (DATA8)'A', (DATA8)'S', (DATA8)'D', (DATA8)'F', (DATA8)'G', (DATA8)'H', (DATA8)'J', (DATA8)'K', (DATA8)'L', 0x0D, 0x00 };
			lay1[2] = new DATA8[lms2012.MAX_KEYB_WIDTH] { 0x01, (DATA8)'Z', (DATA8)'X', (DATA8)'C', (DATA8)'V', (DATA8)'B', (DATA8)'N', (DATA8)'M', 0x0D, 0x0D, 0x0D, 0x00 };
			lay1[3] = new DATA8[lms2012.MAX_KEYB_WIDTH] { (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', 0x0D, 0x00 };

			lay2[0] = new DATA8[lms2012.MAX_KEYB_WIDTH] { (DATA8)'q', (DATA8)'w', (DATA8)'e', (DATA8)'r', (DATA8)'t', (DATA8)'y', (DATA8)'u', (DATA8)'i', (DATA8)'o', (DATA8)'p', 0x08, 0x00 };
			lay2[1] = new DATA8[lms2012.MAX_KEYB_WIDTH] { 0x03, (DATA8)'a', (DATA8)'s', (DATA8)'d', (DATA8)'f', (DATA8)'g', (DATA8)'h', (DATA8)'j', (DATA8)'k', (DATA8)'l', 0x0D, 0x00 };
			lay2[2] = new DATA8[lms2012.MAX_KEYB_WIDTH] { 0x01, (DATA8)'z', (DATA8)'x', (DATA8)'c', (DATA8)'v', (DATA8)'b', (DATA8)'n', (DATA8)'m', 0x0D, 0x0D, 0x0D, 0x00 };
			lay2[3] = new DATA8[lms2012.MAX_KEYB_WIDTH] { (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', 0x0D, 0x00 };

			lay3[0] = new DATA8[lms2012.MAX_KEYB_WIDTH] { (DATA8)'1', (DATA8)'2', (DATA8)'3', (DATA8)'4', (DATA8)'5', (DATA8)'6', (DATA8)'7', (DATA8)'8', (DATA8)'9', (DATA8)'0', 0x08, 0x00 };
			lay3[1] = new DATA8[lms2012.MAX_KEYB_WIDTH] { 0x04, (DATA8)'+', (DATA8)'-', (DATA8)'=', (DATA8)'<', (DATA8)'>', (DATA8)'/', (DATA8)'\\', (DATA8)'*', (DATA8)':', 0x0D, 0x00 };
			lay3[2] = new DATA8[lms2012.MAX_KEYB_WIDTH] { 0x04, (DATA8)'(', (DATA8)')', (DATA8)'_', (DATA8)'.', (DATA8)'@', (DATA8)'!', (DATA8)'?', 0x0D, 0x0D, 0x0D, 0x00 };
			lay3[3] = new DATA8[lms2012.MAX_KEYB_WIDTH] { (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', 0x0D, 0x00 };

			KeyboardLayout[0] = lay1; KeyboardLayout[1] = lay2; KeyboardLayout[2] = lay3;

			pK = &Inst.UiInstance.Keyboard;

			if (*pCharSet != 0)
			{
				(*pK).CharSet = *pCharSet;
				*pCharSet = 0;
				(*pK).ScreenStartX = X;
				(*pK).ScreenStartY = Y;

				if ((Icon >= 0) && (Icon < (sbyte)NIcon.ICON_BRICK1))
				{
					(*pK).IconStartX = cUiAlignX((short)((*pK).ScreenStartX + 7));
					(*pK).IconStartY = (short)((*pK).ScreenStartY + 4);
					(*pK).TextStartX = (*pK).IconStartX + dLcdGetIconWidth(IconType.NORMAL_ICON);
				}
				else
				{
					(*pK).TextStartX = cUiAlignX((short)((*pK).ScreenStartX + 9));
				}
				(*pK).TextStartY = (short)((*pK).ScreenStartY + 7);
				(*pK).StringStartX = (short)((*pK).ScreenStartX + 8);
				(*pK).StringStartY = (short)((*pK).ScreenStartY + 22);
				(*pK).KeybStartX = (short)((*pK).ScreenStartX + 13);
				(*pK).KeybStartY = (short)((*pK).ScreenStartY + 40);
				(*pK).KeybSpaceX = 11;
				(*pK).KeybSpaceY = 14;
				(*pK).KeybHeight = 13;
				(*pK).KeybWidth = 9;
				(*pK).Layout = 0;
				(*pK).PointerX = 10;
				(*pK).PointerY = 1;
				(*pK).NeedUpdate = 1;
			}

			Width = strlen((char*)KeyboardLayout[(*pK).Layout][(*pK).PointerY]) - 1;
			Height = MAX_KEYB_HEIGHT - 1;

			Inc = cUiGetHorz();
			(*pK).PointerX += Inc;
			if ((*pK).PointerX < 0)
			{
				(*pK).PointerX = 0;
			}
			if ((*pK).PointerX > Width)
			{
				(*pK).PointerX = Width;
			}
			Inc = cUiGetVert();
			(*pK).PointerY += Inc;
			if ((*pK).PointerY < 0)
			{
				(*pK).PointerY = 0;
			}
			if ((*pK).PointerY > Height)
			{
				(*pK).PointerY = Height;
			}


			TmpChar = KeyboardLayout[(*pK).Layout][(*pK).PointerY][(*pK).PointerX];

			if (cUiGetShortPress((sbyte)ButtonType.BACK_BUTTON) != 0)
			{
				SelectedChar = 0x0D;
				pAnswer[0] = 0;
			}
			if (cUiGetShortPress((sbyte)ButtonType.ENTER_BUTTON) != 0)
			{
				SelectedChar = TmpChar;

				switch (SelectedChar)
				{
					case 0x01:
						{
							(*pK).Layout = 2;
						}
						break;

					case 0x02:
						{
							(*pK).Layout = 0;
						}
						break;

					case 0x03:
						{
							(*pK).Layout = 1;
						}
						break;

					case 0x04:
						{
							(*pK).Layout = 0;
						}
						break;

					case 0x08:
						{
							Tmp = (DATA16)strlen((char*)pAnswer);
							if (Tmp != 0)
							{
								Tmp--;
								pAnswer[Tmp] = 0;
							}
						}
						break;

					case (sbyte)'\r':
						{
						}
						break;

					default:
						{
							if (ValidateChar(&SelectedChar, (*pK).CharSet) == Result.OK)
							{
								Tmp = (DATA16)strlen((char*)pAnswer);
								pAnswer[Tmp] = SelectedChar;
								if (++Tmp >= Lng)
								{
									Tmp--;
								}
								pAnswer[Tmp] = 0;
							}
						}
						break;


				}

				TmpChar = KeyboardLayout[(*pK).Layout][(*pK).PointerY][(*pK).PointerX];

				(*pK).NeedUpdate = 1;
			}

			if (((*pK).OldX != (*pK).PointerX) || ((*pK).OldY != (*pK).PointerY))
			{
				(*pK).OldX = (*pK).PointerX;
				(*pK).OldY = (*pK).PointerY;
				(*pK).NeedUpdate = 1;
			}

			if ((*pK).NeedUpdate != 0)
			{
				//* UPDATE ***************************************************************************************************
				(*pK).NeedUpdate = 0;

				switch ((*pK).Layout)
				{
					case 0:
						{
							dLcdDrawPicture((*Inst.UiInstance.pLcd).Lcd, Color, (*pK).ScreenStartX, (*pK).ScreenStartY, keyboardc_width, keyboardc_height, (UBYTE*)keyboardc_bits);
						}
						break;

					case 1:
						{
							dLcdDrawPicture((*Inst.UiInstance.pLcd).Lcd, Color, (*pK).ScreenStartX, (*pK).ScreenStartY, keyboards_width, keyboards_height, (UBYTE*)keyboards_bits);
						}
						break;

					case 2:
						{
							dLcdDrawPicture((*Inst.UiInstance.pLcd).Lcd, Color, (*pK).ScreenStartX, (*pK).ScreenStartY, keyboardn_width, keyboardn_height, (UBYTE*)keyboardn_bits);
						}
						break;

				}
				if ((Icon >= 0) && (Icon < (sbyte)NIcon.ICON_BRICK1))
				{
					dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, Color, (*pK).IconStartX, (*pK).IconStartY, IconType.NORMAL_ICON, Icon);
				}
				if (pText[0] != 0)
				{
					dLcdDrawText((*Inst.UiInstance.pLcd).Lcd, Color, (*pK).TextStartX, (*pK).TextStartY, FontType.SMALL_FONT, pText);
				}


				X4 = 0;
				X3 = strlen((char*)pAnswer);
				if (X3 > 15)
				{
					X4 = (short)(X3 - 15);
				}

				dLcdDrawText((*Inst.UiInstance.pLcd).Lcd, Color, (*pK).StringStartX, (*pK).StringStartY, FontType.NORMAL_FONT, &pAnswer[X4]);
				dLcdDrawChar((*Inst.UiInstance.pLcd).Lcd, Color, (*pK).StringStartX + (X3 - X4) * 8, (*pK).StringStartY, FontType.NORMAL_FONT, '_');



				SX = (short)((*pK).KeybStartX + (*pK).PointerX * (*pK).KeybSpaceX);
				SY = (short)((*pK).KeybStartY + (*pK).PointerY * (*pK).KeybSpaceY);

				switch (TmpChar)
				{
					case 0x01:
					case 0x02:
					case 0x03:
						{
							dLcdInverseRect((*Inst.UiInstance.pLcd).Lcd, SX - 8, SY, (*pK).KeybWidth + 8, (*pK).KeybHeight);
						}
						break;

					case 0x04:
						{
							dLcdInverseRect((*Inst.UiInstance.pLcd).Lcd, SX - 8, (*pK).KeybStartY + 1 * (*pK).KeybSpaceY, (*pK).KeybWidth + 8, (*pK).KeybHeight * 2 + 1);
						}
						break;

					case 0x08:
						{
							dLcdInverseRect((*Inst.UiInstance.pLcd).Lcd, SX + 2, SY, (*pK).KeybWidth + 5, (*pK).KeybHeight);
						}
						break;

					case 0x0D:
						{
							SX = (short)((*pK).KeybStartX + 112);
							SY = (short)((*pK).KeybStartY + 1 * (*pK).KeybSpaceY);
							dLcdInverseRect((*Inst.UiInstance.pLcd).Lcd, SX, SY, (*pK).KeybWidth + 5, (*pK).KeybSpaceY + 1);
							SX = (short)((*pK).KeybStartX + 103);
							SY = (short)((*pK).KeybStartY + 1 + 2 * (*pK).KeybSpaceY);
							dLcdInverseRect((*Inst.UiInstance.pLcd).Lcd, SX, SY, (*pK).KeybWidth + 14, (*pK).KeybSpaceY * 2 - 4);
						}
						break;

					case 0x20:
						{
							dLcdInverseRect((*Inst.UiInstance.pLcd).Lcd, (*pK).KeybStartX + 11, SY + 1, (*pK).KeybWidth + 68, (*pK).KeybHeight - 3);
						}
						break;

					default:
						{
							dLcdInverseRect((*Inst.UiInstance.pLcd).Lcd, SX + 1, SY, (*pK).KeybWidth, (*pK).KeybHeight);
						}
						break;

				}
				cUiUpdateLcd();
				Inst.UiInstance.ScreenBusy = 0;
			}

			return (SelectedChar);
		}


		void cUiDrawBar(DATA8 Color, DATA16 X, DATA16 Y, DATA16 X1, DATA16 Y1, DATA16 Min, DATA16 Max, DATA16 Act)
		{
			DATA16 Tmp;
			DATA16 Items;
			DATA16 KnobHeight = 7;

			Items = (short)(Max - Min);

			switch (X1)
			{
				case 5:
					{
						KnobHeight = 7;
					}
					break;

				case 6:
					{
						KnobHeight = 9;
					}
					break;

				default:
					{
						if (Items > 0)
						{
							KnobHeight = (short)(Y1 / Items);
						}
					}
					break;

			}

			if (Act < Min)
			{
				Act = Min;
			}
			if (Act > Max)
			{
				Act = Max;
			}

			// Draw scroll bar
			dLcdRect((*Inst.UiInstance.pLcd).Lcd, Color, X, Y, X1, Y1);

			// Draw nob
			Tmp = Y;
			if ((Items > 1) && (Act > 0))
			{
				Tmp += (short)((Y1 - KnobHeight) * (Act - 1) / (Items - 1));
			}

			switch (X1)
			{
				case 5:
					{
						dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X + 1, Tmp);
						dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X + 2, Tmp);
						dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X + 3, Tmp);
						Tmp++;
						dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X + 1, Tmp);
						dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X + 3, Tmp);
						Tmp++;
						dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X + 2, Tmp);
						Tmp++;
						dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X + 1, Tmp);
						dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X + 3, Tmp);
						Tmp++;
						dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X + 2, Tmp);
						Tmp++;
						dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X + 1, Tmp);
						dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X + 3, Tmp);
						Tmp++;
						dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X + 1, Tmp);
						dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X + 2, Tmp);
						dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X + 3, Tmp);
					}
					break;

				case 6:
					{
						dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X + 2, Tmp);
						dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X + 3, Tmp);
						Tmp++;
						dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X + 1, Tmp);
						dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X + 4, Tmp);
						Tmp++;
						Tmp++;
						dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X + 2, Tmp);
						dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X + 3, Tmp);
						Tmp++;
						Tmp++;
						dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X + 2, Tmp);
						dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X + 3, Tmp);
						Tmp++;
						Tmp++;
						dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X + 1, Tmp);
						dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X + 4, Tmp);
						Tmp++;
						dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X + 2, Tmp);
						dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X + 3, Tmp);
					}
					break;

				default:
					{
						dLcdFillRect((*Inst.UiInstance.pLcd).Lcd, Color, X, Tmp, X1, KnobHeight);
					}
					break;

			}

		}

		Result cUiBrowser(DATA8 Type, DATA16 X, DATA16 Y, DATA16 X1, DATA16 Y1, DATA8 Lng, DATA8* pType, DATA8* pAnswer)
		{
			Result Result = Result.BUSY;
			DATA32 Image;
			BROWSER* pB;
			PRGID PrgId;
			OBJID ObjId;
			DATA16 Tmp;
			DATA16 Indent;
			DATA16 Item;
			DATA16 TotalItems;
			DATA8 TmpType;
			DATA8 Folder;
			DATA8 OldPriority;
			DATA8 Priority;
			DATA8 Color;
			DATA16 Ignore;
			DATA8 Data8;
			DATA32 Total;
			DATA32 Free;
			Result TmpResult;
			HANDLER TmpHandle;

			PrgId = Inst.CurrentProgramId();
			ObjId = Inst.CallingObjectId();
			pB = &Inst.UiInstance.Browser;

			Color = (sbyte)FG_COLOR;

			// Test ignore horizontal update
			if ((Type & 0x20) != 0)
			{
				Ignore = -1;
			}
			else
			{
				if ((Type & 0x10) != 0)
				{
					Ignore = 1;
				}
				else
				{
					Ignore = 0;
				}
			}

			// Isolate browser type
			Type &= 0x0F;

			Inst.CheckUsbstick(&Data8, &Total, &Free, 0);
			if (Data8 != 0)
			{
				Inst.UiInstance.UiUpdate = 1;
			}
			Inst.CheckSdcard(&Data8, &Total, &Free, 0);
			if (Data8 != 0)
			{
				Inst.UiInstance.UiUpdate = 1;
			}

			if (ProgramStatusChange(Slot.USER_SLOT) == STOPPED)
			{
				if (Type != (sbyte)BrowserType.BROWSE_FILES)
				{
					Result = Result.OK;
					*pType = 0;
				}
			}

			if ((*pType == (sbyte)FileType.TYPE_REFRESH_BROWSER))
			{
				Inst.UiInstance.UiUpdate = 1;
			}

			if ((*pType == (sbyte)FileType.TYPE_RESTART_BROWSER))
			{
				if ((*pB).hFiles != 0)
				{
					cMemoryCloseFolder((*pB).PrgId, &(*pB).hFiles);
				}
				if ((*pB).hFolders != 0)
				{
					cMemoryCloseFolder((*pB).PrgId, &(*pB).hFolders);
				}
				(*pB).PrgId = 0;
				(*pB).ObjId = 0;
				//    pAnswer[0]          =  0;
				*pType = 0;
			}

			if (((*pB).PrgId == 0) && ((*pB).ObjId == 0))
			{
				//* INIT *****************************************************************************************************

				// Define screen
				(*pB).ScreenStartX = X;
				(*pB).ScreenStartY = Y;
				(*pB).ScreenWidth = X1;
				(*pB).ScreenHeight = Y1;

				// calculate lines on screen
				(*pB).LineSpace = 5;
				(*pB).IconHeight = dLcdGetIconHeight(NORMAL_ICON);
				(*pB).LineHeight = (short)((*pB).IconHeight + (*pB).LineSpace);
				(*pB).Lines = (short)((*pB).ScreenHeight / (*pB).LineHeight);

				// calculate chars and lines on screen
				(*pB).CharWidth = dLcdGetFontWidth(NORMAL_FONT);
				(*pB).CharHeight = dLcdGetFontHeight(NORMAL_FONT);
				(*pB).IconWidth = dLcdGetIconWidth(NORMAL_ICON);
				(*pB).Chars = (short)(((*pB).ScreenWidth - (*pB).IconWidth) / (*pB).CharWidth);

				// calculate start of icon
				(*pB).IconStartX = cUiAlignX((*pB).ScreenStartX);
				(*pB).IconStartY = (short)((*pB).ScreenStartY + (*pB).LineSpace / 2);

				// calculate start of text
				(*pB).TextStartX = cUiAlignX((short)((*pB).ScreenStartX + (*pB).IconWidth));
				(*pB).TextStartY = (short)((*pB).ScreenStartY + ((*pB).LineHeight - (*pB).CharHeight) / 2);

				// Calculate selection barBrowser
				(*pB).SelectStartX = (short)((*pB).ScreenStartX + 1);
				(*pB).SelectWidth = (short)((*pB).ScreenWidth - ((*pB).CharWidth + 5));
				(*pB).SelectStartY = (short)((*pB).IconStartY - 1);
				(*pB).SelectHeight = (short)((*pB).IconHeight + 2);

				// Calculate scroll bar
				(*pB).ScrollWidth = 6;
				(*pB).NobHeight = 9;
				(*pB).ScrollStartX = (short)((*pB).ScreenStartX + (*pB).ScreenWidth - (*pB).ScrollWidth);
				(*pB).ScrollStartY = (short)((*pB).ScreenStartY + 1);
				(*pB).ScrollHeight = (short)((*pB).ScreenHeight - 2);
				(*pB).ScrollSpan = (short)((*pB).ScrollHeight - (*pB).NobHeight);

				strncpy((char*)(*pB).TopFolder, (char*)pAnswer, MAX_FILENAME_SIZE);

				(*pB).PrgId = PrgId;
				(*pB).ObjId = ObjId;

				(*pB).OldFiles = 0;
				(*pB).Folders = 0;
				(*pB).OpenFolder = 0;
				(*pB).Files = 0;
				(*pB).ItemStart = 1;
				(*pB).ItemPointer = 1;
				(*pB).NeedUpdate = 1;
				Inst.UiInstance.UiUpdate = 1;
			}

			if (((*pB).PrgId == PrgId) && ((*pB).ObjId == ObjId))
			{
				//* CTRL *****************************************************************************************************


				if (Inst.UiInstance.UiUpdate != 0)
				{
					Inst.UiInstance.UiUpdate = 0;

					if ((*pB).hFiles != 0)
					{
						cMemoryCloseFolder((*pB).PrgId, &(*pB).hFiles);
					}
					if ((*pB).hFolders != 0)
					{
						cMemoryCloseFolder((*pB).PrgId, &(*pB).hFolders);
					}

				  (*pB).OpenFolder = 0;
					(*pB).Files = 0;
					*pType = 0;

					switch (Type)
					{
						case (sbyte)BrowserType.BROWSE_FOLDERS:
						case (sbyte)BrowserType.BROWSE_FOLDS_FILES:
							{
								if (cMemoryOpenFolder(PrgId, (sbyte)FileType.TYPE_FOLDER, (*pB).TopFolder, &(*pB).hFolders) == Result.OK)
								{
									//******************************************************************************************************
									if ((*pB).OpenFolder != 0)
									{
										cMemoryGetItem((*pB).PrgId, (*pB).hFolders, (*pB).OpenFolder, FOLDERNAME_SIZE + SUBFOLDERNAME_SIZE, (*pB).SubFolder, &TmpType);
										if (strcmp((char*)(*pB).SubFolder, SDCARD_FOLDER) == 0)
										{
											Item = (*pB).ItemPointer;
											cMemoryGetItemName((*pB).PrgId, (*pB).hFolders, Item, MAX_FILENAME_SIZE, (*pB).Filename, pType, &Priority);
											Result = cMemoryGetItem((*pB).PrgId, (*pB).hFolders, Item, FOLDERNAME_SIZE + SUBFOLDERNAME_SIZE, (*pB).FullPath, pType);
											*pType = (sbyte)FileType.TYPE_SDCARD;

											snprintf((char*)pAnswer, Lng, "%s", (char*)(*pB).FullPath);
										}
										else
										{
											if (strcmp((char*)(*pB).SubFolder, USBSTICK_FOLDER) == 0)
											{
												Item = (*pB).ItemPointer;
												cMemoryGetItemName((*pB).PrgId, (*pB).hFolders, Item, MAX_FILENAME_SIZE, (*pB).Filename, pType, &Priority);
												Result = cMemoryGetItem((*pB).PrgId, (*pB).hFolders, Item, FOLDERNAME_SIZE + SUBFOLDERNAME_SIZE, (*pB).FullPath, pType);
												*pType = (sbyte)FileType.TYPE_USBSTICK;

												snprintf((char*)pAnswer, Lng, "%s", (char*)(*pB).FullPath);
											}
											else
											{
												Result = cMemoryOpenFolder(PrgId, FILETYPE_UNKNOWN, (*pB).SubFolder, &(*pB).hFiles);
												Result = Result.BUSY;
											}
										}
									}
									//******************************************************************************************************
								}
								else
								{
									(*pB).PrgId = 0;
									(*pB).ObjId = 0;
								}
							}
							break;

						case (sbyte)BrowserType.BROWSE_CACHE:
							{
							}
							break;

						case (sbyte)BrowserType.BROWSE_FILES:
							{
								if (cMemoryOpenFolder(PrgId, (sbyte)FileType.FILETYPE_UNKNOWN, (*pB).TopFolder, &(*pB).hFiles) == Result.OK)
								{


								}
								else
								{
									(*pB).PrgId = 0;
									(*pB).ObjId = 0;
								}
							}
							break;

					}
				}

				if (strstr((char*)(*pB).SubFolder, SDCARD_FOLDER) != null)
				{
					(*pB).Sdcard = 1;
				}
				else
				{
					(*pB).Sdcard = 0;
				}

				if (strstr((char*)(*pB).SubFolder, USBSTICK_FOLDER) != null)
				{
					(*pB).Usbstick = 1;
				}
				else
				{
					(*pB).Usbstick = 0;
				}

				TmpResult = Result.OK;
				switch (Type)
				{
					case (sbyte)BrowserType.BROWSE_FOLDERS:
					case (sbyte)BrowserType.BROWSE_FOLDS_FILES:
						{
							// Collect folders in directory
							TmpResult = cMemoryGetFolderItems((*pB).PrgId, (*pB).hFolders, &(*pB).Folders);

							// Collect files in folder
							if (((*pB).OpenFolder != 0) && (TmpResult == Result.OK))
							{
								TmpResult = cMemoryGetFolderItems((*pB).PrgId, (*pB).hFiles, &(*pB).Files);
							}
						}
						break;

					case (sbyte)BrowserType.BROWSE_CACHE:
						{
							(*pB).Folders = (DATA16)cMemoryGetCacheFiles();
						}
						break;

					case (sbyte)BrowserType.BROWSE_FILES:
						{
							TmpResult = cMemoryGetFolderItems((*pB).PrgId, (*pB).hFiles, &(*pB).Files);
						}
						break;

				}

				if (((*pB).OpenFolder != 0) && ((*pB).OpenFolder == (*pB).ItemPointer))
				{
					if (cUiGetShortPress((sbyte)ButtonType.BACK_BUTTON) != 0)
					{
						// Close folder
						cMemoryCloseFolder((*pB).PrgId, &(*pB).hFiles);
						if ((*pB).ItemPointer > (*pB).OpenFolder)
						{
							(*pB).ItemPointer -= (*pB).Files;
						}
					  (*pB).OpenFolder = 0;
						(*pB).Files = 0;
					}
				}

				if ((*pB).Sdcard == 1)
				{
					if ((*pB).OpenFolder == 0)
					{
						if (cUiGetShortPress((sbyte)ButtonType.BACK_BUTTON) != 0)
						{
							// Collapse sdcard
							if ((*pB).hFiles != 0)
							{
								cMemoryCloseFolder((*pB).PrgId, &(*pB).hFiles);
							}
							if ((*pB).hFolders != 0)
							{
								cMemoryCloseFolder((*pB).PrgId, &(*pB).hFolders);
							}
							(*pB).PrgId = 0;
							(*pB).ObjId = 0;
							strcpy((char*)pAnswer, vmPRJS_DIR);
							*pType = 0;
							(*pB).SubFolder[0] = 0;
						}
					}
				}
				if ((*pB).Usbstick == 1)
				{
					if ((*pB).OpenFolder == 0)
					{
						if (cUiGetShortPress((sbyte)ButtonType.BACK_BUTTON) != 0)
						{
							// Collapse usbstick
							if ((*pB).hFiles != 0)
							{
								cMemoryCloseFolder((*pB).PrgId, &(*pB).hFiles);
							}
							if ((*pB).hFolders != 0)
							{
								cMemoryCloseFolder((*pB).PrgId, &(*pB).hFolders);
							}
							(*pB).PrgId = 0;
							(*pB).ObjId = 0;
							strcpy((char*)pAnswer, vmPRJS_DIR);
							*pType = 0;
							(*pB).SubFolder[0] = 0;
						}
					}
				}
				if ((*pB).OldFiles != ((*pB).Files + (*pB).Folders))
				{
					(*pB).OldFiles = (short)((*pB).Files + (*pB).Folders);
					(*pB).NeedUpdate = 1;
				}

				if (cUiGetShortPress((sbyte)ButtonType.ENTER_BUTTON) != 0)
				{
					(*pB).OldFiles = 0;
					if ((*pB).OpenFolder != 0)
					{
						if (((*pB).ItemPointer > (*pB).OpenFolder) && ((*pB).ItemPointer <= ((*pB).OpenFolder + (*pB).Files)))
						{ // File selected

							Item = (sbyte)((*pB).ItemPointer - (*pB).OpenFolder);
							Result = cMemoryGetItem((*pB).PrgId, (*pB).hFiles, Item, Lng, (*pB).FullPath, pType);

							snprintf((char*)pAnswer, Lng, "%s", (char*)(*pB).FullPath);
						}
						else
						{ // Folder selected

							if ((*pB).OpenFolder == (*pB).ItemPointer)
							{ // Enter on open folder

								Item = (*pB).OpenFolder;
								Result = cMemoryGetItem((*pB).PrgId, (*pB).hFolders, Item, Lng, pAnswer, pType);
							}
							else
							{ // Close folder
								cMemoryCloseFolder((*pB).PrgId, &(*pB).hFiles);
								if ((*pB).ItemPointer > (*pB).OpenFolder)
								{
									(*pB).ItemPointer -= (*pB).Files;
								}
							  (*pB).OpenFolder = 0;
								(*pB).Files = 0;
							}
						}
					}
					if ((*pB).OpenFolder == 0)
					{ // Open folder

						switch (Type)
						{
							case (sbyte)BrowserType.BROWSE_FOLDERS:
								{ // Folder

									Item = (*pB).ItemPointer;
									cMemoryGetItemName((*pB).PrgId, (*pB).hFolders, Item, MAX_FILENAME_SIZE, (*pB).Filename, pType, &Priority);
									Result = cMemoryGetItem((*pB).PrgId, (*pB).hFolders, Item, FOLDERNAME_SIZE + SUBFOLDERNAME_SIZE, (*pB).FullPath, pType);

									snprintf((char*)pAnswer, Lng, "%s/%s", (char*)(*pB).FullPath, (char*)(*pB).Filename);
									*pType = (sbyte)FileType.TYPE_BYTECODE;
								}
								break;

							case (sbyte)BrowserType.BROWSE_FOLDS_FILES:
								{ // Folder & File

									(*pB).OpenFolder = (*pB).ItemPointer;
									cMemoryGetItem((*pB).PrgId, (*pB).hFolders, (*pB).OpenFolder, FOLDERNAME_SIZE + SUBFOLDERNAME_SIZE, (*pB).SubFolder, &TmpType);
									if (strcmp((char*)(*pB).SubFolder, SDCARD_FOLDER) == 0)
									{
										Item = (*pB).ItemPointer;
										cMemoryGetItemName((*pB).PrgId, (*pB).hFolders, Item, MAX_FILENAME_SIZE, (*pB).Filename, pType, &Priority);
										Result = cMemoryGetItem((*pB).PrgId, (*pB).hFolders, Item, FOLDERNAME_SIZE + SUBFOLDERNAME_SIZE, (*pB).FullPath, pType);
										*pType = (sbyte)FileType.TYPE_SDCARD;

										snprintf((char*)pAnswer, Lng, "%s", (char*)(*pB).FullPath);
									}
									else
									{
										if (strcmp((char*)(*pB).SubFolder, USBSTICK_FOLDER) == 0)
										{
											Item = (*pB).ItemPointer;
											cMemoryGetItemName((*pB).PrgId, (*pB).hFolders, Item, MAX_FILENAME_SIZE, (*pB).Filename, pType, &Priority);
											Result = cMemoryGetItem((*pB).PrgId, (*pB).hFolders, Item, FOLDERNAME_SIZE + SUBFOLDERNAME_SIZE, (*pB).FullPath, pType);
											*pType = (sbyte)FileType.TYPE_USBSTICK;

											snprintf((char*)pAnswer, Lng, "%s", (char*)(*pB).FullPath);
										}
										else
										{
											(*pB).ItemStart = (*pB).ItemPointer;
											Result = cMemoryOpenFolder(PrgId, FILETYPE_UNKNOWN, (*pB).SubFolder, &(*pB).hFiles);

											Result = Result.BUSY;
										}
									}
								}
								break;
							case (sbyte)BrowserType.BROWSE_CACHE:
								{ // Cache

									Item = (*pB).ItemPointer;

									*pType = cMemoryGetCacheName(Item, FOLDERNAME_SIZE + SUBFOLDERNAME_SIZE, (char*)(*pB).FullPath, (char*)(*pB).Filename);
									snprintf((char*)pAnswer, Lng, "%s", (char*)(*pB).FullPath);
									Result = Result.OK;
								}
								break;
							case (sbyte)BrowserType.BROWSE_FILES:
								{ // File

									if (((*pB).ItemPointer > (*pB).OpenFolder) && ((*pB).ItemPointer <= ((*pB).OpenFolder + (*pB).Files)))
									{ // File selected

										Item = (short)((*pB).ItemPointer - (*pB).OpenFolder);

										Result = cMemoryGetItem((*pB).PrgId, (*pB).hFiles, Item, Lng, (*pB).FullPath, pType);

										snprintf((char*)pAnswer, Lng, "%s", (char*)(*pB).FullPath);
										Result = Result.OK;
									}
								}
								break;

						}
					}
				  (*pB).NeedUpdate = 1;
				}

				TotalItems = (short)((*pB).Folders + (*pB).Files);
				if (TmpResult == Result.OK)
				{
					if (TotalItems != 0)
					{
						if ((*pB).ItemPointer > TotalItems)
						{

							(*pB).ItemPointer = TotalItems;
							(*pB).NeedUpdate = 1;
						}
						if ((*pB).ItemStart > (*pB).ItemPointer)
						{
							(*pB).ItemStart = (*pB).ItemPointer;
							(*pB).NeedUpdate = 1;
						}
					}
					else
					{
						(*pB).ItemStart = 1;
						(*pB).ItemPointer = 1;
					}
				}

				Tmp = cUiGetVert();
				if (Tmp != 0)
				{ // up/down arrow pressed

					(*pB).NeedUpdate = 1;

					// Calculate item pointer
					(*pB).ItemPointer += Tmp;
					if ((*pB).ItemPointer < 1)
					{
						(*pB).ItemPointer = 1;
						(*pB).NeedUpdate = 0;
					}
					if ((*pB).ItemPointer > TotalItems)
					{
						(*pB).ItemPointer = TotalItems;
						(*pB).NeedUpdate = 0;
					}
				}

				// Calculate item start
				if ((*pB).ItemPointer < (*pB).ItemStart)
				{
					if ((*pB).ItemPointer > 0)
					{
						(*pB).ItemStart = (*pB).ItemPointer;
					}
				}
				if ((*pB).ItemPointer >= ((*pB).ItemStart + (*pB).Lines))
				{
					(*pB).ItemStart = (short)((*pB).ItemPointer - (*pB).Lines);
					(*pB).ItemStart++;
				}

				if ((*pB).NeedUpdate != 0)
				{
					//* UPDATE ***************************************************************************************************
					(*pB).NeedUpdate = 0;

					// clear screen
					dLcdFillRect((*Inst.UiInstance.pLcd).Lcd, BG_COLOR, (*pB).ScreenStartX, (*pB).ScreenStartY, (*pB).ScreenWidth, (*pB).ScreenHeight);

					OldPriority = 0;
					for (Tmp = 0; Tmp < (*pB).Lines; Tmp++)
					{
						Item = (short)(Tmp + (*pB).ItemStart);
						Folder = 1;
						Priority = OldPriority;

						if (Item <= TotalItems)
						{
							if ((*pB).OpenFolder != 0)
							{
								if ((Item > (*pB).OpenFolder) && (Item <= ((*pB).OpenFolder + (*pB).Files)))
								{
									Item -= (*pB).OpenFolder;
									Folder = 0;
								}
								else
								{
									if (Item > (*pB).OpenFolder)
									{
										Item -= (*pB).Files;
									}
								}
							}
							//*** Graphics ***********************************************************************************************

							if (Folder != 0)
							{ // Show folder

								switch (Type)
								{
									case (sbyte)BrowserType.BROWSE_FOLDERS:
										{
											cMemoryGetItemName((*pB).PrgId, (*pB).hFolders, Item, (*pB).Chars, (*pB).Filename, &TmpType, &Priority);
											if (cMemoryGetItemIcon((*pB).PrgId, (*pB).hFolders, Item, &TmpHandle, &Image) == Result.OK)
											{
												dLcdDrawBitmap((*Inst.UiInstance.pLcd).Lcd, Color, (*pB).IconStartX, (*pB).IconStartY + (Tmp * (*pB).LineHeight), (IP)Image);
												cMemoryCloseFile((*pB).PrgId, TmpHandle);
											}
											else
											{
												dLcdDrawPicture((*Inst.UiInstance.pLcd).Lcd, Color, (*pB).IconStartX, (*pB).IconStartY + (Tmp * (*pB).LineHeight), PCApp_width, PCApp_height, (UBYTE*)PCApp_bits);
											}

									  (*pB).Text[0] = 0;
											if (strcmp((char*)(*pB).Filename, "Bluetooth") == 0)
											{
												if (Inst.UiInstance.BtOn != 0)
												{
													(*pB).Text[0] = (sbyte)'+';
												}
												else
												{
													(*pB).Text[0] = (sbyte)'-';
												}
											}
											else
											{
												if (strcmp((char*)(*pB).Filename, "WiFi") == 0)
												{
													if (Inst.UiInstance.WiFiOn != 0)
													{
														(*pB).Text[0] = (sbyte)'+';
													}
													else
													{
														(*pB).Text[0] = (sbyte)'-';
													}
												}
												else
												{
													if (cMemoryGetItemText((*pB).PrgId, (*pB).hFolders, Item, (*pB).Chars, (*pB).Text) != Result.OK)
													{
														(*pB).Text[0] = 0;
													}
												}
											}
											switch ((*pB).Text[0])
											{
												case 0:
													{
													}
													break;

												case (sbyte)'+':
													{
														Indent = ((*pB).Chars - 1) * (*pB).CharWidth - dLcdGetIconWidth(IconType.MENU_ICON);
														dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, Color, (*pB).TextStartX + Indent, ((*pB).TextStartY - 2) + (Tmp * (*pB).LineHeight), MENU_ICON, ICON_CHECKED);
													}
													break;

												case (sbyte)'-':
													{
														Indent = ((*pB).Chars - 1) * (*pB).CharWidth - dLcdGetIconWidth(IconType.MENU_ICON);
														dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, Color, (*pB).TextStartX + Indent, ((*pB).TextStartY - 2) + (Tmp * (*pB).LineHeight), MENU_ICON, ICON_CHECKBOX);
													}
													break;

												default:
													{
														Indent = (((*pB).Chars - 1) - (DATA16)strlen((char*)(*pB).Text)) * (*pB).CharWidth;
														dLcdDrawText((*Inst.UiInstance.pLcd).Lcd, Color, (*pB).TextStartX + Indent, (*pB).TextStartY + (Tmp * (*pB).LineHeight), NORMAL_FONT, (*pB).Text);
													}
													break;

											}

										}
										break;

									case (sbyte)BrowserType.BROWSE_FOLDS_FILES:
										{
											cMemoryGetItemName((*pB).PrgId, (*pB).hFolders, Item, (*pB).Chars, (*pB).Filename, &TmpType, &Priority);
											dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, Color, (*pB).IconStartX, (*pB).IconStartY + (Tmp * (*pB).LineHeight), IconType.NORMAL_ICON, FiletypeToNormalIcon[TmpType]);

											if ((Priority == 1) || (Priority == 2))
											{
												dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, Color, (*pB).IconStartX, (*pB).IconStartY + (Tmp * (*pB).LineHeight), IconType.NORMAL_ICON, ICON_FOLDER2);
											}
											else
											{
												if (Priority == 3)
												{
													dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, Color, (*pB).IconStartX, (*pB).IconStartY + (Tmp * (*pB).LineHeight), IconType.NORMAL_ICON, ICON_SD);
												}
												else
												{
													if (Priority == 4)
													{
														dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, Color, (*pB).IconStartX, (*pB).IconStartY + (Tmp * (*pB).LineHeight), IconType.NORMAL_ICON, ICON_USB);
													}
													else
													{
														dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, Color, (*pB).IconStartX, (*pB).IconStartY + (Tmp * (*pB).LineHeight), IconType.NORMAL_ICON, FiletypeToNormalIcon[TmpType]);
													}
												}
											}
											if (Priority != OldPriority)
											{
												if ((Priority == 1) || (Priority >= 3))
												{
													if (Tmp != 0)
													{
														dLcdDrawDotLine((*Inst.UiInstance.pLcd).Lcd, Color, (*pB).SelectStartX, (*pB).SelectStartY + ((Tmp - 1) * (*pB).LineHeight) + (*pB).LineHeight - 2, (*pB).SelectStartX + (*pB).SelectWidth, (*pB).SelectStartY + ((Tmp - 1) * (*pB).LineHeight) + (*pB).LineHeight - 2, 1, 2);
													}
												}
											}
										}
										break;

									case (sbyte)BrowserType.BROWSE_CACHE:
										{
											TmpType = cMemoryGetCacheName(Item, (*pB).Chars, (char*)(*pB).FullPath, (char*)(*pB).Filename);
											dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, Color, (*pB).IconStartX, (*pB).IconStartY + (Tmp * (*pB).LineHeight), IconType.NORMAL_ICON, FiletypeToNormalIcon[TmpType]);
										}
										break;

									case (sbyte)BrowserType.BROWSE_FILES:
										{
											cMemoryGetItemName((*pB).PrgId, (*pB).hFiles, Item, (*pB).Chars, (*pB).Filename, &TmpType, &Priority);
											dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, Color, (*pB).IconStartX, (*pB).IconStartY + (Tmp * (*pB).LineHeight), IconType.NORMAL_ICON, FiletypeToNormalIcon[TmpType]);
										}
										break;

								}
								// Draw folder name
								dLcdDrawText((*Inst.UiInstance.pLcd).Lcd, Color, (*pB).TextStartX, (*pB).TextStartY + (Tmp * (*pB).LineHeight), FontType.NORMAL_FONT, (*pB).Filename);

								// Draw open folder
								if (Item == (*pB).OpenFolder)
								{
									dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, Color, 144, (*pB).IconStartY + (Tmp * (*pB).LineHeight), IconType.NORMAL_ICON, ICON_OPENFOLDER);
								}

								// Draw selection
								if ((*pB).ItemPointer == (Tmp + (*pB).ItemStart))
								{
									dLcdInverseRect((*Inst.UiInstance.pLcd).Lcd, (*pB).SelectStartX, (*pB).SelectStartY + (Tmp * (*pB).LineHeight), (*pB).SelectWidth + 1, (*pB).SelectHeight);
								}

								// Draw end line
								switch (Type)
								{
									case (sbyte)BrowserType.BROWSE_FOLDERS:
									case (sbyte)BrowserType.BROWSE_FOLDS_FILES:
									case (sbyte)BrowserType.BROWSE_FILES:
										{
											if (((Tmp + (*pB).ItemStart) == TotalItems) && (Tmp < ((*pB).Lines - 1)))
											{
												dLcdDrawDotLine((*Inst.UiInstance.pLcd).Lcd, Color, (*pB).SelectStartX, (*pB).SelectStartY + (Tmp * (*pB).LineHeight) + (*pB).LineHeight - 2, (*pB).SelectStartX + (*pB).SelectWidth, (*pB).SelectStartY + (Tmp * (*pB).LineHeight) + (*pB).LineHeight - 2, 1, 2);
											}
										}
										break;

									case (sbyte)BrowserType.BROWSE_CACHE:
										{
											if (((Tmp + (*pB).ItemStart) == 1) && (Tmp < ((*pB).Lines - 1)))
											{
												dLcdDrawDotLine((*Inst.UiInstance.pLcd).Lcd, Color, (*pB).SelectStartX, (*pB).SelectStartY + (Tmp * (*pB).LineHeight) + (*pB).LineHeight - 2, (*pB).SelectStartX + (*pB).SelectWidth, (*pB).SelectStartY + (Tmp * (*pB).LineHeight) + (*pB).LineHeight - 2, 1, 2);
											}
											if (((Tmp + (*pB).ItemStart) == TotalItems) && (Tmp < ((*pB).Lines - 1)))
											{
												dLcdDrawDotLine((*Inst.UiInstance.pLcd).Lcd, Color, (*pB).SelectStartX, (*pB).SelectStartY + (Tmp * (*pB).LineHeight) + (*pB).LineHeight - 2, (*pB).SelectStartX + (*pB).SelectWidth, (*pB).SelectStartY + (Tmp * (*pB).LineHeight) + (*pB).LineHeight - 2, 1, 2);
											}
										}
										break;

								}
							}
							else
							{ // Show file

								// Get file name and type
								cMemoryGetItemName((*pB).PrgId, (*pB).hFiles, Item, (*pB).Chars - 1, (*pB).Filename, &TmpType, &Priority);

								// show File icons
								dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, Color, (*pB).IconStartX + (*pB).CharWidth, (*pB).IconStartY + (Tmp * (*pB).LineHeight), IconType.NORMAL_ICON, FiletypeToNormalIcon[TmpType]);

								// Draw file name
								dLcdDrawText((*Inst.UiInstance.pLcd).Lcd, Color, (*pB).TextStartX + (*pB).CharWidth, (*pB).TextStartY + (Tmp * (*pB).LineHeight), FontType.NORMAL_FONT, (*pB).Filename);

								// Draw folder line
								if ((Tmp == ((*pB).Lines - 1)) || (Item == (*pB).Files))
								{
									dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, Color, (*pB).IconStartX + (*pB).CharWidth - 3, (*pB).SelectStartY + (Tmp * (*pB).LineHeight), (*pB).IconStartX + (*pB).CharWidth - 3, (*pB).SelectStartY + (Tmp * (*pB).LineHeight) + (*pB).SelectHeight - 1);
								}
								else
								{
									dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, Color, (*pB).IconStartX + (*pB).CharWidth - 3, (*pB).SelectStartY + (Tmp * (*pB).LineHeight), (*pB).IconStartX + (*pB).CharWidth - 3, (*pB).SelectStartY + (Tmp * (*pB).LineHeight) + (*pB).LineHeight - 1);
								}

								// Draw selection
								if ((*pB).ItemPointer == (Tmp + (*pB).ItemStart))
								{
									dLcdInverseRect((*Inst.UiInstance.pLcd).Lcd, (*pB).SelectStartX + (*pB).CharWidth, (*pB).SelectStartY + (Tmp * (*pB).LineHeight), (*pB).SelectWidth + 1 - (*pB).CharWidth, (*pB).SelectHeight);
								}
							}

							//************************************************************************************************************
						}
						OldPriority = Priority;
					}

					cUiDrawBar(1, (*pB).ScrollStartX, (*pB).ScrollStartY, (*pB).ScrollWidth, (*pB).ScrollHeight, 0, TotalItems, (*pB).ItemPointer);

					// Update
					cUiUpdateLcd();
					Inst.UiInstance.ScreenBusy = 0;
				}

				if (Result != Result.OK)
				{
					Tmp = cUiTestHorz();
					if (Ignore == Tmp)
					{
						Tmp = cUiGetHorz();
						Tmp = 0;
					}

					if ((Tmp != 0) || (cUiTestShortPress((sbyte)ButtonType.BACK_BUTTON) != 0) || (cUiTestLongPress((sbyte)ButtonType.BACK_BUTTON) != 0))
					{
						if (Type != (sbyte)BrowserType.BROWSE_CACHE)
						{
							if ((*pB).OpenFolder != 0)
							{
								if ((*pB).hFiles != 0)
								{
									cMemoryCloseFolder((*pB).PrgId, &(*pB).hFiles);
								}
							}
							if ((*pB).hFolders != 0)
							{
								cMemoryCloseFolder((*pB).PrgId, &(*pB).hFolders);
							}
						}
						(*pB).PrgId = 0;
						(*pB).ObjId = 0;
						(*pB).SubFolder[0] = 0;
						pAnswer[0] = 0;
						*pType = 0;
						Result = Result.OK;
					}
				}
				else
				{
					(*pB).NeedUpdate = 1;
				}
			}
			else
			{
				pAnswer[0] = 0;
				*pType = TYPE_RESTART_BROWSER;
				Result = Result.FAIL;
			}

			if (*pType > 0)
			{
				if ((*pB).Sdcard != 0)
				{
					*pType |= TYPE_SDCARD;
				}
				if ((*pB).Usbstick != 0)
				{
					*pType |= TYPE_USBSTICK;
				}
			}

			if (Result != Result.BUSY)
			{
				//* EXIT *****************************************************************************************************

			}

			return (Result);
		}

		DATA16 cUiTextboxGetLines(DATA8* pText, DATA32 Size, DATA8 Del)
		{
			DATA32 Point = 0;
			DATA16 Lines = 0;
			DATA8 DelPoi;

			if (Del < (sbyte)Delimeter.DELS)
			{
				while (pText[Point] != 0 && (Point < Size))
				{
					DelPoi = 0;
					while ((pText[Point] != 0) && (Point < Size) && (Delimiter[Del][DelPoi]) && (pText[Point] == Delimiter[Del][DelPoi]))
					{
						DelPoi++;
						Point++;
					}
					if (Delimiter[Del][DelPoi] == 0)
					{
						Lines++;
					}
					else
					{
						if ((pText[Point] != 0) && (Point < Size))
						{
							Point++;
						}
					}
				}
			}

			return (Lines);
		}


		void cUiTextboxAppendLine(DATA8* pText, DATA32 Size, DATA8 Del, DATA8* pLine, DATA8 Font)
		{
			DATA32 Point = 0;
			DATA8 DelPoi = 0;

			if (Del < (sbyte)Delimeter.DELS)
			{
				while ((pText[Point] != 0) && (Point < Size))
				{
					Point++;
				}
				if ((Point < Size) && (Font != 0))
				{
					pText[Point] = Font;
					Point++;
				}

				while ((Point < Size) && (*pLine != 0))
				{
					pText[Point] = *pLine;
					Point++;
					pLine++;
				}
				while ((Point < Size) && (Delimiter[Del][DelPoi]))
				{
					pText[Point] = Delimiter[Del][DelPoi];
					Point++;
					DelPoi++;
				}
			}
		}


		DATA32 cUiTextboxFindLine(DATA8* pText, DATA32 Size, DATA8 Del, DATA16 Line, DATA8* pFont)
		{
			DATA32 Result = -1;
			DATA32 Point = 0;
			DATA8 DelPoi = 0;

			*pFont = 0;
			if (Del < (sbyte)Delimeter.DELS)
			{
				Result = Point;
				while ((Line != 0) && (pText[Point] != 0) && (Point < Size))
				{

					DelPoi = 0;
					while ((pText[Point] != 0) && (Point < Size) && (Delimiter[Del][DelPoi]) && (pText[Point] == Delimiter[Del][DelPoi]))
					{
						DelPoi++;
						Point++;
					}
					if (Delimiter[Del][DelPoi] == 0)
					{
						Line--;
						if (Line != 0)
						{
							Result = Point;
						}
					}
					else
					{
						if ((pText[Point] != 0) && (Point < Size))
						{
							Point++;
						}
					}
				}
				if (Line != 0)
				{
					Result = -1;
				}
				if (Result >= 0)
				{
					if ((pText[Result] > 0) && (pText[Result] < (sbyte)FontType.FONTTYPES))
					{
						*pFont = pText[Result];
						Result++;
					}
				}
			}

			return (Result);
		}

		void cUiTextboxReadLine(DATA8* pText, DATA32 Size, DATA8 Del, DATA8 Lng, DATA16 Line, DATA8* pLine, DATA8* pFont)
		{
			DATA32 Start;
			DATA32 Point = 0;
			DATA8 DelPoi = 0;
			DATA8 Run = 1;

			Start = cUiTextboxFindLine(pText, Size, Del, Line, pFont);
			Point = Start;

			pLine[0] = 0;

			if (Point >= 0)
			{
				while ((Run != 0) && (pText[Point] != 0) && (Point < Size))
				{
					DelPoi = 0;
					while ((pText[Point] != 0) && (Point < Size) && (Delimiter[Del][DelPoi]) && (pText[Point] == Delimiter[Del][DelPoi]))
					{
						DelPoi++;
						Point++;
					}
					if (Delimiter[Del][DelPoi] == 0)
					{
						Run = 0;
					}
					else
					{
						if ((pText[Point] != 0) && (Point < Size))
						{
							Point++;
						}
					}
				}
				Point -= (DATA32)DelPoi;

				if (((Point - Start) + 1) < (DATA32)Lng)
				{
					Lng = (DATA8)((Point - Start) + 1);
				}
				snprintf((char*)pLine, Lng, "%s", (char*)&pText[Start]);
			}
		}


		Result cUiTextbox(DATA16 X, DATA16 Y, DATA16 X1, DATA16 Y1, DATA8* pText, DATA32 Size, DATA8 Del, DATA16* pLine)
		{
			Result Result = Result.BUSY;
			TXTBOX* pB;
			DATA16 Item;
			DATA16 TotalItems;
			DATA16 Tmp;
			DATA16 Ypos;
			DATA8 Color;

			pB = &Inst.UiInstance.Txtbox;
			Color = (sbyte)FG_COLOR;

			if (*pLine < 0)
			{
				//* INIT *****************************************************************************************************
				// Define screen
				(*pB).ScreenStartX = X;
				(*pB).ScreenStartY = Y;
				(*pB).ScreenWidth = X1;
				(*pB).ScreenHeight = Y1;

				(*pB).Font = Inst.UiInstance.Font;

				// calculate chars and lines on screen
				(*pB).CharWidth = dLcdGetFontWidth((*pB).Font);
				(*pB).CharHeight = dLcdGetFontHeight((*pB).Font);
				(*pB).Chars = ((short)((*pB).ScreenWidth / (*pB).CharWidth));

				// calculate lines on screen
				(*pB).LineSpace = 5;
				(*pB).LineHeight = (short)((*pB).CharHeight + (*pB).LineSpace);
				(*pB).Lines = (short)((*pB).ScreenHeight / (*pB).LineHeight);

				// calculate start of text
				(*pB).TextStartX = cUiAlignX((*pB).ScreenStartX);
				(*pB).TextStartY = (short)((*pB).ScreenStartY + ((*pB).LineHeight - (*pB).CharHeight) / 2);

				// Calculate selection barBrowser
				(*pB).SelectStartX = (*pB).ScreenStartX;
				(*pB).SelectWidth = (short)((*pB).ScreenWidth - ((*pB).CharWidth + 5));
				(*pB).SelectStartY = (short)((*pB).TextStartY - 1);
				(*pB).SelectHeight = (short)((*pB).CharHeight + 1);

				// Calculate scroll bar
				(*pB).ScrollWidth = 5;
				(*pB).NobHeight = 7;
				(*pB).ScrollStartX = (short)((*pB).ScreenStartX + (*pB).ScreenWidth - (*pB).ScrollWidth);
				(*pB).ScrollStartY = (short)((*pB).ScreenStartY + 1);
				(*pB).ScrollHeight = (short)((*pB).ScreenHeight - 2);
				(*pB).ScrollSpan = (short)((*pB).ScrollHeight - (*pB).NobHeight);

				(*pB).Items = cUiTextboxGetLines(pText, Size, Del);
				(*pB).ItemStart = 1;
				(*pB).ItemPointer = 1;

				(*pB).NeedUpdate = 1;

				*pLine = 0;
			}

			TotalItems = (*pB).Items;

			Tmp = cUiGetVert();
			if (Tmp != 0)
			{ // up/down arrow pressed

				(*pB).NeedUpdate = 1;

				// Calculate item pointer
				(*pB).ItemPointer += Tmp;
				if ((*pB).ItemPointer < 1)
				{
					(*pB).ItemPointer = 1;
					(*pB).NeedUpdate = 0;
				}
				if ((*pB).ItemPointer > TotalItems)
				{
					(*pB).ItemPointer = TotalItems;
					(*pB).NeedUpdate = 0;
				}
			}

			// Calculate item start
			if ((*pB).ItemPointer < (*pB).ItemStart)
			{
				if ((*pB).ItemPointer > 0)
				{
					(*pB).ItemStart = (*pB).ItemPointer;
				}
			}
			if ((*pB).ItemPointer >= ((*pB).ItemStart + (*pB).Lines))
			{
				(*pB).ItemStart = (short)((*pB).ItemPointer - (*pB).Lines);
				(*pB).ItemStart++;
			}

			if (cUiGetShortPress((sbyte)ButtonType.ENTER_BUTTON) != 0)
			{
				*pLine = (*pB).ItemPointer;

				Result = Result.OK;
			}
			if (cUiGetShortPress((sbyte)ButtonType.BACK_BUTTON) != 0)
			{
				*pLine = -1;

				Result = Result.OK;
			}


			if ((*pB).NeedUpdate != 0)
			{
				//* UPDATE ***************************************************************************************************
				(*pB).NeedUpdate = 0;

				// clear screen
				dLcdFillRect((*Inst.UiInstance.pLcd).Lcd, BG_COLOR, (*pB).ScreenStartX, (*pB).ScreenStartY, (*pB).ScreenWidth, (*pB).ScreenHeight);

				Ypos = (sbyte)((*pB).TextStartY + 2);

				for (Tmp = 0; Tmp < (*pB).Lines; Tmp++)
				{
					Item = (sbyte)(Tmp + (*pB).ItemStart);

					if (Item <= TotalItems)
					{
						cUiTextboxReadLine(pText, Size, Del, TEXTSIZE, Item, (*pB).Text, &(*pB).Font);

						// calculate chars and lines on screen
						(*pB).CharWidth = dLcdGetFontWidth((*pB).Font);
						(*pB).CharHeight = dLcdGetFontHeight((*pB).Font);

						// calculate lines on screen
						(*pB).LineSpace = 2;
						(*pB).LineHeight = (short)((*pB).CharHeight + (*pB).LineSpace);
						(*pB).Lines = (short)((*pB).ScreenHeight / (*pB).LineHeight);

						// Calculate selection barBrowser
						(*pB).SelectStartX = (*pB).ScreenStartX;
						(*pB).SelectWidth = (short)((*pB).ScreenWidth - ((*pB).ScrollWidth + 2));
						(*pB).SelectStartY = (short)((*pB).TextStartY - 1);
						(*pB).SelectHeight = (short)((*pB).CharHeight + 1);

						(*pB).Chars = (short)((*pB).SelectWidth / (*pB).CharWidth);

						(*pB).Text[(*pB).Chars] = 0;

						if ((Ypos + (*pB).LineHeight) <= ((*pB).ScreenStartY + (*pB).ScreenHeight))
						{
							dLcdDrawText((*Inst.UiInstance.pLcd).Lcd, Color, (*pB).TextStartX, Ypos, (*pB).Font, (*pB).Text);
						}
						else
						{
							Tmp = (*pB).Lines;
						}
					}

					cUiDrawBar(1, (*pB).ScrollStartX, (*pB).ScrollStartY, (*pB).ScrollWidth, (*pB).ScrollHeight, 0, TotalItems, (*pB).ItemPointer);

					if ((Ypos + (*pB).LineHeight) <= ((*pB).ScreenStartY + (*pB).ScreenHeight))
					{
						// Draw selection
						if ((*pB).ItemPointer == (Tmp + (*pB).ItemStart))
						{
							dLcdInverseRect((*Inst.UiInstance.pLcd).Lcd, (*pB).SelectStartX, Ypos - 1, (*pB).SelectWidth, (*pB).LineHeight);
						}
					}
					Ypos += (*pB).LineHeight;
				}

				// Update
				cUiUpdateLcd();
				Inst.UiInstance.ScreenBusy = 0;
			}

			return (Result);
		}


		void cUiGraphSetup(DATA16 StartX, DATA16 SizeX, DATA8 Items, DATA16* pOffset, DATA16* pSpan, DATAF* pMin, DATAF* pMax, DATAF* pVal)
		{
			DATA16 Item;
			DATA16 Pointer;

			Inst.UiInstance.Graph.Initialized = 0;

			Inst.UiInstance.Graph.pOffset = pOffset;
			Inst.UiInstance.Graph.pSpan = pSpan;
			Inst.UiInstance.Graph.pMin = pMin;
			Inst.UiInstance.Graph.pMax = pMax;
			Inst.UiInstance.Graph.pVal = pVal;

			if (Items < 0)
			{
				Items = 0;
			}
			if (Items > GRAPH_BUFFERS)
			{
				Items = GRAPH_BUFFERS;
			}


			Inst.UiInstance.Graph.GraphStartX = StartX;
			Inst.UiInstance.Graph.GraphSizeX = SizeX;
			Inst.UiInstance.Graph.Items = Items;
			Inst.UiInstance.Graph.Pointer = 0;

			for (Item = 0; Item < Inst.UiInstance.Graph.Items; Item++)
			{
				for (Pointer = 0; Pointer < Inst.UiInstance.Graph.GraphSizeX; Pointer++)
				{
					Inst.UiInstance.Graph.Buffer[Item][Pointer] = DATAF_NAN;
				}
			}

			Inst.UiInstance.Graph.Initialized = 1;

			// Simulate graph
			Inst.UiInstance.Graph.Value = Inst.UiInstance.Graph.pMin[0];
			Inst.UiInstance.Graph.Down = 0;
			Inst.UiInstance.Graph.Inc = (Inst.UiInstance.Graph.pMax[0] - Inst.UiInstance.Graph.pMin[0]) / (DATAF)20;
		}


		void cUiGraphSample()
		{
			DATAF Sample;
			DATA16 Item;
			DATA16 Pointer;

			if (Inst.UiInstance.Graph.Initialized != 0)
			{ // Only if initialized

				if (Inst.UiInstance.Graph.Pointer < Inst.UiInstance.Graph.GraphSizeX)
				{
					for (Item = 0; Item < (Inst.UiInstance.Graph.Items); Item++)
					{
						// Insert sample
						Sample = Inst.UiInstance.Graph.pVal[Item];

						if (!(float.IsNaN(Sample)))
						{
							Inst.UiInstance.Graph.Buffer[Item][Inst.UiInstance.Graph.Pointer] = Sample;
						}
						else
						{
							Inst.UiInstance.Graph.Buffer[Item][Inst.UiInstance.Graph.Pointer] = DATAF_NAN;
						}
					}
					Inst.UiInstance.Graph.Pointer++;
				}
				else
				{
					// Scroll buffers
					for (Item = 0; Item < (Inst.UiInstance.Graph.Items); Item++)
					{
						for (Pointer = 0; Pointer < (Inst.UiInstance.Graph.GraphSizeX - 1); Pointer++)
						{
							Inst.UiInstance.Graph.Buffer[Item][Pointer] = Inst.UiInstance.Graph.Buffer[Item][Pointer + 1];
						}

						// Insert sample
						Sample = Inst.UiInstance.Graph.pVal[Item];

						if (!(float.IsNaN(Sample)))
						{
							Inst.UiInstance.Graph.Buffer[Item][Pointer] = Sample;
						}
						else
						{
							Inst.UiInstance.Graph.Buffer[Item][Pointer] = DATAF_NAN;
						}
					}
				}
			}
		}


		void cUiGraphDraw(DATA8 View, DATAF* pActual, DATAF* pLowest, DATAF* pHighest, DATAF* pAverage)
		{
			DATAF Sample;
			DATA8 Samples;
			DATA16 Value;
			DATA16 Item;
			DATA16 Pointer;
			DATA16 X;
			DATA16 Y1;
			DATA16 Y2;
			DATA8 Color = 1;

			*pActual = DATAF_NAN;
			*pLowest = DATAF_NAN;
			*pHighest = DATAF_NAN;
			*pAverage = DATAF_NAN;
			Samples = 0;

			if (Inst.UiInstance.Graph.Initialized != 0)
			{ // Only if initialized

				if (Inst.UiInstance.ScreenBlocked == 0)
				{

					// View or all
					if ((View >= 0) && (View < Inst.UiInstance.Graph.Items))
					{
						Item = View;

						Y1 = (short)(Inst.UiInstance.Graph.pOffset[Item] + Inst.UiInstance.Graph.pSpan[Item]);

						// Draw buffers
						X = Inst.UiInstance.Graph.GraphStartX;
						for (Pointer = 0; Pointer < Inst.UiInstance.Graph.Pointer; Pointer++)
						{
							Sample = Inst.UiInstance.Graph.Buffer[Item][Pointer];
							if (!(float.IsNaN(Sample)))
							{
								*pActual = Sample;
								if (float.IsNaN(*pAverage))
								{
									*pAverage = (DATAF)0;
									*pLowest = *pActual;
									*pHighest = *pActual;
								}
								else
								{
									if (*pActual < *pLowest)
									{
										*pLowest = *pActual;
									}
									if (*pActual > *pHighest)
									{
										*pHighest = *pActual;
									}
								}
								*pAverage += *pActual;
								Samples++;

								// Scale Y axis
								Value = (DATA16)((((Sample - Inst.UiInstance.Graph.pMin[Item]) * (DATAF)Inst.UiInstance.Graph.pSpan[Item]) / (Inst.UiInstance.Graph.pMax[Item] - Inst.UiInstance.Graph.pMin[Item])));

								// Limit Y axis
								if (Value > Inst.UiInstance.Graph.pSpan[Item])
								{
									Value = Inst.UiInstance.Graph.pSpan[Item];
								}
								if (Value < 0)
								{
									Value = 0;
								}
								/*
											printf("S=%-3d V=%3.0f L=%3.0f H=%3.0f A=%3.0f v=%3.0f ^=%3.0f O=%3d S=%3d Y=%d\r\n",Samples,*pActual,*pLowest,*pHighest,*pAverage,Inst.UiInstance.Graph.pMin[Item],Inst.UiInstance.Graph.pMax[Item],Inst.UiInstance.Graph.pOffset[Item],Inst.UiInstance.Graph.pSpan[Item],Value);
								*/
								Y2 = (short)((Inst.UiInstance.Graph.pOffset[Item] + Inst.UiInstance.Graph.pSpan[Item]) - Value);
								if (Pointer > 1)
								{
									if (Y2 > Y1)
									{
										dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, Color, X - 2, Y1 - 1, X - 1, Y2 - 1);
										dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, Color, X, Y1 + 1, X + 1, Y2 + 1);
									}
									else
									{
										if (Y2 < Y1)
										{
											dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, Color, X, Y1 - 1, X + 1, Y2 - 1);
											dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, Color, X - 2, Y1 + 1, X - 1, Y2 + 1);
										}
										else
										{
											dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, Color, X - 1, Y1 - 1, X, Y2 - 1);
											dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, Color, X - 1, Y1 + 1, X, Y2 + 1);
										}
									}
									dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, Color, X - 1, Y1, X, Y2);
								}
								else
								{
									dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X, Y2);
								}

								Y1 = Y2;
							}
							X++;
						}
						if (Samples != 0)
						{
							*pAverage = *pAverage / (DATAF)Samples;
						}

					}
					else
					{
						// Draw buffers
						for (Item = 0; Item < Inst.UiInstance.Graph.Items; Item++)
						{
							Y1 = (short)(Inst.UiInstance.Graph.pOffset[Item] + Inst.UiInstance.Graph.pSpan[Item]);

							X = (short)(Inst.UiInstance.Graph.GraphStartX + 1);
							for (Pointer = 0; Pointer < Inst.UiInstance.Graph.Pointer; Pointer++)
							{
								Sample = Inst.UiInstance.Graph.Buffer[Item][Pointer];

								// Scale Y axis
								Value = (DATA16)((((Sample - Inst.UiInstance.Graph.pMin[Item]) * (DATAF)Inst.UiInstance.Graph.pSpan[Item]) / (Inst.UiInstance.Graph.pMax[Item] - Inst.UiInstance.Graph.pMin[Item])));

								// Limit Y axis
								if (Value > Inst.UiInstance.Graph.pSpan[Item])
								{
									Value = Inst.UiInstance.Graph.pSpan[Item];
								}
								if (Value < 0)
								{
									Value = 0;
								}
								Y2 = (short)((Inst.UiInstance.Graph.pOffset[Item] + Inst.UiInstance.Graph.pSpan[Item]) - Value);
								if (Pointer > 1)
								{

									if (Y2 > Y1)
									{
										dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, Color, X - 2, Y1 - 1, X - 1, Y2 - 1);
										dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, Color, X, Y1 + 1, X + 1, Y2 + 1);
									}
									else
									{
										if (Y2 < Y1)
										{
											dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, Color, X, Y1 - 1, X + 1, Y2 - 1);
											dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, Color, X - 2, Y1 + 1, X - 1, Y2 + 1);
										}
										else
										{
											dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, Color, X - 1, Y1 - 1, X, Y2 - 1);
											dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, Color, X - 1, Y1 + 1, X, Y2 + 1);
										}
									}
									dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, Color, X - 1, Y1, X, Y2);

								}
								else
								{
									dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X, Y2);
								}

								Y1 = Y2;
								X++;
							}
						}
					}
					Inst.UiInstance.ScreenBusy = 1;
				}
			}
		}

		void cUiDraw()
		{
			PRGID TmpPrgId;
			OBJID TmpObjId;
			IP TmpIp;
			DATA8[] GBuffer = new DATA8[25];
			UBYTE[] pBmp = new UBYTE[LCD_BUFFER_SIZE];
			DATA8 Cmd;
			DATA8 Color;
			DATA16 X;
			DATA16 Y;
			DATA16 X1;
			DATA16 Y1;
			DATA16 Y2;
			DATA16 Y3;
			DATA32 Size;
			DATA16 R;
			DATA8* pText;
			DATA8 No;
			DATAF DataF;
			DATA8 Figures;
			DATA8 Decimals;
			IP pI;
			DATA8* pState;
			DATA8* pAnswer;
			DATA8 Lng;
			DATA8 SelectedChar;
			DATA8* pType;
			DATA8 Type;
			DATA16 On;
			DATA16 Off;
			DATA16 CharWidth;
			DATA16 CharHeight;
			DATA8 TmpColor;
			DATA16 Tmp;
			DATA8 Length;
			DATA8* pUnit;
			DATA32* pIcons;
			DATA8 Items;
			DATA8 View;
			DATA16* pOffset;
			DATA16* pSpan;
			DATAF* pMin;
			DATAF* pMax;
			DATAF* pVal;
			DATA16 Min;
			DATA16 Max;
			DATA16 Act;
			DATAF Actual;
			DATAF Lowest;
			DATAF Highest;
			DATAF Average;
			DATA8 Icon1;
			DATA8 Icon2;
			DATA8 Icon3;
			DATA8 Blocked;
			DATA8 Open;
			DATA8 Del;
			DATA8* pCharSet;
			DATA16* pLine;

			TmpPrgId = Inst.CurrentProgramId();

			if ((TmpPrgId != (ushort)Slot.GUI_SLOT) && (TmpPrgId != (ushort)Slot.DEBUG_SLOT))
			{
				Inst.UiInstance.RunScreenEnabled = 0;
			}
			if (Inst.UiInstance.ScreenBlocked == 0)
			{
				Blocked = 0;
			}
			else
			{
				TmpObjId = Inst.CallingObjectId();
				if ((TmpPrgId == Inst.UiInstance.ScreenPrgId) && (TmpObjId == Inst.UiInstance.ScreenObjId))
				{
					Blocked = 0;
				}
				else
				{
					Blocked = 1;
				}
			}

			TmpIp = Inst.GetObjectIp();
			Cmd = *(DATA8*)Inst.PrimParPointer();

			switch (Cmd)
			{ // Function
				case (sbyte)UiDrawSubcode.UPDATE:
					{
						if (Blocked == 0)
						{
							cUiUpdateLcd();
							Inst.UiInstance.ScreenBusy = 0;
						}
					}
					break;
				case (sbyte)UiDrawSubcode.CLEAN:
					{
						if (Blocked == 0)
						{
							Inst.UiInstance.Font = (sbyte)FontType.NORMAL_FONT;

							Color = (sbyte)BG_COLOR;
							if (Color != 0)
							{
								Color = -1;
							}
							memset(&((*Inst.UiInstance.pLcd).Lcd[0]), Color, LCD_BUFFER_SIZE);

							Inst.UiInstance.ScreenBusy = 1;
						}
					}
					break;
				case (sbyte)UiDrawSubcode.TEXTBOX:
					{
						X = *(DATA16*)Inst.PrimParPointer();  // start x
						Y = *(DATA16*)Inst.PrimParPointer();  // start y
						X1 = *(DATA16*)Inst.PrimParPointer();  // size x
						Y1 = *(DATA16*)Inst.PrimParPointer();  // size y
						pText = (DATA8*)Inst.PrimParPointer();    // textbox
						Size = *(DATA32*)Inst.PrimParPointer();  // textbox size
						Del = *(DATA8*)Inst.PrimParPointer();   // delimitter
						pLine = (DATA16*)Inst.PrimParPointer();   // line

						if (Blocked == 0)
						{
							if (cUiTextbox(X, Y, X1, Y1, pText, Size, Del, pLine) == Result.BUSY)
							{
								Inst.SetObjectIp(TmpIp - 1);
								Inst.SetDispatchStatus(BUSYBREAK);
							}
						}
						else
						{
							Inst.SetObjectIp(TmpIp - 1);
							Inst.SetDispatchStatus(BUSYBREAK);
						}
					}
					break;
				case (sbyte)UiDrawSubcode.FILLRECT:
					{
						Color = *(DATA8*)Inst.PrimParPointer();
						X = *(DATA16*)Inst.PrimParPointer();
						Y = *(DATA16*)Inst.PrimParPointer();
						X1 = *(DATA16*)Inst.PrimParPointer();
						Y1 = *(DATA16*)Inst.PrimParPointer();
						if (Blocked == 0)
						{
							dLcdFillRect((*Inst.UiInstance.pLcd).Lcd, Color, X, Y, X1, Y1);
							Inst.UiInstance.ScreenBusy = 1;
						}
					}
					break;
				case (sbyte)UiDrawSubcode.INVERSERECT:
					{
						X = *(DATA16*)Inst.PrimParPointer();
						Y = *(DATA16*)Inst.PrimParPointer();
						X1 = *(DATA16*)Inst.PrimParPointer();
						Y1 = *(DATA16*)Inst.PrimParPointer();
						if (Blocked == 0)
						{
							dLcdInverseRect((*Inst.UiInstance.pLcd).Lcd, X, Y, X1, Y1);
							Inst.UiInstance.ScreenBusy = 1;
						}
					}
					break;
				case (sbyte)UiDrawSubcode.RECT:
					{
						Color = *(DATA8*)Inst.PrimParPointer();
						X = *(DATA16*)Inst.PrimParPointer();
						Y = *(DATA16*)Inst.PrimParPointer();
						X1 = *(DATA16*)Inst.PrimParPointer();
						Y1 = *(DATA16*)Inst.PrimParPointer();
						if (Blocked == 0)
						{
							dLcdRect((*Inst.UiInstance.pLcd).Lcd, Color, X, Y, X1, Y1);
							Inst.UiInstance.ScreenBusy = 1;
						}
					}
					break;
				case (sbyte)UiDrawSubcode.PIXEL:
					{
						Color = *(DATA8*)Inst.PrimParPointer();
						X = *(DATA16*)Inst.PrimParPointer();
						Y = *(DATA16*)Inst.PrimParPointer();
						if (Blocked == 0)
						{
							dLcdDrawPixel((*Inst.UiInstance.pLcd).Lcd, Color, X, Y);
							Inst.UiInstance.ScreenBusy = 1;
						}
					}
					break;
				case (sbyte)UiDrawSubcode.LINE:
					{
						Color = *(DATA8*)Inst.PrimParPointer();
						X = *(DATA16*)Inst.PrimParPointer();
						Y = *(DATA16*)Inst.PrimParPointer();
						X1 = *(DATA16*)Inst.PrimParPointer();
						Y1 = *(DATA16*)Inst.PrimParPointer();
						if (Blocked == 0)
						{
							dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, Color, X, Y, X1, Y1);
							Inst.UiInstance.ScreenBusy = 1;
						}
					}
					break;
				case (sbyte)UiDrawSubcode.DOTLINE:
					{
						Color = *(DATA8*)Inst.PrimParPointer();
						X = *(DATA16*)Inst.PrimParPointer();
						Y = *(DATA16*)Inst.PrimParPointer();
						X1 = *(DATA16*)Inst.PrimParPointer();
						Y1 = *(DATA16*)Inst.PrimParPointer();
						On = *(DATA16*)Inst.PrimParPointer();
						Off = *(DATA16*)Inst.PrimParPointer();
						if (Blocked == 0)
						{
							dLcdDrawDotLine((*Inst.UiInstance.pLcd).Lcd, Color, X, Y, X1, Y1, On, Off);
							Inst.UiInstance.ScreenBusy = 1;
						}
					}
					break;
				case (sbyte)UiDrawSubcode.CIRCLE:
					{
						Color = *(DATA8*)Inst.PrimParPointer();
						X = *(DATA16*)Inst.PrimParPointer();
						Y = *(DATA16*)Inst.PrimParPointer();
						R = *(DATA16*)Inst.PrimParPointer();
						if (R != 0)
						{
							if (Blocked == 0)
							{
								dLcdDrawCircle((*Inst.UiInstance.pLcd).Lcd, Color, X, Y, R);
								Inst.UiInstance.ScreenBusy = 1;
							}
						}
					}
					break;
				case (sbyte)UiDrawSubcode.FILLCIRCLE:
					{
						Color = *(DATA8*)Inst.PrimParPointer();
						X = *(DATA16*)Inst.PrimParPointer();
						Y = *(DATA16*)Inst.PrimParPointer();
						R = *(DATA16*)Inst.PrimParPointer();
						if (R != 0)
						{
							if (Blocked == 0)
							{
								dLcdDrawFilledCircle((*Inst.UiInstance.pLcd).Lcd, Color, X, Y, R);
								Inst.UiInstance.ScreenBusy = 1;
							}
						}
					}
					break;
				case (sbyte)UiDrawSubcode.TEXT:
					{
						Color = *(DATA8*)Inst.PrimParPointer();
						X = *(DATA16*)Inst.PrimParPointer();
						Y = *(DATA16*)Inst.PrimParPointer();
						pText = (DATA8*)Inst.PrimParPointer();
						if (Blocked == 0)
						{
							dLcdDrawText((*Inst.UiInstance.pLcd).Lcd, Color, X, Y, Inst.UiInstance.Font, pText);
							Inst.UiInstance.ScreenBusy = 1;
						}
					}
					break;
				case (sbyte)UiDrawSubcode.ICON:
					{
						Color = *(DATA8*)Inst.PrimParPointer();
						X = *(DATA16*)Inst.PrimParPointer();
						Y = *(DATA16*)Inst.PrimParPointer();
						Type = *(DATA8*)Inst.PrimParPointer();
						No = *(DATA8*)Inst.PrimParPointer();
						if (Blocked == 0)
						{
							dLcdDrawIcon((*Inst.UiInstance.pLcd).Lcd, Color, X, Y, Type, No);
							Inst.UiInstance.ScreenBusy = 1;
						}
					}
					break;
				case (sbyte)UiDrawSubcode.BMPFILE:
					{
						Color = *(DATA8*)Inst.PrimParPointer();
						X = *(DATA16*)Inst.PrimParPointer();
						Y = *(DATA16*)Inst.PrimParPointer();
						pText = (DATA8*)Inst.PrimParPointer();

						if (Blocked == 0)
						{
							if (cMemoryGetImage(pText, LCD_BUFFER_SIZE, pBmp) == Result.OK)
							{
								dLcdDrawBitmap((*Inst.UiInstance.pLcd).Lcd, Color, X, Y, pBmp);
								Inst.UiInstance.ScreenBusy = 1;
							}
						}
					}
					break;
				case (sbyte)UiDrawSubcode.PICTURE:
					{
						Color = *(DATA8*)Inst.PrimParPointer();
						X = *(DATA16*)Inst.PrimParPointer();
						Y = *(DATA16*)Inst.PrimParPointer();
						pI = *(IP*)Inst.PrimParPointer();
						if (pI != null)
						{
							if (Blocked == 0)
							{
								dLcdDrawBitmap((*Inst.UiInstance.pLcd).Lcd, Color, X, Y, pI);
								Inst.UiInstance.ScreenBusy = 1;
							}
						}
					}
					break;
				case (sbyte)UiDrawSubcode.VALUE:
					{
						Color = *(DATA8*)Inst.PrimParPointer();
						X = *(DATA16*)Inst.PrimParPointer();
						Y = *(DATA16*)Inst.PrimParPointer();
						DataF = *(DATAF*)Inst.PrimParPointer();
						Figures = *(DATA8*)Inst.PrimParPointer();
						Decimals = *(DATA8*)Inst.PrimParPointer();

						if (float.IsNaN(DataF))
						{
							for (Lng = 0; Lng < Figures; Lng++)
							{
								GBuffer[Lng] = (sbyte)'-';
							}
						}
						else
						{
							if (Figures < 0)
							{
								Figures = (sbyte)(0 - Figures);
								snprintf((char*)GBuffer, 24, "%.*f", Decimals, DataF);
							}
							else
							{
								snprintf((char*)GBuffer, 24, "%*.*f", Figures, Decimals, DataF);
							}
							if (GBuffer[0] == '-')
							{ // Negative

								Figures++;
							}
						}
						GBuffer[Figures] = 0;
						pText = GBuffer;
						if (Blocked == 0)
						{
							dLcdDrawText((*Inst.UiInstance.pLcd).Lcd, Color, X, Y, Inst.UiInstance.Font, pText);
							Inst.UiInstance.ScreenBusy = 1;
						}
					}
					break;
				case (sbyte)UiDrawSubcode.VIEW_VALUE:
					{
						Color = *(DATA8*)Inst.PrimParPointer();
						X = *(DATA16*)Inst.PrimParPointer();
						Y = *(DATA16*)Inst.PrimParPointer();
						DataF = *(DATAF*)Inst.PrimParPointer();
						Figures = *(DATA8*)Inst.PrimParPointer();
						Decimals = *(DATA8*)Inst.PrimParPointer();
						if (Blocked == 0)
						{
							TmpColor = Color;
							CharWidth = dLcdGetFontWidth(Inst.UiInstance.Font);
							CharHeight = dLcdGetFontHeight(Inst.UiInstance.Font);
							X1 = (short)(((CharWidth + 2) / 3) - 1);
							Y1 = (short)(CharHeight / 2);

							Lng = (DATA8)snprintf((char*)GBuffer, 24, "%.*f", Decimals, DataF);

							if (Lng != 0)
							{
								if (GBuffer[0] == '-')
								{ // Negative

									TmpColor = Color;
									Lng--;
									pText = &GBuffer[1];
								}
								else
								{ // Positive

									TmpColor = (sbyte)(1 - Color);
									pText = GBuffer;
								}

								// Make sure negative sign is deleted from last time
								dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, 1 - Color, X - X1, Y + Y1, X + (Figures * CharWidth), Y + Y1);
								if (CharHeight > 12)
								{
									dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, 1 - Color, X - X1, Y + Y1 - 1, X + (Figures * CharWidth), Y + Y1 - 1);
								}

								// Check for "not a number"
								Tmp = 0;
								while ((pText[Tmp] != 0) && (pText[Tmp] != 'n'))
								{
									Tmp++;
								}
								if (pText[Tmp] == 'n')
								{ // "nan"

									for (Tmp = 0; Tmp < (DATA16)Figures; Tmp++)
									{
										GBuffer[Tmp] = (sbyte)'-';
									}
									GBuffer[Tmp] = 0;

									// Draw figures
									dLcdDrawText((*Inst.UiInstance.pLcd).Lcd, Color, X, Y, Inst.UiInstance.Font, GBuffer);
								}
								else
								{ // Normal number

									// Check number of figures
									if (Lng > Figures)
									{ // Limit figures

										for (Tmp = 0; Tmp < (DATA16)Figures; Tmp++)
										{
											GBuffer[Tmp] = (sbyte)'>';
										}
										GBuffer[Tmp] = 0;
										Lng = (sbyte)Figures;
										pText = GBuffer;
										TmpColor = (sbyte)(1 - Color);

										// Find X indent
										Tmp = (short)(((DATA16)Figures - Lng) * CharWidth);
									}
									else
									{ // Centre figures

										// Find X indent
										Tmp = (short)(((((DATA16)Figures - Lng) + 1) / 2) * CharWidth);
									}

									// Draw figures
									dLcdDrawText((*Inst.UiInstance.pLcd).Lcd, Color, X + Tmp, Y, Inst.UiInstance.Font, pText);

									// Draw negative sign
									dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, TmpColor, X - X1 + Tmp, Y + Y1, X + Tmp, Y + Y1);
									if (CharHeight > 12)
									{
										dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, TmpColor, X - X1 + Tmp, Y + Y1 - 1, X + Tmp, Y + Y1 - 1);
									}
								}
							}
							Inst.UiInstance.ScreenBusy = 1;
						}
					}
					break;
				case (sbyte)UiDrawSubcode.VIEW_UNIT:
					{
						Color = *(DATA8*)Inst.PrimParPointer();
						X = *(DATA16*)Inst.PrimParPointer();
						Y = *(DATA16*)Inst.PrimParPointer();
						DataF = *(DATAF*)Inst.PrimParPointer();
						Figures = *(DATA8*)Inst.PrimParPointer();
						Decimals = *(DATA8*)Inst.PrimParPointer();
						Length = *(DATA8*)Inst.PrimParPointer();
						pUnit = (DATA8*)Inst.PrimParPointer();

						if (Blocked == 0)
						{
							TmpColor = Color;
							CharWidth = dLcdGetFontWidth(LARGE_FONT);
							CharHeight = dLcdGetFontHeight(LARGE_FONT);
							X1 = (short)(((CharWidth + 2) / 3) - 1);
							Y1 = (short)(CharHeight / 2);

							Lng = (DATA8)snprintf((char*)GBuffer, 24, "%.*f", Decimals, DataF);

							if (Lng != 0)
							{
								if (GBuffer[0] == '-')
								{ // Negative

									TmpColor = Color;
									Lng--;
									pText = &GBuffer[1];
								}
								else
								{ // Positive

									TmpColor = (sbyte)(1 - Color);
									pText = GBuffer;
								}

								// Make sure negative sign is deleted from last time
								dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, 1 - Color, X - X1, Y + Y1, X + (Figures * CharWidth), Y + Y1);
								if (CharHeight > 12)
								{
									dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, 1 - Color, X - X1, Y + Y1 - 1, X + (Figures * CharWidth), Y + Y1 - 1);
								}

								// Check for "not a number"
								Tmp = 0;
								while ((pText[Tmp] != 0) && (pText[Tmp] != 'n'))
								{
									Tmp++;
								}
								if (pText[Tmp] == 'n')
								{ // "nan"

									for (Tmp = 0; Tmp < (DATA16)Figures; Tmp++)
									{
										GBuffer[Tmp] = (sbyte)'-';
									}
									GBuffer[Tmp] = 0;

									// Draw figures
									dLcdDrawText((*Inst.UiInstance.pLcd).Lcd, Color, X, Y, FontType.LARGE_FONT, GBuffer);
								}
								else
								{ // Normal number

									// Check number of figures
									if (Lng > Figures)
									{ // Limit figures

										for (Tmp = 0; Tmp < (DATA16)Figures; Tmp++)
										{
											GBuffer[Tmp] = (sbyte)'>';
										}
										GBuffer[Tmp] = 0;
										Lng = Figures;
										pText = GBuffer;
										TmpColor = (sbyte)(1 - Color);

										// Find X indent
										Tmp = (short)(((DATA16)Figures - Lng) * CharWidth);
									}
									else
									{ // Centre figures

										// Find X indent
										Tmp = (short)(((((DATA16)Figures - Lng) + 1) / 2) * CharWidth);
									}
									Tmp = 0;

									// Draw figures
									dLcdDrawText((*Inst.UiInstance.pLcd).Lcd, Color, X + Tmp, Y, FontType.LARGE_FONT, pText);

									// Draw negative sign
									dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, TmpColor, X - X1 + Tmp, Y + Y1, X + Tmp, Y + Y1);
									if (CharHeight > 12)
									{
										dLcdDrawLine((*Inst.UiInstance.pLcd).Lcd, TmpColor, X - X1 + Tmp, Y + Y1 - 1, X + Tmp, Y + Y1 - 1);
									}

									Tmp = (short)(((((DATA16)Lng))) * CharWidth);
									snprintf((char*)GBuffer, Length, "%s", pUnit);
									dLcdDrawText((*Inst.UiInstance.pLcd).Lcd, Color, X + Tmp, Y, FontType.SMALL_FONT, GBuffer);

								}
							}
							Inst.UiInstance.ScreenBusy = 1;
						}
					}
					break;
				case (sbyte)UiDrawSubcode.NOTIFICATION:
					{
						Color = *(DATA8*)Inst.PrimParPointer();
						X = *(DATA16*)Inst.PrimParPointer();  // start x
						Y = *(DATA16*)Inst.PrimParPointer();  // start y
						Icon1 = *(DATA8*)Inst.PrimParPointer();
						Icon2 = *(DATA8*)Inst.PrimParPointer();
						Icon3 = *(DATA8*)Inst.PrimParPointer();
						pText = (DATA8*)Inst.PrimParPointer();
						pState = (DATA8*)Inst.PrimParPointer();

						if (Blocked == 0)
						{
							if (cUiNotification(Color, X, Y, Icon1, Icon2, Icon3, pText, pState) == (sbyte)Result.BUSY)
							{
								Inst.SetObjectIp(TmpIp - 1);
								Inst.SetDispatchStatus(DSPSTAT.BUSYBREAK);
							}
						}
						else
						{
							Inst.SetObjectIp(TmpIp - 1);
							Inst.SetDispatchStatus(DSPSTAT.BUSYBREAK);
						}
					}
					break;
				case (sbyte)UiDrawSubcode.QUESTION:
					{
						Color = *(DATA8*)Inst.PrimParPointer();
						X = *(DATA16*)Inst.PrimParPointer();  // start x
						Y = *(DATA16*)Inst.PrimParPointer();  // start y
						Icon1 = *(DATA8*)Inst.PrimParPointer();
						Icon2 = *(DATA8*)Inst.PrimParPointer();
						pText = (DATA8*)Inst.PrimParPointer();
						pState = (DATA8*)Inst.PrimParPointer();
						pAnswer = (DATA8*)Inst.PrimParPointer();

						if (Blocked == 0)
						{
							if (cUiQuestion(Color, X, Y, Icon1, Icon2, pText, pState, pAnswer) == (sbyte)Result.BUSY)
							{
								Inst.SetObjectIp(TmpIp - 1);
								Inst.SetDispatchStatus(DSPSTAT.BUSYBREAK);
							}
						}
						else
						{
							Inst.SetObjectIp(TmpIp - 1);
							Inst.SetDispatchStatus(DSPSTAT.BUSYBREAK);
						}
					}
					break;
				case (sbyte)UiDrawSubcode.ICON_QUESTION:
					{
						Color = *(DATA8*)Inst.PrimParPointer();
						X = *(DATA16*)Inst.PrimParPointer();  // start x
						Y = *(DATA16*)Inst.PrimParPointer();  // start y
						pState = (DATA8*)Inst.PrimParPointer();
						pIcons = (DATA32*)Inst.PrimParPointer();

						if (Blocked == 0)
						{
							if (cUiIconQuestion(Color, X, Y, pState, pIcons) == Result.BUSY)
							{
								Inst.SetObjectIp(TmpIp - 1);
								Inst.SetDispatchStatus(DSPSTAT.BUSYBREAK);
							}
						}
						else
						{
							Inst.SetObjectIp(TmpIp - 1);
							Inst.SetDispatchStatus(DSPSTAT.BUSYBREAK);
						}
					}
					break;
				case (sbyte)UiDrawSubcode.KEYBOARD:
					{
						Color = *(DATA8*)Inst.PrimParPointer();
						X = *(DATA16*)Inst.PrimParPointer();  // start x
						Y = *(DATA16*)Inst.PrimParPointer();  // start y
						No = *(DATA8*)Inst.PrimParPointer();   // Icon
						Lng = *(DATA8*)Inst.PrimParPointer();   // length
						pText = (DATA8*)Inst.PrimParPointer();    // default
						pCharSet = (DATA8*)Inst.PrimParPointer();    // valid char set
						pAnswer = (DATA8*)Inst.PrimParPointer();    // string

						if (VMInstance.Handle >= 0)
						{
							pAnswer = (DATA8*)VmMemoryResize(VMInstance.Handle, (DATA32)Lng);
						}

						if (Blocked == 0)
						{
							SelectedChar = cUiKeyboard(Color, X, Y, No, Lng, pText, pCharSet, pAnswer);

							// Wait for "ENTER"
							if (SelectedChar != 0x0D)
							{
								Inst.SetObjectIp(TmpIp - 1);
								Inst.SetDispatchStatus(DSPSTAT.BUSYBREAK);
							}
						}
						else
						{
							Inst.SetObjectIp(TmpIp - 1);
							Inst.SetDispatchStatus(DSPSTAT.BUSYBREAK);
						}
					}
					break;
				case (sbyte)UiDrawSubcode.BROWSE:
					{
						Type = *(DATA8*)Inst.PrimParPointer();   // Browser type
						X = *(DATA16*)Inst.PrimParPointer();  // start x
						Y = *(DATA16*)Inst.PrimParPointer();  // start y
						X1 = *(DATA16*)Inst.PrimParPointer();  // size x
						Y1 = *(DATA16*)Inst.PrimParPointer();  // size y
						Lng = *(DATA8*)Inst.PrimParPointer();   // length
						pType = (DATA8*)Inst.PrimParPointer();    // item type
						pAnswer = (DATA8*)Inst.PrimParPointer();    // item name

						if (VMInstance.Handle >= 0)
						{
							pAnswer = (DATA8*)VmMemoryResize(VMInstance.Handle, (DATA32)Lng);
						}

						if (Blocked == 0)
						{
							if (cUiBrowser(Type, X, Y, X1, Y1, Lng, pType, pAnswer) == Result.BUSY)
							{
								Inst.SetObjectIp(TmpIp - 1);
								Inst.SetDispatchStatus(DSPSTAT.BUSYBREAK);
							}
						}
						else
						{
							Inst.SetObjectIp(TmpIp - 1);
							Inst.SetDispatchStatus(DSPSTAT.BUSYBREAK);
						}
					}
					break;
				case (sbyte)UiDrawSubcode.VERTBAR:
					{
						Color = *(DATA8*)Inst.PrimParPointer();
						X = *(DATA16*)Inst.PrimParPointer();  // start x
						Y = *(DATA16*)Inst.PrimParPointer();  // start y
						X1 = *(DATA16*)Inst.PrimParPointer();  // size x
						Y1 = *(DATA16*)Inst.PrimParPointer();  // size y
						Min = *(DATA16*)Inst.PrimParPointer();  // min
						Max = *(DATA16*)Inst.PrimParPointer();  // max
						Act = *(DATA16*)Inst.PrimParPointer();  // actual

						if (Blocked == 0)
						{
							cUiDrawBar(Color, X, Y, X1, Y1, Min, Max, Act);
						}
					}
					break;
				case (sbyte)UiDrawSubcode.SELECT_FONT:
					{
						Inst.UiInstance.Font = *(DATA8*)Inst.PrimParPointer();
						if (Blocked == 0)
						{
							if (Inst.UiInstance.Font >= (sbyte)FontType.FONTTYPES)
							{
								Inst.UiInstance.Font = ((sbyte)FontType.FONTTYPES - 1);
							}
							if (Inst.UiInstance.Font < 0)
							{
								Inst.UiInstance.Font = 0;
							}
						}
					}
					break;
				case (sbyte)UiDrawSubcode.TOPLINE:
					{
						Inst.UiInstance.TopLineEnabled = *(DATA8*)Inst.PrimParPointer();
					}
					break;
				case (sbyte)UiDrawSubcode.FILLWINDOW:
					{
						Color = *(DATA8*)Inst.PrimParPointer();
						Y = *(DATA16*)Inst.PrimParPointer();  // start y
						Y1 = *(DATA16*)Inst.PrimParPointer();  // size y
						if (Blocked == 0)
						{
							Inst.UiInstance.Font = (sbyte)FontType.NORMAL_FONT;

							if ((Y + Y1) < LCD_HEIGHT)
							{
								if ((Color == 0) || (Color == 1))
								{
									Y *= ((LCD_WIDTH + 7) / 8);

									if (Y1 != 0)
									{
										Y1 *= ((LCD_WIDTH + 7) / 8);
									}
									else
									{
										Y1 = (short)(LCD_BUFFER_SIZE - Y);
									}

									if (Color != 0)
									{
										Color = -1;
									}
									memset(&((*Inst.UiInstance.pLcd).Lcd[Y]), Color, Y1);
								}
								else
								{
									if (Y1 == 0)
									{
										Y1 = LCD_HEIGHT;
									}
									Y2 = ((LCD_WIDTH + 7) / 8);
									for (Tmp = Y; Tmp < Y1; Tmp++)
									{
										Y3 = (short)(Tmp * ((LCD_WIDTH + 7) / 8));
										memset(&((*Inst.UiInstance.pLcd).Lcd[Y3]), Color, Y2);
										Color = (sbyte)~Color;
									}
								}
							}

							Inst.UiInstance.ScreenBusy = 1;
						}
					}
					break;
				case (sbyte)UiDrawSubcode.STORE:
					{
						No = *(DATA8*)Inst.PrimParPointer();
						if (Blocked == 0)
						{
							if (No < LCD_STORE_LEVELS)
							{
								LCDCopy(&Inst.UiInstance.LcdSafe, &Inst.UiInstance.LcdPool[No], sizeof(LCD));
							}
						}
					}
					break;
				case (sbyte)UiDrawSubcode.RESTORE:
					{
						No = *(DATA8*)Inst.PrimParPointer();
						if (Blocked == 0)
						{
							if (No < LCD_STORE_LEVELS)
							{
								LCDCopy(&Inst.UiInstance.LcdPool[No], &Inst.UiInstance.LcdSafe, sizeof(LCD));
								Inst.UiInstance.ScreenBusy = 1;
							}
						}
					}
					break;
				case (sbyte)UiDrawSubcode.GRAPH_SETUP:
					{
						X = *(DATA16*)Inst.PrimParPointer();  // start x
						X1 = *(DATA16*)Inst.PrimParPointer();  // size y
						Items = *(DATA8*)Inst.PrimParPointer();   // items
						pOffset = (DATA16*)Inst.PrimParPointer();  // handle to offset Y
						pSpan = (DATA16*)Inst.PrimParPointer();  // handle to span y
						pMin = (DATAF*)Inst.PrimParPointer();  // handle to min
						pMax = (DATAF*)Inst.PrimParPointer();  // handle to max
						pVal = (DATAF*)Inst.PrimParPointer();  // handle to val

						if (Blocked == 0)
						{
							cUiGraphSetup(X, X1, Items, pOffset, pSpan, pMin, pMax, pVal);
						}
					}
					break;
				case (sbyte)UiDrawSubcode.GRAPH_DRAW:
					{
						View = *(DATA8*)Inst.PrimParPointer();   // view

						cUiGraphDraw(View, &Actual, &Lowest, &Highest, &Average);

						*(DATAF*)Inst.PrimParPointer() = Actual;
						*(DATAF*)Inst.PrimParPointer() = Lowest;
						*(DATAF*)Inst.PrimParPointer() = Highest;
						*(DATAF*)Inst.PrimParPointer() = Average;
					}
					break;
				case (sbyte)UiDrawSubcode.SCROLL:
					{
						Y = *(DATA16*)Inst.PrimParPointer();
						if ((Y > 0) && (Y < LCD_HEIGHT))
						{
							dLcdScroll((*Inst.UiInstance.pLcd).Lcd, Y);
						}
					}
					break;
				case (sbyte)UiDrawSubcode.POPUP:
					{
						Open = *(DATA8*)Inst.PrimParPointer();
						if (Blocked == 0)
						{
							if (Open != 0)
							{
								if (Inst.UiInstance.ScreenBusy == 0)
								{
									TmpObjId = Inst.CallingObjectId();

									LCDCopy(&Inst.UiInstance.LcdSafe, &Inst.UiInstance.LcdSave, sizeof(Inst.UiInstance.LcdSave));
									Inst.UiInstance.ScreenPrgId = TmpPrgId;
									Inst.UiInstance.ScreenObjId = TmpObjId;
									Inst.UiInstance.ScreenBlocked = 1;
								}
								else
								{ // Wait on scrreen

									Inst.SetObjectIp(TmpIp - 1);
									Inst.SetDispatchStatus(DSPSTAT.BUSYBREAK);
								}
							}
							else
							{
								LCDCopy(&Inst.UiInstance.LcdSave, &Inst.UiInstance.LcdSafe, sizeof(Inst.UiInstance.LcdSafe));
								dLcdUpdate(Inst.UiInstance.pLcd);

								Inst.UiInstance.ScreenPrgId = -1;
								Inst.UiInstance.ScreenObjId = -1;
								Inst.UiInstance.ScreenBlocked = 0;
							}
						}
						else
						{ // Wait on not blocked

							Inst.SetObjectIp(TmpIp - 1);
							Inst.SetDispatchStatus(DSPSTAT.BUSYBREAK);
						}
					}
					break;
			}
		}


		/*! \page cUi
		 *  <hr size="1"/>
		 *  <b>     opUI_FLUSH ()  </b>
		 *
		 *- User Interface flush buffers\n
		 *- Dispatch status unchanged
		 *
		 */
		/*! \brief  opUI_FLUSH byte code
		 *
		 */
		void cUiFlush()
		{
			cUiFlushBuffer();
		}

		void cUiRead()
		{
			IP TmpIp;
			DATA8 Cmd;
			DATA8 Lng;
			DATA8 Data8;
			DATA8* pSource;
			DATA8* pDestination;
			DATA16 Data16;
			IMGDATA Tmp;
			DATA32 pImage;
			DATA32 Length;
			DATA32 Total;
			DATA32 Size;
			IMGHEAD* pImgHead;
			OBJHEAD* pObjHead;


			TmpIp = Inst.GetObjectIp();
			Cmd = *(DATA8*)Inst.PrimParPointer();

			switch (Cmd)
			{ // Function
				case (sbyte)UiReadSubcode.GET_STRING:
					{
						if (Inst.UiInstance.Keys != 0)
						{
							Lng = *(DATA8*)Inst.PrimParPointer();
							pDestination = (DATA8*)Inst.PrimParPointer();
							pSource = (DATA8*)Inst.UiInstance.KeyBuffer;

							while ((Inst.UiInstance.Keys != 0) && (Lng != 0))
							{
								*pDestination = *pSource;
								pDestination++;
								pSource++;
								Inst.UiInstance.Keys--;
								Lng--;
							}
							*pDestination = 0;
							Inst.UiInstance.KeyBufIn = 0;
						}
						else
						{
							Inst.SetObjectIp(TmpIp - 1);
							Inst.SetDispatchStatus(DSPSTAT.BUSYBREAK);
						}
					}
					break;
				case (sbyte)UiReadSubcode.KEY:
					{
						if (Inst.UiInstance.KeyBufIn != 0)
						{
							*(DATA8*)Inst.PrimParPointer() = (DATA8)Inst.UiInstance.KeyBuffer[0];
							Inst.UiInstance.KeyBufIn--;

							for (Lng = 0; Lng < Inst.UiInstance.KeyBufIn; Lng++)
							{
								Inst.UiInstance.KeyBuffer[Lng] = Inst.UiInstance.KeyBuffer[Lng + 1];
							}
						}
						else
						{
							*(DATA8*)Inst.PrimParPointer() = (DATA8)0;
						}
					}
					break;
				case (sbyte)UiReadSubcode.GET_SHUTDOWN:
					{
						*(DATA8*)Inst.PrimParPointer() = Inst.UiInstance.ShutDown;
						Inst.UiInstance.ShutDown = 0;
					}
					break;
				case (sbyte)UiReadSubcode.GET_WARNING:
					{
						*(DATA8*)Inst.PrimParPointer() = Inst.UiInstance.Warning;
					}
					break;
				case (sbyte)UiReadSubcode.GET_LBATT:
					{
						Data16 = (DATA16)(Inst.UiInstance.Vbatt * 1000.0);
						Data16 -= Inst.UiInstance.BattIndicatorLow;
						Data16 = (short)((Data16 * 100) / (Inst.UiInstance.BattIndicatorHigh - Inst.UiInstance.BattIndicatorLow));
						if (Data16 > 100)
						{
							Data16 = 100;
						}
						if (Data16 < 0)
						{
							Data16 = 0;
						}
						*(DATA8*)Inst.PrimParPointer() = (DATA8)Data16;
					}
					break;
				case (sbyte)UiWriteSubcode.ADDRESS:
					{
						if (Inst.UiInstance.Keys != 0)
						{
							*(DATA32*)Inst.PrimParPointer() = (DATA32)atol((const char*)Inst.UiInstance.KeyBuffer);
							Inst.UiInstance.Keys = 0;
						}
						else
						{
							Inst.SetObjectIp(TmpIp - 1);
							Inst.SetDispatchStatus(DSPSTAT.BUSYBREAK);
						}
					}
					break;
				case (sbyte)UiWriteSubcode.CODE:
					{
						if (Inst.UiInstance.Keys != 0)
						{
							Length = *(DATA32*)Inst.PrimParPointer();
							pImage = *(DATA32*)Inst.PrimParPointer();

							pImgHead = (IMGHEAD*)pImage;
							pObjHead = (OBJHEAD*)(pImage + sizeof(IMGHEAD));
							pDestination = (DATA8*)(pImage + sizeof(IMGHEAD) + sizeof(OBJHEAD));

							if (Length > (sizeof(IMGHEAD) + sizeof(OBJHEAD)))
							{

								(*pImgHead).Sign[0] = (byte)'l';
								(*pImgHead).Sign[1] = (byte)'e';
								(*pImgHead).Sign[2] = (byte)'g';
								(*pImgHead).Sign[3] = (byte)'o';
								(*pImgHead).ImageSize = 0;
								(*pImgHead).VersionInfo = (UWORD)(VERS * 100.0);
								(*pImgHead).NumberOfObjects = 1;
								(*pImgHead).GlobalBytes = 0;

								(*pObjHead).OffsetToInstructions = (IP)(sizeof(IMGHEAD) + sizeof(OBJHEAD));
								(*pObjHead).OwnerObjectId = 0;
								(*pObjHead).TriggerCount = 0;
								(*pObjHead).LocalBytes = MAX_COMMAND_LOCALS;

								pSource = (DATA8*)Inst.UiInstance.KeyBuffer;
								Size = sizeof(IMGHEAD) + sizeof(OBJHEAD);

								Length -= sizeof(IMGHEAD) + sizeof(OBJHEAD);
								Length--;
								while ((Inst.UiInstance.Keys != 0) && (Length != 0))
								{
									Tmp = (IMGDATA)(AtoN(*pSource) << 4);
									pSource++;
									Inst.UiInstance.Keys--;
									if (Inst.UiInstance.Keys != 0)
									{
										Tmp += (IMGDATA)(AtoN(*pSource));
										pSource++;
										Inst.UiInstance.Keys--;
									}
									else
									{
										Tmp = 0;
									}
									*pDestination = (sbyte)Tmp;
									pDestination++;
									Length--;
									Size++;
								}
								*pDestination = opOBJECT_END;
								Size++;
								(*pImgHead).ImageSize = Size;
								memset(Inst.UiInstance.Globals, 0, sizeof(Inst.UiInstance.Globals));

								*(DATA32*)Inst.PrimParPointer() = (DATA32)Inst.UiInstance.Globals;
								*(DATA8*)Inst.PrimParPointer() = 1;
							}
						}
						else
						{
							Inst.SetObjectIp(TmpIp - 1);
							Inst.SetDispatchStatus(DSPSTAT.BUSYBREAK);
						}
					}
					break;
				case (sbyte)UiReadSubcode.GET_HW_VERS:
					{
						Lng = *(DATA8*)Inst.PrimParPointer();
						pDestination = (DATA8*)Inst.PrimParPointer();

						if (VMInstance.Handle >= 0)
						{
							Data8 = (DATA8)strlen((char*)Inst.UiInstance.HwVers) + 1;
							if ((Lng > Data8) || (Lng == -1))
							{
								Lng = Data8;
							}
							pDestination = (DATA8*)VmMemoryResize(VMInstance.Handle, (DATA32)Lng);
						}
						if (pDestination != null)
						{
							snprintf((char*)pDestination, Lng, "%s", Inst.UiInstance.HwVers);
						}
					}
					break;
				case (sbyte)UiReadSubcode.GET_FW_VERS:
					{
						Lng = *(DATA8*)Inst.PrimParPointer();
						pDestination = (DATA8*)Inst.PrimParPointer();

						if (VMInstance.Handle >= 0)
						{
							Data8 = (DATA8)strlen((char*)Inst.UiInstance.FwVers) + 1;
							if ((Lng > Data8) || (Lng == -1))
							{
								Lng = Data8;
							}
							pDestination = (DATA8*)VmMemoryResize(VMInstance.Handle, (DATA32)Lng);
						}
						if (pDestination != null)
						{
							snprintf((char*)pDestination, Lng, "%s", Inst.UiInstance.FwVers);
						}
					}
					break;
				case (sbyte)UiReadSubcode.GET_FW_BUILD:
					{
						Lng = *(DATA8*)Inst.PrimParPointer();
						pDestination = (DATA8*)Inst.PrimParPointer();

						if (VMInstance.Handle >= 0)
						{
							Data8 = (DATA8)strlen((char*)Inst.UiInstance.FwBuild) + 1;
							if ((Lng > Data8) || (Lng == -1))
							{
								Lng = Data8;
							}
							pDestination = (DATA8*)VmMemoryResize(VMInstance.Handle, (DATA32)Lng);
						}
						if (pDestination != null)
						{
							snprintf((char*)pDestination, Lng, "%s", Inst.UiInstance.FwBuild);
						}
					}
					break;
				case (sbyte)UiReadSubcode.GET_OS_VERS:
					{
						Lng = *(DATA8*)Inst.PrimParPointer();
						pDestination = (DATA8*)Inst.PrimParPointer();

						if (VMInstance.Handle >= 0)
						{
							Data8 = (DATA8)strlen((char*)Inst.UiInstance.OsVers) + 1;
							if ((Lng > Data8) || (Lng == -1))
							{
								Lng = Data8;
							}
							pDestination = (DATA8*)VmMemoryResize(VMInstance.Handle, (DATA32)Lng);
						}
						if (pDestination != null)
						{
							snprintf((char*)pDestination, Lng, "%s", Inst.UiInstance.OsVers);
						}
					}
					break;
				case (sbyte)UiReadSubcode.GET_OS_BUILD:
					{
						Lng = *(DATA8*)Inst.PrimParPointer();
						pDestination = (DATA8*)Inst.PrimParPointer();

						if (VMInstance.Handle >= 0)
						{
							Data8 = (DATA8)strlen((char*)Inst.UiInstance.OsBuild) + 1;
							if ((Lng > Data8) || (Lng == -1))
							{
								Lng = Data8;
							}
							pDestination = (DATA8*)VmMemoryResize(VMInstance.Handle, (DATA32)Lng);
						}
						if (pDestination != null)
						{
							snprintf((char*)pDestination, Lng, "%s", Inst.UiInstance.OsBuild);
						}
					}
					break;
				case (sbyte)UiReadSubcode.GET_VERSION:
					{
						snprintf((char*)Inst.UiInstance.ImageBuffer, IMAGEBUFFER_SIZE, "%s V%4.2f%c(%s %s)", PROJECT, VERS, SPECIALVERS, __DATE__, __TIME__);
						Lng = *(DATA8*)Inst.PrimParPointer();
						pDestination = (DATA8*)Inst.PrimParPointer();
						pSource = (DATA8*)Inst.UiInstance.ImageBuffer;

						if (VMInstance.Handle >= 0)
						{
							Data8 = (DATA8)strlen((char*)Inst.UiInstance.ImageBuffer) + 1;
							if ((Lng > Data8) || (Lng == -1))
							{
								Lng = Data8;
							}
							pDestination = (DATA8*)VmMemoryResize(VMInstance.Handle, (DATA32)Lng);
						}
						if (pDestination != null)
						{
							snprintf((char*)pDestination, Lng, "%s", Inst.UiInstance.ImageBuffer);
						}
					}
					break;
				case (sbyte)UiReadSubcode.GET_IP:
					{
						Lng = *(DATA8*)Inst.PrimParPointer();
						pDestination = (DATA8*)Inst.PrimParPointer();

						if (VMInstance.Handle >= 0)
						{
							Data8 = IPADDR_SIZE;
							if ((Lng > Data8) || (Lng == -1))
							{
								Lng = Data8;
							}
							pDestination = (DATA8*)VmMemoryResize(VMInstance.Handle, (DATA32)Lng);
						}
						if (pDestination != null)
						{
							snprintf((char*)pDestination, Lng, "%s", Inst.UiInstance.IpAddr);
						}

					}
					break;
				case (sbyte)UiReadSubcode.GET_POWER:
					{
						*(DATAF*)Inst.PrimParPointer() = Inst.UiInstance.Vbatt;
						*(DATAF*)Inst.PrimParPointer() = Inst.UiInstance.Ibatt;
						*(DATAF*)Inst.PrimParPointer() = Inst.UiInstance.Iintegrated;
						*(DATAF*)Inst.PrimParPointer() = Inst.UiInstance.Imotor;
					}
					break;
				case (sbyte)UiReadSubcode.GET_VBATT:
					{
						*(DATAF*)Inst.PrimParPointer() = Inst.UiInstance.Vbatt;
					}
					break;
				case (sbyte)UiReadSubcode.GET_IBATT:
					{
						*(DATAF*)Inst.PrimParPointer() = Inst.UiInstance.Ibatt;
					}
					break;
				case (sbyte)UiReadSubcode.GET_IINT:
					{
						*(DATAF*)Inst.PrimParPointer() = Inst.UiInstance.Iintegrated;
					}
					break;
				case (sbyte)UiReadSubcode.GET_IMOTOR:
					{
						*(DATAF*)Inst.PrimParPointer() = Inst.UiInstance.Imotor;
					}
					break;
				case (sbyte)UiReadSubcode.GET_EVENT:
					{
						*(DATA8*)Inst.PrimParPointer() = Inst.UiInstance.Event;
						Inst.UiInstance.Event = 0;
					}
					break;
				case (sbyte)UiReadSubcode.GET_TBATT:
					{
						*(DATAF*)Inst.PrimParPointer() = Inst.UiInstance.Tbatt;
					}
					break;
				case (sbyte)UiReadSubcode.TEXTBOX_READ:
					{
						pSource = (DATA8*)Inst.PrimParPointer();
						Size = *(DATA32*)Inst.PrimParPointer();
						Data8 = *(DATA8*)Inst.PrimParPointer();
						Lng = *(DATA8*)Inst.PrimParPointer();
						Data16 = *(DATA16*)Inst.PrimParPointer();
						pDestination = (DATA8*)Inst.PrimParPointer();

						cUiTextboxReadLine(pSource, Size, Data8, Lng, Data16, pDestination, &Data8);
					}
					break;
				case (sbyte)UiReadSubcode.GET_SDCARD:
					{
						*(DATA8*)Inst.PrimParPointer() = Inst.CheckSdcard(&Data8, &Total, &Size, 0);
						*(DATA32*)Inst.PrimParPointer() = Total;
						*(DATA32*)Inst.PrimParPointer() = Size;
					}
					break;
				case (sbyte)UiReadSubcode.GET_USBSTICK:
					{
						*(DATA8*)Inst.PrimParPointer() = Inst.CheckUsbstick(&Data8, &Total, &Size, 0);
						*(DATA32*)Inst.PrimParPointer() = Total;
						*(DATA32*)Inst.PrimParPointer() = Size;
					}
					break;

			}
		}

		void cUiWrite()
		{
			IP TmpIp;
			DATA8 Cmd;
			DATA8* pSource;
			DSPSTAT DspStat = DSPSTAT.BUSYBREAK;
			DATA8[] Buffer = new DATA8[50];
			DATA8 Data8;
			DATA16 Data16;
			DATA32 Data32;
			DATA32 pGlobal;
			DATA32 Tmp;
			DATAF DataF;
			DATA8 Figures;
			DATA8 Decimals;
			DATA8 No;
			DATA8* pText;


			TmpIp = Inst.GetObjectIp();
			Cmd = *(DATA8*)Inst.PrimParPointer();

			switch (Cmd)
			{ // Function

				case (sbyte)UiWriteSubcode.WRITE_FLUSH:
					{
						cUiFlush();
						DspStat = DSPSTAT.NOBREAK;
					}
					break;
				case (sbyte)UiWriteSubcode.FLOATVALUE:
					{
						DataF = *(DATAF*)Inst.PrimParPointer();
						Figures = *(DATA8*)Inst.PrimParPointer();
						Decimals = *(DATA8*)Inst.PrimParPointer();

						snprintf((char*)Buffer, 32, "%*.*f", Figures, Decimals, DataF);
						cUiWriteString(Buffer);

						DspStat = DSPSTAT.NOBREAK;
					}
					break;
				case (sbyte)UiWriteSubcode.STAMP:
					{ // write time, prgid, objid, ip

						pSource = (DATA8*)Inst.PrimParPointer();
						snprintf((char*)Buffer, 50, "####[ %09u %01u %03u %06u %s]####\r\n", GetTime(), Inst.CurrentProgramId(), Inst.CallingObjectId(), CurrentObjectIp(), pSource);
						cUiWriteString(Buffer);
						cUiFlush();
						DspStat = DSPSTAT.NOBREAK;
					}
					break;
				case (sbyte)UiWriteSubcode.PUT_STRING:
					{
						pSource = (DATA8*)Inst.PrimParPointer();
						cUiWriteString(pSource);
						DspStat = DSPSTAT.NOBREAK;
					}
					break;
				case (sbyte)UiWriteSubcode.CODE:
					{
						pGlobal = *(DATA32*)Inst.PrimParPointer();
						Data32 = *(DATA32*)Inst.PrimParPointer();

						pSource = (DATA8*)pGlobal;

						cUiWriteString((DATA8*)"\r\n    ");
						for (Tmp = 0; Tmp < Data32; Tmp++)
						{
							snprintf((char*)Buffer, 7, "%02X ", pSource[Tmp] & 0xFF);
							cUiWriteString(Buffer);
							if (((Tmp & 0x3) == 0x3) && ((Tmp & 0xF) != 0xF))
							{
								cUiWriteString((DATA8*)" ");
							}
							if (((Tmp & 0xF) == 0xF) && (Tmp < (Data32 - 1)))
							{
								cUiWriteString((DATA8*)"\r\n    ");
							}
						}
						cUiWriteString((DATA8*)"\r\n");
						DspStat = DSPSTAT.NOBREAK;
					}
					break;
				case (sbyte)UiWriteSubcode.TEXTBOX_APPEND:
					{
						pText = (DATA8*)Inst.PrimParPointer();
						Data32 = *(DATA32*)Inst.PrimParPointer();
						Data8 = *(DATA8*)Inst.PrimParPointer();
						pSource = (DATA8*)Inst.PrimParPointer();

						cUiTextboxAppendLine(pText, Data32, Data8, pSource, Inst.UiInstance.Font);

						DspStat = DSPSTAT.NOBREAK;
					}
					break;
				case (sbyte)UiWriteSubcode.SET_BUSY:
					{
						Data8 = *(DATA8*)Inst.PrimParPointer();

						if (Data8 != 0)
						{
							unchecked { Inst.UiInstance.Warning |= (sbyte)Warning.WARNING_BUSY; }
						}
						else
						{
							Inst.UiInstance.Warning &= (sbyte)~Warning.WARNING_BUSY;
						}

						DspStat = DSPSTAT.NOBREAK;
					}
					break;
				case (sbyte)UiWriteSubcode.VALUE8:
					{
						Data8 = *(DATA8*)Inst.PrimParPointer();
						if (Data8 != DATA8_NAN)
						{
							snprintf((char*)Buffer, 7, "%d", (int)Data8);
						}
						else
						{
							snprintf((char*)Buffer, 7, "nan");
						}
						cUiWriteString(Buffer);

						DspStat = DSPSTAT.NOBREAK;
					}
					break;
				case (sbyte)UiWriteSubcode.VALUE16:
					{
						Data16 = *(DATA16*)Inst.PrimParPointer();
						if (Data16 != DATA16_NAN)
						{
							snprintf((char*)Buffer, 9, "%d", Data16 & 0xFFFF);
						}
						else
						{
							snprintf((char*)Buffer, 7, "nan");
						}
						cUiWriteString(Buffer);

						DspStat = DSPSTAT.NOBREAK;
					}
					break;
				case (sbyte)UiWriteSubcode.VALUE32:
					{
						Data32 = *(DATA32*)Inst.PrimParPointer();
						if (Data32 != DATA32_NAN)
						{
							snprintf((char*)Buffer, 14, "%ld", (long unsigned int)(Data32 & 0xFFFFFFFF));
						}
						else
						{
							snprintf((char*)Buffer, 7, "nan");
						}

						cUiWriteString(Buffer);

						DspStat = DSPSTAT.NOBREAK;
					}
					break;
				case (sbyte)UiWriteSubcode.VALUEF:
					{
						DataF = *(DATAF*)Inst.PrimParPointer();
						snprintf((char*)Buffer, 24, "%f", DataF);
						cUiWriteString(Buffer);

						DspStat = DSPSTAT.NOBREAK;
					}
					break;
				case (sbyte)UiWriteSubcode.LED:
					{
						Data8 = *(DATA8*)Inst.PrimParPointer();
						if (Data8 < 0)
						{
							Data8 = 0;
						}
						if (Data8 >= LEDPATTERNS)
						{
							Data8 = LEDPATTERNS - 1;
						}
						cUiSetLed(Data8);
						Inst.UiInstance.RunLedEnabled = 0;

						DspStat = DSPSTAT.NOBREAK;
					}
					break;
				case (sbyte)UiWriteSubcode.POWER:
					{
						Data8 = *(DATA8*)Inst.PrimParPointer();

						if (Inst.UiInstance.PowerFile >= 0)
						{
							ioctl(Inst.UiInstance.PowerFile, 0, (size_t) & Data8);
						}

						DspStat = DSPSTAT.NOBREAK;
					}
					break;
				case (sbyte)UiWriteSubcode.TERMINAL:
					{
						No = *(DATA8*)Inst.PrimParPointer();

						if (No != 0)
						{
							Inst.SetTerminalEnable(1);
						}
						else
						{
                            Inst.SetTerminalEnable(0);
						}

						DspStat = DSPSTAT.NOBREAK;
					}
					break;
				case (sbyte)UiWriteSubcode.SET_TESTPIN:
					{
						Data8 = *(DATA8*)Inst.PrimParPointer();
						cUiTestpin(Data8);
						DspStat = DSPSTAT.NOBREAK;
					}
					break;
				case (sbyte)UiWriteSubcode.INIT_RUN:
					{
						Inst.UiInstance.RunScreenEnabled = 3;

						DspStat = DSPSTAT.NOBREAK;
					}
					break;
				case (sbyte)UiWriteSubcode.UPDATE_RUN:
					{
						DspStat = DSPSTAT.NOBREAK;
					}
					break;
				case (sbyte)UiWriteSubcode.GRAPH_SAMPLE:
					{
						cUiGraphSample();
						DspStat = DSPSTAT.NOBREAK;
					}
					break;
				case (sbyte)UiWriteSubcode.DOWNLOAD_END:
					{
						Inst.UiInstance.UiUpdate = 1;
						cUiDownloadSuccesSound();
						DspStat = DSPSTAT.NOBREAK;
					}
					break;
				case (sbyte)UiWriteSubcode.SCREEN_BLOCK:
					{
						Inst.UiInstance.ScreenBlocked = *(DATA8*)Inst.PrimParPointer();
						DspStat = DSPSTAT.NOBREAK;
					}
					break;
				default:
					{
						DspStat = DSPSTAT.FAILBREAK;
					}
					break;
			}

			if (DspStat == DSPSTAT.BUSYBREAK)
			{ // Rewind IP

				Inst.SetObjectIp(TmpIp - 1);
			}
			Inst.SetDispatchStatus(DspStat);
		}

		void cUiButton()
		{
			PRGID TmpPrgId;
			OBJID TmpObjId;
			IP TmpIp;
			DATA8 Cmd;
			DATA8 Button;
			DATA8 State;
			DATA16 Inc;
			DATA8 Blocked;

			TmpIp = Inst.GetObjectIp();
			TmpPrgId = Inst.CurrentProgramId();

			if (Inst.UiInstance.ScreenBlocked == 0)
			{
				Blocked = 0;
			}
			else
			{
				TmpObjId = Inst.CallingObjectId();
				if ((TmpPrgId == Inst.UiInstance.ScreenPrgId) && (TmpObjId == Inst.UiInstance.ScreenObjId))
				{
					Blocked = 0;
				}
				else
				{
					Blocked = 1;
				}
			}

			Cmd = *(DATA8*)Inst.PrimParPointer();

			State = 0;
			Inc = 0;

			switch (Cmd)
			{ // Function
				case (sbyte)UiButtonSubcode.PRESS:
					{
						Button = *(DATA8*)Inst.PrimParPointer();
						cUiSetPress(Button, 1);
					}
					break;
				case (sbyte)UiButtonSubcode.RELEASE:
					{
						Button = *(DATA8*)Inst.PrimParPointer();
						cUiSetPress(Button, 0);
					}
					break;
				case (sbyte)UiButtonSubcode.SHORTPRESS:
					{
						Button = *(DATA8*)Inst.PrimParPointer();

						if (Blocked == 0)
						{
							State = cUiGetShortPress(Button);
						}
						*(DATA8*)Inst.PrimParPointer() = State;
					}
					break;
				case (sbyte)UiButtonSubcode.GET_BUMBED:
					{
						Button = *(DATA8*)Inst.PrimParPointer();

						if (Blocked == 0)
						{
							State = cUiGetBumbed(Button);
						}
						*(DATA8*)Inst.PrimParPointer() = State;
					}
					break;
				case (sbyte)UiButtonSubcode.PRESSED:
					{
						Button = *(DATA8*)Inst.PrimParPointer();

						if (Blocked == 0)
						{
							State = cUiGetPress(Button);
						}
						*(DATA8*)Inst.PrimParPointer() = State;
					}
					break;
				case (sbyte)UiButtonSubcode.LONGPRESS:
					{
						Button = *(DATA8*)Inst.PrimParPointer();

						if (Blocked == 0)
						{
							State = cUiGetLongPress(Button);
						}
						*(DATA8*)Inst.PrimParPointer() = State;
					}
					break;
				case (sbyte)UiButtonSubcode.FLUSH:
					{
						if (Blocked == 0)
						{
							cUiButtonFlush();
						}
					}
					break;
				case (sbyte)UiButtonSubcode.WAIT_FOR_PRESS:
					{
						if (Blocked == 0)
						{
							if (cUiWaitForPress() == 0)
							{
								Inst.SetObjectIp(TmpIp - 1);
								Inst.SetDispatchStatus(DSPSTAT.BUSYBREAK);
							}
						}
						else
						{
							Inst.SetObjectIp(TmpIp - 1);
							Inst.SetDispatchStatus(DSPSTAT.BUSYBREAK);
						}
					}
					break;
				case (sbyte)UiButtonSubcode.GET_HORZ:
					{
						if (Blocked == 0)
						{
							Inc = cUiGetHorz();
						}
						*(DATA16*)Inst.PrimParPointer() = Inc;
					}
					break;
				case (sbyte)UiButtonSubcode.GET_VERT:
					{
						if (Blocked == 0)
						{
							Inc = cUiGetVert();
						}
						*(DATA16*)Inst.PrimParPointer() = Inc;
					}
					break;
				case (sbyte)UiButtonSubcode.SET_BACK_BLOCK:
					{
						Inst.UiInstance.BackButtonBlocked = *(DATA8*)Inst.PrimParPointer();
					}
					break;
				case (sbyte)UiButtonSubcode.GET_BACK_BLOCK:
					{
						*(DATA8*)Inst.PrimParPointer() = Inst.UiInstance.BackButtonBlocked;
					}
					break;
				case (sbyte)UiButtonSubcode.TESTSHORTPRESS:
					{
						Button = *(DATA8*)Inst.PrimParPointer();

						if (Blocked == 0)
						{
							State = cUiTestShortPress(Button);
						}
						*(DATA8*)Inst.PrimParPointer() = State;
					}
					break;
				case (sbyte)UiButtonSubcode.TESTLONGPRESS:
					{
						Button = *(DATA8*)Inst.PrimParPointer();

						if (Blocked == 0)
						{
							State = cUiTestLongPress(Button);
						}
						*(DATA8*)Inst.PrimParPointer() = State;
					}
					break;
				case (sbyte)UiButtonSubcode.GET_CLICK:
					{
						*(DATA8*)Inst.PrimParPointer() = Inst.UiInstance.Click;
						Inst.UiInstance.Click = 0;
					}
					break;
			}
		}

		public unsafe void cUiKeepAlive()
		{
			cUiAlive();
			*(DATA8*)Inst.PrimParPointer() = Inst.GetSleepMinutes();
		}
	}
}