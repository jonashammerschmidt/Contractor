﻿using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace Contractor.CLI
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

        [XmlElement(ElementName = "Includes")]
        public IncludesXml Includes { get; set; }

        [XmlAttribute(AttributeName = "minContractorVersion")]
        public string MinContractorVersion { get; set; }
    }

    [XmlRoot(ElementName = "Include")]
    public class ContractorIncludeXml
    {
        [XmlElement(ElementName = "Modules")]
        public ModulesXml Modules { get; set; }
    }

    [XmlRoot(ElementName = "Paths")]
    public class PathsXml
    {
        [XmlElement(ElementName = "BackendDestinationFolder")]
        public string BackendDestinationFolder { get; set; }

        [XmlElement(ElementName = "BackendGeneratedDestinationFolder")]
        public string BackendGeneratedDestinationFolder { get; set; }

        [XmlElement(ElementName = "DbDestinationFolder")]
        public string DbDestinationFolder { get; set; }

        [XmlElement(ElementName = "FrontendDestinationFolder")]
        public string FrontendDestinationFolder { get; set; }

        [XmlElement(ElementName = "ProjectName")]
        public string ProjectName { get; set; }

        [XmlElement(ElementName = "GeneratedProjectName")]
        public string GeneratedProjectName { get; set; }

        [XmlElement(ElementName = "DbProjectName")]
        public string DbProjectName { get; set; }

        [XmlElement(ElementName = "DbContextName")]
        public string DbContextName { get; set; }
    }

    [XmlRoot(ElementName = "Replacements")]
    public class ReplacementsXml
    {
        [XmlElement(ElementName = "Replacement")]
        public List<ReplacementXml> Replacements { get; set; } = new ();
    }

    [XmlRoot(ElementName = "Replacement")]
    public class ReplacementXml
    {
        [XmlAttribute(AttributeName = "pattern")]
        public string Pattern { get; set; }

        [XmlAttribute(AttributeName = "replaceWith")]
        public string ReplaceWith { get; set; }
    }

    [XmlRoot(ElementName = "Includes")]
    public class IncludesXml
    {
        [XmlElement(ElementName = "Include")]
        public List<IncludeXml> Includes { get; set; } = new ();
    }

    [XmlRoot(ElementName = "Include")]
    public class IncludeXml
    {
        [XmlAttribute(AttributeName = "src")]
        public string Src { get; set; }
    }

    [XmlRoot(ElementName = "Modules")]
    public class ModulesXml
    {
        [XmlElement(ElementName = "Module")]
        public List<ModuleXml> Modules { get; set; } = new ();
    }

    [XmlRoot(ElementName = "Module")]
    public class ModuleXml
    {
        [XmlElement(ElementName = "Entity")]
        public List<EntityXml> Entities { get; set; } = new ();

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "skip")]
        public bool Skip { get; set; }
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

        [XmlAttribute(AttributeName = "idType")]
        public string IdType { get; set; }

        [XmlAttribute(AttributeName = "skip")]
        public bool Skip { get; set; }

        [XmlElement(ElementName = "Property")]
        public List<PropertyXml> Properties { get; set; } = new ();

        [XmlElement(ElementName = "Relation1ToN")]
        public List<Relation1ToNXml> Relation1ToN { get; set; } = new ();

        [XmlElement(ElementName = "Relation1To1")]
        public List<Relation1To1Xml> Relations1To1 { get; set; } = new ();

        [XmlElement(ElementName = "Index")]
        public List<IndexXml> Indices { get; set; } = new ();

        [XmlElement(ElementName = "Check")]
        public List<CheckXml> Checks { get; set; } = new ();
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

        [XmlAttribute(AttributeName = "displayProperty")]
        public bool IsDisplayProperty { get; set; }

        [XmlAttribute(AttributeName = "hidden")]
        public bool IsHidden { get; set; }
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

        [XmlAttribute(AttributeName = "where")]
        public string Where { get; set; }
    }

    [XmlRoot(ElementName = "Check")]
    public class CheckXml
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "query")]
        public string Query { get; set; }
    }

    [XmlRoot(ElementName = "Relation1ToN")]
    public class Relation1ToNXml
    {
        [XmlAttribute(AttributeName = "entityNameFrom")]
        public string EntityNameFrom { get; set; }

        [XmlAttribute(AttributeName = "propertyNameFrom")]
        public string PropertyNameFrom { get; set; }

        [XmlAttribute(AttributeName = "propertyNameTo")]
        public string PropertyNameTo { get; set; }

        [XmlAttribute(AttributeName = "optional")]
        public bool IsOptional { get; set; }

        [XmlAttribute(AttributeName = "onDelete")]
        public string OnDelete { get; set; }
    }

    [XmlRoot(ElementName = "Relation1To1")]
    public class Relation1To1Xml
    {
        [XmlAttribute(AttributeName = "entityNameFrom")]
        public string EntityNameFrom { get; set; }

        [XmlAttribute(AttributeName = "propertyNameFrom")]
        public string PropertyNameFrom { get; set; }

        [XmlAttribute(AttributeName = "propertyNameTo")]
        public string PropertyNameTo { get; set; }

        [XmlAttribute(AttributeName = "optional")]
        public bool IsOptional { get; set; }

        [XmlAttribute(AttributeName = "onDelete")]
        public string OnDelete { get; set; }
    }
}