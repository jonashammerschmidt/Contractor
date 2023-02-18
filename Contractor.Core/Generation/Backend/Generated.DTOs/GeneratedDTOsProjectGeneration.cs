using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    internal class GeneratedDTOsProjectGeneration
    {
        public static readonly string DomainFolder = "DTOs\\Modules\\Domain\\Entities";
        public static readonly string DtoFolder = DomainFolder + "\\DTOs";

        public static readonly string TemplateFolder = Folder.Executable + @"\Generation\Backend\Generated.DTOs\Templates";

        internal static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, EntityDtoGeneration>();
            serviceCollection.AddSingleton<EntityDtoMethodsAddition>();

            serviceCollection.AddSingleton<ClassGeneration, EntityDtoExpandedGeneration>();
            serviceCollection.AddSingleton<EntityDtoExpandedMethodsAddition>();
            serviceCollection.AddSingleton<EntityDtoExpandedToMethodsAddition>();
            serviceCollection.AddSingleton<EntityDtoExpandedFromOneToOneMethodsAddition>();

            serviceCollection.AddSingleton<ClassGeneration, EntityDtoDataGeneration>();
        }
    }
}