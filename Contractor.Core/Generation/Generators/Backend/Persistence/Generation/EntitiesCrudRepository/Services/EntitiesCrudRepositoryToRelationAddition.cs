﻿using Contractor.Core.BaseClasses;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Backend.Persistence
{
    internal class EntitiesCrudRepositoryToRelationAddition : RelationSideAdditionToExisitingFileGeneration
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
                $"{relationSide.Entity.Module.Options.Paths.DbProjectName}.Generated.DbContext.Modules.{relationSide.RelationBeforePreProcessor.EntityFrom.Module.Name}.{relationSide.RelationBeforePreProcessor.EntityFrom.NamePlural}");

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains($"{relationSide.Entity.Name}Dto.ToEf{relationSide.Entity.Name}Dto(");
            stringEditor.NextUntil(line => !line.Contains("this.Lookup"));
            if (relationSide.IsOptional)
            {
                stringEditor.InsertLine($"            ef{relationSide.Entity.Name}Dto.{relationSide.Name}Id = {relationSide.Entity.NameLower}.{relationSide.RelationBeforePreProcessor.PropertyNameFrom}Id.HasValue ? this.Lookup{relationSide.RelationBeforePreProcessor.EntityFrom.Name}{relationSide.RelationBeforePreProcessor.EntityFrom.ScopeEntity.Name}Id({relationSide.Entity.NameLower}.{relationSide.RelationBeforePreProcessor.PropertyNameFrom}Id.Value) : null;");
            }
            else
            {
                stringEditor.InsertLine($"            ef{relationSide.Entity.Name}Dto.{relationSide.Name}Id = this.Lookup{relationSide.RelationBeforePreProcessor.EntityFrom.Name}{relationSide.RelationBeforePreProcessor.EntityFrom.ScopeEntity.Name}Id({relationSide.Entity.NameLower}.{relationSide.RelationBeforePreProcessor.PropertyNameFrom}Id);");
            }

            stringEditor.NextThatContains($"{relationSide.Entity.Name}DtoData.UpdateEf{relationSide.Entity.Name}Dto(");
            stringEditor.NextUntil(line => !line.Contains("this.Lookup"));
            if (relationSide.IsOptional)
            {
                stringEditor.InsertLine($"            ef{relationSide.Entity.Name}Dto.{relationSide.Name}Id = {relationSide.Entity.NameLower}.{relationSide.RelationBeforePreProcessor.PropertyNameFrom}Id.HasValue ? this.Lookup{relationSide.RelationBeforePreProcessor.EntityFrom.Name}{relationSide.RelationBeforePreProcessor.EntityFrom.ScopeEntity.Name}Id({relationSide.Entity.NameLower}.{relationSide.RelationBeforePreProcessor.PropertyNameFrom}Id.Value) : null;");
            }
            else
            {
                stringEditor.InsertLine($"            ef{relationSide.Entity.Name}Dto.{relationSide.Name}Id = this.Lookup{relationSide.RelationBeforePreProcessor.EntityFrom.Name}{relationSide.RelationBeforePreProcessor.EntityFrom.ScopeEntity.Name}Id({relationSide.Entity.NameLower}.{relationSide.RelationBeforePreProcessor.PropertyNameFrom}Id);");
            }

            if (!stringEditor.GetText().Contains($"Guid Lookup{relationSide.RelationBeforePreProcessor.EntityFrom.Name}{relationSide.RelationBeforePreProcessor.EntityFrom.ScopeEntity.Name}Id("))
            {
                stringEditor.NextThatStartsWith("    }");
                stringEditor.InsertLine("");
                stringEditor.InsertLine($"        private Guid Lookup{relationSide.RelationBeforePreProcessor.EntityFrom.Name}{relationSide.RelationBeforePreProcessor.EntityFrom.ScopeEntity.Name}Id(Guid {relationSide.RelationBeforePreProcessor.EntityFrom.NameLower}Id)");
                stringEditor.InsertLine("        {");
                stringEditor.InsertLine($"            Ef{relationSide.RelationBeforePreProcessor.EntityFrom.Name}Dto ef{relationSide.RelationBeforePreProcessor.EntityFrom.Name}Dto = this.dbContext.{relationSide.RelationBeforePreProcessor.EntityFrom.NamePlural}");
                stringEditor.InsertLine($"                .Where(ef{relationSide.RelationBeforePreProcessor.EntityFrom.Name}Dto => ef{relationSide.RelationBeforePreProcessor.EntityFrom.Name}Dto.Id == {relationSide.RelationBeforePreProcessor.EntityFrom.NameLower}Id)");
                stringEditor.InsertLine("                .SingleOrDefault();");
                stringEditor.InsertLine("");
                stringEditor.InsertLine($"            if (ef{relationSide.RelationBeforePreProcessor.EntityFrom.Name}Dto == null)");
                stringEditor.InsertLine("            {");
                stringEditor.InsertLine($"                throw new NotFoundResultException(\"{relationSide.RelationBeforePreProcessor.EntityFrom.NameReadable} ({{id}}) konnte nicht gefunden werden.\", {relationSide.RelationBeforePreProcessor.EntityFrom.NameLower}Id);");
                stringEditor.InsertLine("            }");
                stringEditor.InsertLine("");
                stringEditor.InsertLine($"            return ef{relationSide.RelationBeforePreProcessor.EntityFrom.Name}Dto.{relationSide.RelationBeforePreProcessor.EntityFrom.ScopeEntity.Name}Id;");
                stringEditor.InsertLine("        }");
            }

            return stringEditor.GetText();
        }
    }
}