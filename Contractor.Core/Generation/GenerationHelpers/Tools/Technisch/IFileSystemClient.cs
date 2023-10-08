using Contractor.Core.MetaModell;

namespace Contractor.Core.Tools
{
    internal interface IFileSystemClient
    {
        string ReadAllText(GenerationOptions options, params string[] pathParts);

        string ReadAllText(Module module, params string[] pathParts);

        string ReadAllText(Entity entity, params string[] pathParts);

        string ReadAllText(Property property, params string[] pathParts);

        void WriteAllText(string contents, params string[] pathParts);

        void SaveAll(GenerationOptions generationOptions);
    }
}