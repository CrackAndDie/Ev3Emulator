using EV3DecompilerLib.Decompile;

namespace Ev3EmulatorCore.Lms.Cui
{
	public partial class ClcdClass
	{
		public struct LCD
		{
			public byte[] Lcd = new byte[lms2012.LCD_BUFFER_SIZE];

			public LCD()
			{
			}
		}
	}
}
