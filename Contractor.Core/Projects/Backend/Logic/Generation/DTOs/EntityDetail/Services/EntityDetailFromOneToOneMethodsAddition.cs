using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic
{
    internal class EntityDetailFromOneToOneMethodsAddition
    {
        public FileSystemClient fileSystemClient;
        public PathService pathService;

        public EntityDetailFromOneToOneMethodsAddition(
            FileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        } 

        public void Add(IRelationAdditionOptions options, string domainFolder, string templateFileName, string namespaceToAdd)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            fileData = UsingStatements.Add(fileData, "System.Linq");
            fileData = UsingStatements.Add(fileData, namespaceToAdd);

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        private string GetFilePath(IRelationAdditionOptions options, string domainFolder, string templateFileName)
        {
            IEntityAdditionOptions entityOptions = RelationAdditionOptions.GetPropertyForFrom(options);
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForDTOs(entityOptions, domainFolder);
            string fileName = templateFileName.Replace("Entity", entityOptions.EntityName);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(IRelationAdditionOptions options, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(filePath);

            // ----------- DbSet -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("FromDb" + options.EntityNameFrom);
            stringEditor.Next(line => line.Trim().Equals("};"));

            stringEditor.InsertLine($"                {options.PropertyNameTo} = {options.EntityNameTo}.FromDb{options.EntityNameTo}(db{options.EntityNameFrom}Detail.{options.PropertyNameTo}),");

            return stringEditor.GetText();
        }
    }
}