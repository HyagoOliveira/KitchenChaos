using UnityEngine;
using KitchenChaos.Items;
using KitchenChaos.VisualEffects;
using KitchenChaos.UI;

namespace KitchenChaos.Counters
{
    /// <summary>
    /// Delivery Counter component to hold Items using <see cref="ItemHolder"/>.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(HighlighterContainer))]
    public class DeliveryCounter : MonoBehaviour, IItemTransfer
    {
        [SerializeField] private OrderSettings orderSettings;
        [SerializeField] protected HighlighterContainer highlightableContainer;

        protected virtual void Reset()
        {
            highlightableContainer = GetComponent<HighlighterContainer>();
        }

        public virtual bool TryTransferItem(IItemHolder fromHolder)
        {
            var item = fromHolder.CurrentItem;
            var plate = item as Plate;

            if (plate == null) return false;

            fromHolder.ReleaseItem();
            orderSettings.Delivery(plate);

            return true;
        }
    }
}