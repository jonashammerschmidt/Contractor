using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Options;
using System.IO;
using System.Text.RegularExpressions;

namespace Contractor.Core.Tools
{
    internal class ApiDtoPropertyAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public ApiDtoPropertyAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void AddPropertyToDTO(Property property, string domainFolder, string templateFileName)
        {
            string filePath = this.pathService.GetAbsolutePathForBackend(property, domainFolder, templateFileName);
            string fileData = UpdateFileData(property, filePath);

            this.fileSystemClient.WriteAllText(fileData, filePath);
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

            fileData = UsingStatements.Add(fileData, "System.ComponentModel.DataAnnotations");

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

            if (!property.IsOptional)
            {
                stringEditor.InsertLine("        [Required]");
            }

            if (property.Type == PropertyTypes.String && property.TypeExtra != null && property.IsOptional)
            {
                stringEditor.InsertLine($"        [StringLength({property.TypeExtra})]");
            }

            if (property.Type == PropertyTypes.String && property.TypeExtra != null && !property.IsOptional)
            {
                stringEditor.InsertLine($"        [StringLength({property.TypeExtra}, MinimumLength = 1)]");
            }

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
            return line.Replace(" ", "").Contains("{get;set;}") || line.Contains("[");
        }
    }
}