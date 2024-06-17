using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Frontend.Pages
{
    public class EntitiesPageTsFromOneToOnePropertyAddition : RelationSideAdditionToExisitingFileGeneration
    {
        public EntitiesPageTsFromOneToOnePropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            fileData = ImportStatements.Add(fileData, "DropdownDataSource",
                "@core-app/components/ui/dropdown-data-source/dropdown-pagination-data-source");

            fileData = ImportStatements.Add(fileData, $"{relationSide.Entity.NamePlural}CrudService",
                $"@core-app/model/{relationSide.Entity.Module.NameKebab}" +
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