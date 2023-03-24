using UnityEngine;
using KitchenChaos.UI;
using KitchenChaos.Orders;

namespace KitchenChaos.Tutorials
{
    [DisallowMultipleComponent]
    public sealed class ShowOrderTutorialStep : AbstractTutorialStep
    {
        [SerializeField, Min(0f)] private float timeToRead = 3F;

        private OrderDisplayer orderManager;

        private void Awake() => orderManager = FindObjectOfType<OrderDisplayer>(includeInactive: true);

        internal override string GetDescription() => $"Orders are received on the top left corner.";

        internal override void Begin()
        {
            OrderManager.Instance.Settings.StartOrdering();
            Invoke(nameof(Complete), timeToRead);
        }
    }
}