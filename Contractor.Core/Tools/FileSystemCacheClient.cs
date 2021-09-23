﻿using System.Collections.Generic;
using System.IO;

namespace Contractor.Core.Tools
{
    internal class FileSystemCacheClient : IFileSystemClient
    {
        public Dictionary<string, string> fileCache = new Dictionary<string, string>();

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
            if (path.EndsWith(".cs"))
            {
                fileContent = UsingStatements.Sort(fileContent);
            }

            if (!this.fileCache.ContainsKey(path))
            {
                fileCache.Add(path, fileContent);
            }
            else
            {
                fileCache[path] = fileContent;
            }
        }

        public void SaveAll()
        {
            foreach (var fileCacheItem in this.fileCache)
            {
                string dirPath = Path.GetDirectoryName(fileCacheItem.Key);
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                File.WriteAllText(fileCacheItem.Key, fileCacheItem.Value);

                // string filename = fileCacheItem.Key
                //     .Split(new[] { "/", "\\" }, StringSplitOptions.None)
                //     .Last();

                // System.Console.WriteLine($"Written to {filename}");
            }
        }
    }
}