using KitchenChaos.Items;
using UnityEngine;

namespace KitchenChaos.Tutorials
{
    [DisallowMultipleComponent]
    public sealed class CuttingCheeseTutorialStep : AbstractTutorialStep
    {
        internal override string GetDescription() => "Cutting the Cheese";

        internal override void Begin()
        {
            manager.EnableCheeseBurgerTitle(true);
            Invoke(nameof(GuideToCheeseCounter), 2F);
        }

        private void GuideToCheeseCounter()
        {
            manager.CheeseCounter.IsEnabled = true;
            manager.PlaceArrowOverCounter(manager.CheeseCounter);
            manager.SetDescription($"Collect a Cheese using {GetCollectButtonSprite()}");

            manager.CheeseCounter.OnItemCollectedFromInside += HandleCheeseCollected;
        }

        private void HandleCheeseCollected(IItemCollectable _)
        {
            manager.CheeseCounter.OnItemCollectedFromInside -= HandleCheeseCollected;

            manager.HideArrow();
            manager.CheeseCounter.IsEnabled = false;

            CompleteDescriptionAndInvoke(GuideToPlaceCheeseOverCuttingTable);
        }

        private void GuideToPlaceCheeseOverCuttingTable()
        {
            manager.CuttingBoard.Counter.IsEnabled = true;
            manager.PlaceArrowOverPreparator(manager.CuttingBoard);
            manager.SetDescription($"Place the Cheese over the Cutting Board using {GetCollectButtonSprite()}");

            manager.CuttingBoard.OnItemPlaced += HandleCheesePlacedOverCuttingBoard;
        }

        private void HandleCheesePlacedOverCuttingBoard(IItemCollectable item)
        {
            var isCuttableCheese = item.IsIngredient(IngredientName.Cheese, IngredientStatus.Cuttable);
            if (!isCuttableCheese) return;

            manager.CuttingBoard.OnItemPlaced -= HandleCheesePlacedOverCuttingBoard;

            manager.HideArrow();

            CompleteDescriptionAndInvoke(GuideToUseCuttingBoard);
            manager.CuttingBoard.OnPreparationStarted += HandleCuttingBoardPreparationStarted;
        }

        private void GuideToUseCuttingBoard()
        {
            var isSlicingCheese = manager.CuttingBoard.IsPreparing;
            if (isSlicingCheese) return;

            manager.PlaceArrowOverPreparator(manager.CuttingBoard);
            manager.SetDescription($"Start cutting using {GetInteractWithEnvironmentButtonSprite()}");
        }

        private void HandleCuttingBoardPreparationStarted()
        {
            manager.CuttingBoard.OnPreparationStarted -= HandleCuttingBoardPreparationStarted;

            manager.HideArrow();
            manager.CuttingBoard.Counter.IsEnabled = false;

            Complete();
        }
    }
}