using Contractor.Core.Options;
using System.Collections.Generic;
using System.IO;

namespace Contractor.Core.Tools
{
    internal class FileSystemCacheClient : IFileSystemClient
    {
        public Dictionary<string, string> fileCache = new Dictionary<string, string>();

        public Dictionary<string, string> fileWriteCache = new Dictionary<string, string>();

        public string ReadAllText(string path)
        {
            if (!this.fileCache.ContainsKey(path))
            {
                string fileContent = File.ReadAllText(path);
                fileCache.Add(path, fileContent);
            }

            return fileCache[path];
        }

        public void WriteAllText(string path, string fileContent)
        {
            if (!this.fileCache.ContainsKey(path))
            {
                fileCache.Add(path, fileContent);
                fileWriteCache.Add(path, fileContent);
            }
            else
            {
                fileCache[path] = fileContent;
                fileWriteCache[path] = fileContent;
            }
        }

        public void SaveAll(IContractorOptions contractorOptions)
        {
            foreach (var fileCacheItem in this.fileWriteCache)
            {
                var filePath = fileCacheItem.Key;
                var fileContent = fileCacheItem.Value;
                foreach (var replacement in contractorOptions.Replacements)
                {
                    fileContent = fileContent.Replace(replacement.Key, replacement.Value);
                }

                if (filePath.EndsWith(".cs"))
                {
                    fileContent = UsingStatements.Sort(fileContent);
                }
                else if (filePath.EndsWith(".ts"))
                {
                    fileContent = ImportStatements.Order(fileContent);
                }

                string dirPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                File.WriteAllText(filePath, fileContent);
            }
        }
    }
}