using Contractor.Core.MetaModell;

namespace Contractor.CLI.Tests.Builder.MetaModell;

public class Relation1ToNBuilder
{
    private Relation1ToN _relation = new Relation1ToN();

    public Relation1ToNBuilder WithEntity(string name)
    {
        _relation.EntityNameFrom = name;
        return this;
    }

    public Relation1ToNBuilder WithPropertyNameFrom(string propertyName)
    {
        _relation.PropertyNameFrom = propertyName;
        return this;
    }

    public Relation1ToNBuilder WithPropertyNameTo(string propertyName)
    {
        _relation.PropertyNameTo = propertyName;
        return this;
    }

    public Relation1ToN Build() => _relation;
}
