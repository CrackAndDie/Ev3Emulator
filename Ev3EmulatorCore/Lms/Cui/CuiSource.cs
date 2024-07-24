using EV3DecompilerLib.Decompile;
using Ev3EmulatorCore.Extensions;
using Ev3EmulatorCore.Helpers;
using Ev3EmulatorCore.Lms.Lms2012;
using System;

namespace Ev3EmulatorCore.Lms.Cui
{
	public partial class CuiClass
	{
		public void cUiDownloadSuccessSound()
		{
			UBYTE[] locals = new UBYTE[1];
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

						var mindstorms = LmsInstance.Inst.UiBmpHandler.Get(UiBmpHandler.BmpType.Mindstorms);
						LmsInstance.Inst.DlcdClass.dLcdDrawPicture(LmsInstance.Inst.UiInstance.Lcd, lms2012.FG_COLOR, 8, 39, mindstorms.Width, mindstorms.Height, mindstorms.Data);
						cUiUpdateLcd();

						LmsInstance.Inst.DlcdClass.dLcdDrawText(LmsInstance.Inst.UiInstance.Lcd, lms2012.FG_COLOR, 8, 79, lms2012.FontType.SMALL_FONT, LmsInstance.Inst.VmInstance.Program[(int)lms2012.Slot.USER_SLOT].Name);
						cUiUpdateLcd();

						LmsInstance.Inst.UiInstance.RunScreenTimer = 0;
						LmsInstance.Inst.UiInstance.RunScreenCounter = 0;

						LmsInstance.Inst.UiInstance.RunScreenEnabled++;
						break;
					case 4:
						if (LmsInstance.Inst.UiInstance.RunLedEnabled != 0)
						{
							cUiSetLed((byte)lms2012.LedPattern.LED_GREEN_PULSE);
						}

						var ani1x = LmsInstance.Inst.UiBmpHandler.Get(UiBmpHandler.BmpType.Ani1x);
						LmsInstance.Inst.DlcdClass.dLcdDrawPicture(LmsInstance.Inst.UiInstance.Lcd, lms2012.FG_COLOR, 8, 67, ani1x.Width, ani1x.Height, ani1x.Data);
						cUiUpdateLcd();

						LmsInstance.Inst.UiInstance.RunScreenTimer = LmsInstance.Inst.UiInstance.MilliSeconds;
						LmsInstance.Inst.UiInstance.RunScreenEnabled++;
						break;
					case 5:
					case 6:
					case 7:
					case 8:
						LmsInstance.Inst.UiInstance.RunScreenEnabled++;
						break;
					case 9:
						var ani2x = LmsInstance.Inst.UiBmpHandler.Get(UiBmpHandler.BmpType.Ani2x);
						LmsInstance.Inst.DlcdClass.dLcdDrawPicture(LmsInstance.Inst.UiInstance.Lcd, lms2012.FG_COLOR, 8, 67, ani2x.Width, ani2x.Height, ani2x.Data);
						cUiUpdateLcd();

						LmsInstance.Inst.UiInstance.RunScreenEnabled++;
						break;
					case 10:
						var ani3x = LmsInstance.Inst.UiBmpHandler.Get(UiBmpHandler.BmpType.Ani3x);
						LmsInstance.Inst.DlcdClass.dLcdDrawPicture(LmsInstance.Inst.UiInstance.Lcd, lms2012.FG_COLOR, 8, 67, ani3x.Width, ani3x.Height, ani3x.Data);
						cUiUpdateLcd();

						LmsInstance.Inst.UiInstance.RunScreenEnabled++;
						break;
					case 11:
						var ani4x = LmsInstance.Inst.UiBmpHandler.Get(UiBmpHandler.BmpType.Ani4x);
						LmsInstance.Inst.DlcdClass.dLcdDrawPicture(LmsInstance.Inst.UiInstance.Lcd, lms2012.FG_COLOR, 8, 67, ani4x.Width, ani4x.Height, ani4x.Data);
						cUiUpdateLcd();

						LmsInstance.Inst.UiInstance.RunScreenEnabled++;
						break;
					case 12:
						var ani5x = LmsInstance.Inst.UiBmpHandler.Get(UiBmpHandler.BmpType.Ani5x);
						LmsInstance.Inst.DlcdClass.dLcdDrawPicture(LmsInstance.Inst.UiInstance.Lcd, lms2012.FG_COLOR, 8, 67, ani5x.Width, ani5x.Height, ani5x.Data);
						cUiUpdateLcd();

						LmsInstance.Inst.UiInstance.RunScreenEnabled++;
						break;
					default:
						var ani6x = LmsInstance.Inst.UiBmpHandler.Get(UiBmpHandler.BmpType.Ani6x);
						LmsInstance.Inst.DlcdClass.dLcdDrawPicture(LmsInstance.Inst.UiInstance.Lcd, lms2012.FG_COLOR, 8, 67, ani6x.Width, ani6x.Height, ani6x.Data);
						cUiUpdateLcd();

						LmsInstance.Inst.UiInstance.RunScreenEnabled = 4;
						break;
				}
			}
		}

		// TODO cUiCheckVoltage(void) was here
		// TODO cUiCheckPower(UWORD Time) was here
		// TODO cUiCheckTemp(void) was here
		// TODO cUiCheckMemory(void) was here

		public void cUiCheckAlive(ushort time)
		{
			ulong timeout;

			LmsInstance.Inst.UiInstance.SleepTimer += time;

			if ((LmsInstance.Inst.UiInstance.Activated & lms2012.BUTTON_ALIVE) != 0)
			{
				unchecked { LmsInstance.Inst.UiInstance.Activated &= (byte)~lms2012.BUTTON_ALIVE; }
				cUiAlive();
			}

			timeout = LmsInstance.Inst.GetSleepMinutes();

			if (timeout != 0)
			{
				if (LmsInstance.Inst.UiInstance.SleepTimer >= (timeout * 60000L))
				{
					LmsInstance.Inst.UiInstance.ShutDown = 1;
				}
			}
		}

		public void cUiUpdate(ushort time)
		{
			byte warning;
			byte tmp;

			LmsInstance.Inst.UiInstance.MilliSeconds += time;

			cUiUpdateButtons(time);
			cUiUpdateInput();
			cUiUpdateCnt();

			if (LmsInstance.Inst.UiInstance.MilliSeconds - LmsInstance.Inst.UiInstance.UpdateStateTimer >= 50)
			{
				LmsInstance.Inst.UiInstance.UpdateStateTimer = LmsInstance.Inst.UiInstance.MilliSeconds;

				if (LmsInstance.Inst.UiInstance.Event == 0)
				{
					LmsInstance.Inst.UiInstance.Event = LmsInstance.Inst.CcomClass.cComGetEvent();
				}

				switch (LmsInstance.Inst.UiInstance.UpdateState++)
				{
					case 0:
						// TODO some checks were here
						break;
					case 1:
						cUiRunScreen();
						break;
					case 2:
						cUiCheckAlive(400);
						// TODO some checks were here
						break;
					case 3:
						cUiRunScreen();
						break;
					case 4:
						// TODO some checks were here
						break;
					case 5:
						cUiRunScreen();
						break;
					case 6:
						if (LmsInstance.Inst.UiInstance.ScreenBusy == 0)
						{
							cUiUpdateTopline();
							LmsInstance.Inst.DlcdClass.dLcdUpdate(LmsInstance.Inst.UiInstance.Lcd);
						}
						break;
					default:
						cUiRunScreen();
						LmsInstance.Inst.UiInstance.UpdateState = 0;
						LmsInstance.Inst.UiInstance.ReadyForWarnings = 1;
						break;
				}

				if (LmsInstance.Inst.UiInstance.Warning != 0)
				{
					if (LmsInstance.Inst.UiInstance.Warnlight == 0)
					{
						LmsInstance.Inst.UiInstance.Warnlight = 1;
						cUiSetLed(LmsInstance.Inst.UiInstance.LedState);
					}
				}
				else
				{
					if (LmsInstance.Inst.UiInstance.Warnlight != 0)
					{
						LmsInstance.Inst.UiInstance.Warnlight = 0;
						cUiSetLed(LmsInstance.Inst.UiInstance.LedState);
					}
				}

				warning = (byte)(LmsInstance.Inst.UiInstance.Warning & (byte)lms2012.Warning.WARNINGS);
				tmp = (byte)(warning & ~LmsInstance.Inst.UiInstance.WarningShowed);

				if (tmp != 0)
				{
					// TODO!!!! showing warnings
					// https://github.com/mindboards/ev3sources/blob/master/lms2012/c_ui/source/c_ui.c#L2622
				}

				// Find warnings that have been showed but not confirmed
				tmp = LmsInstance.Inst.UiInstance.WarningShowed;
				tmp &= (byte)(~LmsInstance.Inst.UiInstance.WarningShowed);

				if (tmp != 0)
				{
					// TODO!!!! showing warnings
					// https://github.com/mindboards/ev3sources/blob/master/lms2012/c_ui/source/c_ui.c#L2693
				}

				// Find warnings that have been showed, confirmed and removed
				tmp = (byte)~warning;
				tmp &= (byte)(lms2012.Warning.WARNINGS);
				tmp &= (byte)(LmsInstance.Inst.UiInstance.WarningShowed);
				tmp &= (byte)(LmsInstance.Inst.UiInstance.WarningConfirmed);

				unchecked
				{
					LmsInstance.Inst.UiInstance.WarningShowed &= (byte)~tmp;
					LmsInstance.Inst.UiInstance.WarningConfirmed &= (byte)~tmp;
				}
			}
		}

		public lms2012.Result cUiNotification(byte color, short x, short y, lms2012.NIcon icon1, lms2012.NIcon icon2, lms2012.NIcon icon3, byte[] text, ref byte state)
		{
			lms2012.Result result = lms2012.Result.BUSY;
			NOTIFY pQ;
			short availableX;
			short usedX;
			short line;
			short charIn;
			short charOut;
			byte character;
			short x2;
			short y2;
			short availableY;
			short usedY;

			var pop3 = LmsInstance.Inst.UiBmpHandler.Get(UiBmpHandler.BmpType.POP3);
			pQ = LmsInstance.Inst.UiInstance.Notify;

			if (state == 0)
			{
				state = 1;
				pQ.ScreenStartX = x;
				pQ.ScreenStartY = y;
				pQ.ScreenWidth = pop3.Width;
				pQ.ScreenHeight = pop3.Height;
				pQ.IconStartY = (short)(pQ.ScreenStartY + 10);
				pQ.IconWidth = LmsInstance.Inst.DlcdClass.dLcdGetIconWidth(lms2012.IconType.LARGE_ICON);
				pQ.IconHeight = LmsInstance.Inst.DlcdClass.dLcdGetIconHeight(lms2012.IconType.LARGE_ICON);
				pQ.IconSpaceX = pQ.IconWidth;

				pQ.YesNoStartX = (short)(pQ.ScreenStartX + (pQ.ScreenWidth / 2));
				pQ.YesNoStartX -= (short)((pQ.IconWidth + 8) / 2);
				pQ.YesNoStartX = cUiAlignX(pQ.YesNoStartX);
				pQ.YesNoStartY = (short)(pQ.ScreenStartY + 40);

				pQ.LineStartX = (short)(pQ.ScreenStartX + 5);
				pQ.LineStartY = (short)(pQ.ScreenStartY + 39);
				pQ.LineEndX = (short)(pQ.LineStartX + 134);

				pQ.NoOfIcons = 0;
				if (icon1 > lms2012.NIcon.ICON_NONE)
				{
					pQ.NoOfIcons++;
				}
				if (icon2 > lms2012.NIcon.ICON_NONE)
				{
					pQ.NoOfIcons++;
				}
				if (icon3 > lms2012.NIcon.ICON_NONE)
				{
					pQ.NoOfIcons++;
				}

				pQ.TextLines = 0;
				if (text.Length > 0)
				{
					pQ.IconStartX = (short)(pQ.ScreenStartX + 8);
					pQ.IconStartX = cUiAlignX(pQ.IconStartX);

					availableX = pQ.ScreenWidth;
					availableX -= (short)((pQ.IconStartX - pQ.ScreenStartX) * 2);

					availableX -= (short)(pQ.NoOfIcons * pQ.IconSpaceX);

					pQ.NoOfChars = (short)text.Length;

					pQ.Font = lms2012.FontType.SMALL_FONT;
					pQ.FontWidth = LmsInstance.Inst.DlcdClass.dLcdGetFontWidth(lms2012.FontType.SMALL_FONT);
					usedX = (short)(pQ.FontWidth * pQ.NoOfChars);
					line = 0;

					if (usedX <= availableX)
					{
						if (availableX - usedX >= 32)
						{
							pQ.IconStartX += 32;
						}

						pQ.TextLine[line].WriteBytes(text);
						line++;
						pQ.TextLines++;

						pQ.TextStartX = (short)(pQ.IconStartX + (pQ.NoOfIcons * pQ.IconSpaceX));
						pQ.TextStartY = (short)(pQ.ScreenStartY + 18);
						pQ.TextSpaceY = (short)(LmsInstance.Inst.DlcdClass.dLcdGetFontHeight(pQ.Font) + 1);
					}
					else
					{
						pQ.Font = lms2012.FontType.TINY_FONT;
						pQ.FontWidth = LmsInstance.Inst.DlcdClass.dLcdGetFontWidth(lms2012.FontType.TINY_FONT);
						usedX = (short)(pQ.FontWidth * pQ.NoOfChars);
						availableX -= (short)(pQ.FontWidth);
						charIn = 0;

						while ((text[charIn] != 0) && (line < lms2012.MAX_NOTIFY_LINES))
						{
							charOut = 0;
							usedX = 0;
							while ((text[charIn] != 0) && (charOut < lms2012.MAX_NOTIFY_LINE_CHARS) && (usedX < (availableX - pQ.FontWidth)))
							{
								character = text[charIn];
								if (character == '_')
								{
									character = (byte)' ';
								}
								pQ.TextLine[line][charOut] = character;
								charIn++;
								charOut++;
								usedX += pQ.FontWidth;
							}
							while ((charOut > 8) && (text[charIn] != ' ') && (text[charIn] != '_') && (text[charIn] != 0))
							{
								charIn--;
								charOut--;
							}
							if (text[charIn] != 0)
							{
								charIn++;
							}
							pQ.TextLine[line][charOut] = 0;
							line++;
						}

						pQ.TextLines = line;

						pQ.TextStartX = (short)(pQ.IconStartX + (pQ.NoOfIcons * pQ.IconSpaceX) + pQ.FontWidth);
						pQ.TextSpaceY = (short)(LmsInstance.Inst.DlcdClass.dLcdGetFontHeight(pQ.Font) + 1);


						availableY = (short)(pQ.LineStartY - (pQ.ScreenStartY + 5));
						usedY = (short)(pQ.TextLines * pQ.TextSpaceY);

						while (usedY > availableY)
						{
							pQ.TextLines--;
							usedY = (short)(pQ.TextLines * pQ.TextSpaceY);
						}
						y2 = (short)((availableY - usedY) / 2);

						pQ.TextStartY = (short)(pQ.ScreenStartY + y2 + 5);
					}
				}
				else
				{
					pQ.IconStartX = (short)(pQ.ScreenStartX + (pQ.ScreenWidth / 2));
					pQ.IconStartX -= (short)((pQ.IconWidth + 8) / 2);
					pQ.IconStartX -= (short)((pQ.NoOfIcons / 2) * pQ.IconWidth);
					pQ.IconStartX = (short)(cUiAlignX(pQ.IconStartX));
					pQ.TextStartY = (short)(pQ.ScreenStartY + 8);
				}
				pQ.NeedUpdate = 1;
			}

			if (pQ.NeedUpdate != 0)
			{
				//* UPDATE ***************************************************************************************************
				pQ.NeedUpdate = 0;

				LmsInstance.Inst.DlcdClass.dLcdDrawPicture(LmsInstance.Inst.UiInstance.Lcd, color, pQ.ScreenStartX, pQ.ScreenStartY, pop3.Width, pop3.Height, pop3.Data);

				x2 = pQ.IconStartX;

				if (icon1 > lms2012.NIcon.ICON_NONE)
				{
					LmsInstance.Inst.DlcdClass.dLcdDrawIcon(LmsInstance.Inst.UiInstance.Lcd, color, x2, pQ.IconStartY, lms2012.IconType.LARGE_ICON, (sbyte)icon1);
					x2 += pQ.IconSpaceX;
				}
				if (icon2 > lms2012.NIcon.ICON_NONE)
				{
					LmsInstance.Inst.DlcdClass.dLcdDrawIcon(LmsInstance.Inst.UiInstance.Lcd, color, x2, pQ.IconStartY, lms2012.IconType.LARGE_ICON, (sbyte)icon2);
					x2 += pQ.IconSpaceX;
				}
				if (icon3 > lms2012.NIcon.ICON_NONE)
				{
					LmsInstance.Inst.DlcdClass.dLcdDrawIcon(LmsInstance.Inst.UiInstance.Lcd, color, x2, pQ.IconStartY, lms2012.IconType.LARGE_ICON, (sbyte)icon3);
					x2 += pQ.IconSpaceX;
				}

				line = 0;
				y2 = pQ.TextStartY;
				while (line < pQ.TextLines)
				{
					LmsInstance.Inst.DlcdClass.dLcdDrawText(LmsInstance.Inst.UiInstance.Lcd, color, pQ.TextStartX, y2, pQ.Font, pQ.TextLine[line]);
					y2 += pQ.TextSpaceY;
					line++;
				}

				LmsInstance.Inst.DlcdClass.dLcdDrawLine(LmsInstance.Inst.UiInstance.Lcd, color, pQ.LineStartX, pQ.LineStartY, pQ.LineEndX, pQ.LineStartY);

				LmsInstance.Inst.DlcdClass.dLcdDrawIcon(LmsInstance.Inst.UiInstance.Lcd, color, pQ.YesNoStartX, pQ.YesNoStartY, lms2012.IconType.LARGE_ICON, (byte)lms2012.LIcon.YES_SEL);

				cUiUpdateLcd();
				LmsInstance.Inst.UiInstance.ScreenBusy = 0;
			}

			if (cUiGetShortPress((byte)lms2012.ButtonType.ENTER_BUTTON) != 0)
			{
				cUiButtonFlush();
				result = lms2012.Result.OK;
				state = 0;
			}

			return result;
		}

		lms2012.Result cUiQuestion(byte Color, short X, short Y, lms2012.NIcon Icon1, lms2012.NIcon Icon2, byte[] pText, ref byte pState, sbyte[] pAnswer)
		{
			lms2012.Result Result = lms2012.Result.BUSY;
			TQUESTION pQ;
			short Inc;

			var pop3 = LmsInstance.Inst.UiBmpHandler.Get(UiBmpHandler.BmpType.POP3);
			pQ = LmsInstance.Inst.UiInstance.Question;

			Inc = cUiGetHorz();
			if (Inc != 0)
			{
				pQ.NeedUpdate = 1;

				pAnswer[0] += (sbyte)Inc;

				if (pAnswer[0] > 1)
				{
					pAnswer[0] = 1;
					pQ.NeedUpdate = 0;
				}
				if (pAnswer[0] < 0)
				{
					pAnswer[0] = 0;
					pQ.NeedUpdate = 0;
				}
			}

			if (pState == 0)
			{
				pState = 1;
				pQ.ScreenStartX = X;
				pQ.ScreenStartY = Y;
				pQ.IconWidth = LmsInstance.Inst.DlcdClass.dLcdGetIconWidth(lms2012.IconType.LARGE_ICON);
				pQ.IconHeight = LmsInstance.Inst.DlcdClass.dLcdGetIconHeight(lms2012.IconType.LARGE_ICON);

				pQ.NoOfIcons = 0;
				if (Icon1 > lms2012.NIcon.ICON_NONE)
				{
					pQ.NoOfIcons++;
				}
				if (Icon2 > lms2012.NIcon.ICON_NONE)
				{
					pQ.NoOfIcons++;
				}
				pQ.Default = (byte)pAnswer[0];

				pQ.NeedUpdate = 1;
			}


			if (pQ.NeedUpdate != 0)
			{
				//* UPDATE ***************************************************************************************************
				pQ.NeedUpdate = 0;

				LmsInstance.Inst.DlcdClass.dLcdDrawPicture(LmsInstance.Inst.UiInstance.Lcd, Color, pQ.ScreenStartX, pQ.ScreenStartY, pop3.Width, pop3.Height, pop3.Data);
				pQ.ScreenWidth = pop3.Width;
				pQ.ScreenHeight = pop3.Height;

				pQ.IconStartX = (short)(pQ.ScreenStartX + (pQ.ScreenWidth / 2));
				if (pQ.NoOfIcons > 1)
				{
					pQ.IconStartX -= pQ.IconWidth;
				}
				else
				{
					pQ.IconStartX -= (short)(pQ.IconWidth / 2);
				}
				pQ.IconStartX = cUiAlignX(pQ.IconStartX);
				pQ.IconSpaceX = pQ.IconWidth;
				pQ.IconStartY = (short)(pQ.ScreenStartY + 10);

				pQ.YesNoStartX = (short)(pQ.ScreenStartX + (pQ.ScreenWidth / 2));
				pQ.YesNoStartX -= 8;
				pQ.YesNoStartX -= pQ.IconWidth;
				pQ.YesNoStartX = cUiAlignX(pQ.YesNoStartX);
				pQ.YesNoSpaceX = (short)(pQ.IconWidth + 16);
				pQ.YesNoStartY = (short)(pQ.ScreenStartY + 40);

				pQ.LineStartX = (short)(pQ.ScreenStartX + 5);
				pQ.LineStartY = (short)(pQ.ScreenStartY + 39);
				pQ.LineEndX = (short)(pQ.LineStartX + 134);

				switch (pQ.NoOfIcons)
				{
					case 1:
						{
							LmsInstance.Inst.DlcdClass.dLcdDrawIcon(LmsInstance.Inst.UiInstance.Lcd, Color, pQ.IconStartX, pQ.IconStartY, lms2012.IconType.LARGE_ICON, (byte)Icon1);
						}
						break;

					case 2:
						{
							LmsInstance.Inst.DlcdClass.dLcdDrawIcon(LmsInstance.Inst.UiInstance.Lcd, Color, pQ.IconStartX, pQ.IconStartY, lms2012.IconType.LARGE_ICON, (byte)Icon1);
							LmsInstance.Inst.DlcdClass.dLcdDrawIcon(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(pQ.IconStartX + pQ.IconSpaceX), pQ.IconStartY, lms2012.IconType.LARGE_ICON, (byte)Icon2);
						}
						break;

				}

				if (pAnswer[0] == 0)
				{
					LmsInstance.Inst.DlcdClass.dLcdDrawIcon(LmsInstance.Inst.UiInstance.Lcd, Color, pQ.YesNoStartX, pQ.YesNoStartY, lms2012.IconType.LARGE_ICON, (int)lms2012.LIcon.NO_SEL);
					LmsInstance.Inst.DlcdClass.dLcdDrawIcon(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(pQ.YesNoStartX + pQ.YesNoSpaceX), pQ.YesNoStartY, lms2012.IconType.LARGE_ICON, (int)lms2012.LIcon.YES_NOTSEL);
				}
				else
				{
					LmsInstance.Inst.DlcdClass.dLcdDrawIcon(LmsInstance.Inst.UiInstance.Lcd, Color, pQ.YesNoStartX, pQ.YesNoStartY, lms2012.IconType.LARGE_ICON, (int)lms2012.LIcon.NO_NOTSEL);
					LmsInstance.Inst.DlcdClass.dLcdDrawIcon(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(pQ.YesNoStartX + pQ.YesNoSpaceX), pQ.YesNoStartY, lms2012.IconType.LARGE_ICON, (int)lms2012.LIcon.YES_SEL);
				}

				LmsInstance.Inst.DlcdClass.dLcdDrawLine(LmsInstance.Inst.UiInstance.Lcd, Color, pQ.LineStartX, pQ.LineStartY, pQ.LineEndX, pQ.LineStartY);

				cUiUpdateLcd();
				LmsInstance.Inst.UiInstance.ScreenBusy = 0;
			}
			if (cUiGetShortPress((byte)lms2012.ButtonType.ENTER_BUTTON) != 0)
			{
				cUiButtonFlush();
				Result = lms2012.Result.OK;
				pState = 0;
			}
			if (cUiGetShortPress((byte)lms2012.ButtonType.BACK_BUTTON) != 0)
			{
				cUiButtonFlush();
				Result = lms2012.Result.OK;
				pState = 0;
				pAnswer[0] = -1;
			}

			return (Result);
		}

		lms2012.Result cUiIconQuestion(byte Color, short X, short Y, ref byte pState, ref int pIcons)
		{
			lms2012.Result Result = lms2012.Result.BUSY;
			IQUESTION pQ;
			int Mask;
			int TmpIcons;
			short Tmp;
			short Loop;
			byte Icon;

			var pop2 = LmsInstance.Inst.UiBmpHandler.Get(UiBmpHandler.BmpType.POP2);
			pQ = LmsInstance.Inst.UiInstance.IconQuestion;

			if (pState == 0)
			{
				pState = 1;
				pQ.ScreenStartX = X;
				pQ.ScreenStartY = Y;
				pQ.ScreenWidth = pop2.Width;
				pQ.ScreenHeight = pop2.Height;
				pQ.IconWidth = LmsInstance.Inst.DlcdClass.dLcdGetIconWidth(lms2012.IconType.LARGE_ICON);
				pQ.IconHeight = LmsInstance.Inst.DlcdClass.dLcdGetIconHeight(lms2012.IconType.LARGE_ICON);
				pQ.Frame = 2;
				pQ.Icons = pIcons;
				pQ.NoOfIcons = 0;
				pQ.PointerX = 0;

				TmpIcons = pQ.Icons;
				while (TmpIcons != 0)
				{
					if ((TmpIcons & 1) != 0)
					{
						pQ.NoOfIcons++;
					}
					TmpIcons >>= 1;
				}

				if (pQ.NoOfIcons != 0)
				{
					pQ.IconStartY = (short)(pQ.ScreenStartY + ((pQ.ScreenHeight - pQ.IconHeight) / 2));

					pQ.IconSpaceX = (short)(((pQ.ScreenWidth - (pQ.IconWidth * pQ.NoOfIcons)) / pQ.NoOfIcons) + pQ.IconWidth);
					pQ.IconSpaceX = (short)(pQ.IconSpaceX & ~7);

					Tmp = (short)(pQ.IconSpaceX * pQ.NoOfIcons - (pQ.IconSpaceX - pQ.IconWidth));

					pQ.IconStartX = (short)(pQ.ScreenStartX + ((pQ.ScreenWidth - Tmp) / 2));
					pQ.IconStartX = (short)((pQ.IconStartX + 7) & ~7);

					pQ.SelectStartX = (short)(pQ.IconStartX - 1);
					pQ.SelectStartY = (short)(pQ.IconStartY - 1);
					pQ.SelectWidth = (short)(pQ.IconWidth + 2);
					pQ.SelectHeight = (short)(pQ.IconHeight + 2);
					pQ.SelectSpaceX = pQ.IconSpaceX;
				}
				pQ.NeedUpdate = 1;
			}

			if (pQ.NoOfIcons != 0)
			{
				// Check for move pointer
				Tmp = cUiGetHorz();
				if (Tmp != 0)
				{
					pQ.PointerX += Tmp;

					pQ.NeedUpdate = 1;

					if (pQ.PointerX < 0)
					{
						pQ.PointerX = 0;
						pQ.NeedUpdate = 0;
					}
					if (pQ.PointerX >= pQ.NoOfIcons)
					{
						pQ.PointerX = (short)(pQ.NoOfIcons - 1);
						pQ.NeedUpdate = 0;
					}
				}
			}

			if (pQ.NeedUpdate != 0)
			{
				//* UPDATE ***************************************************************************************************
				pQ.NeedUpdate = 0;

				LmsInstance.Inst.DlcdClass.dLcdDrawPicture(LmsInstance.Inst.UiInstance.Lcd, Color, pQ.ScreenStartX, pQ.ScreenStartY, pop2.Width, pop2.Height, pop2.Data);
				pQ.ScreenWidth = pop2.Width;
				pQ.ScreenHeight = pop2.Height;

				// Show icons
				Loop = 0;
				Icon = 0;
				TmpIcons = pQ.Icons;
				while (Loop < pQ.NoOfIcons)
				{
					while ((TmpIcons & 1) == 0)
					{
						Icon++;
						TmpIcons >>= 1;
					}
					LmsInstance.Inst.DlcdClass.dLcdDrawIcon(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(pQ.IconStartX + pQ.IconSpaceX * Loop), pQ.IconStartY, lms2012.IconType.LARGE_ICON, Icon);
					Loop++;
					Icon++;
					TmpIcons >>= 1;
				}

				// Show selection
				LmsInstance.Inst.DlcdClass.dLcdInverseRect(LmsInstance.Inst.UiInstance.Lcd, (short)(pQ.SelectStartX + pQ.SelectSpaceX * pQ.PointerX), pQ.SelectStartY, pQ.SelectWidth, pQ.SelectHeight);

				// Update screen
				cUiUpdateLcd();
				LmsInstance.Inst.UiInstance.ScreenBusy = 0;
			}
			if (cUiGetShortPress((byte)lms2012.ButtonType.ENTER_BUTTON) != 0)
			{
				if (pQ.NoOfIcons != 0)
				{
					Mask = 0x00000001;
					TmpIcons = pQ.Icons;
					Loop = (short)(pQ.PointerX + 1);

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
					pIcons = Mask;
				}
				else
				{
					pIcons = 0;
				}
				cUiButtonFlush();
				Result = lms2012.Result.OK;
				pState = 0;
			}
			if (cUiGetShortPress((byte)lms2012.ButtonType.BACK_BUTTON) != 0)
			{
				pIcons = 0;
				cUiButtonFlush();
				Result = lms2012.Result.OK;
				pState = 0;
			}
			return (Result);
		}
		byte cUiKeyboard(byte Color, short X, short Y, lms2012.NIcon Icon, byte Lng, byte[] pText, ref byte pCharSet, byte[] pAnswer)
		{
			KEYB pK;
			short Width;
			short Height;
			short Inc;
			short SX;
			short SY;
			short X3;
			short X4;
			short Tmp;
			byte TmpChar;
			byte SelectedChar = 0;

			var kbs = LmsInstance.Inst.UiBmpHandler.Get(UiBmpHandler.BmpType.KeyboardSmp);
			var kbc = LmsInstance.Inst.UiBmpHandler.Get(UiBmpHandler.BmpType.KeyboardCap);
			var kbn = LmsInstance.Inst.UiBmpHandler.Get(UiBmpHandler.BmpType.KeyboardNum);

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

			byte[][][] KeyboardLayout = new byte[lms2012.MAX_KEYB_DEEPT][][];
			byte[][] lay1 = new byte[lms2012.MAX_KEYB_HEIGHT][];
			byte[][] lay2 = new byte[lms2012.MAX_KEYB_HEIGHT][];
			byte[][] lay3 = new byte[lms2012.MAX_KEYB_HEIGHT][];

			lay1[0] = new byte[lms2012.MAX_KEYB_WIDTH] { (byte)'Q', (byte)'W', (byte)'E', (byte)'R', (byte)'T', (byte)'Y', (byte)'U', (byte)'I', (byte)'O', (byte)'P', 0x08, 0x00 };
			lay1[1] = new byte[lms2012.MAX_KEYB_WIDTH] { 0x03, (byte)'A', (byte)'S', (byte)'D', (byte)'F', (byte)'G', (byte)'H', (byte)'J', (byte)'K', (byte)'L', 0x0D, 0x00 };
			lay1[2] = new byte[lms2012.MAX_KEYB_WIDTH] { 0x01, (byte)'Z', (byte)'X', (byte)'C', (byte)'V', (byte)'B', (byte)'N', (byte)'M', 0x0D, 0x0D, 0x0D, 0x00 };
			lay1[3] = new byte[lms2012.MAX_KEYB_WIDTH] { (byte)' ', (byte)' ', (byte)' ', (byte)' ', (byte)' ', (byte)' ', (byte)' ', (byte)' ', (byte)' ', (byte)' ', 0x0D, 0x00 };

			lay2[0] = new byte[lms2012.MAX_KEYB_WIDTH] { (byte)'q', (byte)'w', (byte)'e', (byte)'r', (byte)'t', (byte)'y', (byte)'u', (byte)'i', (byte)'o', (byte)'p', 0x08, 0x00 };
			lay2[1] = new byte[lms2012.MAX_KEYB_WIDTH] { 0x03, (byte)'a', (byte)'s', (byte)'d', (byte)'f', (byte)'g', (byte)'h', (byte)'j', (byte)'k', (byte)'l', 0x0D, 0x00 };
			lay2[2] = new byte[lms2012.MAX_KEYB_WIDTH] { 0x01, (byte)'z', (byte)'x', (byte)'c', (byte)'v', (byte)'b', (byte)'n', (byte)'m', 0x0D, 0x0D, 0x0D, 0x00 };
			lay2[3] = new byte[lms2012.MAX_KEYB_WIDTH] { (byte)' ', (byte)' ', (byte)' ', (byte)' ', (byte)' ', (byte)' ', (byte)' ', (byte)' ', (byte)' ', (byte)' ', 0x0D, 0x00 };

			lay3[0] = new byte[lms2012.MAX_KEYB_WIDTH] { (byte)'1', (byte)'2', (byte)'3', (byte)'4', (byte)'5', (byte)'6', (byte)'7', (byte)'8', (byte)'9', (byte)'0', 0x08, 0x00 };
			lay3[1] = new byte[lms2012.MAX_KEYB_WIDTH] { 0x04, (byte)'+', (byte)'-', (byte)'=', (byte)'<', (byte)'>', (byte)'/', (byte)'\\', (byte)'*', (byte)':', 0x0D, 0x00 };
			lay3[2] = new byte[lms2012.MAX_KEYB_WIDTH] { 0x04, (byte)'(', (byte)')', (byte)'_', (byte)'.', (byte)'@', (byte)'!', (byte)'?', 0x0D, 0x0D, 0x0D, 0x00 };
			lay3[3] = new byte[lms2012.MAX_KEYB_WIDTH] { (byte)' ', (byte)' ', (byte)' ', (byte)' ', (byte)' ', (byte)' ', (byte)' ', (byte)' ', (byte)' ', (byte)' ', 0x0D, 0x00 };

			KeyboardLayout[0] = lay1; KeyboardLayout[1] = lay2; KeyboardLayout[2] = lay3;

			pK = LmsInstance.Inst.UiInstance.Keyboard;

			if (pCharSet != 0)
			{
				pK.CharSet = pCharSet;
				pCharSet = 0;
				pK.ScreenStartX = X;
				pK.ScreenStartY = Y;

				if ((Icon >= 0) && (Icon < lms2012.NIcon.ICON_BRICK1))
				{
					pK.IconStartX = cUiAlignX((short)(pK.ScreenStartX + 7));
					pK.IconStartY = (short)(pK.ScreenStartY + 4);
					pK.TextStartX = (short)(pK.IconStartX + LmsInstance.Inst.DlcdClass.dLcdGetIconWidth(lms2012.IconType.NORMAL_ICON));
				}
				else
				{
					pK.TextStartX = cUiAlignX((short)(pK.ScreenStartX + 9));
				}
				pK.TextStartY = (short)(pK.ScreenStartY + 7);
				pK.StringStartX = (short)(pK.ScreenStartX + 8);
				pK.StringStartY = (short)(pK.ScreenStartY + 22);
				pK.KeybStartX = (short)(pK.ScreenStartX + 13);
				pK.KeybStartY = (short)(pK.ScreenStartY + 40);
				pK.KeybSpaceX = 11;
				pK.KeybSpaceY = 14;
				pK.KeybHeight = 13;
				pK.KeybWidth = 9;
				pK.Layout = 0;
				pK.PointerX = 10;
				pK.PointerY = 1;
				pK.NeedUpdate = 1;
			}

			Width = (short)(KeyboardLayout[pK.Layout][pK.PointerY].Length - 1);
			Height = lms2012.MAX_KEYB_HEIGHT - 1;

			Inc = cUiGetHorz();
			pK.PointerX += Inc;
			if (pK.PointerX < 0)
			{
				pK.PointerX = 0;
			}
			if (pK.PointerX > Width)
			{
				pK.PointerX = Width;
			}
			Inc = cUiGetVert();
			pK.PointerY += Inc;
			if (pK.PointerY < 0)
			{
				pK.PointerY = 0;
			}
			if (pK.PointerY > Height)
			{
				pK.PointerY = Height;
			}

			TmpChar = KeyboardLayout[pK.Layout][pK.PointerY][pK.PointerX];

			if (cUiGetShortPress((byte)lms2012.ButtonType.BACK_BUTTON) != 0)
			{
				SelectedChar = 0x0D;
				pAnswer[0] = 0;
			}
			if (cUiGetShortPress((byte)lms2012.ButtonType.ENTER_BUTTON) != 0)
			{
				SelectedChar = TmpChar;
				switch (SelectedChar)
				{
					case 0x01:
						{
							pK.Layout = 2;
						}
						break;
					case 0x02:
						{
							pK.Layout = 0;
						}
						break;
					case 0x03:
						{
							pK.Layout = 1;
						}
						break;
					case 0x04:
						{
							pK.Layout = 0;
						}
						break;
					case 0x08:
						{
							Tmp = (short)pAnswer.Length;
							if (Tmp != 0)
							{
								Tmp--;
								pAnswer[Tmp] = 0;
							}
						}
						break;
					case (byte)'\r':
						{
						}
						break;
					default:
						{
							var tmpArr = new byte[] { SelectedChar };
							if (LmsInstance.Inst.ValidateChar(tmpArr, pK.CharSet) == lms2012.Result.OK)
							{
								Tmp = (short)pAnswer.Length;
								pAnswer[Tmp] = tmpArr[0];
								if (++Tmp >= Lng)
								{
									Tmp--;
								}
								pAnswer[Tmp] = 0;
							}
						}
						break;
				}

				TmpChar = KeyboardLayout[pK.Layout][pK.PointerY][pK.PointerX];
				pK.NeedUpdate = 1;
			}

			if ((pK.OldX != pK.PointerX) || (pK.OldY != pK.PointerY))
			{
				pK.OldX = pK.PointerX;
				pK.OldY = pK.PointerY;
				pK.NeedUpdate = 1;
			}

			if (pK.NeedUpdate != 0)
			{
				//* UPDATE ***************************************************************************************************
				pK.NeedUpdate = 0;

				switch (pK.Layout)
				{
					case 0:
						{
							LmsInstance.Inst.DlcdClass.dLcdDrawPicture(LmsInstance.Inst.UiInstance.Lcd, Color, pK.ScreenStartX, pK.ScreenStartY, kbc.Width, kbc.Height, kbc.Data);
						}
						break;
					case 1:
						{
							LmsInstance.Inst.DlcdClass.dLcdDrawPicture(LmsInstance.Inst.UiInstance.Lcd, Color, pK.ScreenStartX, pK.ScreenStartY, kbs.Width, kbs.Height, kbs.Data);
						}
						break;
					case 2:
						{
							LmsInstance.Inst.DlcdClass.dLcdDrawPicture(LmsInstance.Inst.UiInstance.Lcd, Color, pK.ScreenStartX, pK.ScreenStartY, kbn.Width, kbn.Height, kbn.Data);
						}
						break;
				}
				if ((Icon >= 0) && (Icon < lms2012.NIcon.ICON_BRICK1))
				{
					LmsInstance.Inst.DlcdClass.dLcdDrawIcon(LmsInstance.Inst.UiInstance.Lcd, Color, pK.IconStartX, pK.IconStartY, lms2012.IconType.NORMAL_ICON, (byte)Icon);
				}
				if (pText[0] != 0)
				{
					LmsInstance.Inst.DlcdClass.dLcdDrawText(LmsInstance.Inst.UiInstance.Lcd, Color, pK.TextStartX, pK.TextStartY, lms2012.FontType.SMALL_FONT, pText);
				}

				X4 = 0;
				X3 = (short)pAnswer.Length;
				if (X3 > 15)
				{
					X4 = (short)(X3 - 15);
				}

				LmsInstance.Inst.DlcdClass.dLcdDrawText(LmsInstance.Inst.UiInstance.Lcd, Color, pK.StringStartX, pK.StringStartY, lms2012.FontType.NORMAL_FONT, pAnswer.Skip(X4).ToArray());
				LmsInstance.Inst.DlcdClass.dLcdDrawChar(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(pK.StringStartX + (X3 - X4) * 8), pK.StringStartY, lms2012.FontType.NORMAL_FONT, (byte)'_');

				SX = (short)(pK.KeybStartX + pK.PointerX * pK.KeybSpaceX);
				SY = (short)(pK.KeybStartY + pK.PointerY * pK.KeybSpaceY);

				switch (TmpChar)
				{
					case 0x01:
					case 0x02:
					case 0x03:
						{
							LmsInstance.Inst.DlcdClass.dLcdInverseRect(LmsInstance.Inst.UiInstance.Lcd, (short)(SX - 8), SY, (short)(pK.KeybWidth + 8), pK.KeybHeight);
						}
						break;
					case 0x04:
						{
							LmsInstance.Inst.DlcdClass.dLcdInverseRect(LmsInstance.Inst.UiInstance.Lcd, (short)(SX - 8), (short)(pK.KeybStartY + 1 * pK.KeybSpaceY), (short)(pK.KeybWidth + 8), (short)(pK.KeybHeight * 2 + 1));
						}
						break;
					case 0x08:
						{
							LmsInstance.Inst.DlcdClass.dLcdInverseRect(LmsInstance.Inst.UiInstance.Lcd, (short)(SX + 2), SY, (short)(pK.KeybWidth + 5), pK.KeybHeight);
						}
						break;
					case 0x0D:
						{
							SX = (short)(pK.KeybStartX + 112);
							SY = (short)(pK.KeybStartY + 1 * pK.KeybSpaceY);
							LmsInstance.Inst.DlcdClass.dLcdInverseRect(LmsInstance.Inst.UiInstance.Lcd, SX, SY, (short)(pK.KeybWidth + 5), (short)(pK.KeybSpaceY + 1));
							SX = (short)(pK.KeybStartX + 103);
							SY = (short)(pK.KeybStartY + 1 + 2 * pK.KeybSpaceY);
							LmsInstance.Inst.DlcdClass.dLcdInverseRect(LmsInstance.Inst.UiInstance.Lcd, SX, SY, (short)(pK.KeybWidth + 14), (short)(pK.KeybSpaceY * 2 - 4));
						}
						break;
					case 0x20:
						{
							LmsInstance.Inst.DlcdClass.dLcdInverseRect(LmsInstance.Inst.UiInstance.Lcd, (short)(pK.KeybStartX + 11), (short)(SY + 1), (short)(pK.KeybWidth + 68), (short)(pK.KeybHeight - 3));
						}
						break;
					default:
						{
							LmsInstance.Inst.DlcdClass.dLcdInverseRect(LmsInstance.Inst.UiInstance.Lcd, (short)(SX + 1), SY, pK.KeybWidth, pK.KeybHeight);
						}
						break;
				}
				cUiUpdateLcd();
				LmsInstance.Inst.UiInstance.ScreenBusy = 0;
			}

			return (SelectedChar);
		}

        void cUiDrawBar(byte Color, short X, short Y, short X1, short Y1, short Min, short Max, short Act)
        {
            short Tmp;
            short Items;
            short KnobHeight = 7;

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
            LmsInstance.Inst.DlcdClass.dLcdRect(LmsInstance.Inst.UiInstance.Lcd, Color, X, Y, X1, Y1);

            // Draw nob
            Tmp = Y;
            if ((Items > 1) && (Act > 0))
            {
                Tmp += (short)(((Y1 - KnobHeight) * (Act - 1)) / (Items - 1));
            }

            switch (X1)
            {
                case 5:
                    {
                        LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X + 1), Tmp);
                        LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X + 2), Tmp);
                        LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X + 3), Tmp);
                        Tmp++;
                        LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X + 1), Tmp);
                        LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X + 3), Tmp);
                        Tmp++;
                        LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X + 2), Tmp);
                        Tmp++;
                        LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X + 1), Tmp);
                        LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X + 3), Tmp);
                        Tmp++;
                        LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X + 2), Tmp);
                        Tmp++;
                        LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X + 1), Tmp);
                        LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X + 3), Tmp);
                        Tmp++;
                        LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X + 1), Tmp);
                        LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X + 2), Tmp);
                        LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X + 3), Tmp);
                    }
                    break;
                case 6:
                    {
                        LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X + 2), Tmp);
                        LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X + 3), Tmp);
                        Tmp++;
                        LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X + 1), Tmp);
                        LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X + 4), Tmp);
                        Tmp++;
                        Tmp++;
                        LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X + 2), Tmp);
                        LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X + 3), Tmp);
                        Tmp++;
                        Tmp++;
                        LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X + 2), Tmp);
                        LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X + 3), Tmp);
                        Tmp++;
                        Tmp++;
                        LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X + 1), Tmp);
                        LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X + 4), Tmp);
                        Tmp++;
                        LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X + 2), Tmp);
                        LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X + 3), Tmp);
                    }
                    break;
                default:
                    {
                        LmsInstance.Inst.DlcdClass.dLcdFillRect(LmsInstance.Inst.UiInstance.Lcd, Color, X, Tmp, X1, KnobHeight);
                    }
                    break;
            }
        }

        lms2012.Result cUiBrowser(byte Type, short X, short Y, short X1, short Y1, byte Lng, ref sbyte pType, byte[] pAnswer)
        {
            lms2012.Result Result = lms2012.Result.BUSY;
            byte[] Image;
            BROWSER pB;
            ushort PrgId;
            ushort ObjId;
            short Tmp;
            short Indent;
            short Item;
            short TotalItems;
            sbyte TmpType = 0;
            byte Folder;
            byte OldPriority;
            byte Priority = 0;
            byte Color;
            short Ignore;
            byte Data8 = 0;
            int Total = 0;
            int Free = 0;
            lms2012.Result TmpResult;
            short TmpHandle = 0;

            PrgId = LmsInstance.Inst.CurrentProgramId();
            ObjId = LmsInstance.Inst.CallingObjectId();
            pB = LmsInstance.Inst.UiInstance.Browser;

            Color = lms2012.FG_COLOR;

            // Test ignore horizontal update
            if (((byte)Type & 0x20) != 0)
            {
                Ignore = -1;
            }
            else
            {
                if (((byte)Type & 0x10) != 0)
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

            LmsInstance.Inst.CheckUsbstick(ref Data8, ref Total, ref Free, 0);
            if (Data8 != 0)
            {
                LmsInstance.Inst.UiInstance.UiUpdate = 1;
            }
            LmsInstance.Inst.CheckSdcard(ref Data8, ref Total, ref Free, 0);
            if (Data8 != 0)
            {
                LmsInstance.Inst.UiInstance.UiUpdate = 1;
            }

            if (LmsInstance.Inst.ProgramStatusChange((ushort)lms2012.Slot.USER_SLOT) == lms2012.ObjectStatus.STOPPED)
            {
                if (Type != (byte)lms2012.BrowserType.BROWSE_FILES)
                {
                    Result = lms2012.Result.OK;
					pType = 0;
                }
            }

            if ((pType == (short)lms2012.FileType.TYPE_REFRESH_BROWSER))
            {
                LmsInstance.Inst.UiInstance.UiUpdate = 1;
            }

            if ((pType == (short)lms2012.FileType.TYPE_RESTART_BROWSER))
            {
                if (pB.hFiles != 0)
                {
                    LmsInstance.Inst.CmemoryClass.cMemoryCloseFolder(pB.PrgId, ref pB.hFiles);
                }
                if (pB.hFolders != 0)
                {
                    LmsInstance.Inst.CmemoryClass.cMemoryCloseFolder(pB.PrgId, ref pB.hFolders);
                }
				pB.PrgId = 0;
                pB.ObjId = 0;
				//    pAnswer[0]          =  0;
				pType = 0;
            }

            if ((pB.PrgId == 0) && (pB.ObjId == 0))
            {
                //* INIT *****************************************************************************************************

                // Define screen
                pB.ScreenStartX = X;
                pB.ScreenStartY = Y;
                pB.ScreenWidth = X1;
                pB.ScreenHeight = Y1;

                // calculate lines on screen
                pB.LineSpace = 5;
                pB.IconHeight = LmsInstance.Inst.DlcdClass.dLcdGetIconHeight(lms2012.IconType.NORMAL_ICON);
                pB.LineHeight = (short)(pB.IconHeight + pB.LineSpace);
                pB.Lines = (short)(pB.ScreenHeight / pB.LineHeight);

                // calculate chars and lines on screen
                pB.CharWidth = LmsInstance.Inst.DlcdClass.dLcdGetFontWidth(lms2012.FontType.NORMAL_FONT);
                pB.CharHeight = LmsInstance.Inst.DlcdClass.dLcdGetFontHeight(lms2012.FontType.NORMAL_FONT);
                pB.IconWidth = LmsInstance.Inst.DlcdClass.dLcdGetIconWidth(lms2012.IconType.NORMAL_ICON);
                pB.Chars = (short)((pB.ScreenWidth - pB.IconWidth) / pB.CharWidth);

                // calculate start of icon
                pB.IconStartX = cUiAlignX(pB.ScreenStartX);
                pB.IconStartY = (short)(pB.ScreenStartY + pB.LineSpace / 2);

                // calculate start of text
                pB.TextStartX = cUiAlignX((short)(pB.ScreenStartX + pB.IconWidth));
                pB.TextStartY = (short)(pB.ScreenStartY + (pB.LineHeight - pB.CharHeight) / 2);

                // Calculate selection barBrowser
                pB.SelectStartX = (short)(pB.ScreenStartX + 1);
                pB.SelectWidth = (short)(pB.ScreenWidth - (pB.CharWidth + 5));
                pB.SelectStartY = (short)(pB.IconStartY - 1);
                pB.SelectHeight = (short)(pB.IconHeight + 2);

                // Calculate scroll bar
                pB.ScrollWidth = 6;
                pB.NobHeight = 9;
                pB.ScrollStartX = (short)(pB.ScreenStartX + pB.ScreenWidth - pB.ScrollWidth);
                pB.ScrollStartY = (short)(pB.ScreenStartY + 1);
                pB.ScrollHeight = (short)(pB.ScreenHeight - 2);
                pB.ScrollSpan = (short)(pB.ScrollHeight - pB.NobHeight);

				pB.TopFolder.WriteBytes(pAnswer);

                pB.PrgId = PrgId;
                pB.ObjId = ObjId;

                pB.OldFiles = 0;
                pB.Folders = 0;
                pB.OpenFolder = 0;
                pB.Files = 0;
                pB.ItemStart = 1;
                pB.ItemPointer = 1;
                pB.NeedUpdate = 1;
                LmsInstance.Inst.UiInstance.UiUpdate = 1;
            }

            if ((pB.PrgId == PrgId) && (pB.ObjId == ObjId))
            {
                //* CTRL *****************************************************************************************************
                if (LmsInstance.Inst.UiInstance.UiUpdate != 0)
                {
                    LmsInstance.Inst.UiInstance.UiUpdate = 0;
                    if (pB.hFiles != 0)
                    {
                        LmsInstance.Inst.CmemoryClass.cMemoryCloseFolder(pB.PrgId, ref pB.hFiles);
                    }
                    if (pB.hFolders != 0)
                    {
                        LmsInstance.Inst.CmemoryClass.cMemoryCloseFolder(pB.PrgId, ref pB.hFolders);
                    }

					pB.OpenFolder = 0;
                    pB.Files = 0;
					pType = 0;

                    switch (Type)
                    {
                        case (byte)lms2012.BrowserType.BROWSE_FOLDERS:
                        case (byte)lms2012.BrowserType.BROWSE_FOLDS_FILES:
                            {
                                if (LmsInstance.Inst.CmemoryClass.cMemoryOpenFolder(PrgId, (byte)lms2012.FileType.TYPE_FOLDER, pB.TopFolder, ref pB.hFolders) == lms2012.Result.OK)
                                {
                                    //******************************************************************************************************
                                    if (pB.OpenFolder != 0)
                                    {
                                        LmsInstance.Inst.CmemoryClass.cMemoryGetItem(pB.PrgId, pB.hFolders, pB.OpenFolder, lms2012.FOLDERNAME_SIZE + lms2012.SUBFOLDERNAME_SIZE, pB.SubFolder, ref TmpType);
                                        if (CommonHelper.Strcmp(pB.SubFolder, lms2012.SDCARD_FOLDER) == 0)
                                        {
                                            Item = pB.ItemPointer;
                                            LmsInstance.Inst.CmemoryClass.cMemoryGetItemName(pB.PrgId, pB.hFolders, Item, lms2012.MAX_FILENAME_SIZE, pB.Filename, ref pType, ref Priority);
                                            Result = LmsInstance.Inst.CmemoryClass.cMemoryGetItem(pB.PrgId, pB.hFolders, Item, lms2012.FOLDERNAME_SIZE + lms2012.SUBFOLDERNAME_SIZE, pB.FullPath, ref pType);
											pType = (sbyte)lms2012.FileType.TYPE_SDCARD;

											pAnswer.WriteBytes(pB.FullPath);
                                        }
                                        else
                                        {
                                            if (CommonHelper.Strcmp(pB.SubFolder, lms2012.USBSTICK_FOLDER) == 0)
                                            {
                                                Item = pB.ItemPointer;
                                                LmsInstance.Inst.CmemoryClass.cMemoryGetItemName(pB.PrgId, pB.hFolders, Item, lms2012.MAX_FILENAME_SIZE, pB.Filename, ref pType, ref Priority);
                                                Result = LmsInstance.Inst.CmemoryClass.cMemoryGetItem(pB.PrgId, pB.hFolders, Item, lms2012.FOLDERNAME_SIZE + lms2012.SUBFOLDERNAME_SIZE, pB.FullPath, ref pType);
                                                pType = (sbyte)lms2012.FileType.TYPE_USBSTICK;

                                                pAnswer.WriteBytes(pB.FullPath);
                                            }
                                            else
                                            {
                                                Result = LmsInstance.Inst.CmemoryClass.cMemoryOpenFolder(PrgId, (byte)lms2012.FileType.FILETYPE_UNKNOWN, pB.SubFolder, ref pB.hFiles);
                                                Result = lms2012.Result.BUSY;
                                            }
                                        }
                                    }
                                    //******************************************************************************************************
                                }
                                else
                                {
                                    pB.PrgId = 0;
                                    pB.ObjId = 0;
                                }
                            }
                            break;
                        case (byte)lms2012.BrowserType.BROWSE_CACHE:
                            {
                            }
                            break;
                        case (byte)lms2012.BrowserType.BROWSE_FILES:
                            {
                                if (LmsInstance.Inst.CmemoryClass.cMemoryOpenFolder(PrgId, (byte)lms2012.FileType.FILETYPE_UNKNOWN, pB.TopFolder, ref pB.hFiles) == lms2012.Result.OK)
                                {
                                }
                                else
                                {
                                    pB.PrgId = 0;
                                    pB.ObjId = 0;
                                }
                            }
                            break;

                    }
                }

				// TODO: probably uncomment
				
                //if (CommonHelper.Strstr((char*)pB.SubFolder, SDCARD_FOLDER))
                //{
                //    pB.Sdcard = 1;
                //}
                //else
                //{
                //    pB.Sdcard = 0;
                //}

                //if (strstr((char*)pB.SubFolder, USBSTICK_FOLDER))
                //{
                //    pB.Usbstick = 1;
                //}
                //else
                //{
                //    pB.Usbstick = 0;
                //}

                TmpResult = lms2012.Result.OK;
                switch (Type)
                {
                    case (byte)lms2012.BrowserType.BROWSE_FOLDERS:
                    case (byte)lms2012.BrowserType.BROWSE_FOLDS_FILES:
                        {
                            // Collect folders in directory
                            TmpResult = LmsInstance.Inst.CmemoryClass.cMemoryGetFolderItems(pB.PrgId, pB.hFolders, ref pB.Folders);

                            // Collect files in folder
                            if ((pB.OpenFolder != 0) && (TmpResult == lms2012.Result.OK))
                            {
                                TmpResult = LmsInstance.Inst.CmemoryClass.cMemoryGetFolderItems(pB.PrgId, pB.hFiles, ref pB.Files);
                            }
                        }
                        break;
                    case (byte)lms2012.BrowserType.BROWSE_CACHE:
                        {
                            pB.Folders = (short)LmsInstance.Inst.CmemoryClass.cMemoryGetCacheFiles();
                        }
                        break;
                    case (byte)lms2012.BrowserType.BROWSE_FILES:
                        {
                            TmpResult = LmsInstance.Inst.CmemoryClass.cMemoryGetFolderItems(pB.PrgId, pB.hFiles, ref pB.Files);
                        }
                        break;

                }

                if ((pB.OpenFolder != 0) && (pB.OpenFolder == pB.ItemPointer))
                {
                    if (cUiGetShortPress((byte)lms2012.ButtonType.BACK_BUTTON) != 0)
                    {
                        // Close folder
                        LmsInstance.Inst.CmemoryClass.cMemoryCloseFolder(pB.PrgId, ref pB.hFiles);
                        if (pB.ItemPointer > pB.OpenFolder)
                        {
                            pB.ItemPointer -= pB.Files;
                        }
						pB.OpenFolder = 0;
                        pB.Files = 0;
                    }
                }

                // TODO: probably uncomment

                //if (pB.Sdcard == 1)
                //{
                //    if (pB.OpenFolder == 0)
                //    {
                //        if (cUiGetShortPress(BACK_BUTTON))
                //        {
                //            // Collapse sdcard
                //            if (pB.hFiles)
                //            {
                //                cMemoryCloseFolder(pB.PrgId, &pB.hFiles);
                //            }
                //            if (pB.hFolders)
                //            {
                //                cMemoryCloseFolder(pB.PrgId, &pB.hFolders);
                //            }
                //          pB.PrgId = 0;
                //            pB.ObjId = 0;
                //            strcpy((char*)pAnswer, vmPRJS_DIR);
                //            *pType = 0;
                //            pB.SubFolder[0] = 0;
                //        }
                //    }
                //}
                //if (pB.Usbstick == 1)
                //{
                //    if (pB.OpenFolder == 0)
                //    {
                //        if (cUiGetShortPress(BACK_BUTTON))
                //        {
                //            // Collapse usbstick
                //            if (pB.hFiles)
                //            {
                //                cMemoryCloseFolder(pB.PrgId, &pB.hFiles);
                //            }
                //            if (pB.hFolders)
                //            {
                //                cMemoryCloseFolder(pB.PrgId, &pB.hFolders);
                //            }
                //          pB.PrgId = 0;
                //            pB.ObjId = 0;
                //            strcpy((char*)pAnswer, vmPRJS_DIR);
                //            *pType = 0;
                //            pB.SubFolder[0] = 0;
                //        }
                //    }
                //}


                if (pB.OldFiles != (pB.Files + pB.Folders))
                {
                    pB.OldFiles = (short)(pB.Files + pB.Folders);
                    pB.NeedUpdate = 1;
                }

                if (cUiGetShortPress((byte)lms2012.ButtonType.ENTER_BUTTON) != 0)
                {
                    pB.OldFiles = 0;
                    if (pB.OpenFolder != 0)
                    {
                        if ((pB.ItemPointer > pB.OpenFolder) && (pB.ItemPointer <= (pB.OpenFolder + pB.Files)))
                        { // File selected

                            Item = (short)(pB.ItemPointer - pB.OpenFolder);
                            Result = LmsInstance.Inst.CmemoryClass.cMemoryGetItem(pB.PrgId, pB.hFiles, Item, Lng, pB.FullPath, ref pType);

                            pAnswer.WriteBytes(pB.FullPath);
                        }
                        else
                        { // Folder selected
                            if (pB.OpenFolder == pB.ItemPointer)
                            { // Enter on open folder

                                Item = pB.OpenFolder;
                                Result = LmsInstance.Inst.CmemoryClass.cMemoryGetItem(pB.PrgId, pB.hFolders, Item, Lng, pAnswer, ref pType);
                            }
                            else
                            {
                                // Close folder
                                LmsInstance.Inst.CmemoryClass.cMemoryCloseFolder(pB.PrgId, ref pB.hFiles);
                                if (pB.ItemPointer > pB.OpenFolder)
                                {
                                    pB.ItemPointer -= pB.Files;
                                }
								pB.OpenFolder = 0;
                                pB.Files = 0;
                            }
                        }
                    }
                    if (pB.OpenFolder == 0)
                    { // Open folder
                        switch (Type)
                        {
                            case (byte)lms2012.BrowserType.BROWSE_FOLDERS:
                                { // Folder

                                    Item = pB.ItemPointer;
                                    LmsInstance.Inst.CmemoryClass.cMemoryGetItemName(pB.PrgId, pB.hFolders, Item, lms2012.MAX_FILENAME_SIZE, pB.Filename, ref pType, ref Priority);
                                    Result = LmsInstance.Inst.CmemoryClass.cMemoryGetItem(pB.PrgId, pB.hFolders, Item, lms2012.FOLDERNAME_SIZE + lms2012.SUBFOLDERNAME_SIZE, pB.FullPath, ref pType);

                                    pAnswer.WriteBytes(pB.FullPath.ConcatArrays(new byte[] { (byte)'/' }).ConcatArrays(pB.Filename));
                                    pType = (sbyte)lms2012.FileType.TYPE_BYTECODE;
                                }
                                break;
                            case (byte)lms2012.BrowserType.BROWSE_FOLDS_FILES:
                                { // Folder & File

                                    pB.OpenFolder = pB.ItemPointer;
                                    LmsInstance.Inst.CmemoryClass.cMemoryGetItem(pB.PrgId, pB.hFolders, pB.OpenFolder, lms2012.FOLDERNAME_SIZE + lms2012.SUBFOLDERNAME_SIZE, pB.SubFolder, ref TmpType);
                                    if (CommonHelper.Strcmp(pB.SubFolder, lms2012.SDCARD_FOLDER) == 0)
                                    {
                                        Item = pB.ItemPointer;
                                        LmsInstance.Inst.CmemoryClass.cMemoryGetItemName(pB.PrgId, pB.hFolders, Item, lms2012.MAX_FILENAME_SIZE, pB.Filename, ref pType, ref Priority);
                                        Result = LmsInstance.Inst.CmemoryClass.cMemoryGetItem(pB.PrgId, pB.hFolders, Item, lms2012.FOLDERNAME_SIZE + lms2012.SUBFOLDERNAME_SIZE, pB.FullPath, ref pType);
                                        pType = (sbyte)lms2012.FileType.TYPE_SDCARD;

                                        pAnswer.WriteBytes(pB.FullPath);
                                    }
                                    else
                                    {
                                        if (CommonHelper.Strcmp(pB.SubFolder, lms2012.USBSTICK_FOLDER) == 0)
                                        {
                                            Item = pB.ItemPointer;
                                            LmsInstance.Inst.CmemoryClass.cMemoryGetItemName(pB.PrgId, pB.hFolders, Item, lms2012.MAX_FILENAME_SIZE, pB.Filename, ref pType, ref Priority);
                                            Result = LmsInstance.Inst.CmemoryClass.cMemoryGetItem(pB.PrgId, pB.hFolders, Item, lms2012.FOLDERNAME_SIZE + lms2012.SUBFOLDERNAME_SIZE, pB.FullPath, ref pType);
                                            pType = (sbyte)lms2012.FileType.TYPE_USBSTICK;

                                            pAnswer.WriteBytes(pB.FullPath);
                                        }
                                        else
                                        {
                                            pB.ItemStart = pB.ItemPointer;
                                            Result = LmsInstance.Inst.CmemoryClass.cMemoryOpenFolder(PrgId, (byte)lms2012.FileType.FILETYPE_UNKNOWN, pB.SubFolder, ref pB.hFiles);

                                            Result = lms2012.Result.BUSY;
                                        }
                                    }
                                }
                                break;
                            case (byte)lms2012.BrowserType.BROWSE_CACHE:
                                { // Cache
                                    Item = pB.ItemPointer;

                                    pType = LmsInstance.Inst.CmemoryClass.cMemoryGetCacheName((byte)Item, lms2012.FOLDERNAME_SIZE + lms2012.SUBFOLDERNAME_SIZE, pB.FullPath, pB.Filename);
                                    pAnswer.WriteBytes(pB.FullPath);
                                    Result = lms2012.Result.OK;
                                }
                                break;
                            case (byte)lms2012.BrowserType.BROWSE_FILES:
                                { // File
                                    if ((pB.ItemPointer > pB.OpenFolder) && (pB.ItemPointer <= (pB.OpenFolder + pB.Files)))
                                    { // File selected
                                        Item = (short)(pB.ItemPointer - pB.OpenFolder);

                                        Result = LmsInstance.Inst.CmemoryClass.cMemoryGetItem(pB.PrgId, pB.hFiles, Item, Lng, pB.FullPath, ref pType);

                                        pAnswer.WriteBytes(pB.FullPath);
                                        Result = lms2012.Result.OK;
                                    }
                                }
                                break;
                        }
                    }
					pB.NeedUpdate = 1;
                }

                TotalItems = (short)(pB.Folders + pB.Files);
                if (TmpResult == lms2012.Result.OK)
                {
                    if (TotalItems != 0)
                    {
                        if (pB.ItemPointer > TotalItems)
                        {
                            pB.ItemPointer = TotalItems;
                            pB.NeedUpdate = 1;
                        }
                        if (pB.ItemStart > pB.ItemPointer)
                        {
                            pB.ItemStart = pB.ItemPointer;
                            pB.NeedUpdate = 1;
                        }
                    }
                    else
                    {
                        pB.ItemStart = 1;
                        pB.ItemPointer = 1;
                    }
                }

                Tmp = cUiGetVert();
                if (Tmp != 0)
                { // up/down arrow pressed

                    pB.NeedUpdate = 1;

                    // Calculate item pointer
                    pB.ItemPointer += Tmp;
                    if (pB.ItemPointer < 1)
                    {
                        pB.ItemPointer = 1;
                        pB.NeedUpdate = 0;
                    }
                    if (pB.ItemPointer > TotalItems)
                    {
                        pB.ItemPointer = TotalItems;
                        pB.NeedUpdate = 0;
                    }
                }

                // Calculate item start
                if (pB.ItemPointer < pB.ItemStart)
                {
                    if (pB.ItemPointer > 0)
                    {
                        pB.ItemStart = pB.ItemPointer;
                    }
                }
                if (pB.ItemPointer >= (pB.ItemStart + pB.Lines))
                {
                    pB.ItemStart = (short)(pB.ItemPointer - pB.Lines);
                    pB.ItemStart++;
                }

                if (pB.NeedUpdate != 0)
                {
                    //* UPDATE ***************************************************************************************************
                    pB.NeedUpdate = 0;

                    //# ifdef DEBUG
                    //# ifndef DISABLE_SDCARD_SUPPORT
                    //                    if (pB.Sdcard)
                    //                    {
                    //                        printf("SDCARD\r\n");
                    //                    }
                    //#endif
                    //# ifndef DISABLE_USBSTICK_SUPPORT
                    //                    if (pB.Usbstick)
                    //                    {
                    //                        printf("USBSTICK\r\n");
                    //                    }
                    //#endif
                    //                    printf("Folders = %3d, OpenFolder = %3d, Files = %3d, ItemStart = %3d, ItemPointer = %3d, TotalItems = %3d\r\n\n", pB.Folders, pB.OpenFolder, pB.Files, pB.ItemStart, pB.ItemPointer, TotalItems);
                    //#endif

                    // clear screen
                    LmsInstance.Inst.DlcdClass.dLcdFillRect(LmsInstance.Inst.UiInstance.Lcd, lms2012.BG_COLOR, pB.ScreenStartX, pB.ScreenStartY, pB.ScreenWidth, pB.ScreenHeight);

                    OldPriority = 0;
                    for (Tmp = 0; Tmp < pB.Lines; Tmp++)
                    {
                        Item = (short)(Tmp + pB.ItemStart);
                        Folder = 1;
                        Priority = OldPriority;

                        if (Item <= TotalItems)
                        {
                            if (pB.OpenFolder != 0)
                            {
                                if ((Item > pB.OpenFolder) && (Item <= (pB.OpenFolder + pB.Files)))
                                {
                                    Item -= pB.OpenFolder;
                                    Folder = 0;
                                }
                                else
                                {
                                    if (Item > pB.OpenFolder)
                                    {
                                        Item -= pB.Files;
                                    }
                                }
                            }

                            //*** Graphics ***********************************************************************************************

                            if (Folder != 0)
                            { // Show folder

                                switch (Type)
                                {
                                    case (byte)lms2012.BrowserType.BROWSE_FOLDERS:
                                        {
                                            LmsInstance.Inst.CmemoryClass.cMemoryGetItemName(pB.PrgId, pB.hFolders, Item, (byte)pB.Chars, pB.Filename, ref TmpType, ref Priority);
                                            if (LmsInstance.Inst.CmemoryClass.cMemoryGetItemIcon(pB.PrgId, pB.hFolders, Item, ref TmpHandle, out Image) == lms2012.Result.OK)
                                            {
                                                LmsInstance.Inst.DlcdClass.dLcdDrawBitmap(LmsInstance.Inst.UiInstance.Lcd, Color, pB.IconStartX, (short)(pB.IconStartY + (Tmp * pB.LineHeight)), Image);
                                                LmsInstance.Inst.CmemoryClass.cMemoryCloseFile(pB.PrgId, ref TmpHandle);
                                            }
                                            else
                                            {
												var appp = LmsInstance.Inst.UiBmpHandler.Get(UiBmpHandler.BmpType.App);
												LmsInstance.Inst.DlcdClass.dLcdDrawPicture(LmsInstance.Inst.UiInstance.Lcd, Color, pB.IconStartX, (short)(pB.IconStartY + (Tmp * pB.LineHeight)), appp.Width, appp.Height, appp.Data);
                                            }

											pB.Text[0] = 0;
                                            if (CommonHelper.Strcmp(pB.Filename, "Bluetooth") == 0)
                                            {
                                                if (LmsInstance.Inst.UiInstance.BtOn != 0)
                                                {
                                                    pB.Text[0] = (byte)'+';
                                                }
                                                else
                                                {
                                                    pB.Text[0] = (byte)'-';
                                                }
                                            }
                                            else
                                            {
                                                if (CommonHelper.Strcmp(pB.Filename, "WiFi") == 0)
                                                {
                                                    if (LmsInstance.Inst.UiInstance.WiFiOn != 0)
                                                    {
                                                        pB.Text[0] = (byte)'+';
                                                    }
                                                    else
                                                    {
                                                        pB.Text[0] = (byte)'-';
                                                    }
                                                }
                                                else
                                                {
                                                    if (LmsInstance.Inst.CmemoryClass.cMemoryGetItemText(pB.PrgId, pB.hFolders, Item, (byte)pB.Chars, pB.Text) != lms2012.Result.OK)
                                                    {
                                                        pB.Text[0] = 0;
                                                    }
                                                }
                                            }
                                            switch (pB.Text[0])
                                            {
                                                case 0:
                                                    {
                                                    }
                                                    break;

                                                case (byte)'+':
                                                    {
                                                        Indent = (short)((pB.Chars - 1) * pB.CharWidth - LmsInstance.Inst.DlcdClass.dLcdGetIconWidth(lms2012.IconType.MENU_ICON));
                                                        LmsInstance.Inst.DlcdClass.dLcdDrawIcon(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(pB.TextStartX + Indent), (short)((pB.TextStartY - 2) + (Tmp * pB.LineHeight)), lms2012.IconType.MENU_ICON, (int)lms2012.MIcon.ICON_CHECKED);
                                                    }
                                                    break;

                                                case (byte)'-':
                                                    {
                                                        Indent = (short)((pB.Chars - 1) * pB.CharWidth - LmsInstance.Inst.DlcdClass.dLcdGetIconWidth(lms2012.IconType.MENU_ICON));
                                                        LmsInstance.Inst.DlcdClass.dLcdDrawIcon(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(pB.TextStartX + Indent), (short)((pB.TextStartY - 2) + (Tmp * pB.LineHeight)), lms2012.IconType.MENU_ICON, (int)lms2012.MIcon.ICON_CHECKBOX);
                                                    }
                                                    break;

                                                default:
                                                    {
                                                        Indent = (short)((pB.Chars - 1) - (pB.Text.Length * pB.CharWidth));
                                                        LmsInstance.Inst.DlcdClass.dLcdDrawText(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(pB.TextStartX + Indent), (short)(pB.TextStartY + (Tmp * pB.LineHeight)), lms2012.FontType.NORMAL_FONT, pB.Text);
                                                    }
                                                    break;

                                            }

                                        }
                                        break;

                                    case (byte)lms2012.BrowserType.BROWSE_FOLDS_FILES:
                                        {
                                            LmsInstance.Inst.CmemoryClass.cMemoryGetItemName(pB.PrgId, pB.hFolders, Item, (byte)pB.Chars, pB.Filename, ref TmpType, ref Priority);
                                            LmsInstance.Inst.DlcdClass.dLcdDrawIcon(LmsInstance.Inst.UiInstance.Lcd, Color, pB.IconStartX, (short)(pB.IconStartY + (Tmp * pB.LineHeight)), lms2012.IconType.NORMAL_ICON, (int)FiletypeToNormalIcon.First(x => (sbyte)x.Key == TmpType).Value);

                                            if ((Priority == 1) || (Priority == 2))
                                            {
                                                LmsInstance.Inst.DlcdClass.dLcdDrawIcon(LmsInstance.Inst.UiInstance.Lcd, Color, pB.IconStartX, (short)(pB.IconStartY + (Tmp * pB.LineHeight)), lms2012.IconType.NORMAL_ICON, (int)lms2012.NIcon.ICON_FOLDER2);
                                            }
                                            else
                                            {
                                                if (Priority == 3)
                                                {
                                                    LmsInstance.Inst.DlcdClass.dLcdDrawIcon(LmsInstance.Inst.UiInstance.Lcd, Color, pB.IconStartX, (short)(pB.IconStartY + (Tmp * pB.LineHeight)), lms2012.IconType.NORMAL_ICON, (int)lms2012.NIcon.ICON_SD);
                                                }
                                                else
                                                {
                                                    if (Priority == 4)
                                                    {
                                                        LmsInstance.Inst.DlcdClass.dLcdDrawIcon(LmsInstance.Inst.UiInstance.Lcd, Color, pB.IconStartX, (short)(pB.IconStartY + (Tmp * pB.LineHeight)), lms2012.IconType.NORMAL_ICON, (int)lms2012.NIcon.ICON_USB);
                                                    }
                                                    else
                                                    {
                                                        LmsInstance.Inst.DlcdClass.dLcdDrawIcon(LmsInstance.Inst.UiInstance.Lcd, Color, pB.IconStartX, (short)(pB.IconStartY + (Tmp * pB.LineHeight)), lms2012.IconType.NORMAL_ICON, (int)FiletypeToNormalIcon.First(x => (sbyte)x.Key == TmpType).Value);
                                                    }
                                                }
                                            }
                                            if (Priority != OldPriority)
                                            {
                                                if ((Priority == 1) || (Priority >= 3))
                                                {
                                                    if (Tmp != 0)
                                                    {
                                                        LmsInstance.Inst.DlcdClass.dLcdDrawDotLine(LmsInstance.Inst.UiInstance.Lcd, Color, pB.SelectStartX, (short)(pB.SelectStartY + ((Tmp - 1) * pB.LineHeight) + pB.LineHeight - 2), (short)(pB.SelectStartX + pB.SelectWidth), (short)(pB.SelectStartY + ((Tmp - 1) * pB.LineHeight) + pB.LineHeight - 2), 1, 2);
                                                    }
                                                }
                                            }
                                        }
                                        break;

                                    case (byte)lms2012.BrowserType.BROWSE_CACHE:
                                        {
                                            TmpType = LmsInstance.Inst.CmemoryClass.cMemoryGetCacheName((byte)Item, (byte)pB.Chars, pB.FullPath, pB.Filename);
                                            LmsInstance.Inst.DlcdClass.dLcdDrawIcon(LmsInstance.Inst.UiInstance.Lcd, Color, pB.IconStartX, (short)(pB.IconStartY + (Tmp * pB.LineHeight)), lms2012.IconType.NORMAL_ICON, (int)FiletypeToNormalIcon.First(x => (sbyte)x.Key == TmpType).Value);
                                        }
                                        break;

                                    case (byte)lms2012.BrowserType.BROWSE_FILES:
                                        {
                                            LmsInstance.Inst.CmemoryClass.cMemoryGetItemName(pB.PrgId, pB.hFiles, Item, (byte)pB.Chars, pB.Filename, ref TmpType, ref Priority);
                                            LmsInstance.Inst.DlcdClass.dLcdDrawIcon(LmsInstance.Inst.UiInstance.Lcd, Color, pB.IconStartX, (short)(pB.IconStartY + (Tmp * pB.LineHeight)), lms2012.IconType.NORMAL_ICON, (int)FiletypeToNormalIcon.First(x => (sbyte)x.Key == TmpType).Value);
                                        }
                                        break;

                                }
                                // Draw folder name
                                LmsInstance.Inst.DlcdClass.dLcdDrawText(LmsInstance.Inst.UiInstance.Lcd, Color, pB.TextStartX, (short)(pB.TextStartY + (Tmp * pB.LineHeight)), lms2012.FontType.NORMAL_FONT, pB.Filename);

                                // Draw open folder
                                if (Item == pB.OpenFolder)
                                {
                                    LmsInstance.Inst.DlcdClass.dLcdDrawIcon(LmsInstance.Inst.UiInstance.Lcd, Color, 144, (short)(pB.IconStartY + (Tmp * pB.LineHeight)), lms2012.IconType.NORMAL_ICON, (int)lms2012.NIcon.ICON_OPENFOLDER);
                                }

                                // Draw selection
                                if (pB.ItemPointer == (Tmp + pB.ItemStart))
                                {
                                    LmsInstance.Inst.DlcdClass.dLcdInverseRect(LmsInstance.Inst.UiInstance.Lcd, pB.SelectStartX, (short)(pB.SelectStartY + (Tmp * pB.LineHeight)), (short)(pB.SelectWidth + 1), pB.SelectHeight);
                                }

                                // Draw end line
                                switch (Type)
                                {
                                    case (byte)lms2012.BrowserType.BROWSE_FOLDERS:
                                    case (byte)lms2012.BrowserType.BROWSE_FOLDS_FILES:
                                    case (byte)lms2012.BrowserType.BROWSE_FILES:
                                        {
                                            if (((Tmp + pB.ItemStart) == TotalItems) && (Tmp < (pB.Lines - 1)))
                                            {
                                                LmsInstance.Inst.DlcdClass.dLcdDrawDotLine(LmsInstance.Inst.UiInstance.Lcd, Color, pB.SelectStartX, (short)(pB.SelectStartY + (Tmp * pB.LineHeight) + pB.LineHeight - 2), (short)(pB.SelectStartX + pB.SelectWidth), (short)(pB.SelectStartY + (Tmp * pB.LineHeight) + pB.LineHeight - 2), 1, 2);
                                            }
                                        }
                                        break;

                                    case (byte)lms2012.BrowserType.BROWSE_CACHE:
                                        {
                                            if (((Tmp + pB.ItemStart) == 1) && (Tmp < (pB.Lines - 1)))
                                            {
                                                LmsInstance.Inst.DlcdClass.dLcdDrawDotLine(LmsInstance.Inst.UiInstance.Lcd, Color, pB.SelectStartX, (short)(pB.SelectStartY + (Tmp * pB.LineHeight) + pB.LineHeight - 2), (short)(pB.SelectStartX + pB.SelectWidth), (short)(pB.SelectStartY + (Tmp * pB.LineHeight) + pB.LineHeight - 2), 1, 2);
                                            }
                                            if (((Tmp + pB.ItemStart) == TotalItems) && (Tmp < (pB.Lines - 1)))
                                            {
                                                LmsInstance.Inst.DlcdClass.dLcdDrawDotLine(LmsInstance.Inst.UiInstance.Lcd, Color, pB.SelectStartX, (short)(pB.SelectStartY + (Tmp * pB.LineHeight) + pB.LineHeight - 2), (short)(pB.SelectStartX + pB.SelectWidth), (short)(pB.SelectStartY + (Tmp * pB.LineHeight) + pB.LineHeight - 2), 1, 2);
                                            }
                                        }
                                        break;

                                }
                            }
                            else
                            { // Show file

                                // Get file name and type
                                LmsInstance.Inst.CmemoryClass.cMemoryGetItemName(pB.PrgId, pB.hFiles, Item, (byte)(pB.Chars - 1), pB.Filename, ref TmpType, ref Priority);

                                // show File icons
                                LmsInstance.Inst.DlcdClass.dLcdDrawIcon(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(pB.IconStartX + pB.CharWidth), (short)(pB.IconStartY + (Tmp * pB.LineHeight)), lms2012.IconType.NORMAL_ICON, (int)FiletypeToNormalIcon.First(x => (sbyte)x.Key == TmpType).Value);

                                // Draw file name
                                LmsInstance.Inst.DlcdClass.dLcdDrawText(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(pB.TextStartX + pB.CharWidth), (short)(pB.TextStartY + (Tmp * pB.LineHeight)), lms2012.FontType.NORMAL_FONT, pB.Filename);

                                // Draw folder line
                                if ((Tmp == (pB.Lines - 1)) || (Item == pB.Files))
                                {
                                    LmsInstance.Inst.DlcdClass.dLcdDrawLine(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(pB.IconStartX + pB.CharWidth - 3), (short)(pB.SelectStartY + (Tmp * pB.LineHeight)), (short)(pB.IconStartX + pB.CharWidth - 3), (short)(pB.SelectStartY + (Tmp * pB.LineHeight) + pB.SelectHeight - 1));
                                }
                                else
                                {
                                    LmsInstance.Inst.DlcdClass.dLcdDrawLine(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(pB.IconStartX + pB.CharWidth - 3), (short)(pB.SelectStartY + (Tmp * pB.LineHeight)), (short)(pB.IconStartX + pB.CharWidth - 3), (short)(pB.SelectStartY + (Tmp * pB.LineHeight) + pB.LineHeight - 1));
                                }

                                // Draw selection
                                if (pB.ItemPointer == (Tmp + pB.ItemStart))
                                {
                                    LmsInstance.Inst.DlcdClass.dLcdInverseRect(LmsInstance.Inst.UiInstance.Lcd, (short)(pB.SelectStartX + pB.CharWidth), (short)(pB.SelectStartY + (Tmp * pB.LineHeight)), (short)(pB.SelectWidth + 1 - pB.CharWidth), pB.SelectHeight);
                                }
                            }

                            //************************************************************************************************************
                        }

                        OldPriority = Priority;
                    }

                    cUiDrawBar(1, pB.ScrollStartX, pB.ScrollStartY, pB.ScrollWidth, pB.ScrollHeight, 0, TotalItems, pB.ItemPointer);

                    // Update
                    cUiUpdateLcd();
                    LmsInstance.Inst.UiInstance.ScreenBusy = 0;
                }

                if (Result != lms2012.Result.OK)
                {
                    Tmp = cUiTestHorz();
                    if (Ignore == Tmp)
                    {
                        Tmp = cUiGetHorz();
                        Tmp = 0;
                    }

                    if ((Tmp != 0) || (cUiTestShortPress((byte)lms2012.ButtonType.BACK_BUTTON) != 0) || (cUiTestLongPress((byte)lms2012.ButtonType.BACK_BUTTON) != 0))
                    {
                        if (Type != (byte)lms2012.BrowserType.BROWSE_CACHE)
                        {
                            if (pB.OpenFolder != 0)
                            {
                                if (pB.hFiles != 0)
                                {
                                    LmsInstance.Inst.CmemoryClass.cMemoryCloseFolder(pB.PrgId, ref pB.hFiles);
                                }
                            }
                            if (pB.hFolders != 0)
                            {
                                LmsInstance.Inst.CmemoryClass.cMemoryCloseFolder(pB.PrgId, ref pB.hFolders);
                            }
                        }
                      pB.PrgId = 0;
                        pB.ObjId = 0;
                        pB.SubFolder[0] = 0;
                        pAnswer[0] = 0;
                        pType = 0;
                        Result = lms2012.Result.OK;
                    }
                }
                else
                {
                    pB.NeedUpdate = 1;
                }
            }
            else
            {
                pAnswer[0] = 0;
                pType = (sbyte)lms2012.FileType.TYPE_RESTART_BROWSER;
                Result = lms2012.Result.FAIL;
            }

            if (pType > 0)
            {
//# ifndef DISABLE_SDCARD_SUPPORT
//                if (pB.Sdcard)
//                {
//                    *pType |= TYPE_SDCARD;
//                }
//#endif
//# ifndef DISABLE_USBSTICK_SUPPORT
//                if (pB.Usbstick)
//                {
//                    *pType |= TYPE_USBSTICK;
//                }
//#endif
            }

            if (Result != lms2012.Result.BUSY)
            {
				//* EXIT *****************************************************************************************************
			}
			return (Result);
		}

		short cUiTextboxGetLines(byte[] pText, int Size, byte Del)
		{
			int Point = 0;
			short Lines = 0;
			byte DelPoi;

			if (Del < (byte)lms2012.Delimeter.DELS)
			{
				while (pText[Point] != 0 && (Point < Size))
				{
					DelPoi = 0;
					while ((pText[Point] != 0) && (Point < Size) && (Delimiter.GetDicValue((lms2012.Delimeter)Del)[DelPoi] != 0) && (pText[Point] == Delimiter.GetDicValue((lms2012.Delimeter)Del)[DelPoi]))
					{
						DelPoi++;
						Point++;
					}
					if (Delimiter.GetDicValue((lms2012.Delimeter)Del)[DelPoi] == 0)
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

		void cUiTextboxAppendLine(byte[] pText, int Size, byte Del, ref byte pLine, byte Font)
		{
			int Point = 0;
			byte DelPoi = 0;

			if (Del < (byte)lms2012.Delimeter.DELS)
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

				while ((Point < Size) && pLine != 0)
				{
					pText[Point] = pLine;
					Point++;
					pLine++;
				}
				while ((Point < Size) && (Delimiter.GetDicValue((lms2012.Delimeter)Del)[DelPoi] != 0))
				{
					pText[Point] = (byte)Delimiter.GetDicValue((lms2012.Delimeter)Del)[DelPoi];
					Point++;
					DelPoi++;
				}
			}
		}


		int cUiTextboxFindLine(byte[] pText, int Size, byte Del, short Line, ref byte pFont)
		{
			int Result = -1;
			int Point = 0;
			byte DelPoi = 0;

			pFont = 0;
			if (Del < (byte)lms2012.Delimeter.DELS)
			{
				Result = Point;
				while ((Line != 0) && (pText[Point] != 0) && (Point < Size))
				{

					DelPoi = 0;
					while ((pText[Point] != 0) && (Point < Size) && (Delimiter.GetDicValue((lms2012.Delimeter)Del)[DelPoi] != 0) && (pText[Point] == Delimiter.GetDicValue((lms2012.Delimeter)Del)[DelPoi]))
					{
						DelPoi++;
						Point++;
					}
					if (Delimiter.GetDicValue((lms2012.Delimeter)Del)[DelPoi] == 0)
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
					if ((pText[Result] > 0) && (pText[Result] < (byte)lms2012.FontType.FONTTYPES))
					{
						pFont = pText[Result];
						Result++;
					}
				}
			}

			return (Result);
		}

		void cUiTextboxReadLine(byte[] pText, int Size, byte Del, byte Lng, short Line, byte[] pLine, ref byte pFont)
		{
			int Start;
			int Point = 0;
			byte DelPoi = 0;
			byte Run = 1;

			Start = cUiTextboxFindLine(pText, Size, Del, Line, ref pFont);
			Point = Start;

			pLine[0] = 0;

			if (Point >= 0)
			{
				while ((Run != 0) && (pText[Point] != 0) && (Point < Size))
				{
					DelPoi = 0;
					while ((pText[Point] != 0) && (Point < Size) && (Delimiter.GetDicValue((lms2012.Delimeter)Del)[DelPoi] != 0) && (pText[Point] == Delimiter.GetDicValue((lms2012.Delimeter)Del)[DelPoi]))
					{
						DelPoi++;
						Point++;
					}
					if (Delimiter.GetDicValue((lms2012.Delimeter)Del)[DelPoi] == 0)
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
				Point -= (int)DelPoi;

				if (((Point - Start) + 1) < (int)Lng)
				{
					Lng = (byte)((Point - Start) + 1);
				}
				pLine.WriteBytes(pText.SubSet(Start));
			}
		}


		lms2012.Result cUiTextbox(short X, short Y, short X1, short Y1, byte[] pText, int Size, byte Del, ref short pLine)
		{
			lms2012.Result Result = lms2012.Result.BUSY;
			TXTBOX pB;
			short Item;
			short TotalItems;
			short Tmp;
			short Ypos;
			byte Color;

			pB = LmsInstance.Inst.UiInstance.Txtbox;
			Color = lms2012.FG_COLOR;

			if (pLine < 0)
			{
				//* INIT *****************************************************************************************************
				// Define screen
				pB.ScreenStartX = X;
				pB.ScreenStartY = Y;
				pB.ScreenWidth = X1;
				pB.ScreenHeight = Y1;

				pB.Font = LmsInstance.Inst.UiInstance.Font;

				// calculate chars and lines on screen
				pB.CharWidth = LmsInstance.Inst.DlcdClass.dLcdGetFontWidth(pB.Font);
				pB.CharHeight = LmsInstance.Inst.DlcdClass.dLcdGetFontHeight(pB.Font);
				pB.Chars = (short)(pB.ScreenWidth / pB.CharWidth);

				// calculate lines on screen
				pB.LineSpace = 5;
				pB.LineHeight = (short)(pB.CharHeight + pB.LineSpace);
				pB.Lines = (short)(pB.ScreenHeight / pB.LineHeight);

				// calculate start of text
				pB.TextStartX = cUiAlignX(pB.ScreenStartX);
				pB.TextStartY = (short)(pB.ScreenStartY + (pB.LineHeight - pB.CharHeight) / 2);

				// Calculate selection barBrowser
				pB.SelectStartX = pB.ScreenStartX;
				pB.SelectWidth = (short)(pB.ScreenWidth - (pB.CharWidth + 5));
				pB.SelectStartY = (short)(pB.TextStartY - 1);
				pB.SelectHeight = (short)(pB.CharHeight + 1);

				// Calculate scroll bar
				pB.ScrollWidth = 5;
				pB.NobHeight = 7;
				pB.ScrollStartX = (short)(pB.ScreenStartX + pB.ScreenWidth - pB.ScrollWidth);
				pB.ScrollStartY = (short)(pB.ScreenStartY + 1);
				pB.ScrollHeight = (short)(pB.ScreenHeight - 2);
				pB.ScrollSpan = (short)(pB.ScrollHeight - pB.NobHeight);

				pB.Items = cUiTextboxGetLines(pText, Size, Del);
				pB.ItemStart = 1;
				pB.ItemPointer = 1;

				pB.NeedUpdate = 1;

				pLine = 0;
			}

			TotalItems = pB.Items;

			Tmp = cUiGetVert();
			if (Tmp != 0)
			{ // up/down arrow pressed

				pB.NeedUpdate = 1;

				// Calculate item pointer
				pB.ItemPointer += Tmp;
				if (pB.ItemPointer < 1)
				{
					pB.ItemPointer = 1;
					pB.NeedUpdate = 0;
				}
				if (pB.ItemPointer > TotalItems)
				{
					pB.ItemPointer = TotalItems;
					pB.NeedUpdate = 0;
				}
			}

			// Calculate item start
			if (pB.ItemPointer < pB.ItemStart)
			{
				if (pB.ItemPointer > 0)
				{
					pB.ItemStart = pB.ItemPointer;
				}
			}
			if (pB.ItemPointer >= (pB.ItemStart + pB.Lines))
			{
				pB.ItemStart = (short)(pB.ItemPointer - pB.Lines);
				pB.ItemStart++;
			}

			if (cUiGetShortPress((byte)lms2012.ButtonType.ENTER_BUTTON) != 0)
			{
				pLine = pB.ItemPointer;

				Result = lms2012.Result.OK;
			}
			if (cUiGetShortPress((byte)lms2012.ButtonType.BACK_BUTTON) != 0)
			{
				pLine = -1;

				Result = lms2012.Result.OK;
			}


			if (pB.NeedUpdate != 0)
			{
				//* UPDATE ***************************************************************************************************
				pB.NeedUpdate = 0;

				// clear screen
				LmsInstance.Inst.DlcdClass.dLcdFillRect(LmsInstance.Inst.UiInstance.Lcd, lms2012.BG_COLOR, pB.ScreenStartX, pB.ScreenStartY, pB.ScreenWidth, pB.ScreenHeight);

				Ypos = (short)(pB.TextStartY + 2);

				for (Tmp = 0; Tmp < pB.Lines; Tmp++)
				{
					Item = (short)(Tmp + pB.ItemStart);

					if (Item <= TotalItems)
					{
						byte tmpFont = (byte)pB.Font;
						cUiTextboxReadLine(pText, Size, Del, lms2012.TEXTSIZE, Item, pB.Text, ref tmpFont);
						pB.Font = (lms2012.FontType)tmpFont;

						// calculate chars and lines on screen
						pB.CharWidth = LmsInstance.Inst.DlcdClass.dLcdGetFontWidth(pB.Font);
						pB.CharHeight = LmsInstance.Inst.DlcdClass.dLcdGetFontHeight(pB.Font);

						// calculate lines on screen
						pB.LineSpace = 2;
						pB.LineHeight = (short)(pB.CharHeight + pB.LineSpace);
						pB.Lines = (short)(pB.ScreenHeight / pB.LineHeight);

						// Calculate selection barBrowser
						pB.SelectStartX = pB.ScreenStartX;
						pB.SelectWidth = (short)(pB.ScreenWidth - (pB.ScrollWidth + 2));
						pB.SelectStartY = (short)(pB.TextStartY - 1);
						pB.SelectHeight = (short)(pB.CharHeight + 1);

						pB.Chars = (short)(pB.SelectWidth / pB.CharWidth);

						pB.Text[pB.Chars] = 0;

						if ((Ypos + pB.LineHeight) <= (pB.ScreenStartY + pB.ScreenHeight))
						{
							LmsInstance.Inst.DlcdClass.dLcdDrawText(LmsInstance.Inst.UiInstance.Lcd, Color, pB.TextStartX, Ypos, pB.Font, pB.Text);
						}
						else
						{
							Tmp = pB.Lines;
						}
					}

					cUiDrawBar(1, pB.ScrollStartX, pB.ScrollStartY, pB.ScrollWidth, pB.ScrollHeight, 0, TotalItems, pB.ItemPointer);

					if ((Ypos + pB.LineHeight) <= (pB.ScreenStartY + pB.ScreenHeight))
					{
						// Draw selection
						if (pB.ItemPointer == (Tmp + pB.ItemStart))
						{
							LmsInstance.Inst.DlcdClass.dLcdInverseRect(LmsInstance.Inst.UiInstance.Lcd, pB.SelectStartX, (short)(Ypos - 1), pB.SelectWidth, pB.LineHeight);
						}
					}
					Ypos += pB.LineHeight;
				}

				// Update
				cUiUpdateLcd();
				LmsInstance.Inst.UiInstance.ScreenBusy = 0;
			}

			return (Result);
		}
		void cUiGraphSetup(short StartX, short SizeX, byte Items, short[] pOffset, short[] pSpan, float[] pMin, float[] pMax, float[] pVal)
		{
			short Item;
			short Pointer;
			LmsInstance.Inst.UiInstance.Graph.Initialized = 0;

			LmsInstance.Inst.UiInstance.Graph.pOffset = pOffset;
			LmsInstance.Inst.UiInstance.Graph.pSpan = pSpan;
			LmsInstance.Inst.UiInstance.Graph.pMin = pMin;
			LmsInstance.Inst.UiInstance.Graph.pMax = pMax;
			LmsInstance.Inst.UiInstance.Graph.pVal = pVal;

			if (Items < 0)
			{
				Items = 0;
			}
			if (Items > lms2012.GRAPH_BUFFERS)
			{
				Items = lms2012.GRAPH_BUFFERS;
			}


			LmsInstance.Inst.UiInstance.Graph.GraphStartX = StartX;
			LmsInstance.Inst.UiInstance.Graph.GraphSizeX = SizeX;
			LmsInstance.Inst.UiInstance.Graph.Items = Items;
			LmsInstance.Inst.UiInstance.Graph.Pointer = 0;

			for (Item = 0; Item < LmsInstance.Inst.UiInstance.Graph.Items; Item++)
			{
				for (Pointer = 0; Pointer < LmsInstance.Inst.UiInstance.Graph.GraphSizeX; Pointer++)
				{
					LmsInstance.Inst.UiInstance.Graph.Buffer[Item][Pointer] = float.NaN;
				}
			}

			LmsInstance.Inst.UiInstance.Graph.Initialized = 1;

			// Simulate graph
			LmsInstance.Inst.UiInstance.Graph.Value = LmsInstance.Inst.UiInstance.Graph.pMin[0];
			LmsInstance.Inst.UiInstance.Graph.Down = 0;
			LmsInstance.Inst.UiInstance.Graph.Inc = (LmsInstance.Inst.UiInstance.Graph.pMax[0] - LmsInstance.Inst.UiInstance.Graph.pMin[0]) / (float)20;
		}

		void cUiGraphSample()
		{
			float Sample;
			short Item;
			short Pointer;

			if (LmsInstance.Inst.UiInstance.Graph.Initialized != 0)
			{ // Only if initialized

				if (LmsInstance.Inst.UiInstance.Graph.Pointer < LmsInstance.Inst.UiInstance.Graph.GraphSizeX)
				{
					for (Item = 0; Item < (LmsInstance.Inst.UiInstance.Graph.Items); Item++)
					{
						// Insert sample
						Sample = LmsInstance.Inst.UiInstance.Graph.pVal[Item];

						if (!(float.IsNaN(Sample)))
						{
							LmsInstance.Inst.UiInstance.Graph.Buffer[Item][LmsInstance.Inst.UiInstance.Graph.Pointer] = Sample;
						}
						else
						{
							LmsInstance.Inst.UiInstance.Graph.Buffer[Item][LmsInstance.Inst.UiInstance.Graph.Pointer] = float.NaN;
						}
					}
					LmsInstance.Inst.UiInstance.Graph.Pointer++;
				}
				else
				{
					// Scroll buffers
					for (Item = 0; Item < (LmsInstance.Inst.UiInstance.Graph.Items); Item++)
					{
						for (Pointer = 0; Pointer < (LmsInstance.Inst.UiInstance.Graph.GraphSizeX - 1); Pointer++)
						{
							LmsInstance.Inst.UiInstance.Graph.Buffer[Item][Pointer] = LmsInstance.Inst.UiInstance.Graph.Buffer[Item][Pointer + 1];
						}

						// Insert sample
						Sample = LmsInstance.Inst.UiInstance.Graph.pVal[Item];

						if (!(float.IsNaN(Sample)))
						{
							LmsInstance.Inst.UiInstance.Graph.Buffer[Item][Pointer] = Sample;
						}
						else
						{
							LmsInstance.Inst.UiInstance.Graph.Buffer[Item][Pointer] = float.NaN;
						}
					}
				}
			}
		}
		void cUiGraphDraw(byte View, ref float pActual, ref float pLowest, ref float pHighest, ref float pAverage)
		{
			float Sample;
			byte Samples;
			short Value;
			short Item;
			short Pointer;
			short X;
			short Y1;
			short Y2;
			byte Color = 1;
			pActual = float.NaN;
			pLowest = float.NaN;
			pHighest = float.NaN;
			pAverage = float.NaN;
			Samples = 0;

			if (LmsInstance.Inst.UiInstance.Graph.Initialized != 0)
			{ 
				// Only if initialized
				if (LmsInstance.Inst.UiInstance.ScreenBlocked == 0)
				{
					// View or all
					if ((View >= 0) && (View < LmsInstance.Inst.UiInstance.Graph.Items))
					{
						Item = View;

						Y1 = (short)(LmsInstance.Inst.UiInstance.Graph.pOffset[Item] + LmsInstance.Inst.UiInstance.Graph.pSpan[Item]);

						// Draw buffers
						X = LmsInstance.Inst.UiInstance.Graph.GraphStartX;
						for (Pointer = 0; Pointer < LmsInstance.Inst.UiInstance.Graph.Pointer; Pointer++)
						{
							Sample = LmsInstance.Inst.UiInstance.Graph.Buffer[Item][Pointer];
							if (!(float.IsNaN(Sample)))
							{
								pActual = Sample;
								if (float.IsNaN(pAverage))
								{
									pAverage = (float)0;
									pLowest = pActual;
									pHighest = pActual;
								}
								else
								{
									if (pActual < pLowest)
									{
										pLowest = pActual;
									}
									if (pActual > pHighest)
									{
										pHighest = pActual;
									}
								}
								pAverage += pActual;
								Samples++;

								// Scale Y axis
								Value = (short)((((Sample - LmsInstance.Inst.UiInstance.Graph.pMin[Item]) * (float)LmsInstance.Inst.UiInstance.Graph.pSpan[Item]) / (LmsInstance.Inst.UiInstance.Graph.pMax[Item] - LmsInstance.Inst.UiInstance.Graph.pMin[Item])));

								// Limit Y axis
								if (Value > LmsInstance.Inst.UiInstance.Graph.pSpan[Item])
								{
									Value = LmsInstance.Inst.UiInstance.Graph.pSpan[Item];
								}
								if (Value < 0)
								{
									Value = 0;
								}
								/*
											printf("S=%-3d V=%3.0f L=%3.0f H=%3.0f A=%3.0f v=%3.0f ^=%3.0f O=%3d S=%3d Y=%d\r\n",Samples,*pActual,*pLowest,*pHighest,*pAverage,UiInstance.Graph.pMin[Item],UiInstance.Graph.pMax[Item],UiInstance.Graph.pOffset[Item],UiInstance.Graph.pSpan[Item],Value);
								*/
								Y2 = (short)((LmsInstance.Inst.UiInstance.Graph.pOffset[Item] + LmsInstance.Inst.UiInstance.Graph.pSpan[Item]) - Value);
								if (Pointer > 1)
								{
									if (Y2 > Y1)
									{
										LmsInstance.Inst.DlcdClass.dLcdDrawLine(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X - 2), (short)(Y1 - 1), (short)(X - 1), (short)(Y2 - 1));
										LmsInstance.Inst.DlcdClass.dLcdDrawLine(LmsInstance.Inst.UiInstance.Lcd, Color, X, (short)(Y1 + 1), (short)(X + 1), (short)(Y2 + 1));
									}
									else
									{
										if (Y2 < Y1)
										{
											LmsInstance.Inst.DlcdClass.dLcdDrawLine(LmsInstance.Inst.UiInstance.Lcd, Color, X, (short)(Y1 - 1), (short)(X + 1), (short)(Y2 - 1));
											LmsInstance.Inst.DlcdClass.dLcdDrawLine(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X - 2), (short)(Y1 + 1), (short)(X - 1), (short)(Y2 + 1));
										}
										else
										{
											LmsInstance.Inst.DlcdClass.dLcdDrawLine(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X - 1), (short)(Y1 - 1), X, (short)(Y2 - 1));
											LmsInstance.Inst.DlcdClass.dLcdDrawLine(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X - 1), (short)(Y1 + 1), X, (short)(Y2 + 1));
										}
									}
									LmsInstance.Inst.DlcdClass.dLcdDrawLine(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X - 1), Y1, X, Y2);
								}
								else
								{
									LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, X, Y2);
								}
								Y1 = Y2;
							}
							X++;
						}
						if (Samples != 0)
						{
							pAverage = pAverage / (float)Samples;
						}
					}
					else
					{
						// Draw buffers
						for (Item = 0; Item < LmsInstance.Inst.UiInstance.Graph.Items; Item++)
						{
							Y1 = (short)(LmsInstance.Inst.UiInstance.Graph.pOffset[Item] + LmsInstance.Inst.UiInstance.Graph.pSpan[Item]);

							X = (short)(LmsInstance.Inst.UiInstance.Graph.GraphStartX + 1);
							for (Pointer = 0; Pointer < LmsInstance.Inst.UiInstance.Graph.Pointer; Pointer++)
							{
								Sample = LmsInstance.Inst.UiInstance.Graph.Buffer[Item][Pointer];

								// Scale Y axis
								Value = (short)((((Sample - LmsInstance.Inst.UiInstance.Graph.pMin[Item]) * (float)LmsInstance.Inst.UiInstance.Graph.pSpan[Item]) / (LmsInstance.Inst.UiInstance.Graph.pMax[Item] - LmsInstance.Inst.UiInstance.Graph.pMin[Item])));

								// Limit Y axis
								if (Value > LmsInstance.Inst.UiInstance.Graph.pSpan[Item])
								{
									Value = LmsInstance.Inst.UiInstance.Graph.pSpan[Item];
								}
								if (Value < 0)
								{
									Value = 0;
								}
								Y2 = (short)((LmsInstance.Inst.UiInstance.Graph.pOffset[Item] + LmsInstance.Inst.UiInstance.Graph.pSpan[Item]) - Value);
								if (Pointer > 1)
								{
									if (Y2 > Y1)
									{
										LmsInstance.Inst.DlcdClass.dLcdDrawLine(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X - 2), (short)(Y1 - 1), (short)(X - 1), (short)(Y2 - 1));
										LmsInstance.Inst.DlcdClass.dLcdDrawLine(LmsInstance.Inst.UiInstance.Lcd, Color, X, (short)(Y1 + 1), (short)(X + 1), (short)(Y2 + 1));
									}
									else
									{
										if (Y2 < Y1)
										{
											LmsInstance.Inst.DlcdClass.dLcdDrawLine(LmsInstance.Inst.UiInstance.Lcd, Color, X, (short)(Y1 - 1), (short)(X + 1), (short)(Y2 - 1));
											LmsInstance.Inst.DlcdClass.dLcdDrawLine(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X - 2), (short)(Y1 + 1), (short)(X - 1), (short)(Y2 + 1));
										}
										else
										{
											LmsInstance.Inst.DlcdClass.dLcdDrawLine(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X - 1), (short)(Y1 - 1), X, (short)(Y2 - 1));
											LmsInstance.Inst.DlcdClass.dLcdDrawLine(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X - 1), (short)(Y1 + 1), X, (short)(Y2 + 1));
										}
									}
									LmsInstance.Inst.DlcdClass.dLcdDrawLine(LmsInstance.Inst.UiInstance.Lcd, Color, (short)(X - 1), Y1, X, Y2);
								}
								else
								{
									LmsInstance.Inst.DlcdClass.dLcdDrawPixel(LmsInstance.Inst.UiInstance.Lcd, Color, X, Y2);
								}
								Y1 = Y2;
								X++;
							}
						}
					}
					LmsInstance.Inst.UiInstance.ScreenBusy = 1;
				}
			}
		}

		public unsafe void cUiDraw()
		{
			ushort TmpPrgId;
			ushort TmpObjId;
			byte[] TmpIp;
			byte[] GBuffer = new byte[25];
			byte[] pBmp = new byte[lms2012.LCD_BUFFER_SIZE];
			byte Cmd;
			byte Color;
			short X;
			short Y;
			short X1;
			short Y1;
			short Y2;
			short Y3;
			int Size;
			short R;
			byte[] pText;
			byte No;
			byte DataF;
			byte Figures;
			byte Decimals;
			byte[] pI;
			byte pState;
			byte[] pAnswer;
			byte Lng;
			byte SelectedChar;
			sbyte pType;
			byte Type;
			short On;
			short Off;
			short CharWidth;
			short CharHeight;
			byte TmpColor;
			short Tmp;
			byte Length;
			byte[] pUnit;
			int pIcons;
			byte Items;
			byte View;
			short[] pOffset;
			short[] pSpan;
			float[] pMin;
			float[] pMax;
			float[] pVal;
			short Min;
			short Max;
			short Act;
			float Actual;
			float Lowest;
			float Highest;
			float Average;
			byte Icon1;
			byte Icon2;
			byte Icon3;
			byte Blocked;
			byte Open;
			byte Del;
			byte pCharSet;
			short pLine;

			TmpPrgId = LmsInstance.Inst.CurrentProgramId();

			if ((TmpPrgId != (short)lms2012.Slot.GUI_SLOT) && (TmpPrgId != (short)lms2012.Slot.DEBUG_SLOT))
			{
				LmsInstance.Inst.UiInstance.RunScreenEnabled = 0;
			}
			if (LmsInstance.Inst.UiInstance.ScreenBlocked == 0)
			{
				Blocked = 0;
			}
			else
			{
				TmpObjId = LmsInstance.Inst.CallingObjectId();
				if ((TmpPrgId == LmsInstance.Inst.UiInstance.ScreenPrgId) && (TmpObjId == LmsInstance.Inst.UiInstance.ScreenObjId))
				{
					Blocked = 0;
				}
				else
				{
					Blocked = 1;
				}
			}

			TmpIp = LmsInstance.Inst.GetObjectIp();
			Cmd = *(byte*)LmsInstance.Inst.PrimParPointer();

			switch (Cmd)
			{ 
				// Function
				case (byte)lms2012.UiDrawSubcode.UPDATE:
					{
						if (Blocked == 0)
						{
							cUiUpdateLcd();
							UiInstance.ScreenBusy = 0;
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.CLEAN:
					{
						if (Blocked == 0)
						{
							UiInstance.Font = NORMAL_FONT;

							Color = BG_COLOR;
							if (Color)
							{
								Color = -1;
							}
							memset(&((*UiInstance.pLcd).Lcd[0]), Color, LCD_BUFFER_SIZE);

							UiInstance.ScreenBusy = 1;
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.TEXTBOX:
					{
						X = *(DATA16*)PrimParPointer();  // start x
						Y = *(DATA16*)PrimParPointer();  // start y
						X1 = *(DATA16*)PrimParPointer();  // size x
						Y1 = *(DATA16*)PrimParPointer();  // size y
						pText = (DATA8*)PrimParPointer();    // textbox
						Size = *(DATA32*)PrimParPointer();  // textbox size
						Del = *(DATA8*)PrimParPointer();   // delimitter
						pLine = (DATA16*)PrimParPointer();   // line

						if (Blocked == 0)
						{
							if (cUiTextbox(X, Y, X1, Y1, pText, Size, Del, pLine) == BUSY)
							{
								SetObjectIp(TmpIp - 1);
								SetDispatchStatus(BUSYBREAK);
							}
						}
						else
						{
							SetObjectIp(TmpIp - 1);
							SetDispatchStatus(BUSYBREAK);
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.FILLRECT:
					{
						Color = *(DATA8*)PrimParPointer();
						X = *(DATA16*)PrimParPointer();
						Y = *(DATA16*)PrimParPointer();
						X1 = *(DATA16*)PrimParPointer();
						Y1 = *(DATA16*)PrimParPointer();
						if (Blocked == 0)
						{
							dLcdFillRect((*UiInstance.pLcd).Lcd, Color, X, Y, X1, Y1);
							UiInstance.ScreenBusy = 1;
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.INVERSERECT:
					{
						X = *(DATA16*)PrimParPointer();
						Y = *(DATA16*)PrimParPointer();
						X1 = *(DATA16*)PrimParPointer();
						Y1 = *(DATA16*)PrimParPointer();
						if (Blocked == 0)
						{
							dLcdInverseRect((*UiInstance.pLcd).Lcd, X, Y, X1, Y1);
							UiInstance.ScreenBusy = 1;
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.RECT:
					{
						Color = *(DATA8*)PrimParPointer();
						X = *(DATA16*)PrimParPointer();
						Y = *(DATA16*)PrimParPointer();
						X1 = *(DATA16*)PrimParPointer();
						Y1 = *(DATA16*)PrimParPointer();
						if (Blocked == 0)
						{
							dLcdRect((*UiInstance.pLcd).Lcd, Color, X, Y, X1, Y1);
							UiInstance.ScreenBusy = 1;
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.PIXEL:
					{
						Color = *(DATA8*)PrimParPointer();
						X = *(DATA16*)PrimParPointer();
						Y = *(DATA16*)PrimParPointer();
						if (Blocked == 0)
						{
							dLcdDrawPixel((*UiInstance.pLcd).Lcd, Color, X, Y);
							UiInstance.ScreenBusy = 1;
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.LINE:
					{
						Color = *(DATA8*)PrimParPointer();
						X = *(DATA16*)PrimParPointer();
						Y = *(DATA16*)PrimParPointer();
						X1 = *(DATA16*)PrimParPointer();
						Y1 = *(DATA16*)PrimParPointer();
						if (Blocked == 0)
						{
							dLcdDrawLine((*UiInstance.pLcd).Lcd, Color, X, Y, X1, Y1);
							UiInstance.ScreenBusy = 1;
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.DOTLINE:
					{
						Color = *(DATA8*)PrimParPointer();
						X = *(DATA16*)PrimParPointer();
						Y = *(DATA16*)PrimParPointer();
						X1 = *(DATA16*)PrimParPointer();
						Y1 = *(DATA16*)PrimParPointer();
						On = *(DATA16*)PrimParPointer();
						Off = *(DATA16*)PrimParPointer();
						if (Blocked == 0)
						{
							dLcdDrawDotLine((*UiInstance.pLcd).Lcd, Color, X, Y, X1, Y1, On, Off);
							UiInstance.ScreenBusy = 1;
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.CIRCLE:
					{
						Color = *(DATA8*)PrimParPointer();
						X = *(DATA16*)PrimParPointer();
						Y = *(DATA16*)PrimParPointer();
						R = *(DATA16*)PrimParPointer();
						if (R)
						{
							if (Blocked == 0)
							{
								dLcdDrawCircle((*UiInstance.pLcd).Lcd, Color, X, Y, R);
								UiInstance.ScreenBusy = 1;
							}
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.FILLCIRCLE:
					{
						Color = *(DATA8*)PrimParPointer();
						X = *(DATA16*)PrimParPointer();
						Y = *(DATA16*)PrimParPointer();
						R = *(DATA16*)PrimParPointer();
						if (R)
						{
							if (Blocked == 0)
							{
								dLcdDrawFilledCircle((*UiInstance.pLcd).Lcd, Color, X, Y, R);
								UiInstance.ScreenBusy = 1;
							}
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.TEXT:
					{
						Color = *(DATA8*)PrimParPointer();
						X = *(DATA16*)PrimParPointer();
						Y = *(DATA16*)PrimParPointer();
						pText = (DATA8*)PrimParPointer();
						if (Blocked == 0)
						{
							dLcdDrawText((*UiInstance.pLcd).Lcd, Color, X, Y, UiInstance.Font, pText);
							UiInstance.ScreenBusy = 1;
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.ICON:
					{
						Color = *(DATA8*)PrimParPointer();
						X = *(DATA16*)PrimParPointer();
						Y = *(DATA16*)PrimParPointer();
						Type = *(DATA8*)PrimParPointer();
						No = *(DATA8*)PrimParPointer();
						if (Blocked == 0)
						{
							dLcdDrawIcon((*UiInstance.pLcd).Lcd, Color, X, Y, Type, No);
							UiInstance.ScreenBusy = 1;
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.BMPFILE:
					{
						Color = *(DATA8*)PrimParPointer();
						X = *(DATA16*)PrimParPointer();
						Y = *(DATA16*)PrimParPointer();
						pText = (DATA8*)PrimParPointer();

						if (Blocked == 0)
						{
							if (cMemoryGetImage(pText, LCD_BUFFER_SIZE, pBmp) == OK)
							{
								dLcdDrawBitmap((*UiInstance.pLcd).Lcd, Color, X, Y, pBmp);
								UiInstance.ScreenBusy = 1;
							}
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.PICTURE:
					{
						Color = *(DATA8*)PrimParPointer();
						X = *(DATA16*)PrimParPointer();
						Y = *(DATA16*)PrimParPointer();
						pI = *(IP*)PrimParPointer();
						if (pI != NULL)
						{
							if (Blocked == 0)
							{
								dLcdDrawBitmap((*UiInstance.pLcd).Lcd, Color, X, Y, pI);
								UiInstance.ScreenBusy = 1;
							}
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.VALUE:
					{
						Color = *(DATA8*)PrimParPointer();
						X = *(DATA16*)PrimParPointer();
						Y = *(DATA16*)PrimParPointer();
						DataF = *(DATAF*)PrimParPointer();
						Figures = *(DATA8*)PrimParPointer();
						Decimals = *(DATA8*)PrimParPointer();

						if (isnan(DataF))
						{
							for (Lng = 0; Lng < Figures; Lng++)
							{
								GBuffer[Lng] = '-';
							}
						}
						else
						{
							if (Figures < 0)
							{
								Figures = 0 - Figures;
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
							dLcdDrawText((*UiInstance.pLcd).Lcd, Color, X, Y, UiInstance.Font, pText);
							UiInstance.ScreenBusy = 1;
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.VIEW_VALUE:
					{
						Color = *(DATA8*)PrimParPointer();
						X = *(DATA16*)PrimParPointer();
						Y = *(DATA16*)PrimParPointer();
						DataF = *(DATAF*)PrimParPointer();
						Figures = *(DATA8*)PrimParPointer();
						Decimals = *(DATA8*)PrimParPointer();
						if (Blocked == 0)
						{

							TmpColor = Color;
							CharWidth = dLcdGetFontWidth(UiInstance.Font);
							CharHeight = dLcdGetFontHeight(UiInstance.Font);
							X1 = ((CharWidth + 2) / 3) - 1;
							Y1 = (CharHeight / 2);

							Lng = (DATA8)snprintf((char*)GBuffer, 24, "%.*f", Decimals, DataF);

							if (Lng)
							{
								if (GBuffer[0] == '-')
								{ // Negative

									TmpColor = Color;
									Lng--;
									pText = &GBuffer[1];
								}
								else
								{ // Positive

									TmpColor = 1 - Color;
									pText = GBuffer;
								}

								// Make sure negative sign is deleted from last time
								dLcdDrawLine((*UiInstance.pLcd).Lcd, 1 - Color, X - X1, Y + Y1, X + (Figures * CharWidth), Y + Y1);
								if (CharHeight > 12)
								{
									dLcdDrawLine((*UiInstance.pLcd).Lcd, 1 - Color, X - X1, Y + Y1 - 1, X + (Figures * CharWidth), Y + Y1 - 1);
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
										GBuffer[Tmp] = '-';
									}
									GBuffer[Tmp] = 0;

									// Draw figures
									dLcdDrawText((*UiInstance.pLcd).Lcd, Color, X, Y, UiInstance.Font, GBuffer);
								}
								else
								{ // Normal number

									// Check number of figures
									if (Lng > Figures)
									{ // Limit figures

										for (Tmp = 0; Tmp < (DATA16)Figures; Tmp++)
										{
											GBuffer[Tmp] = '>';
										}
										GBuffer[Tmp] = 0;
										Lng = (DATA16)Figures;
										pText = GBuffer;
										TmpColor = 1 - Color;

										// Find X indent
										Tmp = ((DATA16)Figures - Lng) * CharWidth;
									}
									else
									{ // Centre figures

										// Find X indent
										Tmp = ((((DATA16)Figures - Lng) + 1) / 2) * CharWidth;
									}

									// Draw figures
									dLcdDrawText((*UiInstance.pLcd).Lcd, Color, X + Tmp, Y, UiInstance.Font, pText);

									// Draw negative sign
									dLcdDrawLine((*UiInstance.pLcd).Lcd, TmpColor, X - X1 + Tmp, Y + Y1, X + Tmp, Y + Y1);
									if (CharHeight > 12)
									{
										dLcdDrawLine((*UiInstance.pLcd).Lcd, TmpColor, X - X1 + Tmp, Y + Y1 - 1, X + Tmp, Y + Y1 - 1);
									}
								}
							}
							UiInstance.ScreenBusy = 1;
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.VIEW_UNIT:
					{
						Color = *(DATA8*)PrimParPointer();
						X = *(DATA16*)PrimParPointer();
						Y = *(DATA16*)PrimParPointer();
						DataF = *(DATAF*)PrimParPointer();
						Figures = *(DATA8*)PrimParPointer();
						Decimals = *(DATA8*)PrimParPointer();
						Length = *(DATA8*)PrimParPointer();
						pUnit = (DATA8*)PrimParPointer();

						if (Blocked == 0)
						{
							TmpColor = Color;
							CharWidth = dLcdGetFontWidth(LARGE_FONT);
							CharHeight = dLcdGetFontHeight(LARGE_FONT);
							X1 = ((CharWidth + 2) / 3) - 1;
							Y1 = (CharHeight / 2);

							Lng = (DATA8)snprintf((char*)GBuffer, 24, "%.*f", Decimals, DataF);

							if (Lng)
							{
								if (GBuffer[0] == '-')
								{ // Negative

									TmpColor = Color;
									Lng--;
									pText = &GBuffer[1];
								}
								else
								{ // Positive

									TmpColor = 1 - Color;
									pText = GBuffer;
								}

								// Make sure negative sign is deleted from last time
								dLcdDrawLine((*UiInstance.pLcd).Lcd, 1 - Color, X - X1, Y + Y1, X + (Figures * CharWidth), Y + Y1);
								if (CharHeight > 12)
								{
									dLcdDrawLine((*UiInstance.pLcd).Lcd, 1 - Color, X - X1, Y + Y1 - 1, X + (Figures * CharWidth), Y + Y1 - 1);
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
										GBuffer[Tmp] = '-';
									}
									GBuffer[Tmp] = 0;

									// Draw figures
									dLcdDrawText((*UiInstance.pLcd).Lcd, Color, X, Y, LARGE_FONT, GBuffer);
								}
								else
								{ // Normal number

									// Check number of figures
									if (Lng > Figures)
									{ // Limit figures

										for (Tmp = 0; Tmp < (DATA16)Figures; Tmp++)
										{
											GBuffer[Tmp] = '>';
										}
										GBuffer[Tmp] = 0;
										Lng = (DATA16)Figures;
										pText = GBuffer;
										TmpColor = 1 - Color;

										// Find X indent
										Tmp = ((DATA16)Figures - Lng) * CharWidth;
									}
									else
									{ // Centre figures

										// Find X indent
										Tmp = ((((DATA16)Figures - Lng) + 1) / 2) * CharWidth;
									}
									Tmp = 0;

									// Draw figures
									dLcdDrawText((*UiInstance.pLcd).Lcd, Color, X + Tmp, Y, LARGE_FONT, pText);

									// Draw negative sign
									dLcdDrawLine((*UiInstance.pLcd).Lcd, TmpColor, X - X1 + Tmp, Y + Y1, X + Tmp, Y + Y1);
									if (CharHeight > 12)
									{
										dLcdDrawLine((*UiInstance.pLcd).Lcd, TmpColor, X - X1 + Tmp, Y + Y1 - 1, X + Tmp, Y + Y1 - 1);
									}

									Tmp = ((((DATA16)Lng))) * CharWidth;
									snprintf((char*)GBuffer, Length, "%s", pUnit);
									dLcdDrawText((*UiInstance.pLcd).Lcd, Color, X + Tmp, Y, SMALL_FONT, GBuffer);

								}
							}
							UiInstance.ScreenBusy = 1;
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.NOTIFICATION:
					{
						Color = *(DATA8*)PrimParPointer();
						X = *(DATA16*)PrimParPointer();  // start x
						Y = *(DATA16*)PrimParPointer();  // start y
						Icon1 = *(DATA8*)PrimParPointer();
						Icon2 = *(DATA8*)PrimParPointer();
						Icon3 = *(DATA8*)PrimParPointer();
						pText = (DATA8*)PrimParPointer();
						pState = (DATA8*)PrimParPointer();

						if (Blocked == 0)
						{
							if (cUiNotification(Color, X, Y, Icon1, Icon2, Icon3, pText, pState) == BUSY)
							{
								SetObjectIp(TmpIp - 1);
								SetDispatchStatus(BUSYBREAK);
							}
						}
						else
						{
							SetObjectIp(TmpIp - 1);
							SetDispatchStatus(BUSYBREAK);
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.QUESTION:
					{
						Color = *(DATA8*)PrimParPointer();
						X = *(DATA16*)PrimParPointer();  // start x
						Y = *(DATA16*)PrimParPointer();  // start y
						Icon1 = *(DATA8*)PrimParPointer();
						Icon2 = *(DATA8*)PrimParPointer();
						pText = (DATA8*)PrimParPointer();
						pState = (DATA8*)PrimParPointer();
						pAnswer = (DATA8*)PrimParPointer();

						if (Blocked == 0)
						{
							if (cUiQuestion(Color, X, Y, Icon1, Icon2, pText, pState, pAnswer) == BUSY)
							{
								SetObjectIp(TmpIp - 1);
								SetDispatchStatus(BUSYBREAK);
							}
						}
						else
						{
							SetObjectIp(TmpIp - 1);
							SetDispatchStatus(BUSYBREAK);
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.ICON_QUESTION:
					{
						Color = *(DATA8*)PrimParPointer();
						X = *(DATA16*)PrimParPointer();  // start x
						Y = *(DATA16*)PrimParPointer();  // start y
						pState = (DATA8*)PrimParPointer();
						pIcons = (DATA32*)PrimParPointer();

						if (Blocked == 0)
						{
							if (cUiIconQuestion(Color, X, Y, pState, pIcons) == BUSY)
							{
								SetObjectIp(TmpIp - 1);
								SetDispatchStatus(BUSYBREAK);
							}
						}
						else
						{
							SetObjectIp(TmpIp - 1);
							SetDispatchStatus(BUSYBREAK);
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.KEYBOARD:
					{
						Color = *(DATA8*)PrimParPointer();
						X = *(DATA16*)PrimParPointer();  // start x
						Y = *(DATA16*)PrimParPointer();  // start y
						No = *(DATA8*)PrimParPointer();   // Icon
						Lng = *(DATA8*)PrimParPointer();   // length
						pText = (DATA8*)PrimParPointer();    // default
						pCharSet = (DATA8*)PrimParPointer();    // valid char set
						pAnswer = (DATA8*)PrimParPointer();    // string

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
								SetObjectIp(TmpIp - 1);
								SetDispatchStatus(BUSYBREAK);
							}
						}
						else
						{
							SetObjectIp(TmpIp - 1);
							SetDispatchStatus(BUSYBREAK);
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.BROWSE:
					{
						Type = *(DATA8*)PrimParPointer();   // Browser type
						X = *(DATA16*)PrimParPointer();  // start x
						Y = *(DATA16*)PrimParPointer();  // start y
						X1 = *(DATA16*)PrimParPointer();  // size x
						Y1 = *(DATA16*)PrimParPointer();  // size y
						Lng = *(DATA8*)PrimParPointer();   // length
						pType = (DATA8*)PrimParPointer();    // item type
						pAnswer = (DATA8*)PrimParPointer();    // item name

						if (VMInstance.Handle >= 0)
						{
							pAnswer = (DATA8*)VmMemoryResize(VMInstance.Handle, (DATA32)Lng);
						}

						if (Blocked == 0)
						{
							if (cUiBrowser(Type, X, Y, X1, Y1, Lng, pType, pAnswer) == BUSY)
							{
								SetObjectIp(TmpIp - 1);
								SetDispatchStatus(BUSYBREAK);
							}
						}
						else
						{
							SetObjectIp(TmpIp - 1);
							SetDispatchStatus(BUSYBREAK);
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.VERTBAR:
					{
						Color = *(DATA8*)PrimParPointer();
						X = *(DATA16*)PrimParPointer();  // start x
						Y = *(DATA16*)PrimParPointer();  // start y
						X1 = *(DATA16*)PrimParPointer();  // size x
						Y1 = *(DATA16*)PrimParPointer();  // size y
						Min = *(DATA16*)PrimParPointer();  // min
						Max = *(DATA16*)PrimParPointer();  // max
						Act = *(DATA16*)PrimParPointer();  // actual

						if (Blocked == 0)
						{
							cUiDrawBar(Color, X, Y, X1, Y1, Min, Max, Act);
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.SELECT_FONT:
					{
						UiInstance.Font = *(DATA8*)PrimParPointer();
						if (Blocked == 0)
						{
							if (UiInstance.Font >= FONTTYPES)
							{
								UiInstance.Font = (FONTTYPES - 1);
							}
							if (UiInstance.Font < 0)
							{
								UiInstance.Font = 0;
							}
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.TOPLINE:
					{
						UiInstance.TopLineEnabled = *(DATA8*)PrimParPointer();
					}
					break;
				case (byte)lms2012.UiDrawSubcode.FILLWINDOW:
					{
						Color = *(DATA8*)PrimParPointer();
						Y = *(DATA16*)PrimParPointer();  // start y
						Y1 = *(DATA16*)PrimParPointer();  // size y
						if (Blocked == 0)
						{
							UiInstance.Font = NORMAL_FONT;

							if ((Y + Y1) < LCD_HEIGHT)
							{
								if ((Color == 0) || (Color == 1))
								{
									Y *= ((LCD_WIDTH + 7) / 8);

									if (Y1)
									{
										Y1 *= ((LCD_WIDTH + 7) / 8);
									}
									else
									{
										Y1 = LCD_BUFFER_SIZE - Y;
									}

									if (Color)
									{
										Color = -1;
									}
									memset(&((*UiInstance.pLcd).Lcd[Y]), Color, Y1);
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
										Y3 = Tmp * ((LCD_WIDTH + 7) / 8);
										memset(&((*UiInstance.pLcd).Lcd[Y3]), Color, Y2);
										Color = ~Color;
									}
								}
							}

							UiInstance.ScreenBusy = 1;
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.STORE:
					{
						No = *(DATA8*)PrimParPointer();
						if (Blocked == 0)
						{
							if (No < LCD_STORE_LEVELS)
							{
								LCDCopy(&UiInstance.LcdSafe, &UiInstance.LcdPool[No], sizeof(LCD));
							}
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.RESTORE:
					{
						No = *(DATA8*)PrimParPointer();
						if (Blocked == 0)
						{
							if (No < LCD_STORE_LEVELS)
							{
								LCDCopy(&UiInstance.LcdPool[No], &UiInstance.LcdSafe, sizeof(LCD));
								UiInstance.ScreenBusy = 1;
							}
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.GRAPH_SETUP:
					{
						X = *(DATA16*)PrimParPointer();  // start x
						X1 = *(DATA16*)PrimParPointer();  // size y
						Items = *(DATA8*)PrimParPointer();   // items
						pOffset = (DATA16*)PrimParPointer();  // handle to offset Y
						pSpan = (DATA16*)PrimParPointer();  // handle to span y
						pMin = (DATAF*)PrimParPointer();  // handle to min
						pMax = (DATAF*)PrimParPointer();  // handle to max
						pVal = (DATAF*)PrimParPointer();  // handle to val

						if (Blocked == 0)
						{
							cUiGraphSetup(X, X1, Items, pOffset, pSpan, pMin, pMax, pVal);
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.GRAPH_DRAW:
					{
						View = *(DATA8*)PrimParPointer();   // view

						cUiGraphDraw(View, &Actual, &Lowest, &Highest, &Average);

						*(DATAF*)PrimParPointer() = Actual;
						*(DATAF*)PrimParPointer() = Lowest;
						*(DATAF*)PrimParPointer() = Highest;
						*(DATAF*)PrimParPointer() = Average;
					}
					break;
				case (byte)lms2012.UiDrawSubcode.SCROLL:
					{
						Y = *(DATA16*)PrimParPointer();
						if ((Y > 0) && (Y < LCD_HEIGHT))
						{
							dLcdScroll((*UiInstance.pLcd).Lcd, Y);
						}
					}
					break;
				case (byte)lms2012.UiDrawSubcode.POPUP:
					{
						Open = *(DATA8*)PrimParPointer();
						if (Blocked == 0)
						{
							if (Open)
							{
								if (!UiInstance.ScreenBusy)
								{
									TmpObjId = CallingObjectId();

									LCDCopy(&UiInstance.LcdSafe, &UiInstance.LcdSave, sizeof(UiInstance.LcdSave));
									UiInstance.ScreenPrgId = TmpPrgId;
									UiInstance.ScreenObjId = TmpObjId;
									UiInstance.ScreenBlocked = 1;
								}
								else
								{ // Wait on scrreen

									SetObjectIp(TmpIp - 1);
									SetDispatchStatus(BUSYBREAK);
								}
							}
							else
							{
								LCDCopy(&UiInstance.LcdSave, &UiInstance.LcdSafe, sizeof(UiInstance.LcdSafe));
								dLcdUpdate(UiInstance.pLcd);

								UiInstance.ScreenPrgId = -1;
								UiInstance.ScreenObjId = -1;
								UiInstance.ScreenBlocked = 0;
							}
						}
						else
						{ // Wait on not blocked

							SetObjectIp(TmpIp - 1);
							SetDispatchStatus(BUSYBREAK);
						}
					}
					break;
			}
		}
	}
}
