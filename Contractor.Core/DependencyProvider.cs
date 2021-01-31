using Microsoft.Extensions.DependencyInjection;
using Contractor.Core.Projects.DB.ProjectFile;
using Contractor.Core.Projects.Persistence;
using Contractor.Core.Template.API;
using Contractor.Core.Template.Contract;
using Contractor.Core.Template.Logic;
using Contractor.Core.Tools;

namespace Contractor.Core
{
    public class DependencyProvider
    {
        public static ServiceProvider GetServiceProvider()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            return serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ContractLogicProjectGeneration>();
            serviceCollection.AddSingleton<ContractPersistenceProjectGeneration>();
            serviceCollection.AddSingleton<ApiProjectGeneration>();
            serviceCollection.AddSingleton<LogicProjectGeneration>();
            serviceCollection.AddSingleton<PersistenceProjectGeneration>();
            serviceCollection.AddSingleton<DBProjectGeneration>();

            serviceCollection.AddSingleton<DtoPropertyAddition>();
            serviceCollection.AddSingleton<DtoAddition>();
            serviceCollection.AddSingleton<EntityCoreAddition>();

            serviceCollection.AddSingleton<DtoDetailMethodsAddition>();
            serviceCollection.AddSingleton<DtoMethodsAddition>();

            serviceCollection.AddSingleton<DomainDependencyProvider>();
            serviceCollection.AddSingleton<EntityCoreDependencyProvider>();

            serviceCollection.AddSingleton<DbContextAddition>();
            serviceCollection.AddSingleton<DbContextEntityAddition>();
            serviceCollection.AddSingleton<DbContextPropertyAddition>();
            serviceCollection.AddSingleton<DbDtoMethodsAddition>();

            serviceCollection.AddSingleton<DbProjectFileDomainAddition>();
            serviceCollection.AddSingleton<DbProjectFileEntityAddition>();
            serviceCollection.AddSingleton<DbTableAddition>();
            serviceCollection.AddSingleton<DbTablePropertyAddition>();

            serviceCollection.AddSingleton<UsingStatementAddition>();
            serviceCollection.AddSingleton<PathService>();
        }
    }
}