using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using YamlDotNet.Serialization;

namespace EV3ModelLib
{
    [DebuggerDisplay("{Name} {(Children!=null ? \"[\"+Children.Count.ToString()+\"]\":null)}")]
    public class Node
    {
        /// <summary>
        /// DisplayName or ShortName (internal easy-to-ready naming)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Reference name (vix or ev3 name used by lego, if any)
        /// </summary>
        public string RefName { get; set; }

        [JsonProperty(Order = 2)]
        [YamlMember(Order = 2)]
        public List<Node> Children { get; }

        [JsonProperty(Order = 1)]
        [YamlMember(Order = 1)]
        public Dictionary<string, Parameter> Parameters { get; }

        [JsonIgnore]
        [YamlIgnore]
        public Node Parent { get; set; }

        [JsonIgnore]
        [YamlIgnore]
        public object SourceObj { get; set; }

        public bool HasWait { get; set; }

        public bool HasSwitch { get; set; }

        public enum NodeTypeEnum
        {
            None = 0,
            CaseItem = 1, Loop = 2, ForkParent = 3, ForkItem = 4,
            Connector = 5, Empty = 6,
            MyBlock = 100, // this is both a myblock used in a program and a myblock top level node
            Program = 200,

            ProjectExtraRoot = 900, ProjectExtraImages = 901, ProjectExtraSounds = 902, ProjectExtraVariables = 903, ProjectExtraWarnings = 904
        };

        [JsonConverter(typeof(StringEnumConverter))]
        [DefaultValue(NodeTypeEnum.None)]
        public NodeTypeEnum NodeType { get; set; }

        public bool ShouldSerializeChildren()
        {
            return Children?.Count > 0;
        }
        public bool ShouldSerializeParameters()
        {
            return Parameters?.Count > 0;
        }

        public Node()
        {
            Parameters = new Dictionary<string, Parameter>();
            Children = new List<Node>();
        }
        public Node(string name) : this()
        {
            Name = name;
        }
        public Parameter AddSimpleParameter(string name, string value)
        {
            var param1 = new Parameter(name, value, this);
            Parameters.Add(name, param1);
            return param1;
        }
        public static PrintOptionsClass PrintOptions
        {
            get { return PrintHelper.PrintOptions.Value; }
            set { PrintHelper.PrintOptions.Value = value; }
        }

        public const string BLOCK_StartBlock = "StartBlock";
        public const string BLOCK_StopBlock = "StopBlock";
        public const string BLOCK_Interrupt = "Interrupt";
        public const string BLOCK_StartLoop = "StartLoop";
        public const string BLOCK_Fork = "ForkParent";
        public const string BLOCK_ForkItem = "Fork";
        public const string BLOCK_Root = "root";
        public const string BLOCK_TopID = "top";
        public const string BLOCK_SwitchCaseItem = "Case";
        public const string BLOCK_WaitForPrefix1 = "WaitFor";
        public const string BLOCK_SwitchPrefix1 = "Switch";
        public const string BLOCK_LoopPrefix1 = "Loop";
        public const string BLOCK_WaitForPrefix = BLOCK_WaitForPrefix1 + ".";
        public const string BLOCK_SwitchPrefix = BLOCK_SwitchPrefix1 + ".";
        public const string BLOCK_LoopPrefix = BLOCK_LoopPrefix1 + ".";
        public const string BLOCK_MyBlockPrefix = "MYBLOCK";
        public const string BLOCK_Empty = "(None)";
        public const string BLOCK_CONNECTOR = "Connector";
        internal const string BLOCK_ReadVariablePrefix = "Variable.Read";
        internal const string BLOCK_WriteVariablePrefix = "Variable.Write";
        internal const string BLOCK_DataType_Text = "Text";
        internal const string BLOCK_DataType_Boolean = "Boolean";
        internal const string BLOCK_DataType_Numeric = "Numeric";
        internal const string BLOCK_DataType_ArrayPostFix = "Array";
        internal const string BLOCK_CaseSelectorString = "CaseSelector.String";
        public const string BLOCK_MergeToFork = "Merge_to_FORK";
        public const string BLOCK_StartLoopDummy = "Dummy_StartLoop";
        public const string BLOCK_NoMatchFound = "No_Match_Found";
        public const string PARAM_LOOP_InterruptName = "InterruptName";
        public const string PARAM_LOOP_InterruptId = "InterruptId";
        public const string PARAM_LOOP_LoopIndex = "Loop_Index";
        public const string PARAM_LOOP_StopCondition = "Stop";
        public const string PARAM_CONNECTOR_from = "from";
        public const string PARAM_CONNECTOR_to = "to";
        public const string PARAM_WiredSpecial_Src = "Wired_1FAC2752-7229-46A0-AA06-E0731CAB9CAF";
        public const string PARAM_WiredSpecial = "WiredSpecial_Empty";
        public const string PARAM_SWITCH_Pattern = "Pattern";
        public const string PROJECT_IMAGES = "(Images)";
        public const string PROJECT_SOUNDS = "(Sounds)";
        public const string PROJECT_VARS = "(Variables)";
        public const string PROJECT_WARNINGS = "(Warnings)";
        public const string CONST_ATTR_USEDBY = "usedBy";
        public const string CONST_SUB_LITERAL = "Sub";

        /// <summary>
        /// graphic file extensions
        /// </summary>
        public const string EXT_RSF_SOUND = ".rsf";
        public const string EXT_RGF_GRAPHICS = ".rgf";
    }
}
