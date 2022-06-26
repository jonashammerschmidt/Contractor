using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Backend.Persistence
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_PERSISTENCE })]
    internal class DbEntityListItemGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PersistenceProjectGeneration.TemplateFolder, "DbEntityListItemTemplate.txt");

        private static readonly string FileName = "DbEntityListItem.cs";

        private readonly DbEntityListItemMethodsAddition dbDtoListItemMethodsAddition;
        private readonly DbEntityListItemToMethodsAddition dbDtoListItemToMethodsAddition;
        private readonly DbEntityListItemFromOneToOneMethodsAddition dbEntityListItemFromOneToOneMethodsAddition;
        private readonly EntityCoreAddition entityCoreAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;

        public DbEntityListItemGeneration(
            DbEntityListItemMethodsAddition dbDtoListItemMethodsAddition,
            DbEntityListItemToMethodsAddition dbDtoListItemToMethodsAddition,
            DbEntityListItemFromOneToOneMethodsAddition dbEntityListItemFromOneToOneMethodsAddition,
            EntityCoreAddition entityCoreAddition,
            DtoPropertyAddition propertyAddition,
            DtoRelationAddition relationAddition)
        {
            this.dbDtoListItemMethodsAddition = dbDtoListItemMethodsAddition;
            this.dbDtoListItemToMethodsAddition = dbDtoListItemToMethodsAddition;
            this.dbEntityListItemFromOneToOneMethodsAddition = dbEntityListItemFromOneToOneMethodsAddition;
            this.entityCoreAddition = entityCoreAddition;
            this.propertyAddition = propertyAddition;
            this.relationAddition = relationAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.entityCoreAddition.AddEntityToBackend(entity, PersistenceProjectGeneration.DtoFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
            this.propertyAddition.AddPropertyToBackendFile(property, PersistenceProjectGeneration.DtoFolder, FileName);
            this.dbDtoListItemMethodsAddition.AddPropertyToBackendFile(property, PersistenceProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "IDb", "");

            this.relationAddition.AddRelationToDTO(relationSideTo, PersistenceProjectGeneration.DtoFolder, FileName,
                $"{relationSideTo.Entity.Module.Options.Paths.ProjectName}.Contract.Persistence.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");

            this.dbDtoListItemToMethodsAddition.AddRelationSideToBackendFile(relationSideTo, PersistenceProjectGeneration.DtoFolder, FileName);
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "IDb", "");

            this.relationAddition.AddRelationToDTO(relationSideFrom, PersistenceProjectGeneration.DtoFolder, FileName,
                $"{relationSideFrom.Entity.Module.Options.Paths.ProjectName}.Contract.Persistence.Modules.{relationSideFrom.OtherEntity.Module.Name}.{relationSideFrom.OtherEntity.NamePlural}");

            this.dbEntityListItemFromOneToOneMethodsAddition.AddRelationSideToBackendFile(relationSideFrom, PersistenceProjectGeneration.DtoFolder, FileName);
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "IDb", "");

            this.relationAddition.AddRelationToDTO(relationSideTo, PersistenceProjectGeneration.DtoFolder, FileName,
                $"{relationSideTo.Entity.Module.Options.Paths.ProjectName}.Contract.Logic.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");

            this.dbDtoListItemToMethodsAddition.AddRelationSideToBackendFile(relationSideTo, PersistenceProjectGeneration.DtoFolder, FileName);
        }
    }
}