using System.IO;
using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Generation.Backend.Api
{
    internal static class ApiProjectGeneration
    {
        internal static readonly string DomainFolder = Path.Combine("API", "Modules", "Domain", "Entities", "API");

        internal static readonly string TemplateFolder = Path.Combine(Folder.Executable, "Generation", "Backend", "Api", "Templates");

        internal static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, DependencyProviderGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntitiesCrudController>();
            serviceCollection.AddSingleton<EntitiesCrudControllerRelationAddition>();
        }
    }
}