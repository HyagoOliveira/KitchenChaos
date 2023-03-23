using UnityEngine;
using KitchenChaos.Items;
using KitchenChaos.VisualEffects;

namespace KitchenChaos.Counters
{
    /// <summary>
    /// Basic Counter component to hold Items using <see cref="ItemHolder"/>.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(HighlighterContainer))]
    public class Counter : MonoBehaviour, IItemHolder, IItemCollector
    {
        [SerializeField] protected ItemHolder holder;
        [SerializeField] protected HighlighterContainer highlightableContainer;

        public IItemCollectable CurrentItem => holder.CurrentItem;

        public bool IsEnabled
        {
            get => enabled;
            set => enabled = value;
        }

        protected virtual void Reset()
        {
            holder = GetComponentInChildren<ItemHolder>();
            highlightableContainer = GetComponent<HighlighterContainer>();
        }

        protected virtual void OnEnable()
        {
            holder.OnItemPlaced += HandleItemPlaced;
            holder.OnItemReleased += HandleItemReleased;
        }

        protected virtual void OnDisable()
        {
            holder.OnItemPlaced -= HandleItemPlaced;
            holder.OnItemReleased -= HandleItemReleased;
        }

        public virtual bool TryTransferItem(IItemHolder fromHolder) => holder.TryTransferItem(fromHolder);

        public virtual bool TryCollectItem(out IItemCollectable item) => holder.TryCollectItem(out item);

        public void ReleaseItem() => holder.ReleaseItem();

        private void HandleItemReleased(IItemCollectable item) =>
            highlightableContainer.Remove(item.GetHighlightable());

        private void HandleItemPlaced(IItemCollectable item) =>
            highlightableContainer.Add(item.GetHighlightable());
    }
}