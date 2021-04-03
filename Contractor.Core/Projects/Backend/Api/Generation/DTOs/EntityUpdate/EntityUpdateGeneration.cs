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
        private readonly ApiDtoPropertyAddition apiPropertyAddition;

        public EntityUpdateGeneration(
            DtoAddition dtoAddition,
            ApiDtoPropertyAddition apiPropertyAddition)
        {
            this.dtoAddition = dtoAddition;
            this.apiPropertyAddition = apiPropertyAddition;
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
            this.apiPropertyAddition.AddPropertyToDTO(options, ApiProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            IRelationSideAdditionOptions relationAdditionOptions =
                RelationAdditionOptions.GetPropertyForTo(options, "Guid", $"{options.EntityNameFrom}Id");
            PropertyAdditionOptions propertyAdditionOptions = new PropertyAdditionOptions(relationAdditionOptions);

            this.apiPropertyAddition.AddPropertyToDTO(propertyAdditionOptions, ApiProjectGeneration.DomainFolder, FileName);
        }
    }
}