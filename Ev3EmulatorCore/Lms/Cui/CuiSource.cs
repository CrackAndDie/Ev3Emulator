using EV3DecompilerLib.Decompile;
using Ev3EmulatorCore.Extensions;
using Ev3EmulatorCore.Lms.Lms2012;

namespace Ev3EmulatorCore.Lms.Cui
{
	public partial class CuiClass
	{
		public void cUiDownloadSuccessSound()
		{
			byte[] locals = new byte[1];
			LmsInstance.Inst.ExecuteBytecode(DownloadSuccesSound, null, locals);
		}

		public void cUiButtonClr()
		{
			for (byte i = 0; i < lms2012.BUTTONS; ++i)
			{
				unchecked
				{
					LmsInstance.Inst.UiInstance.ButtonState[i] &= (byte)~lms2012.BUTTON_CLR;
				}
			}
		}

		public void cUiButtonFlush()
		{
			for (byte i = 0; i < lms2012.BUTTONS; ++i)
			{
				unchecked
				{
					LmsInstance.Inst.UiInstance.ButtonState[i] &= (byte)~lms2012.BUTTON_FLUSH;
				}
			}
		}

		public void cUiSetLed(byte state)
		{
			byte[] buffer = new byte[2];
			LmsInstance.Inst.UiInstance.LedState = state;

			if (LmsInstance.Inst.UiInstance.UiFile >= lms2012.MIN_HANDLE)
			{
				if (LmsInstance.Inst.UiInstance.Warnlight != 0)
				{
					if (state == (byte)lms2012.LedPattern.LED_GREEN_FLASH || state == (byte)lms2012.LedPattern.LED_RED_FLASH || state == (byte)lms2012.LedPattern.LED_ORANGE_FLASH)
					{
						buffer[0] = (byte)lms2012.LedPattern.LED_ORANGE_FLASH + '0';
					}
					else if (state == (byte)lms2012.LedPattern.LED_GREEN_PULSE || state == (byte)lms2012.LedPattern.LED_RED_PULSE || state == (byte)lms2012.LedPattern.LED_ORANGE_PULSE)
					{
						buffer[0] = (byte)lms2012.LedPattern.LED_ORANGE_PULSE + '0';
					}
					else
					{
						buffer[0] = (byte)lms2012.LedPattern.LED_ORANGE + '0';
					}
				}
				else
				{
					buffer[0] = (byte)(LmsInstance.Inst.UiInstance.LedState + '0');
				}
				buffer[1] = 0;
				LmsInstance.Inst.UiFileHandler.Write(LmsInstance.Inst.UiInstance.UiFile, buffer, 2);
			}
		}

		public void cUiAlive()
		{
			LmsInstance.Inst.UiInstance.SleepTimer = 0;
		}

		public lms2012.Result cUiInit()
		{
			lms2012.Result result;

			cUiAlive();

			LmsInstance.Inst.UiInstance.ReadyForWarnings = 0;
			LmsInstance.Inst.UiInstance.UpdateState = 0;
			LmsInstance.Inst.UiInstance.RunLedEnabled = 0;
			LmsInstance.Inst.UiInstance.RunScreenEnabled = 0;
			LmsInstance.Inst.UiInstance.TopLineEnabled = 0;
			LmsInstance.Inst.UiInstance.BackButtonBlocked = 0;
			LmsInstance.Inst.UiInstance.Escape = 0;
			LmsInstance.Inst.UiInstance.KeyBufIn = 0;
			LmsInstance.Inst.UiInstance.Keys = 0;
			LmsInstance.Inst.UiInstance.UiWrBufferSize = 0;

			LmsInstance.Inst.UiInstance.ScreenBusy = 0;
			LmsInstance.Inst.UiInstance.ScreenBlocked = 0;
			LmsInstance.Inst.UiInstance.ScreenPrgId = 0; // there was -1
			LmsInstance.Inst.UiInstance.ScreenObjId = 0; // there was -1

			LmsInstance.Inst.UiInstance.ShutDown = 0; 

			LmsInstance.Inst.UiInstance.PowerShutdown = 0; 
			LmsInstance.Inst.UiInstance.PowerState = 0; 
			LmsInstance.Inst.UiInstance.VoltShutdown = 0; 
			LmsInstance.Inst.UiInstance.Warnlight = 0; 
			LmsInstance.Inst.UiInstance.Warning = 0; 
			LmsInstance.Inst.UiInstance.WarningShowed = 0; 
			LmsInstance.Inst.UiInstance.WarningConfirmed = 0; 
			LmsInstance.Inst.UiInstance.VoltageState = 0;
			
			LmsInstance.Inst.UiInstance.Lcd = LmsInstance.Inst.UiInstance.LcdSafe; 
 
			LmsInstance.Inst.UiInstance.Browser.PrgId = 0; 
			LmsInstance.Inst.UiInstance.Browser.ObjId = 0; 

			LmsInstance.Inst.UiInstance.Tbatt = 0; 
			LmsInstance.Inst.UiInstance.Vbatt = lms2012.DEFAULT_BATTERY_VOLTAGE; 
			LmsInstance.Inst.UiInstance.Ibatt = 0; 
			LmsInstance.Inst.UiInstance.Imotor = 0; 
			LmsInstance.Inst.UiInstance.Iintegrated = 0;

			result = LmsInstance.Inst.DterminalClass.dTerminalInit();

			LmsInstance.Inst.DlcdClass.dLcdInit(LmsInstance.Inst.UiInstance.Lcd.Lcd);

			// https://github.com/mindboards/ev3sources/blob/master/lms2012/c_ui/source/c_ui.c#L780C3-L780C11
			// or 
			// https://github.com/ev3dev/lms2012-compat/blob/ev3dev-stretch/c_ui/c_ui.c#L334
			LmsInstance.Inst.UiInstance.HwVers.WriteString("ev3rcv");
			LmsInstance.Inst.UiInstance.Hw = 0;
			LmsInstance.Inst.UiInstance.FwVers.WriteString("V1.3.37");
			LmsInstance.Inst.UiInstance.FwBuild.WriteString("20Jan201020");

			LmsInstance.Inst.UiInstance.OsVers.WriteString("Anime1337 WAYDH");
			LmsInstance.Inst.UiInstance.OsBuild.WriteString("20Jan201020");

			LmsInstance.Inst.UiInstance.IpAddr.WriteString("0.0.0.0");

			cUiButtonClr();

			LmsInstance.Inst.UiInstance.Accu = 1;
			LmsInstance.Inst.UiInstance.BattIndicatorHigh = lms2012.ACCU_INDICATOR_HIGH;
			LmsInstance.Inst.UiInstance.BattIndicatorLow = lms2012.ACCU_INDICATOR_LOW;
			LmsInstance.Inst.UiInstance.BattWarningHigh = lms2012.ACCU_WARNING_HIGH;
			LmsInstance.Inst.UiInstance.BattWarningLow = lms2012.ACCU_WARNING_LOW;
			LmsInstance.Inst.UiInstance.BattShutdownHigh = lms2012.ACCU_SHUTDOWN_HIGH;
			LmsInstance.Inst.UiInstance.BattShutdownLow = lms2012.ACCU_SHUTDOWN_LOW;

			return result;
		}

		public lms2012.Result cUiOpen()
		{
			lms2012.Result result;

			LmsInstance.Inst.UiInstance.LcdPool[0] = (DlcdClass.LCD)LmsInstance.Inst.UiInstance.LcdSafe.Clone();

			cUiButtonClr();
			cUiSetLed((byte)lms2012.LedPattern.LED_GREEN_PULSE);
			LmsInstance.Inst.UiInstance.RunScreenEnabled = 3;
			LmsInstance.Inst.UiInstance.RunLedEnabled = 1;
			LmsInstance.Inst.UiInstance.TopLineEnabled = 0;

			result = lms2012.Result.OK;
			return result;
		}

		public lms2012.Result cUiClose()
		{
			lms2012.Result result;
			unchecked
			{
				LmsInstance.Inst.UiInstance.Warning &= (byte)~lms2012.Warning.WARNING_BUSY;
			}
			LmsInstance.Inst.UiInstance.RunLedEnabled = 0;
			LmsInstance.Inst.UiInstance.RunScreenEnabled = 0;
			LmsInstance.Inst.UiInstance.TopLineEnabled = 1;
			LmsInstance.Inst.UiInstance.BackButtonBlocked = 0;
			LmsInstance.Inst.UiInstance.Browser.NeedUpdate = 1;
			cUiSetLed((byte)lms2012.LedPattern.LED_GREEN);

			cUiButtonClr();

			result = lms2012.Result.OK;
			return result;
		}

		public lms2012.Result cUiExit()
		{
			lms2012.Result result;

			result = LmsInstance.Inst.DterminalClass.dTerminalExit();

			LmsInstance.Inst.UiFileHandler.OnExit();

			return result;
		}

		public void cUiUpdateButtons(UInt16 time)
		{
			byte button;
			for (button = 0; button < lms2012.BUTTONS; ++button)
			{
				if (LmsInstance.Inst.UiFileHandler.IsButtonPressed(button))
				{
					if (LmsInstance.Inst.UiInstance.ButtonDebounceTimer[button] == 0)
					{
						LmsInstance.Inst.UiInstance.ButtonState[button] |= lms2012.BUTTON_ACTIVE;
					}
					LmsInstance.Inst.UiInstance.ButtonDebounceTimer[button] = lms2012.BUTTON_DEBOUNCE_TIME;
				}
				else
				{
					if (LmsInstance.Inst.UiInstance.ButtonDebounceTimer[button] > 0)
					{
						LmsInstance.Inst.UiInstance.ButtonDebounceTimer[button] -= time;
						if (LmsInstance.Inst.UiInstance.ButtonDebounceTimer[button] <= 0)
						{
							unchecked
							{
								LmsInstance.Inst.UiInstance.ButtonState[button] &= (byte)~lms2012.BUTTON_ACTIVE;
							}
							LmsInstance.Inst.UiInstance.ButtonDebounceTimer[button] = 0;
						}
					}
				}

				if ((LmsInstance.Inst.UiInstance.ButtonState[button] & lms2012.BUTTON_ACTIVE) != 0)
				{
					if ((LmsInstance.Inst.UiInstance.ButtonState[button] & lms2012.BUTTON_PRESSED) == 0)
					{
						LmsInstance.Inst.UiInstance.Activated = lms2012.BUTTON_SET;
						LmsInstance.Inst.UiInstance.ButtonState[button] |= lms2012.BUTTON_PRESSED;
						LmsInstance.Inst.UiInstance.ButtonState[button] |= lms2012.BUTTON_ACTIVATED;
						LmsInstance.Inst.UiInstance.ButtonTimer[button] = 0;
						LmsInstance.Inst.UiInstance.ButtonRepeatTimer[button] = lms2012.BUTTON_START_REPEAT_TIME;
					}

					if (LmsInstance.Inst.UiInstance.ButtonRepeatTimer[button] > time)
					{
						LmsInstance.Inst.UiInstance.ButtonRepeatTimer[button] -= time;
					}
					else
					{
						if ((button != 1) && (button != 5))
						{
							LmsInstance.Inst.UiInstance.Activated |= lms2012.BUTTON_SET;
							LmsInstance.Inst.UiInstance.ButtonState[button] |= lms2012.BUTTON_ACTIVATED;
							LmsInstance.Inst.UiInstance.ButtonRepeatTimer[button] = lms2012.BUTTON_REPEAT_TIME;
						}
					}

					// long press
					LmsInstance.Inst.UiInstance.ButtonTimer[button] += time;
					if (LmsInstance.Inst.UiInstance.ButtonTimer[button] >= lms2012.LONG_PRESS_TIME)
					{
						if ((LmsInstance.Inst.UiInstance.ButtonState[button] & lms2012.BUTTON_LONG_LATCH) == 0)
						{
							LmsInstance.Inst.UiInstance.ButtonState[button] |= lms2012.BUTTON_LONG_LATCH;

							// WARNING: idk what is this
							if (button == 2)
							{
								LmsInstance.Inst.UiInstance.Activated |= lms2012.BUTTON_BUFPRINT;
							}
						}
						LmsInstance.Inst.UiInstance.ButtonState[button] |= lms2012.BUTTON_LONGPRESS;
					}
				}
				else
				{
					// released
					if ((LmsInstance.Inst.UiInstance.ButtonState[button] & lms2012.BUTTON_PRESSED) != 0)
					{
						unchecked
						{
							LmsInstance.Inst.UiInstance.ButtonState[button] &= (byte)~lms2012.BUTTON_PRESSED;
							LmsInstance.Inst.UiInstance.ButtonState[button] &= (byte)~lms2012.BUTTON_LONG_LATCH;
							LmsInstance.Inst.UiInstance.ButtonState[button] |= lms2012.BUTTON_BUMBED;
						}
					}
				}
			}
		}

		public lms2012.Result cUiUpdateInput()
		{
			char key = ' ';

			if (LmsInstance.Inst.GetTerminalEnabled())
			{
				if (LmsInstance.Inst.DterminalClass.dTerminalRead(ref key) == lms2012.Result.OK)
				{
					switch (key)
					{
						case ' ':
						case '<':
							LmsInstance.Inst.UiInstance.Escape = (byte)key;
							break;
						case '\r':
						case '\n':
							if (LmsInstance.Inst.UiInstance.KeyBufIn != 0)
							{
								LmsInstance.Inst.UiInstance.Keys = LmsInstance.Inst.UiInstance.KeyBufIn;
								LmsInstance.Inst.UiInstance.KeyBufIn = 0;
							}
							break;
						default:
							LmsInstance.Inst.UiInstance.KeyBuffer[LmsInstance.Inst.UiInstance.KeyBufIn] = (byte)key;
							if (++LmsInstance.Inst.UiInstance.KeyBufIn >= lms2012.KEYBUF_SIZE)
								LmsInstance.Inst.UiInstance.KeyBufIn--;
							LmsInstance.Inst.UiInstance.KeyBuffer[LmsInstance.Inst.UiInstance.KeyBufIn] = 0;
							break;
					}
				}
			}

			LmsInstance.Inst.DlcdClass.dLcdRead();

			return lms2012.Result.OK;
		}

		public byte cUiEscape()
		{
			byte result = LmsInstance.Inst.UiInstance.Escape;
			LmsInstance.Inst.UiInstance.Escape = 0;
			return result;
		}

		public void cUiTestpin(byte state)
		{
			LmsInstance.Inst.UiFileHandler.Write(UiFileHandler.POWER_FILE_HANDLER, [state], 1);
		}

		public byte AtoN(char ch)
		{
			byte tmp = 0;
			if (ch >= '0' && ch <= '9')
			{
				tmp = (byte)(ch - '0');
			}
			else
			{
				ch |= (char)0x20;
				if (ch >= 'a' && ch <= 'f')
				{
					tmp = (byte)((byte)(ch - 'a') + 10);
				}
			}
			return tmp;
		}

		public void cUiFlushBuffer()
		{
			if (LmsInstance.Inst.UiInstance.UiWrBufferSize != 0)
			{
				if (LmsInstance.Inst.GetTerminalEnabled())
				{
					LmsInstance.Inst.DterminalClass.dTerminalWrite(LmsInstance.Inst.UiInstance.UiWrBuffer, LmsInstance.Inst.UiInstance.UiWrBufferSize);
				}
				LmsInstance.Inst.UiInstance.UiWrBufferSize = 0;
			}
		}

		public void cUiWriteString(char[] str)
		{
			foreach (char c in str)
			{
				LmsInstance.Inst.UiInstance.UiWrBuffer[LmsInstance.Inst.UiInstance.UiWrBufferSize] = (byte)c;
				if (++LmsInstance.Inst.UiInstance.UiWrBufferSize >= lms2012.UI_WR_BUFFER_SIZE)
				{
					cUiFlushBuffer();
				}
			}
		}

		public byte cUiButtonRemap(byte mapped)
		{
			byte real;
			if (mapped >= 0 && mapped < lms2012.BUTTONTYPES)
			{
				real = MappedToReal.First(x => (byte)x.Key == mapped).Value;
			}
			else
			{
				real = lms2012.REAL_ANY_BUTTON;
			}
			return real;
		}

		public void cUiSetPress(byte button, byte press)
		{
			button = cUiButtonRemap(button);

			if (button < lms2012.BUTTONS)
			{
				if (press != 0)
					LmsInstance.Inst.UiInstance.ButtonState[button] |= lms2012.BUTTON_ACTIVE;
				else
					unchecked { LmsInstance.Inst.UiInstance.ButtonState[button] &= (byte)~lms2012.BUTTON_ACTIVE; }
			}
			else
			{
				if (button == lms2012.REAL_ANY_BUTTON)
				{
					if (press != 0)
					{
						for (button = 0; button < lms2012.BUTTONS; ++button)
						{
							LmsInstance.Inst.UiInstance.ButtonState[button] |= lms2012.BUTTON_ACTIVE;
						}
					}
					else
					{
						for (button = 0; button < lms2012.BUTTONS; ++button)
						{
							unchecked { LmsInstance.Inst.UiInstance.ButtonState[button] &= (byte)~lms2012.BUTTON_ACTIVE; }
						}
					}
				}
			}
		}

		public byte cUiGetPress(byte button)
		{
			byte result = 0;
			button = cUiButtonRemap(button);

			if (button < lms2012.BUTTONS)
			{
				if ((LmsInstance.Inst.UiInstance.ButtonState[button] & lms2012.BUTTON_PRESSED) != 0)
					result = 1;
			}
			else
			{
				if (button == lms2012.REAL_ANY_BUTTON)
				{
					for (button = 0; button < lms2012.BUTTONS; ++button)
					{
						if ((LmsInstance.Inst.UiInstance.ButtonState[button] & lms2012.BUTTON_PRESSED) != 0)
							result = 1;
					}
				}
			}
			return result;
		}

		public byte cUiTestShortPress(byte button)
		{
			byte result = 0;
			button = cUiButtonRemap(button);

			if (button < lms2012.BUTTONS)
			{
				if ((LmsInstance.Inst.UiInstance.ButtonState[button] & lms2012.BUTTON_ACTIVATED) != 0)
					result = 1;
			}
			else
			{
				if (button == lms2012.REAL_ANY_BUTTON)
				{
					for (button = 0; button < lms2012.BUTTONS; ++button)
					{
						if ((LmsInstance.Inst.UiInstance.ButtonState[button] & lms2012.BUTTON_ACTIVATED) != 0)
							result = 1;
					}
				}
				else if (button == lms2012.REAL_NO_BUTTON)
				{
					result = 1;
					for (button = 0; button < lms2012.BUTTONS; ++button)
					{
						if ((LmsInstance.Inst.UiInstance.ButtonState[button] & lms2012.BUTTON_ACTIVATED) != 0)
							result = 0;
					}
				}
			}
			return result;
		}

		public byte cUiGetShortPress(byte button)
		{
			byte result = 0;
			button = cUiButtonRemap(button);

			if (button < lms2012.BUTTONS)
			{
				if ((LmsInstance.Inst.UiInstance.ButtonState[button] & lms2012.BUTTON_ACTIVATED) != 0)
				{
					unchecked { LmsInstance.Inst.UiInstance.ButtonState[button] &= (byte)~lms2012.BUTTON_ACTIVATED; }
					result = 1;
				}
			}
			else
			{
				if (button == lms2012.REAL_ANY_BUTTON)
				{
					for (button = 0; button < lms2012.BUTTONS; ++button)
					{
						if ((LmsInstance.Inst.UiInstance.ButtonState[button] & lms2012.BUTTON_ACTIVATED) != 0)
						{
							unchecked { LmsInstance.Inst.UiInstance.ButtonState[button] &= (byte)~lms2012.BUTTON_ACTIVATED; }
							result = 1;
						}
					}
				}
				else if (button == lms2012.REAL_NO_BUTTON)
				{
					result = 1;
					for (button = 0; button < lms2012.BUTTONS; ++button)
					{
						if ((LmsInstance.Inst.UiInstance.ButtonState[button] & lms2012.BUTTON_ACTIVATED) != 0)
						{
							unchecked { LmsInstance.Inst.UiInstance.ButtonState[button] &= (byte)~lms2012.BUTTON_ACTIVATED; }
							result = 0;
						}
					}
				}
			}

			if (result != 0)
				LmsInstance.Inst.UiInstance.Click = 1;

			return result;
		}

		public byte cUiGetBumped(byte button)
		{
			byte result = 0;
			button = cUiButtonRemap(button);

			if (button < lms2012.BUTTONS)
			{
				if ((LmsInstance.Inst.UiInstance.ButtonState[button] & lms2012.BUTTON_BUMBED) != 0)
				{
					unchecked { LmsInstance.Inst.UiInstance.ButtonState[button] &= (byte)~lms2012.BUTTON_BUMBED; }
					result = 1;
				}
			}
			else
			{
				if (button == lms2012.REAL_ANY_BUTTON)
				{
					for (button = 0; button < lms2012.BUTTONS; ++button)
					{
						if ((LmsInstance.Inst.UiInstance.ButtonState[button] & lms2012.BUTTON_BUMBED) != 0)
						{
							unchecked { LmsInstance.Inst.UiInstance.ButtonState[button] &= (byte)~lms2012.BUTTON_BUMBED; }
							result = 1;
						}
					}
				}
				else if (button == lms2012.REAL_NO_BUTTON)
				{
					result = 1;
					for (button = 0; button < lms2012.BUTTONS; ++button)
					{
						if ((LmsInstance.Inst.UiInstance.ButtonState[button] & lms2012.BUTTON_BUMBED) != 0)
						{
							unchecked { LmsInstance.Inst.UiInstance.ButtonState[button] &= (byte)~lms2012.BUTTON_BUMBED; }
							result = 0;
						}
					}
				}
			}

			return result;
		}

		public byte cUiTestLongPress(byte button)
		{
			byte result = 0;
			button = cUiButtonRemap(button);

			if (button < lms2012.BUTTONS)
			{
				if ((LmsInstance.Inst.UiInstance.ButtonState[button] & lms2012.BUTTON_LONGPRESS) != 0)
					result = 1;
			}
			else
			{
				if (button == lms2012.REAL_ANY_BUTTON)
				{
					for (button = 0; button < lms2012.BUTTONS; ++button)
					{
						if ((LmsInstance.Inst.UiInstance.ButtonState[button] & lms2012.BUTTON_LONGPRESS) != 0)
							result = 1;
					}
				}
				else if (button == lms2012.REAL_NO_BUTTON)
				{
					result = 1;
					for (button = 0; button < lms2012.BUTTONS; ++button)
					{
						if ((LmsInstance.Inst.UiInstance.ButtonState[button] & lms2012.BUTTON_LONGPRESS) != 0)
							result = 0;
					}
				}
			}
			return result;
		}

		public byte cUiGetLongPress(byte button)
		{
			byte result = 0;
			button = cUiButtonRemap(button);

			if (button < lms2012.BUTTONS)
			{
				if ((LmsInstance.Inst.UiInstance.ButtonState[button] & lms2012.BUTTON_LONGPRESS) != 0)
				{
					unchecked { LmsInstance.Inst.UiInstance.ButtonState[button] &= (byte)~lms2012.BUTTON_LONGPRESS; }
					result = 1;
				}
			}
			else
			{
				if (button == lms2012.REAL_ANY_BUTTON)
				{
					for (button = 0; button < lms2012.BUTTONS; ++button)
					{
						if ((LmsInstance.Inst.UiInstance.ButtonState[button] & lms2012.BUTTON_LONGPRESS) != 0)
						{
							unchecked { LmsInstance.Inst.UiInstance.ButtonState[button] &= (byte)~lms2012.BUTTON_LONGPRESS; }
							result = 1;
						}
					}
				}
				else if (button == lms2012.REAL_NO_BUTTON)
				{
					result = 1;
					for (button = 0; button < lms2012.BUTTONS; ++button)
					{
						if ((LmsInstance.Inst.UiInstance.ButtonState[button] & lms2012.BUTTON_LONGPRESS) != 0)
						{
							unchecked { LmsInstance.Inst.UiInstance.ButtonState[button] &= (byte)~lms2012.BUTTON_LONGPRESS; }
							result = 0;
						}
					}
				}
			}

			if (result != 0)
				LmsInstance.Inst.UiInstance.Click = 1;

			return result;
		}

		public short cUiTestHorz()
		{
			short result = 0;
			if (cUiTestShortPress((byte)lms2012.ButtonType.LEFT_BUTTON) != 0)
				result = -1;
			if (cUiTestShortPress((byte)lms2012.ButtonType.RIGHT_BUTTON) != 0)
				result = 1;
			return result;
		}

		public short cUiGetHorz()
		{
			short result = 0;
			if (cUiGetShortPress((byte)lms2012.ButtonType.LEFT_BUTTON) != 0)
				result = -1;
			if (cUiGetShortPress((byte)lms2012.ButtonType.RIGHT_BUTTON) != 0)
				result = 1;
			return result;
		}

		public short cUiGetVert()
		{
			short result = 0;
			if (cUiGetShortPress((byte)lms2012.ButtonType.UP_BUTTON) != 0)
				result = -1;
			if (cUiGetShortPress((byte)lms2012.ButtonType.DOWN_BUTTON) != 0)
				result = 1;
			return result;
		}

		public byte cUiWaitForPress()
		{
			return cUiTestShortPress((byte)lms2012.ButtonType.ANY_BUTTON);
		}

		public short cUiAlignX(short x)
		{
			return (short)((x + 7) & ~7);
		}

		public void cUiUpdateCnt()
		{
			// TODO: probably not to do because it is probably a shite
		}

		public void cUiUpdatePower()
		{
			// TODO: probably not to do because it is probably a shite
		}

		void cUiUpdateTopline()
		{
			short x1, x2;
			short v;
			byte btStatus;
			byte wifiStatus;
			byte tmpStatus;
			byte[] name = new byte[lms2012.NAME_LENGTH + 1];

			if (LmsInstance.Inst.UiInstance.TopLineEnabled != 0)
			{
				LmsInstance.Inst.DlcdClass.LcdClearTopline(LmsInstance.Inst.UiInstance.Lcd);

				// show bt status
				tmpStatus = 0;
				btStatus = LmsInstance.Inst.CcomClass.cComGetBtStatus();
				if (btStatus > 0) 
				{
					tmpStatus = 1;
					btStatus >>= 1;
					if (btStatus >= 0 && btStatus < lms2012.TOP_BT_ICONS)
					{
                        LmsInstance.Inst.DlcdClass.dLcdDrawIcon(LmsInstance.Inst.UiInstance.Lcd, lms2012.FG_COLOR, 0, 1, lms2012.IconType.SMALL_ICON, TopLineWifiIconMap[btStatus]);
                    }
                }
				if (LmsInstance.Inst.UiInstance.BtOn != tmpStatus)
				{
					LmsInstance.Inst.UiInstance.BtOn = tmpStatus;
					LmsInstance.Inst.UiInstance.UiUpdate = 1;
                }

                // show wifi status
                tmpStatus = 0;
				wifiStatus = LmsInstance.Inst.CcomClass.cComGetWifiStatus();
                if (wifiStatus > 0)
                {
                    tmpStatus = 1;
                    wifiStatus >>= 1;
                    if (wifiStatus >= 0 && wifiStatus < lms2012.TOP_WIFI_ICONS)
                    {
                        LmsInstance.Inst.DlcdClass.dLcdDrawIcon(LmsInstance.Inst.UiInstance.Lcd, lms2012.FG_COLOR, 16, 1, lms2012.IconType.SMALL_ICON, TopLineWifiIconMap[wifiStatus]);
                    }
                }
                if (LmsInstance.Inst.UiInstance.WiFiOn != tmpStatus)
                {
                    LmsInstance.Inst.UiInstance.WiFiOn = tmpStatus;
                    LmsInstance.Inst.UiInstance.UiUpdate = 1;
                }

				// TODO: battery shite was here

				LmsInstance.Inst.CcomClass.cComGetBrickName(lms2012.NAME_LENGTH + 1, name);

				x1 = LmsInstance.Inst.DlcdClass.dLcdGetFontWidth(lms2012.FontType.SMALL_FONT);
				x2 = (short)(lms2012.LCD_WIDTH / x1);
				x2 -= (short)name.Length;
				x2 /= 2;
				x2 *= x1;
				LmsInstance.Inst.DlcdClass.dLcdDrawText(LmsInstance.Inst.UiInstance.Lcd, lms2012.FG_COLOR, x2, 1, lms2012.FontType.SMALL_FONT, name);

				x1 = LmsInstance.Inst.DlcdClass.dLcdGetIconWidth(lms2012.IconType.SMALL_ICON);
				x2 = (short)((lms2012.LCD_WIDTH - x1) / x1);

				// TODO: show usb status here

				// TODO: show battery was here

				LmsInstance.Inst.DlcdClass.dLcdDrawLine(LmsInstance.Inst.UiInstance.Lcd, lms2012.FG_COLOR, 0, lms2012.TOPLINE_HEIGHT, lms2012.LCD_WIDTH, lms2012.TOPLINE_HEIGHT);
            }
		}

		public void cUiUpdateLcd()
		{
			LmsInstance.Inst.UiInstance.Font = lms2012.FontType.NORMAL_FONT;
			cUiUpdateTopline();
			LmsInstance.Inst.DlcdClass.dLcdUpdate(LmsInstance.Inst.UiInstance.Lcd);
        }

		public void cUiRunScreen()
		{
			if (LmsInstance.Inst.UiInstance.ScreenBlocked == 0)
			{
				switch (LmsInstance.Inst.UiInstance.RunScreenEnabled)
				{
					case 0:
						break;
					case 1:
						LmsInstance.Inst.UiInstance.RunScreenEnabled++;
                        break;
                    case 2:
                        LmsInstance.Inst.UiInstance.RunScreenEnabled++;
                        break;
                    case 3:
                        LmsInstance.Inst.UiInstance.RunScreenNumber = 1;

						//clear
						LmsInstance.Inst.DlcdClass.LcdClear(LmsInstance.Inst.UiInstance.Lcd);
						cUiUpdateLcd();

						LmsInstance.Inst.DlcdClass.dLcdDrawPicture(LmsInstance.Inst.UiInstance.Lcd, lms2012.FG_COLOR, 8, 39, );
                        break;
                }
			}
		}
	}
}
