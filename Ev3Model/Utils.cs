using System.IO;
using System.Text.RegularExpressions;

namespace EV3ModelLib
{
    public static class Utils
    {
        private static readonly Regex regReplaceEscapesAndSpaces = new Regex(@"\\(?=[^\\])");
        public static string UnescapeParamName(string argvalue)
        {
            return regReplaceEscapesAndSpaces.Replace(argvalue, string.Empty);
        }
        public static string UnShortenParamName(string argvalue)
        {
            return argvalue?.Replace(@"__", ", ").Replace(@"_", " ");
        }
        public static string UnShortenAnEscapeParamName(string argvalue)
        {
            return EscapeVIX(argvalue?.Replace(@"__", ", ").Replace(@"_", " "));
        }
        public static string ShortenParamName(string argvalue)
        {
            //!! TODO: check was with escaping - does it still work
            return argvalue?.Replace(@", ", "__").Replace(@" ", "_");
        }
        public static string UnescapeAndShortenParamName(string argvalue)
        {
            return ShortenParamName(UnescapeParamName(argvalue));
        }

        private static Regex reVIXFileEscape = new Regex(@"([\(\), .])");
        public static string EscapeVIX(string s) { return reVIXFileEscape.Replace(s, @"\$1"); }
    }
}
