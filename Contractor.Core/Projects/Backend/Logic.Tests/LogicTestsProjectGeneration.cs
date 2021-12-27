using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Projects.Backend.Logic.Tests
{
    internal class LogicTestsProjectGeneration
    {
        public static readonly string DomainFolder = "Logic.Tests\\Modules\\Domain\\Entities";
        public static readonly string DtoFolder = DomainFolder + "\\DTOs";

        public static readonly string TemplateFolder = Folder.Executable + @"\Projects\Backend\Logic.Tests\Templates";

        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, DbEntityDetailTestGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, DbEntityListItemTestGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, DbEntityTestGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, DbEntityUpdateTestGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityCreateTestGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityDetailTestGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityListItemTestGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityTestGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityUpdateTestGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntitiesCrudLogicTestsGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, EntityTestValuesGeneration>();

            serviceCollection.AddSingleton<DbEntityDetailTestMethodsAddition>();
            serviceCollection.AddSingleton<DbEntityDetailTestFromAssertOneToOneAddition>();
            serviceCollection.AddSingleton<DbEntityListItemTestMethodsAddition>();
            serviceCollection.AddSingleton<DbEntityTestMethodsAddition>();
            serviceCollection.AddSingleton<DbEntityUpdateTestMethodsAddition>();
            serviceCollection.AddSingleton<EntityDetailTestMethodsAddition>();
            serviceCollection.AddSingleton<EntityListItemTestMethodsAddition>();
            serviceCollection.AddSingleton<EntityListItemTestFromOneToOneAssertAddition>();
            serviceCollection.AddSingleton<EntityTestMethodsAddition>();
            serviceCollection.AddSingleton<EntityCreateTestMethodsAddition>();
            serviceCollection.AddSingleton<EntityUpdateTestMethodsAddition>();
            serviceCollection.AddSingleton<EntityTestValuesAddition>();
            serviceCollection.AddSingleton<EntityTestValuesRelationAddition>();
            serviceCollection.AddSingleton<DbEntityDetailTestFromAssertAddition>();
            serviceCollection.AddSingleton<DbEntityDetailTestToAssertAddition>();
            serviceCollection.AddSingleton<DbEntityListItemTestFromAssertAddition>();
            serviceCollection.AddSingleton<DbEntityListItemTestToAssertAddition>();
            serviceCollection.AddSingleton<EntityDetailTestFromAssertAddition>();
            serviceCollection.AddSingleton<EntityDetailTestFromOneToOneAssertAddition>();
            serviceCollection.AddSingleton<EntityDetailTestToAssertAddition>();
            serviceCollection.AddSingleton<EntityListItemTestToAssertAddition>();
            serviceCollection.AddSingleton<EntitiesCrudLogicTestsRelationAddition>();
            serviceCollection.AddSingleton<EntitiesCrudLogicTestsToOneToOneRelationAddition>();
        }
    }
}