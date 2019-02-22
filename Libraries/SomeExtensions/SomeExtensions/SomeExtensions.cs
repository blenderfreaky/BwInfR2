using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SomeExtensions
{
    public static class SomeExtensions
    {
        /// <summary>
        /// Returns the value of the given key if it is present in the dictionary, else it will return the default value
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default(TValue))
        {
            return dictionary.TryGetValue(key, out TValue value) ? value : defaultValue;
        }

        /// <summary>
        /// Returns the value of the given key if it is present in the dictionary, else it will return the default value
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue GetValueOrAddDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default(TValue))
        {
            if (dictionary.TryGetValue(key, out TValue value))
            {
                return value;
            }
            else
            {
                dictionary.Add(key, defaultValue);
                return defaultValue;
            }
        }

        /// <summary>
        /// Adds element to list if it is not already contained in it
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="list"></param>
        /// <param name="value"></param>
        /// <returns>True if the element was new</returns>
        public static bool AddIfNew<TValue>(this IList<TValue> list, TValue value)
        {
            if (list.Contains(value)) return false;
            list.Add(value);
            return true;
        }

        /// <summary>
        /// Adds KeyValuePair to dictionary if the key is not already present
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>True if the element was new</returns>
        public static bool AddIfNew<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key)) return false;
            dictionary.Add(key, value);
            return true;
        }

        /// <summary>
        /// Adds multiple elements to a list, and initializes the list if it is null
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns>True if the list was null</returns>
        public static bool AddRangeAlways<TKey, TValue>(this IDictionary<TKey, IList<TValue>> dictionary, TKey key, IList<TValue> values)
        {
            if (!dictionary.ContainsKey(key) || dictionary[key] == null)
            {
                dictionary[key] = new List<TValue>();
                foreach (TValue val in values) dictionary[key].Add(val);
                return true;
            }
            foreach (TValue val in values) dictionary[key].Add(val);
            return true;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T value in enumerable)
            {
                action(value);
                yield return value;
            }
        }

        public static T2 Case<T1, T2>(T1 var, params Func<T1, T2>[] cases) => cases.First(x => x(var) != null)(var);
    }
}
