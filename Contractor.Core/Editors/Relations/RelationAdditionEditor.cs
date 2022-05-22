using Contractor.Core.Options;
using System.IO;

namespace Contractor.Core.Tools
{
    internal abstract class RelationAdditionEditor
    {
        private readonly IFileSystemClient fileSystemClient;
        private readonly PathService pathService;

        private readonly RelationEnd relationEnd;

        public RelationAdditionEditor(
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
            this.Edit(options, domainFolder, templateFileName, namespacesToAdd, false);
        }

        public void EditForDatabase(IRelationAdditionOptions options, string domainFolder, string templateFileName, params string[] namespacesToAdd)
        {
            this.Edit(options, domainFolder, templateFileName, namespacesToAdd, true);
        }

        private void Edit(IRelationAdditionOptions options, string domainFolder, string templateFileName, string[] namespacesToAdd, bool forDatabase)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName, forDatabase);

            string fileData = this.fileSystemClient.ReadAllText(filePath);
            foreach (string namespaceToAdd in namespacesToAdd)
            {
                fileData = UsingStatements.Add(fileData, namespaceToAdd);
            }

            fileData = UpdateFileData(options, fileData);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(IRelationAdditionOptions options, string domainFolder, string templateFileName, bool forDatabase)
        {
            IEntityAdditionOptions entityOptions = 
                (relationEnd == RelationEnd.From) ?
                    RelationAdditionOptions.GetPropertyForFrom(options) :
                    RelationAdditionOptions.GetPropertyForTo(options);

            string absolutePathForDTOs = 
                (forDatabase) ?
                    this.pathService.GetAbsolutePathForDatabase(entityOptions, domainFolder) :
                    this.pathService.GetAbsolutePathForBackend(entityOptions, domainFolder);
            string filePath = Path.Combine(absolutePathForDTOs, templateFileName);

            filePath = filePath.Replace("Entities", entityOptions.EntityNamePlural);
            filePath = filePath.Replace("Entity", entityOptions.EntityName);

            return filePath;
        }

        protected abstract string UpdateFileData(IRelationAdditionOptions options, string fileData);
    }
}