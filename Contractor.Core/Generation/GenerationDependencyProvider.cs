using Contractor.Core.BaseClasses;
using Contractor.Core.Generation.Backend.Api;
using Contractor.Core.Generation.Backend.Generated.DTOs;
using Contractor.Core.Generation.Backend.Generated.Interfaces;
using Contractor.Core.Generation.Backend.Generated.Persistence;
using Contractor.Core.Generation.Backend.Logic;
using Contractor.Core.Generation.Backend.Misc;
using Contractor.Core.Generation.Backend.Persistence;
using Contractor.Core.Generation.Database.Generated.DbContext;
using Contractor.Core.Generation.Database.Persistence.InsertData.Dev;
using Contractor.Core.Generation.Frontend.Model;
using Contractor.Core.Generation.Frontend.Pages;
using Contractor.Core.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core
{
    public class GenerationDependencyProvider
    {
        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            ConfigureTools(serviceCollection);

            MiscBackendGeneration.ConfigureServices(serviceCollection);
            ApiProjectGeneration.ConfigureServices(serviceCollection);
            LogicProjectGeneration.ConfigureServices(serviceCollection);
            PersistenceProjectGeneration.ConfigureServices(serviceCollection);

            GeneratedDTOsProjectGeneration.ConfigureServices(serviceCollection);
            GeneratedInterfacesProjectGeneration.ConfigureServices(serviceCollection);
            GeneratedPersistenceProjectGeneration.ConfigureServices(serviceCollection);

            PersistenceDbContextProjectGeneration.ConfigureServices(serviceCollection);
            PersistenceInsertDataDevProjectGeneration.ConfigureServices(serviceCollection);

            ModelProjectGeneration.ConfigureServices(serviceCollection);
            PagesProjectGeneration.ConfigureServices(serviceCollection);
        }

        private static void ConfigureTools(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IFileSystemClient, FileSystemCacheClient>();

            serviceCollection.AddSingleton<DomainDependencyProvider>();
            serviceCollection.AddSingleton<EntityCoreDependencyProvider>();

            serviceCollection.AddSingleton<ApiDtoPropertyAddition>();
            serviceCollection.AddSingleton<DtoPropertyAddition>();
            serviceCollection.AddSingleton<DtoInterfacePropertyAddition>();
            serviceCollection.AddSingleton<DtoRelationAddition>();

            serviceCollection.AddSingleton<ModuleCoreAddition>();
            serviceCollection.AddSingleton<EntityCoreAddition>();
            serviceCollection.AddSingleton<FrontendDtoPropertyAddition>();
            serviceCollection.AddSingleton<FrontendDtoRelationAddition>();
            serviceCollection.AddSingleton<FrontendDtoPropertyMethodAddition>();

            serviceCollection.AddSingleton<EntitiesPageHtmlPropertyAddition>();
            serviceCollection.AddSingleton<EntitiesPageTsPropertyAddition>();

            serviceCollection.AddSingleton<PathService>();
        }
    }
}