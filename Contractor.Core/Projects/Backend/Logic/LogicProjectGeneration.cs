using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Projects.Backend.Logic
{
    internal class LogicProjectGeneration
    {
        public static readonly string ProjectFolder = "Logic";
        public static readonly string DomainFolder = "Logic\\Modules\\{Domain}\\{Entities}";
        public static readonly string TemplateFolder = Folder.Executable + @"\Projects\Backend\Logic\Templates";

        internal static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, EntitiesCrudLogicGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityDetailGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityListItemGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, DbEntityGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, LogicDependencyProviderGeneration>();

            serviceCollection.AddSingleton<EntitiesCrudLogicRelationAddition>();
            serviceCollection.AddSingleton<EntityDetailFromMethodsAddition>();
            serviceCollection.AddSingleton<EntityDetailMethodsAddition>();
            serviceCollection.AddSingleton<EntityDetailToMethodsAddition>();
            serviceCollection.AddSingleton<EntityMethodsAddition>();
        }
    }
}