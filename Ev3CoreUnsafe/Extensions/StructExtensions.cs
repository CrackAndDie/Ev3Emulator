using Ev3CoreUnsafe.Cmemory.Interfaces;
using Ev3CoreUnsafe.Cui.Interfaces;
using Ev3CoreUnsafe.Lms2012.Interfaces;
using System.Xml.Linq;

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

		public unsafe static ULONG* GetCalib(this COLORSTRUCT color, int n)
		{
			switch (n)
			{
				case 0:
					return color.Calibration0;
				case 1:
					return color.Calibration1;
				case 2:
					return color.Calibration2;
			}
			return null;
		}

        public unsafe static DATA8* GetRaw(this UART color, int n)
        {
            switch (n)
            {
                case 0:
                    return color.Raw0;
                case 1:
                    return color.Raw1;
                case 2:
                    return color.Raw2;
                case 3:
                    return color.Raw3;
            }
            return null;
        }

        public unsafe static DATA8* GetRaw(this IIC color, int n)
        {
            switch (n)
            {
                case 0:
                    return color.Raw0;
                case 1:
                    return color.Raw1;
                case 2:
                    return color.Raw2;
                case 3:
                    return color.Raw3;
            }
            return null;
        }

        #region folder entries
        public unsafe static DATA8* GetEntry(this FOLDER folder, int n)
        {
            switch (n)
            {
                case 0:
                    return folder.Entry0;
                case 1:
                    return folder.Entry1;
                case 2:
                    return folder.Entry2;
                case 3:
                    return folder.Entry3;
                case 4:
                    return folder.Entry4;
                case 5:
                    return folder.Entry5;
                case 6:
                    return folder.Entry6;
                case 7:
                    return folder.Entry7;
                case 8:
                    return folder.Entry8;
                case 9:
                    return folder.Entry9;
                case 10:
                    return folder.Entry10;
                case 11:
                    return folder.Entry11;
                case 12:
                    return folder.Entry12;
                case 13:
                    return folder.Entry13;
                case 14:
                    return folder.Entry14;
                case 15:
                    return folder.Entry15;
                case 16:
                    return folder.Entry16;
                case 17:
                    return folder.Entry17;
                case 18:
                    return folder.Entry18;
                case 19:
                    return folder.Entry19;
                case 20:
                    return folder.Entry20;
                case 21:
                    return folder.Entry21;
                case 22:
                    return folder.Entry22;
                case 23:
                    return folder.Entry23;
                case 24:
                    return folder.Entry24;
                case 25:
                    return folder.Entry25;
                case 26:
                    return folder.Entry26;
                case 27:
                    return folder.Entry27;
                case 28:
                    return folder.Entry28;
                case 29:
                    return folder.Entry29;
                case 30:
                    return folder.Entry30;
                case 31:
                    return folder.Entry31;
                case 32:
                    return folder.Entry32;
                case 33:
                    return folder.Entry33;
                case 34:
                    return folder.Entry34;
                case 35:
                    return folder.Entry35;
                case 36:
                    return folder.Entry36;
                case 37:
                    return folder.Entry37;
                case 38:
                    return folder.Entry38;
                case 39:
                    return folder.Entry39;
                case 40:
                    return folder.Entry40;
                case 41:
                    return folder.Entry41;
                case 42:
                    return folder.Entry42;
                case 43:
                    return folder.Entry43;
                case 44:
                    return folder.Entry44;
                case 45:
                    return folder.Entry45;
                case 46:
                    return folder.Entry46;
                case 47:
                    return folder.Entry47;
                case 48:
                    return folder.Entry48;
                case 49:
                    return folder.Entry49;
                case 50:
                    return folder.Entry50;
                case 51:
                    return folder.Entry51;
                case 52:
                    return folder.Entry52;
                case 53:
                    return folder.Entry53;
                case 54:
                    return folder.Entry54;
                case 55:
                    return folder.Entry55;
                case 56:
                    return folder.Entry56;
                case 57:
                    return folder.Entry57;
                case 58:
                    return folder.Entry58;
                case 59:
                    return folder.Entry59;
                case 60:
                    return folder.Entry60;
                case 61:
                    return folder.Entry61;
                case 62:
                    return folder.Entry62;
                case 63:
                    return folder.Entry63;
                case 64:
                    return folder.Entry64;
                case 65:
                    return folder.Entry65;
                case 66:
                    return folder.Entry66;
                case 67:
                    return folder.Entry67;
                case 68:
                    return folder.Entry68;
                case 69:
                    return folder.Entry69;
                case 70:
                    return folder.Entry70;
                case 71:
                    return folder.Entry71;
                case 72:
                    return folder.Entry72;
                case 73:
                    return folder.Entry73;
                case 74:
                    return folder.Entry74;
                case 75:
                    return folder.Entry75;
                case 76:
                    return folder.Entry76;
                case 77:
                    return folder.Entry77;
                case 78:
                    return folder.Entry78;
                case 79:
                    return folder.Entry79;
                case 80:
                    return folder.Entry80;
                case 81:
                    return folder.Entry81;
                case 82:
                    return folder.Entry82;
                case 83:
                    return folder.Entry83;
                case 84:
                    return folder.Entry84;
                case 85:
                    return folder.Entry85;
                case 86:
                    return folder.Entry86;
                case 87:
                    return folder.Entry87;
                case 88:
                    return folder.Entry88;
                case 89:
                    return folder.Entry89;
                case 90:
                    return folder.Entry90;
                case 91:
                    return folder.Entry91;
                case 92:
                    return folder.Entry92;
                case 93:
                    return folder.Entry93;
                case 94:
                    return folder.Entry94;
                case 95:
                    return folder.Entry95;
                case 96:
                    return folder.Entry96;
                case 97:
                    return folder.Entry97;
                case 98:
                    return folder.Entry98;
                case 99:
                    return folder.Entry99;
                case 100:
                    return folder.Entry100;
                case 101:
                    return folder.Entry101;
                case 102:
                    return folder.Entry102;
                case 103:
                    return folder.Entry103;
                case 104:
                    return folder.Entry104;
                case 105:
                    return folder.Entry105;
                case 106:
                    return folder.Entry106;
                case 107:
                    return folder.Entry107;
                case 108:
                    return folder.Entry108;
                case 109:
                    return folder.Entry109;
                case 110:
                    return folder.Entry110;
                case 111:
                    return folder.Entry111;
                case 112:
                    return folder.Entry112;
                case 113:
                    return folder.Entry113;
                case 114:
                    return folder.Entry114;
                case 115:
                    return folder.Entry115;
                case 116:
                    return folder.Entry116;
                case 117:
                    return folder.Entry117;
                case 118:
                    return folder.Entry118;
                case 119:
                    return folder.Entry119;
                case 120:
                    return folder.Entry120;
                case 121:
                    return folder.Entry121;
                case 122:
                    return folder.Entry122;
                case 123:
                    return folder.Entry123;
                case 124:
                    return folder.Entry124;
                case 125:
                    return folder.Entry125;
                case 126:
                    return folder.Entry126;
            }
            return null;
        }
        #endregion
    }
}
