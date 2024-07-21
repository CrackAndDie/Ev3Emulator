using System.Collections.Generic;
using System.Linq;

namespace EV3ModelLib
{
    /// <summary>
    /// internal class to colorize pmd tree items
    /// </summary>
    public static class BlockFamilyColorizer
    {
        internal const string CONST_BlockFamily_FlowControl = "FlowControl";
        internal const string CONST_BlockFamily_Action = "Action";
        internal const string CONST_BlockFamily_Sensor = "Sensor";
        internal const string CONST_BlockFamily_DataOperations = "DataOperations";
        internal const string CONST_BlockFamily_Advanced = "Advanced";
        internal const string CONST_BlockFamily_MyBlocks = "MyBlocks";

        /// <summary>
        /// static ctor
        /// </summary>
        static BlockFamilyColorizer()
        {
            // creat block color map
            BlockColorMap = new Dictionary<string, string[]>();
            BlockReference = new Dictionary<string, string>();
            BlockInfo.BlockMapByRef.ToList().ForEach(elem =>
            //BlockTranslateMap.ToList().ForEach(elem =>
            {
                var shortname = elem.Value.ShortName;
                var blockfamily = elem.Value.BlockFamily;
                var bf = BlockFamilyColors[blockfamily];
                BlockColorMap[shortname] = bf;

                BlockReference[shortname] = elem.Key;
            });

            BlockTypeColorMap = new Dictionary<Node.NodeTypeEnum, string[]>()
            {
                [Node.NodeTypeEnum.Loop] = BlockFamilyColors[CONST_BlockFamily_FlowControl],
                [Node.NodeTypeEnum.CaseItem] = BlockFamilyColors[CONST_BlockFamily_FlowControl],
                [Node.NodeTypeEnum.Connector] = BlockFamilyColors[Node.BLOCK_CONNECTOR],
                [Node.NodeTypeEnum.Empty] = BlockFamilyColors[Node.BLOCK_Empty],
                [Node.NodeTypeEnum.ForkParent] = BlockFamilyColors[Node.BLOCK_Fork],
                [Node.NodeTypeEnum.ForkItem] = BlockFamilyColors[Node.BLOCK_Fork],
                [Node.NodeTypeEnum.MyBlock] = BlockFamilyColors[CONST_BlockFamily_MyBlocks],
                [Node.NodeTypeEnum.Program] = BlockFamilyColors[Node.BLOCK_Root],
            };


            BlockColorSwitchAndWaitFor = BlockFamilyColors[CONST_BlockFamily_FlowControl];
            BlockColorMap[Node.BLOCK_Root] = BlockFamilyColors[Node.BLOCK_Root];
            BlockColorMap[Node.BLOCK_NoMatchFound] = BlockFamilyColors["ERROR"];
            BlockColorMap["PARAMS"] = BlockFamilyColors["PARAMS"];
        }

        /// <summary>
        /// Block Color map block_name -> display colors [html, console]
        /// </summary>
        public static Dictionary<string, string[]> BlockColorMap { get; internal set; }
        public static Dictionary<Node.NodeTypeEnum, string[]> BlockTypeColorMap { get; internal set; }
        public static string[] BlockColorSwitchAndWaitFor { get; internal set; }

        /// <summary>
        /// Block Reference to vix
        /// </summary>
        public static Dictionary<string, string> BlockReference { get; internal set; }

        #region JSON constants for parsing
        static Dictionary<string, string[]> BlockFamilyColors = new Dictionary<string, string[]>
        {
            {
                CONST_BlockFamily_Action,
                new string[] { "#73B939", "Green" }
            },
            {
                CONST_BlockFamily_FlowControl,
                new string[] { "#FEAB26", "DarkYellow" }
            },
            {
                CONST_BlockFamily_Sensor,
                new string[] { "#ECD407", "Yellow" }
            },
            {
                CONST_BlockFamily_DataOperations,
                new string[] { "#E32F00", "Red" }
            },
            {
                CONST_BlockFamily_Advanced,
                new string[] { "#0022B2", "Blue" }
            },
            {
                CONST_BlockFamily_MyBlocks,
                new string[] { "#009A9A", "DarkCyan" }
            },
            {
                Node.BLOCK_Fork,
                new string[] { "#777777", "Gray" }
            },
            {
                Node.BLOCK_CONNECTOR,
                new string[] { "#9400D3", "DarkMagenta" }
            },
            {
                Node.BLOCK_Empty,
                new string[] { "#DDDDDD", "Gray" }
            },
            {
                Node.BLOCK_Root,
                new string[] { "magenta", "Magenta" }
            },
            {
                "ERROR",
                new string[] { "red", "DarkRed" }
            },
            {
                "PARAMS",
                new string[] { "DarkGray", "DarkGray" }
            }
        };
        #endregion JSON constants for parsing
    }
}
