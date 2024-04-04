using System.IO;
using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    public class GeneratedDTOsProjectGeneration
    {
        public static readonly string DomainFolder = Path.Combine("Modules", "Domain", "Entities", "DTOs");

        public static readonly string TemplateFolder = Path.Combine(Folder.Executable, "Generation", "Generators", "Backend", "Generated.DTOs", "Templates");
        
        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, EntityDtoGeneration>();
            serviceCollection.AddSingleton<EntityDtoMethodsAddition>();

            serviceCollection.AddSingleton<ClassGeneration, EntityDtoDefaultUpdateGeneration>();

            serviceCollection.AddSingleton<ClassGeneration, EntityDtoExpandedGeneration>();
            serviceCollection.AddSingleton<EntityDtoExpandedToMethodsAddition>();
            serviceCollection.AddSingleton<EntityDtoExpandedFromOneToOneMethodsAddition>();

            serviceCollection.AddSingleton<ClassGeneration, EntityDtoDataGeneration>();
            serviceCollection.AddSingleton<EntityDtoDataMethodsAddition>();
            
            serviceCollection.AddSingleton<EntityDtoForPurposeGeneration>();
            serviceCollection.AddSingleton<EntityDtoForPurposeFromMethodsAddition>();
            serviceCollection.AddSingleton<EntityDtoForPurposeFromOneToOneMethodsAddition>();
            serviceCollection.AddSingleton<EntityDtoForPurposeToMethodsAddition>();
            serviceCollection.AddSingleton<EntityDtoForPurposeClassRenamer>();
            serviceCollection.AddSingleton<EntityDtoForPurposeIncludeInserter>();
        }
    }
}