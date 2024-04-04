using System;
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

            // Kebab-/Pascal-Case to NormalCase (FooBar -> Foo Bar)
            text = string.Concat(text.Select(x => char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');

            text = text.Replace("ae", "ä");
            text = text.Replace("oe", "ö");
            text = text.Replace("ue", "ü");
            text = text.Replace("Ae", "Ä");
            text = text.Replace("Oe", "Ö");
            text = text.Replace("Ue", "Ü");
            return text;
        }

        public static string ToVariableName(this string text)
        {
            text = text.UpperFirstChar();
            text = text.Replace(" ", "");
            text = text.Replace("ß", "ss");
            text = text.Replace("ä", "ae");
            text = text.Replace("ö", "oe");
            text = text.Replace("ü", "ue");
            text = text.Replace("Ä", "Ae");
            text = text.Replace("Ö", "Oe");
            text = text.Replace("Ü", "Ue");
            return text;
        }

        public static string ToKebab(this string text)
        {
            return StringConverter.PascalToKebabCase(text);
        }

        public static string RemoveFirstOccurrence(this string source, string toRemove)
        {
            int index = source.IndexOf(toRemove, StringComparison.Ordinal);
            if (index < 0)
            {
                // The string to remove was not found.
                return source;
            }

            return source.Remove(index, toRemove.Length);
        }
    }
}