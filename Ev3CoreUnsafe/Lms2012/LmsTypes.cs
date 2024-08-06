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

global using unsafe IP = byte*;
global using unsafe LP = byte*;
global using unsafe GP = byte*;

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

using System.Runtime.CompilerServices;
using Ev3CoreUnsafe.Helpers;


public unsafe struct IMGHEAD 
{
	public UBYTE* Sign;   //!< Place holder for the file type identifier                  
	public IMINDEX ImageSize;  //!< Image size                  
	public UWORD VersionInfo;  //!< Version identifier                 
	public OBJID NumberOfObjects;  //!< Total number of objects in image          
	public GBINDEX GlobalBytes;  //!< Number of bytes to allocate for global variables

	public IMGHEAD()
    {
		Sign = CommonHelper.Pointer1d<UBYTE>(4, false);
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
public unsafe struct OBJHEAD                  // Object header
{
	public IP OffsetToInstructions;         //!< Offset to instructions from image start      
	public OBJID OwnerObjectId; //!< Used by BLOCK's to hold the owner id              
	public TRIGGER TriggerCount; //!< Used to determine how many triggers needed before the BLOCK object is activated                
	public LBINDEX LocalBytes;  //!< Number of bytes to allocate for local variables 
}

/*! \struct LABEL
 *          Label data hold information used for labels
 */
public unsafe struct LABEL
{
	public IMINDEX Addr; //!< Offset to breakpoint address from image start
}

// bytecodes.c

public unsafe struct OPCODE
{
    public ULONG Pars;                         //!< Contains parameter info nibbles (max 8)
    public byte* Name;                        //!< Opcode name
}

public unsafe struct SUBCODE
{
    public ULONG Pars;                         //!< Contains parameter info nibbles (max 8)
    public byte* Name;                        //!< Sub code name
}