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

            stringEditor.InsertLine(FrontendDtoDefaultPropertyLine.GetPropertyLine(options));

            return stringEditor.GetText();
        }
    }
}