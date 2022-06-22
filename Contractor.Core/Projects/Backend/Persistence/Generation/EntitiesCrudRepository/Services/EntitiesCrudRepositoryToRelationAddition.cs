using Contractor.Core.Helpers;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Backend.Persistence
{
    internal class EntitiesCrudRepositoryToRelationAddition : RelationAdditionEditor
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
                $"{relationSide.Entity.Module.Options.Paths.DbProjectName}.Persistence.DbContext.Modules.{relationSide.RelationBeforePreProcessor.EntityFrom.Module.Name}.{relationSide.RelationBeforePreProcessor.EntityFrom.NamePlural}");

            StringEditor stringEditor = new StringEditor(fileData);

            stringEditor.NextThatContains($"Db{relationSide.Entity.Name}.ToEf{relationSide.Entity.Name}(");
            stringEditor.NextUntil(line => !line.Contains("this.Lookup"));
            stringEditor.InsertLine($"            ef{relationSide.Entity.Name}.{relationSide.Name}Id = this.Lookup{relationSide.RelationBeforePreProcessor.EntityFrom.Name}{relationSide.RelationBeforePreProcessor.EntityFrom.ScopeEntity.Name}Id(db{relationSide.Entity.Name}.{relationSide.RelationBeforePreProcessor.PropertyNameFrom}Id);");

            stringEditor.NextThatContains($"Db{relationSide.Entity.Name}.UpdateEf{relationSide.Entity.Name}(");
            stringEditor.NextUntil(line => !line.Contains("this.Lookup"));
            stringEditor.InsertLine($"            ef{relationSide.Entity.Name}.{relationSide.Name}Id = this.Lookup{relationSide.RelationBeforePreProcessor.EntityFrom.Name}{relationSide.RelationBeforePreProcessor.EntityFrom.ScopeEntity.Name}Id(db{relationSide.Entity.Name}Update.{relationSide.RelationBeforePreProcessor.PropertyNameFrom}Id);");

            if (!stringEditor.GetText().Contains($"Guid Lookup{relationSide.RelationBeforePreProcessor.EntityFrom.Name}{relationSide.RelationBeforePreProcessor.EntityFrom.ScopeEntity.Name}Id("))
            {
                stringEditor.NextThatStartsWith("    }");
                stringEditor.InsertLine("");
                stringEditor.InsertLine($"        private Guid Lookup{relationSide.RelationBeforePreProcessor.EntityFrom.Name}{relationSide.RelationBeforePreProcessor.EntityFrom.ScopeEntity.Name}Id(Guid {relationSide.RelationBeforePreProcessor.EntityFrom.NameLower}Id)");
                stringEditor.InsertLine("        {");
                stringEditor.InsertLine($"            Ef{relationSide.RelationBeforePreProcessor.EntityFrom.Name} ef{relationSide.RelationBeforePreProcessor.EntityFrom.Name} = this.dbContext.{relationSide.RelationBeforePreProcessor.EntityFrom.NamePlural}");
                stringEditor.InsertLine($"                .Where(ef{relationSide.RelationBeforePreProcessor.EntityFrom.Name} => ef{relationSide.RelationBeforePreProcessor.EntityFrom.Name}.Id == {relationSide.RelationBeforePreProcessor.EntityFrom.NameLower}Id)");
                stringEditor.InsertLine("                .SingleOrDefault();");
                stringEditor.InsertLine("");
                stringEditor.InsertLine($"            if (ef{relationSide.RelationBeforePreProcessor.EntityFrom.Name} == null)");
                stringEditor.InsertLine("            {");
                stringEditor.InsertLine($"                throw new NotFoundResultException(\"{relationSide.RelationBeforePreProcessor.EntityFrom.NameReadable} ({{id}}) konnte nicht gefunden werden.\", {relationSide.RelationBeforePreProcessor.EntityFrom.NameLower}Id);");
                stringEditor.InsertLine("            }");
                stringEditor.InsertLine("");
                stringEditor.InsertLine($"            return ef{relationSide.RelationBeforePreProcessor.EntityFrom.Name}.MandantId;");
                stringEditor.InsertLine("        }");
            }

            return stringEditor.GetText();
        }
    }
}