using Contractor.Core.Jobs;
using System.IO;

namespace Contractor.Core.Tools
{
    public class DtoAddition
    {
        public PathService pathService;

        public DtoAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void AddDto(EntityOptions options, string domainFolder, string templateFilePath, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = GetFileData(options, templateFilePath);

            File.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(EntityOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForDTOs(options, domainFolder);
            string fileName = templateFileName.Replace("Entity", options.EntityName);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string GetFileData(EntityOptions options, string templateFileName)
        {
            string fileData = File.ReadAllText(templateFileName);
            fileData = fileData.Replace("Entities", options.EntityNamePlural);
            fileData = fileData.Replace("Entity", options.EntityName);
            fileData = fileData.Replace("entities", options.EntityNamePluralLower);
            fileData = fileData.Replace("entity", options.EntityNameLower);
            fileData = fileData.Replace("ProjectName", options.ProjectName);
            fileData = fileData.Replace("Domain", options.Domain);
            return fileData;
        }
    }
}