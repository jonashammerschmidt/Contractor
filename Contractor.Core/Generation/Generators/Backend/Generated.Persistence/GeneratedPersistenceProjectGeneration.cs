using System.IO;
using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Generation.Backend.Generated.Persistence
{
    public class GeneratedPersistenceProjectGeneration
    {
        public static readonly string DomainFolder = Path.Combine("Modules", "Domain", "Entities", "Persistence");

        public static readonly string TemplateFolder = Path.Combine(Folder.Executable, "Generation", "Generators", "Backend", "Generated.Persistence", "Templates");

        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, EntitiesReadRepositoryDefaultGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, IEntitiesReadRepositoryDefaultGeneration>();

            serviceCollection.AddSingleton<EntitiesReadRepositoryDefaultFromIncludeAddition>();
            serviceCollection.AddSingleton<EntitiesReadRepositoryDefaultToIncludeAddition>();
            serviceCollection.AddSingleton<EntitiesReadRepositoryDefaultFromOneToOneIncludeAddition>();
            serviceCollection.AddSingleton<EntitiesReadRepositoryDefaultToOneToOneIncludeAddition>();
        }
    }
}