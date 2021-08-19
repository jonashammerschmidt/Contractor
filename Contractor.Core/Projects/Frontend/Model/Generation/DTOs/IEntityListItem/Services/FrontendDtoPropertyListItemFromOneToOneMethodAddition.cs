using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Model
{
    internal class FrontendDtoPropertyListItemFromOneToOneMethodAddition
    {
        public PathService pathService;

        public FrontendDtoPropertyListItemFromOneToOneMethodAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void AddPropertyToDTO(IRelationAdditionOptions options, string domainFolder, string fileName)
        {
            string filePath = GetFilePath(options, domainFolder, fileName);
            string fileData = UpdateFileData(options, filePath);

            TypescriptClassWriter.Write(filePath, fileData);
        }

        private string GetFilePath(IRelationAdditionOptions options, string domainFolder, string fileName)
        {
            IEntityAdditionOptions entityOptions = RelationAdditionOptions.GetPropertyForFrom(options);
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForFrontendModel(entityOptions, domainFolder);
            fileName = fileName.Replace("entity-kebab", StringConverter.PascalToKebabCase(entityOptions.EntityName));
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(IRelationAdditionOptions options, string filePath)
        {
            string fileData = File.ReadAllText(filePath);

            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"public static fromApi{options.EntityNameFrom}ListItem");
            stringEditor.NextThatContains("return {");
            stringEditor.NextThatContains("};");

            stringEditor.InsertLine($"            {options.PropertyNameTo.LowerFirstChar()}: {options.EntityNameTo}.fromApi{options.EntityNameFrom}(api{options.EntityNameTo}ListItem.{options.PropertyNameTo.LowerFirstChar()}),");

            return stringEditor.GetText();
        }
    }
}