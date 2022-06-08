using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Contractor.Core
{
    [XmlRoot(ElementName = "Contractor")]
    public class ContractorXml
    {
        [XmlElement(ElementName = "Paths")]
        public PathsXml Paths { get; set; }

        [XmlElement(ElementName = "Replacements")]
        public ReplacementsXml Replacements { get; set; }

        [XmlElement(ElementName = "Modules")]
        public ModulesXml Modules { get; set; }

        internal ContractorGenerationOptions ToContractorGenerationOptions(XmlDocument xmlDocument)
        {
            return new ContractorGenerationOptions()
            {
                Paths = new Paths()
                {
                    BackendDestinationFolder = this.Paths.BackendDestinationFolder,
                    DbDestinationFolder = this.Paths.DbDestinationFolder,
                    FrontendDestinationFolder = this.Paths.FrontendDestinationFolder,
                    ProjectName = this.Paths.ProjectName,
                    DbProjectName = this.Paths.DbProjectName,
                },
                Replacements = this.Replacements.Replacements.Select(replacement => new Replacement()
                {
                    Pattern = replacement.Pattern,
                    ReplaceWith = replacement.ReplaceWith,
                }),
                Modules = this.Modules.Modules.Select(module => new Module()
                {
                    Name = module.Name,
                    Entities = module.Entities.Select(entity =>
                    {
                        XmlNode xmlNode = xmlDocument.SelectSingleNode($"//Entity[@name='{entity.Name}']");
                        XmlNodeList xmlNodeList = xmlNode.ChildNodes;

                        return new Entity()
                        {
                            Name = entity.Name,
                            NamePlural = entity.NamePlural,
                            ScopeEntityName = entity.ScopeEntityName,
                            Properties = entity.Properties.Select(property => new Property()
                            {
                                Type = property.Type,
                                Name = property.Name,
                                IsOptional = property.IsOptional,
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
                                Order = xmlNodeList.Cast<XmlNode>()
                                    .Select((xmlNode, index) => new { index, xmlNode })
                                    .Where(element => element.xmlNode.Name == "Relation1To1")
                                    .Where(element => element.xmlNode.Attributes["entityNameFrom"].Value == relation1To1.EntityNameFrom)
                                    .Where(element => element.xmlNode.Attributes["propertyNameFrom"].Value == relation1To1.PropertyNameFrom)
                                    .Single()
                                    .index,
                            }).ToList(),
                            Relations1ToN = entity.Relation1ToN.Select(relation1ToN => new Relation1ToN()
                            {
                                EntityNameFrom = relation1ToN.EntityNameFrom,
                                PropertyNameFrom = relation1ToN.PropertyNameFrom,
                                PropertyNameTo = relation1ToN.PropertyNameTo,
                                IsOptional = relation1ToN.IsOptional,
                                Order = xmlNodeList.Cast<XmlNode>()
                                    .Select((xmlNode, index) => new { index, xmlNode })
                                    .Where(element => element.xmlNode.Name == "Relation1ToN")
                                    .Where(element => element.xmlNode.Attributes["entityNameFrom"].Value == relation1ToN.EntityNameFrom)
                                    .Where(element => element.xmlNode.Attributes["propertyNameFrom"].Value == relation1ToN.PropertyNameFrom)
                                    .Single()
                                    .index,
                            }).ToList(),
                            Indices = entity.Indices.Select(index => new Index()
                            {
                                PropertyNames = index.PropertyNames,
                                IsUnique = index.IsUnique,
                                IsClustered = index.IsClustered,
                            }).ToList(),
                        };
                    }).ToList(),
                }).ToList(),
            };
        }
    }

    [XmlRoot(ElementName = "Paths")]
    public class PathsXml
    {
        [XmlElement(ElementName = "BackendDestinationFolder")]
        public string BackendDestinationFolder { get; set; }

        [XmlElement(ElementName = "DbDestinationFolder")]
        public string DbDestinationFolder { get; set; }

        [XmlElement(ElementName = "FrontendDestinationFolder")]
        public string FrontendDestinationFolder { get; set; }

        [XmlElement(ElementName = "ProjectName")]
        public string ProjectName { get; set; }

        [XmlElement(ElementName = "DbProjectName")]
        public string DbProjectName { get; set; }
    }

    [XmlRoot(ElementName = "Replacements")]
    public class ReplacementsXml
    {
        [XmlElement(ElementName = "Replacement")]
        public List<ReplacementXml> Replacements { get; set; }
    }

    [XmlRoot(ElementName = "Replacement")]
    public class ReplacementXml
    {
        [XmlAttribute(AttributeName = "pattern")]
        public string Pattern { get; set; }

        [XmlAttribute(AttributeName = "ReplaceWith")]
        public string ReplaceWith { get; set; }
    }

    [XmlRoot(ElementName = "Modules")]
    public class ModulesXml
    {
        [XmlElement(ElementName = "Module")]
        public List<ModuleXml> Modules { get; set; }
    }

    [XmlRoot(ElementName = "Module")]
    public class ModuleXml
    {
        [XmlElement(ElementName = "Entity")]
        public List<EntityXml> Entities { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "Entity")]
    public class EntityXml
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "namePlural")]
        public string NamePlural { get; set; }

        [XmlAttribute(AttributeName = "scopeEntityName")]
        public string ScopeEntityName { get; set; }

        [XmlElement(ElementName = "Property")]
        public List<PropertyXml> Properties { get; set; }

        [XmlElement(ElementName = "Index")]
        public List<IndexXml> Indices { get; set; }

        [XmlElement(ElementName = "Relation1ToN")]
        public List<Relation1ToNXml> Relation1ToN { get; set; }

        [XmlElement(ElementName = "Relation1To1")]
        public List<Relation1To1Xml> Relations1To1 { get; set; }
    }

    [XmlRoot(ElementName = "Property")]
    public class PropertyXml
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "optional")]
        public bool IsOptional { get; set; }
    }

    [XmlRoot(ElementName = "Index")]
    public class IndexXml
    {
        [XmlAttribute(AttributeName = "property")]
        public string PropertyNames { get; set; }

        [XmlAttribute(AttributeName = "clustered")]
        public bool IsClustered { get; set; }

        [XmlAttribute(AttributeName = "unique")]
        public bool IsUnique { get; set; }
    }

    [XmlRoot(ElementName = "Relation1ToN")]
    public class Relation1ToNXml
    {
        [XmlAttribute(AttributeName = "entityNameFrom")]
        public string EntityNameFrom { get; set; }

        [XmlAttribute(AttributeName = "propertyNameFrom")]
        public string PropertyNameFrom { get; set; }

        [XmlAttribute(AttributeName = "PropertyNameTo")]
        public string PropertyNameTo { get; set; }

        [XmlAttribute(AttributeName = "optional")]
        public bool IsOptional { get; set; }
    }

    [XmlRoot(ElementName = "Relation1To1")]
    public class Relation1To1Xml
    {
        [XmlAttribute(AttributeName = "entityNameFrom")]
        public string EntityNameFrom { get; set; }

        [XmlAttribute(AttributeName = "propertyNameFrom")]
        public string PropertyNameFrom { get; set; }

        [XmlAttribute(AttributeName = "PropertyNameTo")]
        public string PropertyNameTo { get; set; }

        [XmlAttribute(AttributeName = "optional")]
        public bool IsOptional { get; set; }
    }
}