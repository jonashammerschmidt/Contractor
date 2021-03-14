using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Projects.Frontend.Model
{
    internal class ModelProjectGeneration
    {
        internal static readonly string DomainFolder = "src\\app\\model\\{domain-kebab}\\{entities-kebab}";

        internal static readonly string TemplateFolder = Folder.Executable + @"\Projects\Frontend\Model\Templates";

        internal static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, EntitiesCrudServiceGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntitiesModuleGeneration>();
        }
    }
}