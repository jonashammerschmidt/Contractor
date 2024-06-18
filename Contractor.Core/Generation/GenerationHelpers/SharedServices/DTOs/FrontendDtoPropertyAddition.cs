using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;

namespace Contractor.Core.Tools
{
    public class FrontendDtoPropertyAddition : PropertyAdditionToExisitingFileGeneration
    {
        public FrontendDtoPropertyAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Property property, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);
            if (!stringEditor.GetLine().Contains("export interface"))
            {
                stringEditor.NextThatStartsWith($"export interface");
            }
            stringEditor.NextThatContains("}");

            stringEditor.InsertLine(FrontendDtoPropertyLine.GetPropertyLine(property));

            return stringEditor.GetText();
        }
    }
}