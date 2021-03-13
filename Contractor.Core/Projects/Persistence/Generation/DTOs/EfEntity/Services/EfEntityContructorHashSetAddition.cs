using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Persistence
{
    internal class EfEntityContructorHashSetAddition
    {
        public PathService pathService;

        public EfEntityContructorHashSetAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void Add(IRelationAdditionOptions options, string domainFolder, string templateFileName, string namespaceToAdd)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            fileData = UsingStatements.Add(fileData, "System.Collections.Generic");
            fileData = UsingStatements.Add(fileData, namespaceToAdd);

            CsharpClassWriter.Write(filePath, fileData);
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
            string fileData = File.ReadAllText(filePath);

            // ----------- DbSet -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"public Ef{options.EntityNameFrom}()");
            stringEditor.Next(line => line.Trim().Equals("}"));

            stringEditor.InsertLine($"            this.{options.EntityNamePluralTo} = new HashSet<Ef{options.EntityNameTo}>();");

            return stringEditor.GetText();
        }
    }
}