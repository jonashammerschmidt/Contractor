using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Generation.Backend.Logic
{
    internal class LogicProjectGeneration
    {
        public static readonly string ProjectFolder = "API";
        public static readonly string DomainFolder = "API\\Modules\\Domain\\Entities\\Logic";

        public static readonly string TemplateFolder = Folder.Executable + @"\Generation\Backend\Logic\Templates";

        internal static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, EntitiesCrudLogicGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, IEntitiesCrudLogicGeneration>();
        }
    }
}