using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Api
{
    internal class EntityUpdateGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ApiProjectGeneration.TemplateFolder, "EntityUpdateTemplate.txt");

        private static readonly string FileName = "EntityUpdate.cs";

        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;

        public EntityUpdateGeneration(
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition)
        {
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
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
            IPropertyAdditionOptions propertyAdditionOptions =
                RelationAdditionOptions.GetPropertyForTo(options, "Guid", $"{options.EntityNameFrom}Id");
            this.propertyAddition.AddPropertyToDTO(propertyAdditionOptions, ApiProjectGeneration.DomainFolder, FileName);
        }
    }
}