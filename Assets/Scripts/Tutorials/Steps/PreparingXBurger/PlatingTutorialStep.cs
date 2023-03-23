using UnityEngine;
using KitchenChaos.UI;
using KitchenChaos.Items;
using KitchenChaos.Counters;

namespace KitchenChaos.Tutorials
{
    [DisallowMultipleComponent]
    public sealed class PlatingTutorialStep : AbstractTutorialStep
    {
        [SerializeField] private OrderSettings orderSettings;

        private Counter[] counters;
        private Counter plateCounter;

        private const int stoveArrowIndex = 0;
        private const int cuttingBoardArrowIndex = 1;

        private bool isMeatPlated;
        private bool isCheesePlated;

        private void Start() => FindCounters();

        internal override string GetDescription() => "Plating Ingredients";

        internal override void Begin()
        {
            manager.EnableCheeseBurgerTitle(true);
            Invoke(nameof(GuideToBreadCounter), 2F);
        }

        private void GuideToBreadCounter()
        {
            manager.PlaceArrowOverCounter(manager.BreadCounter);
            manager.SetDescription($"Collect a Bread using {GetCollectButtonDisplayName()}.");

            manager.BreadCounter.OnItemCollectedFromInside += HandleBreadCollected;
        }

        private void HandleBreadCollected(IItemCollectable _)
        {
            manager.BreadCounter.OnItemCollectedFromInside -= HandleBreadCollected;

            // Disabling Player Switch so the other Player cannot take the Plate.
            manager.PlayerSettings.DisablePlayerSwitch();

            // Disabling all counter so Player can only interact with the Plate Counter.
            EnableCounters(false);
            plateCounter.IsEnabled = true;

            manager.HideArrow();
            manager.Plate.IsEnabled = true;
            CompleteDescriptionAndInvoke(GuideToPlaceBreadOverPlate);
        }

        private void GuideToPlaceBreadOverPlate()
        {
            manager.PlaceArrowOverPlate();
            manager.SetDescription($"Place the Bread over the Plate using {GetCollectButtonDisplayName()}.");

            manager.Plate.OnIngredientPlated += HandleCheesePlacedOverPlate;
        }

        private void HandleCheesePlacedOverPlate(Ingredient ingredient)
        {
            var isBread = ingredient.IsIngredient(IngredientName.Bread);
            if (!isBread) return;

            manager.Plate.OnIngredientPlated -= HandleCheesePlacedOverPlate;

            manager.HideArrow();
            manager.PlayerSettings.EnablePlayerSwitch();

            EnableCounters(true);

            CompleteDescriptionAndInvoke(CheckWhetherPlayerIsHoldingPlate);
        }

        private void CheckWhetherPlayerIsHoldingPlate()
        {
            var isPlayerHoldingPlate = CurrentPlayer.Interactor.IsHoldingPlate();
            if (isPlayerHoldingPlate) GuideToPlateRemainIngredients();
            else GuideToGetPlate();
        }

        private void GuideToGetPlate()
        {
            manager.PlaceArrowOverPlate();
            manager.SetDescription($"Get the Plate using {GetCollectButtonDisplayName()}.");

            plateCounter.OnItemCollectedFromTop += HandlePlateCollected;
        }

        private void HandlePlateCollected(IItemCollectable _)
        {
            plateCounter.OnItemCollectedFromTop -= HandlePlateCollected;

            manager.HideArrow();
            CompleteDescriptionAndInvoke(GuideToPlateRemainIngredients);
        }

        private void GuideToPlateRemainIngredients()
        {
            manager.PlaceArrowOverPreparator(manager.Stove, stoveArrowIndex);
            manager.PlaceArrowOverPreparator(manager.CuttingBoard, cuttingBoardArrowIndex);

            manager.SetDescription("Plate the other Ingredients.");
            manager.Plate.OnIngredientPlated += HandleRemainIngredientsPlated;
        }

        private void HandleRemainIngredientsPlated(Ingredient ingredient)
        {
            if (ingredient.IsIngredient(IngredientName.Meat, IngredientStatus.Ready))
            {
                manager.HideArrow(stoveArrowIndex);
                isMeatPlated = true;
            }

            if (ingredient.IsIngredient(IngredientName.Cheese, IngredientStatus.Ready))
            {
                manager.HideArrow(cuttingBoardArrowIndex);
                isCheesePlated = true;
            }

            var isRemainIngredientsPlated = isMeatPlated && isCheesePlated;
            if (!isRemainIngredientsPlated) return;

            manager.Plate.OnIngredientPlated -= HandleRemainIngredientsPlated;
            CompleteDescriptionAndInvoke(GuideToDeliveryCounter);
        }

        private void GuideToDeliveryCounter()
        {
            manager.DeliveryCounter.IsEnabled = true;

            manager.PlaceArrowOverDeliveryCounter();
            manager.SetDescription("Delivery your order!");

            orderSettings.OnOrderDelivered += HandleOrderDelivered;
        }

        private void HandleOrderDelivered(Order _)
        {
            orderSettings.OnOrderDelivered -= HandleOrderDelivered;

            manager.HideArrow();
            Complete();
        }

        private void FindCounters()
        {
            counters = FindObjectsByType<Counter>(FindObjectsSortMode.None);

            foreach (var counter in counters)
            {
                if (counter.HasPlate())
                {
                    plateCounter = counter;
                    break;
                }
            }
        }

        private void EnableCounters(bool enabled)
        {
            foreach (var counter in counters)
            {
                counter.IsEnabled = enabled;
            }
        }
    }
}