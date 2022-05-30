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
            string backendDestinationFolder = Path.Join(rootFolder.FullName, "FullstackTemplate\\Krz.FullstackTemplate.Backends\\Krz.FullstackTemplate.Backend.Core");
            string dbDestinationFolder = Path.Join(rootFolder.FullName, "FullstackTemplate\\Krz.FullstackTemplate.Databases\\Krz.FullstackTemplate.Database.Core");
            string frontendDestinationFolder = Path.Join(rootFolder.FullName, "FullstackTemplate\\Krz.FullstackTemplate.Frontends\\Krz.FullstackTemplate.Web.Core");
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
                ProjectName = "Krz.FullstackTemplate.Backend.Core",
                DbProjectName = "Krz.FullstackTemplate.Database.Core",
                DbContextName = "FullstackTemplateCoreDbContext",
                Replacements = new Dictionary<string, string>(),
                IsVerbose = false
            };

            contractorOptions.Replacements.Add("Contract.Architecture", "Krz.FullstackTemplate");
            contractorOptions.Replacements.Add("app-search-dropdown", "search-dropdown");
            contractorOptions.Replacements.Add("app-table-filter-bar", "table-filter-bar");
            contractorOptions.Replacements.Add("src/app/components/ui/dropdown-data-source/dropdown-pagination-data-source", "@krz/material");
            contractorOptions.Replacements.Add("src/app/components/ui/search-dropdown/search-dropdown.module", "@krz/material");
            contractorOptions.Replacements.Add("src/app/components/ui/table-filter-bar/table-filter-bar.module", "@krz/material");
            contractorOptions.Replacements.Add("src/app/services/backend/pagination/i-paged-result", "@krz/material");
            contractorOptions.Replacements.Add("src/app/services/backend/pagination/i-pagination-options", "@krz/material");
            contractorOptions.Replacements.Add("src/app/services/backend/pagination/pagination.data-source", "@krz/material");

            AddBanken(contractorCoreApi, contractorOptions);
            AddKunden(contractorCoreApi, contractorOptions);
            AddKonten(contractorCoreApi, contractorOptions);
            AddRelations(contractorCoreApi, contractorOptions);

            contractorCoreApi.SaveChanges(contractorOptions);
        }

        private static void AddBanken(ContractorCoreApi contractorCoreApi, ContractorOptions contractorOptions)
        {
            // Domain
            var domainAdditionOptions = new DomainAdditionOptions(contractorOptions)
            {
                Domain = "Bankwesen"
            };
            contractorCoreApi.AddDomain(domainAdditionOptions);

            // Entities
            EntityAdditionOptions entityAdditionOptions = new EntityAdditionOptions(domainAdditionOptions)
            {
                EntityName = "SchöneBank",
                EntityNamePlural = "SchöneBänke"
            };
            contractorCoreApi.AddEntity(entityAdditionOptions);

            // Properties
            contractorCoreApi.AddProperty(new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.String,
                PropertyName = "Bezeichnung",
                PropertyTypeExtra = "256",
                HasClusteredIndex = true,
                IsUnique = true,
            });

            contractorCoreApi.AddProperty(new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.Boolean,
                PropertyName = "IsPleite",
                HasClusteredIndex = true,
            });

            contractorCoreApi.AddProperty(new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.String,
                PropertyName = "LegacyBezeichnung",
                PropertyTypeExtra = "256",
                IsOptional = true,
            });

            contractorCoreApi.AddProperty(new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.Boolean,
                PropertyName = "LegacyIsPleite",
                IsOptional = true,
            });
        }

        private static void AddKunden(ContractorCoreApi contractorCoreApi, ContractorOptions contractorOptions)
        {
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
                EntityNamePlural = "Kunden"
            };
            contractorCoreApi.AddEntity(entityAdditionOptions);

            // Properties
            contractorCoreApi.AddProperty(new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.String,
                PropertyName = "Bezeichnung",
                PropertyTypeExtra = "256",
                HasClusteredIndex = true,
            });

            contractorCoreApi.AddProperty(new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.Integer,
                PropertyName = "Kundennummer",
                IsUnique = true,
            });

            contractorCoreApi.AddProperty(new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.Integer,
                PropertyName = "KundennummerLegacy",
                IsOptional = true,
                HasNonClusteredIndex = true,
            });
        }

        private static void AddKonten(ContractorCoreApi contractorCoreApi, ContractorOptions contractorOptions)
        {
            // Domain
            var domainAdditionOptions = new DomainAdditionOptions(contractorOptions)
            {
                Domain = "Kontoführung"
            };
            contractorCoreApi.AddDomain(domainAdditionOptions);

            // Entities
            EntityAdditionOptions entityAdditionOptions = new EntityAdditionOptions(domainAdditionOptions)
            {
                EntityName = "Konto",
                EntityNamePlural = "Konten"
            };
            contractorCoreApi.AddEntity(entityAdditionOptions);

            // Properties
            contractorCoreApi.AddProperty(new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.String,
                PropertyName = "Bezeichnung",
                PropertyTypeExtra = "256"
            });

            contractorCoreApi.AddProperty(new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.Guid,
                PropertyName = "SystemKontoId",
            });

            contractorCoreApi.AddProperty(new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.Double,
                PropertyName = "Balance",
            });

            contractorCoreApi.AddProperty(new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.DateTime,
                PropertyName = "EröffnetAm",
            });

            contractorCoreApi.AddProperty(new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.Guid,
                PropertyName = "LegacySystemKontoId",
                IsOptional = true,
            });

            contractorCoreApi.AddProperty(new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.Double,
                PropertyName = "LegacyBalance",
                IsOptional = true,
            });

            contractorCoreApi.AddProperty(new PropertyAdditionOptions(entityAdditionOptions)
            {
                PropertyType = PropertyTypes.DateTime,
                PropertyName = "LegacyEröffnetAm",
                IsOptional = true,
            });
        }

        private static void AddRelations(ContractorCoreApi contractorCoreApi, ContractorOptions contractorOptions)
        {
            contractorCoreApi.Add1ToNRelation(new RelationAdditionOptions(contractorOptions)
            {
                DomainFrom = "Bankwesen",
                EntityNameFrom = "SchöneBank",
                EntityNamePluralFrom = "SchöneBänke",
                PropertyNameFrom = "MeineBank",
                DomainTo = "Kundenstamm",
                EntityNameTo = "Kunde",
                EntityNamePluralTo = "Kunden",
                PropertyNameTo = "MeineKunden",
                IsOptional = false,
                
            });

            contractorCoreApi.Add1ToNRelation(new RelationAdditionOptions(contractorOptions)
            {
                DomainFrom = "Bankwesen",
                EntityNameFrom = "SchöneBank",
                EntityNamePluralFrom = "SchöneBänke",
                PropertyNameFrom = "BäckupBank",
                DomainTo = "Kundenstamm",
                EntityNameTo = "Kunde",
                EntityNamePluralTo = "Kunden",
                PropertyNameTo = "BäckupKunden",
                IsOptional = true,
            });

            contractorCoreApi.AddOneToOneRelation(new RelationAdditionOptions(contractorOptions)
            {
                DomainFrom = "Kundenstamm",
                EntityNameFrom = "Kunde",
                EntityNamePluralFrom = "Kunden",
                PropertyNameFrom = "MeinKunde",
                DomainTo = "Kontoführung",
                EntityNameTo = "Konto",
                EntityNamePluralTo = "Konten",
                PropertyNameTo = "VerbürgterKunde",
                IsOptional = false,
            });

            contractorCoreApi.AddOneToOneRelation(new RelationAdditionOptions(contractorOptions)
            {
                DomainFrom = "Kundenstamm",
                EntityNameFrom = "Kunde",
                EntityNamePluralFrom = "Kunden",
                PropertyNameFrom = "BäckupKunde",
                DomainTo = "Kontoführung",
                EntityNameTo = "Konto",
                EntityNamePluralTo = "Konten",
                PropertyNameTo = "BäckupKonto",
                IsOptional = true,
            });
        }

        private static DirectoryInfo GetRootFolder()
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            if (currentDirectory.Contains("\\Contractor.CLI"))
            {
                currentDirectory = currentDirectory.Split("\\Contractor.CLI")[0];
            }

            return new DirectoryInfo(currentDirectory);
        }
    }
}