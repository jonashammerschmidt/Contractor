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

        public void Add(IRelationAdditionOptions options, string domainFolder, string templateFileName, string namespaceToAdd, bool addUnique)
        {
            string filePath = GetFilePath(options, domainFolder, templateFileName);
            string fileData = UpdateFileData(options, filePath, addUnique);

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

        private string UpdateFileData(IRelationAdditionOptions options, string filePath, bool addUnique)
        {
            string fileData = File.ReadAllText(filePath);
            StringEditor stringEditor = new StringEditor(fileData);

            string relationsPropertyLine = $"        private readonly I{options.EntityNamePluralFrom}CrudRepository {options.EntityNamePluralLowerFrom}CrudRepository;";

            if (!fileData.Contains(relationsPropertyLine))
            {
                // ----------- Property -----------
                stringEditor.NextThatContains($"private readonly I{options.EntityNamePluralTo}CrudRepository");
                stringEditor.Next(line => !line.Contains("CrudRepository"));
                stringEditor.InsertLine(relationsPropertyLine);

                // ----------- Contructor Parameter -----------
                stringEditor.NextThatContains($"I{options.EntityNamePluralTo}CrudRepository {options.EntityNamePluralLowerTo}CrudRepository,");
                stringEditor.Next(line => !line.Contains("CrudRepository"));
                stringEditor.InsertLine($"            I{options.EntityNamePluralFrom}CrudRepository {options.EntityNamePluralLowerFrom}CrudRepository,");

                // ----------- Contructor Assignment -----------
                stringEditor.NextThatContains($"this.{options.EntityNamePluralLowerTo}CrudRepository =");
                stringEditor.Next(line => !line.Contains("CrudRepository"));
                stringEditor.InsertLine($"            this.{options.EntityNamePluralLowerFrom}CrudRepository = {options.EntityNamePluralLowerFrom}CrudRepository;");
            }

            // ----------- Create Method -----------
            stringEditor.NextThatContains($"Create{options.EntityNameTo}(");
            stringEditor.NextThatContains("{");
            stringEditor.Next();

            stringEditor.InsertLine($"            if (!this.{options.EntityNamePluralLowerFrom}CrudRepository.Does{options.EntityNameFrom}Exist({options.EntityNameLowerTo}Create.{options.PropertyNameFrom}Id))\n" +
                "            {\n" +
                $"                this.logger.LogDebug(\"{options.PropertyNameFrom} konnte nicht gefunden werden.\");\n" +
                $"                return LogicResult<Guid>.NotFound(\"{options.PropertyNameFrom} konnte nicht gefunden werden.\");\n" +
                "            }\n");

            if (addUnique) {
                stringEditor.InsertLine(
                    $"            if (this.{options.EntityNamePluralLowerTo}CrudRepository.Is{options.PropertyNameFrom}IdInUse({options.EntityNameLowerTo}Create.{options.PropertyNameFrom}Id))\n" +
                    "            {\n" +
                    $"                this.logger.LogDebug(\"{options.PropertyNameFrom} bereits vergeben.\");\n" +
                    $"                return LogicResult<Guid>.Conflict(\"{options.PropertyNameFrom} bereits vergeben.\");\n" +
                    "            }\n");
            }

            // ----------- Update Method -----------
            stringEditor.MoveToStart();
            stringEditor.NextThatContains($"ILogicResult Update{options.EntityNameTo}(");
            stringEditor.NextThatContains($"{options.EntityNamePluralLowerTo}CrudRepository.Get{options.EntityNameTo}(");
            stringEditor.Next(line => line.StartsWith("            }"));
            stringEditor.Next();

            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"            if (!this.{options.EntityNamePluralLowerFrom}CrudRepository.Does{options.EntityNameFrom}Exist({options.EntityNameLowerTo}Update.{options.PropertyNameFrom}Id))\n" +
                "            {\n" +
                $"                this.logger.LogDebug(\"{options.PropertyNameFrom} konnte nicht gefunden werden.\");\n" +
                $"                return LogicResult.NotFound(\"{options.PropertyNameFrom} konnte nicht gefunden werden.\");\n" +
                "            }");

            if (addUnique)
            {
                stringEditor.InsertNewLine();
                stringEditor.InsertLine(
                    $"            bool is{options.PropertyNameFrom}IdGettingUpdated = db{options.EntityNameTo}ToUpdate.{options.PropertyNameFrom}Id != {options.EntityNameLowerTo}Update.{options.PropertyNameFrom}Id;\n" +
                    $"            if (is{options.PropertyNameFrom}IdGettingUpdated &&\n" +
                    $"                this.{options.EntityNamePluralLowerTo}CrudRepository.Is{options.PropertyNameFrom}IdInUse({options.EntityNameLowerTo}Update.{options.PropertyNameFrom}Id))\n" +
                    "            {\n" +
                    $"                this.logger.LogDebug(\"{options.PropertyNameFrom} bereits vergeben.\");\n" +
                    $"                return LogicResult.Conflict(\"{options.PropertyNameFrom} bereits vergeben.\");\n" +
                    "            }");
            }

            return stringEditor.GetText();
        }
    }
}