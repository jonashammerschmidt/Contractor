using System.IO;
using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Generation.Frontend.Model
{
    public class ModelProjectGeneration
    {
        public static readonly string DomainFolder = Path.Combine("src", "app", "model", "domain-kebab", "entities-kebab");
        public static readonly string DomainDtoFolder = Path.Combine("src", "app", "model", "domain-kebab", "entities-kebab", "dtos");

        public static readonly string TemplateFolder = Path.Combine(Folder.Executable, "Generation", "Generators", "Frontend", "Model", "Templates");

        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Entity Core
            serviceCollection.AddSingleton<ClassGeneration, EntitiesCrudServiceGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntitiesModuleGeneration>();
        }
    }
}