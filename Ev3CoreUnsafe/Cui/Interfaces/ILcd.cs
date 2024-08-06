using Ev3CoreUnsafe.Lms2012.Interfaces;

namespace Ev3CoreUnsafe.Cui.Interfaces
{
	public interface ILcd
	{
		unsafe void dLcdUpdate(LCD* pLcd);

		unsafe void dLcdAutoUpdate();

		unsafe void dLcdInit(UBYTE* pImage);

		unsafe UBYTE dLcdRead();

		unsafe void dLcdExit();

		unsafe void dLcdScroll(UBYTE* pImage, DATA16 Y0);

		unsafe void dLcdDrawPixel(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0);

		unsafe void dLcdDrawLine(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 X1, DATA16 Y1);

		unsafe void dLcdDrawDotLine(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 X1, DATA16 Y1, DATA16 On, DATA16 Off);

		unsafe void dLcdRect(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 X1, DATA16 Y1);

		unsafe void dLcdFillRect(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 X1, DATA16 Y1);

		unsafe void dLcdInverseRect(UBYTE* pImage, DATA16 X0, DATA16 Y0, DATA16 X1, DATA16 Y1);

		unsafe void dLcdDrawCircle(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 R);

		unsafe DATA16 dLcdGetFontWidth(DATA8 Font);

		unsafe DATA16 dLcdGetFontHeight(DATA8 Font);

		unsafe void dLcdDrawChar(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA8 Font, DATA8 Char);

		unsafe void dLcdDrawText(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA8 Font, DATA8* pText);

		unsafe DATA16 dLcdGetIconWidth(DATA8 Type);

		unsafe DATA16 dLcdGetIconHeight(DATA8 Type);

		unsafe void dLcdDrawPicture(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 IconWidth, DATA16 IconHeight, UBYTE* pIconBits);

		unsafe void dLcdDrawIcon(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA8 Type, DATA8 No);

		unsafe void dLcdGetBitmapSize(IP pBitmap, DATA16* pWidth, DATA16* pHeight);

		unsafe void dLcdDrawBitmap(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0, IP pBitmap);

		unsafe void dLcdDrawFilledCircle(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0, DATA16 R);


		unsafe void dLcdFlodfill(UBYTE* pImage, DATA8 Color, DATA16 X0, DATA16 Y0);
	}

	public unsafe struct FONTINFO
	{
		public UBYTE* pFontBits;           // Pointer to start of font bitmap
		public DATA16 FontHeight;           // Character height (all inclusive)
		public DATA16 FontWidth;            // Character width (all inclusive)
		public DATA16 FontHorz;             // Number of horizontal character in font bitmap
		public DATA8 FontFirst;            // First character supported
		public DATA8 FontLast;             // Last character supported
	}

	public unsafe struct ICONINFO
	{
		public UBYTE* pIconBits;
		public DATA16 IconSize;
		public DATA16 IconHeight;
		public DATA16 IconWidth;
	}
}
