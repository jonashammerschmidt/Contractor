using Contractor.Core.Projects;
using Contractor.Core.Projects.Api;
using Contractor.Core.Projects.Contract.Logic;
using Contractor.Core.Projects.Contract.Persistence;
using Contractor.Core.Projects.DB;
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

            // Project 

            serviceCollection.AddSingleton<IProjectGeneration, LogicProjectGeneration>();
            serviceCollection.AddSingleton<IProjectGeneration, LogicTestsProjectGeneration>();
            serviceCollection.AddSingleton<IProjectGeneration, PersistenceProjectGeneration>();
            serviceCollection.AddSingleton<IProjectGeneration, PersistenceTestsProjectGeneration>();

            // Services

            serviceCollection.AddSingleton<DtoDetailFromMethodsAddition>();
            serviceCollection.AddSingleton<DtoDetailMethodsAddition>();
            serviceCollection.AddSingleton<DtoDetailToMethodsAddition>();
            serviceCollection.AddSingleton<DtoMethodsAddition>();

            serviceCollection.AddSingleton<LogicRelationAddition>();

            serviceCollection.AddSingleton<LogicDbDtoDetailTestMethodsAddition>();
            serviceCollection.AddSingleton<LogicDbDtoTestMethodsAddition>();
            serviceCollection.AddSingleton<LogicDtoDetailTestMethodsAddition>();
            serviceCollection.AddSingleton<LogicDtoTestMethodsAddition>();
            serviceCollection.AddSingleton<LogicDtoCreateTestMethodsAddition>();
            serviceCollection.AddSingleton<LogicDtoUpdateTestMethodsAddition>();
            serviceCollection.AddSingleton<LogicDtoTestValuesAddition>();
            serviceCollection.AddSingleton<LogicDtoTestValuesRelationAddition>();

            serviceCollection.AddSingleton<LogicDbDtoDetailTestFromAssertAddition>();
            serviceCollection.AddSingleton<LogicDbDtoDetailTestToAssertAddition>();
            serviceCollection.AddSingleton<LogicDtoDetailTestFromAssertAddition>();
            serviceCollection.AddSingleton<LogicDtoDetailTestToAssertAddition>();
            serviceCollection.AddSingleton<LogicTestsRelationAddition>();

            serviceCollection.AddSingleton<DbContextEntityAddition>();
            serviceCollection.AddSingleton<DbContextPropertyAddition>();
            serviceCollection.AddSingleton<DbContextRelationToAddition>();
            serviceCollection.AddSingleton<DbDtoMethodsAddition>();
            serviceCollection.AddSingleton<DbDtoDetailMethodsAddition>();
            serviceCollection.AddSingleton<DbDtoDetailFromMethodsAddition>();
            serviceCollection.AddSingleton<DbDtoDetailToMethodsAddition>();
            serviceCollection.AddSingleton<EfDtoContructorHashSetAddition>();
            serviceCollection.AddSingleton<DtoFromRepositoryIncludeAddition>();
            serviceCollection.AddSingleton<DtoToRepositoryIncludeAddition>();

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