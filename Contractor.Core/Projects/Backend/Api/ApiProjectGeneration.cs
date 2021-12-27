using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Projects.Backend.Api
{
    internal class ApiProjectGeneration
    {
        internal static readonly string DomainFolder = "API\\Modules\\Domain\\Entities";
        internal static readonly string DtoFolder = DomainFolder + "\\DTOs";

        internal static readonly string TemplateFolder = Folder.Executable + @"\Projects\Backend\Api\Templates";

        internal static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, EntitiesCrudController>();
            serviceCollection.AddSingleton<EntitiesCrudControllerRelationAddition>();

            serviceCollection.AddSingleton<ClassGeneration, EntityCreateGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityUpdateGeneration>();

        }
    }
}