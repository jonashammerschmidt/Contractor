using System;
using System.Collections.Generic;
using System.Linq;
using Contractor.Core.MetaModell;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    public static class CustomDtoPathHelper
    {
        public static HashSet<Entity> FindEntitiesWithMultiplePathsAndIncludes(CustomDto customDto)
        {
            var result = new HashSet<Entity>();
            var entities = customDto.Properties
                .SelectMany(property => property.PathItems)
                .Select(pathItem => pathItem.OtherEntity)
                .ToHashSet();

            var customDtoEntityTree = GetEntityTree(customDto);
            SortEntityTree(customDtoEntityTree);

            foreach (var entity in entities)
            {
                var treesOfEntity = GetTreesOfEntity(entity.Name, customDtoEntityTree);
                if (treesOfEntity.Count() > 1)
                {
                    var countOfDistinctTrees = treesOfEntity
                        .Select(tree => StringifyEntityTree(tree.Children))
                        .Distinct()
                        .Count();

                    if (countOfDistinctTrees > 1)
                    {
                        result.Add(entity);
                    }
                }
            }

            return result;
        }

        private static List<ChildableEntry<string>> GetEntityTree(CustomDto customDto)
        {
            var entityTreeRoot = new List<ChildableEntry<string>>() { new("root") };

            foreach (var property in customDto.Properties)
            {
                var currentEntityTreeNode = entityTreeRoot.First();
                foreach (var pathItems in property.PathItems)
                {
                    var entityTreeNodeName = pathItems.OtherEntity.Name;
                    var entityTreeNode = currentEntityTreeNode.Children.SingleOrDefault(p => p.Entry == entityTreeNodeName);
                    ;
                    if (entityTreeNode == null)
                    {
                        entityTreeNode = new ChildableEntry<string>(entityTreeNodeName);
                        currentEntityTreeNode.Children.Add(entityTreeNode);
                    }

                    currentEntityTreeNode = entityTreeNode;
                }
            }

            return entityTreeRoot.First().Children;
        }

        private static ChildableEntry<string> FindEntityInPath(string entityName, List<ChildableEntry<string>> customDtoEntityTree)
        {
            ChildableEntry<string> entity = customDtoEntityTree.SingleOrDefault(item => item.Entry == entityName);
            if (entity != null)
            {
                return entity;
            }

            foreach (var customDtoEntityTreeItem in customDtoEntityTree)
            {
                entity = FindEntityInPath(entityName, customDtoEntityTreeItem.Children);
                if (entity != null)
                {
                    return entity;
                }
            }

            return null;
        }

        private static List<ChildableEntry<string>> GetTreesOfEntity(string entityName, List<ChildableEntry<string>> customDtoEntityTree)
        {
            var result = new List<ChildableEntry<string>>();
            foreach (var customDtoEntityTreeItem in customDtoEntityTree)
            {
                result.AddRange(GetTreesOfEntity(entityName, customDtoEntityTreeItem.Children));
            }

            var customDtoEntityItem = customDtoEntityTree.SingleOrDefault(item => item.Entry == entityName);
            if (customDtoEntityItem != null && customDtoEntityItem.Children.Count() > 0)
            {
                result.Add(customDtoEntityItem);
            }

            return result;
        }

        private static void SortEntityTree(List<ChildableEntry<string>> customDtoEntityTree)
        {
            customDtoEntityTree.Sort((item1, item2) => String.Compare(item1.Entry, item2.Entry, StringComparison.Ordinal));
            foreach (var treeItem in customDtoEntityTree)
            {
                SortEntityTree(treeItem.Children);
            }
        }

        private static string StringifyEntityTree(List<ChildableEntry<string>> customDtoEntityTree)
        {
            string currentString = "";
            foreach (var treeItem in customDtoEntityTree)
            {
                currentString += "->" + treeItem.Entry;
                currentString += "(" + StringifyEntityTree(treeItem.Children) + ")";
            }

            return currentString;
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