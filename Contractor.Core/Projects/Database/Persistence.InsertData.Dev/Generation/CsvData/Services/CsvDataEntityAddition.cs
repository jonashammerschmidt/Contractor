using Contractor.Core.Helpers;
using Contractor.Core.Tools;
using System;

namespace Contractor.Core.Projects.Database.Persistence.InsertData.Dev
{
    internal class CsvDataEntityAddition
    {
        private IFileSystemClient fileSystemClient;

        private PathService pathService;

        public CsvDataEntityAddition(IFileSystemClient fileSystemClient, PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Add(Entity entity, string domainFolder, string templateFileName)
        {
            string filePath = this.pathService.GetAbsolutePathForDatabase(entity, domainFolder, templateFileName);
            
            string fileData = UpdateFileData(entity, "");

            this.fileSystemClient.WriteAllText(filePath, fileData);
        }

        protected string UpdateFileData(Entity entity, string fileData)
        {
            Random random = new Random(IntHash.ComputeIntHash($"{entity.Name}"));
            StringEditor stringEditor = new StringEditor(fileData);

            if (entity.HasScope)
            {
                stringEditor.InsertLine($"Id;{entity.ScopeEntity.Name}Id");
                stringEditor.InsertLine($"{TestValueGeneration.GenerateGuid(random)};936071b5-d532-45bb-b7d1-166884e7cf62");
                stringEditor.InsertLine($"{TestValueGeneration.GenerateGuid(random)};936071b5-d532-45bb-b7d1-166884e7cf62");
                stringEditor.InsertLine($"{TestValueGeneration.GenerateGuid(random)};936071b5-d532-45bb-b7d1-166884e7cf62");
                stringEditor.InsertLine($"{TestValueGeneration.GenerateGuid(random)};936071b5-d532-45bb-b7d1-166884e7cf62");
            }
            else
            {
                stringEditor.InsertLine($"Id");
                stringEditor.InsertLine($"{TestValueGeneration.GenerateGuid(random)}");
                stringEditor.InsertLine($"{TestValueGeneration.GenerateGuid(random)}");
                stringEditor.InsertLine($"{TestValueGeneration.GenerateGuid(random)}");
                stringEditor.InsertLine($"{TestValueGeneration.GenerateGuid(random)}");
            }

            return stringEditor.GetText();
        }
    }
}