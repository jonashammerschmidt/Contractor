using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Logic
{
    internal class EntitiesCrudLogicRelationAddition : RelationAdditionEditor
    {
        public EntitiesCrudLogicRelationAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService, RelationEnd.To)
        {
        }

        protected override string UpdateFileData(IRelationAdditionOptions options, string fileData)
        {
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

            if (options.IsOptional)
            {
                stringEditor.InsertLine(
                    $"            if ({options.EntityNameLowerTo}Create.{options.PropertyNameFrom}Id.HasValue &&\n" +
                    $"                !this.{options.EntityNamePluralLowerFrom}CrudRepository.Does{options.EntityNameFrom}Exist({options.EntityNameLowerTo}Create.{options.PropertyNameFrom}Id.Value))\n" +
                     "            {\n" +
                    $"                throw new NotFoundResultException(\"{options.PropertyNameFrom} ({{id}}) konnte nicht gefunden werden.\", {options.EntityNameLowerTo}Create.{options.PropertyNameFrom}Id.Value);\n" +
                     "            }\n");
            }
            else
            {
                stringEditor.InsertLine(
                    $"            if (!this.{options.EntityNamePluralLowerFrom}CrudRepository.Does{options.EntityNameFrom}Exist({options.EntityNameLowerTo}Create.{options.PropertyNameFrom}Id))\n" +
                     "            {\n" +
                    $"                throw new NotFoundResultException(\"{options.PropertyNameFrom} ({{id}}) konnte nicht gefunden werden.\", {options.EntityNameLowerTo}Create.{options.PropertyNameFrom}Id);\n" +
                     "            }\n");
            }

            // ----------- Update Method -----------
            stringEditor.MoveToStart();
            stringEditor.NextThatContains($"ILogicResult Update{options.EntityNameTo}(");
            stringEditor.NextThatContains($"{options.EntityNamePluralLowerTo}CrudRepository.Get{options.EntityNameTo}(");
            stringEditor.Next(line => line.StartsWith("            }"));
            stringEditor.Next();
            stringEditor.InsertNewLine();

            if (options.IsOptional)
            {
                stringEditor.InsertLine(
                    $"            if ({options.EntityNameLowerTo}Update.{options.PropertyNameFrom}Id.HasValue &&\n" +
                    $"                !this.{options.EntityNamePluralLowerFrom}CrudRepository.Does{options.EntityNameFrom}Exist({options.EntityNameLowerTo}Update.{options.PropertyNameFrom}Id.Value))\n" +
                     "            {\n" +
                    $"                throw new NotFoundResultException(\"{options.PropertyNameFrom} ({{id}}) konnte nicht gefunden werden.\", {options.EntityNameLowerTo}Update.{options.PropertyNameFrom}Id.Value);\n" +
                     "            }");
            }
            else
            {
                stringEditor.InsertLine(
                    $"            if (!this.{options.EntityNamePluralLowerFrom}CrudRepository.Does{options.EntityNameFrom}Exist({options.EntityNameLowerTo}Update.{options.PropertyNameFrom}Id))\n" +
                     "            {\n" +
                    $"                throw new NotFoundResultException(\"{options.PropertyNameFrom} ({{id}}) konnte nicht gefunden werden.\", {options.EntityNameLowerTo}Update.{options.PropertyNameFrom}Id);\n" +
                     "            }");
            }

            return stringEditor.GetText();
        }
    }
}