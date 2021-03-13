using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Projects.Logic.Tests
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

            serviceCollection.AddSingleton<DbEntityDetailTestMethodsAddition>();
            serviceCollection.AddSingleton<DbEntityTestMethodsAddition>();
            serviceCollection.AddSingleton<EntityDetailTestMethodsAddition>();
            serviceCollection.AddSingleton<EntityTestMethodsAddition>();
            serviceCollection.AddSingleton<EntityCreateTestMethodsAddition>();
            serviceCollection.AddSingleton<EntityUpdateTestMethodsAddition>();
            serviceCollection.AddSingleton<EntityTestValuesAddition>();
            serviceCollection.AddSingleton<EntityTestValuesRelationAddition>();
            serviceCollection.AddSingleton<DbEntityDetailTestFromAssertAddition>();
            serviceCollection.AddSingleton<DbEntityDetailTestToAssertAddition>();
            serviceCollection.AddSingleton<EntityDetailTestFromAssertAddition>();
            serviceCollection.AddSingleton<EntityDetailTestToAssertAddition>();
            serviceCollection.AddSingleton<EntitiesCrudLogicTestsRelationAddition>();
        }
    }
}