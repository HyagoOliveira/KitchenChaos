using UnityEngine;
using KitchenChaos.Items;

namespace KitchenChaos.Recipes
{
    [CreateAssetMenu(fileName = "RecipeSettings", menuName = EditorPaths.RECIPES + "Settings", order = 110)]
    public sealed class RecipeSettings : ScriptableObject
    {
        [SerializeField] private RecipeData[] recipes;

        internal void Initialize()
        {
            foreach (var recipe in recipes)
            {
                recipe.Initialize();
            }
        }

        public RecipeData GetRandom() => RandomUtils.WeightedRandom<RecipeData>(recipes);

        internal bool CanPlate(IngredientData ingredientData)
        {
            foreach (var recipe in recipes)
            {
                if (recipe.CanPlate(ingredientData)) return true;
            }
            return false;
        }

        internal void Plate(Ingredient[] ingredients)
        {
            var hasRecipe = TryGetClosestRecipe(ingredients, out RecipeData recipe);
            if (!hasRecipe)
            {
                Debug.LogError($"{name} does not contains any recipe for any given ingredient.");
                return;
            }

            foreach (var ingredient in ingredients)
            {
                recipe.Plate(ingredient);
            }
        }

        private bool TryGetClosestRecipe(Ingredient[] ingredients, out RecipeData recipe)
        {
            recipe = GetClosestRecipe(ingredients);
            return recipe != null;
        }

        private RecipeData GetClosestRecipe(Ingredient[] ingredients)
        {
            foreach (var recipe in recipes)
            {
                if (recipe.ContainsAll(ingredients)) return recipe;
            }

            return null;
        }
    }
}