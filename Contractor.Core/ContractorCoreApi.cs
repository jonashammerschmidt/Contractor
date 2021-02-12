using Contractor.Core.Options;
using Contractor.Core.Projects;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core
{
    public class ContractorCoreApi
    {
        private readonly ContractPersistenceProjectGeneration contractPersistenceProjectGeneration;
        private readonly ContractLogicProjectGeneration contractLogicProjectGeneration;
        private readonly PersistenceProjectGeneration persistenceProjectGeneration;
        private readonly LogicProjectGeneration logicProjectGeneration;
        private readonly ApiProjectGeneration apiProjectGeneration;
        private readonly DBProjectGeneration dbProjectGeneration;

        public ContractorCoreApi()
        {
            ServiceProvider serviceProvider = DependencyProvider.GetServiceProvider();
            this.contractPersistenceProjectGeneration = serviceProvider.GetService<ContractPersistenceProjectGeneration>();
            this.contractLogicProjectGeneration = serviceProvider.GetService<ContractLogicProjectGeneration>();
            this.persistenceProjectGeneration = serviceProvider.GetService<PersistenceProjectGeneration>();
            this.logicProjectGeneration = serviceProvider.GetService<LogicProjectGeneration>();
            this.apiProjectGeneration = serviceProvider.GetService<ApiProjectGeneration>();
            this.dbProjectGeneration = serviceProvider.GetService<DBProjectGeneration>();
        }

        public void AddDomain(IDomainAdditionOptions options)
        {
            if (!DomainAdditionOptions.Validate(options))
            {
                throw new OptionValidationException("Die Optionen sind nicht korrekt formatiert.");
            }

            this.contractPersistenceProjectGeneration.AddDomain(options);
            this.contractLogicProjectGeneration.AddDomain(options);
            this.persistenceProjectGeneration.AddDomain(options);
            this.logicProjectGeneration.AddDomain(options);
            this.apiProjectGeneration.AddDomain(options);
            this.dbProjectGeneration.AddDomain(options);
        }

        public void AddEntity(IEntityAdditionOptions options)
        {
            if (!EntityAdditionOptions.Validate(options))
            {
                throw new OptionValidationException("Die Optionen sind nicht korrekt formatiert.");
            }

            this.contractLogicProjectGeneration.AddEntity(options);
            this.contractPersistenceProjectGeneration.AddEntity(options);
            this.persistenceProjectGeneration.AddEntity(options);
            this.logicProjectGeneration.AddEntity(options);
            this.apiProjectGeneration.AddEntity(options);
            this.dbProjectGeneration.AddEntity(options);
        }

        public void AddProperty(IPropertyAdditionOptions options)
        {
            if (!PropertyAdditionOptions.Validate(options))
            {
                throw new OptionValidationException("Die Optionen sind nicht korrekt formatiert.");
            }

            this.contractLogicProjectGeneration.AddProperty(options);
            this.contractPersistenceProjectGeneration.AddProperty(options);
            this.persistenceProjectGeneration.AddProperty(options);
            this.logicProjectGeneration.AddProperty(options);
            this.apiProjectGeneration.AddProperty(options);
            this.dbProjectGeneration.AddProperty(options);
        }

        public void Add1ToNRelation(IRelationAdditionOptions options)
        {
            if (!RelationAdditionOptions.Validate(options))
            {
                throw new OptionValidationException("Die Optionen sind nicht korrekt formatiert.");
            }

            this.contractLogicProjectGeneration.Add1ToNRelation(options);
            this.contractPersistenceProjectGeneration.Add1ToNRelation(options);
            this.persistenceProjectGeneration.Add1ToNRelation(options);
            this.logicProjectGeneration.Add1ToNRelation(options);
            this.apiProjectGeneration.Add1ToNRelation(options);
            this.dbProjectGeneration.Add1ToNRelation(options);
        }
    }
}