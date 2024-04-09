using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Backend.Persistence
{
    public class EntitiesCrudRepositoryToRelationAddition : RelationSideAdditionToExisitingFileGeneration
    {
        public EntitiesCrudRepositoryToRelationAddition(IFileSystemClient fileSystemClient, PathService pathService)
            : base(fileSystemClient, pathService)
        {
        }

        protected override string UpdateFileData(RelationSide relationSide, string fileData)
        {
            if (!relationSide.IsCreatedByPreProcessor)
            {
                return fileData;
            }

            fileData = UsingStatements.Add(fileData,
                $"{relationSide.Entity.Module.Options.Paths.DbProjectName}.Generated.DbContext.Modules.{relationSide.RelationBeforePreProcessor.TargetEntity.Module.Name}.{relationSide.RelationBeforePreProcessor.TargetEntity.NamePlural}");

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains($"{relationSide.Entity.Name}Dto.ToEf{relationSide.Entity.Name}Dto(");
            stringEditor.NextUntil(line => !line.Contains("this.Lookup"));
            if (relationSide.IsOptional)
            {
                stringEditor.InsertLine($"            ef{relationSide.Entity.Name}Dto.{relationSide.Name}Id = {relationSide.Entity.NameLower}.{relationSide.RelationBeforePreProcessor.PropertyNameInSource}Id.HasValue ? this.Lookup{relationSide.RelationBeforePreProcessor.TargetEntity.Name}{relationSide.RelationBeforePreProcessor.TargetEntity.ScopeEntity.Name}Id({relationSide.Entity.NameLower}.{relationSide.RelationBeforePreProcessor.PropertyNameInSource}Id.Value) : null;");
            }
            else
            {
                stringEditor.InsertLine($"            ef{relationSide.Entity.Name}Dto.{relationSide.Name}Id = this.Lookup{relationSide.RelationBeforePreProcessor.TargetEntity.Name}{relationSide.RelationBeforePreProcessor.TargetEntity.ScopeEntity.Name}Id({relationSide.Entity.NameLower}.{relationSide.RelationBeforePreProcessor.PropertyNameInSource}Id);");
            }

            stringEditor.NextThatContains($"{relationSide.Entity.Name}DtoData.UpdateEf{relationSide.Entity.Name}Dto(");
            stringEditor.NextUntil(line => !line.Contains("this.Lookup"));
            if (relationSide.IsOptional)
            {
                stringEditor.InsertLine($"            ef{relationSide.Entity.Name}Dto.{relationSide.Name}Id = {relationSide.Entity.NameLower}.{relationSide.RelationBeforePreProcessor.PropertyNameInSource}Id.HasValue ? this.Lookup{relationSide.RelationBeforePreProcessor.TargetEntity.Name}{relationSide.RelationBeforePreProcessor.TargetEntity.ScopeEntity.Name}Id({relationSide.Entity.NameLower}.{relationSide.RelationBeforePreProcessor.PropertyNameInSource}Id.Value) : null;");
            }
            else
            {
                stringEditor.InsertLine($"            ef{relationSide.Entity.Name}Dto.{relationSide.Name}Id = this.Lookup{relationSide.RelationBeforePreProcessor.TargetEntity.Name}{relationSide.RelationBeforePreProcessor.TargetEntity.ScopeEntity.Name}Id({relationSide.Entity.NameLower}.{relationSide.RelationBeforePreProcessor.PropertyNameInSource}Id);");
            }

            if (!stringEditor.GetText().Contains($"Guid Lookup{relationSide.RelationBeforePreProcessor.TargetEntity.Name}{relationSide.RelationBeforePreProcessor.TargetEntity.ScopeEntity.Name}Id("))
            {
                stringEditor.NextThatStartsWith("    }");
                stringEditor.InsertLine("");
                stringEditor.InsertLine($"        private Guid Lookup{relationSide.RelationBeforePreProcessor.TargetEntity.Name}{relationSide.RelationBeforePreProcessor.TargetEntity.ScopeEntity.Name}Id(Guid {relationSide.RelationBeforePreProcessor.TargetEntity.NameLower}Id)");
                stringEditor.InsertLine("        {");
                stringEditor.InsertLine($"            Ef{relationSide.RelationBeforePreProcessor.TargetEntity.Name}Dto ef{relationSide.RelationBeforePreProcessor.TargetEntity.Name}Dto = this.dbContext.{relationSide.RelationBeforePreProcessor.TargetEntity.NamePlural}");
                stringEditor.InsertLine($"                .Where(ef{relationSide.RelationBeforePreProcessor.TargetEntity.Name}Dto => ef{relationSide.RelationBeforePreProcessor.TargetEntity.Name}Dto.Id == {relationSide.RelationBeforePreProcessor.TargetEntity.NameLower}Id)");
                stringEditor.InsertLine("                .SingleOrDefault();");
                stringEditor.InsertLine("");
                stringEditor.InsertLine($"            if (ef{relationSide.RelationBeforePreProcessor.TargetEntity.Name}Dto == null)");
                stringEditor.InsertLine("            {");
                stringEditor.InsertLine($"                throw new NotFoundResultException(\"{relationSide.RelationBeforePreProcessor.TargetEntity.NameReadable} ({{id}}) konnte nicht gefunden werden.\", {relationSide.RelationBeforePreProcessor.TargetEntity.NameLower}Id);");
                stringEditor.InsertLine("            }");
                stringEditor.InsertLine("");
                stringEditor.InsertLine($"            return ef{relationSide.RelationBeforePreProcessor.TargetEntity.Name}Dto.{relationSide.RelationBeforePreProcessor.TargetEntity.ScopeEntity.Name}Id;");
                stringEditor.InsertLine("        }");
            }

            return stringEditor.GetText();
        }
    }
}