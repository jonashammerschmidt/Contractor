using System.IO;
using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Generation.Frontend.Pages
{
    internal class PagesProjectGeneration
    {
        internal static readonly string PagesFolder = Path.Combine("src", "app", "pages", "domain-kebab");
        internal static readonly string DomainFolder = Path.Combine("src", "app", "pages", "domain-kebab", "entities-kebab");

        internal static readonly string TemplateFolder = Path.Combine(Folder.Executable, "Generation", "Generators", "Frontend", "Pages", "Templates");

        internal static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // App-Routing
            serviceCollection.AddSingleton<ClassGeneration, AppRoutingGeneration>();
            serviceCollection.AddSingleton<AppRoutingDomainAddition>();

            // App-Component
            serviceCollection.AddSingleton<ClassGeneration, AppComponentGeneration>();
            serviceCollection.AddSingleton<AppComponentModuleAddition>();
            serviceCollection.AddSingleton<AppComponentEntityAddition>();

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
            serviceCollection.AddSingleton<EntitiesPageHtmlFromOneToOneRelationAddition>();
            serviceCollection.AddSingleton<EntitiesPageHtmlToRelationAddition>();
            serviceCollection.AddSingleton<EntitiesPageTsPropertyAddition>();
            serviceCollection.AddSingleton<EntitiesPageHtmlToOneToOneRelationAddition>();
            serviceCollection.AddSingleton<EntitiesPageTsFromOneToOnePropertyAddition>();
            serviceCollection.AddSingleton<EntitiesPageTsToOneToOnePropertyAddition>();
            serviceCollection.AddSingleton<EntitiesPageTsToPropertyAddition>();

            // Page-Create
            serviceCollection.AddSingleton<ClassGeneration, EntityCreatePageHtmlGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityCreatePageScssGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityCreatePageTsGeneration>();
            serviceCollection.AddSingleton<EntityCreatePageHtmlPropertyAddition>();
            serviceCollection.AddSingleton<EntityCreatePageHtmlToOneToOnePropertyAddition>();
            serviceCollection.AddSingleton<EntityCreatePageHtmlToPropertyAddition>();
            serviceCollection.AddSingleton<EntityCreatePageTsPropertyAddition>();
            serviceCollection.AddSingleton<EntityCreatePageTsToPropertyAddition>();

            // Page-Detail
            serviceCollection.AddSingleton<ClassGeneration, EntityDetailPageHtmlGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityDetailPageScssGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityDetailPageTsGeneration>();
            serviceCollection.AddSingleton<EntityDetailPageHtmlPropertyAddition>();
            serviceCollection.AddSingleton<EntityDetailPageHtmlToOneToOnePropertyAddition>();
            serviceCollection.AddSingleton<EntityDetailPageHtmlToPropertyAddition>();
            serviceCollection.AddSingleton<EntityDetailPageTsPropertyAddition>();
            serviceCollection.AddSingleton<EntityDetailPageTsToPropertyAddition>();
        }
    }
}