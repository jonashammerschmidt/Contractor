using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using System.IO;
using System.Text.RegularExpressions;

namespace Contractor.Core.Tools
{
    internal class DtoPropertyAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public DtoPropertyAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void AddPropertyToDTO(Property property, string domainFolder, string templateFileName)
        {
            AddPropertyToDTO(property, domainFolder, templateFileName, false);
        }

        public void AddPropertyToDTO(Property property, string domainFolder, string templateFileName, bool forInterface)
        {
            AddPropertyToDTO(property, domainFolder, templateFileName, forInterface, false);
        }

        public void AddPropertyToDTO(Property property, string domainFolder, string templateFileName, bool forInterface, bool forDatabase)
        {
            string filePath = (forDatabase) ?
                this.pathService.GetAbsolutePathForDatabase(property, domainFolder, templateFileName) :
                this.pathService.GetAbsolutePathForBackend(property, domainFolder, templateFileName);

            string fileData = UpdateFileData(property, filePath, forInterface);

            this.fileSystemClient.WriteAllText(fileData, filePath);
        }

        private string UpdateFileData(Property property, string filePath, bool forInterface)
        {
            string fileData = this.fileSystemClient.ReadAllText(property, filePath);

            fileData = AddUsingStatements(property, fileData);
            fileData = AddProperty(fileData, property, forInterface);

            return fileData;
        }

        private string AddUsingStatements(Property property, string fileData)
        {
            if (property.Type == PropertyType.Guid || property.Type == PropertyType.DateTime)
            {
                fileData = UsingStatements.Add(fileData, "System");
            }

            return fileData;
        }

        private string AddProperty(string file, Property property, bool forInterface)
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

            if (forInterface)
                stringEditor.InsertLine(BackendDtoInterfacePropertyLine.GetPropertyLine(property));
            else
                stringEditor.InsertLine(BackendDtoPropertyLine.GetPropertyLine(property));

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