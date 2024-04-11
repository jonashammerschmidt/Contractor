using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Contractor.Core.Generation.Backend.Generated.DTOs;

namespace Contractor.Core.Generation.Backend.Generated.Interfaces
{
    public class GeneratedInterfacesProjectGeneration
    {
        public static readonly string DomainFolder = Path.Combine("Interfaces");

        public static readonly string TemplateFolder = Path.Combine(Folder.Executable, "Generation", "Generators", "Backend", "Generated.Interfaces", "Templates");
        
        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IInterfaceGeneration, InterfaceGeneration>();
            serviceCollection.AddSingleton<InterfaceExtender>();
        }
    }
}