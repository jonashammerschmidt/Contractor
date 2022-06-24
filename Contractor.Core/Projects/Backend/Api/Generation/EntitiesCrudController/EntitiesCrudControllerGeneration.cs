using Contractor.Core.MetaModell;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Api
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_API })]
    internal class EntitiesCrudController : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ApiProjectGeneration.TemplateFolder, "EntitiesCrudControllerTemplate.txt");

        private static readonly string FileName = "EntitiesCrudController.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly EntitiesCrudControllerRelationAddition controllerRelationAddition;

        public EntitiesCrudController(
            EntityCoreAddition entityCoreAddition,
            EntitiesCrudControllerRelationAddition controllerRelationAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.controllerRelationAddition = controllerRelationAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.entityCoreAddition.AddEntityCore(entity, ApiProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromGuidRelationEndTo(relation);
            this.controllerRelationAddition.AddRelationSideToBackendFile(relationSideTo, ApiProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
        }
    }
}