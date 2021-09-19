using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Model
{
    internal class FrontendDtoPropertyFromMethodAddition
    {
        public FileSystemClient fileSystemClient;
        public PathService pathService;

        public FrontendDtoPropertyFromMethodAddition(
            FileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void AddPropertyToDTO(IRelationAdditionOptions options, string domainFolder, string fileName)
        {
            string filePath = GetFilePath(options, domainFolder, fileName);
            string fileData = UpdateFileData(options, filePath);

            this.fileSystemClient.WriteAllText(filePath, fileData);
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
            string fileData = this.fileSystemClient.ReadAllText(filePath);

            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"public static fromApi{options.EntityNameFrom}Detail");
            stringEditor.NextThatContains("return {");
            stringEditor.NextThatContains("};");

            stringEditor.InsertLine($"            {options.PropertyNameTo.LowerFirstChar()}: api{options.EntityNameFrom}Detail.{options.PropertyNameTo.LowerFirstChar()}\n");
            stringEditor.InsertLine($"                .map(api{options.EntityNameTo} => {options.EntityNameTo}.fromApi{options.EntityNameTo}(api{options.EntityNameTo})),");

            return stringEditor.GetText();
        }
    }
}