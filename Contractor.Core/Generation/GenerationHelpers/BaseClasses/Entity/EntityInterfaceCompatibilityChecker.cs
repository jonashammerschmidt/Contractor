using System.Linq;
using Contractor.Core.MetaModell;

namespace Contractor.Core.BaseClasses
{
    public enum EntityInterfaceCompatibility
    {
        None,
        DtoData,
        Dto,
        DtoExpanded
    }

    public class EntityInterfaceCompatibilityChecker
    {
        public EntityInterfaceCompatibility IsInterfaceCompatible(Entity entity, Interface interfaceItem)
        {
            var compatibility = GetBaseCompatible(entity, interfaceItem);
            
            foreach (var property in interfaceItem.Properties)
            {
                bool isId = property.Name == "Id";
                bool isScopeId = property.Name == entity.ScopeEntity?.Name;
                bool isProperty = entity.Properties.Any(p => p.Name == property.Name);
                if (!isId && !isScopeId && !isProperty)
                {
                    return EntityInterfaceCompatibility.None;
                }
            }
            
            foreach (var relation in interfaceItem.Relations)
            {
                bool hasSimpleRelation = HasSimpleRelation(entity, relation.EntityNameFrom, relation.PropertyNameFrom);
                if (!hasSimpleRelation)
                {
                    return EntityInterfaceCompatibility.None;
                }
            }

            return compatibility;
        }

        private bool HasSimpleRelation(Entity entity, string entityNameFrom, string propertyName)
        {
            var isRelations1To1From = entity.Relations1To1.Any(relation => 
                relation.EntityFrom.Name == entityNameFrom
                && (relation.PropertyNameFrom == null || (relation.PropertyNameFrom ?? relation.EntityTo.Name) == propertyName));
            var isRelations1To1To = entity.Relations1To1.Any(relation => 
                relation.EntityTo.Name == entityNameFrom
                && (relation.PropertyNameTo == null || (relation.PropertyNameTo ?? relation.EntityFrom.Name) == propertyName));
            var isRelations1ToNFrom = entity.Relations1ToN.Any(relation => 
                relation.EntityFrom.Name == entityNameFrom
                && (relation.PropertyNameFrom == null || (relation.PropertyNameFrom ?? relation.EntityTo.Name) == propertyName));

            return isRelations1To1From || isRelations1To1To || isRelations1ToNFrom;
        }
        
        public EntityInterfaceCompatibility GetBaseCompatible(Entity entity, Interface interfaceItem)
        {
            if (interfaceItem.Relations.Count > 0)
            {
                return EntityInterfaceCompatibility.DtoExpanded;
            }
            else if (interfaceItem.Properties.Any(p => p.Name == "Id" || p.Name == entity.ScopeEntity?.Name))
            {
                return EntityInterfaceCompatibility.Dto;
            }
            else
            {
                return EntityInterfaceCompatibility.DtoData;
            }
        }
    }
}