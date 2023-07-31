using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Frontend.Pages
{
    internal class EntityCreatePageTsPropertyAddition : PropertyAdditionToExisitingFileGeneration
    {
        public EntityCreatePageTsPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Property property, string fileData)
        {
            if (property.Type == PropertyType.Integer)
            {
                fileData = ImportStatements.Add(fileData, "integerRegex",
                    "src/app/helpers/regex.helper");
            }
            if (property.Type == PropertyType.Guid)
            {
                fileData = ImportStatements.Add(fileData, "guidRegex",
                    "src/app/helpers/regex.helper");
            }

            StringEditor stringEditor = new StringEditor(fileData);
            
            stringEditor.NextThatContains($"export interface {property.Entity.Name}CreateDialogForm");
            stringEditor.NextThatContains("}");
            stringEditor.InsertLine("  " + FrontendDtoPropertyLine.GetPropertyLine(property).Trim());

            stringEditor.NextThatContains("setupFormController() {");
            stringEditor.NextThatContains("});");
            stringEditor.InsertLine(FrontendFormBuilderPropertyLine.GetPropertyLine(property));

            return stringEditor.GetText();
        }
    }
}