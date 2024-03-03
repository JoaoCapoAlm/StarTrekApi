using System.Globalization;
using CrossCutting.Helpers;

namespace CrossCutting.Extensions
{
    public static class StringExtention
    {
        public static string CreateResourceName(this string originalName, string addInTheEndName = "")
        {
            var resourceName = RegexHelper.CaseInsensitiveReplace(originalName, "star", string.Empty);
            resourceName = RegexHelper.CaseInsensitiveReplace(resourceName, "trek", string.Empty);

            TextInfo res = CultureInfo.CurrentCulture.TextInfo;
            res.ToTitleCase(resourceName);

            return $"{RegexHelper.RemoveSpecialCharacters(resourceName.ToString())}{addInTheEndName}";
        }

        public static string CreateSynopsisResource(this string titleResource)
        {
            return $"{titleResource}Synopsis";
        }
    }
}
