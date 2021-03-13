using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Persistence.Tests
{
    internal class EntityTestValuesGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PersistenceTestsProjectGeneration.TemplateFolder, "EntityTestValuesTemplate.txt");

        private static readonly string FileName = "EntityTestValues.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly DtoTestValuesAddition dtoTestValuesAddition;
        private readonly DtoTestValuesRelationAddition dtoTestValuesRelationAddition;

        public EntityTestValuesGeneration(
            EntityCoreAddition entityCoreAddition,
            DtoTestValuesAddition dtoTestValuesAddition,
            DtoTestValuesRelationAddition dtoTestValuesRelationAddition)
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
            this.dtoTestValuesAddition.Add(options, PersistenceTestsProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // To
            this.dtoTestValuesRelationAddition.Add(options, PersistenceTestsProjectGeneration.DomainFolder, FileName);
        }
    }
}