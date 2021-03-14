using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class DomainRoutingEntityAddition
    {
        public PathService pathService;

        public DomainRoutingEntityAddition(
            PathService pathService)
        {
            this.pathService = pathService;
        }

        public void Add(IEntityAdditionOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            TypescriptClassWriter.Write(filePath, fileData);
        }

        private string GetFilePath(IEntityAdditionOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDomain = this.pathService.GetAbsolutePathForFrontendModel(options, domainFolder);
            string fileName = templateFileName.Replace("entities-kebab", StringConverter.PascalToKebabCase(options.EntityNamePlural));
            fileName = fileName.Replace("entity-kebab", StringConverter.PascalToKebabCase(options.EntityName));
            fileName = fileName.Replace("domain-kebab", StringConverter.PascalToKebabCase(options.Domain));
            string filePath = Path.Combine(absolutePathForDomain, fileName);
            return filePath;
        }

        private string UpdateFileData(IEntityAdditionOptions options, string filePath)
        {
            string fileData = File.ReadAllText(filePath);

            StringEditor stringEditor = new StringEditor(fileData);

            // ----------- DbSet -----------
            stringEditor.NextThatContains("const routes: Routes = [");
            stringEditor.NextThatContains("];");

            stringEditor.InsertLine(GetAppRoutingLine(options));

            return stringEditor.GetText();
        }

        private string GetAppRoutingLine(IEntityAdditionOptions options)
        {
            return
              "  {\n" +
             $"    path: '{options.EntityNamePlural.ToKebab()}',\n" +
             $"    loadChildren: () => import('./{options.EntityNamePlural.ToKebab()}/{options.EntityNamePlural.ToKebab()}-pages.module').then(m => m.{options.EntityNamePlural}PagesModule)\n" +
              "  },";
        }
    }
}