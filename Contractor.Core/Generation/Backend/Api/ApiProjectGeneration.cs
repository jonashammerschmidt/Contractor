using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Generation.Backend.Api
{
    internal class ApiProjectGeneration
    {
        internal static readonly string DomainFolder = "API\\Modules\\Domain\\Entities\\API";

        internal static readonly string TemplateFolder = Folder.Executable + @"\Generation\Backend\Api\Templates";

        internal static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, DependencyProviderGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntitiesCrudController>();
            serviceCollection.AddSingleton<EntitiesCrudControllerRelationAddition>();
        }
    }
}