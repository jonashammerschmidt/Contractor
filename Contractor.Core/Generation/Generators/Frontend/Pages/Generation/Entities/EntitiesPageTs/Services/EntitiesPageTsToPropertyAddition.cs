using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Frontend.Pages
{
    public class EntitiesPageTsToPropertyAddition : RelationSideAdditionToExisitingFileGeneration
    {
        public EntitiesPageTsToPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            fileData = ImportStatements.Add(fileData, "DropdownDataSource",
                "@core-app/components/ui/dropdown-data-source/dropdown-pagination-data-source");

            fileData = ImportStatements.Add(fileData, $"{relationSide.OtherEntity.NamePlural}CrudService",
                $"@core-app/model/{relationSide.OtherEntity.Module.NameKebab}" +
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

            stringEditor.NextThatContains("TableDataSource");
            stringEditor.NextThatContains("AddContainsFilters");
            stringEditor.NextThatContains("});");
            stringEditor.InsertLine($"        .AddEqualsFilters('{relationSide.NameLower}Id', () => this.{relationSide.NameLower}SelectedValues)");

            return stringEditor.GetText();
        }

        private static string GetDataSourceLine(RelationSide relationSide)
        {
            return
                $"  {relationSide.NameLower}SelectedValues = [];\n" +
                $"  {relationSide.NameLower}DataSource = new DropdownDataSource({{\n" +
                $"    filterField: '{relationSide.OtherEntity.DisplayProperty.NameLower}',\n" +
                $"    getPagedData: (options) => this.{relationSide.OtherEntity.NamePluralLower}CrudService.getPaged{relationSide.OtherEntity.NamePlural}(options),\n" +
                $"  }});";
        }
    }
}