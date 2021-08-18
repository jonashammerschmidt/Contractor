using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Projects.Backend.Contract.Logic
{
    internal class ContractLogicProjectGeneration
    {
        public static readonly string DomainFolder = "Contract\\Logic\\Modules\\{Domain}\\{Entities}";

        public static readonly string TemplateFolder = Folder.Executable + @"\Projects\Backend\Contract.Logic\Templates";

        internal static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, IEntitiesCrudLogicGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, IEntityGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, IEntityDetailGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, IEntityListItemGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, IEntityCreateGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, IEntityUpdateGeneration>();
        }
    }
}