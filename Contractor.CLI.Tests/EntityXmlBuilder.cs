namespace Contractor.CLI.Tests;

public class EntityXmlBuilder
{
    private readonly EntityXml _entity = new ();

    public EntityXmlBuilder WithName(string name)
    {
        _entity.Name = name;
        return this;
    }

    public EntityXmlBuilder WithScopeEntityName(string scopeEntityName)
    {
        _entity.ScopeEntityName = scopeEntityName;
        return this;
    }

    public EntityXmlBuilder WithProperty(string name)
    {
        _entity.Properties.Add(new PropertyXml { Name = name });
        return this;
    }

    public EntityXmlBuilder WithRelation1ToN(string entityNameFrom)
    {
        _entity.Relation1ToN.Add(new Relation1ToNXml { EntityNameFrom = entityNameFrom });
        return this;
    }

    public EntityXmlBuilder WithRelation1ToN(string entityNameFrom, string propertyNameFrom, string propertyNameTo)
    {
        _entity.Relation1ToN.Add(new Relation1ToNXml { EntityNameFrom = entityNameFrom, PropertyNameFrom = propertyNameFrom, PropertyNameTo = propertyNameTo });
        return this;
    }

    public EntityXmlBuilder WithRelation1To1(string entityNameFrom)
    {
        _entity.Relations1To1.Add(new Relation1To1Xml { EntityNameFrom = entityNameFrom });
        return this;
    }

    public EntityXmlBuilder WithRelation1To1(string entityNameFrom, string propertyNameFrom, string propertyNameTo)
    {
        _entity.Relations1To1.Add(new Relation1To1Xml { EntityNameFrom = entityNameFrom, PropertyNameFrom = propertyNameFrom, PropertyNameTo = propertyNameTo });
        return this;
    }

    public EntityXmlBuilder WithIndex(string propertyNames)
    {
        _entity.Indices.Add(new IndexXml { PropertyNames = propertyNames });
        return this;
    }

    public EntityXml Build() => _entity;
}

public class ContractorXmlBuilder
{
    private ContractorXml _contractorXml = new ContractorXml
    {
        Modules = new ModulesXml { Modules = new List<ModuleXml>() }
    };

    public ContractorXmlBuilder()
    {
        // Initialize the first module to add entities to.
        _contractorXml.Modules.Modules.Add(new ModuleXml { Entities = new List<EntityXml>() });
    }

    public ContractorXmlBuilder AddEntity(Action<EntityXmlBuilder> configure)
    {
        var entityBuilder = new EntityXmlBuilder();
        configure(entityBuilder);
        EntityXml entity = entityBuilder.Build();
        _contractorXml.Modules.Modules.First().Entities.Add(entity);
        return this;
    }

    public ContractorXml Build() => _contractorXml;
}