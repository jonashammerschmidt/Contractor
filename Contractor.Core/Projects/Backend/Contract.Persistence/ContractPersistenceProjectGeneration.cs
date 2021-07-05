using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Projects.Backend.Contract.Persistence
{
    internal class ContractPersistenceProjectGeneration
    {
        public static readonly string DomainFolder = "Contract\\Persistence\\Modules\\{Domain}\\{Entities}";

        public static readonly string TemplateFolder = Folder.Executable + @"\Projects\Backend\Contract.Persistence\Templates";

        internal static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, IEntitiesCrudRepositoryGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, IDbEntityGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, IDbEntityDetailGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, IDbEntityListItemGeneration>();
        }
    }
}