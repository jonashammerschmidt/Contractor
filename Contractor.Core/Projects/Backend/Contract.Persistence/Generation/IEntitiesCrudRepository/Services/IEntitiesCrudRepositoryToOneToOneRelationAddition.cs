using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Contract.Persistence
{
    internal class IEntitiesCrudRepositoryToOneToOneRelationAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public IEntitiesCrudRepositoryToOneToOneRelationAddition(
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

            this.fileSystemClient.WriteAllText(filePath, fileData, options);
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

            // ----------- Create Method -----------
            stringEditor.NextThatContains($"bool Does{options.EntityNameTo}Exist(");
            stringEditor.Next();

            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"        bool Is{options.PropertyNameFrom}IdInUse(Guid {options.PropertyNameFrom.LowerFirstChar()}Id);");

            return stringEditor.GetText();
        }
    }
}