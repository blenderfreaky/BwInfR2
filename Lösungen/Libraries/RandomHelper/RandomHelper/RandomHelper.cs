using RandomHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomHelper
{
    public static class RandomHelper
    {
        public static ICollection<T> Shuffle<T>(this ICollection<T> list, Random random = null) => new ListRandom<T>(list, random).Next(list.Count, true);

        public static T Next<T>(this ICollection<T> list, Random random = null) => new ListRandom<T>(list, random).Next(false);

        public static ICollection<T> Next<T>(this ICollection<T> list, int count, bool take = true, Random random = null) => new ListRandom<T>(list, random).Next(count, take);


        public static IEnumerable<T> ShuffleWeighted<T>(this ICollection<T> list, Random random = null) => new WeightedListRandom<T>(list, random).Next(list.Count, true);
        public static IEnumerable<T> ShuffleWeighted<T>(this ICollection<T> list, Func<T, double> weightCalculator, Random random = null) => new WeightedListRandom<T>(list, random).Next(list.Count, weightCalculator, true);

        public static T NextWeighted<T>(this ICollection<T> list, Random random = null) => new WeightedListRandom<T>(list, random).Next(false);
        public static T NextWeighted<T>(this ICollection<T> list, Func<T, double> weightCalculator, Random random = null) => new WeightedListRandom<T>(list, random).Next(weightCalculator, false);

        public static IEnumerable<T> NextWeighted<T>(this ICollection<T> list, int count, bool take = true, Random random = null) => new WeightedListRandom<T>(list, random).Next(count, take);
        public static IEnumerable<T> NextWeighted<T>(this ICollection<T> list, int count, Func<T, double> weightCalculator, bool take = true, Random random = null) => new WeightedListRandom<T>(list, random).Next(count, weightCalculator, take);

    }
}
