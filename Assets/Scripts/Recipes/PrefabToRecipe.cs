using UnityEngine;
using KitchenChaos.Items;

namespace KitchenChaos.Recipes
{
    /// <summary>
    /// Editor component to setting <see cref="RecipeData.PlatedIngredients"/>
    /// based on the Ingredients children.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class PrefabToRecipe : MonoBehaviour
    {
        public RecipeData recipe;
        public IngredientSettings settings;

        [ContextMenu("Set Recipe Ingredients")]
        public void SetRecipeIngredients()
        {
            var ingredients = GetComponentsInChildren<Ingredient>();
            var platedIngredients = new PlatedIngredient[ingredients.Length];

            for (int i = 0; i < platedIngredients.Length; i++)
            {
                var ingredient = ingredients[i];
                var ingredientData = settings.GetData(ingredient);

                platedIngredients[i] = new PlatedIngredient(
                    ingredientData,
                    ingredient.transform.localPosition,
                    ingredient.transform.localEulerAngles
                );
            }

            recipe.PlatedIngredients = platedIngredients;

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(recipe);
#endif
        }
    }
}