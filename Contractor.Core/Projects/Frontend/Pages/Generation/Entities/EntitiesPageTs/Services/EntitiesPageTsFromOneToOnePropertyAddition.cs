using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntitiesPageTsFromOneToOnePropertyAddition : FrontendRelationAdditionEditor
    {
        public EntitiesPageTsFromOneToOnePropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.From)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            fileData = ImportStatements.Add(fileData, "DropdownPaginationDataSource",
                "src/app/components/ui/dropdown-data-source/dropdown-pagination-data-source");

            fileData = ImportStatements.Add(fileData, $"{options.EntityNamePluralTo}CrudService",
                $"src/app/model/{StringConverter.PascalToKebabCase(options.DomainTo)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralTo)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralTo)}-crud.service");

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("GridColumns: string[]");
            stringEditor.NextThatContains("'detail'");
            stringEditor.InsertLine($"    '{options.PropertyNameTo.LowerFirstChar()}',");

            return stringEditor.GetText();
        }
    }
}