using System.IO;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Generation.Database.Persistence.InsertData.Dev
{
    public class PersistenceInsertDataDevProjectGeneration
    {
        public static readonly string DomainFolder = Path.Combine("InsertData.Dev", "CsvData", "Domain");

        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, CsvDataGeneration>();
            serviceCollection.AddSingleton<CsvDataEntityAddition>();
            serviceCollection.AddSingleton<CsvDataPropertyAddition>();
            serviceCollection.AddSingleton<CsvDataRelationToAddition>();
        }
    }
}