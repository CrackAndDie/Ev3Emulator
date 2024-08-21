/*
 * LEGO® MINDSTORMS EV3
 *
 * Copyright (C) 2010-2013 The LEGO Group
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

#include <stdio.h>
#include <stdlib.h>
 // #include <unistd.h>

#include "bytecodes.h"
#include "lmstypes.h"
#include "validate.h"

#define MAX_SUBCODES        33                //!< Max number of sub codes
#define OPCODE_NAMESIZE     20                //!< Opcode and sub code name length
#define MAX_LABELS          32                //!< Max number of labels per program

#define OC(OpCode,Par1,Par2,Par3,Par4,Par5,Par6,Par7,Par8)                     \
                                                                               \
    [OpCode] = {                                                               \
      .Pars   = ((ULONG)Par1) +                                                \
                ((ULONG)Par2 << 4) +                                           \
                ((ULONG)Par3 << 8) +                                           \
                ((ULONG)Par4 << 12) +                                          \
                ((ULONG)Par5 << 16) +                                          \
                ((ULONG)Par6 << 20) +                                          \
                ((ULONG)Par7 << 24) +                                          \
                ((ULONG)Par8 << 28),                                           \
      .Name   = #OpCode,                                                       \
    }

#define SC(ParameterFormat,SubCode,Par1,Par2,Par3,Par4,Par5,Par6,Par7,Par8)    \
                                                                               \
    [ParameterFormat][SubCode] = {                                             \
        .Pars   = ((ULONG)Par1) +                                              \
                  ((ULONG)Par2 << 4) +                                         \
                  ((ULONG)Par3 << 8) +                                         \
                  ((ULONG)Par4 << 12) +                                        \
                  ((ULONG)Par5 << 16) +                                        \
                  ((ULONG)Par6 << 20) +                                        \
                  ((ULONG)Par7 << 24) +                                        \
                  ((ULONG)Par8 << 28),                                         \
        .Name   = #SubCode,                                                    \
    }

#define SUBP                0x01              //!< Next nibble is sub parameter table no
#define PARNO               0x02              //!< Defines no of following parameters
#define PARLAB              0x03              //!< Defines label no
#define PARVALUES           0x04              //!< Last parameter defines number of values to follow

#define PAR                 0x08              //!< Plain  parameter as below:
#define PAR8                (PAR + DATA_8)    //!< DATA8  parameter
#define PAR16               (PAR + DATA_16)   //!< DATA16 parameter
#define PAR32               (PAR + DATA_32)   //!< DATA32 parameter
#define PARF                (PAR + DATA_F)    //!< DATAF  parameter
#define PARS                (PAR + DATA_S)    //!< DATAS  parameter
#define PARV                (PAR + DATA_V)    //!< Parameter type variable

typedef struct {
	const ULONG Pars; //!< Contains parameter info nibbles (max 8)
	const char* Name; //!< Opcode name
} OPCODE;

typedef struct {
	const ULONG Pars; //!< Contains parameter info nibbles (max 8)
	const char* Name; //!< Sub code name
} SUBCODE;

enum {
	UI_READ_SUBP = 0,
	UI_WRITE_SUBP = 1,
	UI_DRAW_SUBP = 2,
	UI_BUTTON_SUBP = 3,
	FILE_SUBP = 4,
	PROGRAM_INFO_SUBP = 5,
	INFO_SUBP = 6,
	STRINGS_SUBP = 7,
	COM_READ_SUBP = 8,
	COM_WRITE_SUBP = 9,
	SOUND_SUBP = 10,
	INPUT_DEVICE_SUBP = 11,
	ARRAY_SUBP = 12,
	MATH_SUBP = 13,
	COM_GET_SUBP = 14,
	COM_SET_SUBP = 15,
	SUBPS
};

#define FILENAME_SUBP   ARRAY_SUBP
#define TST_SUBP        INFO_SUBP

typedef struct {
	int       Row;
	IMINDEX   ValidateErrorIndex;
} VALIDATE_GLOBALS;

VALIDATE_GLOBALS ValidateInstance;

static const OPCODE const OpCodes[256] = {
	OC(opADD16, PAR16, PAR16, PAR16, 0, 0, 0, 0, 0),
	OC(opADD32, PAR32, PAR32, PAR32, 0, 0, 0, 0, 0),
	OC(opADD8, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opADDF, PARF, PARF, PARF, 0, 0, 0, 0, 0),
	OC(opAND16, PAR16, PAR16, PAR16, 0, 0, 0, 0, 0),
	OC(opAND32, PAR32, PAR32, PAR32, 0, 0, 0, 0, 0),
	OC(opAND8, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opARRAY, PAR8, SUBP, ARRAY_SUBP, 0, 0, 0, 0, 0),
	OC(opARRAY_APPEND, PAR16, PARV, 0, 0, 0, 0, 0, 0),
	OC(opARRAY_READ, PAR16, PAR32, PARV, 0, 0, 0, 0, 0),
	OC(opARRAY_WRITE, PAR16, PAR32, PARV, 0, 0, 0, 0, 0),
	OC(opBP0, 0, 0, 0, 0, 0, 0, 0, 0),
	OC(opBP1, 0, 0, 0, 0, 0, 0, 0, 0),
	OC(opBP2, 0, 0, 0, 0, 0, 0, 0, 0),
	OC(opBP3, 0, 0, 0, 0, 0, 0, 0, 0),
	OC(opBP_SET, PAR16, PAR8, PAR32, 0, 0, 0, 0, 0),
	OC(opCALL, PAR16, PARNO, 0, 0, 0, 0, 0, 0),
	OC(opCOM_GET, PAR8, SUBP, COM_GET_SUBP, 0, 0, 0, 0, 0),
	OC(opCOM_READ, PAR8, SUBP, COM_READ_SUBP, 0, 0, 0, 0, 0),
	OC(opCOM_READDATA, PAR8, PAR8, PAR16, PAR8, 0, 0, 0, 0),
	OC(opCOM_READY, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	OC(opCOM_REMOVE, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	OC(opCOM_SET, PAR8, SUBP, COM_SET_SUBP, 0, 0, 0, 0, 0),
	OC(opCOM_TEST, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opCOM_WRITE, PAR8, SUBP, COM_WRITE_SUBP, 0, 0, 0, 0, 0),
	OC(opCOM_WRITEDATA, PAR8, PAR8, PAR16, PAR8, 0, 0, 0, 0),
	OC(opCOM_WRITEFILE, PAR8, PAR8, PAR8, PAR8, 0, 0, 0, 0),
	OC(opCP_EQ16, PAR16, PAR16, PAR8, 0, 0, 0, 0, 0),
	OC(opCP_EQ32, PAR32, PAR32, PAR8, 0, 0, 0, 0, 0),
	OC(opCP_EQ8, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opCP_EQF, PARF, PARF, PAR8, 0, 0, 0, 0, 0),
	OC(opCP_GT16, PAR16, PAR16, PAR8, 0, 0, 0, 0, 0),
	OC(opCP_GT32, PAR32, PAR32, PAR8, 0, 0, 0, 0, 0),
	OC(opCP_GT8, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opCP_GTEQ16, PAR16, PAR16, PAR8, 0, 0, 0, 0, 0),
	OC(opCP_GTEQ32, PAR32, PAR32, PAR8, 0, 0, 0, 0, 0),
	OC(opCP_GTEQ8, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opCP_GTEQF, PARF, PARF, PAR8, 0, 0, 0, 0, 0),
	OC(opCP_GTF, PARF, PARF, PAR8, 0, 0, 0, 0, 0),
	OC(opCP_LT16, PAR16, PAR16, PAR8, 0, 0, 0, 0, 0),
	OC(opCP_LT32, PAR32, PAR32, PAR8, 0, 0, 0, 0, 0),
	OC(opCP_LT8, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opCP_LTEQ16, PAR16, PAR16, PAR8, 0, 0, 0, 0, 0),
	OC(opCP_LTEQ32, PAR32, PAR32, PAR8, 0, 0, 0, 0, 0),
	OC(opCP_LTEQ8, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opCP_LTEQF, PARF, PARF, PAR8, 0, 0, 0, 0, 0),
	OC(opCP_LTF, PARF, PARF, PAR8, 0, 0, 0, 0, 0),
	OC(opCP_NEQ16, PAR16, PAR16, PAR8, 0, 0, 0, 0, 0),
	OC(opCP_NEQ32, PAR32, PAR32, PAR8, 0, 0, 0, 0, 0),
	OC(opCP_NEQ8, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opCP_NEQF, PARF, PARF, PAR8, 0, 0, 0, 0, 0),
	OC(opDIV16, PAR16, PAR16, PAR16, 0, 0, 0, 0, 0),
	OC(opDIV32, PAR32, PAR32, PAR32, 0, 0, 0, 0, 0),
	OC(opDIV8, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opDIVF, PARF, PARF, PARF, 0, 0, 0, 0, 0),
	OC(opDO, PAR16, PAR32, PAR32, 0, 0, 0, 0, 0),
	OC(opDYNLOAD_ENTRY_0, PAR8, PAR8, PAR16, PARV, 0, 0, 0, 0),
	OC(opDYNLOAD_ENTRY_1, PAR8, PAR8, PAR16, PARV, 0, 0, 0, 0),
	OC(opDYNLOAD_ENTRY_2, PAR8, PAR8, PAR16, PARV, 0, 0, 0, 0),
	OC(opDYNLOAD_ENTRY_3, PAR8, PAR8, PAR16, PARV, 0, 0, 0, 0),
	OC(opDYNLOAD_ENTRY_4, PAR8, PAR8, PAR16, PARV, 0, 0, 0, 0),
	OC(opDYNLOAD_ENTRY_5, PAR8, PAR8, PAR16, PARV, 0, 0, 0, 0),
	OC(opDYNLOAD_ENTRY_6, PAR8, PAR8, PAR16, PARV, 0, 0, 0, 0),
	OC(opDYNLOAD_ENTRY_7, PAR8, PAR8, PAR16, PARV, 0, 0, 0, 0),
	OC(opDYNLOAD_ENTRY_8, PAR8, PAR8, PAR16, PARV, 0, 0, 0, 0),
	OC(opDYNLOAD_ENTRY_9, PAR8, PAR8, PAR16, PARV, 0, 0, 0, 0),
	OC(opDYNLOAD_GET_VM, PAR8, 0, 0, 0, 0, 0, 0, 0),
	OC(opDYNLOAD_VMEXIT, 0, 0, 0, 0, 0, 0, 0, 0),
	OC(opDYNLOAD_VMLOAD, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	OC(opERROR, 0, 0, 0, 0, 0, 0, 0, 0),
	OC(opFILE, PAR8, SUBP, FILE_SUBP, 0, 0, 0, 0, 0),
	OC(opFILENAME, PAR8, SUBP, FILENAME_SUBP, 0, 0, 0, 0, 0),
	OC(opFILE_MD5SUM, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opINFO, PAR8, SUBP, INFO_SUBP, 0, 0, 0, 0, 0),
	OC(opINIT_BYTES, PAR8, PAR32, PARVALUES, PAR8, 0, 0, 0, 0),
	OC(opINPUT_DEVICE, PAR8, SUBP, INPUT_DEVICE_SUBP, 0, 0, 0, 0, 0),
	OC(opINPUT_DEVICE_LIST, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opINPUT_IIC_READ, PAR8, PAR8, PAR8, PAR8, PAR8, 0, 0, 0),
	OC(opINPUT_IIC_STATUS, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opINPUT_IIC_WRITE, PAR8, PAR8, PAR8, PAR8, PAR8, PAR8, 0, 0),
	OC(opINPUT_READ, PAR8, PAR8, PAR8, PAR8, PAR8, 0, 0, 0),
	OC(opINPUT_READEXT, PAR8, PAR8, PAR8, PAR8, PAR8, PARNO, 0, 0),
	OC(opINPUT_READSI, PAR8, PAR8, PAR8, PAR8, PARF, 0, 0, 0),
	OC(opINPUT_READY, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	OC(opINPUT_SAMPLE, PAR32, PAR16, PAR16, PAR8, PAR8, PAR8, PAR8, PARF),
	OC(opINPUT_SET_AUTOID, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opINPUT_SET_CONN, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opINPUT_TEST, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opINPUT_WRITE, PAR8, PAR8, PAR8, PAR8, 0, 0, 0, 0),
	OC(opJR, PAR32, 0, 0, 0, 0, 0, 0, 0),
	OC(opJR_EQ16, PAR16, PAR16, PAR32, 0, 0, 0, 0, 0),
	OC(opJR_EQ32, PAR32, PAR32, PAR32, 0, 0, 0, 0, 0),
	OC(opJR_EQ8, PAR8, PAR8, PAR32, 0, 0, 0, 0, 0),
	OC(opJR_EQF, PARF, PARF, PAR32, 0, 0, 0, 0, 0),
	OC(opJR_FALSE, PAR8, PAR32, 0, 0, 0, 0, 0, 0),
	OC(opJR_GT16, PAR16, PAR16, PAR32, 0, 0, 0, 0, 0),
	OC(opJR_GT32, PAR32, PAR32, PAR32, 0, 0, 0, 0, 0),
	OC(opJR_GT8, PAR8, PAR8, PAR32, 0, 0, 0, 0, 0),
	OC(opJR_GTEQ16, PAR16, PAR16, PAR32, 0, 0, 0, 0, 0),
	OC(opJR_GTEQ32, PAR32, PAR32, PAR32, 0, 0, 0, 0, 0),
	OC(opJR_GTEQ8, PAR8, PAR8, PAR32, 0, 0, 0, 0, 0),
	OC(opJR_GTEQF, PARF, PARF, PAR32, 0, 0, 0, 0, 0),
	OC(opJR_GTF, PARF, PARF, PAR32, 0, 0, 0, 0, 0),
	OC(opJR_LT16, PAR16, PAR16, PAR32, 0, 0, 0, 0, 0),
	OC(opJR_LT32, PAR32, PAR32, PAR32, 0, 0, 0, 0, 0),
	OC(opJR_LT8, PAR8, PAR8, PAR32, 0, 0, 0, 0, 0),
	OC(opJR_LTEQ16, PAR16, PAR16, PAR32, 0, 0, 0, 0, 0),
	OC(opJR_LTEQ32, PAR32, PAR32, PAR32, 0, 0, 0, 0, 0),
	OC(opJR_LTEQ8, PAR8, PAR8, PAR32, 0, 0, 0, 0, 0),
	OC(opJR_LTEQF, PARF, PARF, PAR32, 0, 0, 0, 0, 0),
	OC(opJR_LTF, PARF, PARF, PAR32, 0, 0, 0, 0, 0),
	OC(opJR_NAN, PARF, PAR32, 0, 0, 0, 0, 0, 0),
	OC(opJR_NEQ16, PAR16, PAR16, PAR32, 0, 0, 0, 0, 0),
	OC(opJR_NEQ32, PAR32, PAR32, PAR32, 0, 0, 0, 0, 0),
	OC(opJR_NEQ8, PAR8, PAR8, PAR32, 0, 0, 0, 0, 0),
	OC(opJR_NEQF, PARF, PARF, PAR32, 0, 0, 0, 0, 0),
	OC(opJR_TRUE, PAR8, PAR32, 0, 0, 0, 0, 0, 0),
	OC(opKEEP_ALIVE, PAR8, 0, 0, 0, 0, 0, 0, 0),
	OC(opLABEL, PARLAB, 0, 0, 0, 0, 0, 0, 0),
	OC(opMAILBOX_CLOSE, PAR8, 0, 0, 0, 0, 0, 0, 0),
	OC(opMAILBOX_OPEN, PAR8, PAR8, PAR8, PAR8, PAR8, 0, 0, 0),
	OC(opMAILBOX_READ, PAR8, PAR8, PARNO, 0, 0, 0, 0, 0),
	OC(opMAILBOX_READY, PAR8, 0, 0, 0, 0, 0, 0, 0),
	OC(opMAILBOX_SIZE, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	OC(opMAILBOX_TEST, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	OC(opMAILBOX_WRITE, PAR8, PAR8, PAR8, PAR8, PARNO, 0, 0, 0),
	OC(opMATH, PAR8, SUBP, MATH_SUBP, 0, 0, 0, 0, 0),
	OC(opMEMORY_READ, PAR16, PAR16, PAR32, PAR32, PAR8, 0, 0, 0),
	OC(opMEMORY_USAGE, PAR32, PAR32, 0, 0, 0, 0, 0, 0),
	OC(opMEMORY_WRITE, PAR16, PAR16, PAR32, PAR32, PAR8, 0, 0, 0),
	OC(opMOVE16_16, PAR16, PAR16, 0, 0, 0, 0, 0, 0),
	OC(opMOVE16_32, PAR16, PAR32, 0, 0, 0, 0, 0, 0),
	OC(opMOVE16_8, PAR16, PAR8, 0, 0, 0, 0, 0, 0),
	OC(opMOVE16_F, PAR16, PARF, 0, 0, 0, 0, 0, 0),
	OC(opMOVE32_16, PAR32, PAR16, 0, 0, 0, 0, 0, 0),
	OC(opMOVE32_32, PAR32, PAR32, 0, 0, 0, 0, 0, 0),
	OC(opMOVE32_8, PAR32, PAR8, 0, 0, 0, 0, 0, 0),
	OC(opMOVE32_F, PAR32, PARF, 0, 0, 0, 0, 0, 0),
	OC(opMOVE8_16, PAR8, PAR16, 0, 0, 0, 0, 0, 0),
	OC(opMOVE8_32, PAR8, PAR32, 0, 0, 0, 0, 0, 0),
	OC(opMOVE8_8, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	OC(opMOVE8_F, PAR8, PARF, 0, 0, 0, 0, 0, 0),
	OC(opMOVEF_16, PARF, PAR16, 0, 0, 0, 0, 0, 0),
	OC(opMOVEF_32, PARF, PAR32, 0, 0, 0, 0, 0, 0),
	OC(opMOVEF_8, PARF, PAR8, 0, 0, 0, 0, 0, 0),
	OC(opMOVEF_F, PARF, PARF, 0, 0, 0, 0, 0, 0),
	OC(opMUL16, PAR16, PAR16, PAR16, 0, 0, 0, 0, 0),
	OC(opMUL32, PAR32, PAR32, PAR32, 0, 0, 0, 0, 0),
	OC(opMUL8, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opMULF, PARF, PARF, PARF, 0, 0, 0, 0, 0),
	OC(opNOP, 0, 0, 0, 0, 0, 0, 0, 0),
	OC(opNOTE_TO_FREQ, PAR8, PAR16, 0, 0, 0, 0, 0, 0),
	OC(opOBJECT_END, 0, 0, 0, 0, 0, 0, 0, 0),
	OC(opOBJECT_START, PAR16, 0, 0, 0, 0, 0, 0, 0),
	OC(opOBJECT_STOP, PAR16, 0, 0, 0, 0, 0, 0, 0),
	OC(opOBJECT_TRIG, PAR16, 0, 0, 0, 0, 0, 0, 0),
	OC(opOBJECT_WAIT, PAR16, 0, 0, 0, 0, 0, 0, 0),
	OC(opOR16, PAR16, PAR16, PAR16, 0, 0, 0, 0, 0),
	OC(opOR32, PAR32, PAR32, PAR32, 0, 0, 0, 0, 0),
	OC(opOR8, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opOUTPUT_CLR_COUNT, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	OC(opOUTPUT_GET_COUNT, PAR8, PAR8, PAR32, 0, 0, 0, 0, 0),
	OC(opOUTPUT_POLARITY, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opOUTPUT_POWER, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opOUTPUT_PRG_STOP, 0, 0, 0, 0, 0, 0, 0, 0),
	OC(opOUTPUT_READ, PAR8, PAR8, PAR8, PAR32, 0, 0, 0, 0),
	OC(opOUTPUT_READY, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	OC(opOUTPUT_RESET, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	OC(opOUTPUT_SET_TYPE, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opOUTPUT_SPEED, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opOUTPUT_START, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	OC(opOUTPUT_STEP_POWER, PAR8, PAR8, PAR8, PAR32, PAR32, PAR32, PAR8, 0),
	OC(opOUTPUT_STEP_SPEED, PAR8, PAR8, PAR8, PAR32, PAR32, PAR32, PAR8, 0),
	OC(opOUTPUT_STEP_SYNC, PAR8, PAR8, PAR8, PAR16, PAR32, PAR8, 0, 0),
	OC(opOUTPUT_STOP, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opOUTPUT_TEST, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opOUTPUT_TIME_POWER, PAR8, PAR8, PAR8, PAR32, PAR32, PAR32, PAR8, 0),
	OC(opOUTPUT_TIME_SPEED, PAR8, PAR8, PAR8, PAR32, PAR32, PAR32, PAR8, 0),
	OC(opOUTPUT_TIME_SYNC, PAR8, PAR8, PAR8, PAR16, PAR32, PAR8, 0, 0),
	OC(opPORT_CNV_INPUT, PAR32, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opPORT_CNV_OUTPUT, PAR32, PAR8, PAR8, PAR8, 0, 0, 0, 0),
	OC(opPROBE, PAR16, PAR16, PAR32, PAR32, 0, 0, 0, 0),
	OC(opPROGRAM_INFO, PAR8, SUBP, PROGRAM_INFO_SUBP, 0, 0, 0, 0, 0),
	OC(opPROGRAM_START, PAR16, PAR32, PAR32, PAR8, 0, 0, 0, 0),
	OC(opPROGRAM_STOP, PAR16, 0, 0, 0, 0, 0, 0, 0),
	OC(opRANDOM, PAR16, PAR16, PAR16, 0, 0, 0, 0, 0),
	OC(opREAD16, PAR16, PAR8, PAR16, 0, 0, 0, 0, 0),
	OC(opREAD32, PAR32, PAR8, PAR32, 0, 0, 0, 0, 0),
	OC(opREAD8, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opREADF, PARF, PAR8, PARF, 0, 0, 0, 0, 0),
	OC(opRETURN, 0, 0, 0, 0, 0, 0, 0, 0),
	OC(opRL16, PAR16, PAR16, PAR16, 0, 0, 0, 0, 0),
	OC(opRL32, PAR32, PAR32, PAR32, 0, 0, 0, 0, 0),
	OC(opRL8, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opSELECT16, PAR8, PAR16, PAR16, PAR16, 0, 0, 0, 0),
	OC(opSELECT32, PAR8, PAR32, PAR32, PAR32, 0, 0, 0, 0),
	OC(opSELECT8, PAR8, PAR8, PAR8, PAR8, 0, 0, 0, 0),
	OC(opSELECTF, PAR8, PARF, PARF, PARF, 0, 0, 0, 0),
	OC(opSLEEP, 0, 0, 0, 0, 0, 0, 0, 0),
	OC(opSOUND, PAR8, SUBP, SOUND_SUBP, 0, 0, 0, 0, 0),
	OC(opSOUND_READY, 0, 0, 0, 0, 0, 0, 0, 0),
	OC(opSOUND_TEST, PAR8, 0, 0, 0, 0, 0, 0, 0),
	OC(opSTRINGS, PAR8, SUBP, STRINGS_SUBP, 0, 0, 0, 0, 0),
	OC(opSUB16, PAR16, PAR16, PAR16, 0, 0, 0, 0, 0),
	OC(opSUB32, PAR32, PAR32, PAR32, 0, 0, 0, 0, 0),
	OC(opSUB8, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opSUBF, PARF, PARF, PARF, 0, 0, 0, 0, 0),
	OC(opSYSTEM, PAR8, PAR32, 0, 0, 0, 0, 0, 0),
	OC(opTIMER_READ, PAR32, 0, 0, 0, 0, 0, 0, 0),
	OC(opTIMER_READY, PAR32, 0, 0, 0, 0, 0, 0, 0),
	OC(opTIMER_READ_US, PAR32, 0, 0, 0, 0, 0, 0, 0),
	OC(opTIMER_WAIT, PAR32, PAR32, 0, 0, 0, 0, 0, 0),
	OC(opTST, PAR8, SUBP, TST_SUBP, 0, 0, 0, 0, 0),
	OC(opUI_BUTTON, PAR8, SUBP, UI_BUTTON_SUBP, 0, 0, 0, 0, 0),
	OC(opUI_DRAW, PAR8, SUBP, UI_DRAW_SUBP, 0, 0, 0, 0, 0),
	OC(opUI_FLUSH, 0, 0, 0, 0, 0, 0, 0, 0),
	OC(opUI_READ, PAR8, SUBP, UI_READ_SUBP, 0, 0, 0, 0, 0),
	OC(opUI_WRITE, PAR8, SUBP, UI_WRITE_SUBP, 0, 0, 0, 0, 0),
	OC(opWRITE16, PAR16, PAR8, PAR16, 0, 0, 0, 0, 0),
	OC(opWRITE32, PAR32, PAR8, PAR32, 0, 0, 0, 0, 0),
	OC(opWRITE8, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	OC(opWRITEF, PARF, PAR8, PARF, 0, 0, 0, 0, 0),
	OC(opXOR16, PAR16, PAR16, PAR16, 0, 0, 0, 0, 0),
	OC(opXOR32, PAR32, PAR32, PAR32, 0, 0, 0, 0, 0),
	OC(opXOR8, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
};

static const SUBCODE const SubCodes[SUBPS][MAX_SUBCODES] = {
	SC(ARRAY_SUBP, scCOPY, PAR16, PAR16, 0, 0, 0, 0, 0, 0),
	SC(ARRAY_SUBP, scCREATE16, PAR32, PAR16, 0, 0, 0, 0, 0, 0),
	SC(ARRAY_SUBP, scCREATE32, PAR32, PAR16, 0, 0, 0, 0, 0, 0),
	SC(ARRAY_SUBP, scCREATE8, PAR32, PAR16, 0, 0, 0, 0, 0, 0),
	SC(ARRAY_SUBP, scCREATEF, PAR32, PAR16, 0, 0, 0, 0, 0, 0),
	SC(ARRAY_SUBP, scDESTROY, PAR16, 0, 0, 0, 0, 0, 0, 0),
	SC(ARRAY_SUBP, scFILL, PAR16, PARV, 0, 0, 0, 0, 0, 0),
	SC(ARRAY_SUBP, scINIT16, PAR16, PAR32, PAR32, PARVALUES, PAR16, 0, 0, 0),
	SC(ARRAY_SUBP, scINIT32, PAR16, PAR32, PAR32, PARVALUES, PAR32, 0, 0, 0),
	SC(ARRAY_SUBP, scINIT8, PAR16, PAR32, PAR32, PARVALUES, PAR8, 0, 0, 0),
	SC(ARRAY_SUBP, scINITF, PAR16, PAR32, PAR32, PARVALUES, PARF, 0, 0, 0),
	SC(ARRAY_SUBP, scREAD_CONTENT, PAR16, PAR16, PAR32, PAR32, PAR8, 0, 0, 0),
	SC(ARRAY_SUBP, scREAD_SIZE, PAR16, PAR16, PAR32, 0, 0, 0, 0, 0),
	SC(ARRAY_SUBP, scRESIZE, PAR16, PAR32, 0, 0, 0, 0, 0, 0),
	SC(ARRAY_SUBP, scSET_SIZE, PAR16, PAR32, 0, 0, 0, 0, 0, 0),
	SC(ARRAY_SUBP, scWRITE_CONTENT, PAR16, PAR16, PAR32, PAR32, PAR8, 0, 0, 0),
	SC(COM_GET_SUBP, scCONNEC_ITEM, PAR8, PAR8, PAR8, PAR8, PAR8, 0, 0, 0),
	SC(COM_GET_SUBP, scCONNEC_ITEMS, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(COM_GET_SUBP, scFAVOUR_ITEM, PAR8, PAR8, PAR8, PAR8, PAR8, PAR8, PAR8, 0),
	SC(COM_GET_SUBP, scFAVOUR_ITEMS, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(COM_GET_SUBP, scGET_BRICKNAME, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(COM_GET_SUBP, scGET_ENCRYPT, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	SC(COM_GET_SUBP, scGET_ID, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	SC(COM_GET_SUBP, scGET_INCOMING, PAR8, PAR8, PAR8, PAR8, 0, 0, 0, 0),
	SC(COM_GET_SUBP, scGET_MODE2, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(COM_GET_SUBP, scGET_NETWORK, PAR8, PAR8, PAR8, PAR8, PAR8, 0, 0, 0),
	SC(COM_GET_SUBP, scGET_ON_OFF, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(COM_GET_SUBP, scGET_PIN, PAR8, PAR8, PAR8, PAR8, 0, 0, 0, 0),
	SC(COM_GET_SUBP, scGET_PRESENT, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(COM_GET_SUBP, scGET_RESULT, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	SC(COM_GET_SUBP, scGET_VISIBLE, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(COM_GET_SUBP, scLIST_STATE, PAR8, PAR16, 0, 0, 0, 0, 0, 0),
	SC(COM_GET_SUBP, scSEARCH_ITEM, PAR8, PAR8, PAR8, PAR8, PAR8, PAR8, PAR8, PAR8),
	SC(COM_GET_SUBP, scSEARCH_ITEMS, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(COM_READ_SUBP, scCOMMAND, PAR32, PAR32, PAR32, PAR8, 0, 0, 0, 0),
	SC(COM_SET_SUBP, scSET_BRICKNAME, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(COM_SET_SUBP, scSET_CONNECTION, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	SC(COM_SET_SUBP, scSET_ENCRYPT, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	SC(COM_SET_SUBP, scSET_MODE2, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(COM_SET_SUBP, scSET_MOVEDOWN, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(COM_SET_SUBP, scSET_MOVEUP, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(COM_SET_SUBP, scSET_ON_OFF, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(COM_SET_SUBP, scSET_PASSKEY, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(COM_SET_SUBP, scSET_PIN, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	SC(COM_SET_SUBP, scSET_SEARCH, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(COM_SET_SUBP, scSET_SSID, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(COM_SET_SUBP, scSET_VISIBLE, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(COM_WRITE_SUBP, scREPLY, PAR32, PAR32, PAR8, 0, 0, 0, 0, 0),
	SC(FILE_SUBP, scCLOSE, PAR16, 0, 0, 0, 0, 0, 0, 0),
	SC(FILE_SUBP, scCLOSE_LOG, PAR16, PAR8, 0, 0, 0, 0, 0, 0),
	SC(FILE_SUBP, scDEL_CACHE_FILE, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(FILE_SUBP, scDEL_SUBFOLDER, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(FILE_SUBP, scGET_CACHE_FILE, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	SC(FILE_SUBP, scGET_CACHE_FILES, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(FILE_SUBP, scGET_FOLDERS, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(FILE_SUBP, scGET_HANDLE, PAR8, PAR16, PAR8, 0, 0, 0, 0, 0),
	SC(FILE_SUBP, scGET_IMAGE, PAR8, PAR16, PAR8, PAR32, 0, 0, 0, 0),
	SC(FILE_SUBP, scGET_ITEM, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	SC(FILE_SUBP, scGET_LOG_NAME, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(FILE_SUBP, scGET_LOG_SYNC_TIME, PAR32, PAR32, 0, 0, 0, 0, 0, 0),
	SC(FILE_SUBP, scGET_POOL, PAR32, PAR16, PAR32, 0, 0, 0, 0, 0),
	SC(FILE_SUBP, scGET_SUBFOLDER_NAME, PAR8, PAR8, PAR8, PAR8, 0, 0, 0, 0),
	SC(FILE_SUBP, scLOAD_IMAGE, PAR16, PAR8, PAR32, PAR32, 0, 0, 0, 0),
	SC(FILE_SUBP, scMAKE_FOLDER, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(FILE_SUBP, scMOVE, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(FILE_SUBP, scOPEN_APPEND, PAR8, PAR16, 0, 0, 0, 0, 0, 0),
	SC(FILE_SUBP, scOPEN_LOG, PAR8, PAR32, PAR32, PAR32, PAR32, PAR32, PAR8, PAR16),
	SC(FILE_SUBP, scOPEN_READ, PAR8, PAR16, PAR32, 0, 0, 0, 0, 0),
	SC(FILE_SUBP, scOPEN_WRITE, PAR8, PAR16, 0, 0, 0, 0, 0, 0),
	SC(FILE_SUBP, scPUT_CACHE_FILE, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(FILE_SUBP, scREAD_BYTES, PAR16, PAR16, PAR8, 0, 0, 0, 0, 0),
	SC(FILE_SUBP, scREAD_TEXT, PAR16, PAR8, PAR16, PAR8, 0, 0, 0, 0),
	SC(FILE_SUBP, scREAD_VALUE, PAR16, PAR8, PARF, 0, 0, 0, 0, 0),
	SC(FILE_SUBP, scREMOVE, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(FILE_SUBP, scSET_LOG_SYNC_TIME, PAR32, PAR32, 0, 0, 0, 0, 0, 0),
	SC(FILE_SUBP, scWRITE_BYTES, PAR16, PAR16, PAR8, 0, 0, 0, 0, 0),
	SC(FILE_SUBP, scWRITE_LOG, PAR16, PAR32, PAR8, PARF, 0, 0, 0, 0),
	SC(FILE_SUBP, scWRITE_TEXT, PAR16, PAR8, PAR8, 0, 0, 0, 0, 0),
	SC(FILE_SUBP, scWRITE_VALUE, PAR16, PAR8, PARF, PAR8, PAR8, 0, 0, 0),
	SC(FILENAME_SUBP, scCHECK, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(FILENAME_SUBP, scEXIST, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(FILENAME_SUBP, scGET_FOLDERNAME, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(FILENAME_SUBP, scMERGE, PAR8, PAR8, PAR8, PAR8, PAR8, 0, 0, 0),
	SC(FILENAME_SUBP, scPACK, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(FILENAME_SUBP, scSPLIT, PAR8, PAR8, PAR8, PAR8, PAR8, 0, 0, 0),
	SC(FILENAME_SUBP, scTOTALSIZE, PAR8, PAR32, PAR32, 0, 0, 0, 0, 0),
	SC(FILENAME_SUBP, scUNPACK, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(INFO_SUBP, scERRORTEXT, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	SC(INFO_SUBP, scGET_ERROR, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(INFO_SUBP, scGET_MINUTES, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(INFO_SUBP, scGET_VOLUME, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(INFO_SUBP, scSET_ERROR, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(INFO_SUBP, scSET_MINUTES, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(INFO_SUBP, scSET_VOLUME, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(INPUT_DEVICE_SUBP, scCAL_DEFAULT, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(INPUT_DEVICE_SUBP, scCAL_MAX, PAR8, PAR8, PAR32, 0, 0, 0, 0, 0),
	SC(INPUT_DEVICE_SUBP, scCAL_MIN, PAR8, PAR8, PAR32, 0, 0, 0, 0, 0),
	SC(INPUT_DEVICE_SUBP, scCAL_MINMAX, PAR8, PAR8, PAR32, PAR32, 0, 0, 0, 0),
	SC(INPUT_DEVICE_SUBP, scCLR_ALL, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(INPUT_DEVICE_SUBP, scCLR_CHANGES, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(INPUT_DEVICE_SUBP, scGET_BUMPS, PAR8, PAR8, PARF, 0, 0, 0, 0, 0),
	SC(INPUT_DEVICE_SUBP, scGET_CHANGES, PAR8, PAR8, PARF, 0, 0, 0, 0, 0),
	SC(INPUT_DEVICE_SUBP, scGET_CONNECTION, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	SC(INPUT_DEVICE_SUBP, scGET_FIGURES, PAR8, PAR8, PAR8, PAR8, 0, 0, 0, 0),
	SC(INPUT_DEVICE_SUBP, scGET_FORMAT, PAR8, PAR8, PAR8, PAR8, PAR8, PAR8, 0, 0),
	SC(INPUT_DEVICE_SUBP, scGET_MINMAX, PAR8, PAR8, PARF, PARF, 0, 0, 0, 0),
	SC(INPUT_DEVICE_SUBP, scGET_MODENAME, PAR8, PAR8, PAR8, PAR8, PAR8, 0, 0, 0),
	SC(INPUT_DEVICE_SUBP, scGET_NAME, PAR8, PAR8, PAR8, PAR8, 0, 0, 0, 0),
	SC(INPUT_DEVICE_SUBP, scGET_RAW, PAR8, PAR8, PAR32, 0, 0, 0, 0, 0),
	SC(INPUT_DEVICE_SUBP, scGET_SYMBOL, PAR8, PAR8, PAR8, PAR8, 0, 0, 0, 0),
	SC(INPUT_DEVICE_SUBP, scGET_TYPEMODE, PAR8, PAR8, PAR8, PAR8, 0, 0, 0, 0),
	SC(INPUT_DEVICE_SUBP, scINSERT_TYPE, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	SC(INPUT_DEVICE_SUBP, scREADY_IIC, PAR8, PAR8, PAR8, PAR8, PAR8, PAR8, PAR8, 0),
	SC(INPUT_DEVICE_SUBP, scREADY_PCT, PAR8, PAR8, PAR8, PAR8, PARNO, 0, 0, 0),
	SC(INPUT_DEVICE_SUBP, scREADY_RAW, PAR8, PAR8, PAR8, PAR8, PARNO, 0, 0, 0),
	SC(INPUT_DEVICE_SUBP, scREADY_SI, PAR8, PAR8, PAR8, PAR8, PARNO, 0, 0, 0),
	SC(INPUT_DEVICE_SUBP, scSETUP, PAR8, PAR8, PAR8, PAR16, PAR8, PAR8, PAR8, PAR8),
	SC(INPUT_DEVICE_SUBP, scSET_RAW, PAR8, PAR8, PAR8, PAR32, 0, 0, 0, 0),
	SC(INPUT_DEVICE_SUBP, scSET_TYPEMODE, PAR8, PAR8, PAR8, PAR8, PAR8, 0, 0, 0),
	SC(INPUT_DEVICE_SUBP, scSTOP_ALL, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(MATH_SUBP, scABS, PARF, PARF, 0, 0, 0, 0, 0, 0),
	SC(MATH_SUBP, scACOS, PARF, PARF, 0, 0, 0, 0, 0, 0),
	SC(MATH_SUBP, scASIN, PARF, PARF, 0, 0, 0, 0, 0, 0),
	SC(MATH_SUBP, scATAN, PARF, PARF, 0, 0, 0, 0, 0, 0),
	SC(MATH_SUBP, scCEIL, PARF, PARF, 0, 0, 0, 0, 0, 0),
	SC(MATH_SUBP, scCOS, PARF, PARF, 0, 0, 0, 0, 0, 0),
	SC(MATH_SUBP, scEXP, PARF, PARF, 0, 0, 0, 0, 0, 0),
	SC(MATH_SUBP, scFLOOR, PARF, PARF, 0, 0, 0, 0, 0, 0),
	SC(MATH_SUBP, scLN, PARF, PARF, 0, 0, 0, 0, 0, 0),
	SC(MATH_SUBP, scLOG, PARF, PARF, 0, 0, 0, 0, 0, 0),
	SC(MATH_SUBP, scMOD, PARF, PARF, PARF, 0, 0, 0, 0, 0),
	SC(MATH_SUBP, scMOD16, PAR16, PAR16, PAR16, 0, 0, 0, 0, 0),
	SC(MATH_SUBP, scMOD32, PAR32, PAR32, PAR32, 0, 0, 0, 0, 0),
	SC(MATH_SUBP, scMOD8, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	SC(MATH_SUBP, scNEGATE, PARF, PARF, 0, 0, 0, 0, 0, 0),
	SC(MATH_SUBP, scPOW, PARF, PARF, PARF, 0, 0, 0, 0, 0),
	SC(MATH_SUBP, scROUND, PARF, PARF, 0, 0, 0, 0, 0, 0),
	SC(MATH_SUBP, scSIN, PARF, PARF, 0, 0, 0, 0, 0, 0),
	SC(MATH_SUBP, scSQRT, PARF, PARF, 0, 0, 0, 0, 0, 0),
	SC(MATH_SUBP, scTAN, PARF, PARF, 0, 0, 0, 0, 0, 0),
	SC(MATH_SUBP, scTRUNC, PARF, PAR8, PARF, 0, 0, 0, 0, 0),
	SC(PROGRAM_INFO_SUBP, scGET_PRGNAME, PAR16, PAR8, 0, 0, 0, 0, 0, 0),
	SC(PROGRAM_INFO_SUBP, scGET_PRGRESULT, PAR16, PAR8, 0, 0, 0, 0, 0, 0),
	SC(PROGRAM_INFO_SUBP, scGET_SPEED, PAR16, PAR32, 0, 0, 0, 0, 0, 0),
	SC(PROGRAM_INFO_SUBP, scGET_STATUS, PAR16, PAR8, 0, 0, 0, 0, 0, 0),
	SC(PROGRAM_INFO_SUBP, scOBJ_START, PAR16, PAR16, 0, 0, 0, 0, 0, 0),
	SC(PROGRAM_INFO_SUBP, scOBJ_STOP, PAR16, PAR16, 0, 0, 0, 0, 0, 0),
	SC(PROGRAM_INFO_SUBP, scSET_INSTR, PAR16, 0, 0, 0, 0, 0, 0, 0),
	SC(SOUND_SUBP, scBREAK, 0, 0, 0, 0, 0, 0, 0, 0),
	SC(SOUND_SUBP, scPLAY, PAR8, PARS, 0, 0, 0, 0, 0, 0),
	SC(SOUND_SUBP, scREPEAT, PAR8, PARS, 0, 0, 0, 0, 0, 0),
	SC(SOUND_SUBP, scSERVICE, 0, 0, 0, 0, 0, 0, 0, 0),
	SC(SOUND_SUBP, scTONE, PAR8, PAR16, PAR16, 0, 0, 0, 0, 0),
	SC(STRINGS_SUBP, scADD, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	SC(STRINGS_SUBP, scCOMPARE, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	SC(STRINGS_SUBP, scDUPLICATE, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(STRINGS_SUBP, scGET_SIZE, PAR8, PAR16, 0, 0, 0, 0, 0, 0),
	SC(STRINGS_SUBP, scNUMBER_FORMATTED, PAR32, PAR8, PAR8, PAR8, 0, 0, 0, 0),
	SC(STRINGS_SUBP, scNUMBER_TO_STRING, PAR16, PAR8, PAR8, 0, 0, 0, 0, 0),
	SC(STRINGS_SUBP, scSTRING_TO_VALUE, PAR8, PARF, 0, 0, 0, 0, 0, 0),
	SC(STRINGS_SUBP, scSTRIP, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(STRINGS_SUBP, scSUB, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	SC(STRINGS_SUBP, scVALUE_FORMATTED, PARF, PAR8, PAR8, PAR8, 0, 0, 0, 0),
	SC(STRINGS_SUBP, scVALUE_TO_STRING, PARF, PAR8, PAR8, PAR8, 0, 0, 0, 0),
	SC(TST_SUBP, scTST_ACCU_SWITCH, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(TST_SUBP, scTST_BOOT_MODE2, 0, 0, 0, 0, 0, 0, 0, 0),
	SC(TST_SUBP, scTST_CLOSE, 0, 0, 0, 0, 0, 0, 0, 0),
	SC(TST_SUBP, scTST_CLOSE_MODE2, 0, 0, 0, 0, 0, 0, 0, 0),
	SC(TST_SUBP, scTST_DISABLE_UART, 0, 0, 0, 0, 0, 0, 0, 0),
	SC(TST_SUBP, scTST_ENABLE_UART, PAR32, 0, 0, 0, 0, 0, 0, 0),
	SC(TST_SUBP, scTST_OPEN, 0, 0, 0, 0, 0, 0, 0, 0),
	SC(TST_SUBP, scTST_POLL_MODE2, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(TST_SUBP, scTST_RAM_CHECK, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(TST_SUBP, scTST_READ_ADC, PAR8, PAR16, 0, 0, 0, 0, 0, 0),
	SC(TST_SUBP, scTST_READ_PINS, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	SC(TST_SUBP, scTST_READ_UART, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	SC(TST_SUBP, scTST_WRITE_PINS, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	SC(TST_SUBP, scTST_WRITE_UART, PAR8, PAR8, PAR8, 0, 0, 0, 0, 0),
	SC(UI_BUTTON_SUBP, scFLUSH, 0, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_BUTTON_SUBP, scGET_BACK_BLOCK, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_BUTTON_SUBP, scGET_BUMPED, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(UI_BUTTON_SUBP, scGET_CLICK, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_BUTTON_SUBP, scGET_HORZ, PAR16, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_BUTTON_SUBP, scGET_VERT, PAR16, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_BUTTON_SUBP, scLONGPRESS, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(UI_BUTTON_SUBP, scPRESS, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_BUTTON_SUBP, scPRESSED, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(UI_BUTTON_SUBP, scRELEASE, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_BUTTON_SUBP, scSET_BACK_BLOCK, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_BUTTON_SUBP, scSHORTPRESS, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(UI_BUTTON_SUBP, scTESTLONGPRESS, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(UI_BUTTON_SUBP, scTESTSHORTPRESS, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(UI_BUTTON_SUBP, scWAIT_FOR_PRESS, 0, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_DRAW_SUBP, scBMPFILE, PAR8, PAR16, PAR16, PAR8, 0, 0, 0, 0),
	SC(UI_DRAW_SUBP, scBROWSE, PAR8, PAR16, PAR16, PAR16, PAR16, PAR8, PAR8, PAR8),
	SC(UI_DRAW_SUBP, scCIRCLE, PAR8, PAR16, PAR16, PAR16, 0, 0, 0, 0),
	SC(UI_DRAW_SUBP, scCLEAN, 0, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_DRAW_SUBP, scDOTLINE, PAR8, PAR16, PAR16, PAR16, PAR16, PAR16, PAR16, 0),
	SC(UI_DRAW_SUBP, scFILLCIRCLE, PAR8, PAR16, PAR16, PAR16, 0, 0, 0, 0),
	SC(UI_DRAW_SUBP, scFILLRECT, PAR8, PAR16, PAR16, PAR16, PAR16, 0, 0, 0),
	SC(UI_DRAW_SUBP, scFILLWINDOW, PAR8, PAR16, PAR16, 0, 0, 0, 0, 0),
	SC(UI_DRAW_SUBP, scGRAPH_DRAW, PAR8, PARF, PARF, PARF, PARF, 0, 0, 0),
	SC(UI_DRAW_SUBP, scGRAPH_SETUP, PAR16, PAR16, PAR16, PAR16, PAR8, PAR16, PAR16, PAR16),
	SC(UI_DRAW_SUBP, scICON, PAR8, PAR16, PAR16, PAR8, PAR8, 0, 0, 0),
	SC(UI_DRAW_SUBP, scICON_QUESTION, PAR8, PAR16, PAR16, PAR8, PAR32, 0, 0, 0),
	SC(UI_DRAW_SUBP, scINVERSERECT, PAR16, PAR16, PAR16, PAR16, 0, 0, 0, 0),
	SC(UI_DRAW_SUBP, scKEYBOARD, PAR8, PAR16, PAR16, PAR8, PAR8, PAR8, PAR8, PAR8),
	SC(UI_DRAW_SUBP, scLINE, PAR8, PAR16, PAR16, PAR16, PAR16, 0, 0, 0),
	SC(UI_DRAW_SUBP, scNOTIFICATION, PAR8, PAR16, PAR16, PAR8, PAR8, PAR8, PAR8, PAR8),
	SC(UI_DRAW_SUBP, scPICTURE, PAR8, PAR16, PAR16, PAR32, 0, 0, 0, 0),
	SC(UI_DRAW_SUBP, scPIXEL, PAR8, PAR16, PAR16, 0, 0, 0, 0, 0),
	SC(UI_DRAW_SUBP, scPOPUP, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_DRAW_SUBP, scQUESTION, PAR8, PAR16, PAR16, PAR8, PAR8, PAR8, PAR8, PAR8),
	SC(UI_DRAW_SUBP, scRECTANGLE, PAR8, PAR16, PAR16, PAR16, PAR16, 0, 0, 0),
	SC(UI_DRAW_SUBP, scRESTORE, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_DRAW_SUBP, scSCROLL, PAR16, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_DRAW_SUBP, scSELECT_FONT, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_DRAW_SUBP, scSTORE, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_DRAW_SUBP, scTEXT, PAR8, PAR16, PAR16, PAR8, 0, 0, 0, 0),
	SC(UI_DRAW_SUBP, scTEXTBOX, PAR16, PAR16, PAR16, PAR16, PAR8, PAR32, PAR8, PAR8),
	SC(UI_DRAW_SUBP, scTOPLINE, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_DRAW_SUBP, scUPDATE, 0, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_DRAW_SUBP, scVALUE, PAR8, PAR16, PAR16, PARF, PAR8, PAR8, 0, 0),
	SC(UI_DRAW_SUBP, scVERTBAR, PAR8, PAR16, PAR16, PAR16, PAR16, PAR16, PAR16, PAR16),
	SC(UI_DRAW_SUBP, scVIEW_UNIT, PAR8, PAR16, PAR16, PARF, PAR8, PAR8, PAR8, PAR8),
	SC(UI_DRAW_SUBP, scVIEW_VALUE, PAR8, PAR16, PAR16, PARF, PAR8, PAR8, 0, 0),
	SC(UI_READ_SUBP, scGET_ADDRESS, PAR32, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_READ_SUBP, scGET_CODE, PAR32, PAR32, PAR32, PAR8, 0, 0, 0, 0),
	SC(UI_READ_SUBP, scGET_EVENT, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_READ_SUBP, scGET_FW_BUILD, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(UI_READ_SUBP, scGET_FW_VERS, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(UI_READ_SUBP, scGET_HW_VERS, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(UI_READ_SUBP, scGET_IBATT, PARF, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_READ_SUBP, scGET_IINT, PARF, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_READ_SUBP, scGET_IMOTOR, PARF, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_READ_SUBP, scGET_IP, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(UI_READ_SUBP, scGET_LBATT, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_READ_SUBP, scGET_OS_BUILD, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(UI_READ_SUBP, scGET_OS_VERS, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(UI_READ_SUBP, scGET_POWER, PARF, PARF, PARF, PARF, 0, 0, 0, 0),
	SC(UI_READ_SUBP, scGET_SDCARD, PAR8, PAR32, PAR32, 0, 0, 0, 0, 0),
	SC(UI_READ_SUBP, scGET_SHUTDOWN, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_READ_SUBP, scGET_STRING, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(UI_READ_SUBP, scGET_TBATT, PARF, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_READ_SUBP, scGET_USBSTICK, PAR8, PAR32, PAR32, 0, 0, 0, 0, 0),
	SC(UI_READ_SUBP, scGET_VBATT, PARF, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_READ_SUBP, scGET_VERSION, PAR8, PAR8, 0, 0, 0, 0, 0, 0),
	SC(UI_READ_SUBP, scGET_WARNING, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_READ_SUBP, scKEY, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_READ_SUBP, scTEXTBOX_READ, PAR8, PAR32, PAR8, PAR8, PAR16, PAR8, 0, 0),
	SC(UI_WRITE_SUBP, scADDRESS, PAR32, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_WRITE_SUBP, scALLOW_PULSE, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_WRITE_SUBP, scCODE, PAR8, PAR32, 0, 0, 0, 0, 0, 0),
	SC(UI_WRITE_SUBP, scDOWNLOAD_END, 0, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_WRITE_SUBP, scFLOATVALUE, PARF, PAR8, PAR8, 0, 0, 0, 0, 0),
	SC(UI_WRITE_SUBP, scGRAPH_SAMPLE, 0, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_WRITE_SUBP, scINIT_RUN, 0, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_WRITE_SUBP, scLED, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_WRITE_SUBP, scPOWER, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_WRITE_SUBP, scPUT_STRING, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_WRITE_SUBP, scSCREEN_BLOCK, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_WRITE_SUBP, scSET_BUSY, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_WRITE_SUBP, scSET_PULSE, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_WRITE_SUBP, scSET_TESTPIN, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_WRITE_SUBP, scSTAMP, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_WRITE_SUBP, scTERMINAL, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_WRITE_SUBP, scTEXTBOX_APPEND, PAR8, PAR32, PAR8, PAR8, 0, 0, 0, 0),
	SC(UI_WRITE_SUBP, scUPDATE_RUN, 0, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_WRITE_SUBP, scVALUE16, PAR16, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_WRITE_SUBP, scVALUE32, PAR32, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_WRITE_SUBP, scVALUE8, PAR8, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_WRITE_SUBP, scVALUEF, PARF, 0, 0, 0, 0, 0, 0, 0),
	SC(UI_WRITE_SUBP, scWRITE_FLUSH, 0, 0, 0, 0, 0, 0, 0, 0),
};

static const DATA32 const ParMin[] = {
	[DATA_8] = DATA8_MIN,
	[DATA_16] = DATA16_MIN,
	[DATA_32] = DATA32_MIN,
};

static const DATA32 const ParMax[] = {
	[DATA_8] = DATA8_MAX,
	[DATA_16] = DATA16_MAX,
	[DATA_32] = DATA32_MAX,
};

#ifdef DEBUG
static const char* const ParTypeNames[] = {
	[DATA_8] = "DATA8",
	[DATA_16] = "DATA16",
	[DATA_32] = "DATA32",
	[DATA_F] = "DATAF",
	[DATA_S] = "STRING",
	[DATA_V] = "UNKNOWN",
};

static void ShowOpcode(UBYTE OpCode, char* Buf, int Lng)
{
	ULONG   Pars;
	DATA8   Sub;
	UBYTE   Tab;
	UBYTE   ParType;
	UBYTE   Flag = 0;
	char    TmpBuf[255];
	int     Length;
	int     Size;


	Buf[0] = 0;
	if (OpCodes[OpCode].Name)
	{ // Byte code exist

		Size = 0;
		Length = snprintf(&Buf[Size], Lng - Size, "  %s,", OpCodes[OpCode].Name);
		Size += Length;
		Size += snprintf(&Buf[Size], Lng - Size, "%*.*s", 3 + OPCODE_NAMESIZE - Length, 3 + OPCODE_NAMESIZE - Length, "");

		// Get opcode parameter types
		Pars = OpCodes[OpCode].Pars;
		do
		{ // check for every parameter

			ParType = Pars & 0x0F;
			if (ParType == SUBP)
			{ // Check existence of sub command

				Pars >>= 4;
				Tab = Pars & 0x0F;

				for (Sub = 0; Sub < MAX_SUBCODES; Sub++)
				{
					Pars = 0;
					if (SubCodes[Tab][Sub].Name != NULL)
					{
						if (Flag == 0)
						{
							snprintf(TmpBuf, 255, "%s", Buf);
						}
						else
						{
							Size += snprintf(&Buf[Size], Lng - Size, "\n");
							Size += snprintf(&Buf[Size], Lng - Size, "%s", TmpBuf);
						}
						Flag = 1;
						Size += snprintf(&Buf[Size], Lng - Size, "%s, ", SubCodes[Tab][Sub].Name);
						Pars = SubCodes[Tab][Sub].Pars;
					}
					while ((Pars & 0x0F) >= PAR)
					{ // Check parameter

						if (((Pars >> 4) & 0x0F) != SUBP)
						{
							Size += snprintf(&Buf[Size], Lng - Size, "%s, ", ParTypeNames[(Pars & 0x0F) - PAR]);
						}

						// Next parameter
						Pars >>= 4;
					}
				}
			}
			if (ParType == PARNO)
			{ // Check parameter

				if (((Pars >> 4) & 0x0F) != SUBP)
				{
					Size += snprintf(&Buf[Size], Lng - Size, "%s, ", ParTypeNames[DATA_8]);
				}

				// Next parameter
				Pars >>= 4;
			}
			if (ParType == PARLAB)
			{ // Check parameter

				if (((Pars >> 4) & 0x0F) != SUBP)
				{
					Size += snprintf(&Buf[Size], Lng - Size, "%s, ", ParTypeNames[DATA_8]);
				}

				// Next parameter
				Pars >>= 4;
			}
			if (ParType == PARVALUES)
			{ // Check parameter

			  // Next parameter
				Pars >>= 4;
			}
			if (ParType >= PAR)
			{ // Check parameter

				if (((Pars >> 4) & 0x0F) != SUBP)
				{
					Size += snprintf(&Buf[Size], Lng - Size, "%s, ", ParTypeNames[(Pars & 0x0F) - PAR]);
				}

				// Next parameter
				Pars >>= 4;
			}
		} while (ParType);
		Size += snprintf(&Buf[Size], Lng - Size, "\n");
	}
}
#endif

RESULT cValidateInit(void)
{
	RESULT  Result = FAIL;
#ifdef DEBUG
	FILE* pFile;
	UWORD   OpCode;
	char    Buffer[8000];

	pFile = fopen("../../../bytecodeassembler/o.c", "w");
	fprintf(pFile, "//******************************************************************************\n");
	fprintf(pFile, "//Test Supported Opcodes in V%4.2f\n", VERS);
	fprintf(pFile, "//******************************************************************************\n\n");
	fprintf(pFile, "#define DATA8          LC0(0)                   \n");
	fprintf(pFile, "#define DATA16         GV0(0)                   \n");
	fprintf(pFile, "#define DATA32         LV0(0)                   \n");
	fprintf(pFile, "#define DATAF          LC0(0)                   \n");
	fprintf(pFile, "#define UNKNOWN        LV0(0)                   \n");
	fprintf(pFile, "#define STRING         LCS,'T','E','S','T',0    \n");
	fprintf(pFile, "\n");
	fprintf(pFile, "UBYTE     prg[] =\n");
	fprintf(pFile, "{\n");
	fprintf(pFile, "  PROGRAMHeader(0,2,4),\n");
	fprintf(pFile, "  VMTHREADHeader(0,4),\n");
	fprintf(pFile, "  VMTHREADHeader(0,4),\n");
	fprintf(pFile, "\n");
	for (OpCode = 0; OpCode < 256; OpCode++)
	{
		if ((OpCode != opERROR) && (OpCode != opNOP))
		{
			ShowOpcode((UBYTE)OpCode, Buffer, 8000);
			fprintf(pFile, "%s", Buffer);
			if (OpCode == 0x60)
			{
				//        fprintf(pFile,"  0x60,\n");
			}
		}
	}
	ShowOpcode(opOBJECT_END, Buffer, 8000);
	fprintf(pFile, "%s", Buffer);
	fprintf(pFile, "\n");
	fprintf(pFile, "};\n\n");
	fprintf(pFile, "//******************************************************************************\n");
	fclose(pFile);

	if (system("~/projects/lms2012/bytecodeassembler/oasm") >= 0)
	{
		printf("Compiling\n");
		sync();
	}


#endif
	Result = OK;

	return (Result);
}

RESULT cValidateExit(void)
{
	RESULT  Result = FAIL;

	Result = OK;

	return (Result);
}

static void cValidateSetErrorIndex(IMINDEX Index)
{
	if (Index == 0)
	{
		ValidateInstance.ValidateErrorIndex = 0;
	}
	if (ValidateInstance.ValidateErrorIndex == 0)
	{
		ValidateInstance.ValidateErrorIndex = Index;
	}
}

static IMINDEX cValidateGetErrorIndex(void)
{
	return (ValidateInstance.ValidateErrorIndex);
}

RESULT cValidateDisassemble(IP pI, IMINDEX* pIndex, LABEL* pLabel)
{
	RESULT  Result = FAIL;  // Current status
	IMINDEX OpCode;         // Current opcode
	ULONG   Pars;           // Current parameter types
	UBYTE   ParType;        // Current parameter type
	DATA8   Sub;            // Current sub code if any
	UBYTE   Tab;            // Sub code table index
	ULONG   Value;
	UBYTE   ParCode = 0;
	void* pParValue;
	DATA8   Parameters;
	DATA32  Bytes;
	int     Indent;
	int     LineLength;

	// Check for validation error
	if ((*pIndex) == cValidateGetErrorIndex())
	{
		printf("ERROR ");
	}

	// Get opcode
	OpCode = pI[*pIndex] & 0xFF;

	// Check if opcode exists
	if (OpCodes[OpCode].Name)
	{ // Byte code exists

		Parameters = 0;

		// Print opcode
		LineLength = printf("  /* %4d */  %s,", *pIndex, OpCodes[OpCode].Name);
		Indent = LineLength;

		(*pIndex)++;

		// Check if object ends or error
		if ((OpCode == opOBJECT_END) || (OpCode == opERROR))
		{
			Result = STOP;
		}
		else
		{
			Result = OK;

			// Get opcode parameter types
			Pars = OpCodes[OpCode].Pars;
			do
			{ // check for every parameter

			  // Isolate current parameter type
				ParType = Pars & 0x0F;

				if (ParType == SUBP)
				{ // Prior plain parameter was a sub code

				  // Get sub code from last plain parameter
					Sub = *(DATA8*)pParValue;

					// Isolate next parameter type
					Pars >>= 4;
					Tab = Pars & 0x0F;
					Pars = 0;

					// Check if sub code in right range
					if (Sub < MAX_SUBCODES)
					{ // Sub code range ok

					  // Check if sub code exists
						if (SubCodes[Tab][Sub].Name != NULL)
						{ // Ok

							if (ParCode & PRIMPAR_LONG)
							{ // long format

								if ((ParCode & PRIMPAR_BYTES) == PRIMPAR_1_BYTE)
								{
									LineLength += printf("LC1(%s),", SubCodes[Tab][Sub].Name);
								}
								if ((ParCode & PRIMPAR_BYTES) == PRIMPAR_2_BYTES)
								{
									LineLength += printf("LC2(%s),", SubCodes[Tab][Sub].Name);
								}
								if ((ParCode & PRIMPAR_BYTES) == PRIMPAR_4_BYTES)
								{
									LineLength += printf("LC4(%s),", SubCodes[Tab][Sub].Name);
								}
							}
							else
							{
								LineLength += printf("LC0(%s),", SubCodes[Tab][Sub].Name);
							}

							// Get sub code parameter list
							Pars = SubCodes[Tab][Sub].Pars;
						}
					}
				}

				if (ParType == PARVALUES)
				{
					Bytes = *(DATA32*)pParValue;
					// Next parameter
					Pars >>= 4;
					Pars &= 0x0F;

					while (Bytes)
					{
						//***************************************************************************
						if ((LineLength + 10) >= 80)
						{
							printf("\n%*.*s", Indent, Indent, "");
							LineLength = Indent;
						}
						Value = (ULONG)0;
						pParValue = (void*)&Value;
						ParCode = (UBYTE)pI[(*pIndex)++] & 0xFF;

						// Calculate parameter value

						if (ParCode & PRIMPAR_LONG)
						{ // long format

							if (ParCode & PRIMPAR_HANDLE)
							{
								LineLength += printf("HND(");
							}
							else
							{
								if (ParCode & PRIMPAR_ADDR)
								{
									LineLength += printf("ADR(");
								}
							}
							if (ParCode & PRIMPAR_VARIABLE)
							{ // variable

								if (ParCode & PRIMPAR_GLOBAL)
								{ // global

									LineLength += printf("GV");
								}
								else
								{ // local

									LineLength += printf("LV");
								}
								switch (ParCode & PRIMPAR_BYTES)
								{
								case PRIMPAR_1_BYTE:
								{
									Value = (ULONG)pI[(*pIndex)++];

									LineLength += printf("1(%d)", (int)Value);
								}
								break;

								case PRIMPAR_2_BYTES:
								{
									Value = (ULONG)pI[(*pIndex)++];
									Value |= ((ULONG)pI[(*pIndex)++] << 8);

									LineLength += printf("2(%d)", (int)Value);
								}
								break;

								case PRIMPAR_4_BYTES:
								{
									Value = (ULONG)pI[(*pIndex)++];
									Value |= ((ULONG)pI[(*pIndex)++] << 8);
									Value |= ((ULONG)pI[(*pIndex)++] << 16);
									Value |= ((ULONG)pI[(*pIndex)++] << 24);

									LineLength += printf("4(%d)", (int)Value);
								}
								break;

								}
							}
							else
							{ // constant

								if (ParCode & PRIMPAR_HANDLE)
								{
									LineLength += printf("HND(");
								}
								else
								{
									if (ParCode & PRIMPAR_ADDR)
									{
										LineLength += printf("ADR(");
									}
								}
								if (ParCode & PRIMPAR_LABEL)
								{ // label

									Value = (ULONG)pI[(*pIndex)++];
									if (Value & 0x00000080)
									{ // Adjust if negative

										Value |= 0xFFFFFF00;
									}

									LineLength += printf("LAB1(%d)", (int)(Value & 0xFF));
								}
								else
								{ // value

									switch (ParCode & PRIMPAR_BYTES)
									{
									case PRIMPAR_STRING_OLD:
									case PRIMPAR_STRING:
									{

										LineLength += printf("LCS,");

										while (pI[(*pIndex)])
										{ // Adjust Ip

											if ((LineLength + 5) >= 80)
											{
												printf("\n%*.*s", Indent, Indent, "");
												LineLength = Indent;
											}

											switch (pI[(*pIndex)])
											{
											case '\r':
											{
												LineLength += printf("'\\r',");
											}
											break;

											case '\n':
											{
												LineLength += printf("'\\n',");
											}
											break;

											default:
											{
												LineLength += printf("'%c',", pI[(*pIndex)]);
											}
											break;

											}

											(*pIndex)++;
										}
										(*pIndex)++;

										if ((LineLength + 2) >= 80)
										{
											printf("\n%*.*s", Indent, Indent, "");
											LineLength = Indent;
										}
										LineLength += printf("0");
									}
									break;

									case PRIMPAR_1_BYTE:
									{
										Value = (ULONG)pI[(*pIndex)++];
										if (Value & 0x00000080)
										{ // Adjust if negative

											Value |= 0xFFFFFF00;
										}
										if ((Pars & 0x0f) != SUBP)
										{
											LineLength += printf("LC1(%d)", (int)Value);
										}
									}
									break;

									case PRIMPAR_2_BYTES:
									{
										Value = (ULONG)pI[(*pIndex)++];
										Value |= ((ULONG)pI[(*pIndex)++] << 8);
										if (Value & 0x00008000)
										{ // Adjust if negative

											Value |= 0xFFFF0000;
										}
										if ((Pars & 0x0f) != SUBP)
										{
											LineLength += printf("LC2(%d)", (int)Value);
										}
									}
									break;

									case PRIMPAR_4_BYTES:
									{
										Value = (ULONG)pI[(*pIndex)++];
										Value |= ((ULONG)pI[(*pIndex)++] << 8);
										Value |= ((ULONG)pI[(*pIndex)++] << 16);
										Value |= ((ULONG)pI[(*pIndex)++] << 24);
										if ((Pars & 0x0f) != SUBP)
										{
											LineLength += printf("LC4(%ld)", (long)Value);
										}
									}
									break;

									}
								}
							}
							if (ParCode & PRIMPAR_HANDLE)
							{
								LineLength += printf("),");
							}
							else
							{
								if (ParCode & PRIMPAR_ADDR)
								{
									LineLength += printf("),");
								}
								else
								{
									LineLength += printf(",");
								}
							}
						}
						else
						{ // short format

							if (ParCode & PRIMPAR_VARIABLE)
							{ // variable

								if (ParCode & PRIMPAR_GLOBAL)
								{ // global

									LineLength += printf("GV0(%u)", (unsigned int)(ParCode & PRIMPAR_INDEX));
								}
								else
								{ // local

									LineLength += printf("LV0(%u)", (unsigned int)(ParCode & PRIMPAR_INDEX));
								}
							}
							else
							{ // constant

								Value = (ULONG)(ParCode & PRIMPAR_VALUE);

								if (ParCode & PRIMPAR_CONST_SIGN)
								{ // Adjust if negative

									Value |= ~(ULONG)(PRIMPAR_VALUE);
								}
								LineLength += printf("LC0(%d)", (int)Value);

							}
							LineLength += printf(",");
						}
						if (ParType == PARNO)
						{
							if (!(ParCode & PRIMPAR_VARIABLE))
							{
								Parameters = (DATA8)(*(DATA32*)pParValue);
							}
						}
						if (ParType == PARLAB)
						{
							if (!(ParCode & PRIMPAR_VARIABLE))
							{
							}
						}
						//***************************************************************************
						Bytes--;
					}
					Pars = 0;
				}

				if ((ParType >= PAR) || (ParType == PARNO) || (ParType == PARLAB))
				{ // Plain parameter

					if ((LineLength + 10) >= 80)
					{
						printf("\n%*.*s", Indent, Indent, "");
						LineLength = Indent;
					}
					Value = (ULONG)0;
					pParValue = (void*)&Value;
					ParCode = (UBYTE)pI[(*pIndex)++] & 0xFF;

					// Next parameter
					Pars >>= 4;

					// Calculate parameter value

					if (ParCode & PRIMPAR_LONG)
					{ // long format

						if (ParCode & PRIMPAR_HANDLE)
						{
							LineLength += printf("HND(");
						}
						else
						{
							if (ParCode & PRIMPAR_ADDR)
							{
								LineLength += printf("ADR(");
							}
						}
						if (ParCode & PRIMPAR_VARIABLE)
						{ // variable

							if (ParCode & PRIMPAR_GLOBAL)
							{ // global

								LineLength += printf("GV");
							}
							else
							{ // local

								LineLength += printf("LV");
							}
							switch (ParCode & PRIMPAR_BYTES)
							{
							case PRIMPAR_1_BYTE:
							{
								Value = (ULONG)pI[(*pIndex)++];

								LineLength += printf("1(%d)", (int)Value);
							}
							break;

							case PRIMPAR_2_BYTES:
							{
								Value = (ULONG)pI[(*pIndex)++];
								Value |= ((ULONG)pI[(*pIndex)++] << 8);

								LineLength += printf("2(%d)", (int)Value);
							}
							break;

							case PRIMPAR_4_BYTES:
							{
								Value = (ULONG)pI[(*pIndex)++];
								Value |= ((ULONG)pI[(*pIndex)++] << 8);
								Value |= ((ULONG)pI[(*pIndex)++] << 16);
								Value |= ((ULONG)pI[(*pIndex)++] << 24);

								LineLength += printf("4(%d)", (int)Value);
							}
							break;

							}
						}
						else
						{ // constant

							if (ParCode & PRIMPAR_HANDLE)
							{
								LineLength += printf("HND(");
							}
							else
							{
								if (ParCode & PRIMPAR_ADDR)
								{
									LineLength += printf("ADR(");
								}
							}
							if (ParCode & PRIMPAR_LABEL)
							{ // label

								Value = (ULONG)pI[(*pIndex)++];
								if (Value & 0x00000080)
								{ // Adjust if negative

									Value |= 0xFFFFFF00;
								}

								LineLength += printf("LAB1(%d)", (int)(Value & 0xFF));
							}
							else
							{ // value

								switch (ParCode & PRIMPAR_BYTES)
								{
								case PRIMPAR_STRING_OLD:
								case PRIMPAR_STRING:
								{

									LineLength += printf("LCS,");

									while (pI[(*pIndex)])
									{ // Adjust Ip

										if ((LineLength + 5) >= 80)
										{
											printf("\n%*.*s", Indent, Indent, "");
											LineLength = Indent;
										}

										switch (pI[(*pIndex)])
										{
										case '\r':
										{
											LineLength += printf("'\\r',");
										}
										break;

										case '\n':
										{
											LineLength += printf("'\\n',");
										}
										break;

										default:
										{
											LineLength += printf("'%c',", pI[(*pIndex)]);
										}
										break;

										}

										(*pIndex)++;
									}
									(*pIndex)++;

									if ((LineLength + 2) >= 80)
									{
										printf("\n%*.*s", Indent, Indent, "");
										LineLength = Indent;
									}
									LineLength += printf("0");
								}
								break;

								case PRIMPAR_1_BYTE:
								{
									Value = (ULONG)pI[(*pIndex)++];
									if (Value & 0x00000080)
									{ // Adjust if negative

										Value |= 0xFFFFFF00;
									}
									if ((Pars & 0x0f) != SUBP)
									{
										LineLength += printf("LC1(%d)", (int)Value);
									}
								}
								break;

								case PRIMPAR_2_BYTES:
								{
									Value = (ULONG)pI[(*pIndex)++];
									Value |= ((ULONG)pI[(*pIndex)++] << 8);
									if (Value & 0x00008000)
									{ // Adjust if negative

										Value |= 0xFFFF0000;
									}
									if ((Pars & 0x0f) != SUBP)
									{
										LineLength += printf("LC2(%d)", (int)Value);
									}
								}
								break;

								case PRIMPAR_4_BYTES:
								{
									Value = (ULONG)pI[(*pIndex)++];
									Value |= ((ULONG)pI[(*pIndex)++] << 8);
									Value |= ((ULONG)pI[(*pIndex)++] << 16);
									Value |= ((ULONG)pI[(*pIndex)++] << 24);
									if ((Pars & 0x0f) != SUBP)
									{
										LineLength += printf("LC4(%ld)", (long)Value);
									}
								}
								break;

								}
							}
						}
						if (ParCode & PRIMPAR_HANDLE)
						{
							LineLength += printf("),");
						}
						else
						{
							if (ParCode & PRIMPAR_ADDR)
							{
								LineLength += printf("),");
							}
							else
							{
								LineLength += printf(",");
							}
						}
					}
					else
					{ // short format

						if (ParCode & PRIMPAR_VARIABLE)
						{ // variable

							if (ParCode & PRIMPAR_GLOBAL)
							{ // global

								LineLength += printf("GV0(%u)", (unsigned int)(ParCode & PRIMPAR_INDEX));
							}
							else
							{ // local

								LineLength += printf("LV0(%u)", (unsigned int)(ParCode & PRIMPAR_INDEX));
							}
						}
						else
						{ // constant

							Value = (ULONG)(ParCode & PRIMPAR_VALUE);

							if (ParCode & PRIMPAR_CONST_SIGN)
							{ // Adjust if negative

								Value |= ~(ULONG)(PRIMPAR_VALUE);
							}
							if ((Pars & 0x0f) != SUBP)
							{
								LineLength += printf("LC0(%d)", (int)Value);
							}

						}
						if ((Pars & 0x0f) != SUBP)
						{
							LineLength += printf(",");
						}
					}
					if (ParType == PARNO)
					{
						if (!(ParCode & PRIMPAR_VARIABLE))
						{
							Parameters = (DATA8)(*(DATA32*)pParValue);
						}
					}
					if (ParType == PARLAB)
					{
						if (!(ParCode & PRIMPAR_VARIABLE))
						{


						}
					}
					if (Parameters)
					{
						Parameters--;
						Pars = PAR32;
					}

				}
			} while (ParType || Parameters);
		}
		printf("\n");
	}

	return (Result);
}

static RESULT cValidateDisassembleProgram(PRGID PrgId, IP pI, LABEL* pLabel)
{
	RESULT  Result = OK;
	IMINDEX Size;
	OBJID   ObjIndex;
	IMINDEX MinIndex;
	IMINDEX MaxIndex;
	IMINDEX Index;
	IMINDEX Addr;
	ULONG   LastOffset;
	OBJID   LastObject;
	UBYTE   Stop;
	IMINDEX TmpIndex;
	UBYTE   Type;
	DATA32  Lng;

	IMGHEAD* pIH;
	OBJHEAD* pOH;
	OBJID   Objects;

	pIH = (IMGHEAD*)pI;
	pOH = (OBJHEAD*)&pI[sizeof(IMGHEAD) - sizeof(OBJHEAD)];
	Objects = (*(IMGHEAD*)pI).NumberOfObjects;
	Size = (*(IMGHEAD*)pI).ImageSize;

	printf("\n//****************************************************************************");
	printf("\n// Disassembler Listing");
	printf("\n//****************************************************************************");
	printf("\n\nUBYTE     prg[] =\n{\n");

	Addr = 0;
	LastOffset = 0;
	LastObject = 0;
	// Check for validation error
	if ((cValidateGetErrorIndex()) && (cValidateGetErrorIndex() < sizeof(IMGHEAD)))
	{
		printf("ERROR ");
	}
	printf("  /* %4u */  PROGRAMHeader(%.2f,%d,%d),\n", Addr, (float)(*pIH).VersionInfo / (float)100, (*pIH).NumberOfObjects, (*pIH).GlobalBytes);
	Addr += sizeof(IMGHEAD);

	for (ObjIndex = 1; ObjIndex <= Objects; ObjIndex++)
	{
		if ((cValidateGetErrorIndex() >= Addr) && (cValidateGetErrorIndex() < (Addr + sizeof(OBJHEAD))))
		{
			printf("ERROR ");
		}
		if (pOH[ObjIndex].OwnerObjectId)
		{ // BLOCK object

			ValidateInstance.Row = printf("  /* %4u */  BLOCKHeader(%u,%u,%u),", Addr, (ULONG)pOH[ObjIndex].OffsetToInstructions, pOH[ObjIndex].OwnerObjectId, pOH[ObjIndex].TriggerCount);
		}
		else
		{
			if (pOH[ObjIndex].TriggerCount == 1)
			{ // SUBCALL object

				if (LastOffset == (ULONG)pOH[ObjIndex].OffsetToInstructions)
				{ // SUBCALL alias object

					ValidateInstance.Row = printf("  /* %4u */  SUBCALLHeader(%u,%u),", Addr, (ULONG)pOH[ObjIndex].OffsetToInstructions, pOH[ObjIndex].LocalBytes);
				}
				else
				{
					ValidateInstance.Row = printf("  /* %4u */  SUBCALLHeader(%u,%u),", Addr, (ULONG)pOH[ObjIndex].OffsetToInstructions, pOH[ObjIndex].LocalBytes);
				}
			}
			else
			{ // VMTHREAD object

				ValidateInstance.Row = printf("  /* %4u */  VMTHREADHeader(%u,%u),", Addr, (ULONG)pOH[ObjIndex].OffsetToInstructions, pOH[ObjIndex].LocalBytes);
			}
		}
		ValidateInstance.Row = 41 - ValidateInstance.Row;
		if (LastOffset == (ULONG)pOH[ObjIndex].OffsetToInstructions)
		{
			printf("%*.*s// Object %-3u (Alias for object %u)\n", ValidateInstance.Row, ValidateInstance.Row, " ", ObjIndex, LastObject);
		}
		else
		{
			printf("%*.*s// Object %-3u\n", ValidateInstance.Row, ValidateInstance.Row, " ", ObjIndex);
		}
		ValidateInstance.Row = 0;
		LastOffset = (ULONG)pOH[ObjIndex].OffsetToInstructions;
		LastObject = ObjIndex;
		Addr += sizeof(OBJHEAD);
	}

	for (ObjIndex = 1; ObjIndex <= Objects; ObjIndex++)
	{ // for every object - find minimum and maximum instruction pointer values

		MinIndex = (IMINDEX)pOH[ObjIndex].OffsetToInstructions;

		if (ObjIndex == Objects)
		{
			MaxIndex = Size;
		}
		else
		{
			MaxIndex = (IMINDEX)pOH[ObjIndex + 1].OffsetToInstructions;
		}

		if (pOH[ObjIndex].OwnerObjectId)
		{ // BLOCK object

			printf("\n  /* Object %2d (BLOCK) [%lu..%lu] */\n\n", ObjIndex, (long unsigned int)MinIndex, (long unsigned int)MaxIndex);
		}
		else
		{
			if (pOH[ObjIndex].TriggerCount == 1)
			{ // SUBCALL object

				printf("\n  /* Object %2d (SUBCALL) [%lu..%lu] */\n\n", ObjIndex, (long unsigned int)MinIndex, (long unsigned int)MaxIndex);

				ValidateInstance.Row += printf("  /* %4d */  ", MinIndex);
				TmpIndex = (IMINDEX)pI[MinIndex++];
				printf("%u,", TmpIndex);
				while (TmpIndex)
				{
					Type = pI[MinIndex++];
					if (Type & CALLPAR_IN)
					{
						printf("IN_");
					}
					if (Type & CALLPAR_OUT)
					{
						printf("OUT_");
					}
					switch (Type & CALLPAR_TYPE)
					{
					case CALLPAR_DATA8:
					{
						printf("8,");
					}
					break;

					case CALLPAR_DATA16:
					{
						printf("16,");
					}
					break;

					case CALLPAR_DATA32:
					{
						printf("32,");
					}
					break;

					case CALLPAR_DATAF:
					{
						printf("F,");
					}
					break;

					case CALLPAR_STRING:
					{
						Lng = (DATA32)pI[MinIndex++];
						printf("(%d),", Lng);
					}
					break;

					}
					TmpIndex--;
				}
				printf("\n\n");

			}
			else
			{ // VMTHREAD object

				printf("\n  /* Object %2d (VMTHREAD) [%lu..%lu] */\n\n", ObjIndex, (long unsigned int)MinIndex, (long unsigned int)MaxIndex);
			}
		}

		Stop = OK;
		ValidateInstance.Row = 0;

		for (Index = MinIndex; ((MaxIndex == 0) || (Index < MaxIndex)) && (Stop == OK);)
		{ // for every opcode - find number of parameters

			Stop = cValidateDisassemble(pI, &Index, pLabel);
		}

		Addr = Index;

	}
	printf("};\n");
	printf("\n//****************************************************************************\n\n");


	return (Result);
}

static RESULT cValidateCheckAlignment(ULONG Value, DATA8 Type)
{
	RESULT  Result = OK;

	switch (Type)
	{
	case PAR16:
	{
		if ((Value & 1))
		{
			Result = FAIL;
		}
	}
	break;

	case PAR32:
	{
		if ((Value & 3))
		{
			Result = FAIL;
		}
	}
	break;

	case PARF:
	{
		if ((Value & 3))
		{
			Result = FAIL;
		}
	}
	break;

	default:
	{
		Result = OK;
	}
	break;

	}

	return (Result);
}

static RESULT cValidateBytecode(IP pI, IMINDEX* pIndex, LABEL* pLabel)
{
	RESULT  Result = FAIL;
	RESULT  Aligned = OK;
	IMINDEX OpCode;
	ULONG   Pars;
	DATA8   Sub;
	IMGDATA Tab;
	ULONG   Value;
	UBYTE   ParType = PAR;
	UBYTE   ParCode;
	void* pParValue;
	DATA8   Parameters;
	DATA8   ParNo;
	DATA32  Bytes;

	OpCode = pI[*pIndex] & 0xFF;

	if (OpCodes[OpCode].Name)
	{ // Byte code exist

		Parameters = 0;
		ParNo = 0;

		(*pIndex)++;

		if ((OpCode == opERROR) || (OpCode == opOBJECT_END))
		{
			if (OpCode == opOBJECT_END)
			{
				Result = STOP;
			}
		}
		else
		{
			Result = FAIL;

			if (OpCode == opJR_TRUE)
			{
				Bytes = 0;
			}
			// Get opcode parameter types
			Pars = OpCodes[OpCode].Pars;
			do
			{ // check for every parameter

				ParType = Pars & 0x0F;
				if (ParType == 0)
				{
					Result = OK;
				}

				if (ParType == SUBP)
				{ // Check existence of sub command

					Sub = *(DATA8*)pParValue;
					Pars >>= 4;
					Tab = Pars & 0x0F;
					Pars = 0;

					if (Sub < MAX_SUBCODES)
					{
						if (SubCodes[Tab][Sub].Name != NULL)
						{
							Pars = SubCodes[Tab][Sub].Pars;
							Result = OK;
						}
					}
				}

				if (ParType == PARVALUES)
				{ // Last parameter tells number of bytes to follow

					Bytes = *(DATA32*)pParValue;

					Pars >>= 4;
					Pars &= 0x0F;
					//***************************************************************************

					while (Bytes)
					{
						Value = (ULONG)0;
						pParValue = (void*)&Value;
						ParCode = (UBYTE)pI[(*pIndex)++] & 0xFF;
						Aligned = OK;

						// Calculate parameter value
						if (ParCode & PRIMPAR_LONG)
						{ // long format

							if (ParCode & PRIMPAR_VARIABLE)
							{ // variable

								switch (ParCode & PRIMPAR_BYTES)
								{
								case PRIMPAR_1_BYTE:
								{
									Value = (ULONG)pI[(*pIndex)++];
								}
								break;

								case PRIMPAR_2_BYTES:
								{
									Value = (ULONG)pI[(*pIndex)++];
									Value |= ((ULONG)pI[(*pIndex)++] << 8);
								}
								break;

								case PRIMPAR_4_BYTES:
								{
									Value = (ULONG)pI[(*pIndex)++];
									Value |= ((ULONG)pI[(*pIndex)++] << 8);
									Value |= ((ULONG)pI[(*pIndex)++] << 16);
									Value |= ((ULONG)pI[(*pIndex)++] << 24);
								}
								break;

								}
							}
							else
							{ // constant

								if (ParCode & PRIMPAR_LABEL)
								{ // label

									Value = (ULONG)pI[(*pIndex)++];
									if (Value & 0x00000080)
									{ // Adjust if negative

										Value |= 0xFFFFFF00;
									}
								}
								else
								{ // value

									switch (ParCode & PRIMPAR_BYTES)
									{
									case PRIMPAR_STRING_OLD:
									case PRIMPAR_STRING:
									{
										while (pI[(*pIndex)])
										{ // Adjust Ip
											(*pIndex)++;
										}
										(*pIndex)++;
									}
									break;

									case PRIMPAR_1_BYTE:
									{
										Value = (ULONG)pI[(*pIndex)++];
										if (Value & 0x00000080)
										{ // Adjust if negative

											Value |= 0xFFFFFF00;
										}
									}
									break;

									case PRIMPAR_2_BYTES:
									{
										Value = (ULONG)pI[(*pIndex)++];
										Value |= ((ULONG)pI[(*pIndex)++] << 8);
										if (Value & 0x00008000)
										{ // Adjust if negative

											Value |= 0xFFFF0000;
										}
									}
									break;

									case PRIMPAR_4_BYTES:
									{
										Value = (ULONG)pI[(*pIndex)++];
										Value |= ((ULONG)pI[(*pIndex)++] << 8);
										Value |= ((ULONG)pI[(*pIndex)++] << 16);
										Value |= ((ULONG)pI[(*pIndex)++] << 24);
									}
									break;

									}
								}
							}
						}
						else
						{ // short format

							if (ParCode & PRIMPAR_VARIABLE)
							{ // variable

								Value = (ULONG)(ParCode & PRIMPAR_VALUE);
							}
							else
							{ // constant

								Value = (ULONG)(ParCode & PRIMPAR_VALUE);

								if (ParCode & PRIMPAR_CONST_SIGN)
								{ // Adjust if negative

									Value |= ~(ULONG)(PRIMPAR_VALUE);
								}
							}
						}

						// Check parameter value
						if ((Pars >= PAR8) && (Pars <= PAR32))
						{
							if (((*(DATA32*)pParValue) >= ParMin[Pars - PAR]) && ((*(DATA32*)pParValue) <= ParMax[Pars - PAR]))
							{
								Result = OK;
							}
						}
						if ((Pars == PARF))
						{
							if ((*(DATAF*)pParValue >= DATAF_MIN) && (*(DATAF*)pParValue <= DATAF_MAX))
							{
								Result = OK;
							}
						}
						if (Pars == PARV)
						{
							Result = OK;
						}
						Bytes--;
					}
					//***************************************************************************
					Pars = 0;
				}

				if ((ParType >= PAR) || (ParType == PARNO) || (ParType == PARLAB))
				{ // Check parameter

					Value = (ULONG)0;
					pParValue = (void*)&Value;
					ParCode = (UBYTE)pI[(*pIndex)++] & 0xFF;
					Aligned = OK;

					// Calculate parameter value



					if (ParCode & PRIMPAR_LONG)
					{ // long format

						if (ParCode & PRIMPAR_VARIABLE)
						{ // variable

							switch (ParCode & PRIMPAR_BYTES)
							{
							case PRIMPAR_1_BYTE:
							{
								Value = (ULONG)pI[(*pIndex)++];
							}
							break;

							case PRIMPAR_2_BYTES:
							{
								Value = (ULONG)pI[(*pIndex)++];
								Value |= ((ULONG)pI[(*pIndex)++] << 8);
							}
							break;

							case PRIMPAR_4_BYTES:
							{
								Value = (ULONG)pI[(*pIndex)++];
								Value |= ((ULONG)pI[(*pIndex)++] << 8);
								Value |= ((ULONG)pI[(*pIndex)++] << 16);
								Value |= ((ULONG)pI[(*pIndex)++] << 24);
							}
							break;

							}
						}
						else
						{ // constant

							if (ParCode & PRIMPAR_LABEL)
							{ // label

								Value = (ULONG)pI[(*pIndex)++];
								if (Value & 0x00000080)
								{ // Adjust if negative

									Value |= 0xFFFFFF00;
								}
							}
							else
							{ // value

								switch (ParCode & PRIMPAR_BYTES)
								{
								case PRIMPAR_STRING_OLD:
								case PRIMPAR_STRING:
								{
									while (pI[(*pIndex)])
									{ // Adjust Ip
										(*pIndex)++;
									}
									(*pIndex)++;
								}
								break;

								case PRIMPAR_1_BYTE:
								{
									Value = (ULONG)pI[(*pIndex)++];
									if (Value & 0x00000080)
									{ // Adjust if negative

										Value |= 0xFFFFFF00;
									}
								}
								break;

								case PRIMPAR_2_BYTES:
								{
									Value = (ULONG)pI[(*pIndex)++];
									Value |= ((ULONG)pI[(*pIndex)++] << 8);
									if (Value & 0x00008000)
									{ // Adjust if negative

										Value |= 0xFFFF0000;
									}
								}
								break;

								case PRIMPAR_4_BYTES:
								{
									Value = (ULONG)pI[(*pIndex)++];
									Value |= ((ULONG)pI[(*pIndex)++] << 8);
									Value |= ((ULONG)pI[(*pIndex)++] << 16);
									Value |= ((ULONG)pI[(*pIndex)++] << 24);
								}
								break;

								}
							}
						}
					}
					else
					{ // short format

						if (ParCode & PRIMPAR_VARIABLE)
						{ // variable

							Value = (ULONG)(ParCode & PRIMPAR_VALUE);
						}
						else
						{ // constant

							Value = (ULONG)(ParCode & PRIMPAR_VALUE);

							if (ParCode & PRIMPAR_CONST_SIGN)
							{ // Adjust if negative

								Value |= ~(ULONG)(PRIMPAR_VALUE);
							}
						}
					}

					if (ParCode & PRIMPAR_VARIABLE)
					{
						Result = OK;
					}
					else
					{ // Check constant parameter value

						if ((ParType >= PAR8) && (ParType <= PAR32))
						{
							if (((*(DATA32*)pParValue) >= ParMin[ParType - PAR]) && ((*(DATA32*)pParValue) <= ParMax[ParType - PAR]))
							{
								Result = OK;
							}
						}
						if ((ParType == PARF))
						{
							Result = OK;
						}
					}
					if (ParType == PARV)
					{
						Result = OK;
					}
					if (ParType == PARNO)
					{ // Check number of parameters

						ParNo = 1;
						if (!(ParCode & PRIMPAR_VARIABLE))
						{ // Must be constant

							if (((*(DATA32*)pParValue) >= 0) && ((*(DATA32*)pParValue) <= DATA8_MAX))
							{ // Must be positive and than less than or equal to 127

								Parameters = (DATA8)(*(DATA32*)pParValue);
								Result = OK;
							}
						}
					}
					if (ParType == PARLAB)
					{ // Check number of parameters

						if (!(ParCode & PRIMPAR_VARIABLE))
						{ // Must be constant

							if (((*(DATA32*)pParValue) >= 0) && ((*(DATA32*)pParValue) < MAX_LABELS))
							{
								if (pLabel != NULL)
								{
									pLabel[Value].Addr = *pIndex;
								}

								Result = OK;
							}
						}
					}

					// Check allocation
					if (ParNo == 0)
					{
						if (ParCode & PRIMPAR_LONG)
						{ // long format

							if (ParCode & PRIMPAR_VARIABLE)
							{ // variable

								if (ParCode & PRIMPAR_HANDLE)
								{ // handle

									Aligned = cValidateCheckAlignment(Value, PAR16);
								}
								else
								{

									if (ParCode & PRIMPAR_GLOBAL)
									{ // global
									}
									else
									{ // local
									}
									Aligned = cValidateCheckAlignment(Value, ParType);
								}
							}
							else
							{ // constant

								if (ParCode & PRIMPAR_LABEL)
								{ // label

								}
								else
								{ // value

								}
							}
						}
						else
						{ // short format

							if (ParCode & PRIMPAR_VARIABLE)
							{ // variable

								if (ParCode & PRIMPAR_GLOBAL)
								{ // global
								}
								else
								{ // local
								}
								Aligned = cValidateCheckAlignment(Value, ParType);
							}
						}
					}

					// Next parameter
					Pars >>= 4;
					if (Parameters)
					{
						Parameters--;
						Pars = PAR32;
					}

				}
				if (Aligned != OK)
				{
					Result = FAIL;
				}
			} while ((ParType) && (Result == OK));
		}
	}

	return (Result);
}

RESULT cValidateProgram(PRGID PrgId, IP pI, LABEL* pLabel, DATA8 Disassemble)
{
	RESULT  Result;
	IMGHEAD* pIH;             // Pointer to image header
	IMINDEX TotalSize;        // Total image size
	OBJID   Objects;          // Total number of objects
	IMINDEX TmpSize;
	OBJHEAD* pOH;
	OBJID   ObjIndex;
	IMINDEX ImageIndex;       // Index into total image
	IMINDEX TmpIndex = 0;     // Lached "ImageIndex"
	UBYTE   ParIndex;
	UBYTE   Type;


#ifdef    DEBUG
	printf("Validation started\n");
#endif
	cValidateSetErrorIndex(0);
	pIH = (IMGHEAD*)pI;

	TotalSize = (*(IMGHEAD*)pI).ImageSize;
	Objects = (*(IMGHEAD*)pI).NumberOfObjects;

	// Check size
	ImageIndex = sizeof(IMGHEAD) + Objects * sizeof(OBJHEAD);

	if ((TotalSize < ImageIndex) || (Objects == 0))
	{ // Size too small

		cValidateSetErrorIndex(4);
	}
	else
	{
		pOH = (OBJHEAD*)&pI[sizeof(IMGHEAD) - sizeof(OBJHEAD)];

		// Scan headers for size of instructions
		TmpSize = 0;
		for (ObjIndex = 1; ObjIndex <= Objects; ObjIndex++)
		{
			TmpSize += (IMINDEX)pOH[ObjIndex].OffsetToInstructions;
		}
		Result = OK;

		// Scan every object
		for (ObjIndex = 1; (ObjIndex <= Objects) && (Result == OK); ObjIndex++)
		{
			if (pOH[ObjIndex].OffsetToInstructions == 0)
			{
				pOH[ObjIndex].OffsetToInstructions = (IP)ImageIndex;
			}

			// Check for SUBCALL parameter description
			if ((pOH[ObjIndex].OwnerObjectId == 0) && (pOH[ObjIndex].TriggerCount == 1))
			{ // SUBCALL object

				if (pOH[ObjIndex].OffsetToInstructions >= (IP)ImageIndex)
				{ // Only if not alias

					ParIndex = (IMINDEX)pI[ImageIndex++];
					while (ParIndex)
					{
						Type = pI[ImageIndex++];
						if ((Type & CALLPAR_TYPE) == CALLPAR_STRING)
						{
							ImageIndex++;
						}
						ParIndex--;
					}
				}
				else
				{
					Result = STOP;
				}
			}

			// Scan all byte codes in object
			while ((Result == OK) && (ImageIndex < TotalSize))
			{
				TmpIndex = ImageIndex;
				Result = cValidateBytecode(pI, &ImageIndex, pLabel);
			}
			if (Result == FAIL)
			{
				cValidateSetErrorIndex(TmpIndex);
			}
			else
			{
				Result = OK;
			}
		}
		if (ImageIndex > TotalSize)
		{
#ifdef    DEBUG
			printf("%d %d\n", ImageIndex, TotalSize);
#endif
			cValidateSetErrorIndex(TmpIndex);
		}
	}

	// Check version
	if (((*pIH).VersionInfo < ((UWORD)(BYTECODE_VERSION * 100.0))) && ((*pIH).VersionInfo != 0))
	{
		cValidateSetErrorIndex(8);
	}

#ifdef    DEBUG
	printf("Validation ended\n");
#endif
	if (Disassemble)
	{
		cValidateDisassembleProgram(PrgId, pI, pLabel);
	}
	// Result of validation
	if (cValidateGetErrorIndex())
	{
		if (cValidateGetErrorIndex() == 8)
		{
			Result = OK;
		}
		else
		{
			Result = FAIL;
		}
	}
	else
	{
		Result = OK;
	}

	return (Result);
}
