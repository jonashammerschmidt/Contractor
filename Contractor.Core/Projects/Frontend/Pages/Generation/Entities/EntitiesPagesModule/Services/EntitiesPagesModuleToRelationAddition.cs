using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntitiesPagesModuleToRelationAddition : RelationAdditionEditor
    {
        public EntitiesPagesModuleToRelationAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.To)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains($"    {options.EntityNamePluralTo}Module,");
            stringEditor.Next();

            stringEditor.InsertLine($"    {options.EntityNamePluralFrom}Module,");

            return stringEditor.GetText();
        }
    }
}