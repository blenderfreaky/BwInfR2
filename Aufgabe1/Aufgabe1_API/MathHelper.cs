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

        //TODO: This should be somewhere else
        //public static void RemoveUnsorted<T>(this SortedSet<T> set, T t) => set.RemoveWhere(x => x == t);

        public static void RemoveSimilar<T1, T2>(this SortedSet<T1> set, T2 t2) => set.RemoveWhere(x => x.Equals(t2));
        public static bool ContainsSimilar<T1, T2>(this SortedSet<T1> set, T2 t2) => set.Any(x => x.Equals(t2));
        public static void Deconstruct<T1, T2>(this KeyValuePair<T1, T2> keyValuePair, out T1 key, out T2 value) { key = keyValuePair.Key; value = keyValuePair.Value; }
    }
}
