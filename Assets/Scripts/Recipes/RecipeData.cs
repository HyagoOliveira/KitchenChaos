using UnityEngine;
using KitchenChaos.Items;
using System.Collections.Generic;

namespace KitchenChaos.Recipes
{
    [CreateAssetMenu(fileName = "NewRecipe", menuName = EditorPaths.RECIPES + "Recipe", order = 110)]
    public sealed class RecipeData : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public PlatedIngredient[] PlatedIngredients { get; internal set; }

        private Dictionary<IngredientData, PlatedIngredient> platedIngredients;

        internal void Initialize()
        {
            platedIngredients = new(PlatedIngredients.Length);
            foreach (var platedIngredient in PlatedIngredients)
            {
                platedIngredients.Add(platedIngredient.Data, platedIngredient);
            }
        }

        public float GetPreparationTime(IngredientSettings settings, float additionalTimePerIngredient)
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

        internal bool ContainsAll(Stack<Ingredient> ingredients)
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