using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_LOGIC })]
    internal class EntityDetailGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicProjectGeneration.TemplateFolder, "EntityDetailTemplate.txt");

        private static readonly string FileName = "EntityDetail.cs";

        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition dtoPropertyAddition;
        private readonly DtoRelationAddition relationAddition;
        private readonly EntityDetailMethodsAddition dtoDetailMethodsAddition;
        private readonly EntityDetailFromMethodsAddition dtoDetailFromMethodsAddition;
        private readonly EntityDetailToMethodsAddition dtoDetailToMethodsAddition;
        private readonly EntityDetailFromOneToOneMethodsAddition entityDetailFromOneToOneMethodsAddition;

        public EntityDetailGeneration(
            DtoAddition dtoAddition,
            DtoPropertyAddition dtoPropertyAddition,
            DtoRelationAddition relationAddition,
            EntityDetailMethodsAddition dtoDetailMethodsAddition,
            EntityDetailFromMethodsAddition dtoDetailFromMethodsAddition,
            EntityDetailToMethodsAddition dtoDetailToMethodsAddition,
            EntityDetailFromOneToOneMethodsAddition entityDetailFromOneToOneMethodsAddition)
        {
            this.dtoAddition = dtoAddition;
            this.dtoPropertyAddition = dtoPropertyAddition;
            this.relationAddition = relationAddition;
            this.dtoDetailMethodsAddition = dtoDetailMethodsAddition;
            this.dtoDetailFromMethodsAddition = dtoDetailFromMethodsAddition;
            this.dtoDetailToMethodsAddition = dtoDetailToMethodsAddition;
            this.entityDetailFromOneToOneMethodsAddition = entityDetailFromOneToOneMethodsAddition;
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
            this.dtoDetailMethodsAddition.Edit(property, LogicProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelation(Relation1ToN relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "IEnumerable<I", ">");
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "");

            // From
            this.relationAddition.AddRelationToDTO(relationSideFrom, LogicProjectGeneration.DtoFolder, FileName,
                $"{relationSideFrom.Entity.Module.Options.Paths.ProjectName}.Contract.Logic.Modules.{relationSideFrom.OtherEntity.Module.Name}.{relationSideFrom.OtherEntity.NamePlural}");
            this.dtoDetailFromMethodsAddition.Edit(relationSideFrom, LogicProjectGeneration.DtoFolder, FileName,
                $"{relationSideFrom.Entity.Module.Options.Paths.ProjectName}.Logic.Modules.{relationSideFrom.OtherEntity.Module.Name}.{relationSideFrom.OtherEntity.NamePlural}");

            // To
            this.relationAddition.AddRelationToDTO(relationSideTo, LogicProjectGeneration.DtoFolder, FileName,
                $"{relationSideTo.Entity.Module.Options.Paths.ProjectName}.Contract.Logic.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");
            this.dtoDetailToMethodsAddition.Edit(relationSideTo, LogicProjectGeneration.DtoFolder, FileName);
        }

        protected override void AddOneToOneRelation(Relation1To1 relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "I", "");
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "");

            // From
            this.relationAddition.AddRelationToDTO(relationSideFrom, LogicProjectGeneration.DtoFolder, FileName,
                $"{relationSideFrom.Entity.Module.Options.Paths.ProjectName}.Contract.Logic.Modules.{relationSideFrom.OtherEntity.Module.Name}.{relationSideFrom.OtherEntity.NamePlural}");
            this.entityDetailFromOneToOneMethodsAddition.Edit(relationSideFrom, LogicProjectGeneration.DtoFolder, FileName,
                $"{relationSideFrom.Entity.Module.Options.Paths.ProjectName}.Logic.Modules.{relationSideFrom.OtherEntity.Module.Name}.{relationSideFrom.OtherEntity.NamePlural}");

            // To
            this.relationAddition.AddRelationToDTO(relationSideTo, LogicProjectGeneration.DtoFolder, FileName,
                $"{relationSideTo.Entity.Module.Options.Paths.ProjectName}.Contract.Logic.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");
            this.dtoDetailToMethodsAddition.Edit(relationSideTo, LogicProjectGeneration.DtoFolder, FileName);
        }
    }
}