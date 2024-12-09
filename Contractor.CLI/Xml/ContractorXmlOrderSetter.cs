using System.Xml;
using Contractor.Core.MetaModell;

namespace Contractor.CLI
{
    public static class ContractorXmlOrderSetter
    {
        public static void SetOrder(GenerationOptions generationOptions, XmlDocument xmlDocument)
        {
            foreach (var module in generationOptions.Modules)
            {
                foreach (var entity in module.Entities)
                {
                    XmlNode xmlNode = xmlDocument.SelectSingleNode($"//Entity[@name='{entity.Name}']");
                    if (xmlNode != null)
                    {
                        SetPropertiesOrder(entity, xmlNode);
                        SetRelations1To1Order(entity, xmlNode);
                        SetRelations1ToNOrder(entity, xmlNode);
                    }
                }
            }
        }

        private static void SetPropertiesOrder(Entity entity, XmlNode xmlNode)
        {
            var propertyNodes = xmlNode.SelectNodes("Property");
            if (propertyNodes != null)
            {
                foreach (XmlNode propertyNode in propertyNodes)
                {
                    var propertyName = propertyNode.Attributes?["name"]?.Value;
                    var property = entity.Properties.FirstOrDefault(p => string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase));
                    if (property != null)
                    {
                        property.Order = propertyNode.GetNodeIndex();
                    }
                }
            }
        }
        
        private static void SetRelations1To1Order(Entity entity, XmlNode xmlNode)
        {
            var relationNodes = xmlNode.SelectNodes("Relation1To1");
            if (relationNodes != null)
            {
                foreach (XmlNode relationNode in relationNodes)
                {
                    string entityNameFrom = relationNode.Attributes?["entityNameFrom"]?.Value;
                    string propertyNameFrom = relationNode.Attributes?["propertyNameFrom"]?.Value;
                    var relation = entity.Relations1To1.FirstOrDefault(r =>
                        string.Equals(r.TargetEntity.Name, entityNameFrom, StringComparison.OrdinalIgnoreCase)
                        && string.Equals(r.PropertyNameInSource, propertyNameFrom, StringComparison.OrdinalIgnoreCase));
                    if (relation != null)
                    {
                        relation.Order = relationNode.GetNodeIndex();
                    }
                }
            }
        }

        private static void SetRelations1ToNOrder(Entity entity, XmlNode xmlNode)
        {
            var relationNodes = xmlNode.SelectNodes("Relation1ToN");
            if (relationNodes != null)
            {
                foreach (XmlNode relationNode in relationNodes)
                {
                    string entityNameFrom = relationNode.Attributes?["entityNameFrom"]?.Value;
                    string propertyNameFrom = relationNode.Attributes?["propertyNameFrom"]?.Value;
                    var relation = entity.Relations1ToN.FirstOrDefault(r =>
                        string.Equals(r.TargetEntity.Name, entityNameFrom, StringComparison.OrdinalIgnoreCase)
                        && string.Equals(r.PropertyNameInSource, propertyNameFrom, StringComparison.OrdinalIgnoreCase));
                    if (relation != null)
                    {
                        relation.Order = relationNode.GetNodeIndex();
                    }
                }
            }
        }
        
        private static int GetNodeIndex(this XmlNode node)
        {
            XmlNode parentNode = node.ParentNode;
            if (parentNode != null)
            {
                int index = 0;
                foreach (XmlNode child in parentNode.ChildNodes)
                {
                    if (child == node)
                    {
                        return index;
                    }
                    index++;
                }
            }

            return -1; // Fallback, should not happen if node is part of the document
        }
    }
}
