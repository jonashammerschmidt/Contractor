using Contractor.Core.Options;
using System;
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

        public void SaveAll()
        {
            foreach (var fileCacheItem in this.fileWriteCache)
            {
                string dirPath = Path.GetDirectoryName(fileCacheItem.Key);
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                File.WriteAllText(fileCacheItem.Key, fileCacheItem.Value);
            }
        }
    }
}