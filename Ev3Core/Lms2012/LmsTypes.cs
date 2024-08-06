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

public interface IByteArrayCastable
{
    ArrayPointer<byte> CurrentPointer { get; set; }
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

public class ArrayPointer<T> : IPointer<T>, IEnumerable<T>, ICloneable
{
    public uint Offset = 0;
    public T[] Data = null;

    public int Length
    {
        get { return (int)(Data.Length - Offset); }
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

	public object Clone()
	{
		var n = new ArrayPointer<T>((T[])Data.Clone(), Offset);
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

public class IMGHEAD : IByteArrayCastable
{
    public UBYTE[] Sign //!< Place holder for the file type identifier
	{
		get
		{
			var e1 = CurrentPointer.GetUBYTE(false, 0);
			var e2 = CurrentPointer.GetUBYTE(false, 1);
			var e3 = CurrentPointer.GetUBYTE(false, 2);
			var e4 = CurrentPointer.GetUBYTE(false, 3);
			return new UBYTE[4] { e1, e2, e3, e4 };
		}
		set
		{
			CurrentPointer.SetUBYTE(value[0], false, 0);
			CurrentPointer.SetUBYTE(value[1], false, 1);
			CurrentPointer.SetUBYTE(value[2], false, 2);
			CurrentPointer.SetUBYTE(value[3], false, 3);
		}
	}                       
	public IMINDEX ImageSize  //!< Image size
	{
		get
		{
			return CurrentPointer.GetULONG(false, 4);
		}
		set
		{
			CurrentPointer.SetULONG(value, false, 4);
		}
	}                     
	public UWORD VersionInfo  //!< Version identifier
	{
		get
		{
			return CurrentPointer.GetUWORD(false, 8);
		}
		set
		{
			CurrentPointer.SetUWORD(value, false, 8);
		}
	}                  
	public OBJID NumberOfObjects  //!< Total number of objects in image
	{
		get
		{
			return CurrentPointer.GetUWORD(false, 10);
		}
		set
		{
			CurrentPointer.SetUWORD(value, false, 10);
		}
	}            
	public GBINDEX GlobalBytes  //!< Number of bytes to allocate for global variables
	{
		get
		{
			return CurrentPointer.GetULONG(false, 12);
		}
		set
		{
			CurrentPointer.SetULONG(value, false, 12);
		}
	}                  

	public const int Sizeof = 16;

	public static IMGHEAD GetObject(ArrayPointer<byte> arr, int tmpOffset = 0)
	{
		var el = new IMGHEAD();
		el.CurrentPointer = arr.Copy(tmpOffset);
		return el;
	}

	public static ArrayPointer<IMGHEAD> GetArray(ArrayPointer<byte> arr, int tmpOffset = 0)
	{
		List<IMGHEAD> tmp = new List<IMGHEAD>();
		for (int i = 0; i < arr.Length / Sizeof; ++i)
		{
			var el = new IMGHEAD();
			el.CurrentPointer = arr.Copy(tmpOffset + (i * Sizeof));
			tmp.Add(el);
		}
		return new ArrayPointer<IMGHEAD>(tmp.ToArray());
	}

	public GP CurrentPointer { get; set; } = new ArrayPointer<byte>(new byte[Sizeof]);

	public IMGHEAD()
    {
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
public class OBJHEAD : IByteArrayCastable                  // Object header
{
    private IP _offsetToInstructions = new IP();         //!< Offset to instructions from image start
    public IP OffsetToInstructions 
    { 
        get
        {
            var off = CurrentPointer.GetULONG(false, 0);
			return new ArrayPointer<byte>(_offsetToInstructions.Data, off);
        }
        set
        {
			_offsetToInstructions.Data = value.Data;
			CurrentPointer.SetULONG(value.Offset, false, 0);
		}
    }         
    public OBJID OwnerObjectId //!< Used by BLOCK's to hold the owner id
	{ 
        get 
        {
            return CurrentPointer.GetUWORD(false, 4);
		} 
        set 
        {
			CurrentPointer.SetUWORD(value, false, 4);
		}
    }                
    public TRIGGER TriggerCount //!< Used to determine how many triggers needed before the BLOCK object is activated
	{ 
        get 
        {
			return CurrentPointer.GetUWORD(false, 6);
		} 
        set 
        {
			CurrentPointer.SetUWORD(value, false, 6);
		} 
    }                 
    public LBINDEX LocalBytes  //!< Number of bytes to allocate for local variables
	{
		get
		{
			return CurrentPointer.GetULONG(false, 8);
		}
		set
		{
			CurrentPointer.SetULONG(value, false, 8);
		}
	}                  

	public const int Sizeof = 12;

	public static OBJHEAD GetObject(ArrayPointer<byte> arr, int tmpOffset = 0)
	{
		var el = new OBJHEAD();
		el.CurrentPointer = arr.Copy(tmpOffset);
		return el;
	}

	public static ArrayPointer<OBJHEAD> GetArray(ArrayPointer<byte> arr, int tmpOffset = 0)
    {
        List<OBJHEAD> tmp = new List<OBJHEAD>();
        for (int i = 0; i < arr.Length / Sizeof; ++i)
        {
            var el = new OBJHEAD();
            el.CurrentPointer = arr.Copy(tmpOffset + (i * Sizeof));
            tmp.Add(el);
		}
        return new ArrayPointer<OBJHEAD>(tmp.ToArray());
	}

	public GP CurrentPointer { get; set; } = new ArrayPointer<byte>(new byte[Sizeof]);
}

/*! \struct LABEL
 *          Label data hold information used for labels
 */
public class LABEL : IByteArrayCastable
{
	public IMINDEX Addr
	{
		get
		{
			return CurrentPointer.GetULONG(false, 0);
		}
		set
		{
			CurrentPointer.SetULONG(value, false, 0);
		}
	}                          //!< Offset to breakpoint address from image start

	public const int Sizeof = 4;

	public static LABEL GetObject(ArrayPointer<byte> arr, int tmpOffset = 0)
	{
		var el = new LABEL();
		el.CurrentPointer = arr.Copy(tmpOffset);
		return el;
	}

	public static ArrayPointer<LABEL> GetArray(ArrayPointer<byte> arr, int tmpOffset = 0)
	{
		List<LABEL> tmp = new List<LABEL>();
		for (int i = 0; i < arr.Length / Sizeof; ++i)
		{
			var el = new LABEL();
			el.CurrentPointer = arr.Copy(tmpOffset + (i * Sizeof));
			tmp.Add(el);
		}
		return new ArrayPointer<LABEL>(tmp.ToArray());
	}

	public GP CurrentPointer { get; set; } = new ArrayPointer<byte>(new byte[Sizeof]);
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