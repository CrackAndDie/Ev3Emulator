using Ev3Core.Lms2012.Interfaces;

namespace Ev3Core.Cui.Interfaces
{
	public interface ILcd
	{
		void dLcdUpdate(LCD pLcd);

		void dLcdAutoUpdate();

		void dLcdInit(ArrayPointer<UBYTE> pImage);

		UBYTE dLcdRead();

		void dLcdExit();

		void dLcdScroll(ArrayPointer<UBYTE> pImage, DATA16 Y0);

		void dLcdDrawPixel(ArrayPointer<UBYTE> pImage, DATA8 Color, DATA16 X0, DATA16 Y0);

		void dLcdDrawLine(ArrayPointer<UBYTE> pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 X1, DATA16 Y1);

		void dLcdDrawDotLine(ArrayPointer<UBYTE> pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 X1, DATA16 Y1, DATA16 On, DATA16 Off);

		void dLcdRect(ArrayPointer<UBYTE> pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 X1, DATA16 Y1);

		void dLcdFillRect(ArrayPointer<UBYTE> pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 X1, DATA16 Y1);

		void dLcdInverseRect(ArrayPointer<UBYTE> pImage, DATA16 X0, DATA16 Y0, DATA16 X1, DATA16 Y1);

		void dLcdDrawCircle(ArrayPointer<UBYTE> pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 R);

		DATA16 dLcdGetFontWidth(DATA8 Font);

		DATA16 dLcdGetFontHeight(DATA8 Font);

		void dLcdDrawChar(ArrayPointer<UBYTE> pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA8 Font, UBYTE Char);

		void dLcdDrawText(ArrayPointer<UBYTE> pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA8 Font, ArrayPointer<UBYTE> pText);

		DATA16 dLcdGetIconWidth(DATA8 Type);

		DATA16 dLcdGetIconHeight(DATA8 Type);

		void dLcdDrawPicture(ArrayPointer<UBYTE> pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 IconWidth, DATA16 IconHeight, ArrayPointer<UBYTE> pIconBits);

		void dLcdDrawIcon(ArrayPointer<UBYTE> pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA8 Type, DATA8 No);

		void dLcdGetBitmapSize(IP pBitmap, VarPointer<DATA16> pWidth, VarPointer<DATA16> pHeight);

		void dLcdDrawBitmap(ArrayPointer<UBYTE> pImage, DATA8 Color, DATA16 X0, DATA16 Y0, IP pBitmap);

		void dLcdDrawFilledCircle(ArrayPointer<UBYTE> pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 R);


		void dLcdFlodfill(ArrayPointer<UBYTE> pImage, DATA8 Color, DATA16 X0, DATA16 Y0);
	}

	public class FONTINFO
	{
		public ArrayPointer<UBYTE> pFontBits;           // Pointer to start of font bitmap
		public DATA16 FontHeight;           // Character height (all inclusive)
		public DATA16 FontWidth;            // Character width (all inclusive)
		public DATA16 FontHorz;             // Number of horizontal character in font bitmap
		public DATA8 FontFirst;            // First character supported
		public DATA8 FontLast;             // Last character supported
	}

	public class ICONINFO
	{
		public ArrayPointer<UBYTE> pIconBits;
		public DATA16 IconSize;
		public DATA16 IconHeight;
		public DATA16 IconWidth;
	}
}
