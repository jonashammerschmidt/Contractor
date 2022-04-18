using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Persistence.Tests
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_PERSISTENCE_TESTS })]
    internal class EntitiesCrudRepositoryTestsGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PersistenceTestsProjectGeneration.TemplateFolder, "EntitiesCrudRepositoryTestsTemplate.txt");

        private static readonly string FileName = "EntitiesCrudRepositoryTests.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly EntitiesCrudRepositoryTestsToOneToOneRelationAddition entitiesCrudRepositoryTestsToOneToOneRelationAddition;

        public EntitiesCrudRepositoryTestsGeneration(
            EntityCoreAddition entityCoreAddition,
            EntitiesCrudRepositoryTestsToOneToOneRelationAddition entitiesCrudRepositoryTestsToOneToOneRelationAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.entitiesCrudRepositoryTestsToOneToOneRelationAddition = entitiesCrudRepositoryTestsToOneToOneRelationAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            string templatePath = TemplateFileName.GetFileNameForEntityAddition(options, TemplatePath);
            this.entityCoreAddition.AddEntityCore(options, PersistenceTestsProjectGeneration.DomainFolder, templatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            this.entitiesCrudRepositoryTestsToOneToOneRelationAddition.Edit(options, PersistenceTestsProjectGeneration.DomainFolder, FileName);
        }
    }
}