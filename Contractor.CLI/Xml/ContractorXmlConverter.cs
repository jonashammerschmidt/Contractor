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
            contractorGenerationOptions.PurposeDtos = ConvertPurposeDtos(contractorXml.PurposeDtos);
            contractorGenerationOptions.Interfaces = ConvertInterfaces(contractorXml.Interfaces);

            return contractorGenerationOptions;
        }

        private static string FixPath(string path)
        {
            return Path.Combine(path.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries));
        }

        public static void AddToContractorGenerationOptions(
            GenerationOptions generationOptions,
            ContractorIncludeXml contractorIncludeXml)
        {
            generationOptions.Modules = generationOptions.Modules
                .Concat(ConvertModules(contractorIncludeXml.Modules))
                .ToList();

            generationOptions.PurposeDtos = generationOptions.PurposeDtos
                .Concat(ConvertPurposeDtos(contractorIncludeXml.PurposeDtos))
                .ToList();

            generationOptions.Interfaces = generationOptions.Interfaces
                .Concat(ConvertInterfaces(contractorIncludeXml.Interfaces))
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
                            MinLength = string.IsNullOrWhiteSpace(property.MinLength) ? 1 : int.Parse(property.MinLength),
                        }).ToList(),
                        Relations1To1 = entity.Relations1To1.Select(relation1To1 => new Relation1To1()
                        {
                            TargetEntityName = relation1To1.EntityNameFrom,
                            PropertyNameInSource = relation1To1.PropertyNameFrom,
                            PropertyNameInTarget = relation1To1.PropertyNameTo,
                            IsOptional = relation1To1.IsOptional,
                            OnDelete = ParseRelationDeleteBehaviour(relation1To1.OnDelete),
                        }).ToList(),
                        Relations1ToN = entity.Relation1ToN.Select(relation1ToN => new Relation1ToN()
                        {
                            TargetEntityName = relation1ToN.EntityNameFrom,
                            PropertyNameInSource = relation1ToN.PropertyNameFrom,
                            PropertyNameInTarget = relation1ToN.PropertyNameTo,
                            IsOptional = relation1ToN.IsOptional,
                            OnDelete = ParseRelationDeleteBehaviour(relation1ToN.OnDelete),
                        }).ToList(),
                        Indices = entity.Indices.Select(index => new Contractor.Core.MetaModell.Index()
                        {
                            PropertyNames = index.PropertyNames,
                            Includes = index.IncludeNames,
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

        private static List<PurposeDto> ConvertPurposeDtos(PurposeDtosXml purposeDtos)
        {
            return purposeDtos.PurposeDtos
                .Select(purposeDto =>
                {
                    var dto = new PurposeDto();
                    dto.EntityName = purposeDto.Entity;
                    dto.Purpose = purposeDto.Purpose;
                    dto.Properties = purposeDto.Properties
                        .Select(property =>
                        {
                            var dtoProperty = new PurposeDtoProperty();
                            dtoProperty.Path = property.Path;
                            return dtoProperty;
                        })
                        .ToList();
                    return dto;
                })
                .ToList();
        }

        private static List<Interface> ConvertInterfaces(InterfacesXml purposeDtos)
        {
            return purposeDtos.Interfaces
                .Select(interfaceItem =>
                {
                    var dto = new Interface();
                    dto.Name = interfaceItem.Name;
                    dto.Extends = interfaceItem.Extends;
                    dto.Properties = interfaceItem.Properties
                        .Select(property =>
                        {
                            var interfaceProperty = new InterfaceProperty();
                            interfaceProperty.Name = property.Name;
                            return interfaceProperty;
                        })
                        .ToList();
                    dto.Relations = interfaceItem.Relations
                        .Select(relation =>
                        {
                            var interfaceRelation = new InterfaceRelation();
                            interfaceRelation.TargetEntityName = relation.EntityNameFrom;
                            interfaceRelation.PropertyName = relation.PropertyNameFrom;
                            return interfaceRelation;
                        })
                        .ToList();
                    return dto;
                })
                .ToList();
        }

        private static string ParseRelationDeleteBehaviour(string relationDeleteBehaviour)
        {
            if (relationDeleteBehaviour == "SetNull" || relationDeleteBehaviour == "Cascade")
                return relationDeleteBehaviour;

            return "NoAction";
        }
    }
}