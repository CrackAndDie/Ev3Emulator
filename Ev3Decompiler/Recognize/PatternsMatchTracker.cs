using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace EV3DecompilerLib.Recognize
{
    /// <summary>
    /// Internal Pattern match data for tracking match results simultaneously for all known patterns
    /// </summary>
    internal class PatternsMatchTracker
    {
        internal Dictionary<Pattern, int> matchpositions;
        //internal Dictionary<Pattern, Dictionary<string, string>> matchValuesOut;

        public PatternElem GetPatternElem(Pattern pattern)
        {
            return pattern.Elems[matchpositions[pattern]];
        }

        public PatternsMatchTracker()
        {
            matchpositions = new Dictionary<Pattern, int>();
            Pattern.PATTERNS.ForEach(elem => matchpositions[elem] = 0);
        }

        internal bool RegisterMatch(Pattern pm)
        {
            ++matchpositions[pm];
            return (pm.Elems == null || matchpositions[pm] >= pm.Elems.Count);
        }

        internal Pattern GetMatchingPattern()
        {
            return Pattern.PATTERNS.FirstOrDefault(pm => (pm.Elems != null && matchpositions[pm] == pm.Elems.Count));
        }

        //internal void AddOutput(Pattern pm, string key, string value)
        //{
        //    if (!matchValuesOut.ContainsKey(pm)) matchValuesOut.Add(pm, new Dictionary<string, string>());
        //    matchValuesOut[pm][key] = value;
        //}
    }
}
