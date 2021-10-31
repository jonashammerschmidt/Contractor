using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic
{
    internal class EntityMethodsAddition
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public EntityMethodsAddition(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void Add(IPropertyAdditionOptions options, string domainFolder, string templateFileName)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            this.fileSystemClient.WriteAllText(filePath, fileData, options);
        }

        private string GetFilePath(IPropertyAdditionOptions options, string domainFolder, string templateFileName)
        {
            string absolutePathForDTOs = this.pathService.GetAbsolutePathForDTOs(options, domainFolder);
            string fileName = templateFileName.Replace("Entity", options.EntityName);
            string filePath = Path.Combine(absolutePathForDTOs, fileName);
            return filePath;
        }

        private string UpdateFileData(IPropertyAdditionOptions options, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(filePath);

            // ----------- DbSet -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("UpdateDb" + options.EntityName);
            stringEditor.Next(line => line.Trim().Equals("}"));

            stringEditor.InsertLine($"            db{options.EntityName}.{options.PropertyName} = {options.EntityNameLower}Update.{options.PropertyName};");
            fileData = stringEditor.GetText();

            // ----------- DbSet -----------
            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("FromDb" + options.EntityName);
            stringEditor.Next(line => line.Trim().Equals("};"));

            stringEditor.InsertLine($"                {options.PropertyName} = db{options.EntityName}.{options.PropertyName},");
            fileData = stringEditor.GetText();

            // ----------- DbSet -----------
            stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("CreateDb" + options.EntityName);
            stringEditor.Next(line => line.Trim().Equals("};"));

            stringEditor.InsertLine($"                {options.PropertyName} = {options.EntityNameLower}Create.{options.PropertyName},");

            return stringEditor.GetText();
        }
    }
}