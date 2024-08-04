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
    }
}
