using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EV3ModelLib
{
    public static class PrintHelper
    {
        #region Printing
        /// <summary>
        /// Formatted output for a node either in tree or ev3gbasic format
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string GetPMDListString(this Node node)
        {
            return GetPMDListString_priv(node, 0, Array.Empty<bool>(), true);
        }
        public static string GetFormattedOutput(this IReadOnlyDictionary<string, Node> nodes, bool addSeparator = true)
            => nodes.Values.GetFormattedOutput(addSeparator);
        
        /// <summary>
        /// Formatted output for a list of nodes either in tree or ev3gbasic format
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="addSeparator"></param>
        /// <returns></returns>
        public static string GetFormattedOutput(this IEnumerable<Node> nodes, bool addSeparator = true)
        {
            if (nodes == null) return null;
            StringBuilder sbretval = new StringBuilder();
            int idx = 0;
            foreach (var node1 in nodes)
            {
                if (idx++ > 0 && addSeparator) sbretval.AppendLine(PrintHelper.Separator());
                if (PrintOptions.Value?.TargetColorIndex == PrintOptionsClass.TargetColorEnum.Html) sbretval.Append("<pre>");

                sbretval.Append(node1.GetPMDListString());

                if (PrintOptions.Value?.TargetColorIndex == PrintOptionsClass.TargetColorEnum.Html) sbretval.Append("</pre>");
            }
            return sbretval.ToString();
        }

        /// <summary>
        /// Internal worker for formatted output for a node
        /// </summary>
        /// <param name="lpmd"></param>
        /// <param name="depth"></param>
        /// <param name="isParentLastChildArr"></param>
        /// <param name="isThisLastKid"></param>
        /// <returns></returns>
        private static string GetPMDListString_priv(this Node lpmd, int depth, bool[] isParentLastChildArr, bool isThisLastKid)
        {
            StringBuilder sbretval = new StringBuilder();

            bool[] isParentLastChildArrNew = isParentLastChildArr;

            // No need for ForkParent nodes as with ev3 compilation there is no code after TRIG/FORK statements
            // skip single block if it was the last FORK (no nodes follow, this is the longest one due to reordering)
            bool skipThisSingleBlock = false;
            //if (IsOptimizeParallelLastBlock) if (lpmd.NodeType == Node.NodeTypeEnum.ForkItem && isThisLastKid) skipThisSingleBlock = true;

            // print line starting
            if (!skipThisSingleBlock)
            {
                StringBuilder sb = new StringBuilder();
                isParentLastChildArrNew = isParentLastChildArr.Concat(new bool[1] { isThisLastKid }).ToArray();
                //sb.Append(string.Join(".", isParentLastChildArrNew.ToArray()));
                if (PrintOptions.Value.UseTreeChars)
                {
                    for (int i = 1; i < isParentLastChildArrNew.Length; i++)
                    {
                        bool aLastKidOnLevel = isParentLastChildArrNew[i];
                        bool isThisLastLevel = i == isParentLastChildArr.Length;
                        sb.Append(
                                (aLastKidOnLevel ?
                                        (isThisLastLevel ? " └" : "  ") :
                                        (isThisLastLevel ? " ├" : " │")));
                        sb.Append(
                            isThisLastLevel ? "─ " : " "
                            );
                    }
                }
                else
                {
                    sb.Append("".PadLeft((isParentLastChildArrNew.Length - 1) * 2));
                }

                string colorOverride = null;
                if (PrintOptions != null && PrintOptions.Value.UseColoring && PrintOptions.Value.pmdHighlight != null && PrintOptions.Value.pmdHighlight.Contains(lpmd)) colorOverride = "Blue";
                sbretval.Append(StringWithColor(colorOverride, sb.ToString()));

                // print line content
                sbretval.AppendLine(lpmd.GetPmdPrintString(depth, colorOverride));
            }

            // print children
            foreach (var pmd in lpmd.Children)
                sbretval.Append(GetPMDListString_priv(pmd, depth + 1, isParentLastChildArrNew, lpmd.Children?.Last() == pmd));

            return sbretval.ToString();
        }

        /// <summary>
        /// Internal worker for formatted output for a node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="depth"></param>
        /// <param name="colorOverride"></param>
        /// <returns></returns>
        private static string GetPmdPrintString(this Node node, int depth = 0, string colorOverride = null)
        {
            string colorpar = colorOverride;
            if (PrintOptions != null && PrintOptions.Value.UseColoring && colorpar == null) colorpar = C(BlockFamilyColorizer.BlockColorMap["PARAMS"]);

            StringBuilder sb = new StringBuilder();
            string controlStr = null;
            {
                string color = colorOverride;
                if (PrintOptions != null && PrintOptions.Value.UseColoring && color == null)
                {
                    if (depth == 0)
                    {
                        color = C(BlockFamilyColorizer.BlockColorMap[Node.BLOCK_Root]);
                        if (PrintOptions.Value?.TargetColorIndex == PrintOptionsClass.TargetColorEnum.Html)
                            sb.Append($"<a id='{node.Name}'></a>");
                    }
                    else
                    {
                        if (BlockFamilyColorizer.BlockTypeColorMap.ContainsKey(node.NodeType)) color = C(BlockFamilyColorizer.BlockTypeColorMap[node.NodeType]);
                        //else if (node.HasSwitch || node.HasWait) color = C(BlockFamilyColorizer.BlockColorSwitchAndWaitFor);
                        else if (BlockFamilyColorizer.BlockColorMap.TryGetValue(node.Name, out var colorRecord)) color = C(colorRecord);
                    }
                }

                //-- Node short Name                
                string nameStr = node.Name;

                //-- add link to MyBlock nodes within the text code (not top nodes)
                string prepend_str = null;
                string append_str = null;
                if (node.NodeType.In(Node.NodeTypeEnum.MyBlock, Node.NodeTypeEnum.Program) && node.Parent != null &&
                    PrintOptions.Value?.TargetColorIndex == PrintOptionsClass.TargetColorEnum.Html)
                {
                    prepend_str = $@"<a href='#{node.Name}'>";
                    append_str = @"</a>";
                }

                //-- add EV3GBasic for UseSubHeading
                if (PrintOptions.Value?.UseSubHeading == true && node.NodeType.In(Node.NodeTypeEnum.MyBlock, Node.NodeTypeEnum.Program) && node.Parent == null)
                {
                    nameStr = Node.CONST_SUB_LITERAL + " " + nameStr;
                }

                sb.Append(prepend_str);

                //-- control
                if (node.NodeType == Node.NodeTypeEnum.Loop) controlStr = Node.BLOCK_LoopPrefix1;
                else if (node.HasWait) controlStr = Node.BLOCK_WaitForPrefix1;
                else if (node.HasSwitch) controlStr = Node.BLOCK_SwitchPrefix1;
                if (controlStr != null) sb.Append(StringWithColor(C(BlockFamilyColorizer.BlockColorSwitchAndWaitFor), controlStr + "("));

                sb.Append(StringWithColor(color, nameStr));

                sb.Append(append_str);
            }

            //if (this.TrackerGlobalStart >= 0)
            //    Console.Write($" | GLOBAL{this.TrackerGlobalStart}-GLOBAL{this.TrackerGlobalEnd}");
            //Console.ForegroundColor = ConsoleColor.DarkGray;

            var paramsstr = GetParamsStr(node, node.Parameters.Where(elem => !elem.Value.IsControlParameter).ToList());
            if (!string.IsNullOrWhiteSpace(paramsstr)) sb.Append(StringWithColor(colorpar, " " + paramsstr));

            if (controlStr != null)
            {
                sb.Append(StringWithColor(C(BlockFamilyColorizer.BlockColorSwitchAndWaitFor), ")"));

                var paramsstr2 = GetParamsStr(node, node.Parameters.Where(elem => elem.Value.IsControlParameter).ToList());
                if (!string.IsNullOrWhiteSpace(paramsstr2)) sb.Append(StringWithColor(colorpar, " " + paramsstr2));
            }

            return sb.ToString();
        }

        /// <summary>
        /// internal worker for getting formatted parameters for a node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="params1"></param>
        /// <returns></returns>
        private static string GetParamsStr(Node node, List<KeyValuePair<string, Parameter>> params1)
        {
            if (params1.Count > 0)
            {
                //-- any optional conversion for display
                var paramValues = params1.ToDictionary(
                    // key
                    elemkvp =>
                    {
                        string s = elemkvp.Key;
                        if (node.Parent == null && params1.Any() && PrintOptions.Value.UseSubHeading)
                        {
                            if (!string.IsNullOrEmpty(elemkvp.Value.DataType))
                            {
                                s += ":" + elemkvp.Value.DataType;
                                if (!string.IsNullOrEmpty(elemkvp.Value.MyBlockParamDefaultValue))
                                    s += ":" + elemkvp.Value.MyBlockParamDefaultValue;
                            }
                        }
                        return s;
                    },
                    // value
                    elemkvp =>
                    {
                        string prepend_str = null;
                        string append_str = null;

                        if (PrintOptions.Value.TargetColorIndex == PrintOptionsClass.TargetColorEnum.Html && !PrintOptions.Value.UseParamNames)
                        {
                            prepend_str = $"<span title='{elemkvp.Key}'>";
                            append_str = $"</span>";
                        }

                        if (node.NodeType == Node.NodeTypeEnum.ProjectExtraImages)
                        {
                            if (elemkvp.Value?.GetValue()?.StartsWith("data:") == true)
                            {
                                return PrintOptions.Value.TargetColorIndex != PrintOptionsClass.TargetColorEnum.Html ?
                                    "(binary)" :
                                    $"<a id='bitmap_{node.Name}' class='rgf'><img src='{elemkvp.Value?.Value}'></a>";
                            }
                        }
                        else if (node.NodeType == Node.NodeTypeEnum.ProjectExtraSounds)
                        {
                            if (elemkvp.Value?.GetValue()?.StartsWith("data:") == true)
                            {
                                return PrintOptions.Value.TargetColorIndex != PrintOptionsClass.TargetColorEnum.Html ?
                                    "(binary)" :
                                    $"<a id='sound_{node.Name}' class='rsf' href='#'><img src='img/play.png' class='notooltip'><audio src='{elemkvp.Value?.Value}'/></a>";
                            }
                        };

                        //-- adding html specific links
                        if (PrintOptions.Value.TargetColorIndex == PrintOptionsClass.TargetColorEnum.Html)
                        {
                            //-- in case usedby is added to project, add anchors
                            if (node.NodeType > Node.NodeTypeEnum.ProjectExtraRoot)
                            {
                                if (elemkvp.Key == "usedBy" && !string.IsNullOrEmpty(elemkvp.Value?.Value))
                                {
                                    return string.Join(", ", elemkvp.Value?.Value.Split(',')
                                    .Select(e => e.Trim())
                                    .Select(e2 => $@"<a href='#{e2}'>{e2}</a>")
                                    .ToArray());
                                }
                            };

                            //-- special tag display file filename and sound sources
                            if (node.Name == "Display.File" && elemkvp.Key == "Filename")
                            {
                                // Filename = "Neutral" --> link .rgf_displayfile in baselogic.js
                                prepend_str += $"<a href='#bitmap_{elemkvp.Value?.Value}' class='rgf_displayfile'>";
                                append_str = "</a>" + append_str;
                            }
                            else if (node.Name == "Sound.File" && elemkvp.Key == "Name")
                            {
                                // Filename = "Hello" --> link .rsf_soundfile in baselogic.js
                                prepend_str += $"<a href='#sound_{elemkvp.Value?.Value}' class='rsf_soundfile'>";
                                append_str = "</a>" + append_str;
                            }
                        }

                        //-- return formatted parameter value
                        var retval = elemkvp.Value?.GetValue();
                        if (elemkvp.Value.DataType == "String" && string.IsNullOrEmpty(elemkvp.Value.Variable))
                            retval = "'" + retval?.Replace("'", "''") + "'";
                        return prepend_str + retval + append_str;
                    });
                var strparam = (PrintOptions.Value.UseParamNames ?
                        string.Join(" | ", paramValues.Select(elemkvp => $"{elemkvp.Key}: {elemkvp.Value}").ToArray()) :
                        (PrintOptions.Value.UseSubHeading && node.Parent == null) ?
                            string.Join(", ", paramValues.Select(elemkvp => $"{elemkvp.Key}:={elemkvp.Value}").ToArray()) :
                            string.Join(", ", paramValues.Values.ToArray())
                    );

                return strparam;
            }

            return null;
        }

        /// <summary>
        /// Color code
        /// </summary>
        /// <param name="colorrecord"></param>
        /// <returns></returns>
        private static string C(string[] colorrecord)
        {
            return PrintOptions.Value.TargetColorIndex >= 0 ?
                colorrecord[(int)PrintOptions.Value.TargetColorIndex] :
                "Gray";
        }

        /// <summary>
        /// Colorize output content
        /// </summary>
        /// <param name="colorFront"></param>
        /// <param name="colorBack"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        /// <see cref="https://github.com/AlexGhiondea/OutputColorizer"/>
        internal static string StringWithColor(string colorFront, string content)
        {
            if (content == null) return null;

            if (colorFront != null)
            {
                switch (PrintOptions.Value?.TargetColorIndex)
                {
                    case PrintOptionsClass.TargetColorEnum.Console:
                        content = content.Replace("[", @"\[").Replace("]", @"\]"); //TODO
                        return $"[{colorFront}!{content}]";
                    case PrintOptionsClass.TargetColorEnum.Html:
                        return $"<span style='color:{colorFront}'>{content}</span>";
                    default:
                        return content;
                }
            }
            else
            {
                return content;
            }
        }

        /// <summary>
        /// Add separator
        /// </summary>
        /// <returns></returns>
        public static string Separator()
        {
            return PrintOptions.Value.TargetColorIndex != PrintOptionsClass.TargetColorEnum.Html ?
                StringWithColor("DarkCyan", new String('=', 80)) :
                "<hr>";
        }

        /// <summary>
        /// printing options
        /// </summary>
        public static ThreadLocal<PrintOptionsClass> PrintOptions = new ThreadLocal<PrintOptionsClass>(() =>
        {
            return new PrintOptionsClass();
        });
        #endregion Printing

        /// <summary>
        /// generic extension for texting "In" in a enumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="set"></param>
        /// <returns></returns>
        public static bool In<T>(this T x, params T[] set)
        {
            return set.Contains(x);
        }

        /// <summary>
        /// Get value for a parameter
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string GetValue(this Parameter param)
        {
            return PrintOptions.Value.UseFormattedValues ? param.ValueFormatted : param.Value;
        }
    }

    /// <summary>
    /// Print Options
    /// </summary>
    public class PrintOptionsClass
    {
        public enum TargetColorEnum
        {
            None = -1, Html = 0, Console = 1
        }
        public TargetColorEnum TargetColorIndex { get; set; } = TargetColorEnum.Console;
        public bool UseColoring { get { return TargetColorIndex != TargetColorEnum.None; } }
        public List<Node> pmdHighlight { get; set; } = null;
        public bool UseTreeChars { get; set; } = true;
        public bool UseParamNames { get; set; } = true;
        public bool UseFormattedValues { get; set; } = true;
        public bool UseSubHeading { get; set; } = false; // add "Sub" to the top elements -> for EV3GBasic
        public void ApplyEV3GBasicFormat()
        {
            this.UseTreeChars = false;
            this.UseParamNames = false;
            this.UseFormattedValues = true;
            this.UseSubHeading = true;
        }
    }
}
