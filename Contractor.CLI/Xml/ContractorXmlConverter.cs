using Contractor.Core.MetaModell;
using System.Xml;

namespace Contractor.CLI
{
    public class ContractorXmlConverter
    {
        public static GenerationOptions ToContractorGenerationOptions(
            ContractorXml contractorXml,
            string contractorXmlFolderPath)
        {
            var contractorGenerationOptions = new GenerationOptions();
            contractorGenerationOptions.Paths = new Paths();
            contractorGenerationOptions.Paths.BackendDestinationFolder = Path.GetFullPath(Path.Combine(contractorXmlFolderPath, FixPath(contractorXml.Paths.BackendDestinationFolder)));
            contractorGenerationOptions.Paths.BackendGeneratedDestinationFolder = Path.GetFullPath(Path.Combine(contractorXmlFolderPath, FixPath(contractorXml.Paths.BackendGeneratedDestinationFolder)));
            contractorGenerationOptions.Paths.DbDestinationFolder = Path.GetFullPath(Path.Combine(contractorXmlFolderPath, FixPath(contractorXml.Paths.DbDestinationFolder)));
            contractorGenerationOptions.Paths.FrontendDestinationFolder = Path.GetFullPath(Path.Combine(contractorXmlFolderPath, FixPath(contractorXml.Paths.FrontendDestinationFolder)));
            contractorGenerationOptions.Paths.ProjectName = contractorXml.Paths.ProjectName;
            contractorGenerationOptions.Paths.GeneratedProjectName = contractorXml.Paths.GeneratedProjectName;
            contractorGenerationOptions.Paths.DbProjectName = contractorXml.Paths.DbProjectName;
            contractorGenerationOptions.Paths.DbContextName = contractorXml.Paths.DbContextName;
            contractorGenerationOptions.Replacements = contractorXml.Replacements.Replacements
                .Select(replacement =>
                {
                    var replacement1 = new Replacement();
                    replacement1.Pattern = replacement.Pattern;
                    replacement1.ReplaceWith = replacement.ReplaceWith;
                    return replacement1;
                })
                .ToList();
            contractorGenerationOptions.Modules = ConvertModules(contractorXml.Modules);
            contractorGenerationOptions.CustomDtos = contractorXml.CustomDtos.CustomDtos
                .Select(customDto =>
                {
                    var dto = new CustomDto();
                    dto.EntityName = customDto.Entity;
                    dto.Purpose = customDto.Purpose;
                    dto.Properties = customDto.Properties
                        .Select(property =>
                        {
                            var dtoProperty = new CustomDtoProperty();
                            dtoProperty.Path = property.Path;
                            return dtoProperty;
                        })
                        .ToList();
                    return dto;
                })
                .ToList();

            return contractorGenerationOptions;
        }

        private static string FixPath(string path)
        {
            return Path.Combine(path.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries));
        }

        internal static void AddToContractorGenerationOptions(
            GenerationOptions generationOptions,
            ContractorIncludeXml contractorIncludeXml)
        {
            List<Module> modules = ConvertModules(contractorIncludeXml.Modules);

            generationOptions.Modules = generationOptions.Modules
                .Concat(modules)
                .ToList();
        }

        private static List<Module> ConvertModules(ModulesXml modules)
        {
            return modules.Modules.Select(module => new Module()
            {
                Name = module.Name,
                Skip = module.Skip,
                Entities = module.Entities.Select(entity =>
                {
                    return new Entity()
                    {
                        Name = entity.Name,
                        NamePlural = entity.NamePlural,
                        ScopeEntityName = entity.ScopeEntityName,
                        IdType = entity.IdType,
                        Skip = entity.Skip,
                        Properties = entity.Properties.Select(property => new Property()
                        {
                            Type = property.Type,
                            Name = property.Name,
                            IsOptional = property.IsOptional,
                            IsDisplayProperty = property.IsDisplayProperty,
                            IsHidden = property.IsHidden,
                        }).ToList(),
                        Relations1To1 = entity.Relations1To1.Select(relation1To1 => new Relation1To1()
                        {
                            EntityNameFrom = relation1To1.EntityNameFrom,
                            PropertyNameFrom = relation1To1.PropertyNameFrom,
                            PropertyNameTo = relation1To1.PropertyNameTo,
                            IsOptional = relation1To1.IsOptional,
                            OnDelete = ParseRelationDeleteBehaviour(relation1To1.OnDelete),
                        }).ToList(),
                        Relations1ToN = entity.Relation1ToN.Select(relation1ToN => new Relation1ToN()
                        {
                            EntityNameFrom = relation1ToN.EntityNameFrom,
                            PropertyNameFrom = relation1ToN.PropertyNameFrom,
                            PropertyNameTo = relation1ToN.PropertyNameTo,
                            IsOptional = relation1ToN.IsOptional,
                            OnDelete = ParseRelationDeleteBehaviour(relation1ToN.OnDelete),
                        }).ToList(),
                        Indices = entity.Indices.Select(index => new Contractor.Core.MetaModell.Index()
                        {
                            PropertyNames = index.PropertyNames,
                            IsUnique = index.IsUnique,
                            IsClustered = index.IsClustered,
                            Where = index.Where,
                        }).ToList(),
                        Checks = entity.Checks.Select(check => new Check()
                        {
                            Name = check.Name,
                            Query = check.Query,
                        }).ToList(),
                    };
                }).ToList(),
            }).ToList();
        }

        private static string ParseRelationDeleteBehaviour(string relationDeleteBehaviour)
        {
            if (relationDeleteBehaviour == "SetNull" || relationDeleteBehaviour == "Cascade")
                return relationDeleteBehaviour;

            return "NoAction";
        }
    }
}