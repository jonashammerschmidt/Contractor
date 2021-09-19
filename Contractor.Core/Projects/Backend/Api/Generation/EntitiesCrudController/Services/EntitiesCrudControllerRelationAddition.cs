using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Api
{
    internal class EntitiesCrudControllerRelationAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public EntitiesCrudControllerRelationAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Add(IRelationAdditionOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(IRelationAdditionOptions options, string domainFolder, string templateFileName)
        {
            IEntityAdditionOptions entityOptions = RelationAdditionOptions.GetPropertyForTo(options);
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForEntity(entityOptions, domainFolder);
            string fileName = templateFileName.Replace("Entities", entityOptions.EntityNamePlural);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(IRelationAdditionOptions options, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(filePath);
            StringEditor stringEditor = new StringEditor(fileData);

            string paginationLineStart = "        [Pagination(FilterFields = new[] { ";
            stringEditor.NextThatContains(paginationLineStart);
            var paginationLineEnd = stringEditor.GetLine().Substring(paginationLineStart.Length);
            var updatedLine = paginationLineStart + "\"" + options.PropertyNameFrom + "Id\", " + paginationLineEnd;
            stringEditor.SetLine(updatedLine);

            return stringEditor.GetText();
        }
    }
}