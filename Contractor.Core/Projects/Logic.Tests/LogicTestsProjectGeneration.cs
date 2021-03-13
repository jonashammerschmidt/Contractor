using Contractor.Core.Helpers;
using Contractor.Core.Projects.Logic.Tests;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Projects
{
    internal class LogicTestsProjectGeneration
    {
        public static readonly string DomainFolder = "Logic.Tests\\Modules\\{Domain}\\{Entities}";

        public static readonly string TemplateFolder = Folder.Executable + @"\Projects\Logic.Tests\Templates";

        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, DbEntityDetailTestGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, DbEntityTestGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityCreateTestGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityDetailTestGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityTestGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityUpdateTestGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntitiesCrudLogicTestsGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityTestValuesGeneration>();

            serviceCollection.AddSingleton<LogicDbDtoDetailTestMethodsAddition>();
            serviceCollection.AddSingleton<LogicDbDtoTestMethodsAddition>();
            serviceCollection.AddSingleton<LogicDtoDetailTestMethodsAddition>();
            serviceCollection.AddSingleton<LogicDtoTestMethodsAddition>();
            serviceCollection.AddSingleton<LogicDtoCreateTestMethodsAddition>();
            serviceCollection.AddSingleton<LogicDtoUpdateTestMethodsAddition>();
            serviceCollection.AddSingleton<LogicDtoTestValuesAddition>();
            serviceCollection.AddSingleton<LogicDtoTestValuesRelationAddition>();
            serviceCollection.AddSingleton<LogicDbDtoDetailTestFromAssertAddition>();
            serviceCollection.AddSingleton<LogicDbDtoDetailTestToAssertAddition>();
            serviceCollection.AddSingleton<LogicDtoDetailTestFromAssertAddition>();
            serviceCollection.AddSingleton<LogicDtoDetailTestToAssertAddition>();
            serviceCollection.AddSingleton<LogicTestsRelationAddition>();
        }
    }
}