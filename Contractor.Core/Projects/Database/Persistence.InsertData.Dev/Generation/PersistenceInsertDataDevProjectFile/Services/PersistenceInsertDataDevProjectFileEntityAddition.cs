using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System;

namespace Contractor.Core.Projects.Database.Persistence.InsertData.Dev
{
    internal class PersistenceInsertDataDevProjectFileEntityAddition
    {
        private IFileSystemClient fileSystemClient;

        private PathService pathService;

        public PersistenceInsertDataDevProjectFileEntityAddition(IFileSystemClient fileSystemClient, PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Add(Entity entity)
        {
            string filePath = this.pathService.GetAbsolutePathForDatabase(entity, "InsertData.Dev\\Core.Persistence.InsertData.Dev.csproj");

            string fileData = this.fileSystemClient.ReadAllText(entity, filePath);
            fileData = UpdateFileData(entity, fileData);

            this.fileSystemClient.WriteAllText(fileData, filePath);
        }

        private string UpdateFileData(Entity entity, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.MoveToEnd();
            stringEditor.PrevThatContains("</ItemGroup>");

            stringEditor.InsertLine(
                $"    <None Update=\"CsvData\\{entity.Module.Name}\\dbo.{entity.NamePlural}.csv\">\n" +
                 "      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>\n" +
                 "    </None>");

            return stringEditor.GetText();
        }
    }
}