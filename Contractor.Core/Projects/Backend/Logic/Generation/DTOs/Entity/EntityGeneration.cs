using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_LOGIC })]
    internal class EntityGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicProjectGeneration.TemplateFolder, "EntityTemplate.txt");

        private static readonly string FileName = "Entity.cs";

        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition dtoPropertyAddition;
        private readonly EntityMethodsAddition dtoMethodsAddition;

        public EntityGeneration(
            DtoAddition dtoAddition,
            DtoPropertyAddition dtoPropertyAddition,
            EntityMethodsAddition dtoMethodsAddition)
        {
            this.dtoAddition = dtoAddition;
            this.dtoPropertyAddition = dtoPropertyAddition;
            this.dtoMethodsAddition = dtoMethodsAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.dtoAddition.AddDto(entity, LogicProjectGeneration.DtoFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
            this.dtoPropertyAddition.AddPropertyToDTO(property, LogicProjectGeneration.DtoFolder, FileName);
            this.dtoMethodsAddition.Edit(property, LogicProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelation(Relation1ToN relation)
        {
            // To
            RelationSide relationSide = RelationSide.FromGuidRelationEndTo(relation);
            this.dtoPropertyAddition.AddPropertyToDTO(relationSide, LogicProjectGeneration.DtoFolder, FileName);
            this.dtoMethodsAddition.Edit(relationSide, LogicProjectGeneration.DtoFolder, FileName);
        }

        protected override void AddOneToOneRelation(Relation1To1 relation)
        {
            this.Add1ToNRelation(new Relation1ToN(relation));
        }
    }
}