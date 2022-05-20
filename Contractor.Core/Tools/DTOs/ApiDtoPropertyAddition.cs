using Contractor.Core.Helpers;
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

        public void AddPropertyToDTO(IPropertyAdditionOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(IPropertyAdditionOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForBackend(options, domainFolder);
            string fileName = templateFileName.Replace("Entity", options.EntityName);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(IPropertyAdditionOptions options, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(filePath);

            fileData = AddUsingStatements(options, fileData);
            fileData = AddProperty(fileData, options);

            return fileData;
        }

        private string AddUsingStatements(IPropertyAdditionOptions options, string fileData)
        {
            if (options.PropertyType == PropertyTypes.Guid || options.PropertyType == PropertyTypes.DateTime)
            {
                fileData = UsingStatements.Add(fileData, "System");
            }

            fileData = UsingStatements.Add(fileData, "System.ComponentModel.DataAnnotations");

            return fileData;
        }

        private string AddProperty(string file, IPropertyAdditionOptions options)
        {
            StringEditor stringEditor = new StringEditor(file);
            FindStartingLineForNewProperty(file, options, stringEditor);

            if (!stringEditor.GetLine().Contains("}"))
            {
                stringEditor.Prev();
            }

            if (ContainsProperty(file))
            {
                stringEditor.InsertNewLine();
            }

            if (!options.IsOptional)
            {
                stringEditor.InsertLine("        [Required]");
            }

            if (options.PropertyType == PropertyTypes.String && options.PropertyTypeExtra != null && options.IsOptional)
            {
                stringEditor.InsertLine($"        [StringLength({options.PropertyTypeExtra})]");
            }

            if (options.PropertyType == PropertyTypes.String && options.PropertyTypeExtra != null && !options.IsOptional)
            {
                stringEditor.InsertLine($"        [StringLength({options.PropertyTypeExtra}, MinimumLength = 1)]");
            }

            stringEditor.InsertLine(BackendDtoPropertyLine.GetPropertyLine(options));

            return stringEditor.GetText();
        }

        private void FindStartingLineForNewProperty(string file, IPropertyAdditionOptions options, StringEditor stringEditor)
        {
            bool hasConstructor = Regex.IsMatch(file, $"public .*{options.EntityName}.*\\(");
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
            return line.Replace(" ", "").Contains("{get;set;}") || line.Contains("[");
        }
    }
}