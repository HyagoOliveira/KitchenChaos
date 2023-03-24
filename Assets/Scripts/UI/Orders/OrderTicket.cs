using UnityEngine;
using UnityEngine.UI;
using KitchenChaos.Orders;
using KitchenChaos.Recipes;

namespace KitchenChaos.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animation))]
    [RequireComponent(typeof(RectTransform))]
    public sealed class OrderTicket : MonoBehaviour
    {
        [SerializeField] private Slider waitingTimer;
        [SerializeField] private Animation animation;
        [SerializeField] private IngredientIcon iconPrefab;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private HorizontalLayoutGroup iconsGroup;

        public float WaitingTime
        {
            get => waitingTimer.value;
            set => waitingTimer.value = value;
        }

        private void Reset()
        {
            animation = GetComponent<Animation>();
            waitingTimer = GetComponentInChildren<Slider>();
            rectTransform = GetComponent<RectTransform>();
            iconsGroup = GetComponentInChildren<HorizontalLayoutGroup>();
        }

        internal void Initialize(Order order)
        {
            SetIcons(order.PlatedIngredients);
            SetWidth(order.PlatedIngredients.Length);
            SetInitialWaitingTime(order.WaitingTime);

            order.OnFailed += HandleFailed;
            order.OnDelivered += HandleDelivered;
            order.OnWaitingTimeUpdated += HandleWaitingTimeUpdated;
        }

        internal void Dispose(Order order)
        {
            order.OnFailed -= HandleFailed;
            order.OnDelivered -= HandleDelivered;
            order.OnWaitingTimeUpdated -= HandleWaitingTimeUpdated;
        }

        private void SetInitialWaitingTime(float initialWaitingTime)
        {
            waitingTimer.minValue = 0f;
            waitingTimer.maxValue = initialWaitingTime;
            WaitingTime = initialWaitingTime;
        }

        private void SetWidth(int ingredientsCount)
        {
            const float widthPerIngredient = 80f;
            const float spacePerIngredient = 40f;

            var size = rectTransform.sizeDelta;

            size.x = ingredientsCount * (widthPerIngredient + spacePerIngredient);

            rectTransform.sizeDelta = size;
        }

        private void SetIcons(PlatedIngredient[] ingredients)
        {
            foreach (var ingredient in ingredients)
            {
                AddIcon(ingredient.Data.Icon);
            }
        }

        private void AddIcon(Sprite icon)
        {
            var instance = Instantiate(iconPrefab, parent: iconsGroup.transform);
            instance.Icon = icon;
        }

        private void HandleFailed() => PlayFailAnimation();
        private void HandleDelivered() => PlayDeliveredAnimation();
        private void HandleWaitingTimeUpdated(float time) => WaitingTime = time;

        public void PlayFailAnimation() => animation.Play("OrderTicket@Failed");
        public void PlayDeliveredAnimation() => animation.Play("OrderTicket@Delivered");
    }
}