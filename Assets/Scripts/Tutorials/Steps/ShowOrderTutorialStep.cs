using UnityEngine;
using KitchenChaos.UI;

namespace KitchenChaos.Tutorials
{
    [DisallowMultipleComponent]
    public sealed class ShowOrderTutorialStep : AbstractTutorialStep
    {
        [SerializeField] private OrderSettings orderSettings;
        [SerializeField, Min(0f)] private float timeToRead = 3F;

        private OrderManager orderManager;

        private void Awake() => orderManager = FindObjectOfType<OrderManager>(includeInactive: true);

        internal override string GetDescription() => $"Orders are received on the top left corner.";

        internal override void Begin()
        {
            orderManager.StartOrderingTutorial(orderSettings);
            Invoke(nameof(Complete), timeToRead);
        }
    }
}