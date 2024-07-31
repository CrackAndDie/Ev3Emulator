using Ev3Core;
using Ev3Core.Cui.Interfaces;
using Ev3Core.Enums;
using Ev3Core.Extensions;
using Ev3Core.Helpers;
using Ev3Core.Lms2012.Interfaces;
using System.Collections.Generic;
using System.Security.Cryptography;
using static Ev3Core.Defines;

namespace Ev3Core.Cui
{
	public class Ui : IUi
	{
		#region battery shite 
		/*************************** Model parameters *******************************/
		//Approx. initial internal resistance of 6 Energizer industrial batteries :
		float R_bat_init = 0.63468f;
		//Bjarke's proposal for spring resistance :
		//float spring_resistance = 0.42;
		//Batteries' heat capacity :
		float heat_cap_bat = 136.6598f;
		//Newtonian cooling constant for electronics :
		float K_bat_loss_to_elec = -0.0003f; //-0.000789767;
											 //Newtonian heating constant for electronics :
		float K_bat_gain_from_elec = 0.001242896f; //0.001035746;
												   //Newtonian cooling constant for environment :
		float K_bat_to_room = -0.00012f;
		//Battery power Boost
		float battery_power_boost = 1.7f;
		//Battery R_bat negative gain
		float R_bat_neg_gain = 1.00f;

		//Slope of electronics lossless heating curve (linear!!!) [Deg.C / s] :
		float K_elec_heat_slope = 0.0123175f;
		//Newtonian cooling constant for battery packs :
		float K_elec_loss_to_bat = -0.004137487f;
		//Newtonian heating constant for battery packs :
		float K_elec_gain_from_bat = 0.002027574f; //0.00152068;
												   //Newtonian cooling constant for environment :
		float K_elec_to_room = -0.001931431f; //-0.001843639;

		// anime
		byte has_passed_7v5_flag = (byte)'N';
		float R_bat = 0; //Internal resistance of the batteries
		float R_bat_model_old = 0;//Old internal resistance of the battery model
		float T_bat = 0; //Battery temperature
		float T_elec = 0; //EV3 electronics temperature
		int index = 0; //Keeps track of sample index since power-on
		float I_bat_mean = 0; //Running mean current

		// Function for estimating new battery temperature based on measurements
		// of battery voltage and battery power.
		float new_bat_temp(float V_bat, float I_bat)
		{
			const float sample_period = 0.4f; //Algorithm update period in seconds

			float R_bat_model;    //Internal resistance of the battery model

			float slope_A;        //Slope obtained by linear interpolation
			float intercept_b;    //Offset obtained by linear interpolation
			const float I_1A = 0.05f;      //Current carrying capacity at bottom of the curve
			const float I_2A = 2.0f;      //Current carrying capacity at the top of the curve

			float R_1A = 0.0f;          //Internal resistance of the batteries at 1A and V_bat
			float R_2A = 0.0f;          //Internal resistance of the batteries at 2A and V_bat

			//Flag that prevents initialization of R_bat when the battery is charging
			float dT_bat_own = 0.0f; //Batteries' own heat
			float dT_bat_loss_to_elec = 0.0f; // Batteries' heat loss to electronics
			float dT_bat_gain_from_elec = 0.0f; //Batteries' heat gain from electronics
			float dT_bat_loss_to_room = 0.0f; //Batteries' cooling from environment

			float dT_elec_own = 0.0f; //Electronics' own heat
			float dT_elec_loss_to_bat = 0.0f;//Electronics' heat loss to the battery pack
			float dT_elec_gain_from_bat = 0.0f;//Electronics' heat gain from battery packs
			float dT_elec_loss_to_room = 0.0f; //Electronics' heat loss to the environment

			/***************************************************************************/

			//Update the average current: I_bat_mean
			if (index > 0)
			{
				I_bat_mean = ((index) * I_bat_mean + I_bat) / (index + 1);
			}
			else
			{
				I_bat_mean = I_bat;
			}

			index = index + 1;

			//Calculate R_1A as a function of V_bat (internal resistance at 1A continuous)
			R_1A = 0.014071f * (V_bat * V_bat * V_bat * V_bat)
			  - 0.335324f * (V_bat * V_bat * V_bat)
			  + 2.933404f * (V_bat * V_bat)
			  - 11.243047f * V_bat
			  + 16.897461f;

			//Calculate R_2A as a function of V_bat (internal resistance at 2A continuous)
			R_2A = 0.014420f * (V_bat * V_bat * V_bat * V_bat)
			  - 0.316728f * (V_bat * V_bat * V_bat)
			  + 2.559347f * (V_bat * V_bat)
			  - 9.084076f * V_bat
			  + 12.794176f;

			//Calculate the slope by linear interpolation between R_1A and R_2A
			slope_A = (R_1A - R_2A) / (I_1A - I_2A);

			//Calculate intercept by linear interpolation between R1_A and R2_A
			intercept_b = R_1A - slope_A * R_1A;

			//Reload R_bat_model:
			R_bat_model = slope_A * I_bat_mean + intercept_b;

			//Calculate batteries' internal resistance: R_bat
			if ((V_bat > 7.5) && (has_passed_7v5_flag == 'N'))
			{
				R_bat = R_bat_init; //7.5 V not passed a first time
			}
			else
			{
				//Only update R_bat with positive outcomes: R_bat_model - R_bat_model_old
				//R_bat updated with the change in model R_bat is not equal value in the model!
				if ((R_bat_model - R_bat_model_old) > 0)
				{
					R_bat = R_bat + R_bat_model - R_bat_model_old;
				}
				else // The negative outcome of R_bat_model added to only part of R_bat
				{
					R_bat = R_bat + (R_bat_neg_gain * (R_bat_model - R_bat_model_old));
				}
				//Make sure we initialize R_bat later
				has_passed_7v5_flag = (byte)'Y';
			}

			//Save R_bat_model for use in the next function call
			R_bat_model_old = R_bat_model;

			//			//Debug code:
			//# ifdef __dbg1
			//			if (index < 500)
			//			{
			//				printf("%c %f %f %f %f %f %f\n", has_passed_7v5_flag, R_1A, R_2A,
			//					   slope_A, intercept_b, R_bat_model - R_bat_model_old, R_bat);
			//			}
			//#endif

			/*****Calculate the 4 types of temperature change for the batteries******/

			//Calculate the batteries' own temperature change
			dT_bat_own = R_bat * I_bat * I_bat * sample_period * battery_power_boost
						 / heat_cap_bat;

			//Calculate the batteries' heat loss to the electronics
			if (T_bat > T_elec)
			{
				dT_bat_loss_to_elec = K_bat_loss_to_elec * (T_bat - T_elec)
									  * sample_period;
			}
			else
			{
				dT_bat_loss_to_elec = 0.0f;
			}

			//Calculate the batteries' heat gain from the electronics
			if (T_bat < T_elec)
			{
				dT_bat_gain_from_elec = K_bat_gain_from_elec * (T_elec - T_bat)
				  * sample_period;
			}
			else
			{
				dT_bat_gain_from_elec = 0.0f;
			}

			//Calculate the batteries' heat loss to environment
			dT_bat_loss_to_room = K_bat_to_room * T_bat * sample_period;
			/************************************************************************/



			/*****Calculate the 4 types of temperature change for the electronics****/

			//Calculate the electronics' own temperature change
			dT_elec_own = K_elec_heat_slope * sample_period;

			//Calculate the electronics' heat loss to the batteries
			if (T_elec > T_bat)
			{
				dT_elec_loss_to_bat = K_elec_loss_to_bat * (T_elec - T_bat)
				  * sample_period;
			}
			else
			{
				dT_elec_loss_to_bat = 0.0f;
			}

			//Calculate the electronics' heat gain from the batteries
			if (T_elec < T_bat)
			{
				dT_elec_gain_from_bat = K_elec_gain_from_bat * (T_bat - T_elec)
				  * sample_period;
			}
			else
			{
				dT_elec_gain_from_bat = 0.0f;
			}

			//Calculate the electronics' heat loss to the environment
			dT_elec_loss_to_room = K_elec_to_room * T_elec * sample_period;

			//			/*****************************************************************************/
			//			//Debug code:
			//# ifdef __dbg2
			//			if (index < 500)
			//			{
			//				printf("%f %f %f %f %f <> %f %f %f %f %f\n", dT_bat_own, dT_bat_loss_to_elec,
			//					   dT_bat_gain_from_elec, dT_bat_loss_to_room, T_bat,
			//					   dT_elec_own, dT_elec_loss_to_bat, dT_elec_gain_from_bat,
			//					   dT_elec_loss_to_room, T_elec);
			//			}
			//#endif



			//Refresh battery temperature
			T_bat = T_bat + dT_bat_own + dT_bat_loss_to_elec
			  + dT_bat_gain_from_elec + dT_bat_loss_to_room;

			//Refresh electronics temperature
			T_elec = T_elec + dT_elec_own + dT_elec_loss_to_bat
			  + dT_elec_gain_from_bat + dT_elec_loss_to_room;

			return T_bat;

		}
		#endregion

		public void cUiDownloadSuccesSound()
		{
			VARDATA[] Locals = new VARDATA[1];
			GH.Lms.ExecuteByteCode(DownloadSuccesSound, null, Locals);
		}

		public void cUiButtonClr()
		{
			DATA8 Button;
			for (Button = 0; Button < BUTTONS; Button++)
			{
				GH.UiInstance.ButtonState[Button] &= ~BUTTON_CLR;
			}
		}

		public void cUiButtonFlush()
		{
			DATA8 Button;
			for (Button = 0; Button < BUTTONS; Button++)
			{
				GH.UiInstance.ButtonState[Button] &= ~BUTTON_FLUSH;
			}
		}

		public void cUiSetLed(sbyte State)
		{
			DATA8[] Buffer = new sbyte[2];

			GH.UiInstance.LedState = State;
			if (GH.UiInstance.Warnlight != 0)
			{
				if ((State == LED_GREEN_FLASH) || (State == LED_RED_FLASH) || (State == LED_ORANGE_FLASH))
				{
					Buffer[0] = LED_ORANGE_FLASH + '0';
				}
				else
				{
					if ((State == LED_GREEN_PULSE) || (State == LED_RED_PULSE) || (State == LED_ORANGE_PULSE))
					{
						Buffer[0] = LED_ORANGE_PULSE + '0';
					}
					else
					{
						Buffer[0] = LED_ORANGE + '0';
					}
				}
			}
			else
			{
				Buffer[0] = (sbyte)(GH.UiInstance.LedState + '0');
			}
			Buffer[1] = 0;
			GH.Ev3System.LedHandler.SetLed(Buffer);
		}

		public void cUiAlive()
		{
			GH.UiInstance.SleepTimer = 0;
		}

		public RESULT cUiInit()
		{
			RESULT Result = OK;
			DATAF Hw;

			cUiAlive();

			GH.UiInstance.ReadyForWarnings = 0;
			GH.UiInstance.UpdateState = 0;
			GH.UiInstance.RunLedEnabled = 0;
			GH.UiInstance.RunScreenEnabled = 0;
			GH.UiInstance.TopLineEnabled = 0;
			GH.UiInstance.BackButtonBlocked = 0;
			GH.UiInstance.Escape = 0;
			GH.UiInstance.KeyBufIn = 0;
			GH.UiInstance.Keys = 0;
			GH.UiInstance.UiWrBufferSize = 0;

			GH.UiInstance.ScreenBusy = 0;
			GH.UiInstance.ScreenBlocked = 0;
			GH.UiInstance.ScreenPrgId = ushort.MaxValue;
			GH.UiInstance.ScreenObjId = ushort.MaxValue;

			GH.UiInstance.PowerInitialized = 0;
			GH.UiInstance.ShutDown = 0;

			GH.UiInstance.PowerShutdown = 0;
			GH.UiInstance.PowerState = 0;
			GH.UiInstance.VoltShutdown = 0;
			GH.UiInstance.Warnlight = 0;
			GH.UiInstance.Warning = 0;
			GH.UiInstance.WarningShowed = 0;
			GH.UiInstance.WarningConfirmed = 0;
			GH.UiInstance.VoltageState = 0;

			GH.UiInstance.pLcd = GH.UiInstance.LcdSafe;
			GH.UiInstance.pUi = GH.UiInstance.UiSafe;

			GH.UiInstance.Browser.PrgId = 0;
			GH.UiInstance.Browser.ObjId = 0;

			GH.UiInstance.Tbatt = 0.0f;
			GH.UiInstance.Vbatt = 9.0f;
			GH.UiInstance.Ibatt = 0.0f;
			GH.UiInstance.Imotor = 0.0f;
			GH.UiInstance.Iintegrated = 0.0f;

			GH.UiInstance.Ibatt = 0.1f;
			GH.UiInstance.Imotor = 0.0f;

			Result = GH.Terminal.dTerminalInit();

			GH.Lcd.dLcdInit(GH.UiInstance.pLcd.Lcd);

			Hw = 0;
			Hw *= (DATAF)10;
			GH.UiInstance.Hw = (DATA8)Hw;

			cUiButtonClr();

			GH.UiInstance.BattIndicatorHigh = BATT_INDICATOR_HIGH;
			GH.UiInstance.BattIndicatorLow = BATT_INDICATOR_LOW;
			GH.UiInstance.BattWarningHigh = BATT_WARNING_HIGH;
			GH.UiInstance.BattWarningLow = BATT_WARNING_LOW;
			GH.UiInstance.BattShutdownHigh = BATT_SHUTDOWN_HIGH;
			GH.UiInstance.BattShutdownLow = BATT_SHUTDOWN_LOW;

			GH.UiInstance.Accu = 0;

			return (Result);
		}

		public RESULT cUiOpen()
		{
			RESULT Result = RESULT.FAIL;

			// Save screen before run
			Array.Copy(GH.UiInstance.LcdSafe.Lcd, GH.UiInstance.LcdPool[0].Lcd, LCD.LcdSizeof);

			cUiButtonClr();
			cUiSetLed(LED_GREEN_PULSE);
			GH.UiInstance.RunScreenEnabled = 3;
			GH.UiInstance.RunLedEnabled = 1;
			GH.UiInstance.TopLineEnabled = 0;

			Result = OK;

			return (Result);
		}

		public RESULT cUiClose()
		{
			RESULT Result = RESULT.FAIL;

			unchecked { GH.UiInstance.Warning &= (sbyte)~WARNING_BUSY; }
			GH.UiInstance.RunLedEnabled = 0;
			GH.UiInstance.RunScreenEnabled = 0;
			GH.UiInstance.TopLineEnabled = 1;
			GH.UiInstance.BackButtonBlocked = 0;
			GH.UiInstance.Browser.NeedUpdate = 1;
			cUiSetLed(LED_GREEN);

			cUiButtonClr();

			Result = OK;

			return (Result);
		}

		public RESULT cUiExit()
		{
			RESULT Result = RESULT.FAIL;

			Result = GH.Terminal.dTerminalExit();

			Result = OK;

			return (Result);
		}

		public void cUiUpdateButtons(short Time)
		{
			DATA8 Button;

			for (Button = 0; Button < BUTTONS; Button++)
			{

				// Check real hardware buttons

				if (GH.UiInstance.pUi.Pressed[Button] != 0)
				{ // Button pressed

					if (GH.UiInstance.ButtonDebounceTimer[Button] == 0)
					{ // Button activated

						GH.UiInstance.ButtonState[Button] |= BUTTON_ACTIVE;
					}

					GH.UiInstance.ButtonDebounceTimer[Button] = BUTTON_DEBOUNCE_TIME;
				}
				else
				{ // Button not pressed

					if (GH.UiInstance.ButtonDebounceTimer[Button] > 0)
					{ // Debounce delay

						GH.UiInstance.ButtonDebounceTimer[Button] -= Time;

						if (GH.UiInstance.ButtonDebounceTimer[Button] <= 0)
						{ // Button released

							GH.UiInstance.ButtonState[Button] &= ~BUTTON_ACTIVE;
							GH.UiInstance.ButtonDebounceTimer[Button] = 0;
						}
					}
				}

				// Check virtual buttons (hardware, direct command, PC)

				if ((GH.UiInstance.ButtonState[Button] & BUTTON_ACTIVE) != 0)
				{
					if ((GH.UiInstance.ButtonState[Button] & BUTTON_PRESSED) == 0)
					{ // Button activated

						GH.UiInstance.Activated = BUTTON_SET;
						GH.UiInstance.ButtonState[Button] |= BUTTON_PRESSED;
						GH.UiInstance.ButtonState[Button] |= BUTTON_ACTIVATED;
						GH.UiInstance.ButtonTimer[Button] = 0;
						GH.UiInstance.ButtonRepeatTimer[Button] = BUTTON_START_REPEAT_TIME;
					}

					// Control auto repeat

					if (GH.UiInstance.ButtonRepeatTimer[Button] > Time)
					{
						GH.UiInstance.ButtonRepeatTimer[Button] -= Time;
					}
					else
					{
						if ((Button != 1) && (Button != 5))
						{ // No repeat on ENTER and BACK

							GH.UiInstance.Activated |= BUTTON_SET;
							GH.UiInstance.ButtonState[Button] |= BUTTON_ACTIVATED;
							GH.UiInstance.ButtonRepeatTimer[Button] = BUTTON_REPEAT_TIME;
						}
					}

					// Control long press

					GH.UiInstance.ButtonTimer[Button] += Time;

					if (GH.UiInstance.ButtonTimer[Button] >= LONG_PRESS_TIME)
					{
						if ((GH.UiInstance.ButtonState[Button] & BUTTON_LONG_LATCH) == 0)
						{ // Only once

							GH.UiInstance.ButtonState[Button] |= BUTTON_LONG_LATCH;

							if (Button == 2)
							{
								GH.UiInstance.Activated |= BUTTON_BUFPRINT;
							}
						}
						GH.UiInstance.ButtonState[Button] |= BUTTON_LONGPRESS;
					}

				}
				else
				{
					if ((GH.UiInstance.ButtonState[Button] & BUTTON_PRESSED) != 0)
					{ // Button released

						GH.UiInstance.ButtonState[Button] &= ~BUTTON_PRESSED;
						GH.UiInstance.ButtonState[Button] &= ~BUTTON_LONG_LATCH;
						GH.UiInstance.ButtonState[Button] |= BUTTON_BUMBED;
					}
				}
			}
		}

		public RESULT cUiUpdateInput()
		{
			UBYTE Key = 0;

			if (GH.Lms.GetTerminalEnable() != 0)
			{
				if (GH.Terminal.dTerminalRead(ref Key) == OK)
				{
					switch (Key)
					{
						case (byte)' ':
							{
								GH.UiInstance.Escape = (sbyte)Key;
							}
							break;

						case (byte)'<':
							{
								GH.UiInstance.Escape = (sbyte)Key;
							}
							break;

						case (byte)'\r':
						case (byte)'\n':
							{
								if (GH.UiInstance.KeyBufIn != 0)
								{
									GH.UiInstance.Keys = GH.UiInstance.KeyBufIn;
									GH.UiInstance.KeyBufIn = 0;
								}
							}
							break;

						default:
							{
								GH.UiInstance.KeyBuffer[GH.UiInstance.KeyBufIn] = (sbyte)Key;
								if (++GH.UiInstance.KeyBufIn >= KEYBUF_SIZE)
								{
									GH.UiInstance.KeyBufIn--;
								}
								GH.UiInstance.KeyBuffer[GH.UiInstance.KeyBufIn] = 0;
							}
							break;

					}
				}
			}
			GH.Lcd.dLcdRead();

			return (OK);
		}

		public sbyte cUiEscape()
		{
			DATA8 Result;

			Result = GH.UiInstance.Escape;
			GH.UiInstance.Escape = 0;

			return (Result);
		}


		public void cUiTestpin(sbyte State)
		{
			DATA8 Data8;

			Data8 = State;
			// TODO: wtf
			//if (GH.UiInstance.PowerFile >= MIN_HANDLE)
			//{
			//	write(GH.UiInstance.PowerFile, &Data8, 1);
			//}
			GH.Ev3System.Logger.LogWarning($"Tried to use unimplemented shite in: {System.Environment.StackTrace}");
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
					Tmp = (byte)((UBYTE)(Char - 'a') + 10);
				}
			}

			return (Tmp);
		}

		void cUiFlushBuffer()
		{
			if (GH.UiInstance.UiWrBufferSize != 0)
			{
				if (GH.Lms.GetTerminalEnable() != 0)
				{
					GH.Terminal.dTerminalWrite(GH.UiInstance.UiWrBuffer, GH.UiInstance.UiWrBufferSize);
				}
				GH.UiInstance.UiWrBufferSize = 0;
			}
		}

		void cUiWriteString(string pString)
		{
			cUiWriteString(pString.ToCharArray().Select(x => (DATA8)x).ToArray());
		}

		void cUiWriteString(DATA8[] pString)
		{
			foreach (var ch in pString)
			{
				GH.UiInstance.UiWrBuffer[GH.UiInstance.UiWrBufferSize] = ch;
				if (++GH.UiInstance.UiWrBufferSize >= UI_WR_BUFFER_SIZE)
				{
					cUiFlushBuffer();
				}
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
					GH.UiInstance.ButtonState[Button] |= BUTTON_ACTIVE;
				}
				else
				{
					GH.UiInstance.ButtonState[Button] &= ~BUTTON_ACTIVE;
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
							GH.UiInstance.ButtonState[Button] |= BUTTON_ACTIVE;
						}
					}
					else
					{
						for (Button = 0; Button < BUTTONS; Button++)
						{
							GH.UiInstance.ButtonState[Button] &= ~BUTTON_ACTIVE;
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
				if ((GH.UiInstance.ButtonState[Button] & BUTTON_PRESSED) != 0)
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
						if ((GH.UiInstance.ButtonState[Button] & BUTTON_PRESSED) != 0)
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
				if ((GH.UiInstance.ButtonState[Button] & BUTTON_ACTIVATED) != 0)
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
						if ((GH.UiInstance.ButtonState[Button] & BUTTON_ACTIVATED) != 0)
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
							if ((GH.UiInstance.ButtonState[Button] & BUTTON_ACTIVATED) != 0)
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
				if ((GH.UiInstance.ButtonState[Button] & BUTTON_ACTIVATED) != 0)
				{
					GH.UiInstance.ButtonState[Button] &= ~BUTTON_ACTIVATED;
					Result = 1;
				}
			}
			else
			{
				if (Button == REAL_ANY_BUTTON)
				{
					for (Button = 0; Button < BUTTONS; Button++)
					{
						if ((GH.UiInstance.ButtonState[Button] & BUTTON_ACTIVATED) != 0)
						{
							GH.UiInstance.ButtonState[Button] &= ~BUTTON_ACTIVATED;
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
							if ((GH.UiInstance.ButtonState[Button] & BUTTON_ACTIVATED) != 0)
							{
								GH.UiInstance.ButtonState[Button] &= ~BUTTON_ACTIVATED;
								Result = 0;
							}
						}
					}
				}
			}
			if (Result != 0)
			{
				GH.UiInstance.Click = 1;
			}

			return (Result);
		}


		DATA8 cUiGetBumbed(DATA8 Button)
		{
			DATA8 Result = 0;

			Button = cUiButtonRemap(Button);

			if (Button < BUTTONS)
			{
				if ((GH.UiInstance.ButtonState[Button] & BUTTON_BUMBED) != 0)
				{
					GH.UiInstance.ButtonState[Button] &= ~BUTTON_BUMBED;
					Result = 1;
				}
			}
			else
			{
				if (Button == REAL_ANY_BUTTON)
				{
					for (Button = 0; Button < BUTTONS; Button++)
					{
						if ((GH.UiInstance.ButtonState[Button] & BUTTON_BUMBED) != 0)
						{
							GH.UiInstance.ButtonState[Button] &= ~BUTTON_BUMBED;
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
							if ((GH.UiInstance.ButtonState[Button] & BUTTON_BUMBED) != 0)
							{
								GH.UiInstance.ButtonState[Button] &= ~BUTTON_BUMBED;
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
				if ((GH.UiInstance.ButtonState[Button] & BUTTON_LONGPRESS) != 0)
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
						if ((GH.UiInstance.ButtonState[Button] & BUTTON_LONGPRESS) != 0)
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
							if ((GH.UiInstance.ButtonState[Button] & BUTTON_LONGPRESS) != 0)
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
				if ((GH.UiInstance.ButtonState[Button] & BUTTON_LONGPRESS) != 0)
				{
					GH.UiInstance.ButtonState[Button] &= ~BUTTON_LONGPRESS;
					Result = 1;
				}
			}
			else
			{
				if (Button == REAL_ANY_BUTTON)
				{
					for (Button = 0; Button < BUTTONS; Button++)
					{
						if ((GH.UiInstance.ButtonState[Button] & BUTTON_LONGPRESS) != 0)
						{
							GH.UiInstance.ButtonState[Button] &= ~BUTTON_LONGPRESS;
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
							if ((GH.UiInstance.ButtonState[Button] & BUTTON_LONGPRESS) != 0)
							{
								GH.UiInstance.ButtonState[Button] &= ~BUTTON_LONGPRESS;
								Result = 0;
							}
						}
					}
				}
			}
			if (Result != 0)
			{
				GH.UiInstance.Click = 1;
			}

			return (Result);
		}

		DATA16 cUiTestHorz()
		{
			DATA16 Result = 0;

			if (cUiTestShortPress(LEFT_BUTTON) != 0)
			{
				Result = -1;
			}
			if (cUiTestShortPress(RIGHT_BUTTON) != 0)
			{
				Result = 1;
			}

			return (Result);
		}


		DATA16 cUiGetHorz()
		{
			DATA16 Result = 0;

			if (cUiGetShortPress(LEFT_BUTTON) != 0)
			{
				Result = -1;
			}
			if (cUiGetShortPress(RIGHT_BUTTON) != 0)
			{
				Result = 1;
			}

			return (Result);
		}


		DATA16 cUiGetVert()
		{
			DATA16 Result = 0;

			if (cUiGetShortPress(UP_BUTTON) != 0)
			{
				Result = -1;
			}
			if (cUiGetShortPress(DOWN_BUTTON) != 0)
			{
				Result = 1;
			}

			return (Result);
		}


		DATA8 cUiWaitForPress()
		{
			DATA8 Result = 0;

			Result = cUiTestShortPress(ANY_BUTTON);

			return (Result);
		}


		DATA16 cUiAlignX(DATA16 X)
		{
			return ((short)((X + 7) & ~7));
		}

		void cUiUpdateCnt()
		{
			if (GH.UiInstance.PowerInitialized != 0) 
			{
				GH.UiInstance.CinCnt *= (DATAF)(AVR_CIN - 1);
				GH.UiInstance.CoutCnt *= (DATAF)(AVR_COUT - 1);
				GH.UiInstance.VinCnt *= (DATAF)(AVR_VIN - 1);

				GH.UiInstance.CinCnt += (DATAF)GH.UiInstance.Analog.BatteryCurrent;
				GH.UiInstance.CoutCnt += (DATAF)GH.UiInstance.Analog.MotorCurrent;
				GH.UiInstance.VinCnt += (DATAF)GH.UiInstance.Analog.Cell123456;

				GH.UiInstance.CinCnt /= (DATAF)(AVR_CIN);
				GH.UiInstance.CoutCnt /= (DATAF)(AVR_COUT);
				GH.UiInstance.VinCnt /= (DATAF)(AVR_VIN);
			}
			else
			{
				GH.UiInstance.CinCnt = (DATAF)GH.UiInstance.Analog.BatteryCurrent;
				GH.UiInstance.CoutCnt = (DATAF)GH.UiInstance.Analog.MotorCurrent;
				GH.UiInstance.VinCnt = (DATAF)GH.UiInstance.Analog.Cell123456;
				GH.UiInstance.PowerInitialized = 1;
			}
		}


		void cUiUpdatePower()
		{
			DATAF CinV;
			DATAF CoutV;

			if ((GH.UiInstance.Hw == FINAL) || (GH.UiInstance.Hw == FINALB))
			{
				CinV = CNT_V(GH.UiInstance.CinCnt) / AMP_CIN;
				GH.UiInstance.Vbatt = (CNT_V(GH.UiInstance.VinCnt) / AMP_VIN) + CinV + VCE;

				GH.UiInstance.Ibatt = CinV / SHUNT_IN;
				CoutV = CNT_V(GH.UiInstance.CoutCnt) / AMP_COUT;
				GH.UiInstance.Imotor = CoutV / SHUNT_OUT;

			}
			else
			{
				CinV = CNT_V(GH.UiInstance.CinCnt) / EP2_AMP_CIN;
				GH.UiInstance.Vbatt = (CNT_V(GH.UiInstance.VinCnt) / AMP_VIN) + CinV + VCE;

				GH.UiInstance.Ibatt = CinV / EP2_SHUNT_IN;
				GH.UiInstance.Imotor = 0;

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

			if (GH.UiInstance.TopLineEnabled != 0)
			{
				// Clear top line
				LCDClearTopline(GH.UiInstance.pLcd.Lcd);

				// Show BT status
				TmpStatus = 0;
				BtStatus = (sbyte)GH.Com.cComGetBtStatus();
				if (BtStatus > 0)
				{
					TmpStatus = 1;
					BtStatus >>= 1;
					if ((BtStatus >= 0) && (BtStatus < TOP_BT_ICONS))
					{
						GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, FG_COLOR, 0, 1, SMALL_ICON, (sbyte)TopLineBtIconMap[BtStatus]);
					}
				}
				if (GH.UiInstance.BtOn != TmpStatus)
				{
					GH.UiInstance.BtOn = TmpStatus;
					GH.UiInstance.UiUpdate = 1;
				}

				// Show WIFI status
				TmpStatus = 0;
				WifiStatus = (sbyte)GH.Com.cComGetWifiStatus();
				if (WifiStatus > 0)
				{
					TmpStatus = 1;
					WifiStatus >>= 1;
					if ((WifiStatus >= 0) && (WifiStatus < TOP_WIFI_ICONS))
					{
						GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, FG_COLOR, 16, 1, SMALL_ICON, (sbyte)TopLineWifiIconMap[WifiStatus]);
					}
				}
				if (GH.UiInstance.WiFiOn != TmpStatus)
				{
					GH.UiInstance.WiFiOn = TmpStatus;
					GH.UiInstance.UiUpdate = 1;
				}

				// Show brick name
				GH.Com.cComGetBrickName(NAME_LENGTH + 1, Name);

				X1 = GH.Lcd.dLcdGetFontWidth(SMALL_FONT);
				X2 = (short)(LCD_WIDTH / X1);
				X2 -= (short)(Name.Length);
				X2 /= 2;
				X2 *= X1;
				GH.Lcd.dLcdDrawText(GH.UiInstance.pLcd.Lcd, FG_COLOR, X2, 1, SMALL_FONT, Name);

				// Calculate number of icons
				X1 = GH.Lcd.dLcdGetIconWidth(SMALL_ICON);
				X2 = (short)((LCD_WIDTH - X1) / X1);

				// Show battery
				V = (DATA16)(GH.UiInstance.Vbatt * 1000.0);
				V -= GH.UiInstance.BattIndicatorLow;
				V = (short)((V * (TOP_BATT_ICONS - 1)) / (GH.UiInstance.BattIndicatorHigh - GH.UiInstance.BattIndicatorLow));
				if (V > (TOP_BATT_ICONS - 1))
				{
					V = (TOP_BATT_ICONS - 1);
				}
				if (V < 0)
				{
					V = 0;
				}
				GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, FG_COLOR, (short)(X2 * X1), 1, SMALL_ICON, (sbyte)TopLineBattIconMap[V]);

				if (GH.UiInstance.Accu == 0)
				{
					GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, FG_COLOR, 176, 4);
					GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, FG_COLOR, 176, 5);
				}

				// Show bottom line
				GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, FG_COLOR, 0, TOPLINE_HEIGHT, LCD_WIDTH, TOPLINE_HEIGHT);
			}
		}


		void cUiUpdateLcd()
		{
			GH.UiInstance.Font = NORMAL_FONT;
			cUiUpdateTopline();
			GH.Lcd.dLcdUpdate(GH.UiInstance.pLcd);
		}

		void cUiRunScreen()
		{ // 100mS

			if (GH.UiInstance.ScreenBlocked == 0)
			{
				switch (GH.UiInstance.RunScreenEnabled)
				{
					case 0:
						{
						}
						break;

					case 1:
						{ // 100mS

							GH.UiInstance.RunScreenEnabled++;
						}
						break;

					case 2:
						{ // 200mS

							GH.UiInstance.RunScreenEnabled++;
						}
						break;

					case 3:
						{
							// Init animation number
							GH.UiInstance.RunScreenNumber = 1;

							// Clear screen
							LCDClear(GH.UiInstance.pLcd.Lcd);
							cUiUpdateLcd();


							// Enable top line

							// Draw fixed image
							var mind = BmpHelper.Get(BmpType.Mindstorms);
							GH.Lcd.dLcdDrawPicture(GH.UiInstance.pLcd.Lcd, FG_COLOR, 8, 39, mind.Width, mind.Height, mind.Data);
							cUiUpdateLcd();

							// Draw user program name
							GH.Lcd.dLcdDrawText(GH.UiInstance.pLcd.Lcd, FG_COLOR, 8, 79, 1, GH.VMInstance.Program[USER_SLOT].Name);
							cUiUpdateLcd();

							GH.UiInstance.RunScreenTimer = 0;
							GH.UiInstance.RunScreenCounter = 0;

							GH.UiInstance.RunScreenEnabled++;
							// 0mS -> Ani1

							if (GH.UiInstance.RunLedEnabled != 0)
							{
								cUiSetLed(LED_GREEN_PULSE);
							}

							var ani1 = BmpHelper.Get(BmpType.Ani1x);
							GH.Lcd.dLcdDrawPicture(GH.UiInstance.pLcd.Lcd, FG_COLOR, 8, 67, ani1.Width, ani1.Height, ani1.Data);
							cUiUpdateLcd();

							GH.UiInstance.RunScreenTimer = GH.UiInstance.MilliSeconds;
							GH.UiInstance.RunScreenEnabled++;
						}
						break;

					case 5:
						{ // 100mS

							GH.UiInstance.RunScreenEnabled++;
						}
						break;

					case 6:
						{ // 200mS

							GH.UiInstance.RunScreenEnabled++;
						}
						break;

					case 7:
						{ // 300mS

							GH.UiInstance.RunScreenEnabled++;
						}
						break;

					case 8:
						{ // 400mS

							GH.UiInstance.RunScreenEnabled++;
						}
						break;

					case 9:
						{ // 500mS -> Ani2

							var ani2 = BmpHelper.Get(BmpType.Ani2x);
							GH.Lcd.dLcdDrawPicture(GH.UiInstance.pLcd.Lcd, FG_COLOR, 8, 67, ani2.Width, ani2.Height, ani2.Data);
							cUiUpdateLcd();

							GH.UiInstance.RunScreenEnabled++;
						}
						break;

					case 10:
						{ // 600mS -> Ani3

							var ani3 = BmpHelper.Get(BmpType.Ani3x);
							GH.Lcd.dLcdDrawPicture(GH.UiInstance.pLcd.Lcd, FG_COLOR, 8, 67, ani3.Width, ani3.Height, ani3.Data);
							cUiUpdateLcd();

							GH.UiInstance.RunScreenEnabled++;
						}
						break;

					case 11:
						{ // 700ms -> Ani4

							var ani4 = BmpHelper.Get(BmpType.Ani4x);
							GH.Lcd.dLcdDrawPicture(GH.UiInstance.pLcd.Lcd, FG_COLOR, 8, 67, ani4.Width, ani4.Height, ani4.Data);
							cUiUpdateLcd();

							GH.UiInstance.RunScreenEnabled++;
						}
						break;

					case 12:
						{ // 800ms -> Ani5

							var ani5 = BmpHelper.Get(BmpType.Ani5x);
							GH.Lcd.dLcdDrawPicture(GH.UiInstance.pLcd.Lcd, FG_COLOR, 8, 67, ani5.Width, ani5.Height, ani5.Data);
							cUiUpdateLcd();

							GH.UiInstance.RunScreenEnabled++;
						}
						break;

					default:
						{ // 900ms -> Ani6

							var ani6 = BmpHelper.Get(BmpType.Ani6x);
							GH.Lcd.dLcdDrawPicture(GH.UiInstance.pLcd.Lcd, FG_COLOR, 8, 67, ani6.Width, ani6.Height, ani6.Data);
							cUiUpdateLcd();

							GH.UiInstance.RunScreenEnabled = 4;
						}
						break;

				}
			}
		}

		void cUiCheckTemp()
		{
			if ((GH.UiInstance.MilliSeconds - GH.UiInstance.TempTimer) >= CALL_INTERVAL)
			{
				GH.UiInstance.TempTimer += CALL_INTERVAL;
				GH.UiInstance.Tbatt = new_bat_temp(GH.UiInstance.Vbatt, (GH.UiInstance.Ibatt * (DATAF)1.1));
			}

			if (GH.UiInstance.Tbatt >= TEMP_SHUTDOWN_WARNING)
			{
				GH.UiInstance.Warning |= WARNING_TEMP;
			}
			else
			{
				GH.UiInstance.Warning &= ~WARNING_TEMP;
			}


			if (GH.UiInstance.Tbatt >= TEMP_SHUTDOWN_FAIL)
			{
				GH.Lms.ProgramEnd(USER_SLOT);
				GH.UiInstance.ShutDown = 1;
			}
		}

		void cUiCheckMemory()
		{ // 400mS

			DATA32 Total = 0;
			DATA32 Free = 0;

			GH.Memory.cMemoryGetUsage(ref Total, ref Free, 0);

			if (Free > LOW_MEMORY)
			{ // Good

				GH.UiInstance.Warning &= ~WARNING_MEMORY;
			}
			else
			{ // Bad

				GH.UiInstance.Warning |= WARNING_MEMORY;
			}
		}

		void cUiCheckAlive(UWORD Time)
		{
			ULONG Timeout;

			GH.UiInstance.SleepTimer += Time;

			if ((GH.UiInstance.Activated & BUTTON_ALIVE) != 0)
			{
				GH.UiInstance.Activated &= ~BUTTON_ALIVE;
				cUiAlive();
			}
			Timeout = (ULONG)GH.Lms.GetSleepMinutes();

			if (Timeout != 0)
			{
				if (GH.UiInstance.SleepTimer >= (Timeout * 60000L))
				{
					GH.UiInstance.ShutDown = 1;
				}
			}
		}

		public void cUiUpdate(ushort Time)
		{
			DATA8 Warning;
			DATA8 Tmp;

			GH.UiInstance.MilliSeconds += (ULONG)Time;

			cUiUpdateButtons((short)Time);
			cUiUpdateInput();
			cUiUpdateCnt();

			if ((GH.UiInstance.MilliSeconds - GH.UiInstance.UpdateStateTimer) >= 50)
			{
				GH.UiInstance.UpdateStateTimer = GH.UiInstance.MilliSeconds;

				if (GH.UiInstance.Event == 0)
				{
					GH.UiInstance.Event = GH.Com.cComGetEvent();
				}

				switch (GH.UiInstance.UpdateState++)
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

							cUiCheckAlive(400);
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

							if (GH.UiInstance.ScreenBusy == 0)
							{
								cUiUpdateTopline();
								GH.Lcd.dLcdUpdate(GH.UiInstance.pLcd);
							}
						}
						break;

					default:
						{ // 400 mS

							cUiRunScreen();
							GH.UiInstance.UpdateState = 0;
							GH.UiInstance.ReadyForWarnings = 1;
						}
						break;

				}

				if (GH.UiInstance.Warning != 0)
				{ // Some warning present

					if (GH.UiInstance.Warnlight == 0)
					{ // If not on - turn orange light on

						GH.UiInstance.Warnlight = 1;
						cUiSetLed(GH.UiInstance.LedState);
					}
				}
				else
				{ // No warning

					if (GH.UiInstance.Warnlight != 0)
					{ // If orange light on - turn it off

						GH.UiInstance.Warnlight = 0;
						cUiSetLed(GH.UiInstance.LedState);
					}
				}

				// Get valid popup warnings
				Warning = (sbyte)(GH.UiInstance.Warning & WARNINGS);

				// Find warnings that has not been showed
				Tmp = (sbyte)(Warning & ~GH.UiInstance.WarningShowed);

				if (Tmp != 0)
				{ // Show popup

					if (GH.UiInstance.ScreenBusy == 0)
					{ // Wait on screen

						if (GH.UiInstance.ScreenBlocked == 0)
						{
							Array.Copy(GH.UiInstance.LcdSafe.Lcd, GH.UiInstance.LcdSave.Lcd, LCD.LcdSizeof);
							var pop3 = BmpHelper.Get(BmpType.POP3);
							GH.Lcd.dLcdDrawPicture(GH.UiInstance.pLcd.Lcd, FG_COLOR, vmPOP3_ABS_X, vmPOP3_ABS_Y, pop3.Width, pop3.Height, pop3.Data);

							if ((Tmp & WARNING_TEMP) != 0)
							{
								GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, FG_COLOR, vmPOP3_ABS_WARN_ICON_X1, vmPOP3_ABS_WARN_ICON_Y, LARGE_ICON, WARNSIGN);
								GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, FG_COLOR, vmPOP3_ABS_WARN_ICON_X2, vmPOP3_ABS_WARN_SPEC_ICON_Y, LARGE_ICON, WARN_POWER);
								GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, FG_COLOR, vmPOP3_ABS_WARN_ICON_X3, vmPOP3_ABS_WARN_SPEC_ICON_Y, LARGE_ICON, TO_MANUAL);
								GH.UiInstance.WarningShowed |= WARNING_TEMP;
							}
							else
							{
								if ((Tmp & WARNING_CURRENT) != 0)
								{
									GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, FG_COLOR, vmPOP3_ABS_WARN_ICON_X1, vmPOP3_ABS_WARN_ICON_Y, LARGE_ICON, WARNSIGN);
									GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, FG_COLOR, vmPOP3_ABS_WARN_ICON_X2, vmPOP3_ABS_WARN_SPEC_ICON_Y, LARGE_ICON, WARN_POWER);
									GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, FG_COLOR, vmPOP3_ABS_WARN_ICON_X3, vmPOP3_ABS_WARN_SPEC_ICON_Y, LARGE_ICON, TO_MANUAL);
									GH.UiInstance.WarningShowed |= WARNING_CURRENT;
								}
								else
								{
									if ((Tmp & WARNING_VOLTAGE) != 0)
									{
										GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, FG_COLOR, vmPOP3_ABS_WARN_ICON_X, vmPOP3_ABS_WARN_ICON_Y, LARGE_ICON, WARNSIGN);
										GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, FG_COLOR, vmPOP3_ABS_WARN_SPEC_ICON_X, vmPOP3_ABS_WARN_SPEC_ICON_Y, LARGE_ICON, WARN_BATT);
										GH.UiInstance.WarningShowed |= WARNING_VOLTAGE;
									}
									else
									{
										if ((Tmp & WARNING_MEMORY) != 0)
										{
											GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, FG_COLOR, vmPOP3_ABS_WARN_ICON_X, vmPOP3_ABS_WARN_ICON_Y, LARGE_ICON, WARNSIGN);
											GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, FG_COLOR, vmPOP3_ABS_WARN_SPEC_ICON_X, vmPOP3_ABS_WARN_SPEC_ICON_Y, LARGE_ICON, WARN_MEMORY);
											GH.UiInstance.WarningShowed |= WARNING_MEMORY;
										}
										else
										{
											if ((Tmp & WARNING_DSPSTAT) != 0)
											{
												GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, FG_COLOR, vmPOP3_ABS_WARN_ICON_X, vmPOP3_ABS_WARN_ICON_Y, LARGE_ICON, WARNSIGN);
												GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, FG_COLOR, vmPOP3_ABS_WARN_SPEC_ICON_X, vmPOP3_ABS_WARN_SPEC_ICON_Y, LARGE_ICON, PROGRAM_ERROR);
												GH.UiInstance.WarningShowed |= WARNING_DSPSTAT;
											}
											else
											{
											}
										}
									}
								}
							}
							GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, FG_COLOR, vmPOP3_ABS_WARN_LINE_X, vmPOP3_ABS_WARN_LINE_Y, vmPOP3_ABS_WARN_LINE_ENDX, vmPOP3_ABS_WARN_LINE_Y);
							GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, FG_COLOR, vmPOP3_ABS_WARN_YES_X, vmPOP3_ABS_WARN_YES_Y, LARGE_ICON, YES_SEL);
							GH.Lcd.dLcdUpdate(GH.UiInstance.pLcd);
							cUiButtonFlush();
							GH.UiInstance.ScreenBlocked = 1;
						}
					}
				}

				// Find warnings that have been showed but not confirmed
				Tmp = GH.UiInstance.WarningShowed;
				Tmp &= (sbyte)(~GH.UiInstance.WarningConfirmed);

				if (Tmp != 0)
				{
					if (cUiGetShortPress(ENTER_BUTTON) != 0)
					{
						GH.UiInstance.ScreenBlocked = 0;
						Array.Copy(GH.UiInstance.LcdSave.Lcd, GH.UiInstance.LcdSafe.Lcd, LCD.LcdSizeof);
						GH.Lcd.dLcdUpdate(GH.UiInstance.pLcd);
						if ((Tmp & WARNING_TEMP) != 0)
						{
							GH.UiInstance.WarningConfirmed |= WARNING_TEMP;
						}
						else
						{
							if ((Tmp & WARNING_CURRENT) != 0)
							{
								GH.UiInstance.WarningConfirmed |= WARNING_CURRENT;
							}
							else
							{
								if ((Tmp & WARNING_VOLTAGE) != 0)
								{
									GH.UiInstance.WarningConfirmed |= WARNING_VOLTAGE;
								}
								else
								{
									if ((Tmp & WARNING_MEMORY) != 0)
									{
										GH.UiInstance.WarningConfirmed |= WARNING_MEMORY;
									}
									else
									{
										if ((Tmp & WARNING_DSPSTAT) != 0)
										{
											GH.UiInstance.WarningConfirmed |= WARNING_DSPSTAT;
											GH.UiInstance.Warning &= ~WARNING_DSPSTAT;
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
				Tmp &= WARNINGS;
				Tmp &= GH.UiInstance.WarningShowed;
				Tmp &= GH.UiInstance.WarningConfirmed;

				GH.UiInstance.WarningShowed &= (sbyte)(~Tmp);
				GH.UiInstance.WarningConfirmed &= (sbyte)(~Tmp);
			}
		}

		DATA8 cUiNotification(DATA8 Color, DATA16 X, DATA16 Y, DATA8 Icon1, DATA8 Icon2, DATA8 Icon3, DATA8[] pText, ref DATA8 pState)
		{
			RESULT Result = RESULT.BUSY;
			NOTIFY pQ;
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

			pQ = GH.UiInstance.Notify;

			var pop3 = BmpHelper.Get(BmpType.POP3);

			if (pState == 0)
			{
				pState = 1;
				pQ.ScreenStartX = X;
				pQ.ScreenStartY = Y;
				pQ.ScreenWidth = pop3.Width;
				pQ.ScreenHeight = pop3.Height;
				pQ.IconStartY = (short)(pQ.ScreenStartY + 10);
				pQ.IconWidth = GH.Lcd.dLcdGetIconWidth(LARGE_ICON);
				pQ.IconHeight = GH.Lcd.dLcdGetIconHeight(LARGE_ICON);
				pQ.IconSpaceX = pQ.IconWidth;

				pQ.YesNoStartX = (short)(pQ.ScreenStartX + (pQ.ScreenWidth / 2));
				pQ.YesNoStartX -= (short)((pQ.IconWidth + 8) / 2);
				pQ.YesNoStartX = cUiAlignX(pQ.YesNoStartX);
				pQ.YesNoStartY = (short)(pQ.ScreenStartY + 40);

				pQ.LineStartX = (short)(pQ.ScreenStartX + 5);
				pQ.LineStartY = (short)(pQ.ScreenStartY + 39);
				pQ.LineEndX = (short)(pQ.LineStartX + 134);

				// Find no of icons
				pQ.NoOfIcons = 0;
				if (Icon1 > ICON_NONE)
				{
					pQ.NoOfIcons++;
				}
				if (Icon2 > ICON_NONE)
				{
					pQ.NoOfIcons++;
				}
				if (Icon3 > ICON_NONE)
				{
					pQ.NoOfIcons++;
				}

				// Find no of text lines
				pQ.TextLines = 0;
				if (pText.Length > 0)
				{
					pQ.IconStartX = (short)(pQ.ScreenStartX + 8);
					pQ.IconStartX = cUiAlignX(pQ.IconStartX);

					AvailableX = pQ.ScreenWidth;
					AvailableX -= (short)(((pQ.IconStartX - pQ.ScreenStartX)) * 2);

					AvailableX -= (short)(pQ.NoOfIcons * pQ.IconSpaceX);


					pQ.NoOfChars = (short)pText.Length;


					pQ.Font = SMALL_FONT;
					pQ.FontWidth = GH.Lcd.dLcdGetFontWidth(pQ.Font);
					UsedX = (short)(pQ.FontWidth * pQ.NoOfChars);

					Line = 0;

					if (UsedX <= AvailableX)
					{ // One line - small font

						if ((AvailableX - UsedX) >= 32)
						{
							pQ.IconStartX += 32;
						}

						CommonHelper.Snprintf<sbyte>(pQ.TextLine[Line], 0, MAX_NOTIFY_LINE_CHARS, pText);
						Line++;
						pQ.TextLines++;

						pQ.TextStartX = (short)(pQ.IconStartX + (pQ.NoOfIcons * pQ.IconSpaceX));
						pQ.TextStartY = (short)(pQ.ScreenStartY + 18);
						pQ.TextSpaceY = (short)(GH.Lcd.dLcdGetFontHeight(pQ.Font) + 1);
					}
					else
					{ // one or more lines - tiny font

						pQ.Font = TINY_FONT;
						pQ.FontWidth = GH.Lcd.dLcdGetFontWidth(pQ.Font);
						UsedX = (short)(pQ.FontWidth * pQ.NoOfChars);
						AvailableX -= pQ.FontWidth;

						CharIn = 0;

						while ((pText.Length > CharIn) && (Line < MAX_NOTIFY_LINES))
						{
							CharOut = 0;
							UsedX = 0;
							while ((pText.Length > CharIn) && (CharOut < MAX_NOTIFY_LINE_CHARS) && (UsedX < (AvailableX - pQ.FontWidth)))
							{
								Character = pText[CharIn];
								if (Character == '_')
								{
									Character = (sbyte)' ';
								}
							  pQ.TextLine[Line][CharOut] = Character;
								CharIn++;
								CharOut++;
								UsedX += pQ.FontWidth;
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
						  pQ.TextLine[Line][CharOut] = 0;
							Line++;
						}

					  pQ.TextLines = Line;

						pQ.TextStartX = (short)(pQ.IconStartX + (pQ.NoOfIcons * pQ.IconSpaceX) + pQ.FontWidth);
						pQ.TextSpaceY = (short)(GH.Lcd.dLcdGetFontHeight(pQ.Font) + 1);


						AvailableY = (short)(pQ.LineStartY - (pQ.ScreenStartY + 5));
						UsedY = (short)(pQ.TextLines * pQ.TextSpaceY);

						while (UsedY > AvailableY)
						{
							pQ.TextLines--;
							UsedY = (short)(pQ.TextLines * pQ.TextSpaceY);
						}
						Y2 = (short)((AvailableY - UsedY) / 2);

						pQ.TextStartY = (short)(pQ.ScreenStartY + Y2 + 5);
					}

				}
				else
				{
					pQ.IconStartX = (short)(pQ.ScreenStartX + (pQ.ScreenWidth / 2));
					pQ.IconStartX -= (short)((pQ.IconWidth + 8) / 2);
					pQ.IconStartX -= (short)((pQ.NoOfIcons / 2) * pQ.IconWidth);
					pQ.IconStartX = cUiAlignX(pQ.IconStartX);
					pQ.TextStartY = (short)(pQ.ScreenStartY + 8);
				}

			  pQ.NeedUpdate = 1;
			}


			if (pQ.NeedUpdate != 0)
			{
				//* UPDATE ***************************************************************************************************
				pQ.NeedUpdate = 0;

				GH.Lcd.dLcdDrawPicture(GH.UiInstance.pLcd.Lcd, Color, pQ.ScreenStartX, pQ.ScreenStartY, pop3.Width, pop3.Height, pop3.Data);

				X2 = pQ.IconStartX;

				if (Icon1 > ICON_NONE)
				{
					GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, Color, X2, pQ.IconStartY, LARGE_ICON, Icon1);
					X2 += pQ.IconSpaceX;
				}
				if (Icon2 > ICON_NONE)
				{
					GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, Color, X2, pQ.IconStartY, LARGE_ICON, Icon2);
					X2 += pQ.IconSpaceX;
				}
				if (Icon3 > ICON_NONE)
				{
					GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, Color, X2, pQ.IconStartY, LARGE_ICON, Icon3);
					X2 += pQ.IconSpaceX;
				}

				Line = 0;
				Y2 = pQ.TextStartY;
				while (Line < pQ.TextLines)
				{
					GH.Lcd.dLcdDrawText(GH.UiInstance.pLcd.Lcd, Color, pQ.TextStartX, Y2, pQ.Font, pQ.TextLine[Line]);
					Y2 += pQ.TextSpaceY;
					Line++;
				}

				GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, Color, pQ.LineStartX, pQ.LineStartY, pQ.LineEndX, pQ.LineStartY);

				GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, Color, pQ.YesNoStartX, pQ.YesNoStartY, LARGE_ICON, YES_SEL);

				cUiUpdateLcd();
				GH.UiInstance.ScreenBusy = 0;
			}

			if (cUiGetShortPress(ENTER_BUTTON) != 0)
			{
				cUiButtonFlush();
				Result = OK;
				pState = 0;
			}

			return ((sbyte)Result);
		}


		DATA8 cUiQuestion(DATA8 Color, DATA16 X, DATA16 Y, DATA8 Icon1, DATA8 Icon2, DATA8[] pText, ref DATA8 pState, ref DATA8 pAnswer)
		{
			RESULT Result = RESULT.BUSY;
			TQUESTION pQ;
			DATA16 Inc;

			pQ = GH.UiInstance.Question;

			var pop3 = BmpHelper.Get(BmpType.POP3);

			Inc = cUiGetHorz();
			if (Inc != 0)
			{
				pQ.NeedUpdate = 1;

				pAnswer += (sbyte)Inc;

				if (pAnswer > 1)
				{
					pAnswer = 1;
					pQ.NeedUpdate = 0;
				}
				if (pAnswer < 0)
				{
					pAnswer = 0;
					pQ.NeedUpdate = 0;
				}
			}

			if (pState == 0)
			{
				pState = 1;
				pQ.ScreenStartX = X;
				pQ.ScreenStartY = Y;
				pQ.IconWidth = GH.Lcd.dLcdGetIconWidth(LARGE_ICON);
				pQ.IconHeight = GH.Lcd.dLcdGetIconHeight(LARGE_ICON);

				pQ.NoOfIcons = 0;
				if (Icon1 > ICON_NONE)
				{
					pQ.NoOfIcons++;
				}
				if (Icon2 > ICON_NONE)
				{
					pQ.NoOfIcons++;
				}
				pQ.Default = pAnswer;

				pQ.NeedUpdate = 1;
			}


			if (pQ.NeedUpdate != 0)
			{
				//* UPDATE ***************************************************************************************************
				pQ.NeedUpdate = 0;

				GH.Lcd.dLcdDrawPicture(GH.UiInstance.pLcd.Lcd, Color, pQ.ScreenStartX, pQ.ScreenStartY, pop3.Width, pop3.Height, pop3.Data);
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
							GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, Color, pQ.IconStartX, pQ.IconStartY, LARGE_ICON, Icon1);
						}
						break;

					case 2:
						{
							GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, Color, pQ.IconStartX, pQ.IconStartY, LARGE_ICON, Icon1);
							GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, Color, (short)(pQ.IconStartX + pQ.IconSpaceX), pQ.IconStartY, LARGE_ICON, Icon2);
						}
						break;

				}

				if (pAnswer == 0)
				{
					GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, Color, pQ.YesNoStartX, pQ.YesNoStartY, LARGE_ICON, NO_SEL);
					GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, Color, (short)(pQ.YesNoStartX + pQ.YesNoSpaceX), pQ.YesNoStartY, LARGE_ICON, YES_NOTSEL);
				}
				else
				{
					GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, Color, pQ.YesNoStartX, pQ.YesNoStartY, LARGE_ICON, NO_NOTSEL);
					GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, Color, (short)(pQ.YesNoStartX + pQ.YesNoSpaceX), pQ.YesNoStartY, LARGE_ICON, YES_SEL);
				}

				GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, Color, pQ.LineStartX, pQ.LineStartY, pQ.LineEndX, pQ.LineStartY);

				cUiUpdateLcd();
				GH.UiInstance.ScreenBusy = 0;
			}
			if (cUiGetShortPress(ENTER_BUTTON) != 0)
			{
				cUiButtonFlush();
				Result = OK;
				pState = 0;
			}
			if (cUiGetShortPress(BACK_BUTTON) != 0)
			{
				cUiButtonFlush();
				Result = OK;
				pState = 0;
				pAnswer = -1;
			}

			return ((sbyte)Result);
		}


		RESULT cUiIconQuestion(DATA8 Color, DATA16 X, DATA16 Y, ref DATA8 pState, ref DATA32 pIcons)
		{
			RESULT Result = RESULT.BUSY;
			IQUESTION pQ;
			DATA32 Mask;
			DATA32 TmpIcons;
			DATA16 Tmp;
			DATA16 Loop;
			DATA8 Icon;

			pQ = GH.UiInstance.IconQuestion;

			var pop2 = BmpHelper.Get(BmpType.POP2);

			if (pState == 0)
			{
				pState = 1;
				pQ.ScreenStartX = X;
				pQ.ScreenStartY = Y;
				pQ.ScreenWidth = pop2.Width;
				pQ.ScreenHeight = pop2.Height;
				pQ.IconWidth = GH.Lcd.dLcdGetIconWidth(LARGE_ICON);
				pQ.IconHeight = GH.Lcd.dLcdGetIconHeight(LARGE_ICON);
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

				GH.Lcd.dLcdDrawPicture(GH.UiInstance.pLcd.Lcd, Color, pQ.ScreenStartX, pQ.ScreenStartY, pop2.Width, pop2.Height, pop2.Data);
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
					GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, Color, (short)(pQ.IconStartX + pQ.IconSpaceX * Loop), pQ.IconStartY, LARGE_ICON, Icon);
					Loop++;
					Icon++;
					TmpIcons >>= 1;
				}

				// Show selection
				GH.Lcd.dLcdInverseRect(GH.UiInstance.pLcd.Lcd, (short)(pQ.SelectStartX + pQ.SelectSpaceX * pQ.PointerX), pQ.SelectStartY, pQ.SelectWidth, pQ.SelectHeight);

				// Update screen
				cUiUpdateLcd();
				GH.UiInstance.ScreenBusy = 0;
			}
			if (cUiGetShortPress(ENTER_BUTTON) != 0)
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
				Result = OK;
				pState = 0;
			}
			if (cUiGetShortPress(BACK_BUTTON) != 0)
			{
				pIcons = 0;
				cUiButtonFlush();
				Result = OK;
				pState = 0;
			}

			return (Result);
		}

		DATA8 cUiKeyboard(DATA8 Color, DATA16 X, DATA16 Y, DATA8 Icon, DATA8 Lng, DATA8[] pText, ref DATA8 pCharSet, DATA8[] pAnswer)
		{
			KEYB pK;
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


			DATA8[][][] KeyboardLayout = new DATA8[MAX_KEYB_DEEPT][][];
			DATA8[][] lay1 = new DATA8[MAX_KEYB_HEIGHT][];
			DATA8[][] lay2 = new DATA8[MAX_KEYB_HEIGHT][];
			DATA8[][] lay3 = new DATA8[MAX_KEYB_HEIGHT][];

			lay1[0] = new DATA8[MAX_KEYB_WIDTH] { (DATA8)'Q', (DATA8)'W', (DATA8)'E', (DATA8)'R', (DATA8)'T', (DATA8)'Y', (DATA8)'U', (DATA8)'I', (DATA8)'O', (DATA8)'P', 0x08, 0x00 };
			lay1[1] = new DATA8[MAX_KEYB_WIDTH] { 0x03, (DATA8)'A', (DATA8)'S', (DATA8)'D', (DATA8)'F', (DATA8)'G', (DATA8)'H', (DATA8)'J', (DATA8)'K', (DATA8)'L', 0x0D, 0x00 };
			lay1[2] = new DATA8[MAX_KEYB_WIDTH] { 0x01, (DATA8)'Z', (DATA8)'X', (DATA8)'C', (DATA8)'V', (DATA8)'B', (DATA8)'N', (DATA8)'M', 0x0D, 0x0D, 0x0D, 0x00 };
			lay1[3] = new DATA8[MAX_KEYB_WIDTH] { (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', 0x0D, 0x00 };

			lay2[0] = new DATA8[MAX_KEYB_WIDTH] { (DATA8)'q', (DATA8)'w', (DATA8)'e', (DATA8)'r', (DATA8)'t', (DATA8)'y', (DATA8)'u', (DATA8)'i', (DATA8)'o', (DATA8)'p', 0x08, 0x00 };
			lay2[1] = new DATA8[MAX_KEYB_WIDTH] { 0x03, (DATA8)'a', (DATA8)'s', (DATA8)'d', (DATA8)'f', (DATA8)'g', (DATA8)'h', (DATA8)'j', (DATA8)'k', (DATA8)'l', 0x0D, 0x00 };
			lay2[2] = new DATA8[MAX_KEYB_WIDTH] { 0x01, (DATA8)'z', (DATA8)'x', (DATA8)'c', (DATA8)'v', (DATA8)'b', (DATA8)'n', (DATA8)'m', 0x0D, 0x0D, 0x0D, 0x00 };
			lay2[3] = new DATA8[MAX_KEYB_WIDTH] { (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', 0x0D, 0x00 };

			lay3[0] = new DATA8[MAX_KEYB_WIDTH] { (DATA8)'1', (DATA8)'2', (DATA8)'3', (DATA8)'4', (DATA8)'5', (DATA8)'6', (DATA8)'7', (DATA8)'8', (DATA8)'9', (DATA8)'0', 0x08, 0x00 };
			lay3[1] = new DATA8[MAX_KEYB_WIDTH] { 0x04, (DATA8)'+', (DATA8)'-', (DATA8)'=', (DATA8)'<', (DATA8)'>', (DATA8)'/', (DATA8)'\\', (DATA8)'*', (DATA8)':', 0x0D, 0x00 };
			lay3[2] = new DATA8[MAX_KEYB_WIDTH] { 0x04, (DATA8)'(', (DATA8)')', (DATA8)'_', (DATA8)'.', (DATA8)'@', (DATA8)'!', (DATA8)'?', 0x0D, 0x0D, 0x0D, 0x00 };
			lay3[3] = new DATA8[MAX_KEYB_WIDTH] { (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', (DATA8)' ', 0x0D, 0x00 };

			KeyboardLayout[0] = lay1; KeyboardLayout[1] = lay2; KeyboardLayout[2] = lay3;

			pK = GH.UiInstance.Keyboard;

			if (pCharSet != 0)
			{
				pK.CharSet = pCharSet;
				pCharSet = 0;
				pK.ScreenStartX = X;
				pK.ScreenStartY = Y;

				if ((Icon >= 0) && (Icon < N_ICON_NOS))
				{
					pK.IconStartX = cUiAlignX((short)(pK.ScreenStartX + 7));
					pK.IconStartY = (short)(pK.ScreenStartY + 4);
					pK.TextStartX = (short)(pK.IconStartX + GH.Lcd.dLcdGetIconWidth(NORMAL_ICON));
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
			Height = MAX_KEYB_HEIGHT - 1;

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

			if (cUiGetShortPress(BACK_BUTTON) != 0)
			{
				SelectedChar = 0x0D;
				pAnswer[0] = 0;
			}
			if (cUiGetShortPress(ENTER_BUTTON) != 0)
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
							Tmp = (DATA16)pAnswer.Length;
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
							if (GH.Lms.ValidateChar(ref SelectedChar, pK.CharSet) == OK)
							{
								Tmp = (DATA16)pAnswer.Length;
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
							var keyc = BmpHelper.Get(BmpType.KeyboardCap);
							GH.Lcd.dLcdDrawPicture(GH.UiInstance.pLcd.Lcd, Color, pK.ScreenStartX, pK.ScreenStartY, keyc.Width, keyc.Height, keyc.Data);
						}
						break;

					case 1:
						{
							var keys = BmpHelper.Get(BmpType.KeyboardSmp);
							GH.Lcd.dLcdDrawPicture(GH.UiInstance.pLcd.Lcd, Color, pK.ScreenStartX, pK.ScreenStartY, keys.Width, keys.Height, keys.Data);
						}
						break;

					case 2:
						{
							var keyn = BmpHelper.Get(BmpType.KeyboardNum);
							GH.Lcd.dLcdDrawPicture(GH.UiInstance.pLcd.Lcd, Color, pK.ScreenStartX, pK.ScreenStartY, keyn.Width, keyn.Height, keyn.Data);
						}
						break;

				}
				if ((Icon >= 0) && (Icon < N_ICON_NOS))
				{
					GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, Color, pK.IconStartX, pK.IconStartY, NORMAL_ICON, Icon);
				}
				if (pText.Length > 0)
				{
					GH.Lcd.dLcdDrawText(GH.UiInstance.pLcd.Lcd, Color, pK.TextStartX, pK.TextStartY, SMALL_FONT, pText);
				}


				X4 = 0;
				X3 = (short)pAnswer.Length;
				if (X3 > 15)
				{
					X4 = (short)(X3 - 15);
				}

				GH.Lcd.dLcdDrawText(GH.UiInstance.pLcd.Lcd, Color, pK.StringStartX, pK.StringStartY, NORMAL_FONT, pAnswer.Skip(X4).ToArray());
				GH.Lcd.dLcdDrawChar(GH.UiInstance.pLcd.Lcd, Color, (short)(pK.StringStartX + (X3 - X4) * 8), pK.StringStartY, NORMAL_FONT, (sbyte)'_');

				SX = (short)(pK.KeybStartX + pK.PointerX * pK.KeybSpaceX);
				SY = (short)(pK.KeybStartY + pK.PointerY * pK.KeybSpaceY);

				switch (TmpChar)
				{
					case 0x01:
					case 0x02:
					case 0x03:
						{
							GH.Lcd.dLcdInverseRect(GH.UiInstance.pLcd.Lcd, (short)(SX - 8), SY, (short)(pK.KeybWidth + 8), pK.KeybHeight);
						}
						break;

					case 0x04:
						{
							GH.Lcd.dLcdInverseRect(GH.UiInstance.pLcd.Lcd, (short)(SX - 8), (short)(pK.KeybStartY + 1 * pK.KeybSpaceY), (short)(pK.KeybWidth + 8), (short)(pK.KeybHeight * 2 + 1));
						}
						break;

					case 0x08:
						{
							GH.Lcd.dLcdInverseRect(GH.UiInstance.pLcd.Lcd, (short)(SX + 2), SY, (short)(pK.KeybWidth + 5), pK.KeybHeight);
						}
						break;

					case 0x0D:
						{
							SX = (short)(pK.KeybStartX + 112);
							SY = (short)(pK.KeybStartY + 1 * pK.KeybSpaceY);
							GH.Lcd.dLcdInverseRect(GH.UiInstance.pLcd.Lcd, SX, SY, (short)(pK.KeybWidth + 5), (short)(pK.KeybSpaceY + 1));
							SX = (short)(pK.KeybStartX + 103);
							SY = (short)(pK.KeybStartY + 1 + 2 * pK.KeybSpaceY);
							GH.Lcd.dLcdInverseRect(GH.UiInstance.pLcd.Lcd, SX, SY, (short)(pK.KeybWidth + 14), (short)(pK.KeybSpaceY * 2 - 4));
						}
						break;

					case 0x20:
						{
							GH.Lcd.dLcdInverseRect(GH.UiInstance.pLcd.Lcd, (short)(pK.KeybStartX + 11), (short)(SY + 1), (short)(pK.KeybWidth + 68), (short)(pK.KeybHeight - 3));
						}
						break;

					default:
						{
							GH.Lcd.dLcdInverseRect(GH.UiInstance.pLcd.Lcd, (short)(SX + 1), SY, pK.KeybWidth, pK.KeybHeight);
						}
						break;

				}
				cUiUpdateLcd();
				GH.UiInstance.ScreenBusy = 0;
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
			GH.Lcd.dLcdRect(GH.UiInstance.pLcd.Lcd, Color, X, Y, X1, Y1);

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
						GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, (short)(X + 1), Tmp);
						GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, (short)(X + 2), Tmp);
						GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, (short)(X + 3), Tmp);
						Tmp++;
						GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, (short)(X + 1), Tmp);
						GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, (short)(X + 3), Tmp);
						Tmp++;
						GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, (short)(X + 2), Tmp);
						Tmp++;
						GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, (short)(X + 1), Tmp);
						GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, (short)(X + 3), Tmp);
						Tmp++;
						GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, (short)(X + 2), Tmp);
						Tmp++;
						GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, (short)(X + 1), Tmp);
						GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, (short)(X + 3), Tmp);
						Tmp++;
						GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, (short)(X + 1), Tmp);
						GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, (short)(X + 2), Tmp);
						GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, (short)(X + 3), Tmp);
					}
					break;
				case 6:
					{
						GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, (short)(X + 2), Tmp);
						GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, (short)(X + 3), Tmp);
						Tmp++;
						GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, (short)(X + 1), Tmp);
						GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, (short)(X + 4), Tmp);
						Tmp++;
						Tmp++;
						GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, (short)(X + 2), Tmp);
						GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, (short)(X + 3), Tmp);
						Tmp++;
						Tmp++;
						GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, (short)(X + 2), Tmp);
						GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, (short)(X + 3), Tmp);
						Tmp++;
						Tmp++;
						GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, (short)(X + 1), Tmp);
						GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, (short)(X + 4), Tmp);
						Tmp++;
						GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, (short)(X + 2), Tmp);
						GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, (short)(X + 3), Tmp);
					}
					break;
				default:
					{
						GH.Lcd.dLcdFillRect(GH.UiInstance.pLcd.Lcd, Color, X, Tmp, X1, KnobHeight);
					}
					break;
			}
		}

		RESULT cUiBrowser(DATA8 Type, DATA16 X, DATA16 Y, DATA16 X1, DATA16 Y1, DATA8 Lng, ref DATA8 pType, DATA8[] pAnswer)
		{
			RESULT Result = RESULT.BUSY;
			DATA32[] Image = null;
			BROWSER pB;
			PRGID PrgId;
			OBJID ObjId;
			DATA16 Tmp;
			DATA16 Indent;
			DATA16 Item;
			DATA16 TotalItems;
			DATA8 TmpType = 0;
			DATA8 Folder;
			DATA8 OldPriority = 0;
			DATA8 Priority = 0;
			DATA8 Color;
			DATA16 Ignore;
			DATA8 Data8 = 0;
			DATA32 Total = 0;
			DATA32 Free = 0;
			RESULT TmpResult;
			FileInfo TmpHandle = null;

			PrgId = GH.Lms.CurrentProgramId();
			ObjId = GH.Lms.CallingObjectId();
			pB = GH.UiInstance.Browser;

			Color = FG_COLOR;

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

			GH.Lms.CheckUsbstick(ref Data8, ref Total, ref Free, 0);
			if (Data8 != 0)
			{
				GH.UiInstance.UiUpdate = 1;
			}
			GH.Lms.CheckSdcard(ref Data8, ref Total, ref Free, 0);
			if (Data8 != 0)
			{
				GH.UiInstance.UiUpdate = 1;
			}

			if ((int)GH.Lms.ProgramStatusChange(USER_SLOT) == STOPPED)
			{
				if (Type != BROWSE_FILES)
				{
					Result = OK;
					pType = 0;
				}
			}

			if ((pType == TYPE_REFRESH_BROWSER))
			{
				GH.UiInstance.UiUpdate = 1;
			}

			if ((pType == TYPE_RESTART_BROWSER))
			{
				if (pB.hFiles != null)
				{
					GH.Memory.cMemoryCloseFolder(pB.PrgId, pB.hFiles);
				}
				if (pB.hFolders != null)
				{
					GH.Memory.cMemoryCloseFolder(pB.PrgId, pB.hFolders);
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
				pB.IconHeight = GH.Lcd.dLcdGetIconHeight(NORMAL_ICON);
				pB.LineHeight = (short)(pB.IconHeight + pB.LineSpace);
				pB.Lines = ((short)(pB.ScreenHeight / pB.LineHeight));

				// calculate chars and lines on screen
				pB.CharWidth = GH.Lcd.dLcdGetFontWidth(NORMAL_FONT);
				pB.CharHeight = GH.Lcd.dLcdGetFontHeight(NORMAL_FONT);
				pB.IconWidth = GH.Lcd.dLcdGetIconWidth(NORMAL_ICON);
				pB.Chars = ((short)((pB.ScreenWidth - pB.IconWidth) / pB.CharWidth));

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

				CommonHelper.Strncpy(pB.TopFolder, pAnswer, MAX_FILENAME_SIZE);

				pB.PrgId = PrgId;
				pB.ObjId = ObjId;

				pB.OldFiles = 0;
				pB.Folders = 0;
				pB.OpenFolder = 0;
				pB.Files = 0;
				pB.ItemStart = 1;
				pB.ItemPointer = 1;
				pB.NeedUpdate = 1;
				GH.UiInstance.UiUpdate = 1;
			}

			if ((pB.PrgId == PrgId) && (pB.ObjId == ObjId))
			{
				//* CTRL *****************************************************************************************************


				if (GH.UiInstance.UiUpdate != 0)
				{
					GH.UiInstance.UiUpdate = 0;

					if (pB.hFiles != null)
					{
						GH.Memory.cMemoryCloseFolder(pB.PrgId, pB.hFiles);
					}
					if (pB.hFolders != null)
					{
						GH.Memory.cMemoryCloseFolder(pB.PrgId, pB.hFolders);
					}

					pB.OpenFolder = 0;
					pB.Files = 0;
					pType = 0;

					switch (Type)
					{
						case BROWSE_FOLDERS:
						case BROWSE_FOLDS_FILES:
							{
								if (GH.Memory.cMemoryOpenFolder(PrgId, TYPE_FOLDER, pB.TopFolder, ref pB.hFolders) == OK)
								{
									//******************************************************************************************************
									if (pB.OpenFolder != 0)
									{
										GH.Memory.cMemoryGetItem(pB.PrgId, pB.hFolders, pB.OpenFolder, FOLDERNAME_SIZE + SUBFOLDERNAME_SIZE, pB.SubFolder, ref TmpType);
										if (CommonHelper.Strcmp(pB.SubFolder, SDCARD_FOLDER) == 0)
										{
											Item = pB.ItemPointer;
											GH.Memory.cMemoryGetItemName(pB.PrgId, pB.hFolders, Item, MAX_FILENAME_SIZE, pB.Filename, ref pType, ref Priority);
											Result = GH.Memory.cMemoryGetItem(pB.PrgId, pB.hFolders, Item, FOLDERNAME_SIZE + SUBFOLDERNAME_SIZE, pB.FullPath, ref pType);
											pType = TYPE_SDCARD;

											CommonHelper.Snprintf(pAnswer, 0, Lng, pB.FullPath);
										}
										else
										{
											if (CommonHelper.Strcmp(pB.SubFolder, USBSTICK_FOLDER) == 0)
											{
												Item = pB.ItemPointer;
												GH.Memory.cMemoryGetItemName(pB.PrgId, pB.hFolders, Item, MAX_FILENAME_SIZE, pB.Filename, ref pType, ref Priority);
												Result = GH.Memory.cMemoryGetItem(pB.PrgId, pB.hFolders, Item, FOLDERNAME_SIZE + SUBFOLDERNAME_SIZE, pB.FullPath, ref pType);
												pType = TYPE_USBSTICK;

												CommonHelper.Snprintf(pAnswer, 0, Lng, pB.FullPath);
											}
											else
											{
												Result = GH.Memory.cMemoryOpenFolder(PrgId, FILETYPE_UNKNOWN, pB.SubFolder, ref pB.hFiles);
												Result = RESULT.BUSY;
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

						case BROWSE_CACHE:
							{
							}
							break;

						case BROWSE_FILES:
							{
								if (GH.Memory.cMemoryOpenFolder(PrgId, FILETYPE_UNKNOWN, pB.TopFolder, ref pB.hFiles) == OK)
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

				if (CommonHelper.Strstr(pB.SubFolder, SDCARD_FOLDER.ToSbyteArray()))
				{
					pB.Sdcard = 1;
				}
				else
				{
					pB.Sdcard = 0;
				}

				if (CommonHelper.Strstr(pB.SubFolder, USBSTICK_FOLDER.ToSbyteArray()))
				{
					pB.Usbstick = 1;
				}
				else
				{
					pB.Usbstick = 0;
				}

				TmpResult = OK;
				switch (Type)
				{
					case BROWSE_FOLDERS:
					case BROWSE_FOLDS_FILES:
						{
							// Collect folders in directory
							TmpResult = GH.Memory.cMemoryGetFolderItems(pB.PrgId, pB.hFolders, ref pB.Folders);

							// Collect files in folder
							if ((pB.OpenFolder != 0) && (TmpResult == OK))
							{
								TmpResult = GH.Memory.cMemoryGetFolderItems(pB.PrgId, pB.hFiles, ref pB.Files);
							}
						}
						break;

					case BROWSE_CACHE:
						{
							pB.Folders = (DATA16)GH.Memory.cMemoryGetCacheFiles();
						}
						break;

					case BROWSE_FILES:
						{
							TmpResult = GH.Memory.cMemoryGetFolderItems(pB.PrgId, pB.hFiles, ref pB.Files);
						}
						break;

				}

				if ((pB.OpenFolder != 0) && (pB.OpenFolder == pB.ItemPointer))
				{
					if (cUiGetShortPress(BACK_BUTTON) != 0)
					{
						// Close folder
						GH.Memory.cMemoryCloseFolder(pB.PrgId, pB.hFiles);
						if (pB.ItemPointer > pB.OpenFolder)
						{
							pB.ItemPointer -= pB.Files;
						}
						pB.OpenFolder = 0;
						pB.Files = 0;
					}
				}

				if (pB.Sdcard == 1)
				{
					if (pB.OpenFolder == 0)
					{
						if (cUiGetShortPress(BACK_BUTTON) != 0)
						{
							// Collapse sdcard
							if (pB.hFiles != null)
							{
								GH.Memory.cMemoryCloseFolder(pB.PrgId, pB.hFiles);
							}
							if (pB.hFolders != null)
							{
								GH.Memory.cMemoryCloseFolder(pB.PrgId, pB.hFolders);
							}
							pB.PrgId = 0;
							pB.ObjId = 0;
							CommonHelper.Strcpy(pAnswer, vmPRJS_DIR);
							pType = 0;
							pB.SubFolder[0] = 0;
						}
					}
				}

				if (pB.Usbstick == 1)
				{
					if (pB.OpenFolder == 0)
					{
						if (cUiGetShortPress(BACK_BUTTON) != 0)
						{
							// Collapse usbstick
							if (pB.hFiles != null)
							{
								GH.Memory.cMemoryCloseFolder(pB.PrgId, pB.hFiles);
							}
							if (pB.hFolders != null)
							{
								GH.Memory.cMemoryCloseFolder(pB.PrgId, pB.hFolders);
							}
							pB.PrgId = 0;
							pB.ObjId = 0;
							CommonHelper.Strcpy(pAnswer, vmPRJS_DIR);
							pType = 0;
							pB.SubFolder[0] = 0;
						}
					}
				}
				if (pB.OldFiles != (pB.Files + pB.Folders))
				{
					pB.OldFiles = ((short)(pB.Files + pB.Folders));
					pB.NeedUpdate = 1;
				}

				if (cUiGetShortPress(ENTER_BUTTON) != 0)
				{
					pB.OldFiles = 0;
					if (pB.OpenFolder != 0)
					{
						if ((pB.ItemPointer > pB.OpenFolder) && (pB.ItemPointer <= (pB.OpenFolder + pB.Files)))
						{ // File selected

							Item = (short)(pB.ItemPointer - pB.OpenFolder);
							Result = GH.Memory.cMemoryGetItem(pB.PrgId, pB.hFiles, Item, Lng, pB.FullPath, ref pType);

							CommonHelper.Snprintf(pAnswer, 0, Lng, pB.FullPath);
						}
						else
						{ // Folder selected

							if (pB.OpenFolder == pB.ItemPointer)
							{ // Enter on open folder

								Item = pB.OpenFolder;
								Result = GH.Memory.cMemoryGetItem(pB.PrgId, pB.hFolders, Item, Lng, pAnswer, ref pType);
							}
							else
							{ // Close folder
								GH.Memory.cMemoryCloseFolder(pB.PrgId, pB.hFiles);
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
							case BROWSE_FOLDERS:
								{ // Folder

									Item = pB.ItemPointer;
									GH.Memory.cMemoryGetItemName(pB.PrgId, pB.hFolders, Item, MAX_FILENAME_SIZE, pB.Filename, ref pType, ref Priority);
									Result = GH.Memory.cMemoryGetItem(pB.PrgId, pB.hFolders, Item, FOLDERNAME_SIZE + SUBFOLDERNAME_SIZE, pB.FullPath, ref pType);

									CommonHelper.Snprintf(pAnswer, 0, Lng, pB.FullPath, new sbyte[] { (sbyte)'/' }, pB.Filename);
									pType = TYPE_BYTECODE;
								}
								break;

							case BROWSE_FOLDS_FILES:
								{ // Folder & File

									pB.OpenFolder = pB.ItemPointer;
									GH.Memory.cMemoryGetItem(pB.PrgId, pB.hFolders, pB.OpenFolder, FOLDERNAME_SIZE + SUBFOLDERNAME_SIZE, pB.SubFolder, ref TmpType);
									if (CommonHelper.Strcmp(pB.SubFolder, SDCARD_FOLDER) == 0)
									{
										Item = pB.ItemPointer;
										GH.Memory.cMemoryGetItemName(pB.PrgId, pB.hFolders, Item, MAX_FILENAME_SIZE, pB.Filename, ref pType, ref Priority);
										Result = GH.Memory.cMemoryGetItem(pB.PrgId, pB.hFolders, Item, FOLDERNAME_SIZE + SUBFOLDERNAME_SIZE, pB.FullPath, ref pType);
										pType = TYPE_SDCARD;

										CommonHelper.Snprintf(pAnswer, 0, Lng, pB.FullPath);
									}
									else
									{
										if (CommonHelper.Strcmp(pB.SubFolder, USBSTICK_FOLDER) == 0)
										{
											Item = pB.ItemPointer;
											GH.Memory.cMemoryGetItemName(pB.PrgId, pB.hFolders, Item, MAX_FILENAME_SIZE, pB.Filename, ref pType, ref Priority);
											Result = GH.Memory.cMemoryGetItem(pB.PrgId, pB.hFolders, Item, FOLDERNAME_SIZE + SUBFOLDERNAME_SIZE, pB.FullPath, ref pType);
											pType = TYPE_USBSTICK;

											CommonHelper.Snprintf(pAnswer, 0, Lng, pB.FullPath);
										}
										else
										{
											pB.ItemStart = pB.ItemPointer;
											Result = GH.Memory.cMemoryOpenFolder(PrgId, FILETYPE_UNKNOWN, pB.SubFolder, ref pB.hFiles);

											Result = RESULT.BUSY;
										}
									}
								}
								break;

							case BROWSE_CACHE:
								{ // Cache

									Item = pB.ItemPointer;

									pType = GH.Memory.cMemoryGetCacheName((sbyte)Item, FOLDERNAME_SIZE + SUBFOLDERNAME_SIZE, pB.FullPath, pB.Filename);
									CommonHelper.Snprintf(pAnswer, 0, Lng, pB.FullPath);
									Result = OK;
								}
								break;

							case BROWSE_FILES:
								{ // File

									if ((pB.ItemPointer > pB.OpenFolder) && (pB.ItemPointer <= (pB.OpenFolder + pB.Files)))
									{ // File selected

										Item = (short)(pB.ItemPointer - pB.OpenFolder);

										Result = GH.Memory.cMemoryGetItem(pB.PrgId, pB.hFiles, Item, Lng, pB.FullPath, ref pType);

										CommonHelper.Snprintf(pAnswer, 0, Lng, pB.FullPath);
										Result = OK;
									}
								}
								break;

						}
					}
				  pB.NeedUpdate = 1;
				}

				TotalItems = (short)(pB.Folders + pB.Files);
				if (TmpResult == OK)
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
					// clear screen
					GH.Lcd.dLcdFillRect(GH.UiInstance.pLcd.Lcd, BG_COLOR, pB.ScreenStartX, pB.ScreenStartY, pB.ScreenWidth, pB.ScreenHeight);

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
									case BROWSE_FOLDERS:
										{
											GH.Memory.cMemoryGetItemName(pB.PrgId, pB.hFolders, Item, (sbyte)pB.Chars, pB.Filename, ref TmpType, ref Priority);
											if (GH.Memory.cMemoryGetItemIcon(pB.PrgId, pB.hFolders, Item, ref TmpHandle, Image) == OK)
											{
												GH.Lcd.dLcdDrawBitmap(GH.UiInstance.pLcd.Lcd, Color, pB.IconStartX, (sbyte)(pB.IconStartY + (Tmp * pB.LineHeight)), CommonHelper.CastArray<int, byte>(Image));
												GH.Memory.cMemoryCloseFile(pB.PrgId, TmpHandle);
											}
											else
											{
												var pcApp = BmpHelper.Get(BmpType.App);
												GH.Lcd.dLcdDrawPicture(GH.UiInstance.pLcd.Lcd, Color, pB.IconStartX, (short)(pB.IconStartY + (Tmp * pB.LineHeight)), pcApp.Width, pcApp.Height, pcApp.Data);
											}

											pB.Text[0] = 0;
											if (CommonHelper.Strcmp(pB.Filename, "Bluetooth") == 0)
											{
												if (GH.UiInstance.BtOn != 0)
												{
													pB.Text[0] = (sbyte)'+';
												}
												else
												{
													pB.Text[0] = (sbyte)'-';
												}
											}
											else
											{
												if (CommonHelper.Strcmp(pB.Filename, "WiFi") == 0)
												{
													if (GH.UiInstance.WiFiOn != 0)
													{
														pB.Text[0] = (sbyte)'+';
													}
													else
													{
														pB.Text[0] = (sbyte)'-';
													}
												}
												else
												{
													if (GH.Memory.cMemoryGetItemText(pB.PrgId, pB.hFolders, Item, (sbyte)pB.Chars, pB.Text) != OK)
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

												case (sbyte)'+':
													{
														Indent = (short)((pB.Chars - 1) * pB.CharWidth - GH.Lcd.dLcdGetIconWidth(MENU_ICON));
														GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, Color, (short)(pB.TextStartX + Indent), (short)((pB.TextStartY - 2) + (Tmp * pB.LineHeight)), MENU_ICON, ICON_CHECKED);
													}
													break;

												case (sbyte)'-':
													{
														Indent = (short)((pB.Chars - 1) * pB.CharWidth - GH.Lcd.dLcdGetIconWidth(MENU_ICON));
														GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, Color, (short)(pB.TextStartX + Indent), (short)((pB.TextStartY - 2) + (Tmp * pB.LineHeight)), MENU_ICON, ICON_CHECKBOX);
													}
													break;

												default:
													{
														Indent = (short)(((pB.Chars - 1) - (DATA16)pB.Text.Length) * pB.CharWidth);
														GH.Lcd.dLcdDrawText(GH.UiInstance.pLcd.Lcd, Color, (short)(pB.TextStartX + Indent), (short)(pB.TextStartY + (Tmp * pB.LineHeight)), NORMAL_FONT, pB.Text);
													}
													break;

											}

										}
										break;

									case BROWSE_FOLDS_FILES:
										{
											GH.Memory.cMemoryGetItemName(pB.PrgId, pB.hFolders, Item, (sbyte)pB.Chars, pB.Filename, ref TmpType, ref Priority);
											GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, Color, pB.IconStartX, (short)(pB.IconStartY + (Tmp * pB.LineHeight)), NORMAL_ICON, FiletypeToNormalIcon[TmpType]);

											if ((Priority == 1) || (Priority == 2))
											{
												GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, Color, pB.IconStartX, (short)(pB.IconStartY + (Tmp * pB.LineHeight)), NORMAL_ICON, ICON_FOLDER2);
											}
											else
											{
												if (Priority == 3)
												{
													GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, Color, pB.IconStartX, (short)(pB.IconStartY + (Tmp * pB.LineHeight)), NORMAL_ICON, ICON_SD);
												}
												else
												{
													if (Priority == 4)
													{
														GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, Color, pB.IconStartX, (short)(pB.IconStartY + (Tmp * pB.LineHeight)), NORMAL_ICON, ICON_USB);
													}
													else
													{
														GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, Color, pB.IconStartX, (short)(pB.IconStartY + (Tmp * pB.LineHeight)), NORMAL_ICON, FiletypeToNormalIcon[TmpType]);
													}
												}
											}
											if (Priority != OldPriority)
											{
												if ((Priority == 1) || (Priority >= 3))
												{
													if (Tmp != 0)
													{
														GH.Lcd.dLcdDrawDotLine(GH.UiInstance.pLcd.Lcd, Color, pB.SelectStartX, (short)(pB.SelectStartY + ((Tmp - 1) * pB.LineHeight) + pB.LineHeight - 2), (short)(pB.SelectStartX + pB.SelectWidth), (short)(pB.SelectStartY + ((Tmp - 1) * pB.LineHeight) + pB.LineHeight - 2), 1, 2);
													}
												}
											}
										}
										break;

									case BROWSE_CACHE:
										{
											TmpType = GH.Memory.cMemoryGetCacheName((sbyte)Item, (sbyte)pB.Chars, pB.FullPath, pB.Filename);
											GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, Color, pB.IconStartX, (short)(pB.IconStartY + (Tmp * pB.LineHeight)), NORMAL_ICON, FiletypeToNormalIcon[TmpType]);
										}
										break;

									case BROWSE_FILES:
										{
											GH.Memory.cMemoryGetItemName(pB.PrgId, pB.hFiles, Item, (sbyte)pB.Chars, pB.Filename, ref TmpType, ref Priority);
											GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, Color, pB.IconStartX, (short)(pB.IconStartY + (Tmp * pB.LineHeight)), NORMAL_ICON, FiletypeToNormalIcon[TmpType]);
										}
										break;

								}
								// Draw folder name
								GH.Lcd.dLcdDrawText(GH.UiInstance.pLcd.Lcd, Color, pB.TextStartX, (short)(pB.TextStartY + (Tmp * pB.LineHeight)), NORMAL_FONT, pB.Filename);

								// Draw open folder
								if (Item == pB.OpenFolder)
								{
									GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, Color, 144, (short)(pB.IconStartY + (Tmp * pB.LineHeight)), NORMAL_ICON, ICON_OPENFOLDER);
								}

								// Draw selection
								if (pB.ItemPointer == (Tmp + pB.ItemStart))
								{
									GH.Lcd.dLcdInverseRect(GH.UiInstance.pLcd.Lcd, pB.SelectStartX, (short)(pB.SelectStartY + (Tmp * pB.LineHeight)), (short)(pB.SelectWidth + 1), pB.SelectHeight);
								}

								// Draw end line
								switch (Type)
								{
									case BROWSE_FOLDERS:
									case BROWSE_FOLDS_FILES:
									case BROWSE_FILES:
										{
											if (((Tmp + pB.ItemStart) == TotalItems) && (Tmp < (pB.Lines - 1)))
											{
												GH.Lcd.dLcdDrawDotLine(GH.UiInstance.pLcd.Lcd, Color, pB.SelectStartX, (short)(pB.SelectStartY + (Tmp * pB.LineHeight) + pB.LineHeight - 2), (short)(pB.SelectStartX + pB.SelectWidth), (short)(pB.SelectStartY + (Tmp * pB.LineHeight) + pB.LineHeight - 2), 1, 2);
											}
										}
										break;

									case BROWSE_CACHE:
										{
											if (((Tmp + pB.ItemStart) == 1) && (Tmp < (pB.Lines - 1)))
											{
												GH.Lcd.dLcdDrawDotLine(GH.UiInstance.pLcd.Lcd, Color, pB.SelectStartX, (short)(pB.SelectStartY + (Tmp * pB.LineHeight) + pB.LineHeight - 2), (short)(pB.SelectStartX + pB.SelectWidth), (short)(pB.SelectStartY + (Tmp * pB.LineHeight) + pB.LineHeight - 2), 1, 2);
											}
											if (((Tmp + pB.ItemStart) == TotalItems) && (Tmp < (pB.Lines - 1)))
											{
												GH.Lcd.dLcdDrawDotLine(GH.UiInstance.pLcd.Lcd, Color, pB.SelectStartX, (short)(pB.SelectStartY + (Tmp * pB.LineHeight) + pB.LineHeight - 2), (short)(pB.SelectStartX + pB.SelectWidth), (short)(pB.SelectStartY + (Tmp * pB.LineHeight) + pB.LineHeight - 2), 1, 2);
											}
										}
										break;

								}
							}
							else
							{ // Show file

								// Get file name and type
								GH.Memory.cMemoryGetItemName(pB.PrgId, pB.hFiles, Item, (sbyte)(pB.Chars - 1), pB.Filename, ref TmpType, ref Priority);

								// show File icons
								GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, Color, (short)(pB.IconStartX + pB.CharWidth), (short)(pB.IconStartY + (Tmp * pB.LineHeight)), NORMAL_ICON, FiletypeToNormalIcon[TmpType]);

								// Draw file name
								GH.Lcd.dLcdDrawText(GH.UiInstance.pLcd.Lcd, Color, (short)(pB.TextStartX + pB.CharWidth), (short)(pB.TextStartY + (Tmp * pB.LineHeight)), NORMAL_FONT, pB.Filename);

								// Draw folder line
								if ((Tmp == (pB.Lines - 1)) || (Item == pB.Files))
								{
									GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, Color, (short)(pB.IconStartX + pB.CharWidth - 3), (short)(pB.SelectStartY + (Tmp * pB.LineHeight)), (short)(pB.IconStartX + pB.CharWidth - 3), (short)(pB.SelectStartY + (Tmp * pB.LineHeight) + pB.SelectHeight - 1));
								}
								else
								{
									GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, Color, (short)(pB.IconStartX + pB.CharWidth - 3), (short)(pB.SelectStartY + (Tmp * pB.LineHeight)), (short)(pB.IconStartX + pB.CharWidth - 3), (short)(pB.SelectStartY + (Tmp * pB.LineHeight) + pB.LineHeight - 1));
								}

								// Draw selection
								if (pB.ItemPointer == (Tmp + pB.ItemStart))
								{
									GH.Lcd.dLcdInverseRect(GH.UiInstance.pLcd.Lcd, (short)(pB.SelectStartX + pB.CharWidth), (short)(pB.SelectStartY + (Tmp * pB.LineHeight)), (short)(pB.SelectWidth + 1 - pB.CharWidth), pB.SelectHeight);
								}
							}

							//************************************************************************************************************
						}
						OldPriority = Priority;
					}

					cUiDrawBar(1, pB.ScrollStartX, pB.ScrollStartY, pB.ScrollWidth, pB.ScrollHeight, 0, TotalItems, pB.ItemPointer);

					// Update
					cUiUpdateLcd();
					GH.UiInstance.ScreenBusy = 0;
				}

				if (Result != OK)
				{
					Tmp = cUiTestHorz();
					if (Ignore == Tmp)
					{
						Tmp = cUiGetHorz();
						Tmp = 0;
					}

					if ((Tmp != 0) || (cUiTestShortPress(BACK_BUTTON) != 0) || (cUiTestLongPress(BACK_BUTTON) != 0))
					{
						if (Type != BROWSE_CACHE)
						{
							if (pB.OpenFolder != 0)
							{
								if (pB.hFiles != null)
								{
									GH.Memory.cMemoryCloseFolder(pB.PrgId, pB.hFiles);
								}
							}
							if (pB.hFolders != null)
							{
								GH.Memory.cMemoryCloseFolder(pB.PrgId, pB.hFolders);
							}
						}
						pB.PrgId = 0;
						pB.ObjId = 0;
						pB.SubFolder[0] = 0;
						pAnswer[0] = 0;
						pType = 0;
						Result = OK;
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
				pType = TYPE_RESTART_BROWSER;
				Result = RESULT.FAIL;
			}

			if (pType > 0)
			{
				if (pB.Sdcard != 0)
				{
					pType |= TYPE_SDCARD;
				}
				if (pB.Usbstick != 0)
				{
					pType |= TYPE_USBSTICK;
				}
			}

			if (Result != RESULT.BUSY)
			{
				//* EXIT *****************************************************************************************************
			}

			return (Result);
		}

		DATA16 cUiTextboxGetLines(DATA8[] pText, DATA32 Size, DATA8 Del)
		{
			DATA32 Point = 0;
			DATA16 Lines = 0;
			DATA8 DelPoi;

			if (Del < DELS)
			{
				while (pText.Length > Point && (Point < Size))
				{
					DelPoi = 0;
					while ((pText.Length > Point) && (Point < Size) && (Delimiter[Del].Length > DelPoi) && (pText[Point] == Delimiter[Del][DelPoi]))
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
						if ((pText.Length > Point) && (Point < Size))
						{
							Point++;
						}
					}
				}
			}

			return (Lines);
		}

		void cUiTextboxAppendLine(DATA8[] pText, DATA32 Size, DATA8 Del, ref DATA8 pLine, DATA8 Font)
		{
			DATA32 Point = 0;
			DATA8 DelPoi = 0;

			if (Del < DELS)
			{
				while ((pText.Length > Point) && (Point < Size))
				{
					Point++;
				}
				if ((Point < Size) && (Font != 0))
				{
					pText[Point] = Font;
					Point++;
				}

				while ((Point < Size) && (pLine != 0))
				{
					pText[Point] = pLine;
					Point++;
					pLine++;
				}
				while ((Point < Size) && (Delimiter[Del].Length > DelPoi))
				{
					pText[Point] = Delimiter[Del][DelPoi];
					Point++;
					DelPoi++;
				}
			}
		}

		DATA32 cUiTextboxFindLine(DATA8[] pText, DATA32 Size, DATA8 Del, DATA16 Line, ref DATA8 pFont)
		{
			DATA32 Result = -1;
			DATA32 Point = 0;
			DATA8 DelPoi = 0;

			pFont = 0;
			if (Del < DELS)
			{
				Result = Point;
				while ((Line != 0) && (pText.Length > Point) && (Point < Size))
				{

					DelPoi = 0;
					while ((pText.Length > Point) && (Point < Size) && (Delimiter[Del].Length > DelPoi) && (pText[Point] == Delimiter[Del][DelPoi]))
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
						if ((pText.Length > Point) && (Point < Size))
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
					if ((pText[Result] > 0) && (pText[Result] < FONTTYPES))
					{
						pFont = pText[Result];
						Result++;
					}
				}
			}

			return (Result);
		}

		void cUiTextboxReadLine(DATA8[] pText, DATA32 Size, DATA8 Del, DATA8 Lng, DATA16 Line, DATA8[] pLine, ref DATA8 pFont)
		{
			DATA32 Start;
			DATA32 Point = 0;
			DATA8 DelPoi = 0;
			DATA8 Run = 1;

			Start = cUiTextboxFindLine(pText, Size, Del, Line, ref pFont);
			Point = Start;

			pLine[0] = 0;

			if (Point >= 0)
			{
				while ((Run != 0) && (pText.Length > Point) && (Point < Size))
				{
					DelPoi = 0;
					while ((pText.Length > Point) && (Point < Size) && (Delimiter[Del].Length > DelPoi) && (pText[Point] == Delimiter[Del][DelPoi]))
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
						if ((pText.Length > Point) && (Point < Size))
						{
							Point++;
						}
					}
				}
				Point -= (DATA32)DelPoi;

				if (((Point - Start) + 1) < (DATA32)Lng)
				{
					Lng = (sbyte)((DATA8)(Point - Start) + 1);
				}
				CommonHelper.Snprintf(pLine, 0, Lng, pText.Skip(Start).ToArray());
			}
		}

		RESULT cUiTextbox(DATA16 X, DATA16 Y, DATA16 X1, DATA16 Y1, DATA8[] pText, DATA32 Size, DATA8 Del, ref DATA16 pLine)
		{
			RESULT Result = RESULT.BUSY;
			TXTBOX pB;
			DATA16 Item;
			DATA16 TotalItems;
			DATA16 Tmp;
			DATA16 Ypos;
			DATA8 Color;

			pB = GH.UiInstance.Txtbox;
			Color = FG_COLOR;

			if (pLine < 0)
			{
				//* INIT *****************************************************************************************************
				// Define screen
				pB.ScreenStartX = X;
				pB.ScreenStartY = Y;
				pB.ScreenWidth = X1;
				pB.ScreenHeight = Y1;

				pB.Font = GH.UiInstance.Font;

				// calculate chars and lines on screen
				pB.CharWidth = GH.Lcd.dLcdGetFontWidth(pB.Font);
				pB.CharHeight = GH.Lcd.dLcdGetFontHeight(pB.Font);
				pB.Chars = ((short)(pB.ScreenWidth / pB.CharWidth));

				// calculate lines on screen
				pB.LineSpace = 5;
				pB.LineHeight = (short)(pB.CharHeight + pB.LineSpace);
				pB.Lines = ((short)(pB.ScreenHeight / pB.LineHeight));

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

			if (cUiGetShortPress(ENTER_BUTTON) != 0)
			{
				pLine = pB.ItemPointer;

				Result = OK;
			}
			if (cUiGetShortPress(BACK_BUTTON) != 0)
			{
				pLine = -1;

				Result = OK;
			}

			if (pB.NeedUpdate != 0)
			{
				//* UPDATE ***************************************************************************************************
				pB.NeedUpdate = 0;

				// clear screen
				GH.Lcd.dLcdFillRect(GH.UiInstance.pLcd.Lcd, BG_COLOR, pB.ScreenStartX, pB.ScreenStartY, pB.ScreenWidth, pB.ScreenHeight);

				Ypos = (short)(pB.TextStartY + 2);

				for (Tmp = 0; Tmp < pB.Lines; Tmp++)
				{
					Item = (short)(Tmp + pB.ItemStart);

					if (Item <= TotalItems)
					{
						cUiTextboxReadLine(pText, Size, Del, TEXTSIZE, Item, pB.Text, ref pB.Font);

						// calculate chars and lines on screen
						pB.CharWidth = GH.Lcd.dLcdGetFontWidth(pB.Font);
						pB.CharHeight = GH.Lcd.dLcdGetFontHeight(pB.Font);

						// calculate lines on screen
						pB.LineSpace = 2;
						pB.LineHeight = (short)(pB.CharHeight + pB.LineSpace);
						pB.Lines = ((short)(pB.ScreenHeight / pB.LineHeight));

						// Calculate selection barBrowser
						pB.SelectStartX = pB.ScreenStartX;
						pB.SelectWidth = (short)(pB.ScreenWidth - (pB.ScrollWidth + 2));
						pB.SelectStartY = (short)(pB.TextStartY - 1);
						pB.SelectHeight = (short)(pB.CharHeight + 1);

						pB.Chars = ((short)(pB.SelectWidth / pB.CharWidth));

						pB.Text[pB.Chars] = 0;

						if ((Ypos + pB.LineHeight) <= (pB.ScreenStartY + pB.ScreenHeight))
						{
							GH.Lcd.dLcdDrawText(GH.UiInstance.pLcd.Lcd, Color, pB.TextStartX, Ypos, pB.Font, pB.Text);
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
							GH.Lcd.dLcdInverseRect(GH.UiInstance.pLcd.Lcd, pB.SelectStartX, (short)(Ypos - 1), pB.SelectWidth, pB.LineHeight);
						}
					}
					Ypos += pB.LineHeight;
				}

				// Update
				cUiUpdateLcd();
				GH.UiInstance.ScreenBusy = 0;
			}

			return (Result);
		}

		void cUiGraphSetup(DATA16 StartX, DATA16 SizeX, DATA8 Items, DATA16[] pOffset, DATA16[] pSpan, DATAF[] pMin, DATAF[] pMax, DATAF[] pVal)
		{
			DATA16 Item;
			DATA16 Pointer;

			GH.UiInstance.Graph.Initialized = 0;

			GH.UiInstance.Graph.pOffset = pOffset;
			GH.UiInstance.Graph.pSpan = pSpan;
			GH.UiInstance.Graph.pMin = pMin;
			GH.UiInstance.Graph.pMax = pMax;
			GH.UiInstance.Graph.pVal = pVal;

			if (Items < 0)
			{
				Items = 0;
			}
			if (Items > GRAPH_BUFFERS)
			{
				Items = GRAPH_BUFFERS;
			}


			GH.UiInstance.Graph.GraphStartX = StartX;
			GH.UiInstance.Graph.GraphSizeX = SizeX;
			GH.UiInstance.Graph.Items = Items;
			GH.UiInstance.Graph.Pointer = 0;

			for (Item = 0; Item < GH.UiInstance.Graph.Items; Item++)
			{
				for (Pointer = 0; Pointer < GH.UiInstance.Graph.GraphSizeX; Pointer++)
				{
					GH.UiInstance.Graph.Buffer[Item][Pointer] = DATAF_NAN;
				}
			}

			GH.UiInstance.Graph.Initialized = 1;

			// Simulate graph
			GH.UiInstance.Graph.Value = GH.UiInstance.Graph.pMin[0];
			GH.UiInstance.Graph.Down = 0;
			GH.UiInstance.Graph.Inc = (GH.UiInstance.Graph.pMax[0] - GH.UiInstance.Graph.pMin[0]) / (DATAF)20;
		}

		void cUiGraphSample()
		{
			DATAF Sample;
			DATA16 Item;
			DATA16 Pointer;

			if (GH.UiInstance.Graph.Initialized != 0)
			{ // Only if initialized

				if (GH.UiInstance.Graph.Pointer < GH.UiInstance.Graph.GraphSizeX)
				{
					for (Item = 0; Item < (GH.UiInstance.Graph.Items); Item++)
					{
						// Insert sample
						Sample = GH.UiInstance.Graph.pVal[Item];

						if (!(float.IsNaN(Sample)))
						{
							GH.UiInstance.Graph.Buffer[Item][GH.UiInstance.Graph.Pointer] = Sample;
						}
						else
						{
							GH.UiInstance.Graph.Buffer[Item][GH.UiInstance.Graph.Pointer] = DATAF_NAN;
						}
					}
					GH.UiInstance.Graph.Pointer++;
				}
				else
				{
					// Scroll buffers
					for (Item = 0; Item < (GH.UiInstance.Graph.Items); Item++)
					{
						for (Pointer = 0; Pointer < (GH.UiInstance.Graph.GraphSizeX - 1); Pointer++)
						{
							GH.UiInstance.Graph.Buffer[Item][Pointer] = GH.UiInstance.Graph.Buffer[Item][Pointer + 1];
						}

						// Insert sample
						Sample = GH.UiInstance.Graph.pVal[Item];

						if (!(float.IsNaN(Sample)))
						{
							GH.UiInstance.Graph.Buffer[Item][Pointer] = Sample;
						}
						else
						{
							GH.UiInstance.Graph.Buffer[Item][Pointer] = DATAF_NAN;
						}
					}
				}
			}
		}

		void cUiGraphDraw(DATA8 View, ref DATAF pActual, ref DATAF pLowest, ref DATAF pHighest, ref DATAF pAverage)
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

			pActual = DATAF_NAN;
			pLowest = DATAF_NAN;
			pHighest = DATAF_NAN;
			pAverage = DATAF_NAN;
			Samples = 0;

			if (GH.UiInstance.Graph.Initialized != 0)
			{ // Only if initialized

				if (GH.UiInstance.ScreenBlocked == 0)
				{

					// View or all
					if ((View >= 0) && (View < GH.UiInstance.Graph.Items))
					{
						Item = View;

						Y1 = ((short)(GH.UiInstance.Graph.pOffset[Item] + GH.UiInstance.Graph.pSpan[Item]));

						// Draw buffers
						X = GH.UiInstance.Graph.GraphStartX;
						for (Pointer = 0; Pointer < GH.UiInstance.Graph.Pointer; Pointer++)
						{
							Sample = GH.UiInstance.Graph.Buffer[Item][Pointer];
							if (!(float.IsNaN(Sample)))
							{
								pActual = Sample;
								if (float.IsNaN(pAverage))
								{
									pAverage = (DATAF)0;
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
								Value = (DATA16)((((Sample - GH.UiInstance.Graph.pMin[Item]) * (DATAF)GH.UiInstance.Graph.pSpan[Item]) / (GH.UiInstance.Graph.pMax[Item] - GH.UiInstance.Graph.pMin[Item])));

								// Limit Y axis
								if (Value > GH.UiInstance.Graph.pSpan[Item])
								{
									Value = GH.UiInstance.Graph.pSpan[Item];
								}
								if (Value < 0)
								{
									Value = 0;
								}
								/*
											printf("S=%-3d V=%3.0f L=%3.0f H=%3.0f A=%3.0f v=%3.0f ^=%3.0f O=%3d S=%3d Y=%d\r\n",Samples,*pActual,*pLowest,*pHighest,*pAverage,UiInstance.Graph.pMin[Item],UiInstance.Graph.pMax[Item],UiInstance.Graph.pOffset[Item],UiInstance.Graph.pSpan[Item],Value);
								*/
								Y2 = (short)((GH.UiInstance.Graph.pOffset[Item] + GH.UiInstance.Graph.pSpan[Item]) - Value);
								if (Pointer > 1)
								{
									if (Y2 > Y1)
									{
										GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, Color, (short)(X - 2), (short)(Y1 - 1), (short)(X - 1), (short)(Y2 - 1));
										GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, Color, X, (short)(Y1 + 1), (short)(X + 1), (short)(Y2 + 1));
									}
									else
									{
										if (Y2 < Y1)
										{
											GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, Color, X, (short)(Y1 - 1), (short)(X + 1), (short)(Y2 - 1));
											GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, Color, (short)(X - 2), (short)(Y1 + 1), (short)(X - 1), (short)(Y2 + 1));
										}
										else
										{
											GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, Color, (short)(X - 1), (short)(Y1 - 1), X, (short)(Y2 - 1));
											GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, Color, (short)(X - 1), (short)(Y1 + 1), X, (short)(Y2 + 1));
										}
									}
									GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, Color, (short)(X - 1), Y1, X, Y2);
								}
								else
								{
									GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, X, Y2);
								}

								Y1 = Y2;
							}
							X++;
						}
						if (Samples != 0)
						{
							pAverage = pAverage / (DATAF)Samples;
						}

					}
					else
					{
						// Draw buffers
						for (Item = 0; Item < GH.UiInstance.Graph.Items; Item++)
						{
							Y1 = ((short)(GH.UiInstance.Graph.pOffset[Item] + GH.UiInstance.Graph.pSpan[Item]));

							X = (short)(GH.UiInstance.Graph.GraphStartX + 1);
							for (Pointer = 0; Pointer < GH.UiInstance.Graph.Pointer; Pointer++)
							{
								Sample = GH.UiInstance.Graph.Buffer[Item][Pointer];

								// Scale Y axis
								Value = (DATA16)((((Sample - GH.UiInstance.Graph.pMin[Item]) * (DATAF)GH.UiInstance.Graph.pSpan[Item]) / (GH.UiInstance.Graph.pMax[Item] - GH.UiInstance.Graph.pMin[Item])));

								// Limit Y axis
								if (Value > GH.UiInstance.Graph.pSpan[Item])
								{
									Value = GH.UiInstance.Graph.pSpan[Item];
								}
								if (Value < 0)
								{
									Value = 0;
								}
								Y2 = (short)((GH.UiInstance.Graph.pOffset[Item] + GH.UiInstance.Graph.pSpan[Item]) - Value);
								if (Pointer > 1)
								{

									if (Y2 > Y1)
									{
										GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, Color, (short)(X - 2), (short)(Y1 - 1), (short)(X - 1), (short)(Y2 - 1));
										GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, Color, X, (short)(Y1 + 1), (short)(X + 1), (short)(Y2 + 1));
									}
									else
									{
										if (Y2 < Y1)
										{
											GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, Color, X, (short)(Y1 - 1), (short)(X + 1), (short)(Y2 - 1));
											GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, Color, (short)(X - 2), (short)(Y1 + 1), (short)(X - 1), (short)(Y2 + 1));
										}
										else
										{
											GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, Color, (short)(X - 1), (short)(Y1 - 1), X, (short)(Y2 - 1));
											GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, Color, (short)(X - 1), (short)(Y1 + 1), X, (short)(Y2 + 1));
										}
									}
									GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, Color, (short)(X - 1), Y1, X, Y2);

								}
								else
								{
									GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, X, Y2);
								}

								Y1 = Y2;
								X++;
							}
						}
					}
					GH.UiInstance.ScreenBusy = 1;
				}
			}
		}

		public void cUiDraw()
		{
			PRGID TmpPrgId;
			OBJID TmpObjId;
			IP TmpIp;
			int TmpIpInd;
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
			DATA8[] pText;
			DATA8 No;
			DATAF DataF;
			DATA8 Figures;
			DATA8 Decimals;
			IP pI;
			DATA8 pState;
			DATA8[] pAnswer;
			DATA8 Lng = 0;
			DATA8 SelectedChar;
			DATA8 pType;
			DATA8 Type;
			DATA16 On;
			DATA16 Off;
			DATA16 CharWidth;
			DATA16 CharHeight;
			DATA8 TmpColor;
			DATA16 Tmp;
			DATA8 Length;
			DATA8[] pUnit;
			DATA32 pIcons;
			DATA8 Items;
			DATA8 View;
			DATA16[] pOffset;
			DATA16[] pSpan;
			DATAF[] pMin;
			DATAF[] pMax;
			DATAF[] pVal;
			DATA16 Min;
			DATA16 Max;
			DATA16 Act;
			DATAF Actual = 0;
			DATAF Lowest = 0;
			DATAF Highest = 0;
			DATAF Average = 0;
			DATA8 Icon1;
			DATA8 Icon2;
			DATA8 Icon3;
			DATA8 Blocked;
			DATA8 Open;
			DATA8 Del;
			DATA8 pCharSet;
			DATA16 pLine;


			TmpPrgId = GH.Lms.CurrentProgramId();

			if ((TmpPrgId != GUI_SLOT) && (TmpPrgId != DEBUG_SLOT))
			{
				GH.UiInstance.RunScreenEnabled = 0;
			}
			if (GH.UiInstance.ScreenBlocked == 0)
			{
				Blocked = 0;
			}
			else
			{
				TmpObjId = GH.Lms.CallingObjectId();
				if ((TmpPrgId == GH.UiInstance.ScreenPrgId) && (TmpObjId == GH.UiInstance.ScreenObjId))
				{
					Blocked = 0;
				}
				else
				{
					Blocked = 1;
				}
			}

			TmpIp = GH.Lms.GetObjectIp();
			TmpIpInd = GH.Lms.GetObjectIpInd();
			Cmd = (DATA8)GH.Lms.PrimParPointer();

			switch (Cmd)
			{ // Function

				case UPDATE:
					{
						if (Blocked == 0)
						{
							cUiUpdateLcd();
							GH.UiInstance.ScreenBusy = 0;
						}
					}
					break;

				case CLEAN:
					{
						if (Blocked == 0)
						{
							GH.UiInstance.Font = NORMAL_FONT;

							Color = BG_COLOR;
							if (Color != 0)
							{
								Color = -1;
							}
							CommonHelper.Memset(GH.UiInstance.pLcd.Lcd, (byte)Color, LCD_BUFFER_SIZE, 0);

							GH.UiInstance.ScreenBusy = 1;
						}
					}
					break;

				case TEXTBOX:
					{
						X = (DATA16)GH.Lms.PrimParPointer();  // start x
						Y = (DATA16)GH.Lms.PrimParPointer();  // start y
						X1 = (DATA16)GH.Lms.PrimParPointer();  // size x
						Y1 = (DATA16)GH.Lms.PrimParPointer();  // size y
						pText = (DATA8[])GH.Lms.PrimParPointer();    // textbox
						Size = (DATA32)GH.Lms.PrimParPointer();  // textbox size
						Del = (DATA8)GH.Lms.PrimParPointer();   // delimitter
						pLine = (DATA16)GH.Lms.PrimParPointer();   // line

						if (Blocked == 0)
						{
							if (cUiTextbox(X, Y, X1, Y1, pText, Size, Del, ref pLine) == RESULT.BUSY)
							{
								GH.Lms.SetObjectIp(TmpIp);
								GH.Lms.SetObjectIpInd(TmpIpInd - 1);
								GH.Lms.SetDispatchStatus(BUSYBREAK);
							}
						}
						else
						{
							GH.Lms.SetObjectIp(TmpIp);
							GH.Lms.SetObjectIpInd(TmpIpInd - 1);
							GH.Lms.SetDispatchStatus(BUSYBREAK);
						}
					}
					break;

				case FILLRECT:
					{
						Color = (DATA8)GH.Lms.PrimParPointer();
						X = (DATA16)GH.Lms.PrimParPointer();
						Y = (DATA16)GH.Lms.PrimParPointer();
						X1 = (DATA16)GH.Lms.PrimParPointer();
						Y1 = (DATA16)GH.Lms.PrimParPointer();
						if (Blocked == 0)
						{
							GH.Lcd.dLcdFillRect(GH.UiInstance.pLcd.Lcd, Color, X, Y, X1, Y1);
							GH.UiInstance.ScreenBusy = 1;
						}
					}
					break;

				case INVERSERECT:
					{
						X = (DATA16)GH.Lms.PrimParPointer();
						Y = (DATA16)GH.Lms.PrimParPointer();
						X1 = (DATA16)GH.Lms.PrimParPointer();
						Y1 = (DATA16)GH.Lms.PrimParPointer();
						if (Blocked == 0)
						{
							GH.Lcd.dLcdInverseRect(GH.UiInstance.pLcd.Lcd, X, Y, X1, Y1);
							GH.UiInstance.ScreenBusy = 1;
						}
					}
					break;

				case RECT:
					{
						Color = (DATA8)GH.Lms.PrimParPointer();
						X = (DATA16)GH.Lms.PrimParPointer();
						Y = (DATA16)GH.Lms.PrimParPointer();
						X1 = (DATA16)GH.Lms.PrimParPointer();
						Y1 = (DATA16)GH.Lms.PrimParPointer();
						if (Blocked == 0)
						{
							GH.Lcd.dLcdRect(GH.UiInstance.pLcd.Lcd, Color, X, Y, X1, Y1);
							GH.UiInstance.ScreenBusy = 1;
						}
					}
					break;

				case PIXEL:
					{
						Color = (DATA8)GH.Lms.PrimParPointer();
						X = (DATA16)GH.Lms.PrimParPointer();
						Y = (DATA16)GH.Lms.PrimParPointer();
						if (Blocked == 0)
						{
							GH.Lcd.dLcdDrawPixel(GH.UiInstance.pLcd.Lcd, Color, X, Y);
							GH.UiInstance.ScreenBusy = 1;
						}
					}
					break;

				case LINE:
					{
						Color = (DATA8)GH.Lms.PrimParPointer();
						X = (DATA16)GH.Lms.PrimParPointer();
						Y = (DATA16)GH.Lms.PrimParPointer();
						X1 = (DATA16)GH.Lms.PrimParPointer();
						Y1 = (DATA16)GH.Lms.PrimParPointer();
						if (Blocked == 0)
						{
							GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, Color, X, Y, X1, Y1);
							GH.UiInstance.ScreenBusy = 1;
						}
					}
					break;

				case DOTLINE:
					{
						Color = (DATA8)GH.Lms.PrimParPointer();
						X = (DATA16)GH.Lms.PrimParPointer();
						Y = (DATA16)GH.Lms.PrimParPointer();
						X1 = (DATA16)GH.Lms.PrimParPointer();
						Y1 = (DATA16)GH.Lms.PrimParPointer();
						On = (DATA16)GH.Lms.PrimParPointer();
						Off = (DATA16)GH.Lms.PrimParPointer();
						if (Blocked == 0)
						{
							GH.Lcd.dLcdDrawDotLine(GH.UiInstance.pLcd.Lcd, Color, X, Y, X1, Y1, On, Off);
							GH.UiInstance.ScreenBusy = 1;
						}
					}
					break;

				case CIRCLE:
					{
						Color = (DATA8)GH.Lms.PrimParPointer();
						X = (DATA16)GH.Lms.PrimParPointer();
						Y = (DATA16)GH.Lms.PrimParPointer();
						R = (DATA16)GH.Lms.PrimParPointer();
						if (R != 0)
						{
							if (Blocked == 0)
							{
								GH.Lcd.dLcdDrawCircle(GH.UiInstance.pLcd.Lcd, Color, X, Y, R);
								GH.UiInstance.ScreenBusy = 1;
							}
						}
					}
					break;

				case FILLCIRCLE:
					{
						Color = (DATA8)GH.Lms.PrimParPointer();
						X = (DATA16)GH.Lms.PrimParPointer();
						Y = (DATA16)GH.Lms.PrimParPointer();
						R = (DATA16)GH.Lms.PrimParPointer();
						if (R != 0)
						{
							if (Blocked == 0)
							{
								GH.Lcd.dLcdDrawFilledCircle(GH.UiInstance.pLcd.Lcd, Color, X, Y, R);
								GH.UiInstance.ScreenBusy = 1;
							}
						}
					}
					break;

				case TEXT:
					{
						Color = (DATA8)GH.Lms.PrimParPointer();
						X = (DATA16)GH.Lms.PrimParPointer();
						Y = (DATA16)GH.Lms.PrimParPointer();
						pText = (DATA8[])GH.Lms.PrimParPointer();
						if (Blocked == 0)
						{
							GH.Lcd.dLcdDrawText(GH.UiInstance.pLcd.Lcd, Color, X, Y, GH.UiInstance.Font, pText);
							GH.UiInstance.ScreenBusy = 1;
						}
					}
					break;

				case ICON:
					{
						Color = (DATA8)GH.Lms.PrimParPointer();
						X = (DATA16)GH.Lms.PrimParPointer();
						Y = (DATA16)GH.Lms.PrimParPointer();
						Type = (DATA8)GH.Lms.PrimParPointer();
						No = (DATA8)GH.Lms.PrimParPointer();
						if (Blocked == 0)
						{
							GH.Lcd.dLcdDrawIcon(GH.UiInstance.pLcd.Lcd, Color, X, Y, Type, No);
							GH.UiInstance.ScreenBusy = 1;
						}
					}
					break;

				case BMPFILE:
					{
						Color = (DATA8)GH.Lms.PrimParPointer();
						X = (DATA16)GH.Lms.PrimParPointer();
						Y = (DATA16)GH.Lms.PrimParPointer();
						pText = (DATA8[])GH.Lms.PrimParPointer();

						if (Blocked == 0)
						{
							if (GH.Memory.cMemoryGetImage(pText, LCD_BUFFER_SIZE, pBmp) == OK)
							{
								GH.Lcd.dLcdDrawBitmap(GH.UiInstance.pLcd.Lcd, Color, X, Y, pBmp);
								GH.UiInstance.ScreenBusy = 1;
							}
						}
					}
					break;

				case PICTURE:
					{
						Color = (DATA8)GH.Lms.PrimParPointer();
						X = (DATA16)GH.Lms.PrimParPointer();
						Y = (DATA16)GH.Lms.PrimParPointer();
						pI = (IP)GH.Lms.PrimParPointer();
						if (pI != null)
						{
							if (Blocked == 0)
							{
								GH.Lcd.dLcdDrawBitmap(GH.UiInstance.pLcd.Lcd, Color, X, Y, pI);
								GH.UiInstance.ScreenBusy = 1;
							}
						}
					}
					break;

				case VALUE:
					{
						Color = (DATA8)GH.Lms.PrimParPointer();
						X = (DATA16)GH.Lms.PrimParPointer();
						Y = (DATA16)GH.Lms.PrimParPointer();
						DataF = (DATAF)GH.Lms.PrimParPointer();
						Figures = (DATA8)GH.Lms.PrimParPointer();
						Decimals = (DATA8)GH.Lms.PrimParPointer();

						if (float.IsNaN(DataF))
						{
							for (Lng = 0; Lng < Figures; Lng++)
							{
								GBuffer[Lng] = (sbyte)'-';
							}
						}
						else
						{
							// TODO: i don't want to do it now
							//if (Figures < 0)
							//{
							//	Figures = (sbyte)(0 - Figures);
							//	CommonHelper.Snprintf(GBuffer, 0, 24, new sbyte[] { Decimals }, DataF);
							//}
							//else
							//{
							//	snprintf((char*)GBuffer, 24, "%*.*f", Figures, Decimals, DataF);
							//}
							GH.Ev3System.Logger.LogWarning($"Tried to use unimplemented shite in: {System.Environment.StackTrace}");
							if (GBuffer[0] == '-')
							{ // Negative

								Figures++;
							}
						}
						GBuffer[Figures] = 0;
						pText = GBuffer;
						if (Blocked == 0)
						{
							GH.Lcd.dLcdDrawText(GH.UiInstance.pLcd.Lcd, Color, X, Y, GH.UiInstance.Font, pText);
							GH.UiInstance.ScreenBusy = 1;
						}
					}
					break;

				case VIEW_VALUE:
					{
						Color = (DATA8)GH.Lms.PrimParPointer();
						X = (DATA16)GH.Lms.PrimParPointer();
						Y = (DATA16)GH.Lms.PrimParPointer();
						DataF = (DATAF)GH.Lms.PrimParPointer();
						Figures = (DATA8)GH.Lms.PrimParPointer();
						Decimals = (DATA8)GH.Lms.PrimParPointer();
						if (Blocked == 0)
						{

							TmpColor = Color;
							CharWidth = GH.Lcd.dLcdGetFontWidth(GH.UiInstance.Font);
							CharHeight = GH.Lcd.dLcdGetFontHeight(GH.UiInstance.Font);
							X1 = (short)(((CharWidth + 2) / 3) - 1);
							Y1 = ((short)(CharHeight / 2));

							// TODO: I don't want to do it now
							//Lng = (DATA8)snprintf((char*)GBuffer, 24, "%.*f", Decimals, DataF);
							GH.Ev3System.Logger.LogWarning($"Tried to use unimplemented shite in: {System.Environment.StackTrace}");

							if (Lng != 0)
							{
								if (GBuffer[0] == '-')
								{ // Negative

									TmpColor = Color;
									Lng--;
									pText = GBuffer.Skip(1).ToArray();
								}
								else
								{ // Positive

									TmpColor = (sbyte)(1 - Color);
									pText = GBuffer;
								}

								// Make sure negative sign is deleted from last time
								GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, (sbyte)(1 - Color), (short)(X - X1), (short)(Y + Y1), (short)(X + (Figures * CharWidth)), (short)(Y + Y1));
								if (CharHeight > 12)
								{
									GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, (sbyte)(1 - Color), (short)(X - X1), (short)(Y + Y1 - 1), (short)(X + (Figures * CharWidth)), (short)(Y + Y1 - 1));
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
									GH.Lcd.dLcdDrawText(GH.UiInstance.pLcd.Lcd, Color, X, Y, GH.UiInstance.Font, GBuffer);
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
										Lng = (sbyte)(DATA16)Figures;
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
									GH.Lcd.dLcdDrawText(GH.UiInstance.pLcd.Lcd, Color, (short)(X + Tmp), Y, GH.UiInstance.Font, pText);

									// Draw negative sign
									GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, TmpColor, (short)(X - X1 + Tmp), (short)(Y + Y1), (short)(X + Tmp), (short)(Y + Y1));
									if (CharHeight > 12)
									{
										GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, TmpColor, (short)(X - X1 + Tmp), (short)(Y + Y1 - 1), (short)(X + Tmp), (short)(Y + Y1 - 1));
									}
								}
							}
							GH.UiInstance.ScreenBusy = 1;
						}
					}
					break;

				case VIEW_UNIT:
					{
						Color = (DATA8)GH.Lms.PrimParPointer();
						X = (DATA16)GH.Lms.PrimParPointer();
						Y = (DATA16)GH.Lms.PrimParPointer();
						DataF = (DATAF)GH.Lms.PrimParPointer();
						Figures = (DATA8)GH.Lms.PrimParPointer();
						Decimals = (DATA8)GH.Lms.PrimParPointer();
						Length = (DATA8)GH.Lms.PrimParPointer();
						pUnit = (DATA8[])GH.Lms.PrimParPointer();

						if (Blocked == 0)
						{
							TmpColor = Color;
							CharWidth = GH.Lcd.dLcdGetFontWidth(LARGE_FONT);
							CharHeight = GH.Lcd.dLcdGetFontHeight(LARGE_FONT);
							X1 = (short)(((CharWidth + 2) / 3) - 1);
							Y1 = ((short)(CharHeight / 2));

							// TODO: I don't want to do it now
							//Lng = (DATA8)snprintf((char*)GBuffer, 24, "%.*f", Decimals, DataF);
							GH.Ev3System.Logger.LogWarning($"Tried to use unimplemented shite in: {System.Environment.StackTrace}");

							if (Lng != 0)
							{
								if (GBuffer[0] == '-')
								{ // Negative

									TmpColor = Color;
									Lng--;
									pText = GBuffer.Skip(1).ToArray();
								}
								else
								{ // Positive

									TmpColor = (sbyte)(1 - Color);
									pText = GBuffer;
								}

								// Make sure negative sign is deleted from last time
								GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, (sbyte)(1 - Color), (short)(X - X1), (short)(Y + Y1), (short)(X + (Figures * CharWidth)), (short)(Y + Y1));
								if (CharHeight > 12)
								{
									GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, (sbyte)(1 - Color), (short)(X - X1), (short)(Y + Y1 - 1), (short)(X + (Figures * CharWidth)), (short)(Y + Y1 - 1));
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
									GH.Lcd.dLcdDrawText(GH.UiInstance.pLcd.Lcd, Color, X, Y, LARGE_FONT, GBuffer);
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
										Lng = (sbyte)(DATA16)Figures;
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
									GH.Lcd.dLcdDrawText(GH.UiInstance.pLcd.Lcd, Color, (short)(X + Tmp), Y, LARGE_FONT, pText);

									// Draw negative sign
									GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, TmpColor, (short)(X - X1 + Tmp), (short)(Y + Y1), (short)(X + Tmp), (short)(Y + Y1));
									if (CharHeight > 12)
									{
										GH.Lcd.dLcdDrawLine(GH.UiInstance.pLcd.Lcd, TmpColor, (short)(X - X1 + Tmp), (short)(Y + Y1 - 1), (short)(X + Tmp), (short)(Y + Y1 - 1));
									}

									Tmp = (short)(((((DATA16)Lng))) * CharWidth);
									CommonHelper.Snprintf(GBuffer, 0, Length, pUnit);
									GH.Lcd.dLcdDrawText(GH.UiInstance.pLcd.Lcd, Color, (short)(X + Tmp), Y, SMALL_FONT, GBuffer);

								}
							}
							GH.UiInstance.ScreenBusy = 1;
						}
					}
					break;

				case NOTIFICATION:
					{
						Color = (DATA8)GH.Lms.PrimParPointer();
						X = (DATA16)GH.Lms.PrimParPointer();  // start x
						Y = (DATA16)GH.Lms.PrimParPointer();  // start y
						Icon1 = (DATA8)GH.Lms.PrimParPointer();
						Icon2 = (DATA8)GH.Lms.PrimParPointer();
						Icon3 = (DATA8)GH.Lms.PrimParPointer();
						pText = (DATA8[])GH.Lms.PrimParPointer();
						pState = (DATA8)GH.Lms.PrimParPointer();

						if (Blocked == 0)
						{
							if (cUiNotification(Color, X, Y, Icon1, Icon2, Icon3, pText, ref pState) == BUSY)
							{
								GH.Lms.SetObjectIp(TmpIp);
								GH.Lms.SetObjectIpInd(TmpIpInd - 1);
								GH.Lms.SetDispatchStatus(BUSYBREAK);
							}
						}
						else
						{
							GH.Lms.SetObjectIp(TmpIp);
							GH.Lms.SetObjectIpInd(TmpIpInd - 1);
							GH.Lms.SetDispatchStatus(BUSYBREAK);
						}
					}
					break;

				case QUESTION:
					{
						Color = (DATA8)GH.Lms.PrimParPointer();
						X = (DATA16)GH.Lms.PrimParPointer();  // start x
						Y = (DATA16)GH.Lms.PrimParPointer();  // start y
						Icon1 = (DATA8)GH.Lms.PrimParPointer();
						Icon2 = (DATA8)GH.Lms.PrimParPointer();
						pText = (DATA8[])GH.Lms.PrimParPointer();
						pState = (DATA8)GH.Lms.PrimParPointer();
						pAnswer = (DATA8[])GH.Lms.PrimParPointer();

						if (Blocked == 0)
						{
							if (cUiQuestion(Color, X, Y, Icon1, Icon2, pText, ref pState, ref pAnswer[0]) == BUSY)
							{
								GH.Lms.SetObjectIp(TmpIp);
								GH.Lms.SetObjectIpInd(TmpIpInd - 1);
								GH.Lms.SetDispatchStatus(BUSYBREAK);
							}
						}
						else
						{
							GH.Lms.SetObjectIp(TmpIp);
							GH.Lms.SetObjectIpInd(TmpIpInd - 1);
							GH.Lms.SetDispatchStatus(BUSYBREAK);
						}
					}
					break;


				case ICON_QUESTION:
					{
						Color = (DATA8)GH.Lms.PrimParPointer();
						X = (DATA16)GH.Lms.PrimParPointer();  // start x
						Y = (DATA16)GH.Lms.PrimParPointer();  // start y
						pState = (DATA8)GH.Lms.PrimParPointer();
						pIcons = (DATA32)GH.Lms.PrimParPointer();

						if (Blocked == 0)
						{
							if (cUiIconQuestion(Color, X, Y, ref pState, ref pIcons) == RESULT.BUSY)
							{
								GH.Lms.SetObjectIp(TmpIp);
								GH.Lms.SetObjectIpInd(TmpIpInd - 1);
								GH.Lms.SetDispatchStatus(BUSYBREAK);
							}
						}
						else
						{
							GH.Lms.SetObjectIp(TmpIp);
							GH.Lms.SetObjectIpInd(TmpIpInd - 1);
							GH.Lms.SetDispatchStatus(BUSYBREAK);
						}
					}
					break;


				case KEYBOARD:
					{
						Color = (DATA8)GH.Lms.PrimParPointer();
						X = (DATA16)GH.Lms.PrimParPointer();  // start x
						Y = (DATA16)GH.Lms.PrimParPointer();  // start y
						No = (DATA8)GH.Lms.PrimParPointer();   // Icon
						Lng = (DATA8)GH.Lms.PrimParPointer();   // length
						pText = (DATA8[])GH.Lms.PrimParPointer();    // default
						pCharSet = (DATA8)GH.Lms.PrimParPointer();    // valid char set
						pAnswer = (DATA8[])GH.Lms.PrimParPointer();    // string

						if (GH.VMInstance.Handle >= 0)
						{
							pAnswer = CommonHelper.CastObjectArray<DATA8>(GH.Lms.VmMemoryResize(GH.VMInstance.Handle, (DATA32)Lng));
						}

						if (Blocked == 0)
						{
							SelectedChar = cUiKeyboard(Color, X, Y, No, Lng, pText, ref pCharSet, pAnswer);

							// Wait for "ENTER"
							if (SelectedChar != 0x0D)
							{
								GH.Lms.SetObjectIp(TmpIp);
								GH.Lms.SetObjectIpInd(TmpIpInd - 1);
								GH.Lms.SetDispatchStatus(BUSYBREAK);
							}
						}
						else
						{
							GH.Lms.SetObjectIp(TmpIp);
							GH.Lms.SetObjectIpInd(TmpIpInd - 1);
							GH.Lms.SetDispatchStatus(BUSYBREAK);
						}
					}
					break;

				case BROWSE:
					{
						Type = (DATA8)GH.Lms.PrimParPointer();   // Browser type
						X = (DATA16)GH.Lms.PrimParPointer();  // start x
						Y = (DATA16)GH.Lms.PrimParPointer();  // start y
						X1 = (DATA16)GH.Lms.PrimParPointer();  // size x
						Y1 = (DATA16)GH.Lms.PrimParPointer();  // size y
						Lng = (DATA8)GH.Lms.PrimParPointer();   // length
						pType = (DATA8)GH.Lms.PrimParPointer();    // item type
						pAnswer = (DATA8[])GH.Lms.PrimParPointer();    // item name

						if (GH.VMInstance.Handle >= 0)
						{
							pAnswer = CommonHelper.CastObjectArray<DATA8>(GH.Lms.VmMemoryResize(GH.VMInstance.Handle, (DATA32)Lng));
						}

						if (Blocked == 0)
						{
							if (cUiBrowser(Type, X, Y, X1, Y1, Lng, ref pType, pAnswer) == RESULT.BUSY)
							{
								GH.Lms.SetObjectIp(TmpIp);
								GH.Lms.SetObjectIpInd(TmpIpInd - 1);
								GH.Lms.SetDispatchStatus(BUSYBREAK);
							}
						}
						else
						{
							GH.Lms.SetObjectIp(TmpIp);
							GH.Lms.SetObjectIpInd(TmpIpInd - 1);
							GH.Lms.SetDispatchStatus(BUSYBREAK);
						}
					}
					break;

				case VERTBAR:
					{
						Color = (DATA8)GH.Lms.PrimParPointer();
						X = (DATA16)GH.Lms.PrimParPointer();  // start x
						Y = (DATA16)GH.Lms.PrimParPointer();  // start y
						X1 = (DATA16)GH.Lms.PrimParPointer();  // size x
						Y1 = (DATA16)GH.Lms.PrimParPointer();  // size y
						Min = (DATA16)GH.Lms.PrimParPointer();  // min
						Max = (DATA16)GH.Lms.PrimParPointer();  // max
						Act = (DATA16)GH.Lms.PrimParPointer();  // actual

						if (Blocked == 0)
						{
							cUiDrawBar(Color, X, Y, X1, Y1, Min, Max, Act);
						}
					}
					break;

				case SELECT_FONT:
					{
						GH.UiInstance.Font = (DATA8)GH.Lms.PrimParPointer();
						if (Blocked == 0)
						{
							if (GH.UiInstance.Font >= FONTTYPES)
							{
								GH.UiInstance.Font = (FONTTYPES - 1);
							}
							if (GH.UiInstance.Font < 0)
							{
								GH.UiInstance.Font = 0;
							}
						}
					}
					break;

				case TOPLINE:
					{
						GH.UiInstance.TopLineEnabled = (DATA8)GH.Lms.PrimParPointer();
					}
					break;

				case FILLWINDOW:
					{
						Color = (DATA8)GH.Lms.PrimParPointer();
						Y = (DATA16)GH.Lms.PrimParPointer();  // start y
						Y1 = (DATA16)GH.Lms.PrimParPointer();  // size y
						if (Blocked == 0)
						{
							GH.UiInstance.Font = NORMAL_FONT;

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
									CommonHelper.Memset(GH.UiInstance.pLcd.Lcd, (byte)Color, Y1, Y);
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
										CommonHelper.Memset(GH.UiInstance.pLcd.Lcd, (byte)Color, Y2, Y3);
										Color = (sbyte)~Color;
									}
								}
							}

							GH.UiInstance.ScreenBusy = 1;
						}
					}
					break;

				case STORE:
					{
						No = (DATA8)GH.Lms.PrimParPointer();
						if (Blocked == 0)
						{
							if (No < LCD_STORE_LEVELS)
							{
								Array.Copy(GH.UiInstance.LcdSafe.Lcd, GH.UiInstance.LcdPool[No].Lcd, LCD.LcdSizeof);
							}
						}
					}
					break;

				case RESTORE:
					{
						No = (DATA8)GH.Lms.PrimParPointer();
						if (Blocked == 0)
						{
							if (No < LCD_STORE_LEVELS)
							{
								Array.Copy(GH.UiInstance.LcdPool[No].Lcd, GH.UiInstance.LcdSafe.Lcd, LCD.LcdSizeof);
								GH.UiInstance.ScreenBusy = 1;
							}
						}
					}
					break;

				case GRAPH_SETUP:
					{
						X = (DATA16)GH.Lms.PrimParPointer();  // start x
						X1 = (DATA16)GH.Lms.PrimParPointer();  // size y
						Items = (DATA8)GH.Lms.PrimParPointer();   // items
						pOffset = (DATA16[])GH.Lms.PrimParPointer();  // handle to offset Y
						pSpan = (DATA16[])GH.Lms.PrimParPointer();  // handle to span y
						pMin = (DATAF[])GH.Lms.PrimParPointer();  // handle to min
						pMax = (DATAF[])GH.Lms.PrimParPointer();  // handle to max
						pVal = (DATAF[])GH.Lms.PrimParPointer();  // handle to val

						if (Blocked == 0)
						{
							cUiGraphSetup(X, X1, Items, pOffset, pSpan, pMin, pMax, pVal);
						}
					}
					break;

				case GRAPH_DRAW:
					{
						View = (DATA8)GH.Lms.PrimParPointer();   // view

						cUiGraphDraw(View, ref Actual, ref Lowest, ref Highest, ref Average);

						GH.Lms.PrimParPointer(Actual);
						GH.Lms.PrimParPointer(Lowest);
						GH.Lms.PrimParPointer(Highest);
						GH.Lms.PrimParPointer(Average);
					}
					break;

				case SCROLL:
					{
						Y = (DATA16)GH.Lms.PrimParPointer();
						if ((Y > 0) && (Y < LCD_HEIGHT))
						{
							GH.Lcd.dLcdScroll(GH.UiInstance.pLcd.Lcd, Y);
						}
					}
					break;

				case POPUP:
					{
						Open = (DATA8)GH.Lms.PrimParPointer();
						if (Blocked == 0)
						{
							if (Open != 0)
							{
								if (GH.UiInstance.ScreenBusy == 0)
								{
									TmpObjId = GH.Lms.CallingObjectId();

									Array.Copy(GH.UiInstance.LcdSafe.Lcd, GH.UiInstance.LcdSave.Lcd, LCD.LcdSizeof);
									GH.UiInstance.ScreenPrgId = TmpPrgId;
									GH.UiInstance.ScreenObjId = TmpObjId;
									GH.UiInstance.ScreenBlocked = 1;
								}
								else
								{ // Wait on scrreen

									GH.Lms.SetObjectIp(TmpIp);
									GH.Lms.SetObjectIpInd(TmpIpInd - 1);
									GH.Lms.SetDispatchStatus(BUSYBREAK);
								}
							}
							else
							{
								Array.Copy(GH.UiInstance.LcdSave.Lcd, GH.UiInstance.LcdSafe.Lcd, LCD.LcdSizeof);
								GH.Lcd.dLcdUpdate(GH.UiInstance.pLcd);

								GH.UiInstance.ScreenPrgId = ushort.MaxValue;
								GH.UiInstance.ScreenObjId = ushort.MaxValue;
								GH.UiInstance.ScreenBlocked = 0;
							}
						}
						else
						{ // Wait on not blocked

							GH.Lms.SetObjectIp(TmpIp);
							GH.Lms.SetObjectIpInd(TmpIpInd - 1);
							GH.Lms.SetDispatchStatus(BUSYBREAK);
						}
					}
					break;

			}
		}

		public void cUiFlush()
		{
			cUiFlushBuffer();
		}

		public void cUiRead()
		{
			IP TmpIp;
			int TmpIpInd;
			DATA8 Cmd;
			DATA8 Lng;
			DATA8 Data8 = 0;
			DATA8[] pSource;
			DATA8[] pDestination;
			DATA16 Data16;
			IMGDATA Tmp;
			IMGHEAD pImage;
			DATA32 Length = 0;
			DATA32 Total = 0;
			DATA32 Size = 0;
			IMGHEAD pImgHead;
			OBJHEAD pObjHead;


			TmpIp = GH.Lms.GetObjectIp();
			TmpIpInd = GH.Lms.GetObjectIpInd();
			Cmd = (DATA8)GH.Lms.PrimParPointer();

			switch (Cmd)
			{ // Function

				case GET_STRING:
					{
						if (GH.UiInstance.Keys != 0)
						{
							Lng = (DATA8)GH.Lms.PrimParPointer();
							pDestination = (DATA8[])GH.Lms.PrimParPointer();
							pSource = (DATA8[])GH.UiInstance.KeyBuffer;

							int pInd = 0;
							while ((GH.UiInstance.Keys != 0) && (Lng != 0))
							{
								pDestination[pInd] = pSource[pInd];
								pInd++;
								GH.UiInstance.Keys--;
								Lng--;
							}
							pDestination = null;
							GH.UiInstance.KeyBufIn = 0;
						}
						else
						{
							GH.Lms.SetObjectIp(TmpIp);
							GH.Lms.SetObjectIpInd(TmpIpInd - 1);
							GH.Lms.SetDispatchStatus(BUSYBREAK);
						}
					}
					break;

				case KEY:
					{
						if (GH.UiInstance.KeyBufIn != 0)
						{
							GH.Lms.PrimParPointer((DATA8)GH.UiInstance.KeyBuffer[0]);
							GH.UiInstance.KeyBufIn--;

							for (Lng = 0; Lng < GH.UiInstance.KeyBufIn; Lng++)
							{
								GH.UiInstance.KeyBuffer[Lng] = GH.UiInstance.KeyBuffer[Lng + 1];
							}
						}
						else
						{
							GH.Lms.PrimParPointer((DATA8)0);
						}
					}
					break;

				case GET_SHUTDOWN:
					{
						GH.Lms.PrimParPointer(GH.UiInstance.ShutDown);
						GH.UiInstance.ShutDown = 0;
					}
					break;

				case GET_WARNING:
					{
						GH.Lms.PrimParPointer(GH.UiInstance.Warning);
					}
					break;

				case GET_LBATT:
					{
						Data16 = (DATA16)(GH.UiInstance.Vbatt * 1000.0);
						Data16 -= GH.UiInstance.BattIndicatorLow;
						Data16 = (short)((Data16 * 100) / (GH.UiInstance.BattIndicatorHigh - GH.UiInstance.BattIndicatorLow));
						if (Data16 > 100)
						{
							Data16 = 100;
						}
						if (Data16 < 0)
						{
							Data16 = 0;
						}
						GH.Lms.PrimParPointer((DATA8)Data16);
					}
					break;

				case ADDRESS:
					{
						if (GH.UiInstance.Keys != 0)
						{
							GH.Lms.PrimParPointer((DATA32)int.Parse(GH.UiInstance.KeyBuffer.AsString()));
							GH.UiInstance.Keys = 0;
						}
						else
						{
							GH.Lms.SetObjectIp(TmpIp);
							GH.Lms.SetObjectIpInd(TmpIpInd - 1);
							GH.Lms.SetDispatchStatus(BUSYBREAK);
						}
					}
					break;

				case CODE:
					{
						if (GH.UiInstance.Keys != 0)
						{
							Length = (DATA32)GH.Lms.PrimParPointer();
							pImage = (IMGHEAD)GH.Lms.PrimParPointer();

							// TODO: WARNING: code shite changes
							pImgHead = pImage;
							pObjHead = (OBJHEAD)GH.Lms.PrimParPointer();
							pDestination = (DATA8[])GH.Lms.PrimParPointer();

							if (Length > (IMGHEAD.SizeOf + OBJHEAD.SizeOf))
							{

								pImgHead.Sign[0] = (byte)'l';
								pImgHead.Sign[1] = (byte)'e';
								pImgHead.Sign[2] = (byte)'g';
								pImgHead.Sign[3] = (byte)'o';
								pImgHead.ImageSize = 0;
								pImgHead.VersionInfo = (UWORD)(VERS * 100.0);
								pImgHead.NumberOfObjects = 1;
								pImgHead.GlobalBytes = 0;

								pObjHead.OffsetToInstructions = new byte[] { IMGHEAD.SizeOf + OBJHEAD.SizeOf };
								pObjHead.OwnerObjectId = 0;
								pObjHead.TriggerCount = 0;
								pObjHead.LocalBytes = MAX_COMMAND_LOCALS;

								pSource = (DATA8[])GH.UiInstance.KeyBuffer;
								Size = IMGHEAD.SizeOf + OBJHEAD.SizeOf;

								Length -= IMGHEAD.SizeOf + OBJHEAD.SizeOf;
								Length--;
								int pSourceInd = 0;
								int pDestInd = 0;
								while ((GH.UiInstance.Keys != 0) && (Length != 0))
								{
									Tmp = (IMGDATA)(AtoN(pSource[pSourceInd]) << 4);
									pSourceInd++;
									GH.UiInstance.Keys--;
									if (GH.UiInstance.Keys != 0)
									{
										Tmp += (IMGDATA)(AtoN(pSource[pSourceInd]));
										pSourceInd++;
										GH.UiInstance.Keys--;
									}
									else
									{
										Tmp = 0;
									}
									pDestination[pDestInd] = (sbyte)Tmp;
									pDestInd++;
									Length--;
									Size++;
								}
								pDestination[pDestInd] = opOBJECT_END;
								Size++;
								pImgHead.ImageSize = (uint)Size;
								CommonHelper.Memset(GH.UiInstance.Globals, 0, GH.UiInstance.Globals.Length);

								GH.Lms.PrimParPointer(GH.UiInstance.Globals);
								GH.Lms.PrimParPointer(1);
							}
						}
						else
						{
							GH.Lms.SetObjectIp(TmpIp);
							GH.Lms.SetObjectIpInd(TmpIpInd - 1);
							GH.Lms.SetDispatchStatus(BUSYBREAK);
						}
					}
					break;

				case GET_HW_VERS:
					{
						Lng = (DATA8)GH.Lms.PrimParPointer();
						pDestination = (DATA8[])GH.Lms.PrimParPointer();

						if (GH.VMInstance.Handle >= 0)
						{
							Data8 = (DATA8)(GH.UiInstance.HwVers.Length + 1);
							if ((Lng > Data8) || (Lng == -1))
							{
								Lng = Data8;
							}
							pDestination = CommonHelper.CastObjectArray<DATA8>(GH.Lms.VmMemoryResize(GH.VMInstance.Handle, (DATA32)Lng));
						}
						if (pDestination != null)
						{
							CommonHelper.Snprintf(pDestination, 0, Lng, CommonHelper.CastArray<char, sbyte>(GH.UiInstance.HwVers));
						}
					}
					break;

				case GET_FW_VERS:
					{
						Lng = (DATA8)GH.Lms.PrimParPointer();
						pDestination = (DATA8[])GH.Lms.PrimParPointer();

						if (GH.VMInstance.Handle >= 0)
						{
							Data8 = (DATA8)(GH.UiInstance.FwVers.Length + 1);
							if ((Lng > Data8) || (Lng == -1))
							{
								Lng = Data8;
							}
							pDestination = CommonHelper.CastObjectArray<DATA8>(GH.Lms.VmMemoryResize(GH.VMInstance.Handle, (DATA32)Lng));
						}
						if (pDestination != null)
						{
							CommonHelper.Snprintf(pDestination, 0, Lng, CommonHelper.CastArray<char, sbyte>(GH.UiInstance.FwVers));
						}
					}
					break;

				case GET_FW_BUILD:
					{
						Lng = (DATA8)GH.Lms.PrimParPointer();
						pDestination = (DATA8[])GH.Lms.PrimParPointer();

						if (GH.VMInstance.Handle >= 0)
						{
							Data8 = (DATA8)(GH.UiInstance.FwBuild.Length + 1);
							if ((Lng > Data8) || (Lng == -1))
							{
								Lng = Data8;
							}
							pDestination = CommonHelper.CastObjectArray<DATA8>(GH.Lms.VmMemoryResize(GH.VMInstance.Handle, (DATA32)Lng));
						}
						if (pDestination != null)
						{
							CommonHelper.Snprintf(pDestination,0, Lng, CommonHelper.CastArray<char, sbyte>(GH.UiInstance.FwBuild));
						}
					}
					break;

				case GET_OS_VERS:
					{
						Lng = (DATA8)GH.Lms.PrimParPointer();
						pDestination = (DATA8[])GH.Lms.PrimParPointer();

						if (GH.VMInstance.Handle >= 0)
						{
							Data8 = (DATA8)(GH.UiInstance.OsVers.Length + 1);
							if ((Lng > Data8) || (Lng == -1))
							{
								Lng = Data8;
							}
							pDestination = CommonHelper.CastObjectArray<DATA8>(GH.Lms.VmMemoryResize(GH.VMInstance.Handle, (DATA32)Lng));
						}
						if (pDestination != null)
						{
							CommonHelper.Snprintf(pDestination, 0, Lng, CommonHelper.CastArray<char, sbyte>(GH.UiInstance.OsVers));
						}
					}
					break;

				case GET_OS_BUILD:
					{
						Lng = (DATA8)GH.Lms.PrimParPointer();
						pDestination = (DATA8[])GH.Lms.PrimParPointer();

						if (GH.VMInstance.Handle >= 0)
						{
							Data8 = (DATA8)(GH.UiInstance.OsBuild.Length + 1);
							if ((Lng > Data8) || (Lng == -1))
							{
								Lng = Data8;
							}
							pDestination = CommonHelper.CastObjectArray<DATA8>(GH.Lms.VmMemoryResize(GH.VMInstance.Handle, (DATA32)Lng));
						}
						if (pDestination != null)
						{
							CommonHelper.Snprintf(pDestination, 0, Lng, CommonHelper.CastArray<char, sbyte>(GH.UiInstance.OsBuild));
						}
					}
					break;

				case GET_VERSION:
					{
						// TODO: I don't want to do it now
						//CommonHelper.Snprintf(GH.UiInstance.ImageBuffer, 0, IMAGEBUFFER_SIZE, "%s V%4.2f%c(%s %s)", PROJECT, VERS, SPECIALVERS, __DATE__, __TIME__);
						GH.Ev3System.Logger.LogWarning($"Tried to use unimplemented shite in: {System.Environment.StackTrace}");
						Lng = (DATA8)GH.Lms.PrimParPointer();
						pDestination = (DATA8[])GH.Lms.PrimParPointer();
						pSource = CommonHelper.CastArray<byte, DATA8>(GH.UiInstance.ImageBuffer);

						if (GH.VMInstance.Handle >= 0)
						{
							Data8 = (DATA8)(GH.UiInstance.ImageBuffer.Length + 1);
							if ((Lng > Data8) || (Lng == -1))
							{
								Lng = Data8;
							}
							pDestination = CommonHelper.CastObjectArray<DATA8>(GH.Lms.VmMemoryResize(GH.VMInstance.Handle, (DATA32)Lng));
						}
						if (pDestination != null)
						{
							CommonHelper.Snprintf(pDestination,0, Lng, CommonHelper.CastArray<byte, sbyte>(GH.UiInstance.ImageBuffer));
						}
					}
					break;

				case GET_IP:
					{
						Lng = (DATA8)GH.Lms.PrimParPointer();
						pDestination = (DATA8[])GH.Lms.PrimParPointer();

						if (GH.VMInstance.Handle >= 0)
						{
							Data8 = IPADDR_SIZE;
							if ((Lng > Data8) || (Lng == -1))
							{
								Lng = Data8;
							}
							pDestination = CommonHelper.CastObjectArray<DATA8>(GH.Lms.VmMemoryResize(GH.VMInstance.Handle, (DATA32)Lng));
						}
						if (pDestination != null)
						{
							CommonHelper.Snprintf(pDestination,0, Lng, CommonHelper.CastArray<char, sbyte>(GH.UiInstance.IpAddr));
						}

					}
					break;

				case GET_POWER:
					{
						GH.Lms.PrimParPointer(GH.UiInstance.Vbatt);
						GH.Lms.PrimParPointer(GH.UiInstance.Ibatt);
						GH.Lms.PrimParPointer(GH.UiInstance.Iintegrated);
						GH.Lms.PrimParPointer(GH.UiInstance.Imotor);
					}
					break;

				case GET_VBATT:
					{
						GH.Lms.PrimParPointer(GH.UiInstance.Vbatt);
					}
					break;

				case GET_IBATT:
					{
						GH.Lms.PrimParPointer(GH.UiInstance.Ibatt);
					}
					break;

				case GET_IINT:
					{
						GH.Lms.PrimParPointer(GH.UiInstance.Iintegrated);
					}
					break;

				case GET_IMOTOR:
					{
						GH.Lms.PrimParPointer(GH.UiInstance.Imotor);
					}
					break;

				case GET_EVENT:
					{
						GH.Lms.PrimParPointer(GH.UiInstance.Event);
						GH.UiInstance.Event = 0;
					}
					break;

				case GET_TBATT:
					{
						GH.Lms.PrimParPointer(GH.UiInstance.Tbatt);
					}
					break;

				case TEXTBOX_READ:
					{
						pSource = (DATA8[])GH.Lms.PrimParPointer();
						Size = (DATA32)GH.Lms.PrimParPointer();
						Data8 = (DATA8)GH.Lms.PrimParPointer();
						Lng = (DATA8)GH.Lms.PrimParPointer();
						Data16 = (DATA16)GH.Lms.PrimParPointer();
						pDestination = (DATA8[])GH.Lms.PrimParPointer();

						cUiTextboxReadLine(pSource, Size, Data8, Lng, Data16, pDestination, ref Data8);
					}
					break;

				case GET_SDCARD:
					{
						GH.Lms.PrimParPointer(GH.Lms.CheckSdcard(ref Data8, ref Total, ref Size, 0));
						GH.Lms.PrimParPointer(Total);
						GH.Lms.PrimParPointer(Size);
					}
					break;

				case GET_USBSTICK:
					{
						GH.Lms.PrimParPointer(GH.Lms.CheckUsbstick(ref Data8, ref Total, ref Size, 0));
						GH.Lms.PrimParPointer(Total);
						GH.Lms.PrimParPointer(Size);
					}
					break;

			}
		}

		public void cUiWrite()
		{
			int TmpIpInd;
			IP TmpIp;
			DATA8 Cmd;
			DATA8[] pSource;
			DSPSTAT DspStat = DSPSTAT.BUSYBREAK;
			DATA8[] Buffer = new DATA8[50];
			DATA8 Data8;
			DATA16 Data16;
			DATA32 Data32;
			DATA8[] pGlobal;
			DATA32 Tmp;
			DATAF DataF;
			DATA8 Figures;
			DATA8 Decimals;
			DATA8 No;
			DATA8[] pText;


			TmpIp = GH.Lms.GetObjectIp();
			TmpIpInd = GH.Lms.GetObjectIpInd();
			Cmd = (DATA8)GH.Lms.PrimParPointer();

			switch (Cmd)
			{ // Function

				case WRITE_FLUSH:
					{
						cUiFlush();
						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case FLOATVALUE:
					{
						DataF = (DATAF)GH.Lms.PrimParPointer();
						Figures = (DATA8)GH.Lms.PrimParPointer();
						Decimals = (DATA8)GH.Lms.PrimParPointer();


						// TODO: don't want to do this now
						// snprintf((char*)Buffer, 32, "%*.*f", Figures, Decimals, DataF);
						GH.Ev3System.Logger.LogWarning($"Tried to use unimplemented shite in: {System.Environment.StackTrace}");
						cUiWriteString(Buffer);

						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case STAMP:
					{ // write time, prgid, objid, ip

						pSource = (DATA8[])GH.Lms.PrimParPointer();
						// TODO: don't want to do this now
						// snprintf((char*)Buffer, 50, "####[ %09u %01u %03u %06u %s]####\r\n", GetTime(), CurrentProgramId(), CallingObjectId(), CurrentObjectIp(), pSource);
						GH.Ev3System.Logger.LogWarning($"Tried to use unimplemented shite in: {System.Environment.StackTrace}");
						cUiWriteString(Buffer);
						cUiFlush();
						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case PUT_STRING:
					{
						pSource = (DATA8[])GH.Lms.PrimParPointer();
						cUiWriteString(pSource);
						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case CODE:
					{
						pGlobal = (DATA8[])GH.Lms.PrimParPointer();
						Data32 = (DATA32)GH.Lms.PrimParPointer();

						pSource = pGlobal;

						cUiWriteString("\r\n    ");
						for (Tmp = 0; Tmp < Data32; Tmp++)
						{
							// TODO: don't want to do this now
							// snprintf((char*)Buffer, 7, "%02X ", pSource[Tmp] & 0xFF);
							GH.Ev3System.Logger.LogWarning($"Tried to use unimplemented shite in: {System.Environment.StackTrace}");
							cUiWriteString(Buffer);
							if (((Tmp & 0x3) == 0x3) && ((Tmp & 0xF) != 0xF))
							{
								cUiWriteString(" ");
							}
							if (((Tmp & 0xF) == 0xF) && (Tmp < (Data32 - 1)))
							{
								cUiWriteString("\r\n    ");
							}
						}
						cUiWriteString("\r\n");
						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case TEXTBOX_APPEND:
					{
						pText = (DATA8[])GH.Lms.PrimParPointer();
						Data32 = (DATA32)GH.Lms.PrimParPointer();
						Data8 = (DATA8)GH.Lms.PrimParPointer();
						pSource = (DATA8[])GH.Lms.PrimParPointer();

						cUiTextboxAppendLine(pText, Data32, Data8, ref pSource[0], GH.UiInstance.Font);

						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case SET_BUSY:
					{
						Data8 = (DATA8)GH.Lms.PrimParPointer();

						if (Data8 != 0)
						{
							unchecked { GH.UiInstance.Warning |= (sbyte)WARNING_BUSY; }
						}
						else
						{
							unchecked { GH.UiInstance.Warning &= (sbyte)~WARNING_BUSY; }
						}

						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case VALUE8:
					{
						Data8 = (DATA8)GH.Lms.PrimParPointer();
						if (Data8 != DATA8_NAN)
						{
							CommonHelper.Snprintf(Buffer, 0, 7, new sbyte[] { Data8 });
						}
						else
						{
							CommonHelper.Snprintf(Buffer, 0, 7, new sbyte[] { (sbyte)'n', (sbyte)'a', (sbyte)'n' });
						}
						cUiWriteString(Buffer);

						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case VALUE16:
					{
						Data16 = (DATA16)GH.Lms.PrimParPointer();
						if (Data16 != DATA16_NAN)
						{
							// TODO: don't want to do this now
							// snprintf((char*)Buffer, 9, "%d", Data16 & 0xFFFF);
							GH.Ev3System.Logger.LogWarning($"Tried to use unimplemented shite in: {System.Environment.StackTrace}");
						}
						else
						{
							CommonHelper.Snprintf(Buffer, 0, 7, new sbyte[] { (sbyte)'n', (sbyte)'a', (sbyte)'n' });
						}
						cUiWriteString(Buffer);

						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case VALUE32:
					{
						Data32 = (DATA32)GH.Lms.PrimParPointer();
						if (Data32 != DATA32_NAN)
						{
							// TODO: don't want to do this now
							// snprintf((char*)Buffer, 14, "%ld", (long unsigned int)(Data32 & 0xFFFFFFFF));
							GH.Ev3System.Logger.LogWarning($"Tried to use unimplemented shite in: {System.Environment.StackTrace}");
						}
						else
						{
							CommonHelper.Snprintf(Buffer, 0, 7, new sbyte[] { (sbyte)'n', (sbyte)'a', (sbyte)'n' });
						}

						cUiWriteString(Buffer);

						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case VALUEF:
					{
						DataF = (DATAF)GH.Lms.PrimParPointer();
						// TODO: don't want to do this now
						// snprintf((char*)Buffer, 24, "%f", DataF);
						GH.Ev3System.Logger.LogWarning($"Tried to use unimplemented shite in: {System.Environment.StackTrace}");
						cUiWriteString(Buffer);

						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case LED:
					{
						Data8 = (DATA8)GH.Lms.PrimParPointer();
						if (Data8 < 0)
						{
							Data8 = 0;
						}
						if (Data8 >= LEDPATTERNS)
						{
							Data8 = LEDPATTERNS - 1;
						}
						cUiSetLed(Data8);
						GH.UiInstance.RunLedEnabled = 0;

						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case POWER:
					{
						Data8 = (DATA8)GH.Lms.PrimParPointer();

						// TODO: don't want to do this now
						//if (GH.UiInstance.PowerFile >= 0)
						//{
						//	ioctl(UiInstance.PowerFile, 0, (size_t) & Data8);
						//}
						GH.Ev3System.Logger.LogWarning($"Tried to use unimplemented shite in: {System.Environment.StackTrace}");

						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case TERMINAL:
					{
						No = (DATA8)GH.Lms.PrimParPointer();

						if (No != 0)
						{
							GH.Lms.SetTerminalEnable(1);
						}
						else
						{
							GH.Lms.SetTerminalEnable(0);
						}

						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case SET_TESTPIN:
					{
						Data8 = (DATA8)GH.Lms.PrimParPointer();
						cUiTestpin(Data8);
						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case INIT_RUN:
					{
						GH.UiInstance.RunScreenEnabled = 3;

						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case UPDATE_RUN:
					{
						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case GRAPH_SAMPLE:
					{
						cUiGraphSample();
						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case DOWNLOAD_END:
					{
						GH.UiInstance.UiUpdate = 1;
						cUiDownloadSuccesSound();
						DspStat = DSPSTAT.NOBREAK;
					}
					break;

				case SCREEN_BLOCK:
					{
						GH.UiInstance.ScreenBlocked = (DATA8)GH.Lms.PrimParPointer();
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

				GH.Lms.SetObjectIp(TmpIp);
				GH.Lms.SetObjectIpInd(TmpIpInd - 1);
			}
			GH.Lms.SetDispatchStatus(DspStat);
		}

		public void cUiButton()
		{
			PRGID TmpPrgId;
			OBJID TmpObjId;
			IP TmpIp;
			int TmpIpInd;
			DATA8 Cmd;
			DATA8 Button;
			DATA8 State;
			DATA16 Inc;
			DATA8 Blocked;

			TmpIp = GH.Lms.GetObjectIp();
			TmpIpInd = GH.Lms.GetObjectIpInd();
			TmpPrgId = GH.Lms.CurrentProgramId();

			if (GH.UiInstance.ScreenBlocked == 0)
			{
				Blocked = 0;
			}
			else
			{
				TmpObjId = GH.Lms.CallingObjectId();
				if ((TmpPrgId == GH.UiInstance.ScreenPrgId) && (TmpObjId == GH.UiInstance.ScreenObjId))
				{
					Blocked = 0;
				}
				else
				{
					Blocked = 1;
				}
			}

			Cmd = (DATA8)GH.Lms.PrimParPointer();

			State = 0;
			Inc = 0;

			switch (Cmd)
			{ // Function

				case PRESS:
					{
						Button = (DATA8)GH.Lms.PrimParPointer();
						cUiSetPress(Button, 1);
					}
					break;

				case RELEASE:
					{
						Button = (DATA8)GH.Lms.PrimParPointer();
						cUiSetPress(Button, 0);
					}
					break;

				case SHORTPRESS:
					{
						Button = (DATA8)GH.Lms.PrimParPointer();

						if (Blocked == 0)
						{
							State = cUiGetShortPress(Button);
						}
						GH.Lms.PrimParPointer(State);
					}
					break;

				case GET_BUMBED:
					{
						Button = (DATA8)GH.Lms.PrimParPointer();

						if (Blocked == 0)
						{
							State = cUiGetBumbed(Button);
						}
						GH.Lms.PrimParPointer(State);
					}
					break;

				case PRESSED:
					{
						Button = (DATA8)GH.Lms.PrimParPointer();

						if (Blocked == 0)
						{
							State = cUiGetPress(Button);
						}
						GH.Lms.PrimParPointer(State);
					}
					break;

				case LONGPRESS:
					{
						Button = (DATA8)GH.Lms.PrimParPointer();

						if (Blocked == 0)
						{
							State = cUiGetLongPress(Button);
						}
						GH.Lms.PrimParPointer(State);
					}
					break;

				case FLUSH:
					{
						if (Blocked == 0)
						{
							cUiButtonFlush();
						}
					}
					break;

				case WAIT_FOR_PRESS:
					{
						if (Blocked == 0)
						{
							if (cUiWaitForPress() == 0)
							{
								GH.Lms.SetObjectIp(TmpIp);
								GH.Lms.SetObjectIpInd(TmpIpInd - 1);
								GH.Lms.SetDispatchStatus(BUSYBREAK);
							}
						}
						else
						{
							GH.Lms.SetObjectIp(TmpIp);
							GH.Lms.SetObjectIpInd(TmpIpInd - 1);
							GH.Lms.SetDispatchStatus(BUSYBREAK);
						}
					}
					break;

				case GET_HORZ:
					{
						if (Blocked == 0)
						{
							Inc = cUiGetHorz();
						}
						GH.Lms.PrimParPointer(Inc);
					}
					break;

				case GET_VERT:
					{
						if (Blocked == 0)
						{
							Inc = cUiGetVert();
						}
						GH.Lms.PrimParPointer(Inc);
					}
					break;

				case SET_BACK_BLOCK:
					{
						GH.UiInstance.BackButtonBlocked = (DATA8)GH.Lms.PrimParPointer();
					}
					break;

				case GET_BACK_BLOCK:
					{
						GH.Lms.PrimParPointer(GH.UiInstance.BackButtonBlocked);
					}
					break;

				case TESTSHORTPRESS:
					{
						Button = (DATA8)GH.Lms.PrimParPointer();

						if (Blocked == 0)
						{
							State = cUiTestShortPress(Button);
						}
						GH.Lms.PrimParPointer(State);
					}
					break;

				case TESTLONGPRESS:
					{
						Button = (DATA8)GH.Lms.PrimParPointer();

						if (Blocked == 0)
						{
							State = cUiTestLongPress(Button);
						}
						GH.Lms.PrimParPointer(State);
					}
					break;

				case GET_CLICK:
					{
						GH.Lms.PrimParPointer(GH.UiInstance.Click);
						GH.UiInstance.Click = 0;
					}
					break;
			}
		}

		public void cUiKeepAlive()
		{
			cUiAlive();
			GH.Lms.PrimParPointer((DATA8)GH.Lms.GetSleepMinutes());
		}
	}
}
