using UnityEngine;
using KitchenChaos.Items;

namespace KitchenChaos.Recipes
{
    /// <summary>
    /// Editor component utility to transfer all enabled <see cref="Ingredient"/> 
    /// GameObjects children into <see cref="settings"/>.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class PrefabToRecipe : MonoBehaviour
    {
        public RecipeData recipe;
        public IngredientSettings settings;

        [ContextMenu("Transfer Ingredients to Recipe")]
        public void TransferIngredientsToRecipe()
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
            print("Ingredients were transfered to Recipe.");
        }
    }
}