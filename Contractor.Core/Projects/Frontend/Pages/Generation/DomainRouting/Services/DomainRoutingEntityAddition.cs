using Contractor.Core.Helpers;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class DomainRoutingEntityAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public DomainRoutingEntityAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Add(Entity entity, string domainFolder, string templateFileName)
        {
            string filePath = this.pathService.GetAbsolutePathForFrontend(entity, domainFolder, templateFileName);
            string fileData = UpdateFileData(entity, filePath);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string UpdateFileData(Entity entity, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(entity, filePath);

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("const routes: Routes = [");
            stringEditor.NextThatContains("];");

            stringEditor.InsertLine(GetAppRoutingLine(entity));

            return stringEditor.GetText();
        }

        private string GetAppRoutingLine(Entity entity)
        {
            return
              "  {\n" +
             $"    path: '{entity.NamePluralKebab}',\n" +
             $"    loadChildren: () => import('./{entity.NamePluralKebab}/{entity.NamePluralKebab}-pages.module')\n" +
             $"      .then(m => m.{entity.NamePlural}PagesModule)\n" +
              "  },";
        }
    }
}