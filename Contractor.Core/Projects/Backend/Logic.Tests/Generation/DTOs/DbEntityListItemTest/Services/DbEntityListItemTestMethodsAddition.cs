﻿
using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic.Tests
{
    internal class DbEntityListItemTestMethodsAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public DbEntityListItemTestMethodsAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Add(IPropertyAdditionOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(IPropertyAdditionOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForDTOs(options, domainFolder);
            string fileName = templateFileName.Replace("Entity", options.EntityName);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(IPropertyAdditionOptions options, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(filePath);

            // ----------- Creators -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"public static IDb{options.EntityName}ListItem Default()");
            stringEditor.Next(line => line.Trim().Equals("};"));
            stringEditor.InsertLine($"                {options.PropertyName} = {options.EntityName}TestValues.{options.PropertyName}Default,");
            fileData = stringEditor.GetText();

            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"public static IDb{options.EntityName}ListItem Default2()");
            stringEditor.Next(line => line.Trim().Equals("};"));
            stringEditor.InsertLine($"                {options.PropertyName} = {options.EntityName}TestValues.{options.PropertyName}Default2,");
            fileData = stringEditor.GetText();

            return stringEditor.GetText();
        }
    }
}