using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Model
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_MODEL })]
    internal class IEntityUpdateGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ModelProjectGeneration.TemplateFolder, "i-entity-kebab-update.template.txt");

        private static readonly string FileName = "dtos\\i-entity-kebab-update.ts";

        private readonly FrontendEntityAddition frontendEntityAddition;
        private readonly FrontendDtoPropertyAddition frontendDtoPropertyAddition;
        private readonly FrontendDtoPropertyMethodAddition frontendDtoPropertyMethodAddition;
        private readonly IEntityUpdateMethodAddition entityUpdateMethodAddition;

        public IEntityUpdateGeneration(
            FrontendEntityAddition frontendEntityAddition,
            FrontendDtoPropertyAddition frontendDtoPropertyAddition,
            FrontendDtoPropertyMethodAddition frontendDtoPropertyMethodAddition,
            IEntityUpdateMethodAddition entityUpdateMethodAddition)
        {
            this.frontendEntityAddition = frontendEntityAddition;
            this.frontendDtoPropertyAddition = frontendDtoPropertyAddition;
            this.frontendDtoPropertyMethodAddition = frontendDtoPropertyMethodAddition;
            this.entityUpdateMethodAddition = entityUpdateMethodAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.frontendEntityAddition.AddEntity(entity, ModelProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
            this.frontendDtoPropertyAddition.AddPropertyToDTO(property, ModelProjectGeneration.DomainFolder, FileName);

            this.frontendDtoPropertyMethodAddition.AddPropertyToDTO(property, "fromEntityDetail", "iEntityDetail", ModelProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromGuidRelationEndTo(relation);
            this.frontendDtoPropertyAddition.AddPropertyToDTO(relationSideTo, ModelProjectGeneration.DomainFolder, FileName);
            this.entityUpdateMethodAddition.Edit(relationSideTo, ModelProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            this.Add1ToNRelationSideTo(new Relation1ToN(relation));
        }
    }
}