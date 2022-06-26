using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Frontend.Pages
{
    internal class EntitiesPageTsPropertyAddition : PropertyAdditionToExisitingFileGeneration
    {
        public EntitiesPageTsPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Property property, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("GridColumns: string[]");
            stringEditor.NextThatContains("'detail'");

            stringEditor.InsertLine($"    '{property.Name.LowerFirstChar()}',");

            return stringEditor.GetText();
        }
    }
}