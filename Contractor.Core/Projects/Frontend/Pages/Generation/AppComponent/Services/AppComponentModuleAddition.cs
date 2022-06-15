using Contractor.Core.Helpers;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class AppComponentModuleAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public AppComponentModuleAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Add(Module module)
        {
            string filePath = Path.Combine(module.Options.Paths.FrontendDestinationFolder, "src\\app\\app.component.ts");
            string fileData = UpdateFileData(module, filePath);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string UpdateFileData(Module module, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(module, filePath);

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains(" addMainMenuItems(");
            stringEditor.NextThatContains("if (permission.benutzerVerwalten === 1");
            stringEditor.Prev();

            stringEditor.InsertNewLine();
            stringEditor.InsertLine(
                 "    this.mainMenu.push({\n" +
                $"      text: '{module.Name}',\n" +
                $"      routerLink: '/{module.NameKebab}',\n" +
                 "      subMenu: [\n" +
                 "      ]\n" +
                 "    });");

            return stringEditor.GetText();
        }
    }
}