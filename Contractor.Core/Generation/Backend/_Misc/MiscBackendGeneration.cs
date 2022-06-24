using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Generation.Backend.Misc
{
    internal class MiscBackendGeneration
    {

        internal static readonly string TemplateFolder = Folder.Executable + @"\Generation\Backend\_Misc\Templates";

        internal static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, E2ePostmanGeneration>();
            serviceCollection.AddSingleton<E2ePostmanEntityAddition>();
            serviceCollection.AddSingleton<E2ePostmanPropertyAddition>();
            serviceCollection.AddSingleton<E2ePostmanRelationSideAddition>();

        }
    }
}