using Contractor.Core.MetaModell;

namespace Contractor.CLI.Tests.Builder.MetaModell;

public class Relation1To1Builder
{
    private Relation1To1 _relation = new Relation1To1();

    public Relation1To1Builder WithEntity(string name)
    {
        _relation.TargetEntityName = name;
        return this;
    }

    public Relation1To1Builder WithPropertyNameFrom(string propertyName)
    {
        _relation.PropertyNameInSource = propertyName;
        return this;
    }

    public Relation1To1Builder WithPropertyNameTo(string propertyName)
    {
        _relation.PropertyNameInTarget = propertyName;
        return this;
    }

    public Relation1To1 Build() => _relation;
}
