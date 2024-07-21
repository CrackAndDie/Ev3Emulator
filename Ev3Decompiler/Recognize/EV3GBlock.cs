using EV3DecompilerLib.Decompile;
using EV3ModelLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EV3DecompilerLib.Recognize
{
    /// <summary>
    /// Match data for creating a tree structure
    /// Represents one EV3-G block
    /// </summary>
    [DebuggerDisplay("{Name} OBJECT{TargetObjectId}")]
    public class EV3GBlock : List<EV3GBlock>
    {
        #region public members
        public string Name
        {
            get { return _Name; }
            internal set
            {
                if (NameRef == null) NameRef = value; // set reference name at first set, skip any prefixes
                _Name = value;
            }
        }
        public string NameRef { get; set; }
        private string _Name;
        //public int ObjectId { get; internal set; }
        public int TargetObjectId { get; internal set; }
        public long Offset;
        public int OffsetLineId;
        public int OffsetEndLineId;
        public int TrackerGlobalStart = -1;
        public int TrackerGlobalEnd = -1;

        public enum GNodeType
        {
            None, Loop, Fork, ForkItem,
            MyBlockNode = 100, // this is both a myblock used in a program and a myblock top level node
            TopLevel = 200
        };
        public bool HasSwitch { get; set; }
        public bool HasWait { get; set; }

        public GNodeType NodeType = GNodeType.None;

        internal bool IsLegoStandardSub { get; set; }
        internal List<(LMSObject, LMSStatement)> VisitedObjects { get; set; }

        public Dictionary<string, string> Parameters { get; internal set; } = new Dictionary<string, string>();
        public Dictionary<string, CallparamExt> ParameterTypes { get; internal set; } = new Dictionary<string, CallparamExt>();
        public EV3GBlock Root;
        #endregion public members

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="root"></param>
        public EV3GBlock(EV3GBlock root, string name = null)
        {
            this.Root = root;
            this.Name = name;
        }

        /// <summary>
        /// Previous peer node
        /// </summary>
        internal EV3GBlock PrevPeer
        {
            get
            {
                if (Root == null) return null;
                int? idx = Root.FindIndex(elem => elem == this);
                if (!idx.HasValue || idx < 0 || idx == 0) return null;
                return Root[idx.Value - 1];
            }
        }

        /// <summary>
        /// Next peer node
        /// </summary>
        internal EV3GBlock NextPeer
        {
            get
            {
                if (Root == null) return null;
                int? idx = Root.FindIndex(elem => elem == this);
                if (!idx.HasValue || idx < 0 || idx == Root.Count - 1) return null;
                return Root[idx.Value + 1];
            }
        }

        /// <summary>
        /// Index of element within Root
        /// </summary>
        public int Index
        {
            get
            {
                if (Root == null) return -1;
                return Root.IndexOf(this);
            }
        }

        #region output to string
        /// <summary>
        /// ToString functon override
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //return this.Name +
            //    //(TargetObjectId > 0 ? $"[OBJ{TargetObjectId}]" : null) +
            //    (this.Parameters.Count > 0 ? " " + string.Join(" | ", this.Parameters.Select(elemkvp => $"{elemkvp.Key}: {elemkvp.Value}").ToArray()) : null);
            return this.Name;
        }

        /// <summary>
        /// Get Node VIX reference value
        /// </summary>
        /// <returns></returns>
        public string GetReference()
        {
            return BlockInfo.MapShortToRef?.Contains(this.NameRef) == true ? BlockInfo.MapShortToRef[this.NameRef].First() : null;
        }

        #endregion output to string

        #region constants
        public const string PORT_MOTOR = "MotorPort";
        public const string PORT_MOTORS = "Ports";
        public const string PORT_SENSOR = "Port";
        public const string PORT_SENSOR2 = "Port_Number";
        #endregion constants

        /// <summary>
        /// Clone shall be a deep clone (used for myblocks)
        /// </summary>
        /// <returns></returns>
        public EV3GBlock Clone()
        {
            return (EV3GBlock)MemberwiseClone();
        }

        #region static ctor
        static EV3GBlock()
        {
            EV3GBlock2ModelConverter.RegisterConverter();
        }
        #endregion
    }
}
