using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomHelper
{
    public class ListRandom<T>
    {
        private Random random;
        private ICollection<T> list;

        public ListRandom(ICollection<T> list)
        {
            random = new Random();
            this.list = list;
        }

        public ListRandom(ICollection<T> list, Random random)
        {
            if (random == null) random = new Random();
            this.random = random;
            this.list = list;
        }

        /// <summary>
        /// Returns a random element
        /// </summary>
        /// <param name="take">Whether to remove the item from selection afterwards</param>
        /// <returns></returns>
        public T Next(bool take = true)
        {
            if (list == null || list.Count() == 0)
            {
                return default(T);
            }

            int index = random.Next(0, list.Count());
            T element = list.ElementAt(index);
            if (take) list.Remove(element);
            return element;
        }

        /// <summary>
        /// Returns a list of randomly picked elements
        /// </summary>
        /// <param name="count">The number of elements</param>
        /// <param name="take">Whether to remove the items from selection afterwards</param>
        /// <returns></returns>
        public ICollection<T> Next(int count, bool take = true)
        {
            List<T> elements = new List<T>(count);
            for (int i = 0; i < count; i++)
            {
                elements.Add(Next(take));
            }
            return elements;
        }

        public ICollection<T> Shuffle() => Next(list.Count, true);

        /// <summary>
        /// Returns the amount of elements remaining in selection
        /// </summary>
        public int Count => list.Count();

        public void Add(T element) => list.Add(element);
    }
}
