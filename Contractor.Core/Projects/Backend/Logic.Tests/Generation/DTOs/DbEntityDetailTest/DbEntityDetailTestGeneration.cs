using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic.Tests
{
    internal class DbEntityDetailTestGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicTestsProjectGeneration.TemplateFolder, "DbEntityDetailTestTemplate.txt");

        private static readonly string FileName = "DbEntityDetailTest.cs";

        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;
        private readonly DbEntityDetailTestFromAssertAddition logicDbDtoDetailTestFromAssertAddition;
        private readonly DbEntityDetailTestFromAssertOneToOneAddition dbEntityDetailTestFromAssertOneToOneAddition;
        private readonly DbEntityDetailTestToAssertAddition logicDbDtoDetailTestToAssertAddition;
        private readonly DbEntityDetailTestMethodsAddition logicDbDtoDetailTestMethodsAddition;

        public DbEntityDetailTestGeneration(
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            DtoRelationAddition relationAddition,
            DbEntityDetailTestFromAssertAddition logicDbDtoDetailTestFromAssertAddition,
            DbEntityDetailTestFromAssertOneToOneAddition dbEntityDetailTestFromAssertOneToOneAddition,
            DbEntityDetailTestToAssertAddition logicDbDtoDetailTestToAssertAddition,
            DbEntityDetailTestMethodsAddition logicDbDtoDetailTestMethodsAddition)
        {
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
            this.relationAddition = relationAddition;
            this.logicDbDtoDetailTestFromAssertAddition = logicDbDtoDetailTestFromAssertAddition;
            this.dbEntityDetailTestFromAssertOneToOneAddition = dbEntityDetailTestFromAssertOneToOneAddition;
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
            IRelationSideAdditionOptions dbFromOptions = RelationAdditionOptions.GetPropertyForFrom(options, $"IEnumerable<IDb{options.EntityNameTo}>");
            this.relationAddition.AddRelationToDTO(dbFromOptions, LogicTestsProjectGeneration.DomainFolder, FileName);
            this.logicDbDtoDetailTestFromAssertAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);

            // To
            IRelationSideAdditionOptions dbToOptions = RelationAdditionOptions.GetPropertyForTo(options, $"IDb{options.EntityNameFrom}");
            this.relationAddition.AddRelationToDTO(dbToOptions, LogicTestsProjectGeneration.DomainFolder, FileName);
            this.logicDbDtoDetailTestToAssertAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            // From ------------- TODO --------------
            IRelationSideAdditionOptions dbFromOptions = RelationAdditionOptions.GetPropertyForFrom(options, $"IDb{options.EntityNameTo}");
            this.relationAddition.AddRelationToDTO(dbFromOptions, LogicTestsProjectGeneration.DomainFolder, FileName);
            this.dbEntityDetailTestFromAssertOneToOneAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);

            // To
            IRelationSideAdditionOptions dbToOptions = RelationAdditionOptions.GetPropertyForTo(options, $"IDb{options.EntityNameFrom}");
            this.relationAddition.AddRelationToDTO(dbToOptions, LogicTestsProjectGeneration.DomainFolder, FileName);
            this.logicDbDtoDetailTestToAssertAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);
        }
    }
}