﻿using Contractor.Core.Helpers;
using Contractor.Core.Options;
using System.IO;

namespace Contractor.Core.Tools
{
    internal class FrontendDtoAddition
    {
        public PathService pathService;

        public FrontendDtoAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void AddDto(IEntityAdditionOptions options, string domainFolder, string templateFilePath, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = GetFileData(options, templateFilePath);

            TypescriptClassWriter.Write(filePath, fileData);
        }

        private string GetFilePath(IEntityAdditionOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForFrontendModel(options, domainFolder);
            string fileName = templateFileName.Replace("entity-kebab", StringConverter.PascalToKebabCase(options.EntityName));
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string GetFileData(IEntityAdditionOptions options, string templateFileName)
        {
            string fileData = File.ReadAllText(templateFileName);
            fileData = fileData.Replace("ProjectName", options.ProjectName);
            fileData = fileData.Replace("entities-kebab", StringConverter.PascalToKebabCase(options.EntityNamePlural));
            fileData = fileData.Replace("entity-kebab", StringConverter.PascalToKebabCase(options.EntityName));
            fileData = fileData.Replace("Domain", options.Domain);
            fileData = fileData.Replace("Entities", options.EntityNamePlural);
            fileData = fileData.Replace("Entity", options.EntityName);
            fileData = fileData.Replace("entities", options.EntityNamePluralLower);
            fileData = fileData.Replace("entity", options.EntityNameLower);
            return fileData;
        }
    }
}