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
            serviceCollection.AddSingleton<ClassGeneration, EntityDefaultDtoGeneration>();
            serviceCollection.AddSingleton<EntityDefaultDtoMethodsAddition>();

            serviceCollection.AddSingleton<ClassGeneration, EntityDetailDefaultDtoGeneration>();
            serviceCollection.AddSingleton<EntityDetailDefaultDtoMethodsAddition >();
            serviceCollection.AddSingleton<EntityDetailDefaultDtoToMethodsAddition >();
            serviceCollection.AddSingleton<EntityDetailDefaultDtoFromOneToOneMethodsAddition>();
        }
    }
}