using Contractor.Core.MetaModell;

namespace Contractor.CLI.Tests.Builder.MetaModell;

public class ModuleBuilder
{
    private Module _module = new Module
    {
        Entities = new List<Entity>()
    };

    public ModuleBuilder WithName(string name)
    {
        _module.Name = name;
        return this;
    }

    public ModuleBuilder AddEntity(Entity entity)
    {
        _module.Entities.Add(entity);
        return this;
    }

    public ModuleBuilder Skip(bool skip)
    {
        _module.Skip = skip;
        return this;
    }

    public Module Build() => _module;
}
