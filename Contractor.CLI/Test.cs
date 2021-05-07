﻿using Contractor.Core;
using Contractor.Core.Options;
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
            var contractorOptions = new ContractorOptions()
            {
                BackendDestinationFolder = backendDestinationFolder,
                DbDestinationFolder = dbDestinationFolder,
                FrontendDestinationFolder = frontendDestinationFolder,
                ProjectName = "Contract.Architecture.Backend.Core",
                DbProjectName = "Contract.Architecture.Database.Core",
            };

            AddBanken(contractorOptions);
            AddKunden(contractorOptions);
            AddRelation(contractorOptions);
        }

        private static void AddBanken(ContractorOptions contractorOptions)
        {
            ContractorCoreApi contractorCoreApi = new ContractorCoreApi();

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
                PropertyName = "Name",
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

        private static void AddKunden(ContractorOptions contractorOptions)
        {
            ContractorCoreApi contractorCoreApi = new ContractorCoreApi();

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
                PropertyName = "Name",
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

        private static void AddRelation(ContractorOptions contractorOptions)
        {
            ContractorCoreApi contractorCoreApi = new ContractorCoreApi();

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
            };
            contractorCoreApi.Add1ToNRelation(relationOptions);
        }

        private static DirectoryInfo GetRootFolder()
        {
            return Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent;
        }
    }
}