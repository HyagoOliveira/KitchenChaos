using UnityEngine;
using KitchenChaos.Orders;
using System.Collections.Generic;

namespace KitchenChaos.Items
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ItemHolder))]
    public sealed class ItemHolderStack : MonoBehaviour
    {
        [SerializeField] private Plate platePrefab;
        [SerializeField] private ItemHolder holder;

        private Vector3 initialPosition;
        private Stack<IItemCollectable> stack = new Stack<IItemCollectable>(10);

        private void Reset() => holder = GetComponent<ItemHolder>();
        private void Awake() => initialPosition = transform.position;

        private void OnEnable()
        {
            holder.OnItemPlaced += HandleItemPlaced;
            holder.OnItemReleased += HandleItemReleased;

            OrderManager.Instance.Settings.OnPlateReturned += HandlePlateReturned;
        }

        private void OnDisable()
        {
            holder.OnItemPlaced -= HandleItemPlaced;
            holder.OnItemReleased -= HandleItemReleased;

            OrderManager.Instance.Settings.OnPlateReturned -= HandlePlateReturned;
        }

        private void HandleItemPlaced(IItemCollectable item)
        {
            const float plateHeight = 0.12F;

            var position = initialPosition + Vector3.up * stack.Count * plateHeight;
            item.SetPosition(position);

            stack.Push(item);
        }

        private void HandleItemReleased(IItemCollectable _)
        {
            var current = stack.Pop();
            var hasNextItem = stack.TryPeek(out IItemCollectable nextItem);
            if (hasNextItem) holder.CurrentItem = nextItem;
        }

        private void HandlePlateReturned()
        {
            var plate = Instantiate(platePrefab);
            plate.gameObject.name += "_" + stack.Count;
            holder.PlaceItem(plate);
        }
    }
}