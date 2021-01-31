using Contractor.Core;
using Contractor.Core.Jobs.DomainAddition;
using Contractor.Core.Jobs.EntityAddition;
using System.IO;

namespace Contractor.CLI
{
    public class Test
    {
        public static void Testen()
        {
            DirectoryInfo rootFolder = GetRootFolder();
            string backendDestinationFolder = Path.Join(rootFolder.FullName, "Contract.Architecture\\Contract.Architecture.Backend");
            string dbDestinationFolder = Path.Join(rootFolder.FullName, "Contract.Architecture\\Contract.Architecture.DB");
            TestApiProjectGeneration(backendDestinationFolder, dbDestinationFolder);
        }

        private static void TestApiProjectGeneration(string backendDestinationFolder, string dbDestinationFolder)
        {
            ContractorCoreApi contractorCoreApi = new ContractorCoreApi();
            var options = new DomainOptions()
            {
                BackendDestinationFolder = backendDestinationFolder,
                DbDestinationFolder = dbDestinationFolder,
                ProjectName = "Contract.Architecture",
                DbProjectName = "Contract.Architecture.DB",
                Domain = "Bankwesen"
            };

            // Domain
            DomainOptions domainAdditionOptions = new DomainOptions(options);
            contractorCoreApi.ClearDomain(domainAdditionOptions);
            contractorCoreApi.AddDomain(domainAdditionOptions);

            // Entities
            EntityOptions entityAdditionOptions = new EntityOptions(options)
            {
                EntityName = "Bank",
                EntityNamePlural = "Banken"
            };
            contractorCoreApi.AddEntity(entityAdditionOptions);

            entityAdditionOptions.EntityName = "BankKunde";
            entityAdditionOptions.EntityNamePlural = "BankKunden";
            contractorCoreApi.AddEntity(entityAdditionOptions);

            // Properties
            PropertyOptions propertyAdditionOptions = new PropertyOptions(options)
            {
                EntityName = "Bank",
                EntityNamePlural = "Banken",
                PropertyType = "string",
                PropertyName = "Test",
                PropertyTypeExtra = "256"
            };
            contractorCoreApi.AddProperty(propertyAdditionOptions);

            propertyAdditionOptions.PropertyType = "bool";
            propertyAdditionOptions.PropertyName = "IsTestValue";
            propertyAdditionOptions.PropertyTypeExtra = null;
            contractorCoreApi.AddProperty(propertyAdditionOptions);

            propertyAdditionOptions.PropertyType = "int";
            propertyAdditionOptions.PropertyName = "Count";
            propertyAdditionOptions.PropertyTypeExtra = null;
            contractorCoreApi.AddProperty(propertyAdditionOptions);
        }

        private static DirectoryInfo GetRootFolder()
        {
            return Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent;
        }
    }
}