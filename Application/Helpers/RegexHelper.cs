using System.Text.RegularExpressions;

namespace Application.Helpers
{
    public static partial class RegexHelper
    {
        [GeneratedRegex(@"^\d+$")]
        private static partial Regex NumericRegex();
        [GeneratedRegex("[ ]")]
        private static partial Regex SpaceRegex();
        [GeneratedRegex("[*'\",_&#^@:-]")]
        private static partial Regex SpecialCharacters();

        public static string RemoveSpecialCharacters(string text)
        {
            text = SpecialCharacters().Replace(text, string.Empty);

            return SpaceRegex().Replace(text, string.Empty);
        }

        public static string CaseInsensitiveReplace(string originalString, string oldValue, string newValue)
        {
            var regex = new Regex(oldValue, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            return regex.Replace(originalString, newValue);
        }

        public static bool StringIsNumeric(string text) => NumericRegex().IsMatch(text);
    }
}
