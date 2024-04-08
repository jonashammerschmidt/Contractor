using Contractor.Core.MetaModell;
using Index = Contractor.Core.MetaModell.Index;

namespace Contractor.CLI.Tests.Builder.MetaModell;

public class EntityBuilder
{
    private Entity _entity = new Entity
    {
        Properties = new List<Property>(),
        Relations1To1 = new List<Relation1To1>(),
        Relations1ToN = new List<Relation1ToN>(),
        Indices = new List<Index>(),
        Checks = new List<Check>()
    };

    public EntityBuilder WithName(string name, string plural)
    {
        _entity.Name = name;
        _entity.NamePlural = plural;
        return this;
    }

    public EntityBuilder WithScopeEntityName(string scopeEntityName)
    {
        _entity.ScopeEntityName = scopeEntityName;
        return this;
    }

    public EntityBuilder AddProperty(Property property)
    {
        _entity.Properties.Add(property);
        return this;
    }

    public EntityBuilder AddRelation1To1(Relation1To1 relation)
    {
        _entity.Relations1To1.Add(relation);
        relation.EntityTo = this._entity;
        return this;
    }

    public EntityBuilder AddRelation1ToN(Relation1ToN relation)
    {
        _entity.Relations1ToN.Add(relation);
        relation.EntityTo = this._entity;
        return this;
    }

    public Entity Build() => _entity;
}
