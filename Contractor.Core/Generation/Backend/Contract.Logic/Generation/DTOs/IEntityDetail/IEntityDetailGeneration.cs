using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Backend.Contract.Logic
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_CONTRACT_LOGIC })]
    internal class IEntityDetailGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ContractLogicProjectGeneration.TemplateFolder, "IEntityDetailTemplate.txt");

        private static readonly string FileName = "IEntityDetail.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly DtoInterfacePropertyAddition dtoInterfacePropertyAddition;
        private readonly DtoRelationAddition relationAddition;

        public IEntityDetailGeneration(
            EntityCoreAddition entityCoreAddition,
            DtoInterfacePropertyAddition dtoInterfacePropertyAddition,
            DtoRelationAddition relationAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.dtoInterfacePropertyAddition = dtoInterfacePropertyAddition;
            this.relationAddition = relationAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.entityCoreAddition.AddEntityToBackend(entity, ContractLogicProjectGeneration.DtoFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
            this.dtoInterfacePropertyAddition.AddPropertyToBackendFile(property, ContractLogicProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "IEnumerable<I", ">");

            this.relationAddition.AddRelationToDTO(relationSideFrom, ContractLogicProjectGeneration.DtoFolder, FileName, true,
                $"{relationSideFrom.Entity.Module.Options.Paths.ProjectName}.Contract.Logic.Modules.{relationSideFrom.OtherEntity.Module.Name}.{relationSideFrom.OtherEntity.NamePlural}");
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