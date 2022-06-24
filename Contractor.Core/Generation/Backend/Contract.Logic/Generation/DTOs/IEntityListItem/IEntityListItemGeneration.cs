using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Backend.Contract.Logic
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_CONTRACT_LOGIC })]
    internal class IEntityListItemGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ContractLogicProjectGeneration.TemplateFolder, "IEntityListItemTemplate.txt");

        private static readonly string FileName = "IEntityListItem.cs";

        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;

        public IEntityListItemGeneration(
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            DtoRelationAddition relationAddition)
        {
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
            this.relationAddition = relationAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.dtoAddition.AddDto(entity, ContractLogicProjectGeneration.DtoFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
            this.propertyAddition.AddPropertyToDTO(property, ContractLogicProjectGeneration.DtoFolder, FileName, true);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "");

            this.relationAddition.AddRelationToDTO(relationSideTo, ContractLogicProjectGeneration.DtoFolder, FileName, true,
                $"{relationSideTo.Entity.Module.Options.Paths.ProjectName}.Contract.Logic.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "I", "");

            this.relationAddition.AddRelationToDTO(relationSideFrom, ContractLogicProjectGeneration.DtoFolder, FileName, true,
                $"{relationSideFrom.Entity.Module.Options.Paths.ProjectName}.Contract.Logic.Modules.{relationSideFrom.OtherEntity.Module.Name}.{relationSideFrom.OtherEntity.NamePlural}");
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "");
            this.relationAddition.AddRelationToDTO(relationSideTo, ContractLogicProjectGeneration.DtoFolder, FileName, true,
                $"{relationSideTo.Entity.Module.Options.Paths.ProjectName}.Contract.Logic.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");
        }
    }
}