using Contractor.Core.Projects;
using Contractor.Core.Projects.Api;
using Contractor.Core.Projects.Contract.Logic;
using Contractor.Core.Projects.Contract.Persistence;
using Contractor.Core.Projects.DB;
using Contractor.Core.Projects.Logic;
using Contractor.Core.Projects.Persistence;
using Contractor.Core.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core
{
    internal class DependencyProvider
    {
        public static ServiceProvider GetServiceProvider()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            return serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            ConfigureTools(serviceCollection);

            ApiProjectGeneration.ConfigureServices(serviceCollection);
            ContractLogicProjectGeneration.ConfigureServices(serviceCollection);
            ContractPersistenceProjectGeneration.ConfigureServices(serviceCollection);
            DBProjectGeneration.ConfigureServices(serviceCollection);
            LogicProjectGeneration.ConfigureServices(serviceCollection);
            LogicTestsProjectGeneration.ConfigureServices(serviceCollection);
            PersistenceProjectGeneration.ConfigureServices(serviceCollection);

            // Project
            serviceCollection.AddSingleton<IProjectGeneration, PersistenceTestsProjectGeneration>();

            // Persistence.Tests
            serviceCollection.AddSingleton<InMemoryDbContextEntityAddition>();
            serviceCollection.AddSingleton<DbDtoTestMethodsAddition>();
            serviceCollection.AddSingleton<DbDtoDetailTestMethodsAddition>();
            serviceCollection.AddSingleton<DtoTestValuesAddition>();
            serviceCollection.AddSingleton<DtoTestValuesRelationAddition>();
            serviceCollection.AddSingleton<DbDtoDetailTestFromAssertAddition>();
            serviceCollection.AddSingleton<DbDtoDetailTestToAssertAddition>();
        }

        private static void ConfigureTools(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<DomainDependencyProvider>();
            serviceCollection.AddSingleton<EntityCoreDependencyProvider>();

            serviceCollection.AddSingleton<DtoAddition>();
            serviceCollection.AddSingleton<DtoPropertyAddition>();
            serviceCollection.AddSingleton<EntityCoreAddition>();

            serviceCollection.AddSingleton<PathService>();
        }
    }
}