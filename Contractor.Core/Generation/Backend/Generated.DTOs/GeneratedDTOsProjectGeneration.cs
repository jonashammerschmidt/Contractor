using System.IO;
using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    internal class GeneratedDTOsProjectGeneration
    {
        public static readonly string DomainFolder = Path.Combine("Modules", "Domain", "Entities", "DTOs");

        public static readonly string TemplateFolder = Path.Combine(Folder.Executable, "Generation", "Backend", "Generated.DTOs", "Templates");
        
        internal static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, EntityDtoGeneration>();
            serviceCollection.AddSingleton<EntityDtoMethodsAddition>();

            serviceCollection.AddSingleton<ClassGeneration, EntityDtoDefaultUpdateGeneration>();

            serviceCollection.AddSingleton<ClassGeneration, EntityDtoExpandedGeneration>();
            serviceCollection.AddSingleton<EntityDtoExpandedToMethodsAddition>();
            serviceCollection.AddSingleton<EntityDtoExpandedFromOneToOneMethodsAddition>();

            serviceCollection.AddSingleton<ClassGeneration, EntityDtoDataGeneration>();
            serviceCollection.AddSingleton<EntityDtoDataMethodsAddition>();
        }
    }
}