using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Frontend.Pages
{
    public class EntitiesPagesModuleToRelationAddition : RelationSideAdditionToExisitingFileGeneration
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
                $"@core-app/model/{relationSide.OtherEntity.Module.NameKebab}" +
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