using Contractor.Core.Helpers;
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

        public void UpdateDependencyProvider(Entity entity, string projectFolder, string fileName)
        {
            string filePath = GetFilePath(entity, projectFolder, fileName);
            string fileData = UpdateFileData(entity, filePath, projectFolder);

            this.fileSystemClient.WriteAllText(fileData, filePath);
        }

        private string GetFilePath(Entity entity, string projectFolder, string fileName)
        {
            return Path.Combine(entity.Module.Options.Paths.BackendDestinationFolder, projectFolder, fileName);
        }

        private string UpdateFileData(Entity entity, string filePath, string projectFolder)
        {
            string fileData = this.fileSystemClient.ReadAllText(entity, filePath);

            fileData = AddServices(fileData, entity, projectFolder);

            return fileData;
        }

        private string AddServices(string fileData, Entity entity, string projectFolder)
        {
            string contractNamespace = GetContractNamespace(entity, projectFolder);
            fileData = UsingStatements.Add(fileData, contractNamespace);

            string projectNamespace = GetProjectNamespace(entity, projectFolder);
            fileData = UsingStatements.Add(fileData, projectNamespace);

            // Insert into Startup-Method
            StringEditor stringEditor = new StringEditor(fileData);
            string addScopedStatement = GetAddScopedStatement(entity.NamePlural, projectFolder);
            stringEditor.NextThatContains($"void Startup{entity.Module.Name}");
            stringEditor.Next();
            stringEditor.NextThatContains("}");

            if (!stringEditor.GetLineAtOffset(-1).Trim().Equals("{"))
            {
                stringEditor.InsertNewLine();
            }
            stringEditor.InsertLine($"            // {entity.NamePlural}");
            stringEditor.InsertLine(addScopedStatement);

            return stringEditor.GetText();
        }

        private string GetContractNamespace(Entity entity, string projectFolder)
        {
            if (projectFolder.Equals("Logic"))
            {
                return $"{entity.Module.Options.Paths.ProjectName}.Contract.Logic.Modules.{entity.Module.Name}.{entity.NamePlural}";
            }
            else if (projectFolder.Equals("Persistence"))
            {
                return $"{entity.Module.Options.Paths.ProjectName}.Contract.Persistence.Modules.{entity.Module.Name}.{entity.NamePlural}";
            }

            throw new ArgumentException("Argument 'projectFolder' invalid");
        }

        private string GetProjectNamespace(Entity entity, string projectFolder)
        {
            if (projectFolder.Equals("Logic"))
            {
                return $"{entity.Module.Options.Paths.ProjectName}.Logic.Modules.{entity.Module.Name}.{entity.NamePlural}";
            }
            else if (projectFolder.Equals("Persistence"))
            {
                return $"{entity.Module.Options.Paths.ProjectName}.Persistence.Modules.{entity.Module.Name}.{entity.NamePlural}";
            }

            throw new ArgumentException("Argument 'projectFolder' invalid");
        }

        private string GetAddScopedStatement(string entityNamePlural, string projectFolder)
        {
            if (projectFolder.Equals("Logic"))
            {
                return $"            services.AddScoped<I{entityNamePlural}CrudLogic, {entityNamePlural}CrudLogic>();";
            }
            else if (projectFolder.Equals("Persistence"))
            {
                return $"            services.AddScoped<I{entityNamePlural}CrudRepository, {entityNamePlural}CrudRepository>();";
            }

            throw new ArgumentException("Argument 'projectFolder' invalid");
        }
    }
}