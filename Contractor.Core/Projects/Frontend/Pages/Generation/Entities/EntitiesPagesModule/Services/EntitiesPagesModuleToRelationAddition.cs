using Contractor.Core.Helpers;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntitiesPagesModuleToRelationAddition : FrontendRelationAdditionEditor
    {
        public EntitiesPagesModuleToRelationAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            if (fileData.Contains($" {relationSide.OtherEntity.NamePlural}Module,"))
            {
                return fileData;
            }

            fileData = ImportStatements.Add(fileData, $"{relationSide.OtherEntity.NamePlural}Module",
                $"src/app/model/{relationSide.OtherEntity.Module.NameKebab}" +
                $"/{relationSide.OtherEntity.NamePluralKebab}" +
                $"/{relationSide.OtherEntity.NamePluralKebab}.module");

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains($"    {relationSide.Entity.NamePlural}Module,");
            stringEditor.Next();

            stringEditor.InsertLine($"    {relationSide.OtherEntity.NamePlural}Module,");

            return stringEditor.GetText();
        }
    }
}