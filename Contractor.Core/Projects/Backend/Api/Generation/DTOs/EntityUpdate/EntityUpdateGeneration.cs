using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Api
{
    internal class EntityUpdateGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ApiProjectGeneration.TemplateFolder, "EntityUpdateTemplate.txt");

        private static readonly string FileName = "EntityUpdate.cs";

        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;

        public EntityUpdateGeneration(
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            DtoRelationAddition relationAddition)
        {
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
            this.relationAddition = relationAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.dtoAddition.AddDto(options, ApiProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, ApiProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            IRelationSideAdditionOptions relationSideAdditionOptions =
                RelationAdditionOptions.GetPropertyForTo(options, "Guid", $"{options.EntityNameFrom}Id");
            this.relationAddition.AddRelationToDTO(relationSideAdditionOptions, ApiProjectGeneration.DomainFolder, FileName);
        }
    }
}