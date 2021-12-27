using Contractor.Core.Helpers;
using Contractor.Core.Projects.Backend.Logic.Tests;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Projects.Backend.Persistence
{
    internal class PersistenceProjectGeneration
    {
        public static readonly string DomainFolder = "Persistence\\Modules\\Domain\\Entities";
        public static readonly string DtoFolder = DomainFolder + "\\DTOs";

        public static readonly string TemplateFolder = Folder.Executable + @"\Projects\Backend\Persistence\Templates";

        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, DbContextGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, DependencyProviderGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, DbEntityGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, DbEntityDetailGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, DbEntityListItemGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EfEntityGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntitiesCrudRepositoryGeneration>();

            serviceCollection.AddSingleton<DbContextEntityAddition>();
            serviceCollection.AddSingleton<DbContextPropertyAddition>();
            serviceCollection.AddSingleton<DbContextRelationToAddition>();
            serviceCollection.AddSingleton<DbContextRelationToOneToOneAddition>();
            serviceCollection.AddSingleton<DbEntityMethodsAddition>();
            serviceCollection.AddSingleton<DbEntityDetailMethodsAddition>();
            serviceCollection.AddSingleton<DbEntityDetailFromMethodsAddition>();
            serviceCollection.AddSingleton<DbEntityDetailFromOneToOneMethodsAddition>();
            serviceCollection.AddSingleton<DbEntityDetailToMethodsAddition>();
            serviceCollection.AddSingleton<DbEntityListItemMethodsAddition>();
            serviceCollection.AddSingleton<DbEntityListItemFromOneToOneMethodsAddition>();
            serviceCollection.AddSingleton<DbEntityListItemToMethodsAddition>();
            serviceCollection.AddSingleton<EfEntityContructorHashSetAddition>();
            serviceCollection.AddSingleton<EntitiesCrudRepositoryFromIncludeAddition>();
            serviceCollection.AddSingleton<EntitiesCrudRepositoryToIncludeAddition>();
            serviceCollection.AddSingleton<EntitiesCrudRepositoryFromOneToOneIncludeAddition>();
            serviceCollection.AddSingleton<EntitiesCrudRepositoryToOneToOneIncludeAddition>();
        }
    }
}