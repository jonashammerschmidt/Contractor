using Contractor.Core.Projects.Backend.Api;
using Contractor.Core.Projects.Backend.Contract.Logic;
using Contractor.Core.Projects.Backend.Contract.Persistence;
using Contractor.Core.Projects.Backend.Logic;
using Contractor.Core.Projects.Backend.Logic.Tests;
using Contractor.Core.Projects.Backend.Persistence;
using Contractor.Core.Projects.Backend.Persistence.Tests;
using Contractor.Core.Projects.Database;
using Contractor.Core.Projects.Frontend.Model;
using Contractor.Core.Projects.Frontend.Pages;
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
            LogicProjectGeneration.ConfigureServices(serviceCollection);
            LogicTestsProjectGeneration.ConfigureServices(serviceCollection);
            PersistenceProjectGeneration.ConfigureServices(serviceCollection);
            PersistenceTestsProjectGeneration.ConfigureServices(serviceCollection);

            DBProjectGeneration.ConfigureServices(serviceCollection);

            ModelProjectGeneration.ConfigureServices(serviceCollection);
            PagesProjectGeneration.ConfigureServices(serviceCollection);
        }

        private static void ConfigureTools(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<DomainDependencyProvider>();
            serviceCollection.AddSingleton<EntityCoreDependencyProvider>();

            serviceCollection.AddSingleton<DtoAddition>();
            serviceCollection.AddSingleton<DtoPropertyAddition>();
            serviceCollection.AddSingleton<EntityCoreAddition>();

            serviceCollection.AddSingleton<FrontendModelEntityCoreAddition>();
            serviceCollection.AddSingleton<FrontendPagesEntityCoreAddition>();
            serviceCollection.AddSingleton<FrontendDtoAddition>();

            serviceCollection.AddSingleton<PathService>();
        }
    }
}