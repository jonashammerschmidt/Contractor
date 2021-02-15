using Contractor.Core.Projects;
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
            serviceCollection.AddSingleton<ContractLogicProjectGeneration>();
            serviceCollection.AddSingleton<ContractPersistenceProjectGeneration>();
            serviceCollection.AddSingleton<ApiProjectGeneration>();
            serviceCollection.AddSingleton<LogicProjectGeneration>();
            serviceCollection.AddSingleton<LogicTestsProjectGeneration>();
            serviceCollection.AddSingleton<PersistenceProjectGeneration>();
            serviceCollection.AddSingleton<PersistenceTestsProjectGeneration>();
            serviceCollection.AddSingleton<DBProjectGeneration>();

            serviceCollection.AddSingleton<DtoPropertyAddition>();
            serviceCollection.AddSingleton<DtoAddition>();
            serviceCollection.AddSingleton<EntityCoreAddition>();

            serviceCollection.AddSingleton<DtoDetailFromMethodsAddition>();
            serviceCollection.AddSingleton<DtoDetailMethodsAddition>();
            serviceCollection.AddSingleton<DtoDetailToMethodsAddition>();
            serviceCollection.AddSingleton<DtoMethodsAddition>();

            serviceCollection.AddSingleton<DomainDependencyProvider>();
            serviceCollection.AddSingleton<EntityCoreDependencyProvider>();

            serviceCollection.AddSingleton<LogicRelationAddition>();

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

            serviceCollection.AddSingleton<DbProjectFileDomainAddition>();
            serviceCollection.AddSingleton<DbProjectFileEntityAddition>();
            serviceCollection.AddSingleton<DbTableAddition>();
            serviceCollection.AddSingleton<DbTablePropertyAddition>();
            serviceCollection.AddSingleton<DbTableRelationContraintAddition>();

            serviceCollection.AddSingleton<InMemoryDbContextEntityAddition>();
            serviceCollection.AddSingleton<DbDtoTestMethodsAddition>();
            serviceCollection.AddSingleton<DbDtoDetailTestMethodsAddition>();
            serviceCollection.AddSingleton<DtoTestValuesAddition>();
            serviceCollection.AddSingleton<DtoTestValuesRelationAddition>();
            serviceCollection.AddSingleton<DbDtoDetailTestFromAssertAddition>();
            serviceCollection.AddSingleton<DbDtoDetailTestToAssertAddition>();

            serviceCollection.AddSingleton<PathService>();
        }
    }
}