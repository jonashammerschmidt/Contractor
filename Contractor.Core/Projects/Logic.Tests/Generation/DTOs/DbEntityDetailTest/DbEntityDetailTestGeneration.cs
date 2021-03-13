using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Logic.Tests
{
    internal class DbEntityDetailTestGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicTestsProjectGeneration.TemplateFolder, "DbEntityDetailTestTemplate.txt");

        private static readonly string FileName = "DbEntityDetailTest.cs";

        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly LogicDbDtoDetailTestFromAssertAddition logicDbDtoDetailTestFromAssertAddition;
        private readonly LogicDbDtoDetailTestToAssertAddition logicDbDtoDetailTestToAssertAddition;
        private readonly LogicDbDtoDetailTestMethodsAddition logicDbDtoDetailTestMethodsAddition;

        public DbEntityDetailTestGeneration(
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            LogicDbDtoDetailTestFromAssertAddition logicDbDtoDetailTestFromAssertAddition,
            LogicDbDtoDetailTestToAssertAddition logicDbDtoDetailTestToAssertAddition,
            LogicDbDtoDetailTestMethodsAddition logicDbDtoDetailTestMethodsAddition)
        {
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
            this.logicDbDtoDetailTestFromAssertAddition = logicDbDtoDetailTestFromAssertAddition;
            this.logicDbDtoDetailTestToAssertAddition = logicDbDtoDetailTestToAssertAddition;
            this.logicDbDtoDetailTestMethodsAddition = logicDbDtoDetailTestMethodsAddition;
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
            this.logicDbDtoDetailTestMethodsAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // From
            IPropertyAdditionOptions dbFromOptions = RelationAdditionOptions.GetPropertyForFrom(options, $"IEnumerable<IDb{options.EntityNameTo}>", $"{options.EntityNamePluralTo}");
            this.propertyAddition.AddPropertyToDTO(dbFromOptions, LogicTestsProjectGeneration.DomainFolder, FileName);
            this.logicDbDtoDetailTestFromAssertAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);

            // To
            IPropertyAdditionOptions dbToOptions = RelationAdditionOptions.GetPropertyForTo(options, $"IDb{options.EntityNameFrom}", $"{options.EntityNameFrom}");
            this.propertyAddition.AddPropertyToDTO(dbToOptions, LogicTestsProjectGeneration.DomainFolder, FileName);
            this.logicDbDtoDetailTestToAssertAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);
        }
    }
}