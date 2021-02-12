using Contractor.Core.Helpers;
using Contractor.Core.Options;
using System.IO;
using System.Text.RegularExpressions;

namespace Contractor.Core.Tools
{
    internal class DtoPropertyAddition
    {
        public PathService pathService;

        public DtoPropertyAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void AddPropertyToDTO(IPropertyAdditionOptions options, string domainFolder, string templateFileName)
        {
            AddPropertyToDTO(options, domainFolder, templateFileName, false);
        }

        public void AddPropertyToDTO(IPropertyAdditionOptions options, string domainFolder, string templateFileName, string namespaceToAdd)
        {
            AddPropertyToDTO(options, domainFolder, templateFileName, false, namespaceToAdd);
        }

        public void AddPropertyToDTO(IPropertyAdditionOptions options, string domainFolder, string templateFileName, bool forInterface)
        {
            AddPropertyToDTO(options, domainFolder, templateFileName, forInterface, null);
        }

        public void AddPropertyToDTO(IPropertyAdditionOptions options, string domainFolder, string templateFileName, bool forInterface, string namespaceToAdd)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath, forInterface);

            if (namespaceToAdd != null)
            {
                fileData = UsingStatements.Add(fileData, namespaceToAdd);
            }

            CsharpClassWriter.Write(filePath, fileData);
        }

        private string GetFilePath(IPropertyAdditionOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForDTOs(options, domainFolder);
            string fileName = templateFileName.Replace("Entity", options.EntityName);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(IPropertyAdditionOptions options, string filePath, bool forInterface)
        {
            string fileData = File.ReadAllText(filePath);

            fileData = AddUsingStatements(options, fileData);
            fileData = AddProperty(fileData, options, forInterface);

            return fileData;
        }

        private string AddUsingStatements(IPropertyAdditionOptions options, string fileData)
        {
            if (options.PropertyType.Contains("Guid") || options.PropertyType.Contains("DateTime"))
            {
                fileData = UsingStatements.Add(fileData, "System");
            }

            if (options.PropertyType.Contains("Enumerable"))
            {
                fileData = UsingStatements.Add(fileData, "System.Collections.Generic");
            }

            return fileData;
        }

        private string AddProperty(string file, IPropertyAdditionOptions options, bool forInterface)
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

            if (forInterface)
                stringEditor.InsertLine(GetInterfaceProperty(options.PropertyType, options.PropertyName));
            else
                stringEditor.InsertLine(GetProperty(options.PropertyType, options.PropertyName));

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
            return line.Replace(" ", "").Contains("{get;set;}");
        }

        private string GetProperty(string propertyType, string propertyName)
        {
            return $"        public " + propertyType + " " + propertyName + " { get; set; }";
        }

        private string GetInterfaceProperty(string propertyType, string propertyName)
        {
            return $"        " + propertyType + " " + propertyName + " { get; set; }";
        }
    }
}