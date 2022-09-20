using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Generation.Database.Persistence.InsertData.Dev
{
    internal class PersistenceInsertDataDevProjectGeneration
    {
        public static readonly string DomainFolder = "InsertData.Dev\\CsvData\\Domain";

        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, CsvDataGeneration>();
            serviceCollection.AddSingleton<CsvDataEntityAddition>();
            serviceCollection.AddSingleton<CsvDataPropertyAddition>();
            serviceCollection.AddSingleton<CsvDataRelationToAddition>();
        }
    }
}