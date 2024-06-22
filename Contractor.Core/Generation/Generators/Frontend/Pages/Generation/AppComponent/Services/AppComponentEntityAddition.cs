using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Frontend.Pages
{
    public class AppComponentEntityAddition
    {
        private IFileSystemClient fileSystemClient;

        public AppComponentEntityAddition(
            IFileSystemClient fileSystemClient)
        {
            this.fileSystemClient = fileSystemClient;
        }

        public void Add(Entity entity)
        {
            string filePath = Path.Combine(entity.Module.Options.Paths.FrontendDestinationFolder, "src", "app", "app.component.ts");
            string fileData = UpdateFileData(entity, filePath);

            this.fileSystemClient.WriteAllText(fileData, filePath);
        }

        private string UpdateFileData(Entity entity, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(entity, filePath);

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains(" addMainMenuItems(");
            stringEditor.NextThatContains($"text: '{entity.Module.Name}',");
            stringEditor.NextThatContains($"subMenu: [");
            stringEditor.NextThatStartsWith("      ]");

            stringEditor.InsertLine(
                 "        {\n" +
                $"          text: '{entity.NamePluralReadable}',\n" +
                $"          routerLink: '/{entity.Module.NameKebab}/{entity.NamePluralKebab}',\n" +
                 "        },");

            return stringEditor.GetText();
        }
    }
}