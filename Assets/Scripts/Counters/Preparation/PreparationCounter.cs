using UnityEngine;
using KitchenChaos.Items;
using System;

namespace KitchenChaos.Counters
{
    [DisallowMultipleComponent]
    public sealed class PreparationCounter : Counter, IInteractable
    {
        [SerializeField] private AbstractIngredientPreparator preparator;

        public event Action OnInteracted;

        public bool IsPreparing => preparator.IsPreparing;

        protected override void Reset()
        {
            base.Reset();
            preparator = GetComponentInChildren<AbstractIngredientPreparator>();
        }

        public override bool TryCollectItem(out IItemCollectable item)
        {
            if (IsPreparing)
            {
                item = null;
                return false;
            }

            return base.TryCollectItem(out item);
        }

        public override bool TryTransferItem(IItemHolder fromHolder) =>
            !preparator.IsPreparing && base.TryTransferItem(fromHolder);

        public void Interact()
        {
            preparator.Interact();
            OnInteracted?.Invoke();
        }
    }
}