using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;
using System.Text.RegularExpressions;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntitiesPageTsToPropertyAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public EntitiesPageTsToPropertyAddition(
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

            fileData = ImportStatements.Add(fileData, $"{options.EntityNamePluralFrom}CrudService",
                $"src/app/model/{StringConverter.PascalToKebabCase(options.DomainFrom)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralFrom)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralFrom)}-crud.service");

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(IRelationAdditionOptions options, string domainFolder, string templateFileName)
        {
            IEntityAdditionOptions entityOptions = RelationAdditionOptions.GetPropertyForTo(options);
            string absolutePathForDomain = this.pathService.GetAbsolutePathForFrontend(entityOptions, domainFolder);
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

            stringEditor.NextThatContains("// Table");
            stringEditor.InsertLine(GetDataSourceLine(options));
            stringEditor.InsertNewLine();

            stringEditor.NextThatContains("GridColumns: string[]");
            stringEditor.NextThatContains("'detail'");
            stringEditor.InsertLine($"    '{options.PropertyNameFrom.LowerFirstChar()}',");

            string constructorLine = $"    private {options.EntityNamePluralLowerFrom}CrudService: {options.EntityNamePluralFrom}CrudService,";
            stringEditor.NextThatContains("constructor(");
            if (!fileData.Contains(constructorLine))
            {
                stringEditor.Next();
                stringEditor.InsertLine(constructorLine);
            }

            stringEditor.NextThatContains("PaginationDataSource");
            stringEditor.NextThatContains("() => [");
            stringEditor.NextThatContains("]);");
            stringEditor.InsertLine("        {");
            stringEditor.InsertLine($"          filterField: '" + options.PropertyNameFrom.LowerFirstChar() + "Id',");
            stringEditor.InsertLine($"          equalsFilters: this.{options.PropertyNameFrom.LowerFirstChar()}SelectedValues");
            stringEditor.InsertLine("        },");

            return stringEditor.GetText();
        }

        private static string GetDataSourceLine(IRelationAdditionOptions options)
        {
            return 
                $"  {options.PropertyNameFrom.LowerFirstChar()}SelectedValues = [];\n" +
                $"  {options.PropertyNameFrom.LowerFirstChar()}DataSource = new DropdownPaginationDataSource(\n" +
                $"    (options) => this.{options.EntityNamePluralLowerFrom}CrudService.getPaged{options.EntityNamePluralFrom}(options),\n" +
                 "    'bezeichnung');";
        }

    }
}