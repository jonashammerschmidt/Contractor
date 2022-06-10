using Contractor.Core.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Contractor.Core.Tools
{
    public static class ImportStatements
    {
        public static string Add(string fileData, string types, string path)
        {
            string importStatement = $"import {{ {types} }} from '{path}';";

            if (fileData.Contains(importStatement))
            {
                return fileData;
            }

            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.InsertLine(importStatement);
            if (!GetImportStatements(fileData).Any(line => line.StartsWith("import ")))
            {
                stringEditor.InsertNewLine();
            }

            return stringEditor.GetText();
        }

        public static string Order(string fileContent)
        {
            List<string> importLines = new List<string>();
            List<string> otherLines = new List<string>();
            LoadLines(fileContent, importLines, otherLines);

            IDictionary<string, string> imports = ParseImports(importLines);
            imports = ConsolidateImports(imports);

            importLines = imports
                .Select(import => $"import {{ {import.Value} }} from '{import.Key}';")
                .ToList();

            var final = string.Join("\n", TrimArray(otherLines)) +
                "\n";

            if (TrimArray(importLines).Count() > 0)
            {
                final = string.Join("\n", TrimArray(importLines)) +
                    "\n" +
                    "\n" +
                    final;
            }

            return final;
        }

        private static IEnumerable<string> GetImportStatements(string fileData)
        {
            IEnumerable<string> importStatementsArray = fileData
                .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
                .Where(line => line.Trim().StartsWith("import "));

            return importStatementsArray;
        }

        private static void LoadLines(string fileContent, List<string> importLines, List<string> otherLines)
        {
            bool inImport = false;
            string importLine = "";
            foreach (string line in fileContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None))
            {
                if (inImport)
                {
                    if (line.EndsWith(";"))
                    {
                        inImport = false;
                        importLines.Add(importLine);
                        importLine = "";
                    }
                }
                else if (line.StartsWith("import {"))
                {
                    if (line.EndsWith(";"))
                    {
                        importLines.Add(line);
                    }
                    else
                    {
                        inImport = true;
                        importLine += line;
                    }
                }
                else
                {
                    otherLines.Add(line);
                }
            }
        }

        private static Dictionary<string, string> ParseImports(List<string> importLines)
        {
            Dictionary<string, string> imports = new Dictionary<string, string>();
            foreach (var importLine in importLines)
            {
                var keys = importLine.Split('{')[1].Split('}')[0].Split(',').Select(key => key.Trim());
                string value = importLine.Split(new[] { "\"", "'" }, StringSplitOptions.None)[1];

                foreach (var key in keys)
                {
                    if (!imports.ContainsKey(key))
                    {
                        imports.Add(key, value);
                    }
                }
            }

            return imports;
        }

        private static IDictionary<string, string> ConsolidateImports(IDictionary<string, string> imports)
        {
            SortedDictionary<string, string> newImports = new SortedDictionary<string, string>(new ImportComparer());

            foreach (var import in imports)
            {
                if (newImports.ContainsKey(import.Value))
                {
                    newImports[import.Value] += ", " + import.Key;
                }
                else
                {
                    newImports.Add(import.Value, import.Key);
                }
            }

            foreach (string key in newImports.Keys.ToArray())
            {
                newImports[key] = string.Join(
                    ", ",
                    newImports[key]
                        .Split(',')
                        .Select(v => v.Trim())
                        .OrderBy(v => v));
            }

            return newImports;
        }

        private static IEnumerable<string> TrimArray(IEnumerable<string> stringArray)
        {
            List<string> stringList = stringArray.ToList();

            for (int i = 0; i < stringList.Count(); i++)
            {
                if (stringList.ElementAt(i).Trim().Length == 0) {
                    stringList.RemoveAt(i--);
                } else {
                    break;
                }
            }

            
            for (int i = stringList.Count() - 1; i >= 0; i--)
            {
                if (stringList.ElementAt(i).Trim().Length == 0) {
                    stringList.RemoveAt(i);
                } else {
                    break;
                }
            }

            return stringList;
        }

        private class ImportComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                bool xStartsWithDot = x.StartsWith(".");
                bool yStartsWithDot = y.StartsWith(".");
                if (xStartsWithDot && !yStartsWithDot)
                {
                    return 1;
                }
                else if (!xStartsWithDot && yStartsWithDot)
                {
                    return -1;
                }

                bool xStartsWithAt = x.StartsWith("@");
                bool yStartsWithAt = y.StartsWith("@");
                if (xStartsWithAt && !yStartsWithAt)
                {
                    return -1;
                }
                else if (!xStartsWithAt && yStartsWithAt)
                {
                    return 1;
                }

                return x.CompareTo(y);
            }
        }
    }
}