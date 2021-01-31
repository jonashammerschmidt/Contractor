using System.Text.RegularExpressions;

namespace Contractor.Core.Helpers
{
    public static partial class StringExtension
    {
        public static bool IsAlpha(this string text)
        {
            return Regex.IsMatch(text, "^[a-zA-Z]+$");
        }

        public static string UpperFirstChar(this string text)
        {
            return char.ToUpper(text[0]) + text.Substring(1);
        }
    }
}
