using UnityEngine;
using KitchenChaos.Items;
using KitchenChaos.Counters;
using KitchenChaos.Orders;

namespace KitchenChaos.Tutorials
{
    [DisallowMultipleComponent]
    public sealed class PlatingTutorialStep : AbstractTutorialStep
    {
        private Counter plateCounter;

        private const int stoveArrowIndex = 0;
        private const int cuttingBoardArrowIndex = 1;

        private bool isMeatPlated;
        private bool isCheesePlated;

        internal override string GetDescription() => "Plating Ingredients";

        internal override void Begin()
        {
            FindPlateCounter();
            manager.EnableCheeseBurgerTitle(true);
            Invoke(nameof(GuideToBreadCounter), 2F);
        }

        private void GuideToBreadCounter()
        {
            manager.BreadCounter.IsEnabled = true;
            manager.PlaceArrowOverCounter(manager.BreadCounter);
            manager.SetDescription($"Collect a Bread using {GetCollectButtonSprite()}");

            manager.BreadCounter.OnItemCollectedFromInside += HandleBreadCollected;
        }

        private void HandleBreadCollected(IItemCollectable _)
        {
            manager.BreadCounter.OnItemCollectedFromInside -= HandleBreadCollected;

            // Disabling Player Switch so the other Player cannot take the Plate.
            manager.PlayerSettings.DisablePlayerSwitch();
            manager.BreadCounter.IsEnabled = false;

            manager.HideArrow();
            manager.Plate.IsEnabled = true;
            plateCounter.IsEnabled = true;

            CompleteDescriptionAndInvoke(GuideToPlaceBreadOverPlate);
        }

        private void GuideToPlaceBreadOverPlate()
        {
            manager.PlaceArrowOverPlate();
            manager.SetDescription($"Place the Bread over the Plate using {GetCollectButtonSprite()}");

            manager.Plate.OnIngredientPlated += HandleCheesePlacedOverPlate;
        }

        private void HandleCheesePlacedOverPlate(Ingredient ingredient)
        {
            var isBread = ingredient.IsIngredient(IngredientName.Bread);
            if (!isBread) return;

            manager.Plate.OnIngredientPlated -= HandleCheesePlacedOverPlate;

            manager.HideArrow();
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
            manager.SetDescription($"Get the Plate using {GetCollectButtonSprite()}");

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
            manager.Stove.Counter.IsEnabled = true;
            manager.CuttingBoard.Counter.IsEnabled = true;

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
                manager.Stove.Counter.IsEnabled = false;
                isMeatPlated = true;
            }

            if (ingredient.IsIngredient(IngredientName.Cheese, IngredientStatus.Ready))
            {
                manager.HideArrow(cuttingBoardArrowIndex);
                manager.CuttingBoard.Counter.IsEnabled = false;
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
            manager.PlayerSettings.EnablePlayerSwitch();

            manager.PlaceArrowOverDeliveryCounter();
            manager.SetDescription("Delivery your order!");

            OrderManager.Instance.Settings.OnOrderDelivered += HandleOrderDelivered;
        }

        private void HandleOrderDelivered(Order _)
        {
            OrderManager.Instance.Settings.OnOrderDelivered -= HandleOrderDelivered;

            manager.HideArrow();
            Complete();
        }

        private void FindPlateCounter()
        {
            foreach (var counter in manager.Counters)
            {
                if (counter.HasPlate())
                {
                    plateCounter = counter;
                    break;
                }
            }
        }
    }
}