using EV3DecompilerLib.Recognize;
using EV3ModelLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EV3DecompilerLib.Recognize
{
    internal class EV3GBlock2ModelConverter : INodeConverter
    {
        static EV3GBlock2ModelConverter()
        {
            RegisterConverter();
        }

        public static void RegisterConverter()
        {
            NodeConversion.RegisterConverter(typeof(EV3GBlock2ModelConverter));
        }

        public Node Convert(object ev3block)
        {
            if (!(ev3block is EV3GBlock)) return null;

            return ConvertBlock(ev3block as EV3GBlock, null);
        }
        private Parameter ConvertEV3GParam(string name, string value, Node parent)
        {
            var retval = new Parameter(name, value, parent)
            {
                //!!! ValueRaw =  
                //Source  = 
                //DataType = ,
                //DataTypeContext = 
            };

            return retval;
        }

        /// <summary>
        /// Iterate one EV3GBlock and convert to Node
        /// </summary>
        /// <param name="block1"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private Node ConvertBlock(EV3GBlock block1, Node parent)
        {
            Node node1 = new Node()
            {
                Name = block1.NameRef,
                Parent = parent,
                SourceObj = block1,
            };

            if (block1.NodeType == EV3GBlock.GNodeType.Loop)
            {
                node1.NodeType = Node.NodeTypeEnum.Loop;
                if (node1.Name == "Wait.Timer") node1.Name = "LoopCondition.Time"; //TimeCompare -> TimeCompareLoop //TODO: detect the two block differently
				if (block1.Root.Parameters.ContainsKey("InterruptName"))
				{
					var pr = block1.Root.Parameters["InterruptName"];
					node1.Parameters.Add("InterruptName", new Parameter("InterruptName", pr, node1));
				}
				else if (block1.Root.Parameters.ContainsKey("InterruptId"))
				{
					var pr = block1.Root.Parameters["InterruptId"];
					node1.Parameters.Add("InterruptName", new Parameter("InterruptName", pr, node1));
				}
			}
            else if (block1.NodeType == EV3GBlock.GNodeType.TopLevel) node1.NodeType = Node.NodeTypeEnum.Program;
            else if (block1.NodeType == EV3GBlock.GNodeType.MyBlockNode) node1.NodeType = Node.NodeTypeEnum.MyBlock;
            else if (block1.HasSwitch) node1.HasSwitch = true;
            else if (block1.HasWait) node1.HasWait = true;
            //else if (block1.is) node1.NodeType = Node.NodeTypeEnum.;
            if (block1.NodeType.HasFlag(EV3GBlock.GNodeType.Fork)) node1.NodeType = Node.NodeTypeEnum.ForkParent;
            if (block1.NodeType.HasFlag(EV3GBlock.GNodeType.ForkItem)) node1.NodeType = Node.NodeTypeEnum.ForkItem;

            string refblock = block1.GetReference();
            node1.RefName = refblock;
            //if (refblock == null) return (null, 0, null);

            //TODO: loopid vs InterruptName for loops

            if (block1.Parameters.Count > 0)
            {
                BlockInfo refblockDescriptor = null;
                if (refblock != null) BlockInfo.BlockMapByRef.TryGetValue(refblock, out refblockDescriptor);

                //TODO iterate on blockinfo parameters

                var paramsTemp = new List<KeyValuePair<int, Parameter>>();
                foreach (var param1 in block1.Parameters)
                {
                    string value = param1.Value;
                    string key_psort = param1.Key;
                    string dataType = null;
                    int callindex = 0;

                    //-- change and escape key
                    if (key_psort == "@result" || key_psort == "@Result") key_psort = "Result";
                    if (key_psort.StartsWith("@Result.", StringComparison.CurrentCultureIgnoreCase)) key_psort = key_psort.Substring("@Result.".Length);

                    var keypunescaped = key_psort.Replace("__", ", ").Replace("_", " ");  // Pressed__Released_or_Bumped --> "__" means to be escaped to "\,\ ";

                    //-- create converted param
                    Parameter param2 = ConvertEV3GParam(key_psort, param1.Value, node1);

                    //-- RefBlock from BlockItems (vix) parameter types
                    if (refblockDescriptor != null && refblockDescriptor.Params != null)
                    {
                        if (refblockDescriptor.Params.TryGetValue(key_psort, out BlockParamInfo paramDescriptor))
                        {
                            param2.DataType = dataType = paramDescriptor.DataType;
                            param2.IsInput = paramDescriptor.CallDirectionInput;
                            //int.TryParse(paramDescriptor.CallIndex, out callindex);
                            callindex = paramDescriptor.CallIndex;
                        }
                    }
                    else
                    {
                        if (parent != null && parent.HasSwitch && key_psort == "Pattern")
                        {
                            param2.DataType = dataType = parent.Parameters.Select(elem => elem.Value).FirstOrDefault(elem => !elem.IsInput)?.DataType;
                            // pattern shall go first
                            callindex = -1;
                        }

                        if (parent == null && node1.NodeType == Node.NodeTypeEnum.MyBlock)
                        {
                            param2.DataType = block1.ParameterTypes[key_psort].GetOutboundType();
                            param2.IsInput = block1.ParameterTypes[key_psort].IsInput;
                        }
                    }

                    //-- check if variable
                    if (Regex.IsMatch(value, @"^LOCAL\d+$"))
                    {
                        param2.Variable = value;
                        param2.Value = value;
                    }

                    //-- MyBlock inpupt parameters are always variables
                    if (parent == null && node1.NodeType == Node.NodeTypeEnum.MyBlock)
                    {
                        param2.Variable = value;
                    }

                    //-- convert value
                    if (param2.Variable == null)
                    {
                        switch (dataType)
                        {
                            case "Boolean":
                                {
                                    param2.Value = (value != "0" && value != "False").ToString();
                                }
                                break;
                        }
                    }

                    //-- add new param to list
                    //node1.Parameters[key] = param2;
                    if (!param2.IsInput) callindex += 1000;
                    paramsTemp.Add(new KeyValuePair<int, Parameter>(callindex, param2));
                }

                //-- reorder params
                paramsTemp
                        //.OrderBy(pt => pt.Key)
                        .OrderBy(pt => !pt.Value.IsInput)
                        .ThenBy(pt => pt.Key) // callindex
                        .ToList()
                        .ForEach(elem => node1.Parameters.Add(elem.Value.Name, elem.Value));
            }

            //-- CHILD blocks convert child blocks
            if (block1.Count > 0)
            {
                block1.ForEach(blocksub =>
                {
                    var nodesub = ConvertBlock(blocksub, node1);
                    if (block1.HasSwitch) nodesub.NodeType = Node.NodeTypeEnum.CaseItem;
                    node1.Children.Add(nodesub);
                });
            }

            //-- handle case nodes
            if (block1.HasSwitch)
            {
                //-- SWITCH remember which ndoe wwas default
                var defaultCaseGBlock = block1.FirstOrDefault(elem => elem.Parameters["Pattern"] == Pattern.CASE_DEFAULT_Value);

                //-- DataType is equal to first output parameter (usually Result)
                var parout = node1.Parameters.FirstOrDefault(elem => !elem.Value.IsInput);
                //if (parout.Key != "Result") Console.WriteLine($"PROBLEM2 Result - {parout.Key}");
                string datatype = parout.Value?.DataType;

                List<string> caseValues = node1.Children?.Select(elem => elem.Parameters["Pattern"].Value).ToList();

                //-- search for a proper value for the default case as this is eliminated during the compilation process
                Node defaultCaseNode = null;
                string defaultCaseValue = null;
                switch (datatype)
                {
                    case "Boolean":
                        {
                            Dictionary<Node, bool> caseValuesSub = node1.Children?.ToList().ToDictionary(
                                elem => elem,
                                elem =>
                                {
                                    var pattern = elem.Parameters["Pattern"].Value;
                                    bool.TryParse(pattern, out bool boolval);

                                    if (elem.SourceObj == defaultCaseGBlock) defaultCaseNode = elem;
                                    return boolval;
                                }
                            );
                            bool aValue = !caseValuesSub.FirstOrDefault(elem => elem.Key != defaultCaseNode).Value;
                            defaultCaseValue = aValue.ToString();
                        }
                        break;

                    case "Int32":
                        {
                            Dictionary<Node, int> caseValuesSub = node1.Children?.ToList().ToDictionary(
                                elem => elem,
                                elem =>
                                {
                                    var pattern = elem.Parameters["Pattern"].Value;
                                    int.TryParse(pattern, out int numval);

                                    if (elem.SourceObj == defaultCaseGBlock) defaultCaseNode = elem;
                                    return numval;
                                }
                            );
                            int aValue = 999; while (caseValuesSub.ContainsValue(aValue)) aValue++;
                            defaultCaseValue = aValue.ToString();
                        }
                        break;

                    case "Single":
                        {
                            Dictionary<Node, double> caseValuesSub = node1.Children?.ToList().ToDictionary(
                                elem => elem,
                                elem =>
                                {
                                    var pattern = elem.Parameters["Pattern"].Value;
                                    double.TryParse(pattern, out double numval);

                                    if (elem.SourceObj == defaultCaseGBlock) defaultCaseNode = elem;
                                    return numval;
                                }
                            );
                            int aValue = 999; while (caseValuesSub.ContainsValue(aValue)) aValue++;
                            defaultCaseValue = aValue.ToString();
                        }
                        break;

                    case "String":
                        {
                            defaultCaseNode = node1.Children?.FirstOrDefault(elem => elem.SourceObj == defaultCaseGBlock);
                            defaultCaseValue = defaultCaseNode?.Parameters["Pattern"].Value;
                        }
                        break;
                }

                //-- set switch.default, set case.pattern
                if (defaultCaseNode != null)
                {
                    defaultCaseNode.Parameters["Pattern"].Value = defaultCaseValue;
                    node1.Parameters["Default"] = new Parameter("Default", defaultCaseValue, node1);
                }
            }

            return node1;
        }
    }
}
