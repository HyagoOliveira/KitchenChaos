using System;
using UnityEngine;

namespace KitchenChaos.Items
{
    [DisallowMultipleComponent]
    public sealed class ItemHolder : MonoBehaviour, IItemHolder, IItemCollector
    {
        public event Action<IItemCollectable> OnItemReleased;
        public event Action<IItemCollectable> OnItemPlaced;

        public bool IsEnabled
        {
            get => enabled;
            set => enabled = value;
        }

        public IItemCollectable CurrentItem { get; internal set; }

        private void Start() => CheckChildItem();

        public void PlaceItem(IItemCollectable item)
        {
            item.GetCollectible().PickUp(transform);
            CurrentItem = item;

            OnItemPlaced?.Invoke(CurrentItem);
        }

        public void ReleaseItem()
        {
            var item = CurrentItem;

            CurrentItem?.GetCollectible().Drop();
            CurrentItem = null;

            OnItemReleased?.Invoke(item);
        }

        public void ReplaceItem(IItemCollectable item)
        {
            DestroyItem();
            PlaceItem(item);
        }

        public bool TryCollectItem(out IItemCollectable item)
        {
            if (IsEmpty() || !CurrentItem.IsEnabled)
            {
                item = null;
                return false;
            }

            item = CurrentItem;
            ReleaseItem();

            return item != null;
        }

        public bool TryTransferItem(IItemHolder fromHolder)
        {
            if (HasItem())
            {
                var isPlate = IsPlate(out Plate plate);
                if (isPlate) return plate.TryTransferItem(fromHolder);

                return false;
            }

            var item = fromHolder.CurrentItem;

            fromHolder.ReleaseItem();
            PlaceItem(item);

            return true;
        }

        public void DestroyItem()
        {
            var item = CurrentItem;

            ReleaseItem();
            item?.Destroy();
        }

        public bool HasItem() => CurrentItem != null;

        public bool IsEmpty() => CurrentItem == null;

        public bool IsItem<T>(out T item) where T : IItemCollectable
        {
            if (HasItem())
            {
                if (CurrentItem is T genericItem)
                {
                    item = genericItem;
                    return true;
                }
            }

            item = default;
            return false;
        }

        public bool IsPlate(out Plate plate) => IsItem(out plate);

        private void CheckChildItem()
        {
            var childItem = GetComponentInChildren<IItemCollectable>();
            if (childItem != null) PlaceItem(childItem);
        }
    }
}