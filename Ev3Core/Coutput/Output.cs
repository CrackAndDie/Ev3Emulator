using Ev3Core.Coutput.Interfaces;
using Ev3Core.Enums;
using Ev3Core.Extensions;
using Ev3Core.Helpers;
using Ev3Core.Lms2012.Interfaces;
using static Ev3Core.Defines;

namespace Ev3Core.Coutput
{
	public class Output : IOutput
	{
		uint DELAY_COUNTER = 0;
		UBYTE BusyOnes = 0;

		void OutputReset()
		{
			UBYTE Tmp;
			UBYTE[] StopArr = new UBYTE[3];

			for (Tmp = 0; Tmp < OUTPUTS; Tmp++)
			{
				GH.OutputInstance.Owner[Tmp] = 0;
			}

			StopArr[0] = opOUTPUT_STOP;
			StopArr[1] = 0x0F;
			StopArr[2] = 0x00;
			GH.Ev3System.OutputHandler.WritePwmData(StopArr, 3);
		}

		public RESULT cOutputInit()
		{
			RESULT Result = RESULT.FAIL;
			MOTORDATA pTmp;

			// To ensure that pMotor is never uninitialised
			GH.OutputInstance.pMotor = 0;

			Result = cOutputOpen();

			return (Result);
		}

		public RESULT cOutputOpen()
		{
			RESULT Result = RESULT.FAIL;

			UBYTE PrgStart = opPROGRAM_START;

			OutputReset();

			GH.Ev3System.OutputHandler.WritePwmData(new byte[1] { PrgStart }, 1);

			Result = OK;

			return (Result);
		}

		public RESULT cOutputClose()
		{
			return (OK);
		}

		public RESULT cOutputExit()
		{
			RESULT Result = RESULT.FAIL;

			OutputReset();

			Result = OK;

			return (Result);
		}

		public void cOutputSetTypes(ArrayPointer<UBYTE> pTypes)
		{
			UBYTE[] TypeArr = new byte[5];

			TypeArr[0] = opOUTPUT_SET_TYPE;
			Array.Copy(TypeArr, 1, pTypes.GetSkipped(), 0, 4);

			GH.Ev3System.OutputHandler.WritePwmData(TypeArr, 5);
		}

		UBYTE cOutputPackParam(DATA32 Val, UBYTE[] pStr)
		{
			UBYTE Len;

			Len = 0;
			if ((Val < 32) && (Val > -32))
			{
				pStr[Len] = (UBYTE)(Val & 0x0000003F);
				Len++;
			}
			else
			{
				if ((Val < DATA8_MAX) && (Val > DATA8_MIN))
				{
					pStr[Len] = 0x81;
					Len++;
					pStr[Len] = (UBYTE)Val;
					Len++;
				}
				else
				{
					if ((Val < DATA16_MAX) && (Val > DATA16_MIN))
					{
						pStr[Len] = 0x82;
						Len++;
						(pStr)[Len] = (UBYTE)(Val & 0x00FF);
						Len++;
						(pStr)[Len] = (UBYTE)((Val >> 8) & 0x00FF);
						Len++;
					}
					else
					{
						pStr[Len] = 0x83;
						Len++;
						(pStr)[Len] = (UBYTE)(Val & 0x000000FF);
						Len++;
						(pStr)[Len] = (UBYTE)((Val >> 8) & 0x000000FF);
						Len++;
						(pStr)[Len] = (UBYTE)((Val >> 16) & 0x000000FF);
						Len++;
						(pStr)[Len] = (UBYTE)((Val >> 24) & 0x000000FF);
						Len++;
					}
				}
			}
			return (Len);
		}

		public void ResetDelayCounter(UBYTE Pattern)
		{
			BusyOnes = Pattern;
			DELAY_COUNTER = 0;
		}

		public byte cMotorGetBusyFlags()
		{
			int test, test2;

			var a = GH.Ev3System.OutputHandler.GetMotorBusyFlags();
			test = a.Item1;
			test2 = a.Item2;
			if (DELAY_COUNTER < 25)
			{
				test = BusyOnes;
				DELAY_COUNTER++;
			}

			return ((byte)test);
		}

		public void cMotorSetBusyFlags(byte Flags)
		{
			GH.Ev3System.OutputHandler.SetMotorBusyFlags(Flags);
		}

		public void cOutputPrgStop()
		{
			DSPSTAT DspStat = DSPSTAT.NOBREAK;
			UBYTE PrgStop;

			PrgStop = opPROGRAM_STOP;
			GH.Ev3System.OutputHandler.WritePwmData(new byte[1] { PrgStop }, 1);
			GH.Lms.SetDispatchStatus(DspStat);
		}

		public void cOutputSetType()
		{
			DATA8 Layer;
			DATA8 No;
			UBYTE Type;
			UBYTE Len;
			DSPSTAT DspStat = DSPSTAT.NOBREAK;
			IP TmpIp;

			TmpIp = GH.Lms.GetObjectIp();
			Len = 0;
			Layer = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			No = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			Type = GH.Lms.PrimParPointer().GetUBYTE();

			if (Layer == 0)
			{
				if ((No >= 0) && (No < OUTPUTS))
				{
					if (GH.OutputInstance.OutputType[No] != Type)
					{
						GH.OutputInstance.OutputType[No] = Type;

						if ((Type == TYPE_NONE) || (Type == TYPE_ERROR))
						{
							GH.Ev3System.Logger.LogInfo($"Output {'A' + No} Disable\r\n");
						}
						else
						{
							GH.Ev3System.Logger.LogInfo($"Output {'A' + No} Enable\r\n");
						}
					}
				}
			}
			else
			{
				// TODO: daisy shite if you want
			}
			GH.Lms.SetDispatchStatus(DspStat);
		}

		public void cOutputReset()
		{
			DATA8 Layer;
			UBYTE Nos;
			UBYTE Len;
			DSPSTAT DspStat = DSPSTAT.NOBREAK;
			IP TmpIp;

			UBYTE[] ResetArr = new byte[2];

			TmpIp = GH.Lms.GetObjectIp();
			Len = 0;
			Layer = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			Nos = (UBYTE)GH.Lms.PrimParPointer().GetUBYTE();

			if (Layer == 0)
			{

				ResetArr[0] = opOUTPUT_RESET;
				ResetArr[1] = Nos;

				GH.Ev3System.OutputHandler.WritePwmData(ResetArr, 2);
			}
			else
			{
				// TODO: daisy shite if you want
			}
			GH.Lms.SetDispatchStatus(DspStat);
		}

		public void cOutputStop()
		{
			DATA8 Layer;
			UBYTE Nos;
			UBYTE Brake;
			UBYTE Len;
			DSPSTAT DspStat = DSPSTAT.NOBREAK;
			IP TmpIp;

			UBYTE[] StopArr = new byte[3];

			TmpIp = GH.Lms.GetObjectIp();
			Len = 0;
			Layer = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			Nos = (UBYTE)GH.Lms.PrimParPointer().GetUBYTE();
			Brake = (UBYTE)GH.Lms.PrimParPointer().GetUBYTE();

			if (Layer == 0)
			{
				StopArr[0] = (UBYTE)opOUTPUT_STOP;
				StopArr[1] = Nos;
				StopArr[2] = Brake;

				GH.Ev3System.OutputHandler.WritePwmData(StopArr, 3);
			}
			else
			{
				// TODO: daisy shite if you want
			}
			GH.Lms.SetDispatchStatus(DspStat);
		}

		public void cOutputSpeed()
		{
			DATA8 Layer;
			UBYTE Nos;
			UBYTE Speed;
			UBYTE Len;
			DSPSTAT DspStat = DSPSTAT.NOBREAK;
			IP TmpIp;

			UBYTE[] SetSpeed = new byte[3];

			TmpIp = GH.Lms.GetObjectIp();
			Len = 0;
			Layer = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			Nos = (UBYTE)GH.Lms.PrimParPointer().GetUBYTE();
			Speed = (UBYTE)GH.Lms.PrimParPointer().GetUBYTE();

			if (Layer == 0)
			{
				SetSpeed[0] = (UBYTE)opOUTPUT_SPEED;
				SetSpeed[1] = Nos;
				SetSpeed[2] = Speed;

				GH.Ev3System.OutputHandler.WritePwmData(SetSpeed, 3);
			}
			else
			{
				// TODO: daisy shite if you want
			}
			GH.Lms.SetDispatchStatus(DspStat);
		}

		public void cOutputPower()
		{
			DATA8 Layer;
			UBYTE Nos;
			UBYTE Power;
			UBYTE[] SetPower = new byte[3];
			DATA8 Len;
			DSPSTAT DspStat = DSPSTAT.NOBREAK;
			IP TmpIp;

			TmpIp = GH.Lms.GetObjectIp();
			Len = 0;
			Layer = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			Nos = (UBYTE)GH.Lms.PrimParPointer().GetUBYTE();
			Power = (UBYTE)GH.Lms.PrimParPointer().GetUBYTE();

			if (Layer == 0)
			{
				SetPower[0] = (UBYTE)opOUTPUT_POWER;
				SetPower[1] = Nos;
				SetPower[2] = Power;
				GH.Ev3System.OutputHandler.WritePwmData(SetPower, 3);
			}
			else
			{
				// TODO: daisy shite if you want
			}
			GH.Lms.SetDispatchStatus(DspStat);
		}

		public void cOutputStart()
		{
			DATA8 Tmp;
			DATA8 Layer;
			UBYTE Nos;
			DATA8 Len;
			DSPSTAT DspStat = DSPSTAT.NOBREAK;
			IP TmpIp;

			UBYTE[] StartMotor = new byte[2];

			TmpIp = GH.Lms.GetObjectIp();
			Len = 0;
			Layer = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			Nos = (UBYTE)GH.Lms.PrimParPointer().GetUBYTE();

			if (Layer == 0)
			{
				StartMotor[0] = (UBYTE)opOUTPUT_START;
				StartMotor[1] = Nos;
				GH.Ev3System.OutputHandler.WritePwmData(StartMotor, 2);
				for (Tmp = 0; Tmp < OUTPUTS; Tmp++)
				{
					if ((Nos & (0x01 << Tmp)) != 0)
					{
						GH.OutputInstance.Owner[Tmp] = GH.Lms.CallingObjectId();
					}
				}
			}
			else
			{
				// TODO: daisy shite if you want
			}
			GH.Lms.SetDispatchStatus(DspStat);
		}

		public void cOutputPolarity()
		{
			DATA8 Layer;
			UBYTE[] Polarity = new byte[3];
			UBYTE Len;
			DSPSTAT DspStat = DSPSTAT.NOBREAK;
			IP TmpIp;

			TmpIp = GH.Lms.GetObjectIp();
			Len = 0;
			Layer = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			Polarity[0] = (UBYTE)opOUTPUT_POLARITY;
			Polarity[1] = (UBYTE)GH.Lms.PrimParPointer().GetUBYTE();
			Polarity[2] = (UBYTE)GH.Lms.PrimParPointer().GetUBYTE();

			if (Layer == 0)
			{
				GH.Ev3System.OutputHandler.WritePwmData(Polarity, 3);
			}
			else
			{
				// TODO: daisy shite if you want
			}
			GH.Lms.SetDispatchStatus(DspStat);
		}

		public void cOutputStepPower()
		{
			DATA8 Layer;
			DATA8 Tmp;
			STEPPOWER StepPower = new STEPPOWER();
			UBYTE Len;
			DSPSTAT DspStat = DSPSTAT.NOBREAK;
			IP TmpIp;

			TmpIp = GH.Lms.GetObjectIp();
			Len = 0;
			Layer = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			StepPower.Cmd = opOUTPUT_STEP_POWER;
			StepPower.Nos = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			StepPower.Power = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			StepPower.Step1 = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
			StepPower.Step2 = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
			StepPower.Step3 = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
			StepPower.Brake = (DATA8)GH.Lms.PrimParPointer().GetDATA8();

			if (0 == Layer)
			{
				GH.Ev3System.OutputHandler.WritePwmData(StepPower.ToByteArray(), STEPPOWER.Sizeof);
				for (Tmp = 0; Tmp < OUTPUTS; Tmp++)
				{
					// Set calling id for all involved inputs
					if ((StepPower.Nos & (0x01 << Tmp)) != 0)
					{
						GH.OutputInstance.Owner[Tmp] = GH.Lms.CallingObjectId();
					}
				}
			}
			else
			{
				// TODO: daisy shite if you want
			}
			GH.Lms.SetDispatchStatus(DspStat);
		}

		public void cOutputTimePower()
		{
			DATA8 Layer;
			DATA8 Tmp;
			TIMEPOWER TimePower = new TIMEPOWER();
			UBYTE Len;
			DSPSTAT DspStat = DSPSTAT.NOBREAK;
			IP TmpIp;

			TmpIp = GH.Lms.GetObjectIp();
			Len = 0;
			Layer = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			TimePower.Cmd = opOUTPUT_TIME_POWER;
			TimePower.Nos = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			TimePower.Power = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			TimePower.Time1 = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
			TimePower.Time2 = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
			TimePower.Time3 = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
			TimePower.Brake = (DATA8)GH.Lms.PrimParPointer().GetDATA8();

			if (0 == Layer)
			{
				GH.Ev3System.OutputHandler.WritePwmData(TimePower.ToByteArray(), TIMEPOWER.Sizeof);
				for (Tmp = 0; Tmp < OUTPUTS; Tmp++)
				{
					// Set calling id for all involved inputs
					if ((TimePower.Nos & (0x01 << Tmp)) != 0)
					{
						GH.OutputInstance.Owner[Tmp] = GH.Lms.CallingObjectId();
					}
				}
			}
			else
			{
				// TODO: daisy shite if you want
			}
			GH.Lms.SetDispatchStatus(DspStat);
		}

		public void cOutputStepSpeed()
		{
			DATA8 Layer;
			DATA8 Tmp;
			STEPSPEED StepSpeed = new STEPSPEED();
			UBYTE Len;
			DSPSTAT DspStat = DSPSTAT.NOBREAK;
			IP TmpIp;

			TmpIp = GH.Lms.GetObjectIp();
			Len = 0;
			Layer = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			StepSpeed.Cmd = opOUTPUT_STEP_SPEED;
			StepSpeed.Nos = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			StepSpeed.Speed = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			StepSpeed.Step1 = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
			StepSpeed.Step2 = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
			StepSpeed.Step3 = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
			StepSpeed.Brake = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			if (0 == Layer)
			{
				GH.Ev3System.OutputHandler.WritePwmData(StepSpeed.ToByteArray(), STEPSPEED.Sizeof);
				for (Tmp = 0; Tmp < OUTPUTS; Tmp++)
				{// Set calling id for all involved inputs

					if ((StepSpeed.Nos & (0x01 << Tmp)) != 0)
					{
						GH.OutputInstance.Owner[Tmp] = GH.Lms.CallingObjectId();
					}
				}
			}
			else
			{
				// TODO: daisy shite if you want
			}
			GH.Lms.SetDispatchStatus(DspStat);
		}

		public void cOutputTimeSpeed()
		{
			DATA8 Layer;
			DATA8 Tmp;
			TIMESPEED TimeSpeed = new TIMESPEED();
			UBYTE Len;
			DSPSTAT DspStat = DSPSTAT.NOBREAK;
			IP TmpIp;

			TmpIp = GH.Lms.GetObjectIp();
			Len = 0;
			Layer = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			TimeSpeed.Cmd = opOUTPUT_TIME_SPEED;
			TimeSpeed.Nos = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			TimeSpeed.Speed = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			TimeSpeed.Time1 = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
			TimeSpeed.Time2 = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
			TimeSpeed.Time3 = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
			TimeSpeed.Brake = (DATA8)GH.Lms.PrimParPointer().GetDATA8();

			if (0 == Layer)
			{
				GH.Ev3System.OutputHandler.WritePwmData(TimeSpeed.ToByteArray(), TIMESPEED.Sizeof);
				for (Tmp = 0; Tmp < OUTPUTS; Tmp++)
				{
					// Set calling id for all involved inputs
					if ((TimeSpeed.Nos & (0x01 << Tmp)) != 0)
					{
						GH.OutputInstance.Owner[Tmp] = GH.Lms.CallingObjectId();
					}
				}
			}
			else
			{
				// TODO: daisy shite if you want
			}
			GH.Lms.SetDispatchStatus(DspStat);
		}

		public void cOutputStepSync()
		{
			DATA8 Layer;
			DATA8 Tmp;
			STEPSYNC StepSync = new STEPSYNC();
			UBYTE Len;
			DSPSTAT DspStat = DSPSTAT.NOBREAK;
			IP TmpIp;

			TmpIp = GH.Lms.GetObjectIp();
			Len = 0;
			Layer = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			StepSync.Cmd = opOUTPUT_STEP_SYNC;
			StepSync.Nos = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			StepSync.Speed = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			StepSync.Turn = (DATA16)GH.Lms.PrimParPointer().GetDATA16();
			StepSync.Step = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
			StepSync.Brake = (DATA8)GH.Lms.PrimParPointer().GetDATA8();

			if (0 == Layer)
			{
				GH.Ev3System.OutputHandler.WritePwmData(StepSync.ToByteArray(), STEPSYNC.Sizeof);
				for (Tmp = 0; Tmp < OUTPUTS; Tmp++)
				{
					// Set calling id for all involved outputs
					if ((StepSync.Nos & (0x01 << Tmp)) != 0)
					{
						GH.OutputInstance.Owner[Tmp] = GH.Lms.CallingObjectId();
					}
				}
			}
			else
			{
				// TODO: daisy shite if you want
			}
			GH.Lms.SetDispatchStatus(DspStat);
		}

		public void cOutputTimeSync()
		{
			DATA8 Layer;
			DATA8 Tmp;
			TIMESYNC TimeSync = new TIMESYNC();
			UBYTE Len;
			DSPSTAT DspStat = DSPSTAT.NOBREAK;
			IP TmpIp;

			TmpIp = GH.Lms.GetObjectIp();
			Len = 0;
			Layer = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			TimeSync.Cmd = opOUTPUT_TIME_SYNC;
			TimeSync.Nos = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			TimeSync.Speed = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			TimeSync.Turn = (DATA16)GH.Lms.PrimParPointer().GetDATA16();
			TimeSync.Time = (DATA32)GH.Lms.PrimParPointer().GetDATA32();
			TimeSync.Brake = (DATA8)GH.Lms.PrimParPointer().GetDATA8();

			if (0 == Layer)
			{
				GH.Ev3System.OutputHandler.WritePwmData(TimeSync.ToByteArray(), TIMESYNC.Sizeof);
				for (Tmp = 0; Tmp < OUTPUTS; Tmp++)
				{
					// Set calling id for all involved outputs
					if ((TimeSync.Nos & (0x01 << Tmp)) != 0)
					{
						GH.OutputInstance.Owner[Tmp] = GH.Lms.CallingObjectId();
					}
				}
			}
			else
			{
				// TODO: daisy shite if you want
			}
			GH.Lms.SetDispatchStatus(DspStat);
		}

		public void cOutputRead()
		{
			DATA8 Layer;
			DATA8 No;
			DATA8 Speed = 0;
			DATA32 Tacho = 0;

			Layer = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			No = (DATA8)GH.Lms.PrimParPointer().GetDATA8();

			if (0 == Layer)
			{
				// TODO: UPDATE TACHO AND OTHER
				if (No < OUTPUTS)
				{
					Speed = GH.OutputInstance.MotorData[No].Speed;
					Tacho = GH.OutputInstance.MotorData[No].TachoCounts;
				}
			}
			GH.Lms.PrimParPointer().SetDATA8((DATA8)Speed);
			GH.Lms.PrimParPointer().SetDATA32((DATA32)Tacho);
		}

		public void cOutputReady()
		{
			OBJID Owner;
			DATA8 Layer, Tmp, Nos;
			IP TmpIp;
			DSPSTAT DspStat = DSPSTAT.NOBREAK;
			UBYTE Bits;

			int test;
			int test2;

			char[] BusyReturn = new char[10]; // Busy mask

			TmpIp = GH.Lms.GetObjectIp();

			Layer = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			Nos = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			Owner = GH.Lms.CallingObjectId();

			if (0 == Layer)
			{
				var a = GH.Ev3System.OutputHandler.GetMotorBusyFlags();
				test = a.Item1;
				test2 = a.Item2;

				for (Tmp = 0; (Tmp < OUTPUTS) && (DspStat == DSPSTAT.NOBREAK); Tmp++)
				{
					// Check resources for NOTREADY
					if ((Nos & (1 << Tmp)) != 0)
					{
						// Only relevant ones
						if ((test & (1 << Tmp)) != 0)
						{
							// If BUSY check for OVERRULED
							if (GH.OutputInstance.Owner[Tmp] == Owner)
							{
								DspStat = DSPSTAT.BUSYBREAK;
							}
						}
					}
				}
			}
			else
			{
				// TODO: daisy shite if you want
			}

			if (DspStat == DSPSTAT.BUSYBREAK)
			{
				// Rewind IP
				GH.Lms.SetObjectIp(TmpIp - 1);
			}
			GH.Lms.SetDispatchStatus(DspStat);
		}

		public void cOutputTest()
		{
			DATA8 Layer, Nos, Busy = 0;

			int test;
			int test2;

			char[] BusyReturn = new char[20]; // Busy mask

			Layer = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			Nos = (DATA8)GH.Lms.PrimParPointer().GetDATA8();

			if (0 == Layer)
			{
				var a = GH.Ev3System.OutputHandler.GetMotorBusyFlags();
				test = a.Item1;
				test2 = a.Item2;
				if ((Nos & (DATA8)test2) != 0)
				{
					Busy = 1;
				}
			}
			else
			{
				// TODO: daisy shite if you want
			}
			GH.Lms.PrimParPointer().SetDATA8((DATA8)Busy);
		}

		public void cOutputClrCount()
		{
			DATA8 Layer;
			UBYTE[] ClrCnt = new UBYTE[2];
			UBYTE Len;
			DSPSTAT DspStat = DSPSTAT.NOBREAK;
			IP TmpIp;
			UBYTE Tmp;

			TmpIp = GH.Lms.GetObjectIp();
			Len = 0;
			Layer = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			ClrCnt[0] = opOUTPUT_CLR_COUNT;
			ClrCnt[1] = (UBYTE)GH.Lms.PrimParPointer().GetUBYTE();

			if (0 == Layer)
			{
				GH.Ev3System.OutputHandler.WritePwmData(ClrCnt, 2);

				// Also the user layer entry to get immediate clear
				for (Tmp = 0; Tmp < OUTPUTS; Tmp++)
				{
					if ((ClrCnt[1] & (1 << Tmp)) != 0)
					{
						GH.OutputInstance.MotorData[Tmp].TachoSensor = 0;
					}
				}
			}
			else
			{
				// TODO: daisy shite if you want
			}
			GH.Lms.SetDispatchStatus(DspStat);
		}

		public void cOutputGetCount()
		{
			DATA8 Layer;
			DATA8 No;
			DATA32 Tacho = 0;

			Layer = (DATA8)GH.Lms.PrimParPointer().GetDATA8();
			No = (DATA8)GH.Lms.PrimParPointer().GetDATA8();

			if (0 == Layer)
			{
				// TODO: UPDATE TACHO AND OTHER
				if (No < OUTPUTS)
				{
					Tacho = GH.OutputInstance.MotorData[No].TachoSensor;
				}
			}
			GH.Lms.PrimParPointer().SetDATA32((DATA32)Tacho);
		}
	}
}
