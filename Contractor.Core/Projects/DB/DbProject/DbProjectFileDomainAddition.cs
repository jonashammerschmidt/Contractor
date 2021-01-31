using Contractor.Core.Helpers;
using Contractor.Core.Jobs.DomainAddition;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.DB.ProjectFile
{
    public class DbProjectFileDomainAddition
    {
        public PathService pathService;

        public DbProjectFileDomainAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void Add(DomainOptions options)
        {
            string filePath = GetFilePath(options);
            string fileData = UpdateFileData(options, filePath);

            File.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(DomainOptions options)
        {
            string fileName = options.DbProjectName + ".sqlproj";
            string filePath = Path.Combine(options.DbDestinationFolder, fileName);
            return filePath;
        }

        private string UpdateFileData(DomainOptions options, string filePath)
        {
            string fileData = File.ReadAllText(filePath);

            string dbDomainFolderLine = GetDbDomainFolderLine(options);

            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("Folder Include");
            stringEditor.Prev();
            stringEditor.Next(line => line.CompareTo(dbDomainFolderLine) > 0 || line.Contains("</ItemGroup>"));

            stringEditor.InsertLine(dbDomainFolderLine);

            return stringEditor.GetText();
        }

        private string GetDbDomainFolderLine(DomainOptions options)
        {
            return $"    <Folder Include=\"dbo\\Tables\\{options.Domain}\" />";
        }
    }
}