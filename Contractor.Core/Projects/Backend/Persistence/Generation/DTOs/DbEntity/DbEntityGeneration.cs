using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Persistence
{
    internal class DbEntityGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PersistenceProjectGeneration.TemplateFolder, "DbEntityTemplate.txt");

        private static readonly string FileName = "DbEntity.cs";

        private readonly DbEntityMethodsAddition dbDtoMethodsAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;

        public DbEntityGeneration(
            DbEntityMethodsAddition dbDtoMethodsAddition,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition)
        {
            this.dbDtoMethodsAddition = dbDtoMethodsAddition;
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
            this.dbDtoMethodsAddition.Add(options, PersistenceProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            IRelationSideAdditionOptions relationSideAdditionOptions =
                RelationAdditionOptions.GetPropertyForTo(options, "Guid", $"{options.EntityNameFrom}Id");
            PropertyAdditionOptions propertyAdditionOptions = new PropertyAdditionOptions(relationSideAdditionOptions);

            this.propertyAddition.AddPropertyToDTO(propertyAdditionOptions, PersistenceProjectGeneration.DomainFolder, FileName);
            this.dbDtoMethodsAddition.Add(propertyAdditionOptions, PersistenceProjectGeneration.DomainFolder, FileName);
        }
    }
}