using System;
using UnityEngine;
using KitchenChaos.Items;

namespace KitchenChaos.Counters
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animation))]
    public sealed class IngredientCounter : Counter
    {
        [SerializeField] private Animation animation;
        [SerializeField] private SpriteRenderer icon;
        [SerializeField] private IngredientData data;

        public event Action<IItemCollectable> OnItemCollectedFromInside;

        protected override void Reset()
        {
            base.Reset();
            animation = GetComponent<Animation>();
            icon = GetComponentInChildren<SpriteRenderer>();
        }

        private void OnValidate() => UpdateIcon();

        public override bool TryCollectItem(out IItemCollectable item)
        {
            var wasItemCollected = base.TryCollectItem(out item);
            if (wasItemCollected) return true;

            PlayOpenAnimation();
            item = data.Spawn();

            OnItemCollectedFromInside?.Invoke(item);

            return true;
        }

        public IngredientName GetIngredientName() => data.Name;

        private void PlayOpenAnimation() => animation.Play();

        private void UpdateIcon()
        {
            if (icon == null || data == null || data.Icon == null) return;
            icon.sprite = data.Icon;
        }
    }
}