using System;
using System.Collections.Generic;
using System.Linq;

namespace Contractor.Core.Helpers
{
    internal static class DependencySortHelper
    {
        public static IEnumerable<T> Sort<T>(
            this IEnumerable<T> source,
            Func<T, IEnumerable<T>> dependencies)
        {
            var sorted = new List<T>();
            var visited = new List<T>();

            foreach (var item in source)
                Visit(item, visited, sorted, dependencies);

            return sorted;
        }

        private static void Visit<T>(T item, List<T> visited, List<T> sorted, Func<T, IEnumerable<T>> dependencies)
        {
            if (!visited.Contains(item))
            {
                visited.Add(item);

                foreach (var dep in dependencies(item))
                    Visit(dep, visited, sorted, dependencies);

                sorted.Add(item);
            }
        }
    }
}