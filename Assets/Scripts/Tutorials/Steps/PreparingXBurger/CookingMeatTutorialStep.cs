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
            manager.SetDescription($"Collect a Meat using {GetCollectButtonSprite()}.");

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
            manager.SetDescription($"Place the Meat over the Stove using {GetCollectButtonSprite()}.");

            manager.Stove.OnItemPlaced += HandleMeatPlacedOverStove;
        }

        private void HandleMeatPlacedOverStove(IItemCollectable item)
        {
            var isRawMeat = item.IsIngredient(IngredientName.Meat, IngredientStatus.Raw);
            if (!isRawMeat) return;

            manager.Stove.OnItemPlaced -= HandleMeatPlacedOverStove;

            manager.HideArrow();

            manager.Stove.OnPreparationStarted += HandleStovePreparationStarted;
            CompleteDescriptionAndInvoke(GuideToUseStove);
        }

        private void GuideToUseStove()
        {
            var isCookingMeat = manager.Stove.IsPreparing;
            if (isCookingMeat) return;

            manager.PlaceArrowOverPreparator(manager.Stove);
            manager.SetDescription($"Start cooking using {GetInteractWithEnvironmentButtonSprite()}.");
        }

        private void HandleStovePreparationStarted()
        {
            manager.Stove.OnPreparationStarted -= HandleStovePreparationStarted;

            manager.Stove.Counter.IsEnabled = false;
            manager.HideArrow();

            Complete();
        }
    }
}