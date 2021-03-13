using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Persistence.Tests
{
    internal class InMemoryDbContextEntityAddition
    {
        public PathService pathService;

        public InMemoryDbContextEntityAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void Add(IEntityAdditionOptions options)
        {
            string filePath = this.pathService.GetAbsolutePathForInMemoryDbContext(options);
            string fileData = UpdateFileData(options, filePath);

            CsharpClassWriter.Write(filePath, fileData);
        }

        private string UpdateFileData(IEntityAdditionOptions options, string filePath)
        {
            string fileData = File.ReadAllText(filePath);

            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Persistence.Modules.{options.Domain}.{options.EntityNamePlural}");
            fileData = UsingStatements.Add(fileData, $"{options.ProjectName}.Persistence.Tests.Modules.{options.Domain}.{options.EntityNamePlural}");

            // ----------- DbSet -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains("persistenceDbContext.SaveChanges();");

            if (options.HasRequestScope)
            {
                stringEditor.InsertLine($"            persistenceDbContext.{options.EntityNamePlural}.Add(Db{options.EntityName}.ToEf{options.EntityName}(Db{options.EntityName}Test.DbDefault(), {options.EntityName}TestValues.{options.RequestScopeName}IdDbDefault));");
                stringEditor.InsertLine($"            persistenceDbContext.{options.EntityNamePlural}.Add(Db{options.EntityName}.ToEf{options.EntityName}(Db{options.EntityName}Test.DbDefault2(), {options.EntityName}TestValues.{options.RequestScopeName}IdDbDefault));");
            }
            else
            {
                stringEditor.InsertLine($"            persistenceDbContext.{options.EntityNamePlural}.Add(Db{options.EntityName}.ToEf{options.EntityName}(Db{options.EntityName}Test.DbDefault()));");
                stringEditor.InsertLine($"            persistenceDbContext.{options.EntityNamePlural}.Add(Db{options.EntityName}.ToEf{options.EntityName}(Db{options.EntityName}Test.DbDefault2()));");
            }

            return stringEditor.GetText();
        }
    }
}