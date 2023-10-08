using Contractor.Core.BaseClasses;
using Contractor.Core.Generation.Backend.Persistence;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Database.Generated.DbContext
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_PERSISTENCE_DB_CONTEXT })]
    internal class EfEntityDtoGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PersistenceDbContextProjectGeneration.TemplateFolder, "EfEntityDtoTemplate.txt");

        private static readonly string FileName = "EfEntityDto.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly EfDtoEntityAddition efDtoEntityAddition;
        private readonly EfDtoPropertyAddition efDtoPropertyAddition;
        private readonly DtoRelationAddition relationAddition;
        private readonly EfDtoContructorHashSetAddition efDtoContructorHashSetAddition;

        public EfEntityDtoGeneration(
            EntityCoreAddition entityCoreAddition,
            EfDtoEntityAddition efDtoEntityAddition,
            EfDtoPropertyAddition efDtoPropertyAddition,
            DtoRelationAddition relationAddition,
            EfDtoContructorHashSetAddition efDtoContructorHashSetAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.efDtoEntityAddition = efDtoEntityAddition;
            this.efDtoPropertyAddition = efDtoPropertyAddition;
            this.relationAddition = relationAddition;
            this.efDtoContructorHashSetAddition = efDtoContructorHashSetAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            string templatePath = TemplateFileName.GetFileNameForEntityAddition(entity, TemplatePath);
            this.entityCoreAddition.AddEntityToDatabase(entity, PersistenceDbContextProjectGeneration.DtoFolder, templatePath, FileName);
            this.efDtoEntityAddition.Edit(entity, PersistenceDbContextProjectGeneration.DtoFolder, FileName);
        }

        protected override void AddProperty(Property property)
        {
            this.efDtoPropertyAddition.AddPropertyToDTO(property, PersistenceDbContextProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "virtual ICollection<Ef", "Dto>");

            this.relationAddition.AddRelationToDTOForDatabase(relationSideFrom, PersistenceDbContextProjectGeneration.DtoFolder, FileName,
                $"{relationSideFrom.Entity.Module.Options.Paths.DbProjectName}.Generated.DbContext.Modules.{relationSideFrom.OtherEntity.Module.Name}.{relationSideFrom.OtherEntity.NamePlural}");

            this.efDtoContructorHashSetAddition.Add(relationSideFrom, PersistenceDbContextProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideToGuid = RelationSide.FromGuidRelationEndTo(relation);
            this.relationAddition.AddRelationToDTOForDatabase(relationSideToGuid, PersistenceDbContextProjectGeneration.DtoFolder, FileName);

            RelationSide relationSideToEfObject = RelationSide.FromObjectRelationEndTo(relation, "virtual Ef", "Dto");
            this.relationAddition.AddRelationToDTOForDatabase(relationSideToEfObject, PersistenceDbContextProjectGeneration.DtoFolder, FileName,
                $"{relationSideToEfObject.Entity.Module.Options.Paths.DbProjectName}.Generated.DbContext.Modules.{relationSideToEfObject.OtherEntity.Module.Name}.{relationSideToEfObject.OtherEntity.NamePlural}");
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "virtual Ef", "Dto");

            this.relationAddition.AddRelationToDTOForDatabase(relationSideFrom, PersistenceDbContextProjectGeneration.DtoFolder, FileName,
                $"{relationSideFrom.Entity.Module.Options.Paths.DbProjectName}.Generated.DbContext.Modules.{relationSideFrom.OtherEntity.Module.Name}.{relationSideFrom.OtherEntity.NamePlural}");
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            RelationSide relationSideToGuid = RelationSide.FromGuidRelationEndTo(relation);
            this.relationAddition.AddRelationToDTOForDatabase(relationSideToGuid, PersistenceDbContextProjectGeneration.DtoFolder, FileName);

            RelationSide relationSideToEfObject = RelationSide.FromObjectRelationEndTo(relation, "virtual Ef", "Dto");
            this.relationAddition.AddRelationToDTOForDatabase(relationSideToEfObject, PersistenceDbContextProjectGeneration.DtoFolder, FileName,
                $"{relationSideToEfObject.Entity.Module.Options.Paths.DbProjectName}.Generated.DbContext.Modules.{relationSideToEfObject.OtherEntity.Module.Name}.{relationSideToEfObject.OtherEntity.NamePlural}");
        }

        protected override void PostGeneration(Entity entity)
        {
            foreach (var scopedEntity in entity.ScopedEntities)
            {
                RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(entity, scopedEntity, "virtual ICollection<Ef", "Dto>");

                this.relationAddition.AddRelationToDTOForDatabase(relationSideFrom, PersistenceDbContextProjectGeneration.DtoFolder, FileName,
                    $"{relationSideFrom.Entity.Module.Options.Paths.DbProjectName}.Generated.DbContext.Modules.{relationSideFrom.OtherEntity.Module.Name}.{relationSideFrom.OtherEntity.NamePlural}");
            }
        }
    }
}