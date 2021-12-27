using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntitiesPagesModuleToRelationAddition : FrontendRelationAdditionEditor
    {
        public EntitiesPagesModuleToRelationAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.To)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
            if (fileData.Contains($"{options.EntityNamePluralFrom}Module,"))
            {
                return fileData;
            }

            fileData = ImportStatements.Add(fileData, $"{options.EntityNamePluralFrom}Module",
                $"src/app/model/{StringConverter.PascalToKebabCase(options.DomainFrom)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralFrom)}" +
                $"/{StringConverter.PascalToKebabCase(options.EntityNamePluralFrom)}.module");

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains($"    {options.EntityNamePluralTo}Module,");
            stringEditor.Next();

            stringEditor.InsertLine($"    {options.EntityNamePluralFrom}Module,");

            return stringEditor.GetText();
        }
    }
}