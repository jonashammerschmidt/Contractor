using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Persistence
{
    internal class EfEntityGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PersistenceProjectGeneration.TemplateFolder, "EfEntityTemplate.txt");

        private static readonly string FileName = "EfEntity.cs";

        private readonly EfEntityContructorHashSetAddition efDtoContructorHashSetAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;

        public EfEntityGeneration(
            EfEntityContructorHashSetAddition efDtoContructorHashSetAddition,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition)
        {
            this.efDtoContructorHashSetAddition = efDtoContructorHashSetAddition;
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            string templatePath = TemplateFileName.GetFileNameForEntityAddition(options, TemplatePath);
            this.dtoAddition.AddDto(options, PersistenceProjectGeneration.DomainFolder, templatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, PersistenceProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // From
            IPropertyAdditionOptions optionsFrom = 
                RelationAdditionOptions.GetPropertyForFrom(options, $"virtual ICollection<Ef{options.EntityNameTo}>", options.EntityNamePluralTo);
            this.propertyAddition.AddPropertyToDTO(optionsFrom, PersistenceProjectGeneration.DomainFolder, FileName);

            this.efDtoContructorHashSetAddition.Add(options, PersistenceProjectGeneration.DomainFolder, FileName,
                $"{options.ProjectName}.Persistence.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");

            // To
            IPropertyAdditionOptions propertyAdditionOptions = RelationAdditionOptions.GetPropertyForTo(options, "Guid", $"{options.EntityNameFrom}Id");
            this.propertyAddition.AddPropertyToDTO(propertyAdditionOptions, PersistenceProjectGeneration.DomainFolder, FileName);
            
            IPropertyAdditionOptions optionsTo2 = RelationAdditionOptions.GetPropertyForTo(options, $"virtual Ef{options.EntityNameFrom}", options.EntityNameFrom);
            this.propertyAddition.AddPropertyToDTO(optionsTo2, PersistenceProjectGeneration.DomainFolder, FileName,
                $"{options.ProjectName}.Persistence.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
        }
    }
}