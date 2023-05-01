using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Frontend.Pages
{
    internal class EntityDetailPageTsPropertyAddition : PropertyAdditionToExisitingFileGeneration
    {
        public EntityDetailPageTsPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Property property, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("this.formBuilder.group({");
            stringEditor.NextThatContains("});");
            stringEditor.InsertLine(FrontendFormBuilderPropertyLine.GetPropertyLine(property));

            return stringEditor.GetText();
        }
    }
}