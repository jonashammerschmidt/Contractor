using Contractor.Core;
using Contractor.Core.Options;
using System.Collections.Generic;
using System.IO;

namespace Contractor.CLI
{
    internal class Test
    {
        public static void Testen()
        {
            DirectoryInfo rootFolder = GetRootFolder();
            string backendDestinationFolder = Path.Join(rootFolder.FullName, "Contract.Architecture\\Contract.Architecture.Backends\\Contract.Architecture.Backend.Core");
            string dbDestinationFolder = Path.Join(rootFolder.FullName, "Contract.Architecture\\Contract.Architecture.Databases\\Contract.Architecture.Database.Core");
            string frontendDestinationFolder = Path.Join(rootFolder.FullName, "Contract.Architecture\\Contract.Architecture.Frontends\\Contract.Architecture.Web.Core");
            TestApiProjectGeneration(backendDestinationFolder, dbDestinationFolder, frontendDestinationFolder);
        }

        private static void TestApiProjectGeneration(
            string backendDestinationFolder,
            string dbDestinationFolder,
            string frontendDestinationFolder)
        {
            ContractorCoreApi contractorCoreApi = new ContractorCoreApi();

            var contractorOptions = new ContractorOptions()
            {
                BackendDestinationFolder = backendDestinationFolder,
                DbDestinationFolder = dbDestinationFolder,
                FrontendDestinationFolder = frontendDestinationFolder,
                ProjectName = "Contract.Architecture.Backend.Core",
                DbProjectName = "Contract.Architecture.Database.Core",
                Replacements = new Dictionary<string, string>(),
                IsVerbose = false
            };

            AddBanken(contractorCoreApi, contractorOptions);
            AddKunden(contractorCoreApi, contractorOptions);
            AddRelation(contractorCoreApi, contractorOptions);

            contractorCoreApi.SaveChanges(contractorOptions);
        }

        private static void AddBanken(ContractorCoreApi contractorCoreApi, ContractorOptions contractorOptions)
        {
            // Domain
            var domainAdditionOptions = new DomainAdditionOptions(contractorOptions)
            {
                Domain = "GegönntesBankwesen"
            };
            contractorCoreApi.AddDomain(domainAdditionOptions);

            // Entities
            EntityAdditionOptions entityAdditionOptions = new EntityAdditionOptions(domainAdditionOptions)
            {
                EntityName = "GegönnteBank",
                EntityNamePlural = "GegönnteBanken"
            };
            contractorCoreApi.AddEntity(entityAdditionOptions);

            // Properties
            PropertyAdditionOptions propertyAdditionOptions = new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.String,
                PropertyName = "Bezeichnung",
                PropertyTypeExtra = "256"
            };
            contractorCoreApi.AddProperty(propertyAdditionOptions);

            propertyAdditionOptions = new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.String,
                PropertyName = "GegönnterName",
                PropertyTypeExtra = "256"
            };
            contractorCoreApi.AddProperty(propertyAdditionOptions);

            propertyAdditionOptions = new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.Boolean,
                PropertyName = "GegönnterBoolean"
            };
            contractorCoreApi.AddProperty(propertyAdditionOptions);

            propertyAdditionOptions = new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.DateTime,
                PropertyName = "GegönntesDateTime",
            };
            contractorCoreApi.AddProperty(propertyAdditionOptions);

            propertyAdditionOptions = new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.Double,
                PropertyName = "GegönnterDouble"
            };
            contractorCoreApi.AddProperty(propertyAdditionOptions);

            propertyAdditionOptions = new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.Guid,
                PropertyName = "GegönnteGuid",
            };
            contractorCoreApi.AddProperty(propertyAdditionOptions);

            propertyAdditionOptions = new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.Integer,
                PropertyName = "GegönnterInteger"
            };
            contractorCoreApi.AddProperty(propertyAdditionOptions);
        }

        private static void AddKunden(ContractorCoreApi contractorCoreApi, ContractorOptions contractorOptions)
        {
            // Domain
            var domainAdditionOptions = new DomainAdditionOptions(contractorOptions)
            {
                Domain = "GegönnterKundenstamm"
            };
            contractorCoreApi.AddDomain(domainAdditionOptions);

            // Entities
            EntityAdditionOptions entityAdditionOptions = new EntityAdditionOptions(domainAdditionOptions)
            {
                EntityName = "GegönnterKunde",
                EntityNamePlural = "GegönnteKunden"
            };
            contractorCoreApi.AddEntity(entityAdditionOptions);

            // Properties
            PropertyAdditionOptions propertyAdditionOptions = new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.String,
                PropertyName = "Bezeichnung",
                PropertyTypeExtra = "256"
            };
            contractorCoreApi.AddProperty(propertyAdditionOptions);

            propertyAdditionOptions = new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.String,
                PropertyName = "GegönnterName",
                PropertyTypeExtra = "256"
            };
            contractorCoreApi.AddProperty(propertyAdditionOptions);

            propertyAdditionOptions = new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.Boolean,
                PropertyName = "GegönnterBoolean",
                IsOptional = true,
            };
            contractorCoreApi.AddProperty(propertyAdditionOptions);

            propertyAdditionOptions = new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.DateTime,
                PropertyName = "GegönnterDateTime",
                IsOptional = true,
            };
            contractorCoreApi.AddProperty(propertyAdditionOptions);

            propertyAdditionOptions = new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.Double,
                PropertyName = "GegönnterDouble",
                IsOptional = true,
            };
            contractorCoreApi.AddProperty(propertyAdditionOptions);

            propertyAdditionOptions = new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.Guid,
                PropertyName = "GegönnterGuid",
                IsOptional = true,
            };
            contractorCoreApi.AddProperty(propertyAdditionOptions);

            propertyAdditionOptions = new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.Integer,
                PropertyName = "GegönnterInteger",
                IsOptional = true,
            };
            contractorCoreApi.AddProperty(propertyAdditionOptions);
        }

        private static void AddRelation(ContractorCoreApi contractorCoreApi, ContractorOptions contractorOptions)
        {
            RelationAdditionOptions relationOptions = new RelationAdditionOptions(contractorOptions)
            {
                DomainFrom = "GegönntesBankwesen",
                EntityNameFrom = "GegönnteBank",
                EntityNamePluralFrom = "GegönnteBanken",
                PropertyNameFrom = "BesteBank",
                DomainTo = "GegönnterKundenstamm",
                EntityNameTo = "GegönnterKunde",
                EntityNamePluralTo = "GegönnteKunden",
                PropertyNameTo = "BesteKunden",
                IsOptional = true,
            };
            contractorCoreApi.Add1ToNRelation(relationOptions);

            relationOptions = new RelationAdditionOptions(contractorOptions)
            {
                DomainFrom = "GegönntesBankwesen",
                EntityNameFrom = "GegönnteBank",
                EntityNamePluralFrom = "GegönnteBanken",
                PropertyNameFrom = "LieblingsBank",
                DomainTo = "GegönnterKundenstamm",
                EntityNameTo = "GegönnterKunde",
                EntityNamePluralTo = "GegönnteKunden",
                PropertyNameTo = "LieblingsKunde",
                IsOptional = true,
            };
            contractorCoreApi.AddOneToOneRelation(relationOptions);
        }

        private static DirectoryInfo GetRootFolder()
        {
            return Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent;
        }
    }
}