using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System;

namespace Contractor.Core.Generation.Database.Persistence.InsertData.Dev
{
    internal class CsvDataPropertyAddition
    {
        private IFileSystemClient fileSystemClient;

        private PathService pathService;

        public CsvDataPropertyAddition(IFileSystemClient fileSystemClient, PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Add(Property property, string domainFolder, string templateFileName)
        {
            string filePath = this.pathService.GetAbsolutePathForDatabase(property, domainFolder, templateFileName);
            string fileData = this.fileSystemClient.ReadAllText(property, filePath);

            fileData = UpdateFileData(property, fileData);

            this.fileSystemClient.WriteAllText(fileData, filePath);
        }

        protected string UpdateFileData(Property property, string fileData)
        {
            Random random = new Random(IntHash.ComputeIntHash($"{property.Entity.Name}.{property.Name}"));
            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.InsertIntoLine($";{property.Name}");
            stringEditor.Next();
            stringEditor.InsertIntoLine($";{TestValueGeneration.GetPropertyLine(property, " 1", random)}");
            stringEditor.Next();
            stringEditor.InsertIntoLine($";{TestValueGeneration.GetPropertyLine(property, " 2", random)}");
            stringEditor.Next();
            if (property.IsOptional)
            {
                stringEditor.InsertIntoLine($";NULL");
                stringEditor.Next();
                stringEditor.InsertIntoLine($";NULL");
            }
            else
            {
                stringEditor.InsertIntoLine($";{TestValueGeneration.GetPropertyLine(property, " 3", random)}");
                stringEditor.Next();
                stringEditor.InsertIntoLine($";{TestValueGeneration.GetPropertyLine(property, " 4", random)}");
            }

            return stringEditor.GetText();
        }
    }
}