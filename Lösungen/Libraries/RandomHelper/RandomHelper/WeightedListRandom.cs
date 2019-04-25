using System;
using System.Collections.Generic;
using System.Linq;

namespace RandomHelper
{
    public class WeightedListRandom<T>
    {
        private Random random;
        private ICollection<T> list;

        public WeightedListRandom(ICollection<T> list)
        {
            random = new Random();
            this.list = list;
        }

        public WeightedListRandom(ICollection<T> list, Random random)
        {
            this.random = random;
            this.list = list;
        }

        /// <summary>
        /// Returns a random element
        /// </summary>
        /// <param name="take">Whether to remove the item from selection afterwards</param>
        /// <returns></returns>
        public T Next(Func<T, double> weightCalculator, bool take = true)
        {
            if (list == null || list.Count() == 0)
            {
                return default(T);
            }

            double totalWeight = list.Sum(weightCalculator);
            double choiceWeight = (double)random.NextDouble() * totalWeight;
            double currentWeight = 0;

            foreach (T element in list)
            {
                currentWeight += weightCalculator(element);

                if (currentWeight >= choiceWeight)
                {
                    if (take) list.Remove(element);
                    return element;
                }
            }

            return default(T);
        }

        /// <summary>
        /// Returns a list of randomly picked elements
        /// </summary>
        /// <param name="count">The number of elements</param>
        /// <param name="take">Whether to remove the items from selection afterwards</param>
        /// <param name="weightCalculator">Function to calculate weight of T</param>
        /// <returns>Returns a list of randomly picked elements</returns>
        public IEnumerable<T> Next(int count, Func<T, double> weightCalculator, bool take = true)
        {
            for (int i = 0; i < count; i++)
            {
                yield return Next(weightCalculator, take);
            }
        }

        /// <summary>
        /// Returns a list of randomly picked elements, using IWeighted.Weight for the weight
        /// </summary>
        /// <param name="count">The number of elements</param>
        /// <param name="take">Whether to remove the items from selection afterwards</param>
        /// <returns>Returns a list of randomly picked elements</returns>
        public IEnumerable<T> Next(int count, bool take = true)
        {
            if (typeof(T).IsAssignableFrom(typeof(IWeighted)))
            {
                return Next(count, x => ((IWeighted)x).Weight, take);
            }
            else
            {
                throw new Exception("T doesn't implement IWeighted");
            }
        }

        public IEnumerable<T> Shuffle() => Next(list.Count, true);
        public IEnumerable<T> Shuffle(Func<T, double> weightCalculator) => Next(list.Count, weightCalculator, true);

        /// <summary>
        /// Returns a random element, using IWeighted.Weight for the weight
        /// </summary>
        /// <param name="count">The number of elements</param>
        /// <param name="take">Whether to remove the items from selection afterwards</param>
        /// <returns>Returns a random element</returns>
        public T Next(bool take = true)
        {
            if (typeof(T).IsAssignableFrom(typeof(IWeighted)))
            {
                return Next(x => ((IWeighted)x).Weight, take);
            }
            else
            {
                throw new Exception("T doesn't implement IWeighted");
            }
        }

        /// <summary>
        /// Returns the amount of elements remaining in selection
        /// </summary>
        public int Count => list.Count();

        public void Add(T element) => list.Add(element);
    }
}
