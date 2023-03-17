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
    public sealed class DeliveryCounter : MonoBehaviour, IItemTransfer
    {
        [SerializeField] private OrderSettings orderSettings;
        [SerializeField] private DeliveryCounterCanvas canvas;
        [SerializeField] private HighlighterContainer highlightableContainer;

        private void Reset()
        {
            canvas = GetComponentInChildren<DeliveryCounterCanvas>();
            highlightableContainer = GetComponent<HighlighterContainer>();
        }

        public bool TryTransferItem(IItemHolder fromHolder)
        {
            var item = fromHolder.CurrentItem;
            var plate = item as Plate;

            if (plate == null)
            {
                canvas.ShowNeedPlate();
                return false;
            }

            fromHolder.ReleaseItem();
            var wasDelivered = orderSettings.TryDelivery(plate, out int tip);

            Destroy(plate.gameObject);

            if (wasDelivered)
            {
                canvas.ShowTip(tip);
                //TODO Add into Score
                return true;
            }

            return false;
        }
    }
}