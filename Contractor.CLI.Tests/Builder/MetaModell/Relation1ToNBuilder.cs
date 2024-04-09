using Contractor.Core.MetaModell;

namespace Contractor.CLI.Tests.Builder.MetaModell;

public class Relation1ToNBuilder
{
    private Relation1ToN _relation = new Relation1ToN();

    public Relation1ToNBuilder WithEntity(string name)
    {
        _relation.TargetEntityName = name;
        return this;
    }

    public Relation1ToNBuilder WithPropertyNameFrom(string propertyName)
    {
        _relation.PropertyNameInSource = propertyName;
        return this;
    }

    public Relation1ToNBuilder WithPropertyNameTo(string propertyName)
    {
        _relation.PropertyNameInTarget = propertyName;
        return this;
    }

    public Relation1ToN Build() => _relation;
}
