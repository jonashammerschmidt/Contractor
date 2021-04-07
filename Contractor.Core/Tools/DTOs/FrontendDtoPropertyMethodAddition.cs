using Contractor.Core.Helpers;
using Contractor.Core.Options;
using System.IO;

namespace Contractor.Core.Tools
{
    internal class FrontendDtoPropertyMethodAddition
    {
        public PathService pathService;

        public FrontendDtoPropertyMethodAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void AddPropertyToDTO(IPropertyAdditionOptions options, string functionName, string variableName, string domainFolder, string fileName)
        {
            functionName = functionName.Replace("Entity", options.EntityName);
            functionName = functionName.Replace("entity", options.EntityName.LowerFirstChar());

            variableName = variableName.Replace("Entity", options.EntityName);
            variableName = variableName.Replace("entity", options.EntityName.LowerFirstChar());

            string filePath = GetFilePath(options, domainFolder, fileName);
            string fileData = UpdateFileData(options, functionName, variableName, filePath);

            TypescriptClassWriter.Write(filePath, fileData);
        }

        private string GetFilePath(IPropertyAdditionOptions options, string domainFolder, string fileName)
        {
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForFrontendModel(options, domainFolder);
            fileName = fileName.Replace("entity-kebab", StringConverter.PascalToKebabCase(options.EntityName));
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(IPropertyAdditionOptions options, string functionName, string variableName, string filePath)
        {
            string fileData = File.ReadAllText(filePath);

            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"public static " + functionName);
            stringEditor.NextThatContains("return {");
            stringEditor.NextThatContains("};");

            stringEditor.InsertLine($"            {options.PropertyName.LowerFirstChar()}: {variableName}.{options.PropertyName.LowerFirstChar()},");

            return stringEditor.GetText();
        }
    }
}