using Contractor.Core.Options;
using System.IO;

namespace Contractor.Core.Tools
{
    internal class UsingStatementAddition
    {
        public PathService pathService;

        public UsingStatementAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void Add(IEntityAdditionOptions options, string domainFolder, string templateFileName, string namespaceToAdd)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = File.ReadAllText(filePath);

            fileData = UsingStatements.Add(fileData, namespaceToAdd);

            CsharpClassWriter.Write(filePath, fileData);
        }

        private string GetFilePath(IEntityAdditionOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDomain = this.pathService.GetAbsolutePathForEntity(options, domainFolder);
            string fileName = templateFileName.Replace("Entities", options.EntityNamePlural);
            fileName = fileName.Replace("Entity", options.EntityName);
            string filePath = Path.Combine(absolutePathForDomain, fileName);
            return filePath;
        }
    }
}