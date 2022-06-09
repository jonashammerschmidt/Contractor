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

        private readonly EfEntityContructorHashSetAddition efDtoContructorHashSetAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;

        public EfEntityGeneration(
            EfEntityContructorHashSetAddition efDtoContructorHashSetAddition,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            DtoRelationAddition relationAddition)
        {
            this.efDtoContructorHashSetAddition = efDtoContructorHashSetAddition;
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

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, PersistenceDbContextProjectGeneration.DtoFolder, FileName, false, true);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // From
            IRelationSideAdditionOptions optionsFrom =
                RelationAdditionOptions.GetPropertyForFrom(options, $"virtual ICollection<Ef{options.EntityNameTo}>");
            this.relationAddition.AddRelationToDTOForDatabase(optionsFrom, PersistenceDbContextProjectGeneration.DtoFolder, FileName);

            this.efDtoContructorHashSetAddition.EditForDatabase(options, PersistenceDbContextProjectGeneration.DtoFolder, FileName,
                $"{options.DbProjectName}.Persistence.DbContext.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");

            // To
            IRelationSideAdditionOptions propertyAdditionOptions = RelationAdditionOptions.GetPropertyForTo(options, "Guid");
            this.relationAddition.AddRelationToDTOForDatabase(propertyAdditionOptions, PersistenceDbContextProjectGeneration.DtoFolder, FileName);

            IRelationSideAdditionOptions optionsTo2 = RelationAdditionOptions.GetPropertyForTo(options, $"virtual Ef{options.EntityNameFrom}");
            this.relationAddition.AddRelationToDTOForDatabase(optionsTo2, PersistenceDbContextProjectGeneration.DtoFolder, FileName,
                $"{options.DbProjectName}.Persistence.DbContext.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            // From
            IRelationSideAdditionOptions optionsFrom =
                RelationAdditionOptions.GetPropertyForFrom(options, $"virtual Ef{options.EntityNameTo}");
            this.relationAddition.AddRelationToDTOForDatabase(optionsFrom, PersistenceDbContextProjectGeneration.DtoFolder, FileName,
                $"{options.DbProjectName}.Persistence.DbContext.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");

            // To
            IRelationSideAdditionOptions propertyAdditionOptions = RelationAdditionOptions.GetPropertyForTo(options, "Guid");
            this.relationAddition.AddRelationToDTOForDatabase(propertyAdditionOptions, PersistenceDbContextProjectGeneration.DtoFolder, FileName);

            IRelationSideAdditionOptions optionsTo2 = RelationAdditionOptions.GetPropertyForTo(options, $"virtual Ef{options.EntityNameFrom}");
            this.relationAddition.AddRelationToDTOForDatabase(optionsTo2, PersistenceDbContextProjectGeneration.DtoFolder, FileName,
                $"{options.DbProjectName}.Persistence.DbContext.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
        }
    }
}