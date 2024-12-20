using System.Text.RegularExpressions;

namespace Contractor.CLI;

public class ContractorXmlValidator
{
    private static readonly Regex NameTester = new Regex("^[a-zA-Z][a-zA-Z0-9]*$");
    
    public static void Validate(ContractorXml contractorXml)
    {
        var entities = contractorXml.Modules.Modules.SelectMany(module => module.Entities);
        ValidateModuleNames(contractorXml); // Neue Modulnamen-Validierung
        ValidateEntities(entities);
        ValidateRelations(entities);
        ValidateProperties(entities);
        ValidateIndices(entities);
        ValidateUniqueRelations(entities);
        ValidateChecks(entities);
    }

    private static void ValidateModuleNames(ContractorXml contractorXml)
    {
        var moduleNames = new HashSet<string>();
        foreach (var module in contractorXml.Modules.Modules)
        {
            if (!moduleNames.Add(module.Name))
            {
                throw new FormatException($"Duplicate module name '{module.Name}' found.");
            }

            if (!NameTester.IsMatch(module.Name))
            {
                throw new FormatException($"Module name '{module.Name}' should only contain letters and numbers, and cannot start with a number.");
            }
        }
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

            if (!NameTester.IsMatch(entity.Name))
            {
                throw new FormatException($"Entity name '{entity.Name}' should only contain letters and numbers, and cannot start with a number.");
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

                if (relation.PropertyNameFrom != null && !NameTester.IsMatch(relation.PropertyNameFrom))
                {
                    throw new FormatException($"PropertyNameFrom '{relation.PropertyNameFrom}' should only contain letters and numbers, and cannot start with a number.");
                }

                if (relation.PropertyNameTo != null && !NameTester.IsMatch(relation.PropertyNameTo))
                {
                    throw new FormatException($"PropertyNameTo '{relation.PropertyNameTo}' should only contain letters and numbers, and cannot start with a number.");
                }
            }

            foreach (var relation in entity.Relations1To1)
            {
                if (!entityNames.Contains(relation.EntityNameFrom))
                {
                    throw new FormatException($"Entity '{entity.Name}': Entity name '{relation.EntityNameFrom}' in relation does not exist.");
                }

                if (relation.PropertyNameFrom != null && !NameTester.IsMatch(relation.PropertyNameFrom))
                {
                    throw new FormatException($"PropertyNameFrom name '{relation.PropertyNameFrom}' should only contain letters and numbers, and cannot start with a number.");
                }

                if (relation.PropertyNameTo != null && !NameTester.IsMatch(relation.PropertyNameTo))
                {
                    throw new FormatException($"PropertyNameTo name '{relation.PropertyNameTo}' should only contain letters and numbers, and cannot start with a number.");
                }
            }
        }
    }

    private static void ValidateProperties(IEnumerable<EntityXml> entities)
    {
        foreach (var entity in entities)
        {
            // Use a List<string> to collect property names, allowing duplicates
            var propertyNames = entity.Properties.Select(p => p.Name).ToList();

            foreach (var propertyName in propertyNames)
            {
                if (!NameTester.IsMatch(propertyName))
                {
                    throw new FormatException($"Property name '{propertyName}' should only contain letters and numbers, and cannot start with a number.");
                }
            }
        
            // Add relation-based property names, which may include duplicates
            entity.Relation1ToN.ForEach(relation =>
                propertyNames.Add((relation.PropertyNameFrom ?? relation.EntityNameFrom) + "Id"));

            if (!string.IsNullOrWhiteSpace(entity.ScopeEntityName))
            {
                propertyNames.Add(entity.ScopeEntityName + "Id");
            }

            // Find the first duplicate property name, if any
            var firstDuplicate = propertyNames
                .GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .FirstOrDefault();

            if (firstDuplicate != null)
            {
                throw new FormatException($"Entity '{entity.Name}': Duplicate property name '{firstDuplicate}'.");
            }

            foreach (var property in entity.Properties)
            {
                var isValidType = Regex.IsMatch(property.Type, @"^(Boolean|ByteArray|DateTime|Double|Guid|Integer|String:\d+)$");
                if (!isValidType)
                {
                    throw new FormatException($"Entity '{entity.Name}': Property '{property.Name}': Invalid type '{property.Type}'");
                }

                var isTypeStringMatchResult = Regex.Match(property.Type, "^String:([0-9]+)$");
                
                if (isTypeStringMatchResult.Success)
                {
                    string maxLengthString = isTypeStringMatchResult.Groups[1].Value;
                    int maxLengthInt = int.Parse(maxLengthString);

                    if (maxLengthInt <= 0)
                    {
                        throw new FormatException($"Entity '{entity.Name}': Property '{property.Name}' of type 'String': maxLength cannot be smaller than 1.");
                    }

                    if (int.TryParse(property.MinLength, out int minLength))
                    {
                        if (minLength < 0)
                        {
                            throw new FormatException($"Entity '{entity.Name}': Property '{property.Name}' of type 'String': minLength cannot be smaller than 0.");
                        }
                        
                        if (maxLengthInt < minLength)
                        {
                            throw new FormatException($"Entity '{entity.Name}': Property '{property.Name}' of type 'String': maxLength cannot be smaller than minLength.");
                        }
                    }
                }
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
                foreach (var propertyName in index.PropertyNames.Split(',').Select(p => p.Trim()))
                {
                    if (!propertyNames.Contains(propertyName))
                    {
                        throw new FormatException(
                            $"Entity '{entity.Name}': Index property '{propertyName}' does not exist. " + string.Join(',', propertyNames));
                    }   
                }

                if (!string.IsNullOrWhiteSpace(index.IncludeNames))
                {
                    foreach (var includeName in index.IncludeNames.Split(',').Select(p => p.Trim()))
                    {
                        if (!propertyNames.Contains(includeName))
                        {
                            throw new FormatException(
                                $"Entity '{entity.Name}': Index include property '{includeName}' does not exist. " + string.Join(',', propertyNames));
                        }   
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
        entity.Relations1To1.ForEach(relation =>
            propertyNames.Add((relation.PropertyNameFrom ?? relation.EntityNameFrom) + "Id"));

        if (!string.IsNullOrWhiteSpace(entity.ScopeEntityName))
        {
            propertyNames.Add(entity.ScopeEntityName + "Id");
        }

        return propertyNames;
    }
    
    private static void ValidateUniqueRelations(IEnumerable<EntityXml> entities)
    {
        // Ein Set zur Überwachung der Einzigartigkeit der Relationen
        var relationSignatures = new HashSet<string>();

        foreach (var entity in entities)
        {
            // Überprüfung der 1:N-Relationen
            foreach (var relation in entity.Relation1ToN)
            {
                // Erstellung der Signatur für die direkte Relation
                var directSignature = $"{entity.Name}.{relation.PropertyNameFrom ?? relation.EntityNameFrom}->{relation.EntityNameFrom}";
                // Erstellung der Signatur für die umgekehrte Relation, unter Berücksichtigung der Plurals
                var reverseSignature = $"{relation.EntityNameFrom}.{relation.PropertyNameTo ?? entity.NamePlural}->{entity.Name}";

                // Überprüfung der Einzigartigkeit der Signaturen
                if (!relationSignatures.Add(directSignature))
                {
                    throw new FormatException($"Duplicate relation found: {directSignature}");
                }
                if (!relationSignatures.Add(reverseSignature))
                {
                    throw new FormatException($"Duplicate reverse relation found: {reverseSignature}");
                }
            }

            // Überprüfung der 1:1-Relationen
            foreach (var relation in entity.Relations1To1)
            {
                // Erstellung der Signatur für die direkte Relation
                var directSignature = $"{entity.Name}.{relation.PropertyNameFrom ?? relation.EntityNameFrom}->{relation.EntityNameFrom}";
                // Erstellung der Signatur für die umgekehrte Relation, unter Berücksichtigung der Plurals
                var reverseSignature = $"{relation.EntityNameFrom}.{relation.PropertyNameTo ?? entity.Name}->{entity.Name}";

                if (!relationSignatures.Add(directSignature))
                {
                    throw new FormatException($"Duplicate relation found: {directSignature}");
                }
                if (!relationSignatures.Add(reverseSignature))
                {
                    throw new FormatException($"Duplicate reverse relation found: {reverseSignature}");
                }
            }
        }
    }

    private static void ValidateChecks(IEnumerable<EntityXml> entities)
    {
        foreach (var entity in entities)
        {
            foreach (var check in entity.Checks)
            {
                if (!NameTester.IsMatch(check.Name))
                {
                    throw new FormatException($"Check name '{check.Name}' should only contain letters and numbers, and cannot start with a number.");
                }
            }
        }
    }
}