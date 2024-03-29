﻿using Contractor.Core.MetaModell;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Contractor.CLI
{
    internal class ContractorXmlConverter
    {
        internal static GenerationOptions ToContractorGenerationOptions(
            ContractorXml contractorXml,
            XmlDocument xmlDocument,
            string contractorXmlFolderPath)
        {
            var contractorGenerationOptions = new GenerationOptions()
            {
                Paths = new Paths()
                {
                    BackendDestinationFolder = Path.GetFullPath(Path.Combine(contractorXmlFolderPath, FixPath(contractorXml.Paths.BackendDestinationFolder))),
                    BackendGeneratedDestinationFolder = Path.GetFullPath(Path.Combine(contractorXmlFolderPath, FixPath(contractorXml.Paths.BackendGeneratedDestinationFolder))),
                    DbDestinationFolder = Path.GetFullPath(Path.Combine(contractorXmlFolderPath, FixPath(contractorXml.Paths.DbDestinationFolder))),
                    FrontendDestinationFolder = Path.GetFullPath(Path.Combine(contractorXmlFolderPath, FixPath(contractorXml.Paths.FrontendDestinationFolder))),
                    ProjectName = contractorXml.Paths.ProjectName,
                    GeneratedProjectName = contractorXml.Paths.GeneratedProjectName,
                    DbProjectName = contractorXml.Paths.DbProjectName,
                    DbContextName = contractorXml.Paths.DbContextName,
                },
                Replacements = contractorXml.Replacements.Replacements.Select(replacement => new Replacement()
                {
                    Pattern = replacement.Pattern,
                    ReplaceWith = replacement.ReplaceWith,
                }),
                Modules = ConvertModules(
                    contractorXml.Modules,
                    entityName => xmlDocument.SelectSingleNode($"//Entity[@name='{entityName}']")),
            };

            return contractorGenerationOptions;
        }

        private static string FixPath(string path)
        {
            return Path.Combine(path.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries));
        }

        internal static void AddToContractorGenerationOptions(
            GenerationOptions generationOptions,
            ContractorIncludeXml contractorIncludeXml,
            XmlDocument contractorIncludeXmlDocument)
        {
            List<Module> modules = ConvertModules(
                    contractorIncludeXml.Modules,
                    entityName => contractorIncludeXmlDocument.SelectSingleNode($"//Entity[@name='{entityName}']"));

            generationOptions.Modules = generationOptions.Modules
                .Concat(modules);
        }

        private static List<Module> ConvertModules(ModulesXml modules, System.Func<string, XmlNode> entityNodeProvider)
        {
            return modules.Modules.Select(module => new Module()
            {
                Name = module.Name,
                Skip = module.Skip,
                Entities = module.Entities.Select(entity =>
                {
                    XmlNode xmlNode = entityNodeProvider.Invoke(entity.Name);
                    XmlNodeList xmlNodeList = xmlNode.ChildNodes;

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
                            Order = xmlNodeList.Cast<XmlNode>()
                                .Select((xmlNode, index) => new { index, xmlNode })
                                .Where(element => element.xmlNode.Name == "Property")
                                .Where(element => element.xmlNode.Attributes["name"].Value == property.Name)
                                .Single()
                                .index,
                        }).ToList(),
                        Relations1To1 = entity.Relations1To1.Select(relation1To1 => new Relation1To1()
                        {
                            EntityNameFrom = relation1To1.EntityNameFrom,
                            PropertyNameFrom = relation1To1.PropertyNameFrom,
                            PropertyNameTo = relation1To1.PropertyNameTo,
                            IsOptional = relation1To1.IsOptional,
                            OnDelete = ParseRelationDeleteBehaviour(relation1To1.OnDelete),
                            Order = xmlNodeList.Cast<XmlNode>()
                                .Select((xmlNode, index) => new { index, xmlNode })
                                .Where(element => element.xmlNode.Name == "Relation1To1")
                                .Where(element => element.xmlNode.Attributes["entityNameFrom"].Value == relation1To1.EntityNameFrom)
                                .Where(element => element.xmlNode.Attributes["propertyNameFrom"]?.Value == relation1To1.PropertyNameFrom)
                                .Single()
                                .index,
                        }).ToList(),
                        Relations1ToN = entity.Relation1ToN.Select(relation1ToN => new Relation1ToN()
                        {
                            EntityNameFrom = relation1ToN.EntityNameFrom,
                            PropertyNameFrom = relation1ToN.PropertyNameFrom,
                            PropertyNameTo = relation1ToN.PropertyNameTo,
                            IsOptional = relation1ToN.IsOptional,
                            OnDelete = ParseRelationDeleteBehaviour(relation1ToN.OnDelete),
                            Order = xmlNodeList.Cast<XmlNode>()
                                .Select((xmlNode, index) => new { index, xmlNode })
                                .Where(element => element.xmlNode.Name == "Relation1ToN")
                                .Where(element => element.xmlNode.Attributes["entityNameFrom"].Value == relation1ToN.EntityNameFrom)
                                .Where(element => element.xmlNode.Attributes["propertyNameFrom"]?.Value == relation1ToN.PropertyNameFrom)
                                .Single()
                                .index,
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