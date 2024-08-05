// lmstypes

global using UBYTE = byte;
global using UWORD = ushort;
global using ULONG = uint;

global using SBYTE = sbyte;
global using SWORD = short;
global using SLONG = int;

global using FLOAT = float;


global using DATA8 = sbyte;
global using DATA16 = short;
global using DATA32 = int;
global using DATAF = float;

global using VARDATA = byte;
global using IMGDATA = byte;

global using PRGID = ushort;

global using OBJID = ushort;

global using IP = ArrayPointer<byte>;
global using LP = ArrayPointer<byte>;
global using GP = ArrayPointer<byte>;

global using IMINDEX = uint;
global using GBINDEX = uint;
global using LBINDEX = uint;
global using TRIGGER = ushort;
global using PARS = byte;
global using IMOFFS = int;

global using HANDLER = short;

// from c_com.h
global using CMDSIZE = ushort;
global using MSGCNT = ushort;
using Ev3Core.Extensions;

public interface IByteCastable<T>
{
    T GetObject(ArrayPointer<byte> buff, bool updateOffset = false);
    void SetData(ArrayPointer<byte> buff, bool updateOffset = false);
}

public abstract class IPointer<T>
{
    public virtual ArrayPointer<T> AsArray()
    {
        return (ArrayPointer<T>)this;
    }

    public virtual VarPointer<T> AsVar()
    {
        return (VarPointer<T>)this;
    }
}

public class VarPointer<T> : IPointer<T>
{
    public T Data = default(T);

	public VarPointer() { }

	public VarPointer(T data)
	{
		Data = data;
	}
}

public class ArrayPointer<T> : IPointer<T>, IEnumerable<T>
{
    public uint Offset = 0;
    public T[] Data = null;

    public int Length
    {
        get { return Data.Skip((int)Offset).ToArray().Length; }
    }

    public ArrayPointer() { }

    public ArrayPointer(T[] data, uint offset = 0)
    {
        Data = data;
        Offset = offset;
    }

    public static ArrayPointer<ArrayPointer<T>> From2d(T[][] arr, uint offset = 0)
    {
        List<ArrayPointer<T>> tmp = new List<ArrayPointer<T>>();
        foreach (var a in arr)
        {
            tmp.Add(new ArrayPointer<T>(a, 0)); // is offset should be applied?
        }
        var ap = new ArrayPointer<ArrayPointer<T>>(tmp.ToArray(), offset);
        return ap;
    }

    public void PutData(IEnumerable<T> data)
    {
        int i = 0;
        foreach (var d in data)
        {
            if (i >= Data.Length)
                return;

            Data[i + Offset] = d;
            i++;
        }
    }

	public ArrayPointer<T> Copy(int offset = -1)
	{
		var n = new ArrayPointer<T>(Data, Offset);
		if (offset >= 0)
			n.Offset += (uint)offset;
		return n;
	}

	public ArrayPointer<T> Copy(uint offset = uint.MaxValue)
    {
        var n = new ArrayPointer<T>(Data, Offset);
        if (offset != uint.MaxValue)
            n.Offset += offset;
        return n;
    }

    /// <summary>
    /// Returns an an original array COPY! with first "offset" elements skipped
    /// </summary>
    /// <returns>A COPIED part of the original array</returns>
    public T[] GetSkipped()
    {
        return Data.Skip((int)Offset).ToArray();
	}

    // methods
    public static ArrayPointer<T> operator ++(ArrayPointer<T> arr)
    {
        arr.Offset++;
        return arr;
    }

    public static ArrayPointer<T> operator --(ArrayPointer<T> arr)
    {
        arr.Offset--;
        return arr;
    }

    public static ArrayPointer<T> operator +(ArrayPointer<T> arr, int val)
    {
        arr.Offset += (uint)val;
        return arr;
    }

    public static ArrayPointer<T> operator -(ArrayPointer<T> arr, int val)
    {
        arr.Offset -= (uint)val;
        return arr;
    }

    public static ArrayPointer<T> operator +(ArrayPointer<T> arr, ArrayPointer<T> arr2)
    {
        arr.Offset += (uint)arr.Offset;
        return arr;
    }

    public static ArrayPointer<T> operator -(ArrayPointer<T> arr, ArrayPointer<T> arr2)
    {
        arr.Offset -= (uint)arr.Offset;
        return arr;
    }

    public T this[int index]
    {
        get
        {
            // get the item for that index.
            return Data[index + Offset];
        }
        set
        {
            // set the item for this index. value will be of type Thing.
            Data[index + Offset] = value;
        }
    }

	public IEnumerator<T> GetEnumerator()
	{
		for (int i = (int)Offset; i < Data.Length; ++i)
        {
            yield return Data[i];
        }
	}

	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
        return this.GetEnumerator();
	}

	public static implicit operator int(ArrayPointer<T> arr)
    {
        return (int)arr.Offset;
    }

    public static implicit operator uint(ArrayPointer<T> arr)
    {
        return arr.Offset;
    }

    public static implicit operator T[](ArrayPointer<T> arr)
    {
        return arr.Data;
    }

    public static implicit operator T(ArrayPointer<T> arr)
    {
        return arr.Data[arr.Offset];
    }

    public static explicit operator ArrayPointer<T>(T[] arr)
    {
        return new ArrayPointer<T>(arr);
    }
}

public class IMGHEAD : IByteCastable<IMGHEAD>
{
    public UBYTE[] Sign = new UBYTE[4];                      //!< Place holder for the file type identifier
    public IMINDEX ImageSize;                    //!< Image size
    public UWORD VersionInfo;                  //!< Version identifier
    public OBJID NumberOfObjects;              //!< Total number of objects in image
    public GBINDEX GlobalBytes;                  //!< Number of bytes to allocate for global variables

    public const int SizeOf = 16;

    public IMGHEAD()
    {
    }

    public IMGHEAD GetObject(GP buff, bool updateOffset = false)
    {
        var prevOffset = buff.Offset;

        Sign[0] = buff.GetUBYTE(true);
        Sign[1] = buff.GetUBYTE(true);
        Sign[2] = buff.GetUBYTE(true);
        Sign[3] = buff.GetUBYTE(true);
        ImageSize = buff.GetULONG(true);
        VersionInfo = buff.GetUWORD(true);
        NumberOfObjects = buff.GetUWORD(true);
        GlobalBytes = buff.GetULONG(true);

        if (!updateOffset) buff.Offset = prevOffset;

        return this;
    }

    public void SetData(GP buff, bool updateOffset = false)
    {
        var prevOffset = buff.Offset;

        buff.SetUBYTE(Sign[0], true);
        buff.SetUBYTE(Sign[1], true);
        buff.SetUBYTE(Sign[2], true);
        buff.SetUBYTE(Sign[3], true);
        buff.SetULONG(ImageSize, true);
        buff.SetUWORD(VersionInfo, true);
        buff.SetUWORD(NumberOfObjects, true);
        buff.SetULONG(GlobalBytes, true);

        if (!updateOffset) buff.Offset = prevOffset;
    }
}

/*! \page imagelayout
 *
 *- OBJHEAD                             (aligned)
 *  - OffsetToInstructions              (4 bytes)
 *  - OwnerObjectId                     (2 bytes)
 *  - TriggerCount                      (2 bytes)
 *  - LocalBytes                        (4 bytes)
 *
 */

/*! \struct OBJHEAD
 *          Object header used for all types of objects (VMTHREAD, SUBCALL, BLOCK and ALIAS)
 */
public class OBJHEAD : IByteCastable<OBJHEAD>                  // Object header
{
    public IP OffsetToInstructions;         //!< Offset to instructions from image start
    public OBJID OwnerObjectId;                //!< Used by BLOCK's to hold the owner id
    public TRIGGER TriggerCount;                 //!< Used to determine how many triggers needed before the BLOCK object is activated
    public LBINDEX LocalBytes;                   //!< Number of bytes to allocate for local variables

	public const int SizeOf = 12;

    public OBJHEAD GetObject(GP buff, bool updateOffset = false)
    {
        var prevOffset = buff.Offset;

        OffsetToInstructions = new IP();
        OffsetToInstructions.Offset = buff.GetULONG(true); // it is OffsetToInstructions
        OwnerObjectId = buff.GetUWORD(true);
        TriggerCount = buff.GetUWORD(true);
        LocalBytes = buff.GetULONG(true);

        if (!updateOffset) buff.Offset = prevOffset;

        return this;
    }

    public void SetData(GP buff, bool updateOffset = false)
    {
        var prevOffset = buff.Offset;

        buff.SetULONG(OffsetToInstructions.Offset, true);
        buff.SetUWORD(OwnerObjectId, true);
        buff.SetUWORD(TriggerCount, true);
        buff.SetULONG(LocalBytes, true);

        if (!updateOffset) buff.Offset = prevOffset;
    }
}

/*! \struct LABEL
 *          Label data hold information used for labels
 */
public class LABEL : IByteCastable<LABEL>
{
	public IMINDEX Addr;                         //!< Offset to breakpoint address from image start

    public LABEL GetObject(GP buff, bool updateOffset = false)
    {
        var prevOffset = buff.Offset;

        Addr = buff.GetULONG(true);

        if (!updateOffset) buff.Offset = prevOffset;

        return this;
    }

    public void SetData(GP buff, bool updateOffset = false)
    {
        var prevOffset = buff.Offset;

        buff.SetULONG(Addr, true);

        if (!updateOffset) buff.Offset = prevOffset;
    }
}

// bytecodes.c

public class OPCODE
{
    public ULONG Pars;                         //!< Contains parameter info nibbles (max 8)
    public char[] Name;                        //!< Opcode name
}

public class SUBCODE
{
    public ULONG Pars;                         //!< Contains parameter info nibbles (max 8)
    public char[] Name;                        //!< Sub code name
}