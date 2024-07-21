using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EV3ModelLib
{
    /// <summary>
    /// Class to process generic robot (ev3, rbf) project related statistics and build project tree
    /// </summary>
    public class Project
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name"></param>
        public Project(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Project name
        /// </summary>
        public string Name { get; }

        public class GlobalVariable
        {
            public string DataType { get; set; }
            public List<string> UsedBy { get; } = new List<string>();
        }
        public class MediaFile
        {
            public string Contents { get; set; }
            public List<string> UsedBy { get; } = new List<string>();
        }
        public class ProgramItem
        {
            public bool IsMyBlock => !string.IsNullOrEmpty(IconForMyBlock);
            public string IconForMyBlock { get; set; }
            public int BlockCount { get; set; }
            public List<string> UsedBy { get; } = new List<string>();
            public Dictionary<string, string> Parameters { get; } = new Dictionary<string, string>();
        }

        public Dictionary<string, GlobalVariable> GlobalVariables { get; } = new Dictionary<string, GlobalVariable>();
        public Dictionary<string, MediaFile> MediaFiles { get; } = new Dictionary<string, MediaFile>();
        public Dictionary<string, string> ProjectInfo { get; } = new Dictionary<string, string>();
        public Dictionary<string, string> Warnings = new Dictionary<string, string>();
        public Dictionary<string, ProgramItem> Dependencies = new Dictionary<string, ProgramItem>();

        public Node ProjectNode { get; private set; }

        public const string CONST_MultiplieValuesSeparator = ", ";

        /// <summary>
        /// Media type by path extension
        /// </summary>
        /// <param name="mediafile1"></param>
        /// <returns></returns>
        protected static string GetMediaTypeByExtension(string mediafile1)
        {
            return Path.GetExtension(mediafile1) == Node.EXT_RGF_GRAPHICS ? "graphic" : "sound";
        }

        /// <summary>
        /// Add warning message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="block"></param>
        protected virtual void AddWarning(string message, string block)
        {
            //ConsoleExt.WriteLine("WARNING: " + message, ConsoleColor.Red);
            Warnings[message] = block;
        }

        /// <summary>
        /// Create Project
        /// </summary>
        /// <returns></returns>
        public Project CreateProject(List<Node> nodes)
        {
            void Recurse(Node block1, Action<Node> action1) { block1.Children.ForEach(elem => { action1(elem); Recurse(elem, action1); }); }
            //var blocks = new List<EV3GBlock>(ev3gMyblocks); blocks.Insert(0, ev3gProgram);
            foreach (var block in nodes)
            {
                var pri = new Project.ProgramItem();
                this.Dependencies[block.Name] = pri;

                //-- check subcalls & totl number of blocks
                int blockcnt = 0;
                Recurse(block, block2 =>
                {
                    blockcnt++;

                    //-- usedby
                    if (block2.NodeType == Node.NodeTypeEnum.MyBlock && !pri.UsedBy.Contains(block2.Name)) pri.UsedBy.Add(block2.Name);

                    //-- variables
                    if (block2.Name.StartsWith(Node.BLOCK_WriteVariablePrefix) || block2.Name.StartsWith(Node.BLOCK_ReadVariablePrefix))
                    {
                        var varName = block2.Parameters["name"].Value;
                        if (!this.GlobalVariables.TryGetValue(varName, out Project.GlobalVariable globalVar))
                        {
                            //var varType = pm.GlobalVariables_VariableId_2_VariableType[varName];
                            this.GlobalVariables[varName] = globalVar = new Project.GlobalVariable() { }; // DataType = varType };
                        }
                        if (!globalVar.UsedBy.Contains(block.Name)) globalVar.UsedBy.Add(block.Name);
                    }

                    //-- rgf,rsf media
                    if (block2.Name == "Display.File")
                    {
                        var sFilename = block2.Parameters["Filename"].Value; //ToDO: error handling
                        sFilename = Path.GetFileNameWithoutExtension(sFilename.Trim('\'')) + Node.EXT_RGF_GRAPHICS;
                        if (!this.MediaFiles.TryGetValue(sFilename, out Project.MediaFile media))
                            this.MediaFiles[sFilename] = new Project.MediaFile();
                        if (!this.MediaFiles[sFilename].UsedBy.Contains(block.Name)) this.MediaFiles[sFilename].UsedBy.Add(block.Name);
                    }
                    else if (block2.Name == "Sound.File")
                    {
                        var sFilename = block2.Parameters["Name"].Value; //ToDO: error handling
                        sFilename = Path.GetFileNameWithoutExtension(sFilename.Trim('\'')) + Node.EXT_RSF_SOUND;
                        if (!this.MediaFiles.TryGetValue(sFilename, out Project.MediaFile media))
                            this.MediaFiles[sFilename] = new Project.MediaFile();
                        if (!this.MediaFiles[sFilename].UsedBy.Contains(block.Name)) this.MediaFiles[sFilename].UsedBy.Add(block.Name);
                    };
                    pri.BlockCount = blockcnt;

                    //-- check if myblock, addparameters
                    if (block.NodeType == Node.NodeTypeEnum.MyBlock)
                    {
                        pri.IconForMyBlock = "empty";
                        //if (!pri.Parameters.Any()) block.ParameterTypes.ToList().ForEach(kvp => { pri.Parameters[kvp.Key] = kvp.Value.ToString(); });
                    }
                });
            }

            //process common routines and return node
            this.ProcessProject();
            return this;
        }

        /// <summary>
        /// Post process step
        /// </summary>
        public Node ProcessProject()
        {
            Dictionary<string, Node> dependencies2 = new Dictionary<string, Node>();
            Dictionary<string, List<string>> dependencies_UsedBy = new Dictionary<string, List<string>>();

            // process dependencies
            foreach (var block1 in Dependencies)
            {
                foreach (var block2 in block1.Value.UsedBy)
                {
                    if (!dependencies_UsedBy.ContainsKey(block2)) dependencies_UsedBy.Add(block2, new List<string>());
                    dependencies_UsedBy[block2].Add(block1.Key);
                }
            }

            // create root node
            ProjectNode = new Node(Name) { NodeType = Node.NodeTypeEnum.ProjectExtraRoot };

            // start creating Nodes
            foreach (var kvp in Dependencies)
            {
                var programNode = new Node(kvp.Key)
                {
                    NodeType = kvp.Value.IsMyBlock ? Node.NodeTypeEnum.MyBlock : Node.NodeTypeEnum.Program,
                    Parent = ProjectNode
                };
                programNode.AddSimpleParameter("#", kvp.Value.BlockCount.ToString());
                if (kvp.Value.Parameters.Any()) programNode.AddSimpleParameter("params", string.Join(CONST_MultiplieValuesSeparator, kvp.Value.Parameters.Select(kvp2 => $"{kvp2.Key}[{kvp2.Value}]").ToArray()));

                ProjectNode.Children.Add(programNode);
                dependencies2[kvp.Key] = programNode;
            }

            // check for unused variables and media files
            foreach (var gv in GlobalVariables)
            {
                if (!gv.Value.UsedBy.Any())
                {
                    AddWarning($"Unused variable {gv.Key}[{gv.Value.DataType}]", null);
                }
            }

            // check for unused media files
            foreach (var mf in MediaFiles)
            {
                if (!mf.Value.UsedBy.Any())
                {
                    string mediaFile = Path.GetFileNameWithoutExtension(mf.Key);
                    string mediaType = GetMediaTypeByExtension(mf.Key);
                    AddWarning($"Unused media {mediaType} file {mediaFile}", null);
                }
            }

            // post process dependencies with LogBlocks
            foreach (var kvp in Dependencies)
            {
                var block1 = kvp.Key;
                var called_blocks = kvp.Value;
                var lb1 = dependencies2[block1];
                foreach (var block2 in called_blocks.UsedBy)
                {
                    if (dependencies2.ContainsKey(block2))
                    {
                        var lb2 = dependencies2[block2];
                        if (!lb1.Children.Contains(lb2))
                        {
                            lb1.Children.Add(lb2);
                            // remove from top if used anywhere else
                            ProjectNode.Children.Remove(lb2);
                        }
                    }
                    else
                    {
                        // Warning: a called MyBlock is missing
                        AddWarning($"Project missing myblock {block2}", block1);
                    }
                }
                // optional: add variables 
                var globalVariablesUsed = GlobalVariables
                                          .Where(item => item.Value.UsedBy.Contains(block1))
                                          .OrderBy(gvarkvp => gvarkvp.Key)
                                          .Select(gvarkvp => $"{gvarkvp.Key}[{gvarkvp.Value.DataType}]")
                                          .ToList();
                if (globalVariablesUsed.Count > 0) lb1.AddSimpleParameter("Vars", string.Join(CONST_MultiplieValuesSeparator, globalVariablesUsed));
            }

            // order callee graph by alphanumeric
            foreach (var kvpDeps in dependencies2)
            {
                var lb1 = kvpDeps.Value;
                lb1.Children.OrderBy(lbi => lbi.Name).ToList();
            }

            //--------------------------
            // add project infos
            ProjectInfo.ToList().ForEach(kvp => ProjectNode.AddSimpleParameter(kvp.Key, kvp.Value));

            //--------------------------
            // add mediafiles to ProjectNode
            foreach (var extIter in new[] { Node.EXT_RGF_GRAPHICS, Node.EXT_RSF_SOUND })
            {
                var media = MediaFiles
                                .Where(mediakvp => Path.GetExtension(mediakvp.Key) == extIter)
                                .OrderBy(mediakvp => mediakvp.Key)
                                .ToList();

                if (media.Count > 0)
                {
                    Node lbMediaNode = new Node(extIter == Node.EXT_RGF_GRAPHICS ? Node.PROJECT_IMAGES : Node.PROJECT_SOUNDS)
                    {
                        NodeType = extIter == Node.EXT_RGF_GRAPHICS ? Node.NodeTypeEnum.ProjectExtraImages : Node.NodeTypeEnum.ProjectExtraSounds,
                        Parent = ProjectNode
                    };

                    lbMediaNode.Children.AddRange(new List<Node>(
                        media.Select(mediakvp =>
                        {
                            var lb1 = new Node(Path.GetFileNameWithoutExtension(mediakvp.Key))
                            {
                                NodeType = lbMediaNode.NodeType,
                                Parent = lbMediaNode
                            };
                            //lb1.BlockDisplayDetails = lbMediaNode.BlockDisplayDetails;
                            if (mediakvp.Value.UsedBy.Count > 0)
                                lb1.AddSimpleParameter(Node.CONST_ATTR_USEDBY,
                                    string.Join(CONST_MultiplieValuesSeparator,
                                        mediakvp.Value.UsedBy.ToArray()
                                        //item.Value.Select(item2 => Path.GetFileNameWithoutExtension(item2)).ToArray()
                                        )
                                    );

                            //-- add rgf bitmap in base64
                            if (lb1.NodeType == Node.NodeTypeEnum.ProjectExtraImages && !string.IsNullOrEmpty(mediakvp.Value.Contents))
                            {
                                lb1.AddSimpleParameter($"bitmap", mediakvp.Value.Contents);
                            }
                            else if (lb1.NodeType == Node.NodeTypeEnum.ProjectExtraSounds && !string.IsNullOrEmpty(mediakvp.Value.Contents))
                            {
                                lb1.AddSimpleParameter($"sound", mediakvp.Value.Contents);
                            }

                            return lb1;
                        }).ToList()
                    ));
                    ProjectNode.Children.Add(lbMediaNode);
                };
            }

            //--------------------------
            // add variables to ProjectNode
            if (GlobalVariables.Any())
            {
                var lbVariables = new Node(Node.PROJECT_VARS)
                {
                    NodeType = Node.NodeTypeEnum.ProjectExtraVariables,
                    Parent = ProjectNode
                };

                lbVariables.Children.AddRange(new List<Node>(
                    GlobalVariables.Select(gvar =>
                    {
                        var lb1 = new Node(gvar.Key)
                        {
                            NodeType = lbVariables.NodeType,
                            Parent = lbVariables
                        };
                        lb1.AddSimpleParameter("Type", gvar.Value.DataType);
                        var usedby = gvar.Value.UsedBy;
                        if (usedby.Any())
                        {
                            lb1.AddSimpleParameter(Node.CONST_ATTR_USEDBY,
                                string.Join(CONST_MultiplieValuesSeparator, usedby.ToArray())
                                );
                        }
                        return lb1;
                    })
                            ));
                ProjectNode.Children.Add(lbVariables);
            }

            //--------------------------
            // add warnings to ProjectNode
            if (Warnings.Any())
            {
                var lbWarnings = new Node(Node.PROJECT_WARNINGS)
                {
                    NodeType = Node.NodeTypeEnum.ProjectExtraWarnings,
                    Parent = ProjectNode
                };

                lbWarnings.Children.AddRange(new List<Node>(
                    Warnings.Select(item =>
                    {
                        var lb1 = new Node(item.Key)
                        {
                            NodeType = lbWarnings.NodeType,
                            Parent = lbWarnings
                        };
                        //lb1.Params.AddString("Type", gvar.Value.Item1);
                        if (item.Value != null)
                        {
                            lb1.AddSimpleParameter("in", item.Value);
                        }
                        //lb1.BlockDisplayDetails = lbWarnings.BlockDisplayDetails;
                        return lb1;
                    })
                ));
                ProjectNode.Children.Add(lbWarnings);
            }

            // return project node
            return ProjectNode;
        }
    }
}
