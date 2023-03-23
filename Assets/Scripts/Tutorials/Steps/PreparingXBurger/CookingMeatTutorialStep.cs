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
            manager.EnableCheeseBurgerTitle(true);
            Invoke(nameof(GuideToMeatCounter), 2F);
        }

        private void GuideToMeatCounter()
        {
            manager.PlaceArrowOverCounter(manager.MeatCounter);
            manager.SetDescription($"Collect a Meat using {GetCollectButtonDisplayName()}.");

            manager.MeatCounter.OnItemCollected += HandleMeatCollected;
        }

        private void HandleMeatCollected(IItemCollectable _)
        {
            manager.MeatCounter.OnItemCollected -= HandleMeatCollected;

            manager.HideArrow();
            CompleteDescriptionAndInvoke(GuiteToPlaceMeatOverStove);
        }

        private void GuiteToPlaceMeatOverStove()
        {
            manager.PlaceArrowOverPreparator(manager.Stove);
            manager.SetDescription($"Place the Meat over the Stove using {GetCollectButtonDisplayName()}.");

            manager.Stove.OnItemPlaced += HandleMeatPlacedOverStove;
        }

        private void HandleMeatPlacedOverStove(IItemCollectable _)
        {
            manager.Stove.OnItemPlaced -= HandleMeatPlacedOverStove;

            manager.HideArrow();
            CompleteDescriptionAndInvoke(GuiteToTurnOnStove);
        }

        private void GuiteToTurnOnStove()
        {
            manager.PlaceArrowOverPreparator(manager.Stove);
            manager.SetDescription($"Turn on the Stove using {GetInteractWithEnvironmentButtonDisplayName()}.");

            manager.Stove.OnPreparationStarted += HandleStovePreparationStarted;
        }

        private void HandleStovePreparationStarted()
        {
            manager.Stove.OnPreparationStarted -= HandleStovePreparationStarted;

            manager.HideArrow();
            CompleteDescriptionAndInvoke(ShowWaitCooking);
            manager.Stove.OnPreparationCompleted += HandleStovePreparationCompleted;
        }

        private void ShowWaitCooking() => manager.SetDescription("Wait until the meat is cooked.");

        private void HandleStovePreparationCompleted()
        {
            manager.Stove.OnPreparationCompleted -= HandleStovePreparationCompleted;
            Complete();
        }
    }
}