using System.Globalization;
using Application.Helpers;

namespace Application.Helper
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
    }
}
