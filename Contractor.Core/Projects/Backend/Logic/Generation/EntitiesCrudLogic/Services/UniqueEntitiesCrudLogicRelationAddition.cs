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
                    $"            if ({relationSide.Entity.NameLower}Create.{relationSide.Entity.Name}Id.HasValue &&\n" +
                    $"                !this.{relationSide.OtherEntity.NamePluralLower}CrudRepository.Does{relationSide.OtherEntity.Name}Exist({relationSide.Entity.NameLower}Create.{relationSide.Entity.Name}Id.Value))\n" +
                     "            {\n" +
                    $"                throw new NotFoundResultException(\"{relationSide.Entity.Name} ({{id}}) konnte nicht gefunden werden.\", {relationSide.Entity.NameLower}Create.{relationSide.Entity.Name}Id.Value);\n" +
                     "            }\n");
            }
            else
            {
                stringEditor.InsertLine(
                    $"            if (!this.{relationSide.OtherEntity.NamePluralLower}CrudRepository.Does{relationSide.OtherEntity.Name}Exist({relationSide.Entity.NameLower}Create.{relationSide.Entity.Name}Id))\n" +
                     "            {\n" +
                    $"                throw new NotFoundResultException(\"{relationSide.Entity.Name} ({{id}}) konnte nicht gefunden werden.\", {relationSide.Entity.NameLower}Create.{relationSide.Entity.Name}Id);\n" +
                     "            }\n");
            }

            if (relationSide.IsOptional)
            {
                stringEditor.InsertLine(
                    $"            if ({relationSide.Entity.NameLower}Create.{relationSide.Entity.Name}Id.HasValue &&\n" +
                    $"                this.{relationSide.Entity.NamePluralLower}CrudRepository.Is{relationSide.Entity.Name}IdInUse({relationSide.Entity.NameLower}Create.{relationSide.Entity.Name}Id.Value))\n" +
                     "            {\n" +
                    $"                throw new ConflictResultException(\"{relationSide.Entity.Name} ({{id}}) ist bereits vergeben.\", {relationSide.Entity.NameLower}Create.{relationSide.Entity.Name}Id.Value);\n" +
                     "            }\n");
            }
            else
            {
                stringEditor.InsertLine(
                    $"            if (this.{relationSide.Entity.NamePluralLower}CrudRepository.Is{relationSide.Entity.Name}IdInUse({relationSide.Entity.NameLower}Create.{relationSide.Entity.Name}Id))\n" +
                     "            {\n" +
                    $"                throw new ConflictResultException(\"{relationSide.Entity.Name} ({{id}}) ist bereits vergeben.\", {relationSide.Entity.NameLower}Create.{relationSide.Entity.Name}Id);\n" +
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
                    $"            if ({relationSide.Entity.NameLower}Update.{relationSide.Entity.Name}Id.HasValue &&\n" +
                    $"                !this.{relationSide.OtherEntity.NamePluralLower}CrudRepository.Does{relationSide.OtherEntity.Name}Exist({relationSide.Entity.NameLower}Update.{relationSide.Entity.Name}Id.Value))\n" +
                     "            {\n" +
                    $"                throw new NotFoundResultException(\"{relationSide.Entity.Name} ({{id}}) konnte nicht gefunden werden.\", {relationSide.Entity.NameLower}Update.{relationSide.Entity.Name}Id.Value);\n" +
                     "            }");
            }
            else
            {
                stringEditor.InsertLine(
                    $"            if (!this.{relationSide.OtherEntity.NamePluralLower}CrudRepository.Does{relationSide.OtherEntity.Name}Exist({relationSide.Entity.NameLower}Update.{relationSide.Entity.Name}Id))\n" +
                     "            {\n" +
                    $"                throw new NotFoundResultException(\"{relationSide.Entity.Name} ({{id}}) konnte nicht gefunden werden.\", {relationSide.Entity.NameLower}Update.{relationSide.Entity.Name}Id);\n" +
                     "            }");
            }

            stringEditor.InsertNewLine();
            if (relationSide.IsOptional)
            {
                stringEditor.InsertLine(
                    $"            bool is{relationSide.Entity.Name}IdGettingUpdated = db{relationSide.Entity.Name}ToUpdate.{relationSide.Entity.Name}Id != {relationSide.Entity.NameLower}Update.{relationSide.Entity.Name}Id.Value;\n" +
                    $"            if (is{relationSide.Entity.Name}IdGettingUpdated &&\n" +
                    $"                {relationSide.Entity.NameLower}Update.{relationSide.Entity.Name}Id.HasValue &&\n" +
                    $"                this.{relationSide.Entity.NamePluralLower}CrudRepository.Is{relationSide.Entity.Name}IdInUse({relationSide.Entity.NameLower}Update.{relationSide.Entity.Name}Id.Value))\n" +
                     "            {\n" +
                    $"                throw new ConflictResultException(\"{relationSide.Entity.Name} ({{id}}) ist bereits vergeben.\", {relationSide.Entity.NameLower}Update.{relationSide.Entity.Name}Id.Value);\n" +
                     "            }");
            }
            else
            {
                stringEditor.InsertLine(
                    $"            bool is{relationSide.Entity.Name}IdGettingUpdated = db{relationSide.Entity.Name}ToUpdate.{relationSide.Entity.Name}Id != {relationSide.Entity.NameLower}Update.{relationSide.Entity.Name}Id;\n" +
                    $"            if (is{relationSide.Entity.Name}IdGettingUpdated &&\n" +
                    $"                this.{relationSide.Entity.NamePluralLower}CrudRepository.Is{relationSide.Entity.Name}IdInUse({relationSide.Entity.NameLower}Update.{relationSide.Entity.Name}Id))\n" +
                     "            {\n" +
                    $"                throw new ConflictResultException(\"{relationSide.Entity.Name} ({{id}}) ist bereits vergeben.\", {relationSide.Entity.NameLower}Update.{relationSide.Entity.Name}Id);\n" +
                     "            }");
            }

            return stringEditor.GetText();
        }
    }
}