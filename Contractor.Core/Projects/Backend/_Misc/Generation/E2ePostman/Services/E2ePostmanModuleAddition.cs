using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Misc
{
    internal class E2ePostmanModuleAddition : ModuleAdditionToExisitingFileGeneration
    {
        public E2ePostmanModuleAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(Module module, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatStartsWith("\t\"item\": [");
            stringEditor.NextThatStartsWith("\t],");

            stringEditor.Prev();
            stringEditor.InsertIntoLine(",");
            stringEditor.Next();

            stringEditor.InsertLine(
                 "\t\t{\n" +
                $"\t\t\t\"name\": \"{module.Name}\",\n" +
                 "\t\t\t\"item\": [\n" +
                 "\t\t\t]\n" +
                 "\t\t}");

            return stringEditor.GetText();
        }
    }
}