using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aufgabe1_API
{
    public static class MathHelper
    {
        public static double ModuloAngle(double angle)
        {
            while (angle < 0) angle += Math.PI * 2;
            while (angle > Math.PI * 2) angle -= Math.PI * 2;
            return angle;
        }
        public static int GetAngleSide(double angle1, double angle2) => angle1 == angle2 ? 0 : ModuloAngle(angle2 - angle1).CompareTo(Math.PI);
        public static double Clamp(double value, double min, double max) => value < min ? min : value > max ? max : value;
    }

    public static class GeneralHelper
    {
        public static void Deconstruct<T1, T2>(this KeyValuePair<T1, T2> keyValuePair, out T1 key, out T2 value) { key = keyValuePair.Key; value = keyValuePair.Value; }
        public static (T1 value, T2 comparable) Min<T1, T2>(this IEnumerable<T1> enumerable, Func<T1, T2> selector) where T2 : IComparable<T2>
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
    }
}
