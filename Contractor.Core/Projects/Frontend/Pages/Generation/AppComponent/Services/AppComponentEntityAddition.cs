﻿using Contractor.Core.Helpers;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class AppComponentEntityAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public AppComponentEntityAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Add(Entity entity)
        {
            string filePath = Path.Combine(entity.Module.Options.Paths.FrontendDestinationFolder, "src\\app\\app.component.ts");
            string fileData = UpdateFileData(entity, filePath);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string UpdateFileData(Entity entity, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(entity, filePath);

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains(" addMainMenuItems(");
            stringEditor.NextThatContains($"text: '{entity.Module.Name}',");
            stringEditor.NextThatContains($"subMenu: [");
            stringEditor.Next();

            stringEditor.InsertLine(
                 "        {\n" +
                $"          text: '{entity.NamePluralReadable}',\n" +
                $"          routerLink: '/{entity.Module.NameKebab}/{entity.NamePluralKebab}',\n" +
                 "        },");

            return stringEditor.GetText();
        }
    }
}