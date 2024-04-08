using Contractor.Core.MetaModell;

namespace Contractor.CLI.Tests.Builder.MetaModell;

public class PropertyBuilder
{
    private Property _property = new Property();

    public PropertyBuilder WithName(string name)
    {
        _property.Name = name;
        return this;
    }

    public PropertyBuilder WithType(string type)
    {
        _property.Type = type;
        return this;
    }

    public PropertyBuilder IsOptional(bool isOptional)
    {
        _property.IsOptional = isOptional;
        return this;
    }

    public PropertyBuilder IsDisplayProperty(bool isDisplayProperty)
    {
        _property.IsDisplayProperty = isDisplayProperty;
        return this;
    }

    public Property Build() => _property;
}
