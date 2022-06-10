namespace Contractor.Core.Tools
{
    internal interface IFileSystemClient
    {
        string ReadAllText(ContractorGenerationOptions options, string path);

        string ReadAllText(Module module, string path);

        string ReadAllText(Entity entity, string path);

        string ReadAllText(Property property, string path);

        void WriteAllText(string path, string contents);

        void SaveAll(ContractorGenerationOptions contractorGenerationOptions);
    }
}