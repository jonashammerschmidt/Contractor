﻿using Contractor.Core.Options;
using System.IO;

namespace Contractor.Core.Tools
{
    internal class DtoAddition
    {
        public PathService pathService;

        public DtoAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void AddDto(IEntityAdditionOptions options, string domainFolder, string templateFilePath, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = GetFileData(options, templateFilePath);

            CsharpClassWriter.Write(filePath, fileData);
        }

        private string GetFilePath(IEntityAdditionOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForDTOs(options, domainFolder);
            string fileName = templateFileName.Replace("Entity", options.EntityName);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string GetFileData(IEntityAdditionOptions options, string templateFileName)
        {
            string fileData = File.ReadAllText(templateFileName);
            fileData = fileData.Replace("ProjectName", options.ProjectName);
            if (options.HasRequestScope)
            {
                fileData = fileData.Replace("RequestScopeDomain", options.RequestScopeDomain);
                fileData = fileData.Replace("RequestScope", options.RequestScopeName);
                fileData = fileData.Replace("requestScope", options.RequestScopeNameLower);
            }
            fileData = fileData.Replace("Domain", options.Domain);
            fileData = fileData.Replace("Entities", options.EntityNamePlural);
            fileData = fileData.Replace("Entity", options.EntityName);
            fileData = fileData.Replace("entities", options.EntityNamePluralLower);
            fileData = fileData.Replace("entity", options.EntityNameLower);
            return fileData;
        }
    }
}