using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Projects.Contract.Persistence
{
    internal class ContractPersistenceProjectGeneration
    {
        public static readonly string DomainFolder = "Contract\\Persistence\\Modules\\{Domain}\\{Entities}";

        public static readonly string TemplateFolder = Folder.Executable + @"\Projects\Contract.Persistence\Templates";

        internal static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, IEntitiesCrudRepositoryGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, IDbEntityGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, IDbEntityDetailGeneration>();
        }
    }
}