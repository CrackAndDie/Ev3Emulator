namespace Ev3LowLevelLib
{
	public enum Op
	{
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
	}
}
