using Contractor.Core.Helpers;
using System.Text.RegularExpressions;

namespace Contractor.Core.Tools
{
    public class PropertyLine
    {
        public static void FindStartingLineForNewProperty(string file, string entityName, StringEditor stringEditor)
        {
            bool hasConstructor = Regex.IsMatch(file, $"public .*{entityName}.*\\(");
            bool hasProperty = file.Contains("{ get; set; }");
            if (hasConstructor && hasProperty)
            {
                stringEditor.NextThatContains("{ get; set; }");
            }
            else
            {
                stringEditor.NextThatContains("{")
                          .NextThatContains("{");
            }
            stringEditor.NextUntil(line => !IsLineEmpty(line) && !ContainsProperty(line) && !ContainsAnnotation(line));
        }

        private static bool IsLineEmpty(string line)
        {
            return line.Trim().Length == 0;
        }

        public static bool ContainsProperty(string line)
        {
            return line.Replace(" ", "").Contains("{get;set;}");
        }

        private static bool ContainsAnnotation(string line)
        {
            return line.Trim().StartsWith("[");
        }
    }
}