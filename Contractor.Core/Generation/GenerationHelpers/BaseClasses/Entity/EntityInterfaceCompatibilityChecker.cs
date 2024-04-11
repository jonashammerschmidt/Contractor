using System;
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
        public static EntityInterfaceCompatibility IsInterfaceCompatible(Entity entity, Interface interfaceItem)
        {
            interfaceItem = interfaceItem.ToFlatInterface();
            
            var compatibility = GetBaseCompatible(entity, interfaceItem);
            
            foreach (var interfaceProperty in interfaceItem.Properties)
            {
                bool isId = interfaceProperty.Name == "Id";
                bool isScopeId = entity.ScopeEntity is not null && interfaceProperty.Name == entity.ScopeEntity.Name + "Id";
                bool isProperty = entity.Properties.Any(p => p.Name == interfaceProperty.Name);
                bool isRelation1To1IdProperty = entity.Relations1To1
                    .Any(relation => interfaceProperty.Name == (relation.PropertyNameInSource ?? relation.TargetEntity.Name) + "Id");
                bool isRelation1ToNIdProperty = entity.Relations1ToN
                    .Any(relation => interfaceProperty.Name == (relation.PropertyNameInSource ?? relation.TargetEntity.Name) + "Id");
                if (!isId && !isScopeId && !isProperty && !isRelation1To1IdProperty && !isRelation1ToNIdProperty)
                {
                    return EntityInterfaceCompatibility.None;
                }
            }
            
            foreach (var interfaceRelation in interfaceItem.Relations)
            {
                bool hasSimpleRelation = HasSimpleRelation(entity, interfaceRelation.TargetEntityName, interfaceRelation.PropertyName);
                if (!hasSimpleRelation)
                {
                    return EntityInterfaceCompatibility.None;
                }
            }

            return compatibility;
        }

        private static bool HasSimpleRelation(Entity entity, string otherEntityName, string propertyName)
        {
            var relation = entity.Module.Options.FindRelationOrDefault(entity, propertyName ?? otherEntityName);
            
            if (relation == null)
            {
                return false;
            }

            return relation.TargetEntity != entity || relation is not Relation1ToN;
        }
        
        public static EntityInterfaceCompatibility GetBaseCompatible(Entity entity, Interface interfaceItem)
        {
            if (interfaceItem.Relations.Count > 0)
            {
                return EntityInterfaceCompatibility.DtoExpanded;
            }
            else if (interfaceItem.Properties.Any(p => p.Name == "Id" || p.Name == entity.ScopeEntity?.Name + "Id"))
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