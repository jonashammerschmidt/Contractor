using System.Collections.Generic;
using System.IO;

namespace Contractor.Core.Tools
{
    internal class FileSystemCacheClient : IFileSystemClient
    {
        public Dictionary<string, string> fileCache = new Dictionary<string, string>();

        public Dictionary<string, string> fileWriteCache = new Dictionary<string, string>();

        public string ReadAllText(ContractorGenerationOptions options, string path)
        {
            string fileContent;
            if (!this.fileCache.ContainsKey(path))
            {
                fileContent = File.ReadAllText(path);
                fileCache.Add(path, fileContent);

                fileContent = ModellNameReplacements.ReplaceOptionsPlaceholders(options, fileContent);

                if (!path.Contains("Templates") && !path.EndsWith(".txt"))
                {
                    fileCache[path] = fileContent;
                }
            }
            else
            {
                fileContent = fileCache[path];

                if (path.Contains("Templates") && path.EndsWith(".txt"))
                {
                    fileContent = ModellNameReplacements.ReplaceOptionsPlaceholders(options, fileContent);
                }
            }

            return fileContent;
        }

        public string ReadAllText(Module module, string path)
        {
            string fileContent;
            if (!this.fileCache.ContainsKey(path))
            {
                fileContent = File.ReadAllText(path);
                fileCache.Add(path, fileContent);

                fileContent = ModellNameReplacements.ReplaceOptionsPlaceholders(module.Options, fileContent);
                fileContent = ModellNameReplacements.ReplaceModulePlaceholders(module, fileContent);

                if (!path.Contains("Templates") && !path.EndsWith(".txt"))
                {
                    fileCache[path] = fileContent;
                }
            }
            else
            {
                fileContent = fileCache[path];

                if (path.Contains("Templates") && path.EndsWith(".txt"))
                {
                    fileContent = ModellNameReplacements.ReplaceOptionsPlaceholders(module.Options, fileContent);
                    fileContent = ModellNameReplacements.ReplaceModulePlaceholders(module, fileContent);
                }
            }

            return fileContent;
        }

        public string ReadAllText(Entity entity, string path)
        {
            string fileContent;
            if (!this.fileCache.ContainsKey(path))
            {
                fileContent = File.ReadAllText(path);
                fileCache.Add(path, fileContent);

                fileContent = ModellNameReplacements.ReplaceOptionsPlaceholders(entity.Module.Options, fileContent);
                fileContent = ModellNameReplacements.ReplaceModulePlaceholders(entity.Module, fileContent);
                fileContent = ModellNameReplacements.ReplaceEntityPlaceholders(entity, fileContent);

                if (!path.Contains("Templates") && !path.EndsWith(".txt"))
                {
                    fileCache[path] = fileContent;
                }
            }
            else
            {
                fileContent = fileCache[path];

                if (path.Contains("Templates") && path.EndsWith(".txt"))
                {
                    fileContent = ModellNameReplacements.ReplaceOptionsPlaceholders(entity.Module.Options, fileContent);
                    fileContent = ModellNameReplacements.ReplaceModulePlaceholders(entity.Module, fileContent);
                    fileContent = ModellNameReplacements.ReplaceEntityPlaceholders(entity, fileContent);
                }
            }

            return fileContent;
        }

        public string ReadAllText(Property property, string path)
        {
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

        public void SaveAll(ContractorGenerationOptions contractorGenerationOptions)
        {
            foreach (var fileCacheItem in this.fileWriteCache)
            {
                var filePath = fileCacheItem.Key;
                var fileContent = fileCacheItem.Value;
                foreach (var replacement in contractorGenerationOptions.Replacements)
                {
                    fileContent = fileContent.Replace(replacement.Pattern, replacement.ReplaceWith);
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