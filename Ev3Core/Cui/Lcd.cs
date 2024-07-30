using Ev3Core.Cui.Interfaces;
using Ev3Core.Helpers;
using Ev3Core.Lms2012.Interfaces;
using static Ev3Core.Defines;

namespace Ev3Core.Cui
{
	public class Lcd : ILcd
	{
		private UBYTE[] dbuf = null;

		private static UBYTE[] PixelTab =
		{
			0x00, // 000 00000000
			0xE0, // 001 11100000
			0x1C, // 010 00011100
			0xFC, // 011 11111100
			0x03, // 100 00000011
			0xE3, // 101 11100011
			0x1F, // 110 00011111
			0xFF  // 111 11111111
		};

		private void dLcdExec(LCD pDisp)
		{
			if (CommonHelper.Memcmp(pDisp.Lcd, GH.VMInstance.LcdBuffer.Lcd, pDisp.Lcd.Length) != 0)
			{
				// TODO: send buff somewhere
				Array.Copy(GH.UiInstance.LcdBuffer.Lcd, GH.VMInstance.LcdBuffer.Lcd, GH.VMInstance.LcdBuffer.Lcd.Length);
				GH.VMInstance.LcdUpdated = 1;
			}
		}

		public void dLcdAutoUpdate()
		{
			GH.Ev3System.Logger.LogWarning("Lcd auto update called!!!");
		}

		public void dLcdUpdate(LCD pLcd)
		{
			dLcdExec(pLcd);
		}

		public void dLcdInit(byte[] pImage)
		{
			// TODO: 
		}

		public byte dLcdRead()
		{
			return 0;
		}

		public void dLcdExit()
		{
			// TODO: 
		}

		public void dLcdScroll(byte[] pImage, short Y0)
		{
			CommonHelper.Memmove(pImage, pImage, (LCD_HEIGHT - Y0) * ((LCD_WIDTH + 7) / 8), 0, ((LCD_WIDTH + 7) / 8) * Y0);
			CommonHelper.Memset(pImage, 0, ((LCD_WIDTH + 7) / 8) * Y0, (LCD_HEIGHT - Y0) * ((LCD_WIDTH + 7) / 8));
		}

		public void dLcdDrawPixel(byte[] pImage, sbyte Color, short X0, short Y0)
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

		public void dLcdInversePixel(byte[] pImage, short X0, short Y0)
		{
			if ((X0 >= 0) && (X0 < LCD_WIDTH) && (Y0 >= 0) && (Y0 < LCD_HEIGHT))
			{
				pImage[(X0 >> 3) + Y0 * ((LCD_WIDTH + 7) >> 3)] ^= (byte)(1 << (X0 % 8));
			}
		}

		public sbyte dLcdReadPixel(byte[] pImage, short X0, short Y0)
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

		public void dLcdDrawLine(byte[] pImage, sbyte Color, short X0, short Y0, short X1, short Y1)
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

		public void dLcdDrawDotLine(byte[] pImage, sbyte Color, short X0, short Y0, short X1, short Y1, short On, short Off)
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

		public void dLcdPlotPoints(byte[] pImage, sbyte Color, short X0, short Y0, short X1, short Y1)
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

		public void dLcdDrawCircle(byte[] pImage, sbyte Color, short X0, short Y0, short R)
		{
			short X = 0;
			short Y = R;
			int P = 3 - 2 * R;

			while (X < Y)
			{
				dLcdPlotPoints(pImage, Color, X0, Y0, X, Y);
				if (P < 0)
				{
					P = P + 4 * X + 6;
				}
				else
				{
					P = P + 4 * (X - Y) + 10;
					Y = (short)(Y - 1);
				}
				X = (short)(X + 1);
			}
			dLcdPlotPoints(pImage, Color, X0, Y0, X, Y);
		}

		public short dLcdGetFontHeight(sbyte Font)
		{
			return FontInfo[Font].FontHeight;
		}

		public short dLcdGetFontWidth(sbyte Font)
		{
			return FontInfo[Font].FontWidth;
		}

		public void dLcdDrawChar(byte[] pImage, sbyte Color, short X0, short Y0, sbyte Font, sbyte Char)
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
								if (LcdByteIndex < LCD.LcdSizeof)
								{
									pImage[LcdByteIndex + Tmp] = (byte)FontInfo[Font].pFontBits[CharByteIndex + Tmp];
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
								if (LcdByteIndex < LCD.LcdSizeof)
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
								CharByte = (byte)FontInfo[Font].pFontBits[CharByteIndex + X];

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
								CharByte = (byte)FontInfo[Font].pFontBits[CharByteIndex + X];

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

		public void dLcdDrawText(byte[] pImage, sbyte Color, short X0, short Y0, sbyte Font, sbyte[] pText)
		{
			foreach (var ch in pText)
			{
				if (X0 < (LCD_WIDTH - FontInfo[Font].FontWidth))
				{
					dLcdDrawChar(pImage, Color, X0, Y0, Font, ch);
					X0 += FontInfo[Font].FontWidth;
				}
			}
		}

		public byte[] dLcdGetIconBits(sbyte Type)
		{
			return IconInfo[Type].pIconBits;
		}

		public short dLcdGetIconHeight(sbyte Type)
		{
			return IconInfo[Type].IconHeight;
		}

		public short dLcdGetIconWidth(sbyte Type)
		{
			return IconInfo[Type].IconWidth;
		}

		public short dLcdGetNoOfIcons(sbyte Type)
		{
			return (short)(IconInfo[Type].IconSize / IconInfo[Type].IconHeight);
		}

		public void dLcdDrawPicture(byte[] pImage, sbyte Color, short X0, short Y0, short IconWidth, short IconHeight, byte[] pIconBits)
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

		public void dLcdDrawIcon(byte[] pImage, sbyte Color, short X0, short Y0, sbyte Type, sbyte No)
		{
			DATA16 IconByteIndex;
			DATA16 IconHeight;
			DATA16 IconWidth;
			UBYTE[] pIconBits;

			IconHeight = dLcdGetIconHeight(Type);
			IconWidth = dLcdGetIconWidth(Type);

			if ((No >= 0) && (No <= dLcdGetNoOfIcons(Type)))
			{
				pIconBits = dLcdGetIconBits(Type);
				IconByteIndex = (DATA16)(((DATA16)No * IconWidth * IconHeight) / 8);
				dLcdDrawPicture(pImage, Color, X0, Y0, IconWidth, IconHeight, pIconBits.Skip(IconByteIndex).ToArray());
			}
		}

		public void dLcdGetBitmapSize(byte[] pBitmap, ref short pWidth, ref short pHeight)
		{
			pWidth = 0;
			pHeight = 0;

			if (pBitmap != null && pBitmap.Length > 0)
			{
				pWidth = (DATA16)pBitmap[0];
				pHeight = (DATA16)pBitmap[1];
			}
		}

		public void dLcdDrawBitmap(byte[] pImage, sbyte Color, short X0, short Y0, byte[] pBitmap)
		{
			DATA16 BitmapWidth;
			DATA16 BitmapHeight;
			DATA16 BitmapByteIndex;
			UBYTE[] pBitmapBytes;
			UBYTE BitmapByte;
			DATA16 Tmp, X, Y, TmpX, MaxX;

			DATA16 LcdByteIndex;

			if (pBitmap != null && pBitmap.Length > 0)
			{
				BitmapWidth = (DATA16)pBitmap[0];
				BitmapHeight = (DATA16)pBitmap[1];
				MaxX = (short)(X0 + BitmapWidth);
				pBitmapBytes = pBitmap.Skip(2).ToArray();

				if ((BitmapWidth >= 0) && (BitmapHeight >= 0))
				{
					if ((X0 % 8) != 0 || (BitmapWidth % 8) != 0)
					{ 
						// X is not aligned
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
					{ 
						// X is byte aligned
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

		public void dLcdRect(byte[] pImage, sbyte Color, short X0, short Y0, short X1, short Y1)
		{
			X1--;
			Y1--;
			dLcdDrawLine(pImage, Color, X0, Y0, (short)(X0 + X1), Y0);
			dLcdDrawLine(pImage, Color, (short)(X0 + X1), Y0, (short)(X0 + X1), (short)(Y0 + Y1));
			dLcdDrawLine(pImage, Color, (short)(X0 + X1), (short)(Y0 + Y1), X0, (short)(Y0 + Y1));
			dLcdDrawLine(pImage, Color, X0, (short)(Y0 + Y1), X0, Y0);
		}

		public void dLcdFillRect(byte[] pImage, sbyte Color, short X0, short Y0, short X1, short Y1)
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

		public void dLcdInverseRect(byte[] pImage, short X0, short Y0, short X1, short Y1)
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

		public void dLcdPlotLines(byte[] pImage, sbyte Color, short X0, short Y0, short X1, short Y1)
		{
			dLcdDrawLine(pImage, Color, (short)(X0 - X1), (short)(Y0 + Y1), (short)(X0 + X1), (short)(Y0 + Y1));
			dLcdDrawLine(pImage, Color, (short)(X0 - X1), (short)(Y0 - Y1), (short)(X0 + X1), (short)(Y0 - Y1));
			dLcdDrawLine(pImage, Color, (short)(X0 - Y1), (short)(Y0 + X1), (short)(X0 + Y1), (short)(Y0 + X1));
			dLcdDrawLine(pImage, Color, (short)(X0 - Y1), (short)(Y0 - X1), (short)(X0 + Y1), (short)(Y0 - X1));
		}

		public void dLcdDrawFilledCircle(byte[] pImage, sbyte Color, short X0, short Y0, short R)
		{
			short X = 0;
			short Y = R;
			int P = 3 - 2 * R;

			while (X < Y)
			{
				dLcdPlotLines(pImage, Color, X0, Y0, X, Y);
				if (P < 0)
				{
					P = P + 4 * X + 6;
				}
				else
				{
					P = P + 4 * (X - Y) + 10;
					Y = (short)(Y - 1);
				}
				X = (short)(X + 1);
			}
			dLcdPlotLines(pImage, Color, X0, Y0, X, Y);
		}

		public sbyte dLcdCheckPixel(byte[] pImage, sbyte Color, short X0, short Y0)
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

		public void dLcdFlodfill(byte[] pImage, sbyte Color, short X0, short Y0)
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
