using System;
using System.Collections.Generic;
using System.Linq;

namespace Contractor.Core.Helpers
{
    public static class IEnumerableExtension
    {
        public static int FindIndexOfNth<T>(this IEnumerable<T> data, Func<T, bool> predicate, int n)
        {
            int iterationsLeft = n;

            for (int i = 0; i < data.Count(); i++)
            {
                if (predicate(data.ElementAt(i)))
                {
                    iterationsLeft--;
                    if (iterationsLeft == 0)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (predicate == null) throw new ArgumentNullException("predicate");

            int retVal = 0;
            foreach (var item in items)
            {
                if (predicate(item)) return retVal;
                retVal++;
            }
            return -1;
        }

        public static int IndexOf<T>(this IEnumerable<T> items, T item)
        {
            return items.FindIndex(i => EqualityComparer<T>.Default.Equals(item, i));
        }
    }
}