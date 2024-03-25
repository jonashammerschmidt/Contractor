using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using System.IO;

namespace Contractor.Core.Generation.Backend.Generated.Persistence
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_GENERATED, ClassGenerationTag.BACKEND_GENERATED_PERSISTENCE })]
    public class IEntitiesReadRepositoryDefaultGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(GeneratedPersistenceProjectGeneration.TemplateFolder, "IEntitiesReadRepositoryDefaultTemplate.txt");

        private static readonly string FileName = "IEntitiesReadRepositoryDefault.cs";

        private readonly EntityCoreAddition entityCoreAddition;

        public IEntitiesReadRepositoryDefaultGeneration(
            EntityCoreAddition entityCoreAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.entityCoreAddition.AddEntityToBackendGenerated(entity, GeneratedPersistenceProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
        }
    }
}