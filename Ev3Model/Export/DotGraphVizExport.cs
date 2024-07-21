using System;
using System.Collections.Generic;
using System.Linq;

namespace EV3ModelLib.Export
{
    public static class DotGraphVizExport
    {
        public static string PrintNodeToDot(Node lbNode)
        {
            //graphviz model
            List<string> sb = new List<string>();
            sb.Add(@"digraph ev3graph { rankdir=LR; size=12.5; node[shape=box]; ranksep=1;");
            sb.Add("graph [fontname = \"helvetica\"]; node[fontname = \"helvetica\"]; edge[fontname = \"helvetica\"];");
            void traverseNode(Node lb)
            {
                if (lb.Children == null) return;

                // ipad ev3m has only one node, no subprograms
                if (!lb.Children.Any() && lb == lbNode) sb.Add($"\"{lb.Name}\"");

                foreach (var lb2 in lb.Children)
                {
                    // skip any "project extra" blocks added
                    if (lb2.NodeType > Node.NodeTypeEnum.ProjectExtraRoot) continue;

                    // dependency line
                    var line = $"\"{lb.Name}\" -> \"{lb2.Name}\"";

                    // avoid duplicate dependencies especially for hierarchical myblocks
                    if (!sb.Contains(line)) sb.Add(line);

                    // traverse all children
                    traverseNode(lb2);
                }
            }
            if (lbNode != null)
                traverseNode(lbNode);
            sb.Add("}");
            return string.Join(Environment.NewLine, sb.ToArray());
        }
    }
}
