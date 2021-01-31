using Contractor.Core.Helpers;

namespace Contractor.Core.Tools
{
    public class UsingStatementAddition
    {
        public string Add(string fileData, string namespaceText)
        {
            string usingStatement = "using " + namespaceText + ";";

            StringEditor stringEditor = new StringEditor(fileData);

            // Find line if it is not the first line
            if (stringEditor.GetLine().CompareTo(usingStatement) < 0)
            {
                // Find line for using-Statement
                stringEditor.NextThatContains("using");
                stringEditor.Prev();
                stringEditor.Next(line =>
                {
                    string lineComparer = line.Replace(".", "").Replace(";", "");
                    string usingStatementComparer = usingStatement.Replace(".", "").Replace(";", "");
                    return lineComparer.CompareTo(usingStatementComparer) >= 0 ||
                        (!line.Contains("using") && IsLineEmpty(line));
                });
            }

            // Only add line if not already added
            if (!stringEditor.GetLine().Equals(usingStatement))
            {
                stringEditor.InsertLine(usingStatement);
            }

            return stringEditor.GetText();
        }

        private bool IsLineEmpty(string line)
        {
            return line.Trim().Length == 0;
        }
    }
}