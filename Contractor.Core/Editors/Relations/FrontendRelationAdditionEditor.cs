using Contractor.Core.Helpers;
using Contractor.Core.Options;
using System.IO;

namespace Contractor.Core.Tools
{
    internal abstract class FrontendRelationAdditionEditor
    {
        private IFileSystemClient fileSystemClient;
        private PathService pathService;

        private RelationEnd relationEnd;

        public FrontendRelationAdditionEditor(
            IFileSystemClient fileSystemClient,
            PathService pathService,
            RelationEnd relationEnd)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
            this.relationEnd = relationEnd;
        }

        public void Edit(IRelationAdditionOptions options, string domainFolder, string templateFileName, params string[] namespacesToAdd)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);

            string fileData = this.fileSystemClient.ReadAllText(filePath);
            foreach (string namespaceToAdd in namespacesToAdd)
            {
                fileData = UsingStatements.Add(fileData, namespaceToAdd);
            }

            fileData = UpdateFileData(options, fileData);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(IRelationAdditionOptions options, string domainFolder, string templateFileName)
        {
            IEntityAdditionOptions entityOptions = 
                (relationEnd == RelationEnd.From) ?
                    RelationAdditionOptions.GetPropertyForFrom(options) :
                    RelationAdditionOptions.GetPropertyForTo(options);

            string absolutePathForDTOs = this.pathService.GetAbsolutePathForFrontend(entityOptions, domainFolder);
            string filePath = Path.Combine(absolutePathForDTOs, templateFileName);

            filePath = filePath.Replace("entities-kebab", StringConverter.PascalToKebabCase(entityOptions.EntityNamePlural));
            filePath = filePath.Replace("entity-kebab", StringConverter.PascalToKebabCase(entityOptions.EntityName));
            filePath = filePath.Replace("domain-kebab", StringConverter.PascalToKebabCase(entityOptions.Domain));

            return filePath;
        }

        protected abstract string UpdateFileData(IRelationAdditionOptions options, string fileData);
    }
}