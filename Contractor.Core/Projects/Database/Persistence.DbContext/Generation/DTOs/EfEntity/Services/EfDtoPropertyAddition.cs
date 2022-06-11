using Contractor.Core.Helpers;
using Contractor.Core.Options;
using System.IO;
using System.Text.RegularExpressions;

namespace Contractor.Core.Tools
{
    internal class EfDtoPropertyAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public EfDtoPropertyAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void AddPropertyToDTO(Property property, string domainFolder, string templateFileName)
        {
            string filePath = this.pathService.GetAbsolutePathForDatabase(property, domainFolder, templateFileName);
            string fileData = UpdateFileData(property, filePath);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string UpdateFileData(Property property, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(property, filePath);

            fileData = AddUsingStatements(property, fileData);
            fileData = AddProperty(fileData, property);

            return fileData;
        }

        private string AddUsingStatements(Property property, string fileData)
        {
            if (property.Type == PropertyTypes.Guid || property.Type == PropertyTypes.DateTime)
            {
                fileData = UsingStatements.Add(fileData, "System");
            }

            return fileData;
        }

        private string AddProperty(string file, Property property)
        {
            StringEditor stringEditor = new StringEditor(file);
            FindStartingLineForNewProperty(file, property, stringEditor);

            if (!stringEditor.GetLine().Contains("}"))
            {
                stringEditor.Prev();
            }

            if (ContainsProperty(file))
            {
                stringEditor.InsertNewLine();
            }

            stringEditor.InsertLine(DatabaseEfDtoPropertyLine.GetPropertyLine(property));

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
            stringEditor.Next(line => !IsLineEmpty(line) && !ContainsProperty(line));
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