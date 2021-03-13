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
            serviceCollection.AddSingleton<DbDtoMethodsAddition>();
            serviceCollection.AddSingleton<DbDtoDetailMethodsAddition>();
            serviceCollection.AddSingleton<DbDtoDetailFromMethodsAddition>();
            serviceCollection.AddSingleton<DbDtoDetailToMethodsAddition>();
            serviceCollection.AddSingleton<EfDtoContructorHashSetAddition>();
            serviceCollection.AddSingleton<DtoFromRepositoryIncludeAddition>();
            serviceCollection.AddSingleton<DtoToRepositoryIncludeAddition>();
        }
    }
}