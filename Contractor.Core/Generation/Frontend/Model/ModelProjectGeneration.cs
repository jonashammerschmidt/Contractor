using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Generation.Frontend.Model
{
    internal class ModelProjectGeneration
    {
        internal static readonly string DomainFolder = "src\\app\\model\\domain-kebab\\entities-kebab";

        internal static readonly string TemplateFolder = Folder.Executable + @"\Generation\Frontend\Model\Templates";

        internal static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Entity Core
            serviceCollection.AddSingleton<ClassGeneration, EntitiesCrudServiceGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntitiesModuleGeneration>();

            // dtos
            serviceCollection.AddSingleton<ClassGeneration, IEntityGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, IEntityCreateGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, IEntityDetailGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, IEntityListItemGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, IEntityUpdateGeneration>();
            serviceCollection.AddSingleton<IEntityUpdateMethodAddition>();
        }
    }
}