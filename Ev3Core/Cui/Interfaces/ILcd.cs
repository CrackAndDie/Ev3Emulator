using Ev3Core.Lms2012.Interfaces;

namespace Ev3Core.Cui.Interfaces
{
	public interface ILcd
	{
		void dLcdUpdate(LCD pLcd);

		void dLcdAutoUpdate();

		void dLcdInit(UBYTE[] pImage);

		UBYTE dLcdRead();

		void dLcdExit();

		void dLcdScroll(UBYTE[] pImage, DATA16 Y0);

		void dLcdDrawPixel(UBYTE[] pImage, DATA8 Color, DATA16 X0, DATA16 Y0);

		void dLcdDrawLine(UBYTE[] pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 X1, DATA16 Y1);

		void dLcdDrawDotLine(UBYTE[] pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 X1, DATA16 Y1, DATA16 On, DATA16 Off);

		void dLcdRect(UBYTE[] pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 X1, DATA16 Y1);

		void dLcdFillRect(UBYTE[] pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 X1, DATA16 Y1);

		void dLcdInverseRect(UBYTE[] pImage, DATA16 X0, DATA16 Y0, DATA16 X1, DATA16 Y1);

		void dLcdDrawCircle(UBYTE[] pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 R);

		DATA16 dLcdGetFontWidth(DATA8 Font);

		DATA16 dLcdGetFontHeight(DATA8 Font);

		void dLcdDrawChar(UBYTE[] pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA8 Font, DATA8 Char);

		void dLcdDrawText(UBYTE[] pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA8 Font, DATA8[] pText);

		DATA16 dLcdGetIconWidth(DATA8 Type);

		DATA16 dLcdGetIconHeight(DATA8 Type);

		void dLcdDrawPicture(UBYTE[] pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 IconWidth, DATA16 IconHeight, UBYTE[] pIconBits);

		void dLcdDrawIcon(UBYTE[] pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA8 Type, DATA8 No);

		void dLcdGetBitmapSize(IP pBitmap, ref DATA16 pWidth, ref DATA16 pHeight);

		void dLcdDrawBitmap(UBYTE[] pImage, DATA8 Color, DATA16 X0, DATA16 Y0, IP pBitmap);

		void dLcdDrawFilledCircle(UBYTE[] pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 R);


		void dLcdFlodfill(UBYTE[] pImage, DATA8 Color, DATA16 X0, DATA16 Y0);
	}

	public class FONTINFO
	{
		public byte[] pFontBits;           // Pointer to start of font bitmap
		public DATA16 FontHeight;           // Character height (all inclusive)
		public DATA16 FontWidth;            // Character width (all inclusive)
		public DATA16 FontHorz;             // Number of horizontal character in font bitmap
		public DATA8 FontFirst;            // First character supported
		public DATA8 FontLast;             // Last character supported
	}

	public class ICONINFO
	{
		public byte[] pIconBits;
		public DATA16 IconSize;
		public DATA16 IconHeight;
		public DATA16 IconWidth;
	}
}
