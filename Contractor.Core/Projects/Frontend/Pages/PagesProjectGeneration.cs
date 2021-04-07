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
            // App-Routing
            serviceCollection.AddSingleton<ClassGeneration, AppRoutingGeneration>();
            serviceCollection.AddSingleton<AppRoutingDomainAddition>();

            // Domain Core
            serviceCollection.AddSingleton<ClassGeneration, DomainModuleGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, DomainRoutingGeneration>();
            serviceCollection.AddSingleton<DomainRoutingEntityAddition>();
            serviceCollection.AddSingleton<EntitiesPagesModuleToRelationAddition>();

            // Entity Core
            serviceCollection.AddSingleton<ClassGeneration, EntitiesPagesModuleGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntitiesPagesRoutingGeneration>();

            // Page
            serviceCollection.AddSingleton<ClassGeneration, EntityPageHtmlGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntitiesPageScssGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntitiesPageTsGeneration>();
            serviceCollection.AddSingleton<EntitiesPageHtmlPropertyAddition>();
            serviceCollection.AddSingleton<EntitiesPageHtmlToRelationAddition>();
            serviceCollection.AddSingleton<EntitiesPageTsPropertyAddition>();
            serviceCollection.AddSingleton<EntitiesPageTsToPropertyAddition>();

            // Page-Create
            serviceCollection.AddSingleton<ClassGeneration, EntityCreatePageHtmlGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityCreatePageScssGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityCreatePageTsGeneration>();
            serviceCollection.AddSingleton<EntityCreatePageHtmlPropertyAddition>();
            serviceCollection.AddSingleton<EntityCreatePageHtmlToPropertyAddition>();
            serviceCollection.AddSingleton<EntityCreatePageTsPropertyAddition>();
            serviceCollection.AddSingleton<EntityCreatePageTsToPropertyAddition>();

            // Page-Detail
            serviceCollection.AddSingleton<ClassGeneration, EntityDetailPageHtmlGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityDetailPageScssGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityDetailPageTsGeneration>();
            serviceCollection.AddSingleton<EntityDetailPageHtmlPropertyAddition>();
            serviceCollection.AddSingleton<EntityDetailPageHtmlFromPropertyAddition>();
            serviceCollection.AddSingleton<EntityDetailPageHtmlToPropertyAddition>();
            serviceCollection.AddSingleton<EntityDetailPageTsFromPropertyAddition>();

            // Page-Update
            serviceCollection.AddSingleton<ClassGeneration, EntityUpdatePageHtmlGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityUpdatePageScssGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityUpdatePageTsGeneration>();
            serviceCollection.AddSingleton<EntityUpdatePageHtmlPropertyAddition>();
            serviceCollection.AddSingleton<EntityUpdatePageHtmlToPropertyAddition>();
            serviceCollection.AddSingleton<EntityUpdatePageTsPropertyAddition>();
            serviceCollection.AddSingleton<EntityUpdatePageTsToPropertyAddition>();
        }
    }
}