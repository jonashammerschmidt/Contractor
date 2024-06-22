using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Frontend.Pages
{
    public class AppComponentModuleAddition
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
            string filePath = Path.Combine(module.Options.Paths.FrontendDestinationFolder, "src", "app", "app.component.ts");
            string fileData = UpdateFileData(module, filePath);

            this.fileSystemClient.WriteAllText(fileData, filePath);
        }

        private string UpdateFileData(Module module, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(module, filePath);

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("this.mainMenu[1].menu.push");

            stringEditor.InsertLine(
                 "    this.mainMenu[0].menu.push({\n" +
                $"      text: '{module.Name}',\n" +
                $"      routerLink: '/{module.NameKebab}',\n" +
                 "      subMenu: [\n" +
                 "      ]\n" +
                 "    });");
            stringEditor.InsertNewLine();

            return stringEditor.GetText();
        }
    }
}