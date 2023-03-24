using UnityEngine;
using KitchenChaos.Items;

namespace KitchenChaos.Tutorials
{
    [DisallowMultipleComponent]
    public sealed class CookingMeatTutorialStep : AbstractTutorialStep
    {
        internal override string GetDescription() => "Cooking a Meat";

        internal override void Begin()
        {
            manager.Stove.IsAbleToBurn = false;
            manager.EnableCheeseBurgerTitle(true);

            Invoke(nameof(GuideToMeatCounter), 2F);
        }

        private void GuideToMeatCounter()
        {
            manager.MeatCounter.IsEnabled = true;
            manager.PlaceArrowOverCounter(manager.MeatCounter);
            manager.SetDescription($"Collect a Meat using {GetCollectButtonDisplayName()}.");

            manager.MeatCounter.OnItemCollectedFromInside += HandleMeatCollected;
        }

        private void HandleMeatCollected(IItemCollectable _)
        {
            manager.MeatCounter.OnItemCollectedFromInside -= HandleMeatCollected;

            manager.HideArrow();
            manager.MeatCounter.IsEnabled = false;

            CompleteDescriptionAndInvoke(GuideToPlaceMeatOverStove);
        }

        private void GuideToPlaceMeatOverStove()
        {
            manager.Stove.Counter.IsEnabled = true;
            manager.PlaceArrowOverPreparator(manager.Stove);
            manager.SetDescription($"Place the Meat over the Stove using {GetCollectButtonDisplayName()}.");

            manager.Stove.OnItemPlaced += HandleMeatPlacedOverStove;
        }

        private void HandleMeatPlacedOverStove(IItemCollectable item)
        {
            var isRawMeat = item.IsIngredient(IngredientName.Meat, IngredientStatus.Raw);
            if (!isRawMeat) return;

            manager.Stove.OnItemPlaced -= HandleMeatPlacedOverStove;

            manager.HideArrow();
            CompleteDescriptionAndInvoke(CheckWhetherGuideToUseStove);
        }

        private void CheckWhetherGuideToUseStove()
        {
            var isCookingMeat = manager.Stove.IsPreparing;
            if (isCookingMeat) Complete();
            else GuideToUseStove();
        }

        private void GuideToUseStove()
        {
            manager.PlaceArrowOverPreparator(manager.Stove);
            manager.SetDescription($"Start cooking using {GetInteractWithEnvironmentButtonDisplayName()}.");

            manager.Stove.OnPreparationStarted += HandleStovePreparationStarted;
        }

        private void HandleStovePreparationStarted()
        {
            manager.Stove.OnPreparationStarted -= HandleStovePreparationStarted;

            manager.HideArrow();
            Complete();
        }

        protected override void Complete()
        {
            manager.Stove.Counter.IsEnabled = false;
            base.Complete();
        }
    }
}