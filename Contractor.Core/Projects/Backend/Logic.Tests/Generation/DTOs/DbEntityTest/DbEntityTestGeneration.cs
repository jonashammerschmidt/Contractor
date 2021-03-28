using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic.Tests
{
    internal class DbEntityTestGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicTestsProjectGeneration.TemplateFolder, "DbEntityTestTemplate.txt");

        private static readonly string FileName = "DbEntityTest.cs";

        private readonly DbEntityTestMethodsAddition logicDbDtoTestMethodsAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;

        public DbEntityTestGeneration(
            DbEntityTestMethodsAddition logicDbDtoTestMethodsAddition,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition)
        {
            this.logicDbDtoTestMethodsAddition = logicDbDtoTestMethodsAddition;
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.dtoAddition.AddDto(options, LogicTestsProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, LogicTestsProjectGeneration.DomainFolder, FileName);
            this.logicDbDtoTestMethodsAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            IRelationSideAdditionOptions guidPropertyOptions = RelationAdditionOptions.GetPropertyForTo(options, "Guid", $"{options.EntityNameFrom}Id");
            PropertyAdditionOptions propertyAdditionOptions = new PropertyAdditionOptions(guidPropertyOptions);

            this.propertyAddition.AddPropertyToDTO(propertyAdditionOptions, LogicTestsProjectGeneration.DomainFolder, FileName);
            this.logicDbDtoTestMethodsAddition.Add(propertyAdditionOptions, LogicTestsProjectGeneration.DomainFolder, FileName);
        }
    }
}