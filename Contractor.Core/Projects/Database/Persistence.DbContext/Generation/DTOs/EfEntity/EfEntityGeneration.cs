using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Database.Persistence.DbContext
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_PERSISTENCE_DB_CONTEXT })]
    internal class EfEntityGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PersistenceDbContextProjectGeneration.TemplateFolder, "EfEntityTemplate.txt");

        private static readonly string FileName = "EfEntity.cs";

        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;

        public EfEntityGeneration(
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
            string templatePath = TemplateFileName.GetFileNameForEntityAddition(entity, TemplatePath);
            this.dtoAddition.AddDto(entity, PersistenceDbContextProjectGeneration.DtoFolder, templatePath, FileName, true);
        }

        protected override void AddProperty(Property property)
        {
            this.propertyAddition.AddPropertyToDTO(property, PersistenceDbContextProjectGeneration.DtoFolder, FileName, false, true);
        }

        protected override void Add1ToNRelation(Relation1ToN relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "virtual ICollection<Ef", ">");
            RelationSide relationSideToGuid = RelationSide.FromGuidRelationEndTo(relation);
            RelationSide relationSideToEfObject = RelationSide.FromObjectRelationEndTo(relation, "virtual Ef", "");

            // From
            this.relationAddition.AddRelationToDTOForDatabase(relationSideFrom, PersistenceDbContextProjectGeneration.DtoFolder, FileName);

            // To
            this.relationAddition.AddRelationToDTOForDatabase(relationSideToGuid, PersistenceDbContextProjectGeneration.DtoFolder, FileName);

            this.relationAddition.AddRelationToDTOForDatabase(relationSideToEfObject, PersistenceDbContextProjectGeneration.DtoFolder, FileName,
                $"{relationSideToEfObject.Entity.Module.Options.Paths.DbProjectName}.Persistence.DbContext.Modules.{relationSideToEfObject.OtherEntity.Module.Name}.{relationSideToEfObject.OtherEntity.NamePlural}");
        }

        protected override void AddOneToOneRelation(Relation1To1 relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "virtual Ef", "");
            RelationSide relationSideToGuid = RelationSide.FromGuidRelationEndTo(relation);
            RelationSide relationSideToEfObject = RelationSide.FromObjectRelationEndTo(relation, "virtual Ef", "");

            // From
            this.relationAddition.AddRelationToDTOForDatabase(relationSideFrom, PersistenceDbContextProjectGeneration.DtoFolder, FileName);

            // To
            this.relationAddition.AddRelationToDTOForDatabase(relationSideToGuid, PersistenceDbContextProjectGeneration.DtoFolder, FileName);

            this.relationAddition.AddRelationToDTOForDatabase(relationSideToEfObject, PersistenceDbContextProjectGeneration.DtoFolder, FileName,
                $"{relationSideToEfObject.Entity.Module.Options.Paths.DbProjectName}.Persistence.DbContext.Modules.{relationSideToEfObject.OtherEntity.Module.Name}.{relationSideToEfObject.OtherEntity.NamePlural}");
        }
    }
}