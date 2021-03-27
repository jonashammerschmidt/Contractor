using Contractor.Core.Helpers;
using System;
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

        private static IEnumerable<string> GetImportStatements(string fileData)
        {
            IEnumerable<string> importStatementsArray = fileData
                .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
                .Where(line => line.Trim().StartsWith("import "));

            return importStatementsArray;
        }
    }
}