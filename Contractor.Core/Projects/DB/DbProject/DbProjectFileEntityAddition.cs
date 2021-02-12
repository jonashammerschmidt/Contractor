using Contractor.Core.Helpers;
using Contractor.Core.Jobs;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.DB.ProjectFile
{
    public class DbProjectFileEntityAddition
    {
        public PathService pathService;

        public DbProjectFileEntityAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void Add(IEntityAdditionOptions options)
        {
            string filePath = GetFilePath(options);
            string fileData = UpdateFileData(options, filePath);

            File.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(IEntityAdditionOptions options)
        {
            string fileName = options.DbProjectName + ".sqlproj";
            string filePath = Path.Combine(options.DbDestinationFolder, fileName);
            return filePath;
        }

        private string UpdateFileData(IEntityAdditionOptions options, string filePath)
        {
            string fileData = File.ReadAllText(filePath);

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