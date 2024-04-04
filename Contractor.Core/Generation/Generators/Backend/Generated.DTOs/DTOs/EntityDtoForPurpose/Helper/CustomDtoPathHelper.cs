using System;
using System.Collections.Generic;
using System.Linq;
using Contractor.Core.MetaModell;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    public static class PurposeDtoPathHelper
    {
        public static HashSet<Entity> FindEntitiesWithMultiplePathsAndIncludes(PurposeDto purposeDto)
        {
            var result = new HashSet<Entity>();
            var entities = purposeDto.Properties
                .SelectMany(property => property.PathItems)
                .Select(pathItem => pathItem.OtherEntity)
                .ToHashSet();

            var purposeDtoEntityTree = GetEntityTree(purposeDto);
            SortEntityTree(purposeDtoEntityTree);

            foreach (var entity in entities)
            {
                var treesOfEntity = GetTreesOfEntity(entity.Name, purposeDtoEntityTree);
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

        private static List<ChildableEntry<string>> GetEntityTree(PurposeDto purposeDto)
        {
            var entityTreeRoot = new List<ChildableEntry<string>>() { new("root") };

            foreach (var property in purposeDto.Properties)
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

        private static ChildableEntry<string> FindEntityInPath(string entityName, List<ChildableEntry<string>> purposeDtoEntityTree)
        {
            ChildableEntry<string> entity = purposeDtoEntityTree.SingleOrDefault(item => item.Entry == entityName);
            if (entity != null)
            {
                return entity;
            }

            foreach (var purposeDtoEntityTreeItem in purposeDtoEntityTree)
            {
                entity = FindEntityInPath(entityName, purposeDtoEntityTreeItem.Children);
                if (entity != null)
                {
                    return entity;
                }
            }

            return null;
        }

        private static List<ChildableEntry<string>> GetTreesOfEntity(string entityName, List<ChildableEntry<string>> purposeDtoEntityTree)
        {
            var result = new List<ChildableEntry<string>>();
            foreach (var purposeDtoEntityTreeItem in purposeDtoEntityTree)
            {
                result.AddRange(GetTreesOfEntity(entityName, purposeDtoEntityTreeItem.Children));
            }

            var purposeDtoEntityItem = purposeDtoEntityTree.SingleOrDefault(item => item.Entry == entityName);
            if (purposeDtoEntityItem != null && purposeDtoEntityItem.Children.Count() > 0)
            {
                result.Add(purposeDtoEntityItem);
            }

            return result;
        }

        private static void SortEntityTree(List<ChildableEntry<string>> purposeDtoEntityTree)
        {
            purposeDtoEntityTree.Sort((item1, item2) => String.Compare(item1.Entry, item2.Entry, StringComparison.Ordinal));
            foreach (var treeItem in purposeDtoEntityTree)
            {
                SortEntityTree(treeItem.Children);
            }
        }

        private static string StringifyEntityTree(List<ChildableEntry<string>> purposeDtoEntityTree)
        {
            string currentString = "";
            foreach (var treeItem in purposeDtoEntityTree)
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