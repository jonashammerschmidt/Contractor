using Contractor.Core.Helpers;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Logic
{
    internal class UniqueEntitiesCrudLogicRelationAddition : RelationAdditionEditor
    {
        public UniqueEntitiesCrudLogicRelationAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            StringEditor stringEditor = new StringEditor(fileData);

            string relationsPropertyLine = $"        private readonly I{relationSide.OtherEntity.NamePlural}CrudRepository {relationSide.OtherEntity.NamePluralLower}CrudRepository;";

            if (!fileData.Contains(relationsPropertyLine))
            {
                // ----------- Property -----------
                stringEditor.NextThatContains($"private readonly I{relationSide.Entity.NamePlural}CrudRepository");
                stringEditor.Next(line => !line.Contains("CrudRepository"));
                stringEditor.InsertLine(relationsPropertyLine);

                // ----------- Contructor Parameter -----------
                stringEditor.NextThatContains($"I{relationSide.Entity.NamePlural}CrudRepository {relationSide.Entity.NamePluralLower}CrudRepository,");
                stringEditor.Next(line => !line.Contains("CrudRepository"));
                stringEditor.InsertLine($"            I{relationSide.OtherEntity.NamePlural}CrudRepository {relationSide.OtherEntity.NamePluralLower}CrudRepository,");

                // ----------- Contructor Assignment -----------
                stringEditor.NextThatContains($"this.{relationSide.Entity.NamePluralLower}CrudRepository =");
                stringEditor.Next(line => !line.Contains("CrudRepository"));
                stringEditor.InsertLine($"            this.{relationSide.OtherEntity.NamePluralLower}CrudRepository = {relationSide.OtherEntity.NamePluralLower}CrudRepository;");
            }

            // ----------- Create Method -----------
            stringEditor.NextThatContains($"Create{relationSide.Entity.Name}(");
            stringEditor.NextThatContains("{");
            stringEditor.Next();

            if (relationSide.IsOptional)
            {
                stringEditor.InsertLine(
                    $"            if ({relationSide.Entity.NameLower}Create.{relationSide.Name}.HasValue &&\n" +
                    $"                !this.{relationSide.OtherEntity.NamePluralLower}CrudRepository.Does{relationSide.OtherEntity.Name}Exist({relationSide.Entity.NameLower}Create.{relationSide.Name}.Value))\n" +
                     "            {\n" +
                    $"                throw new NotFoundResultException(\"{relationSide.Entity.Name} ({{id}}) konnte nicht gefunden werden.\", {relationSide.Entity.NameLower}Create.{relationSide.Name}.Value);\n" +
                     "            }\n");
            }
            else
            {
                stringEditor.InsertLine(
                    $"            if (!this.{relationSide.OtherEntity.NamePluralLower}CrudRepository.Does{relationSide.OtherEntity.Name}Exist({relationSide.Entity.NameLower}Create.{relationSide.Name}))\n" +
                     "            {\n" +
                    $"                throw new NotFoundResultException(\"{relationSide.Entity.Name} ({{id}}) konnte nicht gefunden werden.\", {relationSide.Entity.NameLower}Create.{relationSide.Name});\n" +
                     "            }\n");
            }

            if (relationSide.IsOptional)
            {
                stringEditor.InsertLine(
                    $"            if ({relationSide.Entity.NameLower}Create.{relationSide.Name}.HasValue &&\n" +
                    $"                this.{relationSide.Entity.NamePluralLower}CrudRepository.Is{relationSide.Name}InUse({relationSide.Entity.NameLower}Create.{relationSide.Name}.Value))\n" +
                     "            {\n" +
                    $"                throw new ConflictResultException(\"{relationSide.OtherEntity.Name} ({{id}}) ist bereits vergeben.\", {relationSide.Entity.NameLower}Create.{relationSide.Name}.Value);\n" +
                     "            }\n");
            }
            else
            {
                stringEditor.InsertLine(
                    $"            if (this.{relationSide.Entity.NamePluralLower}CrudRepository.Is{relationSide.Name}InUse({relationSide.Entity.NameLower}Create.{relationSide.Name}))\n" +
                     "            {\n" +
                    $"                throw new ConflictResultException(\"{relationSide.OtherEntity.Name} ({{id}}) ist bereits vergeben.\", {relationSide.Entity.NameLower}Create.{relationSide.Name});\n" +
                     "            }\n");
            }

            // ----------- Update Method -----------
            stringEditor.MoveToStart();
            stringEditor.NextThatContains($"ILogicResult Update{relationSide.Entity.Name}(");
            stringEditor.NextThatContains($"{relationSide.Entity.NamePluralLower}CrudRepository.Get{relationSide.Entity.Name}(");
            stringEditor.Next(line => line.StartsWith("            }"));
            stringEditor.Next();
            stringEditor.InsertNewLine();

            if (relationSide.IsOptional)
            {
                stringEditor.InsertLine(
                    $"            if ({relationSide.Entity.NameLower}Update.{relationSide.Name}.HasValue &&\n" +
                    $"                !this.{relationSide.OtherEntity.NamePluralLower}CrudRepository.Does{relationSide.OtherEntity.Name}Exist({relationSide.Entity.NameLower}Update.{relationSide.Name}.Value))\n" +
                     "            {\n" +
                    $"                throw new NotFoundResultException(\"{relationSide.Entity.Name} ({{id}}) konnte nicht gefunden werden.\", {relationSide.Entity.NameLower}Update.{relationSide.Name}.Value);\n" +
                     "            }");
            }
            else
            {
                stringEditor.InsertLine(
                    $"            if (!this.{relationSide.OtherEntity.NamePluralLower}CrudRepository.Does{relationSide.OtherEntity.Name}Exist({relationSide.Entity.NameLower}Update.{relationSide.Name}))\n" +
                     "            {\n" +
                    $"                throw new NotFoundResultException(\"{relationSide.Entity.Name} ({{id}}) konnte nicht gefunden werden.\", {relationSide.Entity.NameLower}Update.{relationSide.Name});\n" +
                     "            }");
            }

            stringEditor.InsertNewLine();
            if (relationSide.IsOptional)
            {
                stringEditor.InsertLine(
                    $"            bool is{relationSide.Name}GettingUpdated = db{relationSide.Entity.Name}ToUpdate.{relationSide.Name} != {relationSide.Entity.NameLower}Update.{relationSide.Name}.Value;\n" +
                    $"            if (is{relationSide.Name}GettingUpdated &&\n" +
                    $"                {relationSide.Entity.NameLower}Update.{relationSide.Name}.HasValue &&\n" +
                    $"                this.{relationSide.Entity.NamePluralLower}CrudRepository.Is{relationSide.Name}InUse({relationSide.Entity.NameLower}Update.{relationSide.Name}.Value))\n" +
                     "            {\n" +
                    $"                throw new ConflictResultException(\"{relationSide.Entity.Name} ({{id}}) ist bereits vergeben.\", {relationSide.Entity.NameLower}Update.{relationSide.Name}.Value);\n" +
                     "            }");
            }
            else
            {
                stringEditor.InsertLine(
                    $"            bool is{relationSide.Name}GettingUpdated = db{relationSide.Entity.Name}ToUpdate.{relationSide.Name} != {relationSide.Entity.NameLower}Update.{relationSide.Name};\n" +
                    $"            if (is{relationSide.Name}GettingUpdated &&\n" +
                    $"                this.{relationSide.Entity.NamePluralLower}CrudRepository.Is{relationSide.Name}InUse({relationSide.Entity.NameLower}Update.{relationSide.Name}))\n" +
                     "            {\n" +
                    $"                throw new ConflictResultException(\"{relationSide.Entity.Name} ({{id}}) ist bereits vergeben.\", {relationSide.Entity.NameLower}Update.{relationSide.Name});\n" +
                     "            }");
            }

            return stringEditor.GetText();
        }
    }
}