using Contractor.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contractor.Core.Tools
{
    public static class UsingStatements
    {
        public static string Add(string fileData, string namespaceText)
        {
            string usingStatement = "using " + namespaceText + ";";

            if (fileData.Contains(usingStatement))
            {
                return fileData;
            }

            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.InsertLine(usingStatement);
            if (!GetUsingStatements(fileData).Contains("using "))
            {
                stringEditor.InsertNewLine();
            }

            return stringEditor.GetText();
        }

        public static string Sort(string fileData)
        {
            string usingStatements = GetUsingStatements(fileData);
            string restOfTheFile = fileData.Substring(usingStatements.Length);

            List<string> lines = usingStatements.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None).ToList();

            lines.Sort((a, b) =>
            {
                if (a.Contains("using") ^ b.Contains("using"))
                {
                    return b.CompareTo(a);
                }

                if (a.Contains("using") && b.Contains("using"))
                {
                    return a.CompareTo(b);
                }

                return 1;
            });

            usingStatements = string.Join("\r\n", lines);

            return usingStatements + restOfTheFile;
        }

        private static string GetUsingStatements(string fileData)
        {
            return fileData.Split(new[] { "namespace" }, StringSplitOptions.None)[0];
        }
    }
}