using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Contractor.Core.Generation.Backend.Generated.DTOs;

namespace Contractor.Core.Generation.Frontend.Interfaces
{
    public class FrontendInterfacesProjectGeneration
    {
        public static readonly string DomainFolder = Path.Combine("src", "app", "dtos", "_interfaces");

        public static readonly string TemplateFolder = Path.Combine(Folder.Executable, "Generation", "Generators", "Frontend", "Interfaces", "Templates");
        
        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IInterfaceGeneration, FrontendInterfaceGeneration>();
            serviceCollection.AddSingleton<FrontendInterfaceExtender>();
        }
    }
}