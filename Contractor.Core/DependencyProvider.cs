using Contractor.Core.Projects.Backend.Api;
using Contractor.Core.Projects.Backend.Contract.Logic;
using Contractor.Core.Projects.Backend.Contract.Persistence;
using Contractor.Core.Projects.Backend.Logic;
using Contractor.Core.Projects.Backend.Logic.Tests;
using Contractor.Core.Projects.Backend.Persistence;
using Contractor.Core.Projects.Database.Persistence.DbContext;
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
            PersistenceDbContextProjectGeneration.ConfigureServices(serviceCollection);

            ModelProjectGeneration.ConfigureServices(serviceCollection);
            PagesProjectGeneration.ConfigureServices(serviceCollection);
        }

        private static void ConfigureTools(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IFileSystemClient, FileSystemCacheClient>();

            serviceCollection.AddSingleton<DomainDependencyProvider>();
            serviceCollection.AddSingleton<EntityCoreDependencyProvider>();

            serviceCollection.AddSingleton<ApiDtoPropertyAddition>();
            serviceCollection.AddSingleton<DtoAddition>();
            serviceCollection.AddSingleton<DtoPropertyAddition>();
            serviceCollection.AddSingleton<DtoRelationAddition>();
            serviceCollection.AddSingleton<EntityCoreAddition>();
            serviceCollection.AddSingleton<UsingStatementAddition>();

            serviceCollection.AddSingleton<FrontendPagesDomainCoreAddition>();
            serviceCollection.AddSingleton<FrontendEntityAddition>();
            serviceCollection.AddSingleton<FrontendEntityAddition>();
            serviceCollection.AddSingleton<FrontendDtoPropertyAddition>();
            serviceCollection.AddSingleton<FrontendDtoRelationAddition>();
            serviceCollection.AddSingleton<FrontendDtoPropertyMethodAddition>();

            serviceCollection.AddSingleton<EntitiesPageHtmlPropertyAddition>();
            serviceCollection.AddSingleton<EntitiesPageTsPropertyAddition>();

            serviceCollection.AddSingleton<PathService>();
        }
    }
}