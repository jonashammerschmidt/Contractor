using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contractor.Core.MetaModell;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    public static class CustomDtoIncludeHelper
    {
        public static string GetIncludeString(CustomDto customDto)
        {
            var customDtoEntityTree = GetEntityTree(customDto);
            var includePaths = new List<List<KeyValuePair<string, string>>>();
            GenerateIncludePaths(customDtoEntityTree, new List<KeyValuePair<string, string>>(), includePaths);
            var result = ConvertIncludePathsToIncludeString(includePaths);

            return result.Remove(result.Length - 1, 1);
        }

        private static List<ChildableEntry<KeyValuePair<string, string>>> GetEntityTree(CustomDto customDto)
        {
            var entityTreeRoot = new List<ChildableEntry<KeyValuePair<string, string>>>() { new(new(null, null)) };

            foreach (var property in customDto.Properties)
            {
                var currentEntityTreeNode = entityTreeRoot.First();
                foreach (var pathItems in property.PathItems)
                {
                    var entityTreeNode = currentEntityTreeNode.Children.SingleOrDefault(p =>
                        p.Entry.Key == pathItems.Entity.Name &&
                        p.Entry.Value == pathItems.PropertyName);

                    if (entityTreeNode == null)
                    {
                        entityTreeNode = new ChildableEntry<KeyValuePair<string, string>>(new(pathItems.Entity.Name, pathItems.PropertyName));
                        currentEntityTreeNode.Children.Add(entityTreeNode);
                    }

                    currentEntityTreeNode = entityTreeNode;
                }
            }

            return entityTreeRoot.First().Children;
        }
        private static void GenerateIncludePaths(List<ChildableEntry<KeyValuePair<string, string>>> tree, List<KeyValuePair<string, string>> currentPath, List<List<KeyValuePair<string, string>>> paths)
        {
            foreach (var node in tree)
            {
                var newPath = new List<KeyValuePair<string, string>>(currentPath)
                {
                    node.Entry
                };

                if (node.Children.Any())
                {
                    GenerateIncludePaths(node.Children, newPath, paths);
                }
                else
                {
                    paths.Add(newPath);
                }
            }
        }

        private static string ConvertIncludePathsToIncludeString(List<List<KeyValuePair<string, string>>> paths)
        {
            var includeString = new StringBuilder();

            foreach (var path in paths)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    var pad = new string(' ', i * 4);

                    if (i == 0)
                    {
                        includeString.Append($"{pad}.Include(ef{path[i].Key} => ef{path[i].Key}.{path[i].Value})\n");
                    }
                    else
                    {
                        includeString.Append($"{pad}.ThenInclude(ef{path[i].Key} => ef{path[i].Key}.{path[i].Value})\n");
                    }
                }
            }

            return includeString.ToString();
        }

        private class ChildableEntry<T>
        {
            public T Entry { get; set; }

            public List<ChildableEntry<T>> Children { get; set; } = new();

            public ChildableEntry(T entry)
            {
                this.Entry = entry;
            }
        }
    }
}