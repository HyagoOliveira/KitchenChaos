using System;
using UnityEngine;

namespace KitchenChaos.Items
{
    [CreateAssetMenu(fileName = "NewIngredient", menuName = EditorPaths.INGREDIENTS + "Ingredient", order = 110)]
    public sealed class IngredientData : ScriptableObject, IEquatable<IngredientData>
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public Ingredient Prefab { get; private set; }
        [field: SerializeField] public IngredientName Name { get; private set; }
        [field: SerializeField] public IngredientStatus Status { get; private set; }
        [field: SerializeField] public float PreparationTime { get; private set; }

        public Ingredient Spawn() => Instantiate<Ingredient>(Prefab);

        public override string ToString() => $"{Name}_{Status}";

        public bool Equals(IngredientData data) =>
            Name == data.Name &&
            Status == data.Status;
    }
}