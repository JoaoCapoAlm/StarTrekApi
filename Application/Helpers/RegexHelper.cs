using System.Text.RegularExpressions;

namespace Application.Helpers
{
    public static partial class RegexHelper
    {
        [GeneratedRegex(@"^\d+$")]
        private static partial Regex OnlyNumbersRegex();
        [GeneratedRegex(@"\s")]
        private static partial Regex WhiteSpaceRegex();
        [GeneratedRegex("[*'\",_&#^@>:</-]")]
        private static partial Regex SpecialCharacters();
        [GeneratedRegex("^[A-Za-z]+$")]
        private static partial Regex OnlyAlphabetRegex();

        public static string RemoveSpecialCharacters(string text)
        {
            text = SpecialCharacters().Replace(text, string.Empty);

            return WhiteSpaceRegex().Replace(text, string.Empty);
        }

        public static string CaseInsensitiveReplace(string originalString, string oldValue, string newValue)
        {
            var regex = new Regex(oldValue, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            return regex.Replace(originalString, newValue);
        }

        public static bool StringIsNumeric(string text) => OnlyNumbersRegex().IsMatch(text);

        public static bool StringIsSimpleAlphabet(string text) => OnlyAlphabetRegex().IsMatch(text);
    }
}
