using Contractor.Core.Generation;
using Contractor.Core.MetaModell;

namespace Contractor.CLI.Tests.Builder.MetaModell;

public class GenerationOptionsBuilder
{
    private GenerationOptions _options = new GenerationOptions
    {
        Paths = new Paths(),
        Replacements = new List<Replacement>(),
        Modules = new List<Module>(),
        PurposeDtos = new List<PurposeDto>(),
        Interfaces = new List<Interface>(),
        Tags = new List<ClassGenerationTag>()
    };

    public GenerationOptionsBuilder AddModule(Module module)
    {
        _options.Modules.Add(module);
        return this;
    }

    public GenerationOptionsBuilder AddPurposeDto(PurposeDto purposeDto)
    {
        _options.PurposeDtos.Add(purposeDto);
        return this;
    }

    public GenerationOptionsBuilder AddInterface(Interface interfaceItem)
    {
        _options.Interfaces.Add(interfaceItem);
        return this;
    }

    public GenerationOptions Build() => _options;
}
