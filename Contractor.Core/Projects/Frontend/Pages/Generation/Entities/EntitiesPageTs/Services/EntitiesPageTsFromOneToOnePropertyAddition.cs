using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntitiesPageTsFromOneToOnePropertyAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public EntitiesPageTsFromOneToOnePropertyAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Add(IRelationAdditionOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            fileData = ImportStatements.Add(fileData, "DropdownPaginationDataSource",
                "src/app/components/ui/dropdown-data-source/dropdown-pagination-data-source");

            fileData = ImportStatements.Add(fileData, $"{options.EntityNamePluralTo}CrudService",
                $"src/app/model/{StringConverter.PascalToKebabCase(options.DomainTo)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralTo)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralTo)}-crud.service");

            this.fileSystemClient.WriteAllText(filePath, fileData, options);
        }

        private string GetFilePath(IRelationAdditionOptions options, string domainFolder, string templateFileName)
        {
            IEntityAdditionOptions entityOptions = RelationAdditionOptions.GetPropertyForFrom(options);
            string absolutePathForDomain = this.pathService.GetAbsolutePathForFrontendModel(entityOptions, domainFolder);
            string fileName = templateFileName.Replace("entities-kebab", StringConverter.PascalToKebabCase(entityOptions.EntityNamePlural));
            fileName = fileName.Replace("entity-kebab", StringConverter.PascalToKebabCase(entityOptions.EntityName));
            fileName = fileName.Replace("domain-kebab", StringConverter.PascalToKebabCase(entityOptions.Domain));
            string filePath = Path.Combine(absolutePathForDomain, fileName);
            return filePath;
        }

        private string UpdateFileData(IRelationAdditionOptions options, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(filePath);

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("GridColumns: string[]");
            stringEditor.NextThatContains("'detail'");
            stringEditor.InsertLine($"    '{options.PropertyNameTo.LowerFirstChar()}',");

            return stringEditor.GetText();
        }
    }
}