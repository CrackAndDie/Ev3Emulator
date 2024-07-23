namespace EV3DecompilerLib.Decompile
{
    /// <summary>
    /// LMS objects class to describe hirarchy
    /// </summary>
    public partial class lms2012
    {
        public struct ObjectHeader
        {
            internal Int32 offset;
            internal UInt16 owner;
            internal Int16 trigger_count;
            internal Int32 local_bytes;

            internal bool is_vmthread
            {
                get
                {
                    return this.owner == 0 && this.trigger_count == 0;
                }
            }
            internal bool is_subcall
            {
                get
                {
                    return this.owner == 0 && this.trigger_count == 1;
                }
            }
            internal bool is_block
            {
                get
                {
                    return this.owner != 0;
                }
            }
        }


        //[StructLayout(LayoutKind.Explicit)]
        internal struct ProgramHeader
        {
            //[FieldOffset(0)]
            //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            char lego1;
            char lego2;
            char lego3;
            char lego4;
            internal Int32 size;
            internal UInt16 _byte_code_version;
            internal Int16 num_objects;
            internal Int32 global_bytes;
            internal string byte_code_version
            {
                get
                {
                    //return string."{0:.2f}".format(self._byte_code_version / 100.0)
                    return $"{(double)_byte_code_version / 100.0:0.00}";
                }
            }
            internal string lego
            {
                get { return $"{lego1}{lego2}{lego3}{lego4}"; }
            }
        }

        internal enum DataFormat
        {
            DATA8 = 0x00,
            DATA16 = 0x01,
            DATA32 = 0x02,
            DATAF = 0x03,
            DATAS = 0x04,
            DATAA = 0x05,
            DATAV = 0x07,
            DATAPCT = 0x10,
            DATARAW = 0x12,
            DATASI = 0x13
        };

        internal static Dictionary<DataFormat, int> _DATAFormat_size = new Dictionary<DataFormat, int> {
            {DataFormat.DATA8, 1 },
            {DataFormat.DATA16,  2 },
            {DataFormat.DATA32,  4},
            {DataFormat.DATAF,  4},
            {DataFormat.DATAS,  1},
            {DataFormat.DATAA,  1},
            {DataFormat.DATAV,  1},
            {DataFormat.DATAPCT,  4},
            {DataFormat.DATARAW,  4},
            {DataFormat.DATASI,  4},
        };

        internal enum Param
        {
            SUBP = 0x01,
            PARNO = 0x02,
            PARLAB = 0x03,
            PARVALUES = 0x04,
            PAR8 = 0x08 + DataFormat.DATA8,
            PAR16 = 0x08 + DataFormat.DATA16,
            PAR32 = 0x08 + DataFormat.DATA32,
            PARF = 0x08 + DataFormat.DATAF,
            PARS = 0x08 + DataFormat.DATAS,
            PARV = 0x08 + DataFormat.DATAV,
        }

        internal enum Subparam
        {
            UI_READ_SUBP = 0,
            UI_WRITE_SUBP = 1,
            UI_DRAW_SUBP = 2,
            UI_BUTTON_SUBP = 3,
            FILE_SUBP = 4,
            PROGRAM_SUBP = 5,
            VM_SUBP = 6,
            TST_SUBP = 6,
            STRING_SUBP = 7,
            COM_READ_SUBP = 8,
            COM_WRITE_SUBP = 9,
            SOUND_SUBP = 10,
            INPUT_SUBP = 11,
            ARRAY_SUBP = 12,
            MATH_SUBP = 13,
            COM_GET_SUBP = 14,
            COM_SET_SUBP = 15,
            FILENAME_SUBP = 16
        }

        internal static Type subcode_type(Subparam sb)
        {
            return _subcode_enums[sb];
        }
        internal static Type subcode_params(Subparam sb)
        {
            return _subcode_enums[sb];
        }

        public enum Op
        {
            // VM
            //                                    0000....
            ERROR = 0x00, //       0000
            NOP = 0x01, //       0001
            PROGRAM_STOP = 0x02, //       0010
            PROGRAM_START = 0x03, //       0011
            OBJECT_STOP = 0x04, //       0100
            OBJECT_START = 0x05, //       0101
            OBJECT_TRIG = 0x06, //       0110
            OBJECT_WAIT = 0x07, //       0111
            RETURN = 0x08, //       1000
            CALL = 0x09, //       1001
            OBJECT_END = 0x0A, //       1010
            SLEEP = 0x0B, //       1011
            PROGRAM_INFO = 0x0C, //       1100
            LABEL = 0x0D, //       1101
            PROBE = 0x0E, //       1110
            DO = 0x0F, //       1111

            // cMath "MATH"
            //                                    0001....
            //                    ADD                 00..
            ADD8 = 0x10, //         00
            ADD16 = 0x11, //         01
            ADD32 = 0x12, //         10
            ADDF = 0x13, //         11
                         //                    SUB                 01..
            SUB8 = 0x14, //         00
            SUB16 = 0x15, //         01
            SUB32 = 0x16, //         10
            SUBF = 0x17, //         11
                         //                    MUL                 10..
            MUL8 = 0x18, //         00
            MUL16 = 0x19, //         01
            MUL32 = 0x1A, //         10
            MULF = 0x1B, //         11
                         //                    DIV                 11..
            DIV8 = 0x1C, //         00
            DIV16 = 0x1D, //         01
            DIV32 = 0x1E, //         10
            DIVF = 0x1F, //         11

            // Logic "LOGIC"
            //        LOGIC                       0010....
            //                    OR                  00..
            OR8 = 0x20, //         00
            OR16 = 0x21, //         01
            OR32 = 0x22, //         10

            //                    AND                 01..
            AND8 = 0x24, //         00
            AND16 = 0x25, //         01
            AND32 = 0x26, //         10

            //                    XOR                 10..
            XOR8 = 0x28, //         00
            XOR16 = 0x29, //         01
            XOR32 = 0x2A, //         10

            //                    RL                  11..
            RL8 = 0x2C, //         00
            RL16 = 0x2D, //         01
            RL32 = 0x2E, //         10

            // cMove "MOVE"
            INIT_BYTES = 0x2F, //       1111
                               //        MOVE                        0011....
                               //                    MOVE8_              00..
            MOVE8_8 = 0x30, //         00
            MOVE8_16 = 0x31, //         01
            MOVE8_32 = 0x32, //         10
            MOVE8_F = 0x33, //         11
                            //                    MOVE16_             01..
            MOVE16_8 = 0x34, //         00
            MOVE16_16 = 0x35, //         01
            MOVE16_32 = 0x36, //         10
            MOVE16_F = 0x37, //         11
                             //                    MOVE32_             10..
            MOVE32_8 = 0x38, //         00
            MOVE32_16 = 0x39, //         01
            MOVE32_32 = 0x3A, //         10
            MOVE32_F = 0x3B, //         11
                             //                    MOVEF_              11..
            MOVEF_8 = 0x3C, //         00
            MOVEF_16 = 0x3D, //         01
            MOVEF_32 = 0x3E, //         10
            MOVEF_F = 0x3F, //         11

            // cBranch "BRANCH"
            //        BRANCH                      010000..
            JR = 0x40, //         00
            JR_FALSE = 0x41, //         01
            JR_TRUE = 0x42, //         10
            JR_NAN = 0x43, //         11

            // cCompare "COMPARE"
            //        COMPARE                     010.....
            //                    CP_LT              001..
            CP_LT8 = 0x44, //         00
            CP_LT16 = 0x45, //         01
            CP_LT32 = 0x46, //         10
            CP_LTF = 0x47, //         11
                           //                    CP_GT              010..
            CP_GT8 = 0x48, //         00
            CP_GT16 = 0x49, //         01
            CP_GT32 = 0x4A, //         10
            CP_GTF = 0x4B, //         11
                           //                    CP_EQ              011..
            CP_EQ8 = 0x4C, //         00
            CP_EQ16 = 0x4D, //         01
            CP_EQ32 = 0x4E, //         10
            CP_EQF = 0x4F, //         11
                           //                    CP_NEQ             100..
            CP_NEQ8 = 0x50, //         00
            CP_NEQ16 = 0x51, //         01
            CP_NEQ32 = 0x52, //         10
            CP_NEQF = 0x53, //         11
                            //                    CP_LTEQ            101..
            CP_LTEQ8 = 0x54, //         00
            CP_LTEQ16 = 0x55, //         01
            CP_LTEQ32 = 0x56, //         10
            CP_LTEQF = 0x57, //         11
                             //                    CP_GTEQ            110..
            CP_GTEQ8 = 0x58, //         00
            CP_GTEQ16 = 0x59, //         01
            CP_GTEQ32 = 0x5A, //         10
            CP_GTEQF = 0x5B, //         11

            // Select "SELECT"
            //        SELECT                      010111..
            SELECT8 = 0x5C, //         00
            SELECT16 = 0x5D, //         01
            SELECT32 = 0x5E, //         10
            SELECTF = 0x5F, //         11


            // VM
            SYSTEM = 0x60,
            PORT_CNV_OUTPUT = 0x61,
            PORT_CNV_INPUT = 0x62,
            NOTE_TO_FREQ = 0x63,

            // cBranch "BRANCH"
            // BRANCH                      011000..
            //?       00
            //?       01
            //?       10
            //?       11
            // JR_LT              001..
            JR_LT8 = 0x64, //         00
            JR_LT16 = 0x65, //         01
            JR_LT32 = 0x66, //         10
            JR_LTF = 0x67, //         11
                           //                    JR_GT              010..
            JR_GT8 = 0x68, //         00
            JR_GT16 = 0x69, //         01
            JR_GT32 = 0x6A, //         10
            JR_GTF = 0x6B, //         11
                           //                    JR_EQ              011..
            JR_EQ8 = 0x6C, //         00
            JR_EQ16 = 0x6D, //         01
            JR_EQ32 = 0x6E, //         10
            JR_EQF = 0x6F, //         11
                           //                    JR_NEQ             100..
            JR_NEQ8 = 0x70, //         00
            JR_NEQ16 = 0x71, //         01
            JR_NEQ32 = 0x72, //         10
            JR_NEQF = 0x73, //         11
                            //                    JR_LTEQ            101..
            JR_LTEQ8 = 0x74, //         00
            JR_LTEQ16 = 0x75, //         01
            JR_LTEQ32 = 0x76, //         10
            JR_LTEQF = 0x77, //         11
                             //                    JR_GTEQ            110..
            JR_GTEQ8 = 0x78, //         00
            JR_GTEQ16 = 0x79, //         01
            JR_GTEQ32 = 0x7A, //         10
            JR_GTEQF = 0x7B, //         11

            // VM
            INFO = 0x7C, //   01111100
            STRINGS = 0x7D, //   01111101
            MEMORY_WRITE = 0x7E, //   01111110
            MEMORY_READ = 0x7F, //   01111111

            //        SYSTEM                      1.......

            // cUi "UI"
            //        UI                          100000..
            UI_FLUSH = 0x80, //         00
            UI_READ = 0x81, //         01
            UI_WRITE = 0x82, //         10
            UI_BUTTON = 0x83, //         11
            UI_DRAW = 0x84, //   10000100

            // cTimer "TIMER"
            TIMER_WAIT = 0x85, //   10000101
            TIMER_READY = 0x86, //   10000110
            TIMER_READ = 0x87, //   10000111

            // VM
            //        BREAKPOINT                  10001...
            BP0 = 0x88, //        000
            BP1 = 0x89, //        001
            BP2 = 0x8A, //        010
            BP3 = 0x8B, //        011
            BP_SET = 0x8C, //   10001100
            MATH = 0x8D, //   10001101
            RANDOM = 0x8E, //   10001110

            // cTimer "TIMER"
            TIMER_READ_US = 0x8F, //   10001111

            // cUi "UI"
            KEEP_ALIVE = 0x90, //   10010000

            // cCom "COM"
            //                                      100100
            COM_READ = 0x91, //         01
            COM_WRITE = 0x92, //         10

            // cSound "SOUND"
            //                                      100101
            SOUND = 0x94, //         00
            SOUND_TEST = 0x95, //         01
            SOUND_READY = 0x96, //         10

            // cInput "INPUT"
            //
            INPUT_SAMPLE = 0x97, //   10010111

            //                                    10011...
            INPUT_DEVICE_LIST = 0x98, //        000
            INPUT_DEVICE = 0x99, //        001
            INPUT_READ = 0x9A, //        010
            INPUT_TEST = 0x9B, //        011
            INPUT_READY = 0x9C, //        100
            INPUT_READSI = 0x9D, //        101
            INPUT_READEXT = 0x9E, //        110
            INPUT_WRITE = 0x9F, //        111
                                // cOutput "OUTPUT"
                                //                                    101.....
            OUTPUT_GET_TYPE = 0xA0, //      00000
            OUTPUT_SET_TYPE = 0xA1, //      00001
            OUTPUT_RESET = 0xA2, //      00010
            OUTPUT_STOP = 0xA3, //      00011
            OUTPUT_POWER = 0xA4, //      00100
            OUTPUT_SPEED = 0xA5, //      00101
            OUTPUT_START = 0xA6, //      00110
            OUTPUT_POLARITY = 0xA7, //      00111
            OUTPUT_READ = 0xA8, //      01000
            OUTPUT_TEST = 0xA9, //      01001
            OUTPUT_READY = 0xAA, //      01010
            OUTPUT_POSITION = 0xAB, //      01011
            OUTPUT_STEP_POWER = 0xAC, //      01100
            OUTPUT_TIME_POWER = 0xAD, //      01101
            OUTPUT_STEP_SPEED = 0xAE, //      01110
            OUTPUT_TIME_SPEED = 0xAF, //      01111

            OUTPUT_STEP_SYNC = 0xB0, //      10000
            OUTPUT_TIME_SYNC = 0xB1, //      10001
            OUTPUT_CLR_COUNT = 0xB2, //      10010
            OUTPUT_GET_COUNT = 0xB3, //      10011

            OUTPUT_PRG_STOP = 0xB4, //      10100

            // cMemory "MEMORY"
            //                                    11000...
            FILE = 0xC0, //        000
            ARRAY = 0xC1, //        001
            ARRAY_WRITE = 0xC2, //        010
            ARRAY_READ = 0xC3, //        011
            ARRAY_APPEND = 0xC4, //        100
            MEMORY_USAGE = 0xC5, //        101
            FILENAME = 0xC6, //        110

            // cMove "READ"
            //                                    110010..
            READ8 = 0xC8, //         00
            READ16 = 0xC9, //         01
            READ32 = 0xCA, //         10
            READF = 0xCB, //         11

            // cMove "WRITE"
            //                                    110011..
            WRITE8 = 0xCC, //         00
            WRITE16 = 0xCD, //         01
            WRITE32 = 0xCE, //         10
            WRITEF = 0xCF, //         11

            // cCom "COM"
            //                                    11010...
            COM_READY = 0xD0, //        000
            COM_READDATA = 0xD1, //        001
            COM_WRITEDATA = 0xD2, //        010
            COM_GET = 0xD3, //        011
            COM_SET = 0xD4, //        100
            COM_TEST = 0xD5, //        101
            COM_REMOVE = 0xD6, //        110
            COM_WRITEFILE = 0xD7, //        111

            //                                    11011...
            MAILBOX_OPEN = 0xD8, //        000
            MAILBOX_WRITE = 0xD9, //        001
            MAILBOX_READ = 0xDA, //        010
            MAILBOX_TEST = 0xDB, //        011
            MAILBOX_READY = 0xDC, //        100
            MAILBOX_CLOSE = 0xDD, //        101

            //        SPARE                       111.....

            // TST
            TST = 0xFF, //  11111111
        }


        internal static Dictionary<Op, Enum[]> _op_code_params = new Dictionary<Op, Enum[]>
        {
            //    VM
            [Op.ERROR] = new Enum[] { },
            [Op.NOP] = new Enum[] { },
            [Op.PROGRAM_STOP] = new Enum[] { Param.PAR16, },
            [Op.PROGRAM_START] = new Enum[] { Param.PAR16, Param.PAR32, Param.PAR32, Param.PAR8 },
            [Op.OBJECT_STOP] = new Enum[] { Param.PAR16, },
            [Op.OBJECT_START] = new Enum[] { Param.PAR16, },
            [Op.OBJECT_TRIG] = new Enum[] { Param.PAR16, },
            [Op.OBJECT_WAIT] = new Enum[] { Param.PAR16, },
            [Op.RETURN] = new Enum[] { },
            [Op.CALL] = new Enum[] { Param.PAR16, Param.PARNO },
            [Op.OBJECT_END] = new Enum[] { },
            [Op.SLEEP] = new Enum[] { },
            [Op.PROGRAM_INFO] = new Enum[] { Subparam.PROGRAM_SUBP, },
            [Op.LABEL] = new Enum[] { Param.PARLAB, },
            [Op.PROBE] = new Enum[] { Param.PAR16, Param.PAR16, Param.PAR32, Param.PAR32 },
            [Op.DO] = new Enum[] { Param.PAR16, Param.PAR32, Param.PAR32 },
            //    Math
            [Op.ADD8] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.ADD16] = new Enum[] { Param.PAR16, Param.PAR16, Param.PAR16 },
            [Op.ADD32] = new Enum[] { Param.PAR32, Param.PAR32, Param.PAR32 },
            [Op.ADDF] = new Enum[] { Param.PARF, Param.PARF, Param.PARF },
            [Op.SUB8] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.SUB16] = new Enum[] { Param.PAR16, Param.PAR16, Param.PAR16 },
            [Op.SUB32] = new Enum[] { Param.PAR32, Param.PAR32, Param.PAR32 },
            [Op.SUBF] = new Enum[] { Param.PARF, Param.PARF, Param.PARF },
            [Op.MUL8] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.MUL16] = new Enum[] { Param.PAR16, Param.PAR16, Param.PAR16 },
            [Op.MUL32] = new Enum[] { Param.PAR32, Param.PAR32, Param.PAR32 },
            [Op.MULF] = new Enum[] { Param.PARF, Param.PARF, Param.PARF },
            [Op.DIV8] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.DIV16] = new Enum[] { Param.PAR16, Param.PAR16, Param.PAR16 },
            [Op.DIV32] = new Enum[] { Param.PAR32, Param.PAR32, Param.PAR32 },
            [Op.DIVF] = new Enum[] { Param.PARF, Param.PARF, Param.PARF },
            //    Logic
            [Op.OR8] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.OR16] = new Enum[] { Param.PAR16, Param.PAR16, Param.PAR16 },
            [Op.OR32] = new Enum[] { Param.PAR32, Param.PAR32, Param.PAR32 },
            [Op.AND8] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.AND16] = new Enum[] { Param.PAR16, Param.PAR16, Param.PAR16 },
            [Op.AND32] = new Enum[] { Param.PAR32, Param.PAR32, Param.PAR32 },
            [Op.XOR8] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.XOR16] = new Enum[] { Param.PAR16, Param.PAR16, Param.PAR16 },
            [Op.XOR32] = new Enum[] { Param.PAR32, Param.PAR32, Param.PAR32 },
            [Op.RL8] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.RL16] = new Enum[] { Param.PAR16, Param.PAR16, Param.PAR16 },
            [Op.RL32] = new Enum[] { Param.PAR32, Param.PAR32, Param.PAR32 },
            //    Move
            [Op.INIT_BYTES] = new Enum[] { Param.PAR8, Param.PAR32, Param.PARVALUES, Param.PAR8 },
            [Op.MOVE8_8] = new Enum[] { Param.PAR8, Param.PAR8 },
            [Op.MOVE8_16] = new Enum[] { Param.PAR8, Param.PAR16 },
            [Op.MOVE8_32] = new Enum[] { Param.PAR8, Param.PAR32 },
            [Op.MOVE8_F] = new Enum[] { Param.PAR8, Param.PARF },
            [Op.MOVE16_8] = new Enum[] { Param.PAR16, Param.PAR8 },
            [Op.MOVE16_16] = new Enum[] { Param.PAR16, Param.PAR16 },
            [Op.MOVE16_32] = new Enum[] { Param.PAR16, Param.PAR32 },
            [Op.MOVE16_F] = new Enum[] { Param.PAR16, Param.PARF },
            [Op.MOVE32_8] = new Enum[] { Param.PAR32, Param.PAR8 },
            [Op.MOVE32_16] = new Enum[] { Param.PAR32, Param.PAR16 },
            [Op.MOVE32_32] = new Enum[] { Param.PAR32, Param.PAR32 },
            [Op.MOVE32_F] = new Enum[] { Param.PAR32, Param.PARF },
            [Op.MOVEF_8] = new Enum[] { Param.PARF, Param.PAR8 },
            [Op.MOVEF_16] = new Enum[] { Param.PARF, Param.PAR16 },
            [Op.MOVEF_32] = new Enum[] { Param.PARF, Param.PAR32 },
            [Op.MOVEF_F] = new Enum[] { Param.PARF, Param.PARF },
            //    Branch
            [Op.JR] = new Enum[] { Param.PAR32, },
            [Op.JR_FALSE] = new Enum[] { Param.PAR8, Param.PAR32 },
            [Op.JR_TRUE] = new Enum[] { Param.PAR8, Param.PAR32 },
            [Op.JR_NAN] = new Enum[] { Param.PARF, Param.PAR32 },
            //    Compare
            [Op.CP_LT8] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.CP_LT16] = new Enum[] { Param.PAR16, Param.PAR16, Param.PAR8 },
            [Op.CP_LT32] = new Enum[] { Param.PAR32, Param.PAR32, Param.PAR8 },
            [Op.CP_LTF] = new Enum[] { Param.PARF, Param.PARF, Param.PAR8 },
            [Op.CP_GT8] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.CP_GT16] = new Enum[] { Param.PAR16, Param.PAR16, Param.PAR8 },
            [Op.CP_GT32] = new Enum[] { Param.PAR32, Param.PAR32, Param.PAR8 },
            [Op.CP_GTF] = new Enum[] { Param.PARF, Param.PARF, Param.PAR8 },
            [Op.CP_EQ8] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.CP_EQ16] = new Enum[] { Param.PAR16, Param.PAR16, Param.PAR8 },
            [Op.CP_EQ32] = new Enum[] { Param.PAR32, Param.PAR32, Param.PAR8 },
            [Op.CP_EQF] = new Enum[] { Param.PARF, Param.PARF, Param.PAR8 },
            [Op.CP_NEQ8] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.CP_NEQ16] = new Enum[] { Param.PAR16, Param.PAR16, Param.PAR8 },
            [Op.CP_NEQ32] = new Enum[] { Param.PAR32, Param.PAR32, Param.PAR8 },
            [Op.CP_NEQF] = new Enum[] { Param.PARF, Param.PARF, Param.PAR8 },
            [Op.CP_LTEQ8] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.CP_LTEQ16] = new Enum[] { Param.PAR16, Param.PAR16, Param.PAR8 },
            [Op.CP_LTEQ32] = new Enum[] { Param.PAR32, Param.PAR32, Param.PAR8 },
            [Op.CP_LTEQF] = new Enum[] { Param.PARF, Param.PARF, Param.PAR8 },
            [Op.CP_GTEQ8] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.CP_GTEQ16] = new Enum[] { Param.PAR16, Param.PAR16, Param.PAR8 },
            [Op.CP_GTEQ32] = new Enum[] { Param.PAR32, Param.PAR32, Param.PAR8 },
            [Op.CP_GTEQF] = new Enum[] { Param.PARF, Param.PARF, Param.PAR8 },
            //    Select
            [Op.SELECT8] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.SELECT16] = new Enum[] { Param.PAR8, Param.PAR16, Param.PAR16, Param.PAR16 },
            [Op.SELECT32] = new Enum[] { Param.PAR8, Param.PAR32, Param.PAR32, Param.PAR32 },
            [Op.SELECTF] = new Enum[] { Param.PAR8, Param.PARF, Param.PARF, Param.PARF },

            [Op.SYSTEM] = new Enum[] { Param.PAR8, Param.PAR32 },
            [Op.PORT_CNV_OUTPUT] = new Enum[] { Param.PAR32, Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.PORT_CNV_INPUT] = new Enum[] { Param.PAR32, Param.PAR8, Param.PAR8 },
            [Op.NOTE_TO_FREQ] = new Enum[] { Param.PAR8, Param.PAR16 },

            //    Branch
            [Op.JR_LT8] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR32 },
            [Op.JR_LT16] = new Enum[] { Param.PAR16, Param.PAR16, Param.PAR32 },
            [Op.JR_LT32] = new Enum[] { Param.PAR32, Param.PAR32, Param.PAR32 },
            [Op.JR_LTF] = new Enum[] { Param.PARF, Param.PARF, Param.PAR32 },
            [Op.JR_GT8] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR32 },
            [Op.JR_GT16] = new Enum[] { Param.PAR16, Param.PAR16, Param.PAR32 },
            [Op.JR_GT32] = new Enum[] { Param.PAR32, Param.PAR32, Param.PAR32 },
            [Op.JR_GTF] = new Enum[] { Param.PARF, Param.PARF, Param.PAR32 },
            [Op.JR_EQ8] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR32 },
            [Op.JR_EQ16] = new Enum[] { Param.PAR16, Param.PAR16, Param.PAR32 },
            [Op.JR_EQ32] = new Enum[] { Param.PAR32, Param.PAR32, Param.PAR32 },
            [Op.JR_EQF] = new Enum[] { Param.PARF, Param.PARF, Param.PAR32 },
            [Op.JR_NEQ8] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR32 },
            [Op.JR_NEQ16] = new Enum[] { Param.PAR16, Param.PAR16, Param.PAR32 },
            [Op.JR_NEQ32] = new Enum[] { Param.PAR32, Param.PAR32, Param.PAR32 },
            [Op.JR_NEQF] = new Enum[] { Param.PARF, Param.PARF, Param.PAR32 },
            [Op.JR_LTEQ8] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR32 },
            [Op.JR_LTEQ16] = new Enum[] { Param.PAR16, Param.PAR16, Param.PAR32 },
            [Op.JR_LTEQ32] = new Enum[] { Param.PAR32, Param.PAR32, Param.PAR32 },
            [Op.JR_LTEQF] = new Enum[] { Param.PARF, Param.PARF, Param.PAR32 },
            [Op.JR_GTEQ8] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR32 },
            [Op.JR_GTEQ16] = new Enum[] { Param.PAR16, Param.PAR16, Param.PAR32 },
            [Op.JR_GTEQ32] = new Enum[] { Param.PAR32, Param.PAR32, Param.PAR32 },
            [Op.JR_GTEQF] = new Enum[] { Param.PARF, Param.PARF, Param.PAR32 },
            // VM
            [Op.INFO] = new Enum[] { Subparam.VM_SUBP, },
            [Op.STRINGS] = new Enum[] { Subparam.STRING_SUBP, },
            [Op.MEMORY_WRITE] = new Enum[] { Param.PAR16, Param.PAR16, Param.PAR32, Param.PAR32, Param.PAR8 },
            [Op.MEMORY_READ] = new Enum[] { Param.PAR16, Param.PAR16, Param.PAR32, Param.PAR32, Param.PAR8 },
            //    UI
            [Op.UI_FLUSH] = new Enum[] { },
            [Op.UI_READ] = new Enum[] { Subparam.UI_READ_SUBP, },
            [Op.UI_WRITE] = new Enum[] { Subparam.UI_WRITE_SUBP, },
            [Op.UI_BUTTON] = new Enum[] { Subparam.UI_BUTTON_SUBP, },
            [Op.UI_DRAW] = new Enum[] { Subparam.UI_DRAW_SUBP, },
            //    Timer
            [Op.TIMER_WAIT] = new Enum[] { Param.PAR32, Param.PAR32 },
            [Op.TIMER_READY] = new Enum[] { Param.PAR32, },
            [Op.TIMER_READ] = new Enum[] { Param.PAR32, },
            //    VM
            [Op.BP0] = new Enum[] { },
            [Op.BP1] = new Enum[] { },
            [Op.BP2] = new Enum[] { },
            [Op.BP3] = new Enum[] { },
            [Op.BP_SET] = new Enum[] { Param.PAR16, Param.PAR8, Param.PAR32 },
            [Op.MATH] = new Enum[] { Subparam.MATH_SUBP, },
            [Op.RANDOM] = new Enum[] { Param.PAR16, Param.PAR16, Param.PAR16 },
            [Op.TIMER_READ_US] = new Enum[] { Param.PAR32, },
            [Op.KEEP_ALIVE] = new Enum[] { Param.PAR8, },
            //    Com
            [Op.COM_READ] = new Enum[] { Subparam.COM_READ_SUBP, },
            [Op.COM_WRITE] = new Enum[] { Subparam.COM_WRITE_SUBP, },
            //    Sound
            [Op.SOUND] = new Enum[] { Subparam.SOUND_SUBP, },
            [Op.SOUND_TEST] = new Enum[] { Param.PAR8, },
            [Op.SOUND_READY] = new Enum[] { },
            //    Input
            [Op.INPUT_SAMPLE] = new Enum[] { Param.PAR32, Param.PAR16, Param.PAR16, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PARF },
            [Op.INPUT_DEVICE_LIST] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.INPUT_DEVICE] = new Enum[] { Subparam.INPUT_SUBP, },
            [Op.INPUT_READ] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.INPUT_READSI] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PARF },
            [Op.INPUT_TEST] = new Enum[] { Param.PAR8, },
            [Op.INPUT_TEST] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.INPUT_READY] = new Enum[] { Param.PAR8, Param.PAR8 },
            [Op.INPUT_READEXT] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PARNO },
            [Op.INPUT_WRITE] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },
            //    Output
            [Op.OUTPUT_GET_TYPE] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.OUTPUT_SET_TYPE] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.OUTPUT_RESET] = new Enum[] { Param.PAR8, Param.PAR8 },
            [Op.OUTPUT_STOP] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.OUTPUT_SPEED] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.OUTPUT_POWER] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.OUTPUT_START] = new Enum[] { Param.PAR8, Param.PAR8 },
            [Op.OUTPUT_POLARITY] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.OUTPUT_READ] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR32 },
            [Op.OUTPUT_READY] = new Enum[] { Param.PAR8, Param.PAR8 },
            [Op.OUTPUT_POSITION] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR32 },
            [Op.OUTPUT_TEST] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.OUTPUT_STEP_POWER] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR32, Param.PAR32, Param.PAR32, Param.PAR8 },
            [Op.OUTPUT_TIME_POWER] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR32, Param.PAR32, Param.PAR32, Param.PAR8 },
            [Op.OUTPUT_STEP_SPEED] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR32, Param.PAR32, Param.PAR32, Param.PAR8 },
            [Op.OUTPUT_TIME_SPEED] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR32, Param.PAR32, Param.PAR32, Param.PAR8 },
            [Op.OUTPUT_STEP_SYNC] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR16, Param.PAR32, Param.PAR8 },
            [Op.OUTPUT_TIME_SYNC] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR16, Param.PAR32, Param.PAR8 },
            [Op.OUTPUT_CLR_COUNT] = new Enum[] { Param.PAR8, Param.PAR8 },
            [Op.OUTPUT_GET_COUNT] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR32 },
            [Op.OUTPUT_PRG_STOP] = new Enum[] { },
            //    Memory
            [Op.FILE] = new Enum[] { Subparam.FILE_SUBP, },
            [Op.ARRAY] = new Enum[] { Subparam.ARRAY_SUBP, },
            [Op.ARRAY_WRITE] = new Enum[] { Param.PAR16, Param.PAR32, Param.PARV },
            [Op.ARRAY_READ] = new Enum[] { Param.PAR16, Param.PAR32, Param.PARV },
            [Op.ARRAY_APPEND] = new Enum[] { Param.PAR16, Param.PARV },
            [Op.MEMORY_USAGE] = new Enum[] { Param.PAR32, Param.PAR32 },
            [Op.FILENAME] = new Enum[] { Subparam.FILENAME_SUBP, },
            //    Move
            [Op.READ8] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.READ16] = new Enum[] { Param.PAR16, Param.PAR8, Param.PAR16 },
            [Op.READ32] = new Enum[] { Param.PAR32, Param.PAR8, Param.PAR32 },
            [Op.READF] = new Enum[] { Param.PARF, Param.PAR8, Param.PARF },
            [Op.WRITE8] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.WRITE16] = new Enum[] { Param.PAR16, Param.PAR8, Param.PAR16 },
            [Op.WRITE32] = new Enum[] { Param.PAR32, Param.PAR8, Param.PAR32 },
            [Op.WRITEF] = new Enum[] { Param.PARF, Param.PAR8, Param.PARF },
            //    Com
            [Op.COM_READY] = new Enum[] { Param.PAR8, Param.PAR8 },
            [Op.COM_READDATA] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR16, Param.PAR8 },
            [Op.COM_WRITEDATA] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR16, Param.PAR8 },
            [Op.COM_GET] = new Enum[] { Subparam.COM_GET_SUBP, },
            [Op.COM_SET] = new Enum[] { Subparam.COM_SET_SUBP, },
            [Op.COM_TEST] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.COM_REMOVE] = new Enum[] { Param.PAR8, Param.PAR8 },
            [Op.COM_WRITEFILE] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },

            [Op.MAILBOX_OPEN] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },
            [Op.MAILBOX_WRITE] = new Enum[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PARNO },
            [Op.MAILBOX_READ] = new Enum[] { Param.PAR8, Param.PAR8, Param.PARNO },
            [Op.MAILBOX_TEST] = new Enum[] { Param.PAR8, Param.PAR8 },
            [Op.MAILBOX_READY] = new Enum[] { Param.PAR8, },
            [Op.MAILBOX_CLOSE] = new Enum[] { Param.PAR8, },
            //    Test
            [Op.TST] = new Enum[] { Subparam.TST_SUBP, },
        };

        // ----------------------------------------
        #region SUBCODES

        internal enum UiReadSubcode
        {
            GET_VBATT = 1,
            GET_IBATT = 2,
            GET_OS_VERS = 3,
            GET_EVENT = 4,
            GET_TBATT = 5,
            GET_IINT = 6,
            GET_IMOTOR = 7,
            GET_STRING = 8,
            GET_HW_VERS = 9,
            GET_FW_VERS = 10,
            GET_FW_BUILD = 11,
            GET_OS_BUILD = 12,
            GET_ADDRESS = 13,
            GET_CODE = 14,
            KEY = 15,
            GET_SHUTDOWN = 16,
            GET_WARNING = 17,
            GET_LBATT = 18,
            TEXTBOX_READ = 21,
            GET_VERSION = 26,
            GET_IP = 27,
            GET_POWER = 29,
            GET_SDCARD = 30,
            GET_USBSTICK = 31,
        }

        internal static Dictionary<Enum, Param[]> _ui_read_subcode_params = new Dictionary<Enum, Param[]>
        {
            [UiReadSubcode.GET_VBATT] = new[] { Param.PARF, },
            [UiReadSubcode.GET_IBATT] = new[] { Param.PARF, },
            [UiReadSubcode.GET_OS_VERS] = new[] { Param.PAR8, Param.PAR8 },
            [UiReadSubcode.GET_EVENT] = new[] { Param.PAR8, },
            [UiReadSubcode.GET_TBATT] = new[] { Param.PARF, },
            [UiReadSubcode.GET_IINT] = new[] { Param.PARF, },
            [UiReadSubcode.GET_IMOTOR] = new[] { Param.PARF, },
            [UiReadSubcode.GET_STRING] = new[] { Param.PAR8, Param.PAR8 },
            [UiReadSubcode.KEY] = new[] { Param.PAR8, },
            [UiReadSubcode.GET_SHUTDOWN] = new[] { Param.PAR8, },
            [UiReadSubcode.GET_WARNING] = new[] { Param.PAR8, },
            [UiReadSubcode.GET_LBATT] = new[] { Param.PAR8, },
            [UiReadSubcode.GET_ADDRESS] = new[] { Param.PAR32, },
            [UiReadSubcode.GET_CODE] = new[] { Param.PAR32, Param.PAR32, Param.PAR32, Param.PAR8 },
            [UiReadSubcode.TEXTBOX_READ] = new[] { Param.PAR8, Param.PAR32, Param.PAR8, Param.PAR8, Param.PAR16, Param.PAR8 },
            [UiReadSubcode.GET_HW_VERS] = new[] { Param.PAR8, Param.PAR8 },
            [UiReadSubcode.GET_FW_VERS] = new[] { Param.PAR8, Param.PAR8 },
            [UiReadSubcode.GET_FW_BUILD] = new[] { Param.PAR8, Param.PAR8 },
            [UiReadSubcode.GET_OS_BUILD] = new[] { Param.PAR8, Param.PAR8 },
            [UiReadSubcode.GET_VERSION] = new[] { Param.PAR8, Param.PAR8 },
            [UiReadSubcode.GET_IP] = new[] { Param.PAR8, Param.PAR8 },
            [UiReadSubcode.GET_POWER] = new[] { Param.PARF, Param.PARF, Param.PARF, Param.PARF },
            [UiReadSubcode.GET_SDCARD] = new[] { Param.PAR8, Param.PAR32, Param.PAR32 },
            [UiReadSubcode.GET_USBSTICK] = new[] { Param.PAR8, Param.PAR32, Param.PAR32 },
        };

        internal enum UiWriteSubcode
        {
            WRITE_FLUSH = 1,
            FLOATVALUE = 2,
            STAMP = 3,
            PUT_STRING = 8,
            VALUE8 = 9,
            VALUE16 = 10,
            VALUE32 = 11,
            VALUEF = 12,
            ADDRESS = 13,
            CODE = 14,
            DOWNLOAD_END = 15,
            SCREEN_BLOCK = 16,
            ALLOW_PULSE = 17,
            SET_PULSE = 18,
            TEXTBOX_APPEND = 21,
            SET_BUSY = 22,
            SET_TESTPIN = 24,
            INIT_RUN = 25,
            UPDATE_RUN = 26,
            LED = 27,
            POWER = 29,
            GRAPH_SAMPLE = 30,
            TERMINAL = 31,
        }

        internal static Dictionary<Enum, Param[]> _ui_write_subcode_params = new Dictionary<Enum, Param[]>
        {
            [UiWriteSubcode.WRITE_FLUSH] = new Param[] { },
            [UiWriteSubcode.FLOATVALUE] = new[] { Param.PARF, Param.PAR8, Param.PAR8 },
            [UiWriteSubcode.STAMP] = new[] { Param.PAR8, },
            [UiWriteSubcode.PUT_STRING] = new[] { Param.PAR8, },
            [UiWriteSubcode.CODE] = new[] { Param.PAR8, Param.PAR32 },
            [UiWriteSubcode.DOWNLOAD_END] = new Param[] { },
            [UiWriteSubcode.SCREEN_BLOCK] = new[] { Param.PAR8, },
            [UiWriteSubcode.ALLOW_PULSE] = new[] { Param.PAR8, },
            [UiWriteSubcode.SET_PULSE] = new[] { Param.PAR8, },
            [UiWriteSubcode.TEXTBOX_APPEND] = new[] { Param.PAR8, Param.PAR32, Param.PAR8, Param.PAR8 },
            [UiWriteSubcode.SET_BUSY] = new[] { Param.PAR8, },
            [UiWriteSubcode.VALUE8] = new[] { Param.PAR8, },
            [UiWriteSubcode.VALUE16] = new[] { Param.PAR16, },
            [UiWriteSubcode.VALUE32] = new[] { Param.PAR32, },
            [UiWriteSubcode.VALUEF] = new[] { Param.PARF, },
            [UiWriteSubcode.ADDRESS] = new[] { Param.PAR32, },
            [UiWriteSubcode.INIT_RUN] = new Param[] { },
            [UiWriteSubcode.UPDATE_RUN] = new Param[] { },
            [UiWriteSubcode.LED] = new[] { Param.PAR8, },
            [UiWriteSubcode.POWER] = new[] { Param.PAR8, },
            [UiWriteSubcode.TERMINAL] = new[] { Param.PAR8, },
            [UiWriteSubcode.GRAPH_SAMPLE] = new Param[] { },
            [UiWriteSubcode.SET_TESTPIN] = new[] { Param.PAR8, },
        };

        internal enum UiButtonSubcode
        {
            SHORTPRESS = 1,
            LONGPRESS = 2,
            WAIT_FOR_PRESS = 3,
            FLUSH = 4,
            PRESS = 5,
            RELEASE = 6,
            GET_HORZ = 7,
            GET_VERT = 8,
            PRESSED = 9,
            SET_BACK_BLOCK = 10,
            GET_BACK_BLOCK = 11,
            TESTSHORTPRESS = 12,
            TESTLONGPRESS = 13,
            GET_BUMBED = 14,
            GET_CLICK = 15,

        }

        internal static Dictionary<Enum, Param[]> _ui_button_subcode_params = new Dictionary<Enum, Param[]>
        {
            [UiButtonSubcode.SHORTPRESS] = new[] { Param.PAR8, Param.PAR8 },
            [UiButtonSubcode.LONGPRESS] = new[] { Param.PAR8, Param.PAR8 },
            [UiButtonSubcode.FLUSH] = new Param[] { },
            [UiButtonSubcode.WAIT_FOR_PRESS] = new Param[] { },
            [UiButtonSubcode.PRESS] = new[] { Param.PAR8, },
            [UiButtonSubcode.RELEASE] = new[] { Param.PAR8, },
            [UiButtonSubcode.GET_HORZ] = new[] { Param.PAR16, },
            [UiButtonSubcode.GET_VERT] = new[] { Param.PAR16, },
            [UiButtonSubcode.PRESSED] = new[] { Param.PAR8, Param.PAR8 },
            [UiButtonSubcode.SET_BACK_BLOCK] = new[] { Param.PAR8, },
            [UiButtonSubcode.GET_BACK_BLOCK] = new[] { Param.PAR8, },
            [UiButtonSubcode.TESTSHORTPRESS] = new[] { Param.PAR8, Param.PAR8 },
            [UiButtonSubcode.TESTLONGPRESS] = new[] { Param.PAR8, Param.PAR8 },
            [UiButtonSubcode.GET_BUMBED] = new[] { Param.PAR8, Param.PAR8 },
            [UiButtonSubcode.GET_CLICK] = new[] { Param.PAR8, },
        };

        internal enum ComReadSubcode
        {
            COMMAND = 14,

        }

        internal static Dictionary<Enum, Param[]> _com_read_subcode_params = new Dictionary<Enum, Param[]>
        {
            [ComReadSubcode.COMMAND] = new[] { Param.PAR32, Param.PAR32, Param.PAR32, Param.PAR8 },
        };

        internal enum ComWriteSubcode
        {
            REPLY = 14,

        }

        internal static Dictionary<Enum, Param[]> _com_write_subcode_params = new Dictionary<Enum, Param[]>
        {
            [ComWriteSubcode.REPLY] = new[] { Param.PAR32, Param.PAR32, Param.PAR8 },
        };

        internal enum ComGetSubcode
        {
            GET_ON_OFF = 1,
            GET_VISIBLE = 2,
            GET_RESULT = 4,
            GET_PIN = 5,
            SEARCH_ITEMS = 8,
            SEARCH_ITEM = 9,
            FAVOUR_ITEMS = 10,
            FAVOUR_ITEM = 11,
            GET_ID = 12,
            GET_BRICKNAME = 13,
            GET_NETWORK = 14,
            GET_PRESENT = 15,
            GET_ENCRYPT = 16,
            CONNEC_ITEMS = 17,
            CONNEC_ITEM = 18,
            GET_INCOMING = 19,
            GET_MODE2 = 20,

        }

        internal static Dictionary<Enum, Param[]> _com_get_subcode_params = new Dictionary<Enum, Param[]>
        {
            [ComGetSubcode.GET_ON_OFF] = new[] { Param.PAR8, Param.PAR8 },
            [ComGetSubcode.GET_VISIBLE] = new[] { Param.PAR8, Param.PAR8 },
            [ComGetSubcode.GET_RESULT] = new[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [ComGetSubcode.GET_PIN] = new[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },
            [ComGetSubcode.SEARCH_ITEMS] = new[] { Param.PAR8, Param.PAR8 },
            [ComGetSubcode.SEARCH_ITEM] = new[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },
            [ComGetSubcode.FAVOUR_ITEMS] = new[] { Param.PAR8, Param.PAR8 },
            [ComGetSubcode.FAVOUR_ITEM] = new[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },
            [ComGetSubcode.GET_ID] = new[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [ComGetSubcode.GET_BRICKNAME] = new[] { Param.PAR8, Param.PAR8 },
            [ComGetSubcode.GET_NETWORK] = new[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },
            [ComGetSubcode.GET_PRESENT] = new[] { Param.PAR8, Param.PAR8 },
            [ComGetSubcode.GET_ENCRYPT] = new[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [ComGetSubcode.CONNEC_ITEMS] = new[] { Param.PAR8, Param.PAR8 },
            [ComGetSubcode.CONNEC_ITEM] = new[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },
            [ComGetSubcode.GET_INCOMING] = new[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },
            [ComGetSubcode.GET_MODE2] = new[] { Param.PAR8, Param.PAR8 },
        };

        internal enum ComSetSubcode
        {
            SET_ON_OFF = 1,
            SET_VISIBLE = 2,
            SET_SEARCH = 3,
            SET_PIN = 5,
            SET_PASSKEY = 6,
            SET_CONNECTION = 7,
            SET_BRICKNAME = 8,
            SET_MOVEUP = 9,
            SET_MOVEDOWN = 10,
            SET_ENCRYPT = 11,
            SET_SSID = 12,
            SET_MODE2 = 13,

        }

        internal static Dictionary<Enum, Param[]> _com_set_subcode_params = new Dictionary<Enum, Param[]>
        {
            [ComSetSubcode.SET_ON_OFF] = new[] { Param.PAR8, Param.PAR8 },
            [ComSetSubcode.SET_VISIBLE] = new[] { Param.PAR8, Param.PAR8 },
            [ComSetSubcode.SET_SEARCH] = new[] { Param.PAR8, Param.PAR8 },
            [ComSetSubcode.SET_PIN] = new[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [ComSetSubcode.SET_PASSKEY] = new[] { Param.PAR8, Param.PAR8 },
            [ComSetSubcode.SET_CONNECTION] = new[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [ComSetSubcode.SET_BRICKNAME] = new[] { Param.PAR8, },
            [ComSetSubcode.SET_MOVEUP] = new[] { Param.PAR8, Param.PAR8 },
            [ComSetSubcode.SET_MOVEDOWN] = new[] { Param.PAR8, Param.PAR8 },
            [ComSetSubcode.SET_ENCRYPT] = new[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [ComSetSubcode.SET_SSID] = new[] { Param.PAR8, Param.PAR8 },
            [ComSetSubcode.SET_MODE2] = new[] { Param.PAR8, Param.PAR8 },
        };

        internal enum InputDeviceSubcode
        {
            INSERT_TYPE = 1,
            GET_FORMAT = 2,
            CAL_MINMAX = 3,
            CAL_DEFAULT = 4,
            GET_TYPEMODE = 5,
            GET_SYMBOL = 6,
            CAL_MIN = 7,
            CAL_MAX = 8,
            SETUP = 9,
            CLR_ALL = 10,
            GET_RAW = 11,
            GET_CONNECTION = 12,
            STOP_ALL = 13,
            SET_TYPEMODE = 14,
            READY_IIC = 15,
            GET_NAME = 21,
            GET_MODENAME = 22,
            SET_RAW = 23,
            GET_FIGURES = 24,
            GET_CHANGES = 25,
            CLR_CHANGES = 26,
            READY_PCT = 27,
            READY_RAW = 28,
            READY_SI = 29,
            GET_MINMAX = 30,
            GET_BUMPS = 31,

        }

        internal static Dictionary<Enum, Param[]> _input_device_subcode_params = new Dictionary<Enum, Param[]>
        {
            [InputDeviceSubcode.INSERT_TYPE] = new[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [InputDeviceSubcode.SET_TYPEMODE] = new[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },
            [InputDeviceSubcode.GET_TYPEMODE] = new[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },
            [InputDeviceSubcode.GET_CONNECTION] = new[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [InputDeviceSubcode.GET_NAME] = new[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },
            [InputDeviceSubcode.GET_SYMBOL] = new[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },
            [InputDeviceSubcode.GET_FORMAT] = new[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },
            [InputDeviceSubcode.GET_RAW] = new[] { Param.PAR8, Param.PAR8, Param.PAR32 },
            [InputDeviceSubcode.GET_MODENAME] = new[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },
            [InputDeviceSubcode.SET_RAW] = new[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR32 },
            [InputDeviceSubcode.GET_FIGURES] = new[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },
            [InputDeviceSubcode.GET_CHANGES] = new[] { Param.PAR8, Param.PAR8, Param.PARF },
            [InputDeviceSubcode.CLR_CHANGES] = new[] { Param.PAR8, Param.PAR8 },
            [InputDeviceSubcode.READY_PCT] = new[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PARNO },
            [InputDeviceSubcode.READY_RAW] = new[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PARNO },
            [InputDeviceSubcode.READY_SI] = new[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PARNO },
            [InputDeviceSubcode.GET_MINMAX] = new[] { Param.PAR8, Param.PAR8, Param.PARF, Param.PARF },
            [InputDeviceSubcode.CAL_MINMAX] = new[] { Param.PAR8, Param.PAR8, Param.PAR32, Param.PAR32 },
            [InputDeviceSubcode.CAL_DEFAULT] = new[] { Param.PAR8, Param.PAR8 },
            [InputDeviceSubcode.CAL_MIN] = new[] { Param.PAR8, Param.PAR8, Param.PAR32 },
            [InputDeviceSubcode.CAL_MAX] = new[] { Param.PAR8, Param.PAR8, Param.PAR32 },
            [InputDeviceSubcode.GET_BUMPS] = new[] { Param.PAR8, Param.PAR8, Param.PARF },
            [InputDeviceSubcode.SETUP] = new[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR16, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },
            [InputDeviceSubcode.CLR_ALL] = new[] { Param.PAR8, },
            [InputDeviceSubcode.STOP_ALL] = new[] { Param.PAR8, },
            [InputDeviceSubcode.READY_IIC] = new[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },
        };

        internal enum ProgramInfoSubcode
        {
            OBJ_STOP = 0,
            OBJ_START = 4,
            GET_STATUS = 22,
            GET_SPEED = 23,
            GET_PRGRESULT = 24,
            SET_INSTR = 25,
        }

        internal static Dictionary<Enum, Param[]> _program_info_subcode_params = new Dictionary<Enum, Param[]>
        {
            [ProgramInfoSubcode.OBJ_STOP] = new[] { Param.PAR16, Param.PAR16 },
            [ProgramInfoSubcode.OBJ_START] = new[] { Param.PAR16, Param.PAR16 },
            [ProgramInfoSubcode.GET_STATUS] = new[] { Param.PAR16, Param.PAR8 },
            [ProgramInfoSubcode.GET_SPEED] = new[] { Param.PAR16, Param.PAR32 },
            [ProgramInfoSubcode.GET_PRGRESULT] = new[] { Param.PAR16, Param.PAR8 },
            [ProgramInfoSubcode.SET_INSTR] = new[] { Param.PAR16, },
        };

        internal enum UiDrawSubcode
        {
            UPDATE = 0,
            CLEAN = 1,
            PIXEL = 2,
            LINE = 3,
            CIRCLE = 4,
            TEXT = 5,
            ICON = 6,
            PICTURE = 7,
            VALUE = 8,
            FILLRECT = 9,
            RECT = 10,
            NOTIFICATION = 11,
            QUESTION = 12,
            KEYBOARD = 13,
            BROWSE = 14,
            VERTBAR = 15,
            INVERSERECT = 16,
            SELECT_FONT = 17,
            TOPLINE = 18,
            FILLWINDOW = 19,
            SCROLL = 20,
            DOTLINE = 21,
            VIEW_VALUE = 22,
            VIEW_UNIT = 23,
            FILLCIRCLE = 24,
            STORE = 25,
            RESTORE = 26,
            ICON_QUESTION = 27,
            BMPFILE = 28,
            POPUP = 29,
            GRAPH_SETUP = 30,
            GRAPH_DRAW = 31,
            TEXTBOX = 32,
        }

        internal static Dictionary<Enum, Param[]> _ui_draw_subcode_params = new Dictionary<Enum, Param[]>
        {
            [UiDrawSubcode.UPDATE] = new Param[] { },
            [UiDrawSubcode.CLEAN] = new Param[] { },
            [UiDrawSubcode.FILLRECT] = new[] { Param.PAR8, Param.PAR16, Param.PAR16, Param.PAR16, Param.PAR16 },
            [UiDrawSubcode.RECT] = new[] { Param.PAR8, Param.PAR16, Param.PAR16, Param.PAR16, Param.PAR16 },
            [UiDrawSubcode.PIXEL] = new[] { Param.PAR8, Param.PAR16, Param.PAR16 },
            [UiDrawSubcode.LINE] = new[] { Param.PAR8, Param.PAR16, Param.PAR16, Param.PAR16, Param.PAR16 },
            [UiDrawSubcode.CIRCLE] = new[] { Param.PAR8, Param.PAR16, Param.PAR16, Param.PAR16 },
            [UiDrawSubcode.TEXT] = new[] { Param.PAR8, Param.PAR16, Param.PAR16, Param.PAR8 },
            [UiDrawSubcode.ICON] = new[] { Param.PAR8, Param.PAR16, Param.PAR16, Param.PAR8, Param.PAR8 },
            [UiDrawSubcode.PICTURE] = new[] { Param.PAR8, Param.PAR16, Param.PAR16, Param.PAR32 },
            [UiDrawSubcode.VALUE] = new[] { Param.PAR8, Param.PAR16, Param.PAR16, Param.PARF, Param.PAR8, Param.PAR8 },
            [UiDrawSubcode.NOTIFICATION] = new[] { Param.PAR8, Param.PAR16, Param.PAR16, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },
            [UiDrawSubcode.QUESTION] = new[] { Param.PAR8, Param.PAR16, Param.PAR16, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },
            [UiDrawSubcode.KEYBOARD] = new[] { Param.PAR8, Param.PAR16, Param.PAR16, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },
            [UiDrawSubcode.BROWSE] = new[] { Param.PAR8, Param.PAR16, Param.PAR16, Param.PAR16, Param.PAR16, Param.PAR8, Param.PAR8, Param.PAR8 },
            [UiDrawSubcode.VERTBAR] = new[] { Param.PAR8, Param.PAR16, Param.PAR16, Param.PAR16, Param.PAR16, Param.PAR16, Param.PAR16, Param.PAR16 },
            [UiDrawSubcode.INVERSERECT] = new[] { Param.PAR16, Param.PAR16, Param.PAR16, Param.PAR16 },
            [UiDrawSubcode.SELECT_FONT] = new[] { Param.PAR8, },
            [UiDrawSubcode.TOPLINE] = new[] { Param.PAR8, },
            [UiDrawSubcode.FILLWINDOW] = new[] { Param.PAR8, Param.PAR16, Param.PAR16 },
            [UiDrawSubcode.SCROLL] = new[] { Param.PAR16, },
            [UiDrawSubcode.DOTLINE] = new[] { Param.PAR8, Param.PAR16, Param.PAR16, Param.PAR16, Param.PAR16, Param.PAR16, Param.PAR16 },
            [UiDrawSubcode.VIEW_VALUE] = new[] { Param.PAR8, Param.PAR16, Param.PAR16, Param.PARF, Param.PAR8, Param.PAR8 },
            [UiDrawSubcode.VIEW_UNIT] = new[] { Param.PAR8, Param.PAR16, Param.PAR16, Param.PARF, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },
            [UiDrawSubcode.FILLCIRCLE] = new[] { Param.PAR8, Param.PAR16, Param.PAR16, Param.PAR16 },
            [UiDrawSubcode.STORE] = new[] { Param.PAR8, },
            [UiDrawSubcode.RESTORE] = new[] { Param.PAR8, },
            [UiDrawSubcode.ICON_QUESTION] = new[] { Param.PAR8, Param.PAR16, Param.PAR16, Param.PAR8, Param.PAR32 },
            [UiDrawSubcode.BMPFILE] = new[] { Param.PAR8, Param.PAR16, Param.PAR16, Param.PAR8 },
            [UiDrawSubcode.GRAPH_SETUP] = new[] { Param.PAR16, Param.PAR16, Param.PAR16, Param.PAR16, Param.PAR8, Param.PAR16, Param.PAR16, Param.PAR16 },
            [UiDrawSubcode.GRAPH_DRAW] = new[] { Param.PAR8, Param.PARF, Param.PARF, Param.PARF, Param.PARF },
            [UiDrawSubcode.POPUP] = new[] { Param.PAR8, },
            [UiDrawSubcode.TEXTBOX] = new[] { Param.PAR16, Param.PAR16, Param.PAR16, Param.PAR16, Param.PAR8, Param.PAR32, Param.PAR8, Param.PAR8 },
        };

        internal enum FileSubcode
        {
            OPEN_APPEND = 0,
            OPEN_READ = 1,
            OPEN_WRITE = 2,
            READ_VALUE = 3,
            WRITE_VALUE = 4,
            READ_TEXT = 5,
            WRITE_TEXT = 6,
            CLOSE = 7,
            LOAD_IMAGE = 8,
            GET_HANDLE = 9,
            MAKE_FOLDER = 10,
            GET_POOL = 11,
            SET_LOG_SYNC_TIME = 12,
            GET_FOLDERS = 13,
            GET_LOG_SYNC_TIME = 14,
            GET_SUBFOLDER_NAME = 15,
            WRITE_LOG = 16,
            CLOSE_LOG = 17,
            GET_IMAGE = 18,
            GET_ITEM = 19,
            GET_CACHE_FILES = 20,
            PUT_CACHE_FILE = 21,
            GET_CACHE_FILE = 22,
            DEL_CACHE_FILE = 23,
            DEL_SUBFOLDER = 24,
            GET_LOG_NAME = 25,

            OPEN_LOG = 27,
            READ_BYTES = 28,
            WRITE_BYTES = 29,
            REMOVE = 30,
            MOVE = 31,
        }

        internal static Dictionary<Enum, Param[]> _file_subcode_params = new Dictionary<Enum, Param[]>
        {
            [FileSubcode.OPEN_APPEND] = new[] { Param.PAR8, Param.PAR16 },
            [FileSubcode.OPEN_READ] = new[] { Param.PAR8, Param.PAR16, Param.PAR32 },
            [FileSubcode.OPEN_WRITE] = new[] { Param.PAR8, Param.PAR16 },
            [FileSubcode.READ_VALUE] = new[] { Param.PAR16, Param.PAR8, Param.PARF },
            [FileSubcode.WRITE_VALUE] = new[] { Param.PAR16, Param.PAR8, Param.PARF, Param.PAR8, Param.PAR8 },
            [FileSubcode.READ_TEXT] = new[] { Param.PAR16, Param.PAR8, Param.PAR16, Param.PAR8 },
            [FileSubcode.WRITE_TEXT] = new[] { Param.PAR16, Param.PAR8, Param.PAR8 },
            [FileSubcode.CLOSE] = new[] { Param.PAR16, },
            [FileSubcode.LOAD_IMAGE] = new[] { Param.PAR16, Param.PAR8, Param.PAR32, Param.PAR32 },
            [FileSubcode.GET_HANDLE] = new[] { Param.PAR8, Param.PAR16, Param.PAR8 },
            [FileSubcode.MAKE_FOLDER] = new[] { Param.PAR8, Param.PAR8 },
            [FileSubcode.GET_LOG_NAME] = new[] { Param.PAR8, Param.PAR8 },
            [FileSubcode.GET_POOL] = new[] { Param.PAR32, Param.PAR16, Param.PAR32 },
            [FileSubcode.GET_FOLDERS] = new[] { Param.PAR8, Param.PAR8 },
            [FileSubcode.GET_SUBFOLDER_NAME] = new[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },
            [FileSubcode.WRITE_LOG] = new[] { Param.PAR16, Param.PAR32, Param.PAR8, Param.PARF },
            [FileSubcode.CLOSE_LOG] = new[] { Param.PAR16, Param.PAR8 },
            [FileSubcode.SET_LOG_SYNC_TIME] = new[] { Param.PAR32, Param.PAR32 },
            [FileSubcode.DEL_SUBFOLDER] = new[] { Param.PAR8, Param.PAR8 },
            [FileSubcode.GET_LOG_SYNC_TIME] = new[] { Param.PAR32, Param.PAR32 },
            [FileSubcode.GET_IMAGE] = new[] { Param.PAR8, Param.PAR16, Param.PAR8, Param.PAR32 },
            [FileSubcode.GET_ITEM] = new[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [FileSubcode.GET_CACHE_FILES] = new[] { Param.PAR8, },
            [FileSubcode.GET_CACHE_FILE] = new[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [FileSubcode.PUT_CACHE_FILE] = new[] { Param.PAR8, },
            [FileSubcode.DEL_CACHE_FILE] = new[] { Param.PAR8, },
            [FileSubcode.OPEN_LOG] = new[] { Param.PAR8, Param.PAR32, Param.PAR32, Param.PAR32, Param.PAR32, Param.PAR32, Param.PAR8, Param.PAR16 },
            [FileSubcode.READ_BYTES] = new[] { Param.PAR16, Param.PAR16, Param.PAR8 },
            [FileSubcode.WRITE_BYTES] = new[] { Param.PAR16, Param.PAR16, Param.PAR8 },
            [FileSubcode.REMOVE] = new[] { Param.PAR8, },
            [FileSubcode.MOVE] = new[] { Param.PAR8, Param.PAR8 },
        };

        internal enum ArraySubcode
        {
            DELETE = 0,
            CREATE8 = 1,
            CREATE16 = 2,
            CREATE32 = 3,
            CREATEF = 4,
            RESIZE = 5,
            FILL = 6,
            COPY = 7,
            INIT8 = 8,
            INIT16 = 9,
            INIT32 = 10,
            INITF = 11,
            SIZE = 12,
            READ_CONTENT = 13,
            WRITE_CONTENT = 14,
            READ_SIZE = 15,
            // # File name subcodes
            EXIST = 16,
            TOTALSIZE = 17,
            SPLIT = 18,
            MERGE = 19,
            CHECK = 20,
            PACK = 21,
            UNPACK = 22,
            GET_FOLDERNAME = 23,
        }

        internal static Dictionary<Enum, Param[]> _array_subcode_params = new Dictionary<Enum, Param[]>
        {
            [ArraySubcode.CREATE8] = new[] { Param.PAR32, Param.PAR16 },
            [ArraySubcode.CREATE16] = new[] { Param.PAR32, Param.PAR16 },
            [ArraySubcode.CREATE32] = new[] { Param.PAR32, Param.PAR16 },
            [ArraySubcode.CREATEF] = new[] { Param.PAR32, Param.PAR16 },
            [ArraySubcode.RESIZE] = new[] { Param.PAR16, Param.PAR32 },
            [ArraySubcode.DELETE] = new[] { Param.PAR16, },
            [ArraySubcode.FILL] = new[] { Param.PAR16, Param.PARV },
            [ArraySubcode.COPY] = new[] { Param.PAR16, Param.PAR16 },
            [ArraySubcode.INIT8] = new[] { Param.PAR16, Param.PAR32, Param.PAR32, Param.PARVALUES, Param.PAR8 },
            [ArraySubcode.INIT16] = new[] { Param.PAR16, Param.PAR32, Param.PAR32, Param.PARVALUES, Param.PAR16 },
            [ArraySubcode.INIT32] = new[] { Param.PAR16, Param.PAR32, Param.PAR32, Param.PARVALUES, Param.PAR32 },
            [ArraySubcode.INITF] = new[] { Param.PAR16, Param.PAR32, Param.PAR32, Param.PARVALUES, Param.PARF },
            [ArraySubcode.SIZE] = new[] { Param.PAR16, Param.PAR32 },
            [ArraySubcode.READ_CONTENT] = new[] { Param.PAR16, Param.PAR16, Param.PAR32, Param.PAR32, Param.PAR8 },
            [ArraySubcode.WRITE_CONTENT] = new[] { Param.PAR16, Param.PAR16, Param.PAR32, Param.PAR32, Param.PAR8 },
            [ArraySubcode.READ_SIZE] = new[] { Param.PAR16, Param.PAR16, Param.PAR32 },
            // # FileSubcode
            [ArraySubcode.EXIST] = new[] { Param.PAR8, Param.PAR8 },
            [ArraySubcode.TOTALSIZE] = new[] { Param.PAR8, Param.PAR32, Param.PAR32 },
            [ArraySubcode.SPLIT] = new[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },
            [ArraySubcode.MERGE] = new[] { Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8, Param.PAR8 },
            [ArraySubcode.CHECK] = new[] { Param.PAR8, Param.PAR8 },
            [ArraySubcode.PACK] = new[] { Param.PAR8, },
            [ArraySubcode.UNPACK] = new[] { Param.PAR8, },
            [ArraySubcode.GET_FOLDERNAME] = new[] { Param.PAR8, Param.PAR8 },
        };

        public enum InfoSubcode : int
        {
            SET_ERROR = 1,
            GET_ERROR = 2,
            ERRORTEXT = 3,

            GET_VOLUME = 4,
            SET_VOLUME = 5,
            GET_MINUTES = 6,
            SET_MINUTES = 7,
            // # Test subcodes
            TST_OPEN = 10,
            TST_CLOSE = 11,
            TST_READ_PINS = 12,
            TST_WRITE_PINS = 13,
            TST_READ_ADC = 14,
            TST_WRITE_UART = 15,
            TST_READ_UART = 16,
            TST_ENABLE_UART = 17,
            TST_DISABLE_UART = 18,
            TST_ACCU_SWITCH = 19,
            TST_BOOT_MODE2 = 20,
            TST_POLL_MODE2 = 21,
            TST_CLOSE_MODE2 = 22,
            TST_RAM_CHECK = 23,
        }

        internal static Dictionary<Enum, Param[]> _info_subcode_params = new Dictionary<Enum, Param[]>
        {
            [InfoSubcode.SET_ERROR] = new[] { Param.PAR8, },
            [InfoSubcode.GET_ERROR] = new[] { Param.PAR8, },
            [InfoSubcode.ERRORTEXT] = new[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [InfoSubcode.GET_VOLUME] = new[] { Param.PAR8, },
            [InfoSubcode.SET_VOLUME] = new[] { Param.PAR8, },
            [InfoSubcode.GET_MINUTES] = new[] { Param.PAR8, },
            [InfoSubcode.SET_MINUTES] = new[] { Param.PAR8, },
            // # TestSubcode
            [InfoSubcode.TST_OPEN] = new Param[] { },
            [InfoSubcode.TST_CLOSE] = new Param[] { },
            [InfoSubcode.TST_READ_PINS] = new[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [InfoSubcode.TST_WRITE_PINS] = new[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [InfoSubcode.TST_READ_ADC] = new[] { Param.PAR8, Param.PAR16 },
            [InfoSubcode.TST_WRITE_UART] = new[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [InfoSubcode.TST_READ_UART] = new[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [InfoSubcode.TST_ENABLE_UART] = new[] { Param.PAR32, },
            [InfoSubcode.TST_DISABLE_UART] = new Param[] { },
            [InfoSubcode.TST_ACCU_SWITCH] = new[] { Param.PAR8, },
            [InfoSubcode.TST_BOOT_MODE2] = new Param[] { },
            [InfoSubcode.TST_POLL_MODE2] = new[] { Param.PAR8, },
            [InfoSubcode.TST_CLOSE_MODE2] = new Param[] { },
            [InfoSubcode.TST_RAM_CHECK] = new[] { Param.PAR8, },
        };

        public enum SoundSubcode : int
		{
            BREAK = 0,
            TONE = 1,
            PLAY = 2,
            REPEAT = 3,
            SERVICE = 4,
        }

        internal static Dictionary<Enum, Param[]> _sound_subcode_params = new Dictionary<Enum, Param[]>
        {
            [SoundSubcode.BREAK] = new Param[] { },
            [SoundSubcode.TONE] = new[] { Param.PAR8, Param.PAR16, Param.PAR16 },
            [SoundSubcode.PLAY] = new[] { Param.PAR8, Param.PARS },
            [SoundSubcode.REPEAT] = new[] { Param.PAR8, Param.PARS },
            [SoundSubcode.SERVICE] = new Param[] { },
        };

        internal enum StringSubcode
        {
            GET_SIZE = 1,
            ADD = 2,
            COMPARE = 3,
            DUPLICATE = 5,
            VALUE_TO_STRING = 6,
            STRING_TO_VALUE = 7,
            STRIP = 8,
            NUMBER_TO_STRING = 9,
            SUB = 10,
            VALUE_FORMATTED = 11,
            NUMBER_FORMATTED = 12,
        }

        internal static Dictionary<Enum, Param[]> _string_subcode_params = new Dictionary<Enum, Param[]>
        {
            [StringSubcode.GET_SIZE] = new[] { Param.PAR8, Param.PAR16 },
            [StringSubcode.ADD] = new[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [StringSubcode.COMPARE] = new[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [StringSubcode.DUPLICATE] = new[] { Param.PAR8, Param.PAR8 },
            [StringSubcode.VALUE_TO_STRING] = new[] { Param.PARF, Param.PAR8, Param.PAR8, Param.PAR8 },
            [StringSubcode.STRING_TO_VALUE] = new[] { Param.PAR8, Param.PARF },
            [StringSubcode.STRIP] = new[] { Param.PAR8, Param.PAR8 },
            [StringSubcode.NUMBER_TO_STRING] = new[] { Param.PAR16, Param.PAR8, Param.PAR8 },
            [StringSubcode.SUB] = new[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [StringSubcode.VALUE_FORMATTED] = new[] { Param.PARF, Param.PAR8, Param.PAR8, Param.PAR8 },
            [StringSubcode.NUMBER_FORMATTED] = new[] { Param.PAR32, Param.PAR8, Param.PAR8, Param.PAR8 },
        };

        internal enum DeviceType
        {
            MODE_KEEP = -1,
            TYPE_KEEP = 0,

            //  # Types defined in "typedata.rcf"
            TYPE_NXT_TOUCH = 1,
            TYPE_NXT_LIGHT = 2,
            TYPE_NXT_SOUND = 3,
            TYPE_NXT_COLOR = 4,
            TYPE_NXT_ULTRASONIC = 5,
            TYPE_NXT_TEMPERATURE = 6,
            TYPE_TACHO = 7,
            TYPE_MINITACHO = 8,
            TYPE_NEWTACHO = 9,

            TYPE_TOUCH = 16,

            // # Types defined in known EV3/UART sensors
            TYPE_COLOR = 29,
            TYPE_ULTRASONIC = 30,
            TYPE_GYRO = 32,
            TYPE_IR = 33,

            // # Type range reserved for third party devices
            TYPE_THIRD_PARTY_START = 50,
            TYPE_THIRD_PARTY_END = 98,
            TYPE_ENERGYMETER = 99,
            TYPE_IIC_UNKNOWN = 100,
            TYPE_NXT_TEST = 101,

            TYPE_NXT_IIC = 123,
            TYPE_TERMINAL = 124,
            TYPE_UNKNOWN = 125,
            TYPE_NONE = 126,
            TYPE_ERROR = 127,
        }
        public enum Slot : int
        {
            GUI_SLOT = 0,
            USER_SLOT = 1,
            CMD_SLOT = 2,
            TERM_SLOT = 3,
            DEBUG_SLOT = 4,
            // # ONLY VALID IN opPROGRAM_STOP
            CURRENT_SLOT = -1,
        }

        public enum ButtonType
        {
            NO_BUTTON = 0,
            UP_BUTTON = 1,
            ENTER_BUTTON = 2,
            DOWN_BUTTON = 3,
            RIGHT_BUTTON = 4,
            LEFT_BUTTON = 5,
            BACK_BUTTON = 6,
            ANY_BUTTON = 7,
        }

        internal enum MathSubcode
        {
            EXP = 1,
            MOD = 2,
            FLOOR = 3,
            CEIL = 4,
            ROUND = 5,
            ABS = 6,
            NEGATE = 7,
            SQRT = 8,
            LOG = 9,
            LN = 10,
            SIN = 11,
            COS = 12,
            TAN = 13,
            ASIN = 14,
            ACOS = 15,
            ATAN = 16,
            MOD8 = 17,
            MOD16 = 18,
            MOD32 = 19,
            POW = 20,
            TRUNC = 21,
        }

        internal static Dictionary<Enum, Param[]> _math_subcode_params = new Dictionary<Enum, Param[]>
        {
            [MathSubcode.EXP] = new[] { Param.PARF, Param.PARF },
            [MathSubcode.MOD] = new[] { Param.PARF, Param.PARF, Param.PARF },
            [MathSubcode.FLOOR] = new[] { Param.PARF, Param.PARF },
            [MathSubcode.CEIL] = new[] { Param.PARF, Param.PARF },
            [MathSubcode.ROUND] = new[] { Param.PARF, Param.PARF },
            [MathSubcode.ABS] = new[] { Param.PARF, Param.PARF },
            [MathSubcode.NEGATE] = new[] { Param.PARF, Param.PARF },
            [MathSubcode.SQRT] = new[] { Param.PARF, Param.PARF },
            [MathSubcode.LOG] = new[] { Param.PARF, Param.PARF },
            [MathSubcode.LN] = new[] { Param.PARF, Param.PARF },
            [MathSubcode.SIN] = new[] { Param.PARF, Param.PARF },
            [MathSubcode.COS] = new[] { Param.PARF, Param.PARF },
            [MathSubcode.TAN] = new[] { Param.PARF, Param.PARF },
            [MathSubcode.ASIN] = new[] { Param.PARF, Param.PARF },
            [MathSubcode.ACOS] = new[] { Param.PARF, Param.PARF },
            [MathSubcode.ATAN] = new[] { Param.PARF, Param.PARF },
            [MathSubcode.MOD8] = new[] { Param.PAR8, Param.PAR8, Param.PAR8 },
            [MathSubcode.MOD16] = new[] { Param.PAR16, Param.PAR16, Param.PAR16 },
            [MathSubcode.MOD32] = new[] { Param.PAR32, Param.PAR32, Param.PAR32 },
            [MathSubcode.POW] = new[] { Param.PARF, Param.PARF, Param.PARF },
            [MathSubcode.TRUNC] = new[] { Param.PARF, Param.PAR8, Param.PARF },
        };

		#endregion SUBCODES
		// ----------------------------------------

		//---

		/*! \enum DSPSTAT
 *
 *        Dispatch status values
 */
		public enum DSPSTAT
		{
			NOBREAK = 0x0100,               //!< Dispatcher running (looping)
			STOPBREAK = 0x0200,               //!< Break because of program stop
			SLEEPBREAK = 0x0400,               //!< Break because of sleeping
			INSTRBREAK = 0x0800,               //!< Break because of opcode break
			BUSYBREAK = 0x1000,               //!< Break because of waiting for completion
			PRGBREAK = 0x2000,               //!< Break because of program break
			USERBREAK = 0x4000,               //!< Break because of user decision
			FAILBREAK = 0x8000                //!< Break because of fail
		}

        public enum BrowserType : byte
        {
            BROWSE_FOLDERS = 0,
            BROWSE_FOLDS_FILES = 1,
            BROWSE_CACHE = 2,
            BROWSE_FILES = 3
        }

        public enum FontType : byte
        {
            NORMAL_FONT = 0,
            SMALL_FONT = 1,
            LARGE_FONT = 2,
            TINY_FONT = 3
        }

        public enum IconType : byte
        {
            NORMAL_ICON = 0,
            SMALL_ICON = 1,
            LARGE_ICON = 2,
            MENU_ICON = 3,
            ARROW_ICON = 4
        }

        public enum StatusIcon : byte
        {
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

            SICON_USB = 21
        }

        public enum NIcon : sbyte
        {
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
            ICON_BRICK1 = 34
        }

        public enum LIcon : byte
        {
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

            WARN_MEMORY = 27
        }

        public enum MIcon : byte
        {
            ICON_STAR = 0,
            ICON_LOCKSTAR = 1,
            ICON_LOCK = 2,
            ICON_PC = 3,
            ICON_PHONE = 4,
            ICON_BRICK = 5,
            ICON_UNKNOWN = 6,
            ICON_FROM_FOLDER = 7,
            ICON_CHECKBOX = 8,
            ICON_CHECKED = 9,
            ICON_XED = 10
        }

        public enum AIcon : byte 
        {
            ICON_LEFT = 1,
            ICON_RIGHT = 2
        }

        public enum BluetoothType : byte
        {
            BTTYPE_PC = 3,
            BTTYPE_PHONE = 4,
            BTTYPE_BRICK = 5,
            BTTYPE_UNKNOWN = 6
        }

        internal enum LedPattern : byte
        {
            LED_BLACK = 0,
            LED_GREEN = 1,
            LED_RED = 2,
            LED_ORANGE = 3,
            LED_GREEN_FLASH = 4,
            LED_RED_FLASH = 5,
            LED_ORANGE_FLASH = 6,
            LED_GREEN_PULSE = 7,
            LED_RED_PULSE = 8,
            LED_ORANGE_PULSE = 9
        }

        internal enum LedType : byte
		{
            LED_ALL = 0,
            LED_RR = 1,
            LED_RG = 2,
            LED_LR = 3,
            LED_LG = 4
        }

        public enum FileType
        {
            FILETYPE_UNKNOWN = 0x00,
            TYPE_FOLDER = 0x01,
            TYPE_SOUND = 0x02,
            TYPE_BYTECODE = 0x03,
            TYPE_GRAPHICS = 0x04,
            TYPE_DATALOG = 0x05,
            TYPE_PROGRAM = 0x06,
            TYPE_TEXT = 0x07,
            TYPE_SDCARD = 0x10,
            TYPE_USBSTICK = 0x20,

            TYPE_RESTART_BROWSER = -1,
            TYPE_REFRESH_BROWSER = -2
        }

        public enum Result
        {
            OK = 0,
            BUSY = 1,
            FAIL = 2,
            STOP = 4,
            START = 8
        }

        internal enum Delimeter
        {
            DEL_NONE = 0,
            DEL_TAB = 1,
            DEL_SPACE = 2,
            DEL_RETURN = 3,
            DEL_COLON = 4,
            DEL_COMMA = 5,
            DEL_LINEFEED = 6,
            DEL_CRLF = 7
        }

        internal enum HardwareTransportLayer
        {
            HW_USB = 1,
            HW_BT = 2,
            HW_WIFI = 3
        }

        internal enum EncryptionType
        {
            ENCRYPT_NONE = 0,
            ENCRYPT_WPA2 = 1
        }

        internal enum Color
        {
            RED = 0,
            GREEN = 1,
            BLUE = 2,
            BLANK = 3
        }

        internal enum NxtColor
        {
            BLACKCOLOR = 1,
            BLUECOLOR = 2,
            GREENCOLOR = 3,
            YELLOWCOLOR = 4,
            REDCOLOR = 5,
            WHITECOLOR = 6
        }

        public enum Warning : byte
        {
            WARNING_TEMP = 0x01,
            WARNING_CURRENT = 0x02,
            WARNING_VOLTAGE = 0x04,
            WARNING_MEMORY = 0x08,
            WARNING_DSPSTAT = 0x10,
            WARNING_RAM = 0x20,
            WARNING_BATTLOW = 0x40,
            WARNING_BUSY = 0x80,

            WARNINGS = 0x3F
        }

		public enum ObjectStatus
        {
            RUNNING = 0x0010,
            WAITING = 0x0020,
            STOPPED = 0x0040,
            HALTED = 0x0080
        }

		public enum DeviceCommand
        {
            DEVCMD_RESET = 0x11,
            DEVCMD_FIRE = 0x11,
            DEVCMD_CHANNEL = 0x12
        }

        internal static Dictionary<Subparam, Type> _subcode_enums = new Dictionary<Subparam, Type>
        {
            [Subparam.PROGRAM_SUBP] = typeof(ProgramInfoSubcode),
            [Subparam.FILE_SUBP] = typeof(FileSubcode),
            [Subparam.ARRAY_SUBP] = typeof(ArraySubcode),
            [Subparam.FILENAME_SUBP] = typeof(ArraySubcode),
            [Subparam.VM_SUBP] = typeof(InfoSubcode),
            [Subparam.STRING_SUBP] = typeof(StringSubcode),
            [Subparam.UI_READ_SUBP] = typeof(UiReadSubcode),
            [Subparam.UI_WRITE_SUBP] = typeof(UiWriteSubcode),
            [Subparam.UI_DRAW_SUBP] = typeof(UiDrawSubcode),
            [Subparam.UI_BUTTON_SUBP] = typeof(UiButtonSubcode),
            [Subparam.COM_READ_SUBP] = typeof(ComReadSubcode),
            [Subparam.COM_WRITE_SUBP] = typeof(ComWriteSubcode),
            [Subparam.SOUND_SUBP] = typeof(SoundSubcode),
            [Subparam.INPUT_SUBP] = typeof(InputDeviceSubcode),
            [Subparam.MATH_SUBP] = typeof(MathSubcode),
            [Subparam.COM_GET_SUBP] = typeof(ComGetSubcode),
            [Subparam.COM_SET_SUBP] = typeof(ComSetSubcode),
        };

        internal static Dictionary<Subparam, Dictionary<Enum, Param[]>> _subcode_params = new Dictionary<Subparam, Dictionary<Enum, Param[]>>
        {
            [Subparam.PROGRAM_SUBP] = _program_info_subcode_params,
            [Subparam.FILE_SUBP] = _file_subcode_params,
            [Subparam.ARRAY_SUBP] = _array_subcode_params,
            [Subparam.FILENAME_SUBP] = _array_subcode_params,
            [Subparam.VM_SUBP] = _info_subcode_params,
            [Subparam.STRING_SUBP] = _string_subcode_params,
            [Subparam.UI_READ_SUBP] = _ui_read_subcode_params,
            [Subparam.UI_WRITE_SUBP] = _ui_write_subcode_params,
            [Subparam.UI_DRAW_SUBP] = _ui_draw_subcode_params,
            [Subparam.UI_BUTTON_SUBP] = _ui_button_subcode_params,
            [Subparam.COM_READ_SUBP] = _com_read_subcode_params,
            [Subparam.COM_WRITE_SUBP] = _com_write_subcode_params,
            [Subparam.SOUND_SUBP] = _sound_subcode_params,
            [Subparam.INPUT_SUBP] = _input_device_subcode_params,
            [Subparam.MATH_SUBP] = _math_subcode_params,
            [Subparam.COM_GET_SUBP] = _com_get_subcode_params,
            [Subparam.COM_SET_SUBP] = _com_set_subcode_params,
        };

        // ----
        internal enum DataDirection
        {
            IN = 0x80,
            OUT = 0x40
        }

		public enum Error
        {
			TOO_MANY_ERRORS_TO_BUFFER,
			TYPEDATA_TABLE_FULL,
			TYPEDATA_FILE_NOT_FOUND,
			ANALOG_DEVICE_FILE_NOT_FOUND,
			ANALOG_SHARED_MEMORY,
			UART_DEVICE_FILE_NOT_FOUND,
			UART_SHARED_MEMORY,
			IIC_DEVICE_FILE_NOT_FOUND,
			IIC_SHARED_MEMORY,
			DISPLAY_SHARED_MEMORY,
			OUTPUT_SHARED_MEMORY,
			COM_COULD_NOT_OPEN_FILE,
			COM_NAME_TOO_SHORT,
			COM_NAME_TOO_LONG,
			COM_INTERNAL,
			VM_INTERNAL,
			VM_PROGRAM_VALIDATION,
			VM_PROGRAM_NOT_STARTED,
			VM_PROGRAM_FAIL_BREAK,
			VM_PROGRAM_INSTRUCTION_BREAK,
			VM_PROGRAM_NOT_FOUND,
			FILE_OPEN_ERROR,
			FILE_READ_ERROR,
			FILE_WRITE_ERROR,
			FILE_CLOSE_ERROR,
			FILE_GET_HANDLE_ERROR,
			FILE_NAME_ERROR,
			USB_SHARED_MEMORY,
			OUT_OF_MEMORY,
			ERRORS
		}

        public enum Callparam
        {
            IN_8 = DataDirection.IN | DataFormat.DATA8,
            IN_16 = DataDirection.IN | DataFormat.DATA16,
            IN_32 = DataDirection.IN | DataFormat.DATA32,
            IN_F = DataDirection.IN | DataFormat.DATAF,
            IN_S = DataDirection.IN | DataFormat.DATAS,
            OUT_8 = DataDirection.OUT | DataFormat.DATA8,
            OUT_16 = DataDirection.OUT | DataFormat.DATA16,
            OUT_32 = DataDirection.OUT | DataFormat.DATA32,
            OUT_F = DataDirection.OUT | DataFormat.DATAF,
            OUT_S = DataDirection.OUT | DataFormat.DATAS,
            IO_8 = IN_8 | OUT_8,
            IO_16 = IN_16 | OUT_16,
            IO_32 = IN_32 | OUT_32,
            IO_F = IN_F | OUT_F,
            IO_S = IN_S | OUT_S
        }
        internal static DataFormat CallParam2DataFormat(Callparam cp)
        {
            return (DataFormat)((int)cp & 0x3F);
        }

		public static byte[] ValidChars =
        {
          0x00,   // 0x00      NUL
          0x00,   // 0x01      SOH
          0x00,   // 0x02      STX
          0x00,   // 0x03      ETX
          0x00,   // 0x04      EOT
          0x00,   // 0x05      ENQ
          0x00,   // 0x06      ACK
          0x00,   // 0x07      BEL
          0x00,   // 0x08      BS
          0x00,   // 0x09      TAB
          0x00,   // 0x0A      LF
          0x00,   // 0x0B      VT
          0x00,   // 0x0C      FF
          0x00,   // 0x0D      CR
          0x00,   // 0x0E      SO
          0x00,   // 0x0F      SI
          0x00,   // 0x10      DLE
          0x00,   // 0x11      DC1
          0x00,   // 0x12      DC2
          0x00,   // 0x13      DC3
          0x00,   // 0x14      DC4
          0x00,   // 0x15      NAK
          0x00,   // 0x16      SYN
          0x00,   // 0x17      ETB
          0x00,   // 0x18      CAN
          0x00,   // 0x19      EM
          0x00,   // 0x1A      SUB
          0x00,   // 0x1B      ESC
          0x00,   // 0x1C      FS
          0x00,   // 0x1D      GS
          0x00,   // 0x1E      RS
          0x00,   // 0x1F      US
          0x12,   // 0x20      (space)
          0x00,   // 0x21      !
          0x00,   // 0x22      "
          0x00,   // 0x23      #
          0x00,   // 0x24      $
          0x00,   // 0x25      %
          0x00,   // 0x26      &
          0x00,   // 0x27      '
          0x00,   // 0x28      (
          0x00,   // 0x29      )
          0x00,   // 0x2A      *
          0x00,   // 0x2B      +
          0x00,   // 0x2C      ,
          0x03,   // 0x2D      -
          0x02,   // 0x2E      .
          0x02,   // 0x2F      /
          0x1F,   // 0x30      0
          0x1F,   // 0x31      1
          0x1F,   // 0x32      2
          0x1F,   // 0x33      3
          0x1F,   // 0x34      4
          0x1F,   // 0x35      5
          0x1F,   // 0x36      6
          0x1F,   // 0x37      7
          0x1F,   // 0x38      8
          0x1F,   // 0x39      9
          0x00,   // 0x3A      :
          0x00,   // 0x3B      ;
          0x00,   // 0x3C      <
          0x00,   // 0x3D      =
          0x00,   // 0x3E      >
          0x00,   // 0x3F      ?
          0x00,   // 0x40      @
          0x1F,   // 0x41      A
          0x1F,   // 0x42      B
          0x1F,   // 0x43      C
          0x1F,   // 0x44      D
          0x1F,   // 0x45      E
          0x1F,   // 0x46      F
          0x1F,   // 0x47      G
          0x1F,   // 0x48      H
          0x1F,   // 0x49      I
          0x1F,   // 0x4A      J
          0x1F,   // 0x4B      K
          0x1F,   // 0x4C      L
          0x1F,   // 0x4D      M
          0x1F,   // 0x4E      N
          0x1F,   // 0x4F      O
          0x1F,   // 0x50      P
          0x1F,   // 0x51      Q
          0x1F,   // 0x52      R
          0x1F,   // 0x53      S
          0x1F,   // 0x54      T
          0x1F,   // 0x55      U
          0x1F,   // 0x56      V
          0x1F,   // 0x57      W
          0x1F,   // 0x58      X
          0x1F,   // 0x59      Y
          0x1F,   // 0x5A      Z
          0x00,   // 0x5B      [
          0x00,   // 0x5C      '\'
          0x00,   // 0x5D      ]
          0x00,   // 0x5E      ^
          0x1F,   // 0x5F      _
          0x00,   // 0x60      `
          0x1F,   // 0x61      a
          0x1F,   // 0x62      b
          0x1F,   // 0x63      c
          0x1F,   // 0x64      d
          0x1F,   // 0x65      e
          0x1F,   // 0x66      f
          0x1F,   // 0x67      g
          0x1F,   // 0x68      h
          0x1F,   // 0x69      i
          0x1F,   // 0x6A      j
          0x1F,   // 0x6B      k
          0x1F,   // 0x6C      l
          0x1F,   // 0x6D      m
          0x1F,   // 0x6E      n
          0x1F,   // 0x6F      o
          0x1F,   // 0x70      p
          0x1F,   // 0x71      q
          0x1F,   // 0x72      r
          0x1F,   // 0x73      s
          0x1F,   // 0x74      t
          0x1F,   // 0x75      u
          0x1F,   // 0x76      v
          0x1F,   // 0x77      w
          0x1F,   // 0x78      x
          0x1F,   // 0x79      y
          0x1F,   // 0x7A      z
          0x00,   // 0x7B      {
          0x00,   // 0x7C      |
          0x00,   // 0x7D      }
          0x02,   // 0x7E      ~
          0x00    // 0x7F      
        };
	}
}
