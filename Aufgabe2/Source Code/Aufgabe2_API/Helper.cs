using System;
using System.Collections.Generic;
using System.Linq;

namespace Aufgabe2_API
{
    public static class MathHelper
    {
        public static int PositiveModulo(int value, int offset, int length)
        {
            while (value < offset) value += length;
            while (value >= offset + length) value -= length;
            return value;
        }
        public static double PositiveModulo(double value, double offset, double length)
        {
            while (value < offset) value += length;
            while (value >= offset + length) value -= length;
            return value;
        }
        public static double ModuloAngle(double angle) => PositiveModulo(angle, 0, 2 * Math.PI);
        public static double ModuloHalfAngle(double angle) => PositiveModulo(angle, 0, Math.PI);

        public static double SmallerAngleSide(double angle) => Math.Min(ModuloAngle(angle), ModuloAngle(-angle));
        public static int GetAngleSide(double angle1, double angle2) => ModuloAngle(angle2 - angle1) == 0 ? 0 : ModuloAngle(angle2 - angle1).CompareTo(Math.PI);
        public static double Clamp(double value, double min, double max) => value < min ? min : value > max ? max : value;

        /// <summary>
        /// Replaces 0 with different value
        /// </summary>
        /// <param name="input">The number to potentially replace</param>
        /// <param name="ifZero">Function returning the replacing value</param>
        /// <returns>Returns input, if input != 0, if input == 0, it returns the return value of ifZero</returns>
        public static int IfZero(this int input, Func<int> ifZero) => input == 0 ? ifZero() : input;

        public static bool Approx(this double first, double second, double epsilon) => Math.Abs(first - second) < epsilon;
        public static bool Approx(this Vector first, Vector second, double epsilonSquared) => first.DistanceSquared(second) < epsilonSquared;
        public static int CompareToApprox(this double first, double second, double epsilon) => Approx(first, second, epsilon) ? 0 : first.CompareTo(second);
    }

    public static class GeneralHelper
    {
        public static void Deconstruct<T1, T2>(this KeyValuePair<T1, T2> keyValuePair, out T1 key, out T2 value) { key = keyValuePair.Key; value = keyValuePair.Value; }
        public static (T1 value, T2 comparable) MinValue<T1, T2>(this IEnumerable<T1> enumerable, Func<T1, T2> selector) where T2 : IComparable<T2>
        {
            T1 first = enumerable.First();
            (T1 value, T2 comparable) min = (first, selector(first));
            foreach (T1 elemValue in enumerable)
            {
                T2 elemComparable = selector(elemValue);
                if (elemComparable.CompareTo(min.comparable) < 0) min = (elemValue, elemComparable);
            }
            return min;
        }
        public static (T1 value, T2 comparable) MaxValue<T1, T2>(this IEnumerable<T1> enumerable, Func<T1, T2> selector) where T2 : IComparable<T2>
        {
            T1 first = enumerable.First();
            (T1 value, T2 comparable) max = (first, selector(first));
            foreach (T1 elemValue in enumerable)
            {
                T2 elemComparable = selector(elemValue);
                if (elemComparable.CompareTo(max.comparable) > 0) max = (elemValue, elemComparable);
            }
            return max;
        }
        public static T MinValue<T>(this IEnumerable<T> enumerable, IComparer<T> comparer)
        {
            T min = enumerable.First();
            foreach (T elem in enumerable) if (comparer.Compare(elem, min) < 0) min = elem;
            return min;
        }
        public static T MaxValue<T>(this IEnumerable<T> enumerable, IComparer<T> comparer)
        {
            T max = enumerable.First();
            foreach (T elem in enumerable) if (comparer.Compare(elem, max) > 0) max = elem;
            return max;
        }
        public static T MinValue<T>(this IEnumerable<T> enumerable, Comparison<T> comparer)
        {
            T min = enumerable.First();
            foreach (T elem in enumerable) if (comparer(elem, min) < 0) min = elem;
            return min;
        }
        public static T MaxValue<T>(this IEnumerable<T> enumerable, Comparison<T> comparer)
        {
            T max = enumerable.First();
            foreach (T elem in enumerable) if (comparer(elem, max) > 0) max = elem;
            return max;
        }
        public static T2 Let<T1, T2>(this T1 obj, Func<T1, T2> func) => func(obj);
        public static void Let<T>(this T obj, Action<T> action) => action(obj);

        // Taken from https://stackoverflow.com/questions/3645644/whats-your-favorite-linq-to-objects-operator-which-is-not-built-in/3645715#3645715

        /// <summary>Adds a single element to the end of an IEnumerable.</summary>
        /// <typeparam name="T">Type of enumerable to return.</typeparam>
        /// <returns>IEnumerable containing all the input elements, followed by the
        /// specified additional element.</returns>
        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T element)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            return concatIterator(element, source, false);
        }

        /// <summary>Adds a single element to the start of an IEnumerable.</summary>
        /// <typeparam name="T">Type of enumerable to return.</typeparam>
        /// <returns>IEnumerable containing the specified additional element, followed by
        /// all the input elements.</returns>
        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> tail, T head)
        {
            if (tail == null)
                throw new ArgumentNullException("tail");
            return concatIterator(head, tail, true);
        }

        private static IEnumerable<T> concatIterator<T>(T extraElement,
            IEnumerable<T> source, bool insertAtStart)
        {
            if (insertAtStart)
                yield return extraElement;
            foreach (var e in source)
                yield return e;
            if (!insertAtStart)
                yield return extraElement;
        }
    }
}
