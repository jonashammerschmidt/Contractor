using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Generation.Backend.Persistence
{
    internal class PersistenceProjectGeneration
    {
        public static readonly string ProjectFolder = "API";

        public static readonly string DomainFolder = @"API\Modules\Domain\Entities\Persistence";

        public static readonly string TemplateFolder = Folder.Executable + @"\Generation\Backend\Persistence\Templates";

        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, EntitiesCrudRepositoryGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, IEntitiesCrudRepositoryGeneration>();

            serviceCollection.AddSingleton<EntitiesCrudRepositoryToRelationAddition>();
            serviceCollection.AddSingleton<EntitiesCrudRepositoryFromIncludeAddition>();
            serviceCollection.AddSingleton<EntitiesCrudRepositoryToIncludeAddition>();
            serviceCollection.AddSingleton<EntitiesCrudRepositoryFromOneToOneIncludeAddition>();
            serviceCollection.AddSingleton<EntitiesCrudRepositoryToOneToOneIncludeAddition>();
        }
    }
}