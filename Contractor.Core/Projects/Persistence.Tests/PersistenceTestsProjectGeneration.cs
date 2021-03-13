using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Projects.Persistence.Tests
{
    internal class PersistenceTestsProjectGeneration
    {
        public static readonly string DomainFolder = "Persistence.Tests\\Modules\\{Domain}\\{Entities}";

        public static readonly string TemplateFolder = Folder.Executable + @"\Projects\Persistence.Tests\Templates";

        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, DbEntityDetailTestGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, DbEntityTestGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntitiesCrudRepositoryTestsGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityTestValuesGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, InMemoryDbContextGeneration>();

            serviceCollection.AddSingleton<InMemoryDbContextEntityAddition>();
            serviceCollection.AddSingleton<DbDtoTestMethodsAddition>();
            serviceCollection.AddSingleton<DbDtoDetailTestMethodsAddition>();
            serviceCollection.AddSingleton<DtoTestValuesAddition>();
            serviceCollection.AddSingleton<DtoTestValuesRelationAddition>();
            serviceCollection.AddSingleton<DbDtoDetailTestFromAssertAddition>();
            serviceCollection.AddSingleton<DbDtoDetailTestToAssertAddition>();
        }
    }
}