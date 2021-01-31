using Contractor.Core.Helpers;
using Contractor.Core.Jobs.EntityAddition;
using System.IO;

namespace Contractor.Core.Tools
{
    public class EntityCoreAddition
    {
        public PathService pathService;

        public EntityCoreAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void AddEntityCore(EntityOptions options, string domainFolder, string templateFilePath, string templateFileName)
        {
            string fileData = GetFileData(options, templateFilePath);
            string filePath = GetFilePath(options, domainFolder, templateFileName);

            File.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(EntityOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDomain = this.pathService.GetAbsolutePathForDomain(options, domainFolder);
            string fileName = templateFileName.Replace("Entities", options.EntityNamePlural);
            string filePath = Path.Combine(absolutePathForDomain, fileName);
            return filePath;
        }

        private string GetFileData(EntityOptions options, string templateFilePath)
        {
            string fileData = File.ReadAllText(templateFilePath);
            fileData = fileData.Replace("ProjectName", options.ProjectName);
            fileData = fileData.Replace("domain-kebab", StringConverter.PascalToKebabCase(options.Domain));
            fileData = fileData.Replace("entity-kebab", StringConverter.PascalToKebabCase(options.EntityNamePlural));
            fileData = fileData.Replace("Domain", options.Domain);
            fileData = fileData.Replace("Entities", options.EntityNamePlural);
            fileData = fileData.Replace("Entity", options.EntityName);
            fileData = fileData.Replace("entities", options.EntityNamePluralLower);
            fileData = fileData.Replace("entity", options.EntityNameLower);
            return fileData;
        }
    }
}