using Contractor.Core.Helpers;
using Contractor.Core.Options;
using System.IO;

namespace Contractor.Core.Tools
{
    internal class FrontendDtoRelationAddition
    {
        public PathService pathService;

        public FrontendDtoRelationAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void AddPropertyToDTO(IRelationSideAdditionOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            TypescriptClassWriter.Write(filePath, fileData);
        }

        public void AddPropertyToDTO(IRelationSideAdditionOptions options, string domainFolder, string templateFileName, string importStatementTypes, string importStatementPath)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            fileData = ImportStatements.Add(fileData, importStatementTypes, importStatementPath);

            TypescriptClassWriter.Write(filePath, fileData);
        }

        private string GetFilePath(IRelationSideAdditionOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForFrontendModel(options, domainFolder);
            string fileName = templateFileName.Replace("entity-kebab", StringConverter.PascalToKebabCase(options.EntityName));
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(IRelationSideAdditionOptions options, string filePath)
        {
            string fileData = File.ReadAllText(filePath);

            StringEditor stringEditor = new StringEditor(fileData);
            if (!stringEditor.GetLine().Contains("export interface"))
            {
                stringEditor.NextThatContains($"export interface");
            }
            stringEditor.NextThatContains("}");

            stringEditor.InsertLine($"    {options.PropertyName.LowerFirstChar()}: {options.PropertyType};");

            return stringEditor.GetText();
        }
    }
}