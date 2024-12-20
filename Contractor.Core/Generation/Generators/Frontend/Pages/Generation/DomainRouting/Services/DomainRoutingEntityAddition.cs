﻿using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Frontend.Pages
{
    public class DomainRoutingEntityAddition
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

            this.fileSystemClient.WriteAllText(fileData, filePath);
        }

        private string UpdateFileData(Entity entity, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(entity, filePath);

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("const routes: Routes = [");

            if (stringEditor.GetText().Contains("'**'"))
            {
                stringEditor.NextThatContains("'**'");
                stringEditor.Prev();
            }
            else
            {
                stringEditor.NextThatContains("];");
            }

            stringEditor.InsertLine("  {");
            stringEditor.InsertLine($"    path: '{entity.NamePluralKebab}',");
            stringEditor.InsertLine($"    loadChildren: () => import('./{entity.NamePluralKebab}/{entity.NamePluralKebab}-pages.module')");
            stringEditor.InsertLine($"      .then(m => m.{entity.NamePlural}PagesModule)");
            stringEditor.InsertLine("  },");

            if (!stringEditor.GetText().Contains("'**'"))
            {
                stringEditor.InsertLine("  {");
                stringEditor.InsertLine($"    path: '**',");
                stringEditor.InsertLine($"    redirectTo: '{entity.NamePluralKebab}',");
                stringEditor.InsertLine("  },");
            }

            return stringEditor.GetText();
        }
    }
}