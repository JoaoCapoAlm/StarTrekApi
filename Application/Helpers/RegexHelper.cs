using System.Text.RegularExpressions;

namespace Application.Helpers
{
    public static class RegexHelper
    {

        public static string RemoveSpecialCharacters(string text)
        {
            Regex regCharacters = new Regex("[*'\",_&#^@:]");
            text = regCharacters.Replace(text, string.Empty);

            Regex regSpace = new Regex("[ ]");
            text = regSpace.Replace(text, "");

            return text;
        }

        public static string CaseInsensitiveReplace(string originalString, string oldValue, string newValue)
        {
            Regex regex = new Regex(oldValue, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            return regex.Replace(originalString, newValue);
        }
    }
}
