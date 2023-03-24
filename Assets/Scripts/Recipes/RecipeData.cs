using UnityEngine;
using KitchenChaos.Items;
using System.Collections.Generic;

namespace KitchenChaos.Recipes
{
    [CreateAssetMenu(fileName = "NewRecipe", menuName = EditorPaths.RECIPES + "Recipe", order = 110)]
    public sealed class RecipeData : ScriptableObject, IChanceable
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public PlatedIngredient[] PlatedIngredients { get; internal set; }
        [field: SerializeField, Range(0, 100)] public float Chance { get; private set; } = 50F;
        [field: SerializeField, Min(0)] public int tip = 1;

        private Dictionary<IngredientData, PlatedIngredient> platedIngredients;

        internal void Initialize()
        {
            platedIngredients = new(PlatedIngredients.Length);
            foreach (var platedIngredient in PlatedIngredients)
            {
                platedIngredients.Add(platedIngredient.Data, platedIngredient);
            }
        }

        public float GetWaitingTime(IngredientSettings settings, float additionalTimePerIngredient)
        {
            var time = PlatedIngredients.Length * additionalTimePerIngredient;
            foreach (var ingredient in PlatedIngredients)
            {
                time += settings.GetPreparationTime(ingredient.Data);
            }

            return time;
        }

        internal void Plate(Ingredient ingredient) =>
            platedIngredients[ingredient.Data].Plate(ingredient);

        internal bool CanPlate(IngredientData ingredientData) =>
            platedIngredients.ContainsKey(ingredientData) &&
            platedIngredients[ingredientData].Equals(ingredientData);

        internal bool ContainsOnly(Ingredient[] ingredients)
        {
            var hasSameNumerOfIngredients = ingredients.Length == platedIngredients.Count;
            return hasSameNumerOfIngredients && ContainsAll(ingredients);
        }

        internal bool ContainsAll(Ingredient[] ingredients)
        {
            foreach (var ingredient in ingredients)
            {
                if (!platedIngredients.ContainsKey(ingredient.Data))
                    return false;
            }
            return true;
        }
    }
}