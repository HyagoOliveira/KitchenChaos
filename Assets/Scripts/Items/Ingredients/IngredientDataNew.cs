using System;
using UnityEngine;

namespace KitchenChaos.Items
{
    [Serializable]
    public sealed class IngredientDataNew : IEquatable<IngredientData>
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public Ingredient Prefab { get; private set; }
        [field: SerializeField] public IngredientStatus Status { get; private set; }
        [field: SerializeField] public float PreparationTime { get; private set; }

        public Ingredient Spawn() => GameObject.Instantiate<Ingredient>(Prefab);

        public bool Equals(IngredientData other) =>
            Prefab == other.Prefab &&
            Status == other.Status;
    }
}