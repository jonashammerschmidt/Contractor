using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Contract.Persistence
{
    internal class IDbEntityDetailGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ContractPersistenceProjectGeneration.TemplateFolder, "IDbEntityDetailTemplate.txt");

        private static readonly string FileName = "IDbEntityDetail.cs";

        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;

        public IDbEntityDetailGeneration(
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition)
        {
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.dtoAddition.AddDto(options, ContractPersistenceProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, ContractPersistenceProjectGeneration.DomainFolder, FileName, true);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // From
            IPropertyAdditionOptions optionsFrom = RelationAdditionOptions.GetPropertyForFrom(options, $"IEnumerable<IDb{options.EntityNameTo}>", $"{options.EntityNamePluralTo}");
            this.propertyAddition.AddPropertyToDTO(optionsFrom, ContractPersistenceProjectGeneration.DomainFolder, FileName, true,
                $"{options.ProjectName}.Contract.Persistence.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");

            // To
            IPropertyAdditionOptions optionsTo = RelationAdditionOptions.GetPropertyForTo(options, $"IDb{options.EntityNameFrom}", options.EntityNameFrom);
            this.propertyAddition.AddPropertyToDTO(optionsTo, ContractPersistenceProjectGeneration.DomainFolder, FileName, true,
                $"{options.ProjectName}.Contract.Persistence.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
        }
    }
}