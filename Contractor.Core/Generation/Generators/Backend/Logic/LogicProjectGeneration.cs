using System.IO;
using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Generation.Backend.Logic
{
    public class LogicProjectGeneration
    {
        public static readonly string ProjectFolder = "API";
        public static readonly string DomainFolder = Path.Combine("API", "Modules", "Domain", "Entities", "Logic");

        public static readonly string TemplateFolder = Path.Combine(Folder.Executable, "Generation", "Generators", "Backend", "Logic", "Templates");
        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, EntitiesCrudLogicGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, IEntitiesCrudLogicGeneration>();
        }
    }
}