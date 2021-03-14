using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic
{
    internal class EntitiesCrudLogicRelationAddition
    {
        public PathService pathService;

        public EntitiesCrudLogicRelationAddition(PathService pathService)
        {
            this.pathService = pathService;
        }

        public void Add(IRelationAdditionOptions options, string domainFolder, string templateFileName, string namespaceToAdd)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath);

            fileData = UsingStatements.Add(fileData, namespaceToAdd);

            CsharpClassWriter.Write(filePath, fileData);
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
            string fileData = File.ReadAllText(filePath);

            // ----------- Property -----------
            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.NextThatContains($"private readonly I{options.EntityNamePluralTo}CrudRepository");
            stringEditor.Next(line => !line.Contains("CrudRepository"));

            stringEditor.InsertLine($"        private readonly I{options.EntityNamePluralFrom}CrudRepository {options.EntityNamePluralLowerFrom}CrudRepository;");

            // ----------- Contructor Parameter -----------
            stringEditor.NextThatContains($"I{options.EntityNamePluralTo}CrudRepository {options.EntityNamePluralLowerTo}CrudRepository,");
            stringEditor.Next(line => !line.Contains("CrudRepository"));

            stringEditor.InsertLine($"            I{options.EntityNamePluralFrom}CrudRepository {options.EntityNamePluralLowerFrom}CrudRepository,");

            // ----------- Contructor Assignment -----------
            stringEditor.NextThatContains($"this.{options.EntityNamePluralLowerTo}CrudRepository =");
            stringEditor.Next(line => !line.Contains("CrudRepository"));

            stringEditor.InsertLine($"            this.{options.EntityNamePluralLowerFrom}CrudRepository = {options.EntityNamePluralLowerFrom}CrudRepository;");

            // ----------- Create Method -----------
            stringEditor.NextThatContains($"Create{options.EntityNameTo}(");
            stringEditor.NextThatContains("{");
            stringEditor.Next();

            stringEditor.InsertLine($"            if (!this.{options.EntityNamePluralLowerFrom}CrudRepository.Does{options.EntityNameFrom}Exist({options.EntityNameLowerTo}Create.{options.EntityNameFrom}Id))\n" +
                "            {\n" +
                $"                this.logger.LogDebug(\"{options.EntityNameFrom} konnte nicht gefunden werden.\");\n" +
                $"                return LogicResult<Guid>.NotFound(\"{options.EntityNameFrom} konnte nicht gefunden werden.\");\n" +
                "            }\n");

            // ----------- Update Method -----------
            stringEditor.NextThatContains($"Update{options.EntityNameTo}(");
            stringEditor.NextThatContains("{");
            stringEditor.Next();

            stringEditor.InsertLine($"            if (!this.{options.EntityNamePluralLowerFrom}CrudRepository.Does{options.EntityNameFrom}Exist({options.EntityNameLowerTo}Update.{options.EntityNameFrom}Id))\n" +
                "            {\n" +
                $"                this.logger.LogDebug(\"{options.EntityNameFrom} konnte nicht gefunden werden.\");\n" +
                $"                return LogicResult.NotFound(\"{options.EntityNameFrom} konnte nicht gefunden werden.\");\n" +
                "            }\n");

            return stringEditor.GetText();
        }
    }
}