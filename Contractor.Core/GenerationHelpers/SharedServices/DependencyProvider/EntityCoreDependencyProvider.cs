using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using System;
using System.IO;

namespace Contractor.Core.Tools
{
    internal class EntityCoreDependencyProvider
    {
        public IFileSystemClient fileSystemClient;
        public PathService pathService;

        public EntityCoreDependencyProvider(
            IFileSystemClient fileSystemClient,
            PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }

        public void UpdateDependencyProvider(Entity entity, string fileName)
        {
            string filePath = GetFilePath(entity, fileName);
            string fileData = UpdateFileData(entity, filePath);

            this.fileSystemClient.WriteAllText(fileData, filePath);
        }

        private string GetFilePath(Entity entity, string fileName)
        {
            return Path.Combine(entity.Module.Options.Paths.BackendDestinationFolder, fileName);
        }

        private string UpdateFileData(Entity entity, string filePath)
        {
            string fileData = this.fileSystemClient.ReadAllText(entity, filePath);

            fileData = AddServices(fileData, entity);

            return fileData;
        }

        private string AddServices(string fileData, Entity entity)
        {
            string projectNamespace = $"{entity.Module.Options.Paths.ProjectName}.Modules.{entity.Module.Name}.{entity.NamePlural}";
            fileData = UsingStatements.Add(fileData, projectNamespace);

            // Insert into Startup-Method
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"void Startup{entity.Module.Name}");
            stringEditor.Next();
            stringEditor.NextThatContains("}");

            if (!stringEditor.GetLineAtOffset(-1).Trim().Equals("{"))
            {
                stringEditor.InsertNewLine();
            }
            stringEditor.InsertLine($"            // {entity.NamePlural}");
            stringEditor.InsertLine($"            services.AddScoped<I{entity.NamePlural}CrudLogic, {entity.NamePlural}CrudLogic>();");
            stringEditor.InsertLine($"            services.AddScoped<I{entity.NamePlural}CrudRepository, {entity.NamePlural}CrudRepository>();");

            return stringEditor.GetText();
        }
    }
}