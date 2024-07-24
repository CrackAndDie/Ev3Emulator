using EV3DecompilerLib.Decompile;

namespace Ev3EmulatorCore.Lms.Cui
{
	public partial class DlcdClass
	{
		public unsafe void dLcdInit(UBYTE* image)
		{
			// TODO
		}

		public UBYTE dLcdRead()
		{
			// TODO
			return 0;
		}

		public unsafe void dLcdDrawIcon(UBYTE* lcd, DATA8 color, DATA16 x0, DATA16 y0, DATA8 tp, DATA8 no) 
		{
			// TODO
		}

		public DATA16 dLcdGetIconWidth(DATA8 icon)
		{
			// TODO
			return 0;
		}

		public DATA16 dLcdGetIconHeight(DATA8 icon)
		{
			// TODO
			return 0;
		}

		public DATA16 dLcdGetFontWidth(DATA8 font)
		{
			// TODO
			return 0;
		}

		public DATA16 dLcdGetFontHeight(DATA8 font)
		{
			// TODO
			return 0;
		}

		public unsafe void dLcdDrawText(UBYTE* lcd, DATA8 color, DATA16 x0, DATA16 y0, DATA8 font, DATA8* text)
		{
			// TODO
		}

		public unsafe void dLcdDrawChar(UBYTE* lcd, DATA8 color, DATA16 x0, DATA16 y0, DATA8 font, DATA8 data)
		{
			// TODO
		}

		public unsafe void dLcdDrawLine(UBYTE* lcd, DATA8 color, DATA16 x0, DATA16 y0, DATA16 x1, DATA16 y1)
		{
			// TODO
		}

		public unsafe void dLcdDrawDotLine(UBYTE* lcd, DATA8 color, DATA16 x0, DATA16 y0, DATA16 x1, DATA16 y1, DATA16 on, DATA16 off)
		{
			// TODO
		}

		public unsafe void dLcdRect(UBYTE* lcd, DATA8 color, DATA16 x0, DATA16 y0, DATA16 x1, DATA16 y1)
        {
            // TODO
        }

		public unsafe void dLcdFillRect(UBYTE* lcd, DATA8 color, DATA16 x0, DATA16 y0, DATA16 x1, DATA16 y1)
        {
            // TODO
        }

		public unsafe void dLcdDrawPixel(UBYTE* lcd, DATA8 color, DATA16 x0, DATA16 y0)
        {
            // TODO
        }

		public unsafe void dLcdDrawPicture(UBYTE* lcd, DATA8 color, DATA16 x0, DATA16 y0, DATA16 iconWidth, DATA16 iconHeight, UBYTE* data)
		{
			// TODO
		}

		public unsafe void dLcdDrawBitmap(UBYTE* lcd, DATA8 color, DATA16 x0, DATA16 y0, IP data)
		{
			// TODO
		}

		public unsafe void dLcdInverseRect(UBYTE* lcd, DATA16 x0, DATA16 y0, DATA16 x1, DATA16 y1)
		{
			// TODO
		}

		public unsafe void dLcdUpdate(UBYTE* lcd)
		{
			// TODO
		}

        public unsafe void LcdClearTopline(UBYTE* lcd)
		{
            for (int i = 0; i < lms2012.LCD_TOPLINE_SIZE; ++i)
            {
                lcd[i] = (byte)lms2012.BG_COLOR;
            }
        }

        public unsafe void LcdClear(UBYTE* lcd)
        {
			for (int i = 0; i < lms2012.LCD_BUFFER_SIZE; ++i)
			{
				lcd[i] = (byte)lms2012.BG_COLOR;
			}
		}

        public unsafe void LcdErase(UBYTE* lcd)
        {
			for (int i = lms2012.LCD_TOPLINE_SIZE; i < lms2012.LCD_BUFFER_SIZE; ++i)
			{
                lcd[i] = (byte)lms2012.BG_COLOR;
            }
        }
    }
}
