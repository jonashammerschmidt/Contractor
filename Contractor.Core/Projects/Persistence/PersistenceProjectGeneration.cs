using Contractor.Core.Helpers;
using Contractor.Core.Projects.Logic.Tests;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Projects.Persistence
{
    internal class PersistenceProjectGeneration
    {
        public static readonly string DomainFolder = "Persistence\\Modules\\{Domain}\\{Entities}";

        public static readonly string TemplateFolder = Folder.Executable + @"\Projects\Persistence\Templates";

        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, DbContextGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, DependencyProviderGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, DbEntityGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, DbEntityDetailGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EfEntityGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntitiesCrudRepositoryGeneration>();

            serviceCollection.AddSingleton<DbContextEntityAddition>();
            serviceCollection.AddSingleton<DbContextPropertyAddition>();
            serviceCollection.AddSingleton<DbContextRelationToAddition>();
            serviceCollection.AddSingleton<DbEntityMethodsAddition>();
            serviceCollection.AddSingleton<DbEntityDetailMethodsAddition>();
            serviceCollection.AddSingleton<DbEntityDetailFromMethodsAddition>();
            serviceCollection.AddSingleton<DbEntityDetailToMethodsAddition>();
            serviceCollection.AddSingleton<EfEntityContructorHashSetAddition>();
            serviceCollection.AddSingleton<EntitiesCrudRepositoryFromIncludeAddition>();
            serviceCollection.AddSingleton<EntitiesCrudRepositoryToIncludeAddition>();
        }
    }
}