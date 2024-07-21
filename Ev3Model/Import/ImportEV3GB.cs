using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EV3ModelLib.Import
{
    /// <summary>
    /// Class to import EV3GB files - ev3g basic simple text file
    /// </summary>
    public class ImportEV3GB : IProjectConverter
    {
        /// <summary>
        /// Create nodes from lines
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static ProjectResultData CreateNodesFromText(string[] lines, string filename = null)
        {
            Dictionary<string, Node> retDict = new Dictionary<string, Node>();

            int index = 0;
            do
            {
                Node retNode = CreateNodesFromText(lines, ref index, filename);
                if (retNode == null) break;

                string sProgramName = retNode.Name;

                //-- ensure unique names
                if (retDict.ContainsKey(sProgramName))
                {
                    var tmpName = sProgramName; int idx = 1;
                    while (retDict.ContainsKey(tmpName)) tmpName = $"{sProgramName}{+idx++}";
                    retNode.Name = sProgramName = tmpName;
                }

                retDict[sProgramName] = retNode;
            } while (true);

            //-- project creation
            Project project = new Project(Path.GetFileName(filename));
            project.CreateProject(retDict.Values.ToList());

            return (retDict, project);
        }

        /// <summary>
        /// Create Nodes or a program fro text lines
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="index">index is used as an offset to handle multiple programs/myblocks</param>
        /// <returns></returns>
        static Node CreateNodesFromText(string[] lines, ref int index, string filename = null)
        {
            if (index >= lines.Length) return null;

            Node retnode = null;
            string sProgramName = null;

            // need to create dummy node if first "useful" block is not the main function name (next node is indented) or is StartBlock
            if (lines[index].Trim() == Node.BLOCK_StartBlock)
            {
                sProgramName = "Program";
                retnode = new Node(sProgramName);
            }
            else
            {
                sProgramName = lines[index].Trim();
            }

            Node nodeParent = retnode;
            Node node1 = null;
            int indentLast = -1;
            Stack<int> indentLevels = new Stack<int>();
            for (; index < lines.Length; index++)
            {
                string line = lines[index];
                if (string.IsNullOrWhiteSpace(line)) continue;
                int indentCurrent = line.TakeWhile(Char.IsWhiteSpace).Count();

                //-- if line contains only "=", more than four, then this is a new program
                //-- can exit as well, when same indentation returns as root node...
                if (Regex.IsMatch(line, @"^\s*={4,}$")) { index++; return retnode; }
                if (indentLevels.Any() && indentCurrent == indentLevels.Last()) { return retnode; }

                if (indentCurrent > indentLast)
                {
                    // this will be a child node, if not first line
                    if (indentLast >= 0) nodeParent = node1;
                    indentLevels.Push(indentCurrent);
                }
                else if (indentCurrent < indentLast)
                {
                    while (indentCurrent < indentLast)
                    {
                        if (nodeParent != null && nodeParent.Parent != null) nodeParent = nodeParent.Parent;

                        //TODO: error handling if stack is empty
                        indentLevels.Pop();
                        indentLast = indentLevels.Peek();
                    }
                }
                indentLast = indentCurrent;

                var nodeTemp = NodeFromText(line.TrimStart(), nodeParent);
                if (nodeTemp == null) continue;

                node1 = nodeTemp;
                if (nodeParent != null) nodeParent.Children.Add(node1);
                if (retnode == null) retnode = node1;
            }

            return retnode;
        }

        /// <summary>
        /// Create a single node from a line
        /// </summary>
        /// <param name="source"></param>
        /// <param name="nodeParent"></param>
        /// <returns></returns>
        static Node NodeFromText(string source, Node nodeParent)
        {
            bool isTopLevel = nodeParent == null;
            if (isTopLevel && source.StartsWith(Node.CONST_SUB_LITERAL)) source = source.Substring(Node.CONST_SUB_LITERAL.Length).TrimStart();

            int spcidx = source.IndexOfAny(new[] { ' ', '(' });
            var blockname = (spcidx < 0 ? source : source.Substring(0, spcidx)).TrimEnd();
            string paramsStr = spcidx >= 0 ? source.Substring(spcidx) : null;
            string paramsControlStr = null;

            Node node1 = new Node(blockname);
            node1.Parent = nodeParent;

            if (source == Node.BLOCK_Empty)
            {
                node1.NodeType = Node.NodeTypeEnum.Empty;
                node1.Name = Node.BLOCK_Empty;
                return node1;
            }
            else if (blockname == Node.BLOCK_LoopPrefix1)
            {
                node1.NodeType = Node.NodeTypeEnum.Loop;
                var match = new Regex(@"\((?<blockname>[\w.]+)(?<stdparams>.*)\)(?<controlparams>.*)$").Match(source.Substring(blockname.Length));
                node1.Name = blockname = match.Groups["blockname"].Value;
                paramsStr = match.Groups["stdparams"].Value;
                paramsControlStr = match.Groups["controlparams"].Value;
                //// if last param is "left" use it as loopname + remove
                //parvals = parvals.Take(parvals.Length - 1).ToArray();
                //TODO: somehow remember this value
            }
            else if (blockname.StartsWith(Node.BLOCK_WaitForPrefix1))
            {
                node1.HasWait = true;
                var match = new Regex(@"\((?<blockname>[\w.]+)(?<stdparams>.*)\)$").Match(source.Substring(blockname.Length));
                node1.Name = blockname = match.Groups["blockname"].Value;
                paramsStr = match.Groups["stdparams"].Value;
            }
            else if (blockname.StartsWith(Node.BLOCK_SwitchPrefix1))
            {
                node1.HasSwitch = true;
                var match = new Regex(@"\((?<blockname>[\w.]+)(?<stdparams>.*)\)$").Match(source.Substring(blockname.Length));
                node1.Name = blockname = match.Groups["blockname"].Value;
                paramsStr = match.Groups["stdparams"].Value;
            }

            //=== process parameter list
            var parvals = ExtractParamsToList(paramsStr);

            if (isTopLevel)
            {
                node1.NodeType = !parvals.Any() ? Node.NodeTypeEnum.Program : Node.NodeTypeEnum.MyBlock;
                //TODO: handle zero params myblocks as well --> todo how?
            }

            //-- standard LEGO EV3G blocks: use block identification if block is known
            if (!isTopLevel && BlockInfo.MapShortToRef.Contains(blockname))
            {
                string refname = BlockInfo.MapShortToRef[blockname].First();
                node1.RefName = refname;
                var bi = BlockInfo.BlockMapByRef[refname];

                // set parameters
                if (bi.Params != null)
                {
                    var paridx = 0;
                    int biparmax = bi.Params.Count;
                    foreach (var elem in
                        bi.Params
                        .OrderByDescending(pi => pi.Value.CallDirectionInput)
                        .ThenBy(pi => pi.Value.CallIndex))
                    {
                        var key = elem.Key;
                        var par1 = bi.Params[key];
                        var parv = paridx < parvals.Count ? parvals[paridx].Param : string.Empty;
                        //var isInput = elem.Value.CallDirectionInput;

                        // skip params not sed with wait & loop -- same as in LogBlock2ModelConverter
                        if ((node1.HasSwitch || node1.NodeType.In(Node.NodeTypeEnum.CaseItem, Node.NodeTypeEnum.Loop)) && !elem.Value.CallDirectionInput) continue;
                        if (node1.HasWait && !elem.Value.CallDirectionInput && elem.Value.IsResult) continue;
                        paridx++;

                        // check if variable
                        string sVariable = CheckIfVariable(parv, out _);

                        if (sVariable == null)
                        {
                            bool isArrayType = elem.Value.DataType.EndsWith("[]");
                            // if Array --> skip/find all elements until "]" is found
                            if (isArrayType)
                            {
                                //if (parv.Trim().StartsWith("[")) // && !parv.Trim().StartsWith("$")
                                //    while (paridx < parvals.Count) { parv += "," + parvals[paridx++].TrimEnd(); if (parv.EndsWith("]")) break; }
                                // convert according to identification
                                if (par1.Identification != null)
                                {
                                    var idvalues = IdentificationHelper.IdentificationValues[par1.Identification];
                                    var parvitems = parv
                                        .TrimStart('[').TrimEnd(']')
                                        .Split(',')
                                        .Select(elem2 => elem2.Trim())
                                        .Select(elem3 => idvalues.FirstOrDefault(ide => ide.Value == elem3).Key ?? elem3)
                                        .ToArray();
                                    parv = "[" + string.Join(",", parvitems) + "]";
                                }
                            }

                            // decode identifiction if found
                            if (elem.Value.Identification != null)
                            {
                                var ida = IdentificationHelper.IdentificationValues[elem.Value.Identification];
                                var parv1 = ida.FirstOrDefault(elem2 => elem2.Value == parv.Trim()).Key;
                                if (parv1 != null) parv = parv1;
                            }
                        }

                        if (parv != null || (parv != null && elem.Value.DataType == "String"))
                        {
                            var param2 = new Parameter(key, parv, node1);
                            param2.IsInput = elem.Value.CallDirectionInput;
                            param2.DataType = elem.Value.DataType;
                            if (sVariable != null) { param2.Variable = sVariable; }
                            node1.Parameters.Add(key, param2);
                        }
                    }
                }

                //-- add interruptname if available for loop
                if (node1.NodeType == Node.NodeTypeEnum.Loop)
                {
                    var parvalscontrol = ExtractParamsToList(paramsControlStr);
                    if (parvalscontrol?.Count > 0)
                        node1.Parameters[Node.PARAM_LOOP_InterruptName] =
                            new Parameter(Node.PARAM_LOOP_InterruptName, parvalscontrol[0].Param, node1) { DataType = "String" };
                    if (parvalscontrol?.Count > 1)
                    {
                        string sVariable = CheckIfVariable(parvalscontrol[1].Param, out _);
                        if (sVariable != null)
                            node1.Parameters[Node.PARAM_LOOP_LoopIndex] =
                                new Parameter(Node.PARAM_LOOP_LoopIndex, null, node1) { Variable = sVariable, IsInput = false };
                    }
                }
            }

            //-- additional, non-standard blocks
            else
            {
                bool isNodeProcessed = false;
                if (!isTopLevel)
                {
                    switch (blockname)
                    {
                        case Node.BLOCK_Fork:
                            node1.NodeType = Node.NodeTypeEnum.ForkParent;
                            isNodeProcessed = true;
                            break;
                        case Node.BLOCK_ForkItem:
                            node1.NodeType = Node.NodeTypeEnum.ForkItem;
                            isNodeProcessed = true;
                            break;
                        case Node.BLOCK_SwitchCaseItem:
                            node1.NodeType = Node.NodeTypeEnum.CaseItem;
                            if (parvals.Any())
                            {
                                var parv = parvals[0].Param.Trim();
                                Parameter paramci = new Parameter(Node.PARAM_SWITCH_Pattern, parv, node1); // identification is added inside
                                node1.Parameters[Node.PARAM_SWITCH_Pattern] = paramci;

                                // decode identifiction if found
                                if (paramci.Identification != null)
                                {
                                    var ida = IdentificationHelper.IdentificationValues[paramci.Identification];
                                    var parv1 = ida.FirstOrDefault(elem2 => elem2.Value == parv.Trim()).Key;
                                    if (parv1 != null)
                                        paramci.Value = parv1;
                                }
                            };
                            isNodeProcessed = true;
                            break;
                        case Node.BLOCK_Empty:
                            node1.NodeType = Node.NodeTypeEnum.Empty;
                            isNodeProcessed = true;
                            break;
                        default:
                            node1.NodeType = Node.NodeTypeEnum.MyBlock; //TODO: check if it really a myblock or a custom vix // HOW?
                            isNodeProcessed = false;
                            break;
                    }
                }

                if (!isNodeProcessed && parvals != null)
                {
                    // add parameters
                    if (!isTopLevel)
                    {
                        for (int idx = 0; idx < parvals.Count; idx++)
                        {
                            string parval = parvals[idx].Param;
                            string sVariable = CheckIfVariable(parval, out bool isInput);

                            string parname = $"Param{idx + 1}";
                            node1.Parameters.Add(parname, new Parameter(parname, parval, node1)
                            {
                                DataType = parvals[idx].IsString ? "String" : null,
                                Variable = sVariable,
                                IsInput = isInput
                            });
                        }
                    }
                    else
                    {
                        // top level (myblock) can have different parameter
                        //Sub MyBlock1 param1steering:Numeric:0:=$steering, param2port:Numeric:23:=$param2port, param3:Text:hello:=$param3

                        //if (node1.Parent.Children.Any()) 
                        // make everythink myblock for now...
                        node1.NodeType = Node.NodeTypeEnum.MyBlock;

                        for (int idx = 0; idx < parvals.Count; idx++)
                        {
                            string parval = parvals[idx].Param;
                            string parname = null;
                            string partype = null;
                            string pardefault = null;
                            var namevaluepair = parval.Split(new[] { ":=" }, StringSplitOptions.None);
                            if (namevaluepair.Length >= 1)
                            {
                                parname = namevaluepair[0].Trim();
                                var vals = parname.Split(':'); //TODO: handle escaping
                                parname = vals[0].Trim();
                                if (vals.Length >= 2) partype = vals[1].Trim();
                                if (vals.Length >= 3) pardefault = vals[2].Trim();
                                if (namevaluepair.Length >= 2) parval = namevaluepair[1].Trim(); else parval = null;
                            }
                            else
                            {
                                parname = $"Param{idx + 1}";
                                parval = parvals[idx].Param.Trim();
                            }

                            string sVariable = CheckIfVariable(parval, out bool isInput);
                            node1.Parameters.Add(parname, new Parameter(parname, parval, node1)
                            {
                                DataType = partype,
                                MyBlockParamDefaultValue = pardefault,
                                Variable = sVariable,
                                IsInput = isInput
                            });
                        }
                    }
                }
                //TODO: handle other / unrecognized blocks
                //TODO: handle MyBlock and params
            }

            return node1;
        }

        private static string CheckIfVariable(string parval, out bool isInput)
        {
            string sVariable = null;
            isInput = false;
            if (parval != null)
            {
                var match = Regex.Match(parval, @"((?<outpar>out)\s+)?\$(?<varname>\w+)");
                if (match.Success)
                {
                    sVariable = match.Groups["varname"].Value;
                    if (match.Groups["outpar"].Success) isInput = false;
                }
            }

            return sVariable;
        }

        private static List<(string Param, bool IsString)> ExtractParamsToList(string paramsStr)
        {
            var parvals = new List<(string Param, bool IsString)>();
            if (!string.IsNullOrEmpty(paramsStr))
            {
                bool bInArray = false;
                bool bInString = false;
                string sParam = String.Empty;
                bool isString = false;

                void _addParam(string sParam1, bool isString1)
                {
                    parvals.Add((Param: sParam1?.Trim().Trim('\''), IsString: isString1));
                }
                for (int i = 0; i < paramsStr.Length; i++)
                {
                    char ch = paramsStr[i];
                    if (!bInString)
                    {
                        if (ch == '\'') { bInString = true; }
                        if (!bInArray && ch == '[') { bInArray = true; }
                        if (bInArray && ch == ']') { bInArray = false; }
                        if (!bInArray && ch == ',')
                        {
                            _addParam(sParam, isString);
                            sParam = string.Empty; isString = false;
                            continue;
                        }

                        sParam += ch;
                    }
                    else
                    {
                        if (ch == '\'')
                        {
                            // handle single quote escaping ''
                            if (i + 1 < paramsStr.Length && paramsStr[i + 1] == '\'') { i++; sParam += ch; continue; }
                            bInString = false;
                            isString = true;
                        }
                        sParam += ch;
                    }
                }
                //CHECK: initial state (not in string, not in array)
                if (sParam != null) _addParam(sParam, isString);
            }
            return parvals;
        }


        /// <summary>
        /// static ctor
        /// </summary>
        static ImportEV3GB()
        {
            RegisterConversion();
        }
        public static void RegisterConversion()
        {
            ProjectConversion.RegisterConverter(".ev3gb", typeof(ImportEV3GB));
        }

        /// <summary>
        /// Project conversion
        /// </summary>
        /// <param name="instream"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        ProjectResultData IProjectConverter.Convert(Stream instream, string filename)
        {
            using (var sr = new StreamReader(instream))
            {
                var alltext = sr.ReadToEnd();
                return ImportEV3GB.Convert(alltext, filename);
            }
        }
        public static ProjectResultData Convert(string alltext, string filename)
        {
            var lines = alltext.Split('\n').SkipWhile(string.IsNullOrWhiteSpace).Select(elem => elem.TrimEnd('\r')).ToArray();
            return CreateNodesFromText(lines, filename);
        }
    }
}
