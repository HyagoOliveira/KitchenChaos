using System;
using UnityEngine;
using System.Collections.Generic;
using KitchenChaos.UI;
using KitchenChaos.Recipes;
using KitchenChaos.Counters;

namespace KitchenChaos.Items
{
    [DisallowMultipleComponent]
    public sealed class Plate : AbstractItem, IItemTransfer
    {
        [SerializeField] private RecipeSettings recipeSettings;
        [SerializeField] private IngredientIcons icons;

        public event Action<Ingredient> OnIngredientPlaced;

        public Stack<Ingredient> Ingredients { get; private set; } = new(4);

        public bool HasIngredients() => Ingredients.Count > 0;

        public bool TryTransferItem(IItemHolder fromHolder)
        {
            var isPreparing = fromHolder is PreparationCounter counter && counter.IsPreparing;
            if (isPreparing) return false;

            var ingredient = fromHolder.CurrentItem as Ingredient;
            var canPlate = ingredient != null && recipeSettings.CanPlate(ingredient.Data);

            if (canPlate)
            {
                fromHolder.ReleaseItem();
                PlateIngredient(ingredient);
            }

            return canPlate;
        }

        public Ingredient RemoveLast()
        {
            var ingredient = Ingredients.Pop();
            ingredient.GetCollectible().Drop();

            icons.Remove(ingredient.Icon);

            return ingredient;
        }

        private void PlateIngredient(Ingredient ingredient)
        {
            ingredient.GetCollectible().PickUp(transform);

            Ingredients.Push(ingredient);
            recipeSettings.Plate(Ingredients);

            icons.Add(ingredient.Icon);

            OnIngredientPlaced?.Invoke(ingredient);
        }
    }
}