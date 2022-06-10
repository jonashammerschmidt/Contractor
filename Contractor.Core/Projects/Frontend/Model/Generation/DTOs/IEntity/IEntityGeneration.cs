using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Model
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_MODEL })]
    internal class IEntityGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ModelProjectGeneration.TemplateFolder, "i-entity-kebab.template.txt");

        private static readonly string FileName = "dtos\\i-entity-kebab.ts";

        private readonly FrontendEntityAddition frontendEntityAddition;
        private readonly FrontendDtoPropertyAddition frontendDtoPropertyAddition;

        public IEntityGeneration(
            FrontendEntityAddition frontendEntityAddition,
            FrontendDtoPropertyAddition frontendDtoPropertyAddition)
        {
            this.frontendEntityAddition = frontendEntityAddition;
            this.frontendDtoPropertyAddition = frontendDtoPropertyAddition;
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
        }

        protected override void Add1ToNRelation(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromGuidRelationEndTo(relation);
            this.frontendDtoPropertyAddition.AddPropertyToDTO(relationSideTo, ModelProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelation(Relation1To1 relation)
        {
            this.Add1ToNRelation(new Relation1ToN(relation));
        }
    }
}