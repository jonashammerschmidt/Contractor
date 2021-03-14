using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Projects.Frontend.Model
{
    internal class ModelProjectGeneration
    {
        internal static readonly string DomainFolder = "src\\app\\model\\{domain-kebab}\\{entities-kebab}";

        internal static readonly string TemplateFolder = Folder.Executable + @"\Projects\Frontend\Model\Templates";

        internal static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Entity Core
            serviceCollection.AddSingleton<ClassGeneration, EntitiesCrudServiceGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntitiesModuleGeneration>();

            // dtos
            serviceCollection.AddSingleton<ClassGeneration, IEntityGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, IEntityCreateGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, IEntityDetailGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, IEntityUpdateGeneration>();

            // dtos/api
            serviceCollection.AddSingleton<ClassGeneration, ApiEntityGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, ApiEntityCreateGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, ApiEntityDetailGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, ApiEntityUpdateGeneration>();
        }
    }
}