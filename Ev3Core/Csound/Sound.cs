using Ev3Core.Csound.Interfaces;
using Ev3Core.Enums;
using Ev3Core.Helpers;
using static Ev3Core.Defines;

namespace Ev3Core.Csound
{
	public class Sound : ISound
	{
		public RESULT cSoundInit()
		{
			// TODO:
			GH.Ev3System.Logger.LogWarning($"Tried to use unimplemented shite in: {System.Environment.StackTrace}");
			return RESULT.OK;
		}

		public RESULT cSoundOpen()
		{
			return RESULT.OK;
		}

		public RESULT cSoundClose()
		{
			// TODO:
			GH.Ev3System.Logger.LogWarning($"Tried to use unimplemented shite in: {System.Environment.StackTrace}");
			GH.SoundInstance.cSoundState = SOUND_STOPPED;
			return RESULT.OK;
		}

		public RESULT cSoundUpdate()
		{
			int BytesRead;
			int i;
			UBYTE[] AdPcmData = new UBYTE[SOUND_ADPCM_CHUNK]; // Temporary ADPCM input buffer
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
						BytesWritten = GH.Ev3System.SoundHandler.PlayChunk(GH.SoundInstance.SoundData, GH.SoundInstance.BytesToWrite);
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
					}
					else  // Get new sound data
					{     // No pending stuff
						if (GH.SoundInstance.SoundDataLength > 0) // Any new data?
						{
							GH.SoundInstance.SoundData[0] = SERVICE;

							if (GH.SoundInstance.SoundFileFormat == FILEFORMAT_ADPCM_SOUND)
							{
								// Adjust the chunk size for ADPCM (nibbles) if necessary
								if (GH.SoundInstance.SoundDataLength > SOUND_ADPCM_CHUNK)
									BytesToRead = SOUND_ADPCM_CHUNK;
								else
									BytesToRead = GH.SoundInstance.SoundDataLength;

								if (GH.SoundInstance.hSoundFile != null)  // Valid file
								{
									BytesRead = SoundHelper.Read(GH.SoundInstance.hSoundFile, AdPcmData, BytesToRead);

									for (i = 0; i < BytesRead; i++)
									{
										GH.SoundInstance.SoundData[2 * i + 1] = cSoundGetAdPcmValue((AdPcmData[i] >> 4) & 0x0F);
										GH.SoundInstance.SoundData[2 * i + 2] = cSoundGetAdPcmValue(AdPcmData[i] & 0x0F);
									}

									GH.SoundInstance.BytesToWrite = (UBYTE)(1 + BytesRead * 2);
								}
							}
							else // Non compressed data
							{
								// Adjust the chunk size if necessary
								if (GH.SoundInstance.SoundDataLength > SOUND_CHUNK)
									BytesToRead = SOUND_CHUNK;
								else
									BytesToRead = GH.SoundInstance.SoundDataLength;

								if (GH.SoundInstance.hSoundFile != null)  // Valid file
								{
									var tmpArr = GH.SoundInstance.SoundData.Skip(1).ToArray();
									BytesRead = SoundHelper.Read(GH.SoundInstance.hSoundFile, tmpArr, BytesToRead);
									Array.Copy(tmpArr, 0, GH.SoundInstance.SoundData, 1, BytesToRead);

									GH.SoundInstance.BytesToWrite = (byte)(BytesRead + 1);
								}
							}

							BytesWritten = GH.Ev3System.SoundHandler.PlayChunk(GH.SoundInstance.SoundData, GH.SoundInstance.BytesToWrite);
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
								GH.SoundInstance.SoundDataLength = 0;
								// TODO make a new write here, so no zero-"sound"
							}
							else
							{
								GH.SoundInstance.cSoundState = SOUND_STOPPED;

								if (GH.SoundInstance.hSoundFile != null)
								{
									GH.SoundInstance.hSoundFile = null;
								}
							}

						}
					}   // No pending write
					break;

				case SOUND_TONE_PLAYING:   // Check for duration done in d_sound :-)

					if (GH.SoundInstance.Sound.Status == OK)
					{
						// DO the finished stuff
						GH.SoundInstance.cSoundState = SOUND_STOPPED;

						if (GH.SoundInstance.hSoundFile != null)
						{
							GH.SoundInstance.hSoundFile = null;
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
			return RESULT.OK;
		}

		public void cSoundEntry()
		{
			int Cmd;
			UWORD Temp1;
			UBYTE Loop = 0;
			UWORD Frequency;
			UWORD Duration;

			UWORD BytesToWrite;
			UWORD BytesWritten = 0;
			DATA8[] SoundData = new DATA8[SOUND_FILE_BUFFER_SIZE + 1]; // Add up for CMD

			DATA8[] pFileName;
			char[] PathName = new char[MAX_FILENAME_SIZE];
			UBYTE Tmp1;
			UBYTE Tmp2;


			Cmd = (DATA8)GH.Lms.PrimParPointer();

			SoundData[0] = (sbyte)Cmd; // General for all commands :-)
			BytesToWrite = 0;


			switch (Cmd)
			{

				case TONE:
					GH.SoundInstance.Sound.Status = BUSY;
					GH.SoundInstance.SoundOwner = (sbyte)GH.Lms.CallingObjectId();
					Temp1 = (ushort)(DATA8)GH.Lms.PrimParPointer();   // Volume level

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

					Frequency = (ushort)(DATA16)GH.Lms.PrimParPointer();
					Duration = (ushort)(DATA16)GH.Lms.PrimParPointer();
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

					if (GH.SoundInstance.hSoundFile != null)
					{
						GH.SoundInstance.hSoundFile = null;
					}

					break;

				case PLAY:
				case REPEAT:
					if (Cmd == REPEAT)
					{
						Loop = 1;
						SoundData[0] = PLAY;  // Yes, but looping :-)
					}
					// Fall through

					// If SoundFile is Flood filled, we must politely
					// close the active handle - else we acts as a "BUG"
					// eating all the crops (handles) ;-)

					GH.SoundInstance.cSoundState = SOUND_STOPPED;  // Yes but only shortly

					if (GH.SoundInstance.hSoundFile != null)  // An active handle?
					{
						GH.SoundInstance.hSoundFile = null;   // Signal it
					}

					GH.SoundInstance.Sound.Status = BUSY;
					GH.SoundInstance.SoundOwner = (sbyte)GH.Lms.CallingObjectId();

					Temp1 = (ushort)(DATA8)GH.Lms.PrimParPointer();  // Volume level
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
					pFileName = (DATA8[])GH.Lms.PrimParPointer();

					if (pFileName != null) // We should have a valid filename
					{
						// Get Path and concatenate

						PathName[0] = (char)0;
						if (pFileName[0] != '.')
						{
							GetResourcePath(PathName, MAX_FILENAME_SIZE);
							sprintf(SoundInstance.PathBuffer, "%s%s.rsf", (char*)PathName, (char*)pFileName);
						}
						else
						{
							sprintf(SoundInstance.PathBuffer, "%s.rsf", (char*)pFileName);
						}

						// Open SoundFile

						SoundInstance.hSoundFile = open(SoundInstance.PathBuffer, O_RDONLY, 0666);

						if (SoundInstance.hSoundFile >= 0)
						{
							// Get actual FileSize
							stat(SoundInstance.PathBuffer, &SoundInstance.FileStatus);
							SoundInstance.SoundFileLength = SoundInstance.FileStatus.st_size;

							// BIG Endianess

							read(SoundInstance.hSoundFile, &Tmp1, 1);
							read(SoundInstance.hSoundFile, &Tmp2, 1);
							SoundInstance.SoundFileFormat = (UWORD)Tmp1 << 8 | (UWORD)Tmp2;

							read(SoundInstance.hSoundFile, &Tmp1, 1);
							read(SoundInstance.hSoundFile, &Tmp2, 1);
							SoundInstance.SoundDataLength = (UWORD)Tmp1 << 8 | (UWORD)Tmp2;

							read(SoundInstance.hSoundFile, &Tmp1, 1);
							read(SoundInstance.hSoundFile, &Tmp2, 1);
							SoundInstance.SoundSampleRate = (UWORD)Tmp1 << 8 | (UWORD)Tmp2;

							read(SoundInstance.hSoundFile, &Tmp1, 1);
							read(SoundInstance.hSoundFile, &Tmp2, 1);
							SoundInstance.SoundPlayMode = (UWORD)Tmp1 << 8 | (UWORD)Tmp2;

							SoundInstance.cSoundState = SOUND_SETUP_FILE;

							if (SoundInstance.SoundFileFormat == FILEFORMAT_ADPCM_SOUND)
								cSoundInitAdPcm();
						}


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
				SoundInstance.SoundDriverDescriptor = open(SOUND_DEVICE_NAME, O_WRONLY);
				if (SoundInstance.SoundDriverDescriptor >= 0)
				{
					BytesWritten = write(SoundInstance.SoundDriverDescriptor, SoundData, BytesToWrite);
					close(SoundInstance.SoundDriverDescriptor);
					SoundInstance.SoundDriverDescriptor = -1;

					if (SoundInstance.cSoundState == SOUND_SETUP_FILE)  // The one and only situation
					{
						SoundInstance.BytesToWrite = 0;                   // Reset
						if (TRUE == Loop)
							SoundInstance.cSoundState = SOUND_FILE_LOOPING;
						else
							SoundInstance.cSoundState = SOUND_FILE_PLAYING;
					}
				}
				else
					SoundInstance.cSoundState = SOUND_STOPPED;          // Couldn't do the job :-(
			}
			else
			{
				BytesToWrite = BytesWritten;
				BytesToWrite = 0;
			}
		}

		

		

		public void cSoundReady()
		{
			throw new NotImplementedException();
		}

		public void cSoundTest()
		{
			throw new NotImplementedException();
		}

		
	}
}
