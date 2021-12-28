using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Persistence.Tests
{
    internal class DbEntityTestGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PersistenceTestsProjectGeneration.TemplateFolder, "DbEntityTestTemplate.txt");

        private static readonly string FileName = "DbEntityTest.cs";

        private readonly DbEntityTestMethodsAddition dbDtoTestMethodsAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;

        public DbEntityTestGeneration(
            DbEntityTestMethodsAddition dbDtoTestMethodsAddition,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            DtoRelationAddition relationAddition)
        {
            this.dbDtoTestMethodsAddition = dbDtoTestMethodsAddition;
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
            this.relationAddition = relationAddition;
        }

        public DbEntityTestGeneration()
        {
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.dtoAddition.AddDto(options, PersistenceTestsProjectGeneration.DtoFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, PersistenceTestsProjectGeneration.DtoFolder, FileName);
            this.dbDtoTestMethodsAddition.Edit(options, PersistenceTestsProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // To
            IRelationSideAdditionOptions guidPropertyOptions =
                RelationAdditionOptions.GetPropertyForTo(options, "Guid");
            this.relationAddition.AddRelationToDTO(guidPropertyOptions, PersistenceTestsProjectGeneration.DtoFolder, FileName);

            PropertyAdditionOptions propertyAdditionOptions = new PropertyAdditionOptions(guidPropertyOptions);
            this.dbDtoTestMethodsAddition.Edit(propertyAdditionOptions, PersistenceTestsProjectGeneration.DtoFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            this.Add1ToNRelation(options);
        }
    }
}