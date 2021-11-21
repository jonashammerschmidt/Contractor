using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Model
{
    internal class IEntityCreateGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ModelProjectGeneration.TemplateFolder, "i-entity-kebab-create.template.txt");

        private static readonly string FileName = "dtos\\i-entity-kebab-create.ts";

        private readonly FrontendDtoAddition frontendDtoAddition;
        private readonly FrontendDtoPropertyAddition frontendDtoPropertyAddition;

        public IEntityCreateGeneration(
            FrontendDtoAddition frontendDtoAddition,
            FrontendDtoPropertyAddition frontendDtoPropertyAddition)
        {
            this.frontendDtoAddition = frontendDtoAddition;
            this.frontendDtoPropertyAddition = frontendDtoPropertyAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.frontendDtoAddition.AddDto(options, ModelProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.frontendDtoPropertyAddition.AddPropertyToDTO(options, ModelProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            IRelationSideAdditionOptions toOptions =
                RelationAdditionOptions.GetPropertyForTo(options, "Guid");
            PropertyAdditionOptions propertyAdditionOptions = new PropertyAdditionOptions(toOptions);

            this.frontendDtoPropertyAddition.AddPropertyToDTO(propertyAdditionOptions, ModelProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            this.Add1ToNRelation(options);
        }
    }
}