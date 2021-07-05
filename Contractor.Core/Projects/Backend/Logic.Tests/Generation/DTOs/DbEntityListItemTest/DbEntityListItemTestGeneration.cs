using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic.Tests
{
    internal class DbEntityListItemTestGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicTestsProjectGeneration.TemplateFolder, "DbEntityListItemTestTemplate.txt");

        private static readonly string FileName = "DbEntityListItemTest.cs";

        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;
        private readonly DbEntityListItemTestToAssertAddition logicDbDtoListItemTestToAssertAddition;
        private readonly DbEntityListItemTestMethodsAddition logicDbDtoListItemTestMethodsAddition;

        public DbEntityListItemTestGeneration(
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            DtoRelationAddition relationAddition,
            DbEntityListItemTestToAssertAddition logicDbDtoListItemTestToAssertAddition,
            DbEntityListItemTestMethodsAddition logicDbDtoListItemTestMethodsAddition)
        {
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
            this.relationAddition = relationAddition;
            this.logicDbDtoListItemTestToAssertAddition = logicDbDtoListItemTestToAssertAddition;
            this.logicDbDtoListItemTestMethodsAddition = logicDbDtoListItemTestMethodsAddition;
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
            this.logicDbDtoListItemTestMethodsAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // To
            IRelationSideAdditionOptions dbToOptions = RelationAdditionOptions.GetPropertyForTo(options, $"IDb{options.EntityNameFrom}");
            this.relationAddition.AddRelationToDTO(dbToOptions, LogicTestsProjectGeneration.DomainFolder, FileName);
            this.logicDbDtoListItemTestToAssertAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);
        }
    }
}