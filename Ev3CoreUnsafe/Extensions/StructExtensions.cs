using Ev3CoreUnsafe.Cui.Interfaces;

namespace Ev3CoreUnsafe.Extensions
{
	public static class StructExtensions
	{
		public unsafe static DATA8* GetTextline(this NOTIFY nf, int n)
		{
			switch (n)
			{
				case 0:
					return nf.TextLine0;
				case 1:
					return nf.TextLine1;
				case 2:
					return nf.TextLine2;
				case 3:
					return nf.TextLine3;
				case 4:
					return nf.TextLine4;
				case 5:
					return nf.TextLine5;
				case 6:
					return nf.TextLine6;
				case 7:
					return nf.TextLine7;
			}
			return null;
		}

		public unsafe static DATAF* GetBuffer(this GRAPH gp, int n)
		{
			switch (n)
			{
				case 0:
					return gp.Buffer0;
				case 1:
					return gp.Buffer1;
				case 2:
					return gp.Buffer2;
				case 3:
					return gp.Buffer3;
				case 4:
					return gp.Buffer4;
				case 5:
					return gp.Buffer5;
				case 6:
					return gp.Buffer6;
				case 7:
					return gp.Buffer7;
			}
			return null;
		}
	}
}
