using System.IO;
using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Generation.Frontend.Model
{
    internal class ModelProjectGeneration
    {
        internal static readonly string DomainFolder = Path.Combine("src", "app", "model", "domain-kebab", "entities-kebab");

        internal static readonly string TemplateFolder = Path.Combine(Folder.Executable, "Generation", "Generators", "Frontend", "Model", "Templates");

        internal static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Entity Core
            serviceCollection.AddSingleton<ClassGeneration, EntitiesCrudServiceGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntitiesModuleGeneration>();

            // dtos
            serviceCollection.AddSingleton<ClassGeneration, IEntityDtoGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, IEntityDtoDataGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, IEntityDtoExpandedGeneration>();
        }
    }
}