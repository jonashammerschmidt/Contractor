using Contractor.Core.Jobs;
using Contractor.Core.Template.API;
using Contractor.Core.Template.Contract;
using Contractor.Core.Template.Logic;
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

        public void AddDomain(DomainOptions options)
        {
            this.contractPersistenceProjectGeneration.AddDomain(options);
            this.contractLogicProjectGeneration.AddDomain(options);
            this.persistenceProjectGeneration.AddDomain(options);
            this.logicProjectGeneration.AddDomain(options);
            this.apiProjectGeneration.AddDomain(options);
            this.dbProjectGeneration.AddDomain(options);
        }

        public void AddEntity(EntityOptions options)
        {
            this.contractLogicProjectGeneration.AddEntity(options);
            this.contractPersistenceProjectGeneration.AddEntity(options);
            this.persistenceProjectGeneration.AddEntity(options);
            this.logicProjectGeneration.AddEntity(options);
            this.apiProjectGeneration.AddEntity(options);
            this.dbProjectGeneration.AddEntity(options);
        }

        public void AddProperty(PropertyOptions options)
        {
            this.contractLogicProjectGeneration.AddProperty(options);
            this.contractPersistenceProjectGeneration.AddProperty(options);
            this.persistenceProjectGeneration.AddProperty(options);
            this.logicProjectGeneration.AddProperty(options);
            this.apiProjectGeneration.AddProperty(options);
            this.dbProjectGeneration.AddProperty(options);
        }
    }
}