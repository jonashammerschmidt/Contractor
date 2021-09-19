using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Database
{
    internal class DbProjectFileEntityAddition
    {
        public FileSystemClient fileSystemClient;
        public PathService pathService;

        public DbProjectFileEntityAddition(
            FileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Add(IEntityAdditionOptions options)
        {
            string filePath = GetFilePath(options);
            string fileData = UpdateFileData(options, filePath);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(IEntityAdditionOptions options)
        {
            string fileName = options.DbProjectName + ".sqlproj";
            string filePath = Path.Combine(options.DbDestinationFolder, fileName);
            return filePath;
        }

        private string UpdateFileData(IEntityAdditionOptions options, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(filePath);

            string dbDomainFolderLine = GetDbDomainFolderLine(options);

            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("Build Include");
            stringEditor.Prev();
            stringEditor.Next(line => line.CompareTo(dbDomainFolderLine) > 0 || line.Contains("</ItemGroup>"));

            stringEditor.InsertLine(dbDomainFolderLine);

            return stringEditor.GetText();
        }

        private string GetDbDomainFolderLine(IEntityAdditionOptions options)
        {
            return $"    <Build Include=\"dbo\\Tables\\{options.Domain}\\{options.EntityNamePlural}.sql\" />";
        }
    }
}