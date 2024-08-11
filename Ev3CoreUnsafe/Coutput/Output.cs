using Ev3CoreUnsafe.Coutput.Interfaces;
using Ev3CoreUnsafe.Enums;
using Ev3CoreUnsafe.Helpers;
using Ev3CoreUnsafe.Lms2012.Interfaces;
using static Ev3CoreUnsafe.Defines;

namespace Ev3CoreUnsafe.Coutput
{
    public unsafe class Output : IOutput
    {
        uint DELAY_COUNTER = 0;
        UBYTE BusyOnes = 0;
        DATA8* DaisyBuf = CommonHelper.Pointer1d<DATA8>(64);

        public void OutputReset()
        {
            UBYTE Tmp;
            DATA8* StopArr = CommonHelper.Pointer1d<DATA8>(3);

            for (Tmp = 0; Tmp < OUTPUTS; Tmp++)
            {
                GH.OutputInstance.Owner[Tmp] = 0;
            }

            unchecked { StopArr[0] = (DATA8)opOUTPUT_STOP; }
            StopArr[1] = 0x0F;
            StopArr[2] = 0x00;
            GH.Ev3System.OutputHandler.WritePwmData(CommonHelper.GetArray((byte*)StopArr, 3));
        }


        public RESULT cOutputInit()
        {
            RESULT Result = RESULT.FAIL;
            // MOTORDATA* pTmp;

            // To ensure that pMotor is never uninitialised
            GH.OutputInstance.pMotor = GH.OutputInstance.MotorData;

            // TODO: mapping shite (probably no need)
            //// Open the handle for writing commands
            //GH.OutputInstance.PwmFile = open(PWM_DEVICE_NAME, O_RDWR);

            //if (GH.OutputInstance.PwmFile >= 0)
            //{

            //    // Open the handle for reading motor values - shared memory
            //    GH.OutputInstance.MotorFile = open(MOTOR_DEVICE_NAME, O_RDWR | O_SYNC);
            //    if (GH.OutputInstance.MotorFile >= 0)
            //    {
            //        pTmp = (MOTORDATA*)mmap(0, sizeof(GH.OutputInstance.MotorData), PROT_READ | PROT_WRITE, MAP_FILE | MAP_SHARED, GH.OutputInstance.MotorFile, 0);

            //        if (pTmp == MAP_FAILED)
            //        {
            //            LogErrorNumber(OUTPUT_SHARED_MEMORY);
            //            close(GH.OutputInstance.MotorFile);
            //            close(GH.OutputInstance.PwmFile);
            //        }
            //        else
            //        {
            //            GH.OutputInstance.pMotor = (MOTORDATA*)pTmp;
            //            Result = cOutputOpen();
            //        }
            //    }
            //}

            return (Result);
        }


        public RESULT cOutputOpen()
        {
            RESULT Result = RESULT.FAIL;

            UBYTE PrgStart = opPROGRAM_START;

            OutputReset();

            GH.Ev3System.OutputHandler.WritePwmData(CommonHelper.GetArray((byte*)&PrgStart, 1));

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

            // TODO: mapping shite (probably no need)
            //if (GH.OutputInstance.MotorFile >= 0)
            //{
            //    munmap(GH.OutputInstance.pMotor, sizeof(GH.OutputInstance.MotorData));
            //    close(GH.OutputInstance.MotorFile);
            //}

            //if (GH.OutputInstance.PwmFile >= 0)
            //{
            //    close(GH.OutputInstance.PwmFile);
            //}

            Result = OK;

            return (Result);
        }


        public void cOutputSetTypes(sbyte* pTypes)
        {
            DATA8* TypeArr = CommonHelper.Pointer1d<DATA8>(5);

            unchecked
            {
                TypeArr[0] = (sbyte)opOUTPUT_SET_TYPE;
            }
            CommonHelper.memcpy((byte*)&TypeArr[1], (byte*)pTypes, 4);

            GH.Ev3System.OutputHandler.WritePwmData(CommonHelper.GetArray((byte*)TypeArr, 5));
        }


        /*! \page cOutput Output
         *  <hr size="1"/>
         *  <b> UBYTE    cOutputPackParam (DATA32 Val, DATA8 *pStr)  </b>
         *
         *- Helper function to pack parameters - always to const parameters -
         *- This is to pack parameters back into a string ready for Daisy
         *- chain transmission
         *
         *  \param  (DATA8)   Val     - 32 bit value you would like to pack
         *  \param  (DATA8*)  pStr    - String pointer where to pack the 32
         *                              bit Val
         */
        public UBYTE cOutputPackParam(DATA32 Val, DATA8* pStr)
        {
            DATA8 Len;

            Len = 0;
            if ((Val < 32) && (Val > -32))
            {
                pStr[Len] = (DATA8)(Val & 0x0000003F);
                Len++;
            }
            else
            {
                if ((Val < DATA8_MAX) && (Val > DATA8_MIN))
                {
                    unchecked
                    {
                        pStr[Len] = (sbyte)0x81;
                    }
                    Len++;
                    pStr[Len] = (DATA8)Val;
                    Len++;
                }
                else
                {
                    if ((Val < DATA16_MAX) && (Val > DATA16_MIN))
                    {
                        unchecked
                        {
                            pStr[Len] = (sbyte)0x82;
                        }
                        Len++;
                        ((UBYTE*)pStr)[Len] = (UBYTE)(Val & 0x00FF);
                        Len++;
                        ((UBYTE*)pStr)[Len] = (UBYTE)((Val >> 8) & 0x00FF);
                        Len++;
                    }
                    else
                    {
                        unchecked
                        {
                            pStr[Len] = (sbyte)0x83;
                        }
                        Len++;
                        ((UBYTE*)pStr)[Len] = (UBYTE)(Val & 0x000000FF);
                        Len++;
                        ((UBYTE*)pStr)[Len] = (UBYTE)((Val >> 8) & 0x000000FF);
                        Len++;
                        ((UBYTE*)pStr)[Len] = (UBYTE)((Val >> 16) & 0x000000FF);
                        Len++;
                        ((UBYTE*)pStr)[Len] = (UBYTE)((Val >> 24) & 0x000000FF);
                        Len++;
                    }
                }
            }
            return ((byte)Len);
        }
        /*

        UBYTE     cMotorGetBusyFlags(void)
        {
          int     test, test2;
          char    BusyReturn[10]; // Busy mask

          if (GH.OutputInstance.PwmFile >= 0)
          {
            read(GH.OutputInstance.PwmFile,BusyReturn,4);
            sscanf(BusyReturn,"%u %u",&test,&test2);
          }
          else
          {
            test = 0;
          }
          printf("cMotorGetBusyFlags test = %d\n\r", test);
          return(test);
        }*/

        public void ResetDelayCounter(UBYTE Pattern)
        {
            BusyOnes = Pattern;
            DELAY_COUNTER = 0;
        }

        public UBYTE cMotorGetBusyFlags()
        {
            int test, test2;
            DATA8* BusyReturn = CommonHelper.Pointer1d<DATA8>(10); // Busy mask

            var tmp = GH.Ev3System.OutputHandler.GetMotorBusyFlags();
            test = tmp.Item1;
            test2 = tmp.Item2;

            if (DELAY_COUNTER < 25)
            {
                test = BusyOnes;
                DELAY_COUNTER++;
            }

            return ((byte)test);
        }


        public void cMotorSetBusyFlags(UBYTE Flags)
        {
            GH.Ev3System.OutputHandler.SetMotorBusyFlags(Flags);
        }



        //******* BYTE CODE SNIPPETS **************************************************


        /*! \page cOutput Output
         *  <hr size="1"/>
         *  <b>     opOUTPUT_PRG_STOP (LAYER, NO, TYPE)  </b>
         *
         *- Program stop\n
         *- Dispatch status unchanged
         *
         */
        /*! \brief  opOUTPUT_PRG_STOP byte code
         *
         */
        public void cOutputPrgStop()
        {
            DSPSTAT DspStat = DSPSTAT.NOBREAK;
            DATA8 PrgStop;

            PrgStop = (DATA8)opPROGRAM_STOP;
            GH.Ev3System.OutputHandler.WritePwmData(CommonHelper.GetArray((byte*)&PrgStop, 1));
            GH.Lms.SetDispatchStatus(DspStat);
        }


        /*! \page cOutput Output
         *  <hr size="1"/>
         *  <b>     opOUTPUT_SET_TYPE (LAYER, NO, TYPE)  </b>
         *
         *- Set output type\n
         *- Dispatch status unchanged
         *
         *  \param  (DATA8)   LAYER   - Chain layer number [0..3]
         *  \param  (DATA8)   NO      - Output no [0..3]
         *  \param  (DATA8)   TYPE    - Output device type
         */
        /*! \brief  opOUTPUT_SET_TYPE byte code
         *
         */
        public void cOutputSetType()
        {
            DATA8 Layer;
            DATA8 No;
            DATA8 Type;
            UBYTE Len;
            DSPSTAT DspStat = DSPSTAT.NOBREAK;
            IP TmpIp;

            TmpIp = GH.Lms.GetObjectIp();
            Len = 0;
            Layer = *(DATA8*)GH.Lms.PrimParPointer();
            No = *(DATA8*)GH.Lms.PrimParPointer();
            Type = *(DATA8*)GH.Lms.PrimParPointer();

            if (Layer == 0)
            {
                if ((No >= 0) && (No < OUTPUTS))
                {
                    if (GH.OutputInstance.OutputType[No] != Type)
                    {
                        GH.OutputInstance.OutputType[No] = Type;

                        if ((Type == TYPE_NONE) || (Type == TYPE_ERROR))
                        {
                            GH.printf($"                Output {'A' + (char)No} Disable\r\n");
                        }
                        else
                        {
                            GH.printf($"                Output {'A' + (char)No} Enable\r\n");
                        }
                    }
                }
            }
            else
            {
                if (GH.Daisy.cDaisyReady() != RESULT.BUSY)
                {
                    DaisyBuf[Len++] = 0;
                    DaisyBuf[Len++] = 0;
                    unchecked
                    {
                        DaisyBuf[Len++] = (sbyte)opOUTPUT_RESET;
                    }
                    Len += cOutputPackParam((DATA32)0, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)No, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)Type, &(DaisyBuf[Len]));
                    if (OK != GH.Daisy.cDaisyDownStreamCmd(DaisyBuf, (sbyte)Len, Layer))
                    {
                        GH.Lms.SetObjectIp(TmpIp - 1);
                        DspStat = DSPSTAT.BUSYBREAK;
                    }
                    //GH.Daisy.cDaisyDownStreamCmd(DaisyBuf, Len, Layer);
                }
                else
                {
                    GH.Lms.SetObjectIp(TmpIp - 1);
                    DspStat = DSPSTAT.BUSYBREAK;
                }
            }
            GH.Lms.SetDispatchStatus(DspStat);
        }


        /*! \page cOutput
         *  <hr size="1"/>
         *  <b>     opOUTPUT_RESET (LAYER, NOS)  </b>
         *
         *- Resets the Tacho counts \n
         *- Dispatch status unchanged
         *
         *  \param  (DATA8)   LAYER   - Chain layer number [0..3]
         *  \param  (DATA8)   NOS     - Output bit field [0x00..0x0F]
         */
        /*! \brief  opOUTPUT_RESET byte code
         *
         */
        public void cOutputReset()
        {
            DATA8 Layer;
            UBYTE Nos;
            UBYTE Len;
            DSPSTAT DspStat = DSPSTAT.NOBREAK;
            IP TmpIp;

            DATA8* ResetArr = CommonHelper.Pointer1d<DATA8>(2);

            TmpIp = GH.Lms.GetObjectIp();
            Len = 0;
            Layer = *(DATA8*)GH.Lms.PrimParPointer();
            Nos = *(UBYTE*)GH.Lms.PrimParPointer();

            if (Layer == 0)
            {
                unchecked
                {
                    ResetArr[0] = (sbyte)opOUTPUT_RESET;
                    ResetArr[1] = (sbyte)Nos;
                }

                GH.Ev3System.OutputHandler.WritePwmData(CommonHelper.GetArray((byte*)ResetArr, 2));
            }
            else
            {
                if (GH.Daisy.cDaisyReady() != RESULT.BUSY)
                {
                    DaisyBuf[Len++] = 0;
                    DaisyBuf[Len++] = 0;
                    unchecked
                    {
                        DaisyBuf[Len++] = (sbyte)opOUTPUT_RESET;
                    }
                    Len += cOutputPackParam((DATA32)0, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)Nos, &(DaisyBuf[Len]));
                    if (OK != GH.Daisy.cDaisyDownStreamCmd(DaisyBuf, (sbyte)Len, Layer))
                    {
                        GH.Lms.SetObjectIp(TmpIp - 1);
                        DspStat = DSPSTAT.BUSYBREAK;
                    }
                    //GH.Daisy.cDaisyDownStreamCmd(DaisyBuf, Len, Layer);
                }
                else
                {
                    GH.Lms.SetObjectIp(TmpIp - 1);
                    DspStat = DSPSTAT.BUSYBREAK;
                }
            }
            GH.Lms.SetDispatchStatus(DspStat);
        }


        /*! \page cOutput
         *  <hr size="1"/>
         *  <b>     opOUTPUT_STOP (LAYER, NOS)  </b>
         *
         *- Stops the outputs\n
         *- Dispatch status unchanged
         *
         *  \param  (DATA8)   LAYER   - Chain layer number [0..3]
         *  \param  (DATA8)   NOS     - Output bit field [0x00..0x0F]
         *  \param  (DATA8)   BRAKE   - Brake [0,1]
         */
        /*! \brief  opOUTPUT_STOP byte code
         *
         */
        public void cOutputStop()
        {
            DATA8 Layer;
            UBYTE Nos;
            UBYTE Brake;
            UBYTE Len;
            DSPSTAT DspStat = DSPSTAT.NOBREAK;
            IP TmpIp;

            DATA8* StopArr = CommonHelper.Pointer1d<DATA8>(3);

            TmpIp = GH.Lms.GetObjectIp();
            Len = 0;
            Layer = *(DATA8*)GH.Lms.PrimParPointer();
            Nos = *(UBYTE*)GH.Lms.PrimParPointer();
            Brake = *(UBYTE*)GH.Lms.PrimParPointer();

            if (Layer == 0)
            {
                unchecked
                {
                    StopArr[0] = (DATA8)opOUTPUT_STOP;
                }
                unchecked
                {
                    StopArr[1] = (sbyte)Nos;
                    StopArr[2] = (sbyte)Brake;
                }

                GH.Ev3System.OutputHandler.WritePwmData(CommonHelper.GetArray((byte*)StopArr, 3));
            }
            else
            {
                if (GH.Daisy.cDaisyReady() != RESULT.BUSY)
                {
                    DaisyBuf[Len++] = 0;
                    DaisyBuf[Len++] = 0;
                    unchecked
                    {
                        DaisyBuf[Len++] = (sbyte)opOUTPUT_STOP;
                    }
                    Len += cOutputPackParam((DATA32)0, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)Nos, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)Brake, &(DaisyBuf[Len]));
                    if (OK != GH.Daisy.cDaisyDownStreamCmd(DaisyBuf, (sbyte)Len, Layer))
                    {
                        GH.Lms.SetObjectIp(TmpIp - 1);
                        DspStat = DSPSTAT.BUSYBREAK;
                    }
                    //GH.Daisy.cDaisyDownStreamCmd(DaisyBuf, Len, Layer);
                }
                else
                {
                    GH.Lms.SetObjectIp(TmpIp - 1);
                    DspStat = DSPSTAT.BUSYBREAK;
                }
            }
            GH.Lms.SetDispatchStatus(DspStat);
        }


        /*! \page cOutput
         *  <hr size="1"/>
         *  <b>     opOUTPUT_SPEED (LAYER, NOS, SPEED)  </b>
         *
         *- Set speed of the outputs\n
         *- Dispatch status unchanged
         *
         *  \param  (DATA8)   LAYER   - Chain layer number [0..3]
         *  \param  (DATA8)   NOS     - Output bit field [0x00..0x0F]
         *  \param  (DATA8)   SPEED   - Speed [-100..100%]
         *
         *
         */
        /*! \brief  opOUTPUT_SPEED byte code
         *
         */
        public void cOutputSpeed()
        {
            DATA8 Layer;
            UBYTE Nos;
            DATA8 Speed;
            UBYTE Len;
            DSPSTAT DspStat = DSPSTAT.NOBREAK;
            IP TmpIp;

            DATA8* SetSpeed = CommonHelper.Pointer1d<DATA8>(3);

            TmpIp = GH.Lms.GetObjectIp();
            Len = 0;
            Layer = *(DATA8*)GH.Lms.PrimParPointer();
            Nos = *(UBYTE*)GH.Lms.PrimParPointer();
            Speed = *(DATA8*)GH.Lms.PrimParPointer();

            if (Layer == 0)
            {
                unchecked
                {
                    SetSpeed[0] = (DATA8)opOUTPUT_SPEED;
                }
                unchecked
                {
                    SetSpeed[1] = (sbyte)Nos;
                }
                SetSpeed[2] = Speed;

                GH.Ev3System.OutputHandler.WritePwmData(CommonHelper.GetArray((byte*)SetSpeed, 3));
            }
            else
            {
                if (GH.Daisy.cDaisyReady() != RESULT.BUSY)
                {
                    DaisyBuf[Len++] = 0;
                    DaisyBuf[Len++] = 0;
                    unchecked
                    {
                        DaisyBuf[Len++] = (sbyte)opOUTPUT_SPEED;
                    }
                    Len += cOutputPackParam((DATA32)0, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)Nos, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)Speed, &(DaisyBuf[Len]));
                    if (OK != GH.Daisy.cDaisyDownStreamCmd(DaisyBuf, (sbyte)Len, Layer))
                    {
                        GH.Lms.SetObjectIp(TmpIp - 1);
                        DspStat = DSPSTAT.BUSYBREAK;
                    }
                }
                else
                {
                    GH.Lms.SetObjectIp(TmpIp - 1);
                    DspStat = DSPSTAT.BUSYBREAK;
                }
            }
            GH.Lms.SetDispatchStatus(DspStat);
        }


        /*! \page cOutput
         *  <hr size="1"/>
         *  <b>     opOUTPUT_POWER (LAYER, NOS, SPEED)  </b>
         *
         *- Set power of the outputs\n
         *- Dispatch status unchanged
         *
         *  \param  (DATA8)   LAYER   - Chain layer number [0..3]
         *  \param  (DATA8)   NOS     - Output bit field [0x00..0x0F]
         *  \param  (DATA8)   POWER   - Power [-100..100%]
         */
        /*! \brief  opOUTPUT_POWER byte code
         *
         */
        public void cOutputPower()
        {
            DATA8 Layer;
            UBYTE Nos;
            DATA8 Power;
            DATA8* SetPower = CommonHelper.Pointer1d<DATA8>(3);
            DATA8 Len;
            DSPSTAT DspStat = DSPSTAT.NOBREAK;
            IP TmpIp;

            TmpIp = GH.Lms.GetObjectIp();
            Len = 0;
            Layer = *(DATA8*)GH.Lms.PrimParPointer();
            Nos = *(UBYTE*)GH.Lms.PrimParPointer();
            Power = *(DATA8*)GH.Lms.PrimParPointer();

            if (Layer == 0)
            {
                unchecked
                {
                    SetPower[0] = (DATA8)opOUTPUT_POWER;
                }
                unchecked
                {
                    SetPower[1] = (sbyte)Nos;
                }
                SetPower[2] = Power;
                GH.Ev3System.OutputHandler.WritePwmData(CommonHelper.GetArray((byte*)SetPower, 3));
            }
            else
            {
                if (GH.Daisy.cDaisyReady() != RESULT.BUSY)
                {
                    DaisyBuf[Len++] = 0;
                    DaisyBuf[Len++] = 0;
                    unchecked
                    {
                        DaisyBuf[Len++] = (sbyte)opOUTPUT_POWER;
                    }
                    Len += (sbyte)cOutputPackParam((DATA32)0, &(DaisyBuf[Len]));
                    Len += (sbyte)cOutputPackParam((DATA32)Nos, &(DaisyBuf[Len]));
                    Len += (sbyte)cOutputPackParam((DATA32)Power, &(DaisyBuf[Len]));

                    if (OK != GH.Daisy.cDaisyDownStreamCmd(DaisyBuf, Len, Layer))
                    {
                        GH.Lms.SetObjectIp(TmpIp - 1);
                        DspStat = DSPSTAT.BUSYBREAK;
                    }
                    else
                    {
                        // printf("cOutPut @ opOUTPUT_POWER after GH.Daisy.cDaisyDownStreamCmd - OK and WriteState = %d\n\r", cDaisyGetLastWriteState());
                    }
                    //GH.Daisy.cDaisyDownStreamCmd(DaisyBuf, Len, Layer);
                }
                else
                {
                    GH.Lms.SetObjectIp(TmpIp - 1);
                    DspStat = DSPSTAT.BUSYBREAK;
                }
            }
            GH.Lms.SetDispatchStatus(DspStat);
        }


        /*! \page cOutput
         *  <hr size="1"/>
         *  <b>     opOUTPUT_START (LAYER, NOS)  </b>
         *
         *- Starts the outputs\n
         *- Dispatch status unchanged
         *
         *  \param  (DATA8)   LAYER   - Chain layer number [0..3]
         *  \param  (DATA8)   NOS     - Output bit field [0x00..0x0F]
         */
        /*! \brief  opOUTPUT_START byte code
         *
         */
        public void cOutputStart()
        {
            DATA8 Tmp;
            DATA8 Layer;
            UBYTE Nos;
            DATA8 Len;
            DSPSTAT DspStat = DSPSTAT.NOBREAK;
            IP TmpIp;

            DATA8* StartMotor = CommonHelper.Pointer1d<DATA8>(2);

            TmpIp = GH.Lms.GetObjectIp();
            Len = 0;
            Layer = *(DATA8*)GH.Lms.PrimParPointer();
            Nos = *(UBYTE*)GH.Lms.PrimParPointer();

            if (Layer == 0)
            {
                unchecked
                {
                    StartMotor[0] = (DATA8)opOUTPUT_START;
                }
                unchecked
                {
                    StartMotor[1] = (sbyte)Nos;
                }

                GH.Ev3System.OutputHandler.WritePwmData(CommonHelper.GetArray((byte*)StartMotor, 2));

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
                if (GH.Daisy.cDaisyReady() != RESULT.BUSY)
                {
                    DaisyBuf[Len++] = 0;
                    DaisyBuf[Len++] = 0;
                    unchecked
                    {
                        DaisyBuf[Len++] = (sbyte)opOUTPUT_START;
                    }
                    Len += (sbyte)cOutputPackParam((DATA32)0, &(DaisyBuf[Len]));
                    Len += (sbyte)cOutputPackParam((DATA32)Nos, &(DaisyBuf[Len]));
                    if (OK != GH.Daisy.cDaisyDownStreamCmd(DaisyBuf, Len, Layer))
                    {
                        GH.Lms.SetObjectIp(TmpIp - 1);
                        DspStat = DSPSTAT.BUSYBREAK;
                    }
                    else
                    {
                        //printf("cOutPut @ opOUTPUT_START after GH.Daisy.cDaisyDownStreamCmd - OK and WriteState = %d\n\r", cDaisyGetLastWriteState());
                    }
                    //GH.Daisy.cDaisyDownStreamCmd(DaisyBuf, Len, Layer);

                }
                else
                {
                    GH.Lms.SetObjectIp(TmpIp - 1);
                    DspStat = DSPSTAT.BUSYBREAK;
                }
            }
            GH.Lms.SetDispatchStatus(DspStat);
        }


        /*! \page cOutput Output
         *  <hr size="1"/>
         *  <b>     opOUTPUT_POLARITY (LAYER, NOS, POL)  </b>
         *
         *- Set polarity of the outputs\n
         *- Dispatch status unchanged
         *
         *  \param  (DATA8)   LAYER   - Chain layer number [0..3]
         *  \param  (DATA8)   NOS     - Output bit field [0x00..0x0F]
         *  \param  (DATA8)   POL     - Polarity [-1,0,1]
         *
         *-  Polarity:
         *   - -1 makes the motor run backward
         *   -  1 makes the motor run forward
         *   -  0 makes the motor run the opposite direction
         */
        /*! \brief  opOUTPUT_POLARITY byte code
         *
         */
        public void cOutputPolarity()
        {
            DATA8 Layer;
            DATA8* Polarity = CommonHelper.Pointer1d<DATA8>(3);
            UBYTE Len;
            DSPSTAT DspStat = DSPSTAT.NOBREAK;
            IP TmpIp;

            TmpIp = GH.Lms.GetObjectIp();
            Len = 0;
            Layer = *(DATA8*)GH.Lms.PrimParPointer();
            unchecked
            {
                Polarity[0] = (DATA8)opOUTPUT_POLARITY;
            }
            Polarity[1] = *(DATA8*)GH.Lms.PrimParPointer();
            Polarity[2] = *(DATA8*)GH.Lms.PrimParPointer();

            if (Layer == 0)
            {
                GH.Ev3System.OutputHandler.WritePwmData(CommonHelper.GetArray((byte*)Polarity, 3));
            }
            else
            {
                if (GH.Daisy.cDaisyReady() != RESULT.BUSY)
                {
                    DaisyBuf[Len++] = 0;
                    DaisyBuf[Len++] = 0;
                    unchecked
                    {
                        DaisyBuf[Len++] = (sbyte)opOUTPUT_POLARITY;
                    }
                    Len += cOutputPackParam((DATA32)0, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)Polarity[1], &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)Polarity[2], &(DaisyBuf[Len]));
                    if (OK != GH.Daisy.cDaisyDownStreamCmd(DaisyBuf, (sbyte)Len, Layer))
                    {
                        GH.Lms.SetObjectIp(TmpIp - 1);
                        DspStat = DSPSTAT.BUSYBREAK;
                    }
                    //GH.Daisy.cDaisyDownStreamCmd(DaisyBuf, Len, Layer);
                }
                else
                {
                    GH.Lms.SetObjectIp(TmpIp - 1);
                    DspStat = DSPSTAT.BUSYBREAK;
                }
            }
            GH.Lms.SetDispatchStatus(DspStat);
        }


        /*! \page cOutput Output
         *  <hr size="1"/>
         *  <b>     opOUTPUT_STEP_POWER (LAYER, NOS, POWER, STEP1, STEP2, STEP3, BRAKE)  </b>
         *
         *- Set Ramp up, constant and rampdown steps and power of the outputs\n
         *- Dispatch status unchanged
         *
         *  \param  (DATA8)   LAYER   - Chain layer number [0..3]
         *  \param  (DATA8)   NOS     - Output bit field [0x00..0x0F]
         *  \param  (DATA8)   POWER   - Power [-100..100]
         *  \param  (DATA32)  STEP1   - Tacho pulses [0..MAX]
         *  \param  (DATA32)  STEP2   - Tacho pulses [0..MAX]
         *  \param  (DATA32)  STEP3   - Tacho pulses [0..MAX]
         *  \param  (DATA8)   BRAKE   - 0 = Coast, 1 = BRAKE
         */
        /*! \brief  opOUTPUT_STEP_POWER byte code
         *
         */
        public void cOutputStepPower()
        {
            DATA8 Layer;
            DATA8 Tmp;
            STEPPOWER StepPower;
            UBYTE Len;
            DSPSTAT DspStat = DSPSTAT.NOBREAK;
            IP TmpIp;

            TmpIp = GH.Lms.GetObjectIp();
            Len = 0;
            Layer = *(DATA8*)GH.Lms.PrimParPointer();
            unchecked
            {
                StepPower.Cmd = (sbyte)opOUTPUT_STEP_POWER;
            }
            StepPower.Nos = *(DATA8*)GH.Lms.PrimParPointer();
            StepPower.Power = *(DATA8*)GH.Lms.PrimParPointer();
            StepPower.Step1 = *(DATA32*)GH.Lms.PrimParPointer();
            StepPower.Step2 = *(DATA32*)GH.Lms.PrimParPointer();
            StepPower.Step3 = *(DATA32*)GH.Lms.PrimParPointer();
            StepPower.Brake = *(DATA8*)GH.Lms.PrimParPointer();

            if (0 == Layer)
            {
                GH.Ev3System.OutputHandler.WritePwmData(CommonHelper.GetArray((byte*)(DATA8*)&(StepPower.Cmd), sizeof(STEPPOWER)));

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
                if (GH.Daisy.cDaisyReady() != RESULT.BUSY)
                {
                    DaisyBuf[Len++] = 0;
                    DaisyBuf[Len++] = 0;
                    unchecked
                    {
                        DaisyBuf[Len++] = (sbyte)opOUTPUT_STEP_POWER;
                    }
                    Len += cOutputPackParam((DATA32)0, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)StepPower.Nos, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)StepPower.Power, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)StepPower.Step1, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)StepPower.Step2, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)StepPower.Step3, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)StepPower.Brake, &(DaisyBuf[Len]));

                    //if(OK != GH.Daisy.cDaisyDownStreamCmd(DaisyBuf, Len, Layer))
                    if (OK != GH.Daisy.cDaisyMotorDownStream(DaisyBuf, (sbyte)Len, Layer, StepPower.Nos))
                    {
                        GH.Lms.SetObjectIp(TmpIp - 1);
                        DspStat = DSPSTAT.BUSYBREAK;
                    }

                    //GH.Daisy.cDaisyMotorDownStream(DaisyBuf, Len, Layer, StepPower.Nos);

                }
                else
                {
                    GH.Lms.SetObjectIp(TmpIp - 1);
                    DspStat = DSPSTAT.BUSYBREAK;
                }
            }
            GH.Lms.SetDispatchStatus(DspStat);
        }


        /*! \page cOutput Output
         *  <hr size="1"/>
         *  <b>     opOUTPUT_TIME_POWER (LAYER, NOS, POWER, TIME1, TIME2, TIME3, BRAKE)  </b>
         *
         *- Set Ramp up, constant and rampdown steps and power of the outputs\n
         *- Dispatch status unchanged
         *
         *  \param  (DATA8)   LAYER   - Chain layer number [0..3]
         *  \param  (DATA8)   NOS     - Output bit field [0x00..0x0F]
         *  \param  (DATA8)   POWER   - Power [-100..100]
         *  \param  (DATA32)  TIME1   - Time in Ms [0..MAX]
         *  \param  (DATA32)  TIME2   - Time in Ms [0..MAX]
         *  \param  (DATA32)  TIME3   - Time in Ms [0..MAX]
         *  \param  (DATA8)   BRAKE   - 0 = Coast, 1 = BRAKE
         */
        /*! \brief  opOUTPUT_TIME_POWER byte code
         *
         */
        public void cOutputTimePower()
        {
            DATA8 Layer;
            DATA8 Tmp;
            TIMEPOWER TimePower;
            UBYTE Len;
            DSPSTAT DspStat = DSPSTAT.NOBREAK;
            IP TmpIp;

            TmpIp = GH.Lms.GetObjectIp();
            Len = 0;
            Layer = *(DATA8*)GH.Lms.PrimParPointer();
            unchecked
            {
                TimePower.Cmd = (sbyte)opOUTPUT_TIME_POWER;
            }
            TimePower.Nos = *(DATA8*)GH.Lms.PrimParPointer();
            TimePower.Power = *(DATA8*)GH.Lms.PrimParPointer();
            TimePower.Time1 = *(DATA32*)GH.Lms.PrimParPointer();
            TimePower.Time2 = *(DATA32*)GH.Lms.PrimParPointer();
            TimePower.Time3 = *(DATA32*)GH.Lms.PrimParPointer();
            TimePower.Brake = *(DATA8*)GH.Lms.PrimParPointer();

            if (0 == Layer)
            {
                GH.Ev3System.OutputHandler.WritePwmData(CommonHelper.GetArray((byte*)(DATA8*)&(TimePower.Cmd), sizeof(TIMEPOWER)));

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
                if (GH.Daisy.cDaisyReady() != RESULT.BUSY)
                {
                    DaisyBuf[Len++] = 0;
                    DaisyBuf[Len++] = 0;
                    unchecked
                    {
                        DaisyBuf[Len++] = (sbyte)opOUTPUT_TIME_POWER;
                    }
                    Len += cOutputPackParam((DATA32)0, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)TimePower.Nos, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)TimePower.Power, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)TimePower.Time1, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)TimePower.Time2, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)TimePower.Time3, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)TimePower.Brake, &(DaisyBuf[Len]));
                    if (OK != GH.Daisy.cDaisyMotorDownStream(DaisyBuf, (sbyte)Len, Layer, TimePower.Nos))
                    {
                        GH.Lms.SetObjectIp(TmpIp - 1);
                        DspStat = DSPSTAT.BUSYBREAK;
                    }
                    //GH.Daisy.cDaisyMotorDownStream(DaisyBuf, Len, Layer, TimePower.Nos);
                }
                else
                {
                    GH.Lms.SetObjectIp(TmpIp - 1);
                    DspStat = DSPSTAT.BUSYBREAK;
                }
            }
            GH.Lms.SetDispatchStatus(DspStat);
        }

        /*! \page cOutput Output
         *  <hr size="1"/>
         *  <b>     opOUTPUT_STEP_SPEED (LAYER, NOS, SPEED, STEP1, STEP2, STEP3, BRAKE)  </b>
         *
         *- Set Ramp up, constant and rampdown steps and power of the outputs\n
         *- Dispatch status unchanged
         *
         *  \param  (DATA8)   LAYER   - Chain layer number [0..3]
         *  \param  (DATA8)   NOS     - Output bit field [0x00..0x0F]
         *  \param  (DATA8)   SPEED   - Power [-100..100]
         *  \param  (DATA32)  STEP1   - Tacho pulses [0..MAX]
         *  \param  (DATA32)  STEP2   - Tacho pulses [0..MAX]
         *  \param  (DATA32)  STEP3   - Tacho pulses [0..MAX]
         *  \param  (DATA8)   BRAKE   - 0 = Coast, 1 = BRAKE
         */
        /*! \brief  opOUTPUT_STEP_SPEED byte code
         *
         */
        public void cOutputStepSpeed()
        {
            DATA8 Layer;
            DATA8 Tmp;
            STEPSPEED StepSpeed;
            UBYTE Len;
            DSPSTAT DspStat = DSPSTAT.NOBREAK;
            IP TmpIp;

            //DEBUG
            //  int i;

            TmpIp = GH.Lms.GetObjectIp();
            Len = 0;
            Layer = *(DATA8*)GH.Lms.PrimParPointer();
            unchecked
            {
                StepSpeed.Cmd = (sbyte)opOUTPUT_STEP_SPEED;
            }
            StepSpeed.Nos = *(DATA8*)GH.Lms.PrimParPointer();
            StepSpeed.Speed = *(DATA8*)GH.Lms.PrimParPointer();
            StepSpeed.Step1 = *(DATA32*)GH.Lms.PrimParPointer();
            StepSpeed.Step2 = *(DATA32*)GH.Lms.PrimParPointer();
            StepSpeed.Step3 = *(DATA32*)GH.Lms.PrimParPointer();
            StepSpeed.Brake = *(DATA8*)GH.Lms.PrimParPointer();
            /*
              printf("StepSpeed.Cmd = %d\n\r", StepSpeed.Cmd);
              printf("StepSpeed.Nos = %d\n\r", StepSpeed.Nos);
              printf("StepSpeed.Speed = %d\n\r", StepSpeed.Speed);
              printf("StepSpeed.Step1 = %d\n\r", StepSpeed.Step1);
              printf("StepSpeed.Step2 = %d\n\r", StepSpeed.Step2);
              printf("StepSpeed.Step3 = %d\n\r", StepSpeed.Step3);
              printf("StepSpeed.Brake = %d\n\r", StepSpeed.Brake);

            */
            if (0 == Layer)
            {
                GH.Ev3System.OutputHandler.WritePwmData(CommonHelper.GetArray((byte*)(DATA8*)&(StepSpeed.Cmd), sizeof(STEPSPEED)));

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
                if (GH.Daisy.cDaisyReady() != RESULT.BUSY)
                {
                    DaisyBuf[Len++] = 0;
                    DaisyBuf[Len++] = 0;
                    unchecked
                    {
                        DaisyBuf[Len++] = (sbyte)opOUTPUT_STEP_SPEED;
                    }
                    Len += cOutputPackParam((DATA32)0, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)StepSpeed.Nos, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)StepSpeed.Speed, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)StepSpeed.Step1, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)StepSpeed.Step2, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)StepSpeed.Step3, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)StepSpeed.Brake, &(DaisyBuf[Len]));

                    /* printf("Len = %d\n\r", Len);
                     for(i = 0; i < Len; i++)
                                         printf("DaisyBuf[%d]= %x\n\r", i, DaisyBuf[i]);
                                       printf("\n\r");
             */

                    if (OK != GH.Daisy.cDaisyMotorDownStream(DaisyBuf, (sbyte)Len, Layer, StepSpeed.Nos))
                    {
                        GH.printf("NOT ok txed cOutputStepSpeed\n\r");
                        GH.Lms.SetObjectIp(TmpIp - 1);
                        DspStat = DSPSTAT.BUSYBREAK;
                    }
                    else
                    {
                        /*        for(i = 0; i < Len; i++)
                                          printf("DaisyBuf[%d]= %d, ", i, DaisyBuf[i]);
                                        printf("\n\r");
                      */
                    }
                    //GH.Daisy.cDaisyMotorDownStream(DaisyBuf, Len, Layer, StepSpeed.Nos);
                }
                else
                {
                    GH.Lms.SetObjectIp(TmpIp - 1);
                    DspStat = DSPSTAT.BUSYBREAK;
                }
            }
            GH.Lms.SetDispatchStatus(DspStat);
        }


        /*! \page cOutput Output
         *  <hr size="1"/>
         *  <b>     opOUTPUT_TIME_SPEED (LAYER, NOS, SPEED, STEP1, STEP2, STEP3, BRAKE)  </b>
         *
         *- Set Ramp up, constant and rampdown steps and power of the outputs\n
         *- Dispatch status unchanged
         *
         *  \param  (DATA8)   LAYER   - Chain layer number [0..3]
         *  \param  (DATA8)   NOS     - Output bit field [0x00..0x0F]
         *  \param  (DATA8)   SPEED   - Power [-100..100]
         *  \param  (DATA32)  STEP1   - Time in mS [0..MAX]
         *  \param  (DATA32)  STEP2   - Time in mS [0..MAX]
         *  \param  (DATA32)  STEP3   - Time in mS [0..MAX]
         *  \param  (DATA8)   BRAKE   - 0 = Coast, 1 = BRAKE
         */
        /*! \brief  opOUTPUT_TIME_SPEED byte code
         *
         */
        public void cOutputTimeSpeed()
        {
            DATA8 Layer;
            DATA8 Tmp;
            TIMESPEED TimeSpeed;
            UBYTE Len;
            DSPSTAT DspStat = DSPSTAT.NOBREAK;
            IP TmpIp;

            TmpIp = GH.Lms.GetObjectIp();
            Len = 0;
            Layer = *(DATA8*)GH.Lms.PrimParPointer();
            unchecked
            {
                TimeSpeed.Cmd = (DATA8)opOUTPUT_TIME_SPEED;
            }
            TimeSpeed.Nos = *(DATA8*)GH.Lms.PrimParPointer();
            TimeSpeed.Speed = *(DATA8*)GH.Lms.PrimParPointer();
            TimeSpeed.Time1 = *(DATA32*)GH.Lms.PrimParPointer();
            TimeSpeed.Time2 = *(DATA32*)GH.Lms.PrimParPointer();
            TimeSpeed.Time3 = *(DATA32*)GH.Lms.PrimParPointer();
            TimeSpeed.Brake = *(DATA8*)GH.Lms.PrimParPointer();

            if (0 == Layer)
            {
                GH.Ev3System.OutputHandler.WritePwmData(CommonHelper.GetArray((byte*)(DATA8*)&(TimeSpeed.Cmd), sizeof(TIMESPEED)));

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
                if (GH.Daisy.cDaisyReady() != RESULT.BUSY)
                {
                    DaisyBuf[Len++] = 0;
                    DaisyBuf[Len++] = 0;
                    unchecked
                    {
                        DaisyBuf[Len++] = (sbyte)opOUTPUT_TIME_SPEED;
                    }
                    Len += cOutputPackParam((DATA32)0, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)TimeSpeed.Nos, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)TimeSpeed.Speed, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)TimeSpeed.Time1, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)TimeSpeed.Time2, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)TimeSpeed.Time3, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)TimeSpeed.Brake, &(DaisyBuf[Len]));
                    if (OK != GH.Daisy.cDaisyMotorDownStream(DaisyBuf, (sbyte)Len, Layer, TimeSpeed.Nos))
                    {
                        GH.Lms.SetObjectIp(TmpIp - 1);
                        DspStat = DSPSTAT.BUSYBREAK;
                    }
                    //GH.Daisy.cDaisyMotorDownStream(DaisyBuf, Len, Layer, TimeSpeed.Nos);
                }
                else
                {
                    GH.Lms.SetObjectIp(TmpIp - 1);
                    DspStat = DSPSTAT.BUSYBREAK;
                }
            }
            GH.Lms.SetDispatchStatus(DspStat);
        }


        /*! \page cOutput Output
         *  <hr size="1"/>
         *  <b>     opOUTPUT_STEP_SYNC (LAYER, NOS, SPEED, TURN, STEP, BRAKE)  </b>
         *
         *- \n
         *- Dispatch status unchanged
         *
         *  \param  (DATA8)   LAYER   - Chain layer number [0..3]
         *  \param  (DATA8)   NOS     - Output bit field   [0x00..0x0F]
         *  \param  (DATA8)   SPEED   - Power              [-100..100]
         *  \param  (DATA16)  TURN    - Turn Ratio         [-200..200]
         *  \param  (DATA32)  STEP    - Tacho Pulses       [0..MAX]
         *  \param  (DATA8)   BRAKE   - 0 = Coast, 1 = BRAKE
         */
        /*! \brief  opOUTPUT_STEP_SYNC byte code
         *
         */
        public void cOutputStepSync()
        {
            DATA8 Layer;
            DATA8 Tmp;
            STEPSYNC StepSync;
            UBYTE Len;
            DSPSTAT DspStat = DSPSTAT.NOBREAK;
            IP TmpIp;

            TmpIp = GH.Lms.GetObjectIp();
            Len = 0;
            Layer = *(DATA8*)GH.Lms.PrimParPointer();
            unchecked
            {
                StepSync.Cmd = (sbyte)opOUTPUT_STEP_SYNC;
            }
            StepSync.Nos = *(DATA8*)GH.Lms.PrimParPointer();
            StepSync.Speed = *(DATA8*)GH.Lms.PrimParPointer();
            StepSync.Turn = *(DATA16*)GH.Lms.PrimParPointer();
            StepSync.Step = *(DATA32*)GH.Lms.PrimParPointer();
            StepSync.Brake = *(DATA8*)GH.Lms.PrimParPointer();

            if (0 == Layer)
            {
                GH.Ev3System.OutputHandler.WritePwmData(CommonHelper.GetArray((byte*)(DATA8*)&(StepSync.Cmd), sizeof(STEPSYNC)));

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
                if (GH.Daisy.cDaisyReady() != RESULT.BUSY)
                {
                    DaisyBuf[Len++] = 0;
                    DaisyBuf[Len++] = 0;
                    unchecked
                    {
                        DaisyBuf[Len++] = (sbyte)opOUTPUT_STEP_SYNC;
                    }
                    Len += cOutputPackParam((DATA32)0, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)StepSync.Nos, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)StepSync.Speed, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)StepSync.Turn, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)StepSync.Step, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)StepSync.Brake, &(DaisyBuf[Len]));
                    if (OK != GH.Daisy.cDaisyMotorDownStream(DaisyBuf, (sbyte)Len, Layer, StepSync.Nos))
                    {
                        GH.Lms.SetObjectIp(TmpIp - 1);
                        DspStat = DSPSTAT.BUSYBREAK;
                    }
                    //GH.Daisy.cDaisyMotorDownStream(DaisyBuf, Len, Layer, StepSync.Nos);
                }
                else
                {
                    GH.Lms.SetObjectIp(TmpIp - 1);
                    DspStat = DSPSTAT.BUSYBREAK;
                }
            }
            GH.Lms.SetDispatchStatus(DspStat);
        }


        /*! \page cOutput Output
         *  <hr size="1"/>
         *  <b>     opOUTPUT_TIME_SYNC (LAYER, NOS, SPEED, TURN, STEP, BRAKE)  </b>
         *
         *- \n
         *- Dispatch status unchanged
         *
         *  \param  (DATA8)   LAYER   - Chain layer number [0..3]
         *  \param  (DATA8)   NOS     - Output bit field   [0x00..0x0F]
         *  \param  (DATA8)   SPEED   - Power              [-100..100]
         *  \param  (DATA16)  TURN    - Turn Ratio         [-200..200]
         *  \param  (DATA32)  TIME    - Time in ms         [0..MAX]
         *  \param  (DATA8)   BRAKE   - 0 = Coast, 1 = BRAKE
         *
         */
        /*! \brief  opOUTPUT_STEP_SYNC byte code
         *
         */
        public void cOutputTimeSync()
        {
            DATA8 Layer;
            DATA8 Tmp;
            TIMESYNC TimeSync;
            UBYTE Len;
            DSPSTAT DspStat = DSPSTAT.NOBREAK;
            IP TmpIp;

            TmpIp = GH.Lms.GetObjectIp();
            Len = 0;
            Layer = *(DATA8*)GH.Lms.PrimParPointer();
            unchecked
            {
                TimeSync.Cmd = (sbyte)opOUTPUT_TIME_SYNC;
            }
            TimeSync.Nos = *(DATA8*)GH.Lms.PrimParPointer();
            TimeSync.Speed = *(DATA8*)GH.Lms.PrimParPointer();
            TimeSync.Turn = *(DATA16*)GH.Lms.PrimParPointer();
            TimeSync.Time = *(DATA32*)GH.Lms.PrimParPointer();
            TimeSync.Brake = *(DATA8*)GH.Lms.PrimParPointer();

            if (0 == Layer)
            {
                GH.Ev3System.OutputHandler.WritePwmData(CommonHelper.GetArray((byte*)(DATA8*)&(TimeSync.Cmd), sizeof(TIMESYNC)));

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
                if (GH.Daisy.cDaisyReady() != RESULT.BUSY)
                {
                    DaisyBuf[Len++] = 0;
                    DaisyBuf[Len++] = 0;
                    unchecked
                    {
                        DaisyBuf[Len++] = (sbyte)opOUTPUT_TIME_SYNC;
                    }
                    Len += cOutputPackParam((DATA32)0, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)TimeSync.Nos, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)TimeSync.Speed, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)TimeSync.Turn, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)TimeSync.Time, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)TimeSync.Brake, &(DaisyBuf[Len]));
                    if (OK != GH.Daisy.cDaisyMotorDownStream(DaisyBuf, (sbyte)Len, Layer, TimeSync.Nos))
                    {
                        GH.Lms.SetObjectIp(TmpIp - 1);
                        DspStat = DSPSTAT.BUSYBREAK;
                    }
                    //GH.Daisy.cDaisyMotorDownStream(DaisyBuf, Len, Layer, TimeSync.Nos);
                }
                else
                {
                    GH.Lms.SetObjectIp(TmpIp - 1);
                    DspStat = DSPSTAT.BUSYBREAK;
                }
            }
            GH.Lms.SetDispatchStatus(DspStat);
        }


        /*! \page   cOutput
         *  <hr size="1"/>
         *  <b>     opOUTPUT_READ (LAYER, NO, *SPEED, *TACHO) </b>
         *
         *  \param  (DATA8)   LAYER   - Chain layer number [0..3]
         *  \param  (DATA8)   NO      - Output no [0..3]
         *  \param  (DATA8)   *SPEED  - Speed [-100..100]
         *  \param  (DATA32)  *TACHO  - Tacho pulses [-MAX .. +MAX]
         *
         */
        /*! \brief  opOUTPUT_READ byte code
         *
         */
        public void cOutputRead()
        {

            DATA8 Layer;
            DATA8 No;
            DATA8 Speed = 0;
            DATA32 Tacho = 0;

            Layer = *(DATA8*)GH.Lms.PrimParPointer();
            No = *(DATA8*)GH.Lms.PrimParPointer();

            if (0 == Layer)
            {
                if (No < OUTPUTS)
                {
                    Speed = GH.OutputInstance.pMotor[No].Speed;
                    Tacho = GH.OutputInstance.pMotor[No].TachoCounts;
                }
            }
            *(DATA8*)GH.Lms.PrimParPointer() = Speed;
            *(DATA32*)GH.Lms.PrimParPointer() = Tacho;
        }



        /*! \page   cOutput
         *  <hr size="1"/>
         *  <b>     opOUTPUT_READY (LAYER, NOS) </b>
         *
         *- Wait for output ready (wait for completion)\n
         *- Dispatch status can change to DSPSTAT.BUSYBREAK
         *- cOUTPUT_START command has no effect on this command
         *
         *  \param  (DATA8)   LAYER   - Chain layer number [0..3]
         *  \param  (DATA8)   NOS     - Output bit field [0x00..0x0F]
         *
         *
         */
        /*! \brief  opOUTPUT_READY byte code
         *
         */
        public void cOutputReady()
        {

            OBJID Owner;
            DATA8 Layer, Tmp, Nos;
            IP TmpIp;
            DSPSTAT DspStat = DSPSTAT.NOBREAK;
            UBYTE Bits;

            int test;
            int test2;

            DATA8* BusyReturn = CommonHelper.Pointer1d<DATA8>(10); // Busy mask

            TmpIp = GH.Lms.GetObjectIp();

            Layer = *(DATA8*)GH.Lms.PrimParPointer();
            Nos = *(DATA8*)GH.Lms.PrimParPointer();
            Owner = GH.Lms.CallingObjectId();

            if (0 == Layer)
            {
                var tmp = GH.Ev3System.OutputHandler.GetMotorBusyFlags();
                test = tmp.Item1;
                test2 = tmp.Item2;

                for (Tmp = 0; (Tmp < OUTPUTS) && (DspStat == DSPSTAT.NOBREAK); Tmp++)
                {
                    // Check resources for NOTREADY
                    if ((Nos & (1 << Tmp)) != 0)
                    {
                        // Only relevant ones
                        if ((test & (1 << Tmp)) != 0)
                        {
                            // If RESULT.BUSY check for OVERRULED
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
                Bits = GH.Daisy.cDaisyCheckBusyBit((byte)Layer, (byte)Nos);
                Bits = 0;

                for (Tmp = 0; (Tmp < OUTPUTS) && (DspStat == DSPSTAT.NOBREAK); Tmp++)
                {
                    // Check resources for NOTREADY
                    if ((Nos & (1 << Tmp)) != 0)
                    {
                        // Only relevant ones
                        if ((Bits & (1 << Tmp)) != 0)
                        {
                            // If RESULT.BUSY check for OVERRULED
                            if (GH.OutputInstance.Owner[Tmp] == Owner)
                            {
                                DspStat = DSPSTAT.BUSYBREAK;
                            }
                        }
                    }
                }
            }

            if (DspStat == DSPSTAT.BUSYBREAK)
            {
                // Rewind IP
                GH.Lms.SetObjectIp(TmpIp - 1);
            }
            GH.Lms.SetDispatchStatus(DspStat);
        }


        /*! \page   cOutput
         *  \anchor opOUTPUT_TEST \n
         *  <hr size="1"/>
         *  <b>     opOUTPUT_TEST (LAYER, NOS, RESULT.BUSY) </b>
         *
         *- Testing if output is not used \n
         *
         *
         *  \param  (DATA8)   LAYER   - Chain layer number [0..3]
         *  \param  (DATA8)   NOS     - Output bit field [0x00..0x0F]
         *  \return (DATA8)   RESULT.BUSY    - Output busy flag (0 = ready, 1 = Busy)
         */
        /*! \brief  opOUTPUT_TEST byte code
         *
         */
        public void cOutputTest()
        {

            DATA8 Layer, Nos, Busy = 0;

            int test;
            int test2;

            DATA8* BusyReturn = CommonHelper.Pointer1d<DATA8>(20); // Busy mask

            Layer = *(DATA8*)GH.Lms.PrimParPointer();
            Nos = *(DATA8*)GH.Lms.PrimParPointer();

            if (0 == Layer)
            {
                var tmp = GH.Ev3System.OutputHandler.GetMotorBusyFlags();
                test = tmp.Item1;
                test2 = tmp.Item2;

                if ((Nos & (DATA8)test2) != 0)
                {
                    Busy = 1;
                }
            }
            else
            {
                if (GH.Daisy.cDaisyCheckBusyBit((byte)Layer, (byte)Nos) != 0)
                {
                    Busy = 1;
                }
            }
            *(DATA8*)GH.Lms.PrimParPointer() = Busy;
        }


        /*! \page   cOutput
         *  \anchor opOUTPUT_TEST \n
         *  <hr size="1"/>
         *  <b>     opOUTPUT_CLR_COUNT (LAYER, NOS) </b>
         *
         *- Clearing tacho count when used as sensor \n
         *
         *
         *  \param  (DATA8)   LAYER   - Chain layer number [0..3]
         *  \param  (DATA8)   NOS     - Output bit field [0x00..0x0F]
         */
        /*! \brief  opOUTPUT_CLR_COUNT byte code
         *
         */
        public void cOutputClrCount()
        {
            DATA8 Layer;
            DATA8* ClrCnt = CommonHelper.Pointer1d<DATA8>(2);
            UBYTE Len;
            DSPSTAT DspStat = DSPSTAT.NOBREAK;
            IP TmpIp;
            UBYTE Tmp;

            TmpIp = GH.Lms.GetObjectIp();
            Len = 0;
            Layer = *(DATA8*)GH.Lms.PrimParPointer();
            unchecked
            {
                ClrCnt[0] = (sbyte)opOUTPUT_CLR_COUNT;
            }
            ClrCnt[1] = *(DATA8*)GH.Lms.PrimParPointer();

            if (0 == Layer)
            {
                GH.Ev3System.OutputHandler.WritePwmData(CommonHelper.GetArray((byte*)ClrCnt, 2));

                // Also the user layer entry to get immediate clear
                for (Tmp = 0; Tmp < OUTPUTS; Tmp++)
                {
                    if ((ClrCnt[1] & (1 << Tmp)) != 0)
                    {
                        GH.OutputInstance.pMotor[Tmp].TachoSensor = 0;
                    }
                }
            }
            else
            {
                if (GH.Daisy.cDaisyReady() != RESULT.BUSY)
                {
                    DaisyBuf[Len++] = 0;
                    DaisyBuf[Len++] = 0;
                    unchecked
                    {
                        DaisyBuf[Len++] = (sbyte)opOUTPUT_CLR_COUNT;
                    }
                    Len += cOutputPackParam((DATA32)0, &(DaisyBuf[Len]));
                    Len += cOutputPackParam((DATA32)ClrCnt[1], &(DaisyBuf[Len]));
                    if (OK != GH.Daisy.cDaisyDownStreamCmd(DaisyBuf, (sbyte)Len, Layer))
                    {
                        GH.Lms.SetObjectIp(TmpIp - 1);
                        DspStat = DSPSTAT.BUSYBREAK;
                    }
                    //GH.Daisy.cDaisyDownStreamCmd(DaisyBuf, Len, Layer);
                }
                else
                {
                    GH.Lms.SetObjectIp(TmpIp - 1);
                    DspStat = DSPSTAT.BUSYBREAK;
                }
            }
            GH.Lms.SetDispatchStatus(DspStat);
        }


        /*! \page   cOutput
         *  \anchor opOUTPUT_TEST \n
         *  <hr size="1"/>
         *  <b>     opOUTPUT_GET_COUNT (LAYER, NOS, *TACHO) </b>
         *
         *- Getting tacho count when used as sensor - values are in shared memory \n
         *
         *
         *  \param  (DATA8)   LAYER   - Chain layer number [0..3]
         *  \param  (DATA8)   NOS     - Output number [0x00..0x0F]
         *  \param  (DATA32)  *TACHO  - Tacho pulses [-MAX .. +MAX]
         */
        /*! \brief  opOUTPUT_GET_COUNT byte code
         *
         */
        public void cOutputGetCount()
        {
            DATA8 Layer;
            DATA8 No;
            DATA32 Tacho = 0;

            Layer = *(DATA8*)GH.Lms.PrimParPointer();
            No = *(DATA8*)GH.Lms.PrimParPointer();

            if (0 == Layer)
            {
                if (No < OUTPUTS)
                {
                    Tacho = GH.OutputInstance.pMotor[No].TachoSensor;
                }
            }
            *(DATA32*)GH.Lms.PrimParPointer() = Tacho;
        }
    }
}
