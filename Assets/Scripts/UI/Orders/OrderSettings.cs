using System;
using UnityEngine;
using KitchenChaos.Items;
using KitchenChaos.Recipes;
using System.Collections;
using System.Collections.Generic;

namespace KitchenChaos.UI
{
    [CreateAssetMenu(fileName = "OrderSettings", menuName = EditorPaths.SO + "Order Settings", order = 110)]
    public sealed class OrderSettings : ScriptableObject
    {
        [SerializeField] private RecipeSettings recipeSettings;
        [SerializeField] private IngredientSettings ingredientSettings;
        [SerializeField] private Order orderPrefab;
        [SerializeField, Min(1)] private int maxOrders = 4;
        [SerializeField, Min(0F)] private float additionalTimePerIngredient = 3F;
        [SerializeField, Min(0F)] private float timeByNewOrder = 15F;

        public event Action<Order> OnOrderFailed;
        public event Action<Order> OnOrderCreated;
        public event Action<Order> OnOrderDelivered;

        public int TotalOrders => orders.Count;

        private List<Order> orders;
        private OrderManager manager;
        private Coroutine ordering;

        internal void Initialize(OrderManager manager)
        {
            orders = new(maxOrders);
            this.manager = manager;
        }

        public void Delivery(Plate plate)
        {
            foreach (var order in orders)
            {
                if (order.TryDelivery(plate))
                {
                    Destroy(plate.gameObject);
                    break;
                }
            }
        }

        internal void CreateRandom() => Create(recipeSettings.GetRandom());

        internal void StartOrdering() => ordering = manager.StartCoroutine(OrderingRoutine());

        internal void CancelOrdering() => manager.StopCoroutine(ordering);

        internal void Create(RecipeData recipe)
        {
            var order = Instantiate(orderPrefab, manager.transform);
            var time = recipe.GetPreparationTime(ingredientSettings, additionalTimePerIngredient);

            order.SetRecipe(recipe);
            order.SetInitialTime(time);
            order.StartCountDown();

            AddOrder(order);
            OnOrderCreated?.Invoke(order);
        }

        private void AddOrder(Order order)
        {
            orders.Add(order);
            BindListeners(order);
        }

        private void RemoveOrder(Order order)
        {
            orders.Remove(order);
            UnBindListeners(order);
        }

        private void BindListeners(Order order)
        {
            order.OnFailed += HandleOrderFailed;
            order.OnDelivered += HandleOrderDelivered;
        }

        private void UnBindListeners(Order order)
        {
            order.OnFailed -= HandleOrderFailed;
            order.OnDelivered -= HandleOrderDelivered;
        }

        private void HandleOrderDelivered(Order order)
        {
            OnOrderDelivered?.Invoke(order);
            RemoveOrder(order);
        }

        private void HandleOrderFailed(Order order)
        {
            OnOrderFailed?.Invoke(order);
            UnBindListeners(order);
        }

        private IEnumerator OrderingRoutine()
        {
            var waitTimeByNewOrder = new WaitForSeconds(timeByNewOrder);
            var waitUntilCanCreateNewOrders = new WaitUntil(CanCreateNewOrders);

            while (true)
            {
                CreateRandom();
                yield return waitTimeByNewOrder;
                yield return waitUntilCanCreateNewOrders;
            }
        }

        private bool CanCreateNewOrders() => TotalOrders < maxOrders;
    }
}