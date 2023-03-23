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
            manager.PlaceArrowOverCounter(manager.CheeseCounter);
            manager.SetDescription($"Collect a Cheese using {GetCollectButtonDisplayName()}.");

            manager.CheeseCounter.OnItemCollected += HandleCheeseCollected;
        }

        private void HandleCheeseCollected(IItemCollectable _)
        {
            manager.CheeseCounter.OnItemCollected -= HandleCheeseCollected;

            manager.HideArrow();
            CompleteDescriptionAndInvoke(GuideToPlaceCheeseOverCuttingTable);
        }

        private void GuideToPlaceCheeseOverCuttingTable()
        {
            manager.PlaceArrowOverPreparator(manager.CuttingBoard);
            manager.SetDescription($"Place the Cheese over the Cutting Board using {GetCollectButtonDisplayName()}.");

            manager.CuttingBoard.OnItemPlaced += HandleCheesePlacedOverCuttingBoard;
        }

        private void HandleCheesePlacedOverCuttingBoard(IItemCollectable item)
        {
            var isCuttableCheese = item.IsIngredient(IngredientName.Cheese, IngredientStatus.Cuttable);
            if (!isCuttableCheese) return;

            manager.CuttingBoard.OnItemPlaced -= HandleCheesePlacedOverCuttingBoard;

            manager.HideArrow();
            CompleteDescriptionAndInvoke(GuideToUseCuttingBoard);
        }

        private void GuideToUseCuttingBoard()
        {
            manager.PlaceArrowOverPreparator(manager.CuttingBoard);
            manager.SetDescription($"Start cutting using {GetInteractWithEnvironmentButtonDisplayName()}.");

            manager.CuttingBoard.OnPreparationStarted += HandleCuttingBoardPreparationStarted;
        }

        private void HandleCuttingBoardPreparationStarted()
        {
            manager.CuttingBoard.OnPreparationStarted -= HandleCuttingBoardPreparationStarted;

            manager.HideArrow();
            Complete();
        }
    }
}