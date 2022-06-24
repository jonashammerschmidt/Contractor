using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Backend.Persistence
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_PERSISTENCE })]
    internal class DbEntityDetailGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PersistenceProjectGeneration.TemplateFolder, "DbEntityDetailTemplate.txt");

        private static readonly string FileName = "DbEntityDetail.cs";

        private readonly DbEntityDetailMethodsAddition dbDtoDetailMethodsAddition;
        private readonly DbEntityDetailFromMethodsAddition dbDtoDetailFromMethodsAddition;
        private readonly DbEntityDetailToMethodsAddition dbDtoDetailToMethodsAddition;
        private readonly DbEntityDetailFromOneToOneMethodsAddition dbEntityDetailFromOneToOneMethodsAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;

        public DbEntityDetailGeneration(
            DbEntityDetailMethodsAddition dbDtoDetailMethodsAddition,
            DbEntityDetailFromMethodsAddition dbDtoDetailFromMethodsAddition,
            DbEntityDetailToMethodsAddition dbDtoDetailToMethodsAddition,
            DbEntityDetailFromOneToOneMethodsAddition dbEntityDetailFromOneToOneMethodsAddition,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            DtoRelationAddition relationAddition)
        {
            this.dbDtoDetailMethodsAddition = dbDtoDetailMethodsAddition;
            this.dbDtoDetailFromMethodsAddition = dbDtoDetailFromMethodsAddition;
            this.dbDtoDetailToMethodsAddition = dbDtoDetailToMethodsAddition;
            this.dbEntityDetailFromOneToOneMethodsAddition = dbEntityDetailFromOneToOneMethodsAddition;
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
            this.relationAddition = relationAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.dtoAddition.AddDto(entity, PersistenceProjectGeneration.DtoFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
            this.propertyAddition.AddPropertyToDTO(property, PersistenceProjectGeneration.DtoFolder, FileName);
            this.dbDtoDetailMethodsAddition.AddPropertyToBackendFile(property, PersistenceProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "IEnumerable<IDb", ">"); 

            this.relationAddition.AddRelationToDTO(relationSideFrom, PersistenceProjectGeneration.DtoFolder, FileName,
                $"{relationSideFrom.OtherEntity.Module.Options.Paths.ProjectName}.Contract.Persistence.Modules.{relationSideFrom.OtherEntity.Module.Name}.{relationSideFrom.OtherEntity.NamePlural}");

            this.dbDtoDetailFromMethodsAddition.AddRelationSideToBackendFile(relationSideFrom, PersistenceProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "IDb", "");

            this.relationAddition.AddRelationToDTO(relationSideTo, PersistenceProjectGeneration.DtoFolder, FileName,
                $"{relationSideTo.Entity.Module.Options.Paths.ProjectName}.Contract.Persistence.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");

            this.dbDtoDetailToMethodsAddition.AddRelationSideToBackendFile(relationSideTo, PersistenceProjectGeneration.DtoFolder, FileName);
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "IDb", "");

            this.relationAddition.AddRelationToDTO(relationSideFrom, PersistenceProjectGeneration.DtoFolder, FileName,
                $"{relationSideFrom.Entity.Module.Options.Paths.ProjectName}.Contract.Persistence.Modules.{relationSideFrom.OtherEntity.Module.Name}.{relationSideFrom.OtherEntity.NamePlural}");

            this.dbEntityDetailFromOneToOneMethodsAddition.AddRelationSideToBackendFile(relationSideFrom, PersistenceProjectGeneration.DtoFolder, FileName);
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "IDb", "");

            this.relationAddition.AddRelationToDTO(relationSideTo, PersistenceProjectGeneration.DtoFolder, FileName,
                $"{relationSideTo.Entity.Module.Options.Paths.ProjectName}.Contract.Persistence.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");

            this.dbDtoDetailToMethodsAddition.AddRelationSideToBackendFile(relationSideTo, PersistenceProjectGeneration.DtoFolder, FileName);
        }
    }
}