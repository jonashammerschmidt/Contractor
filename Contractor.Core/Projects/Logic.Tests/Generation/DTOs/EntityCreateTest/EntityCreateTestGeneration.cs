using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Logic.Tests
{
    internal class EntityCreateTestGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicTestsProjectGeneration.TemplateFolder, "EntityCreateTestTemplate.txt");

        private static readonly string FileName = "EntityCreateTest.cs";

        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly LogicDtoCreateTestMethodsAddition logicDtoCreateTestMethodsAddition;

        public EntityCreateTestGeneration(
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            LogicDtoCreateTestMethodsAddition logicDtoCreateTestMethodsAddition)
        {
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
            this.logicDtoCreateTestMethodsAddition = logicDtoCreateTestMethodsAddition;
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
            this.logicDtoCreateTestMethodsAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            IPropertyAdditionOptions guidPropertyOptions = 
                RelationAdditionOptions.GetPropertyForTo(options, "Guid", $"{options.EntityNameFrom}Id");
            this.propertyAddition.AddPropertyToDTO(guidPropertyOptions, LogicTestsProjectGeneration.DomainFolder, FileName);
            this.logicDtoCreateTestMethodsAddition.Add(guidPropertyOptions, LogicTestsProjectGeneration.DomainFolder, FileName);
        }
    }
}