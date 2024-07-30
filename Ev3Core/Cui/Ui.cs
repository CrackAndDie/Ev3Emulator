using Ev3Core;
using Ev3Core.Cui.Interfaces;
using Ev3Core.Enums;
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

		public void cUiButton()
		{
			throw new NotImplementedException();
		}

		

		public void cUiDraw()
		{
			throw new NotImplementedException();
		}

		

		public void cUiFlush()
		{
			throw new NotImplementedException();
		}

		public void cUiKeepAlive()
		{
			throw new NotImplementedException();
		}

		

		public void cUiRead()
		{
			throw new NotImplementedException();
		}

		

		

		public void cUiWrite()
		{
			throw new NotImplementedException();
		}
	}
}
