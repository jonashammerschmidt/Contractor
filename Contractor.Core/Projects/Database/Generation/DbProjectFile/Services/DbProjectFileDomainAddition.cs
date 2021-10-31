﻿using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Database
{
    internal class DbProjectFileDomainAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public DbProjectFileDomainAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Add(IDomainAdditionOptions options)
        {
            string filePath = GetFilePath(options);
            string fileData = UpdateFileData(options, filePath);

            this.fileSystemClient.WriteAllText(filePath, fileData, options);
        }

        private string GetFilePath(IDomainAdditionOptions options)
        {
            string fileName = options.DbProjectName + ".sqlproj";
            string filePath = Path.Combine(options.DbDestinationFolder, fileName);
            return filePath;
        }

        private string UpdateFileData(IDomainAdditionOptions options, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(filePath);

            string dbDomainFolderLine = GetDbDomainFolderLine(options);

            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("Folder Include");
            stringEditor.Prev();
            stringEditor.Next(line => line.CompareTo(dbDomainFolderLine) > 0 || line.Contains("</ItemGroup>"));

            stringEditor.InsertLine(dbDomainFolderLine);

            return stringEditor.GetText();
        }

        private string GetDbDomainFolderLine(IDomainAdditionOptions options)
        {
            return $"    <Folder Include=\"dbo\\Tables\\{options.Domain}\" />";
        }
    }
}