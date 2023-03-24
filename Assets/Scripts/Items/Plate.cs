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

        private bool hasBread;

        public event Action<Ingredient> OnIngredientPlated;

        public Stack<Ingredient> Ingredients { get; private set; } = new(4);

        public bool HasIngredients() => Ingredients.Count > 0;

        public bool TryTransferItem(IItemHolder fromHolder)
        {
            var isPreparing = fromHolder is PreparationCounter counter && counter.IsPreparing;
            if (isPreparing) return false;

            var isDoubleBread = hasBread && fromHolder.CurrentItem.IsIngredient(IngredientName.Bread);
            if (isDoubleBread) return false;

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
            if (!hasBread) hasBread = ingredient.Name == IngredientName.Bread;

            ingredient.GetCollectible().PickUp(transform);

            Ingredients.Push(ingredient);
            recipeSettings.Plate(Ingredients.ToArray());

            icons.Add(ingredient.Icon);

            OnIngredientPlated?.Invoke(ingredient);
        }
    }
}