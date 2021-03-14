using System.Linq;
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

        public static string LowerFirstChar(this string text)
        {
            return char.ToLower(text[0]) + text.Substring(1);
        }

        public static string ToReadable(this string text)
        {
            text = text.UpperFirstChar();
            text = string.Concat(text.Select(x => char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
            text = text.Replace("ae", "ä");
            text = text.Replace("oe", "ö");
            text = text.Replace("ue", "ü");
            text = text.Replace("Ae", "Ä");
            text = text.Replace("Oe", "Ö");
            text = text.Replace("Ue", "Ü");
            return text;
        }

        public static string ToKebab(this string text)
        {
            return StringConverter.PascalToKebabCase(text);
        }
    }
}