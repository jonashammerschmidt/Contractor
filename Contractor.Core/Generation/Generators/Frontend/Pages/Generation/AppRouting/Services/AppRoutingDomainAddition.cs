using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Frontend.Pages
{
    public class AppRoutingDomainAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public AppRoutingDomainAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Add(Module module)
        {
            string filePath = Path.Combine(module.Options.Paths.FrontendDestinationFolder, "src", "app", "app-routing.module.ts");
            string fileData = UpdateFileData(module, filePath);

            this.fileSystemClient.WriteAllText(fileData, filePath);
        }

        private string UpdateFileData(Module module, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(module, filePath);

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("path: '**',");
            stringEditor.Prev();

            stringEditor.InsertLine(GetAppRoutingLine(module));

            return stringEditor.GetText();
        }

        private string GetAppRoutingLine(Module module)
        {
            return
              "  {\n" +
             $"    path: '{module.Name.ToKebab()}',\n" +
             $"    loadChildren: () => import('./pages/{module.Name.ToKebab()}/{module.Name.ToKebab()}-pages.module')\n" +
             $"      .then(m => m.{module.Name}PagesModule)\n" +
              "  },";
        }
    }
}