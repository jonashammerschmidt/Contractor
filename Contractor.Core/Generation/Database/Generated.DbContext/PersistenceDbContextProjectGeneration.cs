using Contractor.Core.Generation.Backend.Persistence;
using Contractor.Core.Helpers;
using Contractor.Core.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Generation.Database.Generated.DbContext
{
    internal class PersistenceDbContextProjectGeneration
    {
        public static readonly string DomainFolder = "Generated.DbContext\\Modules\\Domain\\Entities";
        public static readonly string DtoFolder = DomainFolder + "\\DTOs";

        public static readonly string TemplateFolder = Folder.Executable + @"\Generation\Database\Generated.DbContext\Templates";

        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, DbContextGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EfEntityGeneration>();

            serviceCollection.AddSingleton<DbContextEntityAddition>();
            serviceCollection.AddSingleton<DbContextPropertyAddition>();
            serviceCollection.AddSingleton<DbContextRelationToAddition>();
            serviceCollection.AddSingleton<DbContextRelationToOneToOneAddition>();

            serviceCollection.AddSingleton<EfDtoEntityAddition>();
            serviceCollection.AddSingleton<EfDtoPropertyAddition>();
            serviceCollection.AddSingleton<EfDtoContructorHashSetAddition>();
        }
    }
}