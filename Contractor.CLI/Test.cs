using Contractor.Core;
using Contractor.Core.Options;
using System.IO;

namespace Contractor.CLI
{
    internal class Test
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
            var contractorOptions = new ContractorOptions()
            {
                BackendDestinationFolder = backendDestinationFolder,
                DbDestinationFolder = dbDestinationFolder,
                ProjectName = "Contract.Architecture",
                DbProjectName = "Contract.Architecture.DB",
            };

            AddBankwesen(contractorOptions);
            //AddKonto(contractorOptions);
            AddKundenstamm(contractorOptions);
            AddRelation(contractorOptions);
        }

        private static void AddBankwesen(ContractorOptions contractorOptions)
        {
            ContractorCoreApi contractorCoreApi = new ContractorCoreApi();

            // Domain
            var domainAdditionOptions = new DomainAdditionOptions(contractorOptions)
            {
                Domain = "Bankwesen"
            };
            contractorCoreApi.AddDomain(domainAdditionOptions);

            // Entities
            EntityAdditionOptions entityAdditionOptions = new EntityAdditionOptions(domainAdditionOptions)
            {
                EntityName = "Bank",
                EntityNamePlural = "Banken",
                ForMandant = false
            };
            contractorCoreApi.AddEntity(entityAdditionOptions);

            // Properties
            PropertyAdditionOptions propertyAdditionOptions = new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = "string",
                PropertyName = "Name",
                PropertyTypeExtra = "256"
            };
            contractorCoreApi.AddProperty(propertyAdditionOptions);
        }

        private static void AddKonto(ContractorOptions contractorOptions)
        {
            ContractorCoreApi contractorCoreApi = new ContractorCoreApi();

            // Entities
            EntityAdditionOptions entityAdditionOptions = new EntityAdditionOptions(contractorOptions)
            {
                Domain = "Bankwesen",
                EntityName = "Konto",
                EntityNamePlural = "Konten",
                ForMandant = false
            };
            contractorCoreApi.AddEntity(entityAdditionOptions);

            // Properties
            PropertyAdditionOptions propertyAdditionOptions = new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = "DateTime",
                PropertyName = "EroeffnetAm"
            };
            contractorCoreApi.AddProperty(propertyAdditionOptions);
        }

        private static void AddKundenstamm(ContractorOptions contractorOptions)
        {
            ContractorCoreApi contractorCoreApi = new ContractorCoreApi();

            // Domain
            var domainAdditionOptions = new DomainAdditionOptions(contractorOptions)
            {
                Domain = "Kundenstamm"
            };
            contractorCoreApi.AddDomain(domainAdditionOptions);

            // Entities
            EntityAdditionOptions entityAdditionOptions = new EntityAdditionOptions(domainAdditionOptions)
            {
                EntityName = "Kunde",
                EntityNamePlural = "Kunden",
                ForMandant = false
            };
            contractorCoreApi.AddEntity(entityAdditionOptions);

            // Properties
            PropertyAdditionOptions propertyAdditionOptions = new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = "int",
                PropertyName = "Balance"
            };
            contractorCoreApi.AddProperty(propertyAdditionOptions);
        }

        private static void AddRelation(ContractorOptions contractorOptions)
        {
            ContractorCoreApi contractorCoreApi = new ContractorCoreApi();

            RelationAdditionOptions relationOptions = new RelationAdditionOptions(contractorOptions)
            {
                DomainFrom = "Bankwesen",
                DomainTo = "Kundenstamm",
                EntityNameFrom = "Bank",
                EntityNamePluralFrom = "Banken",
                EntityNameTo = "Kunde",
                EntityNamePluralTo = "Kunden",
            };
            contractorCoreApi.Add1ToNRelation(relationOptions);

            //relationOptions = new RelationAdditionOptions(contractorOptions)
            //{
            //    DomainFrom = "Bankwesen",
            //    DomainTo = "Kundenstamm",
            //    EntityNameFrom = "Konto",
            //    EntityNamePluralFrom = "Konten",
            //    EntityNameTo = "Kunde",
            //    EntityNamePluralTo = "Kunden",
            //};
            //contractorCoreApi.Add1ToNRelation(relationOptions);
        }

        private static DirectoryInfo GetRootFolder()
        {
            return Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent;
        }
    }
}