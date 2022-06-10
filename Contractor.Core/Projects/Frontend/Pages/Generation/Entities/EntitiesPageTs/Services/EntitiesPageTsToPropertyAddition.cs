using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntitiesPageTsToPropertyAddition : FrontendRelationAdditionEditor
    {
        public EntitiesPageTsToPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            fileData = ImportStatements.Add(fileData, "DropdownPaginationDataSource",
                "src/app/components/ui/dropdown-data-source/dropdown-pagination-data-source");

            fileData = ImportStatements.Add(fileData, $"{relationSide.OtherEntity.NamePlural}CrudService",
                $"src/app/model/{relationSide.OtherEntity.Module.NameKebab}" +
                $"/{relationSide.OtherEntity.NamePluralKebab}" +
                $"/{relationSide.OtherEntity.NamePluralKebab}-crud.service");

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("// Table");
            stringEditor.InsertLine(GetDataSourceLine(relationSide));
            stringEditor.InsertNewLine();

            stringEditor.NextThatContains("GridColumns: string[]");
            stringEditor.NextThatContains("'detail'");
            stringEditor.InsertLine($"    '{relationSide.NameLower}',");

            string constructorLine = $"    private {relationSide.OtherEntity.NamePluralLower}CrudService: {relationSide.OtherEntity.NamePlural}CrudService,";
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
            stringEditor.InsertLine($"          filterField: '" + relationSide.NameLower + "Id',");
            stringEditor.InsertLine($"          equalsFilters: this.{relationSide.NameLower}SelectedValues");
            stringEditor.InsertLine("        },");

            return stringEditor.GetText();
        }

        private static string GetDataSourceLine(RelationSide relationSide)
        {
            return 
                $"  {relationSide.NameLower}SelectedValues = [];\n" +
                $"  {relationSide.NameLower}DataSource = new DropdownPaginationDataSource(\n" +
                $"    (options) => this.{relationSide.OtherEntity.NamePluralLower}CrudService.getPaged{relationSide.OtherEntity.NamePlural}(options),\n" +
                 "    'bezeichnung');";
        }

    }
}