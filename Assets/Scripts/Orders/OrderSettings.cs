using System;
using UnityEngine;
using KitchenChaos.Items;
using KitchenChaos.Recipes;
using System.Collections;
using System.Collections.Generic;

namespace KitchenChaos.Orders
{
    [CreateAssetMenu(fileName = "OrderSettings", menuName = EditorPaths.SO + "Order Settings", order = 110)]
    public sealed class OrderSettings : ScriptableObject
    {
        [SerializeField] private RecipeSettings recipeSettings;
        [SerializeField] private IngredientSettings ingredientSettings;

        [SerializeField, Min(1)] private int maxOrders = 3;
        [SerializeField, Min(0F)] private float additionalTimePerIngredient = 10F;
        [SerializeField, Min(0F)] private float timeByNewOrder = 15F;
        [SerializeField, Min(0F)] private float timeToRemoveAfterFail = 1F;
        [SerializeField, Min(0F)] private float timeToRemoveAfterDelivery = 1F;
        [SerializeField, Min(0F)] private float timeToReturnPlate = 4F;

        public event Action OnPlateReturned;
        public event Action<Order> OnOrderFailed;
        public event Action<Order> OnOrderCreated;
        public event Action<Order> OnOrderRemoved;
        public event Action<Order> OnOrderDelivered;

        public int TotalOrders => orders.Count;

        private List<Order> orders;
        private Coroutine ordering;
        private OrderManager manager;

        internal void Initialize(OrderManager manager)
        {
            if (orders != null) CancelAll();

            orders = new(maxOrders);
            this.manager = manager;
        }

        public bool TryDelivery(Ingredient[] ingredients, out int tip)
        {
            manager.StartCoroutine(ReturnPlateRoutine());

            foreach (var order in orders)
            {
                if (order.TryDelivery(ingredients))
                {
                    tip = order.Tip;
                    return true;
                }
            }

            tip = 0;
            return false;
        }

        public void StartOrdering() => ordering = manager.StartCoroutine(OrderingRoutine());

        public void StopOrdering()
        {
            if (ordering != null) manager.StopCoroutine(ordering);
            CancelAll();
        }

        internal void CreateRandom() => Create(recipeSettings.GetRandom());

        private void Create(RecipeData recipe)
        {
            var waitingTime = recipe.GetWaitingTime(
                ingredientSettings,
                additionalTimePerIngredient
            );
            var order = new Order(recipe, waitingTime);

            Create(order);
        }

        private void Create(Order order)
        {
            order.OnFailed += () => Fail(order);
            order.OnDelivered += () => Delivery(order);

            order.StartCountDownWaitingTime();

            orders.Add(order);
            OnOrderCreated?.Invoke(order);
        }

        private void Remove(Order order)
        {
            order.OnFailed -= () => Fail(order);
            order.OnDelivered -= () => Delivery(order);

            orders.Remove(order);
            OnOrderRemoved?.Invoke(order);
        }

        private void Fail(Order order)
        {
            OnOrderFailed?.Invoke(order);
            manager.StartCoroutine(RemoveRoutine(order, timeToRemoveAfterFail));
        }

        private void Delivery(Order order)
        {
            OnOrderDelivered?.Invoke(order);
            manager.StartCoroutine(RemoveRoutine(order, timeToRemoveAfterDelivery));
        }

        private void CancelAll()
        {
            foreach (var order in orders)
            {
                order.CancelCountDownWaitingTime();
            }
        }

        private bool CanCreateNewOrders() => TotalOrders < maxOrders;

        private IEnumerator OrderingRoutine()
        {
            var waitTimeByNewOrder = new WaitForSeconds(timeByNewOrder);
            var waitUntilCanCreateNewOrders = new WaitUntil(CanCreateNewOrders);

            while (true)
            {
                CreateRandom();
                yield return waitUntilCanCreateNewOrders;
                yield return waitTimeByNewOrder;
            }
        }

        private IEnumerator RemoveRoutine(Order order, float time)
        {
            yield return new WaitForSeconds(time);
            Remove(order);
        }

        private IEnumerator ReturnPlateRoutine()
        {
            yield return new WaitForSeconds(timeToReturnPlate);
            OnPlateReturned?.Invoke();
        }
    }
}