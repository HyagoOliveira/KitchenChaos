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
        [SerializeField, Min(1)] private int maxOrders = 3;
        [SerializeField, Min(0F)] private float additionalTimePerIngredient = 10F;
        [SerializeField, Min(0F)] private float timeByNewOrder = 15F;
        [SerializeField, Min(0F)] private float timeToReturnPlate = 4F;

        public event Action OnPlateReturned;
        public event Action<Order> OnOrderFailed;
        public event Action<Order> OnOrderCreated;
        public event Action<Order> OnOrderDelivered;

        public int TotalOrders => orders.Count;

        private List<Order> orders;
        private OrderDisplayer manager;
        private Coroutine ordering;

        internal void Initialize(OrderDisplayer manager)
        {
            orders = new(maxOrders);
            this.manager = manager;
        }

        public bool TryDelivery(Plate plate, out int tip)
        {
            manager.StartCoroutine(ReturnPlateRoutine());

            foreach (var order in orders)
            {
                if (order.TryDelivery(plate))
                {
                    tip = order.Tip;
                    return true;
                }
            }

            tip = 0;
            return false;
        }

        public void CreateRandom() => Create(recipeSettings.GetRandom());

        public void StartOrdering() => ordering = manager.StartCoroutine(OrderingRoutine());

        public void StopOrdering()
        {
            manager.StopCoroutine(ordering);
            CancelOrders();
        }

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
            order.OnDestroyed += HandleOrderDestroyed;
            order.OnDelivered += HandleOrderDelivered;
        }

        private void UnBindListeners(Order order)
        {
            order.OnFailed -= HandleOrderFailed;
            order.OnDestroyed -= HandleOrderDestroyed;
            order.OnDelivered -= HandleOrderDelivered;
        }

        private void CancelOrders()
        {
            foreach (var order in orders)
            {
                order.CancelCountDown();
            }
        }

        private void HandleOrderFailed(Order order) => OnOrderFailed?.Invoke(order);

        private void HandleOrderDestroyed(Order order) => RemoveOrder(order);

        private void HandleOrderDelivered(Order order)
        {
            OnOrderDelivered?.Invoke(order);
            RemoveOrder(order);
        }

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

        private IEnumerator ReturnPlateRoutine()
        {
            yield return new WaitForSeconds(timeToReturnPlate);
            OnPlateReturned?.Invoke();
        }

        private bool CanCreateNewOrders() => TotalOrders < maxOrders;
    }
}