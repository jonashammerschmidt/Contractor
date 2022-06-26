using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Backend.Logic
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_LOGIC })]
    internal class EntityListItemGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicProjectGeneration.TemplateFolder, "EntityListItemTemplate.txt");

        private static readonly string FileName = "EntityListItem.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly DtoPropertyAddition dtoPropertyAddition;
        private readonly DtoRelationAddition relationAddition;
        private readonly EntityListItemMethodsAddition dtoListItemMethodsAddition;
        private readonly EntityListItemToMethodsAddition dtoListItemToMethodsAddition;
        private readonly EntityListItemFromOneToOneMethodsAddition entityListItemFromOneToOneMethodsAddition;

        public EntityListItemGeneration(
            EntityCoreAddition entityCoreAddition,
            DtoPropertyAddition dtoPropertyAddition,
            DtoRelationAddition relationAddition,
            EntityListItemMethodsAddition dtoListItemMethodsAddition,
            EntityListItemToMethodsAddition dtoListItemToMethodsAddition,
            EntityListItemFromOneToOneMethodsAddition entityListItemFromOneToOneMethodsAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.dtoPropertyAddition = dtoPropertyAddition;
            this.relationAddition = relationAddition;
            this.dtoListItemMethodsAddition = dtoListItemMethodsAddition;
            this.dtoListItemToMethodsAddition = dtoListItemToMethodsAddition;
            this.entityListItemFromOneToOneMethodsAddition = entityListItemFromOneToOneMethodsAddition;
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
            this.dtoListItemMethodsAddition.AddPropertyToBackendFile(property, LogicProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "");

            this.relationAddition.AddRelationToDTO(relationSideTo, LogicProjectGeneration.DtoFolder, FileName,
                $"{relationSideTo.Entity.Module.Options.Paths.ProjectName}.Contract.Logic.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");
            this.dtoListItemToMethodsAddition.AddRelationSideToBackendFile(relationSideTo, LogicProjectGeneration.DtoFolder, FileName);
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "I", "");

            this.relationAddition.AddRelationToDTO(relationSideFrom, LogicProjectGeneration.DtoFolder, FileName,
                $"{relationSideFrom.Entity.Module.Options.Paths.ProjectName}.Contract.Logic.Modules.{relationSideFrom.OtherEntity.Module.Name}.{relationSideFrom.OtherEntity.NamePlural}");
            this.entityListItemFromOneToOneMethodsAddition.AddRelationSideToBackendFile(relationSideFrom, LogicProjectGeneration.DtoFolder, FileName);
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "");

            this.relationAddition.AddRelationToDTO(relationSideTo, LogicProjectGeneration.DtoFolder, FileName,
                $"{relationSideTo.Entity.Module.Options.Paths.ProjectName}.Contract.Logic.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");
            this.dtoListItemToMethodsAddition.AddRelationSideToBackendFile(relationSideTo, LogicProjectGeneration.DtoFolder, FileName);
        }
    }
}