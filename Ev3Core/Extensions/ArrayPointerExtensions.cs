using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ev3Core.Extensions
{
    public static class ArrayPointerExtensions
    {
        public static DATA8 GetDATA8(this ArrayPointer<byte> buf, bool updateOffset = false, uint tmpOffset = 0)
        {
            var ret = (DATA8)buf.Data[buf.Offset + tmpOffset];
            if (updateOffset) buf.Offset += 1;
            return ret;
        }

        public static UBYTE GetUBYTE(this ArrayPointer<byte> buf, bool updateOffset = false, uint tmpOffset = 0)
        {
            var ret = buf.Data[buf.Offset + tmpOffset];
            if (updateOffset) buf.Offset += 1;
            return ret;
        }

        public static DATA16 GetDATA16(this ArrayPointer<byte> buf, bool updateOffset = false, uint tmpOffset = 0)
        {
            var ret = (DATA16)(((DATA16)buf.Data[buf.Offset + tmpOffset]) + ((DATA16)buf.Data[buf.Offset + tmpOffset + 1] << 8));
            if (updateOffset) buf.Offset += 2;
            return ret;
        }

        public static UWORD GetUWORD(this ArrayPointer<byte> buf, bool updateOffset = false, uint tmpOffset = 0)
        {
            var ret = (UWORD)(((UWORD)buf.Data[buf.Offset + tmpOffset]) + ((UWORD)buf.Data[buf.Offset + tmpOffset + 1] << 8));
            if (updateOffset) buf.Offset += 2;
            return ret;
        }

        public static DATA32 GetDATA32(this ArrayPointer<byte> buf, bool updateOffset = false, uint tmpOffset = 0)
        {
            var ret = (DATA32)(((DATA32)buf.Data[buf.Offset + tmpOffset]) + ((DATA32)buf.Data[buf.Offset + tmpOffset + 1] << 8) + ((DATA32)buf.Data[buf.Offset + tmpOffset + 2] << 16) + ((DATA32)buf.Data[buf.Offset + tmpOffset + 3] << 24));
            if (updateOffset) buf.Offset += 4;
            return ret;
        }

        public static ULONG GetULONG(this ArrayPointer<byte> buf, bool updateOffset = false, uint tmpOffset = 0)
        {
            var ret = (ULONG)(((ULONG)buf.Data[buf.Offset + tmpOffset]) + ((ULONG)buf.Data[buf.Offset + tmpOffset + 1] << 8) + ((ULONG)buf.Data[buf.Offset + tmpOffset + 2] << 16) + ((ULONG)buf.Data[buf.Offset + tmpOffset + 3] << 24));
            if (updateOffset) buf.Offset += 4;
            return ret;
        }

        public static DATAF GetDATAF(this ArrayPointer<byte> buf, bool updateOffset = false, uint tmpOffset = 0)
        {
            var ret = (DATAF)(((DATA32)buf.Data[buf.Offset + tmpOffset]) + ((DATA32)buf.Data[buf.Offset + tmpOffset + 1] << 8) + ((DATA32)buf.Data[buf.Offset + tmpOffset + 2] << 16) + ((DATA32)buf.Data[buf.Offset + tmpOffset + 3] << 24));
            if (updateOffset) buf.Offset += 4;
            return ret;
        }

        public static void SetDATA8(this ArrayPointer<byte> buf, DATA8 val, bool updateOffset = false, uint tmpOffset = 0)
        {
            buf.Data[buf.Offset + tmpOffset] = (byte)val;
            if (updateOffset) buf.Offset += 1;
        }

        public static void SetUBYTE(this ArrayPointer<byte> buf, UBYTE val, bool updateOffset = false, uint tmpOffset = 0)
        {
            buf.Data[buf.Offset + tmpOffset] = val;
            if (updateOffset) buf.Offset += 1;
        }

        public static void SetDATA16(this ArrayPointer<byte> buf, DATA16 val, bool updateOffset = false, uint tmpOffset = 0)
        {
            buf.Data[buf.Offset + tmpOffset] = (byte)(val & 0xff);
            buf.Data[buf.Offset + tmpOffset + 1] = (byte)((val >> 8) & 0xff);
            if (updateOffset) buf.Offset += 2;
        }

        public static void SetUWORD(this ArrayPointer<byte> buf, UWORD val, bool updateOffset = false, uint tmpOffset = 0)
        {
            buf.Data[buf.Offset + tmpOffset] = (byte)(val & 0xff);
            buf.Data[buf.Offset + tmpOffset + 1] = (byte)((val >> 8) & 0xff);
            if (updateOffset) buf.Offset += 2;
        }

        public static void SetDATA32(this ArrayPointer<byte> buf, DATA32 val, bool updateOffset = false, uint tmpOffset = 0)
        {
            buf.Data[buf.Offset + tmpOffset] = (byte)(val & 0xff);
            buf.Data[buf.Offset + tmpOffset + 1] = (byte)((val >> 8) & 0xff);
            buf.Data[buf.Offset + tmpOffset + 2] = (byte)((val >> 16) & 0xff);
            buf.Data[buf.Offset + tmpOffset + 3] = (byte)((val >> 24) & 0xff);
            if (updateOffset) buf.Offset += 4;
        }

        public static void SetULONG(this ArrayPointer<byte> buf, ULONG val, bool updateOffset = false, uint tmpOffset = 0)
        {
            buf.Data[buf.Offset + tmpOffset] = (byte)(val & 0xff);
            buf.Data[buf.Offset + tmpOffset + 1] = (byte)((val >> 8) & 0xff);
            buf.Data[buf.Offset + tmpOffset + 2] = (byte)((val >> 16) & 0xff);
            buf.Data[buf.Offset + tmpOffset + 3] = (byte)((val >> 24) & 0xff);
            if (updateOffset) buf.Offset += 4;
        }

        public static void SetDATAF(this ArrayPointer<byte> buf, DATAF val, bool updateOffset = false, uint tmpOffset = 0)
        {
            // TODO: probably another way
            //buf.Data[buf.Offset] = (byte)(val & 0xff);
            //buf.Data[buf.Offset + 1] = (byte)((val >> 8) & 0xff);
            //buf.Data[buf.Offset + 2] = (byte)((val >> 16) & 0xff);
            //buf.Data[buf.Offset + 3] = (byte)((val >> 24) & 0xff);
            //if (updateOffset) buf.Offset += 4;
        }

        public static T GetObject<T>(this ArrayPointer<byte> buf, IByteCastable<T> inst, bool updateOffset = false)
        {
            return inst.GetObject(buf, updateOffset);
        }

        public static void SetObject<T>(this ArrayPointer<byte> buf, IByteCastable<T> inst, bool updateOffset = false)
        {
            inst.SetData(buf, updateOffset);
        }

		#region arrays shite
		public static DATA16[] GetArrayDATA16(this ArrayPointer<byte> buf, bool updateOffset = false, uint tmpOffset = 0)
		{
            List<DATA16> tmp = new List<short>();
            for (int i = 0; i < (buf.Length - buf.Offset - tmpOffset) / 2; ++i)
            {
                tmp.Add(buf.GetDATA16(false, (uint)(i * 2) + tmpOffset));
			}
            if (updateOffset) buf.Offset += (uint)(tmp.Count * 2);
            return tmp.ToArray();
		}

		public static UWORD[] GetArrayUWORD(this ArrayPointer<byte> buf, bool updateOffset = false, uint tmpOffset = 0)
		{
			List<UWORD> tmp = new List<ushort>();
			for (int i = 0; i < (buf.Length - buf.Offset - tmpOffset) / 2; ++i)
			{
				tmp.Add(buf.GetUWORD(false, (uint)(i * 2) + tmpOffset));
			}
			if (updateOffset) buf.Offset += (uint)(tmp.Count * 2);
			return tmp.ToArray();
		}

		public static DATA32[] GetArrayDATA32(this ArrayPointer<byte> buf, bool updateOffset = false, uint tmpOffset = 0)
		{
			List<DATA32> tmp = new List<DATA32>();
			for (int i = 0; i < (buf.Length - buf.Offset - tmpOffset) / 4; ++i)
			{
				tmp.Add(buf.GetDATA32(false, (uint)(i * 4) + tmpOffset));
			}
			if (updateOffset) buf.Offset += (uint)(tmp.Count * 4);
			return tmp.ToArray();
		}

		public static ULONG[] GetArrayULONG(this ArrayPointer<byte> buf, bool updateOffset = false, uint tmpOffset = 0)
		{
			List<ULONG> tmp = new List<ULONG>();
			for (int i = 0; i < (buf.Length - buf.Offset - tmpOffset) / 4; ++i)
			{
				tmp.Add(buf.GetULONG(false, (uint)(i * 4) + tmpOffset));
			}
			if (updateOffset) buf.Offset += (uint)(tmp.Count * 4);
			return tmp.ToArray();
		}

		public static DATAF[] GetArrayDATAF(this ArrayPointer<byte> buf, bool updateOffset = false, uint tmpOffset = 0)
		{
			List<DATAF> tmp = new List<DATAF>();
			for (int i = 0; i < (buf.Length - buf.Offset - tmpOffset) / 4; ++i)
			{
				tmp.Add(buf.GetDATAF(false, (uint)(i * 4) + tmpOffset));
			}
			if (updateOffset) buf.Offset += (uint)(tmp.Count * 4);
			return tmp.ToArray();
		}

		public static void SetArrayDATA16(this ArrayPointer<byte> buf, DATA16[] data, bool updateOffset = false, uint tmpOffset = 0)
		{
            int dataI = 0;
			for (int i = 0; i < (buf.Length - buf.Offset - tmpOffset) / 2; ++i)
			{
				buf.SetDATA16(data[dataI], false, (uint)(i * 2) + tmpOffset);
                dataI++;
			}
			if (updateOffset) buf.Offset += (uint)(data.Length * 2);
		}

		public static void SetArrayUWORD(this ArrayPointer<byte> buf, UWORD[] data, bool updateOffset = false, uint tmpOffset = 0)
		{
			int dataI = 0;
			for (int i = 0; i < (buf.Length - buf.Offset - tmpOffset) / 2; ++i)
			{
				buf.SetUWORD(data[dataI], false, (uint)(i * 2) + tmpOffset);
				dataI++;
			}
			if (updateOffset) buf.Offset += (uint)(data.Length * 2);
		}

		public static void SetArrayDATA32(this ArrayPointer<byte> buf, DATA32[] data, bool updateOffset = false, uint tmpOffset = 0)
		{
			int dataI = 0;
			for (int i = 0; i < (buf.Length - buf.Offset - tmpOffset) / 4; ++i)
			{
				buf.SetDATA32(data[dataI], false, (uint)(i * 4) + tmpOffset);
				dataI++;
			}
			if (updateOffset) buf.Offset += (uint)(data.Length * 4);
		}

		public static void SetArrayULONG(this ArrayPointer<byte> buf, ULONG[] data, bool updateOffset = false, uint tmpOffset = 0)
		{
			int dataI = 0;
			for (int i = 0; i < (buf.Length - buf.Offset - tmpOffset) / 4; ++i)
			{
				buf.SetULONG(data[dataI], false, (uint)(i * 4) + tmpOffset);
				dataI++;
			}
			if (updateOffset) buf.Offset += (uint)(data.Length * 4);
		}

		public static void SetArrayDATAF(this ArrayPointer<byte> buf, DATAF[] data, bool updateOffset = false, uint tmpOffset = 0)
		{
			int dataI = 0;
			for (int i = 0; i < (buf.Length - buf.Offset - tmpOffset) / 4; ++i)
			{
				buf.SetDATAF(data[dataI], false, (uint)(i * 4) + tmpOffset);
				dataI++;
			}
			if (updateOffset) buf.Offset += (uint)(data.Length * 4);
		}
		#endregion
	}
}
