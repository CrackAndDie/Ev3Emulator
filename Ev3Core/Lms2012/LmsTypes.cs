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
global using IP = byte[];
global using LP = byte[];
global using GP = byte[];

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

public class IMGHEAD
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
public class OBJHEAD                   // Object header
{
    public IP OffsetToInstructions;         //!< Offset to instructions from image start
    public OBJID OwnerObjectId;                //!< Used by BLOCK's to hold the owner id
    public TRIGGER TriggerCount;                 //!< Used to determine how many triggers needed before the BLOCK object is activated
    public LBINDEX LocalBytes;                   //!< Number of bytes to allocate for local variables

	public const int SizeOf = 12;

    public static OBJHEAD FromByteArray(byte[] arr)
    {
        return new OBJHEAD()
        {
            // TODO:
        };
    }
}

/*! \struct LABEL
 *          Label data hold information used for labels
 */
public class LABEL
{
	public IMINDEX Addr;                         //!< Offset to breakpoint address from image start
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