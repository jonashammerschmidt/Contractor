using Contractor.Core.Helpers;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntitiesPageTsFromOneToOnePropertyAddition : FrontendRelationAdditionEditor
    {
        public EntitiesPageTsFromOneToOnePropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            fileData = ImportStatements.Add(fileData, "DropdownPaginationDataSource",
                "src/app/components/ui/dropdown-data-source/dropdown-pagination-data-source");

            fileData = ImportStatements.Add(fileData, $"{relationSide.Entity.NamePlural}CrudService",
                $"src/app/model/{relationSide.Entity.Module.NameKebab}" +
                $"/{relationSide.Entity.NamePluralKebab}" +
                $"/{relationSide.Entity.NamePluralKebab}-crud.service");

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("GridColumns: string[]");
            stringEditor.NextThatContains("'detail'");
            stringEditor.InsertLine($"    '{relationSide.NameLower}',");

            return stringEditor.GetText();
        }
    }
}