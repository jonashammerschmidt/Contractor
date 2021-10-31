using Contractor.Core.Options;
using System.IO;

namespace Contractor.Core.Tools
{
    internal class FileSystemClient : IFileSystemClient
    {
        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        public void WriteAllText(string path, string fileContent, IContractorOptions contractorOptions)
        {
            foreach (var replacement in contractorOptions.Replacements)
            {
                fileContent = fileContent.Replace(replacement.Key, replacement.Value);
            }

            if (path.EndsWith(".cs"))
            {
                fileContent = UsingStatements.Sort(fileContent);
            }
            else if (path.EndsWith(".ts"))
            {
                fileContent = ImportStatements.Order(fileContent);
            }

            string dirPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            File.WriteAllText(path, fileContent);
        }

        public void SaveAll()
        {
        }
    }
}