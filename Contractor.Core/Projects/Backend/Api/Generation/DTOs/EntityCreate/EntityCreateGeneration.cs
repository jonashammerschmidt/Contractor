using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Api
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_API })]
    internal class EntityCreateGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ApiProjectGeneration.TemplateFolder, "EntityCreateTemplate.txt");

        private static readonly string FileName = "EntityCreate.cs";

        private readonly DtoAddition dtoAddition;
        private readonly ApiDtoPropertyAddition apiPropertyAddition;

        public EntityCreateGeneration(
            DtoAddition dtoAddition,
            ApiDtoPropertyAddition apiPropertyAddition)
        {
            this.dtoAddition = dtoAddition;
            this.apiPropertyAddition = apiPropertyAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.dtoAddition.AddDto(entity, ApiProjectGeneration.DtoFolder , TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
            this.apiPropertyAddition.AddPropertyToDTO(property, ApiProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSide = RelationSide.FromGuidRelationEndTo(relation);
            this.apiPropertyAddition.AddPropertyToDTO(relationSide, ApiProjectGeneration.DtoFolder, FileName);
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