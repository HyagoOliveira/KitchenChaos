using UnityEngine;
using System.Collections.Generic;
using KitchenChaos.Items;
using KitchenChaos.Counters;

namespace KitchenChaos.Tutorials
{
    public static class TutorialIngredientCounters
    {
        private static Dictionary<IngredientName, IngredientCounter> counters;

        internal static void Load() => counters = FindCounters();

        internal static IngredientCounter FindCounter(IngredientName name) => counters[name];

        private static Dictionary<IngredientName, IngredientCounter> FindCounters()
        {
            var ingredientCounters = GameObject.FindObjectsByType<IngredientCounter>(FindObjectsSortMode.None);
            var counterDict = new Dictionary<IngredientName, IngredientCounter>(ingredientCounters.Length);

            foreach (var counter in ingredientCounters)
            {
                counterDict.Add(counter.GetIngredientName(), counter);
            }

            return counterDict;
        }
    }
}