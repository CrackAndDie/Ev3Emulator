using EV3DecompilerLib.Recognize;
using EV3ModelLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

/*
TODO: add Timer.ChangeTime
TODO: myblocks!
*/
//TODO: ConfigurableWhileLoop.ConfigurableLoopTunnel --> even wire shall be added to root node

namespace EV3DecompilerLib.BuildEV3
{
    public class CreateProgramFromNode : IEV3GZipBlock
    {
        public CreateProgramFromNode(Node node, string sProgName)
        {
            ProgName = sProgName;
            ProgNode = node;
        }

        private Node ProgNode = null;
        private string ProgName = null;
        public int StartX { get; set; } = 0;
        public int StartY { get; set; } = 0;
        public string Name { get { return FileNameConverter(ProgName); } }

        public static string FileNameConverter(string fname)
        {
            return Path.GetFileNameWithoutExtension(fname) + ".ev3p";
        }

        private static XElement AddConfigurableMethodTerminal(string Id, string configuredValue, string DataType, string valueWire, bool IsInput, string bounds)
        {
            var cfgterm = new XElement("ConfigurableMethodTerminal");
            if (configuredValue != null) cfgterm.Add(new XAttribute("ConfiguredValue", configuredValue));
            cfgterm.Add(AddTerminal(Id, IsInput ? "Input" : "Output", DataType, valueWire, "0.5 1", bounds));
            return cfgterm;
        }

        public static XElement AddTerminal(string Id, string Direction, string DataType, string Wire, string hotspot, string bounds)
        {
            var term1 = new XElement("Terminal").AddAttributes(new Dictionary<string, string>()
            {
                ["Id"] = Id,
                ["Direction"] = Direction
            });
            if (!string.IsNullOrEmpty(Wire)) term1.Add(new XAttribute("Wire", Wire));
            if (DataType != null) term1.Add(new XAttribute("DataType", DataType));
            if (hotspot != null) term1.Add(new XAttribute("Hotspot", hotspot));
            if (bounds != null) term1.Add(new XAttribute("Bounds", bounds));

            return term1;
        }

        public XDocument GetContentDoc()
        {
            var blockdiagram = GenerateBlockDiagram();

            //-- add a commentbox
            //if (addComment)
            //{
            //    var comm = @"<Comment Bounds='0 -200 600 80' SizeMode='User' AttachStyle='Free'><Content></Content></Comment>";
            //    var projtitle = $"Restored EV3 Project from\n{ProgName}\n\nEV3MagicTools (EV3TreeVisualizer and EV3BrickMagic) - http://ev3treevis.azurewebsites.net";
            //    var xcomm = XElement.Parse(comm);
            //    xcomm.Element("Content").SetValue(projtitle);
            //    blockdiagram.Add(xcomm);
            //}

            //----------------------------------------------------
            var fps = @"<FrontPanel><fpruntime:FrontPanelCanvas xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' xmlns:fpruntime='clr-namespace:NationalInstruments.LabVIEW.FrontPanelRuntime;assembly=NationalInstruments.LabVIEW.FrontPanelRuntime' xmlns:Model='clr-namespace:NationalInstruments.SourceModel.Designer;assembly=NationalInstruments.SourceModel' x:Name='FrontPanel' Model:DesignerSurfaceProperties.CanSnapToObjects='True' Model:DesignerSurfaceProperties.SnapToObjects='True' Model:DesignerSurfaceProperties.ShowSnaplines='True' Model:DesignerSurfaceProperties.ShowControlAdorners='True' Width='640' Height='480' /></FrontPanel>";
            var xfps = XElement.Parse(fps);

            var doc = new XDocument();
            doc.Add(
                XExt.CreateXElementWithNS("SourceFile", "http://www.ni.com/SourceModel.xsd",
                        new XAttribute("Version", "1.0.2.10"),
                        new XElement("Namespace",
                            new XAttribute("Name", "Project"),
                            XExt.CreateXElementWithNS("VirtualInstrument", "http://www.ni.com/VirtualInstrument.xsd",
                                xfps,
                                blockdiagram)
                            .AddAttributes(new Dictionary<string, string>()
                            {
                                ["IsTopLevel"] = "false",
                                ["IsReentrant"] = "false",
                                ["Version"] = "1.0.2.0",
                                ["OverridingModelDefinitionType"] = "X3VIDocument",
                            })
                        )
                    )
                );
            return doc;
        }

        public string GetContent()
        {
            var doc = GetContentDoc();
            return
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
            doc.ToString();
        }

        Dictionary<string, string> globalDataWires = new Dictionary<string, string>(); //$dwN1 --> w2
        internal class WireItem
        {
            internal string nodeIdStart;
            internal string portStart;
            internal XElement wireElement; // wireElement added only when src->dst is connected, used for additional dst's
            //internal Node scopeNode; // parent node of start node - for scoping
        }
        Dictionary<string, WireItem> globalDataWiresStart = new Dictionary<string, WireItem>(); // w2 --> n5, valueOut, "<Wire ...>"
        static int globalNodeid = 0;
        static int globalWirecount = 0;
        //static int globalY = 0;
        private XElement GenerateBlockDiagram()
        {
            var blockdiagram = new XElement("BlockDiagram", new XAttribute("Name", "__RootDiagram__"));
            var program = ProgNode;
            bool isMultiStart = false;

            bool AddOptionalStartBlock(Node node1)
            {
                //TODO: cases to handle: 
                // node is a "program1.ev3p" node, but first child is not StartBlock
                // node is a standard/valid block, but first child is not StartBlock
                // node is a multi start block node elem, but first child is not StartBlock (per default)
                if (node1.Children.FirstOrDefault()?.Name != Node.BLOCK_StartBlock)
                {
                    var sb = new Node(Node.BLOCK_StartBlock); sb.Parameters.Add("Result", new Parameter("Result", null, sb)); //isInput = false
                    node1.Children.Insert(0, sb);
                    return true;
                }
                return false;
            }

            // -- add a start block
            if (!AddOptionalStartBlock(program) &&
                program.Children.Count(elem => elem.Name == Node.BLOCK_StartBlock) > 1)
            {
                isMultiStart = true;
            }

            // generate blocks
            int iXPosition = StartX;
            int iYPosition = StartY;
            if (!isMultiStart)
            {
                GenerateBlocks(blockdiagram, program.Children, null, true, ref iXPosition, iYPosition);
            }
            else
            {
                foreach (var node2 in program.Children)
                {
                    AddOptionalStartBlock(node2);
                    GenerateBlocks(blockdiagram, node2.Children, null, true, ref iXPosition, iYPosition);

                    iXPosition += 31;
                }
            }

            return blockdiagram;
        }

        private (SequenceEntity, int) GenerateBlocks(XElement rootelement, List<Node> blocks, SequenceEntity sequenceIn,
            bool forceSequenceOut, ref int iXPosition, int iYPosition)
        {
            SequenceEntity seq = sequenceIn;
            int width = 0;
            foreach (var block in blocks)
            {
                (SequenceEntity seq1, int widthSub) = GenerateBlock(rootelement, block, seq,
                    blocks.Last() != block || forceSequenceOut, ref iXPosition, 0, iYPosition, false);

                seq = seq1;
                width += widthSub;
            }
            return (seq, width);
        }

        private (SequenceEntity, int) GenerateBlock(XElement rootelement, Node block, SequenceEntity seqIn, bool generateSequenceOut,
            ref int iXPosition, int iWidthAdjustment, int iYPosition,
            bool forceSimple)
        {
            List<XElement> xblocks = null;
            var nodeId = GetNewNodeId();
            int width = 0;
            int height = 91;
            int dy = 0;
            int dwidth = 0;

            //-- create block
            if (!forceSimple)
            {
                if (block.NodeType == Node.NodeTypeEnum.Loop)
                {
                    //-- loop node
                    //!! var oldglobalY = globalY; globalY += 41; //TODO: check - shouldnt I use a relative Y?
                    XElement xblock1;
                    (xblock1, width) = GenerateLoopNode(rootelement, block, nodeId, iYPosition);
                    if (xblock1 != null) xblocks = new List<XElement>() { xblock1 };

                    //globalY = oldglobalY;
                    height = 176;
                    dy = -51;
                }
                else if (block.HasSwitch)
                {
                    //var oldglobalY = globalY; globalY += 41;
                    //!! //TODO: switch node SequenceTerminal wire shall be the same as the incoming seqienceIn and the outogoing sequenceout
                    (xblocks, width) = GenerateSwitchNode(rootelement, block, nodeId, ref iXPosition);
                    //globalY = oldglobalY;
                    height = 325;
                    dy = -125;
                    iXPosition += 14;
                }
            }

            //-- normal blocks
            if (xblocks == null)
            {
                XElement block1;
                (block1, width) = GenerateBasicNode(rootelement, block, nodeId);
                if (block1 != null) xblocks = new List<XElement>() { block1 };
                //height = 91; dy = 0;
            }

            //-- empty block -> add comment
            if (xblocks == null)
            {
                XElement xblock1;
                (xblock1, width) = GenerateBasicNode(rootelement, new Node("CommentBlock"), nodeId);

                string text = block.Name + "(" +
                    string.Join(", ", block.Parameters.Select(pa => $"{pa.Key}: {pa.Value.Value}").ToArray()) +
                    ")";
                xblock1.Add(new XAttribute("Comment", text));

                xblocks = new List<XElement>() { xblock1 };
                //return (seqIn, 0); //-- skip if empty block
            }

            //-- generate bounds
            width -= iWidthAdjustment + dwidth;
            xblocks.Last().Attribute("Bounds").SetValue($"{iXPosition} {iYPosition + dy} {width} {height}");
            iXPosition += width;

            //-- handle sequence in add to xml root
            if (seqIn != null)
                seqIn.Finish(xblocks.First(), nodeId);
            xblocks.ForEach(block1 => rootelement.Add(block1));

            //---------------------------------
            //-- create new sequence wire -> going out
            SequenceEntity seqOut = generateSequenceOut ?
                new SequenceEntity(rootelement, xblocks.Last(), nodeId, bounds: $"{width - 18} 33 18 18") :
                null;
            return (seqOut, width);
        }

        /// <summary>
        /// Generate Basic/Standard Node
        /// </summary>
        /// <param name="rootelement"></param>
        /// <param name="gblock"></param>
        /// <param name="nodeId"></param>
        /// <param name="iWidthAdjustment"></param>
        /// <returns></returns>
        private (XElement, int) GenerateBasicNode(XElement rootelement, Node block, string nodeId,
            int basewidth_bounds = 54, int minwidth_bounds = 75)
        {
            string refblock = EV3ModelLib.BlockInfo.MapShortToRef.Contains(block.Name) ? EV3ModelLib.BlockInfo.MapShortToRef[block.Name].First() : null;
            if (refblock == null) return (null, 0);
            BlockInfo bi = BlockInfo.BlockMapByRef[refblock];

            string refblock_target = (refblock + (!refblock.StartsWith("X3") ? ".vix" : ""))
                .Replace(".", @"\.");

            string elem = "ConfigurableMethodCall";
            if (block.HasWait) elem = "ConfigurableWaitFor";
            else if (block.HasSwitch) elem = "PairedConfigurableMethodCall";
            else if (refblock.Contains("StartBlockTest")) elem = Node.BLOCK_StartBlock;

            XElement block1 = new XElement(elem).AddAttributes(
                new Dictionary<string, string>()
                {
                    ["Id"] = nodeId,
                    ["Bounds"] = "0 0 0 0", // to be filled out later
                    ["Target"] = refblock_target
                });

            //Dictionary<string, string> paramTypes = new Dictionary<string, string>();
            //BlockInfo.BlockTranslateMap.TryGetValue(refblock, out BlockInfo refblockDescriptor);

            //-- commentblock is very special, handle it separately
            if (block.Name == "CommentBlock")
            {
                if (block.Parameters.TryGetValue("Comment", out Parameter pCommentText))
                {
                    //TODO: escape prop - only "s
                    string sCommentText = pCommentText.Value;
                    block1.Add(new XAttribute("Comment", sCommentText));
                }
                return (block1, 159);
            }

            //-- params
            var displayedparams = 0;
            int terminal_x = 54;
            foreach (var param in block.Parameters)
            {
                var key = param.Key;
                //!! UnShortenAnEscapeParamName
                key = key.Replace("__", @", "); // Pressed__Released_or_Bumped --> "__" means to be escaped to "\,\ ";
                key = key.Replace("_", @" ");
                key = Utils.EscapeVIX(key); // escape sensitive chars "(),. "

                Parameter valuep = param.Value;
                //var value = valuep.Value;
                var value = valuep.Value;
                string valueWire = null;
                var datatype = valuep.DataType;
                bool isInput = valuep.IsInput;
                bool skipdisplay = false;
                bool forcePortVariableConfigValue = false; //-- wired ports do need the special ConfiguredValue attribute as well
                //TODO: channel out param type! from PatterMatcher

                // get param from bi
                if (bi.Params.TryGetValue(param.Key, out BlockParamInfo bpi))
                {
                    // if it is a port & not wired port --> it is hidden
                    if (bpi.IsVisibilitySpecial) skipdisplay = true;
                    else if (param.Value.Identification.In("OneOutputPort", "TwoOutputPorts", "OneInputPort"))
                    {
                        //if (!param.Value.Value.EndsWith(Node.PARAM_WiredSpecial_Src))
                        if (!string.IsNullOrWhiteSpace(param.Value.Variable))
                            forcePortVariableConfigValue = true;
                        else
                            skipdisplay = true;
                    }
                }

                // number of displayed params (on normal line)
                string terminal_bounds;
                if (skipdisplay)
                {
                    terminal_bounds = "0 0 0 0";
                }
                else
                {
                    displayedparams++;
                    terminal_bounds = $"{terminal_x} 56 30 27";
                    terminal_x += 31;
                }

                //paramTypes[param.Key] = datatype; //TODO //??

                //-- check variables
                if (valuep.Variable != null)
                {
                    globalDataWires.TryGetValue(valuep.Variable, out valueWire); // main vmthread local variable -> wireid e.g. LOCAL4 --> w15
                    if (valueWire == null)
                    {
                        valueWire = GetNewWireId();
                        globalDataWires.Add(valuep.Variable, valueWire);
                        globalDataWiresStart.Add(valueWire, new WireItem() { nodeIdStart = nodeId, /*scopeNode = block.Parent,*/ portStart = key, wireElement = null });
                    }
                    else
                    {
                        var nsource = globalDataWiresStart[valueWire];

                        //// if scope is not the same - tunnelling is needed out->to in   //TODO: handle from inside->to outside 
                        //if (nsource.scopeNode != block.Parent)
                        //{
                        //    //<ConfigurableWhileLoop.ConfigurableLoopTunnel AutoIndex="False" Id="b7" Terminals="n2=w7, d0=w7" Bounds="117 143 30 63" />
                        //    new XElement("ConfigurableWhileLoop.ConfigurableLoopTunnel").AddAttributes(new Dictionary<string, string>()
                        //    {
                        //        ["AutoIndex"] = "False",
                        //        ["Id"] = GetNewNodeId("b"),
                        //        ["Terminals"] = GetCurrentNodeId("b"),
                        //        //["Bounds"] = "0 0 0 0", // to be filled out later
                        //    });
                        //}

                        // if wireElement exists, this means we are adding additional destinations
                        //<Wire Id="w17" Joints="N(n12:Value) v(22) h(101) N(n13:X)" /> or N(n12:Value) v(22) h(101) N(n13:X) B(0) N(n14:Y)
                        if (nsource.wireElement == null)
                        {
                            nsource.wireElement = AddWire(
                                nsource.nodeIdStart, nsource.portStart,
                                nodeId, key,
                                valueWire);
                            rootelement.Add(nsource.wireElement);
                        }
                        else
                        {
                            // wire exists: (this means already added, for A->B, but there are multiple targets (A->B,C) to be connected)
                            var val1 = nsource.wireElement.Attribute("Joints").Value;
                            val1 += $" B(0) N({nodeId}:{key})";
                            nsource.wireElement.Attribute("Joints").Value = val1;
                        }
                    }
                    value = !forcePortVariableConfigValue ? null : Node.PARAM_WiredSpecial_Src;
                }

                //TODO: channel out param type! from PatterMatcher
                //!! should detect from datatype, rather than value
                //!!if (Regex.IsMatch(value, @"\[\d+(,\s?\d+)*\]")) datatype = "Single[]";

                //!!if (value == "True" || value == "False") datatype = "Boolean"; //!! TODO // TODO: currently this is 1 or 0
                //!!! CompareType
                //Terminal Id="Direction" Direction="Input" DataType="Int32" --> ParamType: "SHORT"

                //-- add terminal xnode
                block1.Add(AddConfigurableMethodTerminal(key, isInput ? value : null, datatype, valueWire, isInput, terminal_bounds));
            }

            //-- any not yet covered parameters (typically dummy output params)
            if (bi.Params.Count > block.Parameters.Count)
            {
                for (int idx = block.Parameters.Count; idx < bi.Params.Count; idx++)
                {
                    string bpiKey = bi.Params.Keys.Skip(idx).First();
                    var bpi = bi.Params[bpiKey];

                    var bpiKey1 = Utils.UnShortenAnEscapeParamName(bpiKey);
                    block1.Add(AddConfigurableMethodTerminal(bpiKey1, null, bpi.DataType, null, bpi.CallDirectionInput, "0 0 0 0"));
                    // displayedparams not changing
                }
            }

            //-- bounds calculation
            // width calculation
            //      start block width = 70, width = 70 + #params*31, in case of 0 -> +5 (75)
            //      comment = 159, loopindex = 47, loopend = 54 + 31*#param -- basewidth=54, minwidth=75
            int width =
                elem == Node.BLOCK_StartBlock ? 70 :
                        //block.Name == "CommentBlock" ? 159 :
                        Math.Max(75, 70 + displayedparams * 31);
            //Console.WriteLine($"{gblock.Name}\t{width} \t\t displayedparams:{displayedparams}");

            return (block1, width);
        }

        /// <summary>
        /// Generate Loop node
        /// </summary>
        /// <param name="gblock"></param>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        private (XElement, int) GenerateLoopNode(XElement rootelement, Node block, string nodeId, int iYPosition)
        {
            int width = 0;
            int iXPosition = 0;
            //int iYPositionSub = 33;

            XElement block1 = new XElement("ConfigurableWhileLoop").AddAttributes(
                new Dictionary<string, string>()
                {
                    ["Id"] = nodeId,
                    ["Bounds"] = "0 0 0 0", // to be filled out later
                    ["DiagramId"] = GetCurrentNodeId("d"),
                    ["InterruptName"] = block.Parameters.ContainsKey(Node.PARAM_LOOP_InterruptName) ? block.Parameters[Node.PARAM_LOOP_InterruptName].Value : "01"
                });
            SequenceEntity seqSub = null;

            //-- loop index
            var xloopindex = new XElement("ConfigurableWhileLoop.BuiltInMethod", new XAttribute("CallType", "LoopIndex")); block1.Add(xloopindex);
            int widthSub;
            //TODO: handle any connections if available from nodes
            (seqSub, widthSub) = GenerateBlock(xloopindex, new Node("LoopIndex"), seqSub, true, ref iXPosition, 75 - 47, 41, true);
            //Loop\ Index terminal --> 0 56 30 27
            seqSub.blockForWire = block1; // add sequence wires to the loop instead of the direct builtinmethod

            List<Node> subblocks = new List<Node>(block.Children);

            //-- generate blocks within the loop
            (seqSub, widthSub) = GenerateBlocks(block1, subblocks, seqSub, true, ref iXPosition, 41);
            //if (widthSub == 0) { widthSub = 109 - 47; iXPosition += widthSub; }
            width += widthSub;

            //-- generate stop condition
            var xloopend = new XElement("ConfigurableWhileLoop.BuiltInMethod", new XAttribute("CallType", "StopCondition"));
            block1.Add(xloopend);

            //TODO: handle any scopes for terminals
            (_, widthSub) = GenerateBlock(xloopend, block, seqSub, false, ref iXPosition, 101 - 85, 41, true);

            // for some reason loops are 166 wider than calculated
            width += 166;

            return (block1, width);
        }

        private (List<XElement>, int) GenerateSwitchNode(XElement rootelement, Node block, string nodeId, ref int iXPositionMain)
        {
            //return (null, 0);

            List<XElement> retelems = new List<XElement>();
            int width = 0;
            int localY = 40;

            string refblock = EV3ModelLib.BlockInfo.MapShortToRef.Contains(block.Name) ? EV3ModelLib.BlockInfo.MapShortToRef[block.Name].First() : null;
            BlockInfo bi = BlockInfo.BlockMapByRef[refblock];

            //---------------------------------
            //-- create PairedConfigurableMethodCall
            (XElement pairednode1, int widthPairedNode) = GenerateBasicNode(rootelement, block, nodeId, 54, 73);
            retelems.Add(pairednode1);
            var pResult = bi.Params.First(elem => elem.Value.IsResult);
            string datatype = pResult.Value.DataType; // paramTypes["Result"]; //-- ConfigurableMethodTerminal/Terminal/DataType
                                                      //width += widthPairedNode;
            iXPositionMain += widthPairedNode;

            //-- determine flat or tabbed mode
            bool bIsFlatMode = true;
            string strcutureName = bIsFlatMode ? "ConfigurableFlatCaseStructure" : "ConfigurableCaseStructure";

            //---------------------------------
            //-- create ConfigurableFlatCaseStructure main node
            var nodeId2 = GetNewNodeId();
            XElement blockcasestructure = new XElement(strcutureName).AddAttributes(
                new Dictionary<string, string>()
                {
                    ["Id"] = nodeId2,
                    ["Bounds"] = "0 0 0 0", // to be filled out later
                    ["DataType"] = datatype,
                    ["UserSelectorBounds"] = "0 0 0 0",
                    ["Default"] = "", // to be filled out later
                    ["PairedConfigurableMethodCall"] = nodeId,
                });
            retelems.Add(blockcasestructure);

            // tie pairednode as well
            pairednode1.Add(new XAttribute("PairedStructure", nodeId2));

            //---------------------------------
            //-- create cases nodes -- ConfigurableFlatCaseStructure.Case
            //ConfigurableFlatCaseStructure.Case Id="D5" Bounds="10 4 203 164"
            //TODO: no idea which value is default -- maybe guess it -- bool:notother, int:999/anynotused, string:???, color:/???
            int subwidthmax = 0;
            {
                //-- generate CASE sub blocks
                string defaultPattern = block.Parameters.ContainsKey("Default") ? block.Parameters["Default"].Value : null;
                foreach (var gblockcase in block.Children)
                {
                    var pattern = gblockcase.Parameters["Pattern"].Value;

                    // add default attribute of default node - either node has it or first is assumed to be the default (RBF)
                    if (defaultPattern == null) defaultPattern = pattern;
                    var nodeIdCase = GetNewNodeId(pattern == defaultPattern ? "D" : "d");
                    if (pattern == defaultPattern) blockcasestructure.Attribute("Default").Value = nodeIdCase;

                    XElement blockcase1 = new XElement(strcutureName + ".Case").AddAttributes(
                        new Dictionary<string, string>()
                        {
                            ["Id"] = nodeIdCase,
                            ["Pattern"] = pattern,
                        });
                    blockcasestructure.Add(blockcase1);

                    // generate seqquenceterminal blocks
                    XElement seqnode_out = new XElement("SequenceNode").AddAttributes(new Dictionary<string, string>() { ["Id"] = "Output" });
                    XElement seqnode_in = new XElement("SequenceNode").AddAttributes(new Dictionary<string, string>() { ["Id"] = "Input" });
                    blockcase1.Add(seqnode_out);
                    blockcase1.Add(seqnode_in);
                    seqnode_out.Add(new XAttribute("Bounds", $"0 75 18 18"));

                    // iterate on sub blocks
                    var subblocks = gblockcase;
                    SequenceEntity seqSub = new SequenceEntity(blockcase1, seqnode_out, "Output", "SequenceIn", "SequenceTerminal", bounds: "0 0 18 18");
                    //seqnode_out.Elements().First().Add(new XAttribute("Bounds", "0 0 18 18"));

                    if (subblocks.Children.Count > 0)
                    {
                        int widthSub;
                        int iXPosition = 18;
                        (seqSub, widthSub) = GenerateBlocks(blockcase1, subblocks.Children, seqSub, true, ref iXPosition, 40);
                        if (widthSub > subwidthmax) subwidthmax = widthSub;
                    }
                    seqSub.sequenceInName = "SequenceTerminal";
                    seqSub.Finish(seqnode_in, "Input");
                    //seqnode_in.Add(new XAttribute("Bounds", $"{subwidthmax} 73 18 18"));
                }
            }

            width += subwidthmax;
            return (retelems, width);
            //----------------

            /*
            //-- loop index
            var xloopindex = new XElement("ConfigurableWhileLoop.BuiltInMethod", new XAttribute("CallType", "LoopIndex")); block1.Add(xloopindex);
            int widthSub;
            (seqSub, widthSub) = GenerateBlock(xloopindex, new EV3GBlock(null, "LoopIndex"), seqSub, true, ref iXPosition, 75 - 47);
            seqSub.root = block1; // add sequence wires to the loop instead of the direct builtinmethod

            List<EV3GBlock> subblocks = new List<EV3GBlock>(gblock);
            EV3GBlock gloopend = null;
            if (gblock.Last().IsLoopEndNode)
            {
                gloopend = subblocks.Last();
                subblocks.Remove(gloopend);
            }

            //-- generate blocks within the loop
            (seqSub, widthSub) = GenerateBlocks(block1, subblocks, seqSub, true, ref iXPosition);
            width += widthSub;

            //-- generate stop condition
            var xloopend = new XElement("ConfigurableWhileLoop.BuiltInMethod", new XAttribute("CallType", "StopCondition"));
            block1.Add(xloopend);

            if (gloopend.NameRef == "Wait") gloopend.NameRef = "LoopCondition.Time"; //TimeCompare -> TimeCompareLoop //!! //TODO: detect the two block differently
            (_, widthSub) = GenerateBlock(xloopend, gloopend, seqSub, false, ref iXPosition, 101 - 85);

            // for some reason loops are 166 wider than calculated
            width += 166;

            return (block1, width);
            */
        }

        internal class SequenceEntity
        {
            public string wireid;
            public string node1;
            public string node2;
            public XElement blockForWire;
            // most of the time this is the name, in special cases other name is used (e.g. loop)
            internal string sequenceInName = "SequenceIn";
            internal string sequenceOutName = "SequenceOut";

            public void AddSeqOut(XElement blockForTerminal, string bounds)
            {
                XElement term;
                blockForTerminal.Add(term = AddTerminal(sequenceOutName, "Output", "NationalInstruments:SourceModel:DataTypes:X3SequenceWireDataType", wireid, "1 0.5", bounds));
            }
            public void AddSeqIn(XElement blockForTerminal)
            {
                XElement term;
                string bounds = "0 33 18 18"; //CHECK: sometimes (flatcase) this can be 0 0 18 18
                blockForTerminal.Add(term = AddTerminal(sequenceInName, "Input", "NationalInstruments:SourceModel:DataTypes:X3SequenceWireDataType", wireid, "0 0.5", bounds));
            }
            public void AddWireBlock()
            {
                blockForWire.Add(AddWire(node1, sequenceOutName, node2, sequenceInName, wireid));
            }

            internal SequenceEntity(XElement _blockForWire, XElement blockForTerminal, string nodeId,
                string _sequenceInName = "SequenceIn", string _sequenceOutName = "SequenceOut",
                string bounds = null)
            {
                node1 = nodeId;
                node2 = null;
                wireid = GetNewWireId();
                blockForWire = _blockForWire;
                sequenceInName = _sequenceInName;
                sequenceOutName = _sequenceOutName;

                this.AddSeqOut(blockForTerminal, bounds);
            }

            internal void Finish(XElement block1, string nodeId)
            {
                this.node2 = nodeId;
                this.AddSeqIn(block1);
                this.AddWireBlock();
            }

        }

        private static string GetNewWireId()
        {
            return "w" + (++globalWirecount);
        }

        private string GetNewNodeId(string prefix = "n")
        {
            globalNodeid++;
            return GetCurrentNodeId(prefix);
        }
        private string GetCurrentNodeId(string prefix = "n")
        {
            return prefix + globalNodeid.ToString();
        }

        private static XElement AddWire(string src_node, string src_port, string dst_node, string dst_port, string wire)
        {
            return new XElement("Wire").AddAttributes(
                                    new Dictionary<string, string>()
                                    {
                                        ["Id"] = wire,
                                        ["Joints"] = $"N({src_node}:{src_port}) N({dst_node}:{dst_port})",
                                    });
        }

    }
}
