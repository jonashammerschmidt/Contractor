using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_LOGIC })]
    internal class EntitiesCrudLogicGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicProjectGeneration.TemplateFolder, "EntitiesCrudLogicTemplate.txt");

        private static readonly string FileName = "EntitiesCrudLogic.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly EntitiesCrudLogicRelationAddition logicRelationAddition;
        private readonly UniqueEntitiesCrudLogicRelationAddition uniqueLogicRelationAddition;

        public EntitiesCrudLogicGeneration(
            EntityCoreAddition entityCoreAddition,
            EntitiesCrudLogicRelationAddition logicRelationAddition,
            UniqueEntitiesCrudLogicRelationAddition uniquelogicRelationAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.logicRelationAddition = logicRelationAddition;
            this.uniqueLogicRelationAddition = uniquelogicRelationAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.entityCoreAddition.AddEntityCore(entity, LogicProjectGeneration.DomainFolder, TemplatePath, FileName);
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
            this.logicRelationAddition.Edit(relationSideTo, LogicProjectGeneration.DomainFolder, FileName,
                $"{relationSideTo.Entity.Module.Options.Paths.ProjectName}.Contract.Persistence.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            RelationSide relationSideTo = RelationSide.FromGuidRelationEndTo(relation);
            this.uniqueLogicRelationAddition.Edit(relationSideTo, LogicProjectGeneration.DomainFolder, FileName,
                $"{relationSideTo.Entity.Module.Options.Paths.ProjectName}.Contract.Persistence.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");
        }
    }
}