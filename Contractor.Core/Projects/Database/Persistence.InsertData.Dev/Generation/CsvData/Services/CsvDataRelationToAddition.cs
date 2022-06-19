using Contractor.Core.Helpers;
using Contractor.Core.Tools;
using System;

namespace Contractor.Core.Projects.Database.Persistence.InsertData.Dev
{
    internal class CsvDataRelationToAddition
    {
        private IFileSystemClient fileSystemClient;

        private PathService pathService;

        public CsvDataRelationToAddition(IFileSystemClient fileSystemClient, PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Add(RelationSide relationSide, string domainFolder, string templateFileName)
        {
            string filePath = this.pathService.GetAbsolutePathForDatabase(relationSide, domainFolder, templateFileName);
            string fileData = this.fileSystemClient.ReadAllText(relationSide, filePath);

            fileData = UpdateFileData(relationSide, fileData);

            this.fileSystemClient.WriteAllText(fileData, filePath);
        }

        protected string UpdateFileData(RelationSide relationSide, string fileData)
        {
            Random random = new Random(IntHash.ComputeIntHash($"{relationSide.OtherEntity.Name}"));
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.InsertIntoLine($";{relationSide.Name}");
            stringEditor.Next();
            stringEditor.InsertIntoLine($";{TestValueGeneration.GenerateGuid(random)}");
            stringEditor.Next();
            stringEditor.InsertIntoLine($";{TestValueGeneration.GenerateGuid(random)}");
            stringEditor.Next();
            stringEditor.InsertIntoLine($";{TestValueGeneration.GenerateGuid(random)}");
            stringEditor.Next();
            stringEditor.InsertIntoLine($";{TestValueGeneration.GenerateGuid(random)}");

            return stringEditor.GetText();
        }
    }
}