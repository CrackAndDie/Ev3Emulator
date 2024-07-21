using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EV3DecompilerLib.Recognize
{
    /// <summary>
    /// class to describe a pattern element
    /// </summary>
    /// <example>
    ///     OP1
    ///     OP1(REQUIREDVALUE) -- param 1 is matched as prefix, any further params are not checked
    ///     OP1(,REQUIREDVALUE1,,REQUIREDVALUE2) -- param2 and param4 is matched as prefix, any other or further params are not checked
    ///     ^OP -- must be at the beginning of the subcall
    ///     $OP -- must be at the vary end of the subcall e.g. $RETURN
    ///     ?OP -- optional, can match if code contains, but yields to successul match if not exists
    ///     (OP1 | OP2(VAL1,) | OP3()) -- If any of elements match, it will yield to a succesful pattern match
    ///     OP1+ -- Match one or more occurences
    ///     OP1* -- Match zero or more occurences
    /// </example>
    public class PatternElem
    {
        /// <summary>
        /// Conditions that applies to pattern element
        /// </summary>
        [Flags]
        public enum ConditionsFlags { None = 0, AtObjectStart = 1, AtObjectEnd = 2, Optional = 4, EitherOr = 8, MoreThanOnce = 16 };

        /// <summary>
        /// Conditions for the whole element
        /// </summary>
        public ConditionsFlags Conditions { get; set; }

        /// <summary>
        /// Operation to match (can be prefix as well)
        /// </summary>
        public string Op { get; set; }

        /// <summary>
        /// Positional conditions for values (prefix)
        /// </summary>
        public Dictionary<int, string> ValueConditions { get; set; } = new Dictionary<int, string>();

        /// <summary>
        /// Sub Conditions for "either-or" mode
        /// </summary>
        public List<PatternElem> EitherOrPatternElements { get; set; }

        /// <summary>
        /// default ctor
        /// </summary>
        public PatternElem()
        {
            // NOOP
        }

        /// <summary>
        /// ctor from a strnig
        /// </summary>
        /// <param name="patternElemStr"></param>
        public PatternElem(string patternElemStr)
        {
            ParseString(patternElemStr);
        }

        /// <summary>
        /// Parse pattern strings to a list of Pattern Elements
        /// </summary>
        /// <param name="patternString"></param>
        /// <returns></returns>
        private void ParseString(string patternElemStr)
        {
            // this can be flag, reflect this in using not only one char later
            if (patternElemStr.First() == '^') { this.Conditions |= ConditionsFlags.AtObjectStart; patternElemStr = patternElemStr.Substring(1); }
            if (patternElemStr.First() == '$') { this.Conditions |= ConditionsFlags.AtObjectEnd; patternElemStr = patternElemStr.Substring(1); }
            if (patternElemStr.First() == '?') { this.Conditions |= ConditionsFlags.Optional; patternElemStr = patternElemStr.Substring(1); }
            if (patternElemStr.Last() == '+') { this.Conditions |= ConditionsFlags.MoreThanOnce; patternElemStr = patternElemStr.Substring(0, patternElemStr.Length - 1); }
            if (patternElemStr.Last() == '*') { this.Conditions |= ConditionsFlags.MoreThanOnce | ConditionsFlags.Optional; patternElemStr = patternElemStr.Substring(0, patternElemStr.Length - 1); }

            if (patternElemStr.StartsWith("(") && patternElemStr.EndsWith(")"))
            {
                var substrarr = patternElemStr.Trim('(', ')').Split('|');
                this.EitherOrPatternElements = substrarr.Select(elemstr => new PatternElem(elemstr.Trim())).ToList();
                this.Conditions |= ConditionsFlags.EitherOr;
            }
            else // if (!this.Conditions.HasFlag(ConditionsFlags.EitherOr))
            {
                var sa2 = patternElemStr.Split('(', ')'); //op & params
                this.Op = sa2[0];

                var paramConditions_ = sa2.Length > 1 ? sa2[1].Split(',') : null; //params
                if (paramConditions_ != null)
                {
                    for (int i = 0; i < paramConditions_.Length; i++)
                        if (!string.IsNullOrEmpty(paramConditions_[i]))
                            this.ValueConditions[i] = paramConditions_[i];
                }
            }
        }

        /// <summary>
        /// ToString override
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return
                (!string.IsNullOrEmpty(Op) ?
                    Op + "(" + string.Join(", ", ValueConditions?.Select(kvp => $"{kvp.Key}={kvp.Value}").ToArray()) + ")" : null) +
                (Conditions.HasFlag(ConditionsFlags.EitherOr) ? string.Join(" | ", EitherOrPatternElements.Select(elem => elem.ToString()).ToArray()) : null);
        }
    }
}
