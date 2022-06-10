using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Persistence
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_PERSISTENCE })]
    internal class DbEntityListItemGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PersistenceProjectGeneration.TemplateFolder, "DbEntityListItemTemplate.txt");

        private static readonly string FileName = "DbEntityListItem.cs";

        private readonly DbEntityListItemMethodsAddition dbDtoListItemMethodsAddition;
        private readonly DbEntityListItemToMethodsAddition dbDtoListItemToMethodsAddition;
        private readonly DbEntityListItemFromOneToOneMethodsAddition dbEntityListItemFromOneToOneMethodsAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;

        public DbEntityListItemGeneration(
            DbEntityListItemMethodsAddition dbDtoListItemMethodsAddition,
            DbEntityListItemToMethodsAddition dbDtoListItemToMethodsAddition,
            DbEntityListItemFromOneToOneMethodsAddition dbEntityListItemFromOneToOneMethodsAddition,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            DtoRelationAddition relationAddition)
        {
            this.dbDtoListItemMethodsAddition = dbDtoListItemMethodsAddition;
            this.dbDtoListItemToMethodsAddition = dbDtoListItemToMethodsAddition;
            this.dbEntityListItemFromOneToOneMethodsAddition = dbEntityListItemFromOneToOneMethodsAddition;
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
            this.dbDtoListItemMethodsAddition.Edit(property, PersistenceProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelation(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "IDb", "");

            // To
            this.relationAddition.AddRelationToDTO(relationSideTo, PersistenceProjectGeneration.DtoFolder, FileName,
                $"{relationSideTo.Entity.Module.Options.Paths.ProjectName}.Contract.Persistence.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");

            this.dbDtoListItemToMethodsAddition.Edit(relationSideTo, PersistenceProjectGeneration.DtoFolder, FileName,
                $"{relationSideTo.Entity.Module.Options.Paths.ProjectName}.Persistence.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");
        }

        protected override void AddOneToOneRelation(Relation1To1 relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "IDb", "");
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "IDb", "");

            // From
            this.relationAddition.AddRelationToDTO(relationSideFrom, PersistenceProjectGeneration.DtoFolder, FileName,
                $"{relationSideFrom.Entity.Module.Options.Paths.ProjectName}.Contract.Persistence.Modules.{relationSideFrom.OtherEntity.Module.Name}.{relationSideFrom.OtherEntity.NamePlural}");

            this.dbEntityListItemFromOneToOneMethodsAddition.Edit(relationSideFrom, PersistenceProjectGeneration.DtoFolder, FileName,
                $"{relationSideFrom.Entity.Module.Options.Paths.ProjectName}.Persistence.Modules.{relationSideFrom.OtherEntity.Module.Name}.{relationSideFrom.OtherEntity.NamePlural}");

            // To
            this.relationAddition.AddRelationToDTO(relationSideTo, PersistenceProjectGeneration.DtoFolder, FileName,
                $"{relationSideTo.Entity.Module.Options.Paths.ProjectName}.Contract.Logic.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");

            this.dbDtoListItemToMethodsAddition.Edit(relationSideTo, PersistenceProjectGeneration.DtoFolder, FileName,
                $"{relationSideTo.Entity.Module.Options.Paths.ProjectName}.Contract.Logic.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");
        }
    }
}