using System.IO;
using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Generation.Backend.Api
{
    public static class ApiProjectGeneration
    {
        public static readonly string DomainFolder = Path.Combine("API", "Modules", "Domain", "Entities", "API");

        public static readonly string TemplateFolder = Path.Combine(Folder.Executable, "Generation", "Generators", "Backend", "Api", "Templates");

        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, DependencyProviderGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntitiesCrudController>();
            serviceCollection.AddSingleton<EntitiesCrudControllerRelationAddition>();
        }
    }
}