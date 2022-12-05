using Contractor.Core.BaseClasses;
using Contractor.Core.Generation.Backend.Persistence;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;
using Contractor.Core.Generation.Backend.Persistence.DbContext;

namespace Contractor.Core.Generation.Database.Persistence.DbContext
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_PERSISTENCE_DB_CONTEXT })]
    internal class EfEntityGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PersistenceDbContextProjectGeneration.TemplateFolder, "EfEntityTemplate.txt");

        private static readonly string FileName = "EfEntity.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly EfDtoEntityAddition efDtoEntityAddition;
        private readonly EfDtoPropertyAddition efDtoPropertyAddition;
        private readonly EfDtoRelationAddition efRelationAddition;
        private readonly EfDtoConstructorHashSetAddition efDtoConstructorHashSetAddition;

        public EfEntityGeneration(
            EntityCoreAddition entityCoreAddition,
            EfDtoEntityAddition efDtoEntityAddition,
            EfDtoPropertyAddition efDtoPropertyAddition,
            EfDtoRelationAddition efRelationAddition,
            EfDtoConstructorHashSetAddition efDtoConstructorHashSetAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.efDtoEntityAddition = efDtoEntityAddition;
            this.efDtoPropertyAddition = efDtoPropertyAddition;
            this.efRelationAddition = efRelationAddition;
            this.efDtoConstructorHashSetAddition = efDtoConstructorHashSetAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            string templatePath = TemplateFileName.GetFileNameForEntityAddition(entity, TemplatePath);
            this.entityCoreAddition.AddEntityToDatabase(entity, PersistenceDbContextProjectGeneration.DtoFolder,
                templatePath, FileName);
            this.efDtoEntityAddition.Edit(entity, PersistenceDbContextProjectGeneration.DtoFolder, FileName);
        }

        protected override void AddProperty(Property property)
        {
            this.efDtoPropertyAddition.AddPropertyToDto(property, PersistenceDbContextProjectGeneration.DtoFolder,
                FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
            RelationSide relationSideFrom =
                RelationSide.FromObjectRelationEndFrom(relation, "virtual ICollection<Ef", ">");

            this.efRelationAddition.AddRelationToDto(relationSideFrom, PersistenceDbContextProjectGeneration.DtoFolder,
                FileName, true, true,
                $"{relationSideFrom.Entity.Module.Options.Paths.DbProjectName}.Persistence.DbContext.Modules.{relationSideFrom.OtherEntity.Module.Name}.{relationSideFrom.OtherEntity.NamePlural}");

            this.efDtoConstructorHashSetAddition.Add(relationSideFrom, PersistenceDbContextProjectGeneration.DtoFolder,
                FileName);
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideToGuid = RelationSide.FromGuidRelationEndTo(relation);
            this.efRelationAddition.AddRelationToDto(relationSideToGuid,
                PersistenceDbContextProjectGeneration.DtoFolder, FileName, false, false);

            RelationSide relationSideToEfObject = RelationSide.FromObjectRelationEndTo(relation, "virtual Ef", "");
            this.efRelationAddition.AddRelationToDto(relationSideToEfObject,
                PersistenceDbContextProjectGeneration.DtoFolder, FileName, true, false,
                $"{relationSideToEfObject.Entity.Module.Options.Paths.DbProjectName}.Persistence.DbContext.Modules.{relationSideToEfObject.OtherEntity.Module.Name}.{relationSideToEfObject.OtherEntity.NamePlural}");
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "virtual Ef", "");

            this.efRelationAddition.AddRelationToDto(relationSideFrom, PersistenceDbContextProjectGeneration.DtoFolder,
                FileName, true, false,
                $"{relationSideFrom.Entity.Module.Options.Paths.DbProjectName}.Persistence.DbContext.Modules.{relationSideFrom.OtherEntity.Module.Name}.{relationSideFrom.OtherEntity.NamePlural}");
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            RelationSide relationSideToGuid = RelationSide.FromGuidRelationEndTo(relation);
            this.efRelationAddition.AddRelationToDto(relationSideToGuid,
                PersistenceDbContextProjectGeneration.DtoFolder, FileName, false, false);

            RelationSide relationSideToEfObject = RelationSide.FromObjectRelationEndTo(relation, "virtual Ef", "");
            this.efRelationAddition.AddRelationToDto(relationSideToEfObject,
                PersistenceDbContextProjectGeneration.DtoFolder, FileName, true, false,
                $"{relationSideToEfObject.Entity.Module.Options.Paths.DbProjectName}.Persistence.DbContext.Modules.{relationSideToEfObject.OtherEntity.Module.Name}.{relationSideToEfObject.OtherEntity.NamePlural}");
        }

        protected override void PostGeneration(Entity entity)
        {
            foreach (var scopedEntity in entity.ScopedEntities)
            {
                RelationSide relationSideFrom =
                    RelationSide.FromObjectRelationEndFrom(entity, scopedEntity, "virtual ICollection<Ef", ">");

                this.efRelationAddition.AddRelationToDto(relationSideFrom,
                    PersistenceDbContextProjectGeneration.DtoFolder, FileName, true, true,
                    $"{relationSideFrom.Entity.Module.Options.Paths.DbProjectName}.Persistence.DbContext.Modules.{relationSideFrom.OtherEntity.Module.Name}.{relationSideFrom.OtherEntity.NamePlural}");
            }
        }
    }
}