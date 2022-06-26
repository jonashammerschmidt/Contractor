using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Backend.Contract.Logic
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_CONTRACT_LOGIC })]
    internal class IEntityCreateGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ContractLogicProjectGeneration.TemplateFolder, "IEntityCreateTemplate.txt");

        private static readonly string FileName = "IEntityCreate.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly DtoInterfacePropertyAddition dtoInterfacePropertyAddition;
        private readonly DtoRelationAddition relationAddition;

        public IEntityCreateGeneration(
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
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromGuidRelationEndTo(relation);
            this.relationAddition.AddRelationToDTO(relationSideTo, ContractLogicProjectGeneration.DtoFolder, FileName, true);
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