using UnityEngine;
using KitchenChaos.UI;
using KitchenChaos.Items;
using KitchenChaos.Score;
using KitchenChaos.VisualEffects;
using KitchenChaos.Orders;

namespace KitchenChaos.Counters
{
    /// <summary>
    /// Delivery Counter component to hold Items using <see cref="ItemHolder"/>.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(HighlighterContainer))]
    public sealed class DeliveryCounter : MonoBehaviour, IItemTransfer
    {
        [SerializeField] private ScoreSettings scoreSettings;
        [SerializeField] private DeliveryCounterCanvas canvas;
        [SerializeField] private HighlighterContainer highlightableContainer;

        public bool IsEnabled
        {
            get => enabled;
            set => enabled = value;
        }

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
            var ingredients = plate.Ingredients.ToArray();
            var wasDelivered = OrderManager.Instance.Settings.TryDelivery(ingredients, out int tip);

            Destroy(plate.gameObject);

            if (wasDelivered)
            {
                canvas.ShowTip(tip);
                scoreSettings.DeliveryOrder(tip);
                return true;
            }

            canvas.ShowWrongPlate();
            scoreSettings.DeliveryFailedOrder();

            return false;
        }
    }
}