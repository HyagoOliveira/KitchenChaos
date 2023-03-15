using UnityEngine;
using KitchenChaos.Physics;
using KitchenChaos.VisualEffects;

namespace KitchenChaos.Items
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CollectableBody))]
    [RequireComponent(typeof(HighlighterContainer))]
    public sealed class Ingredient : AbstractItem
    {
        [SerializeField] private IngredientData data;

        public IngredientData Data => data;
        public Sprite Icon => data.Icon;
        public IngredientName Name => data.Name;
        public IngredientStatus Status => data.Status;
        public float PreparationTime => data.PreparationTime;
    }
}