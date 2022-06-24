using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Frontend.Pages
{
    internal class EntityCreatePageTsPropertyAddition : PropertyAdditionToExisitingFileGeneration
    {
        public EntityCreatePageTsPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base (fileSystemClient, pathService)
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