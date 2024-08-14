using Ev3CoreUnsafe.Cui.Interfaces;
using Ev3CoreUnsafe.Helpers;
using Ev3CoreUnsafe.Lms2012.Interfaces;
using System.Runtime.CompilerServices;
using static Ev3CoreUnsafe.Defines;

namespace Ev3CoreUnsafe.Cui
{
	public unsafe class Lcd : ILcd
	{
        private static UBYTE* outLcdResult = CommonHelper.Pointer1d<UBYTE>(22784);
        private static UBYTE* vmem = CommonHelper.Pointer1d<UBYTE>(7680);
        private UBYTE* dbuf = vmem;

        private void update_to_fb()
        {
            ulong x, y, offset, mask;

            for (y = 0; y < LCD_HEIGHT; y++)
            {
                for (x = 0; x < LCD_WIDTH; x++)
                {
                    offset = x % 3;
                    if (offset != 0)
                    {
                        mask = (ulong)(((offset >> 1) != 0) ? 0x1 : 0x8);
                    }
                    else
                    {
                        mask = 0x80;
                    }

                    if ((vmem[x / 3 + y * 60] & mask) != 0)
                    {
                        outLcdResult[y * vmLCD_WIDTH + x] = FG_COLOR;
                        // grx_fast_draw_pixel(x, y, GRX_COLOR_BLACK);
                    }
                    else
                    {
						outLcdResult[y * vmLCD_WIDTH + x] = BG_COLOR; 
                        // grx_fast_draw_pixel(x, y, GRX_COLOR_WHITE);
                    }
                }
            }
        }

        public void dLcdExec(LCD* pDisp)
		{
			UBYTE* pSrc;
			UBYTE* pDst;
			ULONG Pixels;
			UWORD X;
			UWORD Y;

			LCD* bufPtr = (LCD*)GH.VMInstance.LcdBuffer;
			LCD* uiBufPtr = (LCD*)GH.UiInstance.LcdBuffer;

			if (CommonHelper.memcmp((byte*)pDisp, (byte*)bufPtr, sizeof(LCD)) != 0)
			{
                pSrc = (*pDisp).Lcd;
                pDst = dbuf;

                for (Y = 0; Y < LCD_HEIGHT; Y++)
                {
                    for (X = 0; X < 7; X++)
                    {
                        Pixels = (ULONG) (* pSrc);
                        pSrc++;
                        Pixels |= (ULONG) (* pSrc << 8);
                        pSrc++;
                        Pixels |= (ULONG) (* pSrc << 16);
                        pSrc++;

                        *pDst = PixelTab[Pixels & 0x07];
                        pDst++;
                        Pixels >>= 3;
                        *pDst = PixelTab[Pixels & 0x07];
                        pDst++;
                        Pixels >>= 3;
                        *pDst = PixelTab[Pixels & 0x07];
                        pDst++;
                        Pixels >>= 3;
                        *pDst = PixelTab[Pixels & 0x07];
                        pDst++;
                        Pixels >>= 3;
                        *pDst = PixelTab[Pixels & 0x07];
                        pDst++;
                        Pixels >>= 3;
                        *pDst = PixelTab[Pixels & 0x07];
                        pDst++;
                        Pixels >>= 3;
                        *pDst = PixelTab[Pixels & 0x07];
                        pDst++;
                        Pixels >>= 3;
                        *pDst = PixelTab[Pixels & 0x07];
                        pDst++;
                    }
                    Pixels = (ULONG) (* pSrc);
                    pSrc++;
                    Pixels |= (ULONG) (* pSrc << 8);
                    pSrc++;

                    *pDst = PixelTab[Pixels & 0x07];
                    pDst++;
                    Pixels >>= 3;
                    *pDst = PixelTab[Pixels & 0x07];
                    pDst++;
                    Pixels >>= 3;
                    *pDst = PixelTab[Pixels & 0x07];
                    pDst++;
                    Pixels >>= 3;
                    *pDst = PixelTab[Pixels & 0x07];
                    pDst++;
                }

                LCDCopy((byte*)uiBufPtr, (byte*)bufPtr, sizeof(LCD));
				update_to_fb();
                GH.Ev3System.LcdHandler.UpdateLcd(CommonHelper.GetArray((byte*)outLcdResult, vmLCD_WIDTH * vmLCD_HEIGHT));
				GH.VMInstance.LcdUpdated = 1;
			}
		}

		public void dLcdAutoUpdate()
		{
			GH.Ev3System.Logger.LogError($"This should not be called {Environment.StackTrace}");
		}

		public void dLcdUpdate(LCD* pDisp)
		{
			dLcdExec(pDisp);
		}


		public void dLcdInit(UBYTE* pImage)
		{
			GH.Ev3System.LcdHandler.Init();
		}


		public UBYTE dLcdRead()
		{
			return (0);
		}


		public void dLcdExit()
		{
			GH.Ev3System.LcdHandler.Exit();
		}

		public void dLcdScroll(UBYTE* pImage, DATA16 Y0)
		{
			CommonHelper.memmove(pImage, &pImage[((LCD_WIDTH + 7) / 8) * Y0], (LCD_HEIGHT - Y0) * ((LCD_WIDTH + 7) / 8));
			CommonHelper.memset(&pImage[(LCD_HEIGHT - Y0) * ((LCD_WIDTH + 7) / 8)], 0, ((LCD_WIDTH + 7) / 8) * Y0);
		}


		public void dLcdDrawPixel(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0)
		{
			if ((X0 >= 0) && (X0 < LCD_WIDTH) && (Y0 >= 0) && (Y0 < LCD_HEIGHT))
			{
				if (Color != 0)
				{
					pImage[(X0 >> 3) + Y0 * ((LCD_WIDTH + 7) >> 3)] |= (byte)(1 << (X0 % 8));
				}
				else
				{
					pImage[(X0 >> 3) + Y0 * ((LCD_WIDTH + 7) >> 3)] &= (byte)~(1 << (X0 % 8));
				}
			}
		}


		public void dLcdInversePixel(UBYTE* pImage, DATA16 X0, DATA16 Y0)
		{
			if ((X0 >= 0) && (X0 < LCD_WIDTH) && (Y0 >= 0) && (Y0 < LCD_HEIGHT))
			{
				pImage[(X0 >> 3) + Y0 * ((LCD_WIDTH + 7) >> 3)] ^= (byte)(1 << (X0 % 8));
			}
		}


		public DATA8 dLcdReadPixel(UBYTE* pImage, DATA16 X0, DATA16 Y0)
		{
			DATA8 Result = 0;

			if ((X0 >= 0) && (X0 < LCD_WIDTH) && (Y0 >= 0) && (Y0 < LCD_HEIGHT))
			{
				if ((pImage[(X0 >> 3) + Y0 * ((LCD_WIDTH + 7) >> 3)] & (1 << (X0 % 8))) != 0)
				{
					Result = 1;
				}
			}

			return (Result);
		}


		public void dLcdDrawLine(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 X1, DATA16 Y1)
		{
			DATA16 XLength;
			DATA16 YLength;
			DATA16 XInc;
			DATA16 YInc;
			DATA16 Diff;
			DATA16 Tmp;

			if (X0 < X1)
			{
				XLength = (short)(X1 - X0);
				XInc = 1;
			}
			else
			{
				XLength = (short)(X0 - X1);
				XInc = -1;
			}
			if (Y0 < Y1)
			{
				YLength = (short)(Y1 - Y0);
				YInc = 1;
			}
			else
			{
				YLength = (short)(Y0 - Y1);
				YInc = -1;
			}
			Diff = (short)(XLength - YLength);

			dLcdDrawPixel(pImage, Color, X0, Y0);

			while ((X0 != X1) || (Y0 != Y1))
			{
				Tmp = (short)(Diff << 1);
				if (Tmp > (-YLength))
				{
					Diff -= YLength;
					X0 += XInc;
				}
				if (Tmp < XLength)
				{
					Diff += XLength;
					Y0 += YInc;
				}
				dLcdDrawPixel(pImage, Color, X0, Y0);
			}
		}


		public void dLcdDrawDotLine(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 X1, DATA16 Y1, DATA16 On, DATA16 Off)
		{
			DATA16 XLength;
			DATA16 YLength;
			DATA16 XInc;
			DATA16 YInc;
			DATA16 Diff;
			DATA16 Tmp;
			DATA16 Count;

			if ((X0 != X1) && (Y0 != Y1))
			{
				dLcdDrawLine(pImage, Color, X0, Y0, X1, Y1);
			}
			else
			{
				if (X0 < X1)
				{
					XLength = (short)(X1 - X0);
					XInc = 1;
				}
				else
				{
					XLength = (short)(X0 - X1);
					XInc = -1;
				}
				if (Y0 < Y1)
				{
					YLength = (short)(Y1 - Y0);
					YInc = 1;
				}
				else
				{
					YLength = (short)(Y0 - Y1);
					YInc = -1;
				}
				Diff = (short)(XLength - YLength);

				dLcdDrawPixel(pImage, Color, X0, Y0);
				Count = 1;

				while ((X0 != X1) || (Y0 != Y1))
				{
					Tmp = (short)(Diff << 1);
					if (Tmp > (-YLength))
					{
						Diff -= YLength;
						X0 += XInc;
					}
					if (Tmp < XLength)
					{
						Diff += XLength;
						Y0 += YInc;
					}
					if (Count < (On + Off))
					{
						if (Count < On)
						{
							dLcdDrawPixel(pImage, Color, X0, Y0);
						}
						else
						{
							dLcdDrawPixel(pImage, (sbyte)(1 - Color), X0, Y0);
						}
					}
					Count++;
					if (Count >= (On + Off))
					{
						Count = 0;
					}
				}
			}
		}


		public void dLcdPlotPoints(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 X1, DATA16 Y1)
		{
			dLcdDrawPixel(pImage, Color, (short)(X0 + X1), (short)(Y0 + Y1));
			dLcdDrawPixel(pImage, Color, (short)(X0 - X1), (short)(Y0 + Y1));
			dLcdDrawPixel(pImage, Color, (short)(X0 + X1), (short)(Y0 - Y1));
			dLcdDrawPixel(pImage, Color, (short)(X0 - X1), (short)(Y0 - Y1));
			dLcdDrawPixel(pImage, Color, (short)(X0 + Y1), (short)(Y0 + X1));
			dLcdDrawPixel(pImage, Color, (short)(X0 - Y1), (short)(Y0 + X1));
			dLcdDrawPixel(pImage, Color, (short)(X0 + Y1), (short)(Y0 - X1));
			dLcdDrawPixel(pImage, Color, (short)(X0 - Y1), (short)(Y0 - X1));
		}


		public void dLcdDrawCircle(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 R)
		{
			int X = 0;
			int Y = R;
			int P = 3 - 2 * R;

			while (X < Y)
			{
				dLcdPlotPoints(pImage, Color, X0, Y0, (short)X, (short)Y);
				if (P < 0)
				{
					P = P + 4 * X + 6;
				}
				else
				{
					P = P + 4 * (X - Y) + 10;
					Y = Y - 1;
				}
				X = X + 1;
			}
			dLcdPlotPoints(pImage, Color, X0, Y0, (short)X, (short)Y);
		}

		public DATA16 dLcdGetFontWidth(DATA8 Font)
		{
			return (FontInfo[Font].FontWidth);
		}


		public DATA16 dLcdGetFontHeight(DATA8 Font)
		{
			return (FontInfo[Font].FontHeight);
		}

		public void dLcdDrawChar(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA8 Font, DATA8 Char)
		{
			DATA16 CharWidth;
			DATA16 CharHeight;
			DATA16 CharByteIndex;
			DATA16 LcdByteIndex;
			UBYTE CharByte;
			DATA16 Tmp, X, Y, TmpX, MaxX;


			CharWidth = FontInfo[Font].FontWidth;
			CharHeight = FontInfo[Font].FontHeight;

			if ((Char >= FontInfo[Font].FontFirst) && (Char <= FontInfo[Font].FontLast))
			{
				Char -= FontInfo[Font].FontFirst;

				CharByteIndex = (short)((Char % FontInfo[Font].FontHorz) * ((CharWidth + 7) / 8));
				CharByteIndex += (short)((Char / FontInfo[Font].FontHorz) * ((CharWidth + 7) / 8) * CharHeight * FontInfo[Font].FontHorz);

				if (((CharWidth % 8) == 0) && ((X0 % 8) == 0))
				{ // Font aligned

					X0 = (short)((X0 >> 3) << 3);
					LcdByteIndex = (short)((X0 >> 3) + Y0 * ((LCD_WIDTH + 7) >> 3));

					if (Color != 0)
					{
						while (CharHeight != 0)
						{
							Tmp = 0;
							do
							{
								if (LcdByteIndex < sizeof(LCD))
								{
									pImage[LcdByteIndex + Tmp] = FontInfo[Font].pFontBits[CharByteIndex + Tmp];
								}
								Tmp++;
							}
							while (Tmp < (CharWidth / 8));

							CharByteIndex += (short)((CharWidth * FontInfo[Font].FontHorz) / 8);
							LcdByteIndex += ((LCD_WIDTH + 7) >> 3);
							CharHeight--;
						}
					}
					else
					{
						while (CharHeight != 0)
						{
							Tmp = 0;
							do
							{
								if (LcdByteIndex < sizeof(LCD))
								{
									pImage[LcdByteIndex + Tmp] = (byte)~FontInfo[Font].pFontBits[CharByteIndex + Tmp];
								}
								Tmp++;
							}
							while (Tmp < (CharWidth / 8));

							CharByteIndex += (short)((CharWidth * FontInfo[Font].FontHorz) / 8);
							LcdByteIndex += ((LCD_WIDTH + 7) >> 3);
							CharHeight--;
						}
					}
				}
				else
				{ // Font not aligned

					MaxX = (short)(X0 + CharWidth);

					if (Color != 0)
					{
						for (Y = 0; Y < CharHeight; Y++)
						{
							TmpX = X0;

							for (X = 0; X < ((CharWidth + 7) / 8); X++)
							{
								CharByte = FontInfo[Font].pFontBits[CharByteIndex + X];

								for (Tmp = 0; (Tmp < 8) && (TmpX < MaxX); Tmp++)
								{
									if ((CharByte & 1) != 0)
									{
										dLcdDrawPixel(pImage, 1, TmpX, Y0);
									}
									else
									{
										dLcdDrawPixel(pImage, 0, TmpX, Y0);
									}
									CharByte >>= 1;
									TmpX++;
								}
							}
							Y0++;
							CharByteIndex += (short)(((CharWidth + 7) / 8) * FontInfo[Font].FontHorz);

						}
					}
					else
					{
						for (Y = 0; Y < CharHeight; Y++)
						{
							TmpX = X0;

							for (X = 0; X < ((CharWidth + 7) / 8); X++)
							{
								CharByte = FontInfo[Font].pFontBits[CharByteIndex + X];

								for (Tmp = 0; (Tmp < 8) && (TmpX < MaxX); Tmp++)
								{
									if ((CharByte & 1) != 0) 
									{
										dLcdDrawPixel(pImage, 0, TmpX, Y0);
									}
									else
									{
										dLcdDrawPixel(pImage, 1, TmpX, Y0);
									}
									CharByte >>= 1;
									TmpX++;
								}
							}
							Y0++;
							CharByteIndex += (short)(((CharWidth + 7) / 8) * FontInfo[Font].FontHorz);
						}
					}
				}
			}
		}


		public void dLcdDrawText(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA8 Font, DATA8* pText)
		{
			while (*pText != 0)
			{
				if (X0 < (LCD_WIDTH - FontInfo[Font].FontWidth))
				{
					dLcdDrawChar(pImage, Color, X0, Y0, Font, *pText);
					X0 += FontInfo[Font].FontWidth;
				}
				pText++;
			}
		}

		public UBYTE* dLcdGetIconBits(DATA8 Type)
		{
			UBYTE* pResult;

			pResult = (UBYTE*)IconInfo[Type].pIconBits;

			return (pResult);
		}


		public DATA16 dLcdGetIconWidth(DATA8 Type)
		{
			return (IconInfo[Type].IconWidth);
		}


		public DATA16 dLcdGetIconHeight(DATA8 Type)
		{
			return (IconInfo[Type].IconHeight);
		}


		public DATA16 dLcdGetNoOfIcons(DATA8 Type)
		{
			return ((short)(IconInfo[Type].IconSize / IconInfo[Type].IconHeight));
		}


		public void dLcdDrawPicture(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 IconWidth, DATA16 IconHeight, UBYTE* pIconBits)
		{
			DATA16 IconByteIndex;
			DATA16 LcdByteIndex;
			DATA16 Tmp;

			IconByteIndex = 0;

			X0 = (short)((X0 >> 3) << 3);
			LcdByteIndex = (short)((X0 >> 3) + Y0 * ((LCD_WIDTH + 7) >> 3));


			if (Color != 0)
			{
				while (IconHeight != 0)
				{
					for (Tmp = 0; Tmp < (IconWidth / 8); Tmp++)
					{
						pImage[LcdByteIndex + Tmp] = pIconBits[IconByteIndex + Tmp];
					}

					IconByteIndex += (short)(IconWidth / 8);
					LcdByteIndex += ((LCD_WIDTH + 7) >> 3);
					IconHeight--;
				}
			}
			else
			{
				while (IconHeight != 0)
				{
					for (Tmp = 0; Tmp < (IconWidth / 8); Tmp++)
					{
						pImage[LcdByteIndex + Tmp] = (byte)~pIconBits[IconByteIndex + Tmp];
					}

					IconByteIndex += (short)(IconWidth / 8);
					LcdByteIndex += ((LCD_WIDTH + 7) >> 3);
					IconHeight--;
				}
			}
		}


		public void dLcdDrawIcon(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA8 Type, DATA8 No)
		{
			DATA16 IconByteIndex;
			DATA16 IconHeight;
			DATA16 IconWidth;
			UBYTE* pIconBits;

			IconHeight = dLcdGetIconHeight(Type);
			IconWidth = dLcdGetIconWidth(Type);

			if ((No >= 0) && (No <= dLcdGetNoOfIcons(Type)))
			{
				pIconBits = dLcdGetIconBits(Type);
				IconByteIndex = (DATA16)((No * IconWidth * IconHeight) / 8);
				dLcdDrawPicture(pImage, Color, X0, Y0, IconWidth, IconHeight, &pIconBits[IconByteIndex]);
			}
		}


		public void dLcdGetBitmapSize(IP pBitmap, DATA16* pWidth, DATA16* pHeight)
		{
			*pWidth = 0;
			*pHeight = 0;

			if ((UIntPtr)pBitmap != 0)
			{
				*pWidth = (DATA16)pBitmap[0];
				*pHeight = (DATA16)pBitmap[1];
			}
		}


		public void dLcdDrawBitmap(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0, IP pBitmap)
		{
			DATA16 BitmapWidth;
			DATA16 BitmapHeight;
			DATA16 BitmapByteIndex;
			UBYTE* pBitmapBytes;
			UBYTE BitmapByte;
			DATA16 Tmp, X, Y, TmpX, MaxX;

			DATA16 LcdByteIndex;

			if ((UIntPtr)pBitmap != 0)
			{

				BitmapWidth = (DATA16)pBitmap[0];
				BitmapHeight = (DATA16)pBitmap[1];
				MaxX = (short)(X0 + BitmapWidth);
				pBitmapBytes = &pBitmap[2];

				if ((BitmapWidth >= 0) && (BitmapHeight >= 0))
				{
					if ((X0 % 8) != 0 || (BitmapWidth % 8) != 0)
					{ // X is not aligned

						BitmapWidth = (short)(((BitmapWidth + 7) >> 3) << 3);

						if (Color != 0)
						{
							for (Y = 0; Y < BitmapHeight; Y++)
							{
								BitmapByteIndex = (short)((Y * BitmapWidth) / 8);
								TmpX = X0;

								for (X = 0; X < (BitmapWidth / 8); X++)
								{
									BitmapByte = pBitmapBytes[BitmapByteIndex + X];

									for (Tmp = 0; (Tmp < 8) && (TmpX < MaxX); Tmp++)
									{
										if ((BitmapByte & 1) != 0)
										{
											dLcdDrawPixel(pImage, 1, TmpX, Y0);
										}
										else
										{
											dLcdDrawPixel(pImage, 0, TmpX, Y0);
										}
										BitmapByte >>= 1;
										TmpX++;
									}
								}
								Y0++;
							}
						}
						else
						{
							for (Y = 0; Y < BitmapHeight; Y++)
							{
								BitmapByteIndex = (short)((Y * BitmapWidth) / 8);
								TmpX = X0;

								for (X = 0; X < (BitmapWidth / 8); X++)
								{
									BitmapByte = pBitmapBytes[BitmapByteIndex + X];

									for (Tmp = 0; (Tmp < 8) && (TmpX < MaxX); Tmp++)
									{
										if ((BitmapByte & 1) != 0)
										{
											dLcdDrawPixel(pImage, 0, TmpX, Y0);
										}
										else
										{
											dLcdDrawPixel(pImage, 1, TmpX, Y0);
										}
										BitmapByte >>= 1;
										TmpX++;
									}
								}
								Y0++;
							}
						}
					}

					else
					{ // X is byte aligned

						BitmapByteIndex = 0;

						LcdByteIndex = (short)((X0 >> 3) + Y0 * ((LCD_WIDTH + 7) >> 3));

						if (Color != 0)
						{
							while (BitmapHeight != 0)
							{
								X = X0;
								for (Tmp = 0; Tmp < (BitmapWidth / 8); Tmp++)
								{
									if (((LcdByteIndex + Tmp) < LCD_BUFFER_SIZE) && (X < LCD_WIDTH) && (X >= 0))
									{
										pImage[LcdByteIndex + Tmp] = pBitmapBytes[BitmapByteIndex + Tmp];
									}
									X += 8;
								}

								BitmapByteIndex += (short)(BitmapWidth / 8);
								LcdByteIndex += ((LCD_WIDTH + 7) >> 3);

								BitmapHeight--;
							}
						}
						else
						{
							while (BitmapHeight != 0)
							{
								X = X0;
								for (Tmp = 0; Tmp < (BitmapWidth / 8); Tmp++)
								{
									if (((LcdByteIndex + Tmp) < LCD_BUFFER_SIZE) && (X < LCD_WIDTH) && (X >= 0))
									{
										pImage[LcdByteIndex + Tmp] = (byte)~pBitmapBytes[BitmapByteIndex + Tmp];
									}
									X += 8;
								}

								BitmapByteIndex += (short)(BitmapWidth / 8);
								LcdByteIndex += ((LCD_WIDTH + 7) >> 3);

								BitmapHeight--;
							}
						}
					}

				}
			}
		}


		public void dLcdRect(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 X1, DATA16 Y1)
		{
			X1--;
			Y1--;
			dLcdDrawLine(pImage, Color, X0, Y0, (short)(X0 + X1), Y0);
			dLcdDrawLine(pImage, Color, (short)(X0 + X1), Y0, (short)(X0 + X1), (short)(Y0 + Y1));
			dLcdDrawLine(pImage, Color, (short)(X0 + X1), (short)(Y0 + Y1), X0, (short)(Y0 + Y1));
			dLcdDrawLine(pImage, Color, X0, (short)(Y0 + Y1), X0, Y0);
		}


		public void dLcdFillRect(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 X1, DATA16 Y1)
		{
			DATA16 StartX;
			DATA16 MaxX;
			DATA16 MaxY;

			StartX = X0;
			MaxX = (short)(X0 + X1);
			MaxY = (short)(Y0 + Y1);

			for (; Y0 < MaxY; Y0++)
			{
				for (X0 = StartX; X0 < MaxX; X0++)
				{
					dLcdDrawPixel(pImage, Color, X0, Y0);
				}
			}
		}


		public void dLcdInverseRect(UBYTE* pImage, DATA16 X0, DATA16 Y0, DATA16 X1, DATA16 Y1)
		{
			DATA16 StartX;
			DATA16 MaxX;
			DATA16 MaxY;

			StartX = X0;
			MaxX = (short)(X0 + X1);
			MaxY = (short)(Y0 + Y1);

			for (; Y0 < MaxY; Y0++)
			{
				for (X0 = StartX; X0 < MaxX; X0++)
				{
					dLcdInversePixel(pImage, X0, Y0);
				}
			}
		}


		public void dLcdPlotLines(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 X1, DATA16 Y1)
		{
			dLcdDrawLine(pImage, Color, (short)(X0 - X1), (short)(Y0 + Y1), (short)(X0 + X1), (short)(Y0 + Y1));
			dLcdDrawLine(pImage, Color, (short)(X0 - X1), (short)(Y0 - Y1), (short)(X0 + X1), (short)(Y0 - Y1));
			dLcdDrawLine(pImage, Color, (short)(X0 - Y1), (short)(Y0 + X1), (short)(X0 + Y1), (short)(Y0 + X1));
			dLcdDrawLine(pImage, Color, (short)(X0 - Y1), (short)(Y0 - X1), (short)(X0 + Y1), (short)(Y0 - X1));
		}


		public void dLcdDrawFilledCircle(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 R)
		{
			int X = 0;
			int Y = R;
			int P = 3 - 2 * R;

			while (X < Y)
			{
				dLcdPlotLines(pImage, Color, X0, Y0, (short)X, (short)Y);
				if (P < 0)
				{
					P = P + 4 * X + 6;
				}
				else
				{
					P = P + 4 * (X - Y) + 10;
					Y = Y - 1;
				}
				X = X + 1;
			}
			dLcdPlotLines(pImage, Color, X0, Y0, (short)X, (short)Y);
		}


		public DATA8 dLcdCheckPixel(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0)
		{
			DATA8 Result = 0;

			if ((X0 >= 0) && (X0 < LCD_WIDTH) && (Y0 >= 0) && (Y0 < LCD_HEIGHT))
			{
				if (dLcdReadPixel(pImage, X0, Y0) != Color)
				{
					Result = 1;
				}
			}

			return (Result);
		}


		public void dLcdFlodfill(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0)
		{
			DATA16 X;
			DATA16 Y;

			Y = Y0;
			X = X0;

			while (dLcdCheckPixel(pImage, Color, X, Y) != 0)
			{
				while (dLcdCheckPixel(pImage, Color, X, Y) != 0)
				{
					if (X != X0)
					{
						dLcdDrawPixel(pImage, Color, X, Y);
					}
					X--;
				}
				X = X0;
				Y--;
			}

			Y = Y0;
			X = X0;

			while (dLcdCheckPixel(pImage, Color, X, Y) != 0)
			{
				while (dLcdCheckPixel(pImage, Color, X, Y) != 0)
				{
					if (X != X0)
					{
						dLcdDrawPixel(pImage, Color, X, Y);
					}
					X--;
				}
				X = X0;
				Y++;
			}

			Y = Y0;
			X = X0;

			while (dLcdCheckPixel(pImage, Color, X, Y) != 0)
			{
				while (dLcdCheckPixel(pImage, Color, X, Y) != 0)
				{
					dLcdDrawPixel(pImage, Color, X, Y);
					X++;
				}
				X = X0;
				Y--;
			}

			Y = (short)(Y0 + 1);
			X = X0;

			while (dLcdCheckPixel(pImage, Color, X, Y) != 0)
			{
				while (dLcdCheckPixel(pImage, Color, X, Y) != 0)
				{
					dLcdDrawPixel(pImage, Color, X, Y);
					X++;
				}
				X = X0;
				Y++;
			}

		}
	}
}
