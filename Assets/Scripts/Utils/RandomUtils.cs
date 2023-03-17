using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace KitchenChaos
{
    public static class RandomUtils
    {
        /// <summary>
        /// Selects a weighted random item.
        /// </summary>
        /// <typeparam name="T">The generic type implementing <see cref="IChanceable"/>.</typeparam>
        /// <param name="items">A list of items implementing <see cref="IChanceable"/>.</param>
        /// <returns>A weighted random item from the given items list.</returns>
        public static T WeightedRandom<T>(IReadOnlyList<IChanceable> items) where T : IChanceable
        {
            var itemCount = items.Count;
            var orderedItems = new float[itemCount + 1];

            for (int i = 0; i < itemCount; i++)
                orderedItems[i + 1] = orderedItems[i] + items[i].Chance;

            var num = Random.value * orderedItems[itemCount];
            var index = BinarySearch(orderedItems, itemCount, num);

            if (index < 0) index = ~index - 1;

            return (T)items[index];
        }

        private static int BinarySearch(float[] array, int length, float value)
        {
            var low = 0;
            var high = length - 1;

            while (low <= high)
            {
                var i = low + ((high - low) >> 1);
                var order = array[i].CompareTo(value);

                if (order == 0) return i;

                if (order < 0) low = i + 1;
                else high = i - 1;
            }

            return ~low;
        }
    }
}