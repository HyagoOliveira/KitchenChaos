using UnityEngine;
using System;
using System.Collections.Generic;
using KitchenChaos.Items;
using KitchenChaos.Counters;

namespace KitchenChaos.Tutorials
{
    public static class TutorialIngredientCounters
    {
        private static readonly Lazy<Dictionary<IngredientName, IngredientCounter>> counters = new(FindCounters);

        internal static IngredientCounter FindCounter(IngredientName name) => counters.Value[name];

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