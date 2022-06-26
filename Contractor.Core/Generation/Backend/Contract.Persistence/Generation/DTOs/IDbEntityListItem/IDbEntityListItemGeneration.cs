using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Backend.Contract.Persistence
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_CONTRACT_PERSISTENCE })]
    internal class IDbEntityListItemGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ContractPersistenceProjectGeneration.TemplateFolder, "IDbEntityListItemTemplate.txt");

        private static readonly string FileName = "IDbEntityListItem.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly DtoInterfacePropertyAddition dtoInterfacePropertyAddition;
        private readonly DtoRelationAddition relationAddition;

        public IDbEntityListItemGeneration(
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
            this.entityCoreAddition.AddEntityToBackend(entity, ContractPersistenceProjectGeneration.DtoFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
            this.dtoInterfacePropertyAddition.AddPropertyToBackendFile(property, ContractPersistenceProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "IDb", "");

            this.relationAddition.AddRelationToDTO(relationSideTo, ContractPersistenceProjectGeneration.DtoFolder, FileName, true,
                $"{relationSideTo.Entity.Module.Options.Paths.ProjectName}.Contract.Persistence.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "IDb", "");

            this.relationAddition.AddRelationToDTO(relationSideFrom, ContractPersistenceProjectGeneration.DtoFolder, FileName, true,
                $"{relationSideFrom.Entity.Module.Options.Paths.ProjectName}.Contract.Persistence.Modules.{relationSideFrom.OtherEntity.Module.Name}.{relationSideFrom.OtherEntity.NamePlural}");
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "IDb", "");

            this.relationAddition.AddRelationToDTO(relationSideTo, ContractPersistenceProjectGeneration.DtoFolder, FileName, true,
                $"{relationSideTo.Entity.Module.Options.Paths.ProjectName}.Contract.Persistence.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");
        }
    }
}