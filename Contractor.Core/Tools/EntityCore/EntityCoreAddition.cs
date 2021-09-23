﻿using Contractor.Core.Helpers;
using Contractor.Core.Options;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Contractor.Core.Tools
{
    internal class EntityCoreAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public EntityCoreAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void AddEntityCore(IEntityAdditionOptions options, string domainFolder, string templateFilePath, string templateFileName)
        {
            string fileData = GetFileData(options, templateFilePath);
            string filePath = GetFilePath(options, domainFolder, templateFileName);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(IEntityAdditionOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDomain = this.pathService.GetAbsolutePathForEntity(options, domainFolder);
            string fileName = templateFileName.Replace("Entities", options.EntityNamePlural);
            fileName = fileName.Replace("Entity", options.EntityName);
            string filePath = Path.Combine(absolutePathForDomain, fileName);
            return filePath;
        }

        private string GetFileData(IEntityAdditionOptions options, string templateFilePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(templateFilePath);
            fileData = fileData.Replace("ProjectName", options.ProjectName);
            fileData = fileData.Replace("domain-kebab", StringConverter.PascalToKebabCase(options.Domain));
            fileData = fileData.Replace("entity-kebab", StringConverter.PascalToKebabCase(options.EntityNamePlural));
            fileData = fileData.Replace("RequestScopeDomain", options.RequestScopeDomain);
            fileData = fileData.Replace("RequestScopes", options.RequestScopeNamePlural);
            fileData = fileData.Replace("RequestScope", options.RequestScopeName);
            fileData = fileData.Replace("Domain", options.Domain);
            fileData = fileData.Replace("Entities", options.EntityNamePlural);
            fileData = fileData.Replace("Entity", options.EntityName);
            fileData = fileData.Replace("entities", options.EntityNamePluralLower);
            fileData = fileData.Replace("entity", options.EntityNameLower);
            fileData = ReplaceGuidPlaceholders(fileData, options.EntityName);

            return fileData;
        }

        private string ReplaceGuidPlaceholders(string fileData, string entityName)
        {
            Random random = new Random(IntHash.ComputeIntHash($"{entityName}"));
            var regex = new Regex(Regex.Escape("{random-guid}"));
            int placeholderCount = fileData.Split(new[] { "{random-guid}" }, StringSplitOptions.None).Length - 1;

            for (int i = 0; i < placeholderCount; i++)
            {
                var guid = new byte[16];
                random.NextBytes(guid);
                fileData = regex.Replace(fileData, new Guid(guid).ToString(), 1);
            }

            return fileData;
        }
    }
}