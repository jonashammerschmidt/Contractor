using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using System.Text.RegularExpressions;

namespace Contractor.Core.Tools
{
    internal class DtoInterfacePropertyAddition : PropertyAdditionToExisitingFileGeneration
    {
        public DtoInterfacePropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Property property, string fileData)
        {
            if (property.Type == PropertyType.Guid || property.Type == PropertyType.DateTime)
            {
                fileData = UsingStatements.Add(fileData, "System");
            }

            StringEditor stringEditor = new StringEditor(fileData);
            FindStartingLineForNewProperty(fileData, property, stringEditor);

            if (!stringEditor.GetLine().Contains("}"))
            {
                stringEditor.Prev();
            }

            if (ContainsProperty(fileData))
            {
                stringEditor.InsertNewLine();
            }

            stringEditor.InsertLine(BackendDtoInterfacePropertyLine.GetPropertyLine(property));
            
            return stringEditor.GetText();
        }

        private void FindStartingLineForNewProperty(string file, Property property, StringEditor stringEditor)
        {
            bool hasConstructor = Regex.IsMatch(file, $"public .*{property.Entity.Name}.*\\(");
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
            stringEditor.NextUntil(line => !IsLineEmpty(line) && !ContainsProperty(line));
        }

        private bool IsLineEmpty(string line)
        {
            return line.Trim().Length == 0;
        }

        private bool ContainsProperty(string line)
        {
            return line.Replace(" ", "").Contains("{get;set;}");
        }
    }
}