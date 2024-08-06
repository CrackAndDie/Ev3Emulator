namespace Ev3CoreUnsafe.Enums
{
	public enum OP
	{
		//  \endverbatim \ref VM \verbatim
		//                                        0000....
		opERROR = 0x00, //      0000
		opNOP = 0x01, //      0001
		opPROGRAM_STOP = 0x02, //      0010
		opPROGRAM_START = 0x03, //      0011
		opOBJECT_STOP = 0x04, //      0100
		opOBJECT_START = 0x05, //      0101
		opOBJECT_TRIG = 0x06, //      0110
		opOBJECT_WAIT = 0x07, //      0111
		opRETURN = 0x08, //      1000
		opCALL = 0x09, //      1001
		opOBJECT_END = 0x0A, //      1010
		opSLEEP = 0x0B, //      1011
		opPROGRAM_INFO = 0x0C, //      1100
		opLABEL = 0x0D, //      1101
		opPROBE = 0x0E, //      1110
		opDO = 0x0F, //      1111

		//  \endverbatim \ref cMath "MATH" \verbatim
		//                                        0001....
		//                    ADD                     00..
		opADD8 = 0x10, //        00
		opADD16 = 0x11, //        01
		opADD32 = 0x12, //        10
		opADDF = 0x13, //        11
					   //                    SUB                     01..
		opSUB8 = 0x14, //        00
		opSUB16 = 0x15, //        01
		opSUB32 = 0x16, //        10
		opSUBF = 0x17, //        11
					   //                    MUL                     10..
		opMUL8 = 0x18, //        00
		opMUL16 = 0x19, //        01
		opMUL32 = 0x1A, //        10
		opMULF = 0x1B, //        11
					   //                    DIV                     11..
		opDIV8 = 0x1C, //        00
		opDIV16 = 0x1D, //        01
		opDIV32 = 0x1E, //        10
		opDIVF = 0x1F, //        11

		//  \endverbatim \ref Logic "LOGIC" \verbatim
		//        LOGIC                           0010....
		//                    OR                      00..
		opOR8 = 0x20, //        00
		opOR16 = 0x21, //        01
		opOR32 = 0x22, //        10

		//                    AND                     01..
		opAND8 = 0x24, //        00
		opAND16 = 0x25, //        01
		opAND32 = 0x26, //        10

		//                    XOR                     10..
		opXOR8 = 0x28, //        00
		opXOR16 = 0x29, //        01
		opXOR32 = 0x2A, //        10

		//                    RL                      11..
		opRL8 = 0x2C, //        00
		opRL16 = 0x2D, //        01
		opRL32 = 0x2E, //        10

		//  \endverbatim \ref cMove "MOVE" \verbatim
		opINIT_BYTES = 0x2F, //      1111
							 //        MOVE                            0011....
							 //                    MOVE8_                  00..
		opMOVE8_8 = 0x30, //        00
		opMOVE8_16 = 0x31, //        01
		opMOVE8_32 = 0x32, //        10
		opMOVE8_F = 0x33, //        11
						  //                    MOVE16_                 01..
		opMOVE16_8 = 0x34, //        00
		opMOVE16_16 = 0x35, //        01
		opMOVE16_32 = 0x36, //        10
		opMOVE16_F = 0x37, //        11
						   //                    MOVE32_                 10..
		opMOVE32_8 = 0x38, //        00
		opMOVE32_16 = 0x39, //        01
		opMOVE32_32 = 0x3A, //        10
		opMOVE32_F = 0x3B, //        11
						   //                    MOVEF_                  11..
		opMOVEF_8 = 0x3C, //        00
		opMOVEF_16 = 0x3D, //        01
		opMOVEF_32 = 0x3E, //        10
		opMOVEF_F = 0x3F, //        11

		//  \endverbatim \ref cBranch "BRANCH" \verbatim
		//        BRANCH                          010000..
		opJR = 0x40, //        00
		opJR_FALSE = 0x41, //        01
		opJR_TRUE = 0x42, //        10
		opJR_NAN = 0x43, //        11

		//  \endverbatim \ref cCompare "COMPARE" \verbatim
		//        COMPARE                         010.....
		//                    CP_LT                  001..
		opCP_LT8 = 0x44, //        00
		opCP_LT16 = 0x45, //        01
		opCP_LT32 = 0x46, //        10
		opCP_LTF = 0x47, //        11
						 //                    CP_GT                  010..
		opCP_GT8 = 0x48, //        00
		opCP_GT16 = 0x49, //        01
		opCP_GT32 = 0x4A, //        10
		opCP_GTF = 0x4B, //        11
						 //                    CP_EQ                  011..
		opCP_EQ8 = 0x4C, //        00
		opCP_EQ16 = 0x4D, //        01
		opCP_EQ32 = 0x4E, //        10
		opCP_EQF = 0x4F, //        11
						 //                    CP_NEQ                 100..
		opCP_NEQ8 = 0x50, //        00
		opCP_NEQ16 = 0x51, //        01
		opCP_NEQ32 = 0x52, //        10
		opCP_NEQF = 0x53, //        11
						  //                    CP_LTEQ                101..
		opCP_LTEQ8 = 0x54, //        00
		opCP_LTEQ16 = 0x55, //        01
		opCP_LTEQ32 = 0x56, //        10
		opCP_LTEQF = 0x57, //        11
						   //                    CP_GTEQ                110..
		opCP_GTEQ8 = 0x58, //        00
		opCP_GTEQ16 = 0x59, //        01
		opCP_GTEQ32 = 0x5A, //        10
		opCP_GTEQF = 0x5B, //        11

		//  \endverbatim \ref Select "SELECT" \verbatim
		//        SELECT                          010111..
		opSELECT8 = 0x5C, //        00
		opSELECT16 = 0x5D, //        01
		opSELECT32 = 0x5E, //        10
		opSELECTF = 0x5F, //        11


		//  \endverbatim \ref VM \verbatim
		opSYSTEM = 0x60,
		opPORT_CNV_OUTPUT = 0x61,
		opPORT_CNV_INPUT = 0x62,
		opNOTE_TO_FREQ = 0x63,

		//  \endverbatim \ref cBranch "BRANCH" \verbatim
		//        BRANCH                          011000..
		//?       00
		//?       01
		//?       10
		//?       11
		//                    JR_LT                  001..
		opJR_LT8 = 0x64, //        00
		opJR_LT16 = 0x65, //        01
		opJR_LT32 = 0x66, //        10
		opJR_LTF = 0x67, //        11
						 //                    JR_GT                  010..
		opJR_GT8 = 0x68, //        00
		opJR_GT16 = 0x69, //        01
		opJR_GT32 = 0x6A, //        10
		opJR_GTF = 0x6B, //        11
						 //                    JR_EQ                  011..
		opJR_EQ8 = 0x6C, //        00
		opJR_EQ16 = 0x6D, //        01
		opJR_EQ32 = 0x6E, //        10
		opJR_EQF = 0x6F, //        11
						 //                    JR_NEQ                 100..
		opJR_NEQ8 = 0x70, //        00
		opJR_NEQ16 = 0x71, //        01
		opJR_NEQ32 = 0x72, //        10
		opJR_NEQF = 0x73, //        11
						  //                    JR_LTEQ                101..
		opJR_LTEQ8 = 0x74, //        00
		opJR_LTEQ16 = 0x75, //        01
		opJR_LTEQ32 = 0x76, //        10
		opJR_LTEQF = 0x77, //        11
						   //                    JR_GTEQ                110..
		opJR_GTEQ8 = 0x78, //        00
		opJR_GTEQ16 = 0x79, //        01
		opJR_GTEQ32 = 0x7A, //        10
		opJR_GTEQF = 0x7B, //        11

		//  \endverbatim \ref VM \verbatim
		opINFO = 0x7C, //  01111100
		opSTRINGS = 0x7D, //  01111101
		opMEMORY_WRITE = 0x7E, //  01111110
		opMEMORY_READ = 0x7F, //  01111111

		//        SYSTEM                          1.......

		//  \endverbatim \ref cUi "UI" \verbatim
		//        UI                              100000..
		opUI_FLUSH = 0x80, //        00
		opUI_READ = 0x81, //        01
		opUI_WRITE = 0x82, //        10
		opUI_BUTTON = 0x83, //        11
		opUI_DRAW = 0x84, //  10000100

		//  \endverbatim \ref cTimer "TIMER" \verbatim
		opTIMER_WAIT = 0x85, //  10000101
		opTIMER_READY = 0x86, //  10000110
		opTIMER_READ = 0x87, //  10000111

		//  \endverbatim \ref VM \verbatim
		//        BREAKPOINT                      10001...
		opBP0 = 0x88, //       000
		opBP1 = 0x89, //       001
		opBP2 = 0x8A, //       010
		opBP3 = 0x8B, //       011
		opBP_SET = 0x8C, //  10001100
		opMATH = 0x8D, //  10001101
		opRANDOM = 0x8E, //  10001110

		//  \endverbatim \ref cTimer "TIMER" \verbatim
		opTIMER_READ_US = 0x8F, //  10001111

		//  \endverbatim \ref cUi "UI" \verbatim
		opKEEP_ALIVE = 0x90, //  10010000

		//  \endverbatim \ref cCom "COM" \verbatim
		//                                        100100
		opCOM_READ = 0x91, //        01
		opCOM_WRITE = 0x92, //        10

		//  \endverbatim \ref cSound "SOUND" \verbatim
		//                                        100101
		opSOUND = 0x94, //        00
		opSOUND_TEST = 0x95, //        01
		opSOUND_READY = 0x96, //        10

		//  \endverbatim \ref cInput "INPUT" \verbatim
		//
		opINPUT_SAMPLE = 0x97, //  10010111

		//                                        10011...
		opINPUT_DEVICE_LIST = 0x98, //       000
		opINPUT_DEVICE = 0x99, //       001
		opINPUT_READ = 0x9A, //       010
		opINPUT_TEST = 0x9B, //       011
		opINPUT_READY = 0x9C, //       100
		opINPUT_READSI = 0x9D, //       101
		opINPUT_READEXT = 0x9E, //       110
		opINPUT_WRITE = 0x9F, //       111
							  //  \endverbatim \ref cOutput "OUTPUT" \verbatim
							  //                                        101.....
		opOUTPUT_GET_TYPE = 0xA0, //     00000
		opOUTPUT_SET_TYPE = 0xA1, //     00001
		opOUTPUT_RESET = 0xA2, //     00010
		opOUTPUT_STOP = 0xA3, //     00011
		opOUTPUT_POWER = 0xA4, //     00100
		opOUTPUT_SPEED = 0xA5, //     00101
		opOUTPUT_START = 0xA6, //     00110
		opOUTPUT_POLARITY = 0xA7, //     00111
		opOUTPUT_READ = 0xA8, //     01000
		opOUTPUT_TEST = 0xA9, //     01001
		opOUTPUT_READY = 0xAA, //     01010
		opOUTPUT_POSITION = 0xAB, //     01011
		opOUTPUT_STEP_POWER = 0xAC, //     01100
		opOUTPUT_TIME_POWER = 0xAD, //     01101
		opOUTPUT_STEP_SPEED = 0xAE, //     01110
		opOUTPUT_TIME_SPEED = 0xAF, //     01111

		opOUTPUT_STEP_SYNC = 0xB0, //     10000
		opOUTPUT_TIME_SYNC = 0xB1, //     10001
		opOUTPUT_CLR_COUNT = 0xB2, //     10010
		opOUTPUT_GET_COUNT = 0xB3, //     10011

		opOUTPUT_PRG_STOP = 0xB4, //     10100

		//  \endverbatim \ref cMemory "MEMORY" \verbatim
		//                                        11000...
		opFILE = 0xC0, //       000
		opARRAY = 0xC1, //       001
		opARRAY_WRITE = 0xC2, //       010
		opARRAY_READ = 0xC3, //       011
		opARRAY_APPEND = 0xC4, //       100
		opMEMORY_USAGE = 0xC5, //       101
		opFILENAME = 0xC6, //       110

		//  \endverbatim \ref cMove "READ" \verbatim
		//                                        110010..
		opREAD8 = 0xC8, //        00
		opREAD16 = 0xC9, //        01
		opREAD32 = 0xCA, //        10
		opREADF = 0xCB, //        11

		//  \endverbatim \ref cMove "WRITE" \verbatim
		//                                        110011..
		opWRITE8 = 0xCC, //        00
		opWRITE16 = 0xCD, //        01
		opWRITE32 = 0xCE, //        10
		opWRITEF = 0xCF, //        11

		//  \endverbatim \ref cCom "COM" \verbatim
		//                                        11010...
		opCOM_READY = 0xD0, //       000
		opCOM_READDATA = 0xD1, //       001
		opCOM_WRITEDATA = 0xD2, //       010
		opCOM_GET = 0xD3, //       011
		opCOM_SET = 0xD4, //       100
		opCOM_TEST = 0xD5, //       101
		opCOM_REMOVE = 0xD6, //       110
		opCOM_WRITEFILE = 0xD7, //       111

		//                                        11011...
		opMAILBOX_OPEN = 0xD8, //       000
		opMAILBOX_WRITE = 0xD9, //       001
		opMAILBOX_READ = 0xDA, //       010
		opMAILBOX_TEST = 0xDB, //       011
		opMAILBOX_READY = 0xDC, //       100
		opMAILBOX_CLOSE = 0xDD, //       101

		//        SPARE                           111.....

		//  \endverbatim \ref TST \verbatim
		opTST = 0xFF  //  11111111
	}
}

namespace Ev3CoreUnsafe
{
	public partial class Defines
	{
		//  \endverbatim \ref VM \verbatim
		//                                        0000....
		public const int opERROR = 0x00; //      0000
		public const int opNOP = 0x01; //      0001
		public const int opPROGRAM_STOP = 0x02; //      0010
		public const int opPROGRAM_START = 0x03; //      0011
		public const int opOBJECT_STOP = 0x04; //      0100
		public const int opOBJECT_START = 0x05; //      0101
		public const int opOBJECT_TRIG = 0x06; //      0110
		public const int opOBJECT_WAIT = 0x07; //      0111
		public const int opRETURN = 0x08; //      1000
		public const int opCALL = 0x09; //      1001
		public const int opOBJECT_END = 0x0A; //      1010
		public const int opSLEEP = 0x0B; //      1011
		public const int opPROGRAM_INFO = 0x0C; //      1100
		public const int opLABEL = 0x0D; //      1101
		public const int opPROBE = 0x0E; //      1110
		public const int opDO = 0x0F; //      1111

		//  \endverbatim \ref cMath "MATH" \verbatim
		//                                        0001....
		//                    ADD                     00..
		public const int opADD8 = 0x10; //        00
		public const int opADD16 = 0x11; //        01
		public const int opADD32 = 0x12; //        10
		public const int opADDF = 0x13; //        11
										//                    SUB                     01..
		public const int opSUB8 = 0x14; //        00
		public const int opSUB16 = 0x15; //        01
		public const int opSUB32 = 0x16; //        10
		public const int opSUBF = 0x17; //        11
										//                    MUL                     10..
		public const int opMUL8 = 0x18; //        00
		public const int opMUL16 = 0x19; //        01
		public const int opMUL32 = 0x1A; //        10
		public const int opMULF = 0x1B; //        11
										//                    DIV                     11..
		public const int opDIV8 = 0x1C; //        00
		public const int opDIV16 = 0x1D; //        01
		public const int opDIV32 = 0x1E; //        10
		public const int opDIVF = 0x1F; //        11

		//  \endverbatim \ref Logic "LOGIC" \verbatim
		//        LOGIC                           0010....
		//                    OR                      00..
		public const int opOR8 = 0x20; //        00
		public const int opOR16 = 0x21; //        01
		public const int opOR32 = 0x22; //        10

		//                    AND                     01..
		public const int opAND8 = 0x24; //        00
		public const int opAND16 = 0x25; //        01
		public const int opAND32 = 0x26; //        10

		//                    XOR                     10..
		public const int opXOR8 = 0x28; //        00
		public const int opXOR16 = 0x29; //        01
		public const int opXOR32 = 0x2A; //        10

		//                    RL                      11..
		public const int opRL8 = 0x2C; //        00
		public const int opRL16 = 0x2D; //        01
		public const int opRL32 = 0x2E; //        10

		//  \endverbatim \ref cMove "MOVE" \verbatim
		public const int opINIT_BYTES = 0x2F; //      1111
											  //        MOVE                            0011....
											  //                    MOVE8_                  00..
		public const int opMOVE8_8 = 0x30; //        00
		public const int opMOVE8_16 = 0x31; //        01
		public const int opMOVE8_32 = 0x32; //        10
		public const int opMOVE8_F = 0x33; //        11
										   //                    MOVE16_                 01..
		public const int opMOVE16_8 = 0x34; //        00
		public const int opMOVE16_16 = 0x35; //        01
		public const int opMOVE16_32 = 0x36; //        10
		public const int opMOVE16_F = 0x37; //        11
											//                    MOVE32_                 10..
		public const int opMOVE32_8 = 0x38; //        00
		public const int opMOVE32_16 = 0x39; //        01
		public const int opMOVE32_32 = 0x3A; //        10
		public const int opMOVE32_F = 0x3B; //        11
											//                    MOVEF_                  11..
		public const int opMOVEF_8 = 0x3C; //        00
		public const int opMOVEF_16 = 0x3D; //        01
		public const int opMOVEF_32 = 0x3E; //        10
		public const int opMOVEF_F = 0x3F; //        11

		//  \endverbatim \ref cBranch "BRANCH" \verbatim
		//        BRANCH                          010000..
		public const int opJR = 0x40; //        00
		public const int opJR_FALSE = 0x41; //        01
		public const int opJR_TRUE = 0x42; //        10
		public const int opJR_NAN = 0x43; //        11

		//  \endverbatim \ref cCompare "COMPARE" \verbatim
		//        COMPARE                         010.....
		//                    CP_LT                  001..
		public const int opCP_LT8 = 0x44; //        00
		public const int opCP_LT16 = 0x45; //        01
		public const int opCP_LT32 = 0x46; //        10
		public const int opCP_LTF = 0x47; //        11
										  //                    CP_GT                  010..
		public const int opCP_GT8 = 0x48; //        00
		public const int opCP_GT16 = 0x49; //        01
		public const int opCP_GT32 = 0x4A; //        10
		public const int opCP_GTF = 0x4B; //        11
										  //                    CP_EQ                  011..
		public const int opCP_EQ8 = 0x4C; //        00
		public const int opCP_EQ16 = 0x4D; //        01
		public const int opCP_EQ32 = 0x4E; //        10
		public const int opCP_EQF = 0x4F; //        11
										  //                    CP_NEQ                 100..
		public const int opCP_NEQ8 = 0x50; //        00
		public const int opCP_NEQ16 = 0x51; //        01
		public const int opCP_NEQ32 = 0x52; //        10
		public const int opCP_NEQF = 0x53; //        11
										   //                    CP_LTEQ                101..
		public const int opCP_LTEQ8 = 0x54; //        00
		public const int opCP_LTEQ16 = 0x55; //        01
		public const int opCP_LTEQ32 = 0x56; //        10
		public const int opCP_LTEQF = 0x57; //        11
											//                    CP_GTEQ                110..
		public const int opCP_GTEQ8 = 0x58; //        00
		public const int opCP_GTEQ16 = 0x59; //        01
		public const int opCP_GTEQ32 = 0x5A; //        10
		public const int opCP_GTEQF = 0x5B; //        11

		//  \endverbatim \ref Select "SELECT" \verbatim
		//        SELECT                          010111..
		public const int opSELECT8 = 0x5C; //        00
		public const int opSELECT16 = 0x5D; //        01
		public const int opSELECT32 = 0x5E; //        10
		public const int opSELECTF = 0x5F; //        11


		//  \endverbatim \ref VM \verbatim
		public const int opSYSTEM = 0x60;
		public const int opPORT_CNV_OUTPUT = 0x61;
		public const int opPORT_CNV_INPUT = 0x62;
		public const int opNOTE_TO_FREQ = 0x63;

		//  \endverbatim \ref cBranch "BRANCH" \verbatim
		//        BRANCH                          011000..
		//?       00
		//?       01
		//?       10
		//?       11
		//                    JR_LT                  001..
		public const int opJR_LT8 = 0x64; //        00
		public const int opJR_LT16 = 0x65; //        01
		public const int opJR_LT32 = 0x66; //        10
		public const int opJR_LTF = 0x67; //        11
										  //                    JR_GT                  010..
		public const int opJR_GT8 = 0x68; //        00
		public const int opJR_GT16 = 0x69; //        01
		public const int opJR_GT32 = 0x6A; //        10
		public const int opJR_GTF = 0x6B; //        11
										  //                    JR_EQ                  011..
		public const int opJR_EQ8 = 0x6C; //        00
		public const int opJR_EQ16 = 0x6D; //        01
		public const int opJR_EQ32 = 0x6E; //        10
		public const int opJR_EQF = 0x6F; //        11
										  //                    JR_NEQ                 100..
		public const int opJR_NEQ8 = 0x70; //        00
		public const int opJR_NEQ16 = 0x71; //        01
		public const int opJR_NEQ32 = 0x72; //        10
		public const int opJR_NEQF = 0x73; //        11
										   //                    JR_LTEQ                101..
		public const int opJR_LTEQ8 = 0x74; //        00
		public const int opJR_LTEQ16 = 0x75; //        01
		public const int opJR_LTEQ32 = 0x76; //        10
		public const int opJR_LTEQF = 0x77; //        11
											//                    JR_GTEQ                110..
		public const int opJR_GTEQ8 = 0x78; //        00
		public const int opJR_GTEQ16 = 0x79; //        01
		public const int opJR_GTEQ32 = 0x7A; //        10
		public const int opJR_GTEQF = 0x7B; //        11

		//  \endverbatim \ref VM \verbatim
		public const int opINFO = 0x7C; //  01111100
		public const int opSTRINGS = 0x7D; //  01111101
		public const int opMEMORY_WRITE = 0x7E; //  01111110
		public const int opMEMORY_READ = 0x7F; //  01111111

		//        SYSTEM                          1.......

		//  \endverbatim \ref cUi "UI" \verbatim
		//        UI                              100000..
		public const int opUI_FLUSH = 0x80; //        00
		public const int opUI_READ = 0x81; //        01
		public const int opUI_WRITE = 0x82; //        10
		public const int opUI_BUTTON = 0x83; //        11
		public const int opUI_DRAW = 0x84; //  10000100

		//  \endverbatim \ref cTimer "TIMER" \verbatim
		public const int opTIMER_WAIT = 0x85; //  10000101
		public const int opTIMER_READY = 0x86; //  10000110
		public const int opTIMER_READ = 0x87; //  10000111

		//  \endverbatim \ref VM \verbatim
		//        BREAKPOINT                      10001...
		public const int opBP0 = 0x88; //       000
		public const int opBP1 = 0x89; //       001
		public const int opBP2 = 0x8A; //       010
		public const int opBP3 = 0x8B; //       011
		public const int opBP_SET = 0x8C; //  10001100
		public const int opMATH = 0x8D; //  10001101
		public const int opRANDOM = 0x8E; //  10001110

		//  \endverbatim \ref cTimer "TIMER" \verbatim
		public const int opTIMER_READ_US = 0x8F; //  10001111

		//  \endverbatim \ref cUi "UI" \verbatim
		public const int opKEEP_ALIVE = 0x90; //  10010000

		//  \endverbatim \ref cCom "COM" \verbatim
		//                                        100100
		public const int opCOM_READ = 0x91; //        01
		public const int opCOM_WRITE = 0x92; //        10

		//  \endverbatim \ref cSound "SOUND" \verbatim
		//                                        100101
		public const int opSOUND = 0x94; //        00
		public const int opSOUND_TEST = 0x95; //        01
		public const int opSOUND_READY = 0x96; //        10

		//  \endverbatim \ref cInput "INPUT" \verbatim
		//
		public const int opINPUT_SAMPLE = 0x97; //  10010111

		//                                        10011...
		public const int opINPUT_DEVICE_LIST = 0x98; //       000
		public const int opINPUT_DEVICE = 0x99; //       001
		public const int opINPUT_READ = 0x9A; //       010
		public const int opINPUT_TEST = 0x9B; //       011
		public const int opINPUT_READY = 0x9C; //       100
		public const int opINPUT_READSI = 0x9D; //       101
		public const int opINPUT_READEXT = 0x9E; //       110
		public const int opINPUT_WRITE = 0x9F; //       111
											   //  \endverbatim \ref cOutput "OUTPUT" \verbatim
											   //                                        101.....
		public const int opOUTPUT_GET_TYPE = 0xA0; //     00000
		public const int opOUTPUT_SET_TYPE = 0xA1; //     00001
		public const int opOUTPUT_RESET = 0xA2; //     00010
		public const int opOUTPUT_STOP = 0xA3; //     00011
		public const int opOUTPUT_POWER = 0xA4; //     00100
		public const int opOUTPUT_SPEED = 0xA5; //     00101
		public const int opOUTPUT_START = 0xA6; //     00110
		public const int opOUTPUT_POLARITY = 0xA7; //     00111
		public const int opOUTPUT_READ = 0xA8; //     01000
		public const int opOUTPUT_TEST = 0xA9; //     01001
		public const int opOUTPUT_READY = 0xAA; //     01010
		public const int opOUTPUT_POSITION = 0xAB; //     01011
		public const int opOUTPUT_STEP_POWER = 0xAC; //     01100
		public const int opOUTPUT_TIME_POWER = 0xAD; //     01101
		public const int opOUTPUT_STEP_SPEED = 0xAE; //     01110
		public const int opOUTPUT_TIME_SPEED = 0xAF; //     01111

		public const int opOUTPUT_STEP_SYNC = 0xB0; //     10000
		public const int opOUTPUT_TIME_SYNC = 0xB1; //     10001
		public const int opOUTPUT_CLR_COUNT = 0xB2; //     10010
		public const int opOUTPUT_GET_COUNT = 0xB3; //     10011

		public const int opOUTPUT_PRG_STOP = 0xB4; //     10100

		//  \endverbatim \ref cMemory "MEMORY" \verbatim
		//                                        11000...
		public const int opFILE = 0xC0; //       000
		public const int opARRAY = 0xC1; //       001
		public const int opARRAY_WRITE = 0xC2; //       010
		public const int opARRAY_READ = 0xC3; //       011
		public const int opARRAY_APPEND = 0xC4; //       100
		public const int opMEMORY_USAGE = 0xC5; //       101
		public const int opFILENAME = 0xC6; //       110

		//  \endverbatim \ref cMove "READ" \verbatim
		//                                        110010..
		public const int opREAD8 = 0xC8; //        00
		public const int opREAD16 = 0xC9; //        01
		public const int opREAD32 = 0xCA; //        10
		public const int opREADF = 0xCB; //        11

		//  \endverbatim \ref cMove "WRITE" \verbatim
		//                                        110011..
		public const int opWRITE8 = 0xCC; //        00
		public const int opWRITE16 = 0xCD; //        01
		public const int opWRITE32 = 0xCE; //        10
		public const int opWRITEF = 0xCF; //        11

		//  \endverbatim \ref cCom "COM" \verbatim
		//                                        11010...
		public const int opCOM_READY = 0xD0; //       000
		public const int opCOM_READDATA = 0xD1; //       001
		public const int opCOM_WRITEDATA = 0xD2; //       010
		public const int opCOM_GET = 0xD3; //       011
		public const int opCOM_SET = 0xD4; //       100
		public const int opCOM_TEST = 0xD5; //       101
		public const int opCOM_REMOVE = 0xD6; //       110
		public const int opCOM_WRITEFILE = 0xD7; //       111

		//                                        11011...
		public const int opMAILBOX_OPEN = 0xD8; //       000
		public const int opMAILBOX_WRITE = 0xD9; //       001
		public const int opMAILBOX_READ = 0xDA; //       010
		public const int opMAILBOX_TEST = 0xDB; //       011
		public const int opMAILBOX_READY = 0xDC; //       100
		public const int opMAILBOX_CLOSE = 0xDD; //       101

		//        SPARE                           111.....

		//  \endverbatim \ref TST \verbatim
		public const int opTST = 0xFF;  //  11111111
	}
}
