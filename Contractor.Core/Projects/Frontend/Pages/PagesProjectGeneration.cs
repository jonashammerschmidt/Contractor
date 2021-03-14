using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class PagesProjectGeneration
    {
        internal static readonly string PagesFolder = "src\\app\\pages\\{domain-kebab}";
        internal static readonly string DomainFolder = "src\\app\\pages\\{domain-kebab}\\{entities-kebab}";

        internal static readonly string TemplateFolder = Folder.Executable + @"\Projects\Frontend\Pages\Templates";

        internal static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Domain Core
            serviceCollection.AddSingleton<ClassGeneration, DomainModuleGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, DomainRoutingGeneration>();

            // Entity Core
            serviceCollection.AddSingleton<ClassGeneration, EntitiesPagesModuleGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntitiesPagesRoutingGeneration>();

            // Page
            serviceCollection.AddSingleton<ClassGeneration, EntityPageHtmlGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntitiesPageScssGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntitiesPageTsGeneration>();

            // Page-Create
            serviceCollection.AddSingleton<ClassGeneration, EntityCreatePageHtmlGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityCreatePageScssGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityCreatePageTsGeneration>();

            // Page-Detail
            serviceCollection.AddSingleton<ClassGeneration, EntityDetailPageHtmlGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityDetailPageScssGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityDetailPageTsGeneration>();
        }
    }
}