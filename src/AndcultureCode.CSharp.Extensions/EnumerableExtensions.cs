using System;
using System.Collections.Generic;
using System.Linq;

namespace AndcultureCode.CSharp.Extensions
{
public static class EnumerableExtensions
    {
        public static T              PickRandom<T>(this IEnumerable<T> source)            => source.PickRandom(1).Single();
        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count) => source.Shuffle().Take(count);
        public static IEnumerable<T> Shuffle<T>   (this IEnumerable<T> source)            => source.OrderBy(x => Guid.NewGuid());

        /// <summary>
        /// Returns a distinct enumerable by a specific property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> property)
        {
            return source.GroupBy(property).Select(x => x.First());
        }

        /// <summary>
        /// Returns a distinct list by a specific property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static IEnumerable<T> DistinctBy<T, TKey>(this List<T> source, Func<T, TKey> property)
        {
            return source.GroupBy(property).Select(x => x.First());
        }

        /// <summary>
        /// Returns a list of items, grouped by adjacent values
        /// Source: https://stackoverflow.com/questions/4681949/use-linq-to-group-a-sequence-of-numbers-with-no-gaps/4682163#4682163
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> GroupAdjacentBy<T>(this IEnumerable<T> source, Func<T, T, bool> predicate)
        {
            using (var e = source.GetEnumerator())
            {
                if (e.MoveNext())
                {
                    var list = new List<T> { e.Current };
                    var pred = e.Current;
                    while (e.MoveNext())
                    {
                        if (predicate(pred, e.Current))
                        {
                            list.Add(e.Current);
                        }
                        else
                        {
                            yield return list;
                            list = new List<T> { e.Current };
                        }
                        pred = e.Current;
                    }
                    yield return list;
                }
            }
        }

        /// <summary>
        /// Determines if the source list is empty
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsEmpty<T>(this IEnumerable<T> source)                          => !source.Any();
        public static bool IsEmpty<T>(this IEnumerable<T> source, Func<T, bool> predicate) => !source.Any(predicate);

        /// <summary>
        /// Determines if the source list is null or empty
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)                          => source == null || source.IsEmpty();
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source, Func<T, bool> predicate) => source == null || source.IsEmpty(predicate);

        /// <summary>
        /// Convenience method so joining strings reads better :)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string Join(this IEnumerable<string> list, string delimiter = ", ")
        {
            return list?.ToList().Join(delimiter);
        }

        /// <summary>
        /// Convenience method for joining dictionary key values into a string
        /// </summary>
        /// <param name="list"></param>
        /// <param name="keyValueDelimiter"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string Join(this IEnumerable<KeyValuePair<string, string>> list, string keyValueDelimiter, string delimiter = ", ")
        {
            return list?.Select(e => e.Join(keyValueDelimiter)).Join(delimiter);
        }

        /// <summary>
        /// Convenience method so joining a list of strings
        /// </summary>
        /// <param name="list"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string Join(this List<string> list, string delimiter = ", ")
        {
            return list == null ? null : string.Join(delimiter, list);
        }

        /// <summary>
        /// Convenience method so joining key value pairs
        /// </summary>
        /// <param name="pair"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string Join(this KeyValuePair<string, string> pair, string delimiter)
        {
            var results = new List<string>
            {
                pair.Key,
                pair.Value
            };

            return results.Where(e => !string.IsNullOrEmpty(e)).Join(delimiter);
        }
    }
}
