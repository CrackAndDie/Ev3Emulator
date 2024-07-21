using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace EV3ModelLib
{
    [DebuggerDisplay("{CallDirectionInput?\"->\":\"\"}@{CallIndex}{CallDirectionInput?\"\":\"->\"} [{DataType}] {Identification}")]
    public class BlockParamInfo
    {
        public string DataType { get; set; }
        public int CallIndex { get; set; }
        public bool CallDirectionInput { get; set; } // all output is eliminated for Loops, Switches
        public string Identification { get; set; }
        public bool IsVisibilitySpecial { get; set; } // special visibility means input is moved to the top - will be used at EV3 generation sizing
        public bool IsResult { get; set; } // Result compiler directive (typically output) gets eliminated at WaitFor blocks (maybe used for Switch?)

        public const string CONST_CALLDIRECTION_INPUT = "Input";
        public const string CONST_CALLDIRECTION_OUTUT = "Output";
    }
    [DebuggerDisplay("{ShortName} [{string.Join(\", \",Params.Keys)}]")]
    public partial class BlockInfo
    {
        public string ShortName { get; internal set; }
        public string Reference { get; internal set; }
        public string IconName { get; internal set; }
        public string BlockFamily { get; internal set; }
        public Dictionary<string, BlockParamInfo> Params { get; internal set; }

        public static readonly ILookup<string, string> MapShortToRef;
        static BlockInfo()
        {
            MapShortToRef = BlockMapByRef.ToLookup(elem => elem.Value.ShortName, elem => elem.Key);
        }
    }
}

