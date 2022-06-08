using Contractor.Core.Options;

namespace Contractor.Core.Tools
{
    internal interface IFileSystemClient
    {
        string ReadAllText(string path);

        void WriteAllText(string path, string contents);

        void SaveAll(ContractorGenerationOptions contractorGenerationOptions);
    }
}