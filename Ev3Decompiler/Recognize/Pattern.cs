using EV3ModelLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EV3DecompilerLib.Recognize
{
    /// <summary>
    /// Pattern class for matching a EV3 command pattern
    /// </summary>
    [DebuggerDisplay("{Name}")]
    internal class Pattern
    {
        #region Block name constants
        internal const string BLOCK_MathAdvanced = "Math.Advanced";
        internal const string BLOCK_Interrupt = Node.BLOCK_Interrupt;
        internal const string BLOCK_StartBlock = Node.BLOCK_StartBlock;
        internal const string BLOCK_Fork = Node.BLOCK_Fork;
        internal const string BLOCK_ForkItem = Node.BLOCK_ForkItem;
        internal const string BLOCK_SwitchCaseItem = Node.BLOCK_SwitchCaseItem;
        internal const string BLOCK_StartLoop = "StartLoop"; // temporary block only
        internal const string BLOCK_WaitForPrefix = Node.BLOCK_WaitForPrefix;
        internal const string BLOCK_MyBlockPrefix = Node.BLOCK_MyBlockPrefix;
        internal const string BLOCK_ReadVariablePrefix = "Variable.Read";
        internal const string BLOCK_WriteVariablePrefix = "Variable.Write";
        internal const string BLOCK_DataType_Text = "Text";
        internal const string BLOCK_DataType_Boolean = "Boolean";
        internal const string BLOCK_DataType_Numeric = "Numeric";
        internal const string BLOCK_DataType_ArrayPostFix = "Array";
        internal const string BLOCK_SwitchPrefix = Node.BLOCK_SwitchPrefix;
        internal const string BLOCK_CaseSelectorString = "CaseSelector.String";
        internal const string CASE_DEFAULT_Value = "DEFAULT*";

        // special blocks to handle
        internal const string BLOCK_MergeToFork = Node.BLOCK_MergeToFork;
        internal const string BLOCK_StartLoopDummy = Node.BLOCK_StartLoopDummy;
        internal const string BLOCK_NoMatchFound = Node.BLOCK_NoMatchFound;
        #endregion Block name constants

        #region Control statements and Special Signatures 
        internal static readonly List<PatternElem> MYBLOCK_PATTERN = Pattern.ParsePatternToElements("(ARRAY(CREATE) | OR32(LOCAL))*;MOVE32_32;MOVE8_8;JR_FALSE;ADD32;JR");
        internal static readonly List<PatternElem> MYBLOCK_PATTERN_WITH_INSTANT_FORK = Pattern.ParsePatternToElements("OR32(LOCAL)+;?MOVEF_F;OBJECT_TRIG+;OBJECT_WAIT+;$RETURN");
        internal static readonly List<PatternElem> LOOP_START_PATTERN = Pattern.ParsePatternToElements("MOVE32_32(GLOBAL,LOCAL);AND32;CP_GT32;JR_EQ8;JR_EQ8;JR;XOR32;MOVE32_32;TIMER_WAIT;TIMER_READY;JR;?OR32;(MOVE32_32|MOVE32_F|MOVEF_32)*"); // sometimes last MOVEs does not exist
        internal static readonly List<PatternElem> WAITFOR_LOOPEND_PATTERN = Pattern.ParsePatternToElements("JR_TRUE;ADD32;JR");
        // might start with ?MOVE32_32;?MOVE32_F;
        internal static readonly List<PatternElem> VARIABLE_WRITE_PATTERN = Pattern.ParsePatternToElements("?ADD32(GLOBAL,1,GLOBAL);MOVE32_32(GLOBAL,LOCAL);AND32(LOCAL,,LOCAL);CP_GT32(LOCAL,0,LOCAL);JR_EQ8(LOCAL,0);JR_EQ8(LOCAL,1);(MOVE8_F(LOCAL,LOCAL)|STRINGS(VALUE_TO_STRING,LOCAL)|ARRAY_APPEND(LOCAL,LOCAL)|ARRAY(COPY,,LOCAL))*;(MOVEF_F(,GLOBAL) | MOVE8_8(,GLOBAL) | ARRAY(COPY,,GLOBAL));JR;JR;?ADD32(GLOBAL,1,GLOBAL)");
        //TODO: variable conversions ==> MOVE8_F is consumed; yet ignore STRINGS(VALUE_TO_STRING conversion as this happens later
        //ADD32(GLOBAL,1,@samevalue.@param.1);MOVE32_32;AND32(LOCAL,0);CP_GT32(LOCAL,0,GLOBAL);JR_EQ8(LOCAL,0,GLOBAL);JR_EQ8(LOCAL,1,GLOBAL);MOVEF_F(@out.value);JR(@savetemp.jump1);JR(@samevalue.jump1);ADD32(GLOBAL,1,@samevalue.@param.1);
        internal static readonly List<PatternElem> VARIABLE_READ_PATTERN = Pattern.ParsePatternToElements("(MOVEF_F(GLOBAL) | MOVE8_8(GLOBAL) | ARRAY(COPY,GLOBAL))");
        internal static readonly List<PatternElem> CALL_BLOCK_PATTERN_MAIN = Pattern.ParsePatternToElements("ADD32(GLOBAL,1,GLOBAL);CALL;ADD32(GLOBAL,1,GLOBAL)");
        internal static readonly List<PatternElem> CALL_BLOCK_PATTERN = Pattern.ParsePatternToElements("?ADD32(GLOBAL,1,GLOBAL);CALL;?ADD32(GLOBAL,1,GLOBAL)");
        internal static readonly List<PatternElem> MATHADV_BLOCK_PATTERN = Pattern.ParsePatternToElements("(SUBF | ADDF | MULF | DIVF | MATH)+");
        #endregion Control statements and Special Signatures 

        #region instance part
        /// <summary>
        /// Name of the Pattern
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Match Type restrictions
        /// </summary>
        public enum MatchTypeEnum { None, NoSubCalls, NoSubCallsExact };
        public MatchTypeEnum MatchType { get; set; }

        /// <summary>
        /// In testmode only selected patterns are matched
        /// </summary>
        public bool TestMode { get; set; }

        /// <summary>
        /// Elements
        /// </summary>
        [JsonIgnore]
        public List<PatternElem> Elems { get; set; }
        public string ElemsBacking;

        /// <summary>
        /// Set elements
        /// </summary>
        /// <param name="value"></param>
        public void SetElems(string value)
        {
            ElemsBacking = value;

            List<PatternElem> elems = ParsePatternToElements(value);
            this.Elems = elems;
        }

        /// <summary>
        /// Output parameters and names
        /// </summary>
        public string ParamsOut { get; set; }

        /// <summary>
        /// Parse pattern strings to a list of Pattern Elements
        /// </summary>
        /// <param name="patternString"></param>
        /// <returns></returns>
        internal static List<PatternElem> ParsePatternToElements(string patternString)
        {
            var values = patternString.Split(';').ToList()
                .Where(elem => !string.IsNullOrEmpty(elem))
                .Select(elem => { return elem.Trim(); }).ToArray();

            var elems = new List<PatternElem>();
            foreach (var patternElem1 in values)
            {
                PatternElem patelem = new PatternElem(patternElem1);
                elems.Add(patelem);
            }

            return elems;
        }
        #endregion instance part

        #region static part
        /// <summary>
        /// command patterns and cache
        /// </summary>
        internal static List<Pattern> PATTERNS = new List<Pattern>
{
    new Pattern
    {
        Name = "MediumMotor.Rotations",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVE32_F", ValueConditions = new Dictionary<int, string> { { 0, "360" } } },
            new PatternElem { Op = "PORT_CNV_OUTPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OUTPUT_STEP_SPEED", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "MotorPort,Speed,Rotations,,Brake_At_End"
    }
,
    new Pattern
    {
        Name = "Motor.Rotations",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MULF", ValueConditions = new Dictionary<int, string> { { 1, "360" } } },
            new PatternElem { Op = "PORT_CNV_OUTPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OUTPUT_STEP_SPEED", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "MotorPort,Speed,Rotations,,Brake_At_End"
    }
,
    new Pattern
    {
        Name = "Move.Stop",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "JR_EQ8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "JR_EQ8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_OUTPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OUTPUT_STOP", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Ports,,Brake_At_End"
    }
,
    new Pattern
    {
        Name = "Motor.Stop",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVE32_32", ValueConditions = new Dictionary<int, string> { { 1, "LOCAL16" } } },
            new PatternElem { Op = "JR_EQ8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_OUTPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OUTPUT_STOP", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "MotorPort,,Brake_At_End"
    }
,
    new Pattern
    {
        Name = "MediumMotor.Degrees",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "PORT_CNV_OUTPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OUTPUT_STEP_SPEED", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "MotorPort,Speed,Degrees,,Brake_At_End"
    }
,
    new Pattern
    {
        Name = "MediumMotor.Time",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "PORT_CNV_OUTPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OUTPUT_TIME_SPEED", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "MotorPort,Speed,Seconds,,Brake_At_End"
    }
,
    new Pattern
    {
        Name = "MediumMotor.Unlimited",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "PORT_CNV_OUTPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OUTPUT_SPEED", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OUTPUT_START", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "MotorPort,Speed,,"
    }
,
    new Pattern
    {
        Name = "MediumMotor.Stop",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "PORT_CNV_OUTPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OUTPUT_STOP", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "MotorPort,,Brake_At_End"
    }
,
    new Pattern
    {
        Name = "MoveTank.Rotations",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MULF", ValueConditions = new Dictionary<int, string> { { 1, "360" } } },
            new PatternElem { Op = "PORT_CNV_OUTPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "DIVF", ValueConditions = new Dictionary<int, string> { { 0, "100" } } },
            new PatternElem { Op = "OUTPUT_STEP_SYNC", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Ports,Speed_Left,Speed_Right,Rotations,,Brake_At_End"
    }
,
    new Pattern
    {
        Name = "MoveTank.Time",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "CALL", ValueConditions = new Dictionary<int, string> { { 7, "LOCAL" } } },
            new PatternElem { Op = "PORT_CNV_OUTPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "DIVF", ValueConditions = new Dictionary<int, string> { { 0, "100" } } },
            new PatternElem { Op = "OUTPUT_TIME_SYNC", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Ports,Speed_Left,Speed_Right,Seconds,,Brake_At_End"
    }
,
    new Pattern
    {
        Name = "MoveTank.Unlimited",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "CALL", ValueConditions = new Dictionary<int, string> { { 7, "1" } } },
            new PatternElem { Op = "PORT_CNV_OUTPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "DIVF", ValueConditions = new Dictionary<int, string> { { 0, "100" } } },
            new PatternElem { Op = "OUTPUT_TIME_SYNC", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Ports,Speed_Left,Speed_Right"
    }
,
    new Pattern
    {
        Name = "MoveTank.Degrees",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "PORT_CNV_OUTPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "DIVF", ValueConditions = new Dictionary<int, string> { { 0, "100" } } },
            new PatternElem { Op = "OUTPUT_STEP_SYNC", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Ports,Speed_Left,Speed_Right,Degrees,,Brake_At_End"
    }
,
    new Pattern
    {
        Name = "Move.Rotations",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MULF", ValueConditions = new Dictionary<int, string> { { 1, "360" } } },
            new PatternElem { Op = "PORT_CNV_OUTPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OUTPUT_STEP_SYNC", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Ports,Steering,Speed,Rotations,,Brake_At_End"
    }
,
    new Pattern
    {
        Name = "Move.Degrees",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "PORT_CNV_OUTPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OUTPUT_STEP_SYNC", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Ports,Steering,Speed,Degrees,,Brake_At_End"
    }
,
    new Pattern
    {
        Name = "Move.Time",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "CALL", ValueConditions = new Dictionary<int, string> { { 7, "LOCAL" } } },
            new PatternElem { Op = "PORT_CNV_OUTPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OUTPUT_TIME_SYNC", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Ports,Steering,Speed,Seconds,,Brake_At_End"
    }
,
    new Pattern
    {
        Name = "Move.Unlimited",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "CALL", ValueConditions = new Dictionary<int, string> { { 7, "0" } } },
            new PatternElem { Op = "PORT_CNV_OUTPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OUTPUT_TIME_SYNC", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Ports,Steering,Speed"
    }
,
    new Pattern
    {
        Name = "InvertMotor",
        Elems = new List<PatternElem> { new PatternElem { Op = "OUTPUT_POLARITY", ValueConditions = new Dictionary<int, string> { } } },
        ParamsOut = "MotorPort,,Invert"
    }
,
    new Pattern
    {
        Name = "Wait.Timer",
        Elems = new List<PatternElem>
        {
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "JR_EQ8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "TIMER_READ", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MULF", ValueConditions = new Dictionary<int, string> { { 1, "1000" } } },
            new PatternElem { Op = "ADDF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "TIMER_READ", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "How_Long"
    }
,
    new Pattern
    {
        Name = "LoopCondition.Unlimited",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem
            {
                Conditions = PatternElem.ConditionsFlags.AtObjectStart,
                Op = "MOVE32_32",
                ValueConditions = new Dictionary<int, string> { { 0, "GLOBAL" }, { 1, "LOCAL" } }
            }
,
            new PatternElem { Op = "AND32", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" }, { 2, "LOCAL" } } },
            new PatternElem { Op = "CP_GT32", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "0" }, { 2, "LOCAL" } } },
            new PatternElem { Op = "MOVE8_8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = ""
    }
,
    new Pattern
    {
        Name = "LoopCondition.Count",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem
            {
                Conditions = PatternElem.ConditionsFlags.AtObjectStart,
                Op = "ADD32",
                ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "1" } }
            }
,
            new PatternElem { Op = "CP_GTEQ32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MOVE32_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "AND32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GT32", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "0" } } },
            new PatternElem { Op = "OR8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MOVE8_8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Iterations_To_Run"
    }
,
    new Pattern
    {
        Name = "LoopCondition.Boolean",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem
            {
                Conditions = PatternElem.ConditionsFlags.AtObjectStart,
                Op = "MOVE32_32",
                ValueConditions = new Dictionary<int, string> { { 0, "GLOBAL" }, { 1, "LOCAL" } }
            }
,
            new PatternElem { Op = "AND32", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" }, { 2, "LOCAL" } } },
            new PatternElem { Op = "CP_GT32", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "0" }, { 2, "LOCAL" } } },
            new PatternElem { Op = "OR8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MOVE8_8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = ",,Do_Stop"
    }
,
    new Pattern
    {
        Name = "ColorSensor.MeasureColor",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 4, "2" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.Color,Port"
    }
,
    new Pattern
    {
        Name = "ColorSensor.MeasureReflectedLight",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_PCT" }, { 4, "0" } } },
            new PatternElem { Op = "MOVE32_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.Value,Port"
    }
,
    new Pattern
    {
        Name = "ColorSensor.MeasureAmbientLight",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_PCT" }, { 4, "1" } } },
            new PatternElem { Op = "MOVE32_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.Value,Port"
    }
,
    new Pattern
    {
        Name = "ColorSensor.CompareColor",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "0" }, { 4, "2" }, { 5, "1" } } },
            new PatternElem { Op = "JR_EQ8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_EQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_EQ32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OR8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OR8", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Port,@Result.Color,,@cmd.globals @out.Set_of_colors,@Result"
    }
,
    new Pattern
    {
        Name = "ColorSensor.CompareReflectedLight",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_PCT" }, { 4, "0" } } },
            new PatternElem { Op = "CP_EQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_NEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Port,@Result.Value,Threshold,Comparison,,@Result"
    }
,
    new Pattern
    {
        Name = "ColorSensor.CompareAmbientLight",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_PCT" }, { 4, "1" } } },
            new PatternElem { Op = "CP_EQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_NEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Port,@Result.Value,Threshold,Comparison,,@Result"
    }
,
    new Pattern
    {
        Name = "ColorSensor.ChangeColor",
        Elems = new List<PatternElem>
        {
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "CALL", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "0" }, { 4, "2" }, { 5, "1" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "JR", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "AND32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GT32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_NEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OR8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Port,@Result.Color"
    }
,
    new Pattern
    {
        Name = "ColorSensor.ChangeReflectedLight",
        Elems = new List<PatternElem>
        {
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "CALL", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_PCT" }, { 3, "0" }, { 4, "0" }, { 5, "1" } } },
            new PatternElem { Op = "MOVE32_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "JR", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "JR", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GT32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OR8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Port,@Result.Value,Amount,Direction"
    }
,
    new Pattern
    {
        Name = "ColorSensor.ChangeAmbientLight",
        Elems = new List<PatternElem>
        {
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "CALL", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_PCT" }, { 3, "0" }, { 4, "1" }, { 5, "1" } } },
            new PatternElem { Op = "MOVE32_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GT32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OR8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Port,@Result.Value,Amount,Direction"
    }
,
    new Pattern
    {
        Name = "ColorSensor.CalibrateMinColor",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "CAL_MIN" }, { 1, "29" }, { 2, "0" } } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "CalibrateValueMin"
    }
,
    new Pattern
    {
        Name = "ColorSensor.CalibrateMaxColor",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "CAL_MAX" }, { 1, "29" }, { 2, "0" } } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "CalibrateValueMax"
    }
,
    new Pattern
    {
        Name = "ColorSensor.CalibrateResetColor",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "CAL_DEFAULT" }, { 1, "29" }, { 2, "0" } } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = ""
    }
,
    new Pattern
    {
        Name = "Gyro.MeasureAngle",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "32" }, { 4, "0" }, { 5, "1" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.Angle,Port"
    }
,
    new Pattern
    {
        Name = "Gyro.MeasureRate",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "32" }, { 4, "1" }, { 5, "1" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.Rate,Port"
    }
,
    new Pattern
    {
        Name = "Gyro.MeasureAngleAndRate",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "32" }, { 4, "3" }, { 5, "2" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.Angle,Port,@Result.Rate"
    }
,
    new Pattern
    {
        Name = "Gyro.ChangeAngle",
        Elems = new List<PatternElem>
        {
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "CALL", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "32" }, { 4, "0" }, { 5, "1" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CALL", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OR8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Port,@Result.Angle,Change,Direction,,,@Result"
    }
,
    new Pattern
    {
        Name = "Gyro.ChangeRate",
        Elems = new List<PatternElem>
        {
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "CALL", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "32" }, { 4, "1" }, { 5, "1" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CALL", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OR8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Port,@Result.Rate,Change,Direction,,,@Result"
    }
,
    new Pattern
    {
        Name = "Gyro.CompareAngle",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "32" }, { 4, "0" }, { 5, "1" } } },
            new PatternElem { Op = "CP_EQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_NEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Port,@Result.Angle,Threshold,Comparison,,@Result"
    }
,
    new Pattern
    {
        Name = "Gyro.CompareRate",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "32" }, { 4, "1" }, { 5, "1" } } },
            new PatternElem { Op = "CP_EQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_NEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Port,@Result.Rate,Threshold,Comparison,,@Result"
    }
,
    new Pattern
    {
        Name = "Gyro.Reset",
        Elems = new List<PatternElem>
        {
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "32" }, { 4, "0" }, { 5, "1" } } },
            new PatternElem { Op = "INPUT_WRITE", ValueConditions = new Dictionary<int, string> { { 2, "1" } } },
            new PatternElem { Op = "INPUT_READY", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Port"
    }
,
    new Pattern
    {
        Name = "RotationSensor.MeasureDegrees",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "ADD8", ValueConditions = new Dictionary<int, string> { { 0, "16" } } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "0" }, { 4, "0" }, { 5, "1" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.Degrees,MotorPort"
    }
,
    new Pattern
    {
        Name = "RotationSensor.MeasureRotation",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "ADD8", ValueConditions = new Dictionary<int, string> { { 1, "16" } } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "0" }, { 4, "1" }, { 5, "1" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.Rotations,MotorPort"
    }
,
    new Pattern
    {
        Name = "RotationSensor.MeasureCurrentSpeed",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "ADD8", ValueConditions = new Dictionary<int, string> { { 0, "16" } } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "0" }, { 4, "2" }, { 5, "1" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.Speed,MotorPort"
    }
,
    new Pattern
    {
        Name = "RotationSensor.CompareDegrees",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "0" }, { 4, "0" }, { 5, "1" } } },
            new PatternElem { Op = "CP_EQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_NEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "MotorPort,@Result.Degrees,ThresholdDegrees,Comparison,,@Result"
    }
,
    new Pattern
    {
        Name = "RotationSensor.CompareRotation",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "0" }, { 4, "1" }, { 5, "1" } } },
            new PatternElem { Op = "CP_EQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_NEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "MotorPort,@Result.Rotations,ThresholdRotations,Comparison,,@Result"
    }
,
    new Pattern
    {
        Name = "RotationSensor.CompareCurrentSpeed",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "0" }, { 4, "2" }, { 5, "1" } } },
            new PatternElem { Op = "CP_EQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_NEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "MotorPort,@Result.Speed,ThresholdSpeed,Comparison,,@Result"
    }
,
    new Pattern
    {
        Name = "RotationSensor.Reset",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_OUTPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OUTPUT_CLR_COUNT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "TIMER_WAIT", ValueConditions = new Dictionary<int, string> { { 0, "25" } } },
            new PatternElem { Op = "TIMER_READY", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "MotorPort"
    }
,
    new Pattern
    {
        Name = "RotationSensor.ChangeDegrees",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "ADD8", ValueConditions = new Dictionary<int, string> { { 0, "16" } } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "0" }, { 4, "0" }, { 5, "1" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "MotorPort,@Result.Degrees,Amount,Direction"
    }
,
    new Pattern
    {
        Name = "RotationSensor.ChangeRotation",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "ADD8", ValueConditions = new Dictionary<int, string> { { 1, "16" } } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "0" }, { 4, "1" }, { 5, "1" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "MotorPort,@Result.Rotations,AmountRotations,Direction"
    }
,
    new Pattern
    {
        Name = "RotationSensor.ChangeSpeed",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "ADD8", ValueConditions = new Dictionary<int, string> { { 0, "16" } } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "0" }, { 4, "2" }, { 5, "1" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "MotorPort,@Result.Speed,Amount,Direction"
    }
,
    new Pattern
    {
        Name = "TouchSensor.Measure",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_READ", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" }, { 2, "0" }, { 3, "0" }, { 4, "LOCAL" } } },
            new PatternElem { Op = "CP_NEQ8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MOVE8_8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Port,@Result.State"
    }
,
    new Pattern
    {
        Name = "TouchSensor.Compare",
        MatchType = Pattern.MatchTypeEnum.NoSubCalls,
        Elems = new List<PatternElem>
        {
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "MOVE32_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_READ", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" }, { 2, "0" }, { 3, "0" }, { 4, "LOCAL" } } },
            new PatternElem { Op = "CP_EQ8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "GET_CHANGES" } } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "CLR_CHANGES" } } }
        }
,
        ParamsOut = "Port,@Result.TouchValue,Pressed__Released_or_Bumped,,@Result.Value"
    }
,
    new Pattern
    {
        Name = "TouchSensor.ChangeState",
        MatchType = Pattern.MatchTypeEnum.NoSubCalls,
        Elems = new List<PatternElem>
        {
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_READ", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" }, { 2, "1" }, { 3, "0" }, { 4, "LOCAL" } } },
            new PatternElem { Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_NEQ8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Port,,,,@Result.State"
    }
,
    new Pattern
    {
        Name = "BrickButton.Measure",
        MatchType = Pattern.MatchTypeEnum.NoSubCalls,
        Elems = new List<PatternElem>
        {
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "MOVE32_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "UI_BUTTON", ValueConditions = new Dictionary<int, string> { { 0, "PRESSED" } } },
            new PatternElem { Op = "CP_NEQ8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_EQ32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { { 1, "1" } } },
            new PatternElem { Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { { 1, "3" } } },
            new PatternElem { Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { { 1, "4" } } },
            new PatternElem { Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { { 1, "5" } } },
            new PatternElem { Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { { 1, "6" } } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.Value"
    }
,
    new Pattern
    {
        Name = "BrickButton.Compare",
        Elems = new List<PatternElem>
        {
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "MOVE32_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "ARRAY", ValueConditions = new Dictionary<int, string> { { 0, "SIZE" } } },
            new PatternElem { Op = "ARRAY_READ", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "UI_BUTTON", ValueConditions = new Dictionary<int, string> { { 0, "PRESSED" } } },
            new PatternElem { Op = "UI_BUTTON", ValueConditions = new Dictionary<int, string> { { 0, "PRESSED" } } },
            new PatternElem { Op = "UI_BUTTON", ValueConditions = new Dictionary<int, string> { { 0, "GET_BUMBED" } } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result,Action,,@cmd.globals @out.Buttons,@Result.Value"
    }
,
    new Pattern
    {
        Name = "BrickButton.ChangeBrickButton",
        Elems = new List<PatternElem>
        {
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "CALL", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "MOVE32_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "UI_BUTTON", ValueConditions = new Dictionary<int, string> { { 0, "PRESSED" } } },
            new PatternElem { Op = "CP_NEQ8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_EQ32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { { 1, "1" } } },
            new PatternElem { Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { { 1, "3" } } },
            new PatternElem { Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { { 1, "4" } } },
            new PatternElem { Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { { 1, "5" } } },
            new PatternElem { Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { { 1, "6" } } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GT32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_NEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OR8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.Value,,"
    }
,
    new Pattern
    {
        Name = "Timer.MeasureTime",
        Elems = new List<PatternElem>
        {
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "MOVEF_8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CALL", ValueConditions = new Dictionary<int, string> { { 1, "LOCAL" }, { 2, "LOCAL" }, { 3, "0" } } },
            new PatternElem { Op = "DIVF", ValueConditions = new Dictionary<int, string> { { 1, "1000" } } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Timer,@Result.Timer_Value"
    }
,
    new Pattern
    {
        Name = "Timer.CompareTime",
        Elems = new List<PatternElem>
        {
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "MOVE32_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "AND32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CALL", ValueConditions = new Dictionary<int, string> { { 1, "LOCAL" }, { 2, "LOCAL" }, { 3, "0" } } },
            new PatternElem { Op = "DIVF", ValueConditions = new Dictionary<int, string> { { 1, "1000" } } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_EQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_NEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OR8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MOVE8_8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Timer,@Result.Timer_Value,Threshold,Comparison,,@Result"
    }
,
    new Pattern
    {
        Name = "Timer.Reset",
        Elems = new List<PatternElem>
        {
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "MOVEF_8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CALL", ValueConditions = new Dictionary<int, string> { { 1, "LOCAL" }, { 2, "LOCAL" }, { 3, "1" } } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Timer,,Threshold,Comparison,"
    }
,
    new Pattern
    {
        Name = "Sound.File",
        Elems = new List<PatternElem> { new PatternElem { Op = "SOUND", ValueConditions = new Dictionary<int, string> { { 0, "PLAY" } } } },
        ParamsOut = "Volume,,Play_Type,@cmd.globals @out.Name"
    }
,
    new Pattern
    {
        Name = "Sound.Stop",
        Elems = new List<PatternElem> { new PatternElem { Op = "SOUND", ValueConditions = new Dictionary<int, string> { { 0, "BREAK" } } } }
    }
,
    new Pattern
    {
        Name = "Sound.Note",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "NOTE_TO_FREQ", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "SOUND", ValueConditions = new Dictionary<int, string> { { 0, "TONE" } } }
        }
,
        ParamsOut = "Duration,Volume,,Play_Type,@cmd.globals @out.Note"
    }
,
    new Pattern
    {
        Name = "Sound.Tone",
        Elems = new List<PatternElem> { new PatternElem { Op = "SOUND", ValueConditions = new Dictionary<int, string> { { 0, "TONE" } } } },
        ParamsOut = "Frequency,Duration,Volume,,Play_Type"
    }
,
    new Pattern
    {
        Name = "LED.On",
        Elems = new List<PatternElem> { new PatternElem { Op = "UI_WRITE", ValueConditions = new Dictionary<int, string> { { 0, "LED" }, { 1, "LOCAL" } } } },
        ParamsOut = "Color,,Pulse"
    }
,
    new Pattern
    {
        Name = "LED.Off",
        Elems = new List<PatternElem> { new PatternElem { Op = "UI_WRITE", ValueConditions = new Dictionary<int, string> { { 0, "LED" }, { 1, "0" } } } }
    }
,
    new Pattern
    {
        Name = "LED.Reset",
        Elems = new List<PatternElem> { new PatternElem { Op = "UI_WRITE", ValueConditions = new Dictionary<int, string> { { 0, "LED" }, { 1, "7" } } } }
    }
,
    new Pattern
    {
        Name = "Display.File",
        Elems = new List<PatternElem> { new PatternElem { Op = "UI_DRAW", ValueConditions = new Dictionary<int, string> { { 0, "BMPFILE" } } } },
        ParamsOut = "X,Y,,@cmd.globals @out.Filename,Clear_Screen"
    }
,
    new Pattern
    {
        Name = "Display.Clear",
        Elems = new List<PatternElem> { new PatternElem { Op = "UI_WRITE", ValueConditions = new Dictionary<int, string> { { 0, "INIT_RUN" } } } }
    }
,
    new Pattern
    {
        Name = "Display.StringGrid",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MULF", ValueConditions = new Dictionary<int, string> { { 1, "10" } } },
            new PatternElem { Op = "MULF", ValueConditions = new Dictionary<int, string> { { 1, "8" } } },
            new PatternElem { Op = "UI_DRAW", ValueConditions = new Dictionary<int, string> { { 0, "TEXT" } } }
        }
,
        ParamsOut = "Row,Column,Size,,@cmd.globals @out.Text,Clear_Screen,Invert_Color"
    }
,
    new Pattern
    {
        Name = "Display.String",
        Elems = new List<PatternElem> { new PatternElem { Op = "UI_DRAW", ValueConditions = new Dictionary<int, string> { { 0, "TEXT" } } } },
        ParamsOut = "X,Y,Size,,@cmd.globals @out.Text,Clear_Screen,Invert_Color"
    }
,
    new Pattern
    {
        Name = "Display.Point",
        Elems = new List<PatternElem> { new PatternElem { Op = "UI_DRAW", ValueConditions = new Dictionary<int, string> { { 0, "PIXEL" } } } },
        ParamsOut = "X,Y,,Invert_Color,Clear_Screen"
    }
,
    new Pattern
    {
        Name = "Display.Line",
        Elems = new List<PatternElem> { new PatternElem { Op = "UI_DRAW", ValueConditions = new Dictionary<int, string> { { 0, "LINE" } } } },
        ParamsOut = "X1,Y1,X2,Y2,,Invert_Color,Clear_Screen"
    }
,
    new Pattern
    {
        Name = "Display.Circle",
        Elems = new List<PatternElem> { new PatternElem { Op = "UI_DRAW", ValueConditions = new Dictionary<int, string> { { 0, "CIRCLE" } } } },
        ParamsOut = "X,Y,Radius,,Clear_Screen,Fill,Invert_Color"
    }
,
    new Pattern
    {
        Name = "Display.Rectangle",
        Elems = new List<PatternElem> { new PatternElem { Op = "UI_DRAW", ValueConditions = new Dictionary<int, string> { { 0, "RECT" } } } },
        ParamsOut = "X,Y,Width,Height,,Clear_Screen,Fill,Invert_Color"
    }
,
    new Pattern
    {
        Name = "StopBlock",
        Elems = new List<PatternElem> { new PatternElem { Op = "PROGRAM_STOP", ValueConditions = new Dictionary<int, string> { } } }
    }
,
    new Pattern
    {
        Name = "FileAccess.Text",
        Elems = new List<PatternElem> { new PatternElem { Op = "FILE", ValueConditions = new Dictionary<int, string> { { 0, "READ_TEXT" } } } },
        ParamsOut = "@Result.Text,@cmd.globals @out.FileName"
    }
,
    new Pattern
    {
        Name = "FileAccess.Numeric",
        Elems = new List<PatternElem> { new PatternElem { Op = "FILE", ValueConditions = new Dictionary<int, string> { { 0, "READ_VALUE" } } } },
        ParamsOut = "@Result.Numeric,@cmd.globals @out.FileName"
    }
,
    new Pattern
    {
        Name = "FileAccess.Write",
        Elems = new List<PatternElem> { new PatternElem { Op = "FILE", ValueConditions = new Dictionary<int, string> { { 0, "WRITE_TEXT" } } } },
        ParamsOut = ",@cmd.globals @out.FileName,@cmd.globals @out.TextIn"
    }
,
    new Pattern
    {
        Name = "FileAccess.Delete",
        Elems = new List<PatternElem> { new PatternElem { Op = "FILE", ValueConditions = new Dictionary<int, string> { { 0, "REMOVE" } } } },
        ParamsOut = ",@cmd.globals @out.FileName"
    }
,
    new Pattern
    {
        Name = "FileAccess.Close",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "FILE", ValueConditions = new Dictionary<int, string> { { 0, "CLOSE" } } },
            new PatternElem { Op = "ARRAY", ValueConditions = new Dictionary<int, string> { { 0, "DELETE" } } }
        }
,
        ParamsOut = ",@cmd.globals @out.FileName"
    }
,
    new Pattern
    {
        Name = "Math.Add",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "ADDF", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" }, { 2, "LOCAL" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" } } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result,X,Y"
    }
,
    new Pattern
    {
        Name = "Math.Subtract",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "SUBF", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" }, { 2, "LOCAL" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" } } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result,X,Y"
    }
,
    new Pattern
    {
        Name = "Math.Multiply",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem
            {
                Conditions = PatternElem.ConditionsFlags.AtObjectStart,
                Op = "MULF",
                ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" }, { 2, "LOCAL" } }
            }
,
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" } } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result,X,Y"
    }
,
    new Pattern
    {
        Name = "Math.Divide",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "DIVF", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" }, { 2, "LOCAL" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" } } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result,X,Y"
    }
,
    new Pattern
    {
        Name = "Math.AbsoluteValue",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MATH", ValueConditions = new Dictionary<int, string> { { 0, "ABS" }, { 1, "LOCAL" }, { 2, "LOCAL" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" } } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result,X"
    }
,
    new Pattern
    {
        Name = "Math.SquareRoot",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MATH", ValueConditions = new Dictionary<int, string> { { 0, "SQRT" }, { 1, "LOCAL" }, { 2, "LOCAL" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" } } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result,X"
    }
,
    new Pattern
    {
        Name = "Math.Exponent",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MATH", ValueConditions = new Dictionary<int, string> { { 0, "POW" }, { 1, "LOCAL" }, { 2, "LOCAL" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" } } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result,Base,Exponent"
    }
,
    new Pattern
    {
        Name = "Random.Numeric",
        Elems = new List<PatternElem> { new PatternElem { Op = "RANDOM", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" }, { 2, "LOCAL" } } } },
        ParamsOut = "@Result.Number,Lower,Upper"
    }
,
    new Pattern
    {
        Name = "Random.Boolean",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "RANDOM", ValueConditions = new Dictionary<int, string> { { 0, "1" }, { 1, "100" }, { 2, "LOCAL" } } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Percent_True,@Result"
    }
,
    new Pattern
    {
        Name = "Text.Merge",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "STRINGS", ValueConditions = new Dictionary<int, string> { { 0, "ADD" } } },
            new PatternElem { Op = "STRINGS", ValueConditions = new Dictionary<int, string> { { 0, "ADD" } } },
            new PatternElem { Op = "ARRAY", ValueConditions = new Dictionary<int, string> { { 0, "COPY" } } }
        }
,
        ParamsOut = "@Result,@cmd.globals @out.A,@cmd.globals @out.B,@cmd.globals @out.C"
    }
,
    new Pattern
    {
        Name = "BooleanOperations.And",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "AND8", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" }, { 2, "LOCAL" } } },
            new PatternElem { Op = "MOVE8_8", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" } } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result,A,B"
    }
,
    new Pattern
    {
        Name = "BooleanOperations.Or",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "OR8", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" }, { 2, "LOCAL" } } },
            new PatternElem { Op = "MOVE8_8", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" } } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result,A,B"
    }
,
    new Pattern
    {
        Name = "BooleanOperations.XOR",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "XOR8", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" }, { 2, "LOCAL" } } },
            new PatternElem { Op = "MOVE8_8", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" } } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result,A,B"
    }
,
    new Pattern
    {
        Name = "BooleanOperations.Not",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "XOR8", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "1" }, { 2, "LOCAL" } } },
            new PatternElem { Op = "MOVE8_8", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" } } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result,A"
    }
,
    new Pattern
    {
        Name = "Compare.Equal",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "CP_EQF", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" }, { 2, "LOCAL" } } },
            new PatternElem { Op = "MOVE8_8", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" } } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "x,y,@Result"
    }
,
    new Pattern
    {
        Name = "Compare.NotEqual",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "CP_NEQF", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" }, { 2, "LOCAL" } } },
            new PatternElem { Op = "MOVE8_8", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" } } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "x,y,@Result"
    }
,
    new Pattern
    {
        Name = "Compare.GreaterThan",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "CP_GTF", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" }, { 2, "LOCAL" } } },
            new PatternElem { Op = "MOVE8_8", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" } } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "x,y,@Result"
    }
,
    new Pattern
    {
        Name = "Compare.GreaterOrEqual",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" }, { 2, "LOCAL" } } },
            new PatternElem { Op = "MOVE8_8", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" } } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "x,y,@Result"
    }
,
    new Pattern
    {
        Name = "Compare.LessThan",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "CP_LTF", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" }, { 2, "LOCAL" } } },
            new PatternElem { Op = "MOVE8_8", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" } } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "x,y,@Result"
    }
,
    new Pattern
    {
        Name = "Compare.LessOrEqual",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" }, { 2, "LOCAL" } } },
            new PatternElem { Op = "MOVE8_8", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL" } } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "x,y,@Result"
    }
,
    new Pattern
    {
        Name = "Range.Inside",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "AND8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MOVE8_8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Test_Value,Lower_Bound,Upper_Bound,@Result"
    }
,
    new Pattern
    {
        Name = "Range.Outside",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "CP_LTF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OR8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MOVE8_8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Test_Value,Lower_Bound,Upper_Bound,@Result"
    }
,
    new Pattern
    {
        Name = "Round.Nearest",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MATH", ValueConditions = new Dictionary<int, string> { { 0, "ROUND" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.Output_Result,Input"
    }
,
    new Pattern
    {
        Name = "Round.Up",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MATH", ValueConditions = new Dictionary<int, string> { { 0, "CEIL" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.Output_Result,Input"
    }
,
    new Pattern
    {
        Name = "Round.Down",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MATH", ValueConditions = new Dictionary<int, string> { { 0, "FLOOR" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.Output_Result,Input"
    }
,
    new Pattern
    {
        Name = "Round.Truncate",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVEF_8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MATH", ValueConditions = new Dictionary<int, string> { { 0, "TRUNC" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.Output_Result,Input,Number_of_Decimals"
    }
,
    new Pattern
    {
        Name = "Interrupt",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVE32_32", ValueConditions = new Dictionary<int, string> { { 0, "GLOBAL" }, { 1, "LOCAL" } } },
            new PatternElem { Op = "OR32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MOVE32_32", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "GLOBAL" } } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "InterruptValue,@cmd.globals @out.InterruptName"
    }
,
    new Pattern
    {
        Name = "Messaging.SendText",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "CALL", ValueConditions = new Dictionary<int, string> { { 2, "GLOBAL" }, { 3, "GLOBAL" }, { 4, "GLOBAL" } } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "MOVE32_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MAILBOX_WRITE", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = ",@cmd.globals @out.Message_Title,@cmd.globals @out.Receiving_Brick_Name,@cmd.globals @out.SentMessage"
    }
,
    new Pattern
    {
        Name = "Messaging.SendNumeric",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "CALL", ValueConditions = new Dictionary<int, string> { { 3, "GLOBAL" }, { 4, "GLOBAL" } } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "MAILBOX_WRITE", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "SentMessage,,@cmd.globals @out.Message_Title,@cmd.globals @out.Receiving_Brick_Name"
    }
,
    new Pattern
    {
        Name = "Messaging.SendBoolean",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "CALL", ValueConditions = new Dictionary<int, string> { { 2, "GLOBAL" }, { 3, "GLOBAL" } } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "MOVE32_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MAILBOX_WRITE", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = ",@cmd.globals @out.Message_Title,@cmd.globals @out.Receiving_Brick_Name,SentMessage"
    }
,
    new Pattern
    {
        Name = "Messaging.ReceiveText",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem
            {
                Conditions = PatternElem.ConditionsFlags.AtObjectStart,
                Op = "ARRAY",
                ValueConditions = new Dictionary<int, string> { { 0, "CREATE8" } }
            }
,
            new PatternElem { Op = "MAILBOX_READ", ValueConditions = new Dictionary<int, string> { { 1, "250" } } },
            new PatternElem { Op = "ARRAY", ValueConditions = new Dictionary<int, string> { { 0, "COPY" } } },
            new PatternElem { Op = "ARRAY", ValueConditions = new Dictionary<int, string> { { 0, "DELETE" } } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.ReceivedMessage"
    }
,
    new Pattern
    {
        Name = "Messaging.ReceiveNumeric",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem
            {
                Conditions = PatternElem.ConditionsFlags.AtObjectStart,
                Op = "MAILBOX_READ",
                ValueConditions = new Dictionary<int, string> { { 1, "4" } }
            }
,
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.ReceivedMessage"
    }
,
    new Pattern
    {
        Name = "Messaging.ReceiveBoolean",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem
            {
                Conditions = PatternElem.ConditionsFlags.AtObjectStart,
                Op = "MAILBOX_READ",
                ValueConditions = new Dictionary<int, string> { { 1, "1" } }
            }
,
            new PatternElem { Op = "MOVE8_8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.ReceivedMessage"
    }
,
    new Pattern
    {
        Name = "Messaging.CompareText",
        Elems = new List<PatternElem>
        {
            new PatternElem
            {
                Conditions = PatternElem.ConditionsFlags.AtObjectStart,
                Op = "ARRAY",
                ValueConditions = new Dictionary<int, string> { { 0, "CREATE8" } }
            }
,
            new PatternElem { Op = "MAILBOX_READ", ValueConditions = new Dictionary<int, string> { { 1, "250" } } },
            new PatternElem { Op = "STRINGS", ValueConditions = new Dictionary<int, string> { { 0, "COMPARE" } } },
            new PatternElem { Op = "ARRAY", ValueConditions = new Dictionary<int, string> { { 0, "COPY" } } },
            new PatternElem { Op = "CP_GT32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "XOR8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "ARRAY", ValueConditions = new Dictionary<int, string> { { 0, "DELETE" } } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Comparison2,,@cmd.globals @out.Message_Title,@Result.ReceivedMessage,@cmd.globals @out.ComparisonText,,@Result"
    }
,
    new Pattern
    {
        Name = "Messaging.CompareNumeric",
        Elems = new List<PatternElem>
        {
            new PatternElem
            {
                Conditions = PatternElem.ConditionsFlags.AtObjectStart,
                Op = "MAILBOX_READ",
                ValueConditions = new Dictionary<int, string> { { 1, "4" } }
            }
,
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "JR_EQ32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_EQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_NEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OR8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.ReceivedMessage,Threshold,,Comparison,@cmd.globals @out.Message_Title,,@Result"
    }
,
    new Pattern
    {
        Name = "Messaging.CompareBoolean",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem
            {
                Conditions = PatternElem.ConditionsFlags.AtObjectStart,
                Op = "MAILBOX_READ",
                ValueConditions = new Dictionary<int, string> { { 1, "1" } }
            }
,
            new PatternElem { Op = "MOVE32_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "AND32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GT32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OR8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MOVE8_8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MOVE8_8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@cmd.globals @out.Message_Title,,@Result,@Result.ReceivedMessage"
    }
,
    new Pattern
    {
        Name = "UnregulatedMotor",
        Elems = new List<PatternElem>
        {
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "MOVE32_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_OUTPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OUTPUT_POWER", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OUTPUT_START", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "JR", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "JR", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "MotorPort,Power"
    }
,
    new Pattern
    {
        Name = "RawSensorValue",
        Elems = new List<PatternElem>
        {
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_RAW" } } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.Raw_Value,Port_Number"
    }
,
    new Pattern
    {
        Name = "CaseSelector.Numeric",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem
            {
                Conditions = PatternElem.ConditionsFlags.AtObjectStart,
                Op = "MOVE32_32",
                ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL0" } }
            }
,
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result,Number"
    }
,
    new Pattern
    {
        Name = "CaseSelector.Boolean",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem
            {
                Conditions = PatternElem.ConditionsFlags.AtObjectStart,
                Op = "MOVE8_8",
                ValueConditions = new Dictionary<int, string> { { 0, "LOCAL" }, { 1, "LOCAL0" } }
            }
,
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result,Boolean"
    }
,
    new Pattern
    {
        Name = "CaseSelector.String",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem
            {
                Conditions = PatternElem.ConditionsFlags.AtObjectStart,
                Op = "ARRAY",
                ValueConditions = new Dictionary<int, string> { { 0, "COPY" }, { 1, "LOCAL" }, { 2, "LOCAL0" } }
            }
,
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result,@cmd.globals @out.String"
    }
,
    new Pattern
    {
        Name = "ArrayOperations.Append_Numeric",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "ARRAY", ValueConditions = new Dictionary<int, string> { { 0, "CREATEF" }, { 1, "1" } } },
            new PatternElem { Op = "ARRAY", ValueConditions = new Dictionary<int, string> { { 0, "COPY" } } },
            new PatternElem { Op = "ARRAY_APPEND", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "ARRAY", ValueConditions = new Dictionary<int, string> { { 0, "COPY" } } },
            new PatternElem { Op = "ARRAY", ValueConditions = new Dictionary<int, string> { { 0, "DELETE" } } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "valueIn,@Result.arrayOutNumeric,@cmd.globals @out.arrayInNumeric"
    }
,
    new Pattern
    {
        Name = "ArrayOperations.Append_Boolean",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "ARRAY", ValueConditions = new Dictionary<int, string> { { 0, "CREATE8" }, { 1, "1" } } },
            new PatternElem { Op = "ARRAY", ValueConditions = new Dictionary<int, string> { { 0, "COPY" } } },
            new PatternElem { Op = "ARRAY_APPEND", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "ARRAY", ValueConditions = new Dictionary<int, string> { { 0, "COPY" } } },
            new PatternElem { Op = "ARRAY", ValueConditions = new Dictionary<int, string> { { 0, "DELETE" } } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.arrayOutBoolean,@cmd.globals @out.arrayInBoolean,valueIn"
    }
,
    new Pattern
    {
        Name = "ArrayOperations.ReadAtIndex_Numeric",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "ARRAY_READ", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.valueOut,Index,@cmd.globals @out.arrayInNumeric"
    }
,
    new Pattern
    {
        Name = "ArrayOperations.ReadAtIndex_Boolean",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "ARRAY_READ", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MOVE8_8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Index,@cmd.globals @out.arrayInBoolean,@Result.valueOut"
    }
,
    new Pattern
    {
        Name = "ArrayOperations.WriteAtIndex_Numeric",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "ARRAY", ValueConditions = new Dictionary<int, string> { { 0, "CREATEF" }, { 1, "1" } } },
            new PatternElem { Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "ARRAY", ValueConditions = new Dictionary<int, string> { { 0, "COPY" } } },
            new PatternElem { Op = "ARRAY_WRITE", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "ARRAY", ValueConditions = new Dictionary<int, string> { { 0, "COPY" } } },
            new PatternElem { Op = "ARRAY", ValueConditions = new Dictionary<int, string> { { 0, "DELETE" } } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Index,Value,@Result.arrayOutNumeric,@cmd.globals @out.arrayInNumeric"
    }
,
    new Pattern
    {
        Name = "ArrayOperations.WriteAtIndex_Boolean",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "ARRAY", ValueConditions = new Dictionary<int, string> { { 0, "CREATE8" }, { 1, "1" } } },
            new PatternElem { Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "ARRAY", ValueConditions = new Dictionary<int, string> { { 0, "COPY" } } },
            new PatternElem { Op = "ARRAY_WRITE", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "ARRAY", ValueConditions = new Dictionary<int, string> { { 0, "COPY" } } },
            new PatternElem { Op = "ARRAY", ValueConditions = new Dictionary<int, string> { { 0, "DELETE" } } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Index,@Result.arrayOutBoolean,@cmd.globals @out.arrayInBoolean,Value"
    }
,
    new Pattern
    {
        Name = "ArrayOperations.Length_Numeric",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "ARRAY", ValueConditions = new Dictionary<int, string> { { 0, "SIZE" } } },
            new PatternElem { Op = "MOVE32_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.Size,@cmd.globals @out.arrayInNumeric"
    }
,
    new Pattern
    {
        Name = "InfraredSensor.MeasureProximity",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "33" }, { 4, "0" }, { 5, "1" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.Proximity,Port"
    }
,
    new Pattern
    {
        Name = "InfraredSensor.MeasureBeaconSeeker",
        MatchType = Pattern.MatchTypeEnum.NoSubCalls,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "33" }, { 4, "1" }, { 5, "8" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.Heading,Port,@Result.Proximity,Channel,@Result.Valid"
    }
,
    new Pattern
    {
        Name = "InfraredSensor.MeasureBeaconRemote",
        MatchType = Pattern.MatchTypeEnum.NoSubCalls,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "0" }, { 4, "2" }, { 5, "4" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Port,@Result.Button,Channel"
    }
,
    new Pattern
    {
        Name = "InfraredSensor.CompareProximity",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "33" }, { 4, "0" }, { 5, "1" } } },
            new PatternElem { Op = "CP_EQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_NEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GT32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OR8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Port,@Result.Proximity,Threshold,Comparison,,@Result"
    }
,
    new Pattern
    {
        Name = "InfraredSensor.CompareBeaconSeekerHeading",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "33" }, { 4, "1" }, { 5, "8" } } },
            new PatternElem { Op = "CP_EQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_NEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GT32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OR8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Port,@Result.Proximity,Channel,HeadingThreshold,Comparison,Heading,@Result"
    }
,
    new Pattern
    {
        Name = "InfraredSensor.CompareRemote",
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "0" }, { 4, "2" }, { 5, "4" } } },
            new PatternElem { Op = "CP_EQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_EQ32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OR8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Port,Channel,@Result.Button,,@cmd.globals @out.Set_of_remote_button_IDs,@Result"
    }
,
    new Pattern
    {
        Name = "InfraredSensor.ChangeProximity",
        Elems = new List<PatternElem>
        {
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "CALL", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "33" }, { 4, "0" }, { 5, "1" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CALL", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Port,@Result.Proximity,Amount,Direction,,,"
    }
,
    new Pattern
    {
        Name = "InfraredSensor.ChangeHeading",
        Elems = new List<PatternElem>
        {
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "CALL", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "33" }, { 4, "1" }, { 5, "8" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL44" } } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Port,Channel,@Result.Heading,HeadingAmount,Direction,,,"
    }
,
    new Pattern
    {
        Name = "InfraredSensor.ChangeBeaconProximity",
        Elems = new List<PatternElem>
        {
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "CALL", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "33" }, { 4, "1" }, { 5, "8" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { { 0, "LOCAL32" } } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_LTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GTEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Port,Channel,@Result.Heading,Amount,Direction,,,"
    }
,
    new Pattern
    {
        Name = "InfraredSensor.ChangeRemote",
        Elems = new List<PatternElem>
        {
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "CALL", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectStart, Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "0" }, { 4, "2" }, { 5, "4" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_NEQF", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "CP_GT32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "OR8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Conditions = PatternElem.ConditionsFlags.AtObjectEnd, Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Port,@Result.Button,Channel,,,"
    }
,
    new Pattern
    {
        Name = "UltrasonicSensor.MeasureCentimeters",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "0" }, { 4, "0" }, { 5, "1" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.Distance,Port"
    }
,
    new Pattern
    {
        Name = "UltrasonicSensor.MeasureInches",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "0" }, { 4, "1" }, { 5, "1" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.DistanceInches,Port"
    }
,
    new Pattern
    {
        Name = "UltrasonicSensor.MeasurePresence",
        MatchType = Pattern.MatchTypeEnum.NoSubCalls,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_PCT" }, { 3, "0" }, { 4, "2" }, { 5, "1" } } },
            new PatternElem { Op = "CP_GT32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "MOVE8_8", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "Port,@Result.Heard"
    }
,
    new Pattern
    {
        Name = "UltrasonicSensor.Centimeters",
        MatchType = Pattern.MatchTypeEnum.NoSubCalls,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "0" }, { 4, "0" }, { 5, "1" } } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "0" }, { 4, "LOCAL" }, { 5, "1" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.Distance,Port,Measuring_Mode"
    }
,
    new Pattern
    {
        Name = "UltrasonicSensor.Inches",
        MatchType = Pattern.MatchTypeEnum.NoSubCalls,
        Elems = new List<PatternElem>
        {
            new PatternElem { Op = "MOVEF_32", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "PORT_CNV_INPUT", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "0" }, { 4, "1" }, { 5, "1" } } },
            new PatternElem { Op = "INPUT_DEVICE", ValueConditions = new Dictionary<int, string> { { 0, "READY_SI" }, { 3, "0" }, { 4, "LOCAL" }, { 5, "1" } } },
            new PatternElem { Op = "MOVEF_F", ValueConditions = new Dictionary<int, string> { } },
            new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } }
        }
,
        ParamsOut = "@Result.DistanceInches,Port,Measuring_Mode"
    }
,
    new Pattern
    {
        Name = "CommentBlock",
        MatchType = Pattern.MatchTypeEnum.NoSubCallsExact,
        Elems = new List<PatternElem> { new PatternElem { Op = "RETURN", ValueConditions = new Dictionary<int, string> { } } },
        ParamsOut = ""
    }
,
    new Pattern
    {
        Name = "KeepAlive",
        MatchType = Pattern.MatchTypeEnum.NoSubCalls,
        Elems = new List<PatternElem> { new PatternElem { Op = "KEEP_ALIVE", ValueConditions = new Dictionary<int, string> { } } },
        ParamsOut = "Time_until_sleep_(ms)"
    }
,
    new Pattern
    {
        Name = "Bluetooth.On",
        MatchType = Pattern.MatchTypeEnum.NoSubCalls,
        Elems = new List<PatternElem> { new PatternElem { Op = "COM_SET", ValueConditions = new Dictionary<int, string> { { 0, "SET_ON_OFF" }, { 1, "2" }, { 2, "1" } } } },
        ParamsOut = ""
    }
,
    new Pattern
    {
        Name = "Bluetooth.Off",
        MatchType = Pattern.MatchTypeEnum.NoSubCalls,
        Elems = new List<PatternElem> { new PatternElem { Op = "COM_SET", ValueConditions = new Dictionary<int, string> { { 0, "SET_ON_OFF" }, { 1, "2" }, { 2, "0" } } } },
        ParamsOut = ""
    }
,
    new Pattern
    {
        Name = "Bluetooth.Initiate",
        MatchType = Pattern.MatchTypeEnum.NoSubCalls,
        Elems = new List<PatternElem> { new PatternElem { Op = "COM_SET", ValueConditions = new Dictionary<int, string> { { 0, "SET_CONNECTION" }, { 1, "2" }, { 3, "1" } } } },
        ParamsOut = ",@cmd.globals @out.Connect_To"
    }
,
    new Pattern
    {
        Name = "Bluetooth.Clear",
        MatchType = Pattern.MatchTypeEnum.NoSubCalls,
        Elems = new List<PatternElem> { new PatternElem { Op = "COM_SET", ValueConditions = new Dictionary<int, string> { { 0, "SET_CONNECTION" }, { 1, "2" }, { 3, "0" } } } },
        ParamsOut = ",@cmd.globals @out.Connect_To"
    }
}
;
        internal static Dictionary<string, HashSet<Pattern>> PATTERNS_CACHE { get; set; }

        /// <summary>
        /// static ctor
        /// create cache indexing
        /// </summary>
        static Pattern()
        {
            PATTERNS = InitializePattern();

            //-- generate pattern cache
            PATTERNS_CACHE = new Dictionary<string, HashSet<Pattern>>();
            foreach (var pat in PATTERNS)
            {
                foreach (var patelem in pat.Elems)
                {
                    if (!PATTERNS_CACHE.ContainsKey(patelem.Op)) PATTERNS_CACHE[patelem.Op] = new HashSet<Pattern>();
                    PATTERNS_CACHE[patelem.Op].Add(pat);
                }
            }
        }

        /// <summary>
        /// Generate initial static command patterns from textual representation 
        /// Each EV3-G block is implemented by several VIX or even bytecode subobjects
        /// only most relevant signatures are captured here
        /// </summary>
        /// <returns></returns>
        internal static List<Pattern> InitializePattern()
        {
            //            var json = @"
            //[
            //  {
            //    'Name': 'MediumMotor.Rotations',
            //    'ElemsBacking': 'MOVE32_F(360,); PORT_CNV_OUTPUT; OUTPUT_STEP_SPEED;',
            //    'ParamsOut': 'MotorPort,Speed,Rotations,,Brake_At_End'
            //  },
            //  {
            //    'Name': 'Motor.Rotations',
            //    'ElemsBacking': 'MULF(,360,); PORT_CNV_OUTPUT; OUTPUT_STEP_SPEED;',
            //    'ParamsOut': 'MotorPort,Speed,Rotations,,Brake_At_End'
            //  },
            //  {
            //    'Name': 'Move.Stop',
            //    'ElemsBacking': 'JR_EQ8; JR_EQ8; PORT_CNV_OUTPUT; OUTPUT_STOP',
            //    'ParamsOut': 'Ports,,Brake_At_End'
            //  },
            //  {
            //    'Name': 'Motor.Stop',
            //    'ElemsBacking': 'MOVE32_32(,LOCAL16); JR_EQ8; PORT_CNV_OUTPUT; OUTPUT_STOP',
            //    'ParamsOut': 'MotorPort,,Brake_At_End'
            //  },

            //  {
            //    'Name': 'MediumMotor.Degrees',
            //    'ElemsBacking': 'PORT_CNV_OUTPUT; OUTPUT_STEP_SPEED;',
            //    'ParamsOut': 'MotorPort,Speed,Degrees,,Brake_At_End'
            //  },
            //  {
            //    'Name': 'MediumMotor.Time',
            //    'ElemsBacking': 'PORT_CNV_OUTPUT; OUTPUT_TIME_SPEED',
            //    'ParamsOut': 'MotorPort,Speed,Seconds,,Brake_At_End'
            //  },
            //  {
            //    'Name': 'MediumMotor.Unlimited',
            //    'ElemsBacking': 'PORT_CNV_OUTPUT; OUTPUT_SPEED; OUTPUT_START',
            //    'ParamsOut': 'MotorPort,Speed,,'
            //  },
            //  {
            //    'Name': 'MediumMotor.Stop',
            //    'ElemsBacking': 'PORT_CNV_OUTPUT; OUTPUT_STOP',
            //    'ParamsOut': 'MotorPort,,Brake_At_End'
            //  },
            //  {
            //    'Name': 'MoveTank.Rotations',
            //    'ElemsBacking': 'MULF(,360); PORT_CNV_OUTPUT; DIVF(100); OUTPUT_STEP_SYNC;',
            //    'ParamsOut': 'Ports,Speed_Left,Speed_Right,Rotations,,Brake_At_End'
            //  },
            //  {
            //    'Name': 'MoveTank.Time',
            //    'ElemsBacking': 'CALL(,,,,,,,LOCAL); PORT_CNV_OUTPUT; DIVF(100); OUTPUT_TIME_SYNC;',
            //    'ParamsOut': 'Ports,Speed_Left,Speed_Right,Seconds,,Brake_At_End'
            //  },
            //  {
            //    'Name': 'MoveTank.Unlimited',
            //    'ElemsBacking': 'CALL(,,,,,,,1); PORT_CNV_OUTPUT; DIVF(100); OUTPUT_TIME_SYNC;',
            //    'ParamsOut': 'Ports,Speed_Left,Speed_Right'
            //  },
            //  {
            //    'Name': 'MoveTank.Degrees',
            //    'ElemsBacking': 'PORT_CNV_OUTPUT; DIVF(100); OUTPUT_STEP_SYNC;',
            //    'ParamsOut': 'Ports,Speed_Left,Speed_Right,Degrees,,Brake_At_End'
            //  },
            //  {
            //    'Name': 'Move.Rotations',
            //    'ElemsBacking': 'MULF(,360); PORT_CNV_OUTPUT; OUTPUT_STEP_SYNC;',
            //    'ParamsOut': 'Ports,Steering,Speed,Rotations,,Brake_At_End'
            //  },
            //  {
            //    'Name': 'Move.Degrees',
            //    'ElemsBacking': 'PORT_CNV_OUTPUT; OUTPUT_STEP_SYNC;',
            //    'ParamsOut': 'Ports,Steering,Speed,Degrees,,Brake_At_End'
            //  },
            //  {
            //    'Name': 'Move.Time',
            //    'ElemsBacking': 'CALL(,,,,,,,LOCAL); PORT_CNV_OUTPUT; OUTPUT_TIME_SYNC;',
            //    'ParamsOut': 'Ports,Steering,Speed,Seconds,,Brake_At_End'
            //  },
            //  {
            //    'Name': 'Move.Unlimited',
            //    'ElemsBacking': 'CALL(,,,,,,,0); PORT_CNV_OUTPUT; OUTPUT_TIME_SYNC;',
            //    'ParamsOut': 'Ports,Steering,Speed'
            //  },
            //  {
            //    'Name': 'InvertMotor',
            //    'ElemsBacking': 'OUTPUT_POLARITY;',
            //    'ParamsOut': 'MotorPort,,Invert'
            //  },


            //  {
            //    'Name': 'Wait.Timer',
            //    'ElemsBacking': '^JR_EQ8;TIMER_READ;MULF(,1000);ADDF;TIMER_READ;CP_GTF;$RETURN;',
            //    'ParamsOut': 'How_Long'
            //  },

            //  {
            //    'Name': 'LoopCondition.Unlimited',
            //    'ElemsBacking': '^MOVE32_32(GLOBAL,LOCAL);AND32(LOCAL,LOCAL,LOCAL);CP_GT32(LOCAL,0,LOCAL);MOVE8_8;$RETURN;',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': ''
            //  },

            //  {
            //    'Name': 'LoopCondition.Count',
            //    'ElemsBacking': '^ADD32(LOCAL,1);CP_GTEQ32;MOVE32_32;AND32;CP_GT32(LOCAL,0);OR8;MOVE8_8;$RETURN;',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': 'Iterations_To_Run'
            //  },

            //  {
            //    'Name': 'LoopCondition.Boolean',
            //    'ElemsBacking': '^MOVE32_32(GLOBAL,LOCAL);AND32(LOCAL,LOCAL,LOCAL);CP_GT32(LOCAL,0,LOCAL);OR8;MOVE8_8;$RETURN;',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': ',,Do_Stop'
            //  },


            //  {
            //    'Name': 'ColorSensor.MeasureColor',
            //    'ElemsBacking': 'MOVEF_32;PORT_CNV_INPUT;INPUT_DEVICE(READY_SI,,,,2);MOVEF_F;RETURN;',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result.Color,Port'
            //  },
            //  {
            //    'Name': 'ColorSensor.MeasureReflectedLight',
            //    'ElemsBacking': 'MOVEF_32;PORT_CNV_INPUT;INPUT_DEVICE(READY_PCT,,,,0);MOVE32_F;MOVEF_F;RETURN;',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result.Value,Port'
            //  },
            //  {
            //    'Name': 'ColorSensor.MeasureAmbientLight',
            //    'ElemsBacking': 'MOVEF_32;PORT_CNV_INPUT;INPUT_DEVICE(READY_PCT,,,,1);MOVE32_F;MOVEF_F;RETURN;',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result.Value,Port'
            //  },
            //  {
            //    'Name': 'ColorSensor.CompareColor',
            //    'ElemsBacking': 'INPUT_DEVICE(READY_SI,,,0,2,1); JR_EQ8; CP_EQF; CP_EQ32; OR8; OR8;',
            //    'ParamsOut': 'Port,@Result.Color,,@cmd.globals @out.Set_of_colors,@Result'
            //  },
            //  {
            //    'Name': 'ColorSensor.CompareReflectedLight',
            //    'ElemsBacking': 'INPUT_DEVICE(READY_PCT,,,,0);CP_EQF;CP_GTF;CP_GTEQF;CP_NEQF;CP_LTF;CP_LTEQF;',
            //    'ParamsOut': 'Port,@Result.Value,Threshold,Comparison,,@Result'
            //  },
            //  {
            //    'Name': 'ColorSensor.CompareAmbientLight',
            //    'ElemsBacking': 'INPUT_DEVICE(READY_PCT,,,,1);CP_EQF;CP_GTF;CP_GTEQF;CP_NEQF;CP_LTF;CP_LTEQF;',
            //    'ParamsOut': 'Port,@Result.Value,Threshold,Comparison,,@Result'
            //  },
            //  {
            //    'Name': 'ColorSensor.ChangeColor',
            //    'ElemsBacking': '^CALL;^MOVEF_32;PORT_CNV_INPUT;INPUT_DEVICE(READY_SI,,,0,2,1);MOVEF_F;$RETURN;JR_EQ32;JR;AND32;CP_GT32;CP_NEQF;OR8;$RETURN;',
            //    'ParamsOut': 'Port,@Result.Color'
            //  },
            //  {
            //    'Name': 'ColorSensor.ChangeReflectedLight',
            //    'ElemsBacking': '^CALL;^MOVEF_32;PORT_CNV_INPUT;INPUT_DEVICE(READY_PCT,,,0,0,1);MOVE32_F;MOVEF_F;$RETURN;JR_EQ32;JR;JR;CP_LTEQF;CP_GTEQF;CP_LTEQF;CP_GTEQF;$RETURN;CP_GT32;OR8;$RETURN;',
            //    'ParamsOut': 'Port,@Result.Value,Amount,Direction'
            //  },
            //  {
            //    'Name': 'ColorSensor.ChangeAmbientLight',
            //    'ElemsBacking': '^CALL;^MOVEF_32;PORT_CNV_INPUT;INPUT_DEVICE(READY_PCT,,,0,1,1);MOVE32_F;MOVEF_F;$RETURN;^JR_EQ32;JR_EQ32;CP_LTEQF;CP_GTEQF;CP_LTEQF;CP_GTEQF;$RETURN;CP_GT32;OR8;$RETURN;',
            //    'ParamsOut': 'Port,@Result.Value,Amount,Direction'
            //  },
            //  {
            //    'Name': 'ColorSensor.CalibrateMinColor',
            //    'ElemsBacking': 'INPUT_DEVICE(CAL_MIN,29,0,);$RETURN;',
            //    'ParamsOut': 'CalibrateValueMin'
            //  },
            //  {
            //    'Name': 'ColorSensor.CalibrateMaxColor',
            //    'ElemsBacking': 'INPUT_DEVICE(CAL_MAX,29,0,);$RETURN;',
            //    'ParamsOut': 'CalibrateValueMax'
            //  },
            //  {
            //    'Name': 'ColorSensor.CalibrateResetColor',
            //    'ElemsBacking': 'INPUT_DEVICE(CAL_DEFAULT,29,0);$RETURN;',
            //    'ParamsOut': ''
            //  },
            //  {
            //    'Name': 'Gyro.MeasureAngle',
            //    'ElemsBacking': 'MOVEF_32;PORT_CNV_INPUT;INPUT_DEVICE(READY_SI,,,32,0,1);MOVEF_F;RETURN;',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result.Angle,Port'
            //  },
            //  {
            //    'Name': 'Gyro.MeasureRate',
            //    'ElemsBacking': 'MOVEF_32;PORT_CNV_INPUT;INPUT_DEVICE(READY_SI,,,32,1,1);MOVEF_F;RETURN;',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result.Rate,Port'
            //  },
            //  {
            //    'Name': 'Gyro.MeasureAngleAndRate',
            //    'ElemsBacking': 'MOVEF_32;PORT_CNV_INPUT;INPUT_DEVICE(READY_SI,,,32,3,2);MOVEF_F;MOVEF_F;RETURN;',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result.Angle,Port,@Result.Rate'
            //  },
            //  {
            //    'Name': 'Gyro.ChangeAngle',
            //    'ElemsBacking': '^CALL;^MOVEF_32;PORT_CNV_INPUT;INPUT_DEVICE(READY_SI,,,32,0,1);MOVEF_F;$RETURN;CALL;^JR_EQ32;JR_EQ32;CP_LTEQF;CP_GTEQF;CP_LTEQF;CP_GTEQF;$RETURN;OR8;$RETURN;',
            //    'ParamsOut': 'Port,@Result.Angle,Change,Direction,,,@Result'
            //  },
            //  {
            //    'Name': 'Gyro.ChangeRate',
            //    'ElemsBacking': '^CALL;^MOVEF_32;PORT_CNV_INPUT;INPUT_DEVICE(READY_SI,,,32,1,1);MOVEF_F;$RETURN;CALL;^JR_EQ32;JR_EQ32;CP_LTEQF;CP_GTEQF;CP_LTEQF;CP_GTEQF;$RETURN;OR8;$RETURN;',
            //    'ParamsOut': 'Port,@Result.Rate,Change,Direction,,,@Result'
            //  },
            //  {
            //    'Name': 'Gyro.CompareAngle',
            //    'ElemsBacking': 'INPUT_DEVICE(READY_SI,,,32,0,1);CP_EQF;CP_GTF;CP_GTEQF;CP_NEQF;CP_LTF;CP_LTEQF;',
            //    'ParamsOut': 'Port,@Result.Angle,Threshold,Comparison,,@Result'
            //  },
            //  {
            //    'Name': 'Gyro.CompareRate',
            //    'ElemsBacking': 'INPUT_DEVICE(READY_SI,,,32,1,1);CP_EQF;CP_GTF;CP_GTEQF;CP_NEQF;CP_LTF;CP_LTEQF;',
            //    'ParamsOut': 'Port,@Result.Rate,Threshold,Comparison,,@Result'
            //  },
            //  {
            //    'Name': 'Gyro.Reset',
            //    'ElemsBacking': '^MOVEF_32;INPUT_DEVICE(READY_SI,,,32,0,1);INPUT_WRITE(,,1);INPUT_READY;$RETURN;',
            //    'ParamsOut': 'Port'
            //  },

            //  {
            //    'Name': 'RotationSensor.MeasureDegrees',
            //    'ElemsBacking': 'MOVEF_32;PORT_CNV_INPUT;ADD8(16);INPUT_DEVICE(READY_SI,,,0,0,1,);MOVEF_F;RETURN;',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result.Degrees,MotorPort'
            //  },
            //  {
            //    'Name': 'RotationSensor.MeasureRotation',
            //    'ElemsBacking': 'MOVEF_32;PORT_CNV_INPUT;ADD8(,16);INPUT_DEVICE(READY_SI,,,0,1,1,);MOVEF_F;RETURN;',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result.Rotations,MotorPort'
            //  },
            //  {
            //    'Name': 'RotationSensor.MeasureCurrentSpeed',
            //    'ElemsBacking': 'MOVEF_32;PORT_CNV_INPUT;ADD8(16);INPUT_DEVICE(READY_SI,,,0,2,1,);MOVEF_F;RETURN;',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result.Speed,MotorPort'
            //  },
            //  {
            //    'Name': 'RotationSensor.CompareDegrees',
            //    'ElemsBacking': 'INPUT_DEVICE(READY_SI,,,0,0,1);CP_EQF;CP_GTF;CP_GTEQF;CP_NEQF;CP_LTF;CP_LTEQF;',
            //    'ParamsOut': 'MotorPort,@Result.Degrees,ThresholdDegrees,Comparison,,@Result'
            //  },
            //  {
            //    'Name': 'RotationSensor.CompareRotation',
            //    'ElemsBacking': 'INPUT_DEVICE(READY_SI,,,0,1,1);CP_EQF;CP_GTF;CP_GTEQF;CP_NEQF;CP_LTF;CP_LTEQF;',
            //    'ParamsOut': 'MotorPort,@Result.Rotations,ThresholdRotations,Comparison,,@Result'
            //  },
            //  {
            //    'Name': 'RotationSensor.CompareCurrentSpeed',
            //    'ElemsBacking': 'INPUT_DEVICE(READY_SI,,,0,2,1);CP_EQF;CP_GTF;CP_GTEQF;CP_NEQF;CP_LTF;CP_LTEQF;',
            //    'ParamsOut': 'MotorPort,@Result.Speed,ThresholdSpeed,Comparison,,@Result'
            //  },
            //  {
            //    'Name': 'RotationSensor.Reset',
            //    'ElemsBacking': 'MOVEF_32;PORT_CNV_OUTPUT;OUTPUT_CLR_COUNT;TIMER_WAIT(25);TIMER_READY;RETURN;',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': 'MotorPort'
            //  },
            //  {
            //    'Name': 'RotationSensor.ChangeDegrees',
            //    'ElemsBacking': 'MOVEF_32;PORT_CNV_INPUT;ADD8(16);INPUT_DEVICE(READY_SI,,,0,0,1,);MOVEF_F;$RETURN;^JR_EQ32;JR_EQ32;CP_LTEQF;CP_GTEQF;CP_LTEQF;CP_GTEQF;$RETURN;',
            //    'ParamsOut': 'MotorPort,@Result.Degrees,Amount,Direction'
            //  },
            //  {
            //    'Name': 'RotationSensor.ChangeRotation',
            //    'ElemsBacking': 'MOVEF_32;PORT_CNV_INPUT;ADD8(,16);INPUT_DEVICE(READY_SI,,,0,1,1,);MOVEF_F;$RETURN;^JR_EQ32;JR_EQ32;CP_LTEQF;CP_GTEQF;CP_LTEQF;CP_GTEQF;$RETURN;',
            //    'ParamsOut': 'MotorPort,@Result.Rotations,AmountRotations,Direction'
            //  },
            //  {
            //    'Name': 'RotationSensor.ChangeSpeed',
            //    'ElemsBacking': 'MOVEF_32;PORT_CNV_INPUT;ADD8(16);INPUT_DEVICE(READY_SI,,,0,2,1,);MOVEF_F;$RETURN;^JR_EQ32;JR_EQ32;CP_LTEQF;CP_GTEQF;CP_LTEQF;CP_GTEQF;$RETURN;',
            //    'ParamsOut': 'MotorPort,@Result.Speed,Amount,Direction'
            //  },

            //  {
            //    'Name': 'TouchSensor.Measure',
            //    'ElemsBacking': '^MOVEF_32;PORT_CNV_INPUT;INPUT_READ(LOCAL,LOCAL,0,0,LOCAL);CP_NEQ8;MOVE8_8;$RETURN;',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': 'Port,@Result.State'
            //  },
            //  {
            //    'Name': 'TouchSensor.Compare',
            //    'ElemsBacking': '^MOVE32_32;PORT_CNV_INPUT;INPUT_READ(LOCAL,LOCAL,0,0,LOCAL);CP_EQ8;INPUT_DEVICE(GET_CHANGES);INPUT_DEVICE(CLR_CHANGES);',
            //    'MatchType': 'NoSubCalls',
            //    'ParamsOut': 'Port,@Result.TouchValue,Pressed__Released_or_Bumped,,@Result.Value'
            //  },
            //  {
            //    'Name': 'TouchSensor.ChangeState',
            //    'ElemsBacking': '^MOVEF_32;PORT_CNV_INPUT;INPUT_READ(LOCAL,LOCAL,1,0,LOCAL);JR_EQ32;CP_NEQ8;$RETURN;',
            //    'MatchType': 'NoSubCalls',
            //    'ParamsOut': 'Port,,,,@Result.State'
            //  },


            //  {
            //    'Name': 'BrickButton.Measure',
            //    'ElemsBacking': '^MOVE32_32;UI_BUTTON(PRESSED);CP_NEQ8;CP_EQ32;JR_EQ32(,1);JR_EQ32(,3);JR_EQ32(,4);JR_EQ32(,5);JR_EQ32(,6);$RETURN;',
            //    'MatchType': 'NoSubCalls',
            //    'ParamsOut': '@Result.Value'
            //  },
            //  {
            //    'Name': 'BrickButton.Compare',
            //    'ElemsBacking': '^MOVE32_32;ARRAY(SIZE);ARRAY_READ;UI_BUTTON(PRESSED);UI_BUTTON(PRESSED);UI_BUTTON(GET_BUMBED);$RETURN;',
            //    'ParamsOut': '@Result,Action,,@cmd.globals @out.Buttons,@Result.Value'
            //  },
            //  {
            //    'Name': 'BrickButton.ChangeBrickButton',
            //    'ElemsBacking': '^CALL;^MOVE32_32;UI_BUTTON(PRESSED);CP_NEQ8;CP_EQ32;JR_EQ32(,1);JR_EQ32(,3);JR_EQ32(,4);JR_EQ32(,5);JR_EQ32(,6);$RETURN;JR_EQ32;CP_GT32;CP_NEQF;OR8;$RETURN;',
            //    'ParamsOut': '@Result.Value,,'
            //  },
            //  {
            //    'Name': 'Timer.MeasureTime',
            //    'ElemsBacking': '^MOVEF_8;CALL(,LOCAL,LOCAL,0);DIVF(,1000);$RETURN;',
            //    'ParamsOut': 'Timer,@Result.Timer_Value'
            //  },
            //  {
            //    'Name': 'Timer.CompareTime',
            //    'ElemsBacking': '^MOVE32_32;AND32;CALL(,LOCAL,LOCAL,0);DIVF(,1000);^JR_EQ32;CP_EQF;CP_GTF;CP_GTEQF;CP_NEQF;CP_LTF;CP_LTEQF;$RETURN;OR8;MOVE8_8;$RETURN;',
            //    'ParamsOut': 'Timer,@Result.Timer_Value,Threshold,Comparison,,@Result'
            //  },
            //  {
            //    'Name': 'Timer.Reset',
            //    'ElemsBacking': '^MOVEF_8;CALL(,LOCAL,LOCAL,1);$RETURN;',
            //    'ParamsOut': 'Timer,,Threshold,Comparison,'
            //  },
            //  {
            //    'Name': 'Sound.File',
            //    'ElemsBacking': 'SOUND(PLAY);',
            //    'ParamsOut': 'Volume,,Play_Type,@cmd.globals @out.Name'
            //  },
            //  {
            //    'Name': 'Sound.Stop',
            //    'ElemsBacking': 'SOUND(BREAK);',
            //    'ParamsOut': null
            //  },
            //  {
            //    'Name': 'Sound.Note',
            //    'ElemsBacking': 'NOTE_TO_FREQ;SOUND(TONE);',
            //    'ParamsOut': 'Duration,Volume,,Play_Type,@cmd.globals @out.Note'
            //  },
            //  {
            //    'Name': 'Sound.Tone',
            //    'ElemsBacking': 'SOUND(TONE);',
            //    'ParamsOut': 'Frequency,Duration,Volume,,Play_Type'
            //  },
            //  {
            //    'Name': 'LED.On',
            //    'ElemsBacking': 'UI_WRITE(LED,LOCAL);',
            //    'ParamsOut': 'Color,,Pulse'
            //  },
            //  {
            //    'Name': 'LED.Off',
            //    'ElemsBacking': 'UI_WRITE(LED,0);',
            //    'ParamsOut': null
            //  },
            //  {
            //    'Name': 'LED.Reset',
            //    'ElemsBacking': 'UI_WRITE(LED,7);',
            //    'ParamsOut': null
            //  },
            //  {
            //    'Name': 'Display.File',
            //    'ElemsBacking': 'UI_DRAW(BMPFILE);',
            //    'ParamsOut': 'X,Y,,@cmd.globals @out.Filename,Clear_Screen'
            //  },
            //  {
            //    'Name': 'Display.Clear',
            //    'ElemsBacking': 'UI_WRITE(INIT_RUN);',
            //    'ParamsOut': null
            //  },
            //  {
            //    'Name': 'Display.StringGrid',
            //    'ElemsBacking': 'MULF(,10); MULF(,8); UI_DRAW(TEXT);',
            //    'ParamsOut': 'Row,Column,Size,,@cmd.globals @out.Text,Clear_Screen,Invert_Color'
            //  },
            //  {
            //    'Name': 'Display.String',
            //    'ElemsBacking': 'UI_DRAW(TEXT);',
            //    'ParamsOut': 'X,Y,Size,,@cmd.globals @out.Text,Clear_Screen,Invert_Color'
            //  },
            //  {
            //    'Name': 'Display.Point',
            //    'ElemsBacking': 'UI_DRAW(PIXEL);',
            //    'ParamsOut': 'X,Y,,Invert_Color,Clear_Screen'
            //  },
            //  {
            //    'Name': 'Display.Line',
            //    'ElemsBacking': 'UI_DRAW(LINE);',
            //    'ParamsOut': 'X1,Y1,X2,Y2,,Invert_Color,Clear_Screen'
            //  },
            //  {
            //    'Name': 'Display.Circle',
            //    'ElemsBacking': 'UI_DRAW(CIRCLE);',
            //    'ParamsOut': 'X,Y,Radius,,Clear_Screen,Fill,Invert_Color'
            //  },
            //  {
            //    'Name': 'Display.Rectangle',
            //    'ElemsBacking': 'UI_DRAW(RECT);',
            //    'ParamsOut': 'X,Y,Width,Height,,Clear_Screen,Fill,Invert_Color'
            //  },
            //  {
            //    'Name': 'StopBlock',
            //    'ElemsBacking': 'PROGRAM_STOP;'
            //  },
            //  {
            //    'Name': 'FileAccess.Text',
            //    'ElemsBacking': 'FILE(READ_TEXT);',
            //    'ParamsOut': '@Result.Text,@cmd.globals @out.FileName'
            //  },
            //  {
            //    'Name': 'FileAccess.Numeric',
            //    'ElemsBacking': 'FILE(READ_VALUE);',
            //    'ParamsOut': '@Result.Numeric,@cmd.globals @out.FileName'
            //  },
            //  {
            //    'Name': 'FileAccess.Write',
            //    'ElemsBacking': 'FILE(WRITE_TEXT);',
            //    'ParamsOut': ',@cmd.globals @out.FileName,@cmd.globals @out.TextIn'
            //  },
            //  {
            //    'Name': 'FileAccess.Delete',
            //    'ElemsBacking': 'FILE(REMOVE);',
            //    'ParamsOut': ',@cmd.globals @out.FileName'
            //  },
            //  {
            //    'Name': 'FileAccess.Close',
            //    'ElemsBacking': 'FILE(CLOSE);ARRAY(DELETE);',
            //    'ParamsOut': ',@cmd.globals @out.FileName'
            //  },
            //  {
            //    'Name': 'Math.Add',
            //    'ElemsBacking': 'ADDF(LOCAL,LOCAL,LOCAL);MOVEF_F(LOCAL,LOCAL);RETURN();',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result,X,Y'
            //  },
            //  {
            //    'Name': 'Math.Subtract',
            //    'ElemsBacking': 'SUBF(LOCAL,LOCAL,LOCAL);MOVEF_F(LOCAL,LOCAL);RETURN();',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result,X,Y'
            //  },
            //  {
            //    'Name': 'Math.Multiply',
            //    'ElemsBacking': '^MULF(LOCAL,LOCAL,LOCAL);MOVEF_F(LOCAL,LOCAL);$RETURN;',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result,X,Y'
            //  },
            //  {
            //    'Name': 'Math.Divide',
            //    'ElemsBacking': 'DIVF(LOCAL,LOCAL,LOCAL);MOVEF_F(LOCAL,LOCAL);RETURN();',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result,X,Y'
            //  },
            //  {
            //    'Name': 'Math.AbsoluteValue',
            //    'ElemsBacking': 'MATH(ABS,LOCAL,LOCAL);MOVEF_F(LOCAL,LOCAL);RETURN();',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result,X'
            //  },
            //  {
            //    'Name': 'Math.SquareRoot',
            //    'ElemsBacking': 'MATH(SQRT,LOCAL,LOCAL);MOVEF_F(LOCAL,LOCAL);RETURN();',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result,X'
            //  },
            //  {
            //    'Name': 'Math.Exponent',
            //    'ElemsBacking': 'MATH(POW,LOCAL,LOCAL);MOVEF_F(LOCAL,LOCAL);RETURN();',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result,Base,Exponent'
            //  },
            //  {
            //    'Name': 'Random.Numeric',
            //    'ElemsBacking': 'RANDOM(LOCAL,LOCAL,LOCAL);',
            //    'ParamsOut': '@Result.Number,Lower,Upper'
            //  },
            //  {
            //    'Name': 'Random.Boolean',
            //    'ElemsBacking': 'RANDOM(1,100,LOCAL);CP_LTEQF();',
            //    'ParamsOut': 'Percent_True,@Result'
            //  },
            //  {
            //    'Name': 'Text.Merge',
            //    'ElemsBacking': 'STRINGS(ADD);STRINGS(ADD);ARRAY(COPY);',
            //    'ParamsOut': '@Result,@cmd.globals @out.A,@cmd.globals @out.B,@cmd.globals @out.C'
            //  },
            //  {
            //    'Name': 'BooleanOperations.And',
            //    'ElemsBacking': 'AND8(LOCAL,LOCAL,LOCAL);MOVE8_8(LOCAL,LOCAL);RETURN();',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result,A,B'
            //  },
            //  {
            //    'Name': 'BooleanOperations.Or',
            //    'ElemsBacking': 'OR8(LOCAL,LOCAL,LOCAL);MOVE8_8(LOCAL,LOCAL);RETURN();',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result,A,B'
            //  },
            //  {
            //    'Name': 'BooleanOperations.XOR',
            //    'ElemsBacking': 'XOR8(LOCAL,LOCAL,LOCAL);MOVE8_8(LOCAL,LOCAL);RETURN();',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result,A,B'
            //  },
            //  {
            //    'Name': 'BooleanOperations.Not',
            //    'ElemsBacking': 'XOR8(LOCAL,1,LOCAL);MOVE8_8(LOCAL,LOCAL);RETURN();',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result,A'
            //  },
            //  {
            //    'Name': 'Compare.Equal',
            //    'ElemsBacking': 'CP_EQF(LOCAL,LOCAL,LOCAL);MOVE8_8(LOCAL,LOCAL);RETURN();',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': 'x,y,@Result'
            //  },
            //  {
            //    'Name': 'Compare.NotEqual',
            //    'ElemsBacking': 'CP_NEQF(LOCAL,LOCAL,LOCAL);MOVE8_8(LOCAL,LOCAL);RETURN();',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': 'x,y,@Result'
            //  },
            //  {
            //    'Name': 'Compare.GreaterThan',
            //    'ElemsBacking': 'CP_GTF(LOCAL,LOCAL,LOCAL);MOVE8_8(LOCAL,LOCAL);RETURN();',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': 'x,y,@Result'
            //  },
            //  {
            //    'Name': 'Compare.GreaterOrEqual',
            //    'ElemsBacking': 'CP_GTEQF(LOCAL,LOCAL,LOCAL);MOVE8_8(LOCAL,LOCAL);RETURN();',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': 'x,y,@Result'
            //  },
            //  {
            //    'Name': 'Compare.LessThan',
            //    'ElemsBacking': 'CP_LTF(LOCAL,LOCAL,LOCAL);MOVE8_8(LOCAL,LOCAL);RETURN();',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': 'x,y,@Result'
            //  },
            //  {
            //    'Name': 'Compare.LessOrEqual',
            //    'ElemsBacking': 'CP_LTEQF(LOCAL,LOCAL,LOCAL);MOVE8_8(LOCAL,LOCAL);RETURN();',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': 'x,y,@Result'
            //  },
            //  {
            //    'Name': 'Range.Inside',
            //    'ElemsBacking': 'CP_GTEQF();CP_GTEQF();AND8();MOVE8_8();RETURN();',
            //    'ParamsOut': 'Test_Value,Lower_Bound,Upper_Bound,@Result'
            //  },
            //  {
            //    'Name': 'Range.Outside',
            //    'ElemsBacking': 'CP_LTF();CP_LTF();OR8();MOVE8_8();RETURN();',
            //    'ParamsOut': 'Test_Value,Lower_Bound,Upper_Bound,@Result'
            //  },
            //  {
            //    'Name': 'Round.Nearest',
            //    'ElemsBacking': 'MATH(ROUND);MOVEF_F();RETURN();',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result.Output_Result,Input'
            //  },
            //  {
            //    'Name': 'Round.Up',
            //    'ElemsBacking': 'MATH(CEIL);MOVEF_F();RETURN();',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result.Output_Result,Input'
            //  },
            //  {
            //    'Name': 'Round.Down',
            //    'ElemsBacking': 'MATH(FLOOR);MOVEF_F();RETURN();',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result.Output_Result,Input'
            //  },
            //  {
            //    'Name': 'Round.Truncate',
            //    'ElemsBacking': 'MOVEF_8();MATH(TRUNC);MOVEF_F();RETURN();',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result.Output_Result,Input,Number_of_Decimals'
            //  },
            //  {
            //    'Name': 'Interrupt',
            //    'ElemsBacking': 'MOVE32_32(GLOBAL,LOCAL);OR32;MOVE32_32(LOCAL,GLOBAL);$RETURN;',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': 'InterruptValue,@cmd.globals @out.InterruptName'
            //  },
            //  {
            //    'Name': 'Messaging.SendText',
            //    'ElemsBacking': 'CALL(,,GLOBAL,GLOBAL,GLOBAL);^MOVE32_32;MAILBOX_WRITE;$RETURN;',
            //    'ParamsOut': ',@cmd.globals @out.Message_Title,@cmd.globals @out.Receiving_Brick_Name,@cmd.globals @out.SentMessage'
            //  },
            //  {
            //    'Name': 'Messaging.SendNumeric',
            //    'ElemsBacking': 'CALL(,,,GLOBAL,GLOBAL);^MAILBOX_WRITE;$RETURN;',
            //    'ParamsOut': 'SentMessage,,@cmd.globals @out.Message_Title,@cmd.globals @out.Receiving_Brick_Name'
            //  },
            //  {
            //    'Name': 'Messaging.SendBoolean',
            //    'ElemsBacking': 'CALL(,,GLOBAL,GLOBAL,);^MOVE32_32;MAILBOX_WRITE;$RETURN;',
            //    'ParamsOut': ',@cmd.globals @out.Message_Title,@cmd.globals @out.Receiving_Brick_Name,SentMessage'
            //  },
            //  {
            //    'Name': 'Messaging.ReceiveText',
            //    'ElemsBacking': '^ARRAY(CREATE8);MAILBOX_READ(,250);ARRAY(COPY);ARRAY(DELETE);$RETURN;',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result.ReceivedMessage'
            //  },
            //  {
            //    'Name': 'Messaging.ReceiveNumeric',
            //    'ElemsBacking': '^MAILBOX_READ(,4,);MOVEF_F;$RETURN;',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result.ReceivedMessage'
            //  },
            //  {
            //    'Name': 'Messaging.ReceiveBoolean',
            //    'ElemsBacking': '^MAILBOX_READ(,1,);MOVE8_8;$RETURN;',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result.ReceivedMessage'
            //  },
            //  {
            //    'Name': 'Messaging.CompareText',
            //    'ElemsBacking': '^ARRAY(CREATE8);MAILBOX_READ(,250);STRINGS(COMPARE);ARRAY(COPY);CP_GT32;XOR8;ARRAY(DELETE);$RETURN;',
            //    'ParamsOut': 'Comparison2,,@cmd.globals @out.Message_Title,@Result.ReceivedMessage,@cmd.globals @out.ComparisonText,,@Result'
            //  },
            //  {
            //    'Name': 'Messaging.CompareNumeric',
            //    'ElemsBacking': '^MAILBOX_READ(,4,);MOVEF_F;^JR_EQ32;CP_EQF;CP_GTF;CP_GTEQF;CP_NEQF;CP_LTF;CP_LTEQF;$RETURN;OR8;$RETURN;',
            //    'ParamsOut': '@Result.ReceivedMessage,Threshold,,Comparison,@cmd.globals @out.Message_Title,,@Result'
            //  },
            //  {
            //    'Name': 'Messaging.CompareBoolean',
            //    'ElemsBacking': '^MAILBOX_READ(,1,);MOVE32_32;AND32;CP_GT32;OR8;MOVE8_8;MOVE8_8;$RETURN;',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@cmd.globals @out.Message_Title,,@Result,@Result.ReceivedMessage'
            //  },
            //  {
            //    'Name': 'UnregulatedMotor',
            //    'ElemsBacking': '^MOVE32_32;PORT_CNV_OUTPUT;OUTPUT_POWER;OUTPUT_START;JR;JR;$RETURN;',
            //    'ParamsOut': 'MotorPort,Power'
            //  },
            //  {
            //    'Name': 'RawSensorValue',
            //    'ElemsBacking': '^MOVEF_32;PORT_CNV_INPUT;INPUT_DEVICE(READY_RAW);$RETURN;',
            //    'ParamsOut': '@Result.Raw_Value,Port_Number'
            //  },

            //  {
            //    'Name': 'CaseSelector.Numeric',
            //    'ElemsBacking': '^MOVE32_32(LOCAL,LOCAL0);$RETURN;',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result,Number'
            //  },
            //  {
            //    'Name': 'CaseSelector.Boolean',
            //    'ElemsBacking': '^MOVE8_8(LOCAL,LOCAL0);$RETURN;',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result,Boolean'
            //  },
            //  {
            //    'Name': 'CaseSelector.String',
            //    'ElemsBacking': '^ARRAY(COPY,LOCAL,LOCAL0);$RETURN;',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result,@cmd.globals @out.String'
            //  },

            //  {
            //    'Name': 'ArrayOperations.Append_Numeric',
            //    'ElemsBacking': 'ARRAY(CREATEF,1);ARRAY(COPY);ARRAY_APPEND;ARRAY(COPY);ARRAY(DELETE);RETURN',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': 'valueIn,@Result.arrayOutNumeric,@cmd.globals @out.arrayInNumeric'
            //  },
            //  {
            //    'Name': 'ArrayOperations.Append_Boolean',
            //    'ElemsBacking': 'ARRAY(CREATE8,1);ARRAY(COPY);ARRAY_APPEND;ARRAY(COPY);ARRAY(DELETE);RETURN',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result.arrayOutBoolean,@cmd.globals @out.arrayInBoolean,valueIn'
            //  },
            //  {
            //    'Name': 'ArrayOperations.ReadAtIndex_Numeric',
            //    'ElemsBacking': 'MOVEF_32;ARRAY_READ;MOVEF_F;RETURN',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result.valueOut,Index,@cmd.globals @out.arrayInNumeric'
            //  },
            //  {
            //    'Name': 'ArrayOperations.ReadAtIndex_Boolean',
            //    'ElemsBacking': 'MOVEF_32;ARRAY_READ;MOVE8_8;RETURN',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': 'Index,@cmd.globals @out.arrayInBoolean,@Result.valueOut'
            //  },
            //  {
            //    'Name': 'ArrayOperations.WriteAtIndex_Numeric',
            //    'ElemsBacking': 'ARRAY(CREATEF,1);MOVEF_32;ARRAY(COPY);ARRAY_WRITE;ARRAY(COPY);ARRAY(DELETE);RETURN',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': 'Index,Value,@Result.arrayOutNumeric,@cmd.globals @out.arrayInNumeric'
            //  },
            //  {
            //    'Name': 'ArrayOperations.WriteAtIndex_Boolean',
            //    'ElemsBacking': 'ARRAY(CREATE8,1);MOVEF_32;ARRAY(COPY);ARRAY_WRITE;ARRAY(COPY);ARRAY(DELETE);RETURN',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': 'Index,@Result.arrayOutBoolean,@cmd.globals @out.arrayInBoolean,Value'
            //  },
            //  {
            //    'Name': 'ArrayOperations.Length_Numeric',
            //    'ElemsBacking': 'ARRAY(SIZE);MOVE32_F;MOVEF_F;RETURN',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result.Size,@cmd.globals @out.arrayInNumeric'
            //  },


            //  {
            //    'Name': 'InfraredSensor.MeasureProximity',
            //    'ElemsBacking': 'MOVEF_32;PORT_CNV_INPUT;INPUT_DEVICE(READY_SI,,,33,0,1);MOVEF_F;RETURN',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result.Proximity,Port'
            //  },
            //  {
            //    'Name': 'InfraredSensor.MeasureBeaconSeeker',
            //    'ElemsBacking': 'MOVEF_32;PORT_CNV_INPUT;INPUT_DEVICE(READY_SI,,,33,1,8);MOVEF_F;RETURN',
            //    'MatchType': 'NoSubCalls',
            //    'ParamsOut': '@Result.Heading,Port,@Result.Proximity,Channel,@Result.Valid'
            //  },
            //  {
            //    'Name': 'InfraredSensor.MeasureBeaconRemote',
            //    'ElemsBacking': 'MOVEF_32;PORT_CNV_INPUT;INPUT_DEVICE(READY_SI,,,0,2,4);MOVEF_F;MOVEF_F;RETURN',
            //    'MatchType': 'NoSubCalls',
            //    'ParamsOut': 'Port,@Result.Button,Channel'
            //  },
            //  {
            //    'Name': 'InfraredSensor.CompareProximity',
            //    'ElemsBacking': 'INPUT_DEVICE(READY_SI,,,33,0,1);CP_EQF;CP_GTF;CP_GTEQF;CP_NEQF;CP_LTF;CP_LTEQF;$RETURN;CP_GT32;OR8;$RETURN',
            //    'ParamsOut': 'Port,@Result.Proximity,Threshold,Comparison,,@Result'
            //  },
            //  {
            //    'Name': 'InfraredSensor.CompareBeaconSeekerHeading',
            //    'ElemsBacking': 'INPUT_DEVICE(READY_SI,,,33,1,8);CP_EQF;CP_GTF;CP_GTEQF;CP_NEQF;CP_LTF;CP_LTEQF;$RETURN;CP_GT32;OR8;$RETURN',
            //    'ParamsOut': 'Port,@Result.Proximity,Channel,HeadingThreshold,Comparison,Heading,@Result'
            //  },
            //  {
            //    'Name': 'InfraredSensor.CompareRemote',
            //    'ElemsBacking': 'INPUT_DEVICE(READY_SI,,,0,2,4);CP_EQF;CP_EQ32;OR8;$RETURN',
            //    'ParamsOut': 'Port,Channel,@Result.Button,,@cmd.globals @out.Set_of_remote_button_IDs,@Result'
            //  },

            //  {
            //    'Name': 'InfraredSensor.ChangeProximity',
            //    'ElemsBacking': '^CALL;^MOVEF_32;PORT_CNV_INPUT;INPUT_DEVICE(READY_SI,,,33,0,1);MOVEF_F;$RETURN;CALL;CP_LTEQF;CP_GTEQF;CP_LTEQF;CP_GTEQF;$RETURN',
            //    'ParamsOut': 'Port,@Result.Proximity,Amount,Direction,,,'
            //  },
            //  {
            //    'Name': 'InfraredSensor.ChangeHeading',
            //    'ElemsBacking': '^CALL;^MOVEF_32;PORT_CNV_INPUT;INPUT_DEVICE(READY_SI,,,33,1,8);MOVEF_F;$RETURN;MOVEF_F(LOCAL44);CP_LTEQF;CP_GTEQF;CP_LTEQF;CP_GTEQF;$RETURN',
            //    'ParamsOut': 'Port,Channel,@Result.Heading,HeadingAmount,Direction,,,'
            //  },
            //  {
            //    'Name': 'InfraredSensor.ChangeBeaconProximity',
            //    'ElemsBacking': '^CALL;^MOVEF_32;PORT_CNV_INPUT;INPUT_DEVICE(READY_SI,,,33,1,8);MOVEF_F;$RETURN;MOVEF_F(LOCAL32);CP_LTEQF;CP_GTEQF;CP_LTEQF;CP_GTEQF;$RETURN',
            //    'ParamsOut': 'Port,Channel,@Result.Heading,Amount,Direction,,,'
            //  },
            //  {
            //    'Name': 'InfraredSensor.ChangeRemote',
            //    'ElemsBacking': '^CALL;^MOVEF_32;PORT_CNV_INPUT;INPUT_DEVICE(READY_SI,,,0,2,4);MOVEF_F;$RETURN;MOVEF_F;CP_NEQF;CP_GT32;OR8;$RETURN',
            //    'ParamsOut': 'Port,@Result.Button,Channel,,,'
            //  },


            //  {
            //    'Name': 'UltrasonicSensor.MeasureCentimeters',
            //    'ElemsBacking': 'MOVEF_32;PORT_CNV_INPUT;INPUT_DEVICE(READY_SI,,,0,0,1);MOVEF_F;RETURN',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result.Distance,Port'
            //  },
            //  {
            //    'Name': 'UltrasonicSensor.MeasureInches',
            //    'ElemsBacking': 'MOVEF_32;PORT_CNV_INPUT;INPUT_DEVICE(READY_SI,,,0,1,1);MOVEF_F;RETURN',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': '@Result.DistanceInches,Port'
            //  },
            //  {
            //    'Name': 'UltrasonicSensor.MeasurePresence',
            //    'ElemsBacking': 'MOVEF_32;PORT_CNV_INPUT;INPUT_DEVICE(READY_PCT,,,0,2,1);CP_GT32;MOVE8_8;RETURN',
            //    'MatchType': 'NoSubCalls',
            //    'ParamsOut': 'Port,@Result.Heard'
            //  },
            //  {
            //    'Name': 'UltrasonicSensor.Centimeters',
            //    'ElemsBacking': 'MOVEF_32;PORT_CNV_INPUT;INPUT_DEVICE(READY_SI,,,0,0,1);INPUT_DEVICE(READY_SI,,,0,LOCAL,1);MOVEF_F;RETURN',
            //    'MatchType': 'NoSubCalls',
            //    'ParamsOut': '@Result.Distance,Port,Measuring_Mode'
            //  },
            //  {
            //    'Name': 'UltrasonicSensor.Inches',
            //    'ElemsBacking': 'MOVEF_32;PORT_CNV_INPUT;INPUT_DEVICE(READY_SI,,,0,1,1);INPUT_DEVICE(READY_SI,,,0,LOCAL,1);MOVEF_F;RETURN',
            //    'MatchType': 'NoSubCalls',
            //    'ParamsOut': '@Result.DistanceInches,Port,Measuring_Mode'
            //  },

            //  {
            //    'Name': 'CommentBlock',
            //    'ElemsBacking': 'RETURN;',
            //    'MatchType': 'NoSubCallsExact',
            //    'ParamsOut': ''
            //  },
            //  {
            //    'Name': 'KeepAlive',
            //    'ElemsBacking': 'KEEP_ALIVE',
            //    'MatchType': 'NoSubCalls',
            //    'ParamsOut': 'Time_until_sleep_(ms)'
            //  },

            //  {
            //    'Name': 'Bluetooth.On',
            //    'ElemsBacking': 'COM_SET(SET_ON_OFF,2,1)',
            //    'MatchType': 'NoSubCalls',
            //    'ParamsOut': ''
            //  },
            //  {
            //    'Name': 'Bluetooth.Off',
            //    'ElemsBacking': 'COM_SET(SET_ON_OFF,2,0)',
            //    'MatchType': 'NoSubCalls',
            //    'ParamsOut': ''
            //  },
            //  {
            //    'Name': 'Bluetooth.Initiate',
            //    'ElemsBacking': 'COM_SET(SET_CONNECTION,2,,1)',
            //    'MatchType': 'NoSubCalls',
            //    'ParamsOut': ',@cmd.globals @out.Connect_To'
            //  },
            //  {
            //    'Name': 'Bluetooth.Clear',
            //    'ElemsBacking': 'COM_SET(SET_CONNECTION,2,,0)',
            //    'MatchType': 'NoSubCalls',
            //    'ParamsOut': ',@cmd.globals @out.Connect_To'
            //  },
            //]
            //";

            //            //TODO: Add Sound.PlayRecordedSoundFile --> EV3programming / iPad --> PlayRecordedSoundFile.gvi
            //            //TODO: Add Byte code version: 1.04

            //            var patterns = JsonConvert.DeserializeObject(json, typeof(List<Pattern>)) as List<Pattern>;
            //            patterns.ForEach(elem => elem.SetElems(elem.ElemsBacking));

            //-- if testmode is set on ANY pattern -> use only those patterns
            var patterns = PATTERNS;
            if (patterns.Any(elem => elem.TestMode))
            {
                patterns = patterns.Where(elem => elem.TestMode).ToList();
                Console.WriteLine($"Test mode activated - testing patterns: {string.Join(", ", patterns.Select(elem => elem.Name).ToArray())}");
                if (PatternMatcher.DEBUG_PRINT_LEVEL == 0) PatternMatcher.DEBUG_PRINT_LEVEL = 4;
            }

            return patterns;
        }
        #endregion static part
    }
}
