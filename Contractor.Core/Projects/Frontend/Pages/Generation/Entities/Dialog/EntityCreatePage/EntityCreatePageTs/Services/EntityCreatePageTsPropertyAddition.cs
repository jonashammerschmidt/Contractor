﻿using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityCreatePageTsPropertyAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public EntityCreatePageTsPropertyAddition(
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
            string absolutePathForDomain = this.pathService.GetAbsolutePathForFrontendModel(options, domainFolder);
            string fileName = templateFileName.Replace("entities-kebab", StringConverter.PascalToKebabCase(options.EntityNamePlural));
            fileName = fileName.Replace("entity-kebab", StringConverter.PascalToKebabCase(options.EntityName));
            fileName = fileName.Replace("domain-kebab", StringConverter.PascalToKebabCase(options.Domain));
            string filePath = Path.Combine(absolutePathForDomain, fileName);
            return filePath;
        }

        private string UpdateFileData(IPropertyAdditionOptions options, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(filePath);

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains("this.formBuilder.group({");
            stringEditor.NextThatContains("});");
            stringEditor.InsertLine(FrontendFormBuilderPropertyLine.GetPropertyLine(options));

            return stringEditor.GetText();
        }
    }
}