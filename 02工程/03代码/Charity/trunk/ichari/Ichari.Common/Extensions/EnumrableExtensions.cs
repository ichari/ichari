using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ichari.Common.Extensions
{
    public static class EnumrableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
                action(element);
        }

        public static void ForEach<T>(this IEnumerable<T> source, int i, Action<T, int> func)
        {
            foreach (T element in source)
            {
                i++;
                func(element, i);
            }
        }

        public static HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> source)
        {
            var r = new HashSet<TSource>();
            foreach (var item in source)
            {
                r.Add(item);
            }
            return r;
        }
    }
}
