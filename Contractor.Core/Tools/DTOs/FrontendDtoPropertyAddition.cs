using Contractor.Core.Helpers;
using Contractor.Core.Options;
using System.IO;

namespace Contractor.Core.Tools
{
    internal class FrontendDtoPropertyAddition
    {
        public PathService pathService;

        public FrontendDtoPropertyAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void AddPropertyToDTO(IPropertyAdditionOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            TypescriptClassWriter.Write(filePath, fileData);
        }

        public void AddPropertyToDTO(IPropertyAdditionOptions options, string domainFolder, string templateFileName, string importStatementTypes, string importStatementPath)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            fileData = ImportStatements.Add(fileData, importStatementTypes, importStatementPath);

            TypescriptClassWriter.Write(filePath, fileData);
        }

        private string GetFilePath(IPropertyAdditionOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForFrontendModel(options, domainFolder);
            string fileName = templateFileName.Replace("entity-kebab", StringConverter.PascalToKebabCase(options.EntityName));
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(IPropertyAdditionOptions options, string filePath)
        {
            string fileData = File.ReadAllText(filePath);

            StringEditor stringEditor = new StringEditor(fileData);
            if (!stringEditor.GetLine().Contains("export interface")) 
            { 
                stringEditor.NextThatContains($"export interface");
            }
            stringEditor.NextThatContains("}");

            stringEditor.InsertLine(GetPropertyLine(options));

            return stringEditor.GetText();
        }

        private static string GetPropertyLine(IPropertyAdditionOptions options)
        {
            // TODO: PropertyName length determines spaces betweeen name and type
            if (options.PropertyType == "string")
            {
                return $"    {options.PropertyName.LowerFirstChar()}: string;";
            }
            else if (options.PropertyType == "int")
            {
                return $"    {options.PropertyName.LowerFirstChar()}: number;";
            }
            else if (options.PropertyType == "Guid")
            {
                return $"    {options.PropertyName.LowerFirstChar()}: string;";
            }
            else if (options.PropertyType == "Guid?")
            {
                return $"    {options.PropertyName.LowerFirstChar()}?: string;";
            }
            else if (options.PropertyType == "bool")
            {
                return $"    {options.PropertyName.LowerFirstChar()}: boolean;";
            }
            else if (options.PropertyType == "DateTime")
            {
                return $"    {options.PropertyName.LowerFirstChar()}: Date;";
            }

            return $"    {options.PropertyName.LowerFirstChar()}: {options.PropertyType};";
        }
    }
}