using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Contract.Persistence
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_CONTRACT_PERSISTENCE })]
    internal class IDbEntityDetailGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ContractPersistenceProjectGeneration.TemplateFolder, "IDbEntityDetailTemplate.txt");

        private static readonly string FileName = "IDbEntityDetail.cs";

        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;

        public IDbEntityDetailGeneration(
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
            this.dtoAddition.AddDto(entity, ContractPersistenceProjectGeneration.DtoFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
            this.propertyAddition.AddPropertyToDTO(property, ContractPersistenceProjectGeneration.DtoFolder, FileName, true);
        }

        protected override void Add1ToNRelation(Relation1ToN relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "IEnumerable<IDb", ">");
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "IDb", "");

            // From
            this.relationAddition.AddRelationToDTO(relationSideFrom, ContractPersistenceProjectGeneration.DtoFolder, FileName, true,
                $"{relationSideFrom.Entity.Module.Options.Paths.ProjectName}.Contract.Persistence.Modules.{relationSideTo.Entity.Module.Name}.{relationSideTo.Entity.NamePlural}");

            // To
            this.relationAddition.AddRelationToDTO(relationSideTo, ContractPersistenceProjectGeneration.DtoFolder, FileName, true,
                $"{relationSideTo.Entity.Module.Options.Paths.ProjectName}.Contract.Persistence.Modules.{relationSideFrom.Entity.Module.Name}.{relationSideFrom.Entity.NamePlural}");
        }

        protected override void AddOneToOneRelation(Relation1To1 relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "IDb", "");
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "IDb", "");

            // From
            this.relationAddition.AddRelationToDTO(relationSideFrom, ContractPersistenceProjectGeneration.DtoFolder, FileName, true,
                $"{relationSideFrom.Entity.Module.Options.Paths.ProjectName}.Contract.Persistence.Modules.{relationSideTo.Entity.Module.Name}.{relationSideTo.Entity.NamePlural}");

            // To
            this.relationAddition.AddRelationToDTO(relationSideTo, ContractPersistenceProjectGeneration.DtoFolder, FileName, true,
                $"{relationSideTo.Entity.Module.Options.Paths.ProjectName}.Contract.Persistence.Modules.{relationSideFrom.Entity.Module.Name}.{relationSideFrom.Entity.NamePlural}");
        }
    }
}