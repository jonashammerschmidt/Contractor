using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class AppRoutingDomainAddition
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

        public void Add(IDomainAdditionOptions options)
        {
            string filePath = this.pathService.GetAbsolutePathForFrontendAppRouting(options);
            string fileData = UpdateFileData(options, filePath);

            this.fileSystemClient.WriteAllText(filePath, fileData, options);
        }

        private string UpdateFileData(IDomainAdditionOptions options, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(filePath);

            StringEditor stringEditor = new StringEditor(fileData);

            // ----------- DbSet -----------
            stringEditor.NextThatContains("path: '**',");
            stringEditor.Prev();

            stringEditor.InsertLine(GetAppRoutingLine(options));

            return stringEditor.GetText();
        }

        private string GetAppRoutingLine(IDomainAdditionOptions options)
        {
            return
              "  {\n" +
             $"    path: '{options.Domain.ToKebab()}',\n" +
             $"    loadChildren: () => import('./pages/{options.Domain.ToKebab()}/{options.Domain.ToKebab()}-pages.module')\n" +
             $"      .then(m => m.{options.Domain}PagesModule)\n" +
              "  },";
        }
    }
}