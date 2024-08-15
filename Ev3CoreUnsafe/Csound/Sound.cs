using Ev3CoreUnsafe.Csound.Interfaces;
using Ev3CoreUnsafe.Enums;
using Ev3CoreUnsafe.Extensions;
using Ev3CoreUnsafe.Helpers;
using Ev3CoreUnsafe.Lms2012.Interfaces;
using System.Runtime.CompilerServices;
using static Ev3CoreUnsafe.Defines;

namespace Ev3CoreUnsafe.Csound
{
	public unsafe class Sound : ISound
	{
		public RESULT cSoundInit()
		{
			RESULT Result = RESULT.FAIL;
			SOUND* pSoundTmp;
			int SndFile;

			GH.SoundInstance.SoundDriverDescriptor = -1;
			GH.SoundInstance.hSoundFile = null;
			GH.SoundInstance.pSound = (SOUND*)GH.SoundInstance.Sound;

			GH.Ev3System.SoundHandler.DonePlaying += OnSoundDonePlaying;

            // TODO: file shite
            // Create a Shared Memory entry for signaling the driver state BUSY or NOT BUSY
            //SndFile = open(SOUND_DEVICE_NAME, O_RDWR | O_SYNC);

            //if (SndFile >= 0)
            //{
            //	pSoundTmp = (SOUND*)mmap(0, sizeof(UWORD), PROT_READ | PROT_WRITE, MAP_FILE | MAP_SHARED, SndFile, 0);

            //	if (pSoundTmp == MAP_FAILED)
            //	{
            //		LogErrorNumber(SOUND_SHARED_MEMORY);
            //	}
            //	else
            //	{
            //		GH.SoundInstance.pSound = pSoundTmp;
            //		Result = OK;
            //	}

            //	close(SndFile);
            //}

            Result = OK;

			return (Result);
		}

		private void OnSoundDonePlaying()
		{
			(*GH.SoundInstance.pSound).Status = OK;
        }

		public RESULT cSoundOpen()
		{
			RESULT Result = RESULT.FAIL;

			Result = OK;

			return (Result);
		}

		public RESULT cSoundClose()
		{
			RESULT Result = RESULT.FAIL;
			UWORD BytesToWrite;
			DATA8* SoundData = CommonHelper.Pointer1d<DATA8>(SOUND_FILE_BUFFER_SIZE + 1); // Add up for CMD

			SoundData[0] = BREAK;
			BytesToWrite = 1;
			GH.Ev3System.SoundHandler.PlayChunk(CommonHelper.GetArray((byte*)SoundData, BytesToWrite));
			GH.SoundInstance.SoundDriverDescriptor = -1;

			GH.SoundInstance.cSoundState = SOUND_STOPPED;

			Result = OK;

			return (Result);
		}

		public void cSoundInitAdPcm()
		{
			// Reset ADPCM values to a known and initial value
			GH.SoundInstance.ValPrev = SOUND_ADPCM_INIT_VALPREV;
			GH.SoundInstance.Index = SOUND_ADPCM_INIT_INDEX;
			GH.SoundInstance.Step = StepSizeTable[GH.SoundInstance.Index];
		}

		public UBYTE cSoundGetAdPcmValue(UBYTE Delta)  // Call ONLY when cSoundInitAdPcm has been called :-)
		{
			SWORD VpDiff;
			UBYTE Sign;

			//
			GH.SoundInstance.Step = StepSizeTable[GH.SoundInstance.Index];
			GH.SoundInstance.Index += IndexTable[Delta]; // Find new index value (for later)

			if (GH.SoundInstance.Index < 0)
				GH.SoundInstance.Index = 0;
			else
			{
				if (GH.SoundInstance.Index > (STEP_SIZE_TABLE_ENTRIES - 1))
					GH.SoundInstance.Index = STEP_SIZE_TABLE_ENTRIES - 1;
			}

			Sign = (byte)(Delta & 8);                     // Separate sign
			Delta = (byte)(Delta & 7);                    // Separate magnitude

			VpDiff = (short)(GH.SoundInstance.Step >> 3);     // Compute difference and new predicted value

			if ((Delta & 4) != 0) VpDiff += GH.SoundInstance.Step;
			if ((Delta & 2) != 0) VpDiff += (short)(GH.SoundInstance.Step >> 1);
			if ((Delta & 1) != 0) VpDiff += (short)(GH.SoundInstance.Step >> 2);

			if (Sign != 0)
				GH.SoundInstance.ValPrev -= VpDiff;    // "Add" with sign
			else
				GH.SoundInstance.ValPrev += VpDiff;

			if (GH.SoundInstance.ValPrev > 255)       // Clamp value to 8-bit unsigned
			{
				GH.SoundInstance.ValPrev = 255;
			}
			else
			{
				if (GH.SoundInstance.ValPrev < 0)
				{
					GH.SoundInstance.ValPrev = 0;
				}
			}

			GH.SoundInstance.Step = StepSizeTable[GH.SoundInstance.Index];  // Update step value

			return ((UBYTE)GH.SoundInstance.ValPrev);                     // Return decoded byte (nibble xlated -> 8 bit)
		}

		public RESULT cSoundUpdate()
		{
			int BytesRead;
			int i;
			UBYTE* AdPcmData = CommonHelper.Pointer1d<UBYTE>(SOUND_ADPCM_CHUNK); // Temporary ADPCM input buffer
			UWORD BytesToRead;
			UBYTE BytesWritten = 0;
			RESULT Result = RESULT.FAIL;

			switch (GH.SoundInstance.cSoundState)
			{
				case SOUND_STOPPED:        // Do nothing
					break;

				case SOUND_SETUP_FILE:     // Keep hands off - should only appear, when needed... but...
					break;

				case SOUND_FILE_LOOPING:   // Make it looping!!
										   // Fall through

				case SOUND_FILE_PLAYING:
					if (GH.SoundInstance.BytesToWrite > 0)  // Do we have "NOT WRITTEN DATA"
					{
						// Yes, write the pending stuff to Driver
						BytesWritten = GH.Ev3System.SoundHandler.PlayChunk(CommonHelper.GetArray(GH.SoundInstance.SoundData, GH.SoundInstance.BytesToWrite));
						GH.SoundInstance.SoundDriverDescriptor = -1;
						Result = OK;
						// Adjust BytesToWrite with Bytes actually written
						if (BytesWritten > 1)
						{
							// TODO: is it ok?
							if (GH.SoundInstance.SoundFileFormat == FILEFORMAT_ADPCM_SOUND)
							{
								GH.SoundInstance.SoundDataLength -= (UBYTE)(BytesWritten / 2); // nibbles in file
								GH.SoundInstance.BytesToWrite -= (UBYTE)(BytesWritten + 1);   // Buffer data incl. CMD
							}
							else
							{
								GH.SoundInstance.SoundDataLength -= BytesWritten;
								GH.SoundInstance.BytesToWrite -= (UBYTE)(BytesWritten + 1);   // Buffer data incl. CMD
							}
						}
					}
					else  // Get new sound data
					{     // No pending stuff
						if (GH.SoundInstance.SoundDataLength > 0) // Any new data?
						{
							GH.SoundInstance.SoundData[0] = SERVICE;

							if (GH.SoundInstance.SoundFileFormat == FILEFORMAT_ADPCM_SOUND)
							{
								// TODO: careful - no adjust
								//// Adjust the chunk size for ADPCM (nibbles) if necessary
								//if (GH.SoundInstance.SoundDataLength > SOUND_ADPCM_CHUNK)
								//	BytesToRead = SOUND_ADPCM_CHUNK;
								//else
								BytesToRead = GH.SoundInstance.SoundDataLength;

								if (*GH.SoundInstance.hSoundFile >= 0)  // Valid file
								{
									using var hand = File.OpenRead(CommonHelper.GetString(GH.SoundInstance.hSoundFile));
									BytesRead = hand.ReadUnsafe(AdPcmData, 0, BytesToRead);

									for (i = 0; i < BytesRead; i++)
									{
										GH.SoundInstance.SoundData[2 * i + 1] = cSoundGetAdPcmValue((byte)((AdPcmData[i] >> 4) & 0x0F));
										GH.SoundInstance.SoundData[2 * i + 2] = cSoundGetAdPcmValue((byte)(AdPcmData[i] & 0x0F));
									}

									GH.SoundInstance.BytesToWrite = (UBYTE)(1 + BytesRead * 2);
								}
							}
							else // Non compressed data
							{
								// TODO: careful - no adjust
								//// Adjust the chunk size if necessary
								//if (GH.SoundInstance.SoundDataLength > SOUND_CHUNK)
								//	BytesToRead = SOUND_CHUNK;
								//else
								BytesToRead = GH.SoundInstance.SoundDataLength;

								if (GH.SoundInstance.hSoundFile != null)  // Valid file
								{
                                    // TODO: UNCOMMENT!!!!
                                    //using var hand = File.OpenRead(CommonHelper.GetString(GH.SoundInstance.hSoundFile));
                                    //BytesRead = hand.ReadUnsafe(&(GH.SoundInstance.SoundData[1]), 0, BytesToRead);

                                    //GH.SoundInstance.BytesToWrite = (byte)(BytesRead + 1);
                                    GH.SoundInstance.BytesToWrite = (byte)(BytesToRead + 1); // REMOVE!!!!!
                                }
							}
							// Now we have or should have some bytes to write down into the driver
							GH.Ev3System.SoundHandler.PlayChunk(CommonHelper.GetArray(GH.SoundInstance.SoundData, GH.SoundInstance.BytesToWrite));
							GH.SoundInstance.SoundDriverDescriptor = -1;
							Result = OK;

							// Adjust BytesToWrite with Bytes actually written
							if (BytesWritten > 1)
							{
								if (GH.SoundInstance.SoundFileFormat == FILEFORMAT_ADPCM_SOUND)
								{
									GH.SoundInstance.SoundDataLength -= (UBYTE)(BytesWritten / 2); // nibbles in file
									GH.SoundInstance.BytesToWrite -= (UBYTE)(BytesWritten + 1);   // Buffer data incl. CMD
								}
								else
								{
									GH.SoundInstance.SoundDataLength -= BytesWritten;
									GH.SoundInstance.BytesToWrite -= (UBYTE)(BytesWritten + 1);   // Buffer data incl. CMD
								}
							}

						} // end new data
						else  // Shut down the SOUND stuff until new request/data i.e.
						{     // SoundDataLength > 0

							if (GH.SoundInstance.cSoundState == SOUND_FILE_LOOPING)
							{
								GH.SoundInstance.SoundDataLength = (ushort)(new FileInfo(CommonHelper.GetString(GH.SoundInstance.hSoundFile))).Length;
								// TODO make a new write here, so no zero-"sound"

							}
							else
							{
								GH.SoundInstance.cSoundState = SOUND_STOPPED;

								if (*GH.SoundInstance.hSoundFile >= 0)
								{
									// close(GH.SoundInstance.hSoundFile);
									*GH.SoundInstance.hSoundFile = 0;
								}

								if (GH.SoundInstance.SoundDriverDescriptor >= 0)
								{
									// close(GH.SoundInstance.SoundDriverDescriptor);
									GH.SoundInstance.SoundDriverDescriptor = -1;
								}

							}

						}
					}   // No pending write
					break;

				case SOUND_TONE_PLAYING:   // Check for duration done in d_sound :-)

					if ((*GH.SoundInstance.pSound).Status == OK)
					{
						// DO the finished stuff
						GH.SoundInstance.cSoundState = SOUND_STOPPED;
						if (GH.SoundInstance.SoundDriverDescriptor >= 0)
						{
							// close(GH.SoundInstance.SoundDriverDescriptor);
							GH.SoundInstance.SoundDriverDescriptor = -1;
						}

						if (*GH.SoundInstance.hSoundFile >= 0)
						{
							// close(GH.SoundInstance.hSoundFile);
							*GH.SoundInstance.hSoundFile = 0;
						}

						Result = OK;
					}
					break;

				default:                    // Do nothing
					break;
			}
			return (Result);
		}

		public RESULT cSoundExit()
		{
			RESULT Result = RESULT.FAIL;

			Result = OK;

			return (Result);
		}

		//******* BYTE CODE SNIPPETS **************************************************

		/*! \page cSound Sound
		 *  <hr size="1"/>
		 *  <b>     opSOUND ()  </b>
		 *
		 *- Memory file entry\n
		 *- Dispatch status unchanged
		 *
		 *  \param  (DATA8)   CMD     - Specific command \ref soundsubcode
		 *
		 *  - CMD = BREAK\n
		 *
		 *\n
		 *  - CMD = TONE
		 *    -  \param  (DATA8)    VOLUME    - Volume [0..100]\n
		 *    -  \param  (DATA16)   FREQUENCY - Frequency [Hz]\n
		 *    -  \param  (DATA16)   DURATION  - Duration [mS]\n
		 *
		 *\n
		 *  - CMD = PLAY
		 *    -  \param  (DATA8)    VOLUME    - Volume [0..100]\n
		 *    -  \param  (DATA8)    NAME      - First character in filename (character string)\n
		 *
		 *\n
		 *  - CMD = REPEAT
		 *    -  \param  (DATA8)    VOLUME    - Volume [0..100]\n
		 *    -  \param  (DATA8)    NAME      - First character in filename (character string)\n
		 *
		 *\n
		 *
		 */
		/*! \brief  opSOUND byte code
		 *
		 */
		public void cSoundEntry()
		{
			int Cmd;
			UWORD Temp1;
			UBYTE Loop = 0;
			UWORD Frequency;
			UWORD Duration;

			UWORD BytesToWrite;
			UWORD BytesWritten = 0;
			DATA8* SoundData = CommonHelper.Pointer1d<DATA8>(SOUND_FILE_BUFFER_SIZE + 1); // Add up for CMD

			DATA8* pFileName;
			DATA8* PathName = CommonHelper.Pointer1d<DATA8>(MAX_FILENAME_SIZE);
			UBYTE Tmp1;
			UBYTE Tmp2;


			Cmd = *(DATA8*)GH.Lms.PrimParPointer();

			SoundData[0] = (sbyte)Cmd; // General for all commands :-)
			BytesToWrite = 0;


			switch (Cmd)
			{

				case TONE:
					(*GH.SoundInstance.pSound).Status = BUSY;
					GH.SoundInstance.SoundOwner = (sbyte)GH.Lms.CallingObjectId();
					Temp1 = (ushort)*(DATA8*)GH.Lms.PrimParPointer();   // Volume level

					// Scale the volume from 1-100% into 13 level steps
					// Could be linear but prepared for speaker and -box adjustments... :-)
					if (Temp1 > 0)
					{
						if (Temp1 > TONE_LEVEL_6)           // >  48%
						{
							if (Temp1 > TONE_LEVEL_9)         // >  72%
							{
								if (Temp1 > TONE_LEVEL_11)      // >  88%
								{
									if (Temp1 > TONE_LEVEL_12)    // >  96%
									{
										SoundData[1] = 13;            // => 100%
									}
									else
									{
										SoundData[1] = 12;            // => 100%
									}
								}
								else
								{
									if (Temp1 > TONE_LEVEL_10)    // >  96%
									{
										SoundData[1] = 11;            // => 100%
									}
									else
									{
										SoundData[1] = 10;            // => 100%
									}
								}
							}
							else
							{
								if (Temp1 > TONE_LEVEL_8)       // >  62.5%
								{
									SoundData[1] = 9;           // => 75%
								}
								else
								{
									if (Temp1 > TONE_LEVEL_7)
									{
										SoundData[1] = 8;           // => 62.5%
									}
									else
									{
										SoundData[1] = 7;           // => 62.5%
									}
								}
							}
						}
						else
						{
							if (Temp1 > TONE_LEVEL_3)         // >  25%
							{
								if (Temp1 > TONE_LEVEL_5)       // >  37.5%
								{
									SoundData[1] = 6;           // => 50%
								}
								else
								{
									if (Temp1 > TONE_LEVEL_4)       // >  37.5%
									{
										SoundData[1] = 5;           // => 37.5%
									}
									else
									{
										SoundData[1] = 4;
									}
								}
							}
							else
							{
								if (Temp1 > TONE_LEVEL_2)       // >  12.5%
								{
									SoundData[1] = 3;
								}
								else
								{
									if (Temp1 > TONE_LEVEL_1)
									{
										SoundData[1] = 2;           // => 25%
									}
									else
									{
										SoundData[1] = 1;           // => 25%
									}
								}
							}
						}
					}
					else
						SoundData[1] = 0;

					Frequency = *(UWORD*)GH.Lms.PrimParPointer();
					Duration = *(UWORD*)GH.Lms.PrimParPointer();
					SoundData[2] = (DATA8)(Frequency);
					SoundData[3] = (DATA8)(Frequency >> 8);
					SoundData[4] = (DATA8)(Duration);
					SoundData[5] = (DATA8)(Duration >> 8);
					BytesToWrite = 6;
					GH.SoundInstance.cSoundState = SOUND_TONE_PLAYING;
					break;

				case BREAK:     //SoundData[0] = Cmd;
					BytesToWrite = 1;
					GH.SoundInstance.cSoundState = SOUND_STOPPED;

					if (*GH.SoundInstance.hSoundFile >= 0)
					{
						// close(GH.SoundInstance.hSoundFile);
						*GH.SoundInstance.hSoundFile = 0;
					}

					break;

				case REPEAT:
				case PLAY:      // If SoundFile is Flood filled, we must politely
								// close the active handle - else we acts as a "BUG"
								// eating all the crops (handles) ;-)

					if (Cmd == REPEAT)
					{
						Loop = 1;
						SoundData[0] = PLAY;  // Yes, but looping :-)
					}

					GH.SoundInstance.cSoundState = SOUND_STOPPED;  // Yes but only shortly

					if ((int)GH.SoundInstance.hSoundFile >= 0)  // An active handle?
					{
						// close(GH.SoundInstance.hSoundFile);  // No more use
						GH.SoundInstance.hSoundFile = null;   // Signal it
					}

					(*GH.SoundInstance.pSound).Status = BUSY;
					GH.SoundInstance.SoundOwner = (sbyte)GH.Lms.CallingObjectId();

					Temp1 = *(UBYTE*)GH.Lms.PrimParPointer();  // Volume level
														// Scale the volume from 1-100% into 1 - 8 level steps
														// Could be linear but prepared for speaker and -box adjustments... :-)
					if (Temp1 > 0)
					{
						if (Temp1 > SND_LEVEL_4)           // >  50%
						{
							if (Temp1 > SND_LEVEL_6)         // >  75%
							{
								if (Temp1 > SND_LEVEL_7)       // >  87.5%
								{
									SoundData[1] = 8;           // => 100%
								}
								else
								{
									SoundData[1] = 7;           // => 87.5%
								}
							}
							else
							{
								if (Temp1 > SND_LEVEL_5)       // >  62.5%
								{
									SoundData[1] = 6;           // => 75%
								}
								else
								{
									SoundData[1] = 5;           // => 62.5%
								}
							}
						}
						else
						{
							if (Temp1 > SND_LEVEL_2)         // >  25%
							{
								if (Temp1 > SND_LEVEL_3)       // >  37.5%
								{
									SoundData[1] = 4;           // => 50%
								}
								else
								{
									SoundData[1] = 3;           // => 37.5%
								}
							}
							else
							{
								if (Temp1 > SND_LEVEL_1)       // >  12.5%
								{
									SoundData[1] = 2;           // => 25%
								}
								else
								{
									SoundData[1] = 1;           // => 12.5%
								}
							}
						}
					}
					else
						SoundData[1] = 0;

					BytesToWrite = 2;

					// Get filename
					pFileName = (DATA8*)GH.Lms.PrimParPointer();

					if (pFileName != null) // We should have a valid filename
					{
						// Get Path and concatenate

						PathName[0] = 0;
						if (pFileName[0] != '.')
						{
							GH.Lms.GetResourcePath(PathName, MAX_FILENAME_SIZE);
							// var tmpTest = CommonHelper.GetString(PathName);
							// var tmpTest2 = CommonHelper.GetString(pFileName);
							CommonHelper.sprintf(GH.SoundInstance.PathBuffer, $"{CommonHelper.GetString(PathName)}{CommonHelper.GetString(pFileName)}.rsf");
						}
						else
						{
							CommonHelper.sprintf(GH.SoundInstance.PathBuffer, $"{CommonHelper.GetString(pFileName)}.rsf");
						}

						// Open SoundFile

						// var tmpTest3 = CommonHelper.GetString(GH.SoundInstance.PathBuffer);
						GH.SoundInstance.hSoundFile = GH.SoundInstance.PathBuffer;
						using var fileH = File.OpenRead(CommonHelper.GetString(GH.SoundInstance.PathBuffer));

						if (*GH.SoundInstance.hSoundFile >= 0)
						{
							// Get actual FileSize
							GH.SoundInstance.SoundFileLength = (ushort)(new FileInfo(CommonHelper.GetString(GH.SoundInstance.PathBuffer))).Length;

							// BIG Endianess

							fileH.ReadUnsafe(&Tmp1, 0, 1);
							fileH.ReadUnsafe(&Tmp2, 0, 1);
							GH.SoundInstance.SoundFileFormat = (ushort)((UWORD)Tmp1 << 8 | (UWORD)Tmp2);

							fileH.ReadUnsafe(&Tmp1, 0, 1);
							fileH.ReadUnsafe(&Tmp2, 0, 1);
							GH.SoundInstance.SoundDataLength = (ushort)((UWORD)Tmp1 << 8 | (UWORD)Tmp2);

							fileH.ReadUnsafe(&Tmp1, 0, 1);
							fileH.ReadUnsafe(&Tmp2, 0, 1);
							GH.SoundInstance.SoundSampleRate = (ushort)((UWORD)Tmp1 << 8 | (UWORD)Tmp2);

							fileH.ReadUnsafe(&Tmp1, 0, 1);
							fileH.ReadUnsafe(&Tmp2, 0, 1);
							GH.SoundInstance.SoundPlayMode = (ushort)((UWORD)Tmp1 << 8 | (UWORD)Tmp2);

							GH.SoundInstance.cSoundState = SOUND_SETUP_FILE;

							if (GH.SoundInstance.SoundFileFormat == FILEFORMAT_ADPCM_SOUND)
								cSoundInitAdPcm();
						}
						fileH.Close();

					}
					else
					{
						//Do some ERROR-handling :-)
						//NOT a valid name from above :-(
					}

					break;

				default:
					BytesToWrite = 0; // An non-valid entry
					break;
			}

			if (BytesToWrite > 0)
			{
				GH.Ev3System.SoundHandler.PlayChunk(CommonHelper.GetArray((byte*)SoundData, BytesToWrite));
				GH.SoundInstance.SoundDriverDescriptor = -1;

				if (GH.SoundInstance.cSoundState == SOUND_SETUP_FILE)  // The one and only situation
				{
					GH.SoundInstance.BytesToWrite = 0;                   // Reset
					if (1 == Loop)
						GH.SoundInstance.cSoundState = SOUND_FILE_LOOPING;
					else
						GH.SoundInstance.cSoundState = SOUND_FILE_PLAYING;
				}
			}
			else
			{
				BytesToWrite = BytesWritten;
				BytesToWrite = 0;
			}
		}

		/*! \page   cSound
		 *  <hr size="1"/>
		 *  <b>     opSOUND_TEST (BUSY) </b>
		 *
		 *- Test if sound busy (playing file or tone\n
		 *- Dispatch status unchanged
		 *
		 *  \return  (DATA8)   BUSY    - Sound busy flag (0 = ready, 1 = busy)
		 *
		 */
		/*! \brief  opSOUND_TEST byte code
		 *
		 */

		public void cSoundTest()
		{
			if ((*GH.SoundInstance.pSound).Status == BUSY)
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 1;
			}
			else
			{
				*(DATA8*)GH.Lms.PrimParPointer() = 0;
			}
		}

		/*! \page   cSound
		 *  <hr size="1"/>
		 *  <b>     opSOUND_READY () </b>
		 *
		 *- Wait for sound ready (wait until sound finished)\n
		 *- Dispatch status can change to BUSYBREAK
		 *
		 */
		/*! \brief  opSOUND_READY byte code
		 *
		 */

		public void cSoundReady()
		{
			IP TmpIp;
			DSPSTAT DspStat = DSPSTAT.NOBREAK;

			TmpIp = GH.Lms.GetObjectIp();


			if ((*GH.SoundInstance.pSound).Status == BUSY)
			{ // If BUSY check for OVERRULED

				{ // Rewind IP and set status
					DspStat = DSPSTAT.BUSYBREAK; // break the interpreter and waits busy
					GH.Lms.SetDispatchStatus(DspStat);
					GH.Lms.SetObjectIp(TmpIp - 1);
				}
			}
		}
	}
}
