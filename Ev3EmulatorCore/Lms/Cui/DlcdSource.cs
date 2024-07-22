using EV3DecompilerLib.Decompile;

namespace Ev3EmulatorCore.Lms.Cui
{
	public partial class DlcdClass
	{
		public void dLcdInit(byte[] image)
		{
			// TODO
		}

		public byte dLcdRead()
		{
			// TODO
			return 0;
		}

		public void dLcdDrawIcon(LCD lcd, byte color, short x0, short y0, lms2012.IconType tp, byte no) 
		{
			// TODO
		}

		public short dLcdGetIconWidth(lms2012.IconType icon)
		{
			// TODO
			return 0;
		}

		public short dLcdGetFontWidth(lms2012.FontType font)
		{
			// TODO
			return 0;
		}

		public void dLcdDrawText(LCD lcd, byte color, short x0, short y0, lms2012.FontType font, byte[] text)
		{
			// TODO
		}

		public void dLcdDrawLine(LCD lcd, byte color, short x0, short y0, short x1, short y1)
		{
			// TODO
		}

		public void dLcdDrawPicture(LCD lcd, byte color, short x0, short y0, short iconWidth, short iconHeight, byte[] data)
		{
			// TODO
		}

		public void dLcdUpdate(LCD lcd)
		{
			// TODO
		}

        public void LcdClearTopline(DlcdClass.LCD lcd)
		{
            for (int i = 0; i < lms2012.LCD_TOPLINE_SIZE; ++i)
            {
                lcd.Lcd[i] = (byte)lms2012.BG_COLOR;
            }
        }

        public void LcdClear(DlcdClass.LCD lcd)
        {
            lcd.Lcd = Enumerable.Repeat((byte)lms2012.BG_COLOR, lms2012.LCD_BUFFER_SIZE).ToArray();
        }

        public void LcdErase(DlcdClass.LCD lcd)
        {
			for (int i = lms2012.LCD_TOPLINE_SIZE; i < lms2012.LCD_BUFFER_SIZE; ++i)
			{
                lcd.Lcd[i] = (byte)lms2012.BG_COLOR;
            }
        }
    }
}
