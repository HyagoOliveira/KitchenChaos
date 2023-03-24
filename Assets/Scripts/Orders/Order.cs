using UnityEngine;
using KitchenChaos.Items;
using KitchenChaos.Recipes;
using System;
using System.Threading;
using System.Collections;
using ActionCode.AwaitableCoroutines;

namespace KitchenChaos.Orders
{
    public sealed class Order
    {
        public event Action OnFailed;
        public event Action OnDelivered;
        public event Action<float> OnWaitingTimeUpdated;

        public int Tip => recipe.tip;
        public string Name => recipe.Name;

        public PlatedIngredient[] PlatedIngredients => recipe.PlatedIngredients;

        public float WaitingTime
        {
            get => waitingTime;
            set
            {
                waitingTime = value;
                OnWaitingTimeUpdated?.Invoke(waitingTime);
            }
        }

        private float waitingTime;

        private readonly RecipeData recipe;
        private readonly CancellationTokenSource countDownWaitingTime;

        public Order(RecipeData recipe, float waitingTime)
        {
            this.recipe = recipe;
            this.waitingTime = waitingTime;

            countDownWaitingTime = new CancellationTokenSource();
        }

        internal bool TryDelivery(Ingredient[] ingredients)
        {
            var canDelivery = recipe.ContainsOnly(ingredients);
            if (canDelivery) Delivery();

            return canDelivery;
        }

        internal void StartCountDownWaitingTime()
        {
            _ = AwaitableCoroutine.Run(
                CountDownWaitingTimeRotine(),
                countDownWaitingTime.Token
            );
        }

        internal void CancelCountDownWaitingTime() => countDownWaitingTime.Cancel();

        private void Delivery()
        {
            CancelCountDownWaitingTime();
            OnDelivered?.Invoke();
        }

        private IEnumerator CountDownWaitingTimeRotine()
        {
            while (WaitingTime > 0F)
            {
                yield return null;
                WaitingTime -= Time.deltaTime;
            }

            WaitingTime = 0F;
            OnFailed?.Invoke();
        }
    }
}