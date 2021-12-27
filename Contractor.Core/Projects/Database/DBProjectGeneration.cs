using Contractor.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Contractor.Core.Projects.Database
{
    internal class DBProjectGeneration
    {
        public static readonly string DomainFolder = "dbo\\Tables\\Domain";

        public static readonly string TemplateFolder = Folder.Executable + @"\Projects\Database\Templates";

        internal static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ClassGeneration, DbProjectFileGeneration>();
            serviceCollection.AddSingleton<ClassGeneration, DbTableGeneration>();

            serviceCollection.AddSingleton<DbProjectFileDomainAddition>();
            serviceCollection.AddSingleton<DbProjectFileEntityAddition>();
            serviceCollection.AddSingleton<DbTableAddition>();
            serviceCollection.AddSingleton<DbTablePropertyAddition>();
            serviceCollection.AddSingleton<DbTableRelationContraintAddition>();
        }
    }
}