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
 *
 * As a special exception, if other files instantiate templates or use macros or
 * inline functions from this file, or you  compile this file and link it with
 * other works to produce a work based on this file, this file does not by itself
 * cause the resulting work to be covered by the GNU General Public License.
 * However the source code for this file must still be made available in accordance
 * with section (3) of the GNU General Public License.
 */

#ifndef   BYTECODES_H_
#define   BYTECODES_H_

#define   BYTECODE_VERSION              1.09

// TODO: this can be dropped after next lmsasm release (>> 1.2.0)
// Work around duplicate enum value in bytecodes
#define DEVCMD_RESET DEVCMD_FIRE

// Defines

#define vmAPPS_DIR "../apps"  // Apps folder
#define vmBG_COLOR (0)  // Background color
#define vmBLUETOOTH_FILE_NAME "Bluetooth"  // File used in "Bluetooth" app to save status
#define vmBRICKNAMESIZE (120)  // Brick name maximal size (including zero termination)
#define vmBTADRSIZE (13)  // Max bluetooth address size including zero termination
#define vmBTPASSKEYSIZE (7)  // Bluetooth pass key size (including zero termination)
#define vmBUTTONS (6)  // Number of buttons in the system
#define vmCALDATA_FILE_NAME "caldata"  // Calibration data filename
#define vmCHAIN_DEPT (4)  // Number of bricks in the USB daisy chain (master + slaves)
#define vmCHARSET_BTPASSKEY (4)  // Character set allowed in bluetooth pass key
#define vmCHARSET_FILENAME (2)  // Character set allowed in file names
#define vmCHARSET_NAME (1)  // Character set allowed in brick name and raw filenames
#define vmCHARSET_WIFIPASSKEY (8)  // Character set allowed in WiFi pass key
#define vmCHARSET_WIFISSID (16)  // Character set allowed in WiFi ssid
#define vmDATA16_MAX (32767)  // DATA16 positive limit
#define vmDATA16_MIN (-32767)  // DATA16 negative limit
#define vmDATA16_NAN (32768)
#define vmDATA32_MAX (2147483647)  // DATA32 positive limit
#define vmDATA32_MIN (-2147483647)  // DATA32 negative limit
#define vmDATA32_NAN (2147483648)
#define vmDATA8_MAX (127)  // DATA8  positive limit
#define vmDATA8_MIN (-127)  // DATA8  negative limit
#define vmDATA8_NAN (128)
#define vmDATAF_MAX (2147483647)  // DATAF  positive limit
#define vmDATAF_MIN (-2147483647)  // DATAF  negative limit
#define vmDATAF_NAN (2143289344)
#define vmDATALOG_FOLDER "../prjs/BrkDL_SAVE"  // Folder for On Brick Data log files
#define vmDEFAULT_SLEEPMINUTES (30)
#define vmDEFAULT_VOLUME (100)
#define vmDIR_DEEPT (127)  // Max directory items allocated including "." and ".."
#define vmERR_STRING_SIZE (32)  // Inclusive zero termination
#define vmEVENT_BT_PIN (1)
#define vmEVENT_BT_REQ_CONF (2)
#define vmEVENT_NONE (0)
#define vmEXTSIZE (5)  // Max extension size including dot and zero termination
#define vmEXT_ARCHIVE ".raf"  // Robot Archive File
#define vmEXT_BYTECODE ".rbf"  // Robot Byte code File
#define vmEXT_CONFIG ".rcf"  // Robot Configuration File
#define vmEXT_DATALOG ".rdf"  // Robot Datalog File
#define vmEXT_GRAPHICS ".rgf"  // Robot Graphics File
#define vmEXT_PROGRAM ".rpf"  // Robot Program File
#define vmEXT_SOUND ".rsf"  // Robot Sound File
#define vmEXT_TEXT ".rtf"  // Robot Text File
#define vmFG_COLOR (1)  // Forground color
#define vmFILENAMESIZE (120)  // Max filename size including path, name, extension and termination (must be divideable by 4)
#define vmINPUTS (4)  // Number of input  ports in the system
#define vmIPSIZE (16)  // Max WIFI IP size including zero termination
#define vmLASTRUN_FILE_NAME "lastrun"  // Last run filename
#define vmLCD_HEIGHT (128)  // LCD vertical pixels
#define vmLCD_STORE_LEVELS (3)  // Store levels
#define vmLCD_WIDTH (178)  // LCD horizontal pixels
#define vmLEDS (4)  // Number of LEDs in the system
#define vmMACSIZE (18)  // Max WIFI MAC size including zero termination
#define vmMAX_VALID_TYPE (101)  // Highest valid device type
#define vmMEMORY_FOLDER "/mnt/ramdisk"  // Folder for non volatile user programs/data
#define vmNAMESIZE (32)  // Max name size including zero termination (must be divideable by 4)
#define vmOUTPUTS (4)  // Number of output ports in the system
#define vmPATHSIZE (84)  // Max path size excluding trailing forward slash including zero termination
#define vmPOP3_ABS_WARN_ICON_X (64)
#define vmPOP3_ABS_WARN_ICON_X1 (40)
#define vmPOP3_ABS_WARN_ICON_X2 (72)
#define vmPOP3_ABS_WARN_ICON_X3 (104)
#define vmPOP3_ABS_WARN_ICON_Y (60)
#define vmPOP3_ABS_WARN_LINE_ENDX (155)
#define vmPOP3_ABS_WARN_LINE_X (21)
#define vmPOP3_ABS_WARN_LINE_Y (89)
#define vmPOP3_ABS_WARN_SPEC_ICON_X (88)
#define vmPOP3_ABS_WARN_SPEC_ICON_Y (60)
#define vmPOP3_ABS_WARN_TEXT_X (80)
#define vmPOP3_ABS_WARN_TEXT_Y (68)
#define vmPOP3_ABS_WARN_YES_X (72)
#define vmPOP3_ABS_WARN_YES_Y (90)
#define vmPOP3_ABS_X (16)
#define vmPOP3_ABS_Y (50)
#define vmPRJS_DIR "../prjs"  // Project folder
#define vmPROGRAM_FOLDER "../prjs/BrkProg_SAVE"  // Folder for On Brick Programming programs
#define vmPULSE_BROWSER (2)
#define vmPULSE_GUI_BACKGROUND (1)
#define vmPULSE_KEY (4)
#define vmSDCARD_FOLDER "../prjs/SD_Card"  // Folder for SD card mount
#define vmSETTINGS_DIR "../sys/settings"  // Folder for non volatile settings
#define vmSLEEP_FILE_NAME "Sleep"  // File used in "Sleep" app to save status
#define vmTMP_DIR "../tmp"  // Temporary folder
#define vmTOOLS_DIR "../tools"  // Tools folder
#define vmTOPLINE_HEIGHT (10)  // Top line vertical pixels
#define vmUSBSTICK_FOLDER "../prjs/USB_Stick"  // Folder for USB stick mount
#define vmVOLUME_FILE_NAME "Volume"  // File used in "Volume" app to save status
#define vmWIFIPASSKEYSIZE (33)  // WiFi pass key size (including zero termination)
#define vmWIFI_FILE_NAME "WiFi"  // File used in "WiFi" app to save status

// Opcodes

typedef enum { 
    opERROR = 0x00,  // This code does not exist in normal program
    opNOP = 0x01,  // This code does absolutely nothing
    opPROGRAM_STOP = 0x02,  // Stops specific program id slot
    opPROGRAM_START = 0x03,  // Start program id slot
    opOBJECT_STOP = 0x04,  // Stops specific object
    opOBJECT_START = 0x05,  // Start specific object
    opOBJECT_TRIG = 0x06,  // Triggers object and run the object if fully triggered
    opOBJECT_WAIT = 0x07,  // Wait until object has run
    opRETURN = 0x08,  // Return from byte code subroutine
    opCALL = 0x09,  // Calls byte code subroutine
    opOBJECT_END = 0x0A,  // Stops current object
    opSLEEP = 0x0B,  // Breaks execution of current VMTHREAD
    opPROGRAM_INFO = 0x0C,  // Get program data
    opLABEL = 0x0D,  // This code does nothing besides marking an address to a label
    opPROBE = 0x0E,  // Display globals or object locals on terminal
    opDO = 0x0F,  // Run byte code snippet
    opADD8 = 0x10,  // Add two 8-bit values DESTINATION = SOURCE1 + SOURCE2
    opADD16 = 0x11,  // Add two 16-bit values DESTINATION = SOURCE1 + SOURCE2
    opADD32 = 0x12,  // Add two 32-bit values DESTINATION = SOURCE1 + SOURCE2
    opADDF = 0x13,  // Add two floating point values DESTINATION = SOURCE1 + SOURCE2
    opSUB8 = 0x14,  // Subtract two 8 bit values DESTINATION = SOURCE1 - SOURCE2
    opSUB16 = 0x15,  // Subtract two 16 bit values DESTINATION = SOURCE1 - SOURCE2
    opSUB32 = 0x16,  // Subtract two 32 bit values DESTINATION = SOURCE1 - SOURCE2
    opSUBF = 0x17,  // Subtract two floating point values DESTINATION = SOURCE1 - SOURCE2
    opMUL8 = 0x18,  // Multiply two 8 bit values DESTINATION = SOURCE1 * SOURCE2
    opMUL16 = 0x19,  // Multiply two 16 bit values DESTINATION = SOURCE1 * SOURCE2
    opMUL32 = 0x1A,  // Multiply two 32 bit values DESTINATION = SOURCE1 * SOURCE2
    opMULF = 0x1B,  // Multiply two floating point values DESTINATION = SOURCE1 * SOURCE2
    opDIV8 = 0x1C,  // Divide two 8 bit values DESTINATION = SOURCE1 / SOURCE2
    opDIV16 = 0x1D,  // Divide two 16 bit values DESTINATION = SOURCE1 / SOURCE2
    opDIV32 = 0x1E,  // Divide two 32 bit values DESTINATION = SOURCE1 / SOURCE2
    opDIVF = 0x1F,  // Divide two floating point values DESTINATION = SOURCE1 / SOURCE2
    opOR8 = 0x20,  // Or two 8 bit values DESTINATION = SOURCE1 | SOURCE2
    opOR16 = 0x21,  // Or two 16 bit values DESTINATION = SOURCE1 | SOURCE2
    opOR32 = 0x22,  // Or two 32 bit values DESTINATION = SOURCE1 | SOURCE2
    opAND8 = 0x24,  // And two 8 bit values DESTINATION = SOURCE1 & SOURCE2
    opAND16 = 0x25,  // And two 16 bit values DESTINATION = SOURCE1 & SOURCE2
    opAND32 = 0x26,  // And two 32 bit values DESTINATION = SOURCE1 & SOURCE2
    opXOR8 = 0x28,  // Exclusive or two 8 bit values DESTINATION = SOURCE1 ^ SOURCE2
    opXOR16 = 0x29,  // Exclusive or two 16 bit values DESTINATION = SOURCE1 ^ SOURCE2
    opXOR32 = 0x2A,  // Exclusive or two 32 bit values DESTINATION = SOURCE1 ^ SOURCE2
    opRL8 = 0x2C,  // Rotate left 8 bit value DESTINATION = SOURCE1 << SOURCE2
    opRL16 = 0x2D,  // Rotate left 16 bit value DESTINATION = SOURCE1 << SOURCE2
    opRL32 = 0x2E,  // Rotate left 32 bit value DESTINATION = SOURCE1 << SOURCE2
    opINIT_BYTES = 0x2F,  // Move LENGTH number of DATA8 from BYTE STREAM to memory DESTINATION START
    opMOVE8_8 = 0x30,  // Move 8 bit value from SOURCE to DESTINATION
    opMOVE8_16 = 0x31,  // Move 8 bit value from SOURCE to DESTINATION
    opMOVE8_32 = 0x32,  // Move 8 bit value from SOURCE to DESTINATION
    opMOVE8_F = 0x33,  // Move 8 bit value from SOURCE to DESTINATION
    opMOVE16_8 = 0x34,  // Move 16 bit value from SOURCE to DESTINATION
    opMOVE16_16 = 0x35,  // Move 16 bit value from SOURCE to DESTINATION
    opMOVE16_32 = 0x36,  // Move 16 bit value from SOURCE to DESTINATION
    opMOVE16_F = 0x37,  // Move 16 bit value from SOURCE to DESTINATION
    opMOVE32_8 = 0x38,  // Move 32 bit value from SOURCE to DESTINATION
    opMOVE32_16 = 0x39,  // Move 32 bit value from SOURCE to DESTINATION
    opMOVE32_32 = 0x3A,  // Move 32 bit value from SOURCE to DESTINATION
    opMOVE32_F = 0x3B,  // Move 32 bit value from SOURCE to DESTINATION
    opMOVEF_8 = 0x3C,  // Move floating point value from SOURCE to DESTINATION
    opMOVEF_16 = 0x3D,  // Move floating point value from SOURCE to DESTINATION
    opMOVEF_32 = 0x3E,  // Move floating point value from SOURCE to DESTINATION
    opMOVEF_F = 0x3F,  // Move floating point value from SOURCE to DESTINATION
    opJR = 0x40,  // Branch unconditionally relative
    opJR_FALSE = 0x41,  // Branch relative if FLAG is FALSE (zero)
    opJR_TRUE = 0x42,  // Branch relative if FLAG is TRUE (non zero)
    opJR_NAN = 0x43,  // Branch relative if VALUE is NAN (not a number)
    opCP_LT8 = 0x44,  // If LEFT is less than RIGTH - set FLAG
    opCP_LT16 = 0x45,  // If LEFT is less than RIGTH - set FLAG
    opCP_LT32 = 0x46,  // If LEFT is less than RIGTH - set FLAG
    opCP_LTF = 0x47,  // If LEFT is less than RIGTH - set FLAG
    opCP_GT8 = 0x48,  // If LEFT is greater than RIGTH - set FLAG
    opCP_GT16 = 0x49,  // If LEFT is greater than RIGTH - set FLAG
    opCP_GT32 = 0x4A,  // If LEFT is greater than RIGTH - set FLAG
    opCP_GTF = 0x4B,  // If LEFT is greater than RIGTH - set FLAG
    opCP_EQ8 = 0x4C,  // If LEFT is equal to RIGTH - set FLAG
    opCP_EQ16 = 0x4D,  // If LEFT is equal to RIGTH - set FLAG
    opCP_EQ32 = 0x4E,  // If LEFT is equal to RIGTH - set FLAG
    opCP_EQF = 0x4F,  // If LEFT is equal to RIGTH - set FLAG
    opCP_NEQ8 = 0x50,  // If LEFT is not equal to RIGTH - set FLAG
    opCP_NEQ16 = 0x51,  // If LEFT is not equal to RIGTH - set FLAG
    opCP_NEQ32 = 0x52,  // If LEFT is not equal to RIGTH - set FLAG
    opCP_NEQF = 0x53,  // If LEFT is not equal to RIGTH - set FLAG
    opCP_LTEQ8 = 0x54,  // If LEFT is less than or equal to RIGTH - set FLAG
    opCP_LTEQ16 = 0x55,  // If LEFT is less than or equal to RIGTH - set FLAG
    opCP_LTEQ32 = 0x56,  // If LEFT is less than or equal to RIGTH - set FLAG
    opCP_LTEQF = 0x57,  // If LEFT is less than or equal to RIGTH - set FLAG
    opCP_GTEQ8 = 0x58,  // If LEFT is greater than or equal to RIGTH - set FLAG
    opCP_GTEQ16 = 0x59,  // If LEFT is greater than or equal to RIGTH - set FLAG
    opCP_GTEQ32 = 0x5A,  // If LEFT is greater than or equal to RIGTH - set FLAG
    opCP_GTEQF = 0x5B,  // If LEFT is greater than or equal to RIGTH - set FLAG
    opSELECT8 = 0x5C,  // If FLAG is set move SOURCE1 to RESULT else move SOURCE2 to RESULT
    opSELECT16 = 0x5D,  // If FLAG is set move SOURCE1 to RESULT else move SOURCE2 to RESULT
    opSELECT32 = 0x5E,  // If FLAG is set move SOURCE1 to RESULT else move SOURCE2 to RESULT
    opSELECTF = 0x5F,  // If FLAG is set move SOURCE1 to RESULT else move SOURCE2 to RESULT
    opSYSTEM = 0x60,  // Executes a system command
    opPORT_CNV_OUTPUT = 0x61,  // Convert encoded port to Layer and Bitfield
    opPORT_CNV_INPUT = 0x62,  // Convert encoded port to Layer and Port
    opNOTE_TO_FREQ = 0x63,  // Convert note to tone
    opJR_LT8 = 0x64,  // Branch relative OFFSET if LEFT is less than RIGHT
    opJR_LT16 = 0x65,  // Branch relative OFFSET if LEFT is less than RIGHT
    opJR_LT32 = 0x66,  // Branch relative OFFSET if LEFT is less than RIGHT
    opJR_LTF = 0x67,  // Branch relative OFFSET if LEFT is less than RIGHT
    opJR_GT8 = 0x68,  // Branch relative OFFSET if LEFT is greater than RIGHT
    opJR_GT16 = 0x69,  // Branch relative OFFSET if LEFT is greater than RIGHT
    opJR_GT32 = 0x6A,  // Branch relative OFFSET if LEFT is greater than RIGHT
    opJR_GTF = 0x6B,  // Branch relative OFFSET if LEFT is greater than RIGHT
    opJR_EQ8 = 0x6C,  // Branch relative OFFSET if LEFT is equal to RIGHT
    opJR_EQ16 = 0x6D,  // Branch relative OFFSET if LEFT is equal to RIGHT
    opJR_EQ32 = 0x6E,  // Branch relative OFFSET if LEFT is equal to RIGHT
    opJR_EQF = 0x6F,  // Branch relative OFFSET if LEFT is equal to RIGHT
    opJR_NEQ8 = 0x70,  // Branch relative OFFSET if LEFT is not equal to RIGHT
    opJR_NEQ16 = 0x71,  // Branch relative OFFSET if LEFT is not equal to RIGHT
    opJR_NEQ32 = 0x72,  // Branch relative OFFSET if LEFT is not equal to RIGHT
    opJR_NEQF = 0x73,  // Branch relative OFFSET if LEFT is not equal to RIGHT
    opJR_LTEQ8 = 0x74,  // Branch relative OFFSET if LEFT is less than or equal to RIGHT
    opJR_LTEQ16 = 0x75,  // Branch relative OFFSET if LEFT is less than or equal to RIGHT
    opJR_LTEQ32 = 0x76,  // Branch relative OFFSET if LEFT is less than or equal to RIGHT
    opJR_LTEQF = 0x77,  // Branch relative OFFSET if LEFT is less than or equal to RIGHT
    opJR_GTEQ8 = 0x78,  // Branch relative OFFSET if LEFT is greater than or equal to RIGHT
    opJR_GTEQ16 = 0x79,  // Branch relative OFFSET if LEFT is greater than or equal to RIGHT
    opJR_GTEQ32 = 0x7A,  // Branch relative OFFSET if LEFT is greater than or equal to RIGHT
    opJR_GTEQF = 0x7B,  // Branch relative OFFSET if LEFT is greater than or equal to RIGHT
    opINFO = 0x7C,  // Info functions entry
    opSTRINGS = 0x7D,  // String function entry
    opMEMORY_WRITE = 0x7E,  // Write VM memory
    opMEMORY_READ = 0x7F,  // Read VM memory
    opUI_FLUSH = 0x80,  // User Interface flush buffers
    opUI_READ = 0x81,  // User Interface read
    opUI_WRITE = 0x82,
    opUI_BUTTON = 0x83,
    opUI_DRAW = 0x84,  // UI draw
    opTIMER_WAIT = 0x85,  // Setup timer to wait TIME mS
    opTIMER_READY = 0x86,  // Wait for timer ready (wait for timeout)
    opTIMER_READ = 0x87,  // Read free running timer [mS]
    opBP0 = 0x88,  // Display globals or object locals on terminal
    opBP1 = 0x89,  // Display globals or object locals on terminal
    opBP2 = 0x8A,  // Display globals or object locals on terminal
    opBP3 = 0x8B,  // Display globals or object locals on terminal
    opBP_SET = 0x8C,  // Set break point in byte code program
    opMATH = 0x8D,  // Math function entry
    opRANDOM = 0x8E,  // Get random value
    opTIMER_READ_US = 0x8F,  // Read free running timer [uS]
    opKEEP_ALIVE = 0x90,  // Keep alive
    opCOM_READ = 0x91,  // Communication read
    opCOM_WRITE = 0x92,  // Communication write
    opSOUND = 0x94,
    opSOUND_TEST = 0x95,  // Test if sound busy (playing file or tone
    opSOUND_READY = 0x96,  // Wait for sound ready (wait until sound finished)
    opINPUT_SAMPLE = 0x97,  // Sample devices
    opINPUT_DEVICE_LIST = 0x98,  // Read all available devices on input and output(chain)
    opINPUT_DEVICE = 0x99,  // Read information about device
    opINPUT_READ = 0x9A,  // Read device value in Percent
    opINPUT_TEST = 0x9B,  // Test if device busy (changing type or mode)
    opINPUT_READY = 0x9C,  // Wait for device ready (wait for valid data)
    opINPUT_READSI = 0x9D,  // Read device value in SI units
    opINPUT_READEXT = 0x9E,  // Read device value
    opINPUT_WRITE = 0x9F,  // Write data to device (only UART devices)
    opOUTPUT_SET_TYPE = 0xA1,  // Set output type
    opOUTPUT_RESET = 0xA2,  // Resets the Tacho counts
    opOUTPUT_STOP = 0xA3,  // Stops the outputs
    opOUTPUT_POWER = 0xA4,  // Set power of the outputs
    opOUTPUT_SPEED = 0xA5,  // Set speed of the outputs
    opOUTPUT_START = 0xA6,  // Starts the outputs
    opOUTPUT_POLARITY = 0xA7,  // Set polarity of the outputs
    opOUTPUT_READ = 0xA8,
    opOUTPUT_TEST = 0xA9,  // Testing if output is not used
    opOUTPUT_READY = 0xAA,  // Wait for output ready (wait for completion)
    opOUTPUT_STEP_POWER = 0xAC,  // Set Ramp up, constant and rampdown steps and power of the outputs
    opOUTPUT_TIME_POWER = 0xAD,  // Set Ramp up, constant and rampdown steps and power of the outputs
    opOUTPUT_STEP_SPEED = 0xAE,  // Set Ramp up, constant and rampdown steps and power of the outputs
    opOUTPUT_TIME_SPEED = 0xAF,  // Set Ramp up, constant and rampdown steps and power of the outputs
    opOUTPUT_STEP_SYNC = 0xB0,
    opOUTPUT_TIME_SYNC = 0xB1,
    opOUTPUT_CLR_COUNT = 0xB2,  // Clearing tacho count when used as sensor
    opOUTPUT_GET_COUNT = 0xB3,  // Getting tacho count when used as sensor - values are in shared memory
    opOUTPUT_PRG_STOP = 0xB4,  // Program stop
    opFILE = 0xC0,  // Memory file entry
    opARRAY = 0xC1,  // Array entry
    opARRAY_WRITE = 0xC2,  // Array element write
    opARRAY_READ = 0xC3,  // Array element read
    opARRAY_APPEND = 0xC4,  // Array element append
    opMEMORY_USAGE = 0xC5,  // Get memory usage
    opFILENAME = 0xC6,  // Memory filename entry
    opREAD8 = 0xC8,  // Read 8 bit value from SOURCE[INDEX] to DESTINATION
    opREAD16 = 0xC9,  // Read 16 bit value from SOURCE[INDEX] to DESTINATION
    opREAD32 = 0xCA,  // Read 32 bit value from SOURCE[INDEX] to DESTINATION
    opREADF = 0xCB,  // Read floating point value from SOURCE[INDEX] to DESTINATION
    opWRITE8 = 0xCC,  // Write 8 bit value from SOURCE to DESTINATION[INDEX]
    opWRITE16 = 0xCD,  // Write 16 bit value from SOURCE to DESTINATION[INDEX]
    opWRITE32 = 0xCE,  // Write 32 bit value from SOURCE to DESTINATION[INDEX]
    opWRITEF = 0xCF,  // Write floating point value from SOURCE to DESTINATION[INDEX]
    opCOM_READY = 0xD0,  // Test if communication is busy
    opCOM_READDATA = 0xD1,  // This code does not exist in normal program
    opCOM_WRITEDATA = 0xD2,  // This code does not exist in normal program
    opCOM_GET = 0xD3,  // Communication get entry
    opCOM_SET = 0xD4,  // Communication set entry
    opCOM_TEST = 0xD5,  // Test if communication is busy
    opCOM_REMOVE = 0xD6,  // Removes a know remote device from the brick
    opCOM_WRITEFILE = 0xD7,  // Sends a file or folder to remote brick.
    opMAILBOX_OPEN = 0xD8,  // Open a mail box on the brick
    opMAILBOX_WRITE = 0xD9,  // Write to mailbox in remote brick
    opMAILBOX_READ = 0xDA,  // Read data from mailbox specified by NO
    opMAILBOX_TEST = 0xDB,  // Tests if new message has been read
    opMAILBOX_READY = 0xDC,  // Waiting from message to be read
    opMAILBOX_CLOSE = 0xDD,  // Closes mailbox indicated by NO
    opINPUT_SET_CONN = 0xE0,  // Set the connection type for a specific port
    opINPUT_IIC_READ = 0xE1,  // Read I2C data from specified port
    opINPUT_IIC_STATUS = 0xE2,  // Read I2C status of specified port
    opINPUT_IIC_WRITE = 0xE3,  // Write I2C data to specified port
    opINPUT_SET_AUTOID = 0xE4,  // Enabled or disable auto-id for a specific sensor port
    opMAILBOX_SIZE = 0xE5,  // Returns the size of the mailbox.
    opFILE_MD5SUM = 0xE6,  // Get md5 sum of a file
    opDYNLOAD_VMLOAD = 0xF0,  // Load the selected VM
    opDYNLOAD_VMEXIT = 0xF1,  // Clean up the dynamic VM loading system
    opDYNLOAD_ENTRY_0 = 0xF2,  // Execute Entry Point function 0 in Third Party VM
    opDYNLOAD_ENTRY_1 = 0xF3,  // Execute Entry Point function 1 in Third Party VM
    opDYNLOAD_ENTRY_2 = 0xF4,  // Execute Entry Point function 2 in Third Party VM
    opDYNLOAD_ENTRY_3 = 0xF5,  // Execute Entry Point function 3 in Third Party VM
    opDYNLOAD_ENTRY_4 = 0xF6,  // Execute Entry Point function 4 in Third Party VM
    opDYNLOAD_ENTRY_5 = 0xF7,  // Execute Entry Point function 5 in Third Party VM
    opDYNLOAD_ENTRY_6 = 0xF8,  // Execute Entry Point function 6 in Third Party VM
    opDYNLOAD_ENTRY_7 = 0xF9,  // Execute Entry Point function 7 in Third Party VM
    opDYNLOAD_ENTRY_8 = 0xFA,  // Execute Entry Point function 8 in Third Party VM
    opDYNLOAD_ENTRY_9 = 0xFB,  // Execute Entry Point function 9 in Third Party VM
    opDYNLOAD_GET_VM = 0xFC,  // Get the index of the currently loaded VM
    opTST = 0xFF,  // System test functions entry
} OP;

// Subcodes

typedef enum { 
    scDESTROY = 0,
    scCREATE8 = 1,
    scCREATE16 = 2,
    scCREATE32 = 3,
    scCREATEF = 4,
    scRESIZE = 5,
    scFILL = 6,
    scCOPY = 7,
    scINIT8 = 8,
    scINIT16 = 9,
    scINIT32 = 10,
    scINITF = 11,
    scSET_SIZE = 12,
    scREAD_CONTENT = 13,
    scWRITE_CONTENT = 14,
    scREAD_SIZE = 15,
    ARRAY_SUBCODES
} ARRAY_SUBCODE;

typedef enum { 
    scGET_ON_OFF = 1,   // Get active state
    scGET_VISIBLE = 2,   // Get visibility state
    scGET_RESULT = 4,   // Get status.
    scGET_PIN = 5,   // Get pin code.
    scLIST_STATE = 7,   // Gets a list state value. This can be compared to previous values to determine if items are added or removed from a list since the last call.

    scSEARCH_ITEMS = 8,   // Get number of item from search.
    scSEARCH_ITEM = 9,   // Get search item informations.
    scFAVOUR_ITEMS = 10,   // Get no of item in favourite list.
    scFAVOUR_ITEM = 11,   // Get favourite item information.
    scGET_ID = 12,   // Get bluetooth address information
    scGET_BRICKNAME = 13,   // Gets the name of the brick
    scGET_NETWORK = 14,   // Gets the network information. WIFI only
    scGET_PRESENT = 15,   // Return if hardare is present. WIFI only
    scGET_ENCRYPT = 16,   // Returns the encryption mode of the hardware. WIFI only
    scCONNEC_ITEMS = 17,
    scCONNEC_ITEM = 18,
    scGET_INCOMING = 19,   // Returns the encryption mode of the hardware. WIFI only
    scGET_MODE2 = 20,
    COM_GET_SUBCODES
} COM_GET_SUBCODE;

typedef enum { 
    scCOMMAND = 14,
    COM_READ_SUBCODES
} COM_READ_SUBCODE;

typedef enum { 
    scSET_ON_OFF = 1,   // Set active state, either on or off
    scSET_VISIBLE = 2,   // Set visibility state - Only available for bluetooth
    scSET_SEARCH = 3,   // Starts or or stops the search for remote devices
    scSET_PIN = 5,   // Set pin code.
    scSET_PASSKEY = 6,   // Set pin code.
    scSET_CONNECTION = 7,   // Initiate or close the connection request to a remote device by the specified name.

    scSET_BRICKNAME = 8,   // Sets the name of the brick
    scSET_MOVEUP = 9,   // Moves the index in list one step up.
    scSET_MOVEDOWN = 10,   // Moves the index in list one step down.
    scSET_ENCRYPT = 11,   // Sets the encryption type for an item in a list.
    scSET_SSID = 12,   // Sets the SSID name. Only used for WIFI
    scSET_MODE2 = 13,   // Set active mode state, either active or not
    COM_SET_SUBCODES
} COM_SET_SUBCODE;

typedef enum { 
    scREPLY = 14,
    COM_WRITE_SUBCODES
} COM_WRITE_SUBCODE;

typedef enum { 
    scOPEN_APPEND = 0,   // Create file or open for append
    scOPEN_READ = 1,   // Open file for read
    scOPEN_WRITE = 2,   // Create file for write
    scREAD_VALUE = 3,   // Read floating point value from file
    scWRITE_VALUE = 4,   // Write floating point value to file
    scREAD_TEXT = 5,   // Read text from file
    scWRITE_TEXT = 6,   // Write text to file
    scCLOSE = 7,   // Close file
    scLOAD_IMAGE = 8,
    scGET_HANDLE = 9,   // Get handle from filename
    scMAKE_FOLDER = 10,   // Make folder if not present
    scGET_POOL = 11,
    scSET_LOG_SYNC_TIME = 12,
    scGET_FOLDERS = 13,
    scGET_LOG_SYNC_TIME = 14,
    scGET_SUBFOLDER_NAME = 15,
    scWRITE_LOG = 16,   // Write time slot samples to file
    scCLOSE_LOG = 17,   // Close data log file
    scGET_IMAGE = 18,
    scGET_ITEM = 19,
    scGET_CACHE_FILES = 20,
    scPUT_CACHE_FILE = 21,
    scGET_CACHE_FILE = 22,
    scDEL_CACHE_FILE = 23,
    scDEL_SUBFOLDER = 24,
    scGET_LOG_NAME = 25,   // Get the current open log filename
    scOPEN_LOG = 27,   // Create file for data logging
    scREAD_BYTES = 28,   // Read a number of bytes from file
    scWRITE_BYTES = 29,   // Write a number of bytes to file
    scREMOVE = 30,   // Delete file
    scMOVE = 31,   // Move file SOURCE to DEST
    FILE_SUBCODES
} FILE_SUBCODE;

typedef enum { 
    scEXIST = 16,   // Test if file exists
    scTOTALSIZE = 17,   // Calculate folder/file size
    scSPLIT = 18,   // Split filename into Folder, name, extension
    scMERGE = 19,   // Merge Folder, name, extension into filename
    scCHECK = 20,   // Check filename
    scPACK = 21,   // Pack file or folder into "raf" container
    scUNPACK = 22,   // Unpack "raf" container
    scGET_FOLDERNAME = 23,   // Get current folder name
    FILENAME_SUBCODES
} FILENAME_SUBCODE;

typedef enum { 
    scSET_ERROR = 1,
    scGET_ERROR = 2,
    scERRORTEXT = 3,   // Convert error number to text string
    scGET_VOLUME = 4,
    scSET_VOLUME = 5,
    scGET_MINUTES = 6,
    scSET_MINUTES = 7,
    INFO_SUBCODES
} INFO_SUBCODE;

typedef enum { 
    scINSERT_TYPE = 1,   // Insert type in table
    scGET_FORMAT = 2,
    scCAL_MINMAX = 3,   // Apply new minimum and maximum raw value for device type to be used in scaling PCT and SI

    scCAL_DEFAULT = 4,   // Apply the default minimum and maximum raw value for device type to be used in scaling PCT and SI

    scGET_TYPEMODE = 5,   // Get device type and mode
    scGET_SYMBOL = 6,
    scCAL_MIN = 7,   // Apply new minimum raw value for device type to be used in scaling PCT and SI

    scCAL_MAX = 8,   // Apply new maximum raw value for device type to be used in scaling PCT and SI

    scSETUP = 9,   // Generic setup/read IIC sensors
    scCLR_ALL = 10,   // Clear all devices (e.c. counters, angle, ...)
    scGET_RAW = 11,
    scGET_CONNECTION = 12,   // Get device connection type (for test)
    scSTOP_ALL = 13,   // Stop all devices (e.c. motors, ...)
    scSET_TYPEMODE = 14,   // Set new type and mode on existing devices
    scREADY_IIC = 15,   // Generic setup/read IIC sensors with result
    scGET_NAME = 21,
    scGET_MODENAME = 22,
    scSET_RAW = 23,
    scGET_FIGURES = 24,
    scGET_CHANGES = 25,
    scCLR_CHANGES = 26,   // Clear changes and bumps
    scREADY_PCT = 27,
    scREADY_RAW = 28,
    scREADY_SI = 29,
    scGET_MINMAX = 30,
    scGET_BUMPS = 31,
    INPUT_DEVICE_SUBCODES
} INPUT_DEVICE_SUBCODE;

typedef enum { 
    scEXP = 1,   // e^x            r = expf(x)
    scMOD = 2,   // Modulo         r = fmod(x,y)
    scFLOOR = 3,   // Floor          r = floor(x)
    scCEIL = 4,   // Ceiling        r = ceil(x)
    scROUND = 5,   // Round          r = round(x)
    scABS = 6,   // Absolute       r = fabs(x)
    scNEGATE = 7,   // Negate         r = 0.0 - x
    scSQRT = 8,   // Squareroot     r = sqrt(x)
    scLOG = 9,   // Log            r = log10(x)
    scLN = 10,   // Ln             r = log(x)
    scSIN = 11,   // Sin (R = sinf(X))
    scCOS = 12,   // Cos (R = cos(X))
    scTAN = 13,   // Tan (R = tanf(X))
    scASIN = 14,   // ASin (R = asinf(X))
    scACOS = 15,   // ACos (R = acos(X))
    scATAN = 16,   // ATan (R = atanf(X))
    scMOD8 = 17,   // Modulo DATA8   r = x % y
    scMOD16 = 18,   // Modulo DATA16  r = x % y
    scMOD32 = 19,   // Modulo DATA32  r = x % y
    scPOW = 20,   // Exponent       r = powf(x,y)
    scTRUNC = 21,   // Truncate       r = (float)((int)(x * pow(y))) / pow(y)
    MATH_SUBCODES
} MATH_SUBCODE;

typedef enum { 
    scOBJ_STOP = 0,
    scOBJ_START = 4,
    scGET_STATUS = 22,
    scGET_SPEED = 23,
    scGET_PRGRESULT = 24,
    scSET_INSTR = 25,   // Set number of instructions before VMThread change
    scGET_PRGNAME = 26,
    PROGRAM_INFO_SUBCODES
} PROGRAM_INFO_SUBCODE;

typedef enum { 
    scBREAK = 0,
    scTONE = 1,
    scPLAY = 2,
    scREPEAT = 3,
    scSERVICE = 4,
    SOUND_SUBCODES
} SOUND_SUBCODE;

typedef enum { 
    scGET_SIZE = 1,   // Get size of string (not including zero termination)
    scADD = 2,   // Add two strings (SOURCE1 + SOURCE2 -> DESTINATION)
    scCOMPARE = 3,   // Compare two strings
    scDUPLICATE = 5,   // Duplicate a string (SOURCE1 -> DESTINATION)
    scVALUE_TO_STRING = 6,   // Convert floating point value to a string (strips trailing zeroes)
    scSTRING_TO_VALUE = 7,
    scSTRIP = 8,   // Strip a string for spaces (SOURCE1 -> DESTINATION)
    scNUMBER_TO_STRING = 9,   // Convert integer value to a string
    scSUB = 10,   // Return DESTINATION: a substring from SOURCE1 that starts were SOURCE2 ends

    scVALUE_FORMATTED = 11,   // Convert floating point value to a formatted string
    scNUMBER_FORMATTED = 12,   // Convert integer number to a formatted string
    STRINGS_SUBCODES
} STRINGS_SUBCODE;

typedef enum { 
    scTST_OPEN = 10,   // Enables test byte codes for 10 seconds
    scTST_CLOSE = 11,   // Disables test byte codes
    scTST_READ_PINS = 12,   // Read connector pin status
    scTST_WRITE_PINS = 13,   // Write to connector pin
    scTST_READ_ADC = 14,   // Read raw count from ADC
    scTST_WRITE_UART = 15,   // Write data to port through UART
    scTST_READ_UART = 16,   // Read data from port through UART
    scTST_ENABLE_UART = 17,   // Enable all UARTs
    scTST_DISABLE_UART = 18,   // Disable all UARTs
    scTST_ACCU_SWITCH = 19,   // Read accu switch state
    scTST_BOOT_MODE2 = 20,   // Turn on mode2
    scTST_POLL_MODE2 = 21,   // Read mode2 status
    scTST_CLOSE_MODE2 = 22,   // Closes mode2
    scTST_RAM_CHECK = 23,   // Read RAM test status status
    TST_SUBCODES
} TST_SUBCODE;

typedef enum { 
    scSHORTPRESS = 1,
    scLONGPRESS = 2,
    scWAIT_FOR_PRESS = 3,
    scFLUSH = 4,
    scPRESS = 5,
    scRELEASE = 6,
    scGET_HORZ = 7,
    scGET_VERT = 8,
    scPRESSED = 9,
    scSET_BACK_BLOCK = 10,
    scGET_BACK_BLOCK = 11,
    scTESTSHORTPRESS = 12,
    scTESTLONGPRESS = 13,
    scGET_BUMPED = 14,
    scGET_CLICK = 15,   // Get and clear click sound request (internal use only)
    UI_BUTTON_SUBCODES
} UI_BUTTON_SUBCODE;

typedef enum { 
    scUPDATE = 0,
    scCLEAN = 1,
    scPIXEL = 2,
    scLINE = 3,
    scCIRCLE = 4,
    scTEXT = 5,
    scICON = 6,
    scPICTURE = 7,
    scVALUE = 8,
    scFILLRECT = 9,
    scRECTANGLE = 10,
    scNOTIFICATION = 11,
    scQUESTION = 12,
    scKEYBOARD = 13,
    scBROWSE = 14,
    scVERTBAR = 15,
    scINVERSERECT = 16,
    scSELECT_FONT = 17,
    scTOPLINE = 18,
    scFILLWINDOW = 19,
    scSCROLL = 20,
    scDOTLINE = 21,
    scVIEW_VALUE = 22,
    scVIEW_UNIT = 23,
    scFILLCIRCLE = 24,
    scSTORE = 25,
    scRESTORE = 26,
    scICON_QUESTION = 27,
    scBMPFILE = 28,
    scPOPUP = 29,
    scGRAPH_SETUP = 30,
    scGRAPH_DRAW = 31,
    scTEXTBOX = 32,
    UI_DRAW_SUBCODES
} UI_DRAW_SUBCODE;

typedef enum { 
    scGET_VBATT = 1,
    scGET_IBATT = 2,
    scGET_OS_VERS = 3,   // Get os version string
    scGET_EVENT = 4,
    scGET_TBATT = 5,
    scGET_IINT = 6,
    scGET_IMOTOR = 7,
    scGET_STRING = 8,   // Get string from terminal
    scGET_HW_VERS = 9,   // Get hardware version string
    scGET_FW_VERS = 10,   // Get firmware version string
    scGET_FW_BUILD = 11,   // Get firmware build string
    scGET_OS_BUILD = 12,   // Get os build string
    scGET_ADDRESS = 13,
    scGET_CODE = 14,
    scKEY = 15,
    scGET_SHUTDOWN = 16,
    scGET_WARNING = 17,
    scGET_LBATT = 18,   // Get battery level in %
    scTEXTBOX_READ = 21,
    scGET_VERSION = 26,   // Get version string
    scGET_IP = 27,   // Get IP address string
    scGET_POWER = 29,
    scGET_SDCARD = 30,
    scGET_USBSTICK = 31,
    UI_READ_SUBCODES
} UI_READ_SUBCODE;

typedef enum { 
    scWRITE_FLUSH = 1,
    scFLOATVALUE = 2,
    scSTAMP = 3,
    scPUT_STRING = 8,
    scVALUE8 = 9,
    scVALUE16 = 10,
    scVALUE32 = 11,
    scVALUEF = 12,
    scADDRESS = 13,
    scCODE = 14,
    scDOWNLOAD_END = 15,   // Send to brick when file down load is completed (plays sound and updates the UI browser)

    scSCREEN_BLOCK = 16,   // Set or clear screen block status (if screen blocked - all graphical screen action are disabled)

    scALLOW_PULSE = 17,
    scSET_PULSE = 18,
    scTEXTBOX_APPEND = 21,   // Append line of text at the bottom of a text box
    scSET_BUSY = 22,
    scSET_TESTPIN = 24,
    scINIT_RUN = 25,   // Start the "Mindstorms" "run" screen
    scUPDATE_RUN = 26,
    scLED = 27,
    scPOWER = 29,
    scGRAPH_SAMPLE = 30,   // Update tick to scroll graph horizontally in memory when drawing graph in "scope" mode

    scTERMINAL = 31,
    UI_WRITE_SUBCODES
} UI_WRITE_SUBCODE;


// enums

typedef enum { 
    ICON_LEFT = 1,
    ICON_RIGHT = 2,
    A_ICON_NOS = 3,
} A_ICON_NO;

typedef enum { 
    BROWSE_FOLDERS = 0,  // Browser for folders
    BROWSE_FOLDS_FILES = 1,  // Browser for folders and files
    BROWSE_CACHE = 2,  // Browser for cached / recent files
    BROWSE_FILES = 3,  // Browser for files
    BROWSERTYPES = 4,  // Maximum browser types supported by the VM
} BROWSERTYPE;

typedef enum { 
    BTTYPE_PC = 3,  // Bluetooth type PC
    BTTYPE_PHONE = 4,  // Bluetooth type PHONE
    BTTYPE_BRICK = 5,  // Bluetooth type BRICK
    BTTYPE_UNKNOWN = 6,  // Bluetooth type UNKNOWN
    BTTYPES = 7,
} BTTYPE;

typedef enum { 
    NO_BUTTON = 0,
    UP_BUTTON = 1,
    ENTER_BUTTON = 2,
    DOWN_BUTTON = 3,
    RIGHT_BUTTON = 4,
    LEFT_BUTTON = 5,
    BACK_BUTTON = 6,
    ANY_BUTTON = 7,
    BUTTONTYPES = 8,
} BUTTONTYPE;

typedef enum { 
    RED = 0,
    GREEN = 1,
    BLUE = 2,
    BLANK = 3,
    COLORS = 4,
} COLOR;

typedef enum { 
    DATA_8 = 0,  // DATA8
    DATA_16 = 1,  // DATA16
    DATA_32 = 2,  // DATA32
    DATA_F = 3,  // DATAF
    DATA_S = 4,  // Zero terminated string
    DATA_A = 5,  // Array handle
    DATA_V = 7,  // Variable type
    DATA_PCT = 16,  // Percent
    DATA_RAW = 18,  // Raw
    DATA_SI = 19,  // SI unit
    DATA_FORMATS = 20,
} DATA_FORMAT;

typedef enum { 
    DEL_NONE = 0,  // No delimiter at all
    DEL_TAB = 1,  // Use tab as delimiter
    DEL_SPACE = 2,  // Use space as delimiter
    DEL_RETURN = 3,  // Use return as delimiter
    DEL_COLON = 4,  // Use colon as delimiter
    DEL_COMMA = 5,  // Use comma as delimiter
    DEL_LINEFEED = 6,  // Use line feed as delimiter
    DEL_CRLF = 7,  // Use return+line feed as delimiter
    DELS = 8,
} DEL;

typedef enum { 
    DEVCMD_RESET = 17,  // UART device reset
    DEVCMD_CHANNEL = 18,  // UART device channel (IR seeker)
    DEVCMDS = 19,
} DEVCMD;

typedef enum { 
    ENCRYPT_NONE = 0,
    ENCRYPT_WPA2 = 1,
    ENCRYPTS = 2,
} ENCRYPT;

typedef enum { 
    TYPE_REFRESH_BROWSER = -2,
    TYPE_RESTART_BROWSER = -1,
    FILETYPE_UNKNOWN = 0,
    TYPE_FOLDER = 1,
    TYPE_SOUND = 2,
    TYPE_BYTECODE = 3,
    TYPE_GRAPHICS = 4,
    TYPE_DATALOG = 5,
    TYPE_PROGRAM = 6,
    TYPE_TEXT = 7,
    TYPE_SDCARD = 16,
    TYPE_USBSTICK = 32,
    FILETYPES = 33,
} FILETYPE;

typedef enum { 
    NORMAL_FONT = 0,
    SMALL_FONT = 1,
    LARGE_FONT = 2,
    TINY_FONT = 3,
    FONTTYPES = 4,  // Maximum font types supported by the VM
} FONTTYPE;

typedef enum { 
    HW_USB = 1,
    HW_BT = 2,
    HW_WIFI = 3,
    HWTYPES = 4,
} HWTYPE;

typedef enum { 
    NORMAL_ICON = 0,  // 24x12_Files_Folders_Settings.bmp
    SMALL_ICON = 1,
    LARGE_ICON = 2,  // 24x22_Yes_No_OFF_FILEOps.bmp
    MENU_ICON = 3,
    ARROW_ICON = 4,  // 8x12_miniArrows.bmp
    ICONTYPES = 5,  // Maximum icon types supported by the VM
} ICONTYPE;

typedef enum { 
    LED_BLACK = 0,
    LED_GREEN = 1,
    LED_RED = 2,
    LED_ORANGE = 3,
    LED_GREEN_FLASH = 4,
    LED_RED_FLASH = 5,
    LED_ORANGE_FLASH = 6,
    LED_GREEN_PULSE = 7,
    LED_RED_PULSE = 8,
    LED_ORANGE_PULSE = 9,
    LEDPATTERNS = 10,
} LEDPATTERN;

typedef enum { 
    LED_ALL = 0,  // All LEDs
    LED_RR = 1,  // Right red
    LED_RG = 2,  // Right green
    LED_LR = 3,  // Left red
    LED_LG = 4,  // Left green
} LEDTYPE;

typedef enum { 
    YES_NOTSEL = 0,
    YES_SEL = 1,
    NO_NOTSEL = 2,
    NO_SEL = 3,
    OFF = 4,
    WAIT_VERT = 5,
    WAIT_HORZ = 6,
    TO_MANUAL = 7,
    WARNSIGN = 8,
    WARN_BATT = 9,
    WARN_POWER = 10,
    WARN_TEMP = 11,
    NO_USBSTICK = 12,
    TO_EXECUTE = 13,
    TO_BRICK = 14,
    TO_SDCARD = 15,
    TO_USBSTICK = 16,
    TO_BLUETOOTH = 17,
    TO_WIFI = 18,
    TO_TRASH = 19,
    TO_COPY = 20,
    TO_FILE = 21,
    CHAR_ERROR = 22,
    COPY_ERROR = 23,
    PROGRAM_ERROR = 24,
    WARN_MEMORY = 27,
    L_ICON_NOS = 28,
} L_ICON_NO;

typedef enum { 
    ICON_STAR = 0,
    ICON_LOCKSTAR = 1,
    ICON_LOCK = 2,
    ICON_PC = 3,  // Bluetooth type PC
    ICON_PHONE = 4,  // Bluetooth type PHONE
    ICON_BRICK = 5,  // Bluetooth type BRICK
    ICON_UNKNOWN = 6,  // Bluetooth type UNKNOWN
    ICON_FROM_FOLDER = 7,
    ICON_CHECKBOX = 8,
    ICON_CHECKED = 9,
    ICON_XED = 10,
    M_ICON_NOS = 11,
} M_ICON_NO;

typedef enum { 
    BLACKCOLOR = 1,
    BLUECOLOR = 2,
    GREENCOLOR = 3,
    YELLOWCOLOR = 4,
    REDCOLOR = 5,
    WHITECOLOR = 6,
} NXTCOLOR;

typedef enum { 
    ICON_NONE = -1,
    ICON_RUN = 0,
    ICON_FOLDER = 1,
    ICON_FOLDER2 = 2,
    ICON_USB = 3,
    ICON_SD = 4,
    ICON_SOUND = 5,
    ICON_IMAGE = 6,
    ICON_SETTINGS = 7,
    ICON_ONOFF = 8,
    ICON_SEARCH = 9,
    ICON_WIFI = 10,
    ICON_CONNECTIONS = 11,
    ICON_ADD_HIDDEN = 12,
    ICON_TRASHBIN = 13,
    ICON_VISIBILITY = 14,
    ICON_KEY = 15,
    ICON_CONNECT = 16,
    ICON_DISCONNECT = 17,
    ICON_UP = 18,
    ICON_DOWN = 19,
    ICON_WAIT1 = 20,
    ICON_WAIT2 = 21,
    ICON_BLUETOOTH = 22,
    ICON_INFO = 23,
    ICON_TEXT = 24,
    ICON_QUESTIONMARK = 27,
    ICON_INFO_FILE = 28,
    ICON_DISC = 29,
    ICON_CONNECTED = 30,
    ICON_OBP = 31,
    ICON_OBD = 32,
    ICON_OPENFOLDER = 33,
    ICON_BRICK1 = 34,
    N_ICON_NOS = 35,
} N_ICON_NO;

typedef enum { 
    RUNNING = 16,  // Object code is running
    WAITING = 32,  // Object is waiting for final trigger
    STOPPED = 64,  // Object is stopped or not triggered yet
    HALTED = 128,  // Object is halted because a call is in progress
} OBJSTAT;

typedef enum { 
    OK = 0,  // No errors to report
    BUSY = 1,  // Busy - try again
    FAIL = 2,  // Something failed
    STOP = 4,  // Stopped
    START = 8,  // Start
} RESULT;

typedef enum { 
    CURRENT_SLOT = -1,
    GUI_SLOT = 0,  // Program slot reserved for executing the user interface
    USER_SLOT = 1,  // Program slot used to execute user projects, apps and tools
    CMD_SLOT = 2,  // Program slot used for direct commands coming from c_com
    TERM_SLOT = 3,  // Program slot used for direct commands coming from c_ui
    DEBUG_SLOT = 4,  // Program slot used to run the debug ui
    SLOTS = 5,  // Maximum slots supported by the VM
} SLOT;

typedef enum { 
    SICON_CHARGING = 0,
    SICON_BATT_4 = 1,
    SICON_BATT_3 = 2,
    SICON_BATT_2 = 3,
    SICON_BATT_1 = 4,
    SICON_BATT_0 = 5,
    SICON_WAIT1 = 6,
    SICON_WAIT2 = 7,
    SICON_BT_ON = 8,
    SICON_BT_VISIBLE = 9,
    SICON_BT_CONNECTED = 10,
    SICON_BT_CONNVISIB = 11,
    SICON_WIFI_3 = 12,
    SICON_WIFI_2 = 13,
    SICON_WIFI_1 = 14,
    SICON_WIFI_CONNECTED = 15,
    SICON_USB = 21,
    S_ICON_NOS = 22,
} S_ICON_NO;

typedef enum { 
    MODE_KEEP = -1,  // Mode value that won't change mode in byte codes (convenient place to define)
    TYPE_KEEP = 0,  // Type value that won't change type in byte codes
    TYPE_NXT_TOUCH = 1,  // Device is NXT touch sensor
    TYPE_NXT_LIGHT = 2,  // Device is NXT light sensor
    TYPE_NXT_SOUND = 3,  // Device is NXT sound sensor
    TYPE_NXT_COLOR = 4,  // Device is NXT color sensor
    TYPE_NXT_ULTRASONIC = 5,  // Device is NXT ultra sonic sensor
    TYPE_NXT_TEMPERATURE = 6,  // Device is NXT temperature sensor
    TYPE_TACHO = 7,  // Device is EV3/NXT tacho motor
    TYPE_MINITACHO = 8,  // Device is EV3 mini tacho motor
    TYPE_NEWTACHO = 9,  // Device is EV3 new tacho motor
    TYPE_TOUCH = 16,  // Device is EV3 touch sensor
    TYPE_COLOR = 29,  // Device is EV3 color sensor
    TYPE_ULTRASONIC = 30,  // Device is EV3 ultra sonic sensor
    TYPE_GYRO = 32,  // Device is EV3 gyro sensor
    TYPE_IR = 33,  // Device is EV3 IR sensor
    TYPE_THIRD_PARTY_START = 50,
    TYPE_THIRD_PARTY_END = 98,
    TYPE_ENERGYMETER = 99,  // Device is energy meter
    TYPE_IIC_UNKNOWN = 100,  // Device type is not known yet
    TYPE_NXT_TEST = 101,  // Device is a NXT ADC test sensor
    TYPE_NXT_IIC = 123,  // Device is NXT IIC sensor
    TYPE_TERMINAL = 124,  // Port is connected to a terminal
    TYPE_UNKNOWN = 125,  // Port not empty but type has not been determined
    TYPE_NONE = 126,  // Port empty or not available
    TYPE_ERROR = 127,  // Port not empty and type is invalid
} TYPE;

typedef enum { 
    WARNING_TEMP = 1,
    WARNING_CURRENT = 2,
    WARNING_VOLTAGE = 4,
    WARNING_MEMORY = 8,
    WARNING_DSPSTAT = 16,
    WARNING_RAM = 32,
    WARNINGS = 63,
    WARNING_BATTLOW = 64,
    WARNING_BUSY = 128,
} WARNING;


// internal defines

#define   DATA8_NAN     ((DATA8)(-128))
#define   DATA16_NAN    ((DATA16)(-32768))
#define   DATA32_NAN    ((DATA32)(0x80000000))
#define   DATAF_NAN     ((float)0 / (float)0) //(0x7FC00000)

#define   DATA8_MIN     vmDATA8_MIN
#define   DATA8_MAX     vmDATA8_MAX
#define   DATA16_MIN    vmDATA16_MIN
#define   DATA16_MAX    vmDATA16_MAX
#define   DATA32_MIN    vmDATA32_MIN
#define   DATA32_MAX    vmDATA32_MAX
#define   DATAF_MIN     vmDATAF_MIN
#define   DATAF_MAX     vmDATAF_MAX

#define   LONGToBytes(_x)               (UBYTE)((_x) & 0xFF),(UBYTE)(((_x) >> 8) & 0xFF),(UBYTE)(((_x) >> 16) & 0xFF),(UBYTE)(((_x) >> 24) & 0xFF)
#define   WORDToBytes(_x)               (UBYTE)((_x) & 0xFF),(UBYTE)(((_x) >> 8) & 0xFF)
#define   BYTEToBytes(_x)               (UBYTE)((_x) & 0xFF)

#define   PROGRAMHeader(VersionInfo,NumberOfObjects,GlobalBytes)\
                                        'L','E','G','O',LONGToBytes(0),WORDToBytes((UWORD)(BYTECODE_VERSION * 100.0)),WORDToBytes(NumberOfObjects),LONGToBytes(GlobalBytes)

#define   VMTHREADHeader(OffsetToInstructions,LocalBytes)\
                                        LONGToBytes(OffsetToInstructions),0,0,0,0,LONGToBytes(LocalBytes)

#define   SUBCALLHeader(OffsetToInstructions,LocalBytes)\
                                        LONGToBytes(OffsetToInstructions),0,0,1,0,LONGToBytes(LocalBytes)

#define   BLOCKHeader(OffsetToInstructions,OwnerObjectId,TriggerCount)\
                                        LONGToBytes(OffsetToInstructions),WORDToBytes(OwnerObjectId),WORDToBytes(TriggerCount),LONGToBytes(0)

//        MACROS FOR PRIMITIVES AND SYSTEM CALLS

#define   PRIMPAR_SHORT                 0x00
#define   PRIMPAR_LONG                  0x80

#define   PRIMPAR_CONST                 0x00
#define   PRIMPAR_VARIABLE              0x40
#define   PRIMPAR_LOCAL                 0x00
#define   PRIMPAR_GLOBAL                0x20
#define   PRIMPAR_HANDLE                0x10
#define   PRIMPAR_ADDR                  0x08

#define   PRIMPAR_INDEX                 0x1F
#define   PRIMPAR_CONST_SIGN            0x20
#define   PRIMPAR_VALUE                 0x3F

#define   PRIMPAR_BYTES                 0x07

#define   PRIMPAR_STRING_OLD            0
#define   PRIMPAR_1_BYTE                1
#define   PRIMPAR_2_BYTES               2
#define   PRIMPAR_4_BYTES               3
#define   PRIMPAR_STRING                4

#define   PRIMPAR_LABEL                 0x20

#define   HND(x)                        (PRIMPAR_HANDLE | (x))
#define   ADR(x)                        (PRIMPAR_ADDR | (x))

#define   LCS                           (PRIMPAR_LONG | PRIMPAR_STRING)

#define   LAB1(v)                       (PRIMPAR_LONG | PRIMPAR_LABEL),((v) & 0xFF)

#define   LC0(v)                        (((v) & PRIMPAR_VALUE) | PRIMPAR_SHORT | PRIMPAR_CONST)
#define   LC1(v)                        (PRIMPAR_LONG  | PRIMPAR_CONST | PRIMPAR_1_BYTE),((v) & 0xFF)
#define   LC2(v)                        (PRIMPAR_LONG  | PRIMPAR_CONST | PRIMPAR_2_BYTES),((v) & 0xFF),(((v) >> 8) & 0xFF)
#define   LC4(v)                        (PRIMPAR_LONG  | PRIMPAR_CONST | PRIMPAR_4_BYTES),((ULONG)(v) & 0xFF),(((ULONG)(v) >> (ULONG)8) & 0xFF),(((ULONG)(v) >> (ULONG)16) & 0xFF),(((ULONG)(v) >> (ULONG)24) & 0xFF)

#define   LV0(i)                        (((i) & PRIMPAR_INDEX) | PRIMPAR_SHORT | PRIMPAR_VARIABLE | PRIMPAR_LOCAL)
#define   LV1(i)                        (PRIMPAR_LONG  | PRIMPAR_VARIABLE | PRIMPAR_LOCAL | PRIMPAR_1_BYTE),((i) & 0xFF)
#define   LV2(i)                        (PRIMPAR_LONG  | PRIMPAR_VARIABLE | PRIMPAR_LOCAL | PRIMPAR_2_BYTES),((i) & 0xFF),(((i) >> 8) & 0xFF)
#define   LV4(i)                        (PRIMPAR_LONG  | PRIMPAR_VARIABLE | PRIMPAR_LOCAL | PRIMPAR_4_BYTES),((i) & 0xFF),(((i) >> 8) & 0xFF),(((i) >> 16) & 0xFF),(((i) >> 24) & 0xFF)

#define   GV0(i)                        (((i) & PRIMPAR_INDEX) | PRIMPAR_SHORT | PRIMPAR_VARIABLE | PRIMPAR_GLOBAL)
#define   GV1(i)                        (PRIMPAR_LONG  | PRIMPAR_VARIABLE | PRIMPAR_GLOBAL | PRIMPAR_1_BYTE),((i) & 0xFF)
#define   GV2(i)                        (PRIMPAR_LONG  | PRIMPAR_VARIABLE | PRIMPAR_GLOBAL | PRIMPAR_2_BYTES),((i) & 0xFF),(((i) >> 8) & 0xFF)
#define   GV4(i)                        (PRIMPAR_LONG  | PRIMPAR_VARIABLE | PRIMPAR_GLOBAL | PRIMPAR_4_BYTES),((i) & 0xFF),(((i) >> 8) & 0xFF),(((i) >> 16) & 0xFF),(((i) >> 24) & 0xFF)

//        MACROS FOR SUB CALLS


#define   CALLPAR_IN                    0x80
#define   CALLPAR_OUT                   0x40

#define   CALLPAR_TYPE                  0x07
#define   CALLPAR_DATA8                 DATA_8
#define   CALLPAR_DATA16                DATA_16
#define   CALLPAR_DATA32                DATA_32
#define   CALLPAR_DATAF                 DATA_F
#define   CALLPAR_STRING                DATA_S

#define   IN_8                          (CALLPAR_IN  | CALLPAR_DATA8)
#define   IN_16                         (CALLPAR_IN  | CALLPAR_DATA16)
#define   IN_32                         (CALLPAR_IN  | CALLPAR_DATA32)
#define   IN_F                          (CALLPAR_IN  | CALLPAR_DATAF)
#define   IN_S                          (CALLPAR_IN  | CALLPAR_STRING)
#define   OUT_8                         (CALLPAR_OUT | CALLPAR_DATA8)
#define   OUT_16                        (CALLPAR_OUT | CALLPAR_DATA16)
#define   OUT_32                        (CALLPAR_OUT | CALLPAR_DATA32)
#define   OUT_F                         (CALLPAR_OUT | CALLPAR_DATAF)
#define   OUT_S                         (CALLPAR_OUT | CALLPAR_STRING)

#define   IO_8                          (IN_8  | OUT_8)
#define   IO_16                         (IN_16 | OUT_16)
#define   IO_32                         (IN_32 | OUT_32)
#define   IO_F                          (IN_F  | OUT_F)
#define   IO_S                          (IN_S  | OUT_S)

#define   IN_OUT_8                      IO_8
#define   IN_OUT_16                     IO_16
#define   IN_OUT_32                     IO_32
#define   IN_OUT_F                      IO_F
#define   IN_OUT_S                      IO_S

#endif /* BYTECODES_H_ */
