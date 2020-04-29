using System.Text.RegularExpressions;

namespace CFLFramework.Utilities.Extensions
{
    public static class StringExtensions
    {
        #region FIELDS

        private const string SpaceCharacter = " ";
        private const string UnderscoreCharacter = "_";
        private const string SnakeCasePattern = "((?<!^)[A-Z0-9])";
        private const string SplitterPattern = "(?<Alpha>[a-zA-Z]*)(?<Numeric>[0-9]*)";
        private const string AlphabetSplit = "Alpha";
        private const string NumericSplit = "Numeric";

        #endregion

        #region BEHAVIORS

        public static string ToSnakeCase(this string stringToConvert)
        {
            MatchEvaluator matchEvaluator = new MatchEvaluator(match => UnderscoreCharacter + match.ToString().ToLower());
            return Regex.Replace(stringToConvert.Replace(SpaceCharacter, string.Empty), SnakeCasePattern, matchEvaluator, RegexOptions.None).ToLower();
        }

        public static string[] SplitStringAndNumber(this string stringToSplit)
        {
            Match splitter = Regex.Match(stringToSplit.Replace(SpaceCharacter, string.Empty), SplitterPattern);
            return new string[] { splitter.Groups[AlphabetSplit].Value, splitter.Groups[NumericSplit].Value };
        }

        #endregion
    }
}
