using UnityEngine;
using ActionCode.Audio;
using ActionCode.Physics;

namespace KitchenChaos.Items
{
    [DisallowMultipleComponent]
    public sealed class ItemHandler : MonoBehaviour
    {
        [SerializeField] private ItemHolder holder;
        [SerializeField] private BoxCaster detector;

        [Header("Audio")]
        [SerializeField] private AudioSourceDictionary dropSource;
        [SerializeField] private AudioSourceDictionary pickupSource;

        private void Reset()
        {
            holder = GetComponentInChildren<ItemHolder>();
            detector = GetComponentInChildren<BoxCaster>();
        }

        public bool IsHoldingPlate() => holder.IsPlate(out Plate _);

        public void TryInteractWithItem()
        {
            if (holder.HasItem())
            {
                if (TryInteractWithPlate()) return;

                var wasItemTransfered = TryTransferItem();
                var canReleaseItemOnTheFloor = !wasItemTransfered && !detector.HasHit;

                if (canReleaseItemOnTheFloor)
                {
                    holder.ReleaseItem();
                    dropSource.PlayRandom();
                }
            }
            else if (TryGetCollectableItem(out IItemCollectable item))
            {
                holder.PlaceItem(item);
                pickupSource.PlayRandom();
            }
        }

        public void TryInteractWithEnvironment()
        {
            var hasInteractable = detector.TryGetEnabledComponent(out IInteractable interactable);
            if (hasInteractable) interactable.Interact();
        }

        private bool TryInteractWithPlate()
        {
            var isHoldingPlate = holder.IsPlate(out Plate plate);
            return
                isHoldingPlate &&
                (
                    TryDisposeLastPlateIngredient(plate) ||
                    TryPlateIngredient(plate)
                );
        }

        private bool TryDisposeLastPlateIngredient(Plate plate)
        {
            if (!plate.HasIngredients()) return false;

            var hasDisposer = detector.TryGetEnabledComponent(out IItemDisposer disposer);
            if (hasDisposer && disposer.IsEnabled) disposer.Dispose(plate.RemoveLast());

            return hasDisposer;
        }

        private bool TryPlateIngredient(Plate plate)
        {
            var hasHolder = detector.TryGetEnabledComponent(out IItemHolder holder);
            return hasHolder && plate.TryTransferItem(holder);
        }

        private bool TryTransferItem()
        {
            var hasTransfer = detector.TryGetEnabledComponent(out IItemTransfer transfer);
            return hasTransfer && transfer.TryTransferItem(this.holder);
        }

        private bool TryGetCollectableItem(out IItemCollectable item)
        {
            var hasCollector = detector.TryGetEnabledComponent(out IItemCollector collector);
            if (hasCollector) return collector.TryCollectItem(out item);

            return detector.TryGetEnabledComponent(out item);
        }
    }
}