﻿using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Database
{
    internal class DbTablePropertyAddition
    {
        public PathService pathService;

        public DbTablePropertyAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void AddProperty(IPropertyAdditionOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = GetFileData(options, filePath);

            File.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(IPropertyAdditionOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDomain = this.pathService.GetAbsolutePathForDbDomain(options, domainFolder);
            string fileName = templateFileName.Replace("Entities", options.EntityNamePlural);
            string filePath = Path.Combine(absolutePathForDomain, fileName);
            return filePath;
        }

        private string GetFileData(IPropertyAdditionOptions options, string filePath)
        {
            string fileData = File.ReadAllText(filePath);

            fileData = fileData.Replace("RequestScopeDomain", options.RequestScopeDomain);
            fileData = fileData.Replace("RequestScopes", options.RequestScopeNamePlural);
            fileData = fileData.Replace("RequestScope", options.RequestScopeName);

            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("PRIMARY KEY CLUSTERED");

            stringEditor.InsertLine(DatabaseTablePropertyLine.GetPropertyLine(options));

            return stringEditor.GetText();
        }
    }
}