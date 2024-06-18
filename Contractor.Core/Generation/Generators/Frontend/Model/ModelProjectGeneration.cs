using System.IO;
using Contractor.Core.Generation.Backend.Generated.DTOs;
using Contractor.Core.Generation.Frontend.Generated.DTOs;
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

            // dtos
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