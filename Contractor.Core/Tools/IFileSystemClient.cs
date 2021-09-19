namespace Contractor.Core.Tools
{
    internal interface IFileSystemClient
    {
        string ReadAllText(string path);

        void WriteAllText(string path, string contents);
    }
}