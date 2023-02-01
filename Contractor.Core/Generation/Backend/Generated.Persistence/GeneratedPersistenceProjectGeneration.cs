using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Generation.Backend.Generated.Persistence
{
    internal class GeneratedPersistenceProjectGeneration
    {
        public static readonly string DomainFolder = "Persistence\\Modules\\Domain\\Entities";

        public static readonly string TemplateFolder = Folder.Executable + @"\Generation\Backend\Generated.Persistence\Templates";

        internal static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, EntitiesReadRepositoryDefaultGeneration>();

            serviceCollection.AddSingleton<EntitiesReadRepositoryDefaultFromIncludeAddition>();
            serviceCollection.AddSingleton<EntitiesReadRepositoryDefaultToIncludeAddition>();
            serviceCollection.AddSingleton<EntitiesReadRepositoryDefaultFromOneToOneIncludeAddition>();
            serviceCollection.AddSingleton<EntitiesReadRepositoryDefaultToOneToOneIncludeAddition>();
        }
    }
}