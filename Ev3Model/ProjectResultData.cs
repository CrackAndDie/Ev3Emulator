using System;
using System.Collections.Generic;

namespace EV3ModelLib
{
    public class ProjectResultData
    {
        public Dictionary<string, Node> Nodes { get; }
        public Project Project { get; }

        public ProjectResultData(Dictionary<string, Node> nodes, Project project)
        {
            Nodes = nodes;
            Project = project;
        }

        public static implicit operator ProjectResultData(ValueTuple<Dictionary<string, Node>, Project> data)
        {
            return new ProjectResultData(data.Item1, data.Item2);
        }
    }
}
