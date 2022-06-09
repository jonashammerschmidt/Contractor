using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Persistence
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

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, PersistenceProjectGeneration.DtoFolder, FileName);
            this.dbDtoDetailMethodsAddition.Edit(options, PersistenceProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // From
            IRelationSideAdditionOptions optionsFrom =
                RelationAdditionOptions.GetPropertyForFrom(options, $"IEnumerable<IDb{options.EntityNameTo}>");
            this.relationAddition.AddRelationToDTO(optionsFrom, PersistenceProjectGeneration.DtoFolder, FileName,
                $"{options.ProjectName}.Contract.Persistence.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");

            this.dbDtoDetailFromMethodsAddition.Edit(options, PersistenceProjectGeneration.DtoFolder, FileName,
                $"{options.ProjectName}.Persistence.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");

            // To
            IRelationSideAdditionOptions optionsTo =
                RelationAdditionOptions.GetPropertyForTo(options, $"IDb{options.EntityNameFrom}");
            this.relationAddition.AddRelationToDTO(optionsTo, PersistenceProjectGeneration.DtoFolder, FileName,
                $"{options.ProjectName}.Contract.Persistence.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");

            this.dbDtoDetailToMethodsAddition.Edit(options, PersistenceProjectGeneration.DtoFolder, FileName,
                $"{options.ProjectName}.Persistence.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            // From
            IRelationSideAdditionOptions optionsFrom =
                RelationAdditionOptions.GetPropertyForFrom(options, $"IDb{options.EntityNameTo}");
            this.relationAddition.AddRelationToDTO(optionsFrom, PersistenceProjectGeneration.DtoFolder, FileName,
                $"{options.ProjectName}.Contract.Persistence.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");

            this.dbEntityDetailFromOneToOneMethodsAddition.Edit(options, PersistenceProjectGeneration.DtoFolder, FileName,
                $"{options.ProjectName}.Persistence.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");

            // To
            IRelationSideAdditionOptions optionsTo =
                RelationAdditionOptions.GetPropertyForTo(options, $"IDb{options.EntityNameFrom}");
            this.relationAddition.AddRelationToDTO(optionsTo, PersistenceProjectGeneration.DtoFolder, FileName,
                $"{options.ProjectName}.Contract.Persistence.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");

            this.dbDtoDetailToMethodsAddition.Edit(options, PersistenceProjectGeneration.DtoFolder, FileName,
                $"{options.ProjectName}.Persistence.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
        }
    }
}