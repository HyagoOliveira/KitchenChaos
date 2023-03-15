using System;
using UnityEngine;
using KitchenChaos.Items;

namespace KitchenChaos.Recipes
{
    [Serializable]
    public struct PlatedIngredient : IEquatable<IngredientData>
    {
        [SerializeField] private IngredientData ingredientData;
        [SerializeField] private Vector3 position;
        [SerializeField] private Vector3 rotation;

        public IngredientData Data => ingredientData;

        public PlatedIngredient(IngredientData ingredientData, Vector3 position, Vector3 rotation)
        {
            this.ingredientData = ingredientData;
            this.position = position;
            this.rotation = rotation;
        }

        public void Plate(Ingredient ingredient)
        {
            var rotation = Quaternion.Euler(this.rotation);
            ingredient.transform.SetLocalPositionAndRotation(position, rotation);
        }

        public bool Equals(IngredientData ingredientData) => this.ingredientData.Equals(ingredientData);
    }
}