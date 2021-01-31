using Contractor.Core.Helpers;
using Contractor.Core.Jobs.DomainAddition;
using System.IO;

namespace Contractor.Core.Tools
{
    public class DtoPropertyAddition
    {
        public PathService pathService;

        public DtoPropertyAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void AddPropertyToDTO(PropertyOptions options, string domainFolder, string templateFileName)
        {
            AddPropertyToDTO(options, domainFolder, templateFileName, false);
        }

        public void AddPropertyToDTO(PropertyOptions options, string domainFolder, string templateFileName, bool forInterface)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath, forInterface);

            File.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(PropertyOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForDTOs(options, domainFolder);
            string fileName = templateFileName.Replace("Entity", options.EntityName);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(PropertyOptions options, string filePath, bool forInterface)
        {
            string fileData = File.ReadAllText(filePath);

            fileData = AddProperty(fileData, options.PropertyType, options.PropertyName, forInterface);

            return fileData;
        }

        private string AddProperty(string file, string propertyType, string propertyName, bool forInterface)
        {
            StringEditor stringEditor = new StringEditor(file);
            stringEditor.NextThatContains("{")
                      .NextThatContains("{")
                      .Next(line => !IsLineEmpty(line) && !ContainsProperty(line));

            if (!stringEditor.GetLine().Contains("}"))
            {
                stringEditor.Prev();
            }

            if (ContainsProperty(file))
            {
                stringEditor.InsertNewLine();
            }

            if (forInterface)
                stringEditor.InsertLine(GetInterfaceProperty(propertyType, propertyName));
            else
                stringEditor.InsertLine(GetProperty(propertyType, propertyName));

            return stringEditor.GetText();
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