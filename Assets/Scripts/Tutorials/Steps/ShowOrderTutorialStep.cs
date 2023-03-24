using UnityEngine;
using KitchenChaos.UI;
using KitchenChaos.Orders;
using KitchenChaos.Recipes;

namespace KitchenChaos.Tutorials
{
    [DisallowMultipleComponent]
    public sealed class ShowOrderTutorialStep : AbstractTutorialStep
    {
        [SerializeField] private RecipeData recipe;
        [SerializeField, Min(0f)] private float timeToRead = 3F;

        private OrderTicketGroup orderManager;

        private void Awake() => orderManager = FindObjectOfType<OrderTicketGroup>(includeInactive: true);

        internal override string GetDescription() => $"Orders are received on the top left corner.";

        internal override void Begin()
        {
            var order = OrderManager.Instance.Settings.Create(recipe);
            order.CancelCountDownWaitingTime();

            Invoke(nameof(Complete), timeToRead);
        }
    }
}