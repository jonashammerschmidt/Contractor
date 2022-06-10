using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Persistence
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_PERSISTENCE })]
    internal class EntitiesCrudRepositoryGeneration : ClassGeneration
    {
        public static readonly string TemplatePath =
            Path.Combine(PersistenceProjectGeneration.TemplateFolder, "EntitiesCrudRepositoryTemplate.txt");

        public static readonly string FileName = "EntitiesCrudRepository.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly EntitiesCrudRepositoryFromIncludeAddition dtoFromRepositoryIncludeAddition;
        private readonly EntitiesCrudRepositoryToIncludeAddition dtoToRepositoryIncludeAddition;
        private readonly EntitiesCrudRepositoryFromOneToOneIncludeAddition entitiesCrudRepositoryFromOneToOneIncludeAddition;
        private readonly EntitiesCrudRepositoryToOneToOneIncludeAddition entitiesCrudRepositoryToOneToOneIncludeAddition;

        public EntitiesCrudRepositoryGeneration(
            EntityCoreAddition entityCoreAddition,
            EntitiesCrudRepositoryFromIncludeAddition dtoFromRepositoryIncludeAddition,
            EntitiesCrudRepositoryToIncludeAddition dtoToRepositoryIncludeAddition,
            EntitiesCrudRepositoryFromOneToOneIncludeAddition entitiesCrudRepositoryFromOneToOneIncludeAddition,
            EntitiesCrudRepositoryToOneToOneIncludeAddition entitiesCrudRepositoryToOneToOneIncludeAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.dtoFromRepositoryIncludeAddition = dtoFromRepositoryIncludeAddition;
            this.dtoToRepositoryIncludeAddition = dtoToRepositoryIncludeAddition;
            this.entitiesCrudRepositoryFromOneToOneIncludeAddition = entitiesCrudRepositoryFromOneToOneIncludeAddition;
            this.entitiesCrudRepositoryToOneToOneIncludeAddition = entitiesCrudRepositoryToOneToOneIncludeAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            string templatePath = TemplateFileName.GetFileNameForEntityAddition(entity, TemplatePath);
            this.entityCoreAddition.AddEntityCore(entity, PersistenceProjectGeneration.DomainFolder, templatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
        }

        protected override void Add1ToNRelation(Relation1ToN relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "", "");
            RelationSide relationSideTo = RelationSide.FromGuidRelationEndTo(relation);

            // From
            this.dtoFromRepositoryIncludeAddition.Edit(relationSideFrom, PersistenceProjectGeneration.DomainFolder, FileName);

            // To
            this.dtoToRepositoryIncludeAddition.Edit(relationSideTo, PersistenceProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelation(Relation1To1 relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "", "");
            RelationSide relationSideTo = RelationSide.FromGuidRelationEndTo(relation);

            // From
            this.entitiesCrudRepositoryFromOneToOneIncludeAddition.Edit(relationSideFrom, PersistenceProjectGeneration.DomainFolder, FileName);

            // To
            this.entitiesCrudRepositoryToOneToOneIncludeAddition.Edit(relationSideTo, PersistenceProjectGeneration.DomainFolder, FileName);
        }
    }
}