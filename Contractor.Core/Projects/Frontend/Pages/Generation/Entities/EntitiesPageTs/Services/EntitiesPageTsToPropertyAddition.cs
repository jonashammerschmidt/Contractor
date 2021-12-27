using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntitiesPageTsToPropertyAddition : RelationAdditionEditor
    {
        public EntitiesPageTsToPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.To)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            fileData = ImportStatements.Add(fileData, "DropdownPaginationDataSource",
                "src/app/components/ui/dropdown-data-source/dropdown-pagination-data-source");

            fileData = ImportStatements.Add(fileData, $"{options.EntityNamePluralFrom}CrudService",
                $"src/app/model/{StringConverter.PascalToKebabCase(options.DomainFrom)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralFrom)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralFrom)}-crud.service");

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