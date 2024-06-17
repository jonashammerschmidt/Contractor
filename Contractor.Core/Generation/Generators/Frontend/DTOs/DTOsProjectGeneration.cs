using System.IO;
using Contractor.Core.Generation.Backend.Generated.DTOs;
using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Generation.Frontend.DTOs
{
    public class DTOsProjectGeneration
    {
        public static readonly string DomainFolder = Path.Combine("src", "app", "dtos", "domain-kebab", "entities-kebab");

        public static readonly string TemplateFolder = Path.Combine(Folder.Executable, "Generation", "Generators", "Frontend", "DTOs", "Templates");

        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, IEntityDtoGeneration>();
            serviceCollection.AddSingleton<IInterfaceGeneration, IEntityDtoGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, IEntityDtoDataGeneration>();
            serviceCollection.AddSingleton<IInterfaceGeneration, IEntityDtoDataGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, IEntityDtoExpandedGeneration>();
            serviceCollection.AddSingleton<IInterfaceGeneration, IEntityDtoExpandedGeneration>();

            serviceCollection.AddSingleton<IEntityDtoForPurposeGeneration>();
            serviceCollection.AddSingleton<IInterfaceGeneration, IEntityDtoForPurposeGeneration>();
            serviceCollection.AddSingleton<IEntityDtoForPurposeClassRenamer>();
        }
    }
}