using System.IO;
using Contractor.Core.Generation.Backend.Generated.DTOs;
using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Generation.Backend.Persistence
{
    public class PersistenceProjectGeneration
    {
        public static readonly string ProjectFolder = "API";

        public static readonly string DomainFolder = Path.Combine("API", "Modules", "Domain", "Entities", "Persistence");

        public static readonly string TemplateFolder = Path.Combine(Folder.Executable, "Generation", "Generators", "Backend", "Persistence", "Templates");
        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, IEntitiesCrudRepositoryGeneration>();
            serviceCollection.AddSingleton<IEntitiesCrudRepositoryGeneration>();
            serviceCollection.AddSingleton<IEntitiesCrudRepositoryPurposeDtoInserter>();

            serviceCollection.AddSingleton<ClassGeneration, EntitiesCrudRepositoryGeneration>();
            serviceCollection.AddSingleton<EntitiesCrudRepositoryGeneration>();
            serviceCollection.AddSingleton<EntitiesCrudRepositoryPurposeDtoInserter>();
            serviceCollection.AddSingleton<EntitiesCrudRepositoryToRelationAddition>();
            serviceCollection.AddSingleton<EntitiesCrudRepositoryToIncludeAddition>();
            serviceCollection.AddSingleton<EntitiesCrudRepositoryToOneToOneIncludeAddition>();
        }
    }
}