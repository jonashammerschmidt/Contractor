using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Projects.Api
{
    internal class ApiProjectGeneration
    {
        internal static readonly string DomainFolder = "API\\Modules\\{Domain}\\{Entities}";

        internal static readonly string TemplateFolder = Folder.Executable + @"\Projects\Api\Templates";

        internal static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, EntitiesCrudControllerGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityCreateGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityUpdateGeneration>();
        }
    }
}