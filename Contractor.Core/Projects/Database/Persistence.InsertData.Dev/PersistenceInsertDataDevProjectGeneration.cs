using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Projects.Database.Persistence.InsertData.Dev
{
    internal class PersistenceInsertDataDevProjectGeneration
    {
        public static readonly string DomainFolder = "InsertData.Dev\\CsvData\\Domain";

        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, PersistenceInsertDataDevProjectFileGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, CsvDataGeneration>();

            serviceCollection.AddSingleton<PersistenceInsertDataDevProjectFileEntityAddition>();
            serviceCollection.AddSingleton<CsvDataEntityAddition>();
            serviceCollection.AddSingleton<CsvDataPropertyAddition>();
            serviceCollection.AddSingleton<CsvDataRelationToAddition>();
        }
    }
}