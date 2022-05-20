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

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            string templatePath = TemplateFileName.GetFileNameForEntityAddition(options, TemplatePath);
            this.dtoAddition.AddDto(options, PersistenceDbContextProjectGeneration.DtoFolder, templatePath, FileName);
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
            this.relationAddition.AddRelationToDTO(optionsFrom, PersistenceDbContextProjectGeneration.DtoFolder, FileName);

            this.efDtoContructorHashSetAddition.Edit(options, PersistenceDbContextProjectGeneration.DtoFolder, FileName,
                $"{options.ProjectName}.Persistence.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");

            // To
            IRelationSideAdditionOptions propertyAdditionOptions = RelationAdditionOptions.GetPropertyForTo(options, "Guid");
            this.relationAddition.AddRelationToDTO(propertyAdditionOptions, PersistenceDbContextProjectGeneration.DtoFolder, FileName);

            IRelationSideAdditionOptions optionsTo2 = RelationAdditionOptions.GetPropertyForTo(options, $"virtual Ef{options.EntityNameFrom}");
            this.relationAddition.AddRelationToDTO(optionsTo2, PersistenceDbContextProjectGeneration.DtoFolder, FileName,
                $"{options.ProjectName}.Persistence.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            // From
            IRelationSideAdditionOptions optionsFrom =
                RelationAdditionOptions.GetPropertyForFrom(options, $"virtual Ef{options.EntityNameTo}");
            this.relationAddition.AddRelationToDTO(optionsFrom, PersistenceDbContextProjectGeneration.DtoFolder, FileName,
                $"{options.ProjectName}.Persistence.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");

            // To
            IRelationSideAdditionOptions propertyAdditionOptions = RelationAdditionOptions.GetPropertyForTo(options, "Guid");
            this.relationAddition.AddRelationToDTO(propertyAdditionOptions, PersistenceDbContextProjectGeneration.DtoFolder, FileName);

            IRelationSideAdditionOptions optionsTo2 = RelationAdditionOptions.GetPropertyForTo(options, $"virtual Ef{options.EntityNameFrom}");
            this.relationAddition.AddRelationToDTO(optionsTo2, PersistenceDbContextProjectGeneration.DtoFolder, FileName,
                $"{options.ProjectName}.Persistence.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
        }
    }
}