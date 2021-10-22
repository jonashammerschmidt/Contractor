using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Projects.Backend.Persistence.Tests
{
    internal class PersistenceTestsProjectGeneration
    {
        public static readonly string DomainFolder = "Persistence.Tests\\Modules\\{Domain}\\{Entities}";

        public static readonly string TemplateFolder = Folder.Executable + @"\Projects\Backend\Persistence.Tests\Templates";

        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, DbEntityDetailTestGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, DbEntityListItemTestGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, DbEntityTestGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, DbEntityUpdateTestGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntitiesCrudRepositoryTestsGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityTestValuesGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, InMemoryDbContextGeneration>();

            serviceCollection.AddSingleton<InMemoryDbContextEntityAddition>();
            serviceCollection.AddSingleton<DbEntityTestMethodsAddition>();
            serviceCollection.AddSingleton<DbEntityDetailTestMethodsAddition>();
            serviceCollection.AddSingleton<DbEntityListItemTestMethodsAddition>();
            serviceCollection.AddSingleton<DbEntityUpdateTestMethodsAddition>();
            serviceCollection.AddSingleton<EntityTestValuesAddition>();
            serviceCollection.AddSingleton<EntityTestValuesRelationAddition>();
            serviceCollection.AddSingleton<DbEntityDetailTestFromAssertAddition>();
            serviceCollection.AddSingleton<DbEntityDetailTestFromOneToOneAssertAddition>();
            serviceCollection.AddSingleton<DbEntityDetailTestToAssertAddition>();
            serviceCollection.AddSingleton<DbEntityListItemTestFromOneToOneAssertAddition>();
            serviceCollection.AddSingleton<DbEntityListItemTestToAssertAddition>();
            serviceCollection.AddSingleton<EntitiesCrudRepositoryTestsToOneToOneRelationAddition>();
        }
    }
}