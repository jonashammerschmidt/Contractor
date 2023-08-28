namespace Contractor.CLI;

public class ContractorXmlValidator
{
    public static void Validate(ContractorXml contractorXml)
    {
        var entities = contractorXml.Modules.Modules.SelectMany(module => module.Entities);
        ValidateEntities(entities);
        ValidateRelations(entities);
        ValidateProperties(entities);
        ValidateIndices(entities);
    }

    private static void ValidateEntities(IEnumerable<EntityXml> entities)
    {
        var entityNames = new HashSet<string>(entities.Select(e => e.Name));
        foreach (var entity in entities)
        {
            if (!string.IsNullOrWhiteSpace(entity.ScopeEntityName) && !entityNames.Contains(entity.ScopeEntityName))
            {
                throw new FormatException($"Entity '{entity.Name}': Scope entity name '{entity.ScopeEntityName}' does not exist.");
            }
        }
    }

    private static void ValidateRelations(IEnumerable<EntityXml> entities)
    {
        var entityNames = new HashSet<string>(entities.Select(e => e.Name));
        foreach (var entity in entities)
        {
            foreach (var relation in entity.Relation1ToN)
            {
                if (!entityNames.Contains(relation.EntityNameFrom))
                {
                    throw new FormatException($"Entity '{entity.Name}': Entity name '{relation.EntityNameFrom}' in relation does not exist.");
                }
            }

            foreach (var relation in entity.Relations1To1)
            {
                if (!entityNames.Contains(relation.EntityNameFrom))
                {
                    throw new FormatException($"Entity '{entity.Name}': Entity name '{relation.EntityNameFrom}' in relation does not exist.");
                }
            }
        }
    }

    private static void ValidateProperties(IEnumerable<EntityXml> entities)
    {
        foreach (var entity in entities)
        {
            var propertyNames = GetPropertyNames(entity);
            var firstDuplicatation = propertyNames
                .GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .FirstOrDefault();

            if (firstDuplicatation != null)
            {
                throw new FormatException($"Entity '{entity.Name}': Duplicate property name '{firstDuplicatation}'.");
            }
        }
    }

    private static void ValidateIndices(IEnumerable<EntityXml> entities)
    {
        foreach (var entity in entities)
        {
            var propertyNames = GetPropertyNames(entity);
            foreach (var index in entity.Indices)
            {
                foreach (var propertyName in index.PropertyNames.Split(','))
                {
                    if (!propertyNames.Contains(propertyName))
                    {
                        throw new FormatException(
                            $"Entity '{entity.Name}': Index property '{propertyName}' does not exist. " + string.Join(',', propertyNames));
                    }   
                }
            }
        }
    }

    private static HashSet<string> GetPropertyNames(EntityXml entity)
    {
        var propertyNames = new HashSet<string>(entity.Properties.Select(p => p.Name));
        entity.Relation1ToN.ForEach(relation =>
            propertyNames.Add((relation.PropertyNameFrom ?? relation.EntityNameFrom) + "Id"));

        if (!string.IsNullOrWhiteSpace(entity.ScopeEntityName))
        {
            propertyNames.Add(entity.ScopeEntityName + "Id");
        }

        return propertyNames;
    }
    
    
}