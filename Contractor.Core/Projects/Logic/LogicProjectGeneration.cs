using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Projects.Logic
{
    internal class LogicProjectGeneration
    {
        public static readonly string ProjectFolder = "Logic";
        public static readonly string DomainFolder = "Logic\\Modules\\{Domain}\\{Entities}";
        public static readonly string TemplateFolder = Folder.Executable + @"\Projects\Logic\Templates";

        internal static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, EntitiesCrudLogicGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityDetailGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, DbEntityGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, LogicDependencyProviderGeneration>();

            serviceCollection.AddSingleton<LogicRelationAddition>();
            serviceCollection.AddSingleton<DtoDetailFromMethodsAddition>();
            serviceCollection.AddSingleton<DtoDetailMethodsAddition>();
            serviceCollection.AddSingleton<DtoDetailToMethodsAddition>();
            serviceCollection.AddSingleton<DtoMethodsAddition>();
        }
    }
}