using Contractor.Core.Helpers;
using Contractor.Core.Options;
using System.IO;

namespace Contractor.Core.Tools
{
    internal class FrontendDtoPropertyDefaultAddition
    {
        public PathService pathService;

        public FrontendDtoPropertyDefaultAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void AddPropertyToDTO(IPropertyAdditionOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

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
            stringEditor.NextThatContains($"export const {options.EntityNameLower}CreateDefault");
            stringEditor.NextThatContains("};");

            stringEditor.InsertLine(GetPropertyLine(options));

            return stringEditor.GetText();
        }

        private static string GetPropertyLine(IPropertyAdditionOptions options)
        {
            // TODO: PropertyName length determines spaces betweeen name and type
            if (options.PropertyType == "string")
            {
                return $"    {options.PropertyName.LowerFirstChar()}: '',";
            }
            else if (options.PropertyType == "int")
            {
                return $"    {options.PropertyName.LowerFirstChar()}: 0,";
            }
            else if (options.PropertyType == "Guid")
            {
                return $"    {options.PropertyName.LowerFirstChar()}: null,";
            }
            else if (options.PropertyType == "Guid?")
            {
                return $"    {options.PropertyName.LowerFirstChar()}?: null,";
            }
            else if (options.PropertyType == "bool")
            {
                return $"    {options.PropertyName.LowerFirstChar()}: false,";
            }
            else if (options.PropertyType == "DateTime")
            {
                return $"    {options.PropertyName.LowerFirstChar()}: new Date(),";
            }

            return $"    {options.PropertyName.LowerFirstChar()}: null,";
        }
    }
}