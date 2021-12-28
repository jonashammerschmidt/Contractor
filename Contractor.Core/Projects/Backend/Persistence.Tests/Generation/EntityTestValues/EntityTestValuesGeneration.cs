using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Persistence.Tests
{
    internal class EntityTestValuesGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PersistenceTestsProjectGeneration.TemplateFolder, "EntityTestValuesTemplate.txt");

        private static readonly string FileName = "EntityTestValues.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly EntityTestValuesAddition dtoTestValuesAddition;
        private readonly EntityTestValuesRelationAddition dtoTestValuesRelationAddition;

        public EntityTestValuesGeneration(
            EntityCoreAddition entityCoreAddition,
            EntityTestValuesAddition dtoTestValuesAddition,
            EntityTestValuesRelationAddition dtoTestValuesRelationAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.dtoTestValuesAddition = dtoTestValuesAddition;
            this.dtoTestValuesRelationAddition = dtoTestValuesRelationAddition;
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
            this.dtoTestValuesAddition.Edit(options, PersistenceTestsProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // To
            this.dtoTestValuesRelationAddition.Edit(options, PersistenceTestsProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            this.Add1ToNRelation(options);
        }
    }
}