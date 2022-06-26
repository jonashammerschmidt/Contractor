using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Backend.Logic
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_LOGIC })]
    internal class EntityGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicProjectGeneration.TemplateFolder, "EntityTemplate.txt");

        private static readonly string FileName = "Entity.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly DtoPropertyAddition dtoPropertyAddition;
        private readonly EntityMethodsAddition dtoMethodsAddition;

        public EntityGeneration(
            EntityCoreAddition entityCoreAddition,
            DtoPropertyAddition dtoPropertyAddition,
            EntityMethodsAddition dtoMethodsAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.dtoPropertyAddition = dtoPropertyAddition;
            this.dtoMethodsAddition = dtoMethodsAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.entityCoreAddition.AddEntityToBackend(entity, LogicProjectGeneration.DtoFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
            this.dtoPropertyAddition.AddPropertyToBackendFile(property, LogicProjectGeneration.DtoFolder, FileName);
            this.dtoMethodsAddition.AddPropertyToBackendFile(property, LogicProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSide = RelationSide.FromGuidRelationEndTo(relation);

            this.dtoPropertyAddition.AddPropertyToBackendFile(relationSide, LogicProjectGeneration.DtoFolder, FileName);
            this.dtoMethodsAddition.AddPropertyToBackendFile(relationSide, LogicProjectGeneration.DtoFolder, FileName);
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