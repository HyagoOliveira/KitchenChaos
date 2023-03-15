using UnityEngine;
using System.Collections.Generic;

namespace KitchenChaos.Items
{
    [CreateAssetMenu(fileName = "IngredientDataCollection", menuName = EditorPaths.INGREDIENTS + "Collection", order = 110)]
    public sealed class IngredientDataCollection : ScriptableObject
    {
        [field: SerializeField] public IngredientName Name { get; private set; }

        [SerializeField] private IngredientData[] ingredients;

        internal IngredientData[] Ingredients => ingredients;

        private Dictionary<IngredientStatus, IngredientData> ingredientsDictionary;

        public bool TryGetIngredientData(IngredientStatus status, out IngredientData data) =>
            ingredientsDictionary.TryGetValue(status, out data);

        internal IngredientData GetPreviousIngredientData(IngredientData data)
        {
            for (int i = 0; i < ingredients.Length; i++)
            {
                var isIngredient = ingredients[i].Equals(data);
                if (isIngredient)
                {
                    var previousIndex = i - 1;
                    var safeIndex = Mathf.Max(0, previousIndex);
                    return ingredients[safeIndex];
                }
            }

            return null;
        }

        internal void InitializeDictionary()
        {
            ingredientsDictionary = new(ingredients.Length);

            foreach (var data in ingredients)
            {
                ingredientsDictionary.Add(data.Status, data);
            }
        }
    }
}