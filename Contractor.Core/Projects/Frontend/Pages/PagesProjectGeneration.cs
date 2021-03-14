using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class PagesProjectGeneration
    {
        internal static readonly string DomainFolder = "src\\app\\pages\\{domain-kebab}\\{entities-kebab}";

        internal static readonly string TemplateFolder = Folder.Executable + @"\Projects\Frontend\Pages\Templates";

        internal static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Entity Core
            serviceCollection.AddSingleton<ClassGeneration, DomainModuleGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, DomainRoutingGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntitiesPagesModuleGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntitiesPagesRoutingGeneration>();
        }
    }
}