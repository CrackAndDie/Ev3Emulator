using EV3DecompilerLib.Decompile;

namespace Ev3EmulatorCore.Lms.Cui
{
	public partial class DlcdClass
	{
		public class LCD : ICloneable
		{
			public byte[] Lcd = new byte[lms2012.LCD_BUFFER_SIZE];

			public LCD()
			{
			}

			public object Clone()
			{
				return new LCD()
				{
					Lcd = (byte[])Lcd.Clone(),
				};
			}
		}
	}
}
